using System;
using System.Xml.Serialization;

namespace Client.Models.PunchoutOrderMessageResponse
{

    [XmlRoot(ElementName = "Status")]
	public class Status
	{
		[XmlAttribute(AttributeName = "code")]
		public string Code { get; set; }
		[XmlAttribute(AttributeName = "text")]
		public string _Text { get; set; }
		[XmlText]
		public string __Text { get; set; }
	}

	[XmlRoot(ElementName = "Response")]
	public class Response
	{
		[XmlElement(ElementName = "Status")]
		public Status Status { get; set; }
	}

	[XmlRoot(ElementName = "cXML")]
	public class PunchoutOrderMessageResponse
	{
		[XmlElement(ElementName = "Response")]
		public Response Response { get; set; }
		[XmlAttribute(AttributeName = "payloadID")]
		public string PayloadID { get; set; }
		[XmlAttribute(AttributeName = "timestamp")]
		public string _Timestamp { get; set; }

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