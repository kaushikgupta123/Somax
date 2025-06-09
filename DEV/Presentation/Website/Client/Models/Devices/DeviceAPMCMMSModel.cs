using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Devices
{
    public class DeviceAPMCMMSModel
    {
        public long IoTDeviceId { get; set; }
        public string ClientLookupID { get; set; }
        [RequiredIf("Mode", "Edit", ErrorMessage = "DeviceNameErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public string Name { get; set; }
        [Required(ErrorMessage = "GlobalSelectEquipment|" + LocalizeResourceSetConstants.Global)]
        public long EquipmentId { get; set; }
        public string Mode { get; set; }
        [Required(ErrorMessage = "GlobalSelectEquipment|" + LocalizeResourceSetConstants.Global)]
        public string Equipment_ClientLookupId { get; set; }
        [RequiredIf("IoTDeviceId", "0", ErrorMessage = "ValidationSomaxMeterErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public long CMMSMeterId { get; set; }
        [RequiredIf("IoTDeviceId", "0", ErrorMessage = "ValidationSomaxMeterErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public string Meter_ClientLookupId { get; set; }
        public int SensorId { get; set; }
    }
}