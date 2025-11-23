namespace Invoices.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxAddressStreetNameLength)]
        public string StreetName { get; set; } = null!;

        public int StreetNumber { get; set; }

        [Required]
        public string PostCode { get; set; } = null!;

        [Required]
        [MaxLength(MaxAddressCityLength)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(MaxAddressCountryLength)]
        public string Country { get; set; } = null!;

        public int ClientId { get; set; }

        public virtual Client Client { get; set; } = null!;
    }
}