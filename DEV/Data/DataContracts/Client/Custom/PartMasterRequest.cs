/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Changed Parameters
* 2015-Mar-24 SOM-585  Roger Lawton        Localized the Status
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Extensions;

using Database;
using Database.Business;

namespace DataContracts
{
    public partial class PartMasterRequest : DataContractBase, IStoredProcedureValidation
    {

        #region Public Variables
        public long EXPartId { get; set; }
        public long EXPartId_Replace { get; set; }
        public string PartMaster_ClientLookupId { get; set; }
        public string Part_ClientLookupId { get; set; }
        public string Part_PrevClientLookupId { get; set; }
        public string PartMasterClientLookUpId_Replace { get; set; }
        public string Part_Description { get; set; }
        public string CompleteBy { get; set; }
        public string PartMaster_LongDescription { get; set; }
        public long CustomQueryDisplayId { get; set; }
        public string Requester { get; set; }
        public string Requester_Email { get; set; }
        public string Status_Display { get; set; }
        public string ApproveDenyBy { get; set; }
        public string ApproveDenyBy2 { get; set; }
        public string LastReviewedBy { get; set; }
        public string PartMasterCreateBy { get; set; }
        public string Site_Name { get; set; }

        public string ValidateFor = string.Empty;
        public string ExOrganizationId { get; set; }
        #region V2-798
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public long CreateById { get; set; }
        public int TotalCount { get; set; }
        #endregion

        #endregion

