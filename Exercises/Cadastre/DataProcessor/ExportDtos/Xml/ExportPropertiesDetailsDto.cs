using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ExportDtos.Xml
{
    [XmlType("Property")]
    public class ExportPropertiesDetailsDto
    {
        [XmlAttribute("postal-code")]
        public string PostalCode { get; set; } = null!;

        [XmlElement("PropertyIdentifier")]
        public string PropertyIdentifier { get; set; } = null!;

        [XmlElement("Area")]
        public int Area { get; set; }

        [XmlElement("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; } = null!;
    }
}