using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.ExportUsersandProducts
{
    [XmlRoot("Users")]
    public class ExportUserAndProductsRootDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public ExportUserAndProductsDetailsDto[] User { get; set; } = null!;
    }
}
