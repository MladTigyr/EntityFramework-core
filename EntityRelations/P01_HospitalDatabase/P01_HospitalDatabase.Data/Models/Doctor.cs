using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    using static Common.EntityValidation.Doctor;
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [MaxLength(NameMaxLength)]
        [Required]
        [Unicode(true)]
        public string Name { get; set; } = null!;

        [MaxLength(SpecialtyMaxLength)]
        [Required]
        [Unicode(true)]
        public string Specialty { get; set; } = null!;

        public virtual ICollection<Visitation> Visitations { get; set; } = null!;
    }
}
