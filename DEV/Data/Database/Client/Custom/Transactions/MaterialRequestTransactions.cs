using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Database.Business;
using System.Data.SqlClient;

namespace Database.Client.Custom.Transactions
{
    public class MaterialRequest_RetrieveChunkSearch : MaterialRequest_TransactionBaseClass
    {

        public List<b_MaterialRequest> MaterialRequestList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (MaterialRequest.MaterialRequestId < 0)
            {
                string message = "MaterialRequest has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_MaterialRequest> tmpList = null;
            MaterialRequest.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            MaterialRequestList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class MaterialRequest_RetrieveByMaterialRequestId : MaterialRequest_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (MaterialRequest.MaterialRequestId == 0)
            {
                string message = "MaterialRequest has an invalid ID.";
                throw new Exception(message);
            }
            base.UseTransaction = false;
        }

        public override void PerformWorkItem()
        {
            // This is too late to set this - the transaction has already been created
            //base.UseTransaction = false;
            MaterialRequest.MaterialRequestRetrieveByMaterialRequestId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


}
