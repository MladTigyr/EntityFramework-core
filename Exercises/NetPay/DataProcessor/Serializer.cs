using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using NetPay.Data;
using NetPay.Data.Models;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ExportDtos.Xml;
using NetPay.Utilities;
using Newtonsoft.Json;

namespace NetPay.DataProcessor
{
    public class Serializer
    {
        public static string ExportHouseholdsWhichHaveExpensesToPay(NetPayContext context)
        {
            var households = new 
            {
                Households = context.Households
                    .Where(h => h.Expenses.Any(e => e.PaymentStatus != PaymentStatus.Paid))
                    .Select(h => new
                    {
                        ContactPerson = h.ContactPerson,
                        Email = h.Email == null ? null : h.Email,
                        PhoneNumber = h.PhoneNumber,
                        Expenses = h.Expenses
                            .Where(e => e.PaymentStatus != PaymentStatus.Paid)
                            .Select(e => new
                            {
                                ExpenseName = e.ExpenseName,
                                Amount = e.Amount,
                                PaymentDate = e.DueDate,
                                ServiceName = e.Service.ServiceName,
                            })
                            .ToArray()
                    })
                    .OrderBy(h => h.ContactPerson)
                    .ToArray()
            };

            ExportHouseholdRootDto dtos = new ExportHouseholdRootDto()
            {
                Households = households.Households
                    .Select(h => new ExportHouseholdDetailsDto
                    {
                        ContactPerson = h.ContactPerson,
                        Email = h.Email,
                        PhoneNumber = h.PhoneNumber,
                        Expenses = h.Expenses
                            .Select(e => new ExportExpenseDetailsDto
                            {
                                ExpenseName = e.ExpenseName,
                                Amount = e.Amount.ToString("F2"),
                                PaymentDate = e.PaymentDate.ToString("yyyy-MM-dd"),
                                ServiceName = e.ServiceName,
                            })
                            .OrderBy(e => e.PaymentDate)
                            .ThenBy(e => e.Amount)
                            .ToArray()
                    })
                    .ToArray()
            };

            string result = XmlSerializeWrapper
                .Serialize(dtos, "Households");

            return result;
        }

        public static string ExportAllServicesWithSuppliers(NetPayContext context)
        {
            var dtos = context.Services
                .AsNoTracking()
                .Select(se => new
                {
                    ServiceName = se.ServiceName,
                    Suppliers = se.SuppliersServices
                        .Select(su => new
                        {
                            SupplierName = su.Supplier.SupplierName
                        })
                        .OrderBy(su => su.SupplierName)
                        .ToArray(),
                })
                .OrderBy(se => se.ServiceName)
                .ToArray();

            string result = JsonConvert
                .SerializeObject(dtos, Formatting.Indented);

            return result;
        }
    }
}
