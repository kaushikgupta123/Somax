using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTDataLayer.EL
{
    public class WorkOrderScheduleEL
    {
        public Int64 ClientId
        { set; get; }
        public Int64 WorkOrderSchedId
        { set; get; }
        public Int64 WorkOrderId
        { set; get; }
        public Int64 PersonnelId
        { set; get; }
        public string Type
        { set; get; }
        public string Description
        { set; get; }
        public Boolean Down
        { set; get; }
        public String ChargeToName
        { set; get; }
        public String Status
        { set; get; }
        public String Priority
        { set; get; }
        public string Craft
        { set; get; }
        public string Crew
        { set; get; }
        public Boolean Rescheduled
        { set; get; }
        public DateTime ScheduledStartDate
        { set; get; }
        public Decimal ScheduledHours
        { set; get; }
        public Int64 ChargeToId
        { set; get; }
        public string Shift
        { set; get; }
        public Boolean WorkOrderCompleted
        { set; get; }
        public string ModifyBy
        { set; get; }
        public DateTime ModifyDate
        { set; get; }
        public string CreateBy
        { set; get; }
        public DateTime CreateDate
        { set; get; }
        public Int32 UpdateIndex
        { set; get; }

    }
}
