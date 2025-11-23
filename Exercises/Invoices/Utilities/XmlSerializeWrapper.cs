namespace Invoices.Utilities
{
    using System.Text;
    using System.Xml.Serialization;

    public static class XmlSerializeWrapper
    {
        public static T? Deserialize<T>(string xmlInput, string rootName)
        {
            XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);

            XmlSerializer serializer = new XmlSerializer(typeof(T), rootAttribute);

            using StringReader xmlReader = new StringReader(xmlInput);

            T? dtos = (T?)serializer.Deserialize(xmlReader);

            return dtos;
        }

        public static string Serialize<T>(T objToSerialize, string rootName, Dictionary<string, string> namespaces = null!)
        {
            StringBuilder sb = new StringBuilder();

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

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringWriter xmlWriter = new StringWriter(sb);

            xmlSerializer.Serialize(xmlWriter, objToSerialize, xmlSerializerNamespaces);

            return sb.ToString();
        }
    }
}
