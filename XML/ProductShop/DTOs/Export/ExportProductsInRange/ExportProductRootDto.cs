using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.ExportProductsInRange
{
    [XmlRoot("Products")]
    public class ExportProductRootDto
    {
        [XmlElement("Product")]
        public ExportProductDetailsDto[] Product { get; set; } = null!;
    }
}
