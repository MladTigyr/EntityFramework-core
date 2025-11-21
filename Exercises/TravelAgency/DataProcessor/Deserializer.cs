using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos.Xml;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Customer> customers = new List<Customer>();
            ICollection<string> customerNames = context.Customers
                .AsNoTracking()
                .Select(c => c.FullName)
                .ToArray();

            ICollection<string> customerEmails = context.Customers
                .AsNoTracking()
                .Select(c => c.Email)
                .ToArray();

            ICollection<string> customerPhones = context.Customers
                .AsNoTracking()
                .Select(c => c.PhoneNumber)
                .ToArray();

            IEnumerable<ImportCustomerDto>? importCustomerDtos = XmlWrapperSerializer
                .Deserialize<ImportCustomerDto[]>(xmlString, "Customers");

            if (importCustomerDtos != null)
            {
                foreach (ImportCustomerDto customerDto in importCustomerDtos)
                {
                    if (!IsValid(customerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (customerNames.Contains(customerDto.FullName) || customerEmails.Contains(customerDto.Email)
                        || customerPhones.Contains(customerDto.PhoneNumber))
                    {
                        sb.AppendLine(DuplicationDataMessage);
                        continue;
                    }

                    Customer customer = new Customer()
                    {
                        PhoneNumber = customerDto.PhoneNumber,
                        FullName = customerDto.FullName,
                        Email = customerDto.Email,
                    };

                    customers.Add(customer);

                    sb.AppendLine(string.Format(SuccessfullyImportedCustomer, customerDto.FullName));
                }

                context.Customers.AddRange(customers);
                context.SaveChanges();
            }

            return sb.ToString();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
