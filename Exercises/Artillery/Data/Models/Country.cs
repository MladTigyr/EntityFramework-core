namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxCountryNameLength)]   
        public string CountryName { get; set; } = null!;

        public int ArmySize { get; set; }

        public virtual ICollection<CountryGun> CountriesGuns { get; set; }
            = new List<CountryGun>();
    }
}