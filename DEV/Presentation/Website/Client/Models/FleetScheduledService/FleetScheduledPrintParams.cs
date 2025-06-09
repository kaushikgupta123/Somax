using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetScheduledService
{
    [Serializable]
    public class FleetScheduledPrintParams
    {
        public string colname { get; set; }
        public string coldir { get; set; }
        public long EquipmentId { get; set; }
        public string SearchText { get; set; }
        public int customQueryDisplayId { get; set; }
        public long ServiceTaskId { get; set; }
        public long ScheduledServiceId { get; set; }
        public string ClientLookupId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public long Last_ServiceOrderId { get; set; }
        public DateTime? LastPerformedDate { get; set; }
        public decimal LastPerformedMeter1 { get; set; }
        public decimal LastPerformedMeter2 { get; set; }
        public decimal Meter1Interval { get; set; }
        public decimal Meter1Threshold { get; set; }
        public decimal Meter2Interval { get; set; }
        public decimal Meter2Threshold { get; set; }
        public DateTime? NextDueDate { get; set; }
        public decimal NextDueMeter1 { get; set; }
        public decimal NextDueMeter2 { get; set; }
        public int TimeInterval { get; set; }
        public string TimeIntervalType { get; set; }
        public int TimeThreshold { get; set; }
        public string TimeThresoldType { get; set; }
        public string MeterType1 { get; set; }
        public string MeterType2 { get; set; }
        public string Meter1Units { get; set; }
        public string Meter2Units { get; set; }
        public string ServiceTask { get; set; }
        public string Schedule { get; set; }
        public string NextDue { get; set; }
        public string LastCompleted { get; set; }
        public string ServiceTaskDesc { get; set; }
        public int TotalCount { get; set; }
    }
}