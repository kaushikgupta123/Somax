using Common.Enumerations;
using Common.Structures;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class ReceiptImpLine_CustomBase : ReceiptImpLine_TransactionBaseClass
    {
        public Int64 ReceiptClientId { get; set; }
    }


    public class ReceiptImpLine_ValidateImport : ReceiptImpLine_CustomBase
    {
        public override void PerformLocalValidation()
        {

            this.ReceiptClientId = this.ReceiptImpLine.ClientId;
            base.PerformLocalValidation();
            this.ReceiptImpLine.ClientId = ReceiptClientId;

        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            ReceiptImpLine.ReceiptImpLineValidateImport(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }
        public override void Preprocess()
        {
            /*throw new NotImplementedException()*/
            ;
        }
    }


    public class ReceiptImpLine_ProcessImport : ReceiptImpLine_CustomBase
    {
        public override void PerformLocalValidation()
        {
            this.ReceiptClientId = this.ReceiptImpLine.ClientId;
            base.PerformLocalValidation();
            this.ReceiptImpLine.ClientId = ReceiptClientId;
            if (ReceiptImpLine.EXPOLineID == 0)
            {
                string message = "EXPOLineID has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            ReceiptImpLine.ReceiptImpLineProcessImport(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(ReceiptImpLine.ReceiptImpLineId > 0);
        }
    }


    public class ReceiptImpLine_RetrievebyPOLineIdFromDatabase : ReceiptImpLine_CustomBase
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            this.ReceiptClientId = this.ReceiptImpLine.ClientId;
            base.PerformLocalValidation();
            this.ReceiptImpLine.ClientId = ReceiptClientId;
            if (ReceiptImpLine.EXPOLineID == 0)
            {
                string message = "EXPOLineID has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            ReceiptImpLine.RetrievebyPOLineIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }



    //public class ReceiptImpLine_RetrievebyReceiptIdFromDatabase : ReceiptImpLine_CustomBase
    //{

    //    public override void PerformLocalValidation()
    //    {
    //        base.UseTransaction = false;    // moved from PerformWorkItem
    //        this.ReceiptClientId = this.ReceiptImpLine.ClientId;
    //        base.PerformLocalValidation();
    //        this.ReceiptImpLine.ClientId = ReceiptClientId;
    //        if (ReceiptImpLine.EXReceiptId == 0)
    //        {
    //            string message = "EXReceiptId has an invalid ID.";
    //            throw new Exception(message);
    //        }
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
    //        ReceiptImpLine.RetrievebyReceiptIdIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    //    }
    //}



    public class ReceiptImpLine_RetrievebyEXReceiptTxnIdFromDatabase : ReceiptImpLine_CustomBase
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            this.ReceiptClientId = this.ReceiptImpLine.ClientId;
            base.PerformLocalValidation();
            this.ReceiptImpLine.ClientId = ReceiptClientId;
            if (ReceiptImpLine.EXReceiptTxnId == 0)
            {
                string message = "EXReceiptTxnId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            ReceiptImpLine.RetrievebyEXReceiptTxnIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


    public class ReceiptImpLine_CreateCustom : ReceiptImpLine_CustomBase
    {

        public override void PerformLocalValidation()
        {
            this.ReceiptClientId = this.ReceiptImpLine.ClientId;
            base.PerformLocalValidation();
            this.ReceiptImpLine.ClientId = ReceiptClientId;
            if (ReceiptImpLine.ReceiptImpLineId > 0)
            {
                string message = "ReceiptImpLine has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            ReceiptImpLine.InsertIntoDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(ReceiptImpLine.ReceiptImpLineId > 0);
        }
    }


    public class ReceiptImpLine_UpdateCustom : ReceiptImpLine_CustomBase
    {

        public override void PerformLocalValidation()
        {
            this.ReceiptClientId = this.ReceiptImpLine.ClientId;
            base.PerformLocalValidation();
            this.ReceiptImpLine.ClientId = ReceiptClientId;
            if (ReceiptImpLine.ReceiptImpLineId == 0)
            {
                string message = "ReceiptImpLine has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            ReceiptImpLine.UpdateInDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

}


