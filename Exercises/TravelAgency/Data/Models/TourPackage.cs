using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Data.Models
{
    using static EntityValidation.EntityValidation;
    public class TourPackage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxPackageNameLength)]
        public string PackageName { get; set; } = null!;

        [MaxLength(MaxPackageDescriptionLength)]
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<TourPackageGuide> TourPackagesGuides { get; set; }
            = new List<TourPackageGuide>();

        public virtual ICollection<Booking> Bookings { get; set; } 
            = new List<Booking>();
    }
}
