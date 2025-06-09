using Client.Models.Project;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Client.BusinessWrapper.Projects
{
    public class TimelineDetailsWrapper
    {
        private DatabaseKey _dbKey;
        private UserData _userData;
        public TimelineDetailsWrapper(UserData userData)
        {
            this._userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        internal List<String> UpdateProjectTask(ProjectTaskTimelineModel projectTaskTimelineModel)
        {
            ProjectTask projectTask = new ProjectTask()
            {
                ProjectTaskId = projectTaskTimelineModel.id
            };
            projectTask.Retrieve(this._userData.DatabaseKey);

            if (!string.IsNullOrEmpty(projectTaskTimelineModel.start_date))
            {
                projectTask.StartDate = DateTime.ParseExact(projectTaskTimelineModel.start_date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                projectTask.StartDate = null;
            }
            if (!string.IsNullOrEmpty(projectTaskTimelineModel.end_date))
            {
                projectTask.EndDate = DateTime.ParseExact(projectTaskTimelineModel.end_date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (projectTask.EndDate > projectTask.StartDate)
                {
                    projectTask.EndDate = projectTask.EndDate.Value.AddDays(-1);
                }                
            }
            else
            {
                projectTask.EndDate = null;
            }
            projectTask.Progress = projectTaskTimelineModel.progress;

            projectTask.Update(this._userData.DatabaseKey);
            return projectTask.ErrorMessages;
        }
        internal List<ProjectTaskTimelineModel> RetrieveProjectTaskByProjectId(long ProjectId)
        {
            ProjectTaskTimelineModel objProjectTask;
            List<ProjectTaskTimelineModel> ProjectTaskModelList = new List<ProjectTaskTimelineModel>();

            ProjectTask projectTask = new ProjectTask()
            {
                ClientId = this._userData.DatabaseKey.Client.ClientId,
                ProjectId = ProjectId
            };
            List<ProjectTask> ProjectTaskList = projectTask.RetrieveProjectTask_ByProjectId(this._userData.DatabaseKey);

            if (ProjectTaskList != null)
            {
                foreach (var item in ProjectTaskList)
                {
                    objProjectTask = new ProjectTaskTimelineModel();
                    objProjectTask.id = item.ProjectTaskId;
                    objProjectTask.text = item.WorkOrderClientlookupId + " - " + item.WorkOrderDescription;
                    if (item.StartDate == null || item.StartDate == default(DateTime))
                    {
                        objProjectTask.start_date = "";
                    }
                    else
                    {
                        objProjectTask.start_date = item.StartDate.Value.ToString("MM/dd/yyyy");
                    }
                    if (item.EndDate == null || item.EndDate == default(DateTime))
                    {
                        objProjectTask.end_date = "";
                        objProjectTask.end_date_grid = "";
                    }
                    else
                    {
                        objProjectTask.end_date_grid = item.EndDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        objProjectTask.end_date = item.EndDate.Value.AddDays(1).ToString("MM/dd/yyyy");                        
                    }
                    objProjectTask.progress = item.Progress;
                    objProjectTask.ProjectId = item.ProjectId;
                    if (string.IsNullOrEmpty(objProjectTask.start_date) && string.IsNullOrEmpty(objProjectTask.end_date))
                    {
                        objProjectTask.unscheduled = true;
                    }
                    else if (!string.IsNullOrEmpty(objProjectTask.start_date) && string.IsNullOrEmpty(objProjectTask.end_date))
                    {
                        // added 2 days to maintain the validation "End date must be greater then start date"
                        objProjectTask.end_date = item.StartDate.Value.AddDays(2).ToString("MM/dd/yyyy");
                    }

                    ProjectTaskModelList.Add(objProjectTask);
                }
            }
            return ProjectTaskModelList;
        }
        //internal ProjectTaskTimelineModel RetrieveProjectByProjectId(long ProjectId)
        //{
        //    ProjectTaskTimelineModel objProjectTask = new ProjectTaskTimelineModel();

        //    Project project = new Project()
        //    {
        //        ClientId = this._userData.DatabaseKey.Client.ClientId,
        //        ProjectId = ProjectId
        //    };
        //    project.Retrieve(this._userData.DatabaseKey);

        //    if (project != null)
        //    {
        //        objProjectTask.id = project.ProjectId;
        //        if (project.ScheduleStart == null || project.ScheduleStart == default(DateTime))
        //        {
        //            objProjectTask.start_date = "";
        //        }
        //        else
        //        {
        //            objProjectTask.start_date = project.ScheduleStart.Value.ToString("MMddyyyy");
        //        }
        //        if (project.ScheduleFinish == null || project.ScheduleFinish == default(DateTime))
        //        {
        //            objProjectTask.end_date = "";
        //        }
        //        else
        //        {
        //            objProjectTask.end_date = project.ScheduleFinish.Value.ToString("MMddyyyy");
        //        }
        //        objProjectTask.parentid = 0;
        //        objProjectTask.text = project.ClientLookupId;
        //    }
        //    return objProjectTask;
        //}
    }
}