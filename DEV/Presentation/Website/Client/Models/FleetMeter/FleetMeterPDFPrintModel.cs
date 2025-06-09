using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetMeter
{
    public class FleetMeterPDFPrintModel
    {
        public string EquipmentImage { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public long NoOfDays { get; set; }
        public string ReadingLine1 { get; set; }
        public string ReadingLine2 { get; set; }
        public DateTime? ReadingDate { get; set; }
        public string SourceType { get; set; }
        public bool Meter2Indicator { get; set; }

    }
}