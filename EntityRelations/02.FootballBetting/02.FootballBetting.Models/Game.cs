using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.FootballBetting.Models
{
    public class Game
    {
        //•	Game – GameId, HomeTeamId, AwayTeamId, HomeTeamGoals, AwayTeamGoals, HomeTeamBetRate, AwayTeamBetRate, DrawBetRate, DateTime, Result

        public int GameId { get; set; }

        public int HomeTeamId { get; set; }

        public virtual Team HomeTeam { get; set; } = null!;

        public int AwayTeamId { get; set; }

        public virtual Team AwayTeam { get; set; } = null!;

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public double HomeTeamBetRate { get; set; }

        public double AwayTeamBetRate { get; set; }

        public double DrawBetRate { get; set; }

        public DateTime DateTime { get; set; }

        public string Result { get; set; } = null!;

        public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; } = new List<PlayerStatistic>();

        public virtual ICollection<Bet> Bets { get; set; } = new List<Bet>();
    }
}
