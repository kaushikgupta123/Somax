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
//using  Common.Interfaces;
//using  Business.Localization;

//using DataContracts;

namespace DataContracts
{
    public partial class InvoiceMatchHeader : DataContractBase, IStoredProcedureValidation
    {
        #region Property
        public string DateRange { get; set; }
        public string DateColumn { get; set; }
        public string Assigned { get; set; }
        public string VendorName { get; set; }
        public string Status_Display { get; set; }
        public int PersonnelId { get; set; }
        public decimal ItemTotal { get; set; }
        public decimal Total { get; set; }
        public decimal Variance { get; set; }
        public int CustomQueryDisplayId { get; set; }

        string ValidateFor = string.Empty;
        public string VendorClientLookupId { get; set; }
        public string POClientLookupId { get; set; }
        public int NumberOfLineItems { get; set; }
        public string Flag { get; set; }
        private bool m_validateClientLookupId;
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public Int32 ChildCount { get; set; }
        public string CompleteATPStartDateVw { get; set; }//V2-373
        public string CompleteATPEndDateVw { get; set; }//V2-373
        public string CompletePStartDateVw { get; set; }//V2-373
        public string CompletePEndDateVw { get; set; }//V2-373
        public string CreateStartDateVw { get; set; }//V2-1061
        public string CreateEndDateVw { get; set; }//V2-1061
        public int InvoiceVariance { get; set; }//V2-1061
        public DateTime? CreateDate { get; set; }//V2-981
        public string CreateBy { get; set; }//V2-981
        public DateTime? ModifyDate { get; set; }//V2-981
        public string ModifyBy { get; set; }//V2-981
        public string Responsible { get; set; }//V2-981
        public string ResponsibleWithClientLookupId { get; set; }//V2-981
        
        public string AuthorizedToPayBy { get; set; }//V2-981
        public string PaidBy { get; set; }//V2-981


        #endregion


        #region Transactions



        public List<InvoiceMatchHeader> RetrieveForSearch(DatabaseKey dbKey)
        {
            InvoiceMatchHeader_RetrieveAllForSearch trans = new InvoiceMatchHeader_RetrieveAllForSearch();
            List<InvoiceMatchHeader> invoiceMatchHeaderList = new List<InvoiceMatchHeader>();
            trans.InvoiceMatchHeader = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            foreach (b_InvoiceMatchHeader invoiceMatchHeader in trans.InvoiceMatchHeaderList)
            {
                InvoiceMatchHeader tmpinvoiceMatchHeader = new InvoiceMatchHeader();
                UpdateFromDatabaseObject(trans.InvoiceMatchHeader);
                tmpinvoiceMatchHeader.UpdateFromDatabaseObjectExtended(invoiceMatchHeader);

                invoiceMatchHeaderList.Add(tmpinvoiceMatchHeader);
            }
            return invoiceMatchHeaderList;
        }

        public List<InvoiceMatchHeader> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            InvoiceMatchHeader_RetrieveChunkSearch trans = new InvoiceMatchHeader_RetrieveChunkSearch();
            List<InvoiceMatchHeader> invoiceMatchHeaderList = new List<InvoiceMatchHeader>();
            trans.InvoiceMatchHeader = this.ToDatabaseObject();
            trans.InvoiceMatchHeader.orderbyColumn = this.orderbyColumn;
            trans.InvoiceMatchHeader.orderBy = this.orderBy;
            trans.InvoiceMatchHeader.CustomQueryDisplayId = this.CustomQueryDisplayId;
            trans.InvoiceMatchHeader.offset1 = this.offset1;
            trans.InvoiceMatchHeader.nextrow = this.nextrow;
            trans.InvoiceMatchHeader.VendorClientLookupId = this.VendorClientLookupId;
            trans.InvoiceMatchHeader.VendorName = this.VendorName;
            trans.InvoiceMatchHeader.POClientLookUpId = this.POClientLookupId;
            trans.InvoiceMatchHeader.SearchText = this.SearchText;
            trans.InvoiceMatchHeader.CompleteATPStartDateVw = this.CompleteATPStartDateVw;//V2-373
            trans.InvoiceMatchHeader.CompleteATPEndDateVw = this.CompleteATPEndDateVw;//V2-373
            trans.InvoiceMatchHeader.CompletePStartDateVw = this.CompletePStartDateVw;//V2-373
            trans.InvoiceMatchHeader.CompletePEndDateVw = this.CompletePEndDateVw;//V2-373
            trans.InvoiceMatchHeader.CreateStartDateVw = this.CreateStartDateVw;//V2-1061
            trans.InvoiceMatchHeader.CreateEndDateVw = this.CreateEndDateVw;//V2-1061
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            foreach (b_InvoiceMatchHeader invoiceMatchHeader in trans.InvoiceMatchHeaderList)
            {
                InvoiceMatchHeader tmpinvoiceMatchHeader = new InvoiceMatchHeader();
                tmpinvoiceMatchHeader.UpdateFromDatabaseObjectExtended(invoiceMatchHeader);
                invoiceMatchHeaderList.Add(tmpinvoiceMatchHeader);
            }
            return invoiceMatchHeaderList;
        }

