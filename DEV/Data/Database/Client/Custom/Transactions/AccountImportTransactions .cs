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

    public class AccountImport_CustomBase : AccountImport_TransactionBaseClass
    {
        public Int64 AccountImportClientId { get; set; }
    }
    public class AccountImport_Validation : AccountImport_CustomBase
    {


        public override void PerformLocalValidation()
        {
            this.AccountImportClientId = this.AccountImport.ClientId;
            base.PerformLocalValidation();
            this.AccountImport.ClientId = AccountImportClientId;
        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            AccountImport.AccountImportValidation(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
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


    public class AccountImport_ProcessImport : AccountImport_CustomBase
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (AccountImport.AccountImportId == 0)
            {
                string message = "AccountImport has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            AccountImport.AccountImportProcessImport(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(AccountImport.AccountImportId > 0);
        }
    }


    //public class AccountImport_RetrieveByAccountNumber : AccountImport_CustomBase
    //{
    //    public override void PerformLocalValidation()
    //    {
    //        base.UseTransaction = false;    // moved from PerformWorkItem
    //        this.AccountImportClientId = this.AccountImport.ClientId;
    //        base.PerformLocalValidation();
    //        this.AccountImport.ClientId = AccountImportClientId;
    //        if (AccountImport.AccountNumber == string.Empty)
    //        {
    //            string message = "AccountNumber has an invalid ID.";
    //            throw new Exception(message);
    //        }
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
    //        AccountImport.AccountImportRetrieveByAccountNumber(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    //    }
    //}


    public class AccountImport_RetrieveByExAccountId : AccountImport_CustomBase
    {
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            this.AccountImportClientId = this.AccountImport.ClientId;
            base.PerformLocalValidation();
            this.AccountImport.ClientId = AccountImportClientId;
            if (AccountImport.ExAccountId == 0)
            {
                string message = "ExAccountId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            AccountImport.AccountImportRetrieveByExAccountId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class AccountImport_CreateCustom : AccountImport_CustomBase
    {

        public override void PerformLocalValidation()
        {
            this.AccountImportClientId = this.AccountImport.ClientId;
            base.PerformLocalValidation();
            this.AccountImport.ClientId = AccountImportClientId;
            if (AccountImport.AccountImportId > 0)
            {
                string message = "AccountImport has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            AccountImport.InsertIntoDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(AccountImport.AccountImportId > 0);
        }
    }


    public class AccountImport_UpdateCustom : AccountImport_CustomBase
    {

        public override void PerformLocalValidation()
        {
            this.AccountImportClientId = this.AccountImport.ClientId;
            base.PerformLocalValidation();
            this.AccountImport.ClientId = AccountImportClientId;
            if (AccountImport.AccountImportId == 0)
            {
                string message = "AccountImport has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            AccountImport.UpdateInDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            // RKL -2020-Aug-22 - Do NOT create a change log entry for this record
            //if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }


}


