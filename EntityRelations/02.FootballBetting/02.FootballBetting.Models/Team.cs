using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.FootballBetting.Models
{
    public class Team
    {
        //TeamId, Name, LogoUrl, Initials (JUV, LIV, ARS…), Budget, PrimaryKitColorId, SecondaryKitColorId, TownId

        //private string initials;

        //public Team()
        //{
        //}

        //public Team(string initials)
        //{
        //    Initials = initials;
        //}

        public int TeamId { get; set; }

        public string Name { get; set; } = null!;

        public string? LogoUrl { get; set; }

        //public string Initials 
        //{
        //    get => initials;
        //    set
        //    {
        //        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value) || value.Length != 3)
        //        {
        //            throw new ArgumentException("Initials must be exactly 3 characters long.");
        //        }

        //        initials = value;
        //    } 
        //}

        public string Initials { get; set; } = null!;

        public decimal Budget { get; set; }

        public int PrimaryKitColorId { get; set; }

        public virtual Color PrimaryKitColor { get; set; } = null!;

        public int SecondaryKitColorId { get; set; }

        public virtual Color SecondaryKitColor { get; set; } = null!;

        public int TownId { get; set; }

        public virtual Town Town { get; set; } = null!;

        public virtual ICollection<Player> Players { get; set; } = new List<Player>();

        public virtual ICollection<Game> HomeGames { get; set; } = new List<Game>();

        public virtual ICollection<Game> AwayGames { get; set; } = new List<Game>();
    }
}
