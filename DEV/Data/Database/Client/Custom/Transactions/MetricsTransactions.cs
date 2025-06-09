using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Client.Custom.Transactions
{
    #region Retrieve Maintenance Details
    public class Metrics_RetrieveMaintenanceDetails : Metrics_TransactionBaseClass
    {
        public List<b_Metrics> MetricsList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
           
        }

        public override void PerformWorkItem()
        {
            List<b_Metrics> tmpList = null;
            Metrics.RetrieveMaintenanceForSiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            MetricsList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Retrieve Inventory Details
    public class Metrics_RetrieveInventoryDetails : Metrics_TransactionBaseClass
    {
        public List<b_Metrics> MetricsList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            List<b_Metrics> tmpList = null;
            Metrics.RetrieveInventoryForSiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            MetricsList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Retrieve Purchasing Details
    public class Metrics_RetrievePurchasingDetails : Metrics_TransactionBaseClass
    {
        public List<b_Metrics> MetricsList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            List<b_Metrics> tmpList = null;
            Metrics.RetrievePurchasingForSiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            MetricsList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Retrieve Metrics Value Sum By Data Date
    public class Metrics_RetrieveMetricsValueSumByDataDate : Metrics_TransactionBaseClass
    {
        public List<b_Metrics> MetricsList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            List<b_Metrics> tmpList = null;
            Metrics.RetrieveMetricsValueSumByDataDate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            MetricsList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region For Admin
    #region Retrieve Maintenance Details For Admin
    public class Metrics_RetrieveMaintenanceDetailsForAdmin : Metrics_TransactionBaseClass
    {
        public List<b_Metrics> MetricsList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            List<b_Metrics> tmpList = null;
            Metrics.RetrieveMaintenanceForAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            MetricsList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Retrieve Inventory Details For Admin
    public class Metrics_RetrieveInventoryDetailsForAdmin : Metrics_TransactionBaseClass
    {
        public List<b_Metrics> MetricsList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            List<b_Metrics> tmpList = null;
            Metrics.RetrieveInventoryForAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            MetricsList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Retrieve Purchasing Details For Admin
    public class Metrics_RetrievePurchasingDetailsForAdmin : Metrics_TransactionBaseClass
    {
        public List<b_Metrics> MetricsList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            List<b_Metrics> tmpList = null;
            Metrics.RetrievePurchasingForAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            MetricsList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #endregion
}
