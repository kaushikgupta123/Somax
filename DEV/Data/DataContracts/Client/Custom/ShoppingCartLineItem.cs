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
* 2017-Oct-18 SOM-1471 Roger Lawton        Changed to have the RetrieveAll Method only retrieve 
*                                          The line items for the specified shopping cart
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
    public partial class ShoppingCartLineItem : DataContractBase, IStoredProcedureValidation
  {
        public string Vendor { get; set; }
        public string Status { get; set; }
        public string CreatedBy_Name { get; set; } 
        public decimal TotalCost { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string VendorID_ClientLookupID { get; set; }
        public string Vendor_Name { get; set; }
        public long SiteId { get; set; }
        public string PartClientLookupId { get; set; }
        // SOM-1471
        public string PMClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string AccountClientLookupId { get; set; }
        public string ErrorMessageRow { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityToDate { get; set; }
        public decimal CurrentAverageCost { get; set; }
        public decimal CurrentAppliedCost { get; set; }
        public decimal CurrentOnHandQuantity { get; set; }
        public string PurchaseRequestNo { get; set; }
        public string PurchaseOrderClientLookupId { get; set; }
        public string ValidateType { get; set; }
        public bool Vendor_InactiveFlag { get; set; }
        public void CreateByFK(DatabaseKey dbKey)
        {
            ShoppingCartLineItem_CreateByFK trans = new ShoppingCartLineItem_CreateByFK();
            trans.ShoppingCartLineItem = this.ToDatabaseObject();
            trans.ShoppingCartLineItem.VendorID_ClientLookupID = this.VendorID_ClientLookupID;
            trans.ShoppingCartLineItem.ChargeToClientLookupId = this.ChargeToClientLookupId;
            trans.ShoppingCartLineItem.SiteId = this.SiteId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.ShoppingCartLineItem);
        }
        public void UpdateLineItem(DatabaseKey dbKey)
        {
            ShoppingCartLineItem_UpdateLineItem trans = new ShoppingCartLineItem_UpdateLineItem();
            trans.ShoppingCartLineItem = this.ToDatabaseObject();
            trans.ShoppingCartLineItem.VendorID_ClientLookupID = this.VendorID_ClientLookupID;
            trans.ShoppingCartLineItem.SiteId = this.SiteId;
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.ShoppingCartLineItem);
        }
        public void ReOrderLineNumber(DatabaseKey dbKey)
        {

            ShoppingCartLineItem_ReOrderLineNumber trans = new ShoppingCartLineItem_ReOrderLineNumber()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ShoppingCartLineItem = this.ToDatabaseObject();

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.ShoppingCartLineItem);

        }
        public void ShoppingCartLineItemRetrieveByShoppingCartLineItemId(DatabaseKey dbKey)
        {
            ShoppingCartLineItem_RetrieveByShoppingCartLineItemId trans = new ShoppingCartLineItem_RetrieveByShoppingCartLineItemId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };


            trans.ShoppingCartLineItem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromExtendedDatabaseObject(trans.ShoppingCartLineItem);
        }
        public void UpdateFromExtendedDatabaseObject(b_ShoppingCartLineItem dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.VendorID_ClientLookupID = string.IsNullOrEmpty(dbObj.VendorID_ClientLookupID) ? "" : dbObj.VendorID_ClientLookupID;
            this.PartClientLookupId = string.IsNullOrEmpty(dbObj.PartClientLookupId) ? "" : dbObj.PartClientLookupId;
            this.TotalCost = dbObj.TotalCost;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.QuantityReceived = dbObj.QuantityReceived;
            this.QuantityToDate = dbObj.QuantityToDate;
            this.CurrentAverageCost = dbObj.CurrentAverageCost;
            this.CurrentAppliedCost = dbObj.CurrentAppliedCost;
            this.CurrentOnHandQuantity = dbObj.CurrentOnHandQuantity;            
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.Vendor_Name = dbObj.Vendor_Name;
        }
        public b_ShoppingCartLineItem ToDatabasObjectExtended()
        {
            b_ShoppingCartLineItem sc = this.ToDatabaseObject();
            sc.SiteId = this.SiteId;
            sc.VendorID_ClientLookupID = this.VendorID_ClientLookupID;
            sc.ChargeToClientLookupId = this.ChargeToClientLookupId;
            return sc;

        }
        public List<ShoppingCartLineItem> RetrieveShoppingCartLineItems(DatabaseKey dbKey)
        {
            ShoppingCart_Retrieve_ForShoppingCart trans = new ShoppingCart_Retrieve_ForShoppingCart()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName

            };
            trans.UseTransaction = false;
            trans.ShoppingCartLineItem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ShoppingCartLineItem> ShoppingCartLineItemList = new List<ShoppingCartLineItem>();
            foreach (b_ShoppingCartLineItem ShoppingCartLineItems in trans.ShoppingCartLineItemList)
            {
                ShoppingCartLineItem tmpShoppingCartLineItems = new ShoppingCartLineItem();

                tmpShoppingCartLineItems.UpdateFromDatabaseObjectForRetrive(ShoppingCartLineItems);
                ShoppingCartLineItemList.Add(tmpShoppingCartLineItems);
            }
            return ShoppingCartLineItemList;
        }
        //SOM-1513
        public List<ShoppingCartLineItem> RetrieveShoppingCartListForPart(DatabaseKey dbKey)
        {
            ShoppingCart_RetrieveShoppingCartListForPart trans = new ShoppingCart_RetrieveShoppingCartListForPart()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName

            };
            trans.UseTransaction = false;
            trans.ShoppingCartLineItem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ShoppingCartLineItem> ShoppingCartLineItemList = new List<ShoppingCartLineItem>();
            foreach (b_ShoppingCartLineItem ShoppingCartLineItems in trans.ShoppingCartLineItemList)
            {
                ShoppingCartLineItem tmpShoppingCartLineItems = new ShoppingCartLineItem();

                tmpShoppingCartLineItems.UpdateFromDatabaseObjectForRetrive(ShoppingCartLineItems);
                ShoppingCartLineItemList.Add(tmpShoppingCartLineItems);
            }
            return ShoppingCartLineItemList;
        }
        #region validation
        public void ValidateNonStock(DatabaseKey dbKey)
        {
          ValidateType = "NonStock";
          Validate<ShoppingCartLineItem>(dbKey);
        }
        public void ValidateCatalog(DatabaseKey dbKey)
        {
          ValidateType = "Catalog";
          Validate<ShoppingCartLineItem>(dbKey);
        }
        public void ValidateNonCatalog(DatabaseKey dbKey)
        {
          ValidateType = "NonCatalog";
          Validate<ShoppingCartLineItem>(dbKey);
        }
        #endregion
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
          List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
          if (ValidateType == "NonStock")
          {
            ShoppingCartLineItem_Validate_NonStockItem trans = new ShoppingCartLineItem_Validate_NonStockItem
            {
              CallerUserInfoId = dbKey.User.UserInfoId,
              CallerUserName = dbKey.UserName,
            };

            trans.ShoppingCartLineItem = this.ToDatabasObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.UseTransaction = false;
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
          if (ValidateType == "Catalog")
          {
            ShoppingCartLineItem_Validate_Catalog trans = new ShoppingCartLineItem_Validate_Catalog
            {
              CallerUserInfoId = dbKey.User.UserInfoId,
              CallerUserName = dbKey.UserName,
            };

            trans.ShoppingCartLineItem = this.ToDatabasObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.UseTransaction = false;
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
          if (ValidateType == "NonCatalog")
          {
            ShoppingCartLineItem_Validate_NonCatalog trans = new ShoppingCartLineItem_Validate_NonCatalog
            {
              CallerUserInfoId = dbKey.User.UserInfoId,
              CallerUserName = dbKey.UserName,
            };

            trans.ShoppingCartLineItem = this.ToDatabasObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.UseTransaction = false;
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
    public void UpdateFromDatabaseObjectForRetrive(b_ShoppingCartLineItem dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Status = dbObj.Status;
            this.CreatedBy_Name = dbObj.CreatedBy_Name;
            this.TotalCost = dbObj.UnitCost * dbObj.OrderQuantity;
            this.PurchaseRequestNo = dbObj.PurchaseRequestNo;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.PMClientLookupId = dbObj.PMClientLookupId;
            this.PurchaseOrderClientLookupId = dbObj.PurchaseOrderClientLookupId;
            this.VendorID_ClientLookupID = dbObj.VendorID_ClientLookupID;
            this.Vendor_Name = dbObj.Vendor_Name;
            this.Vendor_InactiveFlag = dbObj.Vendor_InactiveFlag;
        }
        public List<ShoppingCartLineItem> RetrieveByShoppingCartLineItemId(DatabaseKey dbKey, string Timezone)
        {
            ShoppingCartLineItem_RetrieveByPurchaseOrderLineItem trans = new ShoppingCartLineItem_RetrieveByPurchaseOrderLineItem()

            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ShoppingCartLineItem = new b_ShoppingCartLineItem
            {
                ShoppingCartId = this.ShoppingCartId
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return UpdateFromDatabaseObjectListItem(trans.ShoppingCartLineList, Timezone);
        }
        public static List<ShoppingCartLineItem> UpdateFromDatabaseObjectListItem(List<b_ShoppingCartLineItem> dbObjs, string Timezone)
        {
            List<ShoppingCartLineItem> result = new List<ShoppingCartLineItem>();

            foreach (b_ShoppingCartLineItem dbObj in dbObjs)
            {
                ShoppingCartLineItem tmp = new ShoppingCartLineItem();
                tmp.UpdateFromDatabaseObjectExtended(dbObj, Timezone);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseObjectExtended(b_ShoppingCartLineItem dbObj, string Timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            if (dbObj.RequiredDate != null && dbObj.RequiredDate != DateTime.MinValue)
            {
                this.RequiredDate = dbObj.RequiredDate.ToUserTimeZone(Timezone);
            }
            // SOM-1617
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.TotalCost = Convert.ToDecimal(dbObj.UnitCost * dbObj.OrderQuantity);
            this.Vendor = dbObj.Name;
            this.Vendor_InactiveFlag = dbObj.Vendor_InactiveFlag;
        }
    }
}


