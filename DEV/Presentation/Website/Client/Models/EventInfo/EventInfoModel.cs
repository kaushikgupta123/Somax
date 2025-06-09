using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.EventInfo
{
    public class EventInfoModel
    {
        public long ClientId { get; set; }
        public long EventInfoId { get; set; }
        public long EquipmentId { get; set; }        
        public string EquipClientLookupId { get; set; }
        public string SourceType { get; set; }
        public string EventType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public long ProcessBy_PersonnelId { get; set; }
        public string ProcessBy_Personnel { get; set; }
        public DateTime? ProcessDate { get; set; }
        public string Disposition { get; set; }
        public string DismissReason { get; set; }
        public long WorkOrderId { get; set; }
        public string WOClientLookupId { get; set; }
        public string FaultCode { get; set; }
        public string Comments { get; set; }
        public string SensorId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public IEnumerable<SelectListItem> EventStatusList { get; set; }
        public IEnumerable<SelectListItem> EventSourceTypeList { get; set; }
        public IEnumerable<SelectListItem> EventTypeList { get; set; }
        public IEnumerable<SelectListItem> EventDispositionList { get; set; }
        public IEnumerable<SelectListItem> FaultCodeList { get; set; }
    
    }
}