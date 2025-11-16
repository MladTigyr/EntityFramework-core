using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.ExportCustomers
{
    [XmlRoot("customers")]
    public class ExportCustomerRootDto
    {
        [XmlElement("customer")]
        public ExportCustomerDetailDto[] Customer { get; set; } = null!;
    }
}
