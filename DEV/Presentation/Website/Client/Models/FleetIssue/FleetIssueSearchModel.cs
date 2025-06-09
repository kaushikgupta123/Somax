using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetIssue
{
    public class FleetIssueSearchModel
    {
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public long EquipmentId { get; set; }

        public long FleetIssuesId { get; set; }
        public string ClientLookupId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Defects { get; set; }
        public string ServiceOrderClientLookupId { get; set; }
        public DateTime? RecordDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int TotalCount { get; set; }
    

    }
}