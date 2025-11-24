namespace Boardgames.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class Creator
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxCreatorFirstNameLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(MaxCreatorLastNameLength)]
        public string LastName { get; set; } = null!;

        public virtual ICollection<Boardgame> Boardgames  { get; set; }
            = new List<Boardgame>();
    }
}
