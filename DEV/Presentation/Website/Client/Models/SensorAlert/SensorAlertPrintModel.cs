using System;

namespace Client.Models.SensorAlert
{
    public class SensorAlertPrintModel
    {
        public string ClientLookUpId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}