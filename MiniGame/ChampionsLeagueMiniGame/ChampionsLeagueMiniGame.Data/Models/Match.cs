using ChampionsLeagueMiniGame.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChampionsLeagueMiniGame.Data.Models
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }

        public int HomeTeamId { get; set; }

        public virtual Team HomeTeam { get; set; } = null!;

        public int AwayTeamId { get; set; }

        public virtual Team AwayTeam { get; set; } = null!;

        public Result HomeTeamResult { get; set; }
        public Result AwayTeamResult => 
            HomeTeamResult == Result.Win ? Result.Lose : (HomeTeamResult == Result.Draw ? Result.Draw : Result.Win);

        public int GetPointsForTeam(Team team)
        {
            if (team.TeamId == HomeTeamId)
            {
                return HomeTeamResult == Result.Win ? 3 : (HomeTeamResult == Result.Draw ? 1 : 0);
            }

            return AwayTeamResult == Result.Win ? 3 : (AwayTeamResult == Result.Draw ? 1 : 0);
        }
    }
}
