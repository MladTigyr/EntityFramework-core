using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.ExportSoldProducts
{
    [XmlRoot("Users")]
    public class ExportUserRootDto
    {
        [XmlElement("User")]
        public ExportUserDetailsDto[] User { get; set; } = null!;
    }
}
