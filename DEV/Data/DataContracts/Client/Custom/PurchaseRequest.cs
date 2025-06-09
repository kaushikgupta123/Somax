/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 2014-Oct-15  SOM-369   Roger Lawton      Moved the generation of the po number from 
*                                          the popup page to here. Did this because if you fail 
*                                          validation (for whatever reason) you "burn" a PO Number
*                                          Converted Create Date from UTC to User's TimeZone
*                                          Added using to using Common.Extensions
* 2015-Feb-16  SOM-558   Roger Lawton      Correct Issue with Status                        
* 2017-Feb-13  SOM-1231  Roger Lawton      Return Total Cost for approval workbench
**************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Common.Extensions;
using Database;
using Database.Business;
using Database.Client.Custom.Transactions;

namespace DataContracts
{
    public partial class PurchaseRequest : DataContractBase, IStoredProcedureValidation
    {
        #region Property
        public string Created { get; set; }
        public string StatusDrop { get; set; }
        public Int64 UserInfoId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ProcessedDate { get; set; }
        public string VendorName { get; set; }
        public string VendorPhoneNumber { get; set; }
        public string VendorEmailAddress { get; set; }
        public string VendorClientLookupId { get; set; }
        public bool VendorIsExternal { get; set; }
        public string Creator_PersonnelName { get; set; }
        public string Approved_PersonnelName { get; set; }
        public string Processed_PersonnelName { get; set; }
        public string PRClientLookupId { get; set; }
        public string PersonnelName { get; set; }
        public Int64 PurchaseRequestLineItemId { get; set; }
        public int LineNumber { get; set; }
        public string Description { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string PartId { get; set; }
        public decimal OrderQuantity { get; set; }
        public string UnitofMeasure { get; set; }
        public decimal UnitCost { get; set; }
        public int CountLineItem { get; set; }
        public decimal TotalCost { get; set; }
        public int CustomQueryDisplayId { get; set; }
        // public long PurchaseOrderId { get; set; }
        public string Prefix { get; set; }
        public long PersonnelId { get; set; }
        public long ProcessLogId { get; set; }
        public string CreatedBy { get; set; }//SOM-800,801
        public string PurchaseOrderClientLookupId { get; set; }
        //SOM - 823 
        public string VendorAddress1 { get; set; }
        public string VendorAddress2 { get; set; }
        public string VendorAddress3 { get; set; }
        public string VendorAddressCity { get; set; }
        public string VendorAddressCountry { get; set; }
        public string VendorAddressPostCode { get; set; }
        public string VendorAddressState { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress1 { get; set; }
        public string SiteAddress2 { get; set; }
        public string SiteAddress3 { get; set; }
        public string SiteAddressCity { get; set; }
        public string SiteAddressCountry { get; set; }
        public string SiteAddressPostCode { get; set; }
        public string SiteAddressState { get; set; }
        public string SiteBillToName { get; set; } //945
        public string SiteBillToAddress1 { get; set; }
        public string SiteBillToAddress2 { get; set; }
        public string SiteBillToAddress3 { get; set; }
        public string SiteBillToAddressCity { get; set; }
        public string SiteBillToAddressCountry { get; set; }
        public string SiteBillToAddressPostCode { get; set; }
        public string SiteBillToAddressState { get; set; }
        //---------SOM-1029-Api--------------------
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        // RKL - 2019-10-17 - Added for Coupa Interface
        //  Header
        //    Approved By Email (Used as the Coupa Login)
        //    Ship To Code (From Site.Address3 - Required)
        //  Line Item 
        //    External Vendor Id (Source from Vendor Master)
        //    Terms Description (Terms Description 
        //    Currency Code 
        //    Source Type
        //    Line Type
        //    Account Name 
        //    Commodity Code 
        public long ExVendorId { get; set; }
        public string EXUserId { get; set; }
        public string Ship_to_Code { get; set; }
        public string Terms_Desc { get; set; }
        public string Currency_Code { get; set; }
        public string Source_Type { get; set; }
        public string Line_Type { get; set; }
        public string Acct_Name { get; set; }
        public string Commodity_Code { get; set; }

        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string SearchText { get; set; }
        public string CreatedDate { get; set; }
        public string ProcessingDate { get; set; }
        public Int32 ChildCount { get; set; }
        public int TotalCount { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        //V2-347
        public string CreateStartDate { get; set; }
        public string CreateEndDate { get; set; }
        public string ProcessedStartDate { get; set; }
        public string ProcessedEndDate { get; set; }
        //V2-375
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string ProcessedStartDateVw { get; set; }
        public string ProcessedEndDateVw { get; set; }
        public string CancelandDeniedStartDateVw { get; set; }
        public string CancelandDeniedEndDateVw { get; set; }
        public string EXOracleUserId { get; set; }
        public string UserName { get; set; }
        #region V2-945
        public string PurchaseRequestIDList { get; set; }
        public List<PurchaseRequest> listOfPR { get; set; }
        public List<PRHeaderUDF> listOfPRHeaderUDF { get; set; }
        public List<PurchaseRequestLineItem> listOfPRLI { get; set; }
        public List<PRLineUDF> listOfPRLineUDF { get; set; }
        #endregion
        #endregion
        //V2-643
        public string PartClientLookupId { get; set; }
        public string VendorIDList { get; set; }
        public long PartID { get; set; }
        public decimal? LastPurchaseCost { get; set; }
        public decimal? QtyToOrder { get; set; }
        public string PartIDList { get; set; }
        //V2-738
        public string StoreroomName { get; set; }
        public DataTable PartList { get; set; }
        public void CreateByPKForeignKeys(DatabaseKey dbKey, bool AutoGenPRNum, string AutoGenId, string Prefix)
        {
            Validate<PurchaseRequest>(dbKey);

            if (IsValid)
            {
                // SOM-369 - Generate the PO Number if appropriate
                // If not - check to make sure one exists
                // The stored procedure validation checks for duplicates but not if blank
                if (AutoGenPRNum)
                {
                    ClientLookupId = CustomSequentialId.GetNextId(dbKey, AutoGenId, dbKey.User.DefaultSiteId, Prefix);
                }

                // SOM-369 - End
                PurchaseRequest_CreateByForeignKeys trans = new PurchaseRequest_CreateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.PurchaseRequest = this.ToDatabaseObject();

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                UpdateFromDatabaseObject(trans.PurchaseRequest);

            }
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            PurchaseRequest_ValidateByClientLookupId trans = new PurchaseRequest_ValidateByClientLookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.PurchaseRequest = this.ToDatabaseObject();
            //  trans.PurchaseRequest.VendorClientLookupId = this.VendorClientLookupId;//--added on 09-12-2014---
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (trans.StoredProcValidationErrorList != null)
            {
                foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                {
                    StoredProcValidationError tmp = new StoredProcValidationError();
                    tmp.UpdateFromDatabaseObject(error);
                    errors.Add(tmp);
                }
            }

            return errors;
        }

        public void RetrieveByPKForeignKeys(DatabaseKey dbKey, string UserTimeZone)
        {
            PurchaseRequest_RetrieveByForeignKeys trans = new PurchaseRequest_RetrieveByForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PurchaseRequest = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.PurchaseRequest);
            // Convert to User Time Zone
            //this.CreateDate = trans.PurchaseRequest.CreateDate.ToUserTimeZone(UserTimeZone);
            this.CreateDate = trans.PurchaseRequest.CreateDate.ToUserTimeZone(UserTimeZone);
            this.VendorName = trans.PurchaseRequest.VendorName;
            this.VendorEmailAddress = trans.PurchaseRequest.VendorEmailAddress;
            this.VendorClientLookupId = trans.PurchaseRequest.VendorClientLookupId;
            this.VendorIsExternal = trans.PurchaseRequest.VendorIsExternal;
            this.Creator_PersonnelName = trans.PurchaseRequest.Creator_PersonnelName;
            this.Approved_PersonnelName = trans.PurchaseRequest.Approved_PersonnelName;
            this.Processed_PersonnelName = trans.PurchaseRequest.Processed_PersonnelName;
            this.CountLineItem = trans.PurchaseRequest.CountLineItem;
            this.TotalCost = trans.PurchaseRequest.TotalCost;
            this.ProcessedDate = trans.PurchaseRequest.ProcessedDate.ConvertFromUTCToUser(UserTimeZone);
            this.PurchaseOrderClientLookupId = trans.PurchaseRequest.PurchaseOrderClientLookupId;
            this.CreatedBy = trans.PurchaseRequest.CreatedBy;
            this.StoreroomName = trans.PurchaseRequest.StoreroomName;



        }

        // RKL - 2019-10-17 - Added for Coupa Interface
        public void RetrieveForCoupaExport(DatabaseKey dbKey, string UserTimeZone)
        {
          PurchaseRequest_RetrieveForCoupaExport trans = new PurchaseRequest_RetrieveForCoupaExport()
          {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName
          };

          trans.PurchaseRequest = this.ToDatabaseObject();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();

          UpdateFromDatabaseObject(trans.PurchaseRequest);

          this.Creator_PersonnelName = trans.PurchaseRequest.Creator_PersonnelName;
          this.CountLineItem = trans.PurchaseRequest.CountLineItem;
          this.TotalCost = trans.PurchaseRequest.TotalCost;
          this.ExVendorId = trans.PurchaseRequest.ExVendorId;
          this.EXUserId = trans.PurchaseRequest.EXUserId;
          this.Ship_to_Code = trans.PurchaseRequest.Ship_to_Code;
          this.Terms_Desc = trans.PurchaseRequest.Terms_Desc;
          this.Currency_Code = trans.PurchaseRequest.Currency_Code;
          this.Source_Type = trans.PurchaseRequest.Source_Type;
          this.Line_Type = trans.PurchaseRequest.Line_Type;
          this.Acct_Name = trans.PurchaseRequest.Acct_Name;
          this.Commodity_Code = trans.PurchaseRequest.Commodity_Code;

        // Convert to User Time Zone
        //this.CreateDate = trans.PurchaseRequest.CreateDate.ToUserTimeZone(UserTimeZone);
        //this.VendorName = trans.PurchaseRequest.VendorName;
        //this.VendorEmailAddress = trans.PurchaseRequest.VendorEmailAddress;
        //this.VendorClientLookupId = trans.PurchaseRequest.VendorClientLookupId;
        //this.Approved_PersonnelName = trans.PurchaseRequest.Approved_PersonnelName;
        //this.Processed_PersonnelName = trans.PurchaseRequest.Processed_PersonnelName;
        //this.ProcessedDate = trans.PurchaseRequest.ProcessedDate.ConvertFromUTCToUser(UserTimeZone);
        //this.PurchaseOrderClientLookupId = trans.PurchaseRequest.PurchaseOrderClientLookupId;
        //this.CreatedBy = trans.PurchaseRequest.CreatedBy;
        }

        public List<PurchaseRequest> RetrieveAllForSearchNew(DatabaseKey dbKey)
        {
            PurchaseRequest_RetrieveAllForSearch trans = new PurchaseRequest_RetrieveAllForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequest = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PurchaseRequest> PurchaseRequestlist = new List<PurchaseRequest>();

            foreach (b_PurchaseRequest PurchaseRequest in trans.PurchaseRequestList)
            {
                PurchaseRequest tmppurchaseRequest = new PurchaseRequest();
                tmppurchaseRequest.UpdateFromDatabaseObjectForRetriveAllForSearch(PurchaseRequest);
                PurchaseRequestlist.Add(tmppurchaseRequest);
            }
            return PurchaseRequestlist;
        }

        public List<PurchaseRequest> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            PurchaseRequest_RetrieveChunkSearch trans = new PurchaseRequest_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequest = this.ToDateBaseObjectForRetrieveChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();           

            List<PurchaseRequest> PurchaseRequestlist = new List<PurchaseRequest>();

            foreach (b_PurchaseRequest PurchaseRequest in trans.PurchaseRequestList)
            {
                PurchaseRequest tmppurchaseRequest = new PurchaseRequest();
                tmppurchaseRequest.UpdateFromDatabaseObjectForRetriveAllForSearch(PurchaseRequest);
                PurchaseRequestlist.Add(tmppurchaseRequest);
            }
            return PurchaseRequestlist;
        }
        public b_PurchaseRequest ToDateBaseObjectForRetriveAllForSearch()
        {
            b_PurchaseRequest dbObj = this.ToDatabaseObject();
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;

            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Reason = this.Reason;
            dbObj.Status = this.Status;
            dbObj.Creator_PersonnelName = this.Creator_PersonnelName;
            dbObj.VendorClientLookupId = this.VendorClientLookupId;
            dbObj.VendorName = this.VendorName;
            dbObj.CreateDate = this.CreateDate;
            dbObj.PurchaseOrderClientLookupId = this.PurchaseOrderClientLookupId;
            dbObj.Processed_PersonnelName = this.Processed_PersonnelName;
            dbObj.ProcessedDate = this.ProcessedDate;
            dbObj.SearchText = this.SearchText;
            dbObj.CreatedDate = this.CreatedDate;
            dbObj.ProcessingDate = this.ProcessingDate;
            return dbObj;
        }



        public b_PurchaseRequest ToDateBaseObjectForRetrieveChunkSearch()
        {
            b_PurchaseRequest dbObj = this.ToDatabaseObject();
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;

            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Reason = this.Reason;
            dbObj.Status = this.Status;
            dbObj.Creator_PersonnelName = this.Creator_PersonnelName;
            dbObj.VendorClientLookupId = this.VendorClientLookupId;
            dbObj.VendorName = this.VendorName;
            dbObj.PurchaseOrderClientLookupId = this.PurchaseOrderClientLookupId;
            dbObj.Processed_PersonnelName = this.Processed_PersonnelName;
            dbObj.ProcessedDate = this.ProcessedDate;
            dbObj.SearchText = this.SearchText;
            dbObj.CreateStartDate = this.CreateStartDate;
            dbObj.CreateEndDate = this.CreateEndDate;
            dbObj.ProcessedStartDate = this.ProcessedStartDate;
            dbObj.ProcessedEndDate = this.ProcessedEndDate;
            dbObj.CreateStartDateVw = this.CreateStartDateVw;
            dbObj.CreateEndDateVw = this.CreateEndDateVw;
            dbObj.ProcessedStartDateVw = this.ProcessedStartDateVw;
            dbObj.ProcessedEndDateVw = this.ProcessedEndDateVw;
            dbObj.CancelandDeniedStartDateVw = this.CancelandDeniedStartDateVw;
            dbObj.CancelandDeniedEndDateVw = this.CancelandDeniedEndDateVw;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_PurchaseRequest dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.CustomQueryDisplayId = dbObj.CustomQueryDisplayId;
            this.CreateDate = dbObj.CreateDate;
            this.VendorName = dbObj.VendorName;
            this.VendorPhoneNumber = dbObj.VendorPhoneNumber;
            this.VendorClientLookupId = dbObj.VendorClientLookupId;
            this.VendorIsExternal= dbObj.VendorIsExternal;
            this.Creator_PersonnelName = dbObj.Creator_PersonnelName;
            this.Processed_PersonnelName = dbObj.Processed_PersonnelName;
            this.ProcessedDate = dbObj.ProcessedDate;
            this.PurchaseOrderClientLookupId = dbObj.PurchaseOrderClientLookupId;
            this.orderbyColumn = dbObj.orderbyColumn;
            this.orderBy = dbObj.orderBy;
            this.offset1 = dbObj.offset1;
            this.nextrow = dbObj.nextrow;
            this.SearchText = dbObj.SearchText;
            this.ChildCount = dbObj.ChildCount;
            this.TotalCount = dbObj.TotalCount;

        }

        public List<PurchaseRequest> RetrieveByStatus(DatabaseKey dbKey)
        {
            PurchaseRequest_RetrieveByStatus trans = new PurchaseRequest_RetrieveByStatus()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequest = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PurchaseRequest> PurchaseRequestlist = new List<PurchaseRequest>();

            foreach (b_PurchaseRequest PurchaseRequest in trans.PurchaseRequestList)
            {
                PurchaseRequest tmppurchaseRequest = new PurchaseRequest();
                tmppurchaseRequest.UpdateFromDatabaseObject(PurchaseRequest);
                tmppurchaseRequest.VendorIsExternal = PurchaseRequest.VendorIsExternal;
                tmppurchaseRequest.CountLineItem = PurchaseRequest.CountLineItem;
                // SOM-1686
                tmppurchaseRequest.Creator_PersonnelName = PurchaseRequest.Creator_PersonnelName;
                tmppurchaseRequest.Approved_PersonnelName = PurchaseRequest.Approved_PersonnelName;
                PurchaseRequestlist.Add(tmppurchaseRequest);
            }
            return PurchaseRequestlist;
        }

        public List<PurchaseRequest> RetrieveForInformation(DatabaseKey dbKey)
        {
            PurchaseRequest_RetrieveForInformation trans = new PurchaseRequest_RetrieveForInformation()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequest = this.ToDateBaseObjectRetrieveForInformation();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PurchaseRequest> PurchaseRequestlist = new List<PurchaseRequest>();

            foreach (b_PurchaseRequest PurchaseRequest in trans.PurchaseRequestList)
            {
                PurchaseRequest tmppurchaseRequest = new PurchaseRequest();
                tmppurchaseRequest.UpdateFromDatabaseObjectRetrieveForInformation(PurchaseRequest);
                tmppurchaseRequest.Creator_PersonnelName = PurchaseRequest.Creator_PersonnelName;
                tmppurchaseRequest.Approved_PersonnelName = PurchaseRequest.Approved_PersonnelName;
                tmppurchaseRequest.Processed_PersonnelName = PurchaseRequest.Processed_PersonnelName;
                PurchaseRequestlist.Add(tmppurchaseRequest);
            }
            return PurchaseRequestlist;
        }


        public b_PurchaseRequest ToDateBaseObjectRetrieveForInformation()
        {
            b_PurchaseRequest dbObj = new b_PurchaseRequest();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this._SiteId;
            dbObj.PurchaseRequestId = this.PurchaseRequestId;
            dbObj.PurchaseOrderId = this.PurchaseOrderId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.CreatedBy_PersonnelId = this.CreatedBy_PersonnelId;
            dbObj.CreateDate = this.CreateDate;
            dbObj.ApprovedBy_PersonnelId = this.ApprovedBy_PersonnelId;
            dbObj.Approved_Date = this.Approved_Date;
            dbObj.ProcessBy_PersonnelId = this.ProcessBy_PersonnelId;
            dbObj.Processed_Date = this.Processed_Date;
            dbObj.AutoGenerated = this._AutoGenerated;
            dbObj.UpdateIndex = this._UpdateIndex;
            dbObj.Creator_PersonnelName = this.Creator_PersonnelName;
            dbObj.Approved_PersonnelName = this.Approved_PersonnelName;
            dbObj.Processed_PersonnelName = this.Processed_PersonnelName;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectRetrieveForInformation(b_PurchaseRequest dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PurchaseRequestId = dbObj.PurchaseRequestId;
            this.PurchaseOrderId = dbObj.PurchaseOrderId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.CreatedBy_PersonnelId = dbObj.CreatedBy_PersonnelId;
            this.CreateDate = dbObj.CreateDate;
            this.ApprovedBy_PersonnelId = dbObj.ApprovedBy_PersonnelId;
            this.Approved_Date = dbObj.Approved_Date;
            this.ProcessBy_PersonnelId = dbObj.ProcessBy_PersonnelId;
            this.Processed_Date = dbObj.Processed_Date;
            this.AutoGenerated = dbObj.AutoGenerated;
            this.UpdateIndex = dbObj.UpdateIndex;
            this.Creator_PersonnelName = dbObj.Creator_PersonnelName;
            this.Approved_PersonnelName = dbObj.Approved_PersonnelName;
            this.Processed_PersonnelName = dbObj.Processed_PersonnelName;
            // Turn on auditing
            AuditEnabled = true;
        }
        public long PurchaseRequestConvert(DatabaseKey dbKey)
        {
            PurchaseRequest_Convert trans = new PurchaseRequest_Convert()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequest = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            PurchaseOrderId = trans.PurchaseRequest.PurchaseOrderId;
            return PurchaseRequestId;
        }

        public long PurchaseRequestConvertV2(DatabaseKey dbKey)
        {
            PurchaseRequest_ConvertV2 trans = new PurchaseRequest_ConvertV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequest = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            PurchaseOrderId = trans.PurchaseRequest.PurchaseOrderId;
            return PurchaseRequestId;
        }
        public long PurchaseRequestAutoGeneration(DatabaseKey dbKey)
        {
            PurchaseRequest_AutoGeneration trans = new PurchaseRequest_AutoGeneration()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequest = this.ToDatabaseObject();
            trans.PurchaseRequest = this.ToDatabaseObjectExtended();

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            ProcessLogId = trans.PurchaseRequest.ProcessLogId;
            return ProcessLogId;
        }
        public b_PurchaseRequest ToDatabaseObjectExtended()
        {
            // b_PurchaseRequest dbObj = new b_PurchaseRequest();
            b_PurchaseRequest dbObj = this.ToDatabaseObject();
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.Prefix = this.Prefix;

            return dbObj;
        }
        //SOM - 823
        public void RetrieveByPKForeignKeysForReport(DatabaseKey dbKey, string UserTimeZone)
        {
            PurchaseRequest_RetrieveByForeignKeysForReport trans = new PurchaseRequest_RetrieveByForeignKeysForReport()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PurchaseRequest = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.PurchaseRequest);
            // Convert to User Time Zone
            //this.CreateDate = trans.PurchaseRequest.CreateDate.ToUserTimeZone(UserTimeZone);
            this.CreateDate = trans.PurchaseRequest.CreateDate.ToUserTimeZone(UserTimeZone);
            this.VendorName = trans.PurchaseRequest.VendorName;
            this.VendorEmailAddress = trans.PurchaseRequest.VendorEmailAddress;
            this.VendorClientLookupId = trans.PurchaseRequest.VendorClientLookupId;
            this.Creator_PersonnelName = trans.PurchaseRequest.Creator_PersonnelName;
            this.Approved_PersonnelName = trans.PurchaseRequest.Approved_PersonnelName;
            this.Processed_PersonnelName = trans.PurchaseRequest.Processed_PersonnelName;
            this.CountLineItem = trans.PurchaseRequest.CountLineItem;
            this.TotalCost = trans.PurchaseRequest.TotalCost;
            this.ProcessedDate = trans.PurchaseRequest.ProcessedDate.ConvertFromUTCToUser(UserTimeZone);
            this.PurchaseOrderClientLookupId = trans.PurchaseRequest.PurchaseOrderClientLookupId;
            this.CreatedBy = trans.PurchaseRequest.CreatedBy;

            this.VendorAddress1 = trans.PurchaseRequest.VendorAddress1;
            this.VendorAddress2 = trans.PurchaseRequest.VendorAddress2;
            this.VendorAddress3 = trans.PurchaseRequest.VendorAddress3;
            this.VendorAddressCity = trans.PurchaseRequest.VendorAddressCity;
            this.VendorAddressCountry = trans.PurchaseRequest.VendorAddressCountry;
            this.VendorAddressPostCode = trans.PurchaseRequest.VendorAddressPostCode;
            this.VendorAddressState = trans.PurchaseRequest.VendorAddressState;
            this.SiteName = trans.PurchaseRequest.SiteName;
            this.SiteAddress1 = trans.PurchaseRequest.SiteAddress1;
            this.SiteAddress2 = trans.PurchaseRequest.SiteAddress2;
            this.SiteAddress3 = trans.PurchaseRequest.SiteAddress3;
            this.SiteAddressCity = trans.PurchaseRequest.SiteAddressCity;
            this.SiteAddressCountry = trans.PurchaseRequest.SiteAddressCountry;
            this.SiteAddressPostCode = trans.PurchaseRequest.SiteAddressPostCode;
            this.SiteAddressState = trans.PurchaseRequest.SiteAddressState;
            this.SiteBillToAddress1 = trans.PurchaseRequest.SiteBillToAddress1;
            this.SiteBillToAddress2 = trans.PurchaseRequest.SiteBillToAddress2;
            this.SiteBillToAddress3 = trans.PurchaseRequest.SiteBillToAddress3;
            this.SiteBillToAddressCity = trans.PurchaseRequest.SiteBillToAddressCity;
            this.SiteBillToAddressCountry = trans.PurchaseRequest.SiteBillToAddressCountry;
            this.SiteBillToAddressPostCode = trans.PurchaseRequest.SiteBillToAddressPostCode;
            this.SiteBillToAddressState = trans.PurchaseRequest.SiteBillToAddressState;
        }

        //--------------Calls From API SOM -1029----------------------------------------------------------------------------
        public List<PurchaseRequest> RetrieveForExtraction(DatabaseKey dbKey)
        {
            PurchaseRequest_RetrieveForExtraction trans = new PurchaseRequest_RetrieveForExtraction()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };

            trans.PurchaseRequest = this.ToDatabaseObject();
            trans.PurchaseRequest.StartDate = this.StartDate;
            trans.PurchaseRequest.FinishDate = this.FinishDate;
            trans.PurchaseRequest.SiteId = this.SiteId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PurchaseRequest.UpdateFromDatabaseObjectListExport(trans.PRList);
        }
        public static List<PurchaseRequest> UpdateFromDatabaseObjectListExport(List<b_PurchaseRequest> dbObjs)
        {
            List<PurchaseRequest> result = new List<PurchaseRequest>();

            foreach (b_PurchaseRequest dbObj in dbObjs)
            {
                PurchaseRequest tmp = new PurchaseRequest();
                tmp.ClientId = dbObj.ClientId;
                tmp.SiteId = dbObj.SiteId;
                tmp.PurchaseRequestId = dbObj.PurchaseRequestId;
                tmp.PRClientLookupId = dbObj.PRClientLookupId;
                tmp.VendorClientLookupId = dbObj.VendorClientLookupId;
                tmp.Reason = dbObj.Reason;
                tmp.PersonnelName = dbObj.PersonnelName;
                tmp.PurchaseRequestLineItemId = dbObj.PurchaseRequestLineItemId;
                tmp.LineNumber = dbObj.LineNumber;
                tmp.Description = dbObj.Description;
                tmp.PersonnelName = dbObj.PersonnelName;
                tmp.RequiredDate = dbObj.RequiredDate;
                tmp.PartId = dbObj.PartId;
                tmp.OrderQuantity = dbObj.OrderQuantity;
                tmp.UnitofMeasure = dbObj.UnitofMeasure;
                tmp.UnitCost = dbObj.UnitCost;
                result.Add(tmp);
            }
            return result;
        }
        public List<PurchaseRequest> RetrieveWorkBenchForSearchNew(DatabaseKey dbKey, string TimeZone)
        {
            PurchaseRequest_RetrieveAllWorkbenchSearch trans = new PurchaseRequest_RetrieveAllWorkbenchSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                UseTransaction = false
            };
            trans.PurchaseRequest = this.ToDatabaseObject();
            trans.PurchaseRequest.Created = this.Created;
            trans.PurchaseRequest.StatusDrop = this.StatusDrop;
            // RKL 2016-11-07 - PersonnelId - Not UserInfoId
            trans.PurchaseRequest.PersonnelId = this.PersonnelId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PurchaseRequest> PRlist = new List<PurchaseRequest>();

