namespace Theatre.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Common.EntityValidation;

    [XmlType("Play")]
    public class ImportPlayDto
    {
        [Required]
        [XmlElement("Title")]
        [MinLength(MinPlayTitleLength)]
        [MaxLength(MaxPlayTitleLength)]
        public string Title { get; set; } = null!;

        [Required]
        [XmlElement("Duration")]
        public string Duration { get; set; } = null!;

        [Required]
        [XmlElement("Raiting")]
        [Range(MinPlayRatinValue, MaxPlayRatinValue)]
        public float Rating { get; set; }

        [Required]
        [XmlElement("Genre")]
        public string Genre { get; set; } = null!;

        [Required]
        [XmlElement("Description")]
        [MinLength(MinPlayDescriptionLength)]
        [MaxLength(MaxPlayDescriptionLength)]
        public string Description { get; set; } = null!;

        [Required]
        [XmlElement("Screenwriter")]
        [MinLength(MinPlayScreeWriterLength)]
        [MaxLength(MaxPlayScreeWriterLength)]
        public string Screewriter { get; set; } = null!;
    }
}
