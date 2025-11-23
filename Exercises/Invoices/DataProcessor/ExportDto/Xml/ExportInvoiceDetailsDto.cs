using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto.Xml
{
    [XmlType("Invoice")]
    public class ExportInvoiceDetailsDto
    {
        [XmlElement("InvoiceNumber")]
        public int InvoiceNumber { get; set; }

        [XmlElement("InvoiceAmount")]
        public string InvoiceAmount { get; set; } = null!;

        [XmlElement("DueDate")]
        public string DueDate { get; set; } = null!;

        [XmlElement("Currency")]
        public string Currency { get; set; } = null!;
    }
}