/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date        Task ID   Person             Description
 * =========== ======== =================== ===================================
 * 2011-Dec-09 20110019 Roger Lawton        Added ClientLookupId to search results
 * 2011-Dec-14 20110039 Roger Lawton        Added Lookuplist validation
 ******************************************************************************
 */

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
    public partial class PrevMaintBatchEntry : DataContractBase, IStoredProcedureValidation
    {
        public string MasterJob { get; set; }
        public string MasterDescription { get; set; }
        public string ChargeType { get; set; }
        public string ChargeTo { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public long Frequency { get; set; }

        public string EquipmentClientLookupId { get; set; }
        public string EquipmentName { get; set; }
        public string PrevMaintMasterClientLookupId { get; set; }
        public string PrevMaintMasterDescription { get; set; }

        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }

        public long siteid { get; set; }
        public string ScheduleType { get; set; }
        public DateTime ?  ScheduleThroughDate { get; set; }
        public string ScheduleThroughDateString { get; set; }
        public string OnDemandgroup { get; set; }
        public bool PrintWorkOrders { get; set; }
        public bool PrintAttachments { get; set; }
        public string AssetGroup1Ids { get; set; }
        public string AssetGroup2Ids { get; set; }
        public string AssetGroup3Ids { get; set; }
        public string PrevMaintSchedType { get; set; }
        public string PrevMaintMasterType { get; set; }
        
        public string AssetID { get; set; }
        public string AssetName { get; set; }
        public string MasterJobID { get; set; }
        public string PMDesc { get; set; }
        public DateTime? PrevBEDueDate { get; set; }
        public int TotalCount { get; set; }
        public DateTime? PMRequiredDate { get; set; }
        public int ChildCount { get; set; }
        public string AssignedTo_Name { get; set; }
        public string AssignedMultiple { get; set; }

        #region V2-1082
        public string Shift { get; set; }
        public bool? DownRequired { get; set; }
        #endregion
        public bool? PlanningRequired { get; set; } //V2-1161
        public List<PrevMaintBatchEntry> PrevMaintBatchEntry_ForWorkOrder(DatabaseKey dbKey,long _clientid, long siteid,
            string ScheduleType, DateTime ScheduleThroughDate, string OnDemandgroup, bool PrintWorkOrders,
            bool PrintAttachments)
        {
            List<PrevMaintBatchEntry> PrevMaintBatchEntryList = new List<PrevMaintBatchEntry>();

            PrevMaintBatchEntryTransactions trans = new PrevMaintBatchEntryTransactions 
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                clientId = _clientid,
                siteid = siteid,
                ScheduleType = ScheduleType,
                ScheduleThroughDate = ScheduleThroughDate,
                OnDemandgroup = OnDemandgroup,
                PrintWorkOrders = PrintWorkOrders,
                PrintAttachments = PrintAttachments
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            if (trans.PrevMaintBatchEntryList.Count > 0)
            {
                trans.PrevMaintBatchEntryList.ForEach(x =>
                {
                    PrevMaintBatchEntryList.Add(new PrevMaintBatchEntry() 
                    {
                        PrevMaintBatchEntryId = x.PrevMaintBatchEntryId,
                        PrevMaintBatchHeaderId = x.PrevMaintBatchHeaderId,
                        DueDate = x.DueDate,
                        MasterJob = x.MasterJob,
                        MasterDescription = x.MasterDescription,
                        ChargeType = x.ChargeType,
                        ChargeTo = x.ChargeTo,
                        WorkOrderId = x.WorkOrderId,
                        WorkOrderClientLookupId = x.WorkOrderClientLookupId,
                        Frequency = x.Frequency
                    });
                });
                
            }
            return PrevMaintBatchEntryList;
        }

        public List<PrevMaintBatchEntry> PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibrary(DatabaseKey dbKey, long _clientid, long siteid,
           string ScheduleType, DateTime ScheduleThroughDate, string OnDemandgroup, bool PrintWorkOrders,
           bool PrintAttachments)
        {
            List<PrevMaintBatchEntry> PrevMaintBatchEntryList = new List<PrevMaintBatchEntry>();

            PrevMaintBatchEntryTransactionsFromPrevMaintLibrary trans = new PrevMaintBatchEntryTransactionsFromPrevMaintLibrary
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                clientId = _clientid,
                siteid = siteid,
                ScheduleType = ScheduleType,
                ScheduleThroughDate = ScheduleThroughDate,
                OnDemandgroup = OnDemandgroup,
                PrintWorkOrders = PrintWorkOrders,
                PrintAttachments = PrintAttachments
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            if (trans.PrevMaintBatchEntryList.Count > 0)
            {
                trans.PrevMaintBatchEntryList.ForEach(x =>
                {
                    PrevMaintBatchEntryList.Add(new PrevMaintBatchEntry()
                    {
                        PrevMaintBatchEntryId = x.PrevMaintBatchEntryId,
                        PrevMaintBatchHeaderId = x.PrevMaintBatchHeaderId,
                        DueDate = x.DueDate,
                        MasterJob = x.MasterJob,
                        MasterDescription = x.MasterDescription,
                        ChargeType = x.ChargeType,
                        ChargeTo = x.ChargeTo,
                        WorkOrderId = x.WorkOrderId,
                        WorkOrderClientLookupId = x.WorkOrderClientLookupId,
                        Frequency = x.Frequency
                    });
                });

            }
            return PrevMaintBatchEntryList;
        }

        public List<PrevMaintBatchEntry> PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibrary_V2(DatabaseKey dbKey, long _clientid, long siteid,
         string ScheduleType, DateTime ScheduleThroughDate, string OnDemandgroup, bool PrintWorkOrders,
         bool PrintAttachments, string AssetGroup1Ids,string AssetGroup2Ids,string  AssetGroup3Ids,string PrevMaintSchedType,string PrevMaintMasterType)
        {
            List<PrevMaintBatchEntry> PrevMaintBatchEntryList = new List<PrevMaintBatchEntry>();

            PrevMaintBatchEntryTransactionsFromPrevMaintLibrary_V2 trans = new PrevMaintBatchEntryTransactionsFromPrevMaintLibrary_V2
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                clientId = _clientid,
                siteid = siteid,
                ScheduleType = ScheduleType,
                ScheduleThroughDate = ScheduleThroughDate,
                OnDemandgroup = OnDemandgroup,
                PrintWorkOrders = PrintWorkOrders,
                PrintAttachments = PrintAttachments,
                AssetGroup1Ids= AssetGroup1Ids,
                AssetGroup2Ids= AssetGroup2Ids,
                AssetGroup3Ids= AssetGroup3Ids,
                PrevMaintSchedType= PrevMaintSchedType,
                PrevMaintMasterType= PrevMaintMasterType


            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            if (trans.PrevMaintBatchEntryList.Count > 0)
            {
                trans.PrevMaintBatchEntryList.ForEach(x =>
                {
                    PrevMaintBatchEntryList.Add(new PrevMaintBatchEntry()
                    {
                        PrevMaintBatchEntryId = x.PrevMaintBatchEntryId,
                        PrevMaintBatchHeaderId = x.PrevMaintBatchHeaderId,
                        DueDate = x.DueDate,
                        MasterJob = x.MasterJob,
                        MasterDescription = x.MasterDescription,
                        ChargeType = x.ChargeType,
                        ChargeTo = x.ChargeTo,
                        WorkOrderId = x.WorkOrderId,
                        WorkOrderClientLookupId = x.WorkOrderClientLookupId,
                        Frequency = x.Frequency
                        //EquipmentClientLookupId = x.EquipmentClientLookupId,
                        //EquipmentName = x.EquipmentName,
                        //PrevMaintMasterClientLookupId = x.PrevMaintMasterClientLookupId,
                        //PrevMaintMasterDescription = x.PrevMaintMasterDescription
                    });
                });

            }
            return PrevMaintBatchEntryList;
        }

        public List<PrevMaintBatchEntry> PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibraryChunkSearch_V2(DatabaseKey dbKey, string TimeZone)
        {
            PrevMaintBatchEntryTransactionsFromPrevMaintLibraryForGenWOChunkSearch_V2 trans = new PrevMaintBatchEntryTransactionsFromPrevMaintLibraryForGenWOChunkSearch_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PrevMaintBatchEntry = this.ToDateBaseObjectForPrevMaintLibraryGenWOChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PrevMaintBatchEntry> PrevMaintBatchEntrylist = new List<PrevMaintBatchEntry>();
            foreach (b_PrevMaintBatchEntry PrevMaintBatchEntries in trans.PrevMaintBatchEntry.listOfPrevMaintBatchEntries)
            {
                PrevMaintBatchEntry tmpPrevMaintBatchEntry = new PrevMaintBatchEntry();

                tmpPrevMaintBatchEntry.UpdateFromDatabaseObjectForPrevMaintLibraryGenWOChunkSearch(PrevMaintBatchEntries, TimeZone);
                PrevMaintBatchEntrylist.Add(tmpPrevMaintBatchEntry);
            }
            return PrevMaintBatchEntrylist;
        }


        public b_PrevMaintBatchEntry ToDateBaseObjectForPrevMaintLibraryGenWOChunkSearch()
        {
            b_PrevMaintBatchEntry dbObj = new b_PrevMaintBatchEntry();           
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientId = this.ClientId;
            dbObj.siteid = this.siteid;
            dbObj.ScheduleType = this.ScheduleType;
            dbObj.ScheduleThroughDate = this.ScheduleThroughDate;
            dbObj.OnDemandgroup = this.OnDemandgroup;
            dbObj.PrintWorkOrders = this.PrintWorkOrders;
            dbObj.PrintAttachments = this.PrintAttachments;
            dbObj.AssetGroup1Ids = this.AssetGroup1Ids;
            dbObj.AssetGroup2Ids = this.AssetGroup2Ids;
            dbObj.AssetGroup3Ids = this.AssetGroup3Ids;
            dbObj.PrevMaintSchedType = this.PrevMaintSchedType;
            dbObj.PrevMaintMasterType = this.PrevMaintMasterType;
            dbObj.PrevBEDueDate = this.PrevBEDueDate;
            dbObj.EquipmentClientLookupId = this.EquipmentClientLookupId;
            dbObj.EquipmentName = this.EquipmentName;
            dbObj.PrevMaintMasterClientLookupId = this.PrevMaintMasterClientLookupId;
            dbObj.PrevMaintMasterDescription = this.PrevMaintMasterDescription;
            dbObj.PrevMaintBatchHeaderId = this.PrevMaintBatchHeaderId;
            dbObj.DownRequired = this.DownRequired;
            dbObj.Shift = this.Shift;
            return dbObj;
        }


        public void UpdateFromDatabaseObjectForPrevMaintLibraryGenWOChunkSearch(b_PrevMaintBatchEntry dbObj, string TimeZone)
        {          
            this.TotalCount = dbObj.TotalCount;
            this.PrevMaintBatchEntryId = dbObj.PrevMaintBatchEntryId;
            this.PrevMaintBatchHeaderId = dbObj.PrevMaintBatchHeaderId;
            this.DueDate = dbObj.DueDate;
            this.MasterJob = dbObj.MasterJob;
            this.MasterDescription = dbObj.MasterDescription;
            this.ChargeType = dbObj.ChargeType;
            this.ChargeTo = dbObj.ChargeTo;
            this.WorkOrderId = dbObj.WorkOrderId;
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.Frequency = dbObj.Frequency;
            this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            this.EquipmentName = dbObj.EquipmentName;
            this.PrevMaintMasterClientLookupId = dbObj.PrevMaintMasterClientLookupId;
            this.PrevMaintMasterDescription = dbObj.PrevMaintMasterDescription;
            this.PMRequiredDate = dbObj.PMRequiredDate;
            this.AssignedTo_Name = dbObj.AssignedTo_Name;
            this.AssignedMultiple = dbObj.AssignedMultiple;
            this.ChildCount = dbObj.ChildCount;
            //V2-1082
            this.DownRequired = dbObj.DownRequired;
            this.Shift = dbObj.Shift;
            //V2-1161
            this.PrevMaintSchedId = dbObj.PrevMaintSchedId;
            this.PlanningRequired = dbObj.PlanningRequired;


        }
        // public List<PrevMaintBatchEntry> PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibraryChunkSearch_V2(DatabaseKey dbKey, long _clientid, long siteid,string OrderbyColumn,string OrderBy, long OffSetVal,long NextRow,
        //string ScheduleType, DateTime ScheduleThroughDate, string OnDemandgroup, bool PrintWorkOrders,
        //bool PrintAttachments, long AssetGroup1Id, long AssetGroup2Id, long AssetGroup3Id, string PrevMaintSchedType, string PrevMaintMasterType)
        // {
        //     List<PrevMaintBatchEntry> PrevMaintBatchEntryList = new List<PrevMaintBatchEntry>();

        //     PrevMaintBatchEntryTransactionsFromPrevMaintLibraryChunkSearch_V2 trans = new PrevMaintBatchEntryTransactionsFromPrevMaintLibraryChunkSearch_V2
        //     {
        //         CallerUserInfoId = dbKey.User.UserInfoId,
        //         CallerUserName = dbKey.User.UserName,
        //         clientId = _clientid,
        //         siteid = siteid,
        //         OrderbyColumn = OrderbyColumn,
        //         OrderBy = OrderBy,
        //         OffSetVal = OffSetVal,
        //         NextRow = NextRow,
        //         ScheduleType = ScheduleType,
        //         ScheduleThroughDate = ScheduleThroughDate,
        //         OnDemandgroup = OnDemandgroup,
        //         PrintWorkOrders = PrintWorkOrders,
        //         PrintAttachments = PrintAttachments,
        //         AssetGroup1Id = AssetGroup1Id,
        //         AssetGroup2Id = AssetGroup2Id,
        //         AssetGroup3Id = AssetGroup3Id,
        //         PrevMaintSchedType = PrevMaintSchedType,
        //         PrevMaintMasterType = PrevMaintMasterType


        //     };

        //     trans.dbKey = dbKey.ToTransDbKey();
        //     trans.Execute();

        //     if (trans.PrevMaintBatchEntryList.Count > 0)
        //     {
        //         trans.PrevMaintBatchEntryList.ForEach(x =>
        //         {
        //             PrevMaintBatchEntryList.Add(new PrevMaintBatchEntry()
        //             {
        //                 PrevMaintBatchEntryId = x.PrevMaintBatchEntryId,
        //                 PrevMaintBatchHeaderId = x.PrevMaintBatchHeaderId,
        //                 DueDate = x.DueDate,
        //                 MasterJob = x.MasterJob,
        //                 MasterDescription = x.MasterDescription,
        //                 ChargeType = x.ChargeType,
        //                 ChargeTo = x.ChargeTo,
        //                 WorkOrderId = x.WorkOrderId,
        //                 WorkOrderClientLookupId = x.WorkOrderClientLookupId,
        //                 Frequency = x.Frequency,
        //                 EquipmentClientLookupId = x.EquipmentClientLookupId,
        //                 EquipmentName = x.EquipmentName,
        //                 PrevMaintMasterClientLookupId = x.PrevMaintMasterClientLookupId,
        //                 PrevMaintMasterDescription = x.PrevMaintMasterDescription
        //             });
        //         });

        //     }
        //     return PrevMaintBatchEntryList;
        // }
        public List<PrevMaintBatchEntry> PrevMaintBatchEntry_RetrieveByBatchHeaderId(DatabaseKey dbKey, long PrevMaintBatchHeaderId)
        {
            List<PrevMaintBatchEntry> PrevMaintBatchEntryList = new List<PrevMaintBatchEntry>();

            PrevMaintBatchEntry_RetrieveByBatchHeaderId trans = new PrevMaintBatchEntry_RetrieveByBatchHeaderId
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                PrevMaintBatchHeaderId = PrevMaintBatchHeaderId
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            if (trans.PrevMaintBatchEntryList.Count > 0)
            {
                trans.PrevMaintBatchEntryList.ForEach(x =>
                {
                    PrevMaintBatchEntryList.Add(new PrevMaintBatchEntry()
                    {
                        PrevMaintBatchEntryId = x.PrevMaintBatchEntryId,
                        PrevMaintBatchHeaderId = x.PrevMaintBatchHeaderId,
                        DueDate = x.DueDate,
                        MasterJob = x.MasterJob,
                        MasterDescription = x.MasterDescription,
                        ChargeType = x.ChargeType,
                        ChargeTo = x.ChargeTo,
                        WorkOrderId = x.WorkOrderId,
                        WorkOrderClientLookupId = x.WorkOrderClientLookupId,
                        Frequency = x.Frequency
                    });
                });

            }
            return PrevMaintBatchEntryList;
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            throw new NotImplementedException();
        }
    }
}
