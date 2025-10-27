using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    using static Common.EntityValidation.Medicament;
    public class Medicament
    {
        [Key]
        public int MedicamentId { get; set; }

        [Required]
        [Unicode(true)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; } = null!;

    }
}
