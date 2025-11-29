namespace NetPay.Data.Models
{
    using NetPay.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxExpenseNameLength)]
        public string ExpenseName { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public int HouseholdId { get; set; }

        public virtual Household Household { get; set; } = null!;

        public int ServiceId { get; set; }

        public virtual Service Service { get; set; } = null!;
    }
}