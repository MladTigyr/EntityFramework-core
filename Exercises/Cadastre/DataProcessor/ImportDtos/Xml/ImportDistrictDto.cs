using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos.Xml
{
    using static DataValidation.DataValidation.District;

    [XmlType("District")]
    public class ImportDistrictDto
    {
        [Required]
        [XmlAttribute("Region")]
        public string Region { get; set; } = null!;

        [Required]
        [XmlElement("Name")]
        [MinLength(MinNameLength)]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("PostalCode")]
        [RegularExpression(DistrictPostalCodeRegex)]
        public string PostalCode { get; set; } = null!;

        [XmlArray("Properties")]
        public ImportPropertiesDto[] Property { get; set; } = null!;
    }
}
