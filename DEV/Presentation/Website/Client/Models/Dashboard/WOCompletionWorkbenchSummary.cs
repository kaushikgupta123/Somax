using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Dashboard
{
    public class WOCompletionWorkbenchSummary
    {
        public long WorkOrderId { get; set; }
        public string WorkOrder_ClientLookupId { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public Security security { get; set; }
        public long WorkAssigned_PersonnelId { get; set; }
        public string Assigned { get; set; }
        public DateTime? RequiredDate { get; set; }
        public long ChargeToId { get; set; }

        #region V2-847
        public string AssetGroup1Name { get; set; }
        public string AssetGroup2Name { get; set; }
        public string AssetGroup3Name { get; set; }
        public string AssetGroup1ClientlookupId { get; set; }
        public string AssetGroup2ClientlookupId { get; set; }
        #endregion
        #region V2-1012
        public string AssetLocation { get; set; }
        public string ProjectClientlookupId { get; set; }
        #endregion
    }
}