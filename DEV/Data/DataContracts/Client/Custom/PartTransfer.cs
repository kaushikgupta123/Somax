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
* 
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
    [JsonObject]
    public partial class PartTransfer : DataContractBase, IStoredProcedureValidation
    {
        #region Properties

        public Int64 SiteId { get; set; }
        public bool IsAdmin { get; set; }
        public Int32 CaseNo { get; set; }
        public Int32 CustomQueryDisplayId { get; set; }
        public string Status_Display { get; set; }
        public string Description { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public DateTime CreateDate { get; set; }
        public string RequestPart_ClientLookupId { get; set; }
        public string RequestSite_Name { get; set; }
        public string RequestPart_Description { get; set; }
        public string CreateBy_PersonnelName { get; set; }
        public string IssueSite_Name { get; set; }
        public string IssuePart_ClientLookupId { get; set; }
        public decimal IssuePart_QtyOnHand { get; set; }
        public string IssuePart_Description { get; set; }
        public string LastEventBy_PersonnelName { get; set; }
        public decimal QuantityOutstanding { get; set; }
        public decimal QuantityInTransit { get; set; }
        public decimal QtyOnHand { get; set; }
        public string ShipperName { get; set; }
        public string IssueSite_Address1 { get; set; }
        public string IssueSite_Address2 { get; set; }
        public string IssueSite_CityStateZip { get; set; }
        public DateTime TransactionDate { get; set; }
        public string RequestSite_Address1 { get; set; }
        public string RequestSite_Address2 { get; set; }
        public string RequestSite_CityStateZip { get; set; }
        public string RequesterName { get; set; }
        public decimal AvgCostAfter { get; set; }
        public decimal CostAfter { get; set; }
        public decimal RequestQuantity { get; set; }
        public Int64 LoggedUser_PersonnelId { get; set; }
        public string TransactionType { get; set; }
        public string Comments { get; set; }
        public string EventCode { get; set; }
        public bool ConfirmForceComplete { get; set; }
        public string CancelOrDenyState { get; set; }
        public string CancelDenyReason { get; set; }
        public Int64 PartTransferEventLogId { get; set; }
        public Decimal TxnQuantity { get; set; }    // Transaction Quantity (Received or Issued)
        public string FullName
        {
            get
            {
                string name = NameFirst.Trim() + " " + NameLast.Trim();
                return (string.Compare(",", name.Trim()) == 0) ? "" : name;  // Ensure "," won't be returned if _NameLast, _NameFirst, and _NameMiddle are empty
            }
        }

        #endregion
        public List<PartTransfer> RetrieveAllForSearchNew(DatabaseKey dbKey, string TimeZone)
        {
            PartTransfer_RetrieveAllForSearch trans = new PartTransfer_RetrieveAllForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartTransfer = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PartTransfer> ptlist = new List<PartTransfer>();
           
            foreach (b_PartTransfer parttransfer in trans.PartTransferList)
            {
                PartTransfer tmpparttransfer= new PartTransfer();
                tmpparttransfer.UpdateFromDatabaseObjectExtended(parttransfer,  TimeZone);
                ptlist.Add(tmpparttransfer);
            }
            return ptlist;
        }              
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
           

            return errors;
        }
        public void RetrieveByPKForeignKeys(DatabaseKey dbKey, string Timezone)
        {
            PartTransfer_RetrieveByPKForeignKey trans = new PartTransfer_RetrieveByPKForeignKey()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartTransfer = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtended(trans.PartTransfer, Timezone);
           
        }
        public void PartTransferIssue(DatabaseKey dbKey,  string Timezone)
        {
            //Validate<PartTransfer>(dbKey);

            //if (IsValid)
            //{
                PartTransfer_UpdateIssue trans = new PartTransfer_UpdateIssue()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.PartTransfer = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObjectExtended(trans.PartTransfer, Timezone);
            //}
        }
        public void PartTransferReceive(DatabaseKey dbKey,  string Timezone)
        {
            //Validate<PartTransfer>(dbKey);

            //if (IsValid)
            //{
                PartTransfer_UpdateReceive trans = new PartTransfer_UpdateReceive()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.PartTransfer = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObjectExtended(trans.PartTransfer, Timezone);
            //}
        }
        public void PartTransferSend(DatabaseKey dbKey,  string Timezone)
        {
            //Validate<PartTransfer>(dbKey);

            //if (IsValid)
            //{
                PartTransfer_UpdateSend trans = new PartTransfer_UpdateSend()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.PartTransfer = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObjectExtended(trans.PartTransfer, Timezone);
            //}
        }
        public void PartTransferCancelDeny(DatabaseKey dbKey,  string Timezone)
        {
            //Validate<PartTransfer>(dbKey);

            //if (IsValid)
            //{
                PartTransfer_UpdateCancelDeny trans = new PartTransfer_UpdateCancelDeny()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.PartTransfer = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObjectExtended(trans.PartTransfer, Timezone);
            //}
        }
        public void PartTransferForceComplete(DatabaseKey dbKey,  string Timezone)
        {
            //Validate<PartTransfer>(dbKey);

            //if (IsValid)
            //{
                PartTransfer_UpdateForceComplete trans = new PartTransfer_UpdateForceComplete()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.PartTransfer = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObjectExtended(trans.PartTransfer,  Timezone);
            //}
        }
        public void PartTransferShipment(DatabaseKey dbKey,  string Timezone)
        {
            PartTransfer_Shipment trans = new PartTransfer_Shipment()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartTransfer = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObjectShipment(trans.PartTransfer, Timezone);
        }

        public b_PartTransfer ToDatabaseObjectExtended()
        {
            b_PartTransfer dbObj = this.ToDatabaseObject();
            dbObj.SiteId = this.SiteId;                   // RKL - Not exactly sure what this is for
            dbObj.AvgCostAfter = this.AvgCostAfter;
            dbObj.CostAfter = this.CostAfter;
            dbObj.TransactionType = this.TransactionType;
            dbObj.Comments = this.Comments;
            dbObj.EventCode = this.EventCode;
            dbObj.CancelOrDenyState = this.CancelOrDenyState;
            // RKL - 2023-Jan-27 Added these 
            dbObj.LoggedUser_PersonnelId = this.LoggedUser_PersonnelId;
            dbObj.TxnQuantity = this.TxnQuantity;
            // RKL - 2023-Jan-27 - These items below are all handled by the b_PartTransfer.ToDatabaseObject method 
            /*
            dbObj.ClientId = this.ClientId;
            dbObj.PartTransferId = this.PartTransferId;
            dbObj.RequestSiteId = this.RequestSiteId;
            dbObj.RequestPartId = this.RequestPartId;
            dbObj.IssueSiteId = this.IssueSiteId;
            dbObj.IssuePartId = this.IssuePartId;
            dbObj.Creator_PersonnelId = this.Creator_PersonnelId;
            dbObj.Quantity = this.Quantity;
            dbObj.Reason = this.Reason;
            dbObj.RequiredDate = this.RequiredDate;
            dbObj.ShippingAccountId = this.ShippingAccountId;
            dbObj.Status = this.Status;
            dbObj.QuantityIssued = this.QuantityIssued;
            dbObj.QuantityReceived = this.QuantityReceived;
            dbObj.LastEvent = this.LastEvent;
            dbObj.LastEventDate = this.LastEventDate;
            dbObj.LastEventBy_PersonnelId = this.LastEventBy_PersonnelId;
            dbObj.CreatedBy = this.CreatedBy;
            dbObj.UpdateIndex = this.UpdateIndex;
             
            */
            return dbObj;
        }
        public b_PartTransfer ToDateBaseObjectForRetriveAllForSearch()
        {
            b_PartTransfer dbObj = this.ToDatabaseObject();
            dbObj.SiteId = this.SiteId;
            dbObj.IsAdmin = this.IsAdmin;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_PartTransfer dbObj,  string TimeZone)
        {

            // SOM-279 Localization of Status            
            //switch (this.Status)
            //{
            //    case Common.Constants.PartTransferStatus.Canceled:
            //        this.Status_Display = "Canceled";
            //        break;
            //    case Common.Constants.PartTransferStatus.Complete:
            //        this.Status_Display = "Complete";
            //        break;
            //    case Common.Constants.PartTransferStatus.Denied:
            //        this.Status_Display = "Denied";
            //        break;
            //    case Common.Constants.PartTransferStatus.ForceCompPend:
            //        this.Status_Display = "ForceCompPend";
            //        break;
            //    case Common.Constants.PartTransferStatus.InTransit:
            //        this.Status_Display = "InTransit";
            //        break;
            //    case Common.Constants.PartTransferStatus.Open:
            //        this.Status_Display = "Open";
            //        break;
            //    case Common.Constants.PartTransferStatus.Waiting:
            //        this.Status_Display = "Waiting";
            //        break;
            //    default:
            //        this.Status_Display = string.Empty;
            //        break;
            //}

        }
        public void UpdateFromDatabaseObjectExtended(b_PartTransfer dbObj,  string Timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.PartTransferEventLogId = dbObj.PartTransferEventLogId;
            this.RequestPart_ClientLookupId = dbObj.RequestPart_ClientLookupId;
            this.RequestSite_Name = dbObj.RequestSite_Name;
            this.RequestPart_Description = dbObj.RequestPart_Description;
            this.CreateBy_PersonnelName = dbObj.CreateBy_PersonnelName;
            this.IssueSite_Name = dbObj.IssueSite_Name;
            this.IssuePart_ClientLookupId = dbObj.IssuePart_ClientLookupId;
            this.IssuePart_QtyOnHand = dbObj.IssuePart_QtyOnHand;
            this.CreateBy_PersonnelName = dbObj.CreateBy_PersonnelName;
            this.IssuePart_Description = dbObj.IssuePart_Description;
            this.LastEventBy_PersonnelName = dbObj.LastEventBy_PersonnelName;

            // SOM-1637 - Convert the create date to the user's time zone
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            // SOM-1637 - Convert the LastEventDate to the user's time zone
            if (dbObj.LastEventDate != null && dbObj.LastEventDate != DateTime.MinValue)
            {
                this.LastEventDate = dbObj.LastEventDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.LastEventDate = dbObj.LastEventDate;
            }

            // Localization of Status            
            //switch (this.Status)
            //{
            //    case  Common.Constants.PartTransferStatus.Canceled:
            //        this.Status_Display = "Canceled";
            //        break;
            //    case Common.Constants.PartTransferStatus.Complete:
            //        this.Status_Display = "Complete";
            //        break;
            //    case Common.Constants.PartTransferStatus.Denied:
            //        this.Status_Display = "Denied";
            //        break;
            //    case Common.Constants.PartTransferStatus.ForceCompPend:
            //        this.Status_Display = "ForceCompPend";
            //        break;
            //    case Common.Constants.PartTransferStatus.InTransit:
            //        this.Status_Display = "InTransit";
            //        break;
            //    case Common.Constants.PartTransferStatus.Open:
            //        this.Status_Display = "Open";
            //        break;
            //    case Common.Constants.PartTransferStatus.Waiting:
            //        this.Status_Display = "Waiting";
            //        break;
            //    default:
            //        this.Status_Display = string.Empty;
            //        break;
            //}
            // Localization of Events
            //switch (this.LastEvent)
            //{
            //    case Common.Constants.PartTransferEvents.Canceled:
            //        this.LastEvent = "Canceled";
            //        break;
            //    case Common.Constants.PartTransferEvents.Complete:
            //        this.LastEvent = "Complete";
            //        break;
            //    case Common.Constants.PartTransferEvents.Created:
            //        this.LastEvent = "Created";
            //        break;
            //    case Common.Constants.PartTransferEvents.Denied:
            //        this.LastEvent = "Denied";
            //        break;
            //    case Common.Constants.PartTransferEvents.ForceComplete:
            //        this.LastEvent = "ForceComplete";
            //        break;
            //    case Common.Constants.PartTransferEvents.ForceCompPend:
            //        this.LastEvent = "ForceCompPend";
            //        break;
            //    case Common.Constants.PartTransferEvents.Issue:
            //        this.LastEvent = "Issue";
            //        break;
            //    case Common.Constants.PartTransferEvents.Receipt:
            //        this.LastEvent = "Receipt";
            //        break;
            //    case Common.Constants.PartTransferEvents.Sent:
            //        this.LastEvent = "Sent";
            //        break;
            //}


        }
        public void UpdateFromDatabaseObjectShipment(b_PartTransfer dbObj,  string Timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.RequestPart_ClientLookupId = dbObj.RequestPart_ClientLookupId;
            this.RequestSite_Name = dbObj.RequestSite_Name;
            this.RequestPart_Description = dbObj.RequestPart_Description;
            this.RequesterName = dbObj.RequesterName;
            this.IssueSite_Name = dbObj.IssueSite_Name;
            this.IssuePart_ClientLookupId = dbObj.IssuePart_ClientLookupId;
            this.QtyOnHand = dbObj.QtyOnHand;            
            this.IssuePart_Description = dbObj.IssuePart_Description;
            this.ShipperName = dbObj.ShipperName;
            this.IssueSite_Address1 = dbObj.IssueSite_Address1;
            this.IssueSite_Address2 = dbObj.IssueSite_Address2;
            this.IssueSite_CityStateZip = dbObj.IssueSite_CityStateZip;

            this.RequestSite_Address1 = dbObj.RequestSite_Address1;
            this.RequestSite_Address2 = dbObj.RequestSite_Address2;
            this.RequestSite_CityStateZip = dbObj.RequestSite_CityStateZip;
           
            if (dbObj.TransactionDate != null && dbObj.TransactionDate != DateTime.MinValue)
            {
                this.TransactionDate = dbObj.TransactionDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.TransactionDate = dbObj.TransactionDate;
            }
            // Localization of Status            
            //switch (this.Status)
            //{
            //    case Common.Constants.PartTransferStatus.Canceled:
            //        this.Status_Display = "Canceled";
            //        break;
            //    case Common.Constants.PartTransferStatus.Complete:
            //        this.Status_Display = "Complete";
            //        break;
            //    case Common.Constants.PartTransferStatus.Denied:
            //        this.Status_Display = "Denied";
            //        break;
            //    case Common.Constants.PartTransferStatus.ForceCompPend:
            //        this.Status_Display = "ForceCompPend";
            //        break;
            //    case Common.Constants.PartTransferStatus.InTransit:
            //        this.Status_Display = "InTransit";
            //        break;
            //    case Common.Constants.PartTransferStatus.Open:
            //        this.Status_Display = "Open";
            //        break;
            //    case Common.Constants.PartTransferStatus.Waiting:
            //        this.Status_Display = "Waiting";
            //        break;
            //    default:
            //        this.Status_Display = string.Empty;
            //        break;
            //}
            // Localization of Events
            //switch (this.LastEvent)
            //{
            //    case Common.Constants.PartTransferEvents.Canceled:
            //        this.LastEvent = "Canceled";
            //        break;
            //    case Common.Constants.PartTransferEvents.Complete:
            //        this.LastEvent = "Complete";
            //        break;
            //    case Common.Constants.PartTransferEvents.Created:
            //        this.LastEvent = "Created";
            //        break;
            //    case Common.Constants.PartTransferEvents.Denied:
            //        this.LastEvent = "Denied";
            //        break;
            //    case Common.Constants.PartTransferEvents.ForceComplete:
            //        this.LastEvent = "ForceComplete";
            //        break;
            //    case Common.Constants.PartTransferEvents.ForceCompPend:
            //        this.LastEvent = "ForceCompPend";
            //        break;
            //    case Common.Constants.PartTransferEvents.Issue:
            //        this.LastEvent = "Issue";
            //        break;
            //    case Common.Constants.PartTransferEvents.Receipt:
            //        this.LastEvent = "Receipt";
            //        break;
            //    case Common.Constants.PartTransferEvents.Sent:
            //        this.LastEvent = "Sent";
            //        break;
            //}


        }

    }
}
