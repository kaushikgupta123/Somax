using System;

namespace Client.Models.WorkOrderPlanning
{
    public class WorkOrderPlanningResourceCalendar
    {
        public long WorkOrderId { get; set; }
        public long WorkOrderScheduleId { get; set; }
        public string PersonnelFull { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public string Description { get; set; }
        public decimal ScheduledHours { get; set; }
        public string ScheduledStartDate { get; set; }
        public string Title { get; set; }
        public long PersonnelId { get; set; }
        
    }
}