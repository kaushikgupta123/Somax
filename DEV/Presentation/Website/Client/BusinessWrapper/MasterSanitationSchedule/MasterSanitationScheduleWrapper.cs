using Client.BusinessWrapper.Common;
using Client.Models.MasterSanitationSchedule;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Constants;

namespace Client.BusinessWrapper.MasterSanitationSchedule
{
    public class MasterSanitationScheduleWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData _userData;

        public MasterSanitationScheduleWrapper(UserData userData) : base(userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region  Sanitation 

        #region Search
        public List<MasterSanitationScheduleModel> PopulateSanitShedule()
        {
            List<SanitationMaster> GridSource = new List<SanitationMaster>();
            SanitationMaster SanitationMasterDataContractObject = new SanitationMaster();
            SanitationMasterDataContractObject.ClientId = _dbKey.Client.ClientId;
            SanitationMasterDataContractObject.SiteId = _userData.Site.SiteId;
            GridSource = SanitationMasterDataContractObject.RetrieveToSearchCriteria(_dbKey);
            MasterSanitationScheduleModel objMasterSanitationScheduleModel;
            List<MasterSanitationScheduleModel> MasterSanitationScheduleModelList = new List<MasterSanitationScheduleModel>();
            foreach (var v in GridSource)
            {
                objMasterSanitationScheduleModel = new MasterSanitationScheduleModel();
                objMasterSanitationScheduleModel.SanitationMasterId = v.SanitationMasterId;
                objMasterSanitationScheduleModel.Description = v.Description;
                objMasterSanitationScheduleModel.ChargeToClientLookupId = v.ChargeToClientLookupId;
                objMasterSanitationScheduleModel.Frequency = v.Frequency;
                objMasterSanitationScheduleModel.Assignto_PersonnelId = v.Assignto_PersonnelId;
                objMasterSanitationScheduleModel.Assigned = v.Assigned;
                objMasterSanitationScheduleModel.Shift = v.Shift;
                objMasterSanitationScheduleModel.ScheduledDuration = v.ScheduledDuration;
                objMasterSanitationScheduleModel.NextDue = v.NextDue;
                objMasterSanitationScheduleModel.InactiveFlag = v.InactiveFlag;
                MasterSanitationScheduleModelList.Add(objMasterSanitationScheduleModel);
            }
            return MasterSanitationScheduleModelList;
        }
        #endregion Search

        #region Add-Edit-Delete Master Sanitation Schedule
        public SanitationMaster AddMSSchedule(MasterSanitationScheduleModel _MasterSanitationScheduleModel)
        {
            MasterSanitationScheduleModel objMasterSanitationScheduleModel = new MasterSanitationScheduleModel();
            SanitationMaster objSanitationMaster = new SanitationMaster();
            objSanitationMaster.ClientId = _dbKey.Personnel.ClientId;
            objSanitationMaster.SiteId = _dbKey.Personnel.SiteId;
            objSanitationMaster.DepartmentId = _dbKey.Personnel.DepartmentId;
            objSanitationMaster.AreaId = _dbKey.Personnel.AreaId;
            objSanitationMaster.StoreroomId = _dbKey.Personnel.StoreroomId;
            objSanitationMaster.ChargeType = _MasterSanitationScheduleModel.ChargeType == "PlantLocation" ? _MasterSanitationScheduleModel.ChargeType : ChargeType.Equipment;
            objSanitationMaster.ChargeToClientLookupId = _MasterSanitationScheduleModel.ChargeToClientLookupId;
            objSanitationMaster.NextDue = _MasterSanitationScheduleModel.NextDue;
            objSanitationMaster.ScheduledType = _MasterSanitationScheduleModel.ScheduledType;
            objSanitationMaster.Assignto_PersonnelId = _MasterSanitationScheduleModel.Assignto_PersonnelId ?? 0;
            objSanitationMaster.Shift = _MasterSanitationScheduleModel.Shift;
            objSanitationMaster.ScheduledDuration = _MasterSanitationScheduleModel.ScheduledDuration??0;
            objSanitationMaster.OnDemandGroup = _MasterSanitationScheduleModel.OnDemandGroup;
            objSanitationMaster.Description = _MasterSanitationScheduleModel.Description;
            objSanitationMaster.Frequency = _MasterSanitationScheduleModel.Frequency ?? 0;
            objSanitationMaster.PlantLocationId = _MasterSanitationScheduleModel.ChargeType == "PlantLocation" ? Convert.ToInt64(_MasterSanitationScheduleModel.PlantLocationId) : 0;
            objSanitationMaster.SanitationMaster_CreateByFK(_dbKey);
            return objSanitationMaster;
        }

        public SanitationMaster EditMSSchedule(MasterSanitationScheduleModel _MasterSanitationScheduleModel)
        {
            string ExclusionDays = "0000000";
            MasterSanitationScheduleModel objMasterSanitationScheduleModel = new MasterSanitationScheduleModel();
            DataContracts.SanitationMaster objSanitationMaster = new DataContracts.SanitationMaster()
            {
                ClientId = _dbKey.Client.ClientId,
                SanitationMasterId = _MasterSanitationScheduleModel.SanitationMasterId
            };
            objSanitationMaster.UpdateIndex = _MasterSanitationScheduleModel.UpdateIndex;
            objSanitationMaster.ClientId = _dbKey.Personnel.ClientId;
            objSanitationMaster.SiteId = _dbKey.Personnel.SiteId;
            //Added V2-609
            objSanitationMaster.DepartmentId = _dbKey.Personnel.DepartmentId;
            objSanitationMaster.AreaId = _dbKey.Personnel.AreaId;
            objSanitationMaster.StoreroomId = _dbKey.Personnel.StoreroomId;
            
            objSanitationMaster.ChargeType = _MasterSanitationScheduleModel.ChargeType == "PlantLocation" ? _MasterSanitationScheduleModel.ChargeType : ChargeType.Equipment;
            objSanitationMaster.ChargeToClientLookupId = _MasterSanitationScheduleModel.ChargeToClientLookupId;
            objSanitationMaster.NextDue = _MasterSanitationScheduleModel.NextDue;
            objSanitationMaster.ScheduledType = _MasterSanitationScheduleModel.ScheduledType;
            objSanitationMaster.Assignto_PersonnelId = _MasterSanitationScheduleModel.Assignto_PersonnelId ?? 0;
            objSanitationMaster.Shift = _MasterSanitationScheduleModel.Shift;
            objSanitationMaster.ScheduledDuration = _MasterSanitationScheduleModel.ScheduledDuration??0;
            objSanitationMaster.OnDemandGroup = _MasterSanitationScheduleModel.OnDemandGroup;
            objSanitationMaster.Description = _MasterSanitationScheduleModel.Description;
            objSanitationMaster.Frequency = _MasterSanitationScheduleModel.Frequency ?? 0;
            objSanitationMaster.InactiveFlag = _MasterSanitationScheduleModel.InactiveFlag;
            objSanitationMaster.LastScheduled = _MasterSanitationScheduleModel.LastScheduled;
            objSanitationMaster.PlantLocationId = _MasterSanitationScheduleModel.ChargeType == "PlantLocation" ? Convert.ToInt64(_MasterSanitationScheduleModel.PlantLocationId) : 0;
            if (_MasterSanitationScheduleModel.ExclusionDaysString != null)
            {
                 ExclusionDays = GetExclusionDaysString(_MasterSanitationScheduleModel.ExclusionDaysString);
               
            }
            objSanitationMaster.ExclusionDays = ExclusionDays;
            objSanitationMaster.SanitationMaster_UpdateByFK(_dbKey);
            return objSanitationMaster;
        }

        public List<String> DeleteMSSchedule(long MSScheduleid)
        {
            List<string> EMsg = new List<string>();
            if (MSScheduleid > 0)
            {
                SanitationMaster master = new SanitationMaster();
                master.SanitationMasterId = MSScheduleid;
                master.ClientId = _dbKey.Personnel.ClientId;
                master.SanitationMaster_Delete(_dbKey);
                if (master.ErrorMessages != null && master.ErrorMessages.Count > 0)
                {
                    EMsg = master.ErrorMessages;
                }
            }
            return EMsg;
        }
        private string GetExclusionDaysString(string[] days)
        {
            var defaultExclusiondays = "0000000";
            char[] array = defaultExclusiondays.ToCharArray();
            if (days != null && days.Length > 0)
            {
                foreach (var day in days)
                {
                    array[Convert.ToInt32(day)] = '1';
                }
            }
            return new string(array);
        }
        private string[] GetExclusionDaysArray(string days)
        {
            var daysCharArray = days.ToCharArray();
            List<string> tmpList = new List<string>();
            for (int i = 0; i < daysCharArray.Count(); i++)
            {
                if (daysCharArray[i] == '1')
                {
                    tmpList.Add(i.ToString());
                }
            }
          
            return tmpList.ToArray();
        }
        #endregion

        #region Details
        public MasterSanitationScheduleModel MasterSanitDetails(long objectId, List<Models.DataModel> PersonnelLookUplist)
        {
            SanitationMaster sanitationMaster = new SanitationMaster()
            {
                SanitationMasterId = objectId
            };
            sanitationMaster.SanitationMaster_RetrieveByFK(_dbKey);
            MasterSanitationScheduleModel objMasterSanitationScheduleModel = new MasterSanitationScheduleModel();

            objMasterSanitationScheduleModel.Assignto_PersonnelId = sanitationMaster.Assignto_PersonnelId;
            objMasterSanitationScheduleModel.ChargeToClientLookupId = sanitationMaster.ChargeToClientLookupId;
            objMasterSanitationScheduleModel.ChargeToId = sanitationMaster.ChargeToId;
            objMasterSanitationScheduleModel.PlantLocationId = sanitationMaster.ChargeToId;
            objMasterSanitationScheduleModel.ChargeToName = sanitationMaster.ChargeToName;
            objMasterSanitationScheduleModel.ChargeType = sanitationMaster.ChargeType;
            objMasterSanitationScheduleModel.Description = sanitationMaster.Description;
            objMasterSanitationScheduleModel.Frequency = sanitationMaster.Frequency;
            objMasterSanitationScheduleModel.InactiveFlag = sanitationMaster.InactiveFlag;
            objMasterSanitationScheduleModel.ExclusionDays = sanitationMaster.ExclusionDays;
            objMasterSanitationScheduleModel.ExclusionDaysString = GetExclusionDaysArray(objMasterSanitationScheduleModel.ExclusionDays);
            if (sanitationMaster.LastScheduled != null && sanitationMaster.LastScheduled == default(DateTime))
            {
                objMasterSanitationScheduleModel.LastScheduled = null;
            }
            else
            {
                objMasterSanitationScheduleModel.LastScheduled = sanitationMaster.LastScheduled;
            }

            if (sanitationMaster.NextDue != null && sanitationMaster.NextDue == default(DateTime))
            {
                objMasterSanitationScheduleModel.NextDue = null;
            }
            else
            {
                objMasterSanitationScheduleModel.NextDue = sanitationMaster.NextDue;
            }
            objMasterSanitationScheduleModel.OnDemandGroup = sanitationMaster.OnDemandGroup;
            objMasterSanitationScheduleModel.SanitationMasterId = sanitationMaster.SanitationMasterId;
            objMasterSanitationScheduleModel.ScheduledDuration = sanitationMaster.ScheduledDuration;
            objMasterSanitationScheduleModel.ScheduledType = sanitationMaster.ScheduledType;
            objMasterSanitationScheduleModel.Shift = sanitationMaster.Shift;
            objMasterSanitationScheduleModel.UpdateIndex = sanitationMaster.UpdateIndex;



            if (PersonnelLookUplist != null)
            {
                objMasterSanitationScheduleModel.Assigned = PersonnelLookUplist.Where(x => x.AssignedTo_PersonnelId == objMasterSanitationScheduleModel.Assignto_PersonnelId).Select(x => x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast).FirstOrDefault();
            }
            return objMasterSanitationScheduleModel;
        }
        #endregion Details

        #region Task
        public List<MasterSanitTaskModel> PopulateTask(long objectId)
        {
            SanitationMasterTask task = new SanitationMasterTask();
            List<SanitationMasterTask> sanTaskList = task.SanitationMasterTaskRetrieveBySanitationMasterId(_dbKey, objectId);
            MasterSanitTaskModel objMasterSanitTaskModel;
            List<MasterSanitTaskModel> MasterSanitTaskModelList = new List<MasterSanitTaskModel>();
            foreach (var v in sanTaskList)
            {
                objMasterSanitTaskModel = new MasterSanitTaskModel();
                objMasterSanitTaskModel.OrderNumber = v.OrderNumber;
                objMasterSanitTaskModel.Description = v.Description;
                objMasterSanitTaskModel.PerformTime = v.PerformTime;
                objMasterSanitTaskModel.InactiveFlag = v.InactiveFlag;
                objMasterSanitTaskModel.UpdateIndex = v.UpdateIndex;
                objMasterSanitTaskModel.TaskId = v.TaskId;
                objMasterSanitTaskModel.SanitationMasterId = v.SanitationMasterId;
                objMasterSanitTaskModel.SanitationMasterTaskId = v.SanitationMasterTaskId;
                MasterSanitTaskModelList.Add(objMasterSanitTaskModel);
            }
            return MasterSanitTaskModelList;
        }

        public string CreateTaskNumber(long objectId)
        {
            int taskNumberCount = new SanitationMasterTask().SanitationMasterTaskRetrieveBySanitationMasterId(_dbKey, objectId).Count;
            taskNumberCount++;
            string tasknumber;
            if (taskNumberCount < 10)
                tasknumber = string.Concat("00" + taskNumberCount.ToString());
            else if (taskNumberCount < 100)
                tasknumber = string.Concat("0" + taskNumberCount.ToString());
            else
                tasknumber = taskNumberCount.ToString();
            return tasknumber;
        }
        public List<string> CreateMastSanitTask(MasterSanitTaskModel msTask)
        {
            SanitationMasterTask task = new SanitationMasterTask();
            task.SanitationMasterId = msTask.SanitationMasterId;
            task.ClientId = _dbKey.Personnel.ClientId;
            task.Description = msTask.Description ?? "";
            task.TaskId = msTask.TaskId;
            task.PerformTime = msTask.PerformTime ?? "";
            task.Create(_userData.DatabaseKey);
            return task.ErrorMessages;
        }

        public MasterSanitTaskModel RetrieveTaskForEdit(long objectId)
        {
            SanitationMasterTask task = new SanitationMasterTask()
            {
                SanitationMasterTaskId = objectId// e.Keys[0].ToString().ToLong()
            };

            task.SanitationMasterId = objectId;
            task.Retrieve(_dbKey);

            MasterSanitTaskModel objMasterSanitTaskModel = new MasterSanitTaskModel();

            objMasterSanitTaskModel.OrderNumber = task.OrderNumber;
            objMasterSanitTaskModel.Description = task.Description;
            objMasterSanitTaskModel.PerformTime = task.PerformTime;
            objMasterSanitTaskModel.InactiveFlag = task.InactiveFlag;
            objMasterSanitTaskModel.UpdateIndex = task.UpdateIndex;
            objMasterSanitTaskModel.TaskId = task.TaskId;
            objMasterSanitTaskModel.SanitationMasterId = task.SanitationMasterId;
            objMasterSanitTaskModel.SanitationMasterTaskId = task.SanitationMasterTaskId;
            return objMasterSanitTaskModel;
        }
        public List<string> UpdateMastSanitTask(MasterSanitTaskModel msTask)
        {
            SanitationMasterTask task = new SanitationMasterTask()
            {
                SanitationMasterTaskId = msTask.SanitationMasterTaskId
            };

            task.SanitationMasterId = msTask.SanitationMasterTaskId;// ObjectId;
            task.Retrieve(_dbKey);

            task.TaskId = msTask.TaskId;
            task.Description = msTask.Description ?? "";
            task.PerformTime = msTask.PerformTime ?? "";
            task.InactiveFlag = msTask.InactiveFlag;
            task.Update(_dbKey);
            return task.ErrorMessages;
        }

        public bool DeleteMastSanitTask(long objectId)
        {
            try
            {
                SanitationMasterTask tsk = new SanitationMasterTask()
                {
                    SanitationMasterTaskId = objectId
                };
                tsk.Delete(_dbKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion task

        #region Notes
        //public List<MasterSanitNotesModel> PopulateNotes(long objectId)
        //{
        //    Notes note = new Notes()
        //    {
        //        ObjectId = objectId,
        //        TableName = "SanitationMaster"
        //    };

        //    var NoteList = note.RetrieveByObjectId(_dbKey, _userData.Site.TimeZone);
        //    MasterSanitNotesModel objMasterSanitNotesModel;
        //    List<MasterSanitNotesModel> MasterSanitNotesModelList = new List<MasterSanitNotesModel>();
        //    foreach (var v in NoteList)
        //    {
        //        objMasterSanitNotesModel = new MasterSanitNotesModel();
        //        objMasterSanitNotesModel.NotesId = v.NotesId;
        //        objMasterSanitNotesModel.Subject = v.Subject;
        //        objMasterSanitNotesModel.Content = v.Content;
        //        objMasterSanitNotesModel.UpdateIndex = v.UpdateIndex;
        //        objMasterSanitNotesModel.OwnerName = v.OwnerName;
        //        if (v.ModifiedDate != null && v.ModifiedDate == default(DateTime))
        //        {
        //            objMasterSanitNotesModel.ModifiedDate = null;
        //        }
        //        else
        //        {
        //            objMasterSanitNotesModel.ModifiedDate = v.ModifiedDate;
        //        }
        //        MasterSanitNotesModelList.Add(objMasterSanitNotesModel);
        //    }
        //    return MasterSanitNotesModelList;
        //}
        #endregion

        #region Tool
        public List<MasterSanitationPlanningModel> PopulateTool(long objectId)
        {
            SanitationPlanning sanitationPlanning = new SanitationPlanning();
            var plnList = sanitationPlanning.SanitationPlanningRetrieveByMasterId(_dbKey, _dbKey.Client.ClientId, objectId, "Tool");
            MasterSanitationPlanningModel objMasterSanitationPlanningModel;
            List<MasterSanitationPlanningModel> MasterSanitationPlanningModelList = new List<MasterSanitationPlanningModel>();
            foreach (var v in plnList)
            {
                objMasterSanitationPlanningModel = new MasterSanitationPlanningModel();
                objMasterSanitationPlanningModel.Category = v.Category;
                objMasterSanitationPlanningModel.CategoryValue = v.CategoryValue;
                objMasterSanitationPlanningModel.Description = v.Description;
                objMasterSanitationPlanningModel.Instructions = v.Instructions;
                objMasterSanitationPlanningModel.Quantity = v.Quantity;
                objMasterSanitationPlanningModel.SanitationMasterId = v.SanitationMasterId;
                objMasterSanitationPlanningModel.SanitationPlanningId = v.SanitationPlanningId;
                objMasterSanitationPlanningModel.UnitCost = v.UnitCost;
                MasterSanitationPlanningModelList.Add(objMasterSanitationPlanningModel);
            }
            return MasterSanitationPlanningModelList;
        }
        internal List<string> AddTool(MasterSanitationPlanningModel masterSanitationPlanningModel)
        {
            SanitationPlanning sanitationPlanning = new SanitationPlanning();
            sanitationPlanning.ClientId = _userData.DatabaseKey.Client.ClientId;
            sanitationPlanning.SanitationMasterId = masterSanitationPlanningModel.SanitationMasterId;
            sanitationPlanning.Category = "Tool";
            sanitationPlanning.CategoryId = 0;
            sanitationPlanning.CategoryValue = masterSanitationPlanningModel.CategoryValue;

            sanitationPlanning.Description = masterSanitationPlanningModel.Description ?? string.Empty;
            sanitationPlanning.Dilution = string.Empty;
            sanitationPlanning.Instructions = masterSanitationPlanningModel.Instructions ?? string.Empty; ;
            sanitationPlanning.Quantity = masterSanitationPlanningModel.Quantity ?? 0;

            sanitationPlanning.Create(_userData.DatabaseKey);
            return sanitationPlanning.ErrorMessages;
        }
        public MasterSanitationPlanningModel GetTool(long SanitationMasterId, long SanitationPlanningId)
        {
            MasterSanitationPlanningModel objMasterSanitationPlanningModel = new MasterSanitationPlanningModel();
            SanitationPlanning sanitationPlanning = new SanitationPlanning() { SanitationPlanningId = SanitationPlanningId };
            sanitationPlanning.SanitationMasterId = SanitationMasterId;
            sanitationPlanning.Retrieve(_userData.DatabaseKey);

            objMasterSanitationPlanningModel.SanitationPlanningId = SanitationPlanningId;
            objMasterSanitationPlanningModel.SanitationMasterId = SanitationMasterId;
            objMasterSanitationPlanningModel.CategoryId = sanitationPlanning.CategoryId;
            objMasterSanitationPlanningModel.Category = sanitationPlanning.Category;
            objMasterSanitationPlanningModel.CategoryValue = sanitationPlanning.CategoryValue;
            objMasterSanitationPlanningModel.Instructions = sanitationPlanning.Instructions;
            objMasterSanitationPlanningModel.Quantity = sanitationPlanning.Quantity;

            return objMasterSanitationPlanningModel;
        }
        internal List<string> EditTool(MasterSanitationPlanningModel masterSanitationPlanningModel)
        {

            SanitationPlanning sanitationPlanning = new SanitationPlanning() { SanitationPlanningId = masterSanitationPlanningModel.SanitationPlanningId };
            sanitationPlanning.SanitationMasterId = masterSanitationPlanningModel.SanitationMasterId;
            sanitationPlanning.Retrieve(_userData.DatabaseKey);

            sanitationPlanning.CategoryValue = masterSanitationPlanningModel.CategoryValue;
            sanitationPlanning.Description = masterSanitationPlanningModel.Description ?? string.Empty;
            sanitationPlanning.Instructions = masterSanitationPlanningModel.Instructions ?? string.Empty;
            sanitationPlanning.Quantity = masterSanitationPlanningModel.Quantity ?? 0;
            sanitationPlanning.Update(_userData.DatabaseKey);
            return sanitationPlanning.ErrorMessages;
        }
        internal bool DeleteTools(long SanitationPlanningId)
        {
            try
            {
                SanitationPlanning sanitationPlanning = new SanitationPlanning() { SanitationPlanningId = SanitationPlanningId };
                sanitationPlanning.Delete(_userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Chemical
        public List<MasterSanitationPlanningModel> PopulateChemical(long objectId)
        {
            SanitationPlanning sanitationPlanning = new SanitationPlanning();
            var chmList = sanitationPlanning.SanitationPlanningRetrieveByMasterId(_dbKey, _dbKey.Client.ClientId, objectId, "Chemical");
            MasterSanitationPlanningModel objMasterSanitationPlanningModel;
            List<MasterSanitationPlanningModel> MasterSanitationPlanningModelList = new List<MasterSanitationPlanningModel>();
            foreach (var v in chmList)
            {
                objMasterSanitationPlanningModel = new MasterSanitationPlanningModel();
                objMasterSanitationPlanningModel.Category = v.Category;
                objMasterSanitationPlanningModel.CategoryValue = v.CategoryValue;
                objMasterSanitationPlanningModel.Description = v.Description;
                objMasterSanitationPlanningModel.Dilution = v.Dilution;
                objMasterSanitationPlanningModel.Instructions = v.Instructions;
                objMasterSanitationPlanningModel.Quantity = v.Quantity;
                objMasterSanitationPlanningModel.SanitationMasterId = v.SanitationMasterId;
                objMasterSanitationPlanningModel.SanitationPlanningId = v.SanitationPlanningId;
                objMasterSanitationPlanningModel.UnitCost = v.UnitCost;
                MasterSanitationPlanningModelList.Add(objMasterSanitationPlanningModel);
            }
            return MasterSanitationPlanningModelList;
        }
        internal List<string> AddChemical(MasterSanitationPlanningModel masterSanitationPlanningModel)
        {
            SanitationPlanning sanitationPlanning = new SanitationPlanning();
            sanitationPlanning.ClientId = _userData.DatabaseKey.Client.ClientId;
            sanitationPlanning.SanitationMasterId = masterSanitationPlanningModel.SanitationMasterId;
            sanitationPlanning.Category = "Chemical";
            sanitationPlanning.CategoryId = 0;

            string[] values = masterSanitationPlanningModel.CategoryValue.Split('|').Select(sValue => sValue.Trim()).ToArray();
            sanitationPlanning.CategoryValue = values[0] != string.Empty ? values[0] : "";
            sanitationPlanning.Description = values[1] != string.Empty ? values[1] : "";

            sanitationPlanning.Dilution = masterSanitationPlanningModel.Dilution;
            sanitationPlanning.Instructions = masterSanitationPlanningModel.Instructions;
            sanitationPlanning.Quantity = masterSanitationPlanningModel.Quantity ?? 0;

            sanitationPlanning.Create(_userData.DatabaseKey);
            return sanitationPlanning.ErrorMessages;

        }
        internal List<string> EditChemical(MasterSanitationPlanningModel masterSanitationPlanningModel)
        {

            SanitationPlanning sanitationPlanning = new SanitationPlanning() { SanitationPlanningId = masterSanitationPlanningModel.SanitationPlanningId };
            sanitationPlanning.SanitationMasterId = masterSanitationPlanningModel.SanitationMasterId;
            sanitationPlanning.Retrieve(_userData.DatabaseKey);

            string[] values = masterSanitationPlanningModel.CategoryValue.Split('|').Select(sValue => sValue.Trim()).ToArray();
            sanitationPlanning.CategoryValue = values[0] != string.Empty ? values[0] : "";
            sanitationPlanning.Description = values[1] != string.Empty ? values[1] : "";
            sanitationPlanning.Dilution = masterSanitationPlanningModel.Dilution ?? "";
            sanitationPlanning.Instructions = masterSanitationPlanningModel.Instructions ?? "";
            sanitationPlanning.Quantity = masterSanitationPlanningModel.Quantity ?? 0;
            sanitationPlanning.Update(_userData.DatabaseKey);
            return sanitationPlanning.ErrorMessages;
        }


        internal bool DeleteChemical(long SanitationPlanningId)
        {
            try
            {
                SanitationPlanning sanitationPlanning = new SanitationPlanning() { SanitationPlanningId = SanitationPlanningId };
                sanitationPlanning.Delete(_userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal MasterSanitationPlanningModel ChemicalRetriveById(long sanitationPlanningId)
        {
            MasterSanitationPlanningModel MasterSanPlanningModel = new MasterSanitationPlanningModel();
            SanitationPlanning sanitationPlanning = new SanitationPlanning();
            sanitationPlanning.SanitationPlanningId = sanitationPlanningId;
            sanitationPlanning.Retrieve(_userData.DatabaseKey);
            MasterSanPlanningModel.Category = sanitationPlanning.Category;
            MasterSanPlanningModel.CategoryValue = sanitationPlanning.CategoryValue + "|" + sanitationPlanning.Description;
            MasterSanPlanningModel.Instructions = sanitationPlanning.Instructions;
            MasterSanPlanningModel.Dilution = sanitationPlanning.Dilution;
            MasterSanPlanningModel.Quantity = sanitationPlanning.Quantity;
            MasterSanPlanningModel.SanitationMasterId = sanitationPlanning.SanitationMasterId;
            MasterSanPlanningModel.SanitationPlanningId = sanitationPlanning.SanitationPlanningId;
            return MasterSanPlanningModel;
        }
        #endregion

        #region LookUpList
        public List<DataContracts.LookupList> GetAllLookUpList()
        {
            List<DataContracts.LookupList> objLookUp = new Models.LookupList().RetrieveAll(_dbKey);
            return objLookUp;
        }
        #endregion LookUpList 
        #endregion Sanitation 



        #region Job Generation
        public List<string> OnProccess(SanitationJobGenerationModel obj, ref string TopMessage, ref int SanitationMasterCount, ref int SanitationJobCount, ref List<string> ListOfLocation)
        {

            DataContracts.SanitationJob sanitationJob = new SanitationJob()
            {
                CallerUserInfoId = this._userData.DatabaseKey.User.UserInfoId,
                CallerUserName = this._userData.DatabaseKey.UserName,
                ClientId = this._userData.DatabaseKey.Client.ClientId,
                SiteId = this._userData.DatabaseKey.Personnel.SiteId,
                PersonnelId = this._userData.DatabaseKey.Personnel.PersonnelId,
                Prefix = ""//this.UserData.ClientUIConfiguration.AutoGeneratedIdSettings.PurchaseRequest_AutoGeneratePrefix
            };
            if (obj.RadioButton == "Calendar")  // For calendar Radio button
            {
                sanitationJob.SanitationJob_GenerationReport(this._userData.DatabaseKey);
            }
            else   // For OnDemand RadioButton
            {
                //1 / 11 / 2019
                sanitationJob.OnDemandGroup = obj.OnDemandGroup;
                sanitationJob.ScheduledDate = obj.ScheduledDate;
                sanitationJob.SanitationJob_GenerationReportOnDemand(_userData.DatabaseKey);
            }
            TopMessage = sanitationJob.SanitationJobCount > 0 ? "jobs generated" : "no jobs generated";
            SanitationMasterCount = sanitationJob.SanitationMasterCount;
            SanitationJobCount = sanitationJob.SanitationJobCount;
            if (sanitationJob.SanitationJobCount > 0)
            {
                ListOfLocation = sanitationJob.SanitationJobList.Split(',').ToList();
                ListOfLocation.Remove("");
            }
            return sanitationJob.ErrorMessages;

        }


        public SanitationJobDetailsModel RetrieveBy_SanitationJobId(long ObjectId)
        {
            SanitationJobDetailsModel JobDetails = new SanitationJobDetailsModel();
            SanitationJob SanDetail = new SanitationJob()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                SiteId = _userData.DatabaseKey.User.DefaultSiteId,
                SanitationJobId = ObjectId
            };
            SanDetail.RetrieveByPKForeignKeys(this._userData.DatabaseKey);
            JobDetails = initializeDetailsControls(SanDetail);
            return JobDetails;
        }
        public SanitationJobDetailsModel initializeDetailsControls(SanitationJob obj)
        {
            SanitationJobDetailsModel objdetails = new SanitationJobDetailsModel();
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();

            objdetails.SanitationMasterId = obj.SanitationMasterId;
            objdetails.SanitationJobId = obj.SanitationJobId;
            objdetails.ClientLookupId = obj.ClientLookupId;
            objdetails.Status = obj?.Status ?? string.Empty;
            objdetails.Shift = obj.Shift;
            objdetails.Description = obj?.Description ?? string.Empty;
            objdetails.AssignedTo_PersonnelId = obj.AssignedTo_PersonnelId;

            objdetails.Assigned = (obj.AssignedTo_PersonnelId) > 0 ? obj.Assigned : "";
            objdetails.CreateBy_Name = obj?.CreateByName ?? string.Empty;
            objdetails.CreateBy = obj?.CreateBy ?? string.Empty;
            objdetails.ChargeToId_string = string.IsNullOrEmpty(obj.ChargeTo_ClientLookupId) ? "" : obj.ChargeTo_ClientLookupId;
            objdetails.ChargeTo_Name = obj?.ChargeTo_Name ?? string.Empty;

            objdetails.ChargeType = obj?.ChargeType ?? string.Empty;//hidden

            objdetails.ChargeTo_ClientLookupId = string.IsNullOrEmpty(obj.ChargeTo_ClientLookupId) ? "" : obj.ChargeTo_ClientLookupId;

            objdetails.ChargeToId = obj.ChargeToId;

            objdetails.ScheduledDuration = obj.ScheduledDuration;
            objdetails.ScheduledDate = obj?.ScheduledDate ?? DateTime.MinValue;
            objdetails.CompleteDate = obj?.CompleteDate ?? DateTime.MinValue;
            objdetails.CompleteBy = obj?.CompleteBy ?? string.Empty;
            objdetails.ActualDuration = obj.ActualDuration;
            objdetails.CompleteComments = obj?.CompleteComments ?? string.Empty;
            objdetails.PlantLocationId = obj.PlantLocationId;
            objdetails.CreateDate = obj?.CreateDate ?? DateTime.MinValue;
            objdetails.Status_Display = obj.Status_Display;
            return objdetails;
        }

        public List<DataContracts.SanitationJobTask> PopulateTaskPrint(long objectId)
        {
            List<DataContracts.SanitationJobTask> task = new List<DataContracts.SanitationJobTask>();

            DataContracts.SanitationJobTask sanitationJobTask = new DataContracts.SanitationJobTask()
            {
                ClientId = this._userData.DatabaseKey.Client.ClientId,
                SanitationJobId = Convert.ToInt32(objectId)
            };
            sanitationJobTask.RetrieveByJob(_userData.DatabaseKey, sanitationJobTask).ForEach(t => task.Add(t));
            return task;
        }
        public List<DataContracts.SanitationPlanning> PopulateToolsPrint(long objectId)
        {
            List<DataContracts.SanitationPlanning> tool = new List<DataContracts.SanitationPlanning>();
            DataContracts.SanitationPlanning sanitationPlanning = new DataContracts.SanitationPlanning();
            sanitationPlanning.SanitationPlanningRetrieveByMasterId(_userData.DatabaseKey, _userData.DatabaseKey.Client.ClientId, objectId, "Tool").ForEach(t => tool.Add(t));
            return tool;
        }
        public List<DataContracts.SanitationPlanning> PopulateChemicalsPrint(long objectId)
        {
            List<DataContracts.SanitationPlanning> chem = new List<DataContracts.SanitationPlanning>();
            DataContracts.SanitationPlanning sanitationPlanning = new DataContracts.SanitationPlanning();
            sanitationPlanning.SanitationPlanningRetrieveByMasterId(_userData.DatabaseKey, _userData.DatabaseKey.Client.ClientId, objectId, "Chemical").ForEach(t => chem.Add(t)); ;
            return chem;
        }
        #endregion
    }
}