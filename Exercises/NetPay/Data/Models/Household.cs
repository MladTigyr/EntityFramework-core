namespace NetPay.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;
    public class Household
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxHouseholdContactPersonLength)]
        public string ContactPerson { get; set; } = null!;

        [MaxLength(MaxHouseholdEmailLength)]
        public string? Email { get; set; }

        [Required]
        [RegularExpression(HouseholdRegex)]
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Expense> Expenses { get; set; } 
            = new List<Expense>();
    }
}
