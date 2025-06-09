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
    public class VendorMasterRetrieveAll_ByInactiveFlag : VendorMaster_TransactionBaseClass
    {
        public List<b_VendorMaster> VendorMasterList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformWorkItem()
        {
            b_VendorMaster[] tmpArray = null;

            // Explicitly set id from dbkey
            VendorMaster.ClientId = this.dbKey.Client.ClientId;


            VendorMaster.VendorMaster_RetrieveAll_ByInactiveFlag(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            VendorMasterList = new List<b_VendorMaster>(tmpArray);
        }

    }
    public class Vendor_CreateVendorFromVM : VendorMaster_TransactionBaseClass
    {
        public Int64 SiteId { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            VendorMaster.InsertVendor(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, SiteId);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(VendorMaster.VendorMasterId > 0);
        }
    }
    public class VendorMaster_RetrieveByFK : VendorMaster_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (VendorMaster.VendorMasterId == 0)
            {
                string message = "VendorMaster has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            VendorMaster.RetrieveByforeignKeyFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class RetrieveVendorFromVM : VendorMaster_TransactionBaseClass
    {
        public Int64 SiteId { get; set; }
        public List<b_VendorMaster> VendorList { get; set; }
        public override void Preprocess()
        {
        }

        public override void Postprocess()
        {
        }

        public override void PerformWorkItem()
        {
            b_VendorMaster[] tmpArray = null;

            // Explicitly set id from dbkey
            VendorMaster.ClientId = this.dbKey.Client.ClientId;


            VendorMaster.RetrieveVendorFromVM(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, SiteId, ref tmpArray);

            VendorList = new List<b_VendorMaster>(tmpArray);
        }

    }
    public class VendorMaster_ValidateByClientlookupId : VendorMaster_TransactionBaseClass
    {

        public VendorMaster_ValidateByClientlookupId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;
                VendorMaster.ValidateByClientLookupId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    public class VendorMaster_ValidateByClientlookupIdForChange : VendorMaster_TransactionBaseClass
    {
        public VendorMaster_ValidateByClientlookupIdForChange()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;
                VendorMaster.ValidateByClientLookupIdForChange(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    public class VendorMaster_ChangeClientLookupId : VendorMaster_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (VendorMaster.VendorMasterId == 0)
            {
                string message = "VendorMaster has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            VendorMaster.ChangeClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class VendorMaster_ValidateClientIdVendorAndMaster : VendorMaster_TransactionBaseClass
    {
        public VendorMaster_ValidateClientIdVendorAndMaster()
        {
        }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;
            try
            {
                List<b_StoredProcValidationError> errors = null;
                VendorMaster.ValidateClientIdVendorAndMaster(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    }
}
