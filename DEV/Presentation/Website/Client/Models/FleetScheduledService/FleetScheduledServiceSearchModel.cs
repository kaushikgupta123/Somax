using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetScheduledService
{
    public class FleetScheduledServiceSearchModel
    {
        public long EquipmentId { get; set; }
        public long ScheduledServiceId { get; set; }
        public string ClientLookupId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public int customQueryDisplayId { get; set; }
        public string ServiceTask { get; set; }


        //public string Schedule { get; set; }
        //public string NextDue { get; set; }
        //public string LastCompleted { get; set; }

        //public string LastCompletedLine1 { get; set; }
        //public string LastCompletedLine2 { get; set; }
        //public DateTime? LastPerformedDate { get; set; }

        public string ServiceTaskDesc { get; set; }
      
        public int TimeInterval { get; set; }
        public decimal Meter1Interval { get; set; }
        public decimal Meter2Interval { get; set; }
        public string TimeIntervalType { get; set; }
        public decimal NextDueMeter1 { get; set; }
        public string Meter1Units { get; set; }
        public string Meter2Units { get; set; }
        public DateTime? NextDueDate { get; set; }
        public decimal NextDueMeter2 { get; set; }
        public DateTime? LastPerformedDate { get; set; }
        public decimal LastPerformedMeter1 { get; set; }
        public decimal LastPerformedMeter2 { get; set; }
        public string NextDueMeter1str { get; set; }
        public string NextDueMeter2str { get; set; }
        public string LastCompletedstr { get; set; }
        public int TotalCount { get; set; }
        public bool InactiveFlag{ get; set; }
        public long ServiceTaskId { get; set; }
        public string Schedule { get; set; }
        public string NextDue { get; set; }
        public string RepairReason { get; set; }
        public string System { get; set; }
        public string Assembly { get; set; }
    }
}