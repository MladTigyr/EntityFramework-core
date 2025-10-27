using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.FootballBetting.Models
{
    public class Town
    {
        public int TownId { get; set; }
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }

        public virtual Country Country { get; set; } = null!;

        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

        public virtual ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
