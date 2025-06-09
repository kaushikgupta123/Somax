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
    public class EquipmentMaster_ValidateByName : EquipmentMaster_TransactionBaseClass
    {
        public EquipmentMaster_ValidateByName()
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
                EquipmentMaster.ValidateByName(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    public class EquipmentMaster_RetrieveCreateModifyDate : EquipmentMaster_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    
            base.PerformLocalValidation();
            if (EquipmentMaster.EquipmentMasterId == 0)
            {
                string message = "EquipmentMaster has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            EquipmentMaster.RetrieveCreateModifyDate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

}
