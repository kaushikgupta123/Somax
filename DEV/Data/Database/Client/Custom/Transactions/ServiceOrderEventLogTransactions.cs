using Database.Business;
using System.Collections.Generic;

namespace Database.Transactions
{
    public class ServiceOrderEventLog_RetrieveByServiceOrderId : ServiceOrderEventLog_TransactionBaseClass
    {
        public List<b_ServiceOrderEventLog> ServiceOrderEventLogList { get; set; }

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
            List<b_ServiceOrderEventLog> tmpArray = null;
            ServiceOrderEventLog.RetrieveByServiceOrderId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ServiceOrderEventLogList = new List<b_ServiceOrderEventLog>();
            foreach (b_ServiceOrderEventLog tmpObj in tmpArray)
            {
                ServiceOrderEventLogList.Add(tmpObj);
            }
        }
    }
}
