using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Client.Common
{
    public static class XmlHelper
    {
        public static T XmlDeserializeFromString<T>(string objectData)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(objectData))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static string XmlSerializeFromObject<T>(T objectData)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));

            //using (var sww = new StringWriter())
            using (var sww = new Utf8StringWriter())
            {
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, objectData);
                    return sww.ToString();
                }
            }
        }
        public static T XmlDeserializeFromStringBase64<T>(string objectData)
        {
          byte[] data = Convert.FromBase64String(objectData);
          string xmlString = Encoding.UTF8.GetString(data);
          var serializer = new XmlSerializer(typeof(T));

          using (var reader = new StringReader(xmlString))
          {
            return (T)serializer.Deserialize(reader);
          }
        }
    }
    public class Utf8StringWriter : StringWriter
    {
      public override Encoding Encoding => Encoding.UTF8;
    }
}