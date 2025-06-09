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
    public class ReceiptImpHdr_CustomBase : ReceiptImpHdr_TransactionBaseClass
    {
        public Int64 ReceiptClientId { get; set; }
    }

    public class ReceiptImpHdr_ValidateImport : ReceiptImpHdr_CustomBase
    {


        public override void PerformLocalValidation()
        {

            this.ReceiptClientId = this.ReceiptImpHdr.ClientId;
            base.PerformLocalValidation();
            this.ReceiptImpHdr.ClientId = ReceiptClientId;

        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            ReceiptImpHdr.ReceiptImpHdrValidateImport(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
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


    public class ReceiptImpHdr_ProcessImport : ReceiptImpHdr_CustomBase
    {
        public override void PerformLocalValidation()
        {
            this.ReceiptClientId = this.ReceiptImpHdr.ClientId;
            base.PerformLocalValidation();
            this.ReceiptImpHdr.ClientId = ReceiptClientId;
            if (ReceiptImpHdr.ReceiptImpHdrId == 0)
            {
                string message = "ReceiptImpHdr has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            ReceiptImpHdr.ReceiptImpHdrProcessImport(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(ReceiptImpHdr.ReceiptImpHdrId > 0);
        }
    }


    //public class ReceiptImpHdr_RetrievebyPOIdFromDatabase : ReceiptImpHdr_CustomBase
    //{

    //    public override void PerformLocalValidation()
    //    {
    //        base.UseTransaction = false;    // moved from PerformWorkItem
    //        this.ReceiptClientId = this.ReceiptImpHdr.ClientId;
    //        base.PerformLocalValidation();
    //        this.ReceiptImpHdr.ClientId = ReceiptClientId;
    //        if (ReceiptImpHdr.ReceiptImpHdrId == 0)
    //        {
    //            string message = "ReceiptImpHdr has an invalid ID.";
    //            throw new Exception(message);
    //        }
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
    //        ReceiptImpHdr.RetrievebyPOIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    //    }
    //}


    public class ReceiptImpHdr_RetrievebyReceiptIdFromDatabase : ReceiptImpHdr_CustomBase
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            this.ReceiptClientId = this.ReceiptImpHdr.ClientId;
            base.PerformLocalValidation();
            this.ReceiptImpHdr.ClientId = ReceiptClientId;
            if (ReceiptImpHdr.EXRecieptId == 0)
            {
                string message = "EXRecieptId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            ReceiptImpHdr.RetrievebyReceiptIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


  public class ReceiptImpHdr_CreateCustom : ReceiptImpHdr_CustomBase
    {

    public override void PerformLocalValidation()
    {
            this.ReceiptClientId = this.ReceiptImpHdr.ClientId;
            base.PerformLocalValidation();
            this.ReceiptImpHdr.ClientId = ReceiptClientId;
            if (ReceiptImpHdr.ReceiptImpHdrId > 0)
      {
        string message = "ReceiptImpHdr has an invalid ID.";
        throw new Exception(message);
      }
    }
    public override void PerformWorkItem()
    {
      ReceiptImpHdr.InsertIntoDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    }

    public override void Postprocess()
    {
      base.Postprocess();
      System.Diagnostics.Debug.Assert(ReceiptImpHdr.ReceiptImpHdrId > 0);
    }
  }


  public class ReceiptImpHdr_UpdateCustom : ReceiptImpHdr_CustomBase
    {

    public override void PerformLocalValidation()
    {
            this.ReceiptClientId = this.ReceiptImpHdr.ClientId;
            base.PerformLocalValidation();
            this.ReceiptImpHdr.ClientId = ReceiptClientId;
            if (ReceiptImpHdr.ReceiptImpHdrId == 0)
      {
        string message = "ReceiptImpHdr has an invalid ID.";
        throw new Exception(message);
      }
    }

    public override void PerformWorkItem()
    {
      ReceiptImpHdr.UpdateInDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      // If no have been made, no change log is created
      if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
    }
  }





}


