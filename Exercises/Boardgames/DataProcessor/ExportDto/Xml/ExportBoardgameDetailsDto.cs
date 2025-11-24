namespace Boardgames.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlType("Boardgame")]
    public class ExportBoardgameDetailsDto
    {
        [XmlElement("BoardgameName")]
        public string BoardgameName { get; set; } = null!;

        [XmlElement("BoardgameYearPublished")]
        public int BoardgameYearPublished { get; set; }
    }
}