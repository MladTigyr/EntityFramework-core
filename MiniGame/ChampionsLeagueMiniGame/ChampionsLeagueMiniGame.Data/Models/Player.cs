using ChampionsLeagueMiniGame.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChampionsLeagueMiniGame.Data.Models
{
    using static Common.EntityValidation.Player;
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        [Unicode(true)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(LastNameMaxLength)]
        [Unicode(true)]
        public string LastName { get; set; } = null!;

        public int Age { get; set; }

        [Range(1, 99)]
        public int Rating { get; set; }

        public PrefferedFoot PrefferedFoot { get; set; }

        [Range(1, 99)]
        public int ShirtNumber { get; set; }

        public int PostitionId { get; set; }

        public virtual Position Postion { get; set; } = null!;

        public int NationalityId { get; set; }

        public virtual Nationality Nationality { get; set; } = null!;

        public int TeamId { get; set; }

        public virtual Team Team { get; set; } = null!;
    }
}
