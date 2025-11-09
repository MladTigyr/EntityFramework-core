using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //string jsonFileDirPath = Path
            //    .Combine(Directory.GetCurrentDirectory(), "../../../Datasets/");
            //string jsonFileName = "categories-products.json";
            //string jsonFileText = File
            //    .ReadAllText(jsonFileDirPath + jsonFileName);

            //string result = ImportCategoryProducts(context, jsonFileText);

            //Console.WriteLine(result);

            Console.WriteLine(GetUsersWithProducts(context));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ICollection<User> usersToImport = new List<User>();

            IEnumerable<UserDto>? userDtos = JsonConvert.DeserializeObject<UserDto[]>(inputJson);

            if (userDtos != null)
            {
                foreach (UserDto userDto in userDtos)
                {
                    if (!IsValid(userDto))
                    {
                        continue;
                    }

                    int? parsedAge = null;

                    if(!string.IsNullOrEmpty(userDto.Age))
                    {
                        if(int.TryParse(userDto.Age, out int age))
                        {
                            parsedAge = age;
                        }
                    }

                    User user = new User()
                    {
                        FirstName = userDto.FirstName,
                        LastName = userDto.LastName,
                        Age = parsedAge
                    };

                    usersToImport.Add(user);
                }

                context.Users.AddRange(usersToImport);
                context.SaveChanges();
            }

            return $"Successfully imported {usersToImport.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ICollection<Product> productsToImport = new List<Product>();

            ICollection<int> existingSellerIds = context
                .Users
                .AsNoTracking()
                .Select(u => u.Id)
                .ToList();

            IEnumerable<ProductDto>? productDtos = JsonConvert
                .DeserializeObject<ProductDto[]>(inputJson);

            if (productDtos != null)
            {
                foreach (ProductDto productDto in productDtos)
                {
                    if (!IsValid(productDto))
                    {
                        continue;
                    }

                    bool isPriceValid = decimal
                        .TryParse(productDto.Price, out decimal parsedPrice);

                    if (!isPriceValid)
                    {
                        continue;
                    }

                    bool isSellerIdValid = int.
                        TryParse(productDto.SellerId, out int sellerId);

                    if (!isSellerIdValid || !existingSellerIds.Contains(sellerId))
                    {
                        continue;
                    }

                    int? buyerId = null;

                    if (!string.IsNullOrEmpty(productDto.BuyerId))
                    {
                        if (int.TryParse(productDto.BuyerId, out int parsedBuyerId))
                        {
                            buyerId = parsedBuyerId;
                        }
                    }

                    Product product = new Product()
                    {
                        Name = productDto.Name,
                        Price = parsedPrice,
                        SellerId = sellerId,
                        BuyerId = buyerId
                    };

                    productsToImport.Add(product);
                }

                context.Products.AddRange(productsToImport);

                context.SaveChanges();
            }

            return $"Successfully imported {productsToImport.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            ICollection<Category> categoriesToImport = new List<Category>();

            IEnumerable<CategoryDto>? categoryDtos = JsonConvert
                .DeserializeObject<CategoryDto[]>(inputJson);

            if (categoryDtos != null)
            {
                foreach (CategoryDto categoryDto in categoryDtos)
                {
                    if (!IsValid(categoryDto))
                    {
                        continue;
                    }

                    Category category = new Category()
                    {
                        Name = categoryDto.Name
                    };

                    categoriesToImport.Add(category);
                }

                context.Categories.AddRange(categoriesToImport);

                context.SaveChanges();
            }

            return $"Successfully imported {categoriesToImport.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            ICollection<CategoryProduct> categoryProductsToImport
                = new List<CategoryProduct>();

            ICollection<int> existingCategoryIds = context
                .Categories
                .AsNoTracking()
                .Select(c => c.Id)
                .ToList();

            ICollection<int> existingProductIds = context.Products
                .AsNoTracking()
                .Select(p => p.Id)
                .ToArray();

            IEnumerable<CategoryProductDto>? categoryProductDtos = JsonConvert
                .DeserializeObject<CategoryProductDto[]>(inputJson);

            if (categoryProductDtos != null)
            {
                foreach (CategoryProductDto categoryProductDto in categoryProductDtos)
                {
                    if(!IsValid(categoryProductDtos))
                    {
                        continue;
                    }

                    bool isCategoryIdValid = int
                        .TryParse(categoryProductDto.CategoryId, out int categoryId);

                    if (!isCategoryIdValid || !existingCategoryIds.Contains(categoryId))
                    {
                        continue;
                    }

                    bool isProductIdValid = int
                        .TryParse(categoryProductDto.ProductId, out int productId);

                    if (!isProductIdValid || !existingProductIds.Contains(productId))
                    {
                        continue;
                    }

                    CategoryProduct categoryProduct = new CategoryProduct()
                    {
                        CategoryId = categoryId,
                        ProductId = productId
                    };

                    categoryProductsToImport.Add(categoryProduct);
                }

                context.CategoriesProducts.AddRange(categoryProductsToImport);

                context.SaveChanges();
            }

            return $"Successfully imported {categoryProductsToImport.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context.Products
                .AsNoTracking()
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    p.Seller.FirstName,
                    p.Seller.LastName
                })
                .ToArray();

            var inMemoryOperations = productsInRange
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = string.Concat(p.FirstName, " ", p.LastName)
                })
                .ToArray();

            var jsonResult = JsonConvert
                .SerializeObject(inMemoryOperations, Formatting.Indented);

            return jsonResult;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var soldProducts = context.Users
                .AsNoTracking()
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName =  u.LastName,
                    soldProducts = u.ProductsSold
                        .Where(p => p.BuyerId != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer!.FirstName,
                            buyerLastName = p.Buyer!.LastName
                        })
                        .ToArray()
                })
                .ToArray();

            var jsonResult = JsonConvert
                .SerializeObject(soldProducts, Formatting.Indented);

            return jsonResult;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var getCategories = context.Categories
                .AsNoTracking()
                .OrderByDescending(c => c.CategoriesProducts.Count)
                .Select(c => new 
                {
                    Category = c.Name,
                    ProductsCount = c.CategoriesProducts.Count,
                    AveragePrice = c.CategoriesProducts.Select(cp => cp.Product.Price).Average(),
                    TotalRevenue = c.CategoriesProducts.Select(cp => cp.Product.Price).Sum()
                })
                .ToArray();

            var inMemoryOperations = getCategories
                .Select(c => new
                {
                    category = c.Category,
                    productsCount = c.ProductsCount,
                    averagePrice = c.AveragePrice.ToString("F2"),
                    totalRevenue = c.TotalRevenue.ToString("F2")
                })
                .ToArray();

            var jsonResult = JsonConvert
                .SerializeObject(inMemoryOperations, Formatting.Indented);

            return jsonResult;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var usersWithProducts = context.Users
                .AsNoTracking()
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .OrderByDescending(u => u.ProductsSold.Count(p => p.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count(p => p.BuyerId != null),
                        products = u.ProductsSold
                            .Where(p => p.BuyerId != null)
                            .Select(p => new
                            {
                                name = p.Name,
                                price = p.Price
                            })
                            .ToArray()
                    }
                })
                .ToArray();
            
            var inMemoryOperations = new
            {
                usersCount = usersWithProducts.Length,
                users = usersWithProducts
            };

            var jsonResult = JsonConvert
                .SerializeObject(inMemoryOperations, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            return jsonResult;
        }

        private static bool IsValid(object obj)
        {
            // These two variables are required by the Validator.TryValidateObject method
            // We will not use them for now...
            // We are just using the Validation result (true or false)
            ValidationContext validationContext = new ValidationContext(obj);
            ICollection<ValidationResult> validationResults
                = new List<ValidationResult>();

            return Validator
                .TryValidateObject(obj, validationContext, validationResults);
        }
    }
}