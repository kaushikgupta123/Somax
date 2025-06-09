
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
* Date        JIRA Item Person            Description
* =========== ========= ================= =======================================================
* 2014-Oct-12 SOM-363   Roger Lawton      Removed the Cost property (duplicate of Cost in the 
*                                         generated part of the class)
* 2015-Apr-02 SOM-619   Roger Lawton      Changed Physical Inventory Validation
**************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Database.Business;
using Database;
using static Database.PartHistory_RetrieveByWorkOrderId;

namespace DataContracts
{

    /// <summary>
    /// Business object that stores a record from the PartHistory table.
    /// </summary>
    /// 
    [JsonObject]
    public partial class PartHistory : DataContractBase, IStoredProcedureValidation
    {
        #region Constants
        private const int OUT_STOCK_ERROR_CODE = 43;
        private const int ISSUE_DATE_ERROR_CODE = 44;
        public decimal TotalCost { get; set; }

        //--v2-289--
        public string PersonnelClientLookupId { get; set; }
        public string NameLast { get; set; }
        public string NameFirst { get; set; }
        public string NameMiddle { get; set; }
        public string PersonnelInitial { get; set; }
        public string VMRSFailureCode { get; set; }
        public bool MultiStoreroom { get; set; }
        public bool IspartIssueStockOut { get; set; } //V2-1031
        #endregion

        #region Private Variables
        private bool m_validateProcess;
        private bool m_validateAdd;
        private bool m_validateInventoryReceiptAdd;
        private bool m_validateInventoryReceiptProcess;
        private bool m_validatePhysicalInventoryRecordCount;
        private bool m_validateWOPartsAdd;
        private bool m_validateWOPartsProcess;
        #endregion



        #region Public Methods
        public b_PartHistory ToExtendedDatabaseObject()
        {
            b_PartHistory dbObj = this.ToDatabaseObject();

            dbObj.IssueToClientLookupId = this.IssueToClientLookupId;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.PartClientLookupId = this.PartClientLookupId;
            dbObj.SiteId = this.SiteId;
            dbObj.IsPartIssue = this.IsPartIssue;
            dbObj.IsPerformAdjustment = this.IsPerformAdjustment;
            dbObj.InventoryReceiptId = this.InventoryReceiptId;
            dbObj.IsInventoryReceipt = this.IsInventoryReceipt;
            dbObj.Cost = this.Cost;
            dbObj.PartUPCCode = this.PartUPCCode;
            dbObj.PartAverageCost = this.PartAverageCost;
            dbObj.PhysicalInventoryId = this.PhysicalInventoryId;
            dbObj.IsPhysicalInventory = this.IsPhysicalInventory;
            dbObj.PartDescription = this.PartDescription;
            dbObj.PartStoreroomQtyOnHand = this.PartStoreroomQtyOnHand;
            dbObj.PartQtyCounted = this.PartQtyCounted;
            dbObj.PartStorerommLocation1_1 = this.PartStorerommLocation1_1;
            dbObj.PartStorerommLocation1_2 = this.PartStorerommLocation1_2;
            dbObj.PartStorerommLocation1_3 = this.PartStorerommLocation1_3;
            dbObj.PartStorerommLocation1_4 = this.PartStorerommLocation1_4;
            dbObj.PartStoreroomUpdateIndex = this.PartStoreroomUpdateIndex;
            dbObj.Personnel = this.Personnel;
            dbObj.IssuedTo = this.IssuedTo;
            dbObj.Flag = this.Flag;
            dbObj.TotalCount = this.TotalCount;
            return dbObj;
        }

        #endregion

        #region Transaction Methods

        public PartHistory ValidateAdd(DatabaseKey dbKey)
        {
            m_validateProcess = false;
            m_validateAdd = true;
            m_validateInventoryReceiptAdd = false;
            m_validateInventoryReceiptProcess = false;
            m_validatePhysicalInventoryRecordCount = false;

            Validate<PartHistory>(dbKey);
            if (this.TransactionDate.HasValue && this.TransactionDate.Value.Date.CompareTo(DateTime.Now.AddDays(1).Date) > 0)
            {

            }
            // return objPartHistory;
            //return null;
            return this;
        }
        public List<PartHistory> RetriveByWorkOrderId(DatabaseKey dbKey)
        {
            PartHistory_RetrieveByWorkOrderId trans = new PartHistory_RetrieveByWorkOrderId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartHistory = this.ToDatabaseObject();
            trans.PartHistory = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PartHistory.UpdateFromDatabaseObjectList(trans.PartHistoryList);
        }

