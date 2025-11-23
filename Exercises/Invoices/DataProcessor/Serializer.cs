namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto.Xml;
    using Invoices.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Primitives;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            var clients = new
            {
                Client = context.Clients
                .AsNoTracking()
                .Where(c => c.Invoices.Any(i => i.IssueDate > date))
                .Select(c => new
                {
                    InvoicesCount = c.Invoices.Count,
                    ClientName = c.Name,
                    VatNumber = c.NumberVat,
                    Invoices = c.Invoices
                        .OrderBy(i => i.IssueDate)
                        .ThenByDescending(i => i.DueDate)
                        .Select(i => new
                        {
                            InvoiceNumber = i.Number,
                            InvoiceAmount = i.Amount,
                            DueDate = i.DueDate,
                            Currency = i.CurrencyType,
                        })
                        .ToArray()
                })
                .OrderByDescending(c => c.InvoicesCount)
                .ThenBy(c => c.ClientName)
                .ToArray()
            };

            ExportClientRootDto exportDtos = new ExportClientRootDto()
            {
                Client = clients.Client
                    .Select(c => new ExportXmlClientDetailsDto
                    {
                        InvoicesCount = c.InvoicesCount,
                        ClientName = c.ClientName,
                        VatNumber = c.VatNumber,
                        Invoice = c.Invoices
                            .Select(i => new ExportInvoiceDetailsDto
                            {
                                InvoiceNumber = i.InvoiceNumber,
                                InvoiceAmount = i.InvoiceAmount.ToString("F2"),
                                DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture),
                                Currency = i.Currency.ToString(),
                            })
                            .ToArray(),
                    })
                    .ToArray()
            };

            string result = XmlSerializeWrapper.Serialize(exportDtos, "Clients");

            return result;
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var products = context.Products
            .AsNoTracking()
            .Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
            .Select(p => new
            {
                Name = p.Name,
                Price = p.Price.ToString("F2"),
                Category = p.CategoryType.ToString(),
                Clients = p.ProductsClients
                    .Where(pc => pc.Client.Name.Length >= nameLength)
                    .Select(pc => new
                    {
                        Name = pc.Client.Name,
                        NumberVat = pc.Client.NumberVat,
                    })
                    .OrderBy(c => c.Name)
                    .ToArray()
            })
            .OrderByDescending(p => p.Clients.Count())
            .ThenBy(p => p.Name)
            .Take(5)
            .ToArray();

            string result = JsonConvert
                .SerializeObject(products, Formatting.Indented);

            return result;
        }
    }
}