            foreach (b_PurchaseRequest PurchaseRequest in trans.purchaseRequestList)
            {
                PurchaseRequest tmpPurchaseRequest = new PurchaseRequest();
                // RKL - Moved to the .UpdateFromDatabaseObjectForRetriveAll method
                tmpPurchaseRequest.UpdateFromDatabaseObjectForRetriveAll(PurchaseRequest, TimeZone);
                PRlist.Add(tmpPurchaseRequest);
            }
            return PRlist;
        }

        private void UpdateFromDatabaseObjectForRetriveAll(b_PurchaseRequest dbObj, string TimeZone)
        {

            this.UpdateFromDatabaseObject(dbObj);
            this.PRClientLookupId = dbObj.PRClientLookupId;
            this.Reason = dbObj.Reason;
            this.CreatedBy = dbObj.CreatedBy;

            // SOM-706 - Convert the create date to the user's time zone
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ConvertFromUTCToUser(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            this.VendorClientLookupId = dbObj.VendorClientLookupId;
            this.VendorName = dbObj.VendorName;
            // SOM-1231
            this.TotalCost = dbObj.TotalCost;
        }

        public void UpdateByForeignKeys(DatabaseKey dbKey)
        {
            PurchaseRequest_UpdateByPKForeignKeys trans = new PurchaseRequest_UpdateByPKForeignKeys();
            trans.PurchaseRequest = this.ToDatabaseObject();
            trans.PurchaseRequest.Flag = this.Flag;
            trans.PurchaseRequest.PersonnelId = this.PersonnelId;
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateByDatabase(trans.PurchaseRequest);
        }
        public void UpdateByDatabase(b_PurchaseRequest dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PurchaseRequestId = dbObj.PurchaseRequestId;
            this.SiteId = dbObj.SiteId;
            this.ApprovedBy_PersonnelId = dbObj.ApprovedBy_PersonnelId;
            this.Approved_Date = dbObj.Approved_Date;
            this.Processed_Date = dbObj.Processed_Date;
            this.ProcessBy_PersonnelId = dbObj.ProcessBy_PersonnelId;
            this.Status = dbObj.Status;
            this.UpdateIndex = dbObj.UpdateIndex;
        }
        public string Flag { get; set; }

        #region  Auto PR Generation V2-643
        public List<PurchaseRequest> RetrieveChunkSearchAutoPRGeneration(DatabaseKey dbKey)
        {
            PurchaseRequest_RetrieveChunkSearchAutoPRGeneration trans = new PurchaseRequest_RetrieveChunkSearchAutoPRGeneration()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequest = this.ToDateBaseObjectForRetrieveChunkSearchAutoPRGeneration();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PurchaseRequest> PurchaseRequestlist = new List<PurchaseRequest>();

            foreach (b_PurchaseRequest PurchaseRequest in trans.PurchaseRequestList)
            {
                PurchaseRequest tmppurchaseRequest = new PurchaseRequest();
                tmppurchaseRequest.UpdateFromDatabaseObjectForRetriveForSearchAutoPRGeneration(PurchaseRequest);
                PurchaseRequestlist.Add(tmppurchaseRequest);
            }
            return PurchaseRequestlist;
        }
        public b_PurchaseRequest ToDateBaseObjectForRetrieveChunkSearchAutoPRGeneration()
        {
            b_PurchaseRequest dbObj = this.ToDatabaseObject();
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.VendorIDList = this.VendorIDList;
            dbObj.PartClientLookupId = this.PartClientLookupId;
            dbObj.Description = this.Description;
            dbObj.QtyToOrder = this.QtyToOrder;
            dbObj.UnitofMeasure = this.UnitofMeasure;
            dbObj.LastPurchaseCost = this.LastPurchaseCost;
            dbObj.VendorClientLookupId = this.VendorClientLookupId;
            dbObj.VendorName = this.VendorName;
            return dbObj;
        }

       
        public void UpdateFromDatabaseObjectForRetriveForSearchAutoPRGeneration(b_PurchaseRequest dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.PartID = dbObj.PartID;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.Description = dbObj.Description;
            this.QtyToOrder = dbObj.QtyToOrder;
            this.UnitofMeasure = dbObj.UnitofMeasure;
            this.LastPurchaseCost = dbObj.LastPurchaseCost;
            this.VendorClientLookupId = dbObj.VendorClientLookupId;
            this.VendorName = dbObj.VendorName;
            this.TotalCount = dbObj.TotalCount;

        }

        #endregion
        #region Create PR AutoGenerate  V2-643
        public long PurchaseRequestAutoGeneration_V2(DatabaseKey dbKey)
        {
            DataTable lulist = new DataTable("lulist");
            
            lulist = PartList;
            PurchaseRequest_AutoGeneration_V2 trans = new PurchaseRequest_AutoGeneration_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                lulist= lulist
            };
            trans.PurchaseRequest = this.ToDatabaseObject();
            trans.PurchaseRequest = this.ToDatabaseObjectExtendedAutoGeneration();

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            ProcessLogId = trans.PurchaseRequest.ProcessLogId;
            return ProcessLogId;
        }
        public b_PurchaseRequest ToDatabaseObjectExtendedAutoGeneration()
        {
            b_PurchaseRequest dbObj = this.ToDatabaseObject();
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.PartIDList = this.PartIDList;
            dbObj.Prefix = this.Prefix;
            return dbObj;
        }
        #endregion

