using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using var context = new CarDealerContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();


            string path = "../../../Datasets/";
            string currentFile = "sales.json";

            string combinedPath = Path.Combine(path, currentFile);

            string inputJson = File.ReadAllText(combinedPath);

            string result = GetSalesWithAppliedDiscount(context);
            Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            ICollection<Supplier> suppliers = new List<Supplier>();

            IEnumerable<SuppliersDto>? suppliersDto = JsonConvert.DeserializeObject<SuppliersDto[]>(inputJson);

            if (suppliersDto != null)
            {
                foreach (var item in suppliersDto)
                {
                    if (!IsValid(item))
                    {
                        continue;
                    }

                    bool isPropValid = bool
                        .TryParse(item.IsImporter, out bool isImporterValue);

                    if (!isPropValid)
                    {
                        continue;
                    }

                    Supplier supplier = new Supplier()
                    {
                        Name = item.Name,
                        IsImporter = isImporterValue
                    };

                    suppliers.Add(supplier);
                }

                context.Suppliers.AddRange(suppliers);
                context.SaveChanges();
            }

            return $"Successfully imported {suppliers.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            ICollection<Part> parts = new List<Part>();

            ICollection<int> existingSupplierIds = context.Suppliers
                .Select(s => s.Id)
                .ToList();

            IEnumerable<PartsDto>? partsDtos = JsonConvert.DeserializeObject<PartsDto[]>(inputJson);

            if (partsDtos != null)
            {
                foreach(PartsDto dto in partsDtos)
                {
                    if (!IsValid(dto))
                    {
                        continue;
                    }

                    bool isPriceValid = decimal
                        .TryParse(dto.Price, out decimal priceValue);

                    bool isQuantityValid = int
                        .TryParse(dto.Quantity, out int quantityValue);

                    bool isSupplierIdValid = int
                        .TryParse(dto.SupplierId, out int supplierIdValue);

                    if (!isPriceValid || !isQuantityValid || !isSupplierIdValid || !existingSupplierIds.Contains(supplierIdValue))
                    {
                        continue;
                    }

                    Part part = new Part()
                    {
                        Name = dto.Name,
                        Price = priceValue,
                        Quantity = quantityValue,
                        SupplierId = supplierIdValue
                    };

                    parts.Add(part);
                }

                context.Parts.AddRange(parts);

                context.SaveChanges();
            }

            return $"Successfully imported {parts.Count}.";
        }

        //public static string ImportCars(CarDealerContext context, string inputJson)
        //{
        //    ICollection<Car> cars = new List<Car>();
        //    ICollection<PartCar> partsCars = new List<PartCar>();

        //    ICollection<int> existingPartIds = context.Parts
        //        .Select(p => p.Id)
        //        .ToList();

        //    IEnumerable<CarsDto>? carsDtos = JsonConvert.DeserializeObject<CarsDto[]>(inputJson);

        //    if (carsDtos != null)
        //    {
        //        foreach(CarsDto dto in carsDtos)
        //        {
        //            if (!IsValid(dto))
        //            {
        //                continue;
        //            }

        //            bool isTraveledDistanceValid = long
        //                .TryParse(dto.TraveledDistance, out long traveledDistanceValue);


        //            if (!isTraveledDistanceValid)
        //            {
        //                continue;
        //            }

        //            Car car = new()
        //            {
        //                Make = dto.Make,
        //                Model = dto.Model,
        //                TraveledDistance = traveledDistanceValue,
        //            };

        //            cars.Add(car);

        //            ICollection<int> partsIds = dto.PartsId
        //                .Distinct()
        //                .Where(id => existingPartIds.Contains(id))
        //                .ToArray();

        //            foreach (int partId in partsIds)
        //            {
        //                if (!existingPartIds.Contains(partId))
        //                {
        //                    continue;
        //                }

        //                PartCar currPartCar = new PartCar()
        //                {
        //                    Car = car,
        //                    PartId = partId
        //                };

        //                partsCars.Add(currPartCar);
        //            }
        //        }

        //        context.Cars.AddRange(cars);
        //        context.PartsCars.AddRange(partsCars);

        //        context.SaveChanges();
        //    }

        //    return $"Successfully imported {cars.Count}.";
        //}

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            ICollection<Car> carsToImport = new List<Car>();
            ICollection<PartCar> partsCarsToImport = new List<PartCar>();

            IEnumerable<CarsDto>? carDtos = JsonConvert
                .DeserializeObject<CarsDto[]>(inputJson);
            if (carDtos != null)
            {
                foreach (CarsDto carDto in carDtos)
                {
                    if (!IsValid(carDto))
                    {
                        continue;
                    }

                    Car newCar = new Car()
                    {
                        Make = carDto.Make,
                        Model = carDto.Model,
                        TraveledDistance = carDto.TraveledDistance
                    };
                    carsToImport.Add(newCar);

                    foreach (int partId in carDto.PartsId.Distinct())
                    {
                        if (!context.Parts.Any(p => p.Id == partId))
                        {
                            continue;
                        }

                        PartCar newPartCar = new PartCar()
                        {
                            PartId = partId,
                            Car = newCar
                        };
                        partsCarsToImport.Add(newPartCar);
                    }
                }

                //context.Cars.AddRange(carsToImport);
                context.PartsCars.AddRange(partsCarsToImport);

                context.SaveChanges();
            }

            return $"Successfully imported {carsToImport.Count}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            ICollection<Customer> customers = new List<Customer>();

            IEnumerable<CustomerDto>? customersDtos = 
                JsonConvert.DeserializeObject<CustomerDto[]>(inputJson);

            if (customersDtos != null)
            {
                foreach (CustomerDto customerDto in customersDtos)
                {
                    if (!IsValid(customerDto))
                    {
                        continue;
                    }

                    bool isBirthDateValid = DateTime
                        .TryParse(customerDto.BirthDate, out DateTime birthDateValue);

                    bool isYoungDriverValid = bool
                        .TryParse(customerDto.IsYoungDriver, out bool isYoungDriverValue);

                    if (!isBirthDateValid || !isYoungDriverValid)
                    {
                        continue;
                    }

                    Customer customer = new Customer()
                    {
                        Name = customerDto.Name,
                        BirthDate = birthDateValue,
                        IsYoungDriver = isYoungDriverValue
                    };

                    customers.Add(customer);
                }

                context.Customers.AddRange(customers);

                context.SaveChanges();
            }

            return $"Successfully imported {customers.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            ICollection<Sale> sales = new List<Sale>();
            ICollection<int> existingCarIds = context.Cars
                .Select(c => c.Id)
                .ToList();
            ICollection<int> existingCustomerIds = context.Customers
                .Select(c => c.Id)
                .ToList();

            IEnumerable<SaleDto>? saleDtos =
                JsonConvert.DeserializeObject<SaleDto[]>(inputJson);

            if (saleDtos != null)
            {
                foreach (SaleDto saleDto in saleDtos)
                {
                    if (!IsValid(saleDto))
                    {
                        continue;
                    }

                    bool isCarIdValid = int
                        .TryParse(saleDto.CarId, out int carIdValue);

                    bool isCustomerIdValid = int
                        .TryParse(saleDto.CustomerId, out int customerIdValue);

                    bool isDiscountValid = decimal
                        .TryParse(saleDto.Discount, out decimal discountValue);

                    if (!isCarIdValid || !isCustomerIdValid || 
                        !existingCarIds.Contains(carIdValue) || 
                        !existingCustomerIds.Contains(customerIdValue))
                    {
                        continue;
                    }

                    Sale sale = new Sale()
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

            return $"Successfully imported {sales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .AsNoTracking()
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .ToArray()
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = c.IsYoungDriver
                });

            var customersJson = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return customersJson;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .AsNoTracking()
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();

            var jsonCars = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return jsonCars;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .AsNoTracking()
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            var jsonSuppliers = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return jsonSuppliers;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .AsNoTracking()
                .Include(c => c.PartsCars)
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance
                    },
                    parts = c.PartsCars
                                .Select(pc => new
                                {
                                    Name = pc.Part.Name,
                                    Price = pc.Part.Price
                                })
                                .ToArray()
                })
                .ToArray();

            var carsFormatted = cars
                .Select(cf => new
                {
                    car = cf.car,
                    parts = cf.parts
                            .Select(cfp => new
                            {
                                cfp.Name,
                                Price = cfp.Price.ToString("F2")
                            })
                });

            var jsonCars = JsonConvert.SerializeObject(carsFormatted, Formatting.Indented);

            return jsonCars;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .AsNoTracking()
                .Where(c => c.Sales.Count >= 1)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales
                        .Select(s => s.Car)
                        .SelectMany(ca => ca.PartsCars)
                        .Sum(p => p.Part.Price)
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToArray();

            var jsonCustomers = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return jsonCustomers;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .AsNoTracking()
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount,
                    price = s.Car.PartsCars
                                .Sum(pc => pc.Part.Price),
                    priceWithDiscount = s.Car.PartsCars
                                .Sum(pc => pc.Part.Price) * (1 - (s.Discount / 100))
                })
                .Take(10)
                .ToArray();

            var salesFormatted = sales
                .Select(s => new
                {
                    car = s.car,
                    customerName = s.customerName,
                    discount = s.discount.ToString("F2"),
                    price = s.price.ToString("F2"),
                    priceWithDiscount = s.priceWithDiscount.ToString("F2")
                });

            var jsonCars = JsonConvert.SerializeObject(salesFormatted, Formatting.Indented);

            return jsonCars;
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(dto, validationContext, validationResults);
        }
    }
}