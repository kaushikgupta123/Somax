using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetAsset
{
    public class FleetAssetSummaryModel
    {
        public string Equipment_ClientLookupId { get; set; }
        public string EquipmentName { get; set; }
        public string ImageUrl { get; set; }
        public long OpenWorkOrders { get; set; }
        public long WorkRequests { get; set; }
        public long OverduePms { get; set; }
        public bool RemoveFromService { get; set; }
        public bool ClientOnPremise { get; set; }
    }
}