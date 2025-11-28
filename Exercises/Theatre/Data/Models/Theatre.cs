namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;
    public class Theatre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxTheatreNameLength)]
        public string Name { get; set; } = null!;

        public sbyte NumberOfHalls { get; set; }

        [Required]
        [MaxLength(MaxTheatreDirectorLength)]
        public string Director { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; }
            = new List<Ticket>();
    }
}
