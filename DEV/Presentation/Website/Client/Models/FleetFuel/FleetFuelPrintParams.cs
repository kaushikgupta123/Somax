using Client.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetFuel
{   
    [Serializable]
    public class FleetFuelPrintParams
    {
      
        public string colname { get; set; }
        public string coldir { get; set; }
        public long EquipmentId { get; set; }
        public string SearchText { get; set; }
        public long FuelTrackingId { get; set; }
        public string ClientLookupId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public decimal FuelAmount { get; set; }
        public string FuelUnits { get; set; }
        public decimal UnitCost { get; set; }

        public DateTime? StartReadingDate { get; set; }

        public DateTime? EndReadingDate { get; set; }
        public int TotalCount { get; set; }
        public bool Void { get; set; }
    }
}