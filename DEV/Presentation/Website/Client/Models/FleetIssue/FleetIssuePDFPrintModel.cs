using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetIssue
{
    public class FleetIssuePDFPrintModel
    {
        public string ImageUrl { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public DateTime? RecordDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Defects { get; set; }
        public DateTime? CompleteDate { get; set; }
     
    }
}