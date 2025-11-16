using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.ExportCarsWithParts
{
    [XmlRoot("cars")]
    public class ExportCarPartRootDto
    {
        [XmlElement("car")]
        public ExportCarPartDetailDto[] Car { get; set; } = null!;
    }
}
