/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2014-Nov-11 SOM-419  Roger Lawton       Corrected some issues found in testing
*                                         Changed instances of StoreRoom to Storeroom 
* 2014-Nov-23 SOM-456  Roger Lawton       Not receiving if on a page other than page 1
* 2015-Feb-04 SOM-529  Roger Lawton       Modified the way the po header status is updated
* 2015-Mar-05 SOM-594  Roger Lawton       Modified the way the po header status is updated
* 2016-Apr-04 SOM-960  Roger Lawton       Add SiteId
****************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;
using System.Data.SqlClient;
using Common.Extensions;
using static Database.POReceiptItem_RetrievePOFromParts;

namespace DataContracts
{
    public partial class POReceipt : POReceiptItem
    {
        public POReceipt()
        {
            POPart = new Part();
            POPartStoreRoom = new PartStoreroom();
            POPartHistory = new PartHistory();
            POReceiptItem = new POReceiptItem();
            POReceiptHeader = new POReceiptHeader();
            POlineItem = new PurchaseOrderLineItem();
            POPurchaseOrder = new PurchaseOrder();
            POEstimatedCosts = new EstimatedCosts();


        }


        #region field
        public string Createby { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public Int64 VendorId { get; set; }
        public Part POPart { get; set; }
        public PartStoreroom POPartStoreRoom { get; set; }
        public PartHistory POPartHistory { get; set; }
        public POReceiptItem POReceiptItem { get; set; }
        public Int64 PurchaseOrderId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public Int64 PartId { get; set; }
        public string Description { get; set; }
        public decimal TotalCost { get; set; }
        public string POClientLookupId { get; set; }
        public string PartClientLookupId { get; set; }
        public int PurchaseOrderLineNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public decimal OrderQuantity { get; set; }
        public string Status { get; set; }
        public Int64 SiteId { get; set; }
        public string DateRange { get; set; }

        public POReceiptHeader POReceiptHeader { get; set; }
        public PurchaseOrder POPurchaseOrder { get; set; }
        public PurchaseOrderLineItem POlineItem { get; set; }
        //V2-947
        public List<POReceipt> POReceiptItemlist { get; set; }
        public List<POReceipt> POLineItemsList { get; set; }
        public string AccountClientLookupId { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ManufacturerId { get; set; }
        public int ReceiptNumber { get; set; }
        public int LineNumber { get; set; }
        public string Location { get; set; }
        //V2-947
        #region V2-1011
        public POHeaderUDF POHeaderUDF { get; set; }
        public List<Notes> listOfNotes { get; set; }
        #endregion
        #region V2-1124
        public EstimatedCosts POEstimatedCosts { get; set; }
        #endregion
        #endregion
        public List<POReceipt> RetrievePOFromParts(DatabaseKey dbKey, string UserTimeZone)
        {
            Database.POReceiptItem_RetrievePOFromParts trans = new POReceiptItem_RetrievePOFromParts()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.POReceiptItem = this.ToDatabaseObjectRetrievePOFromParts();
            trans.POReceiptItem.VendorId = this.VendorId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return UpdateFromDatabaseObjectRetrievePOFromParts(trans.POReceiptItemList, UserTimeZone);
        }
        public b_POReceiptItem ToDatabaseObjectRetrievePOFromParts()
        {
            b_POReceiptItem dbObj = new b_POReceiptItem();
            dbObj.ClientId = this.ClientId;
            dbObj.PartId = this.PartId;
            dbObj.SiteId = this.SiteId;
            dbObj.DateRange = this.DateRange;
            return dbObj;
        }
        public List<POReceipt> UpdateFromDatabaseObjectRetrievePOFromParts(List<b_POReceiptItem> dbObjs, string UserTimeZone)
        {
            List<POReceipt> result = new List<POReceipt>();

            foreach (b_POReceiptItem dbObj in dbObjs)
            {
                POReceipt tmp = new POReceipt();
                tmp.POClientLookupId = dbObj.POClientLookupId;
                tmp.PurchaseOrderId = dbObj.PurchaseOrderId;
                tmp.OrderDate = dbObj.OrderDate.ToUserTimeZone(UserTimeZone);
                tmp.VendorClientLookupId = dbObj.VendorClientLookupId;
                tmp.PartId = dbObj.SiteId;
                tmp.VendorName = dbObj.VendorName;
                tmp.OrderQuantity = dbObj.OrderQuantity;
                tmp.UnitCost = dbObj.UnitCost;
                tmp.UnitOfMeasure = dbObj.UnitOfMeasure;
                tmp.Status = dbObj.Status;
                result.Add(tmp);
            }
            return result;
        }

        public b_POReceiptHeader ToDatabaseObjectPOReceiptHeader()
        {
            b_POReceiptHeader dbObj = new b_POReceiptHeader();

            dbObj.ClientId = POReceiptHeader.ClientId;
            dbObj.POReceiptHeaderId = POReceiptHeader.POReceiptHeaderId;
            dbObj.PurchaseOrderId = POReceiptHeader.PurchaseOrderId;
            dbObj.Carrier = POReceiptHeader.Carrier;
            dbObj.Comments = POReceiptHeader.Comments;
            dbObj.FreightAmount = POReceiptHeader.FreightAmount;
            dbObj.FreightBill = POReceiptHeader.FreightBill;
            dbObj.PackingSlip = POReceiptHeader.PackingSlip;
            dbObj.ReceiptNumber = POReceiptHeader.ReceiptNumber;
            dbObj.ReceiveBy_PersonnelID = POReceiptHeader.ReceiveBy_PersonnelID;
            dbObj.ReceiveDate = POReceiptHeader.ReceiveDate;
            dbObj.UpdateIndex = POReceiptHeader.UpdateIndex;
            UpdateIndex = 0;

            return dbObj;
        }

        public List<POReceipt> RetrieveAllNonInvoiced(DatabaseKey dbKey)
        {

            POReceiptItem_RetrievePOFromParts.POReceiptItem_RetrieveAllNonInvoiced trans = new POReceiptItem_RetrievePOFromParts.POReceiptItem_RetrieveAllNonInvoiced()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.POReceiptItem = this.ToDatabaseObject();
            trans.POReceiptItem.VendorId = this.VendorId;
            trans.POReceiptItem.SiteId = this.SiteId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return UpdateFromDatabaseObjectList(trans.POReceiptItemList);
        }
        public List<POReceipt> UpdateFromDatabaseObjectList(List<b_POReceiptItem> dbObjs)
        {
            List<POReceipt> result = new List<POReceipt>();

            foreach (b_POReceiptItem dbObj in dbObjs)
            {
                POReceipt tmp = new POReceipt();
                tmp.UpdateFromDataBaseObjectExtendedList(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDataBaseObjectExtendedList(b_POReceiptItem dbObj)
        {
            //this.ClientId = dbObj.ClientId;
            //this.POReceiptItemId = dbObj.POReceiptItemId;
            //this.POReceiptHeaderId = dbObj.POReceiptHeaderId;
            //this.PurchaseOrderLineItemId = dbObj.PurchaseOrderLineItemId;
            //this.AccountId = dbObj.AccountId;
            //this.Invoiced = dbObj.Invoiced;
            //this.QuantityReceived = dbObj.QuantityReceived;
            //this.Reversed = dbObj.Reversed;
            //this.ReversedBy_PersonnelId = dbObj.ReversedBy_PersonnelId;
            //this.ReversedComments = dbObj.ReversedComments;
            //this.ReversedDate = dbObj.ReversedDate;
            //this.UnitCost = dbObj.UnitCost;
            //this.UnitOfMeasure = dbObj.UnitOfMeasure;
            //this.POClientLookupId = dbObj.POClientLookupId;
            //this.ReceivedDate = dbObj.ReceivedDate;
            //this.PartId = dbObj.PartId;
            //this.TotalCost = dbObj.TotalCost;
            //this.Description = dbObj.Description;
            //this.PurchaseOrderId = dbObj.PurchaseOrderId;
            //this.UpdateIndex = dbObj.UpdateIndex;

            this.POReceiptItemId = dbObj.POReceiptItemId;
            this.PurchaseOrderId = dbObj.PurchaseOrderId;
            this.POClientLookupId = dbObj.POClientLookupId;
            this.ReceivedDate = dbObj.ReceivedDate;
            this.PartId = dbObj.SiteId;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.Description = dbObj.Description;
            this.QuantityReceived = dbObj.QuantityReceived;
            this.UnitOfMeasure = dbObj.UnitOfMeasure;
            this.UnitCost = dbObj.UnitCost;
            this.TotalCost = dbObj.TotalCost;
            this.AccountId = dbObj.AccountId;
        }
        public void PO_Receipt(DatabaseKey dbKey)
        {
            // Validate<WorkOrder>(dbKey);
            POReceiptItem_RetrievePOFromParts.PurchaseOrder_POReceipt trans = new POReceiptItem_RetrievePOFromParts.PurchaseOrder_POReceipt()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

         
            trans.POReceiptItem = this.ToDateBaseObjectRetrievePOFromParts();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // Have to update the Purchase order Update Index in order to support the loop
            this.POPurchaseOrder.UpdateIndex = trans.POReceiptItem.POPurchaseOrder.UpdateIndex;

            //}
        }
        public b_POReceiptItem ToDateBaseObjectRetrievePOFromParts()
        {
            b_POReceiptItem dbObj = this.ToDatabaseObject();          
            dbObj.POPurchaseOrder = this.ToDatabaseObjectPurchaseOrder(); //SOM-529
            dbObj.POHeader = this.ToDatabaseObjectPOReceiptHeader();      //SOM-594
            dbObj.POlineItem = ToDatabaseObjectPoLineItem();
            dbObj.POPart = ToDatabaseObjectPart();
            dbObj.POPartStoreRoom = ToDatabaseObjectPartStoreRoom();
            dbObj.POPartHistory = ToDatabaseObjectPartHistory();
            dbObj.POEstimatedCosts = ToDatabaseObjectEstimatedCosts();

            return dbObj;
        }


        public void PO_Receipt_CreateHeader(DatabaseKey dbKey)
        {
            // Validate<WorkOrder>(dbKey);

            //if (IsValid)
            // {
            POReceiptItem_RetrievePOFromParts.PurchaseOrder_POReceiptCreateHeader trans = new POReceiptItem_RetrievePOFromParts.PurchaseOrder_POReceiptCreateHeader()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.POReceiptItem = new b_POReceiptItem();

            trans.POReceiptItem.POHeader = ToDatabaseObjectPOReceiptHeader();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.POReceiptHeaderId = trans.POReceiptItem.POHeader.POReceiptHeaderId;


            //}
        }

        public b_PurchaseOrderLineItem ToDatabaseObjectPoLineItem()
        {
            b_PurchaseOrderLineItem dbObj = new b_PurchaseOrderLineItem();
            dbObj.ClientId = POlineItem.ClientId;
            dbObj.PurchaseOrderLineItemId = POlineItem.PurchaseOrderLineItemId;
            dbObj.PurchaseOrderId = POlineItem.PurchaseOrderId;
            dbObj.DepartmentId = POlineItem.DepartmentId;
            dbObj.StoreroomId = POlineItem.StoreroomId;
            dbObj.AccountId = POlineItem.AccountId;
            dbObj.ChargeToId = POlineItem.ChargeToId;
            dbObj.ChargeType = POlineItem.ChargeType;
            dbObj.CompleteBy_PersonnelId = POlineItem.CompleteBy_PersonnelId;
            dbObj.CompleteDate = POlineItem.CompleteDate;
            dbObj.Creator_PersonnelId = POlineItem.Creator_PersonnelId;
            dbObj.Description = POlineItem.Description;
            dbObj.EstimatedDelivery = POlineItem.EstimatedDelivery;
            dbObj.LineNumber = POlineItem.LineNumber;
            dbObj.PartId = POlineItem.PartId;
            dbObj.PartStoreroomId = POlineItem.PartStoreroomId;
            dbObj.PRCreator_PersonnelId = POlineItem.PRCreator_PersonnelId;
            dbObj.PurchaseRequestId = POlineItem.PurchaseRequestId;
            dbObj.OrderQuantity = POlineItem.OrderQuantity;
            dbObj.Status = POlineItem.Status;
            dbObj.Taxable = POlineItem.Taxable;
            dbObj.UnitOfMeasure = POlineItem.UnitOfMeasure;
            dbObj.UnitCost = POlineItem.UnitCost;
            dbObj.UpdateIndex = POlineItem.UpdateIndex;
            dbObj.ClientId = POlineItem.ClientId;
            dbObj.PurchaseOrderLineItemId = POlineItem.PurchaseOrderLineItemId;
            dbObj.PurchaseOrderId = POlineItem.PurchaseOrderId;
            dbObj.DepartmentId = POlineItem.DepartmentId;
            dbObj.StoreroomId = POlineItem.StoreroomId;
            dbObj.AccountId = POlineItem.AccountId;
            dbObj.ChargeToId = POlineItem.ChargeToId;
            dbObj.ChargeType = POlineItem.ChargeType;
            dbObj.CompleteBy_PersonnelId = POlineItem.CompleteBy_PersonnelId;
            dbObj.CompleteDate = POlineItem.CompleteDate;
            dbObj.Creator_PersonnelId = POlineItem.Creator_PersonnelId;
            dbObj.Description = POlineItem.Description;
            dbObj.EstimatedDelivery = POlineItem.EstimatedDelivery;
            dbObj.LineNumber = POlineItem.LineNumber;
            dbObj.PartId = POlineItem.PartId;
            dbObj.PartStoreroomId = POlineItem.PartStoreroomId;
            dbObj.PRCreator_PersonnelId = POlineItem.PRCreator_PersonnelId;
            dbObj.PurchaseRequestId = POlineItem.PurchaseRequestId;
            dbObj.OrderQuantity = POlineItem.OrderQuantity;
            dbObj.Status = POlineItem.Status;
            dbObj.Taxable = POlineItem.Taxable;
            dbObj.UnitOfMeasure = POlineItem.UnitOfMeasure;
            dbObj.UnitCost = POlineItem.UnitCost;
            dbObj.UpdateIndex = POlineItem.UpdateIndex;

            return dbObj;
        }
        public b_Part ToDatabaseObjectPart()
        {
            b_Part dbObj = new b_Part();
            dbObj.ClientId = POPart.ClientId;
            dbObj.PartId = POPart.PartId;
            dbObj.AverageCost = POPart.AverageCost;
            dbObj.AppliedCost = POPart.AppliedCost;
            dbObj.UpdateIndex = POPart.UpdateIndex;

            return dbObj;
        }
        public b_PartStoreroom ToDatabaseObjectPartStoreRoom()
        {
            b_PartStoreroom dbObj = new b_PartStoreroom();
            dbObj.ClientId = POPartStoreRoom.ClientId;
            dbObj.PartStoreroomId = POPartStoreRoom.PartStoreroomId;
            dbObj.QtyOnHand = POPartStoreRoom.QtyOnHand;
            dbObj.UpdateIndex = POPartStoreRoom.UpdateIndex;

            return dbObj;
        }
        public b_PartHistory ToDatabaseObjectPartHistory()
        {
            b_PartHistory dbObj = new b_PartHistory();
            dbObj.ClientId = POPartHistory.ClientId;
            dbObj.PartHistoryId = POPartHistory.PartHistoryId;
            dbObj.PartId = POPartHistory.PartId;
            dbObj.PartStoreroomId = POPartHistory.PartStoreroomId;
            dbObj.AccountId = POPartHistory.AccountId;
            dbObj.AverageCostAfter = POPartHistory.AverageCostAfter;
            dbObj.AverageCostBefore = POPartHistory.AverageCostBefore;
            dbObj.ChargeToId_Primary = POPartHistory.ChargeToId_Primary;
            dbObj.ChargeType_Primary = POPartHistory.ChargeType_Primary;
            dbObj.Comments = POPartHistory.Comments;
            dbObj.Cost = POPartHistory.Cost;
            dbObj.CostAfter = POPartHistory.CostAfter;
            dbObj.CostBefore = POPartHistory.CostBefore;
            dbObj.Description = POPartHistory.Description;
            dbObj.PerformedById = POPartHistory.PerformedById;
            dbObj.QtyAfter = POPartHistory.QtyAfter;
            dbObj.QtyBefore = POPartHistory.QtyBefore;
            dbObj.RequestorId = POPartHistory.RequestorId;
            dbObj.StockType = POPartHistory.StockType;
            dbObj.StoreroomId = POPartHistory.StoreroomId;
            dbObj.TransactionDate = POPartHistory.TransactionDate;
            dbObj.TransactionQuantity = POPartHistory.TransactionQuantity;
            dbObj.TransactionType = POPartHistory.TransactionType;
            dbObj.UnitofMeasure = POPartHistory.UnitofMeasure;
            dbObj.CreatedBy = POPartHistory.CreatedBy;
            dbObj.CreatedDate = POPartHistory.CreatedDate;

            return dbObj;
        }

        public b_PurchaseOrder ToDatabaseObjectPurchaseOrder()
        {
            b_PurchaseOrder dbObj = new b_PurchaseOrder();
            dbObj.ClientId = POPurchaseOrder.ClientId;
            dbObj.PurchaseOrderId = POPurchaseOrder.PurchaseOrderId;
            dbObj.SiteId = POPurchaseOrder.SiteId;
            dbObj.DepartmentId = POPurchaseOrder.DepartmentId;
            dbObj.AreaId = POPurchaseOrder.AreaId;
            dbObj.StoreroomId = POPurchaseOrder.StoreroomId;
            dbObj.ClientLookupId = POPurchaseOrder.ClientLookupId;
            dbObj.Attention = POPurchaseOrder.Attention;
            dbObj.Buyer_PersonnelId = POPurchaseOrder.Buyer_PersonnelId;
            dbObj.Carrier = POPurchaseOrder.Carrier;
            dbObj.CompleteBy_PersonnelId = POPurchaseOrder.CompleteBy_PersonnelId;
            dbObj.CompleteDate = POPurchaseOrder.CompleteDate;
            dbObj.Creator_PersonnelId = POPurchaseOrder.Creator_PersonnelId;
            dbObj.FOB = POPurchaseOrder.FOB;
            dbObj.Status = POPurchaseOrder.Status;
            dbObj.Terms = POPurchaseOrder.Terms;
            dbObj.VendorId = POPurchaseOrder.VendorId;
            dbObj.VoidBy_PersonnelId = POPurchaseOrder.VoidBy_PersonnelId;
            dbObj.VoidDate = POPurchaseOrder.VoidDate;
            dbObj.VoidReason = POPurchaseOrder.VoidReason;
            dbObj.UpdateIndex = POPurchaseOrder.UpdateIndex;

            return dbObj;
        }
        public void PO_ReverseReceipt(DatabaseKey dbKey)
        {

            PurchaseOrder_POReverseReceipt trans = new PurchaseOrder_POReverseReceipt()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.POReceiptItem = ToDatabaseObject();          
            trans.POReceiptItem.POlineItem = ToDatabaseObjectPoLineItem();
            trans.POReceiptItem.POPart = ToDatabaseObjectPart();
            trans.POReceiptItem.POPartStoreRoom = ToDatabaseObjectPartStoreRoom();
            trans.POReceiptItem.POPartHistory = ToDatabaseObjectPartHistory();
            trans.POReceiptItem.POPurchaseOrder = ToDatabaseObjectPurchaseOrder();
            trans.POReceiptItem.POHeader = this.ToDatabaseObjectPOReceiptHeader();      //SOM-594
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // Have to update the Purchase order Update Index in order to support the loop
            this.POPurchaseOrder.UpdateIndex = trans.POReceiptItem.POPurchaseOrder.UpdateIndex;

            //}
        }
        #region V2-947
        public POReceipt RetrievePORPrintV2(DatabaseKey dbKey, string UserTimeZone)
        {
           POReceiptRetrieveAllPrint trans = new POReceiptRetrieveAllPrint()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.POReceiptItem = this.ToDatabaseObjectRetrievePORPrint();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            POReceipt objPOReceipt = new POReceipt();
            //PurchaseOrder Receipt Details
            PurchaseOrder tempPOlineItem = new PurchaseOrder();
            tempPOlineItem.UpdateFromDatabaseObjectRetrieveDetailsPOR(trans.POReceiptItem.POPurchaseOrder, UserTimeZone);
            objPOReceipt.POPurchaseOrder = tempPOlineItem;
            objPOReceipt.POReceiptItemlist = objPOReceipt.UpdateFromDatabaseObjectRetrieveLineItem1POR(trans.POReceiptItem.POReceiptItemlist, UserTimeZone);
            #region V2-1011
            POHeaderUDF tmppoheaderudf = new POHeaderUDF();

            tmppoheaderudf.UpdateFromDatabaseObjectPOHeaderUDFPrintExtended(trans.POReceiptItem.POHeaderUDF, UserTimeZone);
            objPOReceipt.POHeaderUDF = tmppoheaderudf;
            List<Notes> NotesList = new List<Notes>();
            objPOReceipt.listOfNotes = new List<Notes>();
            foreach (b_Notes Notes in trans.POReceiptItem.listOfNotes)
            {
                Notes tmpNotes = new Notes();

                tmpNotes.UpdateFromExtendedDatabaseObject(Notes, UserTimeZone);
                NotesList.Add(tmpNotes);
            }
            objPOReceipt.listOfNotes.AddRange(NotesList);
            #endregion

            return objPOReceipt;
        }
        public b_POReceiptItem ToDatabaseObjectRetrievePORPrint()
        {
            b_POReceiptItem dbObj = new b_POReceiptItem();
            dbObj.ClientId = this.ClientId;
            dbObj.POReceiptHeaderId = this.POReceiptHeaderId;
            dbObj.SiteId = this.SiteId;
            dbObj.PurchaseOrderId = this.PurchaseOrderId;
            return dbObj;
        }
        public List<POReceipt> UpdateFromDatabaseObjectRetrieveLineItem1POR(List<b_POReceiptItem> dbObjs, string UserTimeZone)
        {
            List<POReceipt> result = new List<POReceipt>();

            foreach (b_POReceiptItem dbObj in dbObjs)
            {
                POReceipt tmp = new POReceipt();
                tmp.LineNumber = dbObj.LineNumber;
                tmp.PartClientLookupId = dbObj.PartClientLookupId;
                tmp.Description = dbObj.Description;
                tmp.ManufacturerId = dbObj.ManufacturerId;
                tmp.Location = dbObj.Location;
               
                tmp.QuantityReceived = dbObj.QuantityReceived;
                tmp.UnitCost = dbObj.UnitCost;
                tmp.UnitOfMeasure = dbObj.UnitOfMeasure;
                tmp.TotalCost = dbObj.TotalCost;
                tmp.ReceiptNumber = dbObj.ReceiptNumber;
                tmp.AccountClientLookupId = dbObj.AccountClientLookupId;
                tmp.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
                result.Add(tmp);
            }
            return result;
        }
        #endregion

        #region V2-1124
        public b_EstimatedCosts ToDatabaseObjectEstimatedCosts()
        {
            b_EstimatedCosts dbObj = new b_EstimatedCosts();
            dbObj.ClientId = POEstimatedCosts.ClientId;
            dbObj.EstimatedCostsId = POEstimatedCosts.EstimatedCostsId;
            dbObj.ObjectType = POEstimatedCosts.ObjectType;
            dbObj.ObjectId = POEstimatedCosts.ObjectId;
            dbObj.Category = POEstimatedCosts.Category;
            dbObj.CategoryId = POEstimatedCosts.CategoryId;
            dbObj.Description = POEstimatedCosts.Description;
            dbObj.Duration = POEstimatedCosts.Duration;
            dbObj.UnitCost = POEstimatedCosts.UnitCost;
            dbObj.Quantity = POEstimatedCosts.Quantity;
            dbObj.Source = POEstimatedCosts.Source;
            dbObj.VendorId = POEstimatedCosts.VendorId;
            dbObj.UNSPSC = POEstimatedCosts.UNSPSC;
            dbObj.Status = POEstimatedCosts.Status;
            dbObj.StoreroomId = POEstimatedCosts.StoreroomId;
            dbObj.PartStoreroomId = POEstimatedCosts.PartStoreroomId;
            dbObj.AccountId = POEstimatedCosts.AccountId;
            dbObj.UnitOfMeasure = POEstimatedCosts.UnitOfMeasure;
            dbObj.PurchaseRequestId = POEstimatedCosts.PurchaseRequestId;
            dbObj.PurchaseRequestLineItemId = POEstimatedCosts.PurchaseRequestLineItemId;
            dbObj.UpdateIndex = POEstimatedCosts.UpdateIndex;
            return dbObj;
        }
        #endregion
    }
}
