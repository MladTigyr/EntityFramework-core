namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxManufacturerNameLength)]
        public string ManufacturerName { get; set; } = null!;

        [Required]
        [MaxLength(MaxManufacturerFounded)]
        public string Founded { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; }
            = new List<Gun>();
    }
}
