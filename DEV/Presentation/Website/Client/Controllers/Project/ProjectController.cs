
using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Projects;
using Client.Common;
using Client.Common.Constants;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.Project;
using Common.Constants;

using DataContracts;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using static Client.Models.Common.UserMentionDataModel;

namespace Client.Controllers.Project
{
    public class ProjectController : SomaxBaseController
    {

        [CheckUserSecurity(securityType = SecurityConstants.Project)]
        public ActionResult Index()
        {
            ProjectVM objProjectVM = new ProjectVM();
            ProjectModel pm = new ProjectModel();
            objProjectVM.security = this.userData.Security;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            if (userData.DatabaseKey.Client.UseFormalProject == true)
            {
                objProjectVM.ProjectViewList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.Project);
                pm.DateRangeDropListForAllStatus = UtilityFunction.GetTimeRangeDropForAllStatusProj().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                pm.DateRangeDropListForCompletedProject = UtilityFunction.GetTimeRangeDropForCompletedProj().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                pm.DateRangeDropListForClosedProject = UtilityFunction.GetTimeRangeDropForClosedProj().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                //status
                var StatusList = commonWrapper.GetListFromConstVals(LookupListConstants.Project_Status);
                if (StatusList != null)
                {
                    pm.StatusList = StatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                }
                objProjectVM.projectModel = pm;
                LocalizeControls(objProjectVM, LocalizeResourceSetConstants.Project);
                return View(objProjectVM);
            }
            else
            {
                return RedirectToAction("Index", "ProjectCosting", new { page= "ProjectCosting" });
            }
        }

        #region Project search

