using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;
using Data.Database;

namespace Database
{
    public class VendorCatalogImport_ProcessInterface : VendorCatalogImport_TransactionBaseClass
    {

        public VendorCatalogImport_ProcessInterface()
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
                VendorCatalogImport.vendorCatalogImport_ProcessInterface(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName);

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

    public class VendorCatalogImport_RetrieveForImportCheck : VendorCatalogImport_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();          
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            VendorCatalogImport.RetrieveForImportCheck(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
}
