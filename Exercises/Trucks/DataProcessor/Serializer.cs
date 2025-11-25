namespace Trucks.DataProcessor
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ExportDto.Xml;
    using Trucks.Utilities;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var despatchers = new
            {
                Despatcher = context.Despatchers
                    .Where(d => d.Trucks.Count >= 1)
                    .Select(d => new
                    {
                        TrucksCount = d.Trucks.Count,
                        DespatcherName = d.Name,
                        Trucks = d.Trucks
                        .Select(t => new
                        {
                            RegistrationNumber = t.RegistrationNumber,
                            Make = t.MakeType
                        })
                        .OrderBy(t => t.RegistrationNumber)
                        .ToArray()
                    })
                    .OrderByDescending(d => d.TrucksCount)
                    .ThenBy(d => d.DespatcherName)
                    .ToArray()
            };

            ExportDespatcherRootDto exportDtos = new ExportDespatcherRootDto()
            {
                Despatcher = despatchers.Despatcher
                    .Select(d => new ExportDespatcherDetailsDto
                    {
                        TrucksCount = d.TrucksCount,
                        DespatcherName = d.DespatcherName,
                        Trucks = d.Trucks
                            .Select(t => new ExportTruckDetailsDto
                            {
                                RegistrationNumber = t.RegistrationNumber,
                                Make = t.Make.ToString()
                            })
                            .ToArray()
                    })
                    .ToArray()
            };

            string result = XmlSerializeWrapper
                .Serialize(exportDtos, "Despatchers");

            return result;
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                .Where(c => c.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                .ToArray()
                .Select(c => new
                {
                    Name = c.Name,
                    Trucks = c.ClientsTrucks
                        .Where(t => t.Truck.TankCapacity >= capacity)
                        .ToArray()
                        .Select (t => new 
                        {
                            TruckRegistrationNumber = t.Truck.RegistrationNumber,
                            VinNumber = t.Truck.VinNumber,
                            TankCapacity = t.Truck.TankCapacity,
                            CargoCapacity = t.Truck.CargoCapacity,
                            CategoryType = t.Truck.CategoryType.ToString(),
                            MakeType = t.Truck.MakeType.ToString()
                        })
                        .OrderBy(t => t.MakeType)
                        .ThenByDescending(t => t.CargoCapacity)
                        .ToArray(),
                })
                .OrderByDescending(c => c.Trucks.Count())
                .ThenBy(c => c.Name)
                .Take(10)
                .ToArray();

            string result = JsonConvert
                .SerializeObject(clients, Formatting.Indented);

            return result;
        }
    }
}
