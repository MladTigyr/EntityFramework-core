using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            string jsonFileDirPath = Path
                .Combine(Directory.GetCurrentDirectory(), "../../../Datasets/");
            string jsonFileName = "categories-products.json";
            string jsonFileText = File
                .ReadAllText(jsonFileDirPath + jsonFileName);

            string result = ImportCategoryProducts(context, jsonFileText);

            Console.WriteLine(result);
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