        #region V2-693 SOMAX to SAP Purchase request export
        public void RetrieveByIdForExportSAP(DatabaseKey dbKey)
        {
            PurchaseRequest_RetrieveByIdForExportSAP trans = new PurchaseRequest_RetrieveByIdForExportSAP()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PurchaseRequest = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.PurchaseRequest);
            // Convert to User Time Zone
            //this.CreateDate = trans.PurchaseRequest.CreateDate.ToUserTimeZone(UserTimeZone);

            this.VendorClientLookupId = trans.PurchaseRequest.VendorClientLookupId;
            this.VendorIsExternal = trans.PurchaseRequest.VendorIsExternal;
            this.EXOracleUserId = trans.PurchaseRequest.ExOracleUserId;
            this.UserName = trans.PurchaseRequest.UserName;
        }
        #endregion

        #region V2-820

        public List<PurchaseRequest> RetrieveForPurchaseRequestWorkBenchRetrieveAll_V2(DatabaseKey dbKey, string TimeZone)
        {
            PurchaseRequest_RetrieveAllWorkbenchSearch_V2 trans = new PurchaseRequest_RetrieveAllWorkbenchSearch_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                UseTransaction = false
            };
            trans.PurchaseRequest = this.ToDatabaseObject();
            trans.PurchaseRequest.Created = this.Created;
            trans.PurchaseRequest.StatusDrop = this.StatusDrop;
            trans.PurchaseRequest.PersonnelId = this.PersonnelId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PurchaseRequest> PRlist = new List<PurchaseRequest>();

            foreach (b_PurchaseRequest PurchaseRequest in trans.purchaseRequestList)
            {
                PurchaseRequest tmpPurchaseRequest = new PurchaseRequest();
                tmpPurchaseRequest.UpdateFromDatabaseObjectForRetriveAll(PurchaseRequest, TimeZone);
                PRlist.Add(tmpPurchaseRequest);
            }
            return PRlist;
        }

        #endregion

        #region V2-945
        public PurchaseRequest RetrieveAllByPurchaseRequestV2Print(DatabaseKey dbKey, string TimeZone)
        {
            PurchaseRequest_RetrieveAllByPurchaseRequestV2Print trans = new PurchaseRequest_RetrieveAllByPurchaseRequestV2Print()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequest = this.ToDateBaseObjectForRetrieveAllByPurchaseRequestV2Print();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();



            List<PurchaseRequest> PurchaseRequestlist = new List<PurchaseRequest>();
            List<PRHeaderUDF> PRHeaderUDFlist = new List<PRHeaderUDF>();
            List<PurchaseRequestLineItem> PurchaseRequestLineItemlist = new List<PurchaseRequestLineItem>();
            List<PRLineUDF> PRLineUDFlist = new List<PRLineUDF>();

            this.listOfPR = new List<PurchaseRequest>();
            foreach (b_PurchaseRequest purchaseRequest in trans.PurchaseRequest.listOfPR)
            {
                PurchaseRequest tmppurchaseRequest = new PurchaseRequest();

                tmppurchaseRequest.UpdateFromDatabaseObjectPrintPurchaseRequestExtended(purchaseRequest, TimeZone);
                PurchaseRequestlist.Add(tmppurchaseRequest);
            }
            this.listOfPR.AddRange(PurchaseRequestlist);

            this.listOfPRHeaderUDF = new List<PRHeaderUDF>();
            foreach (b_PRHeaderUDF prHeaderUDF in trans.PurchaseRequest.listOfPRHeaderUDF)
            {
                PRHeaderUDF tmpPRHeaderUDF = new PRHeaderUDF();

                tmpPRHeaderUDF.UpdateFromDatabaseObjectPRHeaderUDFtPrintExtended(prHeaderUDF, TimeZone);
                PRHeaderUDFlist.Add(tmpPRHeaderUDF);
            }
            this.listOfPRHeaderUDF.AddRange(PRHeaderUDFlist);

            this.listOfPRLI = new List<PurchaseRequestLineItem>();
            foreach (b_PurchaseRequestLineItem PRLineItem in trans.PurchaseRequest.listOfPRLI)
            {
                PurchaseRequestLineItem tmpPRLineItem = new PurchaseRequestLineItem();

                tmpPRLineItem.UpdateFromDatabaseObjectPRLineItemPrintExtended(PRLineItem, TimeZone);
                PurchaseRequestLineItemlist.Add(tmpPRLineItem);
            }
            this.listOfPRLI.AddRange(PurchaseRequestLineItemlist);

            this.listOfPRLineUDF = new List<PRLineUDF>();
            foreach (b_PRLineUDF prLineUDF in trans.PurchaseRequest.listOfPRLineUDF)
            {
                PRLineUDF tmpPRLineUDF = new PRLineUDF();

                tmpPRLineUDF.UpdateFromDatabaseObjectPRLineUDFPrintExtended(prLineUDF, TimeZone);
                PRLineUDFlist.Add(tmpPRLineUDF);
            }
            this.listOfPRLineUDF.AddRange(PRLineUDFlist);

            return this;
        }
        public b_PurchaseRequest ToDateBaseObjectForRetrieveAllByPurchaseRequestV2Print()
        {
            b_PurchaseRequest dbObj = new b_PurchaseRequest();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PurchaseRequestIDList = this.PurchaseRequestIDList;

            return dbObj;
        }
        public void UpdateFromDatabaseObjectPrintPurchaseRequestExtended(b_PurchaseRequest dbObj, string Timezone)
        {
            this.ClientId = dbObj.ClientId;
            this.PurchaseRequestId = dbObj.PurchaseRequestId;
            this.SiteId = dbObj.SiteId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Reason = dbObj.Reason;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            this.VendorName = dbObj.VendorName;
            this.VendorEmailAddress = dbObj.VendorEmailAddress;
            this.VendorAddress1 = dbObj.VendorAddress1;
            this.VendorAddress2 = dbObj.VendorAddress2;
            this.VendorAddress3 = dbObj.VendorAddress3;
            this.VendorAddressCity = dbObj.VendorAddressCity;
            this.VendorAddressCountry = dbObj.VendorAddressCountry;
            this.VendorAddressPostCode = dbObj.VendorAddressPostCode;
            this.VendorAddressState = dbObj.VendorAddressState;
            this.SiteName = dbObj.SiteName;
            this.SiteAddress1 = dbObj.SiteAddress1;
            this.SiteAddress2 = dbObj.SiteAddress2;
            this.SiteAddress3 = dbObj.SiteAddress3;
            this.SiteAddressCity = dbObj.SiteAddressCity;
            this.SiteAddressCountry = dbObj.SiteAddressCountry;
            this.SiteAddressPostCode = dbObj.SiteAddressPostCode;
            this.SiteAddressState = dbObj.SiteAddressState;
            this.SiteBillToName = dbObj.SiteBillToName;
            this.SiteBillToAddress1 = dbObj.SiteBillToAddress1;
            this.SiteBillToAddress2 = dbObj.SiteBillToAddress2;
            this.SiteBillToAddress3 = dbObj.SiteBillToAddress3;
            this.SiteBillToAddressCity = dbObj.SiteBillToAddressCity;
            this.SiteBillToAddressCountry = dbObj.SiteBillToAddressCountry;
            this.SiteBillToAddressPostCode = dbObj.SiteBillToAddressPostCode;
            this.SiteBillToAddressState = dbObj.SiteBillToAddressState;
            this.Creator_PersonnelName = dbObj.Creator_PersonnelName;
        }
        #endregion

        #region   V2-1196
        public List<PurchaseRequest> RetrieveChunkSearchMultiStoreroomAutoPRGeneration(DatabaseKey dbKey)
        {
            PurchaseRequest_RetrieveChunkSearchMultiStoreroomAutoPRGeneration trans = new PurchaseRequest_RetrieveChunkSearchMultiStoreroomAutoPRGeneration()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequest = this.ToDateBaseObjectForRetrieveChunkSearchMultiStoreroomAutoPRGeneration();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PurchaseRequest> PurchaseRequestlist = new List<PurchaseRequest>();

            foreach (b_PurchaseRequest PurchaseRequest in trans.PurchaseRequestList)
            {
                PurchaseRequest tmppurchaseRequest = new PurchaseRequest();
                tmppurchaseRequest.UpdateFromDatabaseObjectForRetrieveForSearchMultiStoreroomAutoPRGeneration(PurchaseRequest);
                PurchaseRequestlist.Add(tmppurchaseRequest);
            }
            return PurchaseRequestlist;
        }
        public b_PurchaseRequest ToDateBaseObjectForRetrieveChunkSearchMultiStoreroomAutoPRGeneration()
        {
            b_PurchaseRequest dbObj = this.ToDatabaseObject();
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.VendorIDList = this.VendorIDList;
            dbObj.PartClientLookupId = this.PartClientLookupId;
            dbObj.Description = this.Description;
            dbObj.QtyToOrder = this.QtyToOrder;
            dbObj.UnitofMeasure = this.UnitofMeasure;
            dbObj.LastPurchaseCost = this.LastPurchaseCost;
            dbObj.VendorClientLookupId = this.VendorClientLookupId;
            dbObj.VendorName = this.VendorName;
            dbObj.StoreroomId = this.StoreroomId;
            return dbObj;
        }


        public void UpdateFromDatabaseObjectForRetrieveForSearchMultiStoreroomAutoPRGeneration(b_PurchaseRequest dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.PartID = dbObj.PartID;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.Description = dbObj.Description;
            this.QtyToOrder = dbObj.QtyToOrder;
            this.UnitofMeasure = dbObj.UnitofMeasure;
            this.LastPurchaseCost = dbObj.LastPurchaseCost;
            this.VendorClientLookupId = dbObj.VendorClientLookupId;
            this.VendorName = dbObj.VendorName;
            this.StoreroomId = dbObj.StoreroomId;
            this.TotalCount = dbObj.TotalCount;

        }

        #endregion
    }
}
