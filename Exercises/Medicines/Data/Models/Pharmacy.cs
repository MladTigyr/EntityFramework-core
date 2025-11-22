namespace Medicines.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static EntityValidation;

    public class Pharmacy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxPharmacyNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(PhoneNumberRegEx)]
        public string PhoneNumber { get; set; } = null!;

        public bool IsNonStop { get; set; }

        public virtual ICollection<Medicine> Medicines { get; set; }
            = new List<Medicine>();
    }
}
