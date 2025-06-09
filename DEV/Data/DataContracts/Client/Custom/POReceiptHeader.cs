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
 * Date        Log Entry Developer          Description
 * =========== ========= ================== ===================================
 * 2012-Mar-30           Roger Lawton       Created
 ******************************************************************************
 */



using Database.Business;
using Database.Client.Custom.Transactions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace DataContracts
{
    public partial class POReceiptHeader : DataContractBase, IStoredProcedureValidation
  {

    #region Properties
    [DataMember]
    public long PurchaseOrderLineItemId { get; set; }  // not sure where this is used
    public string ReceiveBy_PersonnelName { get; set; }
    public decimal QuantityReceived { get; set; }
    public bool Reversed { get; set; }
    public string ReversedComments { get; set; }
    // RKL - 2017-09-29
    public long ExReceiptTxnId {get;set;}
    public long POReceiptItemId { get; set; }
    string ValidateFor = string.Empty;
    public decimal UOMConversion { get; set; }
     public long POReceiptImportId { get; set; }

     public string ChargeType { get; set; }
     public long ChargeToId { get; set; }
        #endregion

        public static List<POReceiptHeader> UpdateFromDatabaseObjectList(List<b_POReceiptHeader> dbObjs)
    {
        List<POReceiptHeader> result = new List<POReceiptHeader>();

      foreach (b_POReceiptHeader dbObj in dbObjs)
      {
          POReceiptHeader tmp = new POReceiptHeader();
        tmp.UpdateFromDatabaseObjectExtended(dbObj);
        result.Add(tmp);
      }
      return result;
    }
    public void UpdateFromDatabaseObjectExtended(b_POReceiptHeader dbObj)
    {
        this.UpdateFromDatabaseObject(dbObj);
        this.ReceiveBy_PersonnelName = dbObj.ReceiveBy_PersonnelName;
        this.POReceiptItemId = dbObj.POReceiptItemId;
        this.QuantityReceived = dbObj.QuantityReceived;
        this.Reversed = dbObj.Reversed;
        this.ReversedComments = dbObj.ReversedComments;
        this.ExReceiptNo = dbObj.ExReceiptNo;
        this.ExReceiptTxnId = dbObj.ExReceiptTxnId;
        this.UOMConversion = dbObj.UOMConversion;
        this.ChargeType = dbObj.ChargeType;
        this.ChargeToId = dbObj.ChargeToId;
            //this.AccountId_ClientLookupId = dbObj.AccountId_ClientLookupId;
            //this.ATSource_StoreroomName = dbObj.ATSource_StoreroomName;
        }
    public List<POReceiptHeader> RetrieveByPurchaseOrderLineItemId(DatabaseKey dbKey)
    {
        POReceiptHeader_RetrieveByPurchaseOrderLineItem trans = new POReceiptHeader_RetrieveByPurchaseOrderLineItem()
        {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName,
        };
        trans.POReceiptHeader = new b_POReceiptHeader
        {
            PurchaseOrderLineItemId = this.PurchaseOrderLineItemId 
        };
        trans.dbKey = dbKey.ToTransDbKey();
        trans.Execute();
        return UpdateFromDatabaseObjectList(trans.POReceiptHeaderList);
    }

    public List<POReceiptHeader> RetrieveByPurchaseOrderId(DatabaseKey dbKey)
    {
        POReceiptHeader_RetrieveByPurchaseOrderId trans = new POReceiptHeader_RetrieveByPurchaseOrderId()
        {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName,
        };
        trans.POReceiptHeader = new b_POReceiptHeader
        {
            PurchaseOrderId = this.PurchaseOrderId
        };
        trans.dbKey = dbKey.ToTransDbKey();
        trans.Execute();
        return UpdateFromDatabaseObjectList(trans.POReceiptHeaderList);
    }
//----------Call from APi SOM-938-----------------------------------------------------------
    public void CreatePOWithValidation(DatabaseKey dbKey)
    {
        ValidateFor = "ValidatePOAndUOM";
        Validate<POReceiptHeader>(dbKey);
        if (IsValid)
        {
            POReceiptHeader_CreateByPK trans = new POReceiptHeader_CreateByPK();
            trans.POReceiptHeader = this.ToDatabaseObject();
            trans.POReceiptHeader.POReceiptImportId = this.POReceiptImportId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObjectExtend(trans.POReceiptHeader);
        }
    }
    public void UpdateFromDatabaseObjectExtend(b_POReceiptHeader dbObj)
    {
        this.ClientId = dbObj.ClientId;
        this.POReceiptItemId = dbObj.POReceiptItemId;
        this.POReceiptHeaderId = dbObj.POReceiptHeaderId;
        this.PurchaseOrderLineItemId = dbObj.PurchaseOrderLineItemId;
        this.QuantityReceived = dbObj.QuantityReceived;
        this.Reversed = dbObj.Reversed;
        this.PurchaseOrderId = dbObj.PurchaseOrderId;
        this.UpdateIndex = dbObj.UpdateIndex;
        this.PurchaseOrderId = dbObj.PurchaseOrderId;

    }
    public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
    {
        List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

        if (ValidateFor == "ValidatePOAndUOM")
        {
            POReceiptHeader_ValidatePOAndUOMTransaction ptrans = new POReceiptHeader_ValidatePOAndUOMTransaction()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
       
            };

            ptrans.POReceiptHeader = this.ToDatabaseObject();
            ptrans.POReceiptHeader.POReceiptImportId = this.POReceiptImportId;
            ptrans.dbKey = dbKey.ToTransDbKey();
            ptrans.Execute();
            if (ptrans.StoredProcValidationErrorList != null)
            {
                foreach (b_StoredProcValidationError error in ptrans.StoredProcValidationErrorList)
                {
                    StoredProcValidationError tmp = new StoredProcValidationError();
                    tmp.UpdateFromDatabaseObject(error);
                    errors.Add(tmp);
                }
            }
        }
        

        return errors;
    }
    
  }
}
