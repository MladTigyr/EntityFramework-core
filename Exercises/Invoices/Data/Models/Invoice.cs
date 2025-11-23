namespace Invoices.Data.Models
{
    using Invoices.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        public int Number { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public decimal Amount { get; set; }

        public CurrencyType CurrencyType { get; set; }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; } = null!;
    }
}