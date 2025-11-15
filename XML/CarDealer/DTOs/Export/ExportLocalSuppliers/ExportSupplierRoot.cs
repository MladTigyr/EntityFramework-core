using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.ExportLocalSuppliers
{
    [XmlRoot("suppliers")]
    public class ExportSupplierRoot
    {
        [XmlElement("supplier")]
        public ExportSupplierDetails[] Supplier { get; set; } = null!;
    }
}
