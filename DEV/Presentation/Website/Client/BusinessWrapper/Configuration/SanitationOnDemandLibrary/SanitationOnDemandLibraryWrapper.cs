using Client.BusinessWrapper.Common;
using Client.Models.Configuration.SanitationOnDemandLibrary;
using DataContracts;
using System;
using System.Collections.Generic;
namespace Client.BusinessWrapper.Configuration.SanitationOnDemandLibrary
{
    public class SanitationOnDemandLibraryWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public SanitationOnDemandLibraryWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search
        public List<SanitationOnDemandLibModel> PopulateSanitOnDemandLibData()
        {
            SanOnDemandMaster SanOnDemandMaster = new SanOnDemandMaster();
            SanOnDemandMaster.SiteId = userData.DatabaseKey.User.DefaultSiteId;
           // List<SanOnDemandMaster> GridSource = SanOnDemandMaster.Retrieve_SanOnDemandMaster_ByFilterCriteria(this.userData.DatabaseKey, userData.Site.TimeZone);
            List<SanOnDemandMaster> GridSource = SanOnDemandMaster.Retrieve_SanOnDemandMaster_ByFilterCriteria_V2(this.userData.DatabaseKey, userData.Site.TimeZone);
            SanitationOnDemandLibModel objSanitationOnDemandLibModel;
            List<SanitationOnDemandLibModel> SanitationOnDemandLibModelList = new List<SanitationOnDemandLibModel>();
            foreach (var p in GridSource)
            {
                objSanitationOnDemandLibModel = new SanitationOnDemandLibModel();
                objSanitationOnDemandLibModel.SanOnDemandMasterId = p.SanOnDemandMasterId;
                objSanitationOnDemandLibModel.ClientLookUpId = p.ClientLookUpId;
                objSanitationOnDemandLibModel.Description = p.Description;
                objSanitationOnDemandLibModel.CreateDate = p.CreateDate;
                SanitationOnDemandLibModelList.Add(objSanitationOnDemandLibModel);
            }
            return SanitationOnDemandLibModelList;
        }
        #endregion Search
        #region Details
        public SanitationOnDemandLibModel PopulateSanitOnDemandLibData(long sanOnDemandMasterId)
        {
            SanOnDemandMaster sanOnDemandMaster = new SanOnDemandMaster()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SanOnDemandMasterId = sanOnDemandMasterId
            };
            SanOnDemandMaster sanOnDemandMasterDetails = new SanOnDemandMaster();
            sanOnDemandMaster.RetrieveBy_SanOnDemandMasterId(userData.DatabaseKey);
            SanitationOnDemandLibModel sanitationOnDemandLibModel = new SanitationOnDemandLibModel();
            sanitationOnDemandLibModel.ClientLookUpId = sanOnDemandMaster.ClientLookUpId;
            sanitationOnDemandLibModel.Description = sanOnDemandMaster.Description;
            sanitationOnDemandLibModel.Type = sanOnDemandMaster.Type;
            sanitationOnDemandLibModel.InactiveFlag = sanOnDemandMaster.InactiveFlag;
            sanitationOnDemandLibModel.CreateDate = sanOnDemandMaster.CreateDate;
            sanitationOnDemandLibModel.UpdateIndex = sanOnDemandMaster.UpdateIndex;
            return sanitationOnDemandLibModel;
        }
        #endregion Details
        #region Add Edit Sanitation
        public SanOnDemandMaster AddSanit(SanitationOnDemandLibModel objSanitationOnDemandLibModel)
        {
            SanOnDemandMaster sanOnDemandMaster = new SanOnDemandMaster();
            sanOnDemandMaster.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            sanOnDemandMaster.ClientLookUpId = objSanitationOnDemandLibModel.ClientLookUpId;
            sanOnDemandMaster.Description = objSanitationOnDemandLibModel.Description;
            sanOnDemandMaster.Type = objSanitationOnDemandLibModel.Type;
            sanOnDemandMaster.InactiveFlag = objSanitationOnDemandLibModel.InactiveFlag;
            sanOnDemandMaster.CreateByPKForeignKeys(this.userData.DatabaseKey);
            return sanOnDemandMaster;
        }
        public SanOnDemandMaster EditSanit(SanitationOnDemandLibModel objSanitationOnDemandLibModel)
        {
            SanOnDemandMaster sanOnDemandMaster = new SanOnDemandMaster()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SanOnDemandMasterId = objSanitationOnDemandLibModel.SanOnDemandMasterId
            };
            sanOnDemandMaster.RetrieveBy_SanOnDemandMasterId(userData.DatabaseKey);
            sanOnDemandMaster.Description = objSanitationOnDemandLibModel.Description ?? string.Empty;
            sanOnDemandMaster.Type = objSanitationOnDemandLibModel.Type ?? "";
            sanOnDemandMaster.InactiveFlag = objSanitationOnDemandLibModel.InactiveFlag;
            sanOnDemandMaster.UpdateIndex = objSanitationOnDemandLibModel.UpdateIndex;
            sanOnDemandMaster.UpdateBy_SanOnDemandMasterId(userData.DatabaseKey);
            return sanOnDemandMaster;
        }
        #endregion add edit Sanitation
        #region  Task
        public List<SanitOnDemandLibTaskModel> PopulateTask(long sanOnDemandMasterId)
        {
            List<SanitOnDemandLibTaskModel> SanitOnDemandLibTaskModelList = new List<SanitOnDemandLibTaskModel>();
            SanitOnDemandLibTaskModel objSanitOnDemandLibTaskModel;
            if (sanOnDemandMasterId > -1)
            {
                if (sanOnDemandMasterId > -1 && userData != null)
                {
                    SanOnDemandMasterTask task = new SanOnDemandMasterTask()
                    {
                        SanOnDemandMasterId = sanOnDemandMasterId 
                    };
                    List<SanOnDemandMasterTask> SanOnDemandMasterTaskList = task.SanOnDemandMasterTask_RetrieveAllBy_SanOnDemandMasterId(userData.DatabaseKey);
                    foreach (var p in SanOnDemandMasterTaskList)
                    {
                        objSanitOnDemandLibTaskModel = new SanitOnDemandLibTaskModel();
                        objSanitOnDemandLibTaskModel.SanOnDemandMasterTaskId = p.SanOnDemandMasterTaskId;
                        objSanitOnDemandLibTaskModel.TaskId = p.TaskId; 
                        objSanitOnDemandLibTaskModel.Description = p.Description;
                        objSanitOnDemandLibTaskModel.Del = p.Del;
                        objSanitOnDemandLibTaskModel.UpdateIndex = p.UpdateIndex;
                        SanitOnDemandLibTaskModelList.Add(objSanitOnDemandLibTaskModel);
                    }
                }
            }
            return SanitOnDemandLibTaskModelList;
        }
        public SanOnDemandMasterTask AddSanitTask(SanitOnDemandLibTaskModel sanitOnDemandLibTaskModel)
        {
            SanOnDemandMasterTask task = new SanOnDemandMasterTask();
            task.SanOnDemandMasterId = sanitOnDemandLibTaskModel.SanOnDemandMasterId;
            task.ClientId = userData.DatabaseKey.Personnel.ClientId;
            task.TaskId = sanitOnDemandLibTaskModel.TaskId;
            task.Description = sanitOnDemandLibTaskModel.Description;
            task.Create(userData.DatabaseKey);
            return task;
        }
        public SanOnDemandMasterTask EditSanitTask(SanitOnDemandLibTaskModel sanitOnDemandLibTaskModel)
        {
            SanOnDemandMasterTask task = new SanOnDemandMasterTask()
            {
                SanOnDemandMasterTaskId = sanitOnDemandLibTaskModel.SanOnDemandMasterTaskId
            };
            task.SanOnDemandMasterId = sanitOnDemandLibTaskModel.SanOnDemandMasterId;
            task.Retrieve(userData.DatabaseKey);
            task.ClientId = userData.DatabaseKey.Personnel.ClientId;
            task.TaskId = sanitOnDemandLibTaskModel.TaskId;
            task.Description = sanitOnDemandLibTaskModel.Description;
            task.Update(userData.DatabaseKey);
            return task;
        }
        public bool DeleteSanitTask(long sanOnDemandMasterTaskId)
        {
            try
            {
                SanOnDemandMasterTask tsk = new SanOnDemandMasterTask()
                {
                    SanOnDemandMasterTaskId = sanOnDemandMasterTaskId 
                };
                tsk.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion  Task
    }
}