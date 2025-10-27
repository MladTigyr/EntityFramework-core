using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    using static Common.EntityValidation.Visitation;
    public class Visitation
    {
        [Key]
        public int VisitationId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Unicode(true)]
        [MaxLength(CommentMaxLength)]
        public string? Comments {  get; set; }

        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; } = null!;

        public int DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; } = null!;
    }
}
