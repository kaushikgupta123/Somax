using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;

namespace Database
{
    public class PRLineUDF_RetrieveByPurchaseRequestLineItemId : PRLineUDF_TransactionBaseClass
    {
        public override void PerformWorkItem()
        {
            b_PRLineUDF tmpobj = null;
            PRLineUDF.RetrieveByPurchaseRequestLineItemId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            PRLineUDF = tmpobj;
        }
    }
    #region deletion by PurchaseRequestLineItemId
    public class PRLineUDF_DeleteByPurchaseRequestLineItemId : PRLineUDF_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PRLineUDF.PurchaseRequestLineItemId == 0)
            {
                string message = "PRLineUDF has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PRLineUDF.DeleteByPurchaseRequestLineItemId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PRLineUDF.PurchaseRequestLineItemId > 0);
        }
    }
    #endregion
}
