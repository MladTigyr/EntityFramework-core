using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.Utilities
{
    public static class XmlSerializerWrapper
    {
        public static T? Deserialize<T>(string inputXml, string rootAttributeName)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootAttributeName);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), root);

            using StringReader reader = new StringReader(inputXml);

            T? dtos = (T?)xmlSerializer.Deserialize(reader);

            return dtos;
        }

    }
}
