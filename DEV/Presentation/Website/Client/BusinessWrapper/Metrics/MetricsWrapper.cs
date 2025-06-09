using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Client.Models;

namespace Client.BusinessWrapper.Metrics
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

        #region Retrieve Maintenance Details
         public List<MetricsForMaintenanceModel> GetMetricsForMaintenance(int duration)
        {
            List<MetricsForMaintenanceModel> MaintenanceModelList = new List<MetricsForMaintenanceModel>();

            DataContracts.Metrics me = new DataContracts.Metrics()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                TimeFrame= duration
            };
            var met = me.RetrieveMaintenanceDetailsByMetricsName(this.userData.DatabaseKey);
          
            if (met != null)
            {
                // var eqpList = serv.Select(x => new { x.EquipmentId, x.Part_ClientLookupId, x.Part_Description, x.QuantityNeeded, x.QuantityUsed, x.Comment, x.Equipment_Parts_XrefId, x.UpdateIndex, x.PartId }).ToList();

                MetricsForMaintenanceModel objSMeModel;
                foreach (var v in met)
                {
                    objSMeModel = new MetricsForMaintenanceModel();
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

        #region Retrieve Inventory Details
        public List<MetricsForInventoryModel> GetMetricsForInventory()
        {
            List<MetricsForInventoryModel> InventoryModelList = new List<MetricsForInventoryModel>();

            DataContracts.Metrics me = new DataContracts.Metrics()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            var met = me.RetrieveInventoryDetailsByMetricsName(this.userData.DatabaseKey);

            if (met != null)
            {
                // var eqpList = serv.Select(x => new { x.EquipmentId, x.Part_ClientLookupId, x.Part_Description, x.QuantityNeeded, x.QuantityUsed, x.Comment, x.Equipment_Parts_XrefId, x.UpdateIndex, x.PartId }).ToList();

                MetricsForInventoryModel objInvModel;
                foreach (var v in met)
                {
                    objInvModel = new MetricsForInventoryModel();
                    objInvModel.SiteName = v.SiteName;
                    objInvModel.Valuation = v.Valuation;
                    objInvModel.LowParts = v.LowParts;


                    InventoryModelList.Add(objInvModel);
                }
            }

            return InventoryModelList;
        }
        #endregion

        #region Retrieve Purchasing Details
        public List<MetricsForPurchasingModel> GetMetricsForPurchasing(int duration)
        {
            List<MetricsForPurchasingModel> PurchasingModelList = new List<MetricsForPurchasingModel>();

            DataContracts.Metrics me = new DataContracts.Metrics()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                TimeFrame= duration
            };
            var met = me.RetrievePurchasingDetailsByMetricsName(this.userData.DatabaseKey);

            if (met != null)
            {
                // var eqpList = serv.Select(x => new { x.EquipmentId, x.Part_ClientLookupId, x.Part_Description, x.QuantityNeeded, x.QuantityUsed, x.Comment, x.Equipment_Parts_XrefId, x.UpdateIndex, x.PartId }).ToList();

                MetricsForPurchasingModel objPerModel;
                foreach (var v in met)
                {
                    objPerModel = new MetricsForPurchasingModel();
                    objPerModel.SiteName = v.SiteName;
                    objPerModel.PurchaseOrdersCreated = v.PurchaseOrdersCreated;
                    objPerModel.PurchaseOrdersCompleted = v.PurchaseOrdersCompleted;
                    //objPerModel.ReceivedAmount = v.ReceivedAmount; 
                    objPerModel.ReceivedAmount = Math.Round(v.ReceivedAmount, 2);

                    PurchasingModelList.Add(objPerModel);
                }
            }

            return PurchasingModelList;
        }
        #endregion
    }
}