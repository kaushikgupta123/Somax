/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* PurchaseRequestLineItem.cs (Data Contract)
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2015-Mar-18 SOM-608  Roger Lawton       Added Class PurchaseOrderLineItem_CreateWithReplication
* 2016-Oct-06 SOM-1037  Roger Lawton      Added new txn PurchaseOrderLineItem_RetrieveForAlert
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

namespace Database
{
    public class PurchaseOrderLineItem_Transaction : AbstractTransactionManager
    {
        public PurchaseOrderLineItem_Transaction()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseOrderLineItem == null)
            {
                string message = "PurchaseOrderLineItem has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;


            // Explicitly set tenant id from dbkey
            this.PurchaseOrderLineItem.ClientId = this.dbKey.Client.ClientId;

        }

        public b_PurchaseOrderLineItem PurchaseOrderLineItem { get; set; }

        public override void PerformWorkItem()
        {
            // throw new NotImplementedException();
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }

   

    public class PurchaseOrderLineItem_RetrieveByPurchaseOrderId : PurchaseOrderLineItem_TransactionBaseClass
    {
        public List<b_PurchaseOrderLineItem> PurchaseOrderLineItemList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_PurchaseOrderLineItem> tmpArray = null;

            PurchaseOrderLineItem.PurchaseOrderLineItem_RetrieveByPurchaseOrderId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PurchaseOrderLineItemList = new List<b_PurchaseOrderLineItem>();
            foreach (b_PurchaseOrderLineItem tmpObj in tmpArray)
            {
                PurchaseOrderLineItemList.Add(tmpObj);
            }
        }
    }
    public class PurchaseOrderLineItem_RetrieveByPurchaseOrderId_V2 : PurchaseOrderLineItem_TransactionBaseClass
    {
        public List<b_PurchaseOrderLineItem> PurchaseOrderLineItemList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_PurchaseOrderLineItem> tmpArray = null;

            PurchaseOrderLineItem.PurchaseOrderLineItem_RetrieveByPurchaseOrderId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PurchaseOrderLineItemList = new List<b_PurchaseOrderLineItem>();
            foreach (b_PurchaseOrderLineItem tmpObj in tmpArray)
            {
                PurchaseOrderLineItemList.Add(tmpObj);
            }
        }
    }
    //--SOM-892--//
    public class PurchaseOrderLineItem_FilterByPartId : PurchaseOrderLineItem_TransactionBaseClass
    {
        public List<b_PurchaseOrderLineItem> PurchaseOrderLineItemList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_PurchaseOrderLineItem> tmpArray = null;

            PurchaseOrderLineItem.PurchaseOrderLineItem_FilterByPartId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PurchaseOrderLineItemList = new List<b_PurchaseOrderLineItem>();
            foreach (b_PurchaseOrderLineItem tmpObj in tmpArray)
            {
                PurchaseOrderLineItemList.Add(tmpObj);
            }
        }
    }
    public class PurchaseOrderLineItem_RetrieveForAlert : PurchaseOrderLineItem_TransactionBaseClass
    {
      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
        if (PurchaseOrderLineItem.PurchaseOrderLineItemId == 0)
        {
          string message = "PurchaseOrderLineItemId has an invalid ID.";
          throw new Exception(message);
        }
      }

      public override void PerformWorkItem()
      {
        base.UseTransaction = false;
        PurchaseOrderLineItem.PurchaseOrderLineItem_RetrieveForAlert(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }
    }

    public class PurchaseOrderLineItem_RetrieveForAlert_V2 : PurchaseOrderLineItem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseOrderLineItem.PurchaseOrderLineItemId == 0)
            {
                string message = "PurchaseOrderLineItemId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PurchaseOrderLineItem.PurchaseOrderLineItem_RetrieveForAlert_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class PurchaseOrderLineItem_RetrieveByPurchaseOrderLineItemId : PurchaseOrderLineItem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseOrderLineItem.PurchaseOrderLineItemId == 0)
            {
                string message = "PurchaseOrderLineItemId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PurchaseOrderLineItem.PurchaseOrderLineItem_RetrieveByPurchaseOrderLineItemId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class PurchaseOrderLineItem_RetrieveByPurchaseOrderLineItemId_V2 : PurchaseOrderLineItem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseOrderLineItem.PurchaseOrderLineItemId == 0)
            {
                string message = "PurchaseOrderLineItemId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PurchaseOrderLineItem.PurchaseOrderLineItem_RetrieveByPurchaseOrderLineItemId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class PurchaseOrderLineItem_CreateWithReplication : PurchaseOrderLineItem_TransactionBaseClass
    {

      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
        if (PurchaseOrderLineItem.PurchaseOrderLineItemId > 0)
        {
          string message = "PurchaseOrderLineItem has an invalid ID.";
          throw new Exception(message);
        }
      }
      public override void PerformWorkItem()
      {
        PurchaseOrderLineItem.CreateWithReplication(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }

      public override void Postprocess()
      {
        base.Postprocess();
        System.Diagnostics.Debug.Assert(PurchaseOrderLineItem.PurchaseOrderLineItemId > 0);
      }
    }
    public class PurchaseOrderLineItem_CreateWithReplication_V2 : PurchaseOrderLineItem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseOrderLineItem.PurchaseOrderLineItemId > 0)
            {
                string message = "PurchaseOrderLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PurchaseOrderLineItem.CreateWithReplication_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PurchaseOrderLineItem.PurchaseOrderLineItemId > 0);
        }
    }
    public class PurchaseOrderLineItem_CreateFromShoppingCart : PurchaseOrderLineItem_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseOrderLineItem.PurchaseOrderLineItemId > 0)
            {
                string message = "PurchaseOrderLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PurchaseOrderLineItem.CreateFromShoppingCart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PurchaseOrderLineItem.PurchaseOrderLineItemId > 0);
        }
    }


    public class PurchaseOrderLineItem_ValidationTransaction : PurchaseOrderLineItem_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (PurchaseOrderLineItem.PurchaseOrderLineItemId > 0)
            //{
            //    string message = "PurchaseOrderLineItem has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            PurchaseOrderLineItem.PurchaseOrderLineItem_Validation(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


    public class PurchaseOrderLineItem_ValidateByClientLookupId : PurchaseOrderLineItem_TransactionBaseClass
    {
        public PurchaseOrderLineItem_ValidateByClientLookupId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;

                PurchaseOrderLineItem.PurchaseOrderLineItem_Validation(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

                StoredProcValidationErrorList = errors;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }

    public class PurchaseOrderLineItem_ReOrderLineNumber : PurchaseOrderLineItem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseOrderLineItem.PurchaseOrderLineItemId == 0)
            {
                string message = "PurchaseOrderLineItemId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PurchaseOrderLineItem.ReOrderLineNumber(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PurchaseOrderLineItem.PurchaseOrderLineItemId > 0);
        }
    }
    public class PurchaseOrderLineItem_RetriveByWorkOrderId : PurchaseOrderLineItem_TransactionBaseClass
    {
        public List<b_PurchaseOrderLineItem> PurchaseOrderLineItemList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_PurchaseOrderLineItem> tmpArray = null;


            PurchaseOrderLineItem.RetriveByWorkOrderIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PurchaseOrderLineItemList = new List<b_PurchaseOrderLineItem>();
            foreach (b_PurchaseOrderLineItem tmpObj in tmpArray)
            {
                PurchaseOrderLineItemList.Add(tmpObj);
            }
        }


    }

    #region V2-738
    public class PurchaseOrderLineItem_CreateFromShoppingCartForMultiStoreroom : PurchaseOrderLineItem_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseOrderLineItem.PurchaseOrderLineItemId > 0)
            {
                string message = "PurchaseOrderLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PurchaseOrderLineItem.CreateFromShoppingCartForMultiStoreroom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PurchaseOrderLineItem.PurchaseOrderLineItemId > 0);
        }
    }
    public class PurchaseOrderLineItem_RetrieveByPurchaseOrderIdForMultiStoreroom_V2 : PurchaseOrderLineItem_TransactionBaseClass
    {
        public List<b_PurchaseOrderLineItem> PurchaseOrderLineItemList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_PurchaseOrderLineItem> tmpArray = null;

            PurchaseOrderLineItem.PurchaseOrderLineItem_RetrieveByPurchaseOrderIdForMultiStoreroom_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PurchaseOrderLineItemList = new List<b_PurchaseOrderLineItem>();
            foreach (b_PurchaseOrderLineItem tmpObj in tmpArray)
            {
                PurchaseOrderLineItemList.Add(tmpObj);
            }
        }
    }
    #endregion
    #region V2-1047
    public class PurchaseOrderLineItem_RetrieveByPurchaseOrderIdForDirectLineItems_V2 : PurchaseOrderLineItem_TransactionBaseClass
    {
        public List<b_PurchaseOrderLineItem> PurchaseOrderLineItemList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_PurchaseOrderLineItem> tmpArray = null;

            PurchaseOrderLineItem.PurchaseOrderLineItem_RetrieveByPurchaseOrderIdForDirectLineItem_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PurchaseOrderLineItemList = new List<b_PurchaseOrderLineItem>();
            foreach (b_PurchaseOrderLineItem tmpObj in tmpArray)
            {
                PurchaseOrderLineItemList.Add(tmpObj);
            }
        }
    }
    #endregion
}
