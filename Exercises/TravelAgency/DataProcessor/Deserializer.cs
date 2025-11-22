using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos.Json;
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
            StringBuilder sb = new StringBuilder();

            ICollection<Booking> bookings = new List<Booking>();

            //ICollection<Customer> bookingsNames = context.Customers
            //    .AsNoTracking()
            //    .Where(c => c.)

            IEnumerable<ImportBookingsDto>? importBookingsDtos = JsonConvert
                .DeserializeObject<ImportBookingsDto[]>(jsonString);

            if (importBookingsDtos != null)
            {
                foreach (ImportBookingsDto bookingDto in importBookingsDtos)
                {
                    if (!IsValid(bookingDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isDateValid = DateTime
                        .TryParseExact(bookingDto.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out DateTime dateTime);

                    if (!isDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    int customerId = context.Customers
                        .Where(c => c.FullName == bookingDto.CustomerName)
                        .Select(c => c.Id)
                        .FirstOrDefault();

                    int tourPackageId = context.TourPackages
                        .Where(t => t.PackageName == bookingDto.TourPackageName)
                        .Select(t => t.Id)
                        .FirstOrDefault();

                    Booking booking = new Booking()
                    {
                        BookingDate = dateTime,
                        CustomerId = customerId,
                        TourPackageId = tourPackageId
                    };

                    bookings.Add(booking);

                    sb.AppendLine(string.Format(SuccessfullyImportedBooking, bookingDto.TourPackageName , booking.BookingDate.ToString("yyyy-MM-dd")));
                }

                context.Bookings.AddRange(bookings);
                context.SaveChanges(); 
            }

            return sb.ToString();
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
