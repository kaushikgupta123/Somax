using Admin.Models.Metrics;

using DataContracts;

using System;
using System.Collections.Generic;

namespace Admin.BusinessWrapper.Metrics
{
    public class MetricsWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public MetricsWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region For Admin

        #region Retrieve Maintenance Details For Admin
        public List<MetricsMaintenanceModelForAdmin> GetMetricsMaintenanceForAdmin(int duration)
        {
            List<MetricsMaintenanceModelForAdmin> MaintenanceModelList = new List<MetricsMaintenanceModelForAdmin>();

            DataContracts.Metrics me = new DataContracts.Metrics()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                TimeFrame = duration
            };
            var met = me.RetrieveMaintenanceDetailsByMetricsNameForAdmin(this.userData.DatabaseKey);

            if (met != null)
            {
                // var eqpList = serv.Select(x => new { x.EquipmentId, x.Part_ClientLookupId, x.Part_Description, x.QuantityNeeded, x.QuantityUsed, x.Comment, x.Equipment_Parts_XrefId, x.UpdateIndex, x.PartId }).ToList();

                MetricsMaintenanceModelForAdmin objSMeModel;
                foreach (var v in met)
                {
                    objSMeModel = new MetricsMaintenanceModelForAdmin();
                    objSMeModel.ClientName = v.ClientName;
                    objSMeModel.SiteName = v.SiteName;
                    objSMeModel.WorkOrdersCreated = v.WorkOrdersCreated;
                    objSMeModel.WorkOrdersCompleted = v.WorkOrdersCompleted;
                    objSMeModel.LaborHours = v.LaborHours;

                    MaintenanceModelList.Add(objSMeModel);
                }
            }

            return MaintenanceModelList;
        }
        #endregion

        #region Retrieve Inventory Details For Admin
        public List<MetricsInventoryModelForAdmin> GetMetricsForInventoryForAdmin()
        {
            List<MetricsInventoryModelForAdmin> InventoryModelList = new List<MetricsInventoryModelForAdmin>();

            DataContracts.Metrics me = new DataContracts.Metrics()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            var met = me.RetrieveInventoryDetailsByMetricsNameForAdmin(this.userData.DatabaseKey);

            if (met != null)
            {
                // var eqpList = serv.Select(x => new { x.EquipmentId, x.Part_ClientLookupId, x.Part_Description, x.QuantityNeeded, x.QuantityUsed, x.Comment, x.Equipment_Parts_XrefId, x.UpdateIndex, x.PartId }).ToList();

                MetricsInventoryModelForAdmin objInvModel;
                foreach (var v in met)
                {
                    objInvModel = new MetricsInventoryModelForAdmin();
                    objInvModel.ClientName = v.ClientName;
                    objInvModel.SiteName = v.SiteName;
                    objInvModel.Valuation = v.Valuation;
                    objInvModel.LowParts = v.LowParts;


                    InventoryModelList.Add(objInvModel);
                }
            }

            return InventoryModelList;
        }
        #endregion

        #region Retrieve Purchasing Details For Admin
        public List<MetricsPurchasingModelForAdmin> GetMetricsForPurchasingForAdmin(int duration)
        {
            List<MetricsPurchasingModelForAdmin> PurchasingModelList = new List<MetricsPurchasingModelForAdmin>();

            DataContracts.Metrics me = new DataContracts.Metrics()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                TimeFrame = duration
            };
            var met = me.RetrievePurchasingDetailsByMetricsNameForAdmin(this.userData.DatabaseKey);

            if (met != null)
            {
                // var eqpList = serv.Select(x => new { x.EquipmentId, x.Part_ClientLookupId, x.Part_Description, x.QuantityNeeded, x.QuantityUsed, x.Comment, x.Equipment_Parts_XrefId, x.UpdateIndex, x.PartId }).ToList();

                MetricsPurchasingModelForAdmin objPerModel;
                foreach (var v in met)
                {
                    objPerModel = new MetricsPurchasingModelForAdmin();
                    objPerModel.ClientName = v.ClientName;
                    objPerModel.SiteName = v.SiteName;
                    objPerModel.PurchaseOrdersCreated = v.PurchaseOrdersCreated;
                    objPerModel.PurchaseOrdersCompleted = v.PurchaseOrdersCompleted;
                    objPerModel.ReceivedAmount = Math.Round(v.ReceivedAmount, 2);

                    PurchasingModelList.Add(objPerModel);
                }
            }

            return PurchasingModelList;
        }
        #endregion

        #endregion
    }
}