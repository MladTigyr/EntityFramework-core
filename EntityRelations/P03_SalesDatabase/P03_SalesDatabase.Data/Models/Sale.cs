using System.ComponentModel.DataAnnotations;

namespace P03_SalesDatabase.Data.Models
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        [Required]
        public DateTime Date {  get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; } = null!;

        public int StoreId { get; set; }

        public virtual Store Store { get; set; } = null!;
    }
}