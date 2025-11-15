using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.ExportCarsFromMake
{
    [XmlType("car")]
    public class ExportCarMakeDetails
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = null!;

        [XmlAttribute("model")]
        public string Model { get; set; } = null!;

        [XmlAttribute("traveled-distance")]
        public string TravelledDistance { get; set; } = null!;
    }
}