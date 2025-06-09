using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Devices
{
    public class DevicePrintModel
    {
        public string ClientLookupID { get; set; }  //--IoTDevice.ClientLookupId
        public string Name { get; set; }  //--IoTDevice.Name
        //public string IoTDeviceCategory { get; set; }  //--IoTDevice.IoTDeviceCategory
        public string SensorType { get; set; }  //--IoTDevice.IoTSensorType
        public string AssetID { get; set; }    //--IoTDevice.EquipmentClientLookupId
        public string AssetName { get; set; }    //--IoTDevice.EquipmentName
        public decimal LastReading { get; set; }    //--IoTDevice.LastReading
        public DateTime ? LastReadingDate { get; set; }    //--IoTDevice.LastReadingDate
        public bool InactiveFlag { get; set; }   //--IoTDevice.InactiveFlag
    }
}