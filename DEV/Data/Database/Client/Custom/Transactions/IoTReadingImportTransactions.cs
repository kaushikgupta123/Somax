using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class IoTReadingImport_ValidateImport : IoTReadingImport_TransactionBaseClass
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
            IoTReadingImport.IoTReadingImport_ValidateImport(this.Connection, this.Transaction, this.CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }
        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }
    }
    public class IoTReadingImport_ProcessImport : IoTReadingImport_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (IoTReadingImport.IoTReadingImportId == 0)
            {
                string message = "IoTReadingImport has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            IoTReadingImport.IoTReadingImportProcess(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(IoTReadingImport.IoTReadingImportId > 0);
        }
    }
}
