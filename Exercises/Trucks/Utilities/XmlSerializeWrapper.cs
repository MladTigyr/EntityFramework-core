using System.Text;
using System.Xml.Serialization;

namespace Trucks.Utilities
{
    public static class XmlSerializeWrapper
    {
        public static T? Deserialize<T>(string xmlInput, string rootName)
        {
            XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);

            XmlSerializer serializer = new XmlSerializer(typeof(T), rootAttribute);

            using StringReader xmlStream = new StringReader(xmlInput);

            T? dtos = (T?)serializer.Deserialize(xmlStream);

            return dtos;
        }

        public static string Serialize<T>(T? dtosToSerialize, string rootName, Dictionary<string, string>? namespaces = null)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();

            if (namespaces == null)
            {
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);
            }
            else
            {
                foreach (KeyValuePair<string, string> kvp in namespaces)
                {
                    namespaces.Add(kvp.Key, kvp.Value);
                }
            }

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringWriter xmlWriter = new StringWriter(sb);

            xmlSerializer.Serialize(xmlWriter, dtosToSerialize, xmlSerializerNamespaces);

            return sb.ToString();
        }
    }
}
