using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data.Models
{
    using static Common.EntityValidation.Product;
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        [Unicode(true)]
        public string Name { get; set; } = null!;

        [Column(TypeName = "real")]
        public float Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
