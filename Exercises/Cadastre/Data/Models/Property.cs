using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Data.Models
{
    using static DataValidation.DataValidation.Property;
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxPropertyIdentifierLength)]
        public string PropertyIdentifier { get; set; } = null!;

        [Required]
        public int Area { get; set; }

        [MaxLength(MaxDetailsLength)]
        public string? Details { get; set; }

        [Required]
        [MaxLength(MaxAddressLength)]
        public string Address { get; set; } = null!;

        [Required]
        public DateTime DateOfAcquisition { get; set; }

        public int DistrictId { get; set; }

        public virtual District District { get; set; } = null!;

        public virtual ICollection<PropertyCitizen> PropertiesCitizens { get; set; }
            = new List<PropertyCitizen>();
    }
}
