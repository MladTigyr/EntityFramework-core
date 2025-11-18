namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos.Json;
    using Cadastre.DataProcessor.ImportDtos.Xml;
    using Cadastre.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Serialization;
    using static Cadastre.DataValidation.DataValidation.Citizen;
    using static Cadastre.DataValidation.DataValidation.District;
    using static Cadastre.DataValidation.DataValidation.Property;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<District> districts = new List<District>();
            ICollection<string> names = dbContext.Districts
                .AsNoTracking()
                .Select(x => x.Name)
                .ToArray();

            ICollection<Property> properties = new List<Property>();
            ICollection<string> propertyIdentifiers = dbContext.Properties
                .AsNoTracking()
                .Select(pi => pi.PropertyIdentifier)
                .ToArray();
            ICollection<string> addresses = dbContext.Properties
                .AsNoTracking()
                .Select(pi => pi.Address)
                .ToArray();

            IEnumerable<ImportDistrictDto>? importDistrictDtos = XmlSerializeWrapper
                .Deserialize<ImportDistrictDto[]>(xmlDocument, "Districts");

            if (importDistrictDtos != null)
            {
                foreach (ImportDistrictDto districtDto in importDistrictDtos)
                {
                    if (!IsValid(districtDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (names.Contains(districtDto.Name))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isRegionValid = Enum
                        .TryParse<Region>(districtDto.Region, out Region regionValue);

                    if (!isRegionValid)
                    { 
                        continue; 
                    }

                    District district = new District()
                    {
                        Name = districtDto.Name,
                        PostalCode = districtDto.PostalCode,
                        Region = regionValue,
                    };

                    ICollection<string> currPropertiIdentifiers = new List<string>();
                    ICollection<string> currAddresses = new List<string>();

                    foreach (ImportPropertiesDto propertyDto in districtDto.Property)
                    {
                        if (!IsValid(propertyDto))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        if (currPropertiIdentifiers.Contains(propertyDto.PropertyIdentifier) ||
                            propertyIdentifiers.Contains(propertyDto.PropertyIdentifier))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        if (currAddresses.Contains(propertyDto.Address) ||
                            addresses.Contains(propertyDto.Address))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        bool isDateValid = DateTime
                            .TryParseExact(
                            propertyDto.DateOfAcquisition,
                            "dd/MM/yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateTime dateValue);

                        if (!isDateValid) 
                        {
                            continue; 
                        }

                        Property property = new Property()
                        {
                            PropertyIdentifier = propertyDto.PropertyIdentifier,
                            Area = propertyDto.Area,
                            Details = propertyDto.Details,
                            Address = propertyDto.Address,
                            DateOfAcquisition = dateValue,
                        };
                        district.Properties.Add(property);

                        currPropertiIdentifiers.Add(propertyDto.PropertyIdentifier);
                        currAddresses.Add(property.Address);

                        properties.Add(property);
                    }

                    districts.Add(district);

                    dbContext.Districts.AddRange(districts);
                    dbContext.Properties.AddRange(properties);

                    sb.AppendLine(string.Format(SuccessfullyImportedDistrict, district.Name, district.Properties.Count));
                }

                dbContext.SaveChanges();
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Citizen> citizens = new List<Citizen>();
            ICollection<PropertyCitizen> propertyCitizens = new List<PropertyCitizen>();

            IEnumerable<ImportCitizen>? importCitizenDtos = JsonConvert
                .DeserializeObject<ImportCitizen[]>(jsonDocument);

            if (importCitizenDtos != null)
            {
                foreach (ImportCitizen citizenDto in importCitizenDtos)
                {
                    if (!IsValid(citizenDto) || (citizenDto.MaritalStatus != "Unmarried" && citizenDto.MaritalStatus != "Married" && citizenDto.MaritalStatus != "Divorced" && citizenDto.MaritalStatus != "Widowed"))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isMaritalValid = Enum
                        .TryParse<MaritalStatus>(citizenDto.MaritalStatus, out MaritalStatus maritalValue);

                    bool isBirthDateValid = DateTime
                        .TryParseExact(
                        citizenDto.Birthday, 
                        "dd-MM-yyyy", 
                        CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, 
                        out DateTime birthDateValue);

                    if (!isMaritalValid || !isBirthDateValid)
                    {
                        continue;
                    }

                    Citizen citizen = new Citizen()
                    {
                        FirstName = citizenDto.FirstName,
                        LastName = citizenDto.LastName,
                        BirthDate = birthDateValue,
                        MaritalStatus = maritalValue,
                    };

                    citizens.Add(citizen);


                    foreach (int id in citizenDto.Properties)
                    {
                        PropertyCitizen propertyCitizen = new PropertyCitizen()
                        {
                            PropertyId = id,
                            Citizen = citizen,
                        };

                        propertyCitizens.Add(propertyCitizen);
                        citizen.PropertiesCitizens.Add(propertyCitizen);
                    }

                    sb.AppendLine(string.Format(SuccessfullyImportedCitizen, citizen.FirstName, citizen.LastName, citizen.PropertiesCitizens.Count));
                }

                dbContext.Citizens.AddRange(citizens);
                dbContext.PropertiesCitizens.AddRange(propertyCitizens);
                dbContext.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