        [HttpPost]
        public string GetProjectGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, string ClientlookupId = "", string Description = "", string WorkOrderClientLookupId = "", string CreateStartDateVw = "", string CreateEndDateVw = "",
            string CompleteStartDateVw = "", string CompleteEndDateVw = "", string CloseStartDateVw = "", string CloseEndDateVw = "",
             string Status = "", string Order = "1", string txtSearchval = "")
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var filter = CustomQueryDisplayId;
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            ProjectSearchWrapper ProjSearchWrapper = new ProjectSearchWrapper(userData);
            List<ProjectSearchModel> ProjList = ProjSearchWrapper.GetProjectGridData(CustomQueryDisplayId, ClientlookupId, Description, WorkOrderClientLookupId, CreateStartDateVw, CreateEndDateVw, CompleteStartDateVw, CompleteEndDateVw, CloseStartDateVw, CloseEndDateVw, Status, Order, orderDir, skip, length ?? 0, txtSearchval);

            var totalRecords = 0;
            var recordsFiltered = 0;
            if (ProjList != null && ProjList.Count > 0)
            {
                recordsFiltered = ProjList[0].TotalCount;
                totalRecords = ProjList[0].TotalCount;
                //recordsFiltered = 3;
                //totalRecords = 3;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = ProjList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, hiddenColumnList = "" }, JsonSerializerDateSettings);
        }
        #endregion

        #region Details
        public PartialViewResult Details(long ProjectId, string ClientLookupId = "0")
        {
            ProjectVM objProjectVM = new ProjectVM();
            ProjectDetailsWrapper wrapper = new ProjectDetailsWrapper(userData);
            objProjectVM.security = this.userData.Security;
            objProjectVM.projectHeaderSummaryModel = wrapper.GetProjectByProjectIdForProjectDetailsHeader(ProjectId);
            objProjectVM._userdata = this.userData;
            LocalizeControls(objProjectVM, LocalizeResourceSetConstants.Project);
            return PartialView("~/Views/Project/ProjectDetails/_ProjectDetails.cshtml", objProjectVM);
        }
       
        #region WO Project lookup list



        public JsonResult GetProjectWOLookupListchunksearch(int? draw, int? start, int? length, string clientLookupId = "", string ChargeTo = "", string ChargeTo_Name = "", string Description = "", string Status = "", string Priority = "", string Type = "")
        {
            List<WorkOrderForProjectDetailsLookupListModel> modelList = new List<WorkOrderForProjectDetailsLookupListModel>();
            ProjectDetailsWrapper projectDetailsWrapper = new ProjectDetailsWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            ChargeTo = !string.IsNullOrEmpty(ChargeTo) ? ChargeTo.Trim() : string.Empty;
            ChargeTo_Name = !string.IsNullOrEmpty(ChargeTo_Name) ? ChargeTo_Name.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            Status = !string.IsNullOrEmpty(Status) ? Status.Trim() : string.Empty;
            Priority = !string.IsNullOrEmpty(Priority) ? Priority.Trim() : string.Empty;
            Type = !string.IsNullOrEmpty(Type) ? Type.Trim() : string.Empty;
            modelList = projectDetailsWrapper.GetWorkOrderForProjectDetailsLookupListGridData(order, orderDir, skip, length.Value, clientLookupId, ChargeTo, ChargeTo_Name, Description, Status, Priority, Type);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }
            var jsonResult = Json(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetProjectWOLookupListSelectAllData(string colname, string coldir, string clientLookupId = "", string ChargeTo = "", string ChargeTo_Name = "", string Description = "", string Status = "", string Priority = "", string Type = "")
        {
            List<WorkOrderForProjectDetailsLookupListModel> modelList = new List<WorkOrderForProjectDetailsLookupListModel>();
            ProjectDetailsWrapper projectDetailsWrapper = new ProjectDetailsWrapper(userData);
            ChargeTo = !string.IsNullOrEmpty(ChargeTo) ? ChargeTo.Trim() : string.Empty;
            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            ChargeTo_Name = !string.IsNullOrEmpty(ChargeTo_Name) ? ChargeTo_Name.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            Status = !string.IsNullOrEmpty(Status) ? Status.Trim() : string.Empty;
            Priority = !string.IsNullOrEmpty(Priority) ? Priority.Trim() : string.Empty;
            Type = !string.IsNullOrEmpty(Type) ? Type.Trim() : string.Empty;
            modelList = projectDetailsWrapper.GetWorkOrderForProjectDetailsLookupListGridData(colname, coldir, 0, 100000, clientLookupId, ChargeTo, ChargeTo_Name, Description, Status, Priority, Type);
            //return Json(modelList, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(modelList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult AddProjectLineItem(string[] WorkorderIds, long ProjectId)
        {
            var errorList = new List<string>();
            if (WorkorderIds.Length > 0 && ProjectId != 0)
            {

                ProjectDetailsWrapper ProjectSummary = new ProjectDetailsWrapper(userData);
                errorList = ProjectSummary.AddProjectLineItem(WorkorderIds, ProjectId);
                if (errorList != null && errorList.Count > 0)
                {
                    var returnOjb = new { success = false, errorList = errorList };
                    return Json(returnOjb, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var returnOjb = new { success = true };
                    return Json(returnOjb, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var returnOjb = new { success = false, errorList = errorList };
                return Json(returnOjb, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult RemoveProjectLineItem(string[] SelectedProjectIds, string[] SelectedWorkOrderIds)
        {
            var errorList = new List<string>();
            if (SelectedProjectIds.Length > 0 && SelectedWorkOrderIds.Length > 0 && SelectedProjectIds.Length == SelectedWorkOrderIds.Length)
            {
                ProjectDetailsWrapper projectDetailsWrapper = new ProjectDetailsWrapper(userData);
                errorList = projectDetailsWrapper.RemoveProjectLineItem(SelectedProjectIds, SelectedWorkOrderIds);
                if (errorList != null && errorList.Count > 0)
                {
                    var returnOjb = new { success = false, errorList = errorList };
                    return Json(returnOjb, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var returnOjb = new { success = true };
                    return Json(returnOjb, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var returnOjb = new { success = false, errorList = errorList };
                return Json(returnOjb, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Update Project Status
        public JsonResult UpdatingProjectStatus(long ProjectId, string Status)
        {
            var errorList = new List<string>();
            if (ProjectId > 0)
            {
                ProjectDetailsWrapper projectDetailsWrapper = new ProjectDetailsWrapper(userData);
                errorList = projectDetailsWrapper.ProjectStatusUpdating(ProjectId, Status);
                if (errorList != null && errorList.Count > 0)
                {
                    var returnOjb = new { success = false, errorList = errorList };
                    return Json(returnOjb, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var returnOjb = new { success = true };
                    return Json(returnOjb, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var returnOjb = new { success = false, errorList = errorList };
                return Json(returnOjb, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Project task list  Select All
        [HttpPost]
        public JsonResult GetProjectTaskByProjectIdGridSelectAllData(string colname, string coldir, long ProjectId, string WorkOrderClientLookupId = "", string WorkOrderDescription = "", DateTime? StartDate = null, DateTime? EndDate = null,
              string Order = "1", string txtSearchval = "")
        {
            WorkOrderClientLookupId = WorkOrderClientLookupId.Replace("%", "[%]");
            WorkOrderDescription = WorkOrderDescription.Replace("%", "[%]");
            txtSearchval = txtSearchval.Replace("%", "[%]");
            ProjectDetailsWrapper projectDetailsWrapper = new ProjectDetailsWrapper(userData);
            List<ProjectTaskSearchModel> modelList = projectDetailsWrapper.GetProjectTaskByProjectIdGridData(ProjectId, WorkOrderClientLookupId, WorkOrderDescription, StartDate, EndDate, colname, coldir, 0, 100000, txtSearchval);

            return Json(modelList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Activity Event Log

        [HttpPost]
        public PartialViewResult LoadActivity(long ProjectId)
        {
            ProjectVM objProjectVM = new ProjectVM();
            ProjectDetailsWrapper projectDetailsWrapper = new ProjectDetailsWrapper(userData);
            List<EventLogModel> EventLogList = new List<EventLogModel>();
            EventLogList = projectDetailsWrapper.PopulateEventLog(ProjectId);
            objProjectVM.EventLogList = EventLogList;
            LocalizeControls(objProjectVM, LocalizeResourceSetConstants.Project);
            return PartialView("~/Views/Project/ProjectDetails/_ActivityList.cshtml", objProjectVM);
        }
        #endregion
        #region Comments Event Log
        [HttpPost]
        public PartialViewResult LoadComments(long ProjectId)
        {
            ProjectVM objProjectVM = new ProjectVM();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Personnel> personnelsList = new List<Personnel>();
            List<Notes> NotesList = new List<Notes>();
            UserMentionData userMentionData;
            List<UserMentionData> userMentionDatas = new List<UserMentionData>();

            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => personnelsList = coWrapper.MentionList(""));
            tasks[1] = Task.Factory.StartNew(() => NotesList = coWrapper.PopulateComment(ProjectId, "Project"));
            Task.WaitAll(tasks);

            if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {
                foreach (var myuseritem in personnelsList)
                {
                    userMentionData = new UserMentionData();
                    userMentionData.id = myuseritem.UserName;
                    userMentionData.name = myuseritem.FullName;
                    userMentionData.type = myuseritem.PersonnelInitial;
                    userMentionDatas.Add(userMentionData);
                }
                objProjectVM.userMentionData = userMentionDatas;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                objProjectVM.NotesList = NotesList;
            }
            LocalizeControls(objProjectVM, LocalizeResourceSetConstants.Project);
            return PartialView("~/Views/Project/ProjectDetails/_CommentsList.cshtml", objProjectVM);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddComments(long ProjectId, string content, string ClientLookupId, List<string> userList, long noteId = 0, long updatedindex = 0)
        {

            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionData> userMentionDataList = new List<UserMentionData>();
            UserMentionData objUserMentionData;
            if (userList != null && userList.Count > 0)
            {
                foreach (var item in userList)
                {
                    objUserMentionData = new UserMentionData();//new UserMentionData();
                    objUserMentionData.userId = namelist.Where(x => x.UserName == item).Select(y => y.PersonnelId).FirstOrDefault();
                    objUserMentionData.userName = item;
                    objUserMentionData.emailId = namelist.Where(x => x.UserName == item).Select(y => y.Email).FirstOrDefault();
                    userMentionDataList.Add(objUserMentionData);
                }
            }
            //else
            //{
            //    userMentionDataList.Add(null);
            //}

            NotesModel notesModel = new NotesModel();
            notesModel.ObjectId = ProjectId;
            notesModel.Content = content;
            notesModel.NotesId = noteId;
            notesModel.ClientLookupId = ClientLookupId;
            notesModel.updatedindex = updatedindex;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateComment(notesModel, ref Mode, "Project", userMentionDataList);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), ProjectId = ProjectId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #endregion

        #region Inner Grid
        public ActionResult GetProjInnerGrid(long ProjectID)
        {
            ProjectVM objProjVM = new ProjectVM();
            ProjectSearchWrapper pWrapper = new ProjectSearchWrapper(userData);
            objProjVM.projectTaskModel = pWrapper.PopulateLineitems(ProjectID);
            LocalizeControls(objProjVM, LocalizeResourceSetConstants.Project);
            return View("~/Views/Project/ProjectSearch/_InnerGridProjectTask.cshtml", objProjVM);
        }
        #endregion

        #region Project task list chunk search
        [HttpPost]
        public string GetProjectTaskByProjectIdGridDataForChunkSearch(int? draw, int? start, int? length, long ProjectId, string WorkOrderClientLookupId = "", string WorkOrderDescription = "", DateTime? StartDate = null, DateTime? EndDate = null,
              string Order = "1", string txtSearchval = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            WorkOrderClientLookupId = WorkOrderClientLookupId.Replace("%", "[%]");
            WorkOrderDescription = WorkOrderDescription.Replace("%", "[%]");
            txtSearchval = txtSearchval.Replace("%", "[%]");
            ProjectDetailsWrapper projectDetailsWrapper = new ProjectDetailsWrapper(userData);
            List<ProjectTaskSearchModel> ProjTaskList = projectDetailsWrapper.GetProjectTaskByProjectIdGridData(ProjectId, WorkOrderClientLookupId, WorkOrderDescription, StartDate, EndDate, order, orderDir, skip, length ?? 0, txtSearchval);

            var totalRecords = 0;
            var recordsFiltered = 0;
            if (ProjTaskList != null && ProjTaskList.Count > 0)
            {
                recordsFiltered = ProjTaskList[0].TotalCount;
                totalRecords = ProjTaskList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = ProjTaskList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, hiddenColumnList = "" }, JsonSerializerDateSettings);
        }
        #endregion

        #region  Update Project Task Start date and End date
        public JsonResult UpdateProjectTaskStartDateAndEndDate(long ProjectTaskId, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            ProjectDetailsWrapper projectDetailsWrapper = new ProjectDetailsWrapper(userData);

            List<string> updateErrorResult = projectDetailsWrapper.UpdateProjectTaskStartDateAndEndDate(ProjectTaskId, StartDate, EndDate);
            if (updateErrorResult == null)
            {

                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #region  Update Project Task Progress
        public JsonResult UpdateProjectTaskProgressVlaue(long ProjectTaskId, string Progress)
        {
            ProjectDetailsWrapper projectDetailsWrapper = new ProjectDetailsWrapper(userData);

            List<string> updateErrorResult = projectDetailsWrapper.UpdateProjectTaskProgress(ProjectTaskId, Progress);
            if (updateErrorResult == null)
            {

                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #endregion

        #region Timeline
        [HttpPost]
        public PartialViewResult TimelineDetails()
        {
            return PartialView("~/Views/Project/TimelineDetails/GanttChart.cshtml");
        }

        [HttpPost]
        public JsonResult UpdateProjectTaskFromTimeLine(ProjectTaskTimelineModel projectTaskTimelineModel)
        {
            TimelineDetailsWrapper wrapper = new TimelineDetailsWrapper(userData);

            List<string> updateErrorResult = wrapper.UpdateProjectTask(projectTaskTimelineModel);
            if (updateErrorResult == null)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString(), ErrorMessages = updateErrorResult }, JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        public JsonResult RetrieveProjectTaskByProjectId(long ProjectId)
        {
            TimelineDetailsWrapper wrapper = new TimelineDetailsWrapper(userData);
            List<ProjectTaskTimelineModel> projectTaskTimelineModels = new List<ProjectTaskTimelineModel>();
            //var project = wrapper.RetrieveProjectByProjectId(ProjectId);
            //if (project != null)
            //{
            //    projectTaskTimelineModels.Add(project);
            //}
            var tasks = wrapper.RetrieveProjectTaskByProjectId(ProjectId);
            if (tasks != null && tasks.Count > 0)
            {
                projectTaskTimelineModels.AddRange(tasks);
            }
            return Json(new { tasks = projectTaskTimelineModels }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region WorkOrder for Project details chunk lookup list
        public JsonResult GetWOForProjectDetailsLookupListchunksearch(int? draw, int? start, int? length, string clientLookupId = "", string ChargeTo = "", string ChargeTo_Name = "", string Description = "", string Status = "", string Priority = "", string Type = "")
        {
            List<WorkOrderForProjectDetailsLookupListModel> modelList = new List<WorkOrderForProjectDetailsLookupListModel>();
            ProjectDetailsWrapper projectDetailsWrapper = new ProjectDetailsWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            ChargeTo = !string.IsNullOrEmpty(ChargeTo) ? ChargeTo.Trim() : string.Empty;
            ChargeTo_Name = !string.IsNullOrEmpty(ChargeTo_Name) ? ChargeTo_Name.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            Status = !string.IsNullOrEmpty(Status) ? Status.Trim() : string.Empty;
            Priority = !string.IsNullOrEmpty(Priority) ? Priority.Trim() : string.Empty;
            Type = !string.IsNullOrEmpty(Type) ? Type.Trim() : string.Empty;
            modelList = projectDetailsWrapper.GetWorkOrderForProjectDetailsLookupListGridData(order, orderDir, skip, length.Value, clientLookupId, ChargeTo, ChargeTo_Name, Description, Status, Priority, Type);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }
            var jsonResult = Json(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Project Dashboard 
        public PartialViewResult ProjectTaskDashboard(long ProjectId)
        {
            ProjectVM objProjectVM = new ProjectVM()
            {
                projectModel = new ProjectModel()
                {
                    ProjectId = ProjectId
                }
            };

            LocalizeControls(objProjectVM, LocalizeResourceSetConstants.Project);
            return PartialView("~/Views/Project/DashboardDetails/_DashboardDetails.cshtml", objProjectVM);
        }

        [HttpPost]
        public ActionResult ProjectTaskDashboardStatuses(long ProjectId)
        {
            List<KeyValuePair<string, long>> entries = new List<KeyValuePair<string, long>>();
            DashboardDetailsWrapper projectTaskWrapper = new DashboardDetailsWrapper(userData);
            entries = projectTaskWrapper.ProjectTaskDashboardStatuses(ProjectId);

            return Json(entries);
        }

        [HttpPost]
        public ActionResult ProjectTaskDashboardScheduleComplianceStatuses(long ProjectId)
        {
            double percentage = 0;
            ProjectTaskScheduleChart scheduleChart = new ProjectTaskScheduleChart();
            List<ProjectTaskStatus> _chart = new List<ProjectTaskStatus>();
            List<KeyValuePair<string, long>> entries = new List<KeyValuePair<string, long>>();
            ProjectTaskStatus projectTaskStatus;
            DashboardDetailsWrapper projectTaskWrapper = new DashboardDetailsWrapper(userData);
            entries = projectTaskWrapper.ProjectTaskDashboardScheduleComplianceStatuses(ProjectId);
            var TotalOfCompleteSeries = entries.Sum(x => x.Key.ToLower() == ProjectTaskConstants.Complete.ToLower() ? x.Value : 0);

            long Total = entries.Sum(x => x.Value);
            //if (Total > 0)
            //{
            //    percentage = ((float)TotalOfCompleteSeries / (float)Total) * 100;
            //    percentage = Math.Round(percentage, 2);
            //}
            foreach (var ent in entries)
            {
                projectTaskStatus = new ProjectTaskStatus();

                if (ent.Key.ToLower() == ProjectTaskConstants.Complete.ToLower())
                {
                    projectTaskStatus.label = UtilityFunction.GetMessageFromResource("GlobalComplete", LocalizeResourceSetConstants.Global);
                }
                else if (ent.Key.ToLower() == ProjectTaskConstants.Incomplete.ToLower())
                {
                    projectTaskStatus.label = UtilityFunction.GetMessageFromResource("GlobalIncomplete", LocalizeResourceSetConstants.Global);
                }
                if (Total > 0)
                {
                    projectTaskStatus.value = Convert.ToDecimal(Math.Round((float)ent.Value / Total * 100, 2));
                }
                else
                {
                    projectTaskStatus.value = ent.Value;
                }
                _chart.Add(projectTaskStatus);
            }

            scheduleChart.chartdata = _chart;
            //scheduleChart.Percent = percentage.ToString() + "%";
            return Json(scheduleChart);
        }
        #endregion

        #region Print Project
        [HttpPost]
        public JsonResult ProjectSetPrintData(ProjPrintParam projPrintParams)
        {
            Session["PRINTPARAMS"] = projPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [EncryptedActionParameter]
        public ActionResult ProjExportASPDF(string d = "")
        {
            ProjectSearchWrapper ProjSearchWrapper = new ProjectSearchWrapper(userData);
            ProjectPDFPrintModel objProjPrintModel;
            ProjectVM objProjectVM = new ProjectVM();
            List<ProjectPDFPrintModel> pSearchModelList = new List<ProjectPDFPrintModel>();
            var locker = new object();

            ProjPrintParam pRPrintParams = (ProjPrintParam)Session["PRINTPARAMS"];

            //List<ProjectSearchModel> pRList = wopWrapper.GetWorkOrderPlanGridData(pRPrintParams.CustomQueryDisplayId, pRPrintParams.colname, pRPrintParams.coldir, 0, 100000);
            List<ProjectSearchModel> ProjList = ProjSearchWrapper.GetProjectGridData(pRPrintParams.CustomQueryDisplayId, pRPrintParams.ClientlookupId, pRPrintParams.Description, pRPrintParams.WorkOrderClientLookupId,
                pRPrintParams.CreateStartDateVw, pRPrintParams.CreateEndDateVw, pRPrintParams.CompleteStartDateVw, pRPrintParams.CompleteEndDateVw, pRPrintParams.CloseStartDateVw, pRPrintParams.CloseEndDateVw,
                pRPrintParams.Status, pRPrintParams.colname, pRPrintParams.coldir, 0, 100000, pRPrintParams.txtSearchval);
            foreach (var p in ProjList)
            {
                objProjPrintModel = new ProjectPDFPrintModel();
                objProjPrintModel.ProjectId = p.ProjectId;
                objProjPrintModel.ClientlookupId = p.ClientlookupId;
                objProjPrintModel.Description = p.Description;
                if (p.ActualStart != null && p.ActualStart != default(DateTime))
                {
                    objProjPrintModel.StartDateString = p.ActualStart.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objProjPrintModel.StartDateString = "";
                }
                if (p.ActualFinish != null && p.ActualFinish != default(DateTime))
                {
                    objProjPrintModel.EndDateString = p.ActualFinish.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objProjPrintModel.EndDateString = "";
                }

                objProjPrintModel.Status = p.Status;
                if (p.Created != null && p.Created != default(DateTime))
                {
                    objProjPrintModel.CreatedDateString = p.Created.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objProjPrintModel.CreatedDateString = "";
                }
                if (p.CompleteDate != null && p.CompleteDate != default(DateTime))
                {
                    objProjPrintModel.CompletedDateString = p.CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objProjPrintModel.CompletedDateString = "";
                }

                if (p.ChildCount > 0)
                {
                    objProjPrintModel.projectTaskmodel = ProjSearchWrapper.PopulateLineitems(p.ProjectId);
                }
                lock (locker)
                {
                    pSearchModelList.Add(objProjPrintModel);
                }
            }
            objProjectVM.projectPDFPrintModel = pSearchModelList;
            objProjectVM.tableHaederProps = pRPrintParams.tableHaederProps;
            LocalizeControls(objProjectVM, LocalizeResourceSetConstants.Project);
            if (d == "excel")
            {
                return GenerateExcelReportPROJ(objProjectVM, "Project List");
            }
            if (d == "csv")
            {
                return GenerateCsvReportPROJ(objProjectVM, "Project List");
            }
            if (d == "d")
            {
                return new PartialViewAsPdf("~/Views/Project/ProjectSearch/ProjGridPdfPrintTemplate.cshtml", objProjectVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    FileName = "Project List.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new ViewAsPdf("~/Views/Project/ProjectSearch/ProjGridPdfPrintTemplate.cshtml", objProjectVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }

        }
        #endregion

        #region ExportToExcel Functionality for Project
        //This code used to handle export functionality
        public ActionResult GenerateExcelReportPROJ(ProjectVM projectVM, string reportName)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {

                CreatePartsForExcelOrCsvWOP(package, projectVM);
                package.Close();
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream.ToArray(),
                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
               string.Concat(reportName, ".xlsx"));
            }

        }
        public ActionResult GenerateCsvReportPROJ(ProjectVM projectVM, string reportName)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {

                CreatePartsForExcelOrCsvWOP(package, projectVM);
                package.Close();
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream.ToArray(),
                  ".csv",
               string.Concat(reportName, ".csv"));
            }

        }
        //This method creating different parts of excel sheet
        private void CreatePartsForExcelOrCsvWOP(SpreadsheetDocument document, ProjectVM data)
        {
            int length = 0;
            List<int> groupRowIndexes = new List<int>();
            int TotalHeaderColumns = 7;
            SheetData partSheetData = GenerateSheetdataForDetailsWOP(data, ref groupRowIndexes, ref TotalHeaderColumns, ref length);
            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPartContent(workbookPart1);
            bool hasBigRows = true;
            //if (length > 50)
            //{
            //    hasBigRows = true;
            //}

            WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
            GenerateWorkbookStylesPartContent(workbookStylesPart1, hasBigRows);

            WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPartContent(worksheetPart1, partSheetData, groupRowIndexes, TotalHeaderColumns, hasBigRows);

        }

        private SheetData GenerateSheetdataForDetailsWOP(ProjectVM projectVM, ref List<int> groupRowIndexes, ref int TotalHeaderColumns, ref int length)
        {
            //creating content of excel sheet logic is same as genere pdf
            SheetData sheetData1 = new SheetData();
            List<string> Parentcells = new List<string>();
            List<string> Childcells = new List<string>();
            List<string> Allcells = new List<string>();
            sheetData1.Append(CreateHeaderRowPROJ(projectVM));

            List<string> headercells = new List<string>();

            foreach (var item in projectVM.projectPDFPrintModel)
            {
                //create header row
                foreach (var hed in projectVM.tableHaederProps)
                {
                    if (!string.IsNullOrWhiteSpace(hed.property))
                    {
                        headercells.Add(item.GetType().GetProperty(hed.property).GetValue(item, null).ToString());
                    }

                }
                sheetData1.Append(CreateRowData(headercells, TotalHeaderColumns, 1U));
                headercells.Clear();
                if (item.projectTaskmodel.Count > 0)
                {
                    //Create row
                    headercells.Add("Work Order ID");
                    headercells.Add("Description");
                    headercells.Add("Start");
                    headercells.Add("End");
                    headercells.Add("Progress");

                    sheetData1.Append(CreateRowData(headercells, TotalHeaderColumns, 2U));
                    headercells.Clear();
                    ////eEnd row

                    foreach (var line in item.projectTaskmodel)
                    {
                        Childcells.Add(line.WorkOrderClientLookupId.ToString());
                        Childcells.Add(line.WorkOrderDescription);
                        Childcells.Add(line.StartDate);
                        Childcells.Add(line.Enddate);
                        string pervalue = line.ProgressPercentage.ToString() + "%";
                        Childcells.Add(pervalue);


                        sheetData1.Append(CreateRowData(Childcells, TotalHeaderColumns, 1U));
                        Childcells.Clear();

                    }

                    //Create row
                }
            }

            return sheetData1;
        }

        //create header row parent grid
        private Row CreateHeaderRowPROJ(ProjectVM data)
        {
            var headrs = new List<string>();
            headrs = (from d in data.tableHaederProps.ToList().Where(x => x.title != null && x.title.Length > 0).ToList()
                      select d.title).ToList();
            return CreateRowData(headrs, 7, 2U);
        }


        #endregion

        #region Add or Edit Project
        public PartialViewResult AddorEditProject(long ProjectId)
        {
            ProjectSearchWrapper ProjWrapper = new ProjectSearchWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            ProjectVM projectVM = new ProjectVM();
            projectVM.projectAddorEdirModel = new ProjectAddOrEditModel();
            if (ProjectId != 0)
            {
                ProjectDetailsWrapper ProjdetailsWrapper = new ProjectDetailsWrapper(userData);
                projectVM.projectAddorEdirModel = ProjdetailsWrapper.RetrieveProjectRecordByProjectID(ProjectId);
                projectVM.projectAddorEdirModel.PageType = "Edit";
            }
            else
            {
                projectVM.projectAddorEdirModel.PageType = "Add";
            }
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                TypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Project_Type).ToList();

                if (TypeList != null)
                {
                    projectVM.projectAddorEdirModel.TypeList = TypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }

            }
            var plist = comWrapper.PersonnelListForActiveFullUser();
            projectVM.projectAddorEdirModel.OwnerPersonnelList = plist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.PersonnelId.ToString() }).ToList();
            projectVM.projectAddorEdirModel.CoorPersonnelList = plist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.PersonnelId.ToString() }).ToList();

            LocalizeControls(projectVM, LocalizeResourceSetConstants.Project);
            return PartialView("~/Views/Project/ProjectSearch/_ProjectAddOrEdit.cshtml", projectVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddorEditProject(ProjectVM objProjectVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;

            if (ModelState.IsValid)
            {
                DataContracts.Project PROJ = new DataContracts.Project();
                ProjectSearchWrapper ProjWrapper = new ProjectSearchWrapper(userData);

                PROJ = ProjWrapper.AddorEditProject(objProjectVM);

                if (PROJ.ErrorMessages != null && PROJ.ErrorMessages.Count > 0)
                {
                    return Json(PROJ.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, projectId = PROJ.ProjectId, clientLookupId = PROJ.ClientLookupId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}