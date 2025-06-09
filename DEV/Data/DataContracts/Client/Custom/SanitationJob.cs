/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015-2018 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person        Description
* =========== ======== ============== ==============================================================
* 2015-Mar-21 SOM-585  Roger Lawton   Changed Parameters
* 2015-Mar-24 SOM-585  Roger Lawton   Localized the Status
* 2018-Jun-20 SOM-1628 Roger Lawton   Add new method to update sanitation job from external api
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database.Business;
using Database;
using Common.Extensions;

namespace DataContracts
{
    public partial class SanitationJob : DataContractBase, IStoredProcedureValidation
    {
        #region validate types
        private const string Validation_Type_Complete = "Complete";
        private const string Validation_Type_Interface = "Interface";     // SOM-1628
        #endregion
        #region Property
        public string ValidateType { get; set; }
        public string Assigned { get; set; }
        public string OnDemandGroup { get; set; }
        public string CompleteBy { get; set; }
        public string ShiftDescription { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public long PlantLocationId { get; set; }
        public string DisplayTextChecked { get; set; }

        public string Status_Display { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string Prefix { get; set; }
        public long PersonnelId { get; set; }
        public int SanitationMasterCount { get; set; }
        public int SanitationJobCount { get; set; }
        public string SanitationJobList { get; set; }
        public string PersonnelIdList { get; set; }
        public DateTime? ScheduledDateFrom { get; set; }
        public DateTime? ScheduledDateTo { get; set; }
        public string Creator_PersonnelClientLookupId { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string VerificationCompleteDate { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string CheckStatus { get; set; }
        public string CreateByName { get; set; }
        public string PassBy { get; set; }
        public string FailBy { get; set; }

        public string SourceIDClientLookUpId { get; set; }
        public string FailDescription { get; set; }

        [DataMember]
        public string ImageURI { get; set; }

        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public int offset1 { get; set; }
        public int nextrow { get; set; }
        public string ChargeTo { get; set; }
        public string ChargeToName { get; set; }
        public string AssignedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CompletedDate { get; set; }
        public string VerifyDate { get; set; }
        public string ScheduleDate { get; set; }
        public string SearchText { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public Int32 ChildCount { get; set; }
        public int TotalCount { get; set; }
        public string AssetGroup1_ClientLookUpId { get; set; }
        public string AssetGroup2_ClientLookUpId { get; set; }
        public string AssetGroup3_ClientLookUpId { get; set; }
        public List<b_SanitationJob> ListSanJob { get; set; }
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string CompleteStartDateVw { get; set; }
        public string CompleteEndDateVw { get; set; }
        public string FailedStartDateVw { get; set; }
        public string FailedEndDateVw { get; set; }
        public string PassedStartDateVw { get; set; }
        public string PassedEndDateVw { get; set; }
        public string AssetLocation { get; set; }
        public long SanMasterBatchEntryId { get; set; }
        public string SanitationJobIDList { get; set; } //V2-1071
        public List<SanitationJob> listOfSJ { get; set; }
        public List<SanitationPlanning> listOfSanitationTool { get; set; }
        public List<SanitationPlanning> listOfSanitationChemical { get; set; }
        public List<SanitationJobTask> listOfSanitationTask { get; set; }
        public List<Timecard> listOfTimecard { get; set; }
        public List<SanitationJob> listOfCompletion { get; set; }
        public List<SanitationJob> listOfVerification { get; set; }
        public long LoggedInUserPEID { get; set; }//V2-1101
        #endregion

        public List<SanitationJob> RetrieveAll(DatabaseKey dbKey)
        {
            Database.SanitationJob_RetrieveAll trans = new Database.SanitationJob_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SanitationJobList);
        }


        public static List<SanitationJob> UpdateFromDatabaseObjectList(List<b_SanitationJob> dbObjs)
        {
            List<SanitationJob> result = new List<SanitationJob>();

            foreach (b_SanitationJob dbObj in dbObjs)
            {
                SanitationJob tmp = new SanitationJob();
                tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public static List<SanitationJob> UpdateFromDatabaseObjectListForExtraction(List<b_SanitationJob> dbObjs)
        {
            List<SanitationJob> result = new List<SanitationJob>();

            foreach (b_SanitationJob dbObj in dbObjs)
            {
                SanitationJob tmp = new SanitationJob();
                tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseObjectExtended(b_SanitationJob dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ShiftDescription = dbObj.ShiftDescription;
            this.Assigned = dbObj.Assigned;
            this.CompleteBy = dbObj.CompleteBy;
            this.OnDemandGroup = dbObj.OnDemandGroup;
            this.ChargeTo_ClientLookupId = dbObj.ChargeTo_ClientLookupId;
            this.CreateBy = dbObj.CreateBy;
            this.CreateDate = dbObj.CreateDate;
            this.PassBy = dbObj.PassBy;
            this.FailBy = dbObj.FailBy;
            this.SanitationJobCount = dbObj.SanitationJobCount;
            this.Status = dbObj.Status;
            switch (this.Status)
            {
                case Common.Constants.SanitationJobConstant.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.SanitationJobConstant.Canceled:
                    this.Status_Display = "Canceled";
                    break;
                case Common.Constants.SanitationJobConstant.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.SanitationJobConstant.Denied:
                    this.Status_Display = "Denied";
                    break;
                case Common.Constants.SanitationJobConstant.Fail:
                    this.Status_Display = "Fail";
                    break;
                case Common.Constants.SanitationJobConstant.JobRequest:
                    this.Status_Display = "JobRequest";
                    break;
                case Common.Constants.SanitationJobConstant.Open:
                    this.Status_Display = "Open";
                    break;
                case Common.Constants.SanitationJobConstant.Pass:
                    this.Status_Display = "Pass";
                    break;
                case Common.Constants.SanitationJobConstant.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }


        }

        public void UpdateFromDatabaseObjectExtendedRetrieveV2(b_SanitationJob dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ShiftDescription = dbObj.ShiftDescription;
            this.Assigned = dbObj.Assigned;
            this.CompleteBy = dbObj.CompleteBy;
            this.OnDemandGroup = dbObj.OnDemandGroup;
            this.ChargeTo_ClientLookupId = dbObj.ChargeTo_ClientLookupId;
            this.CreateBy = dbObj.CreateBy;
            this.CreateDate = dbObj.CreateDate;
            this.PassBy = dbObj.PassBy;
            this.FailBy = dbObj.FailBy;
            this.SanitationJobCount = dbObj.SanitationJobCount;
            this.Status = dbObj.Status;
            this.AssetLocation = dbObj.AssetLocation;
            switch (this.Status)
            {
                case Common.Constants.SanitationJobConstant.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.SanitationJobConstant.Canceled:
                    this.Status_Display = "Canceled";
                    break;
                case Common.Constants.SanitationJobConstant.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.SanitationJobConstant.Denied:
                    this.Status_Display = "Denied";
                    break;
                case Common.Constants.SanitationJobConstant.Fail:
                    this.Status_Display = "Fail";
                    break;
                case Common.Constants.SanitationJobConstant.JobRequest:
                    this.Status_Display = "JobRequest";
                    break;
                case Common.Constants.SanitationJobConstant.Open:
                    this.Status_Display = "Open";
                    break;
                case Common.Constants.SanitationJobConstant.Pass:
                    this.Status_Display = "Pass";
                    break;
                case Common.Constants.SanitationJobConstant.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }
            this.FailReason = dbObj.FailDescription;
            this.SourceIDClientLookUpId = dbObj.SourceIDClientLookUpId;

        }
        public b_SanitationJob ToDatabaseObjectExtended()
        {
            b_SanitationJob dbObj = this.ToDatabaseObject();
            dbObj.ShiftDescription = this.Shift;
            dbObj.Assigned = this.Assigned;
            dbObj.CompleteBy = this.CompleteBy;
            dbObj.OnDemandGroup = this.OnDemandGroup;
            dbObj.ChargeTo_ClientLookupId = this.ChargeTo_ClientLookupId;
            dbObj.CreateBy = this.CreateBy;
            dbObj.CreateDate = this.CreateDate;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.PlantLocationId = this.PlantLocationId;//SOM-1348
            dbObj.Prefix = this.Prefix;
            return dbObj;
        }

        public long SanitationJob_CustomCreate
            (DatabaseKey dbKey
            , long SanitationJobBatchEntryId
            , string ClientLookupId,
            long Requestor_PersonnelId)
        {
            SanitationJobTransactions_CustomCreate trans = new SanitationJobTransactions_CustomCreate
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.SanitationJob = new b_SanitationJob();
            trans.SanitationJobBatchEntryId = SanitationJobBatchEntryId;
            //trans.SanitationJob.ClientLookupId = ClientLookupId;
            //trans.SanitationJob.Requestor_PersonnelId = Requestor_PersonnelId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return trans.SanitationJob.SanitationJobId;

        }

        public List<SanitationJob> Retrieve_SanitationVerificationWorkBench_ByFilterCriteria(DatabaseKey dbKey, string Timezone)
        {
            SanitationJob_RetrieveAllForVerificationWorkBench trans = new SanitationJob_RetrieveAllForVerificationWorkBench()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanitationJob = this.ToDateBaseObjectForRetriveWorkbenchForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<SanitationJob> sanitationJobList = new List<SanitationJob>();
            foreach (b_SanitationJob sanitationJob in trans.SanitationVerificationJobList)
            {
                SanitationJob tmpsanitationJob = new SanitationJob();
                tmpsanitationJob.UpdateFromDatabaseObjectForVerificationWorkBench(sanitationJob, Timezone);
                sanitationJobList.Add(tmpsanitationJob);
            }
            return sanitationJobList;
        }
        public void UpdateFromDatabaseObjectForVerificationWorkBench(b_SanitationJob dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ChargeTo_ClientLookupId = dbObj.ChargeTo_ClientLookupId;
            this.Assigned_PersonnelClientLookupId = dbObj.Assigned_PersonnelClientLookupId;
            this.CreateBy_PersonnelId = dbObj.CreateBy_PersonnelId;

            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CompleteDate = dbObj.CompleteDate;
            }
        }
        public List<SanitationJob> SanitationJob_RetieveBySearchCriteria(DatabaseKey dbKey)
        {
            SanitationJobTransactions_RetieveBySearchCriteria trans = new SanitationJobTransactions_RetieveBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.SanitationJob = new b_SanitationJob();
            trans.SanitationJob.SiteId = this.SiteId;
            trans.SanitationJob.DepartmentId = this.DepartmentId;
            trans.SanitationJob.AssignedTo_PersonnelId = this.AssignedTo_PersonnelId;
            trans.SanitationJob.ScheduledDate = this.ScheduledDate;
            trans.SanitationJob.Shift = this.Shift;
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SanitationJobList);
        }


        public void SanitationJob_UpdateForComplete(DatabaseKey dbKey, long CallerUserPersonnelId)
        {
            this.ValidateType = Validation_Type_Complete;

            Validate<SanitationJob>(dbKey);

            if (IsValid)
            {
                SanitationJobTransactions_UpdateForComplete trans = new SanitationJobTransactions_UpdateForComplete();
                trans.SanitationJob = this.ToDatabaseObjectExtended();
                trans.CallerUserPersonnelId = CallerUserPersonnelId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                //The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.SanitationJob);
            }
        }
        //SOM-1628
        public void SanitationJob_UpdateForInterface(DatabaseKey dbKey)
        {
            this.ValidateType = Validation_Type_Interface;

            Validate<SanitationJob>(dbKey);

            if (IsValid)
            {
                SanitationJobTransactions_UpdateForInterface trans = new SanitationJobTransactions_UpdateForInterface();
                trans.SanitationJob = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.SanitationJob);
            }
        }

        // RKL - SOM-585 - THIS DOES NOT APPEAR TO BE USED ANYWHERE
        public void SanitationJob_UpdateForReschedule(DatabaseKey dbKey)
        {

            SanitationJobTransactions_UpdateForReschedule trans = new SanitationJobTransactions_UpdateForReschedule()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };

            trans.SanitationJob = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.SanitationJob);
        }
        public void SanitationJob_UpdateForCancel(DatabaseKey dbKey, long CallerUserPersonnelId)
        {
            SanitationJobTransactions_UpdateForCancel trans = new SanitationJobTransactions_UpdateForCancel()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanitationJob = this.ToDatabaseObject();
            trans.CallerUserPersonnelId = CallerUserPersonnelId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.SanitationJob);
        }
        public void SanitationJob_UpdateForReopen(DatabaseKey dbKey)
        {
            SanitationJobTransactions_UpdateForReopen trans = new SanitationJobTransactions_UpdateForReopen()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };

            trans.SanitationJob = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.SanitationJob);
        }
        public List<SanitationJob> SanitationJob_RetrieveAllByFk(DatabaseKey dbKey, DateTime Date, long PersonalId, string Shift, bool ShowCompleted)
        {
            SanitationJobTransactions_RetieveAllByFk trans = new SanitationJobTransactions_RetieveAllByFk()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.SanitationJob = new b_SanitationJob();
            trans.SanitationJob.AssignedTo_PersonnelId = PersonalId;
            trans.SanitationJob.ScheduledDate = Date;
            trans.SanitationJob.Shift = Shift;
            trans.SanitationJob.ShowCompleted = ShowCompleted;
            trans.SanitationJob.SiteId = this.SiteId;
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SanitationJobList);
        }
        public void RetrieveByPKForeignKeys(DatabaseKey dbKey)
        {

            SanitationJobTransactions_RetrieveByForeignKeys trans = new SanitationJobTransactions_RetrieveByForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.SanitationJob = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtended(trans.SanitationJob);

        }

