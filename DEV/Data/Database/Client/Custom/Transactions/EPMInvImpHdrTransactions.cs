using Database.Business;
using System.Collections.Generic;
namespace Database
{
    public class EPMInvImpHdr_ValidateImport : EPMInvImpHdr_TransactionBaseClass
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
            EPMInvImpHdr.EPMInvImpHdr_ValidateImport(this.Connection, this.Transaction, this.CallerUserInfoId, CallerUserName, ref errors);
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

}
