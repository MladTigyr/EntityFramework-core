using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChampionsLeagueMiniGame.Data.Models
{
    using static Common.EntityValidation.Team;
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        [Unicode(true)]
        public string Name { get; set; } = null!;

        public int Overral => 
            Players.Sum(p => p.Rating) / Players.Count;

        public int NationalityId { get; set; }

        public virtual Nationality Nationality { get; set; } = null!;

        public virtual ICollection<Player> Players {  get; set; } = new HashSet<Player>();

        public virtual ICollection<Match> HomeMatches { get; set; } = new List<Match>();
        public virtual ICollection<Match> AwayMatches { get; set; } = new List<Match>();

        [NotMapped]
        public int Points =>
            HomeMatches.Sum(m => m.GetPointsForTeam(this)) +
            AwayMatches.Sum(m => m.GetPointsForTeam(this));
    }
}
