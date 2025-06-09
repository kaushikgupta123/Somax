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

    public class VendorMasterImport_CustomBase : VendorMasterImport_TransactionBaseClass
    {
        public Int64 VendorMasterImportClientId { get; set; }
    }
    public class VendorMasterImport_Validation : VendorMasterImport_CustomBase
    {


        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            VendorMasterImport.VendorMasterImportValidate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
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


    public class VendorMasterImport_ProcessImport : VendorMasterImport_CustomBase
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (VendorMasterImport.VendorMasterImportId == 0)
            {
                string message = "VendorMasterImport has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            VendorMasterImport.VendorMasterImportProcess(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(VendorMasterImport.VendorMasterImportId > 0);
        }
    }

    //public class VendorMasterImport_RetrieveByVendorNumber : VendorMasterImport_CustomBase
    //{
    //    public override void PerformLocalValidation()
    //    {
    //        base.UseTransaction = false;    // moved from PerformWorkItem
    //        this.VendorMasterImportClientId = this.VendorMasterImport.ClientId;
    //        base.PerformLocalValidation();
    //        this.VendorMasterImport.ClientId = VendorMasterImportClientId;
    //        if (VendorMasterImport.VendorNumber == "")
    //        {
    //            string message = "VendorNumber has an invalid ID.";
    //            throw new Exception(message);
    //        }
    //    }
    //    public override void PerformWorkItem()
    //    {
    //        //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
    //        VendorMasterImport.VendorImportRetrieveByVendorNumber(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    //    }

    //}

    public class VendorMasterImport_RetrieveByExVendorId : VendorMasterImport_CustomBase
    {
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            this.VendorMasterImportClientId = this.VendorMasterImport.ClientId;
            base.PerformLocalValidation();
            this.VendorMasterImport.ClientId = VendorMasterImportClientId;
            if (VendorMasterImport.VendorNumber == "")
            {
                string message = "ExVendorId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            VendorMasterImport.VendorImportRetrieveByExVendorId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

    }

    public class VendorMasterImport_CreateCustom : VendorMasterImport_CustomBase
    {

        public override void PerformLocalValidation()
        {
            this.VendorMasterImportClientId = this.VendorMasterImport.ClientId;
            base.PerformLocalValidation();
            this.VendorMasterImport.ClientId = VendorMasterImportClientId;
            if (VendorMasterImport.VendorMasterImportId > 0)
            {
                string message = "VendorMasterImport has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            VendorMasterImport.InsertIntoDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(VendorMasterImport.VendorMasterImportId > 0);
        }
    }

    public class VendorMasterImport_UpdateCustom : VendorMasterImport_CustomBase
    {

        public override void PerformLocalValidation()
        {
            this.VendorMasterImportClientId = this.VendorMasterImport.ClientId;
            base.PerformLocalValidation();
            this.VendorMasterImport.ClientId = VendorMasterImportClientId;
            if (VendorMasterImport.VendorMasterImportId == 0)
            {
                string message = "VendorMasterImport has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            VendorMasterImport.UpdateInDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            // RKL - No need for change log 
            //if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class VendorMasterImport_ProcessInterface : VendorMasterImport_TransactionBaseClass
    {

        public VendorMasterImport_ProcessInterface()
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
                VendorMasterImport.vendorMasterImport_ProcessInterface(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName);

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

    public class RetrieveByClientIdVendorExIdVendorExSiteId : VendorMasterImport_TransactionBaseClass
    {
        public string _ClientLookupID { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            VendorMasterImport.RetrieveByClientIdVendorExIdVendorExSiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, _ClientLookupID);
        }
    }
}


