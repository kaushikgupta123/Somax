using System;
using System.Xml.Serialization;

namespace Client.Models.PunchoutModel
{
    [XmlRoot(ElementName = "Status")]
    public class Status
    {

        [XmlAttribute(AttributeName = "code")]
        public int Code { get; set; }

        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "StartPage")]
    public class StartPage
    {

        [XmlElement(ElementName = "URL")]
        public string URL { get; set; }
    }

    [XmlRoot(ElementName = "PunchOutSetupResponse")]
    public class PunchOutSetupResponse
    {

        [XmlElement(ElementName = "StartPage")]
        public StartPage StartPage { get; set; }
    }

    [XmlRoot(ElementName = "Response")]
    public class Response
    {

        [XmlElement(ElementName = "Status")]
        public Status Status { get; set; }

        [XmlElement(ElementName = "PunchOutSetupResponse")]
        public PunchOutSetupResponse PunchOutSetupResponse { get; set; }
    }

    [XmlRoot(ElementName = "cXML")]
    public class PunchOutSetUpResponse
    {

        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }

        [XmlAttribute(AttributeName = "payloadID")]
        public string PayloadID { get; set; }

        [XmlAttribute(AttributeName = "timestamp")]
        public string _Timestamp { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlIgnore]
        public DateTime? Timestamp
        {
            get
            {
                DateTime dt;
                if (DateTime.TryParse(_Timestamp, out dt))
                {
                    return dt;
                }

                return null;
            }
            set
            {
                _Timestamp = (value == null)
                    ? (string)null
                    : value.Value.ToString("yyyy-MM-ddTHH:mm:ss");
            }
        }
    }
}