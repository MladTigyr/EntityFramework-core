namespace Footballers.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Common.EntityValidation;

    [XmlType("Coach")]
    public class ImportCoachDto
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(MinCoachNameLength)]
        [MaxLength(MaxCoachNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("Nationality")]
        public string Nationality { get; set; } = null!;

        [Required]
        [XmlArray("Footballers")]
        public ImportFootballerDto[] Footballers { get; set; } = null!;
    }
}
