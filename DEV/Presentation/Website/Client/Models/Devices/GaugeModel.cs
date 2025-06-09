using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Devices
{
    public class GaugeModel
    {
        public long IoTDeviceId { get; set; }
        public string DeviceClientLookupId { get; set; }
        public decimal LastReading { get; set; }
        public string Unit { get; set; }
        public string ReadingWithUnit { get; set; }
    }
}