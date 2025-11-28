namespace Theatre.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Common.EntityValidation;

    [XmlType("Cast")]
    public class ImportCastDto
    {
        [Required]
        [XmlElement("FullName")]
        [MinLength(MinCastNameLength)]
        [MaxLength(MaxCastNameLength)]
        public string FullName { get; set; } = null!;

        [Required]
        [XmlElement("IsMainCharacter")]
        public string IsMainCharacter { get; set; } = null!;

        [Required]
        [XmlElement("PhoneNumber")]
        [RegularExpression(CastRegex)]
        public string PhoneNumber { get; set; } = null!;

        [XmlElement("PlayId")]
        public int PlayId { get; set; }
    }
}
