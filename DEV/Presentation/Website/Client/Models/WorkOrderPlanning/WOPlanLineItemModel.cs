using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.WorkOrderPlanning
{
    public class WOPlanLineItemModel
    {
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string RequiredDate { get; set; }
        public string AssetId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string Status { get; set; }
        public string CompleteDate { get; set; }
        public string Type { get; set; }
    }
}