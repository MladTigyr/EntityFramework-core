using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        //•	User – UserId, Username, Name, Password, Email, Balance

        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; } = new List<Bet>();
    }
}
