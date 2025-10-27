using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.FootballBetting.Models
{
    public class Player
    {
        //•	Player – PlayerId, Name, SquadNumber, IsInjured, PositionId , TeamId, TownId 

        public int PlayerId { get; set; }

        public string Name { get; set; } = null!;

        public int SquadNumber { get; set; }

        public bool IsInjured { get; set; }

        public int PositionId { get; set; }

        public virtual Position Position { get; set; } = null!;

        public int TeamId { get; set; }

        public virtual Team Team { get; set; } = null!;

        public int TownId { get; set; }

        public virtual Town Town { get; set; } = null!;

        public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; } = new List<PlayerStatistic>();
    }
}
