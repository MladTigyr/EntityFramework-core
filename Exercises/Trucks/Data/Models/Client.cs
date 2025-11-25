namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxClientNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(MaxClientNationalityLength)]
        public string Nationality { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
            = new List<ClientTruck>();
    }
}
