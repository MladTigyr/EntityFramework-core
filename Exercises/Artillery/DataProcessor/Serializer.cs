
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto.Xml;
    using Artillery.Utilites;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var exportDtos = context.Shells
                .Where(s => s.ShellWeight > shellWeight)
                .ToArray()
                .Select(s => new 
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns
                        .Where(g => g.GunType.ToString() == "AntiAircraftGun")
                        .ToArray()
                        .Select(g => new
                        {
                            GunType = g.GunType.ToString(),
                            GunWeight = g.GunWeight,
                            BarrelLength = g.BarrelLength,
                            Range = g.Range > 3000 ? "Long-range" : "Regular range"
                        })
                        .OrderByDescending(g => g.GunWeight)
                        .ToArray()
                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();

            string result = JsonConvert
                .SerializeObject(exportDtos, Formatting.Indented);

            return result;
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var guns = new ExportGunRootDto
            {
                Guns = context.Guns
                    .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                    .ToArray()
                    .Select(g => new ExportGunDetailsDto
                    {
                        Manufacturer = g.Manufacturer.ManufacturerName,
                        GunType = g.GunType.ToString(),
                        GunWeight = g.GunWeight,
                        BarrelLength= g.BarrelLength,
                        Range = g.Range,
                        Countries = g.CountriesGuns
                            .Where(c => c.Country.ArmySize > 4500000)
                            .ToArray()
                            .Select(c => new ExportCountryDetailsDto
                            {
                                Country = c.Country.CountryName,
                                ArmySize = c.Country.ArmySize,
                            })
                            .OrderBy(c => c.ArmySize)
                            .ToArray()
                    })
                    .OrderBy(g => g.BarrelLength)
                    .ToArray()
            };

            string result = XmlSerializeWrapper
                .Serialize(guns, "Guns");

            return result;
        }
    }
}
