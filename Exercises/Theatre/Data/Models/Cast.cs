namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class Cast
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxCastNameLength)]
        public string FullName { get; set; } = null!;

        public bool IsMainCharacter { get; set; }

        [Required]
        [RegularExpression(CastRegex)]
        public string PhoneNumber { get; set; } = null!;

        public int PlayId { get; set; }

        public virtual Play Play { get; set; } = null!;
    }
}