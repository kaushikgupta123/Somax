using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetAsset
{
    public class FleetAssetScheduledServiceModel
    {
        public long ScheduledServiceId { get; set; }
        public long ServiceTaskId { get; set; }
        public string ServiceTaskDescription { get; set; }
        public int TimeInterval { get; set; }
        public string TimeIntervalType { get; set; }
        public decimal Meter1Interval { get; set; }
        public string Meter1Units { get; set; }
        public decimal Meter2Interval { get; set; }
        public string Meter2Units { get; set; }
        public DateTime? NextDueDate { get; set; }
        public string TimeThresoldType { get; set; }
        public DateTime? LastPerformedDate { get; set; }
        public decimal LastPerformedMeter1 { get; set; }
        public decimal LastPerformedMeter2 { get; set; }
        public decimal NextDueMeter1 { get; set; }
        public decimal NextDueMeter2 { get; set; }
        public string Meter1Type { get; set; }
        public string Meter2Type { get; set; }
    }
}