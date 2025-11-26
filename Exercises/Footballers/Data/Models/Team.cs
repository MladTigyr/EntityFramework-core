namespace Footballers.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxTeamNameLength)]
        [RegularExpression(TeamRegex)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(MaxTeamNationalityLength)]
        public string Nationality { get; set; } = null!;

        public int Trophies { get; set; }

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; } 
            = new List<TeamFootballer>();
    }
}