using System;

namespace Client.Models
{
    public class EventLogModel
    {
        public long ClientId { get; set; }
        public long SiteId { get; set; }        
        public long EventLogId { get; set; }
        public long ObjectId { get; set; }
        public string Event { get; set; }
        public long PersonnelId { get; set; }
        public string Comments { get; set; }
        public long SourceId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string Personnel { get; set; }
        public string PersonnelInitial { get; set; }
        public string Events { get; set; }

    }
}