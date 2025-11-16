namespace CarDealer
{
    using CarDealer.Data;
    using CarDealer.DTOs.Export.ExportCarsFromMake;
    using CarDealer.DTOs.Export.ExportCarsWithDistance;
    using CarDealer.DTOs.Export.ExportCarsWithParts;
    using CarDealer.DTOs.Export.ExportCustomers;
    using CarDealer.DTOs.Export.ExportLocalSuppliers;
    using CarDealer.DTOs.Export.ExportSaleWithAppliedDiscount;
    using CarDealer.DTOs.Import;
    using CarDealer.Models;
    using CarDealer.Utilities;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.Dynamic;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext context = new CarDealerContext();

            //RecreateAndSeedDatabase(context);

            string result = GetSalesWithAppliedDiscount(context);
            Console.WriteLine(result);

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

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            ICollection<Customer> customers = new List<Customer>();

            IEnumerable<ImportCustomerDto>? customerDtos = XmlSerializerWrapper
                .Deserialize<ImportCustomerDto[]>(inputXml, "Customers");

            if (customerDtos != null)
            {
                foreach (ImportCustomerDto customerDto in customerDtos)
                {
                    if (!IsValid(customerDto))
                    {
                        continue;
                    }

                    bool isBirthDayValid = DateTime
                        .TryParse(customerDto.BirthDate, out DateTime birthDayValue);

                    bool isYoungDriverValid = bool
                        .TryParse(customerDto.IsYoungDriver, out bool isYoungDriverValue);

                    if (!isBirthDayValid || !isYoungDriverValid)
                    {
                        continue;
                    }

                    Customer customer = new()
                    {
                        Name = customerDto.Name,
                        BirthDate = birthDayValue,
                        IsYoungDriver = isYoungDriverValue
                    };

                    customers.Add(customer);
                }

                context.Customers.AddRange(customers);

                context.SaveChanges();
            }

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            ICollection<Sale> sales = new List<Sale>();
            ICollection<int> existingCarIds = context.Cars
                .AsNoTracking()
                .Select(c => c.Id)
                .ToArray();
            ICollection<int> existingCustomerIds = context.Customers
                .AsNoTracking()
                .Select(c => c.Id)
                .ToArray();

            IEnumerable<ImportSaleDto>? saleDtos = XmlSerializerWrapper
                .Deserialize<ImportSaleDto[]>(inputXml, "Sales");

            if (saleDtos != null)
            {
                foreach (ImportSaleDto saleDto in saleDtos)
                {
                    if (!IsValid(saleDto))
                    {
                        continue;
                    }

                    bool isCardValid = int
                        .TryParse(saleDto.CarId, out int carIdValue);

                    bool isCustomerValid = int
                        .TryParse(saleDto.CustomerId, out int customerIdValue);

                    bool isDiscountValid = decimal
                        .TryParse(saleDto.Discount, out decimal discountValue);

                    if (!isCardValid || !isCustomerValid || !isDiscountValid ||
                        !existingCarIds.Contains(carIdValue) || !existingCustomerIds.Contains(customerIdValue))
                    {
                        continue;
                    }

                    Sale sale = new()
                    {
                        CarId = carIdValue,
                        CustomerId = customerIdValue,
                        Discount = discountValue
                    };

                    sales.Add(sale);
                }

                context.Sales.AddRange(sales);

                context.SaveChanges();
            }

            return $"Successfully imported {sales.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            ExportCarRoot rootDto = new()
            {
                Car = context.Cars
                    .AsNoTracking()
                    .Where(c => c.TraveledDistance > 2000000)
                    .OrderBy(c => c.Make)
                    .ThenBy(c => c.Model)
                    .Select(c => new ExportCarDetails
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TraveledDistance
                    })
                    .Take(10)
                    .ToArray()
            };

            string result = XmlSerializerWrapper
                .Serialize(rootDto, "cars");

            return result;
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            ExportCarRootMake exportDto = new()
            {
                Car = context.Cars
                    .AsNoTracking()
                    .Where(c => c.Make == "BMW")
                    .OrderBy(c => c.Model)
                    .ThenByDescending(c => c.TraveledDistance)
                    .Select(c => new ExportCarMakeDetails
                    {
                        Id = c.Id.ToString(),
                        Model = c.Model,
                        TravelledDistance = c.TraveledDistance.ToString()
                    })
                    .ToArray()
            };

            string result = XmlSerializerWrapper
                .Serialize(exportDto, "cars");

            return result;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            ExportSupplierRoot exportDto = new()
            {
                Supplier = context.Suppliers
                    .AsNoTracking()
                    .Where(s => s.IsImporter == false)
                    .Select(s => new ExportSupplierDetails
                    {
                        Id = s.Id.ToString(),
                        Name = s.Name,
                        PartsCount = s.Parts.Count.ToString()
                    })
                    .ToArray()
            };

            string result = XmlSerializerWrapper
                .Serialize(exportDto, "suppliers");

            return result;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            ExportCarPartRootDto exportDto = new()
            {
                Car = context.Cars
                    .AsNoTracking()
                    .Select(c => new ExportCarPartDetailDto
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TraveledDistance = c.TraveledDistance,
                        Parts = c.PartsCars
                            .Select(p => new ExportPartCarDetailDto
                            {
                                Name = p.Part.Name,
                                Price = p.Part.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                    })
                    .OrderByDescending(c => c.TraveledDistance)
                    .ThenBy(c => c.Model)
                    .Take(5)
                    .ToArray()
            };

            string result = XmlSerializerWrapper
                .Serialize(exportDto, "cars");

            return result;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .AsNoTracking()
                .Where(c => c.Sales.Count >= 1)
                .Select(c => new
                {
                    Name = c.Name,
                    BoughtCars = c.Sales.Count,
                    IsYoungDriver = c.IsYoungDriver,
                    SpentMoney = c.Sales
                            .SelectMany(c => c.Car.PartsCars)
                            .Sum(p => p.Part.Price)
                            
                })
                .ToArray();

            ExportCustomerRootDto exportDto = new ExportCustomerRootDto()
            {
                Customer = customers
                    .Select(c => new ExportCustomerDetailDto
                    {
                        FullName = c.Name,
                        BoughtCars = c.BoughtCars,
                        SpentMoney = c.IsYoungDriver ? Math.Round(c.SpentMoney * 0.95m, 2)
                        : c.SpentMoney,
                    })
                    .OrderByDescending(c => c.SpentMoney)
                    .ToArray()
            };

            string result = XmlSerializerWrapper
                .Serialize(exportDto, "customers");

            return result;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .AsNoTracking()
                .Select(s => new
                {
                    Car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance,
                    },
                        
                        Discount = s.Discount,
                        CustomerName = s.Customer.Name,
                        Price = s.Car.PartsCars
                            .Sum(pc => pc.Part.Price)
                })
                .ToArray();

            ExportSaleRootDto exportDto = new ExportSaleRootDto()
            {
                Sale = sales
                    .Select(s => new ExportSaleWithDetailsDto
                    {
                        Car = new ExportSaleCarDto
                        {
                            Make = s.Car.Make,
                            Model = s.Car.Model,
                            TraveledDistance = s.Car.TraveledDistance
                        },
                        Discount = (int)s.Discount,
                        CustomerName = s.CustomerName,
                        Price = Math.Round(s.Price, 4),
                        PriceWithDiscount = Math.Round(s.Price * (1 - (s.Discount / 100m)), 4)
                    })
                    .ToArray()
            };

            string result = XmlSerializerWrapper
                .Serialize(exportDto, "sales");

            return result;
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

            inputXml = File.ReadAllText(Path.Combine(path, "customers.xml"));

            result = ImportCustomers(context, inputXml);

            inputXml = File.ReadAllText(Path.Combine(path, "sales.xml"));

            result = ImportSales(context, inputXml);

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