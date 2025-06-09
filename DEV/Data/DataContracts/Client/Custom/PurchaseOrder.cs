/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014-2017 by SOMAX Inc. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 2014-Oct-15  SOM-369   Roger Lawton      Moved the generation of the po number from 
*                                          the popup page to here. Did this because if you fail 
*                                          validation (for whatever reason) you "burn" a PO Number
*                                          Converted Create Date from UTC to User's TimeZone
*                                          Added using to using Common.Extensions
* 2017-Apr-04  SOM-1276  Nick Fuchs        Added Vendor Customer Account
* 2017-Sep-18  SOM-1413  Nick Fuchs        Added Buyer Name                                          
**************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Common.Extensions;
using Database;
using Database.Business;
using Database.Client.Custom.Transactions;

namespace DataContracts
{
    public partial class PurchaseOrder : DataContractBase, IStoredProcedureValidation
    {
        #region

        public WorkFlowLog workflowlog { get; set; }
        public DateTime CreateDate { get; set; }
        public string VendorName { get; set; }
        public string VendorPhoneNumber { get; set; }
        public string VendorCustomerAccount { get; set; }
        public string VendorEmailAddress { get; set; }
        public string VendorClientLookupId { get; set; }
        public string Creator_PersonnelName { get; set; }
        public string Completed_PersonnelName { get; set; }
        public string Buyer_PersonnelName { get; set; }
        public int CountLineItem { get; set; }
        public decimal TotalCost { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string DateRange { get; set; }
        public string DateColumn { get; set; }

        string ValidateFor = string.Empty;
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
        public string SiteBillToName { get; set; }
        public string SiteBillToAddress1 { get; set; }
        public string SiteBillToAddress2 { get; set; }
        public string SiteBillToAddress3 { get; set; }
        public string SiteBillToAddressCity { get; set; }
        public string SiteBillToAddressCountry { get; set; }
        public string SiteBillToAddressPostCode { get; set; }
        public string SiteBillToAddressState { get; set; }
        public string SiteBillToComment { get; set; }
        public string WorkFlowMessageForceComplete { get; set; }
        public Int64 FilterValue { get; set; }
        // som-939
        public long POImportId { get; set; }
        public string UnitOfMeasure { get; set; }
        //- SOM: 936
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string CreateBy { get; set; }
        public string PersonnelEmail { get; set; }

        //V2-305

        public string CreatedDate { get; set; }
        public string CompleteDT { get; set; }
        public string TtlCost { get; set; }

        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string SearchText { get; set; }
        public Int32 ChildCount { get; set; }
        public int TotalCount { get; set; }
        public UtilityAdd utilityAdd { get; set; }

        //V2-347
        public string CompleteStartDateVw { get; set; }
        public string CompleteEndDateVw { get; set; }
        public string StartCompleteDate { get; set; }
        public string EndCompleteDate { get; set; }
        public string StartCreateDate { get; set; }
        public string EndCreateDate { get; set; }

        //V2-364       
        public string strCreatedDate { get; set; }//V2-331
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string StoreroomName { get; set; }  //V2-738

        // V2-946
        public string PurchaseOrderIDList { get; set; }  

        public List<PurchaseOrder> listOfPO { get; set; }
        public List<PurchaseOrderLineItem> listOfPOLineItem { get; set; }

        public List<POHeaderUDF> listOfPOHeaderUDF { get; set; }

        public List<POLineUDF> listOfPOLineUDF { get; set; }
        public List<Attachment> listOfAttachment { get; set; } //V2-949

        //V2-947
        public decimal FreightAmount { get; set; }
        public string FreightBill { get; set; }
        public string PackingSlip { get; set; }
        public string Comments { get; set; }
        public string ReceivedPersonnelName { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public long ReceiptNumber { get; set; }
        //V2-947

        #region V2-1073
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AccountClientLookupId { get; set; }
        public string AccountName { get; set; }
        public int LineNumber { get; set; }
        public string POLDescription { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineTotal { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal ReceivedTotalCost { get; set; }
        public string LineItemStatus { get; set; }
        public string PartClientLookupId { get; set; }
        #endregion

        public string Shipto_ClientLookUpId { get; set; }  //V2-1086
        #region V2-1079
        public string TermDescription { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToAddressCity { get; set; }
        public string ShipToAddressState { get; set; }
        public string ShipToAddressPostCode { get; set; }
        public string ShipToAddressCountry { get; set; }
        public string Buyer_Phone { get; set; }
        #endregion

        //V2-1112
        public DateTime? StatusDate { get; set; }

        #endregion
        public void CreateByPKForeignKeys(DatabaseKey dbKey, bool AutoGenPONum, string AutoGenId, string Prefix)
        {
            Validate<PurchaseOrder>(dbKey);

            if (IsValid)
            {
                // SOM-369 - Generate the PO Number if appropriate
                // If not - check to make sure one exists
                // The stored procedure validation checks for duplicates but not if blank
                if (AutoGenPONum)
                {
                    ClientLookupId = CustomSequentialId.GetNextId(dbKey, AutoGenId, dbKey.User.DefaultSiteId, Prefix);
                }
                // SOM-369 - End
                PurchaseOrder_CreateByForeignKeys trans = new PurchaseOrder_CreateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.objworkflow = new b_WorkFlowLog();
                trans.objworkflow.ObjectName = workflowlog.ObjectName;
                trans.objworkflow.ClientId = workflowlog.ClientId;
                trans.objworkflow.Message = workflowlog.Message;
                trans.objworkflow.UserName = workflowlog.UserName;
                trans.PurchaseOrder = this.ToDatabaseObject();

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                UpdateFromDatabaseObject(trans.PurchaseOrder);
            }
        }
        public void CreateByPKForeignKeys_V2(DatabaseKey dbKey, bool AutoGenPONum, string AutoGenId, string Prefix,string Suffix=null)
        {
            Validate<PurchaseOrder>(dbKey);

            if (IsValid)
            {
                // SOM-369 - Generate the PO Number if appropriate
                // If not - check to make sure one exists
                // The stored procedure validation checks for duplicates but not if blank
                if (AutoGenPONum)
                {
                    ClientLookupId = CustomSequentialId.GetNextId(dbKey, AutoGenId, dbKey.User.DefaultSiteId, Prefix, Suffix);
                }
                // SOM-369 - End
                PurchaseOrder_CreateByForeignKeys_V2 trans = new PurchaseOrder_CreateByForeignKeys_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.objworkflow = new b_WorkFlowLog();
                trans.objworkflow.ObjectName = workflowlog.ObjectName;
                trans.objworkflow.ClientId = workflowlog.ClientId;
                trans.objworkflow.Message = workflowlog.Message;
                trans.objworkflow.UserName = workflowlog.UserName;
                trans.PurchaseOrder = this.ToDatabaseObject();

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                UpdateFromDatabaseObject(trans.PurchaseOrder);
            }
        }
        /// <summary>
        /// Update the Purchase Order Header based on the line item status values
        /// </summary>
        /// <param name="dbObjs"></param>
        /// <returns></returns>
        public void UpdateStatus(DatabaseKey dbKey) 
        {
            PurchaseOrder_UpdateStatus trans = new PurchaseOrder_UpdateStatus();
            trans.PurchaseOrder = this.ToDatabaseObject();
            trans.receiptheader = new b_POReceiptHeader();   // This is required by the stored procedure calling code
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.PurchaseOrder);
        }
        public static List<PurchaseOrder> UpdateFromDatabaseObjectList(List<b_PurchaseOrder> dbObjs)
        {
            List<PurchaseOrder> result = new List<PurchaseOrder>();

            foreach (b_PurchaseOrder dbObj in dbObjs)
            {
                PurchaseOrder tmp = new PurchaseOrder();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public List<PurchaseOrder> RetrieveByClientIDAndSiteIdAndClientLookUpId(DatabaseKey dbKey)
        {
            PurchaseOrder_RetrieveByClientIDAndSiteIdAndClientLookUpId trans = new PurchaseOrder_RetrieveByClientIDAndSiteIdAndClientLookUpId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PurchaseOrder = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.POList));

        }
        public void UpdateWithValidateByVendorPartNumberPurchaseRequestUnitOfMeasure(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateByVendorPartNumberPurchaseRequestUnitOfMeasure";
            Validate<PurchaseOrder>(dbKey);
            if (IsValid)
            {
                PurchaseOrderLineItems_Update trans = new PurchaseOrderLineItems_Update();
                trans.PurchaseOrder = this.ToDatabaseObject();
                trans.PurchaseOrder.POImportId = this.POImportId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.PurchaseOrder);
            }
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (ValidateFor == "ValidateByVendorPartNumberPurchaseRequestUnitOfMeasure")
            {
                PurchaseOrder_ValidateByVendorPartNumberPurchaseRequestUnitOfMeasure trans = new PurchaseOrder_ValidateByVendorPartNumberPurchaseRequestUnitOfMeasure()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PurchaseOrder = this.ToDatabaseObject();
                trans.PurchaseOrder.UnitOfMeasure = this.UnitOfMeasure;
                trans.PurchaseOrder.POImportId = this.POImportId;
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
                PurchaseOrder_ValidateByClientLookupId trans = new PurchaseOrder_ValidateByClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,

                };
                trans.PurchaseOrder = this.ToDatabaseObject();
                trans.PurchaseOrder.VendorClientLookupId = this.VendorClientLookupId;//--added on 09-12-2014---
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

        public void RetrieveByPKForeignKeys(DatabaseKey dbKey, string UserTimeZone)
        {
            PurchaseOrder_RetrieveByForeignKeys trans = new PurchaseOrder_RetrieveByForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PurchaseOrder = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.PurchaseOrder);
            // Convert to User Time Zone
            this.CreateDate = trans.PurchaseOrder.CreateDate.ToUserTimeZone(UserTimeZone);
            this.VendorName = trans.PurchaseOrder.VendorName;
            this.VendorPhoneNumber = trans.PurchaseOrder.VendorPhoneNumber;
            this.VendorCustomerAccount = trans.PurchaseOrder.VendorCustomerAccount;
            this.VendorEmailAddress = trans.PurchaseOrder.VendorEmailAddress;
            this.VendorClientLookupId = trans.PurchaseOrder.VendorClientLookupId;
            this.Creator_PersonnelName = trans.PurchaseOrder.Creator_PersonnelName;
            this.Completed_PersonnelName = trans.PurchaseOrder.Completed_PersonnelName;
            this.Buyer_PersonnelName = trans.PurchaseOrder.Buyer_PersonnelName;
            this.CountLineItem = trans.PurchaseOrder.CountLineItem;
            this.TotalCost = trans.PurchaseOrder.TotalCost;
            this.VendorAddress1 = trans.PurchaseOrder.VendorAddress1;
            this.VendorAddress2 = trans.PurchaseOrder.VendorAddress2;
            this.VendorAddress3 = trans.PurchaseOrder.VendorAddress3;
            this.VendorAddressCity = trans.PurchaseOrder.VendorAddressCity;
            this.VendorAddressCountry = trans.PurchaseOrder.VendorAddressCountry;
            this.VendorAddressPostCode = trans.PurchaseOrder.VendorAddressPostCode;
            this.VendorAddressState = trans.PurchaseOrder.VendorAddressState;
            this.VendorCustomerAccount = trans.PurchaseOrder.VendorCustomerAccount;
            this.SiteName = trans.PurchaseOrder.SiteName;
            this.SiteAddress1 = trans.PurchaseOrder.SiteAddress1;
            this.SiteAddress2 = trans.PurchaseOrder.SiteAddress2;
            this.SiteAddress3 = trans.PurchaseOrder.SiteAddress3;
            this.SiteAddressCity = trans.PurchaseOrder.SiteAddressCity;
            this.SiteAddressCountry = trans.PurchaseOrder.SiteAddressCountry;
            this.SiteAddressPostCode = trans.PurchaseOrder.SiteAddressPostCode;
            this.SiteAddressState = trans.PurchaseOrder.SiteAddressState;
            this.SiteBillToName = trans.PurchaseOrder.SiteBillToName;
            this.SiteBillToAddress1 = trans.PurchaseOrder.SiteBillToAddress1;
            this.SiteBillToAddress2 = trans.PurchaseOrder.SiteBillToAddress2;
            this.SiteBillToAddress3 = trans.PurchaseOrder.SiteBillToAddress3;
            this.SiteBillToAddressCity = trans.PurchaseOrder.SiteBillToAddressCity;
            this.SiteBillToAddressCountry = trans.PurchaseOrder.SiteBillToAddressCountry;
            this.SiteBillToAddressPostCode = trans.PurchaseOrder.SiteBillToAddressPostCode;
            this.SiteBillToAddressState = trans.PurchaseOrder.SiteBillToAddressState;
            this.SiteBillToComment = trans.PurchaseOrder.SiteBillToComment;
            this.ModifyBy = trans.PurchaseOrder.ModifyBy;
            this.ModifyDate = trans.PurchaseOrder.ModifyDate;
            this.CreateBy = trans.PurchaseOrder.CreateBy;
            this.PersonnelEmail = trans.PurchaseOrder.PersonnelEmail;

        }

        public List<PurchaseOrder> RetrieveAllForSearchNew(DatabaseKey dbKey)
        {
            PurchaseOrder_RetrieveAllForSearch trans = new PurchaseOrder_RetrieveAllForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseOrder = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PurchaseOrder> Purchaseorderlist = new List<PurchaseOrder>();

            foreach (b_PurchaseOrder Purchaseorder in trans.PurchaseOrderList)
            {
                PurchaseOrder tmppurchaseorder = new PurchaseOrder();
                tmppurchaseorder.UpdateFromDatabaseObjectForRetriveAllForSearch(Purchaseorder);
                Purchaseorderlist.Add(tmppurchaseorder);
            }
            return Purchaseorderlist;
        }

        //--SOM-892--//
        public List<PurchaseOrder> RetrieveAllForSearchNew(DatabaseKey dbKey, int filtervalue)
        {
            PurchaseOrder_RetrieveAllForSearch trans = new PurchaseOrder_RetrieveAllForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            FilterValue = filtervalue;
            trans.PurchaseOrder = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PurchaseOrder> Purchaseorderlist = new List<PurchaseOrder>();

            foreach (b_PurchaseOrder Purchaseorder in trans.PurchaseOrderList)
            {
                PurchaseOrder tmppurchaseorder = new PurchaseOrder();
                tmppurchaseorder.UpdateFromDatabaseObjectForRetriveAllForSearch(Purchaseorder);
                Purchaseorderlist.Add(tmppurchaseorder);
            }
            return Purchaseorderlist;
        }
        public b_PurchaseOrder ToDateBaseObjectForRetriveAllForSearch()
        {
            b_PurchaseOrder dbObj = this.ToDatabaseObject();

            dbObj.DateRange = this.DateRange;
            dbObj.DateColumn = this.DateColumn;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.orderbyColumn = this.orderbyColumn;

            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Status = this.Status;
            dbObj.VendorClientLookupId = this.VendorClientLookupId;
            dbObj.VendorName = this.VendorName;
            dbObj.Attention = this.Attention;
            dbObj.VendorPhoneNumber = this.VendorPhoneNumber;
            dbObj.Reason = this.Reason;
            dbObj.Buyer_PersonnelName = this.Buyer_PersonnelName;
            dbObj.TtlCost = this.TtlCost;
            dbObj.SearchText = this.SearchText;
            dbObj.FilterValue = this.FilterValue;
            dbObj.CompleteStartDateVw = this.CompleteStartDateVw;
            dbObj.CompleteEndDateVw = this.CompleteEndDateVw;
            dbObj.CreateStartDateVw = this.CreateStartDateVw;
            dbObj.CreateEndDateVw = this.CreateEndDateVw;
            dbObj.StartCompleteDate = this.StartCompleteDate;
            dbObj.EndCompleteDate = this.EndCompleteDate;
            dbObj.StartCreateDate = this.StartCreateDate;
            dbObj.EndCreateDate = this.EndCreateDate;
            dbObj.strCreatedDate = this.strCreatedDate;   //V2-331
            dbObj.Required = this.Required;   //V2-1171
            return dbObj;

        }
        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_PurchaseOrder dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.CreateDate = dbObj.CreateDate;
            this.VendorName = dbObj.VendorName;
            this.VendorPhoneNumber = dbObj.VendorPhoneNumber;
            this.VendorClientLookupId = dbObj.VendorClientLookupId;
            this.Creator_PersonnelName = dbObj.Creator_PersonnelName;
            this.Completed_PersonnelName = dbObj.Completed_PersonnelName;
            this.DateRange = dbObj.DateRange;
            this.DateColumn = dbObj.DateColumn;
            this.Buyer_PersonnelName = dbObj.Buyer_PersonnelName;
            this.TotalCost = dbObj.TotalCost;
            this.ChildCount = dbObj.ChildCount;
            this.TotalCount = dbObj.TotalCount;
            this.strCreatedDate = dbObj.strCreatedDate;//V2-331
            this.Required = dbObj.Required;//V2-1171

            switch (this.Status)
            {
                case Common.Constants.PurchaseOrderStatusConstants.Complete:
                    this.Status = "Complete";
                    break;
                case Common.Constants.PurchaseOrderStatusConstants.Open:
                    this.Status = "Open";
                    break;
                case Common.Constants.PurchaseOrderStatusConstants.Partial:
                    this.Status = "Partial";
                    break;
                case Common.Constants.PurchaseOrderStatusConstants.Void:
                    this.Status = "Void";
                    break;
                default:
                    this.Status = string.Empty;
                    break;
            }

        }
        
        #region V2-331
        public List<PurchaseOrder> RetrievePOReceiptChunkSearch(DatabaseKey dbKey)
        {
            PurchaseOrderReceipt_RetrieveChunkSearch trans = new PurchaseOrderReceipt_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseOrder = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            List<PurchaseOrder> PurchaseOrderlist = new List<PurchaseOrder>();

            foreach (b_PurchaseOrder PurchaseOrder in trans.PurchaseOrderList)
            {
                PurchaseOrder tmpPurchaseOrder = new PurchaseOrder();
                tmpPurchaseOrder.UpdateFromDatabaseObjectForRetriveAllForSearch(PurchaseOrder);
                PurchaseOrderlist.Add(tmpPurchaseOrder);
            }
            return PurchaseOrderlist;
        }
        #endregion
        public void UpdateByPurchaseOrderForceComplete(DatabaseKey dbKey)
        {

            PurchaseOrder_UpdateByForceComplete trans = new PurchaseOrder_UpdateByForceComplete()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PurchaseOrder = this.ToDatabaseObject();
            trans.PurchaseOrder.WorkFlowMessageForceComplete = this.WorkFlowMessageForceComplete;
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }

        public List<PurchaseOrder> RetrieveAll(DatabaseKey dbKey)
        {
            //PurchaseOrder_Retrieve trans = new PurchaseOrder_Retrieve();
            //trans.PurchaseOrder = this.ToDatabaseObject();
            //trans.dbKey = dbKey.ToTransDbKey();
            //trans.Execute();
            //UpdateFromDatabaseObject(trans.PurchaseOrder);

            PurchaseOrder_RetrieveAll_V2 trans = new PurchaseOrder_RetrieveAll_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PurchaseOrder> PurchaseOrderList = new List<PurchaseOrder>();
            foreach (b_PurchaseOrder PurchaseOrder in trans.PurchaseOrderList)
            {
                PurchaseOrder tmpPurchaseOrder = new PurchaseOrder();

                tmpPurchaseOrder.UpdateFromDatabaseObject(PurchaseOrder);
                PurchaseOrderList.Add(tmpPurchaseOrder);
            }
            return PurchaseOrderList;
        }

        public List<PurchaseOrder> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            PurchaseOrder_RetrieveChunkSearch trans = new PurchaseOrder_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseOrder = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();           

            List<PurchaseOrder> PurchaseOrderlist = new List<PurchaseOrder>();

            foreach (b_PurchaseOrder PurchaseOrder in trans.PurchaseOrderList)
            {
                PurchaseOrder tmpPurchaseOrder = new PurchaseOrder();
                tmpPurchaseOrder.UpdateFromDatabaseObjectForRetriveAllForSearch(PurchaseOrder);
                PurchaseOrderlist.Add(tmpPurchaseOrder);
            }
            return PurchaseOrderlist;
        }

        #region V2-738
        public void RetrieveByPKForeignKeys_V2(DatabaseKey dbKey, string UserTimeZone)
        {
            PurchaseOrder_RetrieveByForeignKeys_V2 trans = new PurchaseOrder_RetrieveByForeignKeys_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PurchaseOrder = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.PurchaseOrder);
            // Convert to User Time Zone
            this.CreateDate = trans.PurchaseOrder.CreateDate.ToUserTimeZone(UserTimeZone);
            this.VendorName = trans.PurchaseOrder.VendorName;
            this.VendorPhoneNumber = trans.PurchaseOrder.VendorPhoneNumber;
            this.VendorCustomerAccount = trans.PurchaseOrder.VendorCustomerAccount;
            this.VendorEmailAddress = trans.PurchaseOrder.VendorEmailAddress;
            this.VendorClientLookupId = trans.PurchaseOrder.VendorClientLookupId;
            this.Creator_PersonnelName = trans.PurchaseOrder.Creator_PersonnelName;
            this.Completed_PersonnelName = trans.PurchaseOrder.Completed_PersonnelName;
            this.Buyer_PersonnelName = trans.PurchaseOrder.Buyer_PersonnelName;
            this.CountLineItem = trans.PurchaseOrder.CountLineItem;
            this.TotalCost = trans.PurchaseOrder.TotalCost;
            this.VendorAddress1 = trans.PurchaseOrder.VendorAddress1;
            this.VendorAddress2 = trans.PurchaseOrder.VendorAddress2;
            this.VendorAddress3 = trans.PurchaseOrder.VendorAddress3;
            this.VendorAddressCity = trans.PurchaseOrder.VendorAddressCity;
            this.VendorAddressCountry = trans.PurchaseOrder.VendorAddressCountry;
            this.VendorAddressPostCode = trans.PurchaseOrder.VendorAddressPostCode;
            this.VendorAddressState = trans.PurchaseOrder.VendorAddressState;
            this.VendorCustomerAccount = trans.PurchaseOrder.VendorCustomerAccount;
            this.SiteName = trans.PurchaseOrder.SiteName;
            this.SiteAddress1 = trans.PurchaseOrder.SiteAddress1;
            this.SiteAddress2 = trans.PurchaseOrder.SiteAddress2;
            this.SiteAddress3 = trans.PurchaseOrder.SiteAddress3;
            this.SiteAddressCity = trans.PurchaseOrder.SiteAddressCity;
            this.SiteAddressCountry = trans.PurchaseOrder.SiteAddressCountry;
            this.SiteAddressPostCode = trans.PurchaseOrder.SiteAddressPostCode;
            this.SiteAddressState = trans.PurchaseOrder.SiteAddressState;
            this.SiteBillToName = trans.PurchaseOrder.SiteBillToName;
            this.SiteBillToAddress1 = trans.PurchaseOrder.SiteBillToAddress1;
            this.SiteBillToAddress2 = trans.PurchaseOrder.SiteBillToAddress2;
            this.SiteBillToAddress3 = trans.PurchaseOrder.SiteBillToAddress3;
            this.SiteBillToAddressCity = trans.PurchaseOrder.SiteBillToAddressCity;
            this.SiteBillToAddressCountry = trans.PurchaseOrder.SiteBillToAddressCountry;
            this.SiteBillToAddressPostCode = trans.PurchaseOrder.SiteBillToAddressPostCode;
            this.SiteBillToAddressState = trans.PurchaseOrder.SiteBillToAddressState;
            this.SiteBillToComment = trans.PurchaseOrder.SiteBillToComment;
            this.ModifyBy = trans.PurchaseOrder.ModifyBy;
            this.ModifyDate = trans.PurchaseOrder.ModifyDate;
            this.CreateBy = trans.PurchaseOrder.CreateBy;
            this.PersonnelEmail = trans.PurchaseOrder.PersonnelEmail;
            this.StoreroomName = trans.PurchaseOrder.StoreroomName;
            //V2-1086
            this.Shipto = trans.PurchaseOrder.Shipto;
            this.Shipto_ClientLookUpId = trans.PurchaseOrder.Shipto_ClientLookUpId;
        }
        #endregion

