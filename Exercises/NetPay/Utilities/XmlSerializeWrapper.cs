using System.Text;
using System.Xml.Serialization;

namespace NetPay.Utilities
{
    public static class XmlSerializeWrapper
    {
        public static T? Deserialize<T>(string xml, string rootName)
        {
            XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);

            XmlSerializer serializer = new XmlSerializer(typeof(T), rootAttribute);
            
            using StringReader xmlReader = new StringReader(xml);

            T? dtos = (T?)serializer.Deserialize(xmlReader);

            return dtos;
        }

        public static string Serialize<T>(T? dtos, string rootName, Dictionary<string, string>? namespaces = null)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

            if (namespaces == null)
            {
                xmlns.Add(string.Empty, string.Empty);
            }
            else
            {
                foreach (KeyValuePair<string, string> kvp in namespaces)
                {
                    xmlns.Add(kvp.Key, kvp.Value);
                }
            }

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringWriter xmlWriter = new StringWriter(sb);

            xmlSerializer.Serialize(xmlWriter, dtos, xmlns);

            return sb.ToString();
        }
    }
}
