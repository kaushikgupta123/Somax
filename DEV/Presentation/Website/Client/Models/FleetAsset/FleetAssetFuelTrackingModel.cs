using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetAsset
{
    public class FleetAssetFuelTrackingModel
    {
      
        public long FuelTrackingId { get; set; }
        public decimal FuelAmount { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime? ReadingDate { get; set; }
        public string FuelUnits { get; set; }
        public string Meter1Units { get; set; }
        public string Meter2Units { get; set; }
        public decimal TotalCost { get; set; }
    }
}