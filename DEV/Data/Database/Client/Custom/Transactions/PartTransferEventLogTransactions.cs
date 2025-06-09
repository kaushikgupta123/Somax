using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using System.Data.SqlClient;
using Database.StoredProcedure;

namespace Database.Client.Custom.Transactions
{
    public class PartTransferEventLog_ReteriveByPartTransferId : PartTransferEventLog_TransactionBaseClass
    {
        public List<b_PartTransferEventLog> PartTransferEventLogList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_PartTransferEventLog> tmpArray = null;

            PartTransferEventLog.RetrieveByPartTransferIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PartTransferEventLogList = new List<b_PartTransferEventLog>();
            foreach (b_PartTransferEventLog tmpObj in tmpArray)
            {
                PartTransferEventLogList.Add(tmpObj);
            }
        }
    }

    public class PartTransferEventLog_RetrieveForAlert : PartTransferEventLog_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartTransferEventLog.PartTransferEventLogId == 0)
            {
                string message = "PartTransferEventLog has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PartTransferEventLog.RetrieveForAlert(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


}