        public void RetrieveByPKForeignKeys(DatabaseKey dbKey, string Timezone)
        {

            PartMasterRequestTransactions_RetrieveByForeignKeys trans = new PartMasterRequestTransactions_RetrieveByForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartMasterRequest = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectForRetrive(trans.PartMasterRequest, Timezone);

        }
        public void RetrieveByPKForPMLocalAdd(DatabaseKey dbKey, string Timezone)
        {

            PartMasterRequestTransactions_RetrieveByPKForPMLocalAdd trans = new PartMasterRequestTransactions_RetrieveByPKForPMLocalAdd()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartMasterRequest = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDbForPMLocalAddition(trans.PartMasterRequest, Timezone);

        }
        public void UpdateFromDbForPMLocalAddition(b_PartMasterRequest dbObj, string Timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.ApproveDenyBy = dbObj.ApproveDenyBy;
            this.ApproveDenyBy2 = dbObj.ApproveDenyBy2;
            this.Requester = dbObj.Requester;
            this.Requester_Email = dbObj.Requester_Email;
            this.LastReviewedBy = dbObj.LastReviewedBy;
            this.PartMasterCreateBy = dbObj.PartMasterCreateBy;
            this.PartMaster_ClientLookupId = dbObj.PartMaster_ClientLookupId;
            this.Site_Name = dbObj.Site_Name;
            this.Part_ClientLookupId = dbObj.Part_ClientLookupId;
            this.Part_Description = dbObj.Part_Description;
            this.CompleteBy = dbObj.CompleteBy;
            this.PartMaster_LongDescription = dbObj.PartMaster_LongDescription;
            // Need to convert dates
            // Created Date
            if (dbObj.CreatedDate != null && dbObj.CreatedDate != DateTime.MinValue)
            {
                this.CreatedDate = dbObj.CreatedDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CreatedDate = dbObj.CreatedDate;
            }
            // Approved Date
            if (dbObj.ApproveDeny_Date != null && dbObj.ApproveDeny_Date != DateTime.MinValue)
            {
                this.ApproveDeny_Date = dbObj.ApproveDeny_Date.ToUserTimeZone(Timezone);
            }
            else
            {
                this.ApproveDeny_Date = null;
            }
            // Approved Date
            if (dbObj.ApproveDeny2_Date != null && dbObj.ApproveDeny2_Date != DateTime.MinValue)
            {
                this.ApproveDeny2_Date = dbObj.ApproveDeny2_Date.ToUserTimeZone(Timezone);
            }
            else
            {
                this.ApproveDeny2_Date = null;
            }
            // Reviewed Date
            if (dbObj.LastReviewed_Date != null && dbObj.LastReviewed_Date != DateTime.MinValue)
            {
                this.LastReviewed_Date = dbObj.LastReviewed_Date.ToUserTimeZone(Timezone);
            }
            else
            {
                this.LastReviewed_Date = null;
            }
            // Part Master CompleteDate Date
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CompleteDate = null;
            }
            /*switch (this.Status)
            {
                case Common.Constants.PartMasterRequestStatusConstants.Open:
                    this.Status_Display = loc.PartMasterRequestStatus.Open;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Review:
                    this.Status_Display = loc.PartMasterRequestStatus.Review;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Returned:
                    this.Status_Display = loc.PartMasterRequestStatus.Returned;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Denied:
                    this.Status_Display = loc.PartMasterRequestStatus.Denied;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Approved:
                    this.Status_Display = loc.PartMasterRequestStatus.Approved;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Complete:
                    this.Status_Display = loc.PartMasterRequestStatus.Complete;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.SiteApproved:
                    this.Status_Display = loc.PartMasterRequestStatus.SiteApproved;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Canceled:
                    this.Status_Display = loc.PartMasterRequestStatus.Canceled;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Extracted:
                    this.Status_Display = loc.PartMasterRequestStatus.Extracted;
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }
            switch (this.RequestType)
            {
                case Common.Constants.PartMasterRequestTypeConstants.Addition:
                    this.RequestType = loc.PartMasterRequestTypes.Addition;
                    break;
                case Common.Constants.PartMasterRequestTypeConstants.Replacement:
                    this.RequestType = loc.PartMasterRequestTypes.Replacement;
                    break;
                case Common.Constants.PartMasterRequestTypeConstants.Inactivation:
                    this.RequestType = loc.PartMasterRequestTypes.Inactivation;
                    break;
                case Common.Constants.PartMasterRequestTypeConstants.SX_Replacement:
                    this.RequestType = loc.PartMasterRequestTypes.SX_Replacement;
                    break;
                case Common.Constants.PartMasterRequestTypeConstants.ECO_New:
                    this.RequestType = loc.PartMasterRequestTypes.ECO_New;
                    break;
                case Common.Constants.PartMasterRequestTypeConstants.ECO_Replace:
                    this.RequestType = loc.PartMasterRequestTypes.ECO_Replace;
                    break;
                case Common.Constants.PartMasterRequestTypeConstants.ECO_SX_Replace:
                    this.RequestType = loc.PartMasterRequestTypes.ECO_SX_Replace;
                    break;
                default:
                    this.RequestType = string.Empty;
                    break;
            }*/

        }
        public void UpdateFromDatabaseObjectForRetrive(b_PartMasterRequest dbObj, string Timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.ApproveDenyBy = dbObj.ApproveDenyBy;
            this.ApproveDenyBy2 = dbObj.ApproveDenyBy2;
            this.Requester = dbObj.Requester;
            this.Requester_Email = dbObj.Requester_Email;
            this.LastReviewedBy = dbObj.LastReviewedBy;
            this.PartMasterCreateBy = dbObj.PartMasterCreateBy;
            this.PartMaster_ClientLookupId = dbObj.PartMaster_ClientLookupId;
            this.Part_ClientLookupId = dbObj.Part_ClientLookupId;
            this.Part_PrevClientLookupId = dbObj.Part_PrevClientLookupId;
            this.Site_Name = dbObj.Site_Name;
            this.Part_Description = dbObj.Part_Description;
            // Need to convert dates
            // Created Date
            if (dbObj.CreatedDate != null && dbObj.CreatedDate != DateTime.MinValue)
            {
                this.CreatedDate = dbObj.CreatedDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CreatedDate = dbObj.CreatedDate;
            }
            // Approved Date
            if (dbObj.ApproveDeny_Date != null && dbObj.ApproveDeny_Date != DateTime.MinValue)
            {
                this.ApproveDeny_Date = dbObj.ApproveDeny_Date.ToUserTimeZone(Timezone);
            }
            else
            {
                this.ApproveDeny_Date = null;
            }
            // Approved Date
            if (dbObj.ApproveDeny2_Date != null && dbObj.ApproveDeny2_Date != DateTime.MinValue)
            {
                this.ApproveDeny2_Date = dbObj.ApproveDeny2_Date.ToUserTimeZone(Timezone);
            }
            else
            {
                this.ApproveDeny2_Date = null;
            }
            // Reviewed Date
            if (dbObj.LastReviewed_Date != null && dbObj.LastReviewed_Date != DateTime.MinValue)
            {
                this.LastReviewed_Date = dbObj.LastReviewed_Date.ToUserTimeZone(Timezone);
            }
            else
            {
                this.LastReviewed_Date = null;
            }
            // Part Master Created Date
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CompleteDate = null;
            }
            /*switch (this.Status)
            {
                case Common.Constants.PartMasterRequestStatusConstants.Open:
                    this.Status_Display = loc.PartMasterRequestStatus.Open;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Review:
                    this.Status_Display = loc.PartMasterRequestStatus.Review;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Returned:
                    this.Status_Display = loc.PartMasterRequestStatus.Returned;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Denied:
                    this.Status_Display = loc.PartMasterRequestStatus.Denied;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Approved:
                    this.Status_Display = loc.PartMasterRequestStatus.Approved;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Complete:
                    this.Status_Display = loc.PartMasterRequestStatus.Complete;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.SiteApproved:
                    this.Status_Display = loc.PartMasterRequestStatus.SiteApproved;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Canceled:
                    this.Status_Display = loc.PartMasterRequestStatus.Canceled;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Extracted:
                    this.Status_Display = loc.PartMasterRequestStatus.Extracted;
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }*/
        }
        public void UpdateFromDatabaseObjectExtended(b_PartMasterRequest dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.Requester = dbObj.Requester;
            this.TotalCount = dbObj.TotalCount;
            /*switch (this.RequestType)
            {
                case Common.Constants.PartMasterRequestTypeConstants.Addition:
                    this.RequestType = loc.PartMasterRequestTypes.Addition;
                    break;
                case Common.Constants.PartMasterRequestTypeConstants.Replacement:
                    this.RequestType = loc.PartMasterRequestTypes.Replacement;
                    break;
                case Common.Constants.PartMasterRequestTypeConstants.Inactivation:
                    this.RequestType = loc.PartMasterRequestTypes.Inactivation;
                    break;
                case Common.Constants.PartMasterRequestTypeConstants.SX_Replacement:
                    this.RequestType = loc.PartMasterRequestTypes.SX_Replacement;
                    break;
                case Common.Constants.PartMasterRequestTypeConstants.ECO_New:
                    this.RequestType = loc.PartMasterRequestTypes.ECO_New;
                    break;
                case Common.Constants.PartMasterRequestTypeConstants.ECO_Replace:
                    this.RequestType = loc.PartMasterRequestTypes.ECO_Replace;
                    break;
                case Common.Constants.PartMasterRequestTypeConstants.ECO_SX_Replace:
                    this.RequestType = loc.PartMasterRequestTypes.ECO_SX_Replace;
                    break;
                default:
                    this.RequestType = string.Empty;
                    break;
            }
            switch (this.Status)
            {
                case Common.Constants.PartMasterRequestStatusConstants.Open:
                    this.Status_Display = loc.PartMasterRequestStatus.Open;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Review:
                    this.Status_Display = loc.PartMasterRequestStatus.Review;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Returned:
                    this.Status_Display = loc.PartMasterRequestStatus.Returned;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Denied:
                    this.Status_Display = loc.PartMasterRequestStatus.Denied;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Approved:
                    this.Status_Display = loc.PartMasterRequestStatus.Approved;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Extracted:
                    this.Status_Display = loc.PartMasterRequestStatus.Extracted;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Complete:
                    this.Status_Display = loc.PartMasterRequestStatus.Complete;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.SiteApproved:
                    this.Status_Display = loc.PartMasterRequestStatus.SiteApproved;
                    break;
                case Common.Constants.PartMasterRequestStatusConstants.Canceled:
                    this.Status_Display = loc.PartMasterRequestStatus.Canceled;
                    break;
                default:
                    this.Status_Display = string.Empty;
                    break;
            }*/
        }
        public void AddNewPartNewRequestor(DatabaseKey dbKey)
        {
            PartMasterRequest_CreateNewPartNewRequestor trans = new PartMasterRequest_CreateNewPartNewRequestor();
            if (this.RequestType == Common.Constants.PartMasterRequestTypeConstants.ECO_New)
            {
                trans.PartMasterRequest = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.PartMasterRequest.Part_ClientLookupId = this.Part_ClientLookupId;
                trans.Execute();
                UpdateFromDatabaseObject(trans.PartMasterRequest);
            }
            else
            {
                ValidateFor = "ValidateForSomaxPartLookup";
                Validate<PartMasterRequest>(dbKey);
                if (IsValid)
                {
                    trans.PartMasterRequest = this.ToDatabaseObject();
                    trans.PartMasterRequest.Part_ClientLookupId = this.Part_ClientLookupId;
                    trans.dbKey = dbKey.ToTransDbKey();
                    trans.Execute();
                    UpdateFromDatabaseObject(trans.PartMasterRequest);
                }
            }
            // The create procedure may have populated an auto-incremented key. 

        }
        public void UpdateByPk(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForPartMasterNumberLookup";
            Validate<PartMasterRequest>(dbKey);

            if (IsValid)
            {
                ValidateFor = "ValidateForSomaxPartLookup";
                Validate<PartMasterRequest>(dbKey);
                if (IsValid)
                {
                    PartMasterRequest_UpdateByPK trans = new PartMasterRequest_UpdateByPK();
                    trans.PartMasterRequest = this.ToDatabaseObject();
                    trans.PartMasterRequest.PartMaster_ClientLookupId = this.PartMaster_ClientLookupId;
                    trans.PartMasterRequest.Part_ClientLookupId = this.Part_ClientLookupId;
                    trans.ChangeLog = GetChangeLogObject(dbKey);
                    trans.dbKey = dbKey.ToTransDbKey();
                    trans.Execute();

                    // The create procedure changed the Update Index.
                    UpdateFromDatabaseObject(trans.PartMasterRequest);
                }
            }

        }
        public List<PartMasterRequest> Retrieve_PartMasterRequests_ByFilterCriteria(DatabaseKey dbKey, string Timezone)
        {
            PartMasterRequest_RetrieveAllForSearch trans = new PartMasterRequest_RetrieveAllForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartMasterRequest = this.ToDBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PartMasterRequest> partMasterRequestList = new List<PartMasterRequest>();
            foreach (b_PartMasterRequest partMasterRequest in trans.partMasterRequestList)
            {
                PartMasterRequest tmppartmasterrequest = new PartMasterRequest();

                tmppartmasterrequest.UpdateFromDatabaseObjectExtended(partMasterRequest);
                partMasterRequestList.Add(tmppartmasterrequest);
            }
            return partMasterRequestList;

        }
        public List<PartMasterRequest> Retrieve_PartMasterRequest_ExtractData(DatabaseKey dbKey)
        {
            PartMasterRequest_RetrieveExtractData trans = new PartMasterRequest_RetrieveExtractData()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartMasterRequest = this.ToDatabaseObject();
            trans.PartMasterRequest.ExportLogId = this.ExportLogId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PartMasterRequest> partMasterRequestList = new List<PartMasterRequest>();
            foreach (b_PartMasterRequest partMasterRequest in trans.partMasterRequestList)
            {
                PartMasterRequest tmpsanitationJob = new PartMasterRequest();

                tmpsanitationJob.UpdateFromDbExtract(partMasterRequest);
                partMasterRequestList.Add(tmpsanitationJob);
            }
            return partMasterRequestList;

        }

        public void UpdateFromDbExtract(b_PartMasterRequest dbObj)
        {
            this.Part_ClientLookupId = dbObj.Part_ClientLookupId;
            this.PartMaster_ClientLookupId = dbObj.PartMaster_ClientLookupId;
            this.PartMasterClientLookUpId_Replace = dbObj.PartMasterClientLookUpId_Replace;
            this.EXPartId = dbObj.EXPartId;
            this.EXPartId_Replace = dbObj.EXPartId_Replace;
            this.PartMasterRequestId = dbObj.PartMasterRequestId;
            this.ExOrganizationId = dbObj.ExOrganizationId;
            this.RequestType = dbObj.RequestType;
        }


        public b_PartMasterRequest ToDBaseObjectForRetriveAllForSearch()
        {
            b_PartMasterRequest dbObj = this.ToDatabaseObject();
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.orderBy = this.orderBy;
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.PartMasterRequestId = this.PartMasterRequestId;
            dbObj.CreatedBy_PersonnelId = this.CreatedBy_PersonnelId;
            dbObj.CreateById = this.CreateById;
            dbObj.Justification = this.Justification;
            dbObj.RequestType = this.RequestType;
            dbObj.Status = this.Status;
            dbObj.Manufacturer = this.Manufacturer;
            dbObj.ManufacturerId = this.ManufacturerId;
            return dbObj;
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "ValidateForPartMasterNumberLookup")
            {
                PartMasterRequest_ValidationPartMasterNumberLookup trans = new PartMasterRequest_ValidationPartMasterNumberLookup()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartMasterRequest = this.ToDatabaseObject();
                trans.PartMasterRequest.PartMaster_ClientLookupId = this.PartMaster_ClientLookupId;
                trans.PartMasterRequest.Part_ClientLookupId = this.Part_ClientLookupId;
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
            else if (ValidateFor == "ValidateForSomaxPartLookup")
            {
                PartMasterRequest_ValidationSomaxPartLookup trans = new PartMasterRequest_ValidationSomaxPartLookup()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartMasterRequest = this.ToDatabaseObject();
                trans.PartMasterRequest.PartMaster_ClientLookupId = this.PartMaster_ClientLookupId;
                trans.PartMasterRequest.Part_ClientLookupId = this.Part_ClientLookupId;
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
        public void CreateByFKAssign(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForPartMasterNumberLookup";
            Validate<PartMasterRequest>(dbKey);
            if (IsValid)
            {
                PartMasterRequest_CreateByFK trans = new PartMasterRequest_CreateByFK();
                trans.PartMasterRequest = this.ToDatabaseObject();
                trans.PartMasterRequest.PartMaster_ClientLookupId = this.PartMaster_ClientLookupId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.PartMasterRequest);
            }
        }
        public void CreateByFKInactivate(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForSomaxPartLookup";
            Validate<PartMasterRequest>(dbKey);
            if (IsValid)
            {
                PartMasterRequest_CreateByFK trans = new PartMasterRequest_CreateByFK();
                trans.PartMasterRequest = this.ToDatabaseObject();
                trans.PartMasterRequest.Part_ClientLookupId = this.Part_ClientLookupId;
                trans.PartMasterRequest.RequestType = this.RequestType;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.PartMasterRequest);
            }
        }
        public void CreateByFKReplace(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForPartMasterNumberLookup";
            Validate<PartMasterRequest>(dbKey);


            if (IsValid)
            {
                ValidateFor = "ValidateForSomaxPartLookup";
                Validate<PartMasterRequest>(dbKey);

                if (IsValid)
                {
                    PartMasterRequest_CreateByFK trans = new PartMasterRequest_CreateByFK();
                    trans.PartMasterRequest = this.ToDatabaseObject();
                    trans.PartMasterRequest.Part_ClientLookupId = this.Part_ClientLookupId;
                    trans.PartMasterRequest.PartMaster_ClientLookupId = this.PartMaster_ClientLookupId;
                    trans.PartMasterRequest.RequestType = this.RequestType;
                    trans.dbKey = dbKey.ToTransDbKey();
                    trans.Execute();

                    // The create procedure may have populated an auto-incremented key. 
                    UpdateFromDatabaseObject(trans.PartMasterRequest);
                }
            }
        }
        public void UpdateForDenied(DatabaseKey dbKey)
        {
            PartMasterRequest_UpdateForDenied trans = new PartMasterRequest_UpdateForDenied();
            trans.PartMasterRequest = this.ToDatabaseObject();
            trans.PartMasterRequest.RequestType = this.RequestType;
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.PartMasterRequest);


        }

    }

}
