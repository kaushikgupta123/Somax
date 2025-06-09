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
using System.Data.SqlClient;

namespace DataContracts
{

    public partial class IoTEvent : DataContractBase
    {
        public int CustomQueryDisplayId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public int TotalCount { get; set; }
        public string SearchText { get; set; }
        public DateTime? CreateDate { get; set; }
        public string AssetId { get; set; }
        public string AssetName { get; set; }
        public string ProcessBy_Personnel { get; set; }
        public string Status_Display { get; set; }
        public string WOClientLookupId { get; set; }
        public string IoTDeviceClientLookupId { get; set; }
        public string EquipClientLookupId { get; set; }
        #region V2-538
        public decimal IoTReading_Reading { get; set; }
        public string IoTDevice_SensorUnit { get; set; }
        public decimal IoTDevice_MeterInterval { get; set; }
        public decimal IoTDevice_TriggerHigh { get; set; }
        public decimal IoTDevice_TriggerLow { get; set; }
        public decimal IoTDevice_TriggerHighCrit { get; set; }
        public decimal IoTDevice_TriggerLowCrit { get; set; }
        #endregion
        public b_IoTEvent ToDateBaseObjectExtended()
        {
            b_IoTEvent dbObj = this.ToDatabaseObject();
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.SiteId = this.SiteId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.IoTEventId = this.IoTEventId;
            dbObj.SourceType = this.SourceType;
            dbObj.EventType = this.EventType;
            dbObj.Disposition = this.Disposition;
            dbObj.WOClientLookupId = this.WOClientLookupId;
            dbObj.FaultCode = this.FaultCode;
            dbObj.IoTDeviceClientLookupId = this.IoTDeviceClientLookupId;
            dbObj.CreateDate = this.CreateDate;
            dbObj.AssetId = this.AssetId;
            dbObj.SearchText = this.SearchText;

            return dbObj;
        }
        public void UpdateFromDatabaseObjectExtended(b_IoTEvent dbObj, string TimeZone)
        {
            this.ClientId = dbObj.ClientId;
            this.IoTEventId = dbObj.IoTEventId;
            this.EventType = dbObj.EventType;
            this.SourceType = dbObj.SourceType;
            this.AssetId = dbObj.AssetId;
            this.AssetName = dbObj.AssetName;
            this.Status = dbObj.Status;
            this.Disposition = dbObj.Disposition;
            this.WOClientLookupId = dbObj.WOClientLookupId;
            this.FaultCode = dbObj.FaultCode;
            this.CreateDate = dbObj.CreateDate;
            this.IoTDeviceClientLookupId = dbObj.IoTDeviceClientLookupId;
            this.ProcessDate = dbObj.ProcessDate;
            this.ProcessBy_Personnel = dbObj.ProcessBy_Personnel;
            this.Comments = dbObj.Comments;
            this.TotalCount = dbObj.TotalCount;
        }
        public List<IoTEvent> IoTEventRetrieveChunkSearch(DatabaseKey dbKey, string TimeZone)
        {
            IotEvent_RetrieveChunkSearchFromDetails trans = new IotEvent_RetrieveChunkSearchFromDetails
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.IoTEvent = ToDateBaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();



            List<IoTEvent> IoTEventlist = new List<IoTEvent>();
            foreach (b_IoTEvent IoTEvent in trans.IoTEventList)
            {
                IoTEvent tmpIoTEvent = new IoTEvent();

                tmpIoTEvent.UpdateFromDatabaseObjectExtended(IoTEvent, TimeZone);
                IoTEventlist.Add(tmpIoTEvent);
            }
            return IoTEventlist;
        }

        public void RetrieveByPKForeignkey(DatabaseKey dbKey)
        {
            IoTEvent_RetrieveByPKForeignkey trans = new IoTEvent_RetrieveByPKForeignkey()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };

            trans.IoTEvent = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.IoTEvent);

            this.ProcessBy_Personnel = trans.IoTEvent.ProcessBy_Personnel;
            this.CreateDate = trans.IoTEvent.CreateDate;
            this.WOClientLookupId = trans.IoTEvent.WOClientLookupId;
            this.IoTDeviceClientLookupId = trans.IoTEvent.IoTDeviceClientLookupId;
            this.EquipClientLookupId = trans.IoTEvent.EquipClientLookupId;
            this.AssetName = trans.IoTEvent.AssetName;
            this.DismissReason = trans.IoTEvent.DismissReason;
            this.FaultCode = trans.IoTEvent.FaultCode;
            switch (this.Status)
            {
                case Common.Constants.EventStatusConstants.Open:
                    this.Status_Display = "Open";
                    break;
                case Common.Constants.EventStatusConstants.Processed:
                    this.Status_Display = "Processed";
                    break;

                default:
                    this.Status_Display = string.Empty;
                    break;
            }
            this.IoTReading_Reading = trans.IoTEvent.IoTReading_Reading;
            this.IoTDevice_MeterInterval = trans.IoTEvent.IoTDevice_MeterInterval;
            this.IoTDevice_SensorUnit = trans.IoTEvent.IoTDevice_SensorUnit;
            this.IoTDevice_TriggerHigh = trans.IoTEvent.IoTDevice_TriggerHigh;
            this.IoTDevice_TriggerLow = trans.IoTEvent.IoTDevice_TriggerLow;
            this.IoTDevice_TriggerHighCrit = trans.IoTEvent.IoTDevice_TriggerHighCrit;
            this.IoTDevice_TriggerLowCrit = trans.IoTEvent.IoTDevice_TriggerLowCrit;
        }
    }
}



