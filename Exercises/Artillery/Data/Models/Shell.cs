namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class Shell
    {
        [Key]
        public int Id { get; set; }

        public double ShellWeight { get; set; }

        [Required]
        [MaxLength(MaxShellCaliberLength)]
        public string Caliber { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; }
            = new List<Gun>();
    }
}
