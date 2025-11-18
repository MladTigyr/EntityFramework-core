using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ExportDtos.Xml
{
    [XmlRoot("Properties")]
    public class ExportPropertiesRootDto
    {
        [XmlElement("Property")]
        public ExportPropertiesDetailsDto[] Property { get; set; } = null!;
    }
}
