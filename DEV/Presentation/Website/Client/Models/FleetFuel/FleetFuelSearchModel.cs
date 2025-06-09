using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetFuel
{
    public class FleetFuelSearchModel
    {
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public long EquipmentId { get; set; }

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

        public decimal TotalCost { get; set; }

        public DateTime? ReadingDate { get; set; }
        public int TotalCount { get; set; }
        public bool Void { get; set; }
        public long FleetMeterReadingId { get; set; }
        public decimal Reading { get; set; }

    }
}