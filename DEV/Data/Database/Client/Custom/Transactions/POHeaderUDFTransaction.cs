using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;

namespace Database
{
    public class POHeaderUDF_RetrieveByPurchaseOrderId : POHeaderUDF_TransactionBaseClass
    {
        public override void PerformWorkItem()
        {
            b_POHeaderUDF tmpobj = null;
            POHeaderUDF.RetrieveByPurchaseOrderId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            POHeaderUDF = tmpobj;
        }
    }
}
