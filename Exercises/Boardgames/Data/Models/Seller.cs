namespace Boardgames.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;
    public class Seller
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxSellerNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(MaxSellerAddressLength)]
        public string Address { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        [RegularExpression(SellerWebsiteRegex)]
        public string Website { get; set; } = null!;

        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
            = new List<BoardgameSeller>();
    }
}