        public List<PartHistory> ReturnRetriveByWorkOrderId(DatabaseKey dbKey)
        {
            PartHistory_ReturnRetrieveByWorkOrderId trans = new PartHistory_ReturnRetrieveByWorkOrderId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartHistory = this.ToDatabaseObject();
            trans.PartHistory = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PartHistory.UpdateFromDatabaseObjectListExtended(trans.PartHistoryList);
        }
        public List<PartHistory> RetriveByServiceOrderId(DatabaseKey dbKey)
        {
            PartHistory_RetrieveByServiceOrderId trans = new PartHistory_RetrieveByServiceOrderId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartHistory = this.ToDatabaseObject();
            trans.PartHistory = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PartHistory.UpdateFromDatabaseObjectList(trans.PartHistoryList);
        }
        public void ValidateInventoryReceiptAdd(DatabaseKey dbKey)
        {
            m_validateProcess = false;
            m_validateAdd = false;
            m_validateInventoryReceiptAdd = true;
            m_validateInventoryReceiptProcess = false;
            m_validatePhysicalInventoryRecordCount = false;

            Validate<PartHistory>(dbKey);
            //if (this.TransactionDate.HasValue && this.TransactionDate.Value.Date.CompareTo(DateTime.Now.AddDays(1).Date) > 0)
            //{
            //    Business.Localization.ValidationError locValidationError =
            //        loc.StoredProcValidation.ValidationError.Find(v => v.Code == ISSUE_DATE_ERROR_CODE);
            //    if (locValidationError != null) { ErrorMessages.Add(locValidationError.Message); }
            //}
        }



        public List<PartHistory> RetriveByEquipmentId(DatabaseKey dbKey)
        {
            PartHistory_RetriveByEquipmentId trans = new PartHistory_RetriveByEquipmentId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartHistory = this.ToDatabaseObjectDB();
            trans.PartHistory = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PartHistory.UpdateFromDatabaseObjectListPart(trans.PartHistoryList);
        }

        private b_PartHistory ToDatabaseObjectDB()
        {
            b_PartHistory dbObj = new b_PartHistory();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PartId = this.PartId;
            dbObj.TransactionDate = this.TransactionDate;
            dbObj.TransactionQuantity = this.TransactionQuantity;
            dbObj.UnitofMeasure = this.UnitofMeasure;
            dbObj.Cost = this.Cost;
            dbObj.Personnel = this.Personnel;
            dbObj.IssuedTo = this.IssuedTo;
            return dbObj;
        }

        public static List<PartHistory> UpdateFromDatabaseObjectList(List<b_PartHistory> dbObjs)
        {
            List<PartHistory> result = new List<PartHistory>();

            foreach (b_PartHistory dbObj in dbObjs)
            {
                PartHistory tmp = new PartHistory();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.PartClientLookupId = dbObj.PartClientLookupId;
                tmp.PartDescription = dbObj.PartDescription;
                tmp.TotalCost = Convert.ToDecimal(dbObj.TransactionQuantity * dbObj.Cost);
                tmp.PersonnelClientLookupId = dbObj.PersonnelClientLookupId;
                tmp.NameLast = dbObj.NameLast;
                tmp.NameFirst = dbObj.NameFirst;
                tmp.NameMiddle = dbObj.NameMiddle;
                tmp.PersonnelInitial = dbObj.PersonnelInitial;
                tmp.VMRSFailureCode = dbObj.VMRSFailureCode;
                //V2-624
                tmp.PartId = dbObj.PartId;
                //v2-624
                tmp.UPCCode = dbObj.UPCCode;
                //v2-610
                tmp.TotalCount = dbObj.TotalCount;
               
                result.Add(tmp);
            }
            return result;
        }
        public static List<PartHistory> UpdateFromDatabaseObjectListExtended(List<b_PartHistory> dbObjs)
        {
            List<PartHistory> result = new List<PartHistory>();

            foreach (b_PartHistory dbObj in dbObjs)
            {
                PartHistory tmp = new PartHistory();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.PartClientLookupId = dbObj.PartClientLookupId;
                tmp.PartDescription = dbObj.PartDescription;
                tmp.TotalCost = Convert.ToDecimal(dbObj.TransactionQuantity * dbObj.Cost);
                tmp.PersonnelClientLookupId = dbObj.PersonnelClientLookupId;
                tmp.NameLast = dbObj.NameLast;
                tmp.NameFirst = dbObj.NameFirst;
                tmp.NameMiddle = dbObj.NameMiddle;
                tmp.PersonnelInitial = dbObj.PersonnelInitial;
                tmp.VMRSFailureCode = dbObj.VMRSFailureCode;
                //V2-624
                tmp.PartId = dbObj.PartId;
                //v2-624
                tmp.UPCCode = dbObj.UPCCode;
                //v2-610
                tmp.PerformBy = dbObj.PerformBy;
                //v2-1034
                result.Add(tmp);
            }
            return result;
        }

