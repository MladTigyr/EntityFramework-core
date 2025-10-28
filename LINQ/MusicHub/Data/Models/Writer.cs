using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    using static Common.EntityValidation.Writer;
    public class Writer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        [Unicode(true)]
        public string Name { get; set; } = null!;

        [Unicode(true)]
        public string? Pseudonym { get; set; }

        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
