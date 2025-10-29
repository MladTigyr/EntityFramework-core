using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChampionsLeagueMiniGame.Data.Models
{
    using static Common.EntityValidation.Nationality;
    public class Nationality
    {
        [Key]
        public int NationalityId { get; set; }

        [Required]
        [MaxLength(NationalityMaxLength)]
        [Unicode(true)]
        public string Name { get; set; } = null!;

        public string Abriviation => 
            Name.Length >= 3 ? Name.Substring(0, 3).ToUpper() : Name.ToUpper();

        public virtual ICollection<Player> Players { get; set; } = new HashSet<Player>();

        public virtual ICollection<Team> Teams { get; set; } = new HashSet<Team>();
    }
}