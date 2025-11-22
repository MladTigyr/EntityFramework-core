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

        public static string Serialize<T>(T objToSerialize, string rootName, Dictionary<string, string>? namespaces = null)
        {
            StringBuilder sb = new();

            XmlRootAttribute xmlRootAttribute = new(rootName);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            if (namespaces == null)
            {
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);
            }
            else
            {
                foreach (KeyValuePair<string, string> ns in namespaces)
                {
                    xmlSerializerNamespaces.Add(ns.Key, ns.Value);
                }
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, objToSerialize, xmlSerializerNamespaces);

            return sb.ToString();
        }
    }
}
