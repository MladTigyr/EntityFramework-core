using Cadastre.Data;
using Cadastre.Data.Enumerations;
using Cadastre.DataProcessor.ExportDtos.Xml;
using Cadastre.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            DateTime dateTime = new(2000, 1, 1);

            var properties = dbContext.Properties
                .AsNoTracking()
                .Where(p => p.DateOfAcquisition >= dateTime)
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    Address = p.Address,
                    DateOfAcquisition = p.DateOfAcquisition,
                    Owners = p.PropertiesCitizens
                        .Select(pc => new
                        {
                            LastName = pc.Citizen.LastName,
                            MaritalStatus = pc.Citizen.MaritalStatus
                        })
                        .OrderBy(p => p.LastName)
                        .ToArray()
                })
                .ToArray();

            var exportDtos = properties
                .Select(p => new
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    Address = p.Address,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy"),
                    Owners = p.Owners
                        .Select(o => new
                        {
                            LastName = o.LastName,
                            MaritalStatus = o.MaritalStatus.ToString()
                        })
                });

            string result = JsonConvert
                .SerializeObject(exportDtos, Formatting.Indented);

            return result;

        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            ExportPropertiesRootDto exportDtos = new ExportPropertiesRootDto()
            {
                Property = dbContext.Properties
                    .AsNoTracking()
                    .Where(p => p.Area >= 100)
                    .OrderByDescending(p => p.Area)
                    .ThenBy(p => p.DateOfAcquisition)
                    .Select(p => new ExportPropertiesDetailsDto
                    {
                        PostalCode = p.District.PostalCode,
                        PropertyIdentifier = p.PropertyIdentifier,
                        Area = p.Area,
                        DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy")
                    })
                    .ToArray()
            };

            string result = XmlSerializeWrapper
                .Serialize(exportDtos, "Properties");

            return result;
        }
    }
}
