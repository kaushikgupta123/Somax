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
  
        public class PMSchedUDF_RetrieveByPrevMaintSchedId : PMSchedUDF_TransactionBaseClass
    {
            public b_PMSchedUDF PMSchedUDFs { get; set; }

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
            b_PMSchedUDF tmpList = null;
            PMSchedUDF.RetrieveByPrevMaintSchedIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            PMSchedUDFs = new b_PMSchedUDF();
            PMSchedUDFs = tmpList;

          }
        }
    
}

