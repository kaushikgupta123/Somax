using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.NewLaborScheduling
{
    public class NewLaborSchedulingSearchModel
    {
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; } 
        public long WorkOrderId { get; set; }
        public string WorkOrderClientLookupId { get; set; }      
        public string Type { get; set; }      
        public string Description { get; set; }      
        public string EquipmentClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public decimal ScheduledHours { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public DateTime? RequiredDate { get; set; }      
        public int TotalCount { get; set; }
        public string WoStatus { get; set; }
        public string PersonnelName { get; set; }
        public string PerNextValue { get; set; }
        public DateTime SDNextValue { get; set; }
         public decimal SumPersonnelHour { get; set; }
        public decimal SumScheduledateHour { get; set; }
        public decimal GrandTotalHour { get; set; }
        public long PerIDNextValue { get; set; }
        public Int64 PersonnelId { get; set; }
        public long WorkOrderScheduleId { get; set; }
        public int PartsOnOrder { get; set; } //V2-838

    }
}