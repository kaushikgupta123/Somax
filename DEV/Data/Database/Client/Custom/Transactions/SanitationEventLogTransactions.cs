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
    public class SanitationEventLog_RetrieveBySanitationId :SanitationEventLog_TransactionBaseClass
    {
        public List<b_SanitationEventLog> SanitationEventLogList { get; set; }

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
            List<b_SanitationEventLog> tmpArray = null;

            SanitationEventLog.RetrieveBySanitationJobIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SanitationEventLogList = new List<b_SanitationEventLog>();
            foreach (b_SanitationEventLog tmpObj in tmpArray)
            {
                SanitationEventLogList.Add(tmpObj);
            }
        }
    }
  
  

   
}
