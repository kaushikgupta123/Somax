using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;

namespace Database
{
    public class PRHeaderUDF_RetrieveByPurchaseRequestId : PRHeaderUDF_TransactionBaseClass
    {
        public override void PerformWorkItem()
        {
            b_PRHeaderUDF tmpobj = null;
            PRHeaderUDF.RetrieveByPurchaseRequestId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            PRHeaderUDF = tmpobj;
        }
    }
}
