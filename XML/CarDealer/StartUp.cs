namespace CarDealer
{
    using CarDealer.Data;
    using CarDealer.DTOs.Import;
    using CarDealer.Models;
    using CarDealer.Utilities;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext context = new CarDealerContext();

            RecreateAndSeedDatabase(context);

        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            ICollection<Supplier> suppliers = new List<Supplier>();

            IEnumerable<ImportSuplierDto>? supplierDtos = XmlSerializerWrapper
                .Deserialize<ImportSuplierDto[]>(inputXml, "Suppliers");

            if (supplierDtos != null)
            {
                foreach (ImportSuplierDto supplierDto in supplierDtos)
                {
                    if (!IsValid(supplierDto))
                    {
                        continue;
                    }

                    bool isValidIsImporter = bool
                        .TryParse(supplierDto.IsImporter, out bool isImporterValue);

                    if(!isValidIsImporter)
                    {
                        continue;
                    }

                    Supplier supplier = new Supplier()
                    {
                        Name = supplierDto.Name,
                        IsImporter = isImporterValue
                    };

                    suppliers.Add(supplier);
                }

                context.Suppliers.AddRange(suppliers);

                context.SaveChanges();
            }

            return $"Successfully imported {suppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            ICollection<Part> parts = new List<Part>();
            ICollection<int> existingSupplierids = context.Suppliers
                .AsNoTracking()
                .Select(s => s.Id)
                .ToArray();

            IEnumerable<ImportPartDto>? partDtos = XmlSerializerWrapper
                .Deserialize<ImportPartDto[]>(inputXml, "Parts");

            if (partDtos != null)
            {
                foreach (ImportPartDto partDto in partDtos)
                {
                    if (!IsValid(partDto))
                    {
                        continue;
                    }

                    bool isPriceValid = decimal
                        .TryParse(partDto.Price, out decimal pricevalue);

                    bool isQuantityValid = int
                        .TryParse(partDto.Quantity, out int quantityValue);

                    bool isSupplierIdValid = int
                        .TryParse(partDto.SupplierId, out int supplierIdValue);

                    if (!isPriceValid || !isQuantityValid || !isSupplierIdValid || !existingSupplierids.Contains(supplierIdValue))
                    {
                        continue;
                    }

                    Part part = new()
                    {
                        Name = partDto.Name,
                        Price = pricevalue,
                        Quantity = quantityValue,
                        SupplierId = supplierIdValue
                    };

                    parts.Add(part);
                }

                context.Parts.AddRange(parts);

                context.SaveChanges();
            }

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            ICollection<Car> cars = new List<Car>();
            ICollection<int> existingPartsIds = context.Parts
                .AsNoTracking()
                .Select(p => p.Id)
                .ToArray();

            ICollection<PartCar> partsCars = new List<PartCar>();

            IEnumerable<ImportCarDto>? carDtos = XmlSerializerWrapper
                .Deserialize<ImportCarDto[]>(inputXml, "Cars");

            if (carDtos != null)
            {
                foreach (ImportCarDto carDto in carDtos)
                {
                    if (!IsValid(carDto))
                    {
                        continue;
                    }

                    bool isTravelValid = long
                        .TryParse(carDto.TraveledDistance, out long travelDistanceValue);


                    if (!isTravelValid || !carDto.Parts.Any(p => existingPartsIds.Contains(int.Parse(p.Id))))
                    {
                        continue;
                    }

                    Car car = new()
                    {
                        Make = carDto.Make,
                        Model = carDto.Model,
                        TraveledDistance = travelDistanceValue,
                    };

                    foreach (var element in carDto.Parts.DistinctBy(p => p.Id))
                    {
                        int partId = int.Parse(element.Id);
                        if (existingPartsIds.Contains(partId))
                        {
                            PartCar partCar = new PartCar()
                            {
                                PartId = partId,
                                Car = car
                            };
                            partsCars.Add(partCar);
                        }
                    }

                    cars.Add(car);
                }

                context.Cars.AddRange(cars);
                context.PartsCars.AddRange(partsCars);
                context.SaveChanges();
            }

            return $"Successfully imported {cars.Count}";
        }

        private static void RecreateAndSeedDatabase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            string path = "../../../DataSets/";
            string currFile = "suppliers.xml";

            string inputXml = File.ReadAllText(Path.Combine(path, currFile));

            string result = ImportSuppliers(context, inputXml);

            inputXml = File.ReadAllText(Path.Combine(path, "parts.xml"));

            result = ImportParts(context, inputXml);

            inputXml = File.ReadAllText(Path.Combine(path, "cars.xml"));

            result = ImportCars(context, inputXml);

            Console.WriteLine(result);
        }

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool result = Validator.TryValidateObject(obj, validationContext, validationResults);

            return result;
        }
    }
}