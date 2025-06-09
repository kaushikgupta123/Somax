using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace DataContracts
{

    [Serializable]
    [XmlRoot("History")]
    public class AuditHistory : List<AuditColumn>
    {
        #region Public Methods
        public string Serialize()
        {
            string xmlText = string.Empty;

            // Remove namespace
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using (StringWriter stringWriter = new StringWriter())
            {
                // Remove xml declaration
                XmlWriterSettings settings = new XmlWriterSettings() {OmitXmlDeclaration = true};

                using (XmlWriter writer = XmlWriter.Create(stringWriter, settings))
                {
                    XmlSerializer serializer = new XmlSerializer(this.GetType());
                    serializer.Serialize(writer, this, namespaces);
                    xmlText = stringWriter.ToString();
                }
            }

            return xmlText;
        }

        public static AuditHistory Deserialize(string xmlText)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AuditHistory));
            StringReader reader = new StringReader(xmlText);

            return serializer.Deserialize(reader) as AuditHistory;
        }
        #endregion
    }
}
