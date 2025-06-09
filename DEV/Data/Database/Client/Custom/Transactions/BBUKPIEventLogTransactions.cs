using Database.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Client.Custom.Transactions
{
    public class BBUKPIEventLog_RetrieveByBBUKPIId : BBUKPIEventLog_TransactionBaseClass
    {
        public List<b_BBUKPIEventLog> BBUKPIEventLogList { get; set; }

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
            List<b_BBUKPIEventLog> tmpArray = null;

            BBUKPIEventLog.RetrieveByBBUKPIIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            BBUKPIEventLogList = new List<b_BBUKPIEventLog>();
            foreach (b_BBUKPIEventLog tmpObj in tmpArray)
            {
                BBUKPIEventLogList.Add(tmpObj);
            }
        }
    }
}
