using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.WorkOrderPlanning;
using Client.Common;
using Client.Common.Constants;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.Common.Charts.Fusions;
using Client.Models.WorkOrderPlanning;

using Common.Constants;

using DataContracts;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Newtonsoft.Json;

using Rotativa;
using Rotativa.Options;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using static Client.Models.Common.UserMentionDataModel;

namespace Client.Controllers.WorkOrderPlanning
{
    public class WorkOrderPlanningController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.WorkOrder_Planning)]
        public ActionResult Index()
        {
            WorkOrderPlanningVM objWorkOrderPlanningVM = new WorkOrderPlanningVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objWorkOrderPlanningVM.WorkOrderPlanViewList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.WorkOrderPlan);
            LocalizeControls(objWorkOrderPlanningVM, LocalizeResourceSetConstants.WorkOrderPlanning);
            return View(objWorkOrderPlanningVM);
        }

        #region Work Order Planning Search
        [HttpPost]
        public string GetWoPlanningGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, string Order = "1", string txtSearchval = "")
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var filter = CustomQueryDisplayId;
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            WorkOrderPlanningSearchWrapper WOPSearchWrapper = new WorkOrderPlanningSearchWrapper(userData);
            List<WorkOrderPlanningModel> WOPList = WOPSearchWrapper.GetWorkOrderPlanGridData(CustomQueryDisplayId, Order, orderDir, skip, length ?? 0);

            var totalRecords = 0;
            var recordsFiltered = 0;
            if (WOPList != null && WOPList.Count > 0)
            {
                recordsFiltered = WOPList[0].TotalCount;
                totalRecords = WOPList[0].TotalCount;
                //recordsFiltered = 3;
                //totalRecords = 3;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = WOPList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, hiddenColumnList = "" }, JsonSerializerDateSettings);
        }
        public ActionResult GetWOPInnerGrid(long WorkOrderPlanID)
        {
            WorkOrderPlanningVM objWOPVM = new WorkOrderPlanningVM();
            WorkOrderPlanningSearchWrapper pWrapper = new WorkOrderPlanningSearchWrapper(userData);
            objWOPVM.woPlanLineitemModel = pWrapper.PopulateLineitems(WorkOrderPlanID);
            LocalizeControls(objWOPVM, LocalizeResourceSetConstants.WorkOrderPlanning);
            return View("_InnerGridWOPLineItem", objWOPVM);
        }
        #endregion

        #region WOP Details Plan
        public PartialViewResult Details(long WorkOrderPlanId)
        {
            WorkOrderPlanningVM objWorkOrderPlanningVM = new WorkOrderPlanningVM();
            WorkOrderPlanningModel WOPModel = new WorkOrderPlanningModel();
            WOPModel.WorkOrderPlanId = WorkOrderPlanId;
            objWorkOrderPlanningVM.workorderPlanningModel = WOPModel;
            LocalizeControls(objWorkOrderPlanningVM, LocalizeResourceSetConstants.WorkOrderPlanning);
            return PartialView("_WorkOrderPlanningDetails", objWorkOrderPlanningVM);
        }
        [HttpPost]
        public PartialViewResult PlanHeaderList(string WorkOrderPlanID)
        {
            long WorkOrderPlanIDs = string.IsNullOrEmpty(WorkOrderPlanID) ? 0 : Convert.ToInt64(WorkOrderPlanID);
            WorkOrderPlanningVM objWorkOrderPlanningVM = new WorkOrderPlanningVM();
            WorkOrderPlanningDetailsPlanWrapper WOPlanSummary = new WorkOrderPlanningDetailsPlanWrapper(userData);
            objWorkOrderPlanningVM.workorderPlanSummaryModel = WOPlanSummary.getWorkOderPlanDetailsByWorkOrderPlanId(WorkOrderPlanIDs);
            objWorkOrderPlanningVM._userdata = this.userData;
            LocalizeControls(objWorkOrderPlanningVM, LocalizeResourceSetConstants.WorkOrderPlanning);
            return PartialView("~/Views/WorkOrderPlanning/WorkOrderPlan/_WorkOrderPlanDetailsHeader.cshtml", objWorkOrderPlanningVM);
        }

        public PartialViewResult WorkOrderPlannigDashboard(long WorkOrderPlanID)
        {
            WorkOrderPlanningVM objWorkOrderPlanningVM = new WorkOrderPlanningVM()
            {
                workorderPlanningModel = new WorkOrderPlanningModel()
                {
                    WorkOrderPlanId = WorkOrderPlanID
                }
            };

            LocalizeControls(objWorkOrderPlanningVM, LocalizeResourceSetConstants.WorkOrderPlanning);

            return PartialView("~/Views/WorkOrderPlanning/WorkOrderPlan/_WorkOrderPlanDashboard.cshtml", objWorkOrderPlanningVM);
        }

        public JsonResult AddWOPlanLineItem(string[] WOPlanLineItem, string workOrderPlanId, string Type)
        {
            var errorList = new List<string>();
            if (WOPlanLineItem.Length > 0 && !string.IsNullOrEmpty(workOrderPlanId))
            {
                long workOrderPlanIds = string.IsNullOrEmpty(workOrderPlanId) ? 0 : Convert.ToInt64(workOrderPlanId);
                WorkOrderPlanningDetailsPlanWrapper WOPlanSummary = new WorkOrderPlanningDetailsPlanWrapper(userData);

                errorList = WOPlanSummary.AddWOPlanLineItem(WOPlanLineItem, workOrderPlanIds, Type);
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

        #region  WO WorkOrder PLan chunk lookup list
        public JsonResult GetWOWorkOrderPLannLookupListchunksearch(int? draw, int? start, int? length, string clientLookupId = "", string ChargeTo = "", string ChargeTo_Name = "", string Description = "", string Status = "", string Priority = "", string Type = "")
        {
            List<WorkOrderWOPlanLookupListModel> modelList = new List<WorkOrderWOPlanLookupListModel>();
            WorkOrderPlanningDetailsPlanWrapper woPlanningDetailsPlanWrapper = new WorkOrderPlanningDetailsPlanWrapper(userData);
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
            modelList = woPlanningDetailsPlanWrapper.GetWorkOrderWOPlanLookupListGridData(order, orderDir, skip, length.Value, clientLookupId, ChargeTo, ChargeTo_Name, Description, Status, Priority, Type);

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
        public JsonResult GetWOWorkOrderPLanLookupListSelectAllData(string colname, string coldir, string clientLookupId = "", string ChargeTo = "", string ChargeTo_Name = "", string Description = "", string Status = "", string Priority = "", string Type = "")
        {
            List<WorkOrderWOPlanLookupListModel> modelList = new List<WorkOrderWOPlanLookupListModel>();
            WorkOrderPlanningDetailsPlanWrapper woPlanningDetailsPlanWrapper = new WorkOrderPlanningDetailsPlanWrapper(userData);
            ChargeTo = !string.IsNullOrEmpty(ChargeTo) ? ChargeTo.Trim() : string.Empty;
            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            ChargeTo_Name = !string.IsNullOrEmpty(ChargeTo_Name) ? ChargeTo_Name.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            Status = !string.IsNullOrEmpty(Status) ? Status.Trim() : string.Empty;
            Priority = !string.IsNullOrEmpty(Priority) ? Priority.Trim() : string.Empty;
            Type = !string.IsNullOrEmpty(Type) ? Type.Trim() : string.Empty;
            modelList = woPlanningDetailsPlanWrapper.GetWorkOrderWOPlanLookupListGridData(colname, coldir, 0, 100000, clientLookupId, ChargeTo, ChargeTo_Name, Description, Status, Priority, Type);
            // return Json(modelList, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(modelList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult RemoveWOPlanLineItem(string[] WOPlanLineItem)
        {
            var errorList = new List<string>();
            if (WOPlanLineItem.Length > 0)
            {
                WorkOrderPlanningDetailsPlanWrapper WOPlanningDetailsPlanWrapper = new WorkOrderPlanningDetailsPlanWrapper(userData);

                errorList = WOPlanningDetailsPlanWrapper.RemoveWOPlanLineItem(WOPlanLineItem);
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

        #region WorkOrder Plan Edit
        public PartialViewResult EditWorkOderPlan(long WorkOrderPlanID)
        {
            WorkOrderPlanningVM objWorkOrderPlanningVM = new WorkOrderPlanningVM();
            WorkOrderPlanningModel objWoprkrderPlanModel = new WorkOrderPlanningModel();
            WorkOrderPlanningResourceListWrapper workOrderPlanningResourceListWrapper = new WorkOrderPlanningResourceListWrapper(userData);
            WorkOrderPlanningDetailsPlanWrapper WOPlan = new WorkOrderPlanningDetailsPlanWrapper(userData);
            objWoprkrderPlanModel = WOPlan.getWorkOrderPlan_RetrieveByWorkOrderPlanId(WorkOrderPlanID);
            var totalList = workOrderPlanningResourceListWrapper.SchedulePersonnelList();
            if (totalList != null && totalList.Count > 0)
            {
                objWoprkrderPlanModel.PersonnelList = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();

            }
            objWorkOrderPlanningVM.workorderPlanningModel = objWoprkrderPlanModel;
            LocalizeControls(objWorkOrderPlanningVM, LocalizeResourceSetConstants.WorkOrderPlanning);
            return PartialView("~/Views/WorkOrderPlanning/WorkOrderPlan/_EditWorkOrderPlan.cshtml", objWorkOrderPlanningVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditWorkOrderPlan(WorkOrderPlanningVM objWorkOrderPlanVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            List<string> errList = new List<string>();
            if (ModelState.IsValid)
            {
                WorkOrderPlanningDetailsPlanWrapper WOPWrapper = new WorkOrderPlanningDetailsPlanWrapper(userData);
                var objValue = WOPWrapper.EditWorkOrderPlan(objWorkOrderPlanVM);
                errList = objValue.Item1;
                long WorkOrderPlanID = objValue.Item2;
                if (errList != null && errList.Count > 0)
                {
                    return Json(errList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, workOrderPlanId = WorkOrderPlanID, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetWorkOrderGridDataForWorkOrderPlan(int? draw, int? start, int? length, string workOrderPlanId = "", string equipmentClientLookupId = "", string chargeToName = "", string description = "",
                               DateTime? requiredDate = null, string type = "", string searchText = "", string orderForWO = "0"
       )
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            WorkOrderPlanningVM workOrderPlanningVM = new WorkOrderPlanningVM();
            List<WorkOrderForWorkOrderPlanModel> workOrderForWorkOrderPlanList = new List<WorkOrderForWorkOrderPlanModel>();
            searchText = searchText.Replace("%", "[%]");
            workOrderPlanId = workOrderPlanId.Replace("%", "[%]");
            equipmentClientLookupId = equipmentClientLookupId.Replace("%", "[%]");
            chargeToName = chargeToName.Replace("%", "[%]");
            description = description.Replace("%", "[%]");
            type = type.Replace("%", "[%]");
            searchText = searchText.Replace("%", "[%]");
            long workOrderPlanIdBigint = string.IsNullOrEmpty(workOrderPlanId) ? 0 : Convert.ToInt64(workOrderPlanId);
            string _requiredDate = string.Empty;
            _requiredDate = requiredDate.HasValue ? requiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;

            WorkOrderPlanningDetailsPlanWrapper workOrderPlanningWrapper = new WorkOrderPlanningDetailsPlanWrapper(userData);
            workOrderForWorkOrderPlanList = workOrderPlanningWrapper.GetWorkOrderGridDataForWorkOrderPlan(order, orderDir, skip, length ?? 0, workOrderPlanIdBigint, equipmentClientLookupId, chargeToName, description, _requiredDate, type, searchText);

            var totalRecords = 0;
            var recordsFiltered = 0;

            if (workOrderForWorkOrderPlanList != null && workOrderForWorkOrderPlanList.Count > 0)
            {
                recordsFiltered = workOrderForWorkOrderPlanList[0].TotalCount;
                totalRecords = workOrderForWorkOrderPlanList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;

            var filteredResult = workOrderForWorkOrderPlanList
              .ToList();


            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        public JsonResult GetWorkOrderGridDataForWorkOrderPlanSelectAllData(string colname, string coldir, string workOrderPlanId = "", string equipmentClientLookupId = "", string chargeToName = "", string description = "",
                             DateTime? requiredDate = null, string type = "", string searchText = "", string Order = "0")
        {

            searchText = searchText.Replace("%", "[%]");
            workOrderPlanId = workOrderPlanId.Replace("%", "[%]");
            equipmentClientLookupId = equipmentClientLookupId.Replace("%", "[%]");
            chargeToName = chargeToName.Replace("%", "[%]");
            description = description.Replace("%", "[%]");
            type = type.Replace("%", "[%]");
            searchText = searchText.Replace("%", "[%]");
            string _requiredDate = string.Empty;
            _requiredDate = requiredDate.HasValue ? requiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            long workOrderPlanIdBigint = string.IsNullOrEmpty(workOrderPlanId) ? 0 : Convert.ToInt64(workOrderPlanId);
            WorkOrderPlanningDetailsPlanWrapper workOrderPlanningWrapper = new WorkOrderPlanningDetailsPlanWrapper(userData);
            var modelList = workOrderPlanningWrapper.GetWorkOrderGridDataForWorkOrderPlan(colname, coldir, 0, 100000, workOrderPlanIdBigint, equipmentClientLookupId, chargeToName, description, _requiredDate, type, searchText);

            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatingStatusPlanWorkOderPlan(long WorkOrderPlanID, string Status)
        {
            var errorList = new List<string>();
            if (WorkOrderPlanID > 0)
            {
                WorkOrderPlanningDetailsPlanWrapper WOPlanningDetailsPlanWrapper = new WorkOrderPlanningDetailsPlanWrapper(userData);

                errorList = WOPlanningDetailsPlanWrapper.WorkOrderPlanStatusUpdating(WorkOrderPlanID, Status);
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

        #region Add schedule Plan     
        [HttpPost]
        public PartialViewResult AddSchedulePlanModal(long WorkOrderPlanId)
        {
            WorkOrderPlanningResourceCalendarWrapper wrapper = new WorkOrderPlanningResourceCalendarWrapper(userData);
            WorkOrderPlanningVM workOrderPlanningVM = new WorkOrderPlanningVM();
            var totalList = wrapper.SchedulePersonnelListByAssetGroupMasterQuery();
            if (totalList != null && totalList.Count > 0)
            {
                workOrderPlanningVM.PersonnelList = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            }
            LocalizeControls(workOrderPlanningVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrderPlanning/WorkOrderPlan/_AddScheduleModal.cshtml", workOrderPlanningVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddScheduleWOP(WorkOrderPlanningVM workOrderPlanningVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid && !string.IsNullOrEmpty(workOrderPlanningVM.WOPScheduleModel.WorkOrderIds))
            {
                string[] WorkorderItemIds = workOrderPlanningVM.WOPScheduleModel.WorkOrderIds.Split(',');
                long[] workorderItemIds = WorkorderItemIds.Select(n => Convert.ToInt64(n)).ToArray();
                WorkOrderPlanningDetailsPlanWrapper wrapper = new WorkOrderPlanningDetailsPlanWrapper(userData);
                var ErrorMessages = wrapper.AddScheduleRecord(workOrderPlanningVM.WOPScheduleModel, workorderItemIds);
                if (ErrorMessages != null && ErrorMessages.Count > 0)
                {
                    return Json(ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
        public JsonResult RemoveScheduleWOP(long WorkOrderPlanID, string[] WOPlanLineItem)
        {
            WorkOrderPlan objWorkOrder = new WorkOrderPlan();
            if (WorkOrderPlanID != 0 && WOPlanLineItem.Length != 0)
            {
                long[] WorkorderItemIds = WOPlanLineItem.Select(n => Convert.ToInt64(n)).ToArray();
                WorkOrderPlanningDetailsPlanWrapper planwrapper = new WorkOrderPlanningDetailsPlanWrapper(userData);


                for (int i = 0; i < WorkorderItemIds.Length; i++)
                {
                    long WorkOrderId = planwrapper.getWorkOrderIdByWorkorderItemId(WorkorderItemIds[i]);
                    var WorkOrderSchedData = planwrapper.GetScheduleIDsByworkOrderId(WorkOrderId).ToList();
                    foreach (var obj in WorkOrderSchedData)
                    {
                        objWorkOrder = planwrapper.RemoveWorkOrderSchedule(obj.WorkOrderId, obj.WorkOrderSchedId);
                        if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                        {
                            break;
                        }
                    }

                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        break;
                    }
                }

                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public PartialViewResult LoadComments(long WorkOrderPlanId)
        {
            WorkOrderPlanningVM objWorksOrderVM = new WorkOrderPlanningVM();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Personnel> personnelsList = new List<Personnel>();
            List<Notes> NotesList = new List<Notes>();
            UserMentionData userMentionData;
            List<UserMentionData> userMentionDatas = new List<UserMentionData>();

            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => personnelsList = coWrapper.MentionList(""));
            tasks[1] = Task.Factory.StartNew(() => NotesList = coWrapper.PopulateComment(WorkOrderPlanId, "WorkOrderPlan"));
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
                objWorksOrderVM.userMentionData = userMentionDatas;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                objWorksOrderVM.NotesList = NotesList;
            }
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderPlanning);
            return PartialView("~/Views/WorkOrderPlanning/WorkOrderPlan/_WorkOrderPlanDetailsComments.cshtml", objWorksOrderVM);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddComments(string WorkOrderPlanId, string content, string woClientLookupId, List<string> userList, long noteId = 0, long updatedindex = 0)
        {
            long workOrderPlanIdBigint = string.IsNullOrEmpty(WorkOrderPlanId) ? 0 : Convert.ToInt64(WorkOrderPlanId);
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
            notesModel.ObjectId = workOrderPlanIdBigint;
            notesModel.Content = content;
            notesModel.NotesId = noteId;
            notesModel.ClientLookupId = woClientLookupId;
            notesModel.updatedindex = updatedindex;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateComment(notesModel, ref Mode, "WorkOrderPlan", userMentionDataList);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), WorkOrderPlanId = workOrderPlanIdBigint, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        #region Activity Event Log

        [HttpPost]
        public PartialViewResult LoadActivity(long WorkOrderPlanId)
        {
            WorkOrderPlanningVM objWorksOrderVM = new WorkOrderPlanningVM();
            WorkOrderPlanningDetailsPlanWrapper wopWrapper = new WorkOrderPlanningDetailsPlanWrapper(userData);
            List<EventLogModel> EventLogList = wopWrapper.PopulateEventLog(WorkOrderPlanId);
            objWorksOrderVM.EventLogList = EventLogList;
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderPlanning);
            return PartialView("~/Views/WorkOrderPlanning/WorkOrderPlan/_WorkOrderPlanDetailsActivity.cshtml", objWorksOrderVM);
        }
        #endregion
        #endregion

        #region WOP Resource List
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetResourceListGridData(int? draw, int? start, int? length, long WorkOrderPlanId, string ClientLookupId = "", string Name = "", string Description = "", DateTime? RequiredDate = null, string Type = "", List<string> PersonnelList = null, string SearchText = "", string Order = "0"
       )
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            string _requiredDate = string.Empty;
            SearchText = SearchText.Replace("%", "[%]");
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            Description = Description.Replace("%", "[%]");
            Type = Type.Replace("%", "[%]");
            _requiredDate = RequiredDate.HasValue ? RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            List<string> typeList = new List<string>();
            WorkOrderPlanningResourceListWrapper workOrderPlanningResourceListWrapper = new WorkOrderPlanningResourceListWrapper(userData);
            List<ResourceListSearchModel> ResourceList = workOrderPlanningResourceListWrapper.GetResourceListGridData(WorkOrderPlanId, Order, orderDir, skip, length ?? 0, ClientLookupId, Name, Description, _requiredDate, Type, PersonnelList, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (ResourceList != null && ResourceList.Count > 0)
            {
                recordsFiltered = ResourceList[0].TotalCount;
                totalRecords = ResourceList[0].TotalCount;

            }
            int initialPage = start.Value;
            var filteredResult = ResourceList
              .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetResourceListPrintData(string colname, string coldir, long WorkOrderPlanId, string ClientLookupId = "", string Name = "", string Description = "", DateTime? RequiredDate = null, string Type = "", List<string> PersonnelList = null, string SearchText = ""
)
        {
            List<ResourceListPrintModel> ResourceListPrintModelList = new List<ResourceListPrintModel>();
            ResourceListPrintModel objResourceListPrintModel;
            string _requiredDate = string.Empty;
            string _startScheduledDate = string.Empty;
            string _endScheduledDate = string.Empty;
            SearchText = SearchText.Replace("%", "[%]");
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            Description = Description.Replace("%", "[%]");
            Type = Type.Replace("%", "[%]");
            _requiredDate = RequiredDate.HasValue ? RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            List<string> typeList = new List<string>();
            WorkOrderPlanningResourceListWrapper workOrderPlanningResourceListWrapper = new WorkOrderPlanningResourceListWrapper(userData);
            List<ResourceListSearchModel> ResourceList = workOrderPlanningResourceListWrapper.GetResourceListGridData(WorkOrderPlanId, colname, coldir, 0, 100000, ClientLookupId, Name, Description, _requiredDate, Type, PersonnelList, SearchText);

            foreach (var Resource in ResourceList)
            {
                objResourceListPrintModel = new ResourceListPrintModel();
                objResourceListPrintModel.PersonnelName = Resource.PersonnelName;
                objResourceListPrintModel.WorkOrderClientLookupId = Resource.WorkOrderClientLookupId;
                objResourceListPrintModel.Description = Resource.Description;
                objResourceListPrintModel.Type = Resource.Type;
                objResourceListPrintModel.ScheduledStartDate = Resource.ScheduledStartDate;
                objResourceListPrintModel.ScheduledHours = Resource.ScheduledHours;
                objResourceListPrintModel.RequiredDate = Resource.RequiredDate;
                objResourceListPrintModel.EquipmentClientLookupId = Resource.EquipmentClientLookupId;
                objResourceListPrintModel.ChargeTo_Name = Resource.ChargeTo_Name;

                ResourceListPrintModelList.Add(objResourceListPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = ResourceListPrintModelList }, JsonSerializerDateSettings);
        }
        public JsonResult SetPrintDataRS(RSPrintParams RSPrintParams)
        {
            Session["RSPRINTPARAMS"] = RSPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public ActionResult ExportASPDFRS(string d = "")
        {
            WorkOrderPlanningResourceListWrapper workOrderPlanningResourceListWrapper = new WorkOrderPlanningResourceListWrapper(userData);
            ResourceListPdfPrintModel objResourceListPdfPrintModel;
            List<ResourceListPdfPrintModel> ResourceListPdfPrintModelList = new List<ResourceListPdfPrintModel>();
            WorkOrderPlanningVM workOrderPlanningVM = new WorkOrderPlanningVM();
            var locker = new object();

            RSPrintParams RSPrintParams = (RSPrintParams)Session["RSPRINTPARAMS"];
            List<ResourceListSearchModel> ResourceList = workOrderPlanningResourceListWrapper.GetResourceListGridData(RSPrintParams.WorkOrderPlanId, RSPrintParams.colname, RSPrintParams.coldir, 0, 100000, RSPrintParams.ClientLookupId, RSPrintParams.Name, RSPrintParams.Description, RSPrintParams.RequiredDate, RSPrintParams.Type, RSPrintParams.PersonnelList, RSPrintParams.SearchText);

            foreach (var Resource in ResourceList)
            {
                objResourceListPdfPrintModel = new ResourceListPdfPrintModel();
                objResourceListPdfPrintModel.PersonnelName = Resource.PersonnelName;
                objResourceListPdfPrintModel.WorkOrderClientLookupId = Resource.WorkOrderClientLookupId;
                objResourceListPdfPrintModel.Description = Resource.Description;
                objResourceListPdfPrintModel.Type = Resource.Type;
                if (Resource.ScheduledStartDate != null && Resource.ScheduledStartDate != default(DateTime))
                {
                    objResourceListPdfPrintModel.ScheduledStartDateString = Resource.ScheduledStartDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objResourceListPdfPrintModel.ScheduledStartDateString = "";
                }
                objResourceListPdfPrintModel.ScheduledHours = Resource.ScheduledHours;
                if (Resource.RequiredDate != null && Resource.RequiredDate != default(DateTime))
                {
                    objResourceListPdfPrintModel.RequiredDateString = Resource.RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objResourceListPdfPrintModel.RequiredDateString = "";
                }
                objResourceListPdfPrintModel.EquipmentClientLookupId = Resource.EquipmentClientLookupId;
                objResourceListPdfPrintModel.ChargeTo_Name = Resource.ChargeTo_Name;
                objResourceListPdfPrintModel.PerNextValue = Resource.PerNextValue;
                objResourceListPdfPrintModel.SDNextValue = Resource.SDNextValue;
                objResourceListPdfPrintModel.SumPersonnelHour = Resource.SumPersonnelHour;
                objResourceListPdfPrintModel.SumScheduledateHour = Resource.SumScheduledateHour;
                objResourceListPdfPrintModel.GrandTotalHour = Resource.GrandTotalHour;
                objResourceListPdfPrintModel.PerIDNextValue = Resource.PerIDNextValue;
                objResourceListPdfPrintModel.PersonnelId = Resource.PersonnelId;
                objResourceListPdfPrintModel.WorkOrderScheduleId = Resource.WorkOrderScheduleId;
                objResourceListPdfPrintModel.GroupType = RSPrintParams.colname;
                lock (locker)
                {
                    ResourceListPdfPrintModelList.Add(objResourceListPdfPrintModel);
                }
            }
            workOrderPlanningVM.resourceListPdfPrintModel = ResourceListPdfPrintModelList;
            workOrderPlanningVM.tableHaederProps = RSPrintParams.tableHaederProps;
            LocalizeControls(workOrderPlanningVM, LocalizeResourceSetConstants.Global);

            if (d == "excel")
            {
                return GenerateExcelReport(workOrderPlanningVM, "Resource List");
            }
            else if (d == "csv")
            {
                return GenerateCsvReport(workOrderPlanningVM, "Resource List");
            }
            if (d == "d")
            {
                return new Rotativa.PartialViewAsPdf("ResourceList/WorkOrderPlanningResourceListPdfPrintTemplate", workOrderPlanningVM)
                {
                    PageSize = Size.A4,
                    FileName = "ResourceList.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new ViewAsPdf("ResourceList/WorkOrderPlanningResourceListPdfPrintTemplate", workOrderPlanningVM)
                {
                    PageSize = Size.A4,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
        }
        #region ExportToExcelOrcsvFunctionality
        //This code used to handle export functionality
        public ActionResult GenerateExcelReport(WorkOrderPlanningVM workOrderPlanningVM, string reportName)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {

                CreatePartsForExcelOrCsv(package, workOrderPlanningVM);
                package.Close();
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream.ToArray(),
                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
               string.Concat(reportName, ".xlsx"));
            }

        }
        public ActionResult GenerateCsvReport(WorkOrderPlanningVM workOrderPlanningVM, string reportName)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {

                CreatePartsForExcelOrCsv(package, workOrderPlanningVM);
                package.Close();
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream.ToArray(),
                  ".csv",
               string.Concat(reportName, ".csv"));
            }

        }
        //This method creating different parts of excel sheet
        private void CreatePartsForExcelOrCsv(SpreadsheetDocument document, WorkOrderPlanningVM data)
        {
            int length = 0;
            List<int> groupRowIndexes = new List<int>();
            int TotalHeaderColumns = 0;
            SheetData partSheetData = GenerateSheetdataForDetails(data, ref groupRowIndexes, ref TotalHeaderColumns, ref length);
            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPartContent(workbookPart1);
            bool hasBigRows = false;
            if (length > 50)
            {
                hasBigRows = true;
            }

            WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
            GenerateWorkbookStylesPartContent(workbookStylesPart1, hasBigRows);

            WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPartContent(worksheetPart1, partSheetData, groupRowIndexes, TotalHeaderColumns, hasBigRows);

        }

        private SheetData GenerateSheetdataForDetails(WorkOrderPlanningVM workOrderPlanningVM, ref List<int> groupRowIndexes, ref int TotalHeaderColumns, ref int length)
        {
            //creating content of excel sheet logic is same as genere pdf
            SheetData sheetData1 = new SheetData();
            List<string> Parentcells = new List<string>();
            List<string> Childcells = new List<string>();
            List<string> Allcells = new List<string>();

            string prevGroup = workOrderPlanningVM.resourceListPdfPrintModel.Select(x => x.PersonnelName).FirstOrDefault().ToString();
            string currentGroup = prevGroup;
            long PerIDNextValue = workOrderPlanningVM.resourceListPdfPrintModel.Select(x => x.PerIDNextValue).FirstOrDefault();
            long PersonnelId = 0;
            int printCount = 0;
            int thisRow = 0;
            decimal GrandTotal = workOrderPlanningVM.resourceListPdfPrintModel.Select(x => x.GrandTotalHour).FirstOrDefault();
            string SDNextValue = workOrderPlanningVM.resourceListPdfPrintModel.Select(x => x.SDNextValue).FirstOrDefault().ToString();
            string GroupType = workOrderPlanningVM.resourceListPdfPrintModel.Select(x => x.GroupType).FirstOrDefault();

            int headercount = (from d in workOrderPlanningVM.tableHaederProps.ToList().Where(x => x.title != null && x.title.Length > 0).ToList()
                               select d.title).ToList().Count;
            TotalHeaderColumns = headercount;

            sheetData1.Append(CreateHeaderRow(workOrderPlanningVM));



            foreach (var item in workOrderPlanningVM.resourceListPdfPrintModel)
            {
                PerIDNextValue = item.PerIDNextValue;
                PersonnelId = item.PersonnelId;
                if (GroupType == "0")
                {
                    prevGroup = item.PersonnelName;
                }
                else
                {
                    prevGroup = item.ScheduledStartDateString;
                }


                if (printCount == 0)
                {
                    printCount = 1;

                    //create group header 
                    sheetData1.Append(CreateGroupHeader(prevGroup, headercount));
                    groupRowIndexes.Add(sheetData1.ChildElements.Count);
                }



                if (thisRow % 2 == 0)
                {

                    foreach (var hed in workOrderPlanningVM.tableHaederProps)
                    {
                        if (!string.IsNullOrEmpty(hed.title))
                        {
                            Childcells.Add(item.GetType().GetProperty(hed.property).GetValue(item, null).ToString());
                        }
                    }
                    //create row for child grid
                    sheetData1.Append(CreateRowData(Childcells, TotalHeaderColumns, 1U));
                    Allcells.AddRange(Childcells);
                    Childcells.Clear();

                }
                else
                {

                    foreach (var hed in workOrderPlanningVM.tableHaederProps)
                    {
                        if (!string.IsNullOrEmpty(hed.title))
                        {
                            Childcells.Add(item.GetType().GetProperty(hed.property).GetValue(item, null).ToString());
                        }
                    }
                    //create row for child cells    
                    sheetData1.Append(CreateRowData(Childcells, TotalHeaderColumns, 1U));
                    Allcells.AddRange(Childcells);
                    Childcells.Clear();

                }

                if (GroupType == "0")
                {
                    if (PerIDNextValue != PersonnelId && (!String.IsNullOrEmpty(item.PerNextValue)))
                    {
                        printCount = 1;
                        prevGroup = item.PerNextValue;

                        //create row
                        foreach (var hed in workOrderPlanningVM.tableHaederProps)
                        {
                            if (!string.IsNullOrEmpty(hed.title))
                            {
                                if (hed.title == "Hours")
                                {
                                    Parentcells.Add(item.SumPersonnelHour.ToString());
                                }
                                else
                                {
                                    //blank td
                                    Parentcells.Add("");
                                }
                            }

                        }

                        sheetData1.Append(CreateRowData(Parentcells, TotalHeaderColumns, 2U));
                        //creating row
                        Allcells.AddRange(Parentcells);
                        Parentcells.Clear();

                        //create row for group cell
                        sheetData1.Append(CreateGroupHeader(prevGroup, headercount));
                        groupRowIndexes.Add(sheetData1.ChildElements.Count);

                    }

                    //Print Total Hour count on the last
                    if ((String.IsNullOrEmpty(item.PerNextValue)))
                    {
                        foreach (var hed in workOrderPlanningVM.tableHaederProps)
                        {
                            if (!string.IsNullOrEmpty(hed.title))
                            {
                                if (hed.title == "Hours")
                                {
                                    //create cell
                                    Parentcells.Add(item.SumPersonnelHour.ToString());
                                }
                                else
                                {
                                    Parentcells.Add("");
                                    //create cell
                                }

                            }
                        }
                        //create row darker using 2u
                        sheetData1.Append(CreateRowData(Parentcells, TotalHeaderColumns, 2U));
                        Allcells.AddRange(Parentcells);
                        Parentcells.Clear();

                    }
                }
                else
                {
                    SDNextValue = item.SDNextValue.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    if (SDNextValue != item.ScheduledStartDateString && (item.SDNextValue != DateTime.MinValue))
                    {
                        printCount = 1;
                        prevGroup = SDNextValue;
                        foreach (var hed in workOrderPlanningVM.tableHaederProps)
                        {
                            if (!string.IsNullOrEmpty(hed.title))
                            {
                                if (hed.title == "Hours")
                                {
                                    Parentcells.Add(item.SumScheduledateHour.ToString());
                                }
                                else
                                {
                                    Parentcells.Add("");
                                }
                            }

                        }

                        //create row
                        sheetData1.Append(CreateRowData(Parentcells, TotalHeaderColumns, 2U));
                        Allcells.AddRange(Parentcells);
                        Parentcells.Clear();

                        //creat parent row
                        sheetData1.Append(CreateGroupHeader(prevGroup, headercount));
                        groupRowIndexes.Add(sheetData1.ChildElements.Count);

                    }

                    if ((item.SDNextValue == DateTime.MinValue))
                    {
                        foreach (var hed in workOrderPlanningVM.tableHaederProps)
                        {
                            if (!string.IsNullOrEmpty(hed.title))
                            {
                                if (hed.title == "Hours")
                                {
                                    Parentcells.Add(item.SumScheduledateHour.ToString());
                                    //parent cell
                                }
                                else
                                {
                                    Parentcells.Add("");
                                    //blank cell
                                }

                            }
                        }
                        //create row for parent
                        sheetData1.Append(CreateRowData(Parentcells, TotalHeaderColumns, 2U));
                        Allcells.AddRange(Parentcells);
                        Parentcells.Clear();

                    }

                    thisRow++;
                }

            }
            foreach (var hed in workOrderPlanningVM.tableHaederProps)
            {
                if (!string.IsNullOrEmpty(hed.title))
                {
                    if (hed.title == "Hours")
                    {
                        //create grand total lable cell and grand total cell
                        Parentcells.Add("Grand Total:");
                        Parentcells.Add(GrandTotal.ToString());

                    }
                    else
                    {
                        Parentcells.Add("");
                        //blank td
                    }
                }

            }
            Parentcells.RemoveAt(0);
            sheetData1.Append(CreateRowData(Parentcells, TotalHeaderColumns, 2U));
            Parentcells.Clear();

            //based on this length we identify content has 
            //bigger row so we make cell width fixed while export
            if (Allcells.Count > 0)
            {
                length = Allcells.OrderByDescending(s => s.Length).First().Length;
            }
            return sheetData1;
        }

        //create header row parent grid
        private Row CreateHeaderRow(WorkOrderPlanningVM data)
        {
            var headrs = new List<string>();
            headrs = (from d in data.tableHaederProps.ToList().Where(x => x.title != null && x.title.Length > 0).ToList()
                      select d.title).ToList();
            return CreateRowData(headrs, 0, 2U);
        }
        //create header row for child grid
        private Row CreateHeaderRowForChildGrid(WorkOrderPlanningVM data)
        {
            var headrs = new List<string>();
            headrs = (from d in data.tableHaederProps.ToList().Where(x => x.title != null && x.title.Length > 0).ToList()
                      select d.title).ToList();
            return CreateRowData(headrs, 0, 2U);
        }

        #endregion
        [HttpPost]
        public PartialViewResult ResourceList(bool LockPlan,string PlanStatus)
        {
            WorkOrderPlanningResourceListWrapper workOrderPlanningResourceListWrapper = new WorkOrderPlanningResourceListWrapper(userData);
            WorkOrderPlanningVM objWorkOrderPlanningVM = new WorkOrderPlanningVM();

            ResourceListSearchModel objResourceListSearchModel = new ResourceListSearchModel();
            var totalList = workOrderPlanningResourceListWrapper.SchedulePersonnelListByAssetGroupMasterQuery();
            if (totalList != null && totalList.Count > 0)
            {
                objWorkOrderPlanningVM.PersonnelList = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();

            }
            objResourceListSearchModel.LockPlan = LockPlan;
            objResourceListSearchModel.PlanStatus = PlanStatus;
            objWorkOrderPlanningVM.resourceListSearchModel = objResourceListSearchModel;
            objWorkOrderPlanningVM.ScheduledGroupingList = UtilityFunction.GetGroupingDataForLaborSchedulling().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            LocalizeControls(objWorkOrderPlanningVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrderPlanning/ResourceList/_WorkOrderPlanningResourceListSearch.cshtml", objWorkOrderPlanningVM);
        }

        #region Available Work
        public PartialViewResult AvailableWorkOrders()
        {
            WorkOrderPlanningResourceListWrapper workOrderPlanningResourceListWrapper = new WorkOrderPlanningResourceListWrapper(userData);
            WorkOrderPlanningVM objWorkOrderPlanningVM = new WorkOrderPlanningVM();
            AvailableWorkAssignModel availableWorkAssign = new AvailableWorkAssignModel();
            var totalList = workOrderPlanningResourceListWrapper.SchedulePersonnelListByAssetGroupMasterQuery();
            if (totalList != null && totalList.Count > 0)
            {
                availableWorkAssign.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
                objWorkOrderPlanningVM.availableWorkAssignRLModel = availableWorkAssign;
            }

            LocalizeControls(objWorkOrderPlanningVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrderPlanning/AvailableWork/_AvailableWorkOrder.cshtml", objWorkOrderPlanningVM);
        }
      

       
        public JsonResult GetResourceListAvailable(string colname, string coldir, long WorkOrderPlanId, string ClientLookupId, string ChargeTo, string ChargeToName,
         string Description, string Status, string Priority, string Type, string flag = "0")
        {
            string searchText = "";           
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            ChargeTo = ChargeTo.Replace("%", "[%]");
            ChargeToName = ChargeToName.Replace("%", "[%]");
            Description = Description.Replace("%", "[%]");
            Priority = Priority.Replace("%", "[%]");
            Type = Type.Replace("%", "[%]");         
            WorkOrderPlanningAvailableWorkWrapper workOrderPlanningAvailableWorkWrapper = new WorkOrderPlanningAvailableWorkWrapper(userData);
            var modelList = workOrderPlanningAvailableWorkWrapper.GetAvailableWorkResourceListSchedulingGridData(colname, coldir, 0, 100000, WorkOrderPlanId, ClientLookupId, ChargeTo,
              ChargeToName, Description, Status, Priority, Type, flag, searchText);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetAvailableWorkOrderMainGrid(int? draw, int? start, int? length, long WorkOrderPlanId, string ClientLookupId, string ChargeTo, string ChargeToName,
         string Description, string Status, string Priority, string Type, string flag = "0")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            string searchText = "";
            WorkOrderPlanningAvailableWorkWrapper workOrderPlanningAvailableWorkWrapper = new WorkOrderPlanningAvailableWorkWrapper(userData);
            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;
            List<AvailableWoScheduleModel> ResourceListAvailableList = workOrderPlanningAvailableWorkWrapper.GetAvailableWorkResourceListSchedulingGridData(order, orderDir, skip, length ?? 0, WorkOrderPlanId, ClientLookupId, ChargeTo,
                ChargeToName, Description, Status, Priority, Type, flag, searchText);

            var totalRecords = 0;
            var recordsFiltered = 0;
            if (ResourceListAvailableList != null && ResourceListAvailableList.Count > 0)
            {
                recordsFiltered = ResourceListAvailableList[0].TotalCount;
                totalRecords = ResourceListAvailableList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = ResourceListAvailableList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        [HttpPost]
        public JsonResult AddAvailableWorkAssignRL(WorkOrderPlanningVM wopVM)
        {
            WorkOrderPlanningVM objWOPVM = new WorkOrderPlanningVM();
            WorkOrderPlanningAvailableWorkWrapper workOrderPlanningAvailableWorkWrapper = new WorkOrderPlanningAvailableWorkWrapper(userData);
            System.Text.StringBuilder PersonnelWoList = new System.Text.StringBuilder();
            if (!string.IsNullOrEmpty(wopVM.availableWorkAssignRLModel.WorkOrderIds))
            {           
                string[] workorderIds = wopVM.availableWorkAssignRLModel.WorkOrderIds.Split(',');
                string[] ScheduledDurations = wopVM.availableWorkAssignRLModel.ScheduledDurations.Split(',');
                List<string> errorMessage = new List<string>();
                System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();
                for (int i = 0; i < workorderIds.Length; i++)
                {
                    string Statusmsg = string.Empty;
                    wopVM.availableWorkAssignRLModel.WorkOrderId = Convert.ToInt64(workorderIds[i]);
                    wopVM.availableWorkAssignRLModel.ScheduledDuration = Convert.ToDecimal(ScheduledDurations[i]);
                    var objWorkOrder = workOrderPlanningAvailableWorkWrapper.AddAvailableWorkAssign(wopVM.availableWorkAssignRLModel);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to schedule " + objWorkOrder.ClientLookupId + ": " + objWorkOrder.ErrorMessages[0];
                        errorMessage.Add(errormessage);
                    }
                }
                if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else { return null; }
        }
        #endregion


        #region Add ReSchedule  
        [HttpPost]
        public JsonResult AddReSchedule(WorkOrderPlanningVM WOPVM)
        {
            if (!string.IsNullOrEmpty(WOPVM.woRescheduleModel.WorkOrderIds))
            {
                WorkOrderPlanningVM objWOPVM = new WorkOrderPlanningVM();
            WorkOrderPlanningResourceListWrapper workOrderPlanningResourceListWrapper = new WorkOrderPlanningResourceListWrapper(userData);
            System.Text.StringBuilder PersonnelWoList = new System.Text.StringBuilder();
                string[] workorderIds = WOPVM.woRescheduleModel.WorkOrderIds.Split(',');
                string[] ScheduledDurations = WOPVM.woRescheduleModel.ScheduledDurations.Split(',');
                List<string> errorMessage = new List<string>();
                System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();
                for (int i = 0; i < workorderIds.Length; i++)
                {
                    string Statusmsg = string.Empty;
                    WOPVM.woRescheduleModel.WorkOrderId = Convert.ToInt64(workorderIds[i]);               
                WOPVM.woRescheduleModel.ScheduledDuration = Convert.ToDecimal(ScheduledDurations[i]);
                var objWorkOrder = workOrderPlanningResourceListWrapper.AddReScheduleRecord(WOPVM.woRescheduleModel);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to schedule " + objWorkOrder.ClientLookupId + ": " + objWorkOrder.ErrorMessages[0];
                        errorMessage.Add(errormessage);
                    }
                }
                if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else { return null; }
        }
        #endregion

        #region Remove Schedule
        public JsonResult RemoveScheduleList(RemoveScheduleModel model)
        {
            WorkOrderPlanningVM objWorkOrderPlanningVM = new WorkOrderPlanningVM();
            WorkOrderPlanningResourceListWrapper workOrderPlanningResourceListWrapper = new WorkOrderPlanningResourceListWrapper(userData);
            List<string> errorMessage = new List<string>();
            System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();
            foreach (var item in model.list)
            {
                if (item.Status != WorkOrderStatusConstants.Scheduled)
                {
                    failedWoList.Append(item.ClientLookupId + ",");
                }
                else
                {
                    string Statusmsg = string.Empty;
                    var objWorkOrder = workOrderPlanningResourceListWrapper.RemoveWorkOrderScheduleForResourceList(item.WorkOrderId, item.WorkOrderSchedId);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to remove schedule " + item.ClientLookupId + ": " + objWorkOrder.ErrorMessages[0];
                        errorMessage.Add(errormessage);
                    }
                }
            }
            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errorMessage.Add("Work Order(s) " + failedWoList + " can't be remove schedule. Please check the status.");
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion
        #endregion

        #region WOP Resource Calendar

        #region Calendar bind
        [HttpPost]
        public JsonResult GetResourceCalendarData(string StartDt, string EndDt, long WorkOrderPlanId, List<string> PersonnelList = null)
        {
            WorkOrderPlanningResourceCalendarWrapper CalendarWrapper = new WorkOrderPlanningResourceCalendarWrapper(userData);

            List<WorkOrderPlanningResourceCalendar> CalendarList = CalendarWrapper.GetResourceCalendarData(StartDt, EndDt, WorkOrderPlanId, PersonnelList);

            var ListPersonnel = CalendarList
                                .Select(x => new { x.PersonnelFull, x.PersonnelId, x.ScheduledHours })
                                .GroupBy(g => new { g.PersonnelFull, g.PersonnelId })
                                .Select(y => new { y.Key.PersonnelFull, y.Key.PersonnelId, ScheduledHours = y.Sum(h => h.ScheduledHours) })
                                .OrderBy(x => x.PersonnelFull)
                                .ToList();
            var ReturnObj = new { CalendarList, ListPersonnel };
            return Json(ReturnObj);
        }
        [HttpPost]
        public PartialViewResult ResourceCalendar()
        {
            WorkOrderPlanningResourceCalendarWrapper CalendarWrapper = new WorkOrderPlanningResourceCalendarWrapper(userData);
            WorkOrderPlanningVM workOrderPlanningVM = new WorkOrderPlanningVM();
            IEnumerable<SelectListItem> Personnellist;

            var totalList = CalendarWrapper.SchedulePersonnelListByAssetGroupMasterQuery();
            if (totalList != null && totalList.Count > 0)
            {
                Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();                
                workOrderPlanningVM.PersonnelList = Personnellist;
            }
            LocalizeControls(workOrderPlanningVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrderPlanning/ResourceCalendar/_WorkOrderPlanningResourceCalendar.cshtml", workOrderPlanningVM);
        }
        #endregion

        #region Add schedule calendar     
        [HttpPost]
        public PartialViewResult AddScheduleCalendarModal(long WorkOrderPlanId)
        {
            WorkOrderPlanningResourceCalendarWrapper wrapper = new WorkOrderPlanningResourceCalendarWrapper(userData);
            WorkOrderPlanningVM workOrderPlanningVM = new WorkOrderPlanningVM();

            var totalList = wrapper.SchedulePersonnelListByAssetGroupMasterQuery();
            if (totalList != null && totalList.Count > 0)
            {
                workOrderPlanningVM.PersonnelList = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            }
            List<SelectListItem> WorkOrderList = wrapper.RetrieveWorkOrderListForAddScheduling(WorkOrderPlanId);
            if (WorkOrderList != null && WorkOrderList.Count > 0)
            {
                workOrderPlanningVM.WorkOrderList = WorkOrderList;
            }
            LocalizeControls(workOrderPlanningVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrderPlanning/ResourceCalendar/_AddScheduleCalendar.cshtml", workOrderPlanningVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddScheduleCalendar(WorkOrderPlanningVM workOrderPlanningVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                WorkOrderPlanningResourceCalendarWrapper wrapper = new WorkOrderPlanningResourceCalendarWrapper(userData);                

                var objWorkOrderPlan = wrapper.AddScheduleRecord(workOrderPlanningVM.ResourceCalendarAddScheduleModel);
                if (objWorkOrderPlan.ErrorMessages != null && objWorkOrderPlan.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrderPlan.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Edit Schedule calendar
        public PartialViewResult EditScheduleCalendar(long WorkOrderSchedId, long Workorderid)
        {
            WorkOrderPlanningResourceCalendarWrapper wrapper = new WorkOrderPlanningResourceCalendarWrapper(userData);
            WorkOrderPlanningVM WOplanningVM = new WorkOrderPlanningVM();
            ResourceCalendarEditScheduleModel editSchedlingCalendarModal = new ResourceCalendarEditScheduleModel();
            WorkOrderSchedule workOrderSchedule = wrapper.RetrieveWorkOrderSchedule(Workorderid, WorkOrderSchedId);

            if (workOrderSchedule != null)
            {
                editSchedlingCalendarModal.WorkOrderID = Workorderid;
                editSchedlingCalendarModal.WorkOrderScheduledID = WorkOrderSchedId;
                editSchedlingCalendarModal.Description = workOrderSchedule.Description;
                editSchedlingCalendarModal.ScheduleDate = workOrderSchedule.ScheduledStartDate;
                editSchedlingCalendarModal.Hours = workOrderSchedule.ScheduledHours;
                editSchedlingCalendarModal.PersonnelName = workOrderSchedule.NameFirst + " " + workOrderSchedule.NameLast;
                editSchedlingCalendarModal.ClientLookupId = workOrderSchedule.ClientLookupId;
            }
            WOplanningVM.ResourceCalendarEditScheduleModel = editSchedlingCalendarModal;
            LocalizeControls(WOplanningVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrderPlanning/ResourceCalendar/_EditScheduleCalendar.cshtml", WOplanningVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditScheduleCalendar(WorkOrderPlanningVM WOplanningVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                WorkOrderPlanningResourceCalendarWrapper wrapper = new WorkOrderPlanningResourceCalendarWrapper(userData);

                var workOrderPlan = wrapper.UpdateWorkOrderSchedule(WOplanningVM.ResourceCalendarEditScheduleModel);
                if (workOrderPlan.ErrorMessages != null && workOrderPlan.ErrorMessages.Count > 0)
                {
                    return Json(JsonReturnEnum.failed.ToString(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Remove Schedule calendar
        [HttpPost]
        public JsonResult RemoveScheduleCalendar(long WOId, long WOSchedId)
        {
            WorkOrderPlanningResourceCalendarWrapper wrapper = new WorkOrderPlanningResourceCalendarWrapper(userData);

            var objWorkOrder = wrapper.RemoveWorkOrderSchedule(WOId, WOSchedId);
            if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
            {
                return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Drag and drop schedule calendar
        [HttpPost]
        public JsonResult DragScheduleCalendar(long WorkOrderScheduledID, long WOId, string ScheduleDate)
        {
            WorkOrderPlanningVM workOrderPlanningVM = new WorkOrderPlanningVM();
            WorkOrderPlanningResourceCalendarWrapper wrapper = new WorkOrderPlanningResourceCalendarWrapper(userData);
            DateTime _ScheduleDate = DateTime.ParseExact(ScheduleDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var objWorkOrder = wrapper.DragWorkOrderScheduleFromCalendar(WOId, WorkOrderScheduledID, _ScheduleDate);
            if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
            {
                return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region WOP Dashboard

        #region Schedule Compliance
        [HttpPost]
        public JsonResult GetScheduleComplianceChartData(long WorkOrderPlanID)
        {
            WorkOrderPlanningDashboardWrapper woWrapper = new WorkOrderPlanningDashboardWrapper(userData);
            ScheduleCompliancedoughnutChartModel dModel = woWrapper.GetScheduleComplianceChartData(WorkOrderPlanID);
            return Json(dModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public ActionResult PlannedWorkOrderLineItemStatuses(long WorkOrderPlanID)
        {
            List<KeyValuePair<string, long>> entries = new List<KeyValuePair<string, long>>();
            WorkOrderPlanningDashboardWrapper woWrapper = new WorkOrderPlanningDashboardWrapper(userData);
            entries = woWrapper.WorkOrderPlanningLineItemStatuses(WorkOrderPlanID);

            return Json(entries, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PlannedWorkOrderPlanEstimateHours(long WorkOrderPlanID)
        {
            List<KeyValuePair<string, decimal>> entries = new List<KeyValuePair<string, decimal>>();
            WorkOrderPlanningDashboardWrapper woWrapper = new WorkOrderPlanningDashboardWrapper(userData);
            entries = woWrapper.WorkOrderPlanningEstimateHours(WorkOrderPlanID);

            return Json(entries, JsonRequestBehavior.AllowGet);
        }

        #region Work Orders by Type
        [HttpPost]
        public ActionResult PlannedWorkOrderByType(long WorkOrderPlanId)
        {
            WorkOrderPlanningDashboardWrapper woWrapper = new WorkOrderPlanningDashboardWrapper(userData);
            multiSeriesLine2dModel multiSeriesChart = new multiSeriesLine2dModel();
            multiSeriesChart = woWrapper.GetWorkOrdersByTypeChartData(WorkOrderPlanId);

            return Json(multiSeriesChart, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Work Orders by Assigned
        [HttpPost]
        public ActionResult PlannedWorkOrderByAssigned(long WorkOrderPlanId)
        {
            WorkOrderPlanningDashboardWrapper woWrapper = new WorkOrderPlanningDashboardWrapper(userData);
            overlapping2dModel overlapping2dModel = new overlapping2dModel();
            overlapping2dModel = woWrapper.GetWorkOrdersByAssignedChart(WorkOrderPlanId);
            return Json(overlapping2dModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Hours by Assigned
        [HttpPost]
        public ActionResult PlannedWorkOrderByHours(long WorkOrderPlanId)
        {
            WorkOrderPlanningDashboardWrapper woWrapper = new WorkOrderPlanningDashboardWrapper(userData);
            overlapping2dModel overlapping2dModel = new overlapping2dModel();
            overlapping2dModel = woWrapper.GetHoursByAssignedChart(WorkOrderPlanId);
            return Json(overlapping2dModel, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Print WOP
        [HttpPost]
        public JsonResult WOPSetPrintData(WOPPrintParams wopPrintParams)
        {
            Session["PRINTPARAMS"] = wopPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [EncryptedActionParameter]
        public ActionResult WOPExportASPDF(string d = "")
        {
            WorkOrderPlanningSearchWrapper wopWrapper = new WorkOrderPlanningSearchWrapper(userData);
            WorkOrderPlanPDFPrintModel objWOPPrintModel;
            WorkOrderPlanningVM objWorkOrderPlanVM = new WorkOrderPlanningVM();
            List<WorkOrderPlanPDFPrintModel> pSearchModelList = new List<WorkOrderPlanPDFPrintModel>();
            var locker = new object();

            WOPPrintParams pRPrintParams = (WOPPrintParams)Session["PRINTPARAMS"];

            List<WorkOrderPlanningModel> pRList = wopWrapper.GetWorkOrderPlanGridData(pRPrintParams.CustomQueryDisplayId, pRPrintParams.colname, pRPrintParams.coldir, 0, 100000);

            foreach (var p in pRList)
            {
                objWOPPrintModel = new WorkOrderPlanPDFPrintModel();
                objWOPPrintModel.PlanID = p.WorkOrderPlanId;
                objWOPPrintModel.Description = p.Description;
                if (p.StartDate != null && p.StartDate != default(DateTime))
                {
                    objWOPPrintModel.StartDateString = p.StartDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objWOPPrintModel.StartDateString = "";
                }
                if (p.EndDate != null && p.EndDate != default(DateTime))
                {
                    objWOPPrintModel.EndDateString = p.EndDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objWOPPrintModel.EndDateString = "";
                }

                objWOPPrintModel.Status = p.Status;
                if (p.Created != null && p.Created != default(DateTime))
                {
                    objWOPPrintModel.CreatedDateString = p.Created.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objWOPPrintModel.CreatedDateString = "";
                }
                if (p.Completed != null && p.Completed != default(DateTime))
                {
                    objWOPPrintModel.CompletedDateString = p.Completed.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objWOPPrintModel.CompletedDateString = "";
                }
                if (p.ChildCount > 0)
                {
                    objWOPPrintModel.WOPlanLineItemModelList = wopWrapper.PopulateLineitems(p.WorkOrderPlanId);
                }
                lock (locker)
                {
                    pSearchModelList.Add(objWOPPrintModel);
                }
            }
            objWorkOrderPlanVM.workOrderPlanPDFPrintModel = pSearchModelList;
            objWorkOrderPlanVM.tableHaederProps = pRPrintParams.tableHaederProps;
            LocalizeControls(objWorkOrderPlanVM, LocalizeResourceSetConstants.WorkOrderPlanning);
            if (d == "excel")
            {
                return GenerateExcelReportWOP(objWorkOrderPlanVM, "Work Order Plan List");
            }
            if (d == "csv")
            {
                return GenerateCsvReportWOP(objWorkOrderPlanVM, "Work Order Plan List");
            }
            else if (d == "d")
            {
                return new PartialViewAsPdf("WOPGridPdfPrintTemplate", objWorkOrderPlanVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    FileName = "Work Order Plan.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new ViewAsPdf("WOPGridPdfPrintTemplate", objWorkOrderPlanVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }

        }
        #endregion
        #region ExportToExcelFunctionality for Work Order Planning
        //This code used to handle export functionality
        public ActionResult GenerateExcelReportWOP(WorkOrderPlanningVM workOrderPlanningVM, string reportName)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {

                CreatePartsForExcelOrCsvWOP(package, workOrderPlanningVM);
                package.Close();
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream.ToArray(),
                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
               string.Concat(reportName, ".xlsx"));
            }

        }
        public ActionResult GenerateCsvReportWOP(WorkOrderPlanningVM workOrderPlanningVM, string reportName)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {

                CreatePartsForExcelOrCsvWOP(package, workOrderPlanningVM);
                package.Close();
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream.ToArray(),
                  ".csv",
               string.Concat(reportName, ".csv"));
            }

        }
        //This method creating different parts of excel sheet
        private void CreatePartsForExcelOrCsvWOP(SpreadsheetDocument document, WorkOrderPlanningVM data)
        {
            int length = 0;
            List<int> groupRowIndexes = new List<int>();
            int TotalHeaderColumns = 8;
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

        private SheetData GenerateSheetdataForDetailsWOP(WorkOrderPlanningVM workOrderPlanningVM, ref List<int> groupRowIndexes, ref int TotalHeaderColumns, ref int length)
        {
            //creating content of excel sheet logic is same as genere pdf
            SheetData sheetData1 = new SheetData();
            List<string> Parentcells = new List<string>();
            List<string> Childcells = new List<string>();
            List<string> Allcells = new List<string>();
            sheetData1.Append(CreateHeaderRowWOP(workOrderPlanningVM));

            List<string> headercells = new List<string>();

            foreach (var item in workOrderPlanningVM.workOrderPlanPDFPrintModel)
            {
                //create header row
                foreach (var hed in workOrderPlanningVM.tableHaederProps)
                {
                    if (!string.IsNullOrWhiteSpace(hed.property))
                    {
                        headercells.Add(item.GetType().GetProperty(hed.property).GetValue(item, null).ToString());
                    }

                }
                sheetData1.Append(CreateRowData(headercells, TotalHeaderColumns, 1U));
                headercells.Clear();
                if (item.WOPlanLineItemModelList.Count > 0)
                {
                    //Create row
                    headercells.Add("Work Order ID");
                    headercells.Add("Description");
                    headercells.Add("Required");
                    headercells.Add("Asset ID");
                    headercells.Add("Asset Name");
                    headercells.Add("Status");
                    headercells.Add("Complete Date");
                    headercells.Add("Type");

                    sheetData1.Append(CreateRowData(headercells, TotalHeaderColumns, 2U));
                    headercells.Clear();
                    ////eEnd row

                    foreach (var line in item.WOPlanLineItemModelList)
                    {
                        Childcells.Add(line.ClientLookupId.ToString());
                        Childcells.Add(line.Description);
                        Childcells.Add(line.RequiredDate);
                        Childcells.Add(line.AssetId);
                        Childcells.Add(line.ChargeTo_Name);
                        Childcells.Add(line.Status);
                        Childcells.Add(line.CompleteDate);
                        Childcells.Add(line.Type);

                        sheetData1.Append(CreateRowData(Childcells, TotalHeaderColumns, 1U));
                        Childcells.Clear();

                    }

                    //Create row
                }
            }

            return sheetData1;
        }

        //create header row parent grid
        private Row CreateHeaderRowWOP(WorkOrderPlanningVM data)
        {
            var headrs = new List<string>();
            headrs = (from d in data.tableHaederProps.ToList().Where(x => x.title != null && x.title.Length > 0).ToList()
                      select d.title).ToList();
            return CreateRowData(headrs, 8, 2U);
        }


        #endregion
        #region WOP Add New
        public PartialViewResult AddNewWorkOderPlan()
        {
            WorkOrderPlanningVM objWorkOrderPlanningVM = new WorkOrderPlanningVM();
            WorkOrderPlanningModel objWoprkrderPlanModel = new WorkOrderPlanningModel();
            WorkOrderPlanningSearchWrapper pWrapper = new WorkOrderPlanningSearchWrapper(userData);
            WorkOrderPlanningResourceListWrapper workOrderPlanningResourceListWrapper = new WorkOrderPlanningResourceListWrapper(userData);

            var totalList = workOrderPlanningResourceListWrapper.SchedulePersonnelList();
            if (totalList != null && totalList.Count > 0)
            {
                objWoprkrderPlanModel.PersonnelList = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();

            }
            objWorkOrderPlanningVM.workorderPlanningModel = objWoprkrderPlanModel;
            LocalizeControls(objWorkOrderPlanningVM, LocalizeResourceSetConstants.WorkOrderPlanning);
            return PartialView("_AddNewWorkOrderPlan", objWorkOrderPlanningVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddWorkOrderPlan(WorkOrderPlanningVM objWorkOrderPlanVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;

            if (ModelState.IsValid)
            {
                DataContracts.WorkOrderPlan WOP = new DataContracts.WorkOrderPlan();
                WorkOrderPlanningSearchWrapper WOPWrapper = new WorkOrderPlanningSearchWrapper(userData);

                WOP = WOPWrapper.AddWorkOrderPlan(objWorkOrderPlanVM);

                if (WOP.ErrorMessages != null && WOP.ErrorMessages.Count > 0)
                {
                    return Json(WOP.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, workOrderPlanId = WOP.WorkOrderPlanId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region Updating Status Reopen WorkOderPlan (V2-1026)
        public JsonResult UpdatingStatusReopenWorkOderPlan(long WorkOrderPlanID, string Status)
        {
            var errorList = new List<string>();
            if (WorkOrderPlanID > 0)
            {
                WorkOrderPlanningDetailsPlanWrapper WOPlanningDetailsPlanWrapper = new WorkOrderPlanningDetailsPlanWrapper(userData);

                errorList = WOPlanningDetailsPlanWrapper.WorkOrderPlanStatusUpdating(WorkOrderPlanID, Status);
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

    }




}