using System;

namespace Client.Models
{
    public class WOComplete
    {
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string WorkAssigned_PersonnelClientLookupId { get; set; }
        public string Status_Display { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
        public long WorkOrderId { get; set; }

    }
}