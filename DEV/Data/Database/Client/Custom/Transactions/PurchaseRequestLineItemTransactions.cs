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
* 2015-Mar-18 SOM-608  Roger Lawton       Added Class PurchaseRequestLineItem_CreateWithReplication
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
    public class PurchaseRequestLineItem_Transaction : AbstractTransactionManager
    {
        public PurchaseRequestLineItem_Transaction()
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
            if (PurchaseRequestLineItem == null)
            {
                string message = "PurchaseRequestLineItem has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;


            // Explicitly set tenant id from dbkey
            this.PurchaseRequestLineItem.ClientId = this.dbKey.Client.ClientId;

        }

        public b_PurchaseRequestLineItem PurchaseRequestLineItem { get; set; }

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

    public class PurchaseRequestLineItem_CreateWithReplication : PurchaseRequestLineItem_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseRequestLineItem.PurchaseRequestLineItemId > 0)
            {
                string message = "PurchaseRequestLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PurchaseRequestLineItem.CreateWithReplication(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PurchaseRequestLineItem.PurchaseRequestLineItemId > 0);
        }
    }

    public class PurchaseRequestLineItem_CreateFromShoppingCart : PurchaseRequestLineItem_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseRequestLineItem.PurchaseRequestLineItemId > 0)
            {
                string message = "PurchaseRequestLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PurchaseRequestLineItem.CreateFromShoppingCart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PurchaseRequestLineItem.PurchaseRequestLineItemId > 0);
        }
    }

    public class PurchaseRequestLineItem_CreateFromPunchOutShoppingCart : PurchaseRequestLineItem_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseRequestLineItem.PurchaseRequestLineItemId > 0)
            {
                string message = "PurchaseRequestLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PurchaseRequestLineItem.CreateFromPunchOutShoppingCart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            // System.Diagnostics.Debug.Assert(PurchaseRequestLineItem.PurchaseRequestLineItemId > 0);
        }
    }
    public class PurchaseRequestLineItem_RetrieveByPurchaseRequestId : PurchaseRequestLineItem_TransactionBaseClass
    {
        public List<b_PurchaseRequestLineItem> PurchaseRequestLineItemList { get; set; }

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
            List<b_PurchaseRequestLineItem> tmpArray = null;

            PurchaseRequestLineItem.PurchaseRequestLineItem_RetrieveByPurchaseRequestId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PurchaseRequestLineItemList = new List<b_PurchaseRequestLineItem>();
            foreach (b_PurchaseRequestLineItem tmpObj in tmpArray)
            {
                PurchaseRequestLineItemList.Add(tmpObj);
            }
        }
    }



    public class PurchaseRequestLineItem_RetrieveByPurchaseRequestLineItemId : PurchaseRequestLineItem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseRequestLineItem.PurchaseRequestLineItemId == 0)
            {
                string message = "PurchaseRequestLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PurchaseRequestLineItem.PurchaseRequestLineItem_RetrieveByPurchaseRequestLineItemId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


    public class PurchaseRequestLineItem_RetrieveByPurchaseRequestLineItemId_V2 : PurchaseRequestLineItem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseRequestLineItem.PurchaseRequestLineItemId == 0)
            {
                string message = "PurchaseRequestLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PurchaseRequestLineItem.PurchaseRequestLineItem_RetrieveByPurchaseRequestLineItemIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


    public class PurchaseRequestLineItem_ValidationTransaction : PurchaseRequestLineItem_TransactionBaseClass
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
            PurchaseRequestLineItem.PurchaseRequestLineItem_Validation(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


    //public class PurchaseOrderLineItem_ValidateByClientLookupId : PurchaseOrderLineItem_TransactionBaseClass
    //{
    //    public PurchaseOrderLineItem_ValidateByClientLookupId()
    //    {
    //    }

    //    public override void PerformLocalValidation()
    //    {
    //        base.PerformLocalValidation();

    //    }

    //    // Result Sets
    //    public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

    //    public override void PerformWorkItem()
    //    {
    //        SqlCommand command = null;
    //        string message = String.Empty;

    //        try
    //        {
    //            List<b_StoredProcValidationError> errors = null;

    //            PurchaseOrderLineItem.PurchaseOrderLineItem_Validation(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

    //            StoredProcValidationErrorList = errors;
    //        }
    //        finally
    //        {
    //            if (null != command)
    //            {
    //                command.Dispose();
    //                command = null;
    //            }

    //            message = String.Empty;
    //        }
    //    }

    //    public override void Preprocess()
    //    {
    //        // throw new NotImplementedException();
    //    }

    //    public override void Postprocess()
    //    {
    //        // throw new NotImplementedException();
    //    }
    //}

    public class PurchaseRequestLineItem_ReOrderLineNumber : PurchaseRequestLineItem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            PurchaseRequestLineItem.ReOrderPRLineNumber(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            //base.Postprocess();
            // System.Diagnostics.Debug.Assert(PurchaseRequestLineItem.PurchaseOrderLineItemId > 0);
        }
    }
    //SOM - 867

    public class PurchaseRequestLineItem_RetrieveByPurchaseOrderLineItem : PurchaseRequestLineItem_TransactionBaseClass
    {
        public List<b_PurchaseRequestLineItem> PRLineItemList { get; set; }

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
            List<b_PurchaseRequestLineItem> tmpArray = null;

            PurchaseRequestLineItem.RetrieveByPurchaseOrderLineItemFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PRLineItemList = new List<b_PurchaseRequestLineItem>();
            foreach (b_PurchaseRequestLineItem tmpObj in tmpArray)
            {
                PRLineItemList.Add(tmpObj);
            }
        }
    }

    #region V2-563
    public class PurchaseRequestLineItem_CreateFromAdditionalCatalogItem : PurchaseRequestLineItem_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseRequestLineItem.PurchaseRequestLineItemId > 0)
            {
                string message = "PurchaseRequestLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PurchaseRequestLineItem.CreateFromAdditionalCatalogItem(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PurchaseRequestLineItem.PurchaseRequestLineItemId > 0);
        }
    }
    #endregion

    #region V2-693 SOMAX to SAP Purchase request export
    public class PurchaseRequestLineItem_RetrievePRLineItemByIdForExportSAP : PurchaseRequestLineItem_TransactionBaseClass
    {
        public List<b_PurchaseRequestLineItem> PurchaseRequestLineItemList { get; set; }

        public override void PerformWorkItem()
        {
            List<b_PurchaseRequestLineItem> tmpArray = null;

            PurchaseRequestLineItem.RetrievePRLineItemByIdForExportSAP(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PurchaseRequestLineItemList = new List<b_PurchaseRequestLineItem>();
            foreach (b_PurchaseRequestLineItem tmpObj in tmpArray)
            {
                PurchaseRequestLineItemList.Add(tmpObj);
            }
        }
    }
    #endregion

    #region V2-738
    public class PurchaseRequestLineItem_CreateFromShoppingCartForMultiStoreroom : PurchaseRequestLineItem_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PurchaseRequestLineItem.PurchaseRequestLineItemId > 0)
            {
                string message = "PurchaseRequestLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PurchaseRequestLineItem.CreateFromShoppingCartForMultiStoreroom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PurchaseRequestLineItem.PurchaseRequestLineItemId > 0);
        }
    }
    #endregion
    #region V2-894
    public class PurchaseRequestLineItem_RetrievelookuplistByPartId_V2 : PurchaseRequestLineItem_TransactionBaseClass
    {
        public List<b_PurchaseRequestLineItem> PurchaseRequestLineItemList { get; set; }

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
            List<b_PurchaseRequestLineItem> tmpArray = null;

            PurchaseRequestLineItem.PurchaseRequestLineItem_RetrieveLookupListByPartId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PurchaseRequestLineItemList = new List<b_PurchaseRequestLineItem>();
            foreach (b_PurchaseRequestLineItem tmpObj in tmpArray)
            {
                PurchaseRequestLineItemList.Add(tmpObj);
            }
        }
      
    }
    #endregion

    #region V2-1046
    public class PurchaseRequestLineItem_RetrieveForConsolidate : PurchaseRequestLineItem_TransactionBaseClass
    {
        public List<b_PurchaseRequestLineItem> PurchaseRequestLineItemList { get; set; }

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
            List<b_PurchaseRequestLineItem> tmpArray = null;

            PurchaseRequestLineItem.PurchaseRequestLineItem_RetrieveForConsolidate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PurchaseRequestLineItemList = new List<b_PurchaseRequestLineItem>();
            foreach (b_PurchaseRequestLineItem tmpObj in tmpArray)
            {
                PurchaseRequestLineItemList.Add(tmpObj);
            }
        }
    }

    public class PurchaseRequestLineItem_PRConsolidate : PurchaseRequestLineItem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            PurchaseRequestLineItem.PRConsolidate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            
        }
    }
    #endregion

    #region V2-1063
    public class PurchaseRequestLineItem_RetrieveForMaterialRequest : PurchaseRequestLineItem_TransactionBaseClass
    {
        public List<b_PurchaseRequestLineItem> PurchaseRequestLineItemList { get; set; }

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
            List<b_PurchaseRequestLineItem> tmpArray = null;

            PurchaseRequestLineItem.LineItem_RetrieveForMaterialRequest(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PurchaseRequestLineItemList = new List<b_PurchaseRequestLineItem>();
            foreach (b_PurchaseRequestLineItem tmpObj in tmpArray)
            {
                PurchaseRequestLineItemList.Add(tmpObj);
            }
        }
    }

    #endregion
}
