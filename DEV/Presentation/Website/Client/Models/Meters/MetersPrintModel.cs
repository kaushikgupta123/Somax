using System;

namespace Client.Models.Meters
{
    public class MetersPrintModel
    {
        public string MeterClientLookUpId { get; set; }
        public string MeterName { get; set; }
        public decimal ReadingCurrent { get; set; }
        public DateTime? ReadingDate { get; set; }
        public string PersonnelClientLookUpId { get; set; }
        public decimal ReadingLife { get; set; }
        public decimal MaxReading { get; set; }
        public string ReadingUnits { get; set; }
    }
}