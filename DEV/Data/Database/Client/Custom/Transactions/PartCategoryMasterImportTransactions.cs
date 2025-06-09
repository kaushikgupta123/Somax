using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class PartCategoryMasterImport_ProcessImport : PartCategoryMasterImport_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartCategoryMasterImport.PartCategoryMasterImportId == 0)
            {
                string message = "PartCategoryMasterImport has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PartCategoryMasterImport.PartCategoryMasterImportProcess(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PartCategoryMasterImport.PartCategoryMasterImportId > 0);
        }
    }
    public class PartCategoryMasterImport_RetrieveByClientLookUpID : PartCategoryMasterImport_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PartCategoryMasterImport.RetrieveByClientLookUpIDFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
}
