namespace Artillery.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlType("Country")]
    public class ExportCountryDetailsDto
    {
        [XmlAttribute("Country")]
        public string Country { get; set; } = null!;

        [XmlAttribute("ArmySize")]
        public int ArmySize { get; set; }
    }
}