using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.IoTEvent
{
    public class IoTEventModel
    {
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public long IoTEventId { get; set; }
        public string EquipClientLookupId { get; set; }
        public string AssetID { get; set; }
        public string AssetName { get; set; }
        public string SourceType { get; set; }
        public string EventType { get; set; }
        public string Status { get; set; }
        public string ProcessBy_Personnel { get; set; }
        public DateTime? ProcessDate { get; set; }
        public string Disposition { get; set; }
        public string DismissReason { get; set; }
        public string WOClientLookupId { get; set; }
        public string IoTDeviceClientLookupId { get; set; }
        public string FaultCode { get; set; }
        public string Comments { get; set; }
        public DateTime? CreateDate { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<SelectListItem> EventStatusList { get; set; }
        public IEnumerable<SelectListItem> EventTypeList { get; set; }
        public IEnumerable<SelectListItem> EventDispositionList { get; set; }
    }
}