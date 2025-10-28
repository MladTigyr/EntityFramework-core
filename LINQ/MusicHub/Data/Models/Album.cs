using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    using static Common.EntityValidation.Album;
    public class Album
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        [Unicode(true)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime ReleaseDate { get; set; }

        public int? ProducerId { get; set; }

        [NotMapped]
        public decimal Price => this.Songs.Sum(s => s.Price);

        public virtual Producer Producer { get; set; } = null!;

        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
