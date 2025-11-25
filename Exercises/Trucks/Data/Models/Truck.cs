namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Trucks.Data.Models.Enums;
    using static Common.EntityValidation;

    public class Truck
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(TruckRegex)]
        public string RegistrationNumber { get; set; } = null!;

        [Required]
        public string VinNumber { get; set; } = null!;

        public int TankCapacity { get; set; }

        public int CargoCapacity { get; set; }

        public CategoryType CategoryType { get; set; }

        public MakeType MakeType { get; set; }

        public int DespatcherId { get; set; }

        public virtual Despatcher Despatcher { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
            = new List<ClientTruck>();
    }
}