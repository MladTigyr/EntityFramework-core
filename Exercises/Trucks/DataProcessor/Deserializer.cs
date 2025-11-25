namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto.Json;
    using Trucks.DataProcessor.ImportDto.Xml;
    using Trucks.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Despatcher> despatchers = new List<Despatcher>();

            IEnumerable<ImportDespatcherDto>? importDespatcherDtos = XmlSerializeWrapper
                .Deserialize<ImportDespatcherDto[]>(xmlString, "Despatchers");

            if (importDespatcherDtos != null)
            {
                foreach (ImportDespatcherDto despatcherDto in importDespatcherDtos)
                {
                    if (!IsValid(despatcherDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (despatcherDto.Position == null || despatcherDto.Position == string.Empty)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Despatcher despatcher = new Despatcher()
                    {
                        Name = despatcherDto.Name,
                        Position = despatcherDto.Position,
                    };

                    ICollection<Truck> trucks = new List<Truck>();

                    foreach (ImportTrucksDto truckDto in despatcherDto.Truck) 
                    {
                        if (!IsValid(truckDto))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        Truck truck = new Truck()
                        {
                            RegistrationNumber = truckDto.RegistrationNumber,
                            VinNumber = truckDto.VinNumber,
                            TankCapacity = truckDto.TankCapacity,
                            CargoCapacity = truckDto.CargoCapacity,
                            CategoryType = (CategoryType)truckDto.CategoryType,
                            MakeType = (MakeType)truckDto.MakeType,
                            Despatcher = despatcher,
                        };

                        trucks.Add(truck);
                    }

                    despatcher.Trucks = trucks;
                    despatchers.Add(despatcher);

                    sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count));
                }

                context.Despatchers.AddRange(despatchers);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Client> clients = new List<Client>();
            ICollection<int> truckIds = context.Trucks
                .AsNoTracking()
                .Select(t => t.Id)
                .ToArray();

            IEnumerable<ImportClientDto>? importClientDtos = JsonConvert
                .DeserializeObject<ImportClientDto[]>(jsonString);

            if (importClientDtos != null)
            {
                foreach (ImportClientDto clientDto in importClientDtos)
                {
                    if (!IsValid(clientDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (clientDto.Type == "usual")
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Client client = new Client()
                    {
                        Name = clientDto.Name,
                        Nationality = clientDto.Nationality,
                        Type = clientDto.Type,
                    };

                    ICollection<int> uniqueIds = new HashSet<int>();

                    ICollection<ClientTruck> clientsTrucks = new List<ClientTruck>();

                    foreach (int currId in clientDto.Trucks)
                    {
                        uniqueIds.Add(currId);
                    }

                    foreach (int currId in uniqueIds) 
                    {
                        if (!truckIds.Contains(currId))
                        {
                            sb.AppendLine(ErrorMessage);
                            uniqueIds.Remove(currId);
                            continue;
                        }

                        ClientTruck clientTruck = new ClientTruck()
                        {
                            Client = client,
                            TruckId = currId,
                        };

                        clientsTrucks.Add(clientTruck);
                    }

                    client.ClientsTrucks = clientsTrucks;
                    clients.Add(client);

                    sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
                }

                context.Clients.AddRange(clients);
                context.SaveChanges();
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