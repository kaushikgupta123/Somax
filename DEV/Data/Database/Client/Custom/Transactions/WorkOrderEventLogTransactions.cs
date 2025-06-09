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
    public class WorkOrderEventLog_RetrieveByWorkOrderId : WorkOrderEventLog_TransactionBaseClass
    {
        public List<b_WorkOrderEventLog> WorkOrderEventLogList { get; set; }

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
            List<b_WorkOrderEventLog> tmpArray = null;

            WorkOrderEventLog.RetrieveByWorkOrderIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            WorkOrderEventLogList = new List<b_WorkOrderEventLog>();
            foreach (b_WorkOrderEventLog tmpObj in tmpArray)
            {
                WorkOrderEventLogList.Add(tmpObj);
            }
        }
    }
  
  

   
}
