using Client.BusinessWrapper.Common;

using Client.Models.Configuration.PreventiveMaintenanceLibrary;
using DataContracts;
using System;
using System.Collections.Generic;
namespace Client.BusinessWrapper.Configuration.PreventiveMaintenanceLibrary
{
    public class PrevMaintLibraryWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public PrevMaintLibraryWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search
        public List<PreventiveMaintenanceLibraryModel> populatePrevMaintLibrary()
        {
            PreventiveMaintenanceLibraryModel PrevMaintLibModel;
            List<PreventiveMaintenanceLibraryModel> PrevMaintLibModelList = new List<PreventiveMaintenanceLibraryModel>();
            PrevMaintLibrary pmLibrary = new PrevMaintLibrary();
            pmLibrary.ClientId = userData.DatabaseKey.Client.ClientId;
            pmLibrary.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            List<PrevMaintLibrary> pmLibraryList = pmLibrary.RetrieveAllCustom(this.userData.DatabaseKey);
            foreach (var p in pmLibraryList)
            {
                PrevMaintLibModel = new PreventiveMaintenanceLibraryModel();
                PrevMaintLibModel.PrevMaintLibraryId = p.PrevMaintLibraryId;
                PrevMaintLibModel.ClientLookupId = p.ClientLookupId;
                PrevMaintLibModel.Description = p.Description;
                PrevMaintLibModel.JobDuration = Math.Round(p.JobDuration, 2);
                PrevMaintLibModel.FrequencyType = p.FrequencyType;
                PrevMaintLibModel.Frequency = p.Frequency;
                PrevMaintLibModel.CreateDate = p.CreateDate;
                PrevMaintLibModel.Type = p.Type;
                PrevMaintLibModel.ScheduleType = p.ScheduleType;
                PrevMaintLibModel.ScheduleMethod = p.ScheduleMethod;
                PrevMaintLibModel.InactiveFlag = p.InactiveFlag;
                PrevMaintLibModel.DownRequired = p.DownRequired;
                PrevMaintLibModelList.Add(PrevMaintLibModel);
            }
            return PrevMaintLibModelList;
        }
        #endregion
        #region Details
        public PreventiveMaintenanceLibraryModel populatePreventiveMaintenanceLibraryDetails(long PrevMaintLibraryId)
        {
            PreventiveMaintenanceLibraryModel objPreventiveMaintenanceLibrary = new PreventiveMaintenanceLibraryModel();
            PrevMaintLibrary prevMaintLib = new PrevMaintLibrary()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                PrevMaintLibraryId = PrevMaintLibraryId
            };
            prevMaintLib.Retrieve(userData.DatabaseKey);
            objPreventiveMaintenanceLibrary = initializeControls(prevMaintLib);
            return objPreventiveMaintenanceLibrary;
        }
        public PreventiveMaintenanceLibraryModel initializeControls(PrevMaintLibrary obj)
        {
            PreventiveMaintenanceLibraryModel objPreventiveMaintenanceLibrary = new PreventiveMaintenanceLibraryModel();
            objPreventiveMaintenanceLibrary.PrevMaintLibraryId = obj.PrevMaintLibraryId;
            objPreventiveMaintenanceLibrary.ClientLookupId = obj.ClientLookupId;
            objPreventiveMaintenanceLibrary.Description = obj.Description;
            objPreventiveMaintenanceLibrary.JobDuration = obj.JobDuration;
            objPreventiveMaintenanceLibrary.FrequencyType = obj.FrequencyType;
            objPreventiveMaintenanceLibrary.Frequency = obj.Frequency;
            objPreventiveMaintenanceLibrary.CreateDate = obj.CreateDate;

            objPreventiveMaintenanceLibrary.Type = obj.Type;
            objPreventiveMaintenanceLibrary.ScheduleType = obj.ScheduleType;
            objPreventiveMaintenanceLibrary.ScheduleMethod = obj.ScheduleMethod;
            objPreventiveMaintenanceLibrary.InactiveFlag = obj.InactiveFlag;
            objPreventiveMaintenanceLibrary.DownRequired = obj.DownRequired;
            return objPreventiveMaintenanceLibrary;
        }
        #endregion
        #region Add-Edit Preventive Maintenance Library
        public PrevMaintLibrary AddPrevMaintLibrary(PreventiveMaintenanceLibraryModel _PrevMaintLibraryModel)
        {
            PrevMaintLibrary prevMaintLib = new PrevMaintLibrary();
            prevMaintLib.ClientId = userData.DatabaseKey.Personnel.ClientId;
            // SOM-1701
            // PM Library is a CLIENT Level entitity.  Site should not be in the table
            // Set it to zero
            prevMaintLib.SiteId = 0;//userData.DatabaseKey.Personnel.SiteId;
            prevMaintLib.ClientLookupId = _PrevMaintLibraryModel.ClientLookupId.Trim();
            prevMaintLib.Description = _PrevMaintLibraryModel.Description;
            prevMaintLib.Type = _PrevMaintLibraryModel.Type;
            prevMaintLib.ScheduleType = _PrevMaintLibraryModel.ScheduleType;
            prevMaintLib.ScheduleMethod = _PrevMaintLibraryModel.ScheduleMethod;
            prevMaintLib.FrequencyType = _PrevMaintLibraryModel.FrequencyType;
            prevMaintLib.Frequency = Convert.ToString(_PrevMaintLibraryModel.Frequency) == "" ? 0 : Convert.ToInt32(_PrevMaintLibraryModel.Frequency);
            prevMaintLib.JobDuration = Convert.ToString(_PrevMaintLibraryModel.JobDuration) == "" ? 0 : Convert.ToDecimal(_PrevMaintLibraryModel.JobDuration);
            prevMaintLib.InactiveFlag = _PrevMaintLibraryModel.InactiveFlag;
            prevMaintLib.DownRequired = _PrevMaintLibraryModel.DownRequired;
            prevMaintLib.ValidateByClientLookupId(userData.DatabaseKey);
            if (prevMaintLib.ErrorMessages != null && prevMaintLib.ErrorMessages.Count == 0)
            {
                prevMaintLib.Create(userData.DatabaseKey);
            }
            return prevMaintLib;
        }
        public PrevMaintLibrary EditPrevMaintLibrary(PreventiveMaintenanceLibraryModel _PrevMaintLibraryModel)
        {
            PrevMaintLibrary prevMaintLib = new PrevMaintLibrary()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PrevMaintLibraryId = _PrevMaintLibraryModel.PrevMaintLibraryId
            };
            prevMaintLib.Retrieve(userData.DatabaseKey);
            prevMaintLib.ClientId = userData.DatabaseKey.Personnel.ClientId;
            // SOM-1701
            // PM Library is a CLIENT Level entitity.  Site should not be in the table
            // Set it to zero
            prevMaintLib.SiteId = 0;// userData.DatabaseKey.Personnel.SiteId;
            //prevMaintLib.ClientLookupId = _PrevMaintLibraryModel.ClientLookupId;
            prevMaintLib.Description = _PrevMaintLibraryModel.Description ?? string.Empty;
            prevMaintLib.Type = _PrevMaintLibraryModel.Type ?? string.Empty;
            prevMaintLib.ScheduleType = _PrevMaintLibraryModel.ScheduleType ?? string.Empty;
            prevMaintLib.ScheduleMethod = _PrevMaintLibraryModel.ScheduleMethod ?? string.Empty;
            prevMaintLib.FrequencyType = _PrevMaintLibraryModel.FrequencyType ?? string.Empty;
            prevMaintLib.Frequency = Convert.ToString(_PrevMaintLibraryModel.Frequency) == "" ? 0 : Convert.ToInt32(_PrevMaintLibraryModel.Frequency);
            prevMaintLib.InactiveFlag = _PrevMaintLibraryModel.InactiveFlag;
            prevMaintLib.DownRequired = _PrevMaintLibraryModel.DownRequired;
            prevMaintLib.JobDuration = Convert.ToString(_PrevMaintLibraryModel.JobDuration) == "" ? 0 : Convert.ToDecimal(_PrevMaintLibraryModel.JobDuration);
            prevMaintLib.Update(userData.DatabaseKey);
            return prevMaintLib;
        }
        #endregion

        #region Action

        public List<String> ChangePMLibId(ChangePreventiveLibraryIDModel _ChangePMLibIDModel)
        {
            PrevMaintLibraryVM objPMLibVM = new PrevMaintLibraryVM();            
            List<string> PMsg = new List<string>();
            long _pvId = _ChangePMLibIDModel.PrevMaintLibraryId;
            if (_pvId > 0)
            {
                PrevMaintLibrary prevmaintLib = new PrevMaintLibrary();
                prevmaintLib.PrevMaintLibraryId = _pvId;
                prevmaintLib.ClientLookupId = _ChangePMLibIDModel.ClientLookupId;
                prevmaintLib.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                prevmaintLib.ValidateChangeLookupId(userData.DatabaseKey);
                if (prevmaintLib.ErrorMessages.Count == 0)
                {
                    prevmaintLib.Retrieve(userData.DatabaseKey);
                    prevmaintLib.PrevMaintLibraryId = _ChangePMLibIDModel.PrevMaintLibraryId;
                    prevmaintLib.ClientLookupId = _ChangePMLibIDModel.ClientLookupId;
                    prevmaintLib.UpdateIndex = prevmaintLib.UpdateIndex;
                    prevmaintLib.UpdateForLibClientLookupId(userData.DatabaseKey);
                }
                else
                {
                    PMsg = prevmaintLib.ErrorMessages;
                }
            }
            return PMsg;
        }
        #endregion
        #region Add-Edit Task
        public List<TaskModel> PopulateTasks(long PrevMaintLibraryId)
        {
            TaskModel PrevMaintLibTaskModel;
            List<TaskModel> PrevMaintLibTaskModelList = new List<TaskModel>();
            PrevMaintLibrary pmLibrary = new PrevMaintLibrary();
            PrevMaintLibraryTask task = new PrevMaintLibraryTask()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PrevMaintLibraryId = PrevMaintLibraryId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName
            };
            List<PrevMaintLibraryTask> taskList = task.RetrieveAllTaskByPrevMaintLibraryId(userData.DatabaseKey);
            foreach (var p in taskList)
            {
                PrevMaintLibTaskModel = new TaskModel();
                PrevMaintLibTaskModel.PrevMaintLibraryTaskId = p.PrevMaintLibraryTaskId;
                PrevMaintLibTaskModel.PrevMaintLibraryId = p.PrevMaintLibraryId;
                PrevMaintLibTaskModel.Description = p.Description;
                PrevMaintLibTaskModel.TaskId = p.TaskId;
                PrevMaintLibTaskModelList.Add(PrevMaintLibTaskModel);
            }
            return PrevMaintLibTaskModelList;
        }
        internal bool DeleteTask(long PrevMaintLibraryTaskId)
        {
            try
            {
                PrevMaintLibraryTask tsk = new PrevMaintLibraryTask()
                {
                    PrevMaintLibraryTaskId = PrevMaintLibraryTaskId
                };
                tsk.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<String> AddOrUpdateTask(PrevMaintLibraryVM objVM, ref string Mode)
        {
            PrevMaintLibraryTask task = new PrevMaintLibraryTask();
            if (objVM.taskModel.PrevMaintLibraryTaskId == null)
            {
                Mode = "add";
                task.ClientId = userData.DatabaseKey.Personnel.ClientId;
                task.TaskId = objVM.taskModel.TaskId == null ? "" : objVM.taskModel.TaskId.Trim();
                task.Description = objVM.taskModel.Description == null ? "" : objVM.taskModel.Description.Trim();
                task.PrevMaintLibraryId = objVM.taskModel.PrevMaintLibraryId;
                task.Create(this.userData.DatabaseKey);
            }
            else
            {
                Mode = "update";
                task.PrevMaintLibraryTaskId = objVM.taskModel.PrevMaintLibraryTaskId ?? 0;
                task.Retrieve(this.userData.DatabaseKey);
                task.ClientId = userData.DatabaseKey.Personnel.ClientId;
                task.TaskId = objVM.taskModel.TaskId == null ? "" : objVM.taskModel.TaskId.Trim();
                task.Description = objVM.taskModel.Description == null ? "" : objVM.taskModel.Description.Trim();
                task.Update(this.userData.DatabaseKey);
            }
            return task.ErrorMessages;
        }
        #endregion
    }
}