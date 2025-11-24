namespace Boardgames.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Common.EntityValidation;

    [XmlType("Creator")]
    public class ImportCreatorDetailsDto
    {
        [Required]
        [XmlElement("FirstName")]
        [MinLength(MinCreatorFirstNameLength)]
        [MaxLength(MaxCreatorFirstNameLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [XmlElement("LastName")]
        [MinLength(MinCreatorLastNameLength)]
        [MaxLength(MaxCreatorLastNameLength)]
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public ImportBoardgamesDetailsDto[] Boardgames { get; set; } = null!;
    }
}
