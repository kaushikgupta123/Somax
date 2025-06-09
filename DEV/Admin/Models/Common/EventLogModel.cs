using System;

namespace Admin.Models
{
    public class EventLogModel
    {
        public long ClientId { get; set; }
        public long SiteId { get; set; }        
        public long EventLogId { get; set; }
        public string SiteName { get; set; }
        public string Event { get; set; }
        public string Comments { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int TotalCount { get; set; }
    }
}