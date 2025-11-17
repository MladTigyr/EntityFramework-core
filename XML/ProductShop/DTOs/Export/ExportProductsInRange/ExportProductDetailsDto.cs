using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.ExportProductsInRange
{
    [XmlType("Product")]
    public class ExportProductDetailsDto
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("buyer")]
        public string? Buyer { get; set; }
    }
}