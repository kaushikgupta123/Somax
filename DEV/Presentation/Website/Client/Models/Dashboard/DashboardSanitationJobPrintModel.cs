using System;

namespace Client.Models
{
    public class DashboardSanitationJobPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string Status { get; set; }     
        public DateTime? CreateDate { get; set; }     
        public string Assigned { get; set; }
        public DateTime? CompleteDate { get; set; }
     
    }
}