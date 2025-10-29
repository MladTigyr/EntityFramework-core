using ChampionsLeagueMiniGame.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ChampionsLeagueMiniGame.Data.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        public PositionType PositionType { get; set; }

        public virtual ICollection<Player> Players { get; set; } = new List<Player>();
    }
}