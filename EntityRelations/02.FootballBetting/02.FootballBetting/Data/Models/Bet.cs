using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Bet
    {
        //•	Bet – BetId, Amount, Prediction, DateTime, UserId, GameId

        public int BetId { get; set; }
        public decimal Amount { get; set; }
        public Prediction Prediction { get; set; }
        public DateTime DateTime { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public int GameId { get; set; }
        public virtual Game Game { get; set; } = null!;
    }

    public enum Prediction
    {
        HomeWin,
        AwayWin,
        Draw
    }
}
