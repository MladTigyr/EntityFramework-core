using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.ExportUsersandProducts
{
    [XmlType("Product")]
    public class ExportUserSoldProductsDetailsDto
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}