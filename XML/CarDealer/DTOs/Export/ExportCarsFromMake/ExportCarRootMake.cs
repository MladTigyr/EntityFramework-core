using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.ExportCarsFromMake
{
    [XmlRoot("cars")]
    public class ExportCarRootMake
    {
        [XmlElement("car")]
        public ExportCarMakeDetails[] Car { get; set; } = null!;
    }
}
