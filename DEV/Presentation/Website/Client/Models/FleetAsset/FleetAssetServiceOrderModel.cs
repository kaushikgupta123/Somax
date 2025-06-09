using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetAsset
{
    public class FleetAssetServiceOrderModel
    {
        public long ServiceOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string AssetName { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int ChildCount { get; set; }
    }
}