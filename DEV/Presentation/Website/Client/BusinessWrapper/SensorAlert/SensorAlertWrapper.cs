using Client.Models.SensorAlert;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper.SensorAlert
{
    public class SensorAlertWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public SensorAlertWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<SensorAlertModel> GetSensorAlertDetails()
        {
            SensorAlertProcedure equipment = new SensorAlertProcedure();
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            List<SensorAlertProcedure> SensorAlertProcedureList = equipment.RetrieveAllForSensorAlertData(this.userData.DatabaseKey, userData.Site.TimeZone);
            List<SensorAlertModel> sensorList = new List<SensorAlertModel>();
            SensorAlertModel objSensorAlertModel;
            foreach (var v in SensorAlertProcedureList)
            {
                objSensorAlertModel = new SensorAlertModel();
                objSensorAlertModel.SensorAlertProcedureId = v.SensorAlertProcedureId;
                objSensorAlertModel.ClientLookUpId = v.ClientLookUpId;
                objSensorAlertModel.Description = v.Description;
                objSensorAlertModel.Type = v.Type;
                objSensorAlertModel.CreateDate = v.CreateDate;
                objSensorAlertModel.InactiveFlag = v.InactiveFlag;
                objSensorAlertModel.UpdateIndex = v.UpdateIndex;
                sensorList.Add(objSensorAlertModel);
            }
            return sensorList;
        }
        #endregion Search

        #region Details
        public SensorAlertModel GetSensorAlertData(long objectId)
        {
            SensorAlertProcedure sensorAlertProcedure = new SensorAlertProcedure()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SensorAlertProcedureId = objectId
            };
            sensorAlertProcedure.Retrieve(userData.DatabaseKey);
            SensorAlertModel objSensorAlertModel = new SensorAlertModel();
            objSensorAlertModel.SensorAlertProcedureId = sensorAlertProcedure.SensorAlertProcedureId;
            objSensorAlertModel.ClientLookUpId = sensorAlertProcedure.ClientLookUpId;
            objSensorAlertModel.Description = sensorAlertProcedure.Description;
            objSensorAlertModel.Type = sensorAlertProcedure.Type;
            objSensorAlertModel.CreateDate = sensorAlertProcedure.CreateDate;
            objSensorAlertModel.InactiveFlag = sensorAlertProcedure.InactiveFlag;
            objSensorAlertModel.UpdateIndex = sensorAlertProcedure.UpdateIndex;
            return objSensorAlertModel;
        }
        public SensorAlertProcedure AddSensorAlert(SensorAlertModel sensorAlertModel)
        {
            SensorAlertProcedure sensorAlertProcedure = new SensorAlertProcedure
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };
            sensorAlertProcedure.ClientLookUpId = sensorAlertModel.ClientLookUpId;
            sensorAlertProcedure.Description = sensorAlertModel.Description;
            sensorAlertProcedure.Type = sensorAlertModel.Type;
            sensorAlertProcedure.InactiveFlag = sensorAlertModel.InactiveFlag;
            sensorAlertProcedure.Add_SensorAlertProcedure(this.userData.DatabaseKey);
            return sensorAlertProcedure;
        }
        public SensorAlertProcedure EditSensorAlert(SensorAlertModel sensorAlertModel)
        {
            SensorAlertProcedure SA = new SensorAlertProcedure()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                SensorAlertProcedureId = sensorAlertModel.SensorAlertProcedureId 
            };
            SA.Retrieve(userData.DatabaseKey);
            SA.Description = sensorAlertModel.Description ?? "";
            SA.Type = sensorAlertModel.Type ?? "";
            SA.InactiveFlag = sensorAlertModel.InactiveFlag;
            SA.Update(userData.DatabaseKey);
            return SA;
        }
        #endregion Details

        #region Task
        
        public List<SensorAlertTaskModel> PopulateSaTask(long sensorAlertProcedureId)
        {
            List<SensorAlertTaskModel> SensorAlertTaskModelList = new List<SensorAlertTaskModel>();
            SensorAlertTaskModel objSensorAlertTaskModel;
           SensorAlertProcedureTask task = new SensorAlertProcedureTask();
            task.ClientId = userData.DatabaseKey.Client.ClientId;
            task.SensorAlertProcedureTaskId = sensorAlertProcedureId; // ObjectId;
            task.CallerUserName = userData.DatabaseKey.Client.CallerUserName;
            List<SensorAlertProcedureTask> taskList = task.SensorAlertProcedureTask_RetrieveByProcedureID(userData.DatabaseKey, sensorAlertProcedureId);
            foreach(var v in taskList)
            {
                objSensorAlertTaskModel = new SensorAlertTaskModel();
                objSensorAlertTaskModel.SensorAlertProcedureId = v.SensorAlertProcedureId;
                objSensorAlertTaskModel.SensorAlertProcedureTaskId = v.SensorAlertProcedureTaskId;
                objSensorAlertTaskModel.TaskId = v.TaskId;
                objSensorAlertTaskModel.Description = v.Description;
                objSensorAlertTaskModel.UpdateIndex = v.UpdateIndex;
                SensorAlertTaskModelList.Add(objSensorAlertTaskModel);
            }
            return(SensorAlertTaskModelList);
        }
        
        public SensorAlertProcedureTask AddSaTask(SensorAlertTaskModel sensorAlertTaskModel)
        {
            SensorAlertProcedureTask task = new SensorAlertProcedureTask();
            task.SensorAlertProcedureId = sensorAlertTaskModel.SensorAlertProcedureId; 
            task.ClientId = userData.DatabaseKey.Personnel.ClientId;
            task.TaskId = sensorAlertTaskModel.TaskId; 
            task.Description = sensorAlertTaskModel.Description; 
            task.Create(userData.DatabaseKey);
            return task;
        }
        public SensorAlertProcedureTask EditSaTask(SensorAlertTaskModel sensorAlertTaskModel)
        {
            SensorAlertProcedureTask task = new SensorAlertProcedureTask()
            {
                SensorAlertProcedureTaskId = sensorAlertTaskModel.SensorAlertProcedureTaskId 
            };
            task.SensorAlertProcedureId = sensorAlertTaskModel.SensorAlertProcedureId; 
            task.Retrieve(userData.DatabaseKey);
            task.ClientId = userData.DatabaseKey.Personnel.ClientId;
            task.Description = sensorAlertTaskModel.Description; 
            task.Update(userData.DatabaseKey);
            return task;
        }
        public bool DeleteSaTask(long sensorAlertProcedureTaskId)
        {
            try
            {
                SensorAlertProcedureTask tsk = new SensorAlertProcedureTask()
                {
                    SensorAlertProcedureTaskId = sensorAlertProcedureTaskId
                };
                tsk.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Task
    }

}