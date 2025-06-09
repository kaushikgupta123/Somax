using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetAsset
{
    public class FleetAssetFleetIssueModel
    {
        public long FleetIssuesId { get; set; }
        public long EquipmentId { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public DateTime? RecordDate { get; set; }
        public string Defects { get; set; }
        public string Description { get; set; }
        public string DriverName { get; set; }
        public string Status { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string ServiceOrderClientLookupId { get; set; }
    }
}