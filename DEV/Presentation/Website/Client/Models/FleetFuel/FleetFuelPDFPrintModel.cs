using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetFuel
{
    public class FleetFuelPDFPrintModel
    {
        public string ImageUrl { get; set; }
        public string ClientLookupId { get; set; }

        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public DateTime? ReadingDate { get; set; }

        public decimal FuelAmount { get; set; }
        public string FuelUnits { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}