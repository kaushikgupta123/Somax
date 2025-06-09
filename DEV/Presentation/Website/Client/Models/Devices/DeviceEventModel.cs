using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Devices
{
    public class DeviceEventModel
    {
        public long IoTDeviceId { get; set; }
        public string DeviceClientLookupId { get; set; }
        public long EventID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Disposition { get; set; }
        public DateTime? Created { get; set; }
    }
}