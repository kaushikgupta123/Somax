using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetAsset
{
    public class FleetAssetSearchModel
    {
        public long EquipmentId { get; set; }
        public string ClientLookupId { get; set; }       
        public string Name { get; set; }
        public string VehicleType { get; set; }
        public int VehicleYear { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public bool RemoveFromService { get; set; }
        public DateTime? RemoveFromServiceDate { get; set; }
        public int TotalCount { get; set; }

    }
}