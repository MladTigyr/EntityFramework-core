namespace Artillery.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Common.EntityValidation;

    [XmlType("Manufacturer")]
    public class ImportManufacturerDto
    {
        [Required]
        [XmlElement("ManufacturerName")]
        [MinLength(MinManufacturerNameLength)]
        [MaxLength(MaxManufacturerNameLength)]
        public string ManufacturerName { get; set; } = null!;

        [Required]
        [XmlElement("Founded")]
        [MinLength(MinManufacturerFounded)]
        [MaxLength(MaxManufacturerFounded)]
        public string Founded { get; set; } = null!;
    }
}
