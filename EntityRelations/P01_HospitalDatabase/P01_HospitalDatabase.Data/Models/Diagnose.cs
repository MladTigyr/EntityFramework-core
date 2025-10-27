using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    using static Common.EntityValidation.Diagnose;
    public class Diagnose
    {
        [Key]
        public int DiagnoseId { get; set; }

        [Required]
        [Unicode(true)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Unicode(true)]
        [MaxLength(CommentMaxLength)]
        public string? Comments { get; set; }

        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; } = null!;
    }
}
