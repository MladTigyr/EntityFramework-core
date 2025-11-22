using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ImportDtos.Xml
{
    using static EntityValidation.EntityValidation;

    [XmlType("Customer")]
    public class ImportCustomerDto
    {
        [Required]
        [XmlAttribute("phoneNumber")]
        [RegularExpression(CustomerPhoneNumber)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [XmlElement("FullName")]
        [MinLength(MinCustomerFullNameLength)]
        [MaxLength(MaxCustomerFullNameLength)]
        public string FullName { get; set; } = null!;

        [Required]
        [XmlElement("Email")]
        [MinLength(MinCustomerEmailLength)]
        [MaxLength(MaxCustomerEmailLength)]
        public string Email { get; set; } = null!;
    }
}
