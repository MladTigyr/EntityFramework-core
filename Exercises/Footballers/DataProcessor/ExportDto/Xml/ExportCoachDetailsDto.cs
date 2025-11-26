namespace Footballers.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlType("Coach")]
    public class ExportCoachDetailsDto
    {
        [XmlAttribute("FootballersCount")]
        public int FootballersCount { get; set; }

        [XmlElement("CoachName")]
        public string CoachName = null!;

        [XmlArray("Footballers")]
        public ExportFootballerDetailsDto[] Footballers { get; set; } = null!;
    }
}