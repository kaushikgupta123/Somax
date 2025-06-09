using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetIssue
{
    [Serializable]
    public class FleetIssuePrintParams
    {
        public string colname { get; set; }
        public string coldir { get; set; }
        public long EquipmentId { get; set; }
        public string SearchText { get; set; }
        public long FleetIssuesId { get; set; }
        public string ClientLookupId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
      
        public DateTime? StartRecordDate { get; set; }

        public DateTime? EndRecordDate { get; set; }

        public DateTime? StartCreateDateVw { get; set; }

        public DateTime? EndCreateDateVw { get; set; }
        public int TotalCount { get; set; }
        public int customQueryDisplayId { get; set; }
        public List<string> Defects { get; set; }

    }
}