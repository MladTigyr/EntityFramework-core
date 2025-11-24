namespace Boardgames.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlType("Creator")]
    public class ExportCreatorDetailsDto
    {
        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }

        [XmlElement("CreatorName")]
        public string CreatorName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public ExportBoardgameDetailsDto[] Boardgame { get; set; } = null!;
    }
}