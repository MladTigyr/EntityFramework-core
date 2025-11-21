using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TravelAgency.Utilities
{
    public static class XmlWrapperSerializer
    {
        public static T? Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);

            XmlSerializer serializer = new XmlSerializer(typeof(T), rootAttribute);

            using StringReader xmlReader = new StringReader(inputXml);

            T? dtos = (T?)serializer.Deserialize(xmlReader);

            return dtos;
        }
    }
}
