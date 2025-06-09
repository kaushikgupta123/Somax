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
    public class PartMasterImport_ProcessInterface : PartMasterImport_TransactionBaseClass
    {

        public PartMasterImport_ProcessInterface()
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
                PartMasterImport.PartMasterImport_ProcessInterface(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName);

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

    public class PartMasterClientLookUpID_Retrieve : PartMasterImport_TransactionBaseClass
    {
        public string _ClientLookupID { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
          
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PartMasterImport.RetrieveByClientLookUpIDFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName,_ClientLookupID);
        }
    }
}
