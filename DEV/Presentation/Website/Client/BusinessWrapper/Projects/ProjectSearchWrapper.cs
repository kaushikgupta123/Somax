using Client.Models.Project;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper.Projects
{
    public class ProjectSearchWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;

        public ProjectSearchWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public List<ProjectSearchModel> GetProjectGridData(int CustomQueryDisplayId, string ClientlookupId = "", string Description = "", string WorkOrderClientLookupId = "", string CreateStartDateVw = "", string CreateEndDateVw = "",
            string CompleteStartDateVw = "",string CompleteEndDateVw="",string CloseStartDateVw="",string CloseEndDateVw="",
             string Status="", string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string searchText = "")
        {
            Project Proj = new Project();
            ProjectSearchModel ProjSerModel;
            List<ProjectSearchModel> ProjModelList = new List<ProjectSearchModel>();

            Proj.ClientId = userData.DatabaseKey.Client.ClientId;
            Proj.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            Proj.CustomQueryDisplayId = CustomQueryDisplayId;
            Proj.ClientLookupId = ClientlookupId;
            Proj.Description = Description;
            Proj.WorkOrderClientLookupId = WorkOrderClientLookupId;
            Proj.Status = Status;
            Proj.CreateStartDateVw = CreateStartDateVw;
            Proj.CreateEndDateVw = CreateEndDateVw;
            Proj.CompleteStartDateVw = CompleteStartDateVw;
            Proj.CompleteEndDateVw = CompleteEndDateVw;
            Proj.CloseStartDateVw = CloseStartDateVw;
            Proj.CloseEndDateVw = CloseEndDateVw; 
            Proj.OrderbyColumn = orderbycol;
            Proj.OrderBy = orderDir;
            Proj.OffSetVal = skip;
            Proj.NextRow = length;
            Proj.SearchText = searchText;

            List<Project> projectList = Proj.RetrieveChunkSearch(this.userData.DatabaseKey);

            if (projectList != null)
            {
                foreach (var item in projectList)
                {
                    ProjSerModel = new ProjectSearchModel();
                    ProjSerModel.ProjectId = item.ProjectId;
                    ProjSerModel.ClientlookupId = item.ClientLookupId;
                    ProjSerModel.Description = item.Description;
                    if (item.ActualStart != null && item.ActualStart == default(DateTime))
                    {
                        ProjSerModel.ActualStart = null;
                    }
                    else
                    {
                        ProjSerModel.ActualStart = item.ActualStart;
                    }

                    if (item.ActualFinish != null && item.ActualFinish == default(DateTime))
                    {
                        ProjSerModel.ActualFinish = null;
                    }
                    else
                    {
                        ProjSerModel.ActualFinish = item.ActualFinish;
                    }
                    ProjSerModel.Status = item.Status;
                    if (item.CreateDate != null && item.CreateDate == default(DateTime))
                    {
                        ProjSerModel.Created = null;
                    }
                    else
                    {
                        ProjSerModel.Created = item.CreateDate;
                    }
                    if (item.CompleteDate != null && item.CompleteDate == default(DateTime))
                    {
                        ProjSerModel.CompleteDate = null;
                    }
                    else
                    {
                        ProjSerModel.CompleteDate = item.CompleteDate;
                    }
                    ProjSerModel.ChildCount = item.ChildCount;
                    ProjSerModel.TotalCount = item.TotalCount;
                    ProjModelList.Add(ProjSerModel);
                }
            }



            return ProjModelList;
        }

        internal List<ProjectTaskModel> PopulateLineitems(long ProjectId)
        {
            ProjectTaskModel objLineItem;
            List<ProjectTaskModel> LineItemList = new List<ProjectTaskModel>();

            ProjectTask woPlanLineItem = new ProjectTask()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ProjectId = ProjectId
            };
            List<ProjectTask> woplanLintItemList = woPlanLineItem.RetrieveProjectTask_ByProjectId(this.userData.DatabaseKey);

            if (woplanLintItemList != null)
            {
                foreach (var item in woplanLintItemList)
                {
                    objLineItem = new ProjectTaskModel();
                    objLineItem.ProjectTaskId = item.ProjectTaskId;
                    objLineItem.WorkOrderClientLookupId = item.WorkOrderClientlookupId;
                    objLineItem.WorkOrderDescription = item.WorkOrderDescription;
                    if (item.StartDate == null || item.StartDate == default(DateTime))
                    {
                        objLineItem.StartDate = null;
                    }
                    else
                    {
                        objLineItem.StartDate = item.StartDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (item.EndDate == null || item.EndDate == default(DateTime))
                    {
                        objLineItem.Enddate = null;
                    }
                    else
                    {
                        objLineItem.Enddate = item.EndDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    objLineItem.Progress = item.Progress;
                    objLineItem.ProgressPercentage = Convert.ToInt32(item.Progress * 100);

                    LineItemList.Add(objLineItem);
                }
            }

            return LineItemList;
        }

        #region Add Project
        public Project AddorEditProject(ProjectVM objWOPVM)
        {

            string emptyValue = string.Empty;

            List<string> errList = new List<string>();
            Project proj = new Project();

            if (objWOPVM.projectAddorEdirModel.ProjectId == 0)
            {
                //Add in WorkOrderPlan Table
                proj.ClientId = this.userData.DatabaseKey.User.ClientId;
                proj.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
                proj.AreaId = 0;
                proj.DepartmentId = 0;
                proj.StoreroomId = 0;
                proj.Budget = objWOPVM.projectAddorEdirModel.Budget??0;
                proj.Coordinator_PersonnelId = objWOPVM.projectAddorEdirModel.Coordinator_PersonnelId??0;
                proj.Description = !string.IsNullOrEmpty(objWOPVM.projectAddorEdirModel.Description) ? objWOPVM.projectAddorEdirModel.Description.Trim() : emptyValue;
                proj.FiscalYear = objWOPVM.projectAddorEdirModel.FiscalYear??0;
                proj.Owner_PersonnelId= objWOPVM.projectAddorEdirModel.Owner_PersonnelId ?? 0;
                proj.ScheduleStart = objWOPVM.projectAddorEdirModel.ScheduleStart;
                proj.ScheduleFinish= objWOPVM.projectAddorEdirModel.ScheduleFinish;
                proj.Status = ProjectStatusConstants.Open;
                proj.Type = !string.IsNullOrEmpty(objWOPVM.projectAddorEdirModel.Type) ? objWOPVM.projectAddorEdirModel.Type.Trim() : emptyValue; ;
                proj.ClientLookupId = objWOPVM.projectAddorEdirModel.ClientLookupId.ToUpper().Trim();
                proj.CheckDuplicateProject(this.userData.DatabaseKey);
                if (proj.ErrorMessages == null || proj.ErrorMessages.Count == 0)
                {
                    proj.Create(this.userData.DatabaseKey);

                    //Add in ProjectEventLog table
                    CreatePROJEventLog(proj.ProjectId, ProjectStatusConstants.Open);
                }

            }
            else
            {
                Project objProject = new Project()
                {
                    ClientId = _dbKey.Client.ClientId,
                    ProjectId = objWOPVM.projectAddorEdirModel.ProjectId
                };
                objProject.Retrieve(this.userData.DatabaseKey);
                objProject.Budget = objWOPVM.projectAddorEdirModel.Budget??0;
                objProject.Coordinator_PersonnelId = objWOPVM.projectAddorEdirModel.Coordinator_PersonnelId ?? 0;
                objProject.Description = !string.IsNullOrEmpty(objWOPVM.projectAddorEdirModel.Description) ? objWOPVM.projectAddorEdirModel.Description.Trim() : emptyValue;
                objProject.FiscalYear = objWOPVM.projectAddorEdirModel.FiscalYear??0;
                objProject.Owner_PersonnelId = objWOPVM.projectAddorEdirModel.Owner_PersonnelId ?? 0;
                objProject.ScheduleStart = objWOPVM.projectAddorEdirModel.ScheduleStart;
                objProject.ScheduleFinish = objWOPVM.projectAddorEdirModel.ScheduleFinish;
                objProject.Type = objWOPVM.projectAddorEdirModel.Type==null?"" : objWOPVM.projectAddorEdirModel.Type;
                objProject.ActualStart = objWOPVM.projectAddorEdirModel.ActualStart;
                objProject.ActualFinish = objWOPVM.projectAddorEdirModel.ActualFinish;
                objProject.Update(this.userData.DatabaseKey);
                proj.ProjectId = objProject.ProjectId;
                proj.ClientLookupId = objProject.ClientLookupId;
            }


            return proj;
        }
        #endregion

        #region Common method
        private void CreatePROJEventLog(Int64 ProjectId, string Status)
        {
            ProjectEventLog log = new ProjectEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ProjectId = ProjectId;
            log.TransactionDate = DateTime.UtcNow;
            log.Event = Status;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceTable = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion
    }
}