using Cadastre.Data.Enumerations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Data.Models
{
    using static DataValidation.DataValidation.District;
    public class District
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        public string PostalCode { get; set; } = null!;

        [Required]
        public Region Region { get; set; }

        public virtual ICollection<Property> Properties { get; set; } 
            = new List<Property>();
    }
}
