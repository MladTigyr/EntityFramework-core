namespace Footballers.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;
    public class Coach
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxCoachNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        public string Nationality { get; set; } = null!;

        public virtual ICollection<Footballer> Footballers { get; set; }
            = new List<Footballer>();
    }
}
