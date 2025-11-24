namespace Boardgames.Utilities
{
    using System.Text;
    using System.Xml.Serialization;

    public static class XmlSerializeWrapper
    {
        public static T? Deserialize<T>(string xmlInput, string rootName)
        {
            XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);
            
            XmlSerializer serializer = new XmlSerializer(typeof(T), rootAttribute);

            using StringReader stringReader = new StringReader(xmlInput);

            T? dtos = (T?)serializer.Deserialize(stringReader);

            return dtos;
        }

        public static string Serialize<T>(T? dtosToSerialize, string rootName, Dictionary<string, string>? namespaces = null)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

            if (namespaces == null)
            {
                xmlns.Add(string.Empty, string.Empty);
            }
            else
            {
                foreach(KeyValuePair<string, string> kvp in namespaces)
                {
                    xmlns.Add(kvp.Key, kvp.Value);
                }
            }

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringWriter stringWriter = new StringWriter(sb);

            xmlSerializer.Serialize(stringWriter, dtosToSerialize, xmlns);

            return sb.ToString();
        }
    }
}
