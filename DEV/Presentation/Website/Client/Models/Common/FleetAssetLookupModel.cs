using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class FleetAssetLookupModel
    {
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string VIN { get; set; }
        public long TotalCount { get; set; }
        public long? EquipmentId { get; set; }
        public string Meter1Type { get; set; }
        public string Meter2Type { get; set; }
        public decimal Meter1CurrentReading { get; set; }
        public decimal Meter2CurrentReading { get; set; }
        public DateTime? Meter1CurrentReadingDate { get; set; }
        public DateTime? Meter2CurrentReadingDate { get; set; }
        public string CurrentReadingTime { get; set; }
        public double Meter1DayDiff { get; set; }
        public double Meter2DayDiff { get; set; }
        public string Meter1Unit { get; set; }
        public string Meter2Unit { get; set; }
        public string FuelUnits { get; set; }
    }
}