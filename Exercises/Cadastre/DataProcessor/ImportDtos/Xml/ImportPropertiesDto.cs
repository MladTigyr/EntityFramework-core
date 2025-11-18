using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos.Xml
{
    using static DataValidation.DataValidation.Property;

    [XmlType("Property")]
    public class ImportPropertiesDto
    {

        [Required]
        [XmlElement("PropertyIdentifier")]
        [MinLength(MinPropertyIdentifierLength)]
        [MaxLength(MaxPropertyIdentifierLength)]
        public string PropertyIdentifier { get; set; } = null!;

        [Required]
        [XmlElement("Area")]
        [Range(0, int.MaxValue)]
        public int Area { get; set; }

        [XmlElement("Details")]
        [MinLength(MinDetailsLength)]
        [MaxLength(MaxDetailsLength)]
        public string? Details { get; set; }

        [Required]
        [XmlElement("Address")]
        [MinLength(MinAddressLength)]
        [MaxLength(MaxAddressLength)]
        public string Address { get; set; } = null!;

        [Required]
        [XmlElement("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; } = null!;
    }
}