using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using System.Data.SqlClient;
using Database.StoredProcedure;

namespace Database
{
    public class PurchasingEventLogTransactions_RetrieveByObjectId : PurchasingEventLog_TransactionBaseClass
    {
        public List<b_PurchasingEventLog> PurchasingEventLogList { get; set; }

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
            List<b_PurchasingEventLog> tmpArray = null;

            PurchasingEventLog.RetrieveByObjectIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PurchasingEventLogList = new List<b_PurchasingEventLog>();
            foreach (b_PurchasingEventLog tmpObj in tmpArray)
            {
                PurchasingEventLogList.Add(tmpObj);
            }
        }
    }
  
  

   
}
