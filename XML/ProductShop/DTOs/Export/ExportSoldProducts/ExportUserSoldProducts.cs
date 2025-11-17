using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.ExportSoldProducts
{
    [XmlType("Product")]
    public class ExportUserSoldProducts
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}