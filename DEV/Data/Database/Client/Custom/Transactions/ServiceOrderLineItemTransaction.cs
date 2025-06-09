using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{

    public class ServiceOrderLineItem_RetrieveByServiceOrderId : ServiceOrderLineItem_TransactionBaseClass
    {
        public List<b_ServiceOrderLineItem> ServiceOrderLineItemList { get; set; }

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
            List<b_ServiceOrderLineItem> tmpArray = null;

            ServiceOrderLineItem.ServiceOrderLineItem_RetrieveByServiceOrderId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ServiceOrderLineItemList = new List<b_ServiceOrderLineItem>();
            foreach (b_ServiceOrderLineItem tmpObj in tmpArray)
            {
                ServiceOrderLineItemList.Add(tmpObj);
            }
        }
    }

    public class ServiceOrderLineItem_ValidateForDelete : ServiceOrderLineItem_TransactionBaseClass
    {
        public b_ServiceOrderLineItem ServiceOrderLineData { get; set; }

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
            b_ServiceOrderLineItem tmpArray = null;
            ServiceOrderLineItem.ValidateForDeleteLineItem(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            ServiceOrderLineData = tmpArray;            
        }
    }

    public class ServiceOrderLineItem_DeleteCustom : ServiceOrderLineItem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ServiceOrderLineItem.ServiceOrderLineItemId == 0)
            {
                string message = "ServiceOrderLineItem has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            ServiceOrderLineItem.ServiceOrderLineDeleteFromDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
}
