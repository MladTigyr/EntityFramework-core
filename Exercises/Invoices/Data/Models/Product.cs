namespace Invoices.Data.Models
{
    using Invoices.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxProductNameLength)]
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public CategoryType CategoryType { get; set; }

        public virtual ICollection<ProductClient> ProductsClients { get; set; }
            = new List<ProductClient>();
    }
}
