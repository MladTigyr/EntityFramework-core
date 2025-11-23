namespace Invoices.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using static Common.EntityValidation;
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxClientNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(MaxClientNumberVatLength)]
        public string NumberVat { get; set; } = null!;

        public virtual ICollection<Invoice> Invoices { get; set; }
            = new List<Invoice>();

        public virtual ICollection<Address> Addresses { get; set; }
            = new List<Address>();

        public virtual ICollection<ProductClient> ProductsClients { get; set; }
            = new List<ProductClient>();
    }
}
