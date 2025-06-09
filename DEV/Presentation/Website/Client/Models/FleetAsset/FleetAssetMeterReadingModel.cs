using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetAsset
{
    public class FleetAssetMeterReadingModel
    {
        public DateTime? ReadingDate { get; set; }
        public long NoOfDays { get; set; }
        public string ReadingLine1 { get; set; }
        public string ReadingLine2 { get; set; }
        public string SourceType { get; set; }
        public bool Meter2Indicator { get; set; }
        public string VIN { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

    }
}