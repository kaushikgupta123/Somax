using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Client.Custom.Transactions
{
    #region Search Service Order
    public class ServiceOrder_RetrieveChunkSearch : ServiceOrder_TransactionBaseClass
    {

        public List<b_ServiceOrder> ServiceOrderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ServiceOrder.ServiceOrderId > 0)
            {
                string message = "Service Order has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_ServiceOrder> tmpList = null;
            ServiceOrder.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ServiceOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Details Service Order
    public class ServiceOrder_RetrieveByServiceOrderId : ServiceOrder_TransactionBaseClass
    {
        public b_ServiceOrder objServiceOrder { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ServiceOrder.ServiceOrderId == 0)
            {
                string message = "Service Order has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            b_ServiceOrder tmpobj = null;
            ServiceOrder.RetrieveByServiceOrderId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            objServiceOrder = tmpobj;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class ServiceOrder_RetrievePersonnelInitial : ServiceOrder_TransactionBaseClass
    {
        public b_ServiceOrder objServiceOrder { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ServiceOrder.ServiceOrderId == 0)
            {
                string message = "Service Order has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            b_ServiceOrder tmpobj = null;
            ServiceOrder.RetrievePersonnelInitial(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            objServiceOrder = tmpobj;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class ServiceOrder_AddRemoveScheduleRecord : ServiceOrder_TransactionBaseClass
    {
        public b_ServiceOrder objServiceOrder { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (ServiceOrder.ServiceOrderId == 0)
            //{
            //    string message = "Service Order has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        public override void PerformWorkItem()
        {
            ServiceOrder.AddRemoveScheduleRecord(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Fleet Only
    public class ServiceOrder_RetrieveDashboardChart : ServiceOrder_TransactionBaseClass
    {
        public List<b_ServiceOrder> ServiceOrderList { get; set; }

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
            List<b_ServiceOrder> tmpArray = null;

            ServiceOrder.ServiceOrder_RetrieveDashboardChart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ServiceOrderList = new List<b_ServiceOrder>();
            foreach (b_ServiceOrder tmpObj in tmpArray)
            {
                ServiceOrderList.Add(tmpObj);
            }
        }
    }

    #endregion

    #region Service order history
    public class ServiceOrder_ServiceOrderHistory : ServiceOrder_TransactionBaseClass
    {
        public List<b_ServiceOrder> ServiceOrderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ServiceOrder.ServiceOrderId == 0)
            {
                string message = "Service Order has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_ServiceOrder> tmpList = null;
            ServiceOrder.RetrieveServiceOrderHistory(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ServiceOrderList = tmpList;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Retrieve By EquipmentId
    public class ServiceOrder_RetrieveByEquipmentId : ServiceOrder_TransactionBaseClass
    {
        public List<b_ServiceOrder> ServiceOrderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ServiceOrder.EquipmentId == 0)
            {
                string message = "Service Order has an invalid EquipmentID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            List<b_ServiceOrder> tmpList = null;
            ServiceOrder.RetrieveByEquipmentId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ServiceOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
}
