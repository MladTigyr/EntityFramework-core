namespace Footballers.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Common.EntityValidation;

    [XmlType("Footballer")]
    public class ImportFootballerDto
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(MinFootballerNameLength)]
        [MaxLength(MaxFootballerNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("ContractStartDate")]
        public string ContractStartDate { get; set; } = null!;

        [Required]
        [XmlElement("ContractEndDate")]
        public string ContractEndDate { get; set; } = null!;

        [XmlElement("BestSkillType")]
        [Range(MinFootballerSkillValue, MaxFootballerSkillValue)]
        public int BestSkillType { get; set; }

        [XmlElement("PositionType")]
        [Range(MinFootballerPositionValue, MaxFootballerPositionValue)]
        public int PositionType { get; set; }
    }
}