using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Database;
using Database.Business;
using Common.Extensions;
using Newtonsoft.Json;

namespace DataContracts
{
    public partial class IoTDevice : DataContractBase, IStoredProcedureValidation
    {
        public int CustomQueryDisplayId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string SearchText { get; set; }
        public string DeviceId { get; set; }
        public string AssetId { get; set; }
        public string AssetCategory { get; set; }
        public string AssetName { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public List<IoTDevice> listOfDevice { get; set; }

        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int TotalCount { get; set; }
        public int CaseNo { get; set; }
        public string ValidateFor = string.Empty;
        #region V2-536
        public string SensorAlertProcedureClientLooukupId { get; set; }
        public string CriticalProcedureClientLooukupId { get; set; }
        public string CMMSMeterClientLooukupId { get; set; }
        #endregion
        public IoTDevice CustomSearch(DatabaseKey dbKey, string TimeZone)
        {
            IoTDevice_RetrieveV2 trans = new IoTDevice_RetrieveV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.IoTDevice = this.ToDateBaseObjectForRetriveAllForSearchV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();  
            
            this.listOfDevice = new List<IoTDevice>();
                       
            List<IoTDevice> IoTDevicelist = new List<IoTDevice>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_IoTDevice IoTDevice in trans.IoTDevice.listOfDevice)
            {
                IoTDevice tmpIoTDevice = new IoTDevice();

                tmpIoTDevice.UpdateFromDatabaseObjectForRetriveV2(IoTDevice, TimeZone);
                IoTDevicelist.Add(tmpIoTDevice);
            }
            this.listOfDevice.AddRange(IoTDevicelist);
            return this;
        }

        public b_IoTDevice ToDateBaseObjectForRetriveAllForSearchV2()
        {         
            b_IoTDevice dbObj = new b_IoTDevice();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.AssetId = this.AssetId;
            dbObj.AssetCategory = this.AssetCategory;
            dbObj.Name = this.Name;
            dbObj.SensorType = this.SensorType;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveV2(b_IoTDevice dbObj, string TimeZone)
        {
         
            this.ClientId = dbObj.ClientId;
            this.IoTDeviceId = dbObj.IoTDeviceId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Name = dbObj.Name;
            this.IoTDeviceCategory = dbObj.IoTDeviceCategory;
            this.SensorType = dbObj.SensorType;
            this.EquipmentId = dbObj.EquipmentId;
            this.LastReading = dbObj.LastReading;
            this.SensorID = dbObj.SensorID;
            this.AssetId = dbObj.AssetId;
            this.AssetName = dbObj.AssetName;
            this.LastReadingDate = dbObj.LastReadingDate;
            this.InactiveFlag = dbObj.InactiveFlag;
            this.TotalCount = dbObj.TotalCount;
          }


        public void RetrieveByPKForeignKeys(DatabaseKey dbKey,string Timezone)
        {
            IoTDevice_RetrieveByForeignKeys trans = new IoTDevice_RetrieveByForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.IoTDevice = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.IoTDevice);
            this.AssetId = trans.IoTDevice.AssetId;
            this.AssetName= trans.IoTDevice.AssetName;
            this.ModifyBy = trans.IoTDevice.ModifyBy;
            this.ModifyDate = trans.IoTDevice.ModifyDate;
            this.CreateBy = trans.IoTDevice.CreateBy;
            if (trans.IoTDevice.CreateDate != null && trans.IoTDevice.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = trans.IoTDevice.CreateDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CreateDate = trans.IoTDevice.CreateDate;
            }

            if (trans.IoTDevice.ModifyDate != null && trans.IoTDevice.ModifyDate != DateTime.MinValue)
            {
                this.ModifyDate = trans.IoTDevice.ModifyDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.ModifyDate = trans.IoTDevice.ModifyDate;
            }
            this.SensorAlertProcedureClientLooukupId = trans.IoTDevice.SensorAlertProcedureClientLooukupId;
            this.CriticalProcedureClientLooukupId = trans.IoTDevice.CriticalProcedureClientLooukupId;
            this.CMMSMeterClientLooukupId = trans.IoTDevice.CMMSMeterClientLooukupId;
        }



