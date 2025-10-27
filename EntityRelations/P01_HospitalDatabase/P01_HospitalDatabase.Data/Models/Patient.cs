using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    using static Common.EntityValidation.Patient;
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        [Unicode(true)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(LastNameMaxLength)]
        [Unicode(true)]
        public string LastName { get; set; } = null!;

        [Required]
        [Unicode(true)]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        [Unicode(false)]
        [MaxLength(EmailMaxLength)]
        public string Email { get; set; } = null!;

        public bool HasInsurance { get; set; }

        public virtual ICollection<Visitation> Visitations { get; set; } = null!;

        public virtual ICollection<Diagnose> Diagnoses { get; set; } = null!;

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; } = null!;
    }
}
