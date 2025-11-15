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

        public static string Serialize<T>(T serializeObject, string rootAttributeName, Dictionary<string, string> namespaces = null)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            if (namespaces == null)
            {
                ns.Add(string.Empty, string.Empty);
            }
            else
            {
                foreach (var (prefix, uri) in namespaces)
                {
                    ns.Add(prefix, uri);
                }
            }

            XmlRootAttribute root = new XmlRootAttribute(rootAttributeName);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), root);

            using StringWriter writer = new StringWriter(sb);
            xmlSerializer.Serialize(writer, serializeObject, ns);

            return sb.ToString();
        }

    }
}
