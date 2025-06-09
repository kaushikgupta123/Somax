/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2014-Aug-08 SOM-279  Roger Lawton        Display Localized Status
*                                          Cleaned up and removed some comments
* 2014-Aug-23 SOM-400  Roger Lawton        Display Localized Status on edit (and other) page
* 2014-Aug-27 SOM-310  Roger Lawton        Added ScheduledStartDate, FailureCode and ActualFinishDate
*                                          to method 
*                                          UpdateFromDatabaseObjectForRetriveAllForSearch
* 2014-Sep-29 SOM-346  Roger Lawton        Added method UpdateFromDBOForList
*                                          And   method UpdateFromDBO
*                                          Modified method RetriveByEquipmentId
*                                          Modified method RetrieveByLocationId
* 2015-Jun-06 SOM-687  Roger Lawton        Grid to show First Name Last Name - NOT User Name
* 2015-Jun-23 SOM-706  Roger Lawton        Retrieved dates for grid not converted to user's time 
* 2016-Oct-05 SOM-1123 Roger Lawton        Added Denied Information to 
****************************************************************************************************
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
using Common.Extensions;
using Newtonsoft.Json;

namespace DataContracts
{
    public partial class WorkOrder : DataContractBase, IStoredProcedureValidation
    {
        #region Property
        // SOM-1123
        public string WorkOrderNo { get; set; }
        public string CompleteBy { get; set; }
        public string MaintOnDemandClientLookUpId { get; set; }
        public string DeniedBy_PersonnelId_ClientLookupId { get; set; }
        public string DeniedBy_PersonnelId_Name { get; set; }
        //som-987
        private bool m_validateClientLookupId;
        public bool CreateMode { get; set; }
        public string Requestor_PersonnelClientLookupId { get; set; }
        public string Creator_PersonnelClientLookupId { get; set; }
        public string ChargeToClientLookupId { get; set; }

        public string ApproveBy_PersonnelClientLookupId { get; set; }
        public string Planner_PersonnelClientLookupId { get; set; }
        public string Scheduler_PersonnelClientLookupId { get; set; }
        public string SignoffBy_PersonnelClientLookupId { get; set; }
        public string WorkAssigned_PersonnelClientLookupId { get; set; }
        public string CompleteBy_PersonnelClientLookupId { get; set; }
        public string CloseBy_PersonnelClientLookupId { get; set; }
        public string ReleaseBy_PersonnelClientLookupId { get; set; }
        public string WorkAssigned_Name { get; set; }
        public string Requestor_Name { get; set; }
        public string DateRange { get; set; }
        public string DateColumn { get; set; }
        public string DepartmentName { get; set; }
        public string Creator { get; set; }
        public string Assigned { get; set; }

        public string ValidateFor = string.Empty;
        public DateTime CreateDate { get; set; }
        public string Status_Display { get; set; }
        public Int64 ScheduleId { get; set; }     // RKL - SOM-279 - Display WO Status
        public Int64 PersonnelId { get; set; } //-----SOM-666 
        //For Workbench
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Created { get; set; }
        public string StatusDrop { get; set; }
        public Int64 UserInfoId { get; set; }
        public string ApproveFlag { get; set; }
        public string ScheduleFlag { get; set; }
        public string DeniedFlag { get; set; }
        public string Createby { get; set; }
        public string PersonnelClientLookupId { get; set; }
        public bool check { get; set; }
        public string CompleteBy_PersonnelName { get; set; }
        public string CreateBy_PersonnelName { get; set; }
        public string IncludeCopletedInCompletionWorkbench { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public DateTime? DateDown { get; set; } //iOSIOS01-74
        public decimal MinutesDown { get; set; } //iOSIOS01-74
        public Int64 DownTimeId { get; set; } //iOSIOS01-74

        public System.Data.DataTable WorkOrderNoList { get; set; }
        public string ChargeTo { get; set; }
        //V2-271
        public string CompletedDate { get; set; }
        public string AssignedFullName { get; set; }
        public string SearchText { get; set; }

        //V2-276
        public Int64 ObjectId { get; set; }
        public string IsActualOrEstimated { get; set; }
        public decimal PartCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal OtherCost { get; set; }
        public decimal TotalCost { get; set; }

        //V2 524
        public long WorkOrderScheduleId { get; set; }
        public string ScheduledDateStart { get; set; }

        public string ScheduledDateEnd { get; set; }

        public string RequireDate { get; set; }

        public string CalendarDateStart { get; set; }
        public string CalendarDateEnd { get; set; }

        public string PersonnelName { get; set; }
        #region for seacrh grid load
        public UtilityAdd utilityAdd { get; set; }
        public List<WorkOrder> listOfWO { get; set; }
        public List<Attachment> listOfAttachment { get; set; }

        public List<WorkOrderTask> listOfWOTask { get; set; }

        public List<Timecard> listOfTimecard { get; set; }
        public List<PartHistory> listOfPartHistory { get; set; }
        public List<OtherCosts> listOfOtherCosts { get; set; }

        public List<OtherCosts> listOfSummery { get; set; }

        public List<Instructions> listOfInstructions { get; set; }
        #region V2-944
        public List<WorkOrderUDF> listOfWorkOrderUDF { get; set; }
        public List<WorkOrderSchedule> listOfWorkOrderSchedule { get; set; }
        #endregion
        public int TotalCount { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string Scheduled { get; set; }
        public string ActualFinish { get; set; }
        public string Completed { get; set; }



        public string WONumber { get; set; }
        public string PONumber { get; set; }

        public string POType { get; set; }

        public string POStatus { get; set; }

        public int LineNumber { get; set; }

        public string LineStatus { get; set; }
        public string LineDesc { get; set; }
        public string VendorClientlookupId { get; set; }
        public string VendorName { get; set; }
        public string PersonnelList { get; set; }
        
        //1060
        public string WorkOrderSchedIdsList { get; set; }
        public string WorkOrderClientLookupIdsList { get; set; }
        public string Personnels { get; set; }
        public string PersonnelFull { get; set; }
        public bool IsDeleteFlag { get; set; }
        public long LoggedInUserPEID { get; set; }
        #endregion

        public string AssetGroup1ClientlookupId { get; set; }
        public string AssetGroup2ClientlookupId { get; set; }
        public string AssetGroup3ClientlookupId { get; set; }

        public long AssetGroup1Id { get; set; }
        public long AssetGroup2Id { get; set; }
        public long AssetGroup3Id { get; set; }

        //V2-546
        public string AssetGroup1AdvSearchId { get; set; }
        public string AssetGroup2AdvSearchId { get; set; }
        public string AssetGroup3AdvSearchId { get; set; }

        //v2-347
        public string StartActualFinishDateVw { get; set; }
        public string EndActualFinishDateVw { get; set; }
        public string StartCreateDate { get; set; }
        public string EndCreateDate { get; set; }
        public string StartScheduledDate { get; set; }
        public string EndScheduledDate { get; set; }
        public string StartActualFinishDate { get; set; }
        public string EndActualFinishDate { get; set; }

        //V2-364
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }


        //V2-524
        public string WorkOrderClientLookupId { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public decimal ScheduledHours { get; set; }
        public string PerNextValue { get; set; }
        public DateTime SDNextValue { get; set; }
        public decimal SumPersonnelHour { get; set; }
        public decimal SumScheduledateHour { get; set; }
        public decimal GrandTotalHour { get; set; }
        public long PerIDNextValue { get; set; }

        // V2 576
        public string Mode { get; set; }

        public string TableName { get; set; }
        public string AssetLocation { get; set; }
        public string AssetName { get; set; }
        public string ProjectClientLookupId { get; set; }
        public string AccountClientLookupId { get; set; }
        public string SourceIdClientLookupId { get; set; }
        //630
        public string ChargeToDescription { get; set; }

        // V2-634
        public bool TimecardTab { get; set; }
        public bool AutoAddTimecard { get; set; }
        public List<Timecard> TimecardList { get; set; }
        public WorkOrderUDF WorkOrderUDF { get; set; }
        //V2-634

        public string WorkOrderIDList { get; set; }
        public string SignoffBy_PersonnelClientLookupIdName { get; set; }
        public string EquipmentType { get; set; }
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public bool RemoveFromService { get; set; }

        public string AssetGroup1Description { get; set; }
        public string AssetGroup2Description { get; set; }
        public string AssetGroup3Description { get; set; }
        public string WOCompCriteriaTitle { get; set; }
        public string WOCompCriteria { get; set; }
        public bool WOCompCriteriaTab { get; set; }
        public bool? downRequired { get; set; }//V2-892
        public long CreatedWorkOrderId { get; set; }//V2-1051
        public string PlannerFullName { get; set; } //V2-1078
        public string ProjectIds { get; set; } //V2-1135
        #endregion

        public static List<WorkOrder> UpdateFromDatabaseObjectList(List<b_WorkOrder> dbObjs)
        {
            List<WorkOrder> result = new List<WorkOrder>();

            foreach (b_WorkOrder dbObj in dbObjs)
            {
                WorkOrder tmp = new WorkOrder();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }


        public b_WorkOrder ToDatabaseWorkbenchObject()
        {
            b_WorkOrder dbObj = new b_WorkOrder();
            dbObj.ModifyBy = this.ModifyBy;
            dbObj.ClientId = this.ClientId;
            dbObj.WorkOrderId = this.WorkOrderId;
            dbObj.ApproveFlag = this.ApproveFlag;
            dbObj.ScheduleFlag = this.ScheduleFlag;
            dbObj.DeniedFlag = this.DeniedFlag;
            dbObj.DeniedReason = this.DeniedReason;
            dbObj.ScheduledStartDate = this.ScheduledStartDate;
            dbObj.ScheduledDuration = this.ScheduledDuration;
            dbObj.DeniedComment = this.DeniedComment;
            dbObj.Createby = this.Createby;
            dbObj.Shift = this.Shift;
            return dbObj;
        }

        public static List<b_WorkOrder> ToDatabaseObjectList(List<WorkOrder> objs)
        {
            List<b_WorkOrder> result = new List<b_WorkOrder>();
            foreach (WorkOrder obj in objs)
            {
                result.Add(obj.ToDatabaseObject());
            }

            return result;
        }

        public void UpdateFromDatabaseObjectForWorkflow(b_WorkOrder dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ApproveBy_PersonnelClientLookupId = dbObj.ApproveBy_PersonnelClientLookupId;
            this.Planner_PersonnelClientLookupId = dbObj.Planner_PersonnelClientLookupId;
            this.Scheduler_PersonnelClientLookupId = dbObj.Scheduler_PersonnelClientLookupId;
            this.SignoffBy_PersonnelClientLookupId = dbObj.SignoffBy_PersonnelClientLookupId;
            this.Requestor_PersonnelClientLookupId = dbObj.Requestor_PersonnelClientLookupId;
            this.WorkAssigned_PersonnelClientLookupId = dbObj.WorkAssigned_PersonnelClientLookupId;
            this.Creator_PersonnelClientLookupId = dbObj.Creator_PersonnelClientLookupId;
            this.CompleteBy_PersonnelClientLookupId = dbObj.CompleteBy_PersonnelClientLookupId;
            this.CloseBy_PersonnelClientLookupId = dbObj.CloseBy_PersonnelClientLookupId;

            // Turn on auditing
            AuditEnabled = true;
        }

        public List<WorkOrder> RetrieveForExtraction(DatabaseKey dbKey)
        {
            WORetrieveForExtraction trans = new WORetrieveForExtraction()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.WorkOrder = this.ToDatabaseObject();
            trans.WorkOrder.WorkOrderNoList = this.WorkOrderNoList;

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return (UpdateFromDbObjectExtract(trans.WorkOrderList));
        }
        public static List<WorkOrder> UpdateFromDbObjectExtract(List<b_WorkOrder> dbObjs)
        {
            List<WorkOrder> result = new List<WorkOrder>();

            foreach (b_WorkOrder dbObj in dbObjs)
            {
                WorkOrder tmp = new WorkOrder();
                tmp.UpdateFromDatabaseObject(dbObj);

                {
                    tmp.WorkOrderNo = dbObj.WorkOrderNo;
                    tmp.CompleteBy = dbObj.CompleteBy;
                }
                result.Add(tmp);
            }
            return result;
        }
        //som-987
        public void CreateAndValidateWorkOrderSchedule(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateByWorkOrderByClientLookupId";
            Validate<WorkOrder>(dbKey);
            if (IsValid)
            {
                WorkOrederSchedule_Create trans = new WorkOrederSchedule_Create();
                trans.WorkOrder = this.ToDatabaseObject();
                trans.WorkOrder.ClientLookupId = this.ClientLookupId;
                trans.WorkOrder.ScheduledStartDate = this.ScheduledStartDate;
                trans.WorkOrder.ScheduledDuration = this.ScheduledDuration;
                trans.WorkOrder.PersonnelId = this.PersonnelId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.WorkOrder);
            }
        }

        public void AddScheduleRecord(DatabaseKey dbKey)
        {
            WorkOrderSchedule_AddScheduleRecord trans = new WorkOrderSchedule_AddScheduleRecord();
            trans.WorkOrder = this.ToDatabaseObject();
            trans.WorkOrder.WorkOrderId = this.WorkOrderId;
            trans.WorkOrder.ScheduledStartDate = this.ScheduledStartDate;
            trans.WorkOrder.ScheduledDuration = this.ScheduledDuration;
            trans.WorkOrder.PersonnelList = this.PersonnelList;
            trans.WorkOrder.IsDeleteFlag = this.IsDeleteFlag;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.WorkOrder);

        }

        public void ReassignPersonnelScheduleRecord(DatabaseKey dbKey)
        {
            WorkOrderSchedule_ReassignPersonnelRecord trans = new WorkOrderSchedule_ReassignPersonnelRecord();
            trans.WorkOrder = this.ToDatabaseObject();           
            trans.WorkOrder.PersonnelId = this.PersonnelId;
            trans.WorkOrder.WorkOrderSchedIdsList = this.WorkOrderSchedIdsList;
            
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.WorkOrder);
            this.WorkOrderClientLookupIdsList = trans.WorkOrder.WorkOrderClientLookupIdsList;

        }
        public void CreateWithValidation(DatabaseKey dbKey)
        {
            m_validateClientLookupId = true;
            Validate<WorkOrder>(dbKey);

            if (IsValid)
            {

                WorkOrder_Create trans = new WorkOrder_Create();
                trans.WorkOrder = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.WorkOrder);
            }
        }

