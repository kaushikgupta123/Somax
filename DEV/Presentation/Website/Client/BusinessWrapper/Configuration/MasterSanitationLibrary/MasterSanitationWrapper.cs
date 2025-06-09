using Client.Models.Configuration.MasterSanitationLibrary;
using DataContracts;
using System;
using System.Collections.Generic;
namespace Client.BusinessWrapper.Configuration.MasterSanitationLibrary
{
    public class MasterSanitationWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public MasterSanitationWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region SanLibrary Populate Add Edit
        internal List<MasterSanitationModel> RetrieveAllBySiteId()
        {
            List<MasterSanitationModel> mSanLibraryList = new List<MasterSanitationModel>();
            MasterSanitationModel objSanLibraryModel;
            MasterSanLibrary masterLibrary = new MasterSanLibrary()
            {
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            List<MasterSanLibrary> masterLibraryList = masterLibrary.RetrieveAllCustom(this.userData.DatabaseKey);
            foreach (var item in masterLibraryList)
            {
                objSanLibraryModel = new MasterSanitationModel();
                objSanLibraryModel.MasterSanLibraryId = item.MasterSanLibraryId;
                objSanLibraryModel.ClientLookUpId = item.ClientLookUpId;
                objSanLibraryModel.Description = item.Description;
                objSanLibraryModel.JobDuration = item.JobDuration;
                objSanLibraryModel.FrequencyType = item.FrequencyType;
                objSanLibraryModel.Frequency = item.Frequency;
                objSanLibraryModel.JobDuration = item.JobDuration;
                objSanLibraryModel.CreateDate = item.CreateDate;
                mSanLibraryList.Add(objSanLibraryModel);
            }
            return mSanLibraryList;
        }
        internal MasterSanitationModel populateSaniLibraryDetails(long MasterSanLibraryId)
        {
            MasterSanitationModel objSanLibraryModel = new MasterSanitationModel();
            MasterSanLibrary masterSanLibrary = new MasterSanLibrary()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                MasterSanLibraryId = MasterSanLibraryId
            };
            masterSanLibrary.Retrieve(this.userData.DatabaseKey);
            objSanLibraryModel = PopulateModel(masterSanLibrary);
            return objSanLibraryModel;
        }
        internal MasterSanitationModel PopulateModel(MasterSanLibrary dbObj)
        {
            MasterSanitationModel oModel = new MasterSanitationModel();
            oModel.MasterSanLibraryId = dbObj.MasterSanLibraryId;
            oModel.ClientLookUpId = dbObj.ClientLookUpId;
            oModel.Description = dbObj.Description;
            oModel.Frequency = dbObj.Frequency;
            oModel.FrequencyType = dbObj.FrequencyType;
            oModel.JobDuration = dbObj.JobDuration;
            oModel.ScheduleMethod = dbObj.ScheduleMethod;
            oModel.ScheduleType = dbObj.ScheduleType;
            oModel.InactiveFlag = dbObj.InactiveFlag;
            oModel.CreateDate = dbObj.CreateDate;
            return oModel;
        }
        public List<String> AddOrUpdateMasterSanitation(MasterSanitationVM objVM, ref string Mode, ref long masterSanLibraryId)
        {
            MasterSanLibrary masterLibrary = new MasterSanLibrary()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
            };
            if (objVM.masterSanitationModel.MasterSanLibraryId == 0)
            {
                Mode = "add";
                masterLibrary.ClientLookUpId = objVM.masterSanitationModel.ClientLookUpId.Trim();
                masterLibrary.Description = objVM.masterSanitationModel.Description;
                masterLibrary.ScheduleType = objVM.masterSanitationModel.ScheduleType;
                masterLibrary.ScheduleMethod = objVM.masterSanitationModel.ScheduleMethod;
                masterLibrary.FrequencyType = objVM.masterSanitationModel.FrequencyType;
                masterLibrary.Frequency = objVM.masterSanitationModel.Frequency ?? 0;
                masterLibrary.JobDuration = objVM.masterSanitationModel.JobDuration ?? 0;
                masterLibrary.ValidateByClientLookupId(this.userData.DatabaseKey);
                if(masterLibrary.ErrorMessages.Count == 0)
                {
                    masterLibrary.Create(this.userData.DatabaseKey);
                    masterSanLibraryId = masterLibrary.MasterSanLibraryId;
                }
                else
                {
                    return masterLibrary.ErrorMessages;
                }
        
            }
            else
            {
                Mode = "update";
                masterLibrary.MasterSanLibraryId = objVM.masterSanitationModel.MasterSanLibraryId;
                masterLibrary.Retrieve(this.userData.DatabaseKey);
                masterLibrary.ClientLookUpId = objVM.masterSanitationModel.ClientLookUpId;
                masterLibrary.Description = objVM.masterSanitationModel.Description ?? String.Empty;
                masterLibrary.ScheduleType = objVM.masterSanitationModel.ScheduleType ?? string.Empty;
                masterLibrary.ScheduleMethod = objVM.masterSanitationModel.ScheduleMethod ?? string.Empty;
                masterLibrary.FrequencyType = objVM.masterSanitationModel.FrequencyType ?? string.Empty;
                masterLibrary.Frequency = objVM.masterSanitationModel.Frequency ?? 0;
                masterLibrary.InactiveFlag = objVM.masterSanitationModel.InactiveFlag;
                masterSanLibraryId = masterLibrary.MasterSanLibraryId;
                masterLibrary.Update(this.userData.DatabaseKey);
            }
            return masterLibrary.ErrorMessages;
        }
        #endregion
        #region Task
        internal List<MasterSanLibraryTask> PopulateTasks(long MasterSanLibraryId)
        {
            MasterSanLibraryTask task = new MasterSanLibraryTask()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                MasterSanLibraryId = MasterSanLibraryId,
            };
            List<MasterSanLibraryTask> taskList = task.RetrieveAllTaskByPrevMaintLibraryId(userData.DatabaseKey);
            return taskList;
        }
        internal bool DeleteTask(long MasterSanLibraryTaskId)
        {
            try
            {
                MasterSanLibraryTask task = new MasterSanLibraryTask()
                {
                    MasterSanLibraryTaskId = MasterSanLibraryTaskId
                };
                task.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<String> AddOrUpdateTask(MasterSanitationVM objVM, ref string Mode)
        {
            MasterSanLibraryTask task = new MasterSanLibraryTask()
            {
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            if (objVM.taskModel.MasterSanLibraryTaskId == null)
            {
                Mode = "add";
                task.TaskId = objVM.taskModel.TaskId;
                task.Description = objVM.taskModel.Description;
                task.MasterSanLibraryId = objVM.taskModel.MasterSanLibraryId;
                task.Create(this.userData.DatabaseKey);
            }
            else
            {
                Mode = "update";
                task.MasterSanLibraryTaskId = objVM.taskModel.MasterSanLibraryTaskId ?? 0;
                task.Retrieve(this.userData.DatabaseKey);
                task.TaskId = objVM.taskModel.TaskId.Trim();
                task.Description = objVM.taskModel.Description.Trim();
                task.Update(this.userData.DatabaseKey);
            }
            return task.ErrorMessages;
        }
        #endregion
    }

}