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
    using static Common.EntityValidation.Performer;
    public class Performer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        [Unicode(true)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(LastNameMaxLength)]
        [Unicode(true)]
        public string LastName { get; set; } = null!;

        public int Age { get; set; }

        [Column(TypeName = NetWorthDecimalType)]
        public decimal NetWorth { get; set; }

        public virtual ICollection<SongPerformer> PerformerSongs { get; set; } = new List<SongPerformer>();
    }
}
