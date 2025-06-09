using Database.Business;
using Database.Client.Custom.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class Metrics : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public string SiteName { get; set; }
        public string ClientName { get; set; }
        public decimal WorkOrdersCreated { get; set; }
        public decimal WorkOrdersCompleted { get; set; }
        public decimal LaborHours { get; set; }
        public decimal Valuation { get; set; }
        public decimal LowParts { get; set; }
        public decimal PurchaseOrdersCreated { get; set; }
        public decimal PurchaseOrdersCompleted { get; set; }
        public decimal ReceivedAmount { get; set; }
        public int TimeFrame { get; set; }
        public decimal TotalValue { get; set; }
        public int CaseNo { get; set; }
        public bool IsEnterprise { get; set; }
        #endregion
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            throw new NotImplementedException();
        }
        #region For Maintenance Details
        public List<Metrics> RetrieveMaintenanceDetailsByMetricsName(DatabaseKey dbKey)
        {
            Metrics_RetrieveMaintenanceDetails trans = new Metrics_RetrieveMaintenanceDetails()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                
            };
            trans.Metrics = this.ToDateBaseObjectForRetrieveMaintenanceDetails();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Metrics> Metricslist = new List<Metrics>();

            foreach (b_Metrics metrics in trans.MetricsList)
            {
                Metrics tmpMetrics = new Metrics();
                tmpMetrics.UpdateFromDateBaseObjectForRetrieveMaintenanceDetails(metrics);
                Metricslist.Add(tmpMetrics);
            }
            return Metricslist;
        }
        public b_Metrics ToDateBaseObjectForRetrieveMaintenanceDetails()
        {
            b_Metrics dbObj = this.ToDatabaseObject();
            dbObj.TimeFrame = this.TimeFrame;
            dbObj.ClientId = this.ClientId;
            dbObj.SiteName = this.SiteName;
            dbObj.WorkOrdersCreated = this.WorkOrdersCreated;
            dbObj.WorkOrdersCompleted = this.WorkOrdersCompleted;
            dbObj.LaborHours = this.LaborHours;
            
            return dbObj;
        }
        public void UpdateFromDateBaseObjectForRetrieveMaintenanceDetails(b_Metrics dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.SiteName = dbObj.SiteName;
            this.WorkOrdersCreated = dbObj.WorkOrdersCreated;
            this.WorkOrdersCompleted = dbObj.WorkOrdersCompleted;
            this.LaborHours = dbObj.LaborHours;
            
        }
        #endregion

        #region For Inventory Details
        public List<Metrics> RetrieveInventoryDetailsByMetricsName(DatabaseKey dbKey)
        {
            Metrics_RetrieveInventoryDetails trans = new Metrics_RetrieveInventoryDetails()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Metrics = this.ToDateBaseObjectForRetrieveInventoryDetails();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Metrics> Metricslist = new List<Metrics>();

            foreach (b_Metrics metrics in trans.MetricsList)
            {
                Metrics tmpMetrics = new Metrics();
                tmpMetrics.UpdateFromDateBaseObjectForRetrieveInventoryDetails(metrics);
                Metricslist.Add(tmpMetrics);
            }
            return Metricslist;
        }
        public b_Metrics ToDateBaseObjectForRetrieveInventoryDetails()
        {
            b_Metrics dbObj = this.ToDatabaseObject();
            dbObj.SiteName = this.SiteName;
            dbObj.Valuation = this.Valuation;
            dbObj.LowParts = this.LowParts;
           
            return dbObj;
        }
        public void UpdateFromDateBaseObjectForRetrieveInventoryDetails(b_Metrics dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.SiteName = dbObj.SiteName;
            this.Valuation = dbObj.Valuation;
            this.LowParts = dbObj.LowParts;

        }
        #endregion

        #region For Purchasing Details
        public List<Metrics> RetrievePurchasingDetailsByMetricsName(DatabaseKey dbKey)
        {
            Metrics_RetrievePurchasingDetails trans = new Metrics_RetrievePurchasingDetails()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Metrics = this.ToDateBaseObjectForRetrievePurchasingDetails();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Metrics> Metricslist = new List<Metrics>();

            foreach (b_Metrics metrics in trans.MetricsList)
            {
                Metrics tmpMetrics = new Metrics();
                tmpMetrics.UpdateFromDateBaseObjectForRetrievePurchasingDetails(metrics);
                Metricslist.Add(tmpMetrics);
            }
            return Metricslist;
        }
        public b_Metrics ToDateBaseObjectForRetrievePurchasingDetails()
        {
            b_Metrics dbObj = this.ToDatabaseObject();
            dbObj.TimeFrame = this.TimeFrame;
            dbObj.SiteName = this.SiteName;
            dbObj.PurchaseOrdersCreated = this.PurchaseOrdersCreated;
            dbObj.PurchaseOrdersCompleted = this.PurchaseOrdersCompleted;
            dbObj.ReceivedAmount = this.ReceivedAmount;
            return dbObj;
        }
        public void UpdateFromDateBaseObjectForRetrievePurchasingDetails(b_Metrics dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.SiteName = dbObj.SiteName;
            this.PurchaseOrdersCreated = dbObj.PurchaseOrdersCreated;
            this.PurchaseOrdersCompleted = dbObj.PurchaseOrdersCompleted;
            this.ReceivedAmount = dbObj.ReceivedAmount;
        }
        #endregion

        #region For Metrics Value Sum By Data Date
        public List<Metrics> RetrieveMetricsValueSumByDataDate(DatabaseKey dbKey)
        {
            Metrics_RetrieveMetricsValueSumByDataDate trans = new Metrics_RetrieveMetricsValueSumByDataDate()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Metrics = this.ToDateBaseObjectForRetrieveMetricsValueSumByDataDate();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Metrics> Metricslist = new List<Metrics>();

            foreach (b_Metrics metrics in trans.MetricsList)
            {
                Metrics tmpMetrics = new Metrics();
                tmpMetrics.UpdateFromDateBaseObjectForRetrieveMetricsValueSumByDataDate(metrics);
                Metricslist.Add(tmpMetrics);
            }
            return Metricslist;
        }
        public b_Metrics ToDateBaseObjectForRetrieveMetricsValueSumByDataDate()
        {
            b_Metrics dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.MetricName = this.MetricName;
            dbObj.CaseNo = this.CaseNo;
            dbObj.IsEnterprise = this.IsEnterprise;
            return dbObj;
        }
        public void UpdateFromDateBaseObjectForRetrieveMetricsValueSumByDataDate(b_Metrics dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.TotalValue = dbObj.TotalValue;            
        }
        #endregion

        #region For Admin
        #region For Maintenance Details For Admin
        public List<Metrics> RetrieveMaintenanceDetailsByMetricsNameForAdmin(DatabaseKey dbKey)
        {
            Metrics_RetrieveMaintenanceDetailsForAdmin trans = new Metrics_RetrieveMaintenanceDetailsForAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.Metrics = this.ToDateBaseObjectForRetrieveMaintenanceDetailsForAdmin();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Metrics> Metricslist = new List<Metrics>();

            foreach (b_Metrics metrics in trans.MetricsList)
            {
                Metrics tmpMetrics = new Metrics();
                tmpMetrics.UpdateFromDateBaseObjectForRetrieveMaintenanceDetailsForAdmin(metrics);
                Metricslist.Add(tmpMetrics);
            }
            return Metricslist;
        }
        public b_Metrics ToDateBaseObjectForRetrieveMaintenanceDetailsForAdmin()
        {
            b_Metrics dbObj = this.ToDatabaseObject();
            dbObj.TimeFrame = this.TimeFrame;
            dbObj.ClientId = this.ClientId;
            dbObj.ClientName = this.ClientName;
            dbObj.SiteName = this.SiteName;
            dbObj.WorkOrdersCreated = this.WorkOrdersCreated;
            dbObj.WorkOrdersCompleted = this.WorkOrdersCompleted;
            dbObj.LaborHours = this.LaborHours;

            return dbObj;
        }
        public void UpdateFromDateBaseObjectForRetrieveMaintenanceDetailsForAdmin(b_Metrics dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ClientName = dbObj.ClientName;
            this.SiteName = dbObj.SiteName;
            this.WorkOrdersCreated = dbObj.WorkOrdersCreated;
            this.WorkOrdersCompleted = dbObj.WorkOrdersCompleted;
            this.LaborHours = dbObj.LaborHours;

        }
        #endregion

        #region For Inventory Details For Admin
        public List<Metrics> RetrieveInventoryDetailsByMetricsNameForAdmin(DatabaseKey dbKey)
        {
            Metrics_RetrieveInventoryDetailsForAdmin trans = new Metrics_RetrieveInventoryDetailsForAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Metrics = this.ToDateBaseObjectForRetrieveInventoryDetailsForAdmin();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Metrics> Metricslist = new List<Metrics>();

            foreach (b_Metrics metrics in trans.MetricsList)
            {
                Metrics tmpMetrics = new Metrics();
                tmpMetrics.UpdateFromDateBaseObjectForRetrieveInventoryDetailsForAdmin(metrics);
                Metricslist.Add(tmpMetrics);
            }
            return Metricslist;
        }
        public b_Metrics ToDateBaseObjectForRetrieveInventoryDetailsForAdmin()
        {
            b_Metrics dbObj = this.ToDatabaseObject();
            dbObj.ClientName = this.ClientName;
            dbObj.SiteName = this.SiteName;
            dbObj.Valuation = this.Valuation;
            dbObj.LowParts = this.LowParts;

            return dbObj;
        }
        public void UpdateFromDateBaseObjectForRetrieveInventoryDetailsForAdmin(b_Metrics dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ClientName = dbObj.ClientName;
            this.SiteName = dbObj.SiteName;
            this.Valuation = dbObj.Valuation;
            this.LowParts = dbObj.LowParts;

        }
        #endregion

        #region For Purchasing Details For Admin
        public List<Metrics> RetrievePurchasingDetailsByMetricsNameForAdmin(DatabaseKey dbKey)
        {
            Metrics_RetrievePurchasingDetailsForAdmin trans = new Metrics_RetrievePurchasingDetailsForAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Metrics = this.ToDateBaseObjectForRetrievePurchasingDetailsForAdmin();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Metrics> Metricslist = new List<Metrics>();

            foreach (b_Metrics metrics in trans.MetricsList)
            {
                Metrics tmpMetrics = new Metrics();
                tmpMetrics.UpdateFromDateBaseObjectForRetrievePurchasingDetailsForAdmin(metrics);
                Metricslist.Add(tmpMetrics);
            }
            return Metricslist;
        }
        public b_Metrics ToDateBaseObjectForRetrievePurchasingDetailsForAdmin()
        {
            b_Metrics dbObj = this.ToDatabaseObject();
            dbObj.TimeFrame = this.TimeFrame;
            dbObj.ClientName = this.ClientName;
            dbObj.SiteName = this.SiteName;
            dbObj.PurchaseOrdersCreated = this.PurchaseOrdersCreated;
            dbObj.PurchaseOrdersCompleted = this.PurchaseOrdersCompleted;
            dbObj.ReceivedAmount = this.ReceivedAmount;
            return dbObj;
        }
        public void UpdateFromDateBaseObjectForRetrievePurchasingDetailsForAdmin(b_Metrics dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ClientName = dbObj.ClientName;
            this.SiteName = dbObj.SiteName;
            this.PurchaseOrdersCreated = dbObj.PurchaseOrdersCreated;
            this.PurchaseOrdersCompleted = dbObj.PurchaseOrdersCompleted;
            this.ReceivedAmount = dbObj.ReceivedAmount;
        }
        #endregion

        #endregion
    }
}
