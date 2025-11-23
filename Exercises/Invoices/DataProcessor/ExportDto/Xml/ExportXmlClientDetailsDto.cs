using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto.Xml
{
    [XmlType("Client")]
    public class ExportXmlClientDetailsDto
    {
        [XmlAttribute("InvoicesCount")]
        public int InvoicesCount { get; set; }

        [XmlElement("ClientName")]
        public string ClientName { get; set; } = null!;

        [XmlElement("VatNumber")]
        public string VatNumber { get; set; } = null!;

        [XmlArray("Invoices")]
        public ExportInvoiceDetailsDto[] Invoice { get; set; } = null!;
    }
}