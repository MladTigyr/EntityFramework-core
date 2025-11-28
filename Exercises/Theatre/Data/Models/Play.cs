namespace Theatre.Data.Models
{
    using global::Theatre.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class Play
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxPlayTitleLength)]
        public string Title { get; set; } = null!;

        public TimeSpan Duration { get; set; }

        public float Rating { get; set; }

        public Genre Genre { get; set; }

        [Required]
        [MaxLength(MaxPlayDescriptionLength)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(MaxPlayScreeWriterLength)]
        public string Screenwriter { get; set; } = null!;

        public virtual ICollection<Cast> Casts { get; set; }
            = new List<Cast>();

        public virtual ICollection<Ticket> Tickets { get; set; }
            = new List<Ticket>();
    }
}
