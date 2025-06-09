using System;

namespace Client.Models.MasterSanitationSchedule
{
    public class MasterSanitationPrintModel
    {
        public string Description { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public int Frequency { get; set; }
        public string Assigned { get; set; }
        public string Shift { get; set; }
        public decimal ScheduledDuration { get; set; }
        public DateTime? NextDue { get; set; }
        public bool InactiveFlag { get; set; }
    }
}