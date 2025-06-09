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
* 2015-Oct-21  SOM-822   Roger Lawton      Added InvoiceMatchItem_DeleteItem Class
**************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    public class InvoiceMatchItem_RetrieveByPKForeignKeys : InvoiceMatchItem_TransactionBaseClass
    {
        public List<b_InvoiceMatchItem> InvoiceMatchItemList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (InvoiceMatchItem.InvoiceMatchItemId > 0)
            //{
            //    string message = "InvoiceMatchItemId has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_InvoiceMatchItem> tmpList = null;
            InvoiceMatchItem.RetrieveByPKForeignKeysFromDatabaseList(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref tmpList);
            InvoiceMatchItemList = new List<b_InvoiceMatchItem>();
            foreach (b_InvoiceMatchItem tmpObj in tmpList)
            {
                InvoiceMatchItemList.Add(tmpObj);
            }
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class InvoiceMatchItemValidate : InvoiceMatchItem_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (ProcedureMaster.ProcedureMasterId > 0)
            //{
            //    string message = "ProcedureMaster has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            InvoiceMatchItem.Validate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    public class InvoiceMatchItem_RetrieveByPrimaryKey : InvoiceMatchItem_TransactionBaseClass
    {
        public List<b_InvoiceMatchItem> InvoiceMatchItemList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (InvoiceMatchItem.InvoiceMatchItemId <= 0)
            {
                string message = "InvoiceMatchItemId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_InvoiceMatchItem> tmpList = null;
            InvoiceMatchItem.RetrieveByPrimaryKeyFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

            InvoiceMatchItemList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class InvoiceMatchItem_DeleteItem : InvoiceMatchItem_TransactionBaseClass
    {

      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
        if (InvoiceMatchItem.InvoiceMatchItemId <= 0)
        {
          string message = "InvoiceMatchItem has an invalid ID.";
          throw new Exception(message);
        }
      }
      public override void PerformWorkItem()
      {
        InvoiceMatchItem.DeleteItem(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }

      public override void Postprocess()
      {
        base.Postprocess();
        System.Diagnostics.Debug.Assert(InvoiceMatchItem.InvoiceMatchItemId > 0);
      }
    }
  }