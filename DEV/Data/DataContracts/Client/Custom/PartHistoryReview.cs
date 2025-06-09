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
* Date         JIRA Item Person                 Description
* ===========  ========= ====================== =================================================
* 2015-Apr-07  SOM-637   Roger Lawton           Added columns to the 
*                                               UpdateFromDatabaseObjectRetriveAll method
* 2015-Apr-16  SOM-637   Roger Lawton           Localize Charge Type and Transaction Type
**************************************************************************************************
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

namespace DataContracts
{
    public partial class PartHistoryReview : DataContractBase
    {
        public long SLNo { get; set; }
        public long ClientId { get; set; }
        public long AccountId { get; set; }
        public string ChargeType_Primary { get; set; }
        public long ChargeToId_Primary { get; set; }
        public long RequestorId { get; set; }
        public string Comments { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public long PerformById { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal TransactionQuantity { get; set; }
        public string TransactionType { get; set; }
        public string UnitofMeasure { get; set; }
        public string Account_ClientLookupId { get; set; }
        public string Account_Name { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string Requestor_Name { get; set; }
        public string PerformBy_Name { get; set; }
        public string PurchaseOrder_ClientLookupId { get; set; }
        public string Vendor_ClientLookupId { get; set; }
        public string Vendor_Name { get; set; }
        public long PartId { get; set; }
        public bool Receipts { get; set; }
        public bool Reversals { get; set; }
        public string DateRange { get; set; }

        public List<b_PartHistoryReview> PartHistoryList { get; set; }

        private bool m_validateClientLookupId;
        public string Storeroom { get; set; } //V2-1033

        #region Transactions


        public b_PartHistoryReview ToDateBaseObjectForRetrive()
        {
            b_PartHistoryReview dbObj = new b_PartHistoryReview();

            dbObj.ClientId = this.ClientId;
            dbObj.PartId = this.PartId;
            dbObj.TransactionType = this.TransactionType;
            dbObj.Receipts = this.Receipts;
            dbObj.Reversals = this.Reversals;
            dbObj.DateRange = this.DateRange;

            //dbObj.TransactionType = this.TransactionType;
            //dbObj.Requestor_Name = this.Requestor_Name;
            //dbObj.PerformBy_Name = this.PerformBy_Name;
            //dbObj.TransactionDate = this.TransactionDate;
            //dbObj.TransactionQuantity = this.TransactionQuantity;
            //dbObj.ChargeType_Primary = this.ChargeType_Primary;
            //dbObj.ChargeTo_Name = this.ChargeTo_Name;
            //dbObj.Account_ClientLookupId = this.Account_ClientLookupId;
            //dbObj.PurchaseOrder_ClientLookupId = this.PurchaseOrder_ClientLookupId;
            //dbObj.Vendor_ClientLookupId = this.Vendor_ClientLookupId;
            //dbObj.Vendor_Name = this.Vendor_Name;

            return dbObj;
        }
        public void UpdateFromDatabaseObjectRetriveAll(b_PartHistoryReview dbObj, string Timezone)
        {
            this.ClientId = dbObj.ClientId;
            this.PartId = dbObj.PartId;
            this.Receipts = dbObj.Receipts;
            this.Reversals = dbObj.Reversals;
            this.TransactionType = dbObj.TransactionType;
            this.DateRange = dbObj.DateRange;

            this.SLNo = dbObj.SLNo;
            this.TransactionType = dbObj.TransactionType;
            this.Requestor_Name = dbObj.Requestor_Name;
            this.PerformBy_Name = dbObj.PerformBy_Name;
            #region V2-1180
            if (dbObj.TransactionDate != null && dbObj.TransactionDate != DateTime.MinValue)
            {
                this.TransactionDate = dbObj.TransactionDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.TransactionDate = dbObj.TransactionDate;
            }
            #endregion
            this.TransactionQuantity = dbObj.TransactionQuantity;
            this.ChargeType_Primary = dbObj.ChargeType_Primary;
            this.ChargeTo_ClientLookupId = dbObj.ChargeTo_ClientLookupId;
            this.Account_ClientLookupId = dbObj.Account_ClientLookupId;
            this.PurchaseOrder_ClientLookupId = dbObj.PurchaseOrder_ClientLookupId;
            this.Vendor_ClientLookupId = dbObj.Vendor_ClientLookupId;
            this.Vendor_Name = dbObj.Vendor_Name;
            // som-637
            this.Cost = dbObj.Cost;
            // SOM-637 - RKL - Transaction Type
            switch (dbObj.TransactionType)
            {
                case Common.Constants.PartHistoryTranTypes.Adjustment:
                    this.TransactionType = "Adjustment";
                    break;
                case Common.Constants.PartHistoryTranTypes.NonPOReceipt:
                    this.TransactionType = "Non-PO Receipt";
                    break;
                case Common.Constants.PartHistoryTranTypes.PartAdd:
                    this.TransactionType = "Part Add";
                    break;
                case Common.Constants.PartHistoryTranTypes.PartDelete:
                    this.TransactionType = "Part Delete";
                    break;
                case Common.Constants.PartHistoryTranTypes.PartIssue:
                    this.TransactionType = "Part Issue";
                    break;
                case Common.Constants.PartHistoryTranTypes.PurchaseIssue:
                    this.TransactionType = "Purchase Issue";
                    break;
                case Common.Constants.PartHistoryTranTypes.Receipt:
                    this.TransactionType = "Receipt";
                    break;
                case Common.Constants.PartHistoryTranTypes.ReceiptReverse:
                    this.TransactionType = "Receipt Reversal";
                    break;
                case Common.Constants.PartHistoryTranTypes.StockOut:
                    this.TransactionType = "Stock Out";
                    break;
                case Common.Constants.PartHistoryTranTypes.StockOutAdjustment:
                    this.TransactionType = "Stock Out Adjustment";
                    break;
                case Common.Constants.PartHistoryTranTypes.CostChange:
                    this.TransactionType = "Cost Change";
                    break;
                case Common.Constants.PartHistoryTranTypes.DirectInput:
                    this.TransactionType = "Direct Input";
                    break;
                case Common.Constants.PartHistoryTranTypes.TransferIssue:
                    this.TransactionType = "Transfer Issue";
                    break;
                case Common.Constants.PartHistoryTranTypes.TransferReceipt:
                    this.TransactionType = "Transfer Receipt";
                    break;
                case Common.Constants.PartHistoryTranTypes.TransferAdjust:
                    this.TransactionType = "Transfer Adjust";
                    break;
                case Common.Constants.PartHistoryTranTypes.StoreroomAdd:
                    this.TransactionType = "Storeroom Add";
                    break;
                default:
                    this.TransactionType = string.Empty;
                    break;
            }
            // SOM-637 - Charge Type
            switch (dbObj.ChargeType_Primary)
            {
                case Common.Constants.ChargeType.Equipment:
                    this.ChargeType_Primary = "Equipment";
                    break;
                case Common.Constants.ChargeType.WorkOrder:
                    this.ChargeType_Primary = "Work Order";
                    break;
                case Common.Constants.ChargeType.Account:
                    this.ChargeType_Primary = "Account";
                    break;
                case Common.Constants.ChargeType.Location:
                    this.ChargeType_Primary = "Location";
                    break;
                default:
                    this.ChargeType_Primary = string.Empty;
                    break;

            }
            this.Storeroom = dbObj.Storeroom;
        }

        public static List<PartHistoryReview> RetrievePartHistoryReview(DatabaseKey dbKey, PartHistoryReview parthistoryreview, string TimeZone)
        {
            PartHistoryReview_Retrieve trans = new PartHistoryReview_Retrieve()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartHistoryReview = parthistoryreview.ToDateBaseObjectForRetrive();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PartHistoryReview.UpdateFromDatabaseObjectList(trans.PartHistoryReviewList, TimeZone);
        }
        public static List<PartHistoryReview> UpdateFromDatabaseObjectList(List<b_PartHistoryReview> dbObjs, string TimeZone)
        {
            List<PartHistoryReview> result = new List<PartHistoryReview>();

            foreach (b_PartHistoryReview dbObj in dbObjs)
            {
                PartHistoryReview tmp = new PartHistoryReview();
                tmp.UpdateFromDatabaseObjectRetriveAll(dbObj, TimeZone);

                {
                }
                result.Add(tmp);
            }
            return result;
        }
        #endregion

        #region V2-760
        public static List<PartHistoryReview> RetrievePartHistoryReview_V2(DatabaseKey dbKey, PartHistoryReview parthistoryreview, string TimeZone)
        {
            PartHistoryReview_Retrieve_V2 trans = new PartHistoryReview_Retrieve_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartHistoryReview = parthistoryreview.ToDateBaseObjectForRetrive();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PartHistoryReview.UpdateFromDatabaseObjectList(trans.PartHistoryReviewList, TimeZone);
        }
        #endregion
    }
}