        public List<IoTDevice> RetrieveIoTDeviceForPrint(DatabaseKey dbKey, string TimeZone)
        {
            IoTDevice_RetrieveWorkOrderForPrint trans = new IoTDevice_RetrieveWorkOrderForPrint()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.IoTDevice = this.ToDatabaseObject();
            trans.IoTDevice.CaseNo = this.CaseNo;
            trans.IoTDevice.SearchText = this.SearchText;            
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<IoTDevice> IoTDevicelist = new List<IoTDevice>();

            foreach (b_IoTDevice IoTDevice in trans.IoTDeviceList)
            {
                IoTDevice tmpWorkOrder = new IoTDevice();
                // RKL - Moved to the .UpdateFromDatabaseObjectForRetriveAll method
                tmpWorkOrder.UpdateFromDatabaseObjectForRetriveV2(IoTDevice, TimeZone);
                IoTDevicelist.Add(tmpWorkOrder);
            }
            return IoTDevicelist;
        }

        #region V2-537
        public IoTDevice RetrieveChunkSearchByEquipmentId(DatabaseKey dbKey, string TimeZone)
        {
            IoTDevice_RetrieveChunkSearchByEquipmentIdV2 trans = new IoTDevice_RetrieveChunkSearchByEquipmentIdV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.IoTDevice = this.ToDateBaseObjectForRetrieveChunkSearchByEquipmentId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfDevice = new List<IoTDevice>();

            List<IoTDevice> IoTDevicelist = new List<IoTDevice>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_IoTDevice IoTDevice in trans.IoTDevice.listOfDevice)
            {
                IoTDevice tmpIoTDevice = new IoTDevice();

                tmpIoTDevice.UpdateFromDatabaseObjectForRetrieveChunkSearchByEquipmentId(IoTDevice, TimeZone);
                IoTDevicelist.Add(tmpIoTDevice);
            }
            this.listOfDevice.AddRange(IoTDevicelist);
            return this;
        }

        public b_IoTDevice ToDateBaseObjectForRetrieveChunkSearchByEquipmentId()
        {
            b_IoTDevice dbObj = new b_IoTDevice();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.EquipmentId = this.EquipmentId;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetrieveChunkSearchByEquipmentId(b_IoTDevice dbObj, string TimeZone)
        {

            this.ClientId = dbObj.ClientId;
            this.IoTDeviceId = dbObj.IoTDeviceId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Name = dbObj.Name;
            this.SensorType = dbObj.SensorType;
            this.SensorUnit = dbObj.SensorUnit;
            this.LastReading = dbObj.LastReading;
            if (dbObj.LastReadingDate != null && dbObj.LastReadingDate != DateTime.MinValue)
            {
                this.LastReadingDate = dbObj.LastReadingDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.LastReadingDate = dbObj.LastReadingDate;
            }
            this.InactiveFlag = dbObj.InactiveFlag;
            this.TotalCount = dbObj.TotalCount;
        }
        #endregion


        #region V2-536
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();           
           if (ValidateFor == "CheckDuplicate")
            {
                IoTDevice_ValidateByClientLookupId_V2 trans = new IoTDevice_ValidateByClientLookupId_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.IoTDevice = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);

            }                     
           if (ValidateFor == "Add")
            {
                IoTDevice_ValidateAdd_V2 trans = new IoTDevice_ValidateAdd_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.IoTDevice = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);

            }           

            return errors;
        }
        public void CheckDuplicateIoTDevice(DatabaseKey dbKey)
        {
            ValidateFor = "CheckDuplicate";
            Validate<IoTDevice>(dbKey);
        }
        public void ValidateForAddDevice(DatabaseKey dbKey)
        {
            ValidateFor = "Add";
            Validate<IoTDevice>(dbKey);
        }
        #endregion
    }
}