        public void RetrieveByV2(DatabaseKey dbKey)
        {

            SanitationJobTransactions_RetrieveByV2 trans = new SanitationJobTransactions_RetrieveByV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.SanitationJob = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtendedRetrieveV2(trans.SanitationJob);

        }
        public List<SanitationJob> RetrieveForExtraction(DatabaseKey dbKey)
        {
            SanitationJobTransactions_RetrieveForExtraction trans = new SanitationJobTransactions_RetrieveForExtraction()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SanitationJob = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectListForExtraction(trans.SanitationJobList);
        }
        #region validation
        public void ValidateAdd(DatabaseKey dbKey)
        {
            Validate<SanitationJob>(dbKey);
        }
        #endregion

        //Sanitation Job Generation  SOM - 598
        public void SanitationJob_GenerationReport(DatabaseKey dbKey)
        {

            SanitationJobTransactions_GenerationReport trans = new SanitationJobTransactions_GenerationReport
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanitationJob = this.ToDatabaseObject();
            trans.SanitationJob = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.SanitationMasterCount = trans.SanitationJob.SanitationMasterCount;
            this.SanitationJobCount = trans.SanitationJob.SanitationJobCount;
            this.SanitationJobList = trans.SanitationJob.SanitationJobList;
        }
        public void SanitationJob_GenerationReportOnDemand(DatabaseKey dbKey)
        {

            SanitationJobTransactions_GenerationReportOnDemand trans = new SanitationJobTransactions_GenerationReportOnDemand
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanitationJob = this.ToDatabaseObject();
            trans.SanitationJob = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.SanitationMasterCount = trans.SanitationJob.SanitationMasterCount;
            this.SanitationJobCount = trans.SanitationJob.SanitationJobCount;
            this.SanitationJobList = trans.SanitationJob.SanitationJobList;
        }
        //Sanitation Job Print SOM - 720
        //public void SanitationJob_PrintSanitationJobReport(DatabaseKey dbKey, Business.Localization.Global loc)
        //{
        //    SanitationJobTransactions_PrintSanitationJobReport trans = new SanitationJobTransactions_PrintSanitationJobReport
        //    {
        //        CallerUserInfoId = dbKey.User.UserInfoId,
        //        CallerUserName = dbKey.UserName,
        //    };
        //    trans.SanitationJob = this.ToDatabaseObject();
        //    trans.SanitationJob.PersonnelIdList = this.PersonnelIdList;
        //    trans.SanitationJob.ScheduledDateFrom = this.ScheduledDateFrom;
        //    trans.SanitationJob.ScheduledDateTo = this.ScheduledDateTo;
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.Execute();
        //    this.SanitationJobList = trans.SanitationJob.SanitationJobList;
        //}

