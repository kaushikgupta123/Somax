using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class WoCompletionDetailsHeader
    {
        public string ClientLookupId { get; set; }
        public long WorkOrderId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public string Assigned { get; set; }
        public DateTime? CompleteDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public long WorkAssigned_PersonnelId { get; set; }
        public long ChargeToId { get; set; }
        //**V2-847
        public string AssetGroup1ClientlookupId { get; set; }
        public string AssetGroup2ClientlookupId { get; set; }
        //V2-1012
        public string ProjectClientLookupId { get; set; }
        public string Assetlocation { get; set; }
    }
}