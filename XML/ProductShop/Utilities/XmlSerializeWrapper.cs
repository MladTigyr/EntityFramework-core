using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.Utilities
{
    public static class XmlSerializeWrapper
    {
        public static T? Deserialize<T>(string inputXml, string rootAttributeName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootAttributeName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringReader xmlStream = new StringReader(inputXml);

            T? importDtos = (T?)serializer.Deserialize(xmlStream);

            return importDtos;
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
                foreach (KeyValuePair<string, string> nsKvp in namespaces)
                {
                    xmlSerializerNamespaces.Add(nsKvp.Key, nsKvp.Value);
                }
            }


                XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new(typeof(T), rootAttribute);

            using StringWriter xmlStream = new StringWriter(sb);

            serializer.Serialize(xmlStream, objectToSerialize, xmlSerializerNamespaces);

            return sb.ToString();
        }
    }
}
