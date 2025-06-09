using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;

namespace Database
{
    public class POLine_RetrieveByPurchaseOrderLineItemId : POLineUDF_TransactionBaseClass
    {
        public override void PerformWorkItem()
        {
            b_POLineUDF tmpobj = null;
            POLineUDF.RetrieveByPurchaseOrderLineItemId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            POLineUDF = tmpobj;
        }
    }
    #region deletion by PurchaseOrderLineItemId
    public class POLineUDF_DeleteByPurchaseOrderLineItemId : POLineUDF_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (POLineUDF.PurchaseOrderLineItemId == 0)
            {
                string message = "POLineUDF has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            POLineUDF.DeleteByPurchaseOrderLineItemId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(POLineUDF.PurchaseOrderLineItemId > 0);
        }
    }
    #endregion
}
