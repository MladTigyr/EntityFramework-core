using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.ExportSaleWithAppliedDiscount
{
    [XmlRoot("sales")]
    public class ExportSaleRootDto
    {
        [XmlElement("sale")]
        public ExportSaleWithDetailsDto[] Sale { get; set; } = null!;
    }
}
