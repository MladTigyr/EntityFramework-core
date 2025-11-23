namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Castle.Core.Internal;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto.Json;
    using Invoices.DataProcessor.ImportDto.Xml;
    using Invoices.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Client> clients = new List<Client>();

            IEnumerable<ImportClientDetailsDto>? importClientDetailsDtos = XmlSerializeWrapper
                .Deserialize<ImportClientDetailsDto[]>(xmlString, "Clients");

            if (importClientDetailsDtos != null)
            {
                foreach (ImportClientDetailsDto clientDto in importClientDetailsDtos)
                {
                    if (!IsValid(clientDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Client client = new Client()
                    {
                        Name = clientDto.Name,
                        NumberVat = clientDto.NumberVat,
                    };

                    ICollection<Address> addresses = new List<Address>();

                    foreach (ImportAddressDetailsDto address in clientDto.Address)
                    {
                        if (!IsValid(address))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        if (address.StreetName.IsNullOrEmpty())
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        Address currAddress = new Address()
                        {
                            StreetName = address.StreetName,
                            StreetNumber = address.StreetNumber,
                            PostCode = address.PostCode,
                            City = address.City,
                            Country = address.Country,
                            Client = client
                        };

                        addresses.Add(currAddress);
                    }

                    client.Addresses = addresses;
                    clients.Add(client);

                    sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
                }

                context.Clients.AddRange(clients);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Invoice> invoices = new List<Invoice>();
            ICollection<int> clientIds = context.Clients
                .AsNoTracking()
                .Select(client => client.Id)
                .ToArray();

            IEnumerable<ImportInvoiceDto>? importInvoiceDtos = JsonConvert
                .DeserializeObject<ImportInvoiceDto[]>(jsonString);

            if (importInvoiceDtos != null)
            {
                foreach (ImportInvoiceDto invoiceDto in importInvoiceDtos)
                {
                    if (!IsValid(invoiceDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isIssueDateValid = DateTime
                        .TryParseExact(invoiceDto.IssueDate, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateTime issueDateValue);

                    bool isDueDateValid = DateTime
                        .TryParseExact(invoiceDto.DueDate, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateTime dueDateValue);

                    if (!isIssueDateValid || !isDueDateValid || !clientIds.Contains(invoiceDto.ClientId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Invoice invoice = new Invoice()
                    {
                        Number = invoiceDto.Number,
                        IssueDate = issueDateValue,
                        DueDate = dueDateValue,
                        Amount = invoiceDto.Amount,
                        CurrencyType = (CurrencyType)invoiceDto.CurrencyType,
                        ClientId = invoiceDto.ClientId,
                    };

                    if (invoice.IssueDate > invoice.DueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    invoices.Add(invoice);

                    sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));
                }

                context.Invoices.AddRange(invoices);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Product> products = new List<Product>();
            ICollection<int> clientIds = context.Clients
                .AsNoTracking()
                .Select(c => c.Id)
                .ToArray();

            IEnumerable<ImportProductDto>? importProductDtos = JsonConvert
                .DeserializeObject<ImportProductDto[]>(jsonString);

            if (importProductDtos != null)
            {
                foreach (ImportProductDto productDto in importProductDtos)
                {
                    if (!IsValid(productDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ICollection<int> uniqueIds = new HashSet<int>();

                    foreach (int currClientId in productDto.Clients)
                    {
                        uniqueIds.Add(currClientId);
                    }

                    productDto.Clients = uniqueIds.ToArray();

                    foreach (int id in productDto.Clients)
                    {
                        if (!clientIds.Contains(id))
                        {
                            sb.AppendLine(ErrorMessage);
                            uniqueIds.Remove(id);
                            continue;
                        }
                    }

                    productDto.Clients = uniqueIds.ToArray();

                    Product product = new Product()
                    {
                        Name = productDto.Name,
                        Price = productDto.Price,
                        CategoryType = (CategoryType)productDto.CategoryType,
                    };

                    ICollection<ProductClient> productClients = new List<ProductClient>();

                    foreach (int id in uniqueIds)
                    {
                        ProductClient productClient = new ProductClient()
                        {
                            Product = product,
                            ClientId = id,
                        };

                        productClients.Add(productClient);
                    }

                    product.ProductsClients = productClients;
                    products.Add(product);

                    sb.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name, productClients.Count));
                }

                context.Products.AddRange(products);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
