using System;

namespace Client.Models
{

    public class DowntimeSessionData
    {
        public long DowntimeId { get; set; }
        public string PersonnelClientLookupId { get; set; }
        public DateTime DateDown { get; set; }
        public decimal MinutesDown { get; set; }
        public string WorkOrderClientLookupId { get; set; }
    }
}