        public List<SanitationJob> Retrieve_SanitationJobSearch_ByFilterCriteria(DatabaseKey dbKey, string Timezone)
        {
            SanitationJobSearch_RetrieveAllForSearch trans = new SanitationJobSearch_RetrieveAllForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.SanitationJob = this.ToDBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            List<SanitationJob> sanitationJobList = new List<SanitationJob>();
            foreach (b_SanitationJob sanitationJob in trans.SanitationJobList)
            {
                SanitationJob tmpsanitationJob = new SanitationJob();

                tmpsanitationJob.UpdateFromDatabaseObjectForRetriveAllForSearch(sanitationJob, Timezone);
                sanitationJobList.Add(tmpsanitationJob);
            }
            return sanitationJobList;
        }

        public List<SanitationJob> Retrieve_ChunkSearch(DatabaseKey dbKey, string Timezone)
        {
            SanitationJobSearch_RetrieveChunkSearch trans = new SanitationJobSearch_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.SanitationJob = this.ToDBaseObjectForRetriveAllForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            List<SanitationJob> sanitationJobList = new List<SanitationJob>();

            foreach (b_SanitationJob sanitationJob in trans.SanitationJobList)
            {
                SanitationJob tmpsanitationJob = new SanitationJob();

                tmpsanitationJob.UpdateFromDatabaseObjectForRetriveAllForChunkSearch(sanitationJob, Timezone);
                sanitationJobList.Add(tmpsanitationJob);
            }
            return sanitationJobList;
        }