        public PurchaseOrder RetrieveAllByPurchaseOrdeV2Print(DatabaseKey dbKey, string TimeZone)
        {
            PurchaseOrder_RetrieveAllByPurchaseOrdeV2Print trans = new PurchaseOrder_RetrieveAllByPurchaseOrdeV2Print()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseOrder = this.ToDateBaseObjectForRetrieveAllByPurchaseOrdeV2Print();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            List<PurchaseOrder> PurchaseOrderlist = new List<PurchaseOrder>();
            List<PurchaseOrderLineItem> PurchaseOrderLineItemlist = new List<PurchaseOrderLineItem>();
            List<POHeaderUDF> PoHeaderUDFlist = new List<POHeaderUDF>();
            List<POLineUDF> POLineUDFlist = new List<POLineUDF>();
            List<Attachment> Attachmentlist = new List<Attachment>();

            this.listOfPO = new List<PurchaseOrder>();
            foreach (b_PurchaseOrder purchaseorder in trans.PurchaseOrder.listOfPO)
            {
                PurchaseOrder tmppurchaseorder = new PurchaseOrder();

                tmppurchaseorder.UpdateFromDatabaseObjectPrintPurchaseOrderExtended(purchaseorder, TimeZone);
                PurchaseOrderlist.Add(tmppurchaseorder);
            }
            this.listOfPO.AddRange(PurchaseOrderlist);

            this.listOfPOLineItem = new List<PurchaseOrderLineItem>();
            foreach (b_PurchaseOrderLineItem purchaseorderlineitem in trans.PurchaseOrder.listOfPOLineItem)
            {
                PurchaseOrderLineItem tmpPurchaseOrderLineItem = new PurchaseOrderLineItem();

                // Alist = UpdateFromDatabaseObjectList(trans.AttachmentList, Timezone, dbKey.User.IsSuperUser, dbKey.Personnel.PersonnelId, edit_secure).ToList();
                tmpPurchaseOrderLineItem.UpdateFromDatabaseObjectPurchaseOrderLineItemPrintExtended(purchaseorderlineitem, TimeZone);
                PurchaseOrderLineItemlist.Add(tmpPurchaseOrderLineItem);
            }
            this.listOfPOLineItem.AddRange(PurchaseOrderLineItemlist);

            this.listOfPOHeaderUDF = new List<POHeaderUDF>();
            foreach (b_POHeaderUDF POHeaderUDF in trans.PurchaseOrder.listOfPOHeaderUDF)
            {
                POHeaderUDF tmppoheaderudf = new POHeaderUDF();

                tmppoheaderudf.UpdateFromDatabaseObjectPOHeaderUDFPrintExtended(POHeaderUDF, TimeZone);
                PoHeaderUDFlist.Add(tmppoheaderudf);
            }
            this.listOfPOHeaderUDF.AddRange(PoHeaderUDFlist);

            this.listOfPOLineUDF = new List<POLineUDF>();
            foreach (b_POLineUDF POLineUDF in trans.PurchaseOrder.listOfPOLineUDF)
            {
                POLineUDF tmpPOLineUDF = new POLineUDF();

                tmpPOLineUDF.UpdateFromDatabaseObjectPOLineUDFPrintExtended(POLineUDF, TimeZone);
                POLineUDFlist.Add(tmpPOLineUDF);
            }
            this.listOfPOLineUDF.AddRange(POLineUDFlist);

            this.listOfAttachment = new List<Attachment>();
            foreach (b_Attachment attachment in trans.PurchaseOrder.listOfAttachment)
            {
                Attachment tmpAttachment = new Attachment();

                tmpAttachment.UpdateFromDatabaseObjectPOAttachmentPrintExtended(attachment, TimeZone);
                Attachmentlist.Add(tmpAttachment);
            }
            this.listOfAttachment.AddRange(Attachmentlist);

            return this;
        }


