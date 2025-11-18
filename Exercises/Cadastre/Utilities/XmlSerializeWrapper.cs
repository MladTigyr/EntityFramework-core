using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.Utilities
{
    public static class XmlSerializeWrapper
    {
        public static T? Deserialize<T>(string xmlDocument, string rootName)
        {
            XmlRootAttribute rootAttribute = new(rootName);

            XmlSerializer serializer = new XmlSerializer(typeof(T), rootAttribute);

            using StringReader xmlReader = new StringReader(xmlDocument);

            T? dtos = (T?)serializer.Deserialize(xmlReader);

            return dtos;
        }

        public static string Serialize<T>(T objectToSerialize, string rootName, Dictionary<string, string>? namespaces = null)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();

            if (namespaces == null)
            {
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);  
            }
            else
            {
                foreach(KeyValuePair<string, string> nskvp in namespaces)
                {
                    xmlSerializerNamespaces.Add(nskvp.Key, nskvp.Value);
                }
            }

            XmlRootAttribute xmlRootAttribute = new(rootName);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringWriter xmlWriter = new StringWriter(sb);

            xmlSerializer.Serialize(xmlWriter, objectToSerialize, xmlSerializerNamespaces);

            return sb.ToString();
        }
    }
}
