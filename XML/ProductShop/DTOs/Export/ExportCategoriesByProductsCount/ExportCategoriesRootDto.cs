using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.ExportCategoriesByProductsCount
{
    [XmlRoot("Categories")]
    public class ExportCategoriesRootDto
    {
        [XmlElement("Category")]
        public ExportCategoryDetailsDto[] Category { get; set; } = null!;
    }
}