        public void UpdateFromDatabaseObjectPrintPurchaseOrderExtended(b_PurchaseOrder dbObj, string Timezone)
        { // this.UpdateFromDatabaseObject(dbObj);
            this.ClientId = dbObj.ClientId;
            this.PurchaseOrderId = dbObj.PurchaseOrderId;
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
            if (dbObj.Required != null && dbObj.Required != DateTime.MinValue)
            {
                this.Required = dbObj.Required.ToUserTimeZone(Timezone);
            }
            else
            {
                this.Required = dbObj.Required;
            }
            this.Carrier = dbObj.Carrier;
            this.Attention = dbObj.Attention;
            this.MessageToVendor = dbObj.MessageToVendor;
            this.Terms = dbObj.Terms;
            this.FOB = dbObj.FOB;
            this.VendorCustomerAccount = dbObj.VendorCustomerAccount;
            this.Buyer_PersonnelName = dbObj.Buyer_PersonnelName;
            this.VendorName = dbObj.VendorName;
            this.VendorEmailAddress = dbObj.VendorEmailAddress;
            this.VendorAddress1 = dbObj.VendorAddress1;
            this.VendorAddress2 = dbObj.VendorAddress2;
            this.VendorAddress3 = dbObj.VendorAddress3;
            this.VendorAddressCity = dbObj.VendorAddressCity;
            this.VendorAddressCountry = dbObj.VendorAddressCountry;           
            this.VendorAddressPostCode = dbObj.VendorAddressPostCode;
            this.VendorAddressState = dbObj.VendorAddressState;
            this.VendorPhoneNumber = dbObj.VendorPhoneNumber;
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
            this.SiteBillToComment = dbObj.SiteBillToComment;
            this.Creator_PersonnelName = dbObj.Creator_PersonnelName;            
        }     
        public b_PurchaseOrder ToDateBaseObjectForRetrieveAllByPurchaseOrdeV2Print()
        {
            b_PurchaseOrder dbObj = new b_PurchaseOrder();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PurchaseOrderIDList = this.PurchaseOrderIDList;

            return dbObj;
        }
        #region V2-947
        public void UpdateFromDatabaseObjectRetrieveDetailsPOR(b_PurchaseOrder dbObj, string Timezone)
        {   //vendor
            this.VendorName = dbObj.VendorName;
            this.VendorEmailAddress = dbObj.VendorEmailAddress;
            this.VendorAddress1 = dbObj.VendorAddress1;
            this.VendorAddress2 = dbObj.VendorAddress2;
            this.VendorAddress3 = dbObj.VendorAddress3;
            this.VendorAddressCity = dbObj.VendorAddressCity;
            this.VendorAddressCountry = dbObj.VendorAddressCountry;
            this.VendorAddressPostCode = dbObj.VendorAddressPostCode;
            this.VendorAddressState = dbObj.VendorAddressState;
            //
            this.ClientLookupId = dbObj.ClientLookupId;
            this.ReceiptNumber = dbObj.ReceiptNumber;
            this.ReceiveDate = dbObj.ReceiveDate;
            this.ReceivedPersonnelName = dbObj.ReceivedPersonnelName;
            this.Comments = dbObj.Comments;
            //site
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
           //
            this.Carrier = dbObj.Carrier;
            this.Attention = dbObj.Attention;
            this.MessageToVendor = dbObj.MessageToVendor;
            this.PackingSlip = dbObj.PackingSlip;
            this.FreightAmount = dbObj.FreightAmount;
            this.FreightBill = dbObj.FreightBill;
        }

