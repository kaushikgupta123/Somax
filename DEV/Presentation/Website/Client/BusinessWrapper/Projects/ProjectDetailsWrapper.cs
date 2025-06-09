using Client.Models;
using Client.Models.Project;

using Common.Constants;
using Common.Extensions;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper.Projects
{
    public class ProjectDetailsWrapper
    {
        private DatabaseKey _dbKey;
        private UserData _userData;
        public ProjectDetailsWrapper(UserData userData)
        {
            this._userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Project task grid data
        public List<ProjectTaskSearchModel> GetProjectTaskByProjectIdGridData(long ProjectId, string WorkOrderClientlookupId = "", string WorkOrderDescription = "", DateTime? StartDate = null, DateTime? EndDate = null,
             string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string searchText = "")
        {
            ProjectTask projectTask = new ProjectTask();
            ProjectTaskSearchModel projectTaskSearchModel;
            List<ProjectTaskSearchModel> projectTaskSearchModelList = new List<ProjectTaskSearchModel>();

            projectTask.ClientId = _userData.DatabaseKey.Client.ClientId;
            projectTask.SiteId = _userData.DatabaseKey.User.DefaultSiteId;
            projectTask.ProjectId = ProjectId;
            projectTask.WorkOrderClientlookupId = WorkOrderClientlookupId;
            projectTask.WorkOrderDescription = WorkOrderDescription;
            projectTask.StartDate = StartDate;
            projectTask.EndDate = EndDate;
            projectTask.OrderbyColumn = orderbycol;
            projectTask.OrderBy = orderDir;
            projectTask.OffSetVal = skip;
            projectTask.NextRow = length;
            projectTask.SearchText = searchText;

            List<ProjectTask> projectTaskList = projectTask.RetrieveProjectTask_ByProjectIdForChunkSearch(this._userData.DatabaseKey);

            if (projectTaskList != null)
            {
                foreach (var item in projectTaskList)
                {
                  
                    projectTaskSearchModel = new ProjectTaskSearchModel();
                    projectTaskSearchModel.WorkOrderId = item.WorkOrderId;
                    projectTaskSearchModel.ProjectTaskId = item.ProjectTaskId;
                    projectTaskSearchModel.WorkOrderClientLookupId = item.WorkOrderClientlookupId;
                    projectTaskSearchModel.WorkOrderDescription = item.WorkOrderDescription;
                    if (item.StartDate != null && item.StartDate == default(DateTime))
                    {
                        projectTaskSearchModel.StartDate = "";
                    }
                    else
                    {

                        projectTaskSearchModel.StartDate = Convert.ToDateTime(item.StartDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }

                    if (item.EndDate != null && item.EndDate == default(DateTime))
                    {
                        projectTaskSearchModel.EndDate = "";
                    }
                    else
                    {
                        projectTaskSearchModel.EndDate = Convert.ToDateTime(item.EndDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    projectTaskSearchModel.Progress = item.Progress*100;
                    projectTaskSearchModel.TotalCount = item.TotalCount;
                    projectTaskSearchModelList.Add(projectTaskSearchModel);
                }
            }
            return projectTaskSearchModelList;
        }
        #endregion
        #region Update project task start and end date
        public List<String> UpdateProjectTaskStartDateAndEndDate(long ProjectTaskId, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            ProjectTask projectTask = new ProjectTask()
            {
                ProjectTaskId = ProjectTaskId
            };
            projectTask.Retrieve(this._userData.DatabaseKey);

            projectTask.StartDate = StartDate;
            projectTask.EndDate = EndDate;

            projectTask.Update(this._userData.DatabaseKey);
            return projectTask.ErrorMessages;
        }
        #endregion
        #region Update project task Progress value
        public List<String> UpdateProjectTaskProgress(long ProjectTaskId,string Progress)
        {
            ProjectTask projectTask = new ProjectTask()
            {
                ProjectTaskId = ProjectTaskId
            };
            projectTask.Retrieve(this._userData.DatabaseKey);
            projectTask.Progress =(Convert.ToDecimal(Progress)/100);
            projectTask.Update(this._userData.DatabaseKey);
            return projectTask.ErrorMessages;
        }
        #endregion
        #region Project tab header
        public ProjectDetailsHeaderModel GetProjectByProjectIdForProjectDetailsHeader(long projectId)
        {
            ProjectDetailsHeaderModel projectDetailsHeaderModel = new ProjectDetailsHeaderModel();
            Project project = new Project()
            {
                ClientId = _dbKey.Client.ClientId,
                ProjectId = projectId
            };
            var records = project.RetrieveProjectByProjectIdForHeader(this._userData.DatabaseKey);
            projectDetailsHeaderModel.ProjectId = records.ProjectId;
            projectDetailsHeaderModel.ClientlookupId = records.ClientLookupId;
            projectDetailsHeaderModel.Status = records.Status;
            projectDetailsHeaderModel.Description = records.Description;
            projectDetailsHeaderModel.Coordinator = records.Coordinator;
            projectDetailsHeaderModel.ScheduleStart = records.ScheduleStart != null && records.ScheduleStart != default(DateTime) ? Convert.ToDateTime(records.ScheduleStart).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : ""; 
            projectDetailsHeaderModel.ScheduleFinish = records.ScheduleFinish != null && records.ScheduleFinish != default(DateTime) ? Convert.ToDateTime(records.ScheduleFinish).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : "";
            projectDetailsHeaderModel.CompleteDate = records.CompleteDate != null && records.CompleteDate != default(DateTime) ? Convert.ToDateTime(records.CompleteDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : "";
            projectDetailsHeaderModel.Budget= records.Budget;
            return projectDetailsHeaderModel;
        }
        #endregion

        #region Get WorkOrder ProjectDetailsLookupList Grid Data
        public List<WorkOrderForProjectDetailsLookupListModel> GetWorkOrderForProjectDetailsLookupListGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string ChargeTo="", string ChargeTo_Name = "", string Description = "", string Status = "", string Priority = "", string Type = "", DateTime? RequiredDate = null)
        {

            WorkOrderForProjectDetailsLookupListModel workorderWOPlanLookupListModel;
            List<WorkOrderForProjectDetailsLookupListModel> workorderWOPlanLookupListModelList = new List<WorkOrderForProjectDetailsLookupListModel>();
            List<Project> WorkOrderForProjectDetailsLookupList = new List<Project>();
            Project WOLookupList = new Project();
            WOLookupList.ClientId = this._userData.DatabaseKey.Client.ClientId;
            WOLookupList.SiteId = _userData.DatabaseKey.User.DefaultSiteId;
            WOLookupList.OrderbyColumn = orderbycol;
            WOLookupList.OrderBy = orderDir;
            WOLookupList.OffSetVal = skip;
            WOLookupList.NextRow = length;
            WOLookupList.WorkOrderClientLookupId = clientLookupId;
            WOLookupList.ChargeTo = ChargeTo;
            WOLookupList.ChargeTo_Name = ChargeTo_Name;
            WOLookupList.Description = Description;
            WOLookupList.Status = Status;
            WOLookupList.Priority = Priority;
            WOLookupList.RequiredDate = RequiredDate;
            WOLookupList.Type = Type;
            WorkOrderForProjectDetailsLookupList = WOLookupList.RetrieveWorkOrder_ProjectDetailsLookupListBySearchCriteria(_userData.DatabaseKey);
            foreach (var item in WorkOrderForProjectDetailsLookupList)
            {
                workorderWOPlanLookupListModel = new WorkOrderForProjectDetailsLookupListModel();
                workorderWOPlanLookupListModel.WorkOrderId = item.WorkOrderId;
                workorderWOPlanLookupListModel.ChargeTo = item.ChargeTo;
                workorderWOPlanLookupListModel.ChargeTo_Name = item.ChargeTo_Name;
                workorderWOPlanLookupListModel.Description = item.Description;
                workorderWOPlanLookupListModel.ClientLookupId = item.WorkOrderClientLookupId;
                workorderWOPlanLookupListModel.Status = item.Status;
                workorderWOPlanLookupListModel.Priority = item.Priority;
                workorderWOPlanLookupListModel.Type = item.Type;
                workorderWOPlanLookupListModel.TotalCount = item.TotalCount;
                if (item.RequiredDate != null && item.RequiredDate == default(DateTime))
                {
                    workorderWOPlanLookupListModel.RequiredDate = null;
                }
                else
                {
                    workorderWOPlanLookupListModel.RequiredDate = item.RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                workorderWOPlanLookupListModelList.Add(workorderWOPlanLookupListModel);
            }

            return workorderWOPlanLookupListModelList;
        }
        #endregion

        #region Add WOPlan LineItem
        public List<string> AddProjectLineItem(string[] Workorderids, long ProjectId)
        {
            var errorList = new List<string>();
            if (Workorderids.Length > 0)
            {
                for (int i = 0; i < Workorderids.Length; i++)
                {
                    if (!string.IsNullOrEmpty(Workorderids[i]))
                    {
                        long WorkOrderId = Convert.ToInt64(Workorderids[i].ToString());
                        ProjectTask projectTask = new ProjectTask()
                        {
                            ClientId = _dbKey.Client.ClientId,
                            ProjectId = ProjectId,
                            WorkOrderId = WorkOrderId,
                            Type= "Task"
                        };

                        projectTask.Create(this._userData.DatabaseKey);
                        if (projectTask.ErrorMessages != null && projectTask.ErrorMessages.Count > 0)
                        {
                            errorList = projectTask.ErrorMessages;
                            break;
                        }
                        else
                        {
                            errorList = UpdateWorkorderProjectStatus(WorkOrderId, ProjectId,"Add");
                        }


                    }

                }
            }
            return errorList;


        }
        private List<string>  UpdateWorkorderProjectStatus(long Woid,long ProjectId,string type)
        {
            var errorList = new List<string>();
            WorkOrder workOrder = new WorkOrder()
            {
                WorkOrderId = Woid
            };
            workOrder.Retrieve(this._userData.DatabaseKey);
            if(type=="Add")
            {
                workOrder.ProjectId = ProjectId;
            }
            if (type == "Delete")
            {
                workOrder.ProjectId = 0;
            }
            workOrder.Update(this._userData.DatabaseKey);
            return workOrder.ErrorMessages;
        }
        #endregion
        #region Remove Project LineItem
        public List<string> RemoveProjectLineItem(string[] SelectedProjectIds, string[] SelectedWorkOrderIds)
        {
            var errorList = new List<string>();
            if (SelectedProjectIds.Length > 0 && SelectedWorkOrderIds.Length>0) 
            {
                for (int i = 0; i < SelectedProjectIds.Length; i++)
                {
                    if (!string.IsNullOrEmpty(SelectedProjectIds[i]) && !string.IsNullOrEmpty(SelectedWorkOrderIds[i]))
                    {
                        long ProjectId = Convert.ToInt64(SelectedProjectIds[i].ToString());
                        long WorkOrderId = Convert.ToInt64(SelectedWorkOrderIds[i].ToString());
                        ProjectTask projectTask = new ProjectTask()
                        {
                            ClientId = _dbKey.Client.ClientId,
                            ProjectTaskId = ProjectId,
                        };
                         projectTask.Delete(this._userData.DatabaseKey);
                        if (projectTask.ErrorMessages != null && projectTask.ErrorMessages.Count > 0)
                        {
                            errorList = projectTask.ErrorMessages;
                            break;
                        }
                        else
                        {
                            errorList = UpdateWorkorderProjectStatus(WorkOrderId, ProjectId, "Delete");
                        }
                    }

                }
            }
            return errorList;


        }
        #endregion

        #region  Update Project Status 
        public List<string> ProjectStatusUpdating(long ProjectId, string Status)
        {
            string emptyValue = string.Empty;
            List<string> errList = new List<string>();
            Project objProject = new Project()
            {
                ClientId = _dbKey.Client.ClientId,
                ProjectId = ProjectId
            };
            objProject.Retrieve(this._userData.DatabaseKey);
            // ------Updating ------
          
            string Eventstatus = Status == "Open" ? "ReOpen" : Status == "Canceled" ? "Cancel": Status == "Complete"? "Complete":"";
            string ProjStatus = Status == "Open" ? ProjectStatusConstants.Open : Status == "Canceled" ? ProjectStatusConstants.Canceled : Status == "Complete" ? ProjectStatusConstants.Complete : ""; 
            if (Status == "Complete")
            {
                objProject.CompleteDate = DateTime.UtcNow;
                objProject.CompleteBy_PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
            }
            if (Status == "Canceled")
            {
                objProject.CancelDate = DateTime.UtcNow;
                objProject.CancelBy_PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
            }
            if (Status == "Open")
            {
                objProject.CancelDate = DateTime.UtcNow;
                objProject.CancelBy_PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
            }
            objProject.Status = ProjStatus;
            objProject.Update(this._userData.DatabaseKey);
            // ------creating new event logs ------
            if(Eventstatus!="")
            {
                CreatProjectEventLog(ProjectId, Eventstatus);
            }
           
            return errList;
        }

        #endregion

        #region Common method
        private void CreatProjectEventLog(Int64 ProjectId, string Status)
        {
            ProjectEventLog log = new ProjectEventLog();
            log.ClientId = _userData.DatabaseKey.Client.ClientId;
            log.SiteId = _userData.DatabaseKey.Personnel.SiteId;
            log.ProjectId = ProjectId;
            log.TransactionDate = DateTime.UtcNow;
            log.Event = Status;
            log.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceTable = "";
            log.SourceId = 0;
            log.Create(_userData.DatabaseKey);
        }
        #endregion

        #region Event Log
        public List<EventLogModel> PopulateEventLog(long projectId)
        {
            EventLogModel objEventLogModel;
            List<EventLogModel> EventLogModelList = new List<EventLogModel>();

            ProjectEventLog log = new ProjectEventLog();
            List<ProjectEventLog> data = new List<ProjectEventLog>();
            log.ClientId = _userData.DatabaseKey.Client.ClientId;
            log.SiteId = _userData.DatabaseKey.Personnel.SiteId;
            log.ProjectId = projectId;
            data = log.RetriveByProjectId(_userData.DatabaseKey);

            if (data != null)
            {
                foreach (var item in data)
                {
                    objEventLogModel = new EventLogModel();
                    objEventLogModel.ClientId = item.ClientId;
                    objEventLogModel.SiteId = item.SiteId;
                    objEventLogModel.EventLogId = item.ProjectEventLogId;
                    objEventLogModel.ObjectId = item.ProjectId;
                    if (item.TransactionDate != null && item.TransactionDate == default(DateTime))
                    {
                        objEventLogModel.TransactionDate = null;
                    }
                    else
                    {
                        objEventLogModel.TransactionDate = item.TransactionDate.ToUserTimeZone(_userData.Site.TimeZone);
                    }
                    objEventLogModel.Event = item.Event;
                    objEventLogModel.Comments = item.Comments;
                    objEventLogModel.SourceId = item.SourceId;
                    objEventLogModel.Personnel = item.Personnel;
                    objEventLogModel.Events = item.Events;
                    objEventLogModel.PersonnelInitial = item.PersonnelInitial;
                    EventLogModelList.Add(objEventLogModel);
                }
            }
            return EventLogModelList;
        }

        #endregion

        #region Get Edit Records
        public ProjectAddOrEditModel RetrieveProjectRecordByProjectID(long ProjectId)
        {
            ProjectAddOrEditModel projectAddOrEditModel = new ProjectAddOrEditModel();
            Project objProject = new Project()
            {
                ClientId = _dbKey.Client.ClientId,
                ProjectId = ProjectId
            };
            objProject.Retrieve(this._userData.DatabaseKey);
            projectAddOrEditModel.ProjectId = objProject.ProjectId;
            projectAddOrEditModel.ClientLookupId = objProject.ClientLookupId;
            projectAddOrEditModel.Description = objProject.Description;
            projectAddOrEditModel.ScheduleStart = objProject.ScheduleStart!=null && objProject.ScheduleStart != default(DateTime)? objProject.ScheduleStart:null;
            projectAddOrEditModel.ScheduleFinish = objProject.ScheduleFinish != null && objProject.ScheduleFinish != default(DateTime) ? objProject.ScheduleFinish : null;
            projectAddOrEditModel.Owner_PersonnelId = objProject.Owner_PersonnelId;
            projectAddOrEditModel.Coordinator_PersonnelId = objProject.Coordinator_PersonnelId;
            projectAddOrEditModel.FiscalYear = objProject.FiscalYear;
            projectAddOrEditModel.Budget = objProject.Budget;
            projectAddOrEditModel.Type = objProject.Type;
            projectAddOrEditModel.ActualFinish = objProject.ActualFinish != null && objProject.ActualFinish != default(DateTime) ? objProject.ActualFinish : null;
            projectAddOrEditModel.ActualStart = objProject.ActualStart != null && objProject.ActualStart != default(DateTime) ? objProject.ActualStart : null;

            return projectAddOrEditModel;
        }
        #endregion

        public ProjectModel GetProjectByProjectId(long projectId)
        {
            ProjectModel projectModel = new ProjectModel();
            Project project = new Project()
            {
                ClientId = _dbKey.Client.ClientId,
                ProjectId = projectId
            };
            project.Retrieve(this._userData.DatabaseKey);
            projectModel.ProjectId = project.ProjectId;
            projectModel.ClientLookupId = project.ClientLookupId;
            projectModel.Status = project.Status;            
            return projectModel;
        }
    }
}