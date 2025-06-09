using System;

namespace Client.Models.EventInfo
{
    public class EventInfoPrintModel
    {
        public long EventInfoId { get; set; }
        public string SourceType { get; set; }
        public string EventType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Disposition { get; set; }
        public string WOClientLookupId { get; set; }
        public string FaultCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string SensorId { get; set; }
        public DateTime? ProcessDate { get; set; }
        public string ProcessBy_Personnel { get; set; }
        public string Comments { get; set; }
      
    }
}