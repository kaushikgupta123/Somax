using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.ProjectCosting
{
    public class ProjectCostingSearchPrintModel
    {
        public long ProjectId { get; set; }
        public string ClientlookupId { get; set; }
        public string Description { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualFinish { get; set; }
        public string Status { get; set; }
        public decimal Budget { get; set; }
        public string AG1ClientLookupId { get; set; }
        public string AG2ClientLookupId { get; set; }
        public string AG3ClientLookupId { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Coordinator { get; set; }
    }
}