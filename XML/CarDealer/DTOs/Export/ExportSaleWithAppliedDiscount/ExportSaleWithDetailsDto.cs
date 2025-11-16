using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.ExportSaleWithAppliedDiscount
{
    [XmlType("sale")]
    public class ExportSaleWithDetailsDto
    {
        [XmlElement("car")]
        public ExportSaleCarDto Car { set; get; } = null!;

        [XmlElement("discount")]
        public int Discount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("price-with-discount")]
        public decimal PriceWithDiscount { get; set; }
    }
}