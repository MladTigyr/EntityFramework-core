namespace Trucks.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlType("Despatcher")]
    public class ExportDespatcherDetailsDto
    {
        [XmlAttribute("TrucksCount")]
        public int TrucksCount { get; set; }

        [XmlElement("DespatcherName")]
        public string DespatcherName { get; set; } = null!;

        [XmlArray("Trucks")]
        public ExportTruckDetailsDto[] Trucks { get; set; } = null!;
    }
}