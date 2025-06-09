using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Devices
{
    public class DeviceModel
    {
        public long IoTDeviceId { get; set; }
        public long SiteId { get; set; }
        public long AreaId { get; set; }
        public long DepartmentId { get; set; }
        public string ClientLookupID { get; set; }  //--IoTDevice.ClientLookupId
        public string Name { get; set; }  //--IoTDevice.Name
        public string IoTDeviceCategory { get; set; }  //--IoTDevice.IoTDeviceCategory
        public string SensorType { get; set; }  //--IoTDevice.IoTSensorType
        public string AssetID { get; set; }    //--IoTDevice.EquipmentClientLookupId
        public string AssetName { get; set; }    //--IoTDevice.EquipmentName
        public decimal LastReading { get; set; }    //--IoTDevice.LastReading
        public DateTime? LastReadingDate { get; set; }
        public decimal MeterInterval { get; set; }
        public decimal MeterReadingLife { get; set; }
        public decimal MeterReadingMax { get; set; }
        public int MonnitHeartbeat { get; set; }
        public decimal MonnitLastBatteryLevel { get; set; }
        public int MonnitLastSignalStrength { get; set; }
        public int MonnitNetworkID { get; set; }
        public int MonnitSensorAppID { get; set; }
        public long SensorAlertProcedureId { get; set; }
        public string SensorUnit { get; set; }
        public decimal TriggerHigh { get; set; }
        public decimal TriggerLow { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int UpdateIndex { get; set; }
        public int TotalCount { get; set; }
        public string AssetCategory { get; set; }
        public bool InactiveFlag { get; set; }
        public long EquipmentId { get; set; }
        public int SensorId { get; set; }
        public long CMMSMeterId { get; set; }
        public decimal TriggerHighCrit { get; set; }
        public decimal TriggerLowCrit { get; set; }
        public long CriticalProcedureId { get; set; }
        public IEnumerable<SelectListItem> DeviceCategoryList { get; set; }
        public IEnumerable<SelectListItem> SensorUnitList { get; set; }
        public IEnumerable<SelectListItem> SensorTypeList { get; set; }
        #region V2-536
        public string SensorAlertProcedureClientLooukupId { get; set; }
        public string CriticalProcedureClientLooukupId { get; set; }
        public string CMMSMeterClientLooukupId { get; set; }
        #endregion
    }
}