        #endregion


        #region V2-981
        public b_PurchaseOrder ToDateBaseObjectForPurchaseOrderLookuplistChunkSearch()
        {
            b_PurchaseOrder dbObj = this.ToDatabaseObject();
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Status = this.Status;
            dbObj.VendorClientLookupId = this.VendorClientLookupId;
            dbObj.VendorName = this.VendorName;
            return dbObj;
        }
        public List<PurchaseOrder> GetAllPurchaseOrderLookupListV2(DatabaseKey dbKey, string TimeZone)
        {
            PurchaseOrder_RetrieveChunkSearchLookupListV2 trans = new PurchaseOrder_RetrieveChunkSearchLookupListV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseOrder = this.ToDateBaseObjectForPurchaseOrderLookuplistChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfPO = new List<PurchaseOrder>();

            List<PurchaseOrder> PurchaseOrderlist = new List<PurchaseOrder>();
            foreach (b_PurchaseOrder purchaseorder in trans.PurchaseOrderList)
            {
                PurchaseOrder tmpPurchaseOrder = new PurchaseOrder();

                tmpPurchaseOrder.UpdateFromDatabaseObjectForPurchaseorderLookupListChunkSearch(purchaseorder, TimeZone);
                PurchaseOrderlist.Add(tmpPurchaseOrder);
            }
            return PurchaseOrderlist;
        }
        public void UpdateFromDatabaseObjectForPurchaseorderLookupListChunkSearch(b_PurchaseOrder dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.TotalCount = dbObj.TotalCount;
            this.Status = dbObj.Status;
            this.VendorClientLookupId = dbObj.VendorClientLookupId;
            this.VendorName = dbObj.VendorName;
        }
        #endregion

