using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.FootballBetting.Models
{
    public class Position
    {
        public int PositionId { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
