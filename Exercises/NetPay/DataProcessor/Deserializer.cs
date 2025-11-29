namespace NetPay.DataProcessor
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using NetPay.Data;
    using NetPay.Data.Models;
    using NetPay.Data.Models.Enums;
    using NetPay.DataProcessor.ImportDtos.Json;
    using NetPay.DataProcessor.ImportDtos.Xml;
    using NetPay.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedHousehold = "Successfully imported household. Contact person: {0}";
        private const string SuccessfullyImportedExpense = "Successfully imported expense. {0}, Amount: {1}";

        public static string ImportHouseholds(NetPayContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Household> households = new List<Household>();
            ICollection<string> contactPeople = context.Households
                .AsNoTracking()
                .Select(h => h.ContactPerson)
                .ToList();
            ICollection<string> emails = context.Households
                .Where(h => h.Email != null)
                .Select(h => h.Email)
                .ToList()!;
            ICollection<string> phones = context.Households
                .AsNoTracking()
                .Select(h => h.PhoneNumber)
                .ToList();

            IEnumerable<ImportHouseholdDto>? importHouseholdDtos = XmlSerializeWrapper
                .Deserialize<ImportHouseholdDto[]>(xmlString, "Households");

            if (importHouseholdDtos != null)
            {
                foreach (ImportHouseholdDto householdDto in importHouseholdDtos)
                {
                    if (!IsValid(householdDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (contactPeople.Contains(householdDto.ContactPerson) || emails.Contains(householdDto.Email)
                        || phones.Contains(householdDto.Phone))
                    {
                        sb.AppendLine(DuplicationDataMessage);
                        continue;
                    }

                    Household household = new Household()
                    {
                        ContactPerson = householdDto.ContactPerson,
                        Email = householdDto.Email,
                        PhoneNumber = householdDto.Phone,
                    };

                    households.Add(household);
                    contactPeople.Add(household.ContactPerson);
                    emails.Add(household.Email);
                    phones.Add(household.PhoneNumber);

                    sb.AppendLine(string.Format(SuccessfullyImportedHousehold, household.ContactPerson));
                }

                context.Households.AddRange(households);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportExpenses(NetPayContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Expense> expenses = new List<Expense>();
            ICollection<int> householdIds = context.Households
                .AsNoTracking()
                .Select(h => h.Id)
                .ToArray();
            ICollection<int> serviceIds = context.Services
                .AsNoTracking()
                .Select(s => s.Id)
                .ToArray();

            IEnumerable<ImportExpenseDto>? importExpenseDtos = JsonConvert
                .DeserializeObject<ImportExpenseDto[]>(jsonString);

            if (importExpenseDtos != null)
            {
                foreach (ImportExpenseDto expenseDto in importExpenseDtos)
                {
                    if (!IsValid(expenseDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;   
                    }

                    if (!householdIds.Contains(expenseDto.HouseholdId) || !serviceIds.Contains(expenseDto.ServiceId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isDateValid = DateTime
                        .TryParseExact(expenseDto.DueDate, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDateValue);

                    bool isEnumValid = Enum
                        .TryParse<PaymentStatus>(expenseDto.PaymentStatus, out PaymentStatus paymentStatusValue);

                    if (!isDateValid || !isEnumValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Expense expense = new Expense()
                    {
                        ExpenseName = expenseDto.ExpenseName,
                        Amount = expenseDto.Amount,
                        DueDate = dueDateValue,
                        PaymentStatus = paymentStatusValue,
                        HouseholdId = expenseDto.HouseholdId,
                        ServiceId = expenseDto.ServiceId,
                    };

                    expenses.Add(expense);

                    sb.AppendLine(string.Format(SuccessfullyImportedExpense, expense.ExpenseName, expense.Amount.ToString("F2")));
                }

                context.Expenses.AddRange(expenses);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            foreach(var result in validationResults)
            {
                string currvValidationMessage = result.ErrorMessage;
            }

            return isValid;
        }
    }
}
