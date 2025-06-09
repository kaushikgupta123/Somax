/*
 **************************************************************************************************
 * PROPRIETARY DATA 
 **************************************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 **************************************************************************************************
 * Copyright (c) 2015 by SOMAX Inc.
 * All rights reserved. 
 **************************************************************************************************
 * Date        Task ID   Person             Description
 * =========== ======== =================== =======================================================
 * 2015-Oct-29 SOM-838  Roger Lawton        Added the Update using foreign keys
 **************************************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using  Database;
using  Common.Enumerations;
using  Database.Business;
using  Database.StoredProcedure;

namespace  Database
{
    public class InvoiceMatchHeaderValidationByClientLookUpId : InvoiceMatchHeader_TransactionBaseClass
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
            InvoiceMatchHeader.ValidateByClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    public class ProcedureInvoiceMatchCreateValidationTransaction : InvoiceMatchHeader_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (InvoiceMatchHeader.InvoiceMatchHeaderId > 0)
            {
                string message = "InvoiceMatchHeaderId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            InvoiceMatchHeader.ValidateByClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    public class InvoiceMatchHeader_ChangeClientLookupId : InvoiceMatchHeader_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (InvoiceMatchHeader.InvoiceMatchHeaderId == 0)
            {
                string message = "InvoiceMatchHeader has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            InvoiceMatchHeader.ChangeClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }


    public class InvoiceMatchHeader_RetrieveAllForSearch : InvoiceMatchHeader_TransactionBaseClass
    {
        public List<b_InvoiceMatchHeader> InvoiceMatchHeaderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (InvoiceMatchHeader.InvoiceMatchHeaderId > 0)
            {
                string message = "InvoiceMatchHeaderId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_InvoiceMatchHeader> tmpList = null;
            InvoiceMatchHeader.RetrieveAllForSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            InvoiceMatchHeaderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class InvoiceMatchHeader_RetrieveChunkSearch : InvoiceMatchHeader_TransactionBaseClass
    {
        public List<b_InvoiceMatchHeader> InvoiceMatchHeaderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (InvoiceMatchHeader.InvoiceMatchHeaderId > 0)
            {
                string message = "InvoiceMatchHeaderId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_InvoiceMatchHeader> tmpList = null;
            InvoiceMatchHeader.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            InvoiceMatchHeaderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    #region  V2-1061
    public class InvoiceMatchHeader_ValidateVarianceCheckTransaction : InvoiceMatchHeader_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            InvoiceMatchHeader.ValidateVarianceCheck(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
    public class InvoiceMatchHeader_RetrieveByPKForeignKeys : InvoiceMatchHeader_TransactionBaseClass
    {
        public List<b_InvoiceMatchHeader> InvoiceMatchHeaderList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (InvoiceMatchHeader.InvoiceMatchHeaderId > 0)
            //{
            //    string message = "InvoiceMatchItem has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_InvoiceMatchHeader> tmpList = null;
            InvoiceMatchHeader.RetrieveByPKForeignKeysFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            InvoiceMatchHeaderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    // SOM-838
    public class InvoiceMatchHeader_UpdateByForeignKeys : InvoiceMatchHeader_TransactionBaseClass
    {
      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
        if (InvoiceMatchHeader.InvoiceMatchHeaderId == 0)
        {
          string message = "Invoice Header has an invalid ID.";
          throw new Exception(message);
        }
      }

      public override void PerformWorkItem()
      {
        InvoiceMatchHeader.UpdateByForeignKeys(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
      }
    }

    public class InvoiceMatchHeader_DeleteInvoiceMatchHeaderAndInvoiceMatchItemsId : InvoiceMatchHeader_TransactionBaseClass
    {
        public InvoiceMatchHeader_DeleteInvoiceMatchHeaderAndInvoiceMatchItemsId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets

        public override void PerformWorkItem()
        {
            InvoiceMatchHeader.DeleteInvoiceMatchHeaderAndInvoiceMatchItemsId
                (this.Connection,
                this.Transaction,
                this.CallerUserInfoId,
                this.CallerUserName);
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

 
}