using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Client.Custom.Transactions
{
   
    public class WorkOrderPlanEventLog_RetrieveByWorkOrderPlanId : WorkOrderPlanEventLog_TransactionBaseClass
    {
        public List<b_WorkOrderPlanEventLog> WorkOrderPlanEventLogList { get; set; }

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
            List<b_WorkOrderPlanEventLog> tmpArray = null;

            WorkOrderPlanEventLog.RetrieveByWorkOrderPlanIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            WorkOrderPlanEventLogList = new List<b_WorkOrderPlanEventLog>();
            foreach (b_WorkOrderPlanEventLog tmpObj in tmpArray)
            {
                WorkOrderPlanEventLogList.Add(tmpObj);
            }
        }
    }
}
