using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.ExportSoldProducts
{
    [XmlType("User")]
    public class ExportUserDetailsDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlArray("soldProducts")]
        public ExportUserSoldProducts[] SoldProducts { get; set; } = null!;
    }
}