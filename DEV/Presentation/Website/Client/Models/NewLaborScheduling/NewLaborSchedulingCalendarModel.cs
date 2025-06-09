using System;

namespace Client.Models.NewLaborScheduling
{
    public class NewLaborSchedulingCalendarModel
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
        public int PartOnOrder { get; set; } //V2-838
        public string Tooltip { get; set; } //V2-838
    }
}