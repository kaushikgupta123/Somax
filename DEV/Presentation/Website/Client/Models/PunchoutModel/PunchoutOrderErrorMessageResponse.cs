using System.Xml.Serialization;

namespace Client.Models.PunchoutModel
{
    [XmlRoot(ElementName = "ResponseMessage")]
    public class PunchoutOrderErrorMessageResponse
    {
        [XmlElement(ElementName = "StatusCode")]
        public string StatusCode { get; set; }
        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }
        [XmlElement(ElementName = "URL")]
        public string URL { get; set; }
    }
}