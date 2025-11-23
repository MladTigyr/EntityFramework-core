namespace Invoices.DataProcessor.ExportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlRoot("Clients")]
    public class ExportClientRootDto
    {
        [XmlElement("Client")]
        public ExportXmlClientDetailsDto[] Client { get; set; } = null!;
    }
}
