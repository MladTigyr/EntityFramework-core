using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Data.Models
{
    using static EntityValidation.EntityValidation;
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxCustomerFullNameLength)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(MaxCustomerEmailLength)]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(CustomerPhoneNumber)]
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Booking> Bookings { get; set; } 
            = new List<Booking>();
    }
}
