using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Database.Business;

namespace Database
{
    public class StoreroomTransfer_RetrieveChunkSearch : StoreroomTransfer_TransactionBaseClass
    {

        public List<b_StoreroomTransfer> StoreroomTransferList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (StoreroomTransfer.StoreroomTransferId < 0)
            {
                string message = "StoreroomTransfer has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_StoreroomTransfer> tmpList = null;
            StoreroomTransfer.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            StoreroomTransferList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    #region Outgoing Transfer
    public class StoreroomTransfer_RetrieveOutgoingTransferChunkSearch : StoreroomTransfer_TransactionBaseClass
    {

        public List<b_StoreroomTransfer> StoreroomTransferList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (StoreroomTransfer.StoreroomTransferId < 0)
            {
                string message = "StoreroomTransfer has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_StoreroomTransfer> tmpList = null;
            StoreroomTransfer.RetrieveOutgoingTransferChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            StoreroomTransferList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class StoreroomTransfer_ValidateForReceiptProcess : StoreroomTransfer_TransactionBaseClass
    {
        public StoreroomTransfer_ValidateForReceiptProcess()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public System.Data.DataTable lulist { get; set; }


        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {

                List<b_StoredProcValidationError> errors = null;


                StoreroomTransfer.ValidateForReceiptProcess(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, lulist, ref errors);

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
    #endregion

    #region Incoming Transfer

    public class StoreroomTransfer_ValidateForIssueProcess : StoreroomTransfer_TransactionBaseClass
    {
        public StoreroomTransfer_ValidateForIssueProcess()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public System.Data.DataTable lulist { get; set; }


        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {

                List<b_StoredProcValidationError> errors = null;


                StoreroomTransfer.ValidateForIssueProcess(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, lulist, ref errors);

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
    #endregion

    #region V2-1059
    public class StoreroomTransfer_AutoGeneration_V2 : StoreroomTransfer_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public System.Data.DataTable lulist { get; set; }
        public override void PerformWorkItem()
        {
            StoreroomTransfer.StoreroomTransferAutoGeneration_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, lulist);
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
}
