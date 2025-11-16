using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.ExportCarsWithParts
{
    [XmlType("car")]
    public class ExportCarPartDetailDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; } = null!;

        [XmlAttribute("model")]
        public string Model { get; set; } = null!;

        [XmlAttribute("traveled-distance")]
        public long TraveledDistance { get; set; }

        [XmlArray("parts")]
        public ExportPartCarDetailDto[] Parts { get; set; } = null!;
    }
}