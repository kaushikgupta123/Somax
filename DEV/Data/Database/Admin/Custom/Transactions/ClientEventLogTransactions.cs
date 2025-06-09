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
    public class ClientEventLog_ReteriveByClientEventLog : ClientEventLog_TransactionBaseClass
    {
        public List<b_ClientEventLog> ClientEventLogList { get; set; }
        public long SearchClientId { get; set; }

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
            List<b_ClientEventLog> tmpArray = null;

            ClientEventLog.ClientId = SearchClientId;
            ClientEventLog.RetrieveByClientIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ClientEventLogList = new List<b_ClientEventLog>();
            foreach (b_ClientEventLog tmpObj in tmpArray)
            {
                ClientEventLogList.Add(tmpObj);
            }
        }
    }
    public class ClientEventLog_CreateFromAdmin : ClientEventLog_TransactionBaseClass
    {
        public long SearchClientId { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ClientEventLog.ClientEventLogId > 0)
            {
                string message = "ClientEventLog has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            ClientEventLog.ClientId = SearchClientId;
            ClientEventLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            //base.Postprocess();
            //System.Diagnostics.Debug.Assert(Site.SiteId > 0);
        }
    }
}
