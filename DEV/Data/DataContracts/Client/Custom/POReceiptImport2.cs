/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2017 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================

***************************************************************************************************
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
using Common.Structures;


namespace DataContracts
{
    public partial class POReceiptImport2 : DataContractBase, IStoredProcedureValidation
    {
        #region Properties 
        public Int64 PersonnelId { get; set; }
        public Int64 PurchaseOrderLineItemId { get; set; }
        public bool SendAlert { get; set; }
        string ValidateFor = string.Empty;
        #endregion

        #region Transactions

        public void ProcessInterfacePOReceiptImport(DatabaseKey dbKey)
        {
            Validate<POReceiptImport2>(dbKey);

            if (IsValid)
            {
                POReceiptImport2_ProcessInterfacePOReceiptImport trans = new POReceiptImport2_ProcessInterfacePOReceiptImport()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.POReceiptImport2= this.ToDatabaseObject();
                trans.POReceiptImport2.PersonnelId = this.PersonnelId;
                trans.POReceiptImport2.PurchaseOrderLineItemId = this.PurchaseOrderLineItemId;
                trans.POReceiptImport2.SendAlert = this.SendAlert;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute(); 
                UpdateFromDatabaseObject(trans.POReceiptImport2);
                this.PersonnelId = trans.POReceiptImport2.PersonnelId;
                this.PurchaseOrderLineItemId = trans.POReceiptImport2.PurchaseOrderLineItemId;
                this.SendAlert = trans.POReceiptImport2.SendAlert;
            }
           


        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            POReceiptImport2_Validate ptrans = new POReceiptImport2_Validate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                ptrans.POReceiptImport2 = this.ToDatabaseObject();
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
          
            return errors;
        }
        public void ValidatePOReceiptImport(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateByPOImport2Id";
            Validate<POReceiptImport2>(dbKey);

        }
        public List<POReceiptImport2> POReceiptImportRetrieveAll(DatabaseKey dbKey)
        {
            POReceiptImport2_RetrieveAll trans = new POReceiptImport2_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.POReceiptImport2List);
        }
        public static List<POReceiptImport2> UpdateFromDatabaseObjectList(List<b_POReceiptImport2> dbObjs)
        {
            List<POReceiptImport2> result = new List<POReceiptImport2>();

            foreach (b_POReceiptImport2 dbObj in dbObjs)
            {
                POReceiptImport2 tmp = new POReceiptImport2();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void RetrieveExReceiptId(DatabaseKey dbKey)
        {
            POReceiptImport2ExRecPID_Retrieve trans = new POReceiptImport2ExRecPID_Retrieve();
            trans.POReceiptImport2 = ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.POReceiptImport2);
        }
        #endregion
    }
}
