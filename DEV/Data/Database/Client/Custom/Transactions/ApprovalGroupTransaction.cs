using Database.Business;

using System;

namespace Database
{

    public class ApprovalGroup_RetrieveChunkSearchV2 : ApprovalGroup_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (ApprovalGroup.ApprovalGroupId > 0)
            //{
            //    string message = "ApprovalId has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            b_ApprovalGroup tmpList = null;
            ApprovalGroup.RetrieveApprovalGroupChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class ApprovalGroup_RetrieveById_V2 : ApprovalGroup_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ApprovalGroup.ApprovalGroupId == 0)
            {
                string message = "ApprovalGroup has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            ApprovalGroup.RetrieveByIdFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

}
