using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;
using Database;
using Database.Business;
using Common.Structures;

namespace DataContracts
{

    public partial class SanMasterBatchEntry : DataContractBase, IStoredProcedureValidation
    {
        public string MasterJob { get; set; }
        public string MasterDescription { get; set; }
        public string ChargeType { get; set; }
        public string ChargeTo { get; set; }
        public string Shift { get; set; }
        public long Frequency { get; set; }
        public long siteid { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string EquipmentName { get; set; }
     

        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }

        public long OffSetVal { get; set; }
        public long NextRow { get; set; }

        public DateTime? ScheduleThroughDate { get; set; }

        public string ScheduleType { get; set; }
        public string ScheduleThroughDateString { get; set; }
        public string OnDemandgroup { get; set; }
        public bool PrintSanitationJobs { get; set; }
        public bool PrintAttachments { get; set; }
        public string AssetGroup1Ids { get; set; }
        public string AssetGroup2Ids { get; set; }
        public string AssetGroup3Ids { get; set; }
        public int TotalCount { get; set; }
    
  
        public List<SanMasterBatchEntry> SanBatchEntryForSanitationMasterFromSanitationJob_V2(DatabaseKey dbKey, long _clientid, long siteid,
         string ScheduleType, DateTime ScheduleThroughDate, string OnDemandgroup, bool PrintSanitationJob,
         bool PrintAttachments, string AssetGroup1Ids, string AssetGroup2Ids, string AssetGroup3Ids)
        {
            List<SanMasterBatchEntry> PrevMaintBatchEntryList = new List<SanMasterBatchEntry>();

            SanMasterBatchEntryTransactionsForSanitationMasterFromSanitationJob_V2 trans = new SanMasterBatchEntryTransactionsForSanitationMasterFromSanitationJob_V2
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                clientId = _clientid,
                siteid = siteid,
                ScheduleType = ScheduleType,
                ScheduleThroughDate = ScheduleThroughDate,
                OnDemandgroup = OnDemandgroup,
                PrintSanitationJob = PrintSanitationJob,
                PrintAttachments = PrintAttachments,
                AssetGroup1Ids = AssetGroup1Ids,
                AssetGroup2Ids = AssetGroup2Ids,
                AssetGroup3Ids = AssetGroup3Ids,
             


            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            if (trans.SanMasterBatchEntryList.Count > 0)
            {
                trans.SanMasterBatchEntryList.ForEach(x =>
                {
                    PrevMaintBatchEntryList.Add(new SanMasterBatchEntry()
                    {
                        SanMasterId = x.SanMasterId,
                        SanMasterBatchEntryId = x.SanMasterBatchEntryId,
                        DueDate = x.DueDate,
                        MasterDescription = x.MasterDescription,
                        ChargeType = x.ChargeType,
                        EquipmentClientLookupId = x.EquipmentClientLookupId,
                        EquipmentName = x.EquipmentName,
                        Shift = x.Shift,
                        SanMasterBatchHeaderId = x.SanMasterBatchHeaderId
                    }); 
                });

            }
            return PrevMaintBatchEntryList;
        }
        public List<SanMasterBatchEntry> SanMasterBatchEntryForSanitationJobFromSanitationMasterChunkSearch_V2(DatabaseKey dbKey, string TimeZone)
        {
            SanMasterBatchEntryTransactionsForSanitationJobFromSanitationMasterChunkSearch_V2 trans = new SanMasterBatchEntryTransactionsForSanitationJobFromSanitationMasterChunkSearch_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SanMasterBatchEntry = this.ToDateBaseObjectForSanitationJobFromSanitationMasterChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<SanMasterBatchEntry> SanMasterBatchEntrylist = new List<SanMasterBatchEntry>();
            foreach (b_SanMasterBatchEntry SanMasterBatchEntries in trans.SanMasterBatchEntry.listOfSanMasterBatchEntries)
            {
                SanMasterBatchEntry tmpSanMasterBatchEntry = new SanMasterBatchEntry();

                tmpSanMasterBatchEntry.UpdateFromDatabaseObjectForSanitationJobFromSanitationMasterChunkSearch(SanMasterBatchEntries, TimeZone);
                SanMasterBatchEntrylist.Add(tmpSanMasterBatchEntry);
            }
            return SanMasterBatchEntrylist;
        }
        public b_SanMasterBatchEntry ToDateBaseObjectForSanitationJobFromSanitationMasterChunkSearch()
        {
            b_SanMasterBatchEntry dbObj = new b_SanMasterBatchEntry();
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientId = this.ClientId;
            dbObj.TotalCount = this.TotalCount;
            dbObj.siteid = this.siteid;
            dbObj.ScheduleType = this.ScheduleType;
            dbObj.ScheduleThroughDate = this.ScheduleThroughDate;
            dbObj.OnDemandgroup = this.OnDemandgroup;
            dbObj.PrintSanitationJobs = this.PrintSanitationJobs;
            dbObj.PrintAttachments = this.PrintAttachments;
            dbObj.AssetGroup1Ids = this.AssetGroup1Ids;
            dbObj.AssetGroup2Ids = this.AssetGroup2Ids;
            dbObj.AssetGroup3Ids = this.AssetGroup3Ids;
            dbObj.SanMasterId = this.SanMasterId;
            dbObj.DueDate = this.DueDate;
            dbObj.EquipmentClientLookupId = this.EquipmentClientLookupId;
            dbObj.EquipmentName = this.EquipmentName;
            dbObj.MasterDescription = this.MasterDescription;
            dbObj.Shift = this.Shift;
            dbObj.SanMasterBatchHeaderId = this.SanMasterBatchHeaderId;
            return dbObj;
        }


        public void UpdateFromDatabaseObjectForSanitationJobFromSanitationMasterChunkSearch(b_SanMasterBatchEntry dbObj, string TimeZone)
        {
            this.TotalCount = dbObj.TotalCount;
            this.SanMasterBatchEntryId = dbObj.SanMasterBatchEntryId;
            this.SanMasterBatchHeaderId = dbObj.SanMasterBatchHeaderId;
            this.DueDate = dbObj.DueDate;
            this.MasterJob = dbObj.MasterJob;
            this.MasterDescription = dbObj.MasterDescription;
            this.ChargeType = dbObj.ChargeType;
            this.ChargeTo = dbObj.ChargeTo;
            this.SanMasterId = dbObj.SanMasterId;
            this.Frequency = dbObj.Frequency;
            this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            this.EquipmentName = dbObj.EquipmentName;
            this.Shift = dbObj.Shift;
            this.MasterDescription = dbObj.MasterDescription;
        }
        List<StoredProcValidationError> IStoredProcedureValidation.RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            throw new NotImplementedException();
        }
    }

}
