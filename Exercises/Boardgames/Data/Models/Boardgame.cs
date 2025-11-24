namespace Boardgames.Data.Models
{
    using Boardgames.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class Boardgame
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxBoardGameNameLength)]
        public string Name { get; set; } = null!;

        public double Rating { get; set; }

        public int YearPublished { get; set; }

        public CategoryType CategoryType { get; set; }

        [Required]
        public string Mechanics { get; set; } = null!;

        public int CreatorId { get; set; }

        public virtual Creator Creator { get; set; } = null!;

        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
            = new List<BoardgameSeller>();
    }
}