        public b_SanitationJob ToDBaseObjectForRetriveAllForSearch()
        {
            b_SanitationJob dbObj = this.ToDatabaseObject();
            dbObj.ChargeTo_ClientLookupId = this.ChargeTo_ClientLookupId;
            dbObj.CreateDate = this.CreateDate;
            dbObj.CreateByName = this.CreateBy;
            dbObj.VerifiedBy = this.VerifiedBy;
            //dbObj.Assigned = this.Assigned;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.ChargeTo = this.ChargeTo;
            dbObj.ChargeToName = this.ChargeToName;
            //dbObj.AssignedBy = this.AssignedBy;
            dbObj.AssignedBy = this.Assigned;
            dbObj.CreatedDate = this.CreatedDate;
            dbObj.CompletedDate = this.CompletedDate;
            dbObj.ScheduleDate = this.ScheduleDate;
            dbObj.VerifyDate = this.VerifyDate;
            dbObj.AssetGroup1_ClientLookUpId = this.AssetGroup1_ClientLookUpId;
            dbObj.AssetGroup2_ClientLookUpId = this.AssetGroup2_ClientLookUpId;
            dbObj.AssetGroup3_ClientLookUpId = this.AssetGroup3_ClientLookUpId;
            dbObj.ChildCount = this.ChildCount;
            dbObj.TotalCount = this.TotalCount;
            dbObj.SearchText = this.SearchText;
            //V2-398
            dbObj.CreateStartDateVw = this.CreateStartDateVw;
            dbObj.CreateEndDateVw = this.CreateEndDateVw;
            dbObj.CompleteStartDateVw = this.CompleteStartDateVw;
            dbObj.CompleteEndDateVw = this.CompleteEndDateVw;
            dbObj.FailedStartDateVw = this.FailedStartDateVw;
            dbObj.FailedEndDateVw = this.FailedEndDateVw;
            dbObj.PassedStartDateVw = this.PassedStartDateVw;
            dbObj.PassedEndDateVw = this.PassedEndDateVw;

            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_SanitationJob dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ChargeTo_ClientLookupId = dbObj.ChargeTo_ClientLookupId;
            this.CreateDate = dbObj.CreateDate;
            this.CreateByName = dbObj.CreateByName;
            this.AssetGroup1_ClientLookUpId = dbObj.AssetGroup1_ClientLookUpId;
            this.AssetGroup2_ClientLookUpId = dbObj.AssetGroup2_ClientLookUpId;
            this.AssetGroup3_ClientLookUpId = dbObj.AssetGroup3_ClientLookUpId;
            this.ChildCount = dbObj.ChildCount;
            this.TotalCount = dbObj.TotalCount;
            // SOM-706 - Convert the create date to the user's time zone
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            // V2-222 - Convert the CompleteDate to the user's time zone
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CompleteDate = dbObj.CompleteDate;
            }
            // SOM-1546 
            Assigned = dbObj.Assigned;
            VerifiedBy = dbObj.VerifiedBy;
            VerifiedDate = dbObj.VerifiedDate;
            if (VerifiedDate != null && VerifiedDate != DateTime.MinValue)
            {
                VerifiedDate = VerifiedDate.ToUserTimeZone(TimeZone);
            }
            switch (this.Status)
            {
                case Common.Constants.SanitationJobConstant.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.SanitationJobConstant.Open:
                    this.Status_Display = "Open";
                    break;
                case Common.Constants.SanitationJobConstant.Canceled:
                    this.Status_Display = "Canceled";
                    break;
                case Common.Constants.SanitationJobConstant.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.SanitationJobConstant.Request:
                    this.Status_Display = "Request";
                    break;
                case Common.Constants.SanitationJobConstant.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }

        }

        public b_SanitationJob ToDBaseObjectForRetriveAllForChunkSearch()
        {
            b_SanitationJob dbObj = this.ToDatabaseObject();
            dbObj.ChargeTo_ClientLookupId = this.ChargeTo_ClientLookupId;
            dbObj.CreateDate = this.CreateDate;
            dbObj.CreateByName = this.CreateBy;
            dbObj.VerifiedBy = this.VerifiedBy;
            //dbObj.Assigned = this.Assigned;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.ChargeTo = this.ChargeTo;
            dbObj.ChargeToName = this.ChargeToName;
            dbObj.AssetLocation = this.AssetLocation;
            //dbObj.AssignedBy = this.AssignedBy;
            dbObj.AssignedBy = this.Assigned;
            dbObj.CreatedDate = this.CreatedDate;
            dbObj.CompletedDate = this.CompletedDate;
            dbObj.ScheduleDate = this.ScheduleDate;
            dbObj.VerifyDate = this.VerifyDate;
            dbObj.AssetGroup1_ClientLookUpId = this.AssetGroup1_ClientLookUpId;
            dbObj.AssetGroup2_ClientLookUpId = this.AssetGroup2_ClientLookUpId;
            dbObj.AssetGroup3_ClientLookUpId = this.AssetGroup3_ClientLookUpId;
            dbObj.ChildCount = this.ChildCount;
            dbObj.TotalCount = this.TotalCount;
            dbObj.SearchText = this.SearchText;
            //V2-398
            dbObj.CreateStartDateVw = this.CreateStartDateVw;
            dbObj.CreateEndDateVw = this.CreateEndDateVw;
            dbObj.CompleteStartDateVw = this.CompleteStartDateVw;
            dbObj.CompleteEndDateVw = this.CompleteEndDateVw;
            dbObj.FailedStartDateVw = this.FailedStartDateVw;
            dbObj.FailedEndDateVw = this.FailedEndDateVw;
            dbObj.PassedStartDateVw = this.PassedStartDateVw;
            dbObj.PassedEndDateVw = this.PassedEndDateVw;
            //V2-1101
            dbObj.LoggedInUserPEID = this.LoggedInUserPEID;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetriveAllForChunkSearch(b_SanitationJob dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ChargeTo_ClientLookupId = dbObj.ChargeTo_ClientLookupId;
            this.CreateDate = dbObj.CreateDate;
            this.CreateByName = dbObj.CreateByName;
            this.AssetGroup1_ClientLookUpId = dbObj.AssetGroup1_ClientLookUpId;
            this.AssetGroup2_ClientLookUpId = dbObj.AssetGroup2_ClientLookUpId;
            this.AssetGroup3_ClientLookUpId = dbObj.AssetGroup3_ClientLookUpId;
            this.ChildCount = dbObj.ChildCount;
            this.TotalCount = dbObj.TotalCount;
            // SOM-706 - Convert the create date to the user's time zone
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            // V2-222 - Convert the CompleteDate to the user's time zone
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CompleteDate = dbObj.CompleteDate;
            }
            // SOM-1546 
            Assigned = dbObj.Assigned;
            VerifiedBy = dbObj.VerifiedBy;
            VerifiedDate = dbObj.VerifiedDate;
            if (VerifiedDate != null && VerifiedDate != DateTime.MinValue)
            {
                VerifiedDate = VerifiedDate.ToUserTimeZone(TimeZone);
            }
            switch (this.Status)
            {
                case Common.Constants.SanitationJobConstant.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.SanitationJobConstant.Open:
                    this.Status_Display = "Open";
                    break;
                case Common.Constants.SanitationJobConstant.Canceled:
                    this.Status_Display = "Canceled";
                    break;
                case Common.Constants.SanitationJobConstant.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.SanitationJobConstant.Request:
                    this.Status_Display = "Request";
                    break;
                case Common.Constants.SanitationJobConstant.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }
            this.AssetLocation = dbObj.AssetLocation;
            //V2-910
            PassDate = dbObj.PassDate;
            if (PassDate != null && PassDate != DateTime.MinValue)
            {
                PassDate = PassDate.ToUserTimeZone(TimeZone);
            }
            FailDate = dbObj.FailDate;
            if (FailDate != null && FailDate != DateTime.MinValue)
            {
                FailDate = VerifiedDate.ToUserTimeZone(TimeZone);
            }
        }
        //=================
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (ValidateType == "ADD")//Validate For Add
            {
                SanitationJob_Validate trans = new SanitationJob_Validate
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.ValidateType = this.ValidateType;
                trans.SanitationJob = this.ToDatabaseObjectExtended();
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
            else if (ValidateType == "UPDATE") //Validate For Update
            {
                SanitationJob_Validate_ForUpdate_ByClientlookupIdAndPersonnelId trans = new SanitationJob_Validate_ForUpdate_ByClientlookupIdAndPersonnelId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.SanitationJob = this.ToDBaseObjectFor_Update();
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
            else if (ValidateType == "FAIL") //Validate For FAIL
            {
                SanitationJob_Validate_ForUpdate_ByClientlookupIdAndPersonnelId_V2 trans = new SanitationJob_Validate_ForUpdate_ByClientlookupIdAndPersonnelId_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.SanitationJob = this.ToDBaseObjectFor_Update();
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
            else if (ValidateType == "Complete") //Validate For Update
            {
                SanitationJob_Validate_ForUpdate_ByClientlookupIdAndPersonnelId trans = new SanitationJob_Validate_ForUpdate_ByClientlookupIdAndPersonnelId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.SanitationJob = this.ToDBaseObjectFor_Update();
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
            // SOM-1628
            else if (ValidateType == Validation_Type_Interface) //Validate For Interface
            {
                SanitationJob_Validate_ForInterface trans = new SanitationJob_Validate_ForInterface()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.SanitationJob = this.ToDatabaseObject();
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
                // Validate Type 
                SanitationJob_Validate trans = new SanitationJob_Validate
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.ValidateType = this.ValidateType;
                trans.SanitationJob = this.ToDatabaseObjectExtended();
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

            ValidateType = "";

            return errors;
        }
        //===============
        public void SanitationJob_CreateByFk(DatabaseKey dbKey)
        {
            this.ValidateType = "ADD";

            Validate<SanitationJob>(dbKey);
            if (IsValid)
            {
                SanitationJobTransactions_CreateByFk trans = new SanitationJobTransactions_CreateByFk
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.SanitationJob = new b_SanitationJob();
                trans.SanitationJob = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
            }

        }


        //=============
        public void UpdateByPK_ForeignKeys(DatabaseKey dbKey)
        {
            this.ValidateType = "UPDATE";

            Validate<SanitationJob>(dbKey);

            if (IsValid)
            {
                SanitationJob_UpdateByPK_ForeignKeys trans = new SanitationJob_UpdateByPK_ForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.SanitationJob = this.ToDBaseObjectFor_Update();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.SanitationJob);
            }
        }
        //========
        public b_SanitationJob ToDBaseObjectFor_Update()
        {
            b_SanitationJob dbObj = this.ToDatabaseObject();
            dbObj.ChargeTo_ClientLookupId = this.ChargeTo_ClientLookupId;
            dbObj.PlantLocationId = this.PlantLocationId;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.Creator_PersonnelClientLookupId = this.Creator_PersonnelClientLookupId;
            dbObj.Creator_PersonnelId = this.Creator_PersonnelId;
            dbObj.CheckStatus = this.CheckStatus;
            return dbObj;
        }
        //=======
        public void RetrieveBy_SanitationJobId(DatabaseKey dbKey)
        {
            SanitationJobTransactions_RetrieveBy_SanitationJobId trans = new SanitationJobTransactions_RetrieveBy_SanitationJobId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.SanitationJob = this.ToDBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObjectForRetriveAllForSearch(trans.SanitationJob);

        }
        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_SanitationJob dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ChargeTo_ClientLookupId = dbObj.ChargeTo_ClientLookupId;
            this.CreateBy_Name = dbObj.CreateBy_Name;
            this.CompleteDate = dbObj.CompleteDate;
            this.CompleteBy = dbObj.CompleteBy;
            this.Assigned = dbObj.Assigned;
            this.CreateDate = dbObj.CreateDate;
            this.PassBy = dbObj.PassBy;
            this.FailBy = dbObj.FailBy;
            switch (this.Status)
            {
                case Common.Constants.SanitationJobConstant.Approved:
                    this.Status_Display = "Approved";
                    break;
                case Common.Constants.SanitationJobConstant.Open:
                    this.Status_Display = "Open";
                    break;
                case Common.Constants.SanitationJobConstant.Canceled:
                    this.Status_Display = "Canceled";
                    break;
                case Common.Constants.SanitationJobConstant.Complete:
                    this.Status_Display = "Complete";
                    break;
                case Common.Constants.SanitationJobConstant.Request:
                    this.Status_Display = "Request";
                    break;
                case Common.Constants.SanitationJobConstant.Scheduled:
                    this.Status_Display = "Scheduled";
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }

        }

        public string CreateBy_Name { get; set; }

        public string Assigned_PersonnelClientLookupId { get; set; }
        [DataMember]
        public bool CompleteAllTasks { get; set; }

        #region  ::  Approve WorkBench Work  ::
        public string ApproveStatusDrop { get; set; }
        public string ApproveCreatedDate { get; set; }
        public string CreateBy_PersonnelId { get; set; }
        public string ModifyBy { get; set; }
        public string ScheduleFlag { get; set; }
        public string ApproveFlag { get; set; }
        public string DeniedFlag { get; set; }
        public string ChartType { get; set; }
        public DateTime TooDay { get; set; }
        public int TimeFrame { get; set; }


        public List<SanitationJob> Retrieve_SanitationJobApproveWorkBench_ByFilterCriteria(DatabaseKey dbKey, string Timezone)
        {
            SanitationJob_RetrieveAllForApproveWorkBench trans = new SanitationJob_RetrieveAllForApproveWorkBench()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanitationJob = this.ToDateBaseObjectForRetriveWorkbenchForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<SanitationJob> sanitationJobList = new List<SanitationJob>();
            foreach (b_SanitationJob sanitationJob in trans.SanitationJobList)
            {
                SanitationJob tmpsanitationJob = new SanitationJob();
                tmpsanitationJob.UpdateFromDatabaseObjectForApproveWorkBench(sanitationJob, Timezone);
                sanitationJobList.Add(tmpsanitationJob);
            }
            return sanitationJobList;
        }
        public b_SanitationJob ToDateBaseObjectForRetriveWorkbenchForSearch()
        {
            b_SanitationJob dbObj = this.ToDatabaseObject();
            dbObj.ApproveCreatedDate = this.ApproveCreatedDate;
            dbObj.ApproveStatusDrop = this.ApproveStatusDrop;
            dbObj.VerificationCompleteDate = this.VerificationCompleteDate;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForApproveWorkBench(b_SanitationJob dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ChargeTo_ClientLookupId = dbObj.ChargeTo_ClientLookupId;
            this.Assigned_PersonnelClientLookupId = dbObj.Assigned_PersonnelClientLookupId;
            this.CreateBy_PersonnelId = dbObj.CreateBy_PersonnelId;
            this.CreateDate = dbObj.CreateDate;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
        }
        public void SanitationJob_UpdateFor_ApproveWorkBench(DatabaseKey dbKey)
        {
            SanitationJob_UpdateFor_ApproveWorkBench trans = new SanitationJob_UpdateFor_ApproveWorkBench()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanitationJob = this.ToDBaseObject_UpdateFor_ApproveWorkBench();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.SanitationJob);
        }

        public b_SanitationJob ToDBaseObject_UpdateFor_ApproveWorkBench()
        {
            b_SanitationJob dbObj = this.ToDatabaseObject();
            dbObj.ApproveFlag = this.ApproveFlag;
            dbObj.ScheduleFlag = this.ScheduleFlag;
            dbObj.DeniedFlag = this.DeniedFlag;
            //dbObj.ModifyBy = this.ModifyBy;
            //dbObj.CreateBy = this.CreateBy;
            return dbObj;
        }

        public List<SanitationJob> RetrieveDashboardChart(DatabaseKey dbKey, SanitationJob sj)
        {
            SanitationJob_RetrieveDashboardChart trans = new SanitationJob_RetrieveDashboardChart()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.SanitationJob = sj.ToDatabaseObjectDashBoardChart();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return SanitationJob.UpdateFromDatabaseObjectList(trans.SanitationJobList);
        }

        public b_SanitationJob ToDatabaseObjectDashBoardChart()
        {
            b_SanitationJob dbObj = new b_SanitationJob();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ChartType = this.ChartType;
            dbObj.TooDay = this.TooDay;
            dbObj.TimeFrame = this.TimeFrame;
            return dbObj;
        }

        public List<SanitationJob> SanitationJob_WRDashboardRetrieveBy_Filter_V2(DatabaseKey dbKey, string Timezone)
        {
            SanitationJob_WRDashboardRetrieveBy_Filter_V2 trans = new SanitationJob_WRDashboardRetrieveBy_Filter_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.SanitationJob = this.ToDBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            List<SanitationJob> sanitationJobList = new List<SanitationJob>();
            foreach (b_SanitationJob sanitationJob in trans.SanitationJobList)
            {
                SanitationJob tmpsanitationJob = new SanitationJob();

                tmpsanitationJob.UpdateFromDatabaseObjectForRetriveAllForSearch(sanitationJob, Timezone);
                sanitationJobList.Add(tmpsanitationJob);
            }
            return sanitationJobList;

        }
        #endregion
        #region V2-912
        public void SanitationJob_CreateByFk_V2(DatabaseKey dbKey)
        {
            this.ValidateType = "FAIL"; //validate the Sanitation Clientlookup and personnel id

            Validate<SanitationJob>(dbKey);
            if (IsValid)
            {
                SanitationJobTransactions_CreateByFk trans = new SanitationJobTransactions_CreateByFk
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.SanitationJob = new b_SanitationJob();
                trans.SanitationJob = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                // The create procedure may have populated an auto-incremented key. 
                this.UpdateFromDatabaseObject(trans.SanitationJob);
            }
        }

        #endregion

        #region V2-992
        public void SanitationJob_GenerationFromSanMasterBatchEntry_Days(DatabaseKey dbKey)
        {

            SanitationJobTransactions_GenerationSanMasterBatchEntry_Days trans = new SanitationJobTransactions_GenerationSanMasterBatchEntry_Days
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanitationJob = this.ToDatabaseObject();
            trans.SanitationJob = this.ToDatabaseObjectExtendedSanMasterBatchEntry();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.SanitationMasterCount = trans.SanitationJob.SanitationMasterCount;
            this.SanitationJobCount = trans.SanitationJob.SanitationJobCount;
            this.SanitationJobList = trans.SanitationJob.SanitationJobList;
        }
        public b_SanitationJob ToDatabaseObjectExtendedSanMasterBatchEntry()
        {
            b_SanitationJob dbObj = this.ToDatabaseObject();
            dbObj.ShiftDescription = this.Shift;
            dbObj.Assigned = this.Assigned;
            dbObj.CompleteBy = this.CompleteBy;
            dbObj.OnDemandGroup = this.OnDemandGroup;
            dbObj.ChargeTo_ClientLookupId = this.ChargeTo_ClientLookupId;
            dbObj.CreateBy = this.CreateBy;
            dbObj.CreateDate = this.CreateDate;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.PlantLocationId = this.PlantLocationId;//SOM-1348
            dbObj.Prefix = this.Prefix;
            dbObj.SanMasterBatchEntryId = this.SanMasterBatchEntryId;
            return dbObj;
        }
        public void SanitationJob_GenerationFromSanMasterBatchEntry_OnDemand(DatabaseKey dbKey)
        {

            SanitationJobTransactions_GenerationFromSanMasterBatchEntry_OnDemand trans = new SanitationJobTransactions_GenerationFromSanMasterBatchEntry_OnDemand
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanitationJob = this.ToDatabaseObject();
            trans.SanitationJob = this.ToDatabaseObjectExtendedSanMasterBatchEntry();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.SanitationMasterCount = trans.SanitationJob.SanitationMasterCount;
            this.SanitationJobCount = trans.SanitationJob.SanitationJobCount;
            this.SanitationJobList = trans.SanitationJob.SanitationJobList;
        }
        #endregion

        #region V2-1071
        public SanitationJob RetrieveAllBySanitationJobV2DevExpressPrint(DatabaseKey dbKey, string TimeZone)
        {
            SanitationJob_RetrieveAllByWorkOrdeV2DevExpressPrint trans = new SanitationJob_RetrieveAllByWorkOrdeV2DevExpressPrint()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanitationJob = this.ToDateBaseObjectForRetrieveAllBySanitationJobV2DevExpressPrint();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<SanitationJob> SanitationJoblist = new List<SanitationJob>();
            List<SanitationPlanning> SanitationToolList = new List<SanitationPlanning>();
            List<SanitationPlanning> SanitationChemicalList = new List<SanitationPlanning>();
            List<SanitationJobTask> SanitationJobTaskList = new List<SanitationJobTask>();
            List<Timecard> Timecardlist = new List<Timecard>();

            this.listOfSJ = new List<SanitationJob>();
            foreach (b_SanitationJob sanitationJob in trans.SanitationJob.ListSanJob)
            {
                SanitationJob tmpSanitationJob = new SanitationJob();

                tmpSanitationJob.UpdateFromDatabaseObjectDevExpressPrintSanitationJobExtended(sanitationJob, TimeZone);
                SanitationJoblist.Add(tmpSanitationJob);
            }
            this.listOfSJ.AddRange(SanitationJoblist);
            //For Tool
            this.listOfSanitationTool = new List<SanitationPlanning>();
            foreach (b_SanitationPlanning sanitationPlanning in trans.SanitationJob.listOfSanitationTool)
            {
                SanitationPlanning tmpSanitationPlanning = new SanitationPlanning();

                tmpSanitationPlanning.UpdateFromDatabaseObjectDevExpressPrintSanitationToolExtended(sanitationPlanning);
                SanitationToolList.Add(tmpSanitationPlanning);
            }
            this.listOfSanitationTool.AddRange(SanitationToolList);
            //For Chemical
            this.listOfSanitationChemical = new List<SanitationPlanning>();
            foreach (b_SanitationPlanning sanitationPlanning in trans.SanitationJob.listOfSanitationChemical)
            {
                SanitationPlanning tmpSanitationPlanning = new SanitationPlanning();

                tmpSanitationPlanning.UpdateFromDatabaseObjectDevExpressPrintSanitationChemicalExtended(sanitationPlanning);
                SanitationChemicalList.Add(tmpSanitationPlanning);
            }
            this.listOfSanitationChemical.AddRange(SanitationChemicalList);
            //For Task
            this.listOfSanitationTask = new List<SanitationJobTask>();
            foreach (b_SanitationJobTask sanitationJobTask in trans.SanitationJob.listOfSanitationTask)
            {
                SanitationJobTask tmpSanitationJobTask = new SanitationJobTask();

                tmpSanitationJobTask.UpdateFromDatabaseObjectForSanitation(sanitationJobTask);
                SanitationJobTaskList.Add(tmpSanitationJobTask);
            }
            this.listOfSanitationTask.AddRange(SanitationJobTaskList);
            //For Labour
            this.listOfTimecard = new List<Timecard>();
            foreach (b_Timecard Timecard in trans.SanitationJob.listOfTimecard)
            {
                Timecard tmpTimecard = new Timecard();

                tmpTimecard.UpdateFromDatabaseObjectTimeCardForDevExpressPrintExtended(Timecard);
                Timecardlist.Add(tmpTimecard);
            }
            this.listOfTimecard.AddRange(Timecardlist);
            return this;
        }
        public b_SanitationJob ToDateBaseObjectForRetrieveAllBySanitationJobV2DevExpressPrint()
        {
            b_SanitationJob dbObj = new b_SanitationJob();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.SanitationJobIdList = this.SanitationJobIDList;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectDevExpressPrintSanitationJobExtended(b_SanitationJob dbObj, string Timezone)
        { 
            this.SanitationJobId = dbObj.SanitationJobId;
            this.SanitationMasterId = dbObj.SanitationMasterId;
            this.AreaId = dbObj.AreaId;
            this.DepartmentId = dbObj.DepartmentId;
            this.StoreroomId = dbObj.StoreroomId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.SourceType = dbObj.SourceType;
            this.SourceId = dbObj.SourceId;
            this.ActualDuration = dbObj.ActualDuration;
            this.AssignedTo_PersonnelId = dbObj.AssignedTo_PersonnelId;
            this.ChargeToId = dbObj.ChargeToId;
            this.ChargeType = dbObj.ChargeType;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.CompleteBy_PersonnelId = dbObj.CompleteBy_PersonnelId;
            this.CompleteComments = dbObj.CompleteComments;
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CompleteDate = dbObj.CompleteDate;
            }
            this.Description = dbObj.Description;
            this.Shift = dbObj.Shift;
            this.DownRequired = dbObj.DownRequired;
            this.ScheduledDate = dbObj.ScheduledDate;
            this.ScheduledDuration = dbObj.ScheduledDuration;
            switch (dbObj.Status)
            {
                case Common.Constants.SanitationJobConstant.Approved:
                    this.Status = "Approved";
                    break;
                case Common.Constants.SanitationJobConstant.Canceled:
                    this.Status = "Canceled";
                    break;
                case Common.Constants.SanitationJobConstant.Complete:
                    this.Status = "Complete";
                    break;
                case Common.Constants.SanitationJobConstant.Denied:
                    this.Status = "Denied";
                    break;
                case Common.Constants.SanitationJobConstant.Fail:
                    this.Status = "Fail";
                    break;
                case Common.Constants.SanitationJobConstant.JobRequest:
                    this.Status = "JobRequest";
                    break;
                case Common.Constants.SanitationJobConstant.Open:
                    this.Status = "Open";
                    break;
                case Common.Constants.SanitationJobConstant.Pass:
                    this.Status = "Pass";
                    break;
                case Common.Constants.SanitationJobConstant.Scheduled:
                    this.Status = "Scheduled";
                    break;
                default:
                    this.Status = string.Empty;
                    break;
            }
            this.Creator_PersonnelId = dbObj.Creator_PersonnelId;
            this.ApproveBy_PersonnelId = dbObj.ApproveBy_PersonnelId;
            this.ApproveDate = dbObj.ApproveDate;
            this.DeniedBy_PersonnelId = dbObj.DeniedBy_PersonnelId;
            this.DeniedDate = dbObj.DeniedDate;
            this.DeniedComment = dbObj.DeniedComment;
            this.PassBy_PersonnelId = dbObj.PassBy_PersonnelId;
            this.PassDate = dbObj.PassDate;
            this.FailBy_PersonnelId = dbObj.FailBy_PersonnelId;
            this.FailDate = dbObj.FailDate;
            this.FailReason = dbObj.FailReason;
            this.FailComment = dbObj.FailComment;
            this.SanOnDemandMasterId = dbObj.SanOnDemandMasterId;
            this.Extracted = dbObj.Extracted;
            this.ExportLogId = dbObj.ExportLogId;
            this.ShiftDescription = dbObj.ShiftDescription;
            this.Assigned = dbObj.Assigned;
            this.CompleteBy = dbObj.CompleteBy;
            this.OnDemandGroup = dbObj.OnDemandGroup;
            this.ChargeTo_ClientLookupId = dbObj.ChargeTo_ClientLookupId;
            this.CreateBy = dbObj.CreateBy;

            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            this.PassBy = dbObj.PassBy;
            this.FailBy = dbObj.FailBy;
            this.FailDescription = dbObj.FailDescription;
            this.SourceIDClientLookUpId = dbObj.SourceIDClientLookUpId;
            this.AssetLocation = dbObj.AssetLocation;
        }
        #endregion
    }

}

