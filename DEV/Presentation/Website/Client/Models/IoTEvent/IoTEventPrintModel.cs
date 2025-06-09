using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.IoTEvent
{
    public class IoTEventPrintModel
    {
        public long IoTEventId { get; set; }
        public string SourceType { get; set; }
        public string EventType { get; set; }
        public string AssetID { get; set; }
        public string AssetName { get; set; }
        public string Status { get; set; }
        public string Disposition { get; set; }
        public string WOClientLookupId { get; set; }
        public string FaultCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string IoTDeviceClientLookupId { get; set; }
        public DateTime? ProcessDate { get; set; }
        public string ProcessBy_Personnel { get; set; }
        public string Comments { get; set; }
    }
}