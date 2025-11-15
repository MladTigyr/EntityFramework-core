using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.ExportCarsWithDistance
{
    [XmlRoot("cars")]
    public class ExportCarRoot
    {
        [XmlElement("car")]
        public ExportCarDetails[] Car { get; set; } = null!;
    }
}
