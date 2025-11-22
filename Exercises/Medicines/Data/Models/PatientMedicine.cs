namespace Medicines.Data.Models
{
    using static EntityValidation;
    public class PatientMedicine
    {
        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; } = null!;

        public int MedicineId { get; set; }

        public virtual Medicine Medicine { get; set; } = null!;
    }
}