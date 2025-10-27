using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data.Models
{
    using static Common.EntityValidation.Customer;
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        [Unicode(true)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(EmailMaxLength)]
        [Unicode(false)]
        public string Email { get; set; } = null!;

        [Required]
        public string CreditCardNumber { get; set; } = null!;

        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
