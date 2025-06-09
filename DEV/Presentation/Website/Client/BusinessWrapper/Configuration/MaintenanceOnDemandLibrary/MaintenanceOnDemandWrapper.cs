using Client.Models.Configuration.MaintenanceOnDemandLibrary;
using DataContracts;
using System;
using System.Collections.Generic;
namespace Client.BusinessWrapper.Configuration.MaintenanceOnDemandLibrary
{
    public class MaintenanceOnDemandWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public MaintenanceOnDemandWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search/Detail
        internal List<MaintenanceOnDemandModel> RetrieveAllBySiteId()
        {
            List<MaintenanceOnDemandModel> mOnDemandList = new List<MaintenanceOnDemandModel>();
            MaintenanceOnDemandModel objMaintenanceOnDemandModel;
            MaintOnDemandMaster maintOnDemandMaster = new MaintOnDemandMaster()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<MaintOnDemandMaster> maintOnDemandMasterList = maintOnDemandMaster.RetrieveAllBySiteId(this.userData.DatabaseKey, this.userData.Site.TimeZone);
            foreach (var item in maintOnDemandMasterList)
            {
                objMaintenanceOnDemandModel = new MaintenanceOnDemandModel();
                objMaintenanceOnDemandModel.MaintOnDemandMasterId = item.MaintOnDemandMasterId;
                objMaintenanceOnDemandModel.ClientLookUpId = item.ClientLookUpId;
                objMaintenanceOnDemandModel.Description = item.Description;
                objMaintenanceOnDemandModel.Type = item.Type;
                objMaintenanceOnDemandModel.CreateDate = item.CreateDate;
                mOnDemandList.Add(objMaintenanceOnDemandModel);
            }
            return mOnDemandList;
        }
        internal MaintenanceOnDemandModel populateOndemandDetails(long MaintOnDemandMasterId)
        {
            MaintenanceOnDemandModel oModel = new MaintenanceOnDemandModel();
            MaintOnDemandMaster maintOnDemandMaster = new MaintOnDemandMaster()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                MaintOnDemandMasterId = MaintOnDemandMasterId
            };
            maintOnDemandMaster.Retrieve(userData.DatabaseKey);
            oModel = PopulateModel(maintOnDemandMaster);
            return oModel;
        }
        internal MaintenanceOnDemandModel PopulateModel(MaintOnDemandMaster dbObj)
        {
            MaintenanceOnDemandModel oModel = new MaintenanceOnDemandModel();
            oModel.MaintOnDemandMasterId = dbObj.MaintOnDemandMasterId;
            oModel.ClientLookUpId = dbObj.ClientLookUpId;
            oModel.Type = dbObj.Type;
            oModel.Description = dbObj.Description;
            oModel.CreateDate = dbObj.CreateDate;
            return oModel;
        }
        #endregion
        #region Task
        internal List<MaintOnDemandMasterTask> PopulateTasks(long MaintOnDemandMasterId)
        {
            MaintOnDemandMasterTask task = new MaintOnDemandMasterTask()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                MaintOnDemandMasterId = MaintOnDemandMasterId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName
            };
            List<MaintOnDemandMasterTask> taskList = task.MaintOnDemandMasterTask_RetrieveByProcedureID(userData.DatabaseKey);
            return taskList;
        }
        internal bool DeleteTask(long MasterTaskId)
        {
            try
            {
                MaintOnDemandMasterTask tsk = new MaintOnDemandMasterTask()
                {
                    MaintOnDemandMasterTaskId = MasterTaskId
                };
                tsk.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<String> AddOrUpdateTask(MaintenanceOnDemandVM objVM, ref string Mode)
        {
            MaintOnDemandMasterTask task = new MaintOnDemandMasterTask();
            if (objVM.taskModel.MaintOnDemandMasterTaskId == null)
            {
                Mode = "add";
                task.ClientId = userData.DatabaseKey.Personnel.ClientId;
                task.TaskId = objVM.taskModel.TaskId.Trim();
                task.Description = objVM.taskModel.Description.Trim();
                task.MaintOnDemandMasterId = objVM.taskModel.MaintOnDemandMasterId;
                task.Create(this.userData.DatabaseKey);
            }
            else
            {
                Mode = "update";
                task.MaintOnDemandMasterTaskId = objVM.taskModel.MaintOnDemandMasterTaskId ?? 0;
                task.Retrieve(this.userData.DatabaseKey);
                task.TaskId = objVM.taskModel.TaskId.Trim();
                task.Description = objVM.taskModel.Description.Trim();
                task.Update(this.userData.DatabaseKey);
            }
            return task.ErrorMessages;
        }
        #endregion
        #region On-Demand Add
        public List<String> AddOrUpdateOndemand(MaintenanceOnDemandVM objVM, ref string Mode, ref long onDemandMasterId)
        {
            MaintOnDemandMaster maintOnDemand = new MaintOnDemandMaster()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
            };
            if (objVM.maintenanceOnDemanModel.MaintOnDemandMasterId == 0)
            {
                Mode = "add";
                maintOnDemand.ClientLookUpId = objVM.maintenanceOnDemanModel.ClientLookUpId;
                maintOnDemand.Description = objVM.maintenanceOnDemanModel.Description;
                maintOnDemand.Type = objVM.maintenanceOnDemanModel.Type;
                maintOnDemand.CreateByPKForeignKeys(this.userData.DatabaseKey);
                onDemandMasterId = maintOnDemand.MaintOnDemandMasterId;
            }
            else
            {
                Mode = "update";
                maintOnDemand.MaintOnDemandMasterId = objVM.maintenanceOnDemanModel.MaintOnDemandMasterId;
                maintOnDemand.Retrieve(this.userData.DatabaseKey);
                maintOnDemand.Description = objVM.maintenanceOnDemanModel.Description;
                maintOnDemand.Type = objVM.maintenanceOnDemanModel.Type ?? string.Empty;
                onDemandMasterId = maintOnDemand.MaintOnDemandMasterId;
                maintOnDemand.Update(this.userData.DatabaseKey);
            }
            return maintOnDemand.ErrorMessages;
        }
        #endregion
    }

}