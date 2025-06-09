using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class WOPlanLineItem_RetrieveByWorkOrderPlanId : WOPlanLineItem_TransactionBaseClass
    {
        public List<b_WOPlanLineItem> WOPlanLineItemList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WOPlanLineItem.WorkOrderPlanId == 0)
            {
                string message = "WOPlanLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_WOPlanLineItem> tmpList = null;
            WOPlanLineItem.WOPlanLineItem_RetriveByWorkOrderPlanId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WOPlanLineItemList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
}
