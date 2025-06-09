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
  
        public class WorkOrderUDF_RetrieveByWorkOrderId : WorkOrderUDF_TransactionBaseClass
    {
            public b_WorkOrderUDF WorkOrderUDFs { get; set; }

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
            b_WorkOrderUDF tmpList = null;
            WorkOrderUDF.RetrieveByWorkOrderIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderUDFs = new b_WorkOrderUDF();
            WorkOrderUDFs = tmpList;

          }
        }
    
}

