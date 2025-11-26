namespace Footballers.Data.Models
{
    public class TeamFootballer
    {
        public int TeamId { get; set; }

        public virtual Team Team { get; set; } = null!;

        public int FootballerId { get; set; }

        public virtual Footballer Footballer { get; set; } = null!;
    }
}