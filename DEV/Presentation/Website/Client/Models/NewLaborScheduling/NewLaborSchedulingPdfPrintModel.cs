using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.NewLaborScheduling
{
    public class NewLaborSchedulingPdfPrintModel
    {
        public string PersonnelName { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ScheduledStartDateString { get; set; }
        public decimal ScheduledHours { get; set; }
        public string RequiredDateString { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string PerNextValue { get; set; }
        public DateTime SDNextValue { get; set; }
        public decimal SumPersonnelHour { get; set; }
        public decimal SumScheduledateHour { get; set; }
        public decimal GrandTotalHour { get; set; }
        public long PerIDNextValue { get; set; }
        public Int64 PersonnelId { get; set; }
        public long WorkOrderScheduleId { get; set; }
        public string GroupType { get; set; }// Assigned , SccheduleDate
    }
}