        public void UpdateFromDatabaseObjectExtended(b_InvoiceMatchHeader dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.VendorClientLookupId = dbObj.VendorClientLookupId;
            this.VendorName = dbObj.VendorName;
            this.POClientLookupId = dbObj.POClientLookUpId;
            this.ChildCount = dbObj.ChildCount;
            this.TotalCount = dbObj.TotalCount;
        }

        public List<b_StoredProcValidationError> ToDatabaseObjectList()
        {
            List<b_StoredProcValidationError> dbObj = new List<b_StoredProcValidationError>();
            //dbObj = this.LocationList;
            return dbObj;
        }



        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (m_validateClientLookupId)
            {
                ProcedureInvoiceMatchCreateValidationTransaction trans = new ProcedureInvoiceMatchCreateValidationTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.InvoiceMatchHeader = this.ToDatabaseObject();
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
            else if (ValidateFor == "ValidateByClientlookupId")
            {
                InvoiceMatchHeaderValidationByClientLookUpId trans = new InvoiceMatchHeaderValidationByClientLookUpId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.InvoiceMatchHeader = this.ToDatabaseObject();
                trans.InvoiceMatchHeader.ClientLookupId = this.ClientLookupId;
                trans.InvoiceMatchHeader.Flag = this.Flag;
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
            else if (ValidateFor == "ValidateVarianceCheck")
            {
                InvoiceMatchHeader_ValidateVarianceCheckTransaction trans = new InvoiceMatchHeader_ValidateVarianceCheckTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.InvoiceMatchHeader = this.ToDatabaseObject();
                trans.InvoiceMatchHeader.InvoiceVariance = this.InvoiceVariance;              
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

        //public void CreateTo(DatabaseKey dbKey, string TimeZone)
        //{
        //    m_validateClientLookupId = true;
        //    Validate<InvoiceMatchHeader>(dbKey);
        //    if (IsValid)
        //    {
        //        InvoiceMatchHeader_Create trans = new InvoiceMatchHeader_Create();
        //        trans.InvoiceMatchHeader = this.ToDatabaseObject();
        //        trans.dbKey = dbKey.ToTransDbKey();
        //        trans.Execute();

        //        // The create procedure may have populated an auto-incremented key. 
        //        UpdateFromDatabaseObject(trans.InvoiceMatchHeader);
        //    }
        //}

        public void CreateTo(DatabaseKey dbKey)
        {
            m_validateClientLookupId = true;
            Validate<InvoiceMatchHeader>(dbKey);
            if (IsValid)
            {
                InvoiceMatchHeader_Create trans = new InvoiceMatchHeader_Create();
                trans.InvoiceMatchHeader = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.InvoiceMatchHeader);
            }
        }
        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_InvoiceMatchHeader dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            // Moved from RetrieveAllForSearchNew - Begin
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.InvoiceMatchHeaderId = dbObj.InvoiceMatchHeaderId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Status = dbObj.Status;
            this.VendorClientLookupId = dbObj.VendorClientLookupId;
            this.VendorName = dbObj.VendorName;
            this.ReceiptDate = dbObj.ReceiptDate;
            this.POClientLookupId = dbObj.POClientLookUpId;
            // SOM-279 Localization of Status  
            // SOM-1164 - Changed to use invoicematch instead of work order status values
            switch (this.Status)
            {
                case Common.Constants.InvoiceMatchStatus.Open:
                    this.Status_Display = "Open";
                    break;
                case Common.Constants.InvoiceMatchStatus.Paid:
                    this.Status_Display = "Paid";
                    break;
                case Common.Constants.InvoiceMatchStatus.AuthorizedToPay:
                    this.Status_Display = "AuthorizedToPay";
                    break;
            }
        }

        public List<InvoiceMatchHeader> RetrieveAllForSearchNew(DatabaseKey dbKey)
        {
            InvoiceMatchHeader_RetrieveAllForSearch trans = new InvoiceMatchHeader_RetrieveAllForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.InvoiceMatchHeader = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<InvoiceMatchHeader> invoiceMatchHeaderlist = new List<InvoiceMatchHeader>();
            // RKL - 2014-Aug-08
            // Moved this to UpdateFromDatabaseObjectForRetriveAllForSearch
            foreach (b_InvoiceMatchHeader invoiceMatchHeader in trans.InvoiceMatchHeaderList)
            {
                InvoiceMatchHeader tmpinvoiceMatchHeader = new InvoiceMatchHeader();
                /*
                  tmpworkorder.DepartmentName = workorder.DepartmentName;
                  tmpworkorder.Creator = workorder.Creator;
                  tmpworkorder.Assigned = workorder.Assigned;
                */
                tmpinvoiceMatchHeader.UpdateFromDatabaseObjectForRetriveAllForSearch(invoiceMatchHeader);
                invoiceMatchHeaderlist.Add(tmpinvoiceMatchHeader);
            }
            return invoiceMatchHeaderlist;
        }

        public b_InvoiceMatchHeader ToDateBaseObjectForRetriveAllForSearch()
        {
            b_InvoiceMatchHeader dbObj = this.ToDatabaseObject();

            dbObj.DateRange = this.DateRange;
            dbObj.DateColumn = this.DateColumn;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            return dbObj;
        }
        public void ChangeClientLookupId(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateByClientlookupId";
            Validate<InvoiceMatchHeader>(dbKey);
            if (IsValid)
            {

                InvoiceMatchHeader_ChangeClientLookupId trans = new InvoiceMatchHeader_ChangeClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.InvoiceMatchHeader = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                // The create procedure changed the Update Index.
                UpdateFromDatabaseObjectExtended(trans.InvoiceMatchHeader);
            }
        }
        public void ValidateVarianceCheck(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateVarianceCheck";
            Validate<InvoiceMatchHeader>(dbKey);

        }
        public void RetrieveByPKForeignKeys(DatabaseKey dbKey)
        {
            InvoiceMatchHeader_RetrieveByPKForeignKeys trans = new InvoiceMatchHeader_RetrieveByPKForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.InvoiceMatchHeader = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.InvoiceMatchHeader);
            this.VendorClientLookupId = trans.InvoiceMatchHeader.VendorClientLookupId;
            this.VendorName = trans.InvoiceMatchHeader.VendorName;
            this.Total = trans.InvoiceMatchHeader.Total;
            this.ItemTotal = trans.InvoiceMatchHeader.ItemTotal;
            this.Variance = trans.InvoiceMatchHeader.Variance;
            this.POClientLookupId = trans.InvoiceMatchHeader.POClientLookUpId;
            this.NumberOfLineItems = trans.InvoiceMatchHeader.NumberOfLineItems;
            this.CreateDate = trans.InvoiceMatchHeader.CreateDate;
            this.CreateBy = trans.InvoiceMatchHeader.CreateBy;
            this.ModifyDate = trans.InvoiceMatchHeader.ModifyDate;
            this.ModifyBy = trans.InvoiceMatchHeader.ModifyBy;
            this.Responsible = trans.InvoiceMatchHeader.Responsible;
            this.ResponsibleWithClientLookupId = trans.InvoiceMatchHeader.ResponsibleWithClientLookupId;
            this.AuthorizedToPayBy = trans.InvoiceMatchHeader.AuthorizedToPayBy;
            this.PaidBy = trans.InvoiceMatchHeader.PaidBy;
        }

        public void UpdateByPKForeignKeys(DatabaseKey dbKey)
        {
            Validate<InvoiceMatchHeader>(dbKey);
            if (IsValid)
            {

                InvoiceMatchHeader_UpdateByForeignKeys trans = new InvoiceMatchHeader_UpdateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.InvoiceMatchHeader = this.ToDatabaseObject();

                trans.InvoiceMatchHeader.POClientLookUpId = this.POClientLookupId;
                trans.InvoiceMatchHeader.VendorClientLookupId = this.VendorClientLookupId;

                //trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.InvoiceMatchHeader);
            }
        }

        public void DeleteInvoiceMatchHeaderAndInvoiceMatchItemsId(DatabaseKey dbKey)
        {
            InvoiceMatchHeader_DeleteInvoiceMatchHeaderAndInvoiceMatchItemsId trans = new InvoiceMatchHeader_DeleteInvoiceMatchHeaderAndInvoiceMatchItemsId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.InvoiceMatchHeader = new b_InvoiceMatchHeader();
            trans.InvoiceMatchHeader = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
        #endregion
    }
}
