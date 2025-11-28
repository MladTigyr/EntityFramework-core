namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto.Json;
    using Artillery.DataProcessor.ImportDto.Xml;
    using Artillery.Utilites;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Country> countries = new List<Country>();

            IEnumerable<ImportCountryDto>? importCountryDtos = XmlSerializeWrapper
                .Deserialize<ImportCountryDto[]>(xmlString, "Countries");

            if (importCountryDtos != null)
            {
                foreach (ImportCountryDto countryDto in importCountryDtos)
                {
                    if (!IsValid(countryDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Country country = new Country()
                    {
                        CountryName = countryDto.CountryName,
                        ArmySize = countryDto.ArmySize,
                    };

                    countries.Add(country);

                    sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
                }

                context.Countries.AddRange(countries);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Manufacturer> manufacturers = new List<Manufacturer>();

            ICollection<string> manufacturerNames = new List<string>();

            IEnumerable<ImportManufacturerDto>? importManufacturerDtos = XmlSerializeWrapper
                .Deserialize<ImportManufacturerDto[]>(xmlString, "Manufacturers");

            if (importManufacturerDtos != null)
            {
                foreach (ImportManufacturerDto manufacturerDto in importManufacturerDtos)
                {
                    if (!IsValid(manufacturerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (manufacturerNames.Contains(manufacturerDto.ManufacturerName))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Manufacturer manufacturer = new Manufacturer()
                    {
                        ManufacturerName = manufacturerDto.ManufacturerName,
                        Founded = manufacturerDto.Founded,
                    };

                    manufacturers.Add(manufacturer);
                    manufacturerNames.Add(manufacturerDto.ManufacturerName);

                    string[] foundedElements = manufacturer.Founded
                        .Split(", ", StringSplitOptions.RemoveEmptyEntries);

                    string[] output = foundedElements.Skip(Math.Max(0, foundedElements.Length - 2)).ToArray();

                    sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, string.Join(", ", output)));
                }

                context.Manufacturers.AddRange(manufacturers);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Shell> shells = new List<Shell>();

            IEnumerable<ImportShellDto>? importShellDtos = XmlSerializeWrapper
                .Deserialize<ImportShellDto[]>(xmlString, "Shells");

            if (importShellDtos != null)
            {
                foreach (ImportShellDto shellDto in importShellDtos)
                {
                    if (!IsValid(shellDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Shell shell = new Shell()
                    {
                        ShellWeight = shellDto.ShellWeight,
                        Caliber = shellDto.Caliber,
                    };

                    shells.Add(shell);

                    sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
                }

                context.Shells.AddRange(shells);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Gun> guns = new List<Gun>();

            ICollection<int> counntryIds = context.Countries
                .AsNoTracking()
                .Select(c => c.Id)
                .ToArray();

            IEnumerable<ImportGunDto>? importGunDtos = JsonConvert
                .DeserializeObject<ImportGunDto[]>(jsonString);

            if (importGunDtos != null)
            {
                foreach (ImportGunDto gunDto in importGunDtos)
                {
                    if (!IsValid(gunDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isGunTypeValid = Enum
                        .TryParse<GunType>(gunDto.GunType, out GunType gunTypeValue);

                    if (!isGunTypeValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Gun gun = new Gun()
                    {
                        ManufacturerId = gunDto.ManufacturerId,
                        GunWeight = gunDto.GunWeight,
                        BarrelLength = gunDto.BarrelLength,
                        NumberBuild = gunDto.NumberBuild,
                        Range = gunDto.Range,
                        GunType = gunTypeValue,
                        ShellId = gunDto.ShellId,
                    };

                    ICollection<CountryGun> countryGuns = new List<CountryGun>();

                    foreach (ImportCountryIdDto currId in gunDto.Countries)
                    {
                        CountryGun countryGun = new CountryGun()
                        {
                            CountryId = currId.Id,
                            Gun = gun
                        };
                    }

                    gun.CountriesGuns = countryGuns;
                    guns.Add(gun);

                    sb.AppendLine(string.Format(SuccessfulImportGun, gun.GunType.ToString(), gun.GunWeight, gun.BarrelLength));
                }

                context.Guns.AddRange(guns);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}