        #region V2-1073
        public List<PurchaseOrder> RetrievePOByAccountReport(DatabaseKey dbKey)
        {
            PurchaseOrder_RetrievePOByAccountReport trans = new PurchaseOrder_RetrievePOByAccountReport()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseOrder = this.ToDateBaseObjectForPOByAccountReport();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PurchaseOrder> PurchaseOrderlist = new List<PurchaseOrder>();

            this.listOfPO = new List<PurchaseOrder>();
            foreach (b_PurchaseOrder PurchaseOrder in trans.PurchaseOrderList)
            {
                PurchaseOrder tmpPurchaseOrder = new PurchaseOrder();
                tmpPurchaseOrder.UpdateFromDatabaseObjectForPOByAccountReport(PurchaseOrder);
                PurchaseOrderlist.Add(tmpPurchaseOrder);
            }
            this.listOfPO.AddRange(PurchaseOrderlist);
            return PurchaseOrderlist;
        }

        public b_PurchaseOrder ToDateBaseObjectForPOByAccountReport()
        {
            b_PurchaseOrder dbObj = new b_PurchaseOrder();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.StartDate = this.StartDate;
            dbObj.EndDate = this.EndDate;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForPOByAccountReport(b_PurchaseOrder dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.AccountClientLookupId = dbObj.AccountClientLookupId;
            this.AccountName = dbObj.AccountName;
            this.VendorClientLookupId = dbObj.VendorClientLookupId;
            this.VendorName = dbObj.VendorName;
            this.LineNumber = dbObj.LineNumber;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Reason = dbObj.Reason;
            this.CreateDate = dbObj.CreateDate;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.POLDescription = dbObj.POLDescription;
            this.OrderQuantity = dbObj.OrderQuantity;
            this.UnitOfMeasure = dbObj.UnitOfMeasure;
            this.UnitCost = dbObj.UnitCost;
            this.LineTotal = dbObj.LineTotal;
            this.QuantityReceived = dbObj.QuantityReceived;
            this.ReceivedTotalCost = dbObj.ReceivedTotalCost;
            this.LineItemStatus = dbObj.LineItemStatus;
            this.Status = dbObj.Status;
        }
        #endregion

        #region V2-1079
        public void RetrieveForEDIExport_V2(DatabaseKey dbKey, string UserTimeZone)
        {
            PurchaseOrder_RetrieveForEDIExport_V2 trans = new PurchaseOrder_RetrieveForEDIExport_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PurchaseOrder = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObjectForEDIExport(trans.PurchaseOrder, UserTimeZone);
        }
        public void UpdateFromDatabaseObjectForEDIExport(b_PurchaseOrder dbObj, string UserTimeZone)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Attention = dbObj.Attention;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(UserTimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            this.TermDescription = dbObj.TermDescription;
            this.Carrier = dbObj.Carrier;
            this.Shipto_ClientLookUpId = dbObj.Shipto_ClientLookUpId;
            this.ShipToName = dbObj.ShipToName;
            this.ShipToAddress1 = dbObj.ShipToAddress1;
            this.ShipToAddress2 = dbObj.ShipToAddress2;
            this.ShipToAddress3 = dbObj.ShipToAddress3;
            this.ShipToAddressCity = dbObj.ShipToAddressCity;
            this.ShipToAddressCountry = dbObj.ShipToAddressCountry;
            this.ShipToAddressPostCode = dbObj.ShipToAddressPostCode;
            this.ShipToAddressState = dbObj.ShipToAddressState;
            this.VendorName = dbObj.VendorName;
            this.VendorPhoneNumber = dbObj.VendorPhoneNumber;
            this.VendorClientLookupId = dbObj.VendorClientLookupId;
            this.Buyer_PersonnelName = dbObj.Buyer_PersonnelName;
            this.Buyer_Phone = dbObj.Buyer_Phone;
            this.VendorAddress1 = dbObj.VendorAddress1;
            this.VendorAddress2 = dbObj.VendorAddress2;
            this.VendorAddress3 = dbObj.VendorAddress3;
            this.VendorAddressCity = dbObj.VendorAddressCity;
            this.VendorAddressCountry = dbObj.VendorAddressCountry;
            this.VendorAddressPostCode = dbObj.VendorAddressPostCode;
            this.VendorAddressState = dbObj.VendorAddressState;
            this.TotalCost = dbObj.TotalCost;
        }
        public List<PurchaseOrder> RetrieveByClientIDAndSiteIdAndClientLookUpId_V2(DatabaseKey dbKey)
        {
            PurchaseOrder_RetrieveByClientIDAndSiteIdAndClientLookUpId_V2 trans = new PurchaseOrder_RetrieveByClientIDAndSiteIdAndClientLookUpId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PurchaseOrder = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.POList));

        }
        #endregion


