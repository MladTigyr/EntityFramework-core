using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.ExportUsersandProducts
{
    [XmlType("SoldProducts")]
    public class ExportUserSoldProductsDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public ExportUserSoldProductsDetailsDto[] Product { get; set; } = null!;
    }
}