using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Controllers.Maintenance;
using Client.Models;
using Client.Models.Devices;

using Client.Models.Work_Order;
using Client.Models.WorkOrder;

using Common.Constants;
using Common.Enumerations;
using Common.Extensions;

using Database.Business;

using DataContracts;

using DevExpress.CodeParser;

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Client.BusinessWrapper
{
    public class DevicesWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public DevicesWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        //public List<KeyValuePair<string, string>> PopulateDivListDetails()
        //{
        //    List<KeyValuePair<string, string>> customList = new List<KeyValuePair<string, string>>();
        //    customList = CustomQueryDisplay.RetrieveQueryItemsByTableAndLanguage(userData.DatabaseKey, "Devices", userData.Site.LocalizationLanguage, userData.Site.LocalizationCulture);
        //    if (customList.Count > 0)
        //    {
        //        customList.Insert(0, new KeyValuePair<string, string>("0", "--Select All--"));
        //    }
        //    return customList;
        //}

        public List<DeviceModel> GetDeviceGridData(int customQueryDisplayId, out IEnumerable<SelectListItem> AssetCategoryList, out IEnumerable<SelectListItem> SensorTypeList, int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string deviceClientLookupId = "", string name = "", string assetClentLookupId = "", string assetCategory = "", string sensorType = "", string txtSearchval = "")
        {
            if (!string.IsNullOrEmpty(txtSearchval))
            {
                txtSearchval = txtSearchval.Trim();
            }
            DeviceModel deviceModel;
            List<DeviceModel> deviceModelList = new List<DeviceModel>();
            List<IoTDevice> IoTDevice = new List<IoTDevice>();
            IoTDevice device = new IoTDevice();
            device.ClientId = userData.DatabaseKey.Client.ClientId;
            device.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            device.CustomQueryDisplayId = customQueryDisplayId;
            device.OrderbyColumn = orderbycol;
            device.OrderBy = orderDir;
            device.OffSetVal = skip;
            device.NextRow = length;
            device.SearchText = txtSearchval;
            device.ClientLookupId = (deviceClientLookupId != null) ? deviceClientLookupId : "";
            device.Name = (name != null) ? name : "";
            device.AssetId = (assetClentLookupId != null) ? assetClentLookupId : "";
            device.AssetCategory = (assetCategory != null) ? assetCategory : "";
            device.SensorType = (sensorType != null) ? sensorType : "";
            device.CustomSearch(userData.DatabaseKey, userData.Site.TimeZone);
            var dataList = device.listOfDevice;
            foreach (var item in dataList)
            {
                deviceModel = new DeviceModel();
                deviceModel.IoTDeviceId = item.IoTDeviceId;
                deviceModel.ClientLookupID = item.ClientLookupId;
                deviceModel.Name = item.Name;
                deviceModel.IoTDeviceCategory = item.IoTDeviceCategory;
                deviceModel.SensorType = item.SensorType;
                deviceModel.AssetID = item.AssetId;
                deviceModel.AssetName = item.AssetName;
                deviceModel.LastReading = item.LastReading;
                deviceModel.AssetCategory = item.IoTDeviceCategory; //AssetCategory 
                deviceModel.SensorId = item.SensorID;
                if (item.LastReadingDate != null && item.LastReadingDate == DateTime.MinValue)
                {
                    deviceModel.LastReadingDate = null;
                }
                else
                {
                    deviceModel.LastReadingDate = item.LastReadingDate;
                }
                deviceModel.InactiveFlag = item.InactiveFlag;
                deviceModel.TotalCount = item.TotalCount;
                deviceModelList.Add(deviceModel);
            }

            var dList = deviceModelList.Where(x => x.AssetCategory != "").ToList();
            var tempList = dList.Select(x => new { Text = x.IoTDeviceCategory, Value = x.IoTDeviceCategory }).ToList().Distinct();
            //AssetCategoryList = tempList.Select(y => new SelectListItem { Text = y.Value, Value = y.Value });
            AssetCategoryList = tempList.Select(y => new SelectListItem { Text = UtilityFunction.GetMessageFromResource(y.Value, LocalizeResourceSetConstants.DeviceDetails), Value = y.Value });
            var dList2 = deviceModelList.Where(x => x.SensorType != "").ToList();
            tempList = dList2.Select(x => new { Text = x.SensorType, Value = x.SensorType }).ToList().Distinct();
            //SensorTypeList = tempList.Select(y => new SelectListItem { Text = y.Value, Value = y.Value });
            SensorTypeList = tempList.Select(y => new SelectListItem { Text = UtilityFunction.GetMessageFromResource(y.Value, LocalizeResourceSetConstants.DeviceDetails), Value = y.Value });
            return deviceModelList;
        }

        public List<DeviceModel> GetDevicePrintData(int customQueryDisplayId, string txtSearchval = "")
        {
            DeviceModel devicePrintModel;
            List<DeviceModel> devicePrintModelList = new List<DeviceModel>();
            IoTDevice device = new IoTDevice();
            device.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            device.CaseNo = customQueryDisplayId;
            device.SearchText = txtSearchval;
            var dataList = device.RetrieveIoTDeviceForPrint(userData.DatabaseKey, userData.Site.TimeZone);
            foreach (var item in dataList)
            {
                devicePrintModel = new DeviceModel();
                devicePrintModel.ClientLookupID = item.ClientLookupId;
                devicePrintModel.Name = item.Name;
                devicePrintModel.IoTDeviceCategory = item.IoTDeviceCategory;
                devicePrintModel.SensorType = item.SensorType;
                devicePrintModel.AssetID = item.AssetId;
                devicePrintModel.AssetName = item.AssetName;
                devicePrintModel.LastReading = item.LastReading;
                devicePrintModelList.Add(devicePrintModel);
            }
            return devicePrintModelList;
        }

        #endregion

        #region Details
        public DeviceModel GetDevicedetails(long ioTDeviceId)
        {
            IoTDevice ioTDevice = new IoTDevice();
            ioTDevice.ClientId = userData.DatabaseKey.Client.ClientId;
            ioTDevice.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            ioTDevice.IoTDeviceId = ioTDeviceId;
            ioTDevice.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            DeviceModel deviceModel = new DeviceModel();
            if (ioTDevice != null)
            {
                deviceModel.IoTDeviceId = ioTDevice.IoTDeviceId;
                deviceModel.ClientLookupID = ioTDevice.ClientLookupId;
                deviceModel.Name = ioTDevice.Name;
                deviceModel.IoTDeviceCategory = ioTDevice.IoTDeviceCategory;
                deviceModel.SensorType = ioTDevice.SensorType;
                deviceModel.AssetID = ioTDevice.AssetId;
                deviceModel.EquipmentId = ioTDevice.EquipmentId;
                deviceModel.AssetName = ioTDevice.AssetName;
                deviceModel.InactiveFlag = ioTDevice.InactiveFlag;
                deviceModel.LastReading = ioTDevice.LastReading;
                if (ioTDevice.LastReadingDate != null && ioTDevice.LastReadingDate == DateTime.MinValue)
                {
                    deviceModel.LastReadingDate = null;
                }
                else
                {
                    deviceModel.LastReadingDate = ioTDevice.LastReadingDate;
                }
                deviceModel.MonnitHeartbeat = ioTDevice.MonnitHeartbeat;
                deviceModel.MonnitLastBatteryLevel = ioTDevice.MonnitLastBatteryLevel;
                deviceModel.MonnitLastSignalStrength = ioTDevice.MonnitLastSignalStrength;
                deviceModel.MonnitNetworkID = ioTDevice.MonnitNetworkID;
                deviceModel.MonnitSensorAppID = ioTDevice.MonnitSensorAppID;
                deviceModel.SensorAlertProcedureId = ioTDevice.SensorAlertProcedureId;
                deviceModel.SensorUnit = ioTDevice.SensorUnit;
                deviceModel.TriggerHigh = ioTDevice.TriggerHigh;
                deviceModel.TriggerLow = ioTDevice.TriggerLow;
                deviceModel.SensorId = ioTDevice.SensorID;
                deviceModel.ModifyBy = ioTDevice.ModifyBy;
                deviceModel.ModifyDate = ioTDevice?.ModifyDate ?? DateTime.MinValue;
                deviceModel.CreateBy = ioTDevice.CreateBy;
                deviceModel.CreateDate = ioTDevice?.CreateDate ?? DateTime.MinValue;
                deviceModel.MeterReadingMax = ioTDevice.MeterReadingMax;
                deviceModel.MeterReadingLife = ioTDevice.MeterReadingLife;
                deviceModel.TriggerHighCrit = ioTDevice.TriggerHighCrit;
                deviceModel.TriggerLowCrit = ioTDevice.TriggerLowCrit;
                deviceModel.CMMSMeterId = ioTDevice.CMMSMeterId;
                deviceModel.CriticalProcedureId = ioTDevice.CriticalProcedureId;
                deviceModel.SensorAlertProcedureClientLooukupId = ioTDevice.SensorAlertProcedureClientLooukupId;
                deviceModel.CriticalProcedureClientLooukupId = ioTDevice.CriticalProcedureClientLooukupId;
                deviceModel.CMMSMeterClientLooukupId = ioTDevice.CMMSMeterClientLooukupId;
                deviceModel.MeterInterval = ioTDevice.MeterInterval;
            }
            return deviceModel;
        }

        public IoTDevice EditDeviceData(DeviceVM deviceVM)
        {
            IoTDevice device = new IoTDevice();
            device.ClientId = userData.DatabaseKey.Client.ClientId;
            device.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            device.IoTDeviceId = deviceVM.deviceModel.IoTDeviceId;
            device.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            device.Name = deviceVM.deviceModel.Name;
            device.EquipmentId = deviceVM.deviceModel.EquipmentId;
            device.SensorAlertProcedureId = deviceVM.deviceModel.SensorAlertProcedureId;
            device.SensorUnit = deviceVM.deviceModel.SensorUnit;
            device.TriggerHigh = deviceVM.deviceModel.TriggerHigh;
            device.TriggerLow = deviceVM.deviceModel.TriggerLow;
            device.Update(userData.DatabaseKey);
            return device;
        }

        public List<string> UpdateDeviceActiveStatus(long IoTDeviceId, bool inActiveFlag)
        {
            List<string> errList = new List<string>();
            IoTDevice device = new IoTDevice()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                IoTDeviceId = IoTDeviceId
            };
            device.Retrieve(userData.DatabaseKey);
            device.InactiveFlag = !inActiveFlag;
            if (device.LastReadingDate != null && device.LastReadingDate == DateTime.MinValue)
            {
                device.LastReadingDate = null;
            }
            device.Update(userData.DatabaseKey);
            if (device.ErrorMessages == null || device.ErrorMessages.Count <= 0)
            {
                Site site = new Site
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = _dbKey.Personnel.SiteId
                };
                site.UpdateIoTDeviceCount(_dbKey);
                string eventmsg = (inActiveFlag == true) ? "ActivateDevice" : "InactDevice";
                errList = CreateIoTDeviceEventLog(IoTDeviceId, eventmsg);
                return errList;
            }
            else
            {
                return device.ErrorMessages;
            }

        }

        public List<String> ChangeIoTDeviceId(ChangeDeviceIDModel _ChangeDeviceIDModel)
        {
            List<string> errList = new List<string>();

            if (_ChangeDeviceIDModel.IoTDeviceId > 0)
            {
                IoTDevice device = new IoTDevice();
                device.ClientId = userData.DatabaseKey.Client.ClientId;
                device.SiteId = userData.DatabaseKey.Personnel.SiteId;
                device.IoTDeviceId = _ChangeDeviceIDModel.IoTDeviceId;
                device.Retrieve(userData.DatabaseKey);
                device.ClientLookupId = _ChangeDeviceIDModel.ClientLookupId;
                if (device.LastReadingDate != null && device.LastReadingDate == DateTime.MinValue)
                {
                    device.LastReadingDate = null;
                }
                device.CheckDuplicateIoTDevice(this.userData.DatabaseKey);
                if (device.ErrorMessages == null || device.ErrorMessages.Count == 0)
                {

                    device.Update(userData.DatabaseKey);
                    if (device.ErrorMessages == null || device.ErrorMessages.Count <= 0)
                    {
                        errList = CreateIoTDeviceEventLog(_ChangeDeviceIDModel.IoTDeviceId, "ChangeDeviceID");
                        return errList;
                    }
                    else
                    {
                        return device.ErrorMessages;
                    }
                }
                else
                {
                    return device.ErrorMessages;
                }
            }
            else
            { return errList; }
        }
        public List<string> CreateIoTDeviceEventLog(long IoTDeviceId, string Event)
        {
            IoTDeviceEventLog ioTDeviceEventLog = new IoTDeviceEventLog();
            ioTDeviceEventLog.ClientId = userData.DatabaseKey.User.ClientId;
            ioTDeviceEventLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            ioTDeviceEventLog.IoTDeviceId = IoTDeviceId;
            ioTDeviceEventLog.TransactionDate = DateTime.UtcNow;
            ioTDeviceEventLog.Event = Event;
            ioTDeviceEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            ioTDeviceEventLog.Comments = "";
            ioTDeviceEventLog.Create(userData.DatabaseKey);
            return ioTDeviceEventLog.ErrorMessages;
        }
        #endregion

        #region Event

        public List<DeviceEventModel> GetEventdetails(long ioTDeviceId, string deviceClientLookupId)
        {
            DataContracts.EventInfo eventinfo = new DataContracts.EventInfo();
            eventinfo.ClientId = userData.DatabaseKey.Client.ClientId;
            eventinfo.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            eventinfo.IotClientlookupId = deviceClientLookupId;// "2";
            var dataList = eventinfo.EventInfo_RetrieveForIoT(userData.DatabaseKey, userData.Site.TimeZone);

            DeviceEventModel deviceEventModel;
            List<DeviceEventModel> DeviceEventModelList = new List<DeviceEventModel>();
            foreach (var item in dataList)
            {
                deviceEventModel = new DeviceEventModel();
                deviceEventModel.IoTDeviceId = ioTDeviceId;
                deviceEventModel.DeviceClientLookupId = deviceClientLookupId;
                deviceEventModel.EventID = item.EventInfoId;
                deviceEventModel.Description = item.Description;
                deviceEventModel.Status = item.Status;
                deviceEventModel.Disposition = item.Disposition;
                deviceEventModel.Created = item.CreateDate;
                DeviceEventModelList.Add(deviceEventModel);
            }
            return DeviceEventModelList;
        }

        #endregion

        #region Gauge

        #endregion

        #region V2-536
        #region Add/Edit
        public DeviceAPMOnlyModel GetDetailsForApmOnlyEdit(DeviceModel ioTDevice)
        {
            DeviceAPMOnlyModel deviceModel = new DeviceAPMOnlyModel();
            if (ioTDevice != null)
            {
                deviceModel.IoTDeviceId = ioTDevice.IoTDeviceId;
                deviceModel.ClientLookupID = ioTDevice.ClientLookupID;
                deviceModel.Name = ioTDevice.Name;
                deviceModel.EquipmentId = ioTDevice.EquipmentId;
                deviceModel.Equipment_ClientLookupId = ioTDevice.AssetID;
                deviceModel.MeterInterval = ioTDevice.MeterInterval;
                deviceModel.SensorUnit = ioTDevice.SensorUnit;
                deviceModel.SensorAlertProcedureId = ioTDevice.SensorAlertProcedureId;
                deviceModel.SensorAlertProcedure_ClientLookupId = ioTDevice.SensorAlertProcedureClientLooukupId;
                deviceModel.MeterReadingLife = ioTDevice.MeterReadingLife;
                deviceModel.MeterReadingMax = ioTDevice.MeterReadingMax;
                deviceModel.SensorId = ioTDevice.SensorId;
            }
            return deviceModel;
        }
        public DeviceAPMCMMSModel GetDetailsForApmCMMSEdit(DeviceModel ioTDevice)
        {
            DeviceAPMCMMSModel deviceModel = new DeviceAPMCMMSModel();
            if (ioTDevice != null)
            {
                deviceModel.IoTDeviceId = ioTDevice.IoTDeviceId;
                deviceModel.ClientLookupID = ioTDevice.ClientLookupID;
                deviceModel.Name = ioTDevice.Name;
                deviceModel.EquipmentId = ioTDevice.EquipmentId;
                deviceModel.Equipment_ClientLookupId = ioTDevice.AssetID;
                deviceModel.SensorId = ioTDevice.SensorId;
            }
            return deviceModel;
        }
        public DeviceMonnitSensorModel GetDetailsForMonnitSensorEdit(DeviceModel ioTDevice)
        {
            DeviceMonnitSensorModel deviceModel = new DeviceMonnitSensorModel();
            if (ioTDevice != null)
            {
                deviceModel.IoTDeviceId = ioTDevice.IoTDeviceId;
                deviceModel.ClientLookupID = ioTDevice.ClientLookupID;
                deviceModel.Name = ioTDevice.Name;
                deviceModel.EquipmentId = ioTDevice.EquipmentId;
                deviceModel.Equipment_ClientLookupId = ioTDevice.AssetID;
                deviceModel.SensorUnit = ioTDevice.SensorUnit;
                deviceModel.TriggerHigh = ioTDevice.TriggerHigh;
                deviceModel.TriggerLow = ioTDevice.TriggerLow;
                deviceModel.SensorAlertProcedureId = ioTDevice.SensorAlertProcedureId;
                deviceModel.SensorAlertProcedure_ClientLookupId = ioTDevice.SensorAlertProcedureClientLooukupId;
                deviceModel.TriggerHighCrit = ioTDevice.TriggerHighCrit;
                deviceModel.TriggerLowCrit = ioTDevice.TriggerLowCrit;
                deviceModel.CriticalProcedureId = ioTDevice.CriticalProcedureId;
                deviceModel.CriticalProcedure_ClientLookupId = ioTDevice.CriticalProcedureClientLooukupId;
                deviceModel.SensorId = ioTDevice.SensorId;
            }
            return deviceModel;
        }
        public DeviceSensorModel GetDetailsForSensorEdit(DeviceModel ioTDevice)
        {
            DeviceSensorModel deviceModel = new DeviceSensorModel();
            if (ioTDevice != null)
            {
                deviceModel.IoTDeviceId = ioTDevice.IoTDeviceId;
                deviceModel.ClientLookupID = ioTDevice.ClientLookupID;
                deviceModel.Name = ioTDevice.Name;
                deviceModel.EquipmentId = ioTDevice.EquipmentId;
                deviceModel.Equipment_ClientLookupId = ioTDevice.AssetID;
                deviceModel.SensorUnit = ioTDevice.SensorUnit;
                deviceModel.TriggerHigh = ioTDevice.TriggerHigh;
                deviceModel.TriggerLow = ioTDevice.TriggerLow;
                deviceModel.SensorAlertProcedureId = ioTDevice.SensorAlertProcedureId;
                deviceModel.SensorAlertProcedure_ClientLookupId = ioTDevice.SensorAlertProcedureClientLooukupId;
                deviceModel.TriggerHighCrit = ioTDevice.TriggerHighCrit;
                deviceModel.TriggerLowCrit = ioTDevice.TriggerLowCrit;
                deviceModel.CriticalProcedureId = ioTDevice.CriticalProcedureId;
                deviceModel.CriticalProcedure_ClientLookupId = ioTDevice.CriticalProcedureClientLooukupId;
                deviceModel.SensorId = ioTDevice.SensorId;
            }
            return deviceModel;
        }
        public IoTDevice SaveDeviceAPMOnly(DeviceAPMOnlyModel _deviceModel)
        {
            IoTDevice objDevice = new IoTDevice();
            if (_deviceModel.IoTDeviceId != 0)
            {
                objDevice = EditDeviceAPMOnly(_deviceModel);
            }
            else
            {
                objDevice = AddDeviceAPMOnly(_deviceModel);
            }
            return objDevice;
        }
        public IoTDevice AddDeviceAPMOnly(DeviceAPMOnlyModel deviceModel)
        {
            IoTDevice device = new IoTDevice();

            device.ClientId = userData.DatabaseKey.Client.ClientId;
            device.SiteId = _dbKey.Personnel.SiteId;
            device.ClientLookupId = deviceModel.ClientLookupID;
            device.Name = deviceModel.Name;
            device.IoTDeviceCategory = "Meter";
            device.SensorType = "Meter";
            device.EquipmentId = deviceModel.EquipmentId;
            device.LastReading = deviceModel.LastReading ?? 0;
            device.LastReadingDate = deviceModel.LastReadingDate;
            device.MeterInterval = deviceModel.MeterInterval ?? 0;
            device.MeterReadingLife = deviceModel.MeterReadingLife ?? 0;
            device.MeterReadingMax = deviceModel.MeterReadingMax ?? 0;
            device.SensorAlertProcedureId = deviceModel.SensorAlertProcedureId;
            device.SensorUnit = deviceModel.SensorUnit;
            device.ValidateForAddDevice(_dbKey);
            if (device.ErrorMessages == null || device.ErrorMessages.Count() == 0)
            {
                device.Create(_dbKey);
                Site site = new Site
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = _dbKey.Personnel.SiteId
                };
                site.UpdateIoTDeviceCount(_dbKey);
                CreateIoTDeviceEventLog(device.IoTDeviceId, "CreateDevice");
            }

            return device;
        }
        public IoTDevice EditDeviceAPMOnly(DeviceAPMOnlyModel deviceModel)
        {
            IoTDevice device = new IoTDevice()
            {
                IoTDeviceId = deviceModel.IoTDeviceId
            };
            device.Retrieve(_dbKey);

            device.Name = deviceModel.Name;
            device.EquipmentId = deviceModel.EquipmentId;
            device.MeterInterval = deviceModel.MeterInterval ?? 0;
            device.SensorUnit = deviceModel.SensorUnit;
            device.SensorAlertProcedureId = deviceModel.SensorAlertProcedureId;
            device.MeterReadingLife = deviceModel.MeterReadingLife ?? 0;
            device.MeterReadingMax = deviceModel.MeterReadingMax ?? 0;
            if (device.LastReadingDate != null && device.LastReadingDate == DateTime.MinValue)
            {
                device.LastReadingDate = null;
            }
            device.Update(userData.DatabaseKey);
            return device;
        }
        public IoTDevice SaveDeviceAPMCMMS(DeviceAPMCMMSModel _deviceModel)
        {
            IoTDevice objDevice = new IoTDevice();
            if (_deviceModel.IoTDeviceId != 0)
            {
                objDevice = EditDeviceAPMCMMS(_deviceModel);
            }
            else
            {
                objDevice = AddDeviceAPMCMMS(_deviceModel);
            }
            return objDevice;
        }
        public IoTDevice AddDeviceAPMCMMS(DeviceAPMCMMSModel deviceModel)
        {
            IoTDevice device = new IoTDevice();

            device.ClientId = userData.DatabaseKey.Client.ClientId;
            device.SiteId = _dbKey.Personnel.SiteId;
            device.CMMSMeterId = deviceModel.CMMSMeterId;
            device.IoTDeviceCategory = "Meter";
            device.SensorType = "Meter";
            Meter meter = new Meter()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = _dbKey.Personnel.SiteId,
                MeterId = deviceModel.CMMSMeterId
            };
            meter.Retrieve(_dbKey);
            device.ClientLookupId = meter.ClientLookupId;
            device.Name = meter.Name;
            device.EquipmentId = deviceModel.EquipmentId;
            device.LastReading = meter.ReadingCurrent;
            device.LastReadingDate = meter.ReadingDate;
            device.MeterReadingLife = meter.ReadingLife;
            device.MeterReadingMax = meter.ReadingMax;
            device.SensorUnit = meter.ReadingUnits;
            device.ValidateForAddDevice(_dbKey);
            if (device.ErrorMessages == null || device.ErrorMessages.Count() == 0)
            {
                device.Create(_dbKey);
                Site site = new Site
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = _dbKey.Personnel.SiteId
                };
                meter.IoTDeviceId = device.IoTDeviceId;
                meter.Update(_dbKey);
                site.UpdateIoTDeviceCount(_dbKey);
                CreateIoTDeviceEventLog(device.IoTDeviceId, "CreateDevice");
            }

            return device;
        }

        public IoTDevice EditDeviceAPMCMMS(DeviceAPMCMMSModel deviceModel)
        {
            IoTDevice device = new IoTDevice()
            {
                IoTDeviceId = deviceModel.IoTDeviceId
            };
            device.Retrieve(_dbKey);

            device.Name = deviceModel.Name;
            device.EquipmentId = deviceModel.EquipmentId;
            if (device.LastReadingDate != null && device.LastReadingDate == DateTime.MinValue)
            {
                device.LastReadingDate = null;
            }
            device.Update(userData.DatabaseKey);
            return device;
        }
        public IoTDevice SaveDeviceMonnitSensor(DeviceMonnitSensorModel _deviceModel)
        {
            IoTDevice objDevice = new IoTDevice();
            if (_deviceModel.IoTDeviceId != 0)
            {
                objDevice = EditDeviceMonnitSensor(_deviceModel);
            }
            else
            {
                objDevice = AddDeviceMonnitSensor(_deviceModel);
            }
            return objDevice;
        }
        public IoTDevice AddDeviceMonnitSensor(DeviceMonnitSensorModel deviceModel)
        {
            IoTDevice device = new IoTDevice();

            device.ClientId = userData.DatabaseKey.Client.ClientId;
            device.SiteId = _dbKey.Personnel.SiteId;
            device.ClientLookupId = deviceModel.ClientLookupID;
            device.Name = deviceModel.Name;
            device.IoTDeviceCategory = "MonnitSensor";
            device.SensorType = deviceModel.SensorType;
            device.EquipmentId = deviceModel.EquipmentId ?? 0;
            device.SensorAlertProcedureId = deviceModel.SensorAlertProcedureId ?? 0;
            device.SensorUnit = deviceModel.SensorUnit;
            device.TriggerHigh = deviceModel.TriggerHigh ?? 0;
            device.TriggerLow = deviceModel.TriggerLow ?? 0;
            device.TriggerHighCrit = deviceModel.TriggerHighCrit ?? 0;
            device.TriggerLowCrit = deviceModel.TriggerLowCrit ?? 0;
            device.CriticalProcedureId = deviceModel.CriticalProcedureId ?? 0;
            device.ValidateForAddDevice(_dbKey);
            if (device.ErrorMessages == null || device.ErrorMessages.Count() == 0)
            {
                device.Create(_dbKey);
                Site site = new Site
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = _dbKey.Personnel.SiteId
                };
                site.UpdateIoTDeviceCount(_dbKey);
                CreateIoTDeviceEventLog(device.IoTDeviceId, "CreateDevice");
            }

            return device;

        }
        public IoTDevice EditDeviceMonnitSensor(DeviceMonnitSensorModel deviceModel)
        {
            IoTDevice device = new IoTDevice()
            {
                IoTDeviceId = deviceModel.IoTDeviceId
            };
            device.Retrieve(_dbKey);

            device.Name = deviceModel.Name;
            device.EquipmentId = deviceModel.EquipmentId ?? 0;
            device.SensorUnit = deviceModel.SensorUnit;
            device.TriggerHigh = deviceModel.TriggerHigh ?? 0;
            device.TriggerLow = deviceModel.TriggerLow ?? 0;
            device.SensorAlertProcedureId = deviceModel.SensorAlertProcedureId ?? 0;
            device.TriggerHighCrit = deviceModel.TriggerHighCrit ?? 0;
            device.TriggerLowCrit = deviceModel.TriggerLowCrit ?? 0;
            device.CriticalProcedureId = deviceModel.CriticalProcedureId ?? 0;
            if (device.LastReadingDate != null && device.LastReadingDate == DateTime.MinValue)
            {
                device.LastReadingDate = null;
            }
            device.Update(userData.DatabaseKey);
            return device;
        }
        public IoTDevice SaveDeviceSensor(DeviceSensorModel _deviceModel)
        {
            IoTDevice objDevice = new IoTDevice();
            if (_deviceModel.IoTDeviceId != 0)
            {
                objDevice = EditDeviceSensor(_deviceModel);
            }
            else
            {
                objDevice = AddDeviceSensor(_deviceModel);
            }
            return objDevice;
        }
        public IoTDevice AddDeviceSensor(DeviceSensorModel deviceModel)
        {
            IoTDevice device = new IoTDevice();

            device.ClientId = userData.DatabaseKey.Client.ClientId;
            device.SiteId = _dbKey.Personnel.SiteId;
            device.ClientLookupId = deviceModel.ClientLookupID;
            device.Name = deviceModel.Name;
            device.IoTDeviceCategory = "Sensor";
            device.SensorType = deviceModel.SensorType;
            device.EquipmentId = deviceModel.EquipmentId ?? 0;
            device.SensorAlertProcedureId = deviceModel.SensorAlertProcedureId ?? 0;
            device.SensorUnit = deviceModel.SensorUnit;
            device.TriggerHigh = deviceModel.TriggerHigh ?? 0;
            device.TriggerLow = deviceModel.TriggerLow ?? 0;
            device.TriggerHighCrit = deviceModel.TriggerHighCrit ?? 0;
            device.TriggerLowCrit = deviceModel.TriggerLowCrit ?? 0;
            device.CriticalProcedureId = deviceModel.CriticalProcedureId ?? 0;
            device.ValidateForAddDevice(_dbKey);
            if (device.ErrorMessages == null || device.ErrorMessages.Count() == 0)
            {
                device.Create(_dbKey);
                Site site = new Site
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = _dbKey.Personnel.SiteId
                };
                site.UpdateIoTDeviceCount(_dbKey);
                CreateIoTDeviceEventLog(device.IoTDeviceId, "CreateDevice");
            }

            return device;

        }
        public IoTDevice EditDeviceSensor(DeviceSensorModel deviceModel)
        {
            IoTDevice device = new IoTDevice()
            {
                IoTDeviceId = deviceModel.IoTDeviceId
            };
            device.Retrieve(_dbKey);

            device.Name = deviceModel.Name;
            device.EquipmentId = deviceModel.EquipmentId ?? 0;
            device.SensorUnit = deviceModel.SensorUnit;
            device.TriggerHigh = deviceModel.TriggerHigh ?? 0;
            device.TriggerLow = deviceModel.TriggerLow ?? 0;
            device.SensorAlertProcedureId = deviceModel.SensorAlertProcedureId ?? 0;
            device.TriggerHighCrit = deviceModel.TriggerHighCrit ?? 0;
            device.TriggerLowCrit = deviceModel.TriggerLowCrit ?? 0;
            device.CriticalProcedureId = deviceModel.CriticalProcedureId ?? 0;
            if (device.LastReadingDate != null && device.LastReadingDate == DateTime.MinValue)
            {
                device.LastReadingDate = null;
            }
            device.Update(userData.DatabaseKey);
            return device;
        }
        #endregion
        #endregion
        #region V2-1103
        public List<string> AddRecordReading(AddRecordReadingModal addRecordReadingModal)
        {
            List<string> errList = new List<string>();

            // Check if IoTDeviceId is valid
            if (addRecordReadingModal.IoTDeviceId <= 0)
            {
                return errList;
            }

            // Retrieve the IoTDevice
            IoTDevice device = new IoTDevice
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.Personnel.SiteId,
                IoTDeviceId = addRecordReadingModal.IoTDeviceId
            };
            device.Retrieve(userData.DatabaseKey);

            // Create a new IoTReading
            IoTReading reading = new IoTReading
            {
                ClientId = device.ClientId,
                SiteId = device.SiteId,
                IoTDeviceId = device.IoTDeviceId,
                ReadingUnit = device.SensorUnit,
                Reading = addRecordReadingModal.Reading
            };

            // Parse the reading date and time
            DateTime readingDate = DateTime.ParseExact(
                $"{addRecordReadingModal.ReadingDate} {addRecordReadingModal.ReadingTime}",
                "MM/dd/yyyy h:mm tt",
                CultureInfo.InvariantCulture
            );
            reading.ReadingDate = readingDate == DateTime.MinValue ? (DateTime?)null : readingDate;

            // Check for errors in reading
            if (reading.ErrorMessages != null && reading.ErrorMessages.Count > 0)
            {
                return reading.ErrorMessages;
            }

            // Create IoTReading
            reading.Create(userData.DatabaseKey);

            // Check if reading triggers a warning event
            if ((reading.Reading > device.TriggerHigh && reading.Reading < device.TriggerHighCrit) ||
                (reading.Reading < device.TriggerLow && reading.Reading > device.TriggerLowCrit))
            {
                return HandleEvent(reading, device, EventTypeConstants.Warning, AlertTypeEnum.APMWarningEvent);
            }

            // Check if reading triggers a critical event
            else if (reading.Reading > device.TriggerHighCrit || reading.Reading < device.TriggerLowCrit)
            {
                return HandleEvent(reading, device, EventTypeConstants.Critical, AlertTypeEnum.APMCriticalEvent);
            }
            return errList;
        }

        private List<string> HandleEvent(IoTReading reading, IoTDevice device, string eventType, AlertTypeEnum alertType)
        {
            // Create IoTEvent
            DataContracts.IoTEvent ioTEvent = CreateIoTEventLog(reading.IoTDeviceId, device.EquipmentId, eventType);

            // Create alert
            ProcessAlert objAlert = new ProcessAlert(userData);
            List<long> ioTEventIds = new List<long> { ioTEvent.IoTEventId };
            Task createAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.IoTEvent>(alertType, ioTEventIds));

            // Update IoTDevice
            if (ioTEvent.ErrorMessages == null || ioTEvent.ErrorMessages.Count <= 0)
            {
                UpdateIoTDevice(reading);
            }

            // Update IoTReading
            if (reading.ErrorMessages == null || reading.ErrorMessages.Count <= 0)
            {
                UpdateIoTReading(reading, ioTEvent.IoTEventId);
            }

            return ioTEvent.ErrorMessages ?? reading.ErrorMessages;
        }

        private void UpdateIoTDevice(IoTReading reading)
        {
            IoTDevice iotDevice = new IoTDevice
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                IoTDeviceId = reading.IoTDeviceId
            };
            iotDevice.Retrieve(userData.DatabaseKey);
            iotDevice.LastReading = reading.Reading;
            iotDevice.LastReadingDate = reading.ReadingDate;
            iotDevice.MeterLastDone = reading.Reading;
            iotDevice.MeterLastDoneDate = reading.ReadingDate;
            iotDevice.Update(userData.DatabaseKey);
        }

        private void UpdateIoTReading(IoTReading reading, long ioTEventId)
        {
            IoTReading ioTReading = new IoTReading
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                IoTReadingId = reading.IoTReadingId
            };
            ioTReading.Retrieve(userData.DatabaseKey);
            ioTReading.IoTEventId = ioTEventId;
            ioTReading.Update(userData.DatabaseKey);
        }

        public DataContracts.IoTEvent CreateIoTEventLog(long IoTDeviceId, long EquipmentId, string Event)
        {
            DataContracts.IoTEvent ei = new DataContracts.IoTEvent()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId
            };
            ei.IoTDeviceId = IoTDeviceId;
            ei.EquipmentId = EquipmentId;
            ei.EventType = Event;
            ei.Status = EventStatusConstants.Open;
            ei.ProcessBy_PersonnelId = 0;
            ei.ProcessDate = null;
            ei.Disposition = "";
            ei.DismissReason = "";
            ei.WorkOrderId = 0;
            ei.FaultCode = "";
            ei.Comments = "";
            ei.Create(userData.DatabaseKey);
            return ei;
        }
        #endregion
    }

}