namespace Medicines.Data.Models
{
    using Medicines.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using static EntityValidation;
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxPatientFullNameLength)]
        public string FullName { get; set; } = null!;

        public AgeGroup AgeGroup { get; set; }

        public Gender Gender { get; set; }

        public virtual ICollection<PatientMedicine> PatientsMedicines { get; set; }
            = new List<PatientMedicine>();
    }
}