        #region V2-1112 RetrieveBy EPM PurchaseOrde Print V2
        public PurchaseOrder RetrieveByEPMPurchaseOrdePrintV2(DatabaseKey dbKey, string TimeZone)
        {
            PurchaseOrder_RetrieveByEPMPurchaseOrdePrintV2 trans = new PurchaseOrder_RetrieveByEPMPurchaseOrdePrintV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseOrder = this.ToDateBaseObjectForRetrieveByEPMPurchaseOrdePrintV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            List<PurchaseOrder> PurchaseOrderlist = new List<PurchaseOrder>();
            List<PurchaseOrderLineItem> PurchaseOrderLineItemlist = new List<PurchaseOrderLineItem>();

            this.listOfPO = new List<PurchaseOrder>();
            foreach (b_PurchaseOrder purchaseorder in trans.PurchaseOrder.listOfPO)
            {
                PurchaseOrder tmppurchaseorder = new PurchaseOrder();

                tmppurchaseorder.UpdateFromDatabaseObjectPrintPurchaseOrderEPMExtended(purchaseorder, TimeZone);
                PurchaseOrderlist.Add(tmppurchaseorder);
            }
            this.listOfPO.AddRange(PurchaseOrderlist);

            this.listOfPOLineItem = new List<PurchaseOrderLineItem>();
            foreach (b_PurchaseOrderLineItem purchaseorderlineitem in trans.PurchaseOrder.listOfPOLineItem)
            {
                PurchaseOrderLineItem tmpPurchaseOrderLineItem = new PurchaseOrderLineItem();
                tmpPurchaseOrderLineItem.UpdateFromDatabaseObjectPurchaseOrderLineItemEPMPrintExtended(purchaseorderlineitem, TimeZone);
                PurchaseOrderLineItemlist.Add(tmpPurchaseOrderLineItem);
            }
            this.listOfPOLineItem.AddRange(PurchaseOrderLineItemlist);

            return this;
        }
        public void UpdateFromDatabaseObjectPrintPurchaseOrderEPMExtended(b_PurchaseOrder dbObj, string Timezone)
        { 
            this.ClientId = dbObj.ClientId;
            this.PurchaseOrderId = dbObj.PurchaseOrderId;
            this.SiteId = dbObj.SiteId;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Status = dbObj.Status;
            if (dbObj.StatusDate != null && dbObj.StatusDate != DateTime.MinValue)
            {
                this.StatusDate = dbObj.StatusDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.StatusDate = dbObj.StatusDate;
            }
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CompleteDate = dbObj.CompleteDate;
            }
            if (dbObj.VoidDate != null && dbObj.VoidDate != DateTime.MinValue)
            {
                this.VoidDate = dbObj.VoidDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.VoidDate = dbObj.VoidDate;
            }
            this.StoreroomId = dbObj.StoreroomId;
            this.Shipto = dbObj.Shipto;
            this.Shipto_ClientLookUpId = dbObj.Shipto_ClientLookUpId;