        public static List<PartHistory> UpdateFromDatabaseObjectListPart(List<b_PartHistory> dbObjs)
        {
            List<PartHistory> result = new List<PartHistory>();

            foreach (b_PartHistory dbObj in dbObjs)
            {
                PartHistory tmp = new PartHistory();
                tmp.UpdateFromDatabaseObjectDB(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        private void UpdateFromDatabaseObjectDB(b_PartHistory dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.PartId = dbObj.PartId;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.Description = dbObj.Description;
            this.RequestorId = dbObj.RequestorId;
            this.TransactionDate = dbObj.TransactionDate;
            this.TransactionQuantity = dbObj.TransactionQuantity;
            this.UnitofMeasure = dbObj.UnitofMeasure;
            this.Cost = dbObj.Cost;
            this.Personnel = dbObj.Personnel;
            this.IssuedTo = dbObj.IssuedTo;
            this.ChargeToId_Primary = dbObj.ChargeToId_Primary;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            // Turn on auditing
            AuditEnabled = true;
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (m_validateProcess)
            {
                PartHistory_ValidateProcess trans = new PartHistory_ValidateProcess()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartHistory = this.ToExtendedDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();


                if (trans.StoredProcValidationErrorList != null)
                {
                    // If there is only one error and the error is stock-out, change the m_isStockOut to true
                    var spErrors = trans.StoredProcValidationErrorList;
                    if (spErrors.Count == 1 && spErrors.Exists(e => e.ErrorCode == OUT_STOCK_ERROR_CODE)) { IsStockOut = true; }

                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            else if (m_validateAdd)
            {
                PartHistory_ValidateAdd trans = new PartHistory_ValidateAdd()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartHistory = this.ToExtendedDatabaseObject();
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
            /**************************Added By Indusnet Technologeis*******************************/
            else if (m_validateInventoryReceiptAdd)
            {
                PartHistory_ValidateInventoryReceiptAdd trans = new PartHistory_ValidateInventoryReceiptAdd()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartHistory = this.ToExtendedDatabaseObject();
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
            else if (m_validateInventoryReceiptProcess)
            {
                PartHistory_ValidateInventoryReceiptConfirm trans = new PartHistory_ValidateInventoryReceiptConfirm()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartHistory = this.ToExtendedDatabaseObject();
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
            else if (m_validatePhysicalInventoryRecordCount)
            {
                PartHistory_ValidatePhysicalInventoryRecordCount trans = new PartHistory_ValidatePhysicalInventoryRecordCount()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartHistory = this.ToExtendedDatabaseObject();
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
            /**************************End**************************************************/
            else if (m_validateWOPartsAdd)
            {
                PartHistory_WOPartsAdd trans = new PartHistory_WOPartsAdd()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartHistory = this.ToExtendedDatabaseObject();
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
            if (m_validateWOPartsProcess)
            {
                PartHistory_WOPartsValidateProcess trans = new PartHistory_WOPartsValidateProcess()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartHistory = this.ToExtendedDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();


                if (trans.StoredProcValidationErrorList != null)
                {
                    // If there is only one error and the error is stock-out, change the m_isStockOut to true
                    var spErrors = trans.StoredProcValidationErrorList;
                    if (spErrors.Count == 1 && spErrors.Exists(e => e.ErrorCode == OUT_STOCK_ERROR_CODE)) { IsStockOut = true; }

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

        public decimal NetQty { get; set; }
        public DateTime DateRequired { get; set; }
        public string Part_ClientLookupID { get; set; }
        public string Personnel { get; set; }
        public string IssuedTo { get; set; }

        #endregion


        #region Properties
        public string IssueToClientLookupId { get; set; }
        public string Stroreroom { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string PartClientLookupId { get; set; }
        public List<PartHistory> PartHistoryList { get; set; }
        public List<string> ErrorsFromList { get; set; }
        public bool IsStockOut { get; set; }
        public bool IsPartIssue { get; set; }
        public long SiteId { get; set; }
        public int PartIssueId { get; set; }
        public string ErrorMessagerow { get; set; }
        public bool IsPerformAdjustment { get; set; }
        public int InventoryReceiptId { get; set; }
        public bool IsInventoryReceipt { get; set; }
        public string PartUPCCode { get; set; }
        public decimal PartAverageCost { get; set; }
        public int PhysicalInventoryId { get; set; }
        public bool IsPhysicalInventory { get; set; }
        public string PartDescription { get; set; }
        public decimal PartStoreroomQtyOnHand { get; set; }
        public decimal PartQtyCounted { get; set; }
        public string PartStorerommLocation1_1 { get; set; }
        public string PartStorerommLocation1_2 { get; set; }
        public string PartStorerommLocation1_3 { get; set; }
        public string PartStorerommLocation1_4 { get; set; }
        public int PartStoreroomUpdateIndex { get; set; }
        public string Flag { get; set; }
        //{ set; get; }
        //---------SOM-924-Api--------------------
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        //V2-610 part grid
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public int TotalCount { get; set; }

        public string UPCCode { get; set; }

        public string PerformBy { get; set; }//V2-1034
        

        //V2-751
        public System.Data.DataTable StoreroomTransferList { get; set; }
        public long StoreroomTransferId { get; set; }
        #endregion

        public void InventoryReceipt(DatabaseKey dbKey)
        {
            List<PartHistory> newPartHistoryList = new List<PartHistory>();

            foreach (PartHistory partHistory in PartHistoryList)
            {
                partHistory.ValidateInventoryReceiptConfirm(dbKey);
            }

            newPartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count == 0);

            if (newPartHistoryList.Count > 0)
            {
                PartHistory_InventotyReceipt trans = new PartHistory_InventotyReceipt();
                trans.PartHistory = this.ToDatabaseObject();
                trans.PartHistoryList = this.ToDatabaseObjectList(newPartHistoryList);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
            }

            this.PartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count > 0);

        }

        public List<PartHistory> CreateByForeignKeysnew(DatabaseKey dbKey)
        {
            ErrorsFromList = new List<string>();
            List<PartHistory> PartHistoryListTemp = new List<PartHistory>(); // 
            List<PartHistory> PartHistoryListMain = new List<PartHistory>();

            foreach (PartHistory partHistory in PartHistoryList)
            {
                if (transactionQtyValidation(Convert.ToString(partHistory.TransactionQuantity)))
                {
                    partHistory.ValidateWOCompletionPartsProcess(dbKey);
                    if (partHistory.ErrorMessages.Count == 0)
                    {
                        PartHistoryListMain.Add(partHistory);
                    }
                    else
                    {
                        partHistory.ErrorMessagerow = partHistory.ErrorMessages[0].ToString();
                        ErrorsFromList.AddRange(partHistory.ErrorMessages);
                        PartHistoryListTemp.Add(partHistory);
                    }
                }
                else
                {
                    partHistory.ErrorMessagerow = "Quantity should not exceed 8 digit of whole number Or Quantity should be non zero.";
                    ErrorsFromList.AddRange(partHistory.ErrorMessages);
                    PartHistoryListTemp.Add(partHistory);
                }
            }
            ErrorMessages = ErrorsFromList;
            if (PartHistoryListMain.Count > 0)
            {
                //MultiStoreroom V2-687
                if (PartHistoryListMain.Where(m => m.MultiStoreroom == true).ToList().Count > 0)
                {
                    PartHistory_CreateByPartDetailsV2ForMultiStoreroom trans = new PartHistory_CreateByPartDetailsV2ForMultiStoreroom();
                    trans.PartHistory = this.ToDatabaseObject();
                    trans.PartHistoryList = this.ToDatabaseObjectList(PartHistoryListMain);
                    trans.dbKey = dbKey.ToTransDbKey();
                    trans.Execute();

                }
                else
                {
                    PartHistory_CreateByPartDetailsV2624 trans = new PartHistory_CreateByPartDetailsV2624();
                    trans.PartHistory = this.ToDatabaseObject();
                    trans.PartHistoryList = this.ToDatabaseObjectList(PartHistoryListMain);
                    trans.dbKey = dbKey.ToTransDbKey();
                    trans.Execute();
                }
                
            }
            return PartHistoryListTemp;
        }
        public List<PartHistory> CreateReturnPartByForeignKeysnew(DatabaseKey dbKey)
        {
            ErrorsFromList = new List<string>();
            List<PartHistory> PartHistoryListTemp = new List<PartHistory>(); // 
            List<PartHistory> PartHistoryListMain = new List<PartHistory>();

            foreach (PartHistory partHistory in PartHistoryList)
            {
                if (transactionQtyValidation(Convert.ToString(partHistory.TransactionQuantity)))
                {
                    partHistory.ValidateWOCompletionPartsProcess(dbKey);
                    // RKL - Do not allow a return of a return
                    // Should add this to the Validation routine - may do later
                    if (partHistory.TransactionQuantity < 0)
                    {
                      string qtyErrMsg = "You cannot return a return or a negative quantity";
                      partHistory.ErrorMessagerow = qtyErrMsg;
                      partHistory.ErrorMessages.Add("You cannot return a return or a negative quantity");
                    }
                    if (partHistory.ErrorMessages.Count == 0)
                    {
                        PartHistoryListMain.Add(partHistory);
                    }
                    else
                    {
                        partHistory.ErrorMessagerow = partHistory.ErrorMessages[0].ToString();
                        ErrorsFromList.AddRange(partHistory.ErrorMessages);
                        PartHistoryListTemp.Add(partHistory);
                    }
                }
                else
                {
                    partHistory.ErrorMessagerow = "Quantity should not exceed 8 digit of whole number Or Quantity should be non zero.";
                    ErrorsFromList.AddRange(partHistory.ErrorMessages);
                    PartHistoryListTemp.Add(partHistory);
                }
            }
            ErrorMessages = ErrorsFromList;
            if (PartHistoryListMain.Count > 0)
            { 
                //MultiStoreroom V2-687
                if (PartHistoryListMain.Where(m => m.MultiStoreroom == true).ToList().Count > 0)
                {
                    PartReturn_CreateByPartDetailsForMultiStoreroom trans = new PartReturn_CreateByPartDetailsForMultiStoreroom();
                    trans.PartHistory = this.ToDatabaseObject();
                    trans.PartHistoryList = this.ToDatabaseObjectList(PartHistoryListMain);
                    trans.dbKey = dbKey.ToTransDbKey();
                    trans.Execute();
                }
                else
                {
                    PartReturn_CreateByPartDetails trans = new PartReturn_CreateByPartDetails();
                    trans.PartHistory = this.ToDatabaseObject();
                    trans.PartHistoryList = this.ToDatabaseObjectList(PartHistoryListMain);
                    trans.dbKey = dbKey.ToTransDbKey();
                    trans.Execute();
                }
                   
            }
            return PartHistoryListTemp;
        }
        public void CreateByForeignKeys_V2(DatabaseKey dbKey)
        {
            m_validateProcess = false;
            m_validateAdd = true;
            m_validateInventoryReceiptAdd = false;
            m_validateInventoryReceiptProcess = false;
            m_validatePhysicalInventoryRecordCount = false;
            Validate<PartHistory>(dbKey);
            if (IsValid)//Validation pending
            {
                PartHistory_CreateByPartDetails_V2 trans = new PartHistory_CreateByPartDetails_V2();
                trans.PartHistory = this.ToExtendedDatabaseObject();
                //trans.PartHistoryList = this.ToDatabaseObjectList(PartHistoryListMain);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                UpdateFromDatabaseObjectDB(trans.PartHistory);
            }
        }

        public static bool transactionQtyValidation(string tranValue)
        {
            bool success = true;
            try
            {
                decimal transactionQty = Convert.ToDecimal(string.Format("{0:F6}", Convert.ToDecimal(tranValue)));

                string[] parts = transactionQty.ToString().Split('.');
                Int64 bfPoint = Int64.Parse(parts[0]);
                Int64 afPoint = Int64.Parse(parts[1]);
                if (transactionQty == 0)
                {
                    success = false;
                }
                else if (bfPoint.ToString().Length >= 9)
                {
                    success = false;
                }
                else if (bfPoint == 0)
                {
                    if (transactionQty.ToString().Length >= 9)
                    {
                        success = false;
                    }

                }

            }
            catch { success = false; }

            return success;
        }

        public void ValidateWOCompletionPartsProcess(DatabaseKey dbKey)
        {
            // RKL - 2014-Jul-09 - Change to use the validate process function
            // Need to discuss with Dipak
            // Changed to m_validateprocess = true 
            //            m_validateWOPartsProcess = false
            m_validateProcess = true;
            m_validateAdd = false;
            m_validateInventoryReceiptAdd = false;
            m_validateInventoryReceiptProcess = false;
            m_validatePhysicalInventoryRecordCount = false;
            m_validateWOPartsAdd = false;
            m_validateWOPartsProcess = false;

            Validate<PartHistory>(dbKey);
            if (this.TransactionDate.HasValue && this.TransactionDate.Value.Date.CompareTo(DateTime.Now.AddDays(1).Date) > 0)
            {
                //Business.Localization.ValidationError locValidationError =
                //    loc.StoredProcValidation.ValidationError.Find(v => v.Code == ISSUE_DATE_ERROR_CODE);
                //if (locValidationError != null) { ErrorMessages.Add(locValidationError.Message); }
            }
        }

        public List<b_PartHistory> ToDatabaseObjectList(List<PartHistory> partHistoryList)
        {
            List<b_PartHistory> dbObjs = new List<b_PartHistory>();

            foreach (PartHistory partHistory in partHistoryList)
            {
                dbObjs.Add(partHistory.ToExtendedDatabaseObject());
            }

            return dbObjs;
        }

        public void ValidateInventoryReceiptConfirm(DatabaseKey dbKey)
        {
            m_validateProcess = false;
            m_validateAdd = false;
            m_validateInventoryReceiptAdd = false;
            m_validateInventoryReceiptProcess = true;
            m_validatePhysicalInventoryRecordCount = false;

            Validate<PartHistory>(dbKey);
            //if (this.TransactionDate.HasValue && this.TransactionDate.Value.Date.CompareTo(DateTime.Now.AddDays(1).Date) > 0)
            //{
            //    Business.Localization.ValidationError locValidationError =
            //        loc.StoredProcValidation.ValidationError.Find(v => v.Code == ISSUE_DATE_ERROR_CODE);
            //    if (locValidationError != null) { ErrorMessages.Add(locValidationError.Message); }
            //}
        }

        public void ValidatePhysicalInventoryRecordCount(DatabaseKey dbKey)
        {
            m_validateProcess = false;
            m_validateAdd = false;
            m_validateInventoryReceiptAdd = false;
            m_validateInventoryReceiptProcess = false;
            m_validatePhysicalInventoryRecordCount = true;

            Validate<PartHistory>(dbKey);
        }
        public void PhysicalInventory(DatabaseKey dbKey)
        {
            // Since we performed the same validation when we added the item to the list
            // we do NOT need to perform that validation again
            PartHistory_PhysicalInventory trans = new PartHistory_PhysicalInventory();
            trans.PartHistory = this.ToDatabaseObject();
            trans.PartHistoryList = this.ToDatabaseObjectList(PartHistoryList);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            //this.PartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count > 0);

            /*
            List<PartHistory> newPartHistoryList = new List<PartHistory>();

              foreach (PartHistory partHistory in PartHistoryList)
              {
                partHistory.ValidatePhysicalInventoryRecordCount(dbKey);
              }

              newPartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count == 0);

              if (newPartHistoryList.Count > 0)
              {
                  PartHistory_PhysicalInventory trans = new PartHistory_PhysicalInventory();
                  trans.PartHistory = this.ToDatabaseObject();
                  trans.PartHistoryList = this.ToDatabaseObjectList(newPartHistoryList);
                  trans.dbKey = dbKey.ToTransDbKey();
                  trans.Execute();
              }

              this.PartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count > 0);
              */
        }

        public void UpdatepartCount(DatabaseKey dbKey)
        {
            // Since we performed the same validation when we added the item to the list
            // we do NOT need to perform that validation again
            PartHistory_UpdatePartCount trans = new PartHistory_UpdatePartCount();
            trans.PartHistory = this.ToDatabaseObject();
            trans.PartHistoryList = this.ToDatabaseObjectList(PartHistoryList);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            //this.PartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count > 0);

            /*
            List<PartHistory> newPartHistoryList = new List<PartHistory>();

              foreach (PartHistory partHistory in PartHistoryList)
              {
                partHistory.ValidatePhysicalInventoryRecordCount(dbKey);
              }

              newPartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count == 0);

              if (newPartHistoryList.Count > 0)
              {
                  PartHistory_PhysicalInventory trans = new PartHistory_PhysicalInventory();
                  trans.PartHistory = this.ToDatabaseObject();
                  trans.PartHistoryList = this.ToDatabaseObjectList(newPartHistoryList);
                  trans.dbKey = dbKey.ToTransDbKey();
                  trans.Execute();
              }

              this.PartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count > 0);
              */
        }
        public List<PartHistory> RetriveByPartId(DatabaseKey dbKey)
        {
            PartHistory_RetrieveByPartId trans = new PartHistory_RetrieveByPartId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartHistory = this.ToDatabaseObject();
            trans.PartHistory.SiteId = this.SiteId;
            trans.PartHistory = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PartHistory.UpdateFromDatabaseObjectList(trans.PartHistoryList);
        }

        #region V2-610 part grid
        public List<PartHistory> RetriveForMaintenanceTechnician_V2(DatabaseKey dbKey)
        {
            PartHistory_RetrieveForMaintenanceTechinician_V2 trans = new PartHistory_RetrieveForMaintenanceTechinician_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PartHistory = this.ToDateBaseObjectForRetrieveChunkSearch();
            //trans.PartHistory = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PartHistory.UpdateFromDatabaseObjectListExtended(trans.PartHistoryList);
        }
        public b_PartHistory ToDateBaseObjectForRetrieveChunkSearch()
        {
            b_PartHistory dbObj = this.ToDatabaseObject();            
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.Offset = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            return dbObj;
        }
        #endregion

        public void UpdateFromDatabaseObjectPartHistoryPrintExtended(b_PartHistory dbObj)
        {
            // this.UpdateFromDatabaseObject(dbObj);
            this.ChargeToId_Primary = dbObj.ChargeToId_Primary;
            this.Cost = dbObj.Cost;
            this.TransactionQuantity = dbObj.TransactionQuantity;
            this.UnitofMeasure = dbObj.UnitofMeasure;
            this.Description = dbObj.Description;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.TotalCost = Convert.ToDecimal(dbObj.TransactionQuantity * dbObj.Cost);
        }

        #region V2-687 Inventory Receipt
        public void InventoryReceipt_V2(DatabaseKey dbKey)
        {
            List<PartHistory> newPartHistoryList = new List<PartHistory>();

            foreach (PartHistory partHistory in PartHistoryList)
            {
                partHistory.ValidateInventoryReceiptConfirm(dbKey);
            }

            newPartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count == 0);

            if (newPartHistoryList.Count > 0)
            {
                PartHistory_InventotyReceipt_V2 trans = new PartHistory_InventotyReceipt_V2();
                trans.PartHistory = this.ToDatabaseObject();
                trans.PartHistoryList = this.ToDatabaseObjectList(newPartHistoryList);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
            }

            this.PartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count > 0);

        }
        #endregion

        #region 687 Physical Inventory add
        public void PhysicalInventory_V2(DatabaseKey dbKey)
        {
            // Since we performed the same validation when we added the item to the list
            // we do NOT need to perform that validation again
            PartHistory_PhysicalInventory_V2 trans = new PartHistory_PhysicalInventory_V2();
            trans.PartHistory = this.ToDatabaseObject();
            trans.PartHistoryList = this.ToDatabaseObjectList(PartHistoryList);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            //this.PartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count > 0);

            /*
            List<PartHistory> newPartHistoryList = new List<PartHistory>();

              foreach (PartHistory partHistory in PartHistoryList)
              {
                partHistory.ValidatePhysicalInventoryRecordCount(dbKey);
              }

              newPartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count == 0);

              if (newPartHistoryList.Count > 0)
              {
                  PartHistory_PhysicalInventory trans = new PartHistory_PhysicalInventory();
                  trans.PartHistory = this.ToDatabaseObject();
                  trans.PartHistoryList = this.ToDatabaseObjectList(newPartHistoryList);
                  trans.dbKey = dbKey.ToTransDbKey();
                  trans.Execute();
              }

              this.PartHistoryList = PartHistoryList.FindAll(P => P.ErrorMessages.Count > 0);
              */
        }

        #endregion
        #region Update part Count For MultiStoreroom V2-687
        public void UpdatepartCountForMultiStoreroom(DatabaseKey dbKey)
        {
            PartHistory_UpdatePartCountForMultiStoreroom trans = new PartHistory_UpdatePartCountForMultiStoreroom();
            trans.PartHistory = this.ToDatabaseObject();
            trans.PartHistoryList = this.ToDatabaseObjectList(PartHistoryList);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
        #endregion

        #region 751
        #region Outgoing Transfer
        public void ProcessIssues(DatabaseKey dbKey)
        {
            PartHistory_ProcessIssues trans = new PartHistory_ProcessIssues()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartHistory = this.ToExtendedDatabaseObjectForOutgoingTransferProcess();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
        public b_PartHistory ToExtendedDatabaseObjectForOutgoingTransferProcess()
        {
            b_PartHistory dbObj = new b_PartHistory();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PerformedById = this.PerformedById;
            dbObj.StoreroomTransferList = this.StoreroomTransferList;
            return dbObj;
        }
        #endregion

        #region Incoming Transfer
        public void ProcessReceipt(DatabaseKey dbKey)
        {
            PartHistory_ProcessReceipt trans = new PartHistory_ProcessReceipt()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartHistory = this.ToExtendedDatabaseObjectForIncomingTransferProcess();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
        public b_PartHistory ToExtendedDatabaseObjectForIncomingTransferProcess()
        {
            b_PartHistory dbObj = new b_PartHistory();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PerformedById = this.PerformedById;
            dbObj.StoreroomTransferList = this.StoreroomTransferList;
            return dbObj;
        }
        #endregion

        #region Force Complete
        public void StoreroomTransferForceComplete(DatabaseKey dbKey)
        {
            PartHistory_StoreroomTransferForceComplete trans = new PartHistory_StoreroomTransferForceComplete();
            trans.PartHistory = this.ToExtendedDatabaseObjectForForceComplete();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
        public b_PartHistory ToExtendedDatabaseObjectForForceComplete()
        {
            b_PartHistory dbObj = new b_PartHistory();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PerformedById = this.PerformedById;
            dbObj.StoreroomTransferId = this.StoreroomTransferId;
            return dbObj;
        }
        #endregion
        #endregion
       
        #region V2-1031
        public PartHistory CreateByForeignKeysForIssuePartStockOut(DatabaseKey dbKey)
        {
            ErrorsFromList = new List<string>();
            PartHistory PartHistoryTemp = new PartHistory();
            PartHistory PartHistoryMain = new PartHistory();

                if (transactionQtyValidation(Convert.ToString(this.TransactionQuantity)))
                {
                    this.ValidateWOCompletionPartsProcess(dbKey);
                    if (this.ErrorMessages.Count == 0)
                    {
                        PartHistoryMain=this;
                    }
                    else
                    {
                        this.ErrorMessagerow =   this.ErrorMessages[0].ToString();
                        ErrorsFromList.AddRange(this.ErrorMessages);
                    PartHistoryMain = this;
                    }
                }
                else
                {
                    this.ErrorMessagerow = "Quantity should not exceed 8 digit of whole number Or Quantity should be non zero.";
                    ErrorsFromList.AddRange(this.ErrorMessages);
                    PartHistoryMain = this;
                }
            
            ErrorMessages = ErrorsFromList;
            bool ispartissueStockOut=false;
            if (PartHistoryMain != null && PartHistoryMain.ErrorMessages != null)
            {
                
                if (this.MultiStoreroom == true)
                {
                    PartHistory_CreateByPartDetailsStockOutForMultiStoreroom trans = new PartHistory_CreateByPartDetailsStockOutForMultiStoreroom();
                    trans.PartHistory = ToDatabaseObjectForStockOut(PartHistoryMain);
                    trans.dbKey = dbKey.ToTransDbKey();
                    trans.Execute();
                    ispartissueStockOut = trans.PartHistory.IspartIssueStockOut;
                }
                else
                {
                    PartHistory_CreateByPartDetailsStockOut trans = new PartHistory_CreateByPartDetailsStockOut();
                    trans.PartHistory = ToDatabaseObjectForStockOut(PartHistoryMain);
                    trans.dbKey = dbKey.ToTransDbKey();
                    trans.Execute();
                    ispartissueStockOut = trans.PartHistory.IspartIssueStockOut;
                }
                if(ispartissueStockOut)
                {
                    this.ErrorMessagerow = "There is not enough on hand quantity to issue the part";
                    ErrorsFromList.AddRange(this.ErrorMessages);
                    PartHistoryTemp = this;
                }

            }
            else
            {
                PartHistoryTemp = PartHistoryMain;
            }
            return PartHistoryTemp;
        }
        public b_PartHistory ToDatabaseObjectForStockOut(PartHistory partHistory)
        {
            return partHistory.ToExtendedDaToExtendedDatabaseObjectStockOuttabaseObject(); ;
        }
        public b_PartHistory ToExtendedDaToExtendedDatabaseObjectStockOuttabaseObject()
        {
            b_PartHistory dbObj = this.ToDatabaseObject();

            dbObj.IssueToClientLookupId = this.IssueToClientLookupId;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.PartClientLookupId = this.PartClientLookupId;
            dbObj.SiteId = this.SiteId;
            dbObj.IsPartIssue = this.IsPartIssue;
            dbObj.IsPerformAdjustment = this.IsPerformAdjustment;
            dbObj.InventoryReceiptId = this.InventoryReceiptId;
            dbObj.IsInventoryReceipt = this.IsInventoryReceipt;
            dbObj.Cost = this.Cost;
            dbObj.PartUPCCode = this.PartUPCCode;
            dbObj.PartAverageCost = this.PartAverageCost;
            dbObj.PhysicalInventoryId = this.PhysicalInventoryId;
            dbObj.IsPhysicalInventory = this.IsPhysicalInventory;
            dbObj.PartDescription = this.PartDescription;
            dbObj.PartStoreroomQtyOnHand = this.PartStoreroomQtyOnHand;
            dbObj.PartQtyCounted = this.PartQtyCounted;
            dbObj.PartStorerommLocation1_1 = this.PartStorerommLocation1_1;
            dbObj.PartStorerommLocation1_2 = this.PartStorerommLocation1_2;
            dbObj.PartStorerommLocation1_3 = this.PartStorerommLocation1_3;
            dbObj.PartStorerommLocation1_4 = this.PartStorerommLocation1_4;
            dbObj.PartStoreroomUpdateIndex = this.PartStoreroomUpdateIndex;
            dbObj.Personnel = this.Personnel;
            dbObj.IssuedTo = this.IssuedTo;
            dbObj.Flag = this.Flag;
            dbObj.TotalCount = this.TotalCount;
            dbObj.IspartIssueStockOut = this.IspartIssueStockOut;
            dbObj.StoreroomId = this.StoreroomId;
            return dbObj;
        }
        #endregion
    }
}
