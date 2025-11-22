namespace Medicines.Data.Models
{
    using Medicines.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using static EntityValidation;

    public class Medicine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxMedicineNameLength)]
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public Category Category { get; set; }

        public DateTime ProductionDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        [Required]
        [MaxLength(MaxProducerMedicineLength)]
        public string Producer { get; set; } = null!;

        public int PharmacyId { get; set; }

        public virtual Pharmacy Pharmacy { get; set; } = null!;

        public ICollection<PatientMedicine> PatientsMedicines { get; set; }
            = new List<PatientMedicine>();
    }
}