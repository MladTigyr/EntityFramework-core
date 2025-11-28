namespace Artillery.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Common.EntityValidation;

    [XmlType("Country")]
    public class ImportCountryDto
    {
        [Required]
        [XmlElement("CountryName")]
        [MinLength(MinCountryNameLength)]
        [MaxLength(MaxCountryNameLength)]
        public string CountryName { get; set; } = null!;

        [XmlElement("ArmySize")]
        [Range(MinCountryArmySize, MaxCountryArmySize)]
        public int ArmySize { get; set; }
    }
}