            this.ShipToAddress1 = dbObj.ShipToAddress1;
            this.ShipToAddress2 = dbObj.ShipToAddress2;
            this.ShipToAddress3 = dbObj.ShipToAddress3;
            this.ShipToAddressCity= dbObj.ShipToAddressCity;
            this.ShipToAddressState = dbObj.ShipToAddressState;
            this.ShipToAddressPostCode = dbObj.ShipToAddressPostCode;
            this.ShipToAddressCountry = dbObj.ShipToAddressCountry;

            this.VendorName = dbObj.VendorName;
            this.VendorAddress1 = dbObj.VendorAddress1;
            this.VendorAddress2 = dbObj.VendorAddress2;
            this.VendorAddress3 = dbObj.VendorAddress3;
            this.VendorAddressCity = dbObj.VendorAddressCity;
            this.VendorAddressState = dbObj.VendorAddressState;
            this.VendorAddressPostCode = dbObj.VendorAddressPostCode;
            this.VendorAddressCountry = dbObj.VendorAddressCountry;

            this.SiteName = dbObj.SiteName;
            this.SiteBillToName = dbObj.SiteBillToName;
            this.SiteAddress1 = dbObj.SiteAddress1;
            this.SiteAddress2 = dbObj.SiteAddress2;
            this.SiteAddress3 = dbObj.SiteAddress3;
            this.SiteAddressCity = dbObj.SiteAddressCity;
            this.SiteAddressState = dbObj.SiteAddressState; 
            this.SiteAddressPostCode = dbObj.SiteAddressPostCode;
            this.SiteAddressCountry = dbObj.SiteAddressCountry;
            if (dbObj.Required != null && dbObj.Required != DateTime.MinValue)
            {
                this.Required = dbObj.Required.ToUserTimeZone(Timezone);
            }
            else
            {
                this.Required = dbObj.Required;
            }
            this.Terms = dbObj.Terms;
            this.Carrier = dbObj.Carrier;
            this.Attention = dbObj.Attention;
        }
        public b_PurchaseOrder ToDateBaseObjectForRetrieveByEPMPurchaseOrdePrintV2()
        {
            b_PurchaseOrder dbObj = new b_PurchaseOrder();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PurchaseOrderIDList = this.PurchaseOrderIDList;

            return dbObj;
        }
        #endregion

    }
}
