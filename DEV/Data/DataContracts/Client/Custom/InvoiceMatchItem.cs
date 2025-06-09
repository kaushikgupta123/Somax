/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 2015-Oct-21  SOM-822   Roger Lawton      Added DeleteItem Method
* 2016-Nov-18  SOM-1163  Roger Lawton      Updated Methods
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
//using Common.Interfaces;
//using Business.Localization;

//using DevExpress.Data;
//using DevExpress.Data.Filtering;

namespace DataContracts
{
  public partial class InvoiceMatchItem : DataContractBase, IStoredProcedureValidation
  {
    public string DateRange { get; set; }
    public string DateColumn { get; set; }
    public string Assigned { get; set; }
    public string VendorName { get; set; }
    public string Status_Display { get; set; }
    public int PersonnelId { get; set; }
    public string ItemTotal { get; set; }
    public string Varience { get; set; }
    public int CustomQueryDisplayId { get; set; }

    string ValidateFor = string.Empty;
    public string VendorClientLookupId { get; set; }
    public Int64 PurchaseOrderId { get; set; }
    //public decimal OrderQuantity { get; set; }
    public Int64 LineNumber { get; set; }
    //public string Units { get; set; }
    public decimal TotalCost { get; set; }
    public string PurchaseOrder { get; set; }
    public string Account { get; set; }

    private bool m_validateClientLookupId;

    #region Transactions

    public List<b_StoredProcValidationError> ToDatabaseObjectList()
    {
      List<b_StoredProcValidationError> dbObj = new List<b_StoredProcValidationError>();
      return dbObj;
    }

    public b_InvoiceMatchItem ToDateBaseObjectForRetriveAllForSearch()
    {
      b_InvoiceMatchItem dbObj = this.ToDatabaseObject();

      dbObj.DateRange = this.DateRange;
      dbObj.DateColumn = this.DateColumn;
      dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
      return dbObj;
    }

    public List<InvoiceMatchItem> RetrieveByPKForeignKeys(DatabaseKey dbKey)
    {
      InvoiceMatchItem_RetrieveByPKForeignKeys trans = new InvoiceMatchItem_RetrieveByPKForeignKeys()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName
      };

      trans.InvoiceMatchItem = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      return InvoiceMatchItem.UpdateFromDatabaseObjectList(trans.InvoiceMatchItemList);

    }

    public static List<InvoiceMatchItem> UpdateFromDatabaseObjectList(List<b_InvoiceMatchItem> dbObjs)
    {
      List<InvoiceMatchItem> result = new List<InvoiceMatchItem>();

      foreach (b_InvoiceMatchItem dbObj in dbObjs)
      {
        InvoiceMatchItem tmp = new InvoiceMatchItem();
        tmp.UpdateFromDataBaseObjectExtended(dbObj);
        result.Add(tmp);
      }
      return result;
    }

    public void UpdateFromDataBaseObjectExtended(b_InvoiceMatchItem dbObj)
    {
      this.UpdateFromDatabaseObject(dbObj);
      this.LineNumber = dbObj.LineNumber;
      this.PurchaseOrder = dbObj.PurchaseOrder;
      this.TotalCost = dbObj.TotalCost;
      this.Account = dbObj.Account;
    }

    public void RetrieveByPrimaryKey(DatabaseKey dbKey)
    {
      InvoiceMatchItem_RetrieveByPrimaryKey trans = new InvoiceMatchItem_RetrieveByPrimaryKey()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName
      };

      trans.InvoiceMatchItem = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      UpdateFromDataBaseObjectExtended(trans.InvoiceMatchItem);
    }

    public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
    {
      List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

      if (m_validateClientLookupId)
      {
        InvoiceMatchItemValidate trans = new InvoiceMatchItemValidate()
        {
          CallerUserInfoId = dbKey.User.UserInfoId,
          CallerUserName = dbKey.UserName,
          
        };
        trans.InvoiceMatchItem = this.ToDatabaseObject();
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

    public void CreateTo(DatabaseKey dbKey)
    {
      //m_validateClientLookupId = true;
      //Validate<InvoiceMatchItem>(dbKey);
      InvoiceMatchItem_Create trans = new InvoiceMatchItem_Create();

      trans.InvoiceMatchItem = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      // The create procedure may have populated an auto-incremented key. 
      UpdateFromDatabaseObject(trans.InvoiceMatchItem);
    }
    // SOM-822 - RKL
    public void DeleteItem(DatabaseKey dbKey)
    {
      m_validateClientLookupId = true;
      InvoiceMatchItem_DeleteItem trans = new InvoiceMatchItem_DeleteItem();
      trans.InvoiceMatchItem = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      // The create procedure may have populated an auto-incremented key. 
      UpdateFromDatabaseObject(trans.InvoiceMatchItem);
    }

        
        #endregion
    }
}
