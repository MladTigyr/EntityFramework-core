using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.Utilities
{
    public static class XmlSerializeWrapper
    {
        public static T? Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute rootAttribute = new(rootName);

            XmlSerializer serializer = new XmlSerializer(typeof(T), rootAttribute);

            using StringReader reader = new StringReader(inputXml);

            T? dtos = (T?)serializer.Deserialize(reader);

            return dtos;
        }
    }
}
