namespace ProductShop
{
    using Microsoft.EntityFrameworkCore;
    using ProductShop.Data;
    using ProductShop.DTOs.Export.ExportCategoriesByProductsCount;
    using ProductShop.DTOs.Export.ExportProductsInRange;
    using ProductShop.DTOs.Export.ExportSoldProducts;
    using ProductShop.DTOs.Export.ExportUsersandProducts;
    using ProductShop.DTOs.Import;
    using ProductShop.Models;
    using ProductShop.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Linq;

    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            //Console.WriteLine(CreateAndSeedDatabase(context));

            string result = GetUsersWithProducts(context);

            Console.WriteLine(result);
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            ICollection<User> users = new List<User>();

            IEnumerable<ImportUserDto>? importUserDtos = XmlSerializeWrapper
                .Deserialize<ImportUserDto[]>(inputXml, "Users");

            if (importUserDtos != null)
            {
                foreach (ImportUserDto userDto in importUserDtos)
                {
                    if (!IsValid(userDto))
                    {
                        continue;
                    }

                    bool? isAgeValid = int
                        .TryParse(userDto.Age, out int ageValue);

                    User user = new User()
                    {
                        FirstName = userDto.FirstName,
                        LastName = userDto.LastName,
                        Age = ageValue,
                    };

                    users.Add(user);
                }

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            ICollection<Product> products = new List<Product>();
            ICollection<int> sellers = context.Users
                .AsNoTracking()
                .Select(s => s.Id)
                .ToArray();

            IEnumerable<ImportProductDto>? importProductDtos = XmlSerializeWrapper
                .Deserialize<ImportProductDto[]>(inputXml, "Products");

            if (importProductDtos != null)
            {
                foreach (ImportProductDto productDto in importProductDtos)
                {
                    if (!IsValid(productDto))
                    {
                        continue;
                    }

                    bool isPriceValid = decimal
                        .TryParse(productDto.Price, out decimal priceValue);

                    bool isSellerIdValid = int
                        .TryParse(productDto.SellerId, out int sellerIdValue);

                    int? isNull = null;

                    bool isBuyerIdValid = int
                        .TryParse(productDto.BuyerId, out int buyerIdValue);

                    if (isBuyerIdValid)
                    {
                        isNull = buyerIdValue;
                    }

                    if (!isPriceValid && !isSellerIdValid && !sellers.Contains(sellerIdValue))
                    {
                        continue;
                    }

                    Product product = new()
                    {
                        Name = productDto.Name,
                        Price = priceValue,
                        SellerId = sellerIdValue,
                        BuyerId = isNull
                    };

                    products.Add(product);
                }

                context.Products.AddRange(products);
                context.SaveChanges();
            }

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            ICollection<Category> categories = new List<Category>();

            IEnumerable<ImportCategoryDto>? importCategoryDtos = XmlSerializeWrapper
                .Deserialize<ImportCategoryDto[]>(inputXml, "Categories");

            if (importCategoryDtos != null)
            {
                foreach (ImportCategoryDto categoryDto in importCategoryDtos)
                {
                    if (!IsValid(categoryDto))
                    {
                        continue;
                    }

                    Category category = new()
                    {
                        Name = categoryDto.Name,
                    };

                    categories.Add(category);
                }

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            ICollection<CategoryProduct> categoryProducts = new List<CategoryProduct>();
            ICollection<int> categoriesIds = context.Categories
                .AsNoTracking()
                .Select(c => c.Id)
                .ToArray();
            ICollection<int> productsIds = context.Products
               .AsNoTracking()
               .Select(p => p.Id)
               .ToArray();

            IEnumerable<ImportCategoryProductDto>? importCategoryProductDtos = XmlSerializeWrapper
                .Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

            if (importCategoryProductDtos != null)
            {
                foreach (ImportCategoryProductDto categoryProductDto in importCategoryProductDtos)
                {
                    if(!IsValid(categoryProductDto))
                    {
                        continue;
                    }

                    bool isCategoryValid = int
                        .TryParse(categoryProductDto.CategoryId, out int categoryValue);

                    bool isProductValid = int
                        .TryParse(categoryProductDto.ProductId, out int productValue);

                    if (!isCategoryValid || !isProductValid || 
                        !categoriesIds.Contains(categoryValue) || !productsIds.Contains(productValue))
                    {
                        continue;
                    }

                    CategoryProduct categoryProduct = new CategoryProduct()
                    {
                        CategoryId = categoryValue,
                        ProductId = productValue,
                    };

                    categoryProducts.Add(categoryProduct);
                }

                context.CategoryProducts.AddRange(categoryProducts);
                context.SaveChanges();
            }

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            ExportProductRootDto exportDto = new ExportProductRootDto()
            {
                Product = context.Products
                    .AsNoTracking()
                    .Where(p => p.Price >= 500 && p.Price <= 1000)
                    .Select(p => new ExportProductDetailsDto
                    {
                        Name = p.Name,
                        Price = p.Price,
                        Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName,
                    })
                    .OrderBy(p => p.Price)
                    .Take(10)
                    .ToArray()
            };

            string result = XmlSerializeWrapper
                .Serialize(exportDto, "Products");

            return result;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            ExportUserRootDto exportDto = new ExportUserRootDto()
            {
                User = context.Users
                    .AsNoTracking()
                    .Where(u => u.ProductsSold.Count >= 1)
                    .Select(u => new ExportUserDetailsDto
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        SoldProducts = u.ProductsSold
                            .Select(sp => new ExportUserSoldProducts
                            {
                                Name = sp.Name,
                                Price = sp.Price
                            })
                            .ToArray()
                    })
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Take(5)
                    .ToArray()
            };

            string result = XmlSerializeWrapper
                .Serialize(exportDto, "Users");

            return result;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            ExportCategoriesRootDto exportDto = new ExportCategoriesRootDto()
            {
                Category = context.Categories
                    .AsNoTracking()
                    .Select(c => new ExportCategoryDetailsDto
                    {
                        Name = c.Name,
                        Count = c.CategoryProducts.Count,
                        AveragePrice = c.CategoryProducts.Average(p => p.Product.Price),
                        TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
                    })
                    .OrderByDescending(c => c.Count)
                    .ThenBy(c => c.TotalRevenue)
                    .ToArray()
            };

            string result = XmlSerializeWrapper
                .Serialize(exportDto, "Categories");

            return result;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            ExportUserAndProductsRootDto exportDto = new()
            {
                Count = context.Users
                    .AsNoTracking()
                    .Count(u => u.ProductsSold.Any()),
                User = context.Users
                    .AsNoTracking()
                    .Where(u => u.ProductsSold.Count >= 1)
                    .Select(u => new ExportUserAndProductsDetailsDto
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProducts =  new ExportUserSoldProductsDto
                            {
                                Count = u.ProductsSold.Count,
                                Product = u.ProductsSold
                                .Select(p => new ExportUserSoldProductsDetailsDto
                                {
                                    Name = p.Name,
                                    Price = p.Price,
                                })
                                .OrderByDescending(ps => ps.Price)
                                .ToArray()
                            }
                    })
                    .OrderByDescending(u => u.SoldProducts.Count)
                    .Take(10)
                    .ToArray()
            };

            string result = XmlSerializeWrapper
                .Serialize(exportDto, "Users");

            return result;
        }

        private static string CreateAndSeedDatabase(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            string currPath = "users.xml";
            string path = "../../../Datasets/";

            string combinedPath = File.ReadAllText(Path.Combine(path, currPath));

            string result = ImportUsers(context, combinedPath);

            currPath = "products.xml";
            combinedPath = File.ReadAllText(Path.Combine(path, currPath));

            result = ImportProducts(context, combinedPath);

            combinedPath = File.ReadAllText(Path.Combine(path, "categories.xml"));

            result = ImportCategories(context, combinedPath);

            combinedPath = File.ReadAllText(Path.Combine(path, "categories-products.xml"));

            result = ImportCategoryProducts(context, combinedPath);

            return result;
        }

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new(obj);

            ICollection<ValidationResult> validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults);
        }
    }
}