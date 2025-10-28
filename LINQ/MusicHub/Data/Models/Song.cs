using Microsoft.EntityFrameworkCore;
using MusicHub.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    using static Common.EntityValidation.Song;
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        [Unicode(true)]
        public string Name { get; set; } = null!;

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
        
        [Required]
        public Genre Genre { get; set; }

        public int? AlbumId { get; set; }

        public virtual Album? Album { get; set; }

        public int WriterId { get; set; }

        public virtual Writer Writer { get; set; } = null!;

        [Column(TypeName = PriceDecimalType)]
        public decimal Price { get; set; }

        public virtual ICollection<SongPerformer> SongPerformers { get; set; } = new List<SongPerformer>();
    }
}
