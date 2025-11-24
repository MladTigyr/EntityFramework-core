namespace Boardgames.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Common.EntityValidation;

    [XmlType("Boardgame")]
    public class ImportBoardgamesDetailsDto
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(MinBoardGameNameLength)]
        [MaxLength(MaxBoardGameNameLength)]
        public string Name { get; set; } = null!;

        [XmlElement("Rating")]
        [Range(MinBoardGameRatingValue, MaxBoardGameRatingValue)]
        public double Rating { get; set; }

        [XmlElement("YearPublished")]
        [Range(MinBoardGameYearPublished, MaxBoardGameYearPublished)]
        public int YearPublished { get; set; }

        [XmlElement("CategoryType")]
        [Range(MinBoardGameEnumValue, MaxBoardGameEnumValue)]
        public int CategoryType { get; set; }

        [Required]
        [XmlElement("Mechanics")]
        public string Mechanics { get; set; } = null!;
    }
}