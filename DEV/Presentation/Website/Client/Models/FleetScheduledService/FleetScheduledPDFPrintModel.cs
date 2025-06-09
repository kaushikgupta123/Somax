using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetScheduledService
{
    public class FleetScheduledPDFPrintModel
    {
        public string ImageUrl { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
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

        public string LastCompletedLine1 { get; set; }
        public string LastCompletedLine2 { get; set; }
        public string NextDueScheduledate { get; set; }
        public string NextDueScheduleInterval { get; set; }
    }
}