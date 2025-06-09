using System.Collections.Generic;

namespace InterfaceAPI.Models.MonnitIoTReading
{
    public class MonnitIoTReadingErrorResponseModel
    {
        public MonnitIoTReadingErrorResponseModel()
        {
            IoTRErrMsgList = new List<string>();
            IoTRProcessErrMsgList = new List<string>();
        }
        public int ExAccountId { get; set; }
        public List<string> IoTRProcessErrMsgList { get; set; }
        public List<string> IoTRErrMsgList { get; set; }
    }
}