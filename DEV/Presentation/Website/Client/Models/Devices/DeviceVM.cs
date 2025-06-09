using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Devices
{
    public class DeviceVM : LocalisationBaseVM
    {
        public DeviceModel deviceModel { get; set; }
        public DevicePrintModel devicePrintModel { get; set; }
        public DeviceEventModel deviceEventModel { get; set; }
        public GaugeModel gaugeModel { get; set; }
        public Security security { get; set; }
        public bool APMval { get; set; }
        #region V2-1105
        public string EquipmentClientLookupId { get; set; }
        public bool IsFromEquipment { get; set; }
        public long EquipmentId { get; set; }
        #endregion
       
        public ChangeDeviceIDModel changeDeviceIDModel { get; set; }
        #region V2-536
        public DeviceCategoryModel DeviceCategoryModel { get; set;}
        public DeviceAPMOnlyModel DeviceAPMOnlyModel { get; set;}
        public DeviceAPMCMMSModel DeviceAPMCMMSModel { get; set;}
        public DeviceMonnitSensorModel DeviceMonnitSensorModel { get; set;}
        public DeviceSensorModel DeviceSensorModel { get; set;}
        #endregion
        #region V2-1103
        public AddRecordReadingModal addRecordReadingModal { get; set; }
        #endregion
    }
}