        //end 
        /**/ //- SOM:928
        public List<WorkOrder> WorkOrderRetrieveWorkOrderListByMeterIdAndReadingDate(DatabaseKey dbKey)
        {
            WorkOrder_RetrieveWorkOrderListByMeterIdAndReadingDate trans = new WorkOrder_RetrieveWorkOrderListByMeterIdAndReadingDate()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.WorkOrder = this.ToDatabaseObject();
            trans.WorkOrder.CreateDate = this.CreateDate;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.WorkOrderList);
        }

        /**/

        // RKL - 2014-Jul-31 - Modified to handle other data 
        public void UpdateFromDatabaseObjectExtended(b_WorkOrder dbObj, string Timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.MaintOnDemandClientLookUpId = dbObj.MaintOnDemandClientLookUpId;
            this.ApproveBy_PersonnelClientLookupId = dbObj.ApproveBy_PersonnelClientLookupId;
            this.Assigned = dbObj.Assigned;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.CloseBy_PersonnelClientLookupId = dbObj.CloseBy_PersonnelClientLookupId;
            this.CompleteBy_PersonnelClientLookupId = dbObj.CompleteBy_PersonnelClientLookupId;
            this.CompleteBy_PersonnelName = dbObj.CompleteBy_PersonnelName;
            this.CreateBy_PersonnelName = dbObj.CreateBy_PersonnelName;
            //this.CreateDate = dbObj.CreateDate;
            this.Createby = dbObj.Createby;
            this.Creator_PersonnelClientLookupId = dbObj.Creator_PersonnelClientLookupId;
            this.Planner_PersonnelClientLookupId = dbObj.Planner_PersonnelClientLookupId;
            this.PlannerFullName = dbObj.PlannerFullName; // V2-1157
            this.ReleaseBy_PersonnelClientLookupId = dbObj.ReleaseBy_PersonnelClientLookupId;
            this.Requestor_Name = dbObj.Requestor_Name;
            this.Requestor_PersonnelClientLookupId = dbObj.Requestor_PersonnelClientLookupId;
            this.Scheduler_PersonnelClientLookupId = dbObj.Scheduler_PersonnelClientLookupId;
            this.SignoffBy_PersonnelClientLookupId = dbObj.SignoffBy_PersonnelClientLookupId;
            this.WorkAssigned_Name = dbObj.WorkAssigned_Name;
            this.Createby = dbObj.Createby;
            this.WorkAssigned_PersonnelClientLookupId = dbObj.WorkAssigned_PersonnelClientLookupId;
            this.ModifyBy = dbObj.ModifyBy;
            // SOM-1037 - Denied by 
            this.DeniedBy_PersonnelId_ClientLookupId = dbObj.DeniedBy_PersonnelId_ClientLookupId;
            this.DeniedBy_PersonnelId_Name = dbObj.DeniedBy_PersonnelId_Name;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            // SOM-1637 - Convert the create date to the user's time zone
            if (dbObj.ModifyDate != null && dbObj.ModifyDate != DateTime.MinValue)
            {
                this.ModifyDate = dbObj.ModifyDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.ModifyDate = dbObj.ModifyDate;
            }
            // SOM-1637 - Convert the create date to the user's time zone
            if (dbObj.ModifyDate != null && dbObj.ModifyDate != DateTime.MinValue)
            {
                this.ModifyDate = dbObj.ModifyDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.ModifyDate = dbObj.ModifyDate;
            }
            // SOM-1637 - Convert the complete date to the user's time zone
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CompleteDate = dbObj.CompleteDate;
            }
            this.AssetLocation = dbObj.AssetLocation;
            // SOM-300 Localization of Status            
            switch (this.Status)
            {
                case Common.Constants.WorkOrderStatusConstants.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.WorkOrderStatusConstants.AwaitingApproval:
                    this.Status_Display = "AwaitingApproval";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Canceled:
                    this.Status_Display = "Canceled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Denied:
                    this.Status_Display = "Denied";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.WorkRequest:
                    this.Status_Display = "WorkRequest";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Planning:
                    this.Status_Display = "Planning";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }
            this.Personnels = dbObj.Personnels;
            this.AssignedFullName = dbObj.AssignedFullName;
            this.ProjectClientLookupId = dbObj.ProjectClientLookupId;
            this.AccountClientLookupId = dbObj.AccountClientLookupId;
            this.SourceIdClientLookupId = dbObj.SourceIdClientLookupId;
            this.SignoffBy_PersonnelClientLookupIdName = dbObj.SignoffBy_PersonnelClientLookupIdName;
            //**V2-847
            this.AssetGroup1ClientlookupId = dbObj.AssetGroup1ClientlookupId;
            this.AssetGroup2ClientlookupId = dbObj.AssetGroup2ClientlookupId;
            //**
            //**V2-854
            this.AssetGroup1Description = dbObj.AssetGroup1Description;
            this.AssetGroup2Description = dbObj.AssetGroup2Description;
            //**
            //**V2-1012
            this.AssetLocation = dbObj.AssetLocation;
            this.ProjectClientLookupId = dbObj.ProjectClientLookupId;
            //**
        }

        public void UpdateFromDatabaseObjectPrintWorkOrderExtended(b_WorkOrder dbObj, string Timezone)
        { 
            this.WorkOrderId = dbObj.WorkOrderId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.CompleteBy_PersonnelId = dbObj.CompleteBy_PersonnelId;
            this.CompleteComments = dbObj.CompleteComments;
            // SOM-1637 - Convert the complete date to the user's time zone
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CompleteDate = dbObj.CompleteDate;
            }
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            this.Description = dbObj.Description;
            this.DownRequired = dbObj.DownRequired;
            this.RequiredDate = dbObj.RequiredDate;
            this.ScheduledDuration = dbObj.ScheduledDuration;
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            this.SignoffBy_PersonnelId = dbObj.SignoffBy_PersonnelId;
            this.Status = dbObj.Status;
            // SOM-300 Localization of Status            
            switch (this.Status)
            {
                case Common.Constants.WorkOrderStatusConstants.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.WorkOrderStatusConstants.AwaitingApproval:
                    this.Status_Display = "AwaitingApproval";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Canceled:
                    this.Status_Display = "Canceled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Denied:
                    this.Status_Display = "Denied";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.WorkRequest:
                    this.Status_Display = "WorkRequest";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Planning:
                    this.Status_Display = "Planning";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }
            this.Type = dbObj.Type;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.SignoffBy_PersonnelClientLookupId = dbObj.SignoffBy_PersonnelClientLookupId;
            this.SignoffBy_PersonnelClientLookupIdName = dbObj.SignoffBy_PersonnelClientLookupIdName;
            this.CompleteBy_PersonnelClientLookupId = dbObj.CompleteBy_PersonnelClientLookupId;
            this.Assigned = dbObj.Assigned;
            this.Createby = dbObj.Createby;
            this.CreateBy_PersonnelName = dbObj.CreateBy_PersonnelName;
            this.CompleteBy_PersonnelName = dbObj.CompleteBy_PersonnelName;
            this.AssignedFullName = dbObj.AssignedFullName;
            this.AssetLocation = dbObj.AssetLocation;

            this.EquipmentType = dbObj.EquipmentType;
            this.SerialNumber = dbObj.SerialNumber;
            this.Make = dbObj.Make;
            this.Model = dbObj.Model;
            this.AssetGroup1ClientlookupId = dbObj.AssetGroup1ClientlookupId;
            this.AssetGroup2ClientlookupId = dbObj.AssetGroup2ClientlookupId;
            this.AssetGroup3ClientlookupId = dbObj.AssetGroup3ClientlookupId;
            this.RemoveFromService = dbObj.RemoveFromService;
            this._ChargeToId = dbObj.ChargeToId;
            this.WOCompCriteriaTitle = dbObj.WOCompCriteriaTitle;
            this.WOCompCriteria = dbObj.WOCompCriteria;
            this.WOCompCriteriaTab = dbObj.WOCompCriteriaTab;
            this.Priority = dbObj.Priority;
            this.AccountClientLookupId = dbObj.AccountClientLookupId;
        }
        // RKL - 2014-Jul-31 - Modified to handle other data 
        public b_WorkOrder ToDatabaseObjectExtended()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();
            dbObj.MaintOnDemandClientLookUpId = this.MaintOnDemandClientLookUpId;
            dbObj.ApproveBy_PersonnelClientLookupId = this.ApproveBy_PersonnelClientLookupId;
            dbObj.Assigned = this.Assigned;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.CloseBy_PersonnelClientLookupId = this.CloseBy_PersonnelClientLookupId;
            dbObj.CompleteBy_PersonnelClientLookupId = this.CompleteBy_PersonnelClientLookupId;
            dbObj.CompleteBy_PersonnelName = this.CompleteBy_PersonnelName;
            dbObj.CreateBy_PersonnelName = this.CreateBy_PersonnelName;
            dbObj.CreateDate = this.CreateDate;
            dbObj.Createby = this.Createby;
            dbObj.Creator_PersonnelClientLookupId = this.Creator_PersonnelClientLookupId;
            dbObj.Planner_PersonnelClientLookupId = this.Planner_PersonnelClientLookupId;
            dbObj.ReleaseBy_PersonnelClientLookupId = this.ReleaseBy_PersonnelClientLookupId;
            dbObj.Requestor_Name = this.Requestor_Name;
            dbObj.Requestor_PersonnelClientLookupId = this.Requestor_PersonnelClientLookupId;
            dbObj.Scheduler_PersonnelClientLookupId = this.Scheduler_PersonnelClientLookupId;
            dbObj.SignoffBy_PersonnelClientLookupId = this.SignoffBy_PersonnelClientLookupId;
            dbObj.WorkAssigned_Name = this.WorkAssigned_Name;
            dbObj.WorkAssigned_PersonnelClientLookupId = this.WorkAssigned_PersonnelClientLookupId;
            dbObj.ChargeTo = this.ChargeTo;
            dbObj.ObjectId = this.ObjectId;
            dbObj.IsActualOrEstimated = this.IsActualOrEstimated;
            dbObj.TimecardTab = this.TimecardTab;
            dbObj.AutoAddTimecard = this.AutoAddTimecard;

            return dbObj;
        }


        public b_WorkOrder ToDatabaseObjectUpdatePartsonOrderExtended()
        {
            b_WorkOrder dbObj = new b_WorkOrder();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderId = this.WorkOrderId;
            dbObj.Mode = this.Mode;
            return dbObj;
        }
        public b_WorkOrder ToDatabaseObjectUpdateListPartsonOrderExtended()
        {
            b_WorkOrder dbObj = new b_WorkOrder();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ObjectId = this.ObjectId;
            dbObj.Mode = this.Mode;
            dbObj.TableName = this.TableName;
            return dbObj;
        }
        #region Transactions

        public List<WorkOrder> RetrieveClientLookupIdBySearchCriteria(DatabaseKey dbKey)
        {
            WorkOrder_RetrieveClientLookupIdBySearchCriteria trans = new WorkOrder_RetrieveClientLookupIdBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.WorkOrder = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> workOrderList = new List<WorkOrder>();
            foreach (b_WorkOrder workOrder in trans.WorkOrderList)
            {
                WorkOrder tmpWorkOrder = new WorkOrder()
                {
                    WorkOrderId = workOrder.WorkOrderId,
                    ClientLookupId = workOrder.ClientLookupId
                };
                workOrderList.Add(tmpWorkOrder);
            }

            return workOrderList;
        }

        //SOM-346
        public static List<WorkOrder> RetriveByEquipmentId(DatabaseKey dbKey, WorkOrder workorder, bool lActive)
        {
            WorkOrder_RetrieveByEquipmentId trans = new WorkOrder_RetrieveByEquipmentId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                bActive = lActive
            };

            trans.WorkOrder = workorder.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return WorkOrder.UpdateFromDBOForList(trans.WorkOrderList);
        }

        //SOM-346 - Added
        public static List<WorkOrder> UpdateFromDBOForList(List<b_WorkOrder> dbObjs)
        {
            List<WorkOrder> result = new List<WorkOrder>();

            foreach (b_WorkOrder dbObj in dbObjs)
            {
                WorkOrder tmp = new WorkOrder();
                tmp.UpdateFromDBO(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        //SOM-346 - Added
        public void UpdateFromDBO(b_WorkOrder dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.WorkOrderId = dbObj.WorkOrderId;
            this.SiteId = dbObj.SiteId;
            this.AreaId = dbObj.AreaId;
            this.DepartmentId = dbObj.DepartmentId;
            this.StoreroomId = dbObj.StoreroomId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Description = dbObj.Description;
            this.Status = dbObj.Status;
            this.Type = dbObj.Type;
            this.WorkAssigned_PersonnelClientLookupId = dbObj.WorkAssigned_PersonnelClientLookupId;
            this.CreateDate = dbObj.CreateDate;
            switch (this.Status)
            {
                case Common.Constants.WorkOrderStatusConstants.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.WorkOrderStatusConstants.AwaitingApproval:
                    this.Status_Display = "AwaitingApproval";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Canceled:
                    this.Status_Display = "Canceled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Denied:
                    this.Status_Display = "Denied";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.WorkRequest:
                    this.Status_Display = "WorkRequest";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }
        }

        public static List<WorkOrder> RetriveByEquipmentBIMGuidId(DatabaseKey dbKey, WorkOrder workorder, Guid BIMGuid)
        {
            WorkOrder_RetrieveByEquipmentBIMGuidId trans = new WorkOrder_RetrieveByEquipmentBIMGuidId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                ClientId = dbKey.Client.ClientId,
                BIMGuid = BIMGuid
            };

            trans.WorkOrder = workorder.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return WorkOrder.UpdateFromDatabaseObjectList(trans.WorkOrderList);
        }

        //SOM-346 - Modified
        public static List<WorkOrder> RetrieveByLocationId(DatabaseKey dbKey, WorkOrder workorder, bool lActive)
        {
            WorkOrder_RetrieveByLocationId trans = new WorkOrder_RetrieveByLocationId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                bActive = lActive
            };

            trans.WorkOrder = workorder.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return WorkOrder.UpdateFromDBOForList(trans.WorkOrderList);
        }

        // SOM-1384 
        public void CreateForSensorReading(DatabaseKey dbKey, string Timezone)
        {
            WorkOder_CreateForSensorReading trans = new WorkOder_CreateForSensorReading()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObjectExtended(trans.WorkOrder, Timezone);
        }
        public void CreateFromOnDemandMaster(DatabaseKey dbKey, string Timezone)
        {
            Validate<WorkOrder>(dbKey);

            if (IsValid)
            {
                WorkOrder_CreateFromOnDemandMaster trans = new WorkOrder_CreateFromOnDemandMaster()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                // RKL - 2014-Jul-31 - Use ToDatabaseObjectExtended 
                trans.WorkOrder = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObjectExtended(trans.WorkOrder, Timezone);
            }
        }

        public void CreateFromOnDemandMaster_V2(DatabaseKey dbKey, string Timezone)
        {
            Validate<WorkOrder>(dbKey);

            if (IsValid)
            {
                WorkOrder_CreateFromOnDemandMaster_V2 trans = new WorkOrder_CreateFromOnDemandMaster_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                // RKL - 2014-Jul-31 - Use ToDatabaseObjectExtended 
                trans.WorkOrder = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObjectExtended(trans.WorkOrder, Timezone);
            }
        }
        public void CreateByPKForeignKeys(DatabaseKey dbKey, string Timezone)
        {
            Validate<WorkOrder>(dbKey);

            if (IsValid)
            {
                WorkOrder_CreateByForeignKeys trans = new WorkOrder_CreateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                // RKL - 2014-Jul-31 - Use ToDatabaseObjectExtended 
                trans.WorkOrder = this.ToDatabaseObjectExtended();

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObjectExtended(trans.WorkOrder, Timezone);
            }
        }

        public void CreateByPKForeignKeysForSanitation(DatabaseKey dbKey, string Timezone)
        {
            //Validate<WorkOrder>(dbKey);

            if (true)
            {
                WorkOrder_CreateByForeignKeysForSanitation trans = new WorkOrder_CreateByForeignKeysForSanitation()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                // RKL - 2014-Jul-31 - Use ToDatabaseObjectExtended 
                trans.WorkOrder = this.ToDatabaseObjectExtended();
                trans.WorkOrder.WorkOrderId = 0;// this.WorkOrderId;
                                                // All of the following handled by todatabaseobjectextended

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObjectExtended(trans.WorkOrder, Timezone);
            }
        }

        // Added method - RKL - 2014-Jul-31
        public void CompleteWorkOrder(DatabaseKey dbKey, string Timezone)
        {
            Validate<WorkOrder>(dbKey);

            if (IsValid)
            {
                WorkOrder_CompleteWorkOrder trans = new WorkOrder_CompleteWorkOrder()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.WorkOrder = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObjectExtended(trans.WorkOrder, Timezone);
            }
        }
        public void CompleteWorkOrderWizard(DatabaseKey dbKey, string Timezone)
        {
            Validate<WorkOrder>(dbKey);

            if (IsValid)
            {
                WorkOrder_CompleteWorkOrderFromWizard trans = new WorkOrder_CompleteWorkOrderFromWizard()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.WorkOrder = this.ToDatabaseObjectExtended();
                trans.TimecardList = ToDatabaseObjectTimecard();
                trans.WorkOrderUDF = ToDatabaseObjectWorkOrderUDF();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObjectExtended(trans.WorkOrder, Timezone);
            }
        }
        public b_WorkOrderUDF ToDatabaseObjectWorkOrderUDF()
        {
            b_WorkOrderUDF dbObj = new b_WorkOrderUDF();
            dbObj.ClientId = this.ClientId;
            dbObj.WorkOrderUDFId = this.WorkOrderUDF.WorkOrderUDFId;
            dbObj.Text1 = this.WorkOrderUDF.Text1;
            dbObj.Text2 = this.WorkOrderUDF.Text2;
            dbObj.Text3 = this.WorkOrderUDF.Text3;
            dbObj.Text4 = this.WorkOrderUDF.Text4;
            dbObj.Date1 = this.WorkOrderUDF.Date1;
            dbObj.Date2 = this.WorkOrderUDF.Date2;
            dbObj.Date3 = this.WorkOrderUDF.Date3;
            dbObj.Date4 = this.WorkOrderUDF.Date4;
            dbObj.Bit1 = this.WorkOrderUDF.Bit1;
            dbObj.Bit2 = this.WorkOrderUDF.Bit2;
            dbObj.Bit3 = this.WorkOrderUDF.Bit3;
            dbObj.Bit4 = this.WorkOrderUDF.Bit4;
            dbObj.Numeric1 = this.WorkOrderUDF.Numeric1;
            dbObj.Numeric2 = this.WorkOrderUDF.Numeric2;
            dbObj.Numeric3 = this.WorkOrderUDF.Numeric3;
            dbObj.Numeric4 = this.WorkOrderUDF.Numeric4;
            dbObj.Select1 = this.WorkOrderUDF.Select1;
            dbObj.Select2 = this.WorkOrderUDF.Select2;
            dbObj.Select3 = this.WorkOrderUDF.Select3;
            dbObj.Select4 = this.WorkOrderUDF.Select4;
            dbObj.WorkOrderId = this.WorkOrderUDF.WorkOrderId;
            return dbObj;
        }
        public List<b_Timecard> ToDatabaseObjectTimecard()
        {
            List<b_Timecard> timecards = new List<b_Timecard>();
            foreach (var obj in TimecardList)
            {
                b_Timecard dbObj = new b_Timecard();
                dbObj.ClientId = obj.ClientId;
                dbObj.SiteId = obj.SiteId;
                dbObj.ChargeType_Primary = obj.ChargeType_Primary;
                dbObj.ChargeToId_Primary = obj.ChargeToId_Primary;
                dbObj.PersonnelId = obj.PersonnelId;
                dbObj.StartDate = obj.StartDate;
                dbObj.Hours = obj.Hours;
                timecards.Add(dbObj);
            }
            return timecards;
        }
        public void UpdateByPKForeignKeys(DatabaseKey dbKey, string Timezone)
        {
            Validate<WorkOrder>(dbKey);

            if (IsValid)
            {
                WorkOrder_UpdateByForeignKeys trans = new WorkOrder_UpdateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.WorkOrder = this.ToDatabaseObjectExtended();

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObjectExtended(trans.WorkOrder, Timezone);
            }
        }


        public void UpdateByWorkOrderPlanner(DatabaseKey dbKey, string Timezone)
        {
            Validate<WorkOrder>(dbKey);

            if (IsValid)
            {
                WorkOrder_UpdatePlanner trans = new WorkOrder_UpdatePlanner()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.WorkOrder = this.ToDatabaseObjectExtended();

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObjectExtended(trans.WorkOrder, Timezone);
            }
        }


        public void UpdateByWorkbench(DatabaseKey dbKey)
        {
            WorkOrder_UpdateByWorkbench trans = new WorkOrder_UpdateByWorkbench()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.WorkOrder = this.ToDatabaseWorkbenchObject();

            trans.WorkOrder.ApproveBy_PersonnelId = this.ApproveBy_PersonnelId;
            trans.WorkOrder.WorkAssigned_PersonnelId = this.WorkAssigned_PersonnelId;
            trans.WorkOrder.Createby = this.Createby;
            trans.WorkOrder.ModifyBy = this.ModifyBy;
            trans.WorkOrder.ClientId = this.ClientId;
            trans.WorkOrder.WorkOrderId = this.WorkOrderId;
            trans.WorkOrder.ApproveFlag = this.ApproveFlag;
            trans.WorkOrder.ScheduleFlag = this.ScheduleFlag;
            trans.WorkOrder.DeniedFlag = this.DeniedFlag;
            trans.WorkOrder.DeniedReason = this.DeniedReason;
            trans.WorkOrder.ScheduledStartDate = this.ScheduledStartDate;
            trans.WorkOrder.ScheduledDuration = this.ScheduledDuration;
            trans.WorkOrder.DeniedComment = this.DeniedComment;

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.WorkOrder);
        }

        //SOM-1479
        public void CancelWorkOrder(DatabaseKey dbKey)
        {
            WorkOrder_CancelWorkOrder trans = new WorkOrder_CancelWorkOrder()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.WorkOrder = this.ToDatabaseWorkbenchObject();
            trans.WorkOrder.ClientId = this.ClientId;
            trans.WorkOrder.SiteId = this.SiteId;
            trans.WorkOrder.WorkOrderId = this.WorkOrderId;
            trans.WorkOrder.CompleteBy_PersonnelId = this.CompleteBy_PersonnelId;
            trans.WorkOrder.CompleteComments = this.CompleteComments;
            trans.WorkOrder.CompleteDate = this.CompleteDate;
            trans.WorkOrder.ActualFinishDate = this.ActualFinishDate;
            trans.WorkOrder.CancelReason = this.CancelReason;
            trans.WorkOrder.Status = this.Status;

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.WorkOrder);
        }



        public void UpdateTasksByWorkOrderId(DatabaseKey dbKey)
        {

            UpdateTasksByWorkOrderId trans = new UpdateTasksByWorkOrderId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.WorkOrder = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

        }


        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "ValidateByEquipmentLocation")
            {
                ValidateEquipmentLocationTransaction trans = new ValidateEquipmentLocationTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.WorkOrder = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            else if (ValidateFor == "ValidateByWorkOrderByClientLookupId")
            {
                WorkOrder_ValidateClientLookupId trans = new WorkOrder_ValidateClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                    CreateMode = this.CreateMode
                };
                trans.WorkOrder = this.ToDatabaseObjectExtended();

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            else
            {
                WorkOrder_ValidateByClientLookupId trans = new WorkOrder_ValidateByClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                    CreateMode = this.CreateMode
                };
                trans.WorkOrder = this.ToDatabaseObjectExtended();

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }

            return errors;
        }

        public void RetrieveByPKForeignKeys(DatabaseKey dbKey, string Timezone)
        {

            WorkOrder_RetrieveByForeignKeys trans = new WorkOrder_RetrieveByForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.WorkOrder = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // RKL - 2014-Jul-31 - Use UpdateFromDatabaseObjectExtended 
            UpdateFromDatabaseObjectExtended(trans.WorkOrder, Timezone);
            /*
            UpdateFromDatabaseObject(trans.WorkOrder);
            this.Requestor_PersonnelClientLookupId = trans.WorkOrder.Requestor_PersonnelClientLookupId;
            this.Creator_PersonnelClientLookupId = trans.WorkOrder.Creator_PersonnelClientLookupId;
            this.ChargeToClientLookupId = trans.WorkOrder.ChargeToClientLookupId;
            this.ApproveBy_PersonnelClientLookupId = trans.WorkOrder.ApproveBy_PersonnelClientLookupId;
            this.Planner_PersonnelClientLookupId = trans.WorkOrder.Planner_PersonnelClientLookupId;
            this.Scheduler_PersonnelClientLookupId = trans.WorkOrder.Scheduler_PersonnelClientLookupId;
            this.SignoffBy_PersonnelClientLookupId = trans.WorkOrder.SignoffBy_PersonnelClientLookupId;
            this.WorkAssigned_PersonnelClientLookupId = trans.WorkOrder.WorkAssigned_PersonnelClientLookupId;
            this.CompleteBy_PersonnelClientLookupId = trans.WorkOrder.CompleteBy_PersonnelClientLookupId;
            this.CloseBy_PersonnelClientLookupId = trans.WorkOrder.CloseBy_PersonnelClientLookupId;
            this.ReleaseBy_PersonnelClientLookupId = trans.WorkOrder.ReleaseBy_PersonnelClientLookupId;
            this.CreateDate = trans.WorkOrder.CreateDate;
            this.CreateBy_PersonnelName = trans.WorkOrder.CreateBy_PersonnelName;
            this.CompleteBy_PersonnelName = trans.WorkOrder.CompleteBy_PersonnelName;
            this.Assigned = trans.WorkOrder.Assigned;
            this.Createby = trans.WorkOrder.Createby;
            */
        }

        // Following Method not used anywhere
        // RKL - Comment out
        /*
        public void RetrieveInitialSearchConfigurationData(DatabaseKey dbKey, ClientWebSite local)
        {

            WorkOrder_RetrieveInitialSearchConfigurationData trans = new WorkOrder_RetrieveInitialSearchConfigurationData()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            SearchCriteria = trans.SearchCriteria;

            // Add the Dates
            Load_DateSelection(local);

            // Add the 'Columns'
            Load_ColumnSelection(local);
        }
        */
        public WorkOrder RetrievePersonnelInitial(DatabaseKey dbKey)
        {
            WorkOrder_RetrievePersonnelInitial trans = new WorkOrder_RetrievePersonnelInitial()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            WorkOrder tmpworkorder = new WorkOrder();
            tmpworkorder.UpdateFromDatabaseObjectRetrievePersonnelInitial(trans.WorkOrder);

            return tmpworkorder;
        }
        public void UpdateFromDatabaseObjectRetrievePersonnelInitial(b_WorkOrder dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Personnels = dbObj.Personnels;
            this.PersonnelFull = dbObj.PersonnelFull;
        }

        // SOM-706 - Added timezone parameter
        public List<WorkOrder> RetrieveAllForSearch(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_RetrieveAllForSearch trans = new WorkOrder_RetrieveAllForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> workorderlist = new List<WorkOrder>();

            foreach (b_WorkOrder workorder in trans.WorkOrderList)
            {
                WorkOrder tmpworkorder = new WorkOrder();
                // SOM-706 - Added timezone parameter
                tmpworkorder.UpdateFromDatabaseObjectForRetriveAllForSearch(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }
            return workorderlist;
        }
        //public List<WorkOrder> RetrieveAllWorkOrderId(DatabaseKey dbKey, string TimeZone)
        //{
        //    WorkOrder_RetrieveAllWorkOrderId trans = new WorkOrder_RetrieveAllWorkOrderId()
        //    {
        //        CallerUserInfoId = dbKey.User.UserInfoId,
        //        CallerUserName = dbKey.UserName,
        //    };
        //    trans.WorkOrder = this.ToDateBaseObjectForRetriveAllForSearch();
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.Execute();

        //    List<WorkOrder> workorderlist = new List<WorkOrder>();

        //    foreach (b_WorkOrder workorder in trans.WorkOrderList)
        //    {
        //        WorkOrder tmpworkorder = new WorkOrder();
        //        // SOM-706 - Added timezone parameter
        //        tmpworkorder.UpdateFromDatabaseObjectForRetriveAllForSearch(workorder, TimeZone);
        //        workorderlist.Add(tmpworkorder);
        //    }
        //    return workorderlist;
        //}

        // SOM-706 - Added timezone parameter
        public List<WorkOrder> RetrieveAllForSearchNew(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_RetrieveAllForSearch trans = new WorkOrder_RetrieveAllForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> workorderlist = new List<WorkOrder>();
            // RKL - 2014-Aug-08
            // Moved this to UpdateFromDatabaseObjectForRetriveAllForSearch
            foreach (b_WorkOrder workorder in trans.WorkOrderList)
            {
                WorkOrder tmpworkorder = new WorkOrder();
                /*
                  tmpworkorder.DepartmentName = workorder.DepartmentName;
                  tmpworkorder.Creator = workorder.Creator;
                  tmpworkorder.Assigned = workorder.Assigned;
                */
                // SOM-706 - Added timezone parameter
                tmpworkorder.UpdateFromDatabaseObjectForRetriveAllForSearch(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }
            return workorderlist;
        }

        // SOM-706 - Added timezone parameter
        public List<WorkOrder> RetrieveWorkBenchForSearchNew(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_RetrieveAllWorkbenchSearch trans = new WorkOrder_RetrieveAllWorkbenchSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetriveWorkbenchForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> workorderlist = new List<WorkOrder>();

            foreach (b_WorkOrder workorder in trans.WorkOrderList)
            {
                WorkOrder tmpworkorder = new WorkOrder();
                // RKL - Moved to the .UpdateFromDatabaseObjectForRetriveAllForSearch method
                //tmpworkorder.DepartmentName = workorder.DepartmentName;
                //tmpworkorder.Creator = workorder.Creator;
                //tmpworkorder.Assigned = workorder.Assigned;
                //tmpworkorder.Createby = workorder.Createby;
                // SOM-706 - Added timezone parameter
                tmpworkorder.UpdateFromDatabaseObjectForRetriveAllForSearch(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }
            return workorderlist;
        }

        public List<WorkOrder> RetrieveWorkBenchCompletionForFPSearch(DatabaseKey dbKey)
        {
            WorkOrder_RetrieveAllCompletionWorkbenchForFPSearch trans = new WorkOrder_RetrieveAllCompletionWorkbenchForFPSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetriveWorkbenchCompletion();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> workorderlist = new List<WorkOrder>();

            foreach (b_WorkOrder workorder in trans.WorkOrderList)
            {
                WorkOrder tmpworkorder = new WorkOrder();

                tmpworkorder.UpdateFromDatabaseObjectWorkBenchCompletionRetriveAll(workorder);
                workorderlist.Add(tmpworkorder);
            }
            return workorderlist;
        }


        public void CreateEmergencyWOByPKForeignKeys(DatabaseKey dbKey, string Timezone)
        {
            Validate<WorkOrder>(dbKey);

            if (IsValid)
            {
                WorkOrder_CreateEmergencyByForeignKeys trans = new WorkOrder_CreateEmergencyByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.WorkOrder = this.ToDatabaseObjectExtended();

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObjectExtended(trans.WorkOrder, Timezone);
            }
        }
        //usp_WorkOrder_WRDashboardRetrieveAllForSearch_V2 and usp_WorkOrder_WRDashboardRetrieveAllForPrint_V2
        public b_WorkOrder ToDateBaseObjectForRetriveAllForSearch()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();

            dbObj.DateRange = this.DateRange;
            dbObj.DateColumn = this.DateColumn;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.Requestor_PersonnelClientLookupId = this.Requestor_PersonnelClientLookupId;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.Creator = this.Creator;
            dbObj.Created = this.Created;
            dbObj.Assigned = this.Assigned;
            dbObj.Scheduled = this.Scheduled;
            dbObj.FailureCode = this.FailureCode;
            dbObj.ActualFinish = this.ActualFinish;
            dbObj.Completed = this.Completed;
            dbObj.SearchText = this.SearchText;
            dbObj.PersonnelList = this.PersonnelList;
            dbObj.LoggedInUserPEID = this.LoggedInUserPEID;
            dbObj.AssetGroup1ClientlookupId = this.AssetGroup1ClientlookupId;
            dbObj.AssetGroup2ClientlookupId = this.AssetGroup2ClientlookupId;
            dbObj.AssetGroup3ClientlookupId = this.AssetGroup3ClientlookupId;
            return dbObj;
        }


        public b_WorkOrder ToDateBaseObjectForRetrieveAllByWorkOrdeV2Print()
        {
            b_WorkOrder dbObj = new b_WorkOrder();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderIDList = this.WorkOrderIDList;

            return dbObj;
        }
        //For sp usp_WorkOrder_RetrieveChunkForSearch_V2-V2-347 
        public b_WorkOrder ToDateBaseObjectForRetriveChunkSearch()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();

            dbObj.DateRange = this.DateRange;
            dbObj.DateColumn = this.DateColumn;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.Requestor_PersonnelClientLookupId = this.Requestor_PersonnelClientLookupId;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.AssetLocation = this.AssetLocation;
            dbObj.Creator = this.Creator;
            dbObj.Assigned = this.Assigned;
            dbObj.FailureCode = this.FailureCode;
            dbObj.Completed = this.Completed;
            dbObj.SearchText = this.SearchText;
            dbObj.PersonnelList = this.PersonnelList;
            dbObj.LoggedInUserPEID = this.LoggedInUserPEID;
            dbObj.AssetGroup1ClientlookupId = this.AssetGroup1ClientlookupId;
            dbObj.AssetGroup2ClientlookupId = this.AssetGroup2ClientlookupId;
            dbObj.AssetGroup3ClientlookupId = this.AssetGroup3ClientlookupId;
            //<!--(Added on 25/06/2020)-->
            dbObj.AssetGroup1Id = this.AssetGroup1Id;
            dbObj.AssetGroup2Id = this.AssetGroup2Id;
            dbObj.AssetGroup3Id = this.AssetGroup3Id;
            //<!--(Added on 25/06/2020)-->
            dbObj.StartActualFinishDateVw = this.StartActualFinishDateVw;
            dbObj.EndActualFinishDateVw = this.EndActualFinishDateVw;
            dbObj.StartCreateDate = this.StartCreateDate;
            dbObj.EndCreateDate = this.EndCreateDate;
            dbObj.StartScheduledDate = this.StartScheduledDate;
            dbObj.EndScheduledDate = this.EndScheduledDate;
            dbObj.StartActualFinishDate = this.StartActualFinishDate;
            dbObj.EndActualFinishDate = this.EndActualFinishDate;
            dbObj.CreateStartDateVw = this.CreateStartDateVw;
            dbObj.CreateEndDateVw = this.CreateEndDateVw;

            dbObj.AssetGroup1AdvSearchId = this.AssetGroup1AdvSearchId;
            dbObj.AssetGroup2AdvSearchId = this.AssetGroup2AdvSearchId;
            dbObj.AssetGroup3AdvSearchId = this.AssetGroup3AdvSearchId;
            dbObj.downRequired = this.downRequired;//V2-892
            //V2-984
            dbObj.Assigned=this.Assigned;
            dbObj.RequireDate= this.RequireDate;
            dbObj.PlannerFullName = this.PlannerFullName; //V2-1078
            dbObj.ProjectIds = this.ProjectIds; //V2-1078
            return dbObj;
        }
        public b_WorkOrder ToDateBaseObjectForRetriveWorkbenchForSearch()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();

            dbObj.DateRange = this.DateRange;
            dbObj.DateColumn = this.DateColumn;
            dbObj.Requestor_PersonnelClientLookupId = this.Requestor_PersonnelClientLookupId;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.Created = this.Created;
            dbObj.StatusDrop = this.StatusDrop;
            dbObj.UserInfoId = this.UserInfoId;
            return dbObj;
        }
        public b_WorkOrder ToDateBaseObjectForRetriveWorkbenchCompletion()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();

            dbObj.DateRange = this.DateRange;
            dbObj.DateColumn = this.DateColumn;
            dbObj.PersonnelClientLookupId = this.PersonnelClientLookupId;
            dbObj.Createby = this.Createby;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.Created = this.Created;
            dbObj.StatusDrop = this.StatusDrop;
            dbObj.UserInfoId = this.UserInfoId;
            return dbObj;
        }
        // SOM-706 - Added timezone parameter
        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_WorkOrder dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            // Moved from RetrieveAllForSearchNew - Begin
            this.DepartmentName = dbObj.DepartmentName;
            this.Creator = dbObj.Creator;
            this.Assigned = dbObj.Assigned;
            // Moved from RetrieveAllForSearchNew - End
            this.DateRange = dbObj.DateRange;
            this.DateColumn = dbObj.DateColumn;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.Createby = dbObj.Createby;
            this.PersonnelClientLookupId = dbObj.PersonnelClientLookupId;
            this.Requestor_PersonnelClientLookupId = dbObj.Requestor_PersonnelClientLookupId;
            this.CreateDate = dbObj.CreateDate;
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            this.FailureCode = dbObj.FailureCode;
            this.ActualFinishDate = dbObj.ActualFinishDate;
            // SOM-706 - Convert the create date to the user's time zone
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            //SOM-1533
            //SOM-1581 - Required date should not be converted
            /*
            if (dbObj.RequiredDate != null && dbObj.RequiredDate != DateTime.MinValue)
            {
                this.RequiredDate = dbObj.RequiredDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.RequiredDate = dbObj.RequiredDate;
            }
            */
            // SOM-1023
            this.Priority = dbObj.Priority;
            this.DeniedBy_PersonnelId_ClientLookupId = dbObj.DeniedBy_PersonnelId_ClientLookupId;
            this.DeniedBy_PersonnelId_Name = dbObj.DeniedBy_PersonnelId_Name;
            this.WorkAssigned_PersonnelClientLookupId = dbObj.WorkAssigned_PersonnelClientLookupId; //Code optimization based on Error log
            // SOM-706 - Convert the create date to the user's time zone
            // SOM-279 Localization of Status            
            switch (this.Status)
            {
                case Common.Constants.WorkOrderStatusConstants.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.WorkOrderStatusConstants.AwaitingApproval:
                    this.Status_Display = "AwaitingApproval";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Canceled:
                    this.Status_Display = "Canceled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Denied:
                    this.Status_Display = "Denied";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.WorkRequest:
                    this.Status_Display = "WorkRequest";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }

        }
        public void UpdateFromDatabaseObjectWorkBenchCompletionRetriveAll(b_WorkOrder dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            // SOM-687 - Begin
            this.WorkAssigned_Name = dbObj.WorkAssigned_Name;
            this.CreateBy_PersonnelName = dbObj.CreateBy_PersonnelName;
            // SOM-687 - End
            this.Createby = dbObj.Createby;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.PersonnelClientLookupId = dbObj.PersonnelClientLookupId;
            this.CreateDate = dbObj.CreateDate;

        }
        public long WorkOrder_CreateForPrevMaintenance
            (DatabaseKey dbKey
            , long PrevMaintBatchId
            , string ClientLookupId,
            long Requestor_PersonnelId)
        {
            WorkOrder_CreateForPrevMaintenance trans = new WorkOrder_CreateForPrevMaintenance
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.WorkOrder = new b_WorkOrder();
            trans.WorkOrder.PrevMaintBatchId = PrevMaintBatchId;
            trans.WorkOrder.ClientLookupId = ClientLookupId;
            trans.WorkOrder.Requestor_PersonnelId = Requestor_PersonnelId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return trans.WorkOrder.WorkOrderId;

        }
        public long WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary
          (DatabaseKey dbKey
          , long PrevMaintBatchId
          , string ClientLookupId,
          long Requestor_PersonnelId)
        {
            WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary trans = new WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.WorkOrder = new b_WorkOrder();
            trans.WorkOrder.PrevMaintBatchId = PrevMaintBatchId;
            trans.WorkOrder.ClientLookupId = ClientLookupId;
            trans.WorkOrder.Requestor_PersonnelId = Requestor_PersonnelId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return trans.WorkOrder.WorkOrderId;

        }

        public long WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary_V2
  (DatabaseKey dbKey
  , long PrevMaintBatchId
  , string ClientLookupId,
  long Requestor_PersonnelId, string AssetGroup1Ids, string AssetGroup2Ids, string AssetGroup3Ids, string PrevMaintSchedType, string PrevMaintMasterType, out long LastWorkAssignedPersonnelId)
        {
            WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary_V2 trans = new WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary_V2
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.WorkOrder = new b_WorkOrder();
            trans.WorkOrder.ClientId = dbKey.Client.ClientId ;
            trans.WorkOrder.SiteId = dbKey.User.DefaultSiteId;
            trans.WorkOrder.PrevMaintBatchId = PrevMaintBatchId;
            trans.WorkOrder.ClientLookupId = ClientLookupId;
            trans.WorkOrder.Requestor_PersonnelId = Requestor_PersonnelId;
            trans.WorkOrder.AssetGroup1Ids = AssetGroup1Ids;
            trans.WorkOrder.AssetGroup2Ids = AssetGroup2Ids;
            trans.WorkOrder.AssetGroup3Ids = AssetGroup3Ids;
            trans.WorkOrder.PrevMaintSchedType = PrevMaintSchedType;
            trans.WorkOrder.PrevMaintMasterType = PrevMaintMasterType;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            LastWorkAssignedPersonnelId = trans.WorkOrder.WorkAssigned_PersonnelId;
            return trans.WorkOrder.WorkOrderId;

        }
        /*------Retrive Work Request Only----------------------------------------------------------------------------*/
        // SOM-706 - Added timezone parameter
        public List<WorkOrder> RetrieveAllWorkRequestForSearch(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_RetrieveAllWorkRequestForSearch trans = new WorkOrder_RetrieveAllWorkRequestForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> workorderlist = new List<WorkOrder>();
            // RKL - 2014-Aug-08
            // Moved this to UpdateFromDatabaseObjectForRetriveAllForSearch
            foreach (b_WorkOrder workorder in trans.WorkOrderList)
            {
                WorkOrder tmpworkorder = new WorkOrder();
                /*
                  tmpworkorder.DepartmentName = workorder.DepartmentName;
                  tmpworkorder.Creator = workorder.Creator;
                  tmpworkorder.Assigned = workorder.Assigned;
                */
                // SOM-706 - Added timezone parameter
                tmpworkorder.UpdateFromDatabaseObjectForRetriveAllForSearch(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }
            return workorderlist;
        }

        #endregion

        public WorkOrder RetrieveV2(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_RetrieveV2Search trans = new WorkOrder_RetrieveV2Search()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetriveChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfWO = new List<WorkOrder>();

            List<WorkOrder> workorderlist = new List<WorkOrder>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_WorkOrder workorder in trans.WorkOrder.listOfWO)
            {
                WorkOrder tmpworkorder = new WorkOrder();

                tmpworkorder.UpdateFromDatabaseObjectForRetriveV2(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }
            this.listOfWO.AddRange(workorderlist);
            return this;
        }

        public WorkOrder RetrieveV2Print(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_RetrieveV2Print trans = new WorkOrder_RetrieveV2Print()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.utilityAdd = new UtilityAdd();
            this.listOfWO = new List<WorkOrder>();

            this.utilityAdd.list1 = trans.WorkOrder.utilityAdd.list1;
            this.utilityAdd.list2 = trans.WorkOrder.utilityAdd.list2;
            this.utilityAdd.list3 = trans.WorkOrder.utilityAdd.list3;
            this.utilityAdd.list4 = trans.WorkOrder.utilityAdd.list4;
            this.utilityAdd.list5 = trans.WorkOrder.utilityAdd.list5;
            List<WorkOrder> workorderlist = new List<WorkOrder>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_WorkOrder workorder in trans.WorkOrder.listOfWO)
            {
                WorkOrder tmpworkorder = new WorkOrder();

                tmpworkorder.UpdateFromDatabaseObjectForRetriveV2(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }
            this.listOfWO.AddRange(workorderlist);
            return this;
        }


        public WorkOrder WRDashboardRetrieveAllForSearch(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_WRDashboardRetrieveAllForSearch trans = new WorkOrder_WRDashboardRetrieveAllForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.utilityAdd = new UtilityAdd();
            this.listOfWO = new List<WorkOrder>();

            this.utilityAdd.list1 = trans.WorkOrder.utilityAdd.list1;

            List<WorkOrder> workorderlist = new List<WorkOrder>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_WorkOrder workorder in trans.WorkOrder.listOfWO)
            {
                WorkOrder tmpworkorder = new WorkOrder();

                tmpworkorder.UpdateFromDatabaseObjectForWRDashboardRetrieve(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }
            this.listOfWO.AddRange(workorderlist);
            return this;
        }


        public List<WorkOrder> WorkOrder_WRDashboardRetrieveAllForPrint_V2(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_WRDashboardRetrieveAllForSearchPrint trans = new WorkOrder_WRDashboardRetrieveAllForSearchPrint()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> workorderlist = new List<WorkOrder>();

            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_WorkOrder workorder in trans.WorkOrderList)
            {
                WorkOrder tmpworkorder = new WorkOrder();

                tmpworkorder.UpdateFromDatabaseObjectForWRDashboardRetrieve(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }

            return workorderlist;
        }



        public void UpdateFromDatabaseObjectForWRDashboardRetrieve(b_WorkOrder dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ClientId = dbObj.ClientId;
            this.WorkOrderId = dbObj.WorkOrderId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.SiteId = dbObj.SiteId;
            this.Description = dbObj.Description;
            this.ChargeToId = dbObj.ChargeToId;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.Status = dbObj.Status;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            this.Assigned = dbObj.Assigned;
            this.ScheduledFinishDate = dbObj.ScheduledFinishDate;
            this.CompleteDate = dbObj.CompleteDate;

            this.TotalCount = dbObj.TotalCount;

            // Localization of Status            
            switch (this.Status)
            {
                case Common.Constants.WorkOrderStatusConstants.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.WorkOrderStatusConstants.AwaitingApproval:
                    this.Status_Display = "AwaitingApproval";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Canceled:
                    this.Status_Display = "Canceled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Denied:
                    this.Status_Display = "Denied";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.WorkRequest:
                    this.Status_Display = "WorkRequest";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }

        }

        public void UpdateFromDatabaseObjectformultisearch(b_WorkOrder dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ChargeTo = dbObj.ChargeTo;

        }
        public void UpdateFromDatabaseObjectForRetriveV2(b_WorkOrder dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.DepartmentName = dbObj.DepartmentName;
            this.Creator = dbObj.Creator;
            this.Assigned = dbObj.Assigned;
            this.DateRange = dbObj.DateRange;
            this.DateColumn = dbObj.DateColumn;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.Createby = dbObj.Createby;
            this.PersonnelClientLookupId = dbObj.PersonnelClientLookupId;
            this.Requestor_PersonnelClientLookupId = dbObj.Requestor_PersonnelClientLookupId;
            this.CreateDate = dbObj.CreateDate;
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            this.FailureCode = dbObj.FailureCode;
            this.ActualFinishDate = dbObj.ActualFinishDate;
            this.CompletedDate = dbObj.CompletedDate;
            this.AssignedFullName = dbObj.AssignedFullName;
            this.Personnels = dbObj.Personnels;
            this.AssetGroup1ClientlookupId = dbObj.AssetGroup1ClientlookupId;
            this.AssetGroup2ClientlookupId = dbObj.AssetGroup2ClientlookupId;
            this.AssetGroup3ClientlookupId = dbObj.AssetGroup3ClientlookupId;
            this.SourceType = dbObj.SourceType;//<!--Added on 23/06/2020-->

            this.TotalCount = dbObj.TotalCount;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CompleteDate = dbObj.CompleteDate;
            }

            this.Priority = dbObj.Priority;
            this.DeniedBy_PersonnelId_ClientLookupId = dbObj.DeniedBy_PersonnelId_ClientLookupId;
            this.DeniedBy_PersonnelId_Name = dbObj.DeniedBy_PersonnelId_Name;
            this.ActualDuration = dbObj.ActualDuration;
            // Localization of Status            
            switch (this.Status)
            {
                case Common.Constants.WorkOrderStatusConstants.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.WorkOrderStatusConstants.AwaitingApproval:
                    this.Status_Display = "AwaitingApproval";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Canceled:
                    this.Status_Display = "Canceled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Denied:
                    this.Status_Display = "Denied";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.WorkRequest:
                    this.Status_Display = "WorkRequest";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }
            this.AssetLocation = dbObj.AssetLocation;

            this.ProjectClientLookupId = dbObj.ProjectClientLookupId;//v2-850

            this.DownRequired = dbObj.DownRequired;//v2-892
            this.PlannerFullName = dbObj.PlannerFullName; //V2-1078

        }

        public List<WorkOrder> RetrieveWorkOrdersForUnscheduled(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_RetrieveUnscheduledWorkOrder trans = new WorkOrder_RetrieveUnscheduledWorkOrder()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetriveWorkbenchForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> workorderlist = new List<WorkOrder>();

            foreach (b_WorkOrder workorder in trans.WorkOrderList)
            {
                WorkOrder tmpworkorder = new WorkOrder();
                // SOM-706 - Added timezone parameter
                tmpworkorder.UpdateFromDatabaseObjectForRetriveAllForSearch(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }
            return workorderlist;
        }

        public List<WorkOrder> RetrieveWorkOrdersByClientLookupId(DatabaseKey dbKey)
        {
            WorkOrder_RetrieveByClientLookupId trans = new WorkOrder_RetrieveByClientLookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> workorderlist = new List<WorkOrder>();

            foreach (b_WorkOrder workorder in trans.WOList)
            {
                WorkOrder tmpworkorder = new WorkOrder();
                // SOM-706 - Added timezone parameter
                tmpworkorder.UpdateFromDatabaseObject(workorder);

                workorderlist.Add(tmpworkorder);
            }
            return workorderlist;
        }

        public List<WorkOrder> RetrieveWorkOrdersByScheduleId(DatabaseKey dbKey)
        {
            WorkOrder_RetrieveByScheduleId trans = new WorkOrder_RetrieveByScheduleId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObject();
            trans.WorkOrder.ScheduleId = this.ScheduleId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> workorderlist = new List<WorkOrder>();

            foreach (b_WorkOrder workorder in trans.WOList)
            {
                WorkOrder tmpworkorder = new WorkOrder();
                // SOM-706 - Added timezone parameter
                tmpworkorder.UpdateFromDatabaseObject(workorder);

                workorderlist.Add(tmpworkorder);
            }
            return workorderlist;
        }
        //end

        public string Search { get; set; }

        public List<WorkOrder> RetrieveWorkOrderForPrint(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_RetrieveWorkOrderForPrint trans = new WorkOrder_RetrieveWorkOrderForPrint()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObject();
            trans.WorkOrder.Search = this.Search;
            trans.WorkOrder.PersonnelId = this.PersonnelId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> WOlist = new List<WorkOrder>();

            foreach (b_WorkOrder WorkOrder in trans.WorkOrderList)
            {
                WorkOrder tmpWorkOrder = new WorkOrder();
                // RKL - Moved to the .UpdateFromDatabaseObjectForRetriveAll method
                tmpWorkOrder.UpdateFromDatabaseObjectForRetriveAll(WorkOrder, TimeZone);
                WOlist.Add(tmpWorkOrder);
            }
            return WOlist;
        }
        private void UpdateFromDatabaseObjectForRetriveAll(b_WorkOrder dbObj, string TimeZone)
        {

            this.UpdateFromDatabaseObject(dbObj);
            // SOM-1278
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.CreateBy_PersonnelName = dbObj.CreateBy_PersonnelName;
            this.WorkAssigned_Name = dbObj.WorkAssigned_Name;
            // Status_Display
            // SOM-279 Localization of Status            
            switch (this.Status)
            {
                case Common.Constants.WorkOrderStatusConstants.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.WorkOrderStatusConstants.AwaitingApproval:
                    this.Status_Display = "AwaitingApproval";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Canceled:
                    this.Status_Display = "loc.WorkOrderStatus.Canceled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Denied:
                    this.Status_Display = "Denied";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.WorkRequest:
                    this.Status_Display = "WorkRequest";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }

            // SOM-706 - Convert the create date to the user's time zone
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ConvertFromUTCToUser(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
        }

        public List<WorkOrder> RetrieveAll(DatabaseKey dbKey)
        {

            WorkOrder_RetrieveAll_V2 trans = new WorkOrder_RetrieveAll_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> WorkOrderList = new List<WorkOrder>();
            foreach (b_WorkOrder WorkOrder in trans.WorkOrderList)
            {
                WorkOrder tmpWorkOrder = new WorkOrder();

                tmpWorkOrder.UpdateFromDatabaseObject(WorkOrder);
                WorkOrderList.Add(tmpWorkOrder);
            }
            return WorkOrderList;
        }

        public List<WorkOrder> WorkOrderRetrieveForWorkOrderCostWidget(DatabaseKey dbKey)
        {
            WorkOrder_RetrieveForWorkOrderCostWidget trans = new WorkOrder_RetrieveForWorkOrderCostWidget()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> workorderlist = new List<WorkOrder>();

            foreach (b_WorkOrder workorder in trans.WorkOrderList)
            {
                WorkOrder tmpworkorder = new WorkOrder();
                // SOM-706 - Added timezone parameter
                tmpworkorder.UpdateFromDatabaseForWorkOrderCostWidget(workorder);

                workorderlist.Add(tmpworkorder);
            }
            return workorderlist;
        }

        public void UpdateFromDatabaseForWorkOrderCostWidget(b_WorkOrder dbObjs)
        {
            this.PartCost = dbObjs.PartCost;
            this.LaborCost = dbObjs.LaborCost;
            this.OtherCost = dbObjs.OtherCost;
            this.TotalCost = dbObjs.TotalCost;
        }

        public void UpdateFromDatabaseObjectforRetrievePOandPR(b_WorkOrder dbObj, string TimeZone)
        {
            this.WONumber = dbObj.WONumber;
            this.POType = dbObj.POType;
            this.PONumber = dbObj.PONumber;
            this.POStatus = dbObj.POStatus;
            this.LineNumber = dbObj.LineNumber;
            this.LineStatus = dbObj.LineStatus;
            this.LineDesc = dbObj.LineDesc;
            this.VendorClientlookupId = dbObj.VendorClientlookupId;
            this.VendorName = dbObj.VendorName;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
        }

        public void UpdateFromDatabaseObjectforListLaborSchedulingChunkSearch(b_WorkOrder dbObj, string TimeZone)
        {
            this.WorkOrderId = dbObj.WorkOrderId;
            this.PersonnelId = dbObj.PersonnelId;
            this.WorkOrderScheduleId = dbObj.WorkOrderScheduleId;
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.Type = dbObj.Type;
            this.Description = dbObj.Description;
            this.ScheduledHours = dbObj.ScheduledHours;
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            this.RequiredDate = dbObj.RequiredDate;
            this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.Status = dbObj.Status;
            this.PersonnelName = dbObj.PersonnelName;
            this.PerNextValue = dbObj.PerNextValue;
            this.SDNextValue = dbObj.SDNextValue;
            this.SumPersonnelHour = dbObj.SumPersonnelHour;
            this.SumScheduledateHour = dbObj.SumScheduledateHour;
            this.GrandTotalHour = dbObj.GrandTotalHour;
            this.PerIDNextValue = dbObj.PerIDNextValue;

            this.TotalCount = dbObj.TotalCount;
            this.PartsOnOrder= dbObj.PartsOnOrder; //V2-838
        }
        public void UpdateFromDatabaseObjectforCalendarLaborSchedulingChunkSearch(b_WorkOrder dbObj, string TimeZone)
        {
            this.WorkOrderId = dbObj.WorkOrderId;
            this.WorkOrderScheduleId = dbObj.WorkOrderScheduleId;
            this.PersonnelId = dbObj.PersonnelId;
            this.PersonnelFull = dbObj.PersonnelFull;
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.Description = dbObj.Description;
            this.ScheduledHours = dbObj.ScheduledHours;
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            PartsOnOrder= dbObj.PartsOnOrder; //V2-838
        }
        public List<WorkOrder> RetrievePOandPR(DatabaseKey dbKey, string TimeZone)
        {

            WorkOrder_RetrievePOandPR trans = new WorkOrder_RetrievePOandPR()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> WorkOrderList = new List<WorkOrder>();
            foreach (b_WorkOrder WorkOrder in trans.WorkOrderList)
            {
                WorkOrder tmpWorkOrder = new WorkOrder();

                tmpWorkOrder.UpdateFromDatabaseObjectforRetrievePOandPR(WorkOrder, TimeZone);
                WorkOrderList.Add(tmpWorkOrder);
            }
            return WorkOrderList;
        }

        public void WorkOrderUpdateOnRemovingSchedule(DatabaseKey dbKey, string Timezone)
        {
            Validate<WorkOrder>(dbKey);

            if (IsValid)
            {
                WorkOrder_UpdateOnRemovingSchedule trans = new WorkOrder_UpdateOnRemovingSchedule()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.WorkOrder = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                UpdateFromDatabaseObjectOnRemovingSchedule(trans.WorkOrder, Timezone);

            }
        }
        public void WorkOrderUpdateListPartsonOrder(DatabaseKey dbKey, string Timezone)
        {
            WorkOrder_UpdateListPartsonOrder trans = new WorkOrder_UpdateListPartsonOrder()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.WorkOrder = this.ToDatabaseObjectUpdateListPartsonOrderExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }

        public void WorkOrderUpdatePartsonOrder(DatabaseKey dbKey, string Timezone)
        {
            WorkOrder_UpdatePartsonOrder trans = new WorkOrder_UpdatePartsonOrder()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.WorkOrder = this.ToDatabaseObjectUpdatePartsonOrderExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
        public void UpdateFromDatabaseObjectOnRemovingSchedule(b_WorkOrder dbObj, string Timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            // SOM-1637 - Convert the create date to the user's time zone
            if (dbObj.ModifyDate != null && dbObj.ModifyDate != DateTime.MinValue)
            {
                this.ModifyDate = dbObj.ModifyDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.ModifyDate = dbObj.ModifyDate;
            }
            // SOM-1637 - Convert the create date to the user's time zone
            if (dbObj.ModifyDate != null && dbObj.ModifyDate != DateTime.MinValue)
            {
                this.ModifyDate = dbObj.ModifyDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.ModifyDate = dbObj.ModifyDate;
            }
            // SOM-1637 - Convert the complete date to the user's time zone
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CompleteDate = dbObj.CompleteDate;
            }

        }

        public List<WorkOrder> RetrieveListForLaborSchedulingChunkSearch(DatabaseKey dbKey, string TimeZone)
        {

            WorkOrder_ListForLaborSchedulingChunkSearch trans = new WorkOrder_ListForLaborSchedulingChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObjectListLaborScheduling();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> WorkOrderList = new List<WorkOrder>();
            foreach (b_WorkOrder WorkOrder in trans.WorkOrderList)
            {
                WorkOrder tmpWorkOrder = new WorkOrder();

                tmpWorkOrder.UpdateFromDatabaseObjectforListLaborSchedulingChunkSearch(WorkOrder, TimeZone);
                WorkOrderList.Add(tmpWorkOrder);
            }
            return WorkOrderList;
        }

        public List<WorkOrder> RetrieveCalendarForLaborSchedulingChunkSearch(DatabaseKey dbKey, string TimeZone)
        {

            WorkOrder_CalendarForLaborSchedulingChunkSearch trans = new WorkOrder_CalendarForLaborSchedulingChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObjectListCalendarScheduling();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> WorkOrderList = new List<WorkOrder>();
            foreach (b_WorkOrder WorkOrder in trans.WorkOrderList)
            {
                WorkOrder tmpWorkOrder = new WorkOrder();

                tmpWorkOrder.UpdateFromDatabaseObjectforCalendarLaborSchedulingChunkSearch(WorkOrder, TimeZone);
                WorkOrderList.Add(tmpWorkOrder);
            }
            return WorkOrderList;
        }

        public b_WorkOrder ToDatabaseObjectListLaborScheduling()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PersonnelList = this.PersonnelList;
            dbObj.ScheduledDateStart = this.ScheduledDateStart;
            dbObj.ScheduledDateEnd = this.ScheduledDateEnd;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.Description = this.Description;
            dbObj.RequireDate = this.RequireDate;
            dbObj.Type = this.Type;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.SearchText = this.SearchText;

            return dbObj;
        }

        public b_WorkOrder ToDatabaseObjectListCalendarScheduling()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PersonnelList = this.PersonnelList;
            dbObj.ScheduledDateStart = this.ScheduledDateStart;
            dbObj.ScheduledDateEnd = this.ScheduledDateEnd;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.Description = this.Description;
            dbObj.RequireDate = this.RequireDate;
            dbObj.Type = this.Type;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.SearchText = this.SearchText;
            dbObj.CalendarDateStart = this.CalendarDateStart;
            dbObj.CalendarDateEnd = this.CalendarDateEnd;

            return dbObj;
        }

        public List<WorkOrder> RetrieveAvailableWorkForLaborSchedulingSearch(DatabaseKey dbKey, string TimeZone)
        {

            WorkOrder_AvailableWorkForLaborSchedulingSearch trans = new WorkOrder_AvailableWorkForLaborSchedulingSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObjectAvailableWorkLaborScheduling();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> WorkOrderList = new List<WorkOrder>();
            foreach (b_WorkOrder WorkOrder in trans.WorkOrderList)
            {
                WorkOrder tmpWorkOrder = new WorkOrder();

                tmpWorkOrder.UpdateFromDatabaseObjectforAvailableWorkLaborSchedulingSearch(WorkOrder, TimeZone);
                WorkOrderList.Add(tmpWorkOrder);
            }
            return WorkOrderList;
        }
        public void UpdateFromDatabaseObjectforAvailableWorkLaborSchedulingSearch(b_WorkOrder dbObj, string TimeZone)
        {
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.WorkOrderId = dbObj.WorkOrderId;
            this.Type = dbObj.Type;
            this.Description = dbObj.Description;
            this.ScheduledHours = dbObj.ScheduledHours;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.Status = dbObj.Status;
            this.EquipDown = dbObj.EquipDown;
            this.Priority = dbObj.Priority;
            // V2-773 - do NOT convert scheduled start date
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            //if (dbObj.ScheduledStartDate != null && dbObj.ScheduledStartDate != DateTime.MinValue)
            //{
            //    this.ScheduledStartDate = dbObj.ScheduledStartDate.ToUserTimeZone(TimeZone);
            //}
            //else
            //{
            //    this.ScheduledStartDate = dbObj.ScheduledStartDate;
            //}
            this.ChargeToId = dbObj.ChargeToId;
            this.DownRequired = dbObj.DownRequired;
            // V2-773
            // Required Date should NOT be converted
            this.RequiredDate = dbObj.RequiredDate;
            //if (dbObj.RequiredDate != null && dbObj.RequiredDate != DateTime.MinValue)
            //{
            //    this.RequiredDate = dbObj.RequiredDate.ToUserTimeZone(TimeZone);
            //}
            //else
            //{
            //    this.RequiredDate = dbObj.RequiredDate;
            //}
            this.DepartmentName = dbObj.DepartmentName;
            this.WorkAssigned_Name = dbObj.WorkAssigned_Name;
            this.ScheduledDuration = dbObj.ScheduledDuration;
            this.ChargeTo = dbObj.ChargeTo;
            this.ChargeType = dbObj.ChargeType;
            this.PartsOnOrder = dbObj.PartsOnOrder; //V2-838
            //V2-984
            this.Personnels = dbObj.Personnels;
            this.WorkAssigned_PersonnelId = dbObj.WorkAssigned_PersonnelId;
            this.TotalCount = dbObj.TotalCount;
        }

        public b_WorkOrder ToDatabaseObjectAvailableWorkLaborScheduling()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.ChargeTo = this.ChargeTo;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.Description = this.Description;
            dbObj.Status = this.Status;
            dbObj.Priority = this.Priority;
            dbObj.Type = this.Type;
            dbObj.Assigned = this.Assigned;
            //if (dbObj.RequiredDate != null && dbObj.RequiredDate != DateTime.MinValue)
            //{
            //    dbObj.RequireDate = this.RequiredDate.ToShortDate();
            //}
            //else
            //{
            //    this.RequiredDate = null;
            //}
            dbObj.RequireDate= this.RequireDate;

            dbObj.ScheduleFlag = this.ScheduleFlag;
            return dbObj;
        }
        public List<WorkOrder> RetrieveApprovedWorkOrderForLaborScheduling(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_RetrieveApprovedWorkOrderForLaborScheduling trans = new WorkOrder_RetrieveApprovedWorkOrderForLaborScheduling()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> WorkOrderList = new List<WorkOrder>();
            foreach (b_WorkOrder WorkOrder in trans.WorkOrderList)
            {
                WorkOrder tmpWorkOrder = new WorkOrder();

                tmpWorkOrder.UpdateFromDatabaseObject(WorkOrder);
                WorkOrderList.Add(tmpWorkOrder);
            }
            return WorkOrderList;
        }
        public List<WorkOrder> RetrieveAvailableWorkForLaborSchedulingSearchCalendar(DatabaseKey dbKey, string TimeZone)
        {

            WorkOrder_AvailableWorkForLaborSchedulingSearchCalendar trans = new WorkOrder_AvailableWorkForLaborSchedulingSearchCalendar()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObjectAvailableWorkLaborScheduling();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> WorkOrderList = new List<WorkOrder>();
            foreach (b_WorkOrder WorkOrder in trans.WorkOrderList)
            {
                WorkOrder tmpWorkOrder = new WorkOrder();

                tmpWorkOrder.UpdateFromDatabaseObjectforAvailableWorkLaborSchedulingSearchCalendar(WorkOrder, TimeZone);
                WorkOrderList.Add(tmpWorkOrder);
            }
            return WorkOrderList;
        }
        public void UpdateFromDatabaseObjectforAvailableWorkLaborSchedulingSearchCalendar(b_WorkOrder dbObj, string TimeZone)
        {
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.WorkOrderId = dbObj.WorkOrderId;
            this.Type = dbObj.Type;
            this.Description = dbObj.Description;
            this.ScheduledHours = dbObj.ScheduledHours;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.Status = dbObj.Status;
            this.EquipDown = dbObj.EquipDown;
            this.Priority = dbObj.Priority;
            // V2-773
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            // Do NOT convert scheduled start date
            //if (dbObj.ScheduledStartDate != null && dbObj.ScheduledStartDate != DateTime.MinValue)
            //{
            //    this.ScheduledStartDate = dbObj.ScheduledStartDate.ToUserTimeZone(TimeZone);
            //}
            //else
            //{
            //    this.ScheduledStartDate = dbObj.ScheduledStartDate;
            //}
            this.ChargeToId = dbObj.ChargeToId;
            this.DownRequired = dbObj.DownRequired;
            // V2-773
            // Required Date should NOT be converted
            this.RequiredDate = dbObj.RequiredDate;
            //if (dbObj.RequiredDate != null && dbObj.RequiredDate != DateTime.MinValue)
            //{
            //    this.RequiredDate = dbObj.RequiredDate.ToUserTimeZone(TimeZone);
            //}
            //else
            //{
            //    this.RequiredDate = dbObj.RequiredDate;
            //}
            this.DepartmentName = dbObj.DepartmentName;
            this.WorkAssigned_Name = dbObj.WorkAssigned_Name;
            this.ScheduledDuration = dbObj.ScheduledDuration;
            this.ChargeTo = dbObj.ChargeTo;
            this.ChargeType = dbObj.ChargeType;
            this.PartsOnOrder = dbObj.PartsOnOrder;//V2-838
            //V2-984
            this.Personnels = dbObj.Personnels;
            this.WorkAssigned_PersonnelId = dbObj.WorkAssigned_PersonnelId;
            this.TotalCount = dbObj.TotalCount;
        }

        #region WorkOrder Lookup List

        public List<WorkOrder> GetAllWorkOrderLookupListV2(DatabaseKey dbKey)
        {
            WorkOrder_RetrieveChunkSearchLookupList_V2 trans = new WorkOrder_RetrieveChunkSearchLookupList_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };

            trans.WorkOrder = this.ToDateBaseObjectForWorkOrderLookupListChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfWO = new List<WorkOrder>();

            List<WorkOrder> Workorderlist = new List<WorkOrder>();

            foreach (b_WorkOrder workorder in trans.WorkOrderList)
            {
                WorkOrder tmpWorkOrder = new WorkOrder();
                tmpWorkOrder.UpdateFromDatabaseObjectForWorkOrderLookupListChunkSearch(workorder);
                Workorderlist.Add(tmpWorkOrder);
            }
            return Workorderlist;
        }


        public void UpdateFromDatabaseObjectForWorkOrderLookupListChunkSearch(b_WorkOrder dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.WorkAssigned_Name = dbObj.WorkAssigned_Name;
            this.Requestor_Name = dbObj.Requestor_Name;
            this.TotalCount = dbObj.TotalCount;
        }


        public b_WorkOrder ToDateBaseObjectForWorkOrderLookupListChunkSearch()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();

            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.Description = this.Description;
            dbObj.ClientId = this.ClientId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Description = this.Description;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.WorkAssigned_Name = this.WorkAssigned_Name;
            dbObj.Requestor_Name = this.Requestor_Name;
            dbObj.Status = this.Status;
            dbObj.SiteId = this.SiteId;

            return dbObj;
        }

        #endregion

        #region Work Order Completion Workbench Chunk Search
        public WorkOrder RetrieveV2ForCompletionWorkbench(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_RetrieveV2SearchForCompletionWorkbench trans = new WorkOrder_RetrieveV2SearchForCompletionWorkbench()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetriveChunkSearchCompletionWorkbench();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfWO = new List<WorkOrder>();

            List<WorkOrder> workorderlist = new List<WorkOrder>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_WorkOrder workorder in trans.WorkOrder.listOfWO)
            {
                WorkOrder tmpworkorder = new WorkOrder();

                tmpworkorder.UpdateFromDatabaseObjectForRetriveCompletionWorkbenchV2(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }
            this.listOfWO.AddRange(workorderlist);
            return this;
        }

        public b_WorkOrder ToDateBaseObjectForRetriveChunkSearchCompletionWorkbench()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();

            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.SearchText = this.SearchText;

            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveCompletionWorkbenchV2(b_WorkOrder dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            this.AssetName = dbObj.AssetName;
            this.Priority = dbObj.Priority;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            // V2-773 - 2022-Oct-06 - RKL
            // Scheduled Start Date is selected by the user
            // Do NOT convert from UTC
            // 
            //if (dbObj.ScheduledStartDate != null && dbObj.ScheduledStartDate != DateTime.MinValue)
            //{
            //    this.ScheduledStartDate = dbObj.ScheduledStartDate.ToUserTimeZone(TimeZone);
            //}
            //else
            //{
            //    this.ScheduledStartDate = dbObj.ScheduledStartDate;
            //}
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CompleteDate = dbObj.CompleteDate;
            }

            this.TotalCount = dbObj.TotalCount;

            // Localization of Status            
            switch (this.Status)
            {
                case Common.Constants.WorkOrderStatusConstants.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.WorkOrderStatusConstants.AwaitingApproval:
                    this.Status_Display = "AwaitingApproval";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Canceled:
                    this.Status_Display = "Canceled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Denied:
                    this.Status_Display = "Denied";
                    break;
                case Common.Constants.WorkOrderStatusConstants.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                case Common.Constants.WorkOrderStatusConstants.WorkRequest:
                    this.Status_Display = "WorkRequest";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }
            this.AssignedFullName = dbObj.AssignedFullName;
            this.AssetLocation = dbObj.AssetLocation;
            this.ProjectClientLookupId = dbObj.ProjectClientLookupId;
        }
        #endregion

        #region  Workorder Approval Workbench Search V2-630
        public List<WorkOrder> RetrieveWorkBenchForSearchV2(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_RetrieveAllWorkbenchSearchV2 trans = new WorkOrder_RetrieveAllWorkbenchSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetriveWorkbenchForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> workorderlist = new List<WorkOrder>();

            foreach (b_WorkOrder workorder in trans.WorkOrderList)
            {
                WorkOrder tmpworkorder = new WorkOrder();
                // RKL - Moved to the .UpdateFromDatabaseObjectForRetriveAllForSearch method
                //tmpworkorder.DepartmentName = workorder.DepartmentName;
                //tmpworkorder.Creator = workorder.Creator;
                //tmpworkorder.Assigned = workorder.Assigned;
                //tmpworkorder.Createby = workorder.Createby;
                // SOM-706 - Added timezone parameter
                tmpworkorder.UpdateFromDatabaseObjectForRetriveAllForSearch(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }
            return workorderlist;
        }
        #endregion

        #region Available Work order Daily LaborScheduling V2-630

        public List<WorkOrder> RetrieveAvailableWorkForDailyLaborSchedulingBySearchV2(DatabaseKey dbKey, string TimeZone)
        {

            WorkOrder_AvailableWorkForDailyLaborSchedulingBySearchV2 trans = new WorkOrder_AvailableWorkForDailyLaborSchedulingBySearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseObjectAvailableWorkForDailyLaborSchedulingBySearchV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrder> WorkOrderList = new List<WorkOrder>();
            foreach (b_WorkOrder WorkOrder in trans.WorkOrderList)
            {
                WorkOrder tmpWorkOrder = new WorkOrder();

                tmpWorkOrder.UpdateFromDatabaseObjectforAvailableWorkForDailyLaborSchedulingBySearchV2(WorkOrder, TimeZone);
                WorkOrderList.Add(tmpWorkOrder);
            }
            return WorkOrderList;
        }
        public void UpdateFromDatabaseObjectforAvailableWorkForDailyLaborSchedulingBySearchV2(b_WorkOrder dbObj, string TimeZone)
        {
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.WorkOrderId = dbObj.WorkOrderId;
            this.Type = dbObj.Type;
            this.Description = dbObj.Description;
            this.ScheduledHours = dbObj.ScheduledHours;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.Status = dbObj.Status;
            this.EquipDown = dbObj.EquipDown;
            this.Priority = dbObj.Priority;
            // V2-773
            // Do NOT convert scheduled start date
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            //if (dbObj.ScheduledStartDate != null && dbObj.ScheduledStartDate != DateTime.MinValue)
            //{
            //    this.ScheduledStartDate = dbObj.ScheduledStartDate.ToUserTimeZone(TimeZone);
            //}
            //else
            //{
            //    this.ScheduledStartDate = dbObj.ScheduledStartDate;
            //}
            this.ChargeToId = dbObj.ChargeToId;
            this.DownRequired = dbObj.DownRequired;
            // V2-773
            // Required Date should NOT be converted
            this.RequiredDate = dbObj.RequiredDate;
            //if (dbObj.RequiredDate != null && dbObj.RequiredDate != DateTime.MinValue)
            //{
            //    this.RequiredDate = dbObj.RequiredDate.ToUserTimeZone(TimeZone);
            //}
            //else
            //{
            //    this.RequiredDate = dbObj.RequiredDate;
            //}
            this.DepartmentName = dbObj.DepartmentName;
            this.WorkAssigned_Name = dbObj.WorkAssigned_Name;
            this.ScheduledDuration = dbObj.ScheduledDuration;
            this.ChargeTo = dbObj.ChargeTo;
            this.ChargeToDescription = dbObj.ChargeToDescription;
            this.PartsOnOrder = dbObj.PartsOnOrder; //V2-838
            //V2-984
            this.Personnels = dbObj.Personnels;
            this.WorkAssigned_PersonnelId = dbObj.WorkAssigned_PersonnelId;
            this.TotalCount = dbObj.TotalCount;
        }
        public b_WorkOrder ToDatabaseObjectAvailableWorkForDailyLaborSchedulingBySearchV2()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ScheduleFlag = this.ScheduleFlag;
            #region V2-984
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.ChargeTo = this.ChargeTo;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.Description = this.Description;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy=this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.Status = this.Status;
            dbObj.Priority=this.Priority;
            dbObj.downRequired = this.downRequired;
            dbObj.Type = this.Type;
            dbObj.RequireDate = this.RequireDate;
            dbObj.ScheduledDateStart = this.ScheduledDateStart;
            dbObj.Assigned = this.Assigned;
            dbObj.ScheduledHours = this.ScheduledHours;
            
            #endregion
            return dbObj;
        }
        #endregion


        public WorkOrder RetrieveAllByWorkOrdeV2Print(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_RetrieveAllByWorkOrdeV2Print trans = new WorkOrder_RetrieveAllByWorkOrdeV2Print()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetrieveAllByWorkOrdeV2Print();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();



            List<WorkOrder> workorderlist = new List<WorkOrder>();
            List<Attachment> Attachmentlist = new List<Attachment>();
            List<WorkOrderTask> WorkOrderTasklist = new List<WorkOrderTask>();
            List<Timecard> Timecardlist = new List<Timecard>();
            List<PartHistory> PartHistorylist = new List<PartHistory>();
            List<OtherCosts> OtherCostslist = new List<OtherCosts>();
            List<OtherCosts> OtherCostsSummerylist = new List<OtherCosts>();
            List<Instructions> instructions = new List<Instructions>();
            #region V2-944
            List<WorkOrderUDF> workOrderUDFlist = new List<WorkOrderUDF>();
            List<WorkOrderSchedule> workOrderSchedulelist = new List<WorkOrderSchedule>();
            #endregion

            this.listOfWO = new List<WorkOrder>();
            foreach (b_WorkOrder workorder in trans.WorkOrder.listOfWO)
            {
                WorkOrder tmpworkorder = new WorkOrder();

                tmpworkorder.UpdateFromDatabaseObjectPrintWorkOrderExtended(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }
            this.listOfWO.AddRange(workorderlist);

            this.listOfAttachment = new List<Attachment>();
            foreach (b_Attachment attachment in trans.WorkOrder.listOfAttachment)
            {
                Attachment tmpAttachment = new Attachment();
                tmpAttachment.UpdateFromDatabaseObjectAttachmentPrintExtended(attachment, TimeZone);
                Attachmentlist.Add(tmpAttachment);
            }
            this.listOfAttachment.AddRange(Attachmentlist);

            this.listOfWOTask = new List<WorkOrderTask>();
            foreach (b_WorkOrderTask WorkOrderTask in trans.WorkOrder.listOfWOTask)
            {
                WorkOrderTask tmpworkordertask = new WorkOrderTask();

                tmpworkordertask.UpdateFromDatabaseObjectWoTaskPrintExtended(WorkOrderTask);
                WorkOrderTasklist.Add(tmpworkordertask);
            }
            this.listOfWOTask.AddRange(WorkOrderTasklist);

            this.listOfTimecard = new List<Timecard>();
            foreach (b_Timecard Timecard in trans.WorkOrder.listOfTimecard)
            {
                Timecard tmpTimecard = new Timecard();

                tmpTimecard.UpdateFromDatabaseObjectTimeCardPrintExtended(Timecard);
                Timecardlist.Add(tmpTimecard);
            }
            this.listOfTimecard.AddRange(Timecardlist);

            this.listOfPartHistory = new List<PartHistory>();
            foreach (b_PartHistory PartHistory in trans.WorkOrder.listOfPartHistory)
            {
                PartHistory tmpPartHistory = new PartHistory();

                tmpPartHistory.UpdateFromDatabaseObjectPartHistoryPrintExtended(PartHistory);
                PartHistorylist.Add(tmpPartHistory);
            }
            this.listOfPartHistory.AddRange(PartHistorylist);

            this.listOfOtherCosts = new List<OtherCosts>();
            foreach (b_OtherCosts OtherCosts in trans.WorkOrder.listOfOtherCosts)
            {
                OtherCosts tmpOtherCosts = new OtherCosts();

                tmpOtherCosts.UpdateFromDatabaseObjectOtherCostsPrintExtended(OtherCosts);
                OtherCostslist.Add(tmpOtherCosts);
            }
            this.listOfOtherCosts.AddRange(OtherCostslist);

            this.listOfSummery = new List<OtherCosts>();
            foreach (b_OtherCosts OtherCostsSummery in trans.WorkOrder.listOfSummery)
            {
                OtherCosts tmpOtherCostsSummery = new OtherCosts();

                tmpOtherCostsSummery.UpdateFromDatabaseObjectOtherCostsPrintExtended(OtherCostsSummery);
                OtherCostsSummerylist.Add(tmpOtherCostsSummery);
            }
            this.listOfSummery.AddRange(OtherCostsSummerylist);

            this.listOfInstructions = new List<Instructions>();
            foreach (b_Instructions objInstructions in trans.WorkOrder.listOfInstructions)
            {
                Instructions tmpInstructions = new Instructions();
                tmpInstructions.UpdateFromDatabaseObjectInstructionsPrintExtended(objInstructions);
                instructions.Add(tmpInstructions);
            }
            this.listOfInstructions.AddRange(instructions);

            #region V2-944
            this.listOfWorkOrderUDF = new List<WorkOrderUDF>();
            foreach (b_WorkOrderUDF objworkorderudf in trans.WorkOrder.listOfWorkOrderUDF)
            {
                WorkOrderUDF tmpworkorderudf = new WorkOrderUDF();

                tmpworkorderudf.UpdateFromDatabaseObjectPrintWorkOrderUDFExtended(objworkorderudf, TimeZone);
                workOrderUDFlist.Add(tmpworkorderudf);
            }
            this.listOfWorkOrderUDF.AddRange(workOrderUDFlist);

            this.listOfWorkOrderSchedule = new List<WorkOrderSchedule>();
            foreach (b_WorkOrderSchedule objworkorderschedule in trans.WorkOrder.listOfWorkOrderSchedule)
            {
                WorkOrderSchedule tmpworkorderschedule = new WorkOrderSchedule();

                tmpworkorderschedule.UpdateFromDatabaseObjectPrintWorkOrderScheduleExtended(objworkorderschedule);
                workOrderSchedulelist.Add(tmpworkorderschedule);
            }
            this.listOfWorkOrderSchedule.AddRange(workOrderSchedulelist);
            #endregion

            return this;
        }

        #region V2-1051
        public void CreateWOModel(DatabaseKey dbKey)
        {
            WorkOrder_CreateWOModel_V2 trans = new WorkOrder_CreateWOModel_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDatabaseForWOModel();

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObjectForWOModel(trans.WorkOrder);
        }
        public b_WorkOrder ToDatabaseForWOModel()
        {
            b_WorkOrder dbObj = new b_WorkOrder();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.WorkOrderId = this.WorkOrderId;
            dbObj.Creator_PersonnelId = this.Creator_PersonnelId;
            dbObj.CreatedWorkOrderId = this.CreatedWorkOrderId;

            return dbObj;
        }
        public void UpdateFromDatabaseObjectForWOModel(b_WorkOrder dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.WorkOrderId = dbObj.WorkOrderId;
            this.SiteId = dbObj.SiteId;
            this.CreatedWorkOrderId = dbObj.CreatedWorkOrderId;
        }
        #endregion

        #region WorkOrder Lookup List  V2-1031

        public List<WorkOrder> GetAllWorkOrderLookupListForPartIssueV2(DatabaseKey dbKey)
        {
            WorkOrder_RetrieveChunkSearchLookupListForIssueParts_V2 trans = new WorkOrder_RetrieveChunkSearchLookupListForIssueParts_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };

            trans.WorkOrder = this.ToDateBaseObjectForWorkOrderLookupListForIssuePartsChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfWO = new List<WorkOrder>();

            List<WorkOrder> Workorderlist = new List<WorkOrder>();

            foreach (b_WorkOrder workorder in trans.WorkOrderList)
            {
                WorkOrder tmpWorkOrder = new WorkOrder();
                tmpWorkOrder.UpdateFromDatabaseObjectForWorkOrderLookupListForPartsIssueChunkSearch(workorder);
                Workorderlist.Add(tmpWorkOrder);
            }
            return Workorderlist;
        }


        public void UpdateFromDatabaseObjectForWorkOrderLookupListForPartsIssueChunkSearch(b_WorkOrder dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.WorkAssigned_Name = dbObj.WorkAssigned_Name;
            this.Requestor_Name = dbObj.Requestor_Name;
            this.TotalCount = dbObj.TotalCount;
        }


        public b_WorkOrder ToDateBaseObjectForWorkOrderLookupListForIssuePartsChunkSearch()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();

            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.Description = this.Description;
            dbObj.ClientId = this.ClientId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Description = this.Description;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.WorkAssigned_Name = this.WorkAssigned_Name;
            dbObj.Requestor_Name = this.Requestor_Name;
            dbObj.Status = this.Status;
            dbObj.SiteId = this.SiteId;

            return dbObj;
        }

        #endregion

        #region V2-1087 ProjectCosting Dashboard
        public List<KeyValuePair<string, long>> WorkOrderStatusesCountForDashboard_V2(DatabaseKey dbKey)
        {
            WorkOrderStatusCountForDashboard_V2 trans = new WorkOrderStatusCountForDashboard_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.workOrder = ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return trans.Entries;
        }
        #endregion

        #region Retrieve WorkOrderId By ClientLookupId V2-1178
        public WorkOrder RetrieveWorkOrderIdByClientLookupIdV2(DatabaseKey dbKey)
        {
            WorkOrder_RetrieveWorkOrderIdbyClientLookupId_V2 trans = new WorkOrder_RetrieveWorkOrderIdbyClientLookupId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForRetrieveWorkOrderIdIdByClientLookupIdV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            WorkOrder tmpworkorder = new WorkOrder()
            {
                WorkOrderId = trans.WorkOrderResult.WorkOrderId,
                ClientLookupId = trans.WorkOrderResult.ClientLookupId,
            };

            return tmpworkorder;
        }
        public b_WorkOrder ToDateBaseObjectForRetrieveWorkOrderIdIdByClientLookupIdV2()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();
            dbObj.ClientLookupId = this.ClientLookupId;

            return dbObj;
        }
        #endregion

        #region V2-1177
        public WorkOrder AnalyticsWOStatusDashboardV2(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrder_AnalyticsWOStatusDashboardV2 trans = new WorkOrder_AnalyticsWOStatusDashboardV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrder = this.ToDateBaseObjectForAnalyticsWOStatusDashboard();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfWO = new List<WorkOrder>();

            List<WorkOrder> workorderlist = new List<WorkOrder>();
            foreach (b_WorkOrder workorder in trans.WorkOrder.listOfWO)
            {
                WorkOrder tmpworkorder = new WorkOrder();

                tmpworkorder.UpdateFromDatabaseObjectForAnalyticsWOStatusDashboard(workorder, TimeZone);
                workorderlist.Add(tmpworkorder);
            }
            this.listOfWO.AddRange(workorderlist);
            return this;
        }
        public b_WorkOrder ToDateBaseObjectForAnalyticsWOStatusDashboard()
        {
            b_WorkOrder dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForAnalyticsWOStatusDashboard(b_WorkOrder dbObj, string TimeZone)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.WorkOrderId = dbObj.WorkOrderId;
            this.ClientLookupId = dbObj.ClientLookupId;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CompleteDate = dbObj.CompleteDate;
            }
            this.Status = dbObj.Status;
            this.Type = dbObj.Type;
            this.Type = dbObj.Type;
            this.Priority = dbObj.Priority;
            this.SourceType = dbObj.SourceType;
            this.ActualMaterialCosts = dbObj.ActualMaterialCosts;
            this.LaborCost = dbObj.LaborCost;
            this.ActualLaborHours = dbObj.ActualLaborHours;
            this.TotalCost = dbObj.TotalCost;
        }
        #endregion

    }
}


