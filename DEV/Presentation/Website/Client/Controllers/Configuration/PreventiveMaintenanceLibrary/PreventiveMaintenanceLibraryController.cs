using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Configuration.PreventiveMaintenanceLibrary;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.PreventiveMaintenanceLibrary;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.PreventiveMaintenanceLibrary
{
    public class PreventiveMaintenanceLibraryController : ConfigBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.PrevMaint_Library)]
        public ActionResult Index()
        {
            PrevMaintLibraryVM objPrevMaintLibVM = new PrevMaintLibraryVM();
            PreventiveMaintenanceLibraryModel objPreventiveMaintenanceLibraryModel = new PreventiveMaintenanceLibraryModel();
            LocalizeControls(objPrevMaintLibVM, LocalizeResourceSetConstants.PrevMaintDetails);
            objPrevMaintLibVM.security = this.userData.Security;
            return View("~/Views/Configuration/PreventiveMaintenanceLibrary/index.cshtml", objPrevMaintLibVM);
        }
        [HttpPost]
        public string GetPreventiveMaintenanceLibraryGrid(int? draw, int? start, int? length, string ClientLookupId, string Description, decimal? JobDuration, string FrequencyType, string Frequency, DateTime? CreateDate, string SearchText = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            List<string> FrequencyTypeList = new List<string>();
            PrevMaintLibraryWrapper pWrapper = new PrevMaintLibraryWrapper(userData);
            var PrevMaintLibraryList = pWrapper.populatePrevMaintLibrary();
            PrevMaintLibraryList = this.GetPMLibraryListGridSortByColumnWithOrder(colname[0], orderDir, PrevMaintLibraryList);
            PreventiveMaintenanceLibraryModel objPreventiveMaintenanceLibraryModel;
            List<PreventiveMaintenanceLibraryModel> PreventiveMaintenanceLibraryModelList = new List<PreventiveMaintenanceLibraryModel>();
            if (PrevMaintLibraryList != null)
            {
                FrequencyTypeList = PrevMaintLibraryList.Where(x => x.FrequencyType != "").Select(x => x.FrequencyType).Distinct().ToList();
                PrevMaintLibraryList = this.GetPrevMaintLibrarySearchResult(PrevMaintLibraryList, ClientLookupId, Description, JobDuration, FrequencyType, Frequency, CreateDate, SearchText);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PrevMaintLibraryList.Count();
            totalRecords = PrevMaintLibraryList.Count();
            int initialPage = start.Value;
            var filteredResult = PrevMaintLibraryList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            foreach (var p in filteredResult)
            {
                objPreventiveMaintenanceLibraryModel = new PreventiveMaintenanceLibraryModel();
                objPreventiveMaintenanceLibraryModel.PrevMaintLibraryId = p.PrevMaintLibraryId;
                objPreventiveMaintenanceLibraryModel.ClientLookupId = p.ClientLookupId;
                objPreventiveMaintenanceLibraryModel.Description = p.Description;
                objPreventiveMaintenanceLibraryModel.JobDuration = p.JobDuration;
                objPreventiveMaintenanceLibraryModel.FrequencyType = p.FrequencyType;
                objPreventiveMaintenanceLibraryModel.Frequency = p.Frequency;
                objPreventiveMaintenanceLibraryModel.CreateDate = p.CreateDate;
                objPreventiveMaintenanceLibraryModel.Type = p.Type;
                objPreventiveMaintenanceLibraryModel.ScheduleType = p.ScheduleType;
                objPreventiveMaintenanceLibraryModel.ScheduleMethod = p.ScheduleMethod;
                objPreventiveMaintenanceLibraryModel.InactiveFlag = p.InactiveFlag;
                objPreventiveMaintenanceLibraryModel.DownRequired = p.DownRequired;
                PreventiveMaintenanceLibraryModelList.Add(objPreventiveMaintenanceLibraryModel);
            }
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = PreventiveMaintenanceLibraryModelList, FrequencyTypeList = FrequencyTypeList }, JsonSerializerDateSettings);
        }
        private List<PreventiveMaintenanceLibraryModel> GetPMLibraryListGridSortByColumnWithOrder(string order, string orderDir, List<PreventiveMaintenanceLibraryModel> data)
        {
            List<PreventiveMaintenanceLibraryModel> lst = new List<PreventiveMaintenanceLibraryModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.JobDuration).ToList() : data.OrderBy(p => p.JobDuration).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FrequencyType).ToList() : data.OrderBy(p => p.FrequencyType).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Frequency).ToList() : data.OrderBy(p => p.Frequency).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        public ActionResult PreventiveMaintenanceLibraryDetail(long PrevMaintLibraryId = 0)
        {
            PrevMaintLibraryVM objPrevMaintLibVM = new PrevMaintLibraryVM();
            objPrevMaintLibVM.preventiveMaintenanceLibraryModel = new PreventiveMaintenanceLibraryModel();
            ChangePreventiveLibraryIDModel _ChangePreventiveLibIDModel = new ChangePreventiveLibraryIDModel();
            PrevMaintLibraryWrapper pWrapper = new PrevMaintLibraryWrapper(userData);
            PreventiveMaintenanceLibraryModel objPreventiveMaintenanceLibraryModel = pWrapper.populatePreventiveMaintenanceLibraryDetails(PrevMaintLibraryId);
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AllLookUps = objCommonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var LookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_WO_Type).ToList();
                var valType = LookUplist.Where(x => x.ListValue == objPreventiveMaintenanceLibraryModel.Type).FirstOrDefault();
                if (valType != null)
                {
                    objPreventiveMaintenanceLibraryModel.Type = valType.ListValue + " - " + valType.Description;
                }
            }
            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            if (FrequencyTypeList != null)
            {
                var valType = FrequencyTypeList.Where(x => x.value == objPreventiveMaintenanceLibraryModel.FrequencyType).FirstOrDefault();
                if (valType != null)
                {
                    objPreventiveMaintenanceLibraryModel.FrequencyType = valType.text;
                }

                objPrevMaintLibVM.preventiveMaintenanceLibraryModel = objPreventiveMaintenanceLibraryModel;
            }
            var ScheduleTypeList = UtilityFunction.GetPMLibraryScheduleType();
            if (ScheduleTypeList != null)
            {
                var valType = ScheduleTypeList.Where(x => x.value == objPreventiveMaintenanceLibraryModel.ScheduleType).FirstOrDefault();
                if (valType != null)
                {
                    objPreventiveMaintenanceLibraryModel.ScheduleType = valType.text;
                }
                objPrevMaintLibVM.preventiveMaintenanceLibraryModel = objPreventiveMaintenanceLibraryModel;
            }
            var ScheduleMethodList = UtilityFunction.populateScheduleMethodList();
            if (ScheduleMethodList != null)
            {
                var valType = ScheduleMethodList.Where(x => x.value == objPreventiveMaintenanceLibraryModel.ScheduleMethod).FirstOrDefault();
                if (valType != null)
                {
                    objPreventiveMaintenanceLibraryModel.ScheduleMethod = valType.text;
                }
                objPrevMaintLibVM.preventiveMaintenanceLibraryModel = objPreventiveMaintenanceLibraryModel;
            }
            objPrevMaintLibVM.security = this.userData.Security;

            _ChangePreventiveLibIDModel.PrevMaintLibraryId = objPrevMaintLibVM.preventiveMaintenanceLibraryModel.PrevMaintLibraryId;
            _ChangePreventiveLibIDModel.ClientLookupId = objPrevMaintLibVM.preventiveMaintenanceLibraryModel.ClientLookupId;
            objPrevMaintLibVM.ChangePreventiveLibraryIDModel = _ChangePreventiveLibIDModel;
            LocalizeControls(objPrevMaintLibVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/Configuration/PreventiveMaintenanceLibrary/_PMLibraryDetails.cshtml", objPrevMaintLibVM);
        }

        [HttpGet]
        public PartialViewResult AddEditPMLibrary(long PrevMaintLibraryId = 0)
        {
            PrevMaintLibraryVM objPrevMaintLibVM = new PrevMaintLibraryVM();
            PrevMaintLibraryWrapper pWrapper = new PrevMaintLibraryWrapper(userData);
            PreventiveMaintenanceLibraryModel objPreventiveMaintenanceLibraryModel = new PreventiveMaintenanceLibraryModel();
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            if (PrevMaintLibraryId != 0)
            {
                objPreventiveMaintenanceLibraryModel = pWrapper.populatePreventiveMaintenanceLibraryDetails(PrevMaintLibraryId);
            }
            var AllLookUps = objCommonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var LookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_WO_Type).ToList();
                objPreventiveMaintenanceLibraryModel.TypeList = LookUplist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
            }
            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            if (FrequencyTypeList != null)
            {
                objPreventiveMaintenanceLibraryModel.FrequencyTypeList = FrequencyTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ScheduleTypeList = UtilityFunction.GetPMLibraryScheduleType();
            if (ScheduleTypeList != null)
            {
                objPreventiveMaintenanceLibraryModel.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ScheduleMethodList = UtilityFunction.populateScheduleMethodList();
            if (ScheduleMethodList != null)
            {
                objPreventiveMaintenanceLibraryModel.ScheduleMethodList = ScheduleMethodList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objPrevMaintLibVM.preventiveMaintenanceLibraryModel = objPreventiveMaintenanceLibraryModel;
            LocalizeControls(objPrevMaintLibVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/Configuration/PreventiveMaintenanceLibrary/_AddEditPMLibrary.cshtml", objPrevMaintLibVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditPMLibrary(PrevMaintLibraryVM prevMaintLibraryVM, string Command)
        {
            string Mode = string.Empty;
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                PreventiveMaintenanceLibraryModel pPreventiveMaintenanceLibraryModel = new PreventiveMaintenanceLibraryModel();
                PrevMaintLibraryWrapper pmlWrapper = new PrevMaintLibraryWrapper(userData);
                PrevMaintLibrary pmLibrary = new PrevMaintLibrary();
                if (prevMaintLibraryVM.preventiveMaintenanceLibraryModel != null && prevMaintLibraryVM.preventiveMaintenanceLibraryModel.PrevMaintLibraryId == 0)
                {
                    Mode = "add";
                    pmLibrary = pmlWrapper.AddPrevMaintLibrary(prevMaintLibraryVM.preventiveMaintenanceLibraryModel);
                }
                else
                {
                    pmLibrary = pmlWrapper.EditPrevMaintLibrary(prevMaintLibraryVM.preventiveMaintenanceLibraryModel);

                }
                if (pmLibrary.ErrorMessages != null && pmLibrary.ErrorMessages.Count > 0)
                {
                    return Json(pmLibrary.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, PrevMaintLibraryId = pmLibrary.PrevMaintLibraryId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ChangePrevMaintLibID(PrevMaintLibraryVM prevMaintLibVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            List<String> errorList = new List<string>();
            if (ModelState.IsValid)
            {
                PrevMaintLibraryVM objprevMaintLibVM = new PrevMaintLibraryVM();
                ChangePreventiveLibraryIDModel _ChangePreventiveLibIDModel = new ChangePreventiveLibraryIDModel();
                PrevMaintLibraryWrapper pmlibwrapper = new PrevMaintLibraryWrapper(userData);
                long _pmlibId = prevMaintLibVM.ChangePreventiveLibraryIDModel.PrevMaintLibraryId;
                _ChangePreventiveLibIDModel = prevMaintLibVM.ChangePreventiveLibraryIDModel;              

                   errorList = pmlibwrapper.ChangePMLibId(_ChangePreventiveLibIDModel);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Print
        [HttpGet]
        public string GetPreventiveMaintenanceLibraryPrintData(string colname,string coldir, string ClientLookupId = "", string Description = "", decimal? JobDuration = null, string FrequencyType = "", string Frequency = "", DateTime? CreateDate = null, string SearchText = "")
        {
            List<PreventiveMaintenanceLibraryPrintModel> sanitationJobPrintModelList = new List<PreventiveMaintenanceLibraryPrintModel>();
            PreventiveMaintenanceLibraryPrintModel objPreventiveMaintenanceLibraryPrintModel;
            PrevMaintLibraryWrapper pWrapper = new PrevMaintLibraryWrapper(userData);
            List<PreventiveMaintenanceLibraryPrintModel> preventiveMaintenanceLibraryPrintModelList = new List<PreventiveMaintenanceLibraryPrintModel>();
            List<PreventiveMaintenanceLibraryModel> PrevMaintLibraryList = pWrapper.populatePrevMaintLibrary();
            if (PrevMaintLibraryList != null)
            {
                PrevMaintLibraryList = this.GetPMLibraryListGridSortByColumnWithOrder(colname, coldir, PrevMaintLibraryList);
                PrevMaintLibraryList = this.GetPrevMaintLibrarySearchResult(PrevMaintLibraryList, ClientLookupId, Description, JobDuration, FrequencyType, Frequency, CreateDate, SearchText);
                foreach (var p in PrevMaintLibraryList)
                {
                    objPreventiveMaintenanceLibraryPrintModel = new PreventiveMaintenanceLibraryPrintModel();
                    objPreventiveMaintenanceLibraryPrintModel.ClientLookupId = p.ClientLookupId;
                    objPreventiveMaintenanceLibraryPrintModel.Description = p.Description;
                    objPreventiveMaintenanceLibraryPrintModel.JobDuration = p.JobDuration;
                    objPreventiveMaintenanceLibraryPrintModel.FrequencyType = p.FrequencyType;
                    objPreventiveMaintenanceLibraryPrintModel.Frequency = p.Frequency ?? 0;
                    objPreventiveMaintenanceLibraryPrintModel.CreateDate = p.CreateDate;

                    preventiveMaintenanceLibraryPrintModelList.Add(objPreventiveMaintenanceLibraryPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = preventiveMaintenanceLibraryPrintModelList }, JsonSerializerDateSettings);
        }
        private List<PreventiveMaintenanceLibraryModel> GetPrevMaintLibrarySearchResult(List<PreventiveMaintenanceLibraryModel> PrevMaintLibraryList, string ClientLookupId = "", string Description = "", decimal? JobDuration = null, string FrequencyType = "", string Frequency = "",
                              DateTime? CreateDate = null, string SearchText = "")
        {
            if (PrevMaintLibraryList != null)
            {
                SearchText = SearchText.ToUpper();               
                DateTime dateTime;
                DateTime.TryParseExact(SearchText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                PrevMaintLibraryList = PrevMaintLibraryList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(SearchText))
                                                 || (!string.IsNullOrWhiteSpace(x.Description.Trim()) && x.Description.Trim().ToUpper().Contains(SearchText))
                                                || (x.CreateDate != null && x.CreateDate.Value != default(DateTime) && x.CreateDate.Value.Date.Equals(dateTime))
                                                || (!string.IsNullOrWhiteSpace(x.FrequencyType) && x.FrequencyType.ToUpper().Contains(SearchText))
                                                || (!string.IsNullOrWhiteSpace(Convert.ToString(x.JobDuration)) && Convert.ToString(x.JobDuration).ToUpper().Contains(SearchText))
                                                  || (!string.IsNullOrWhiteSpace(Convert.ToString(x.Frequency)) && Convert.ToString(x.Frequency).ToUpper().Contains(SearchText))                                              
                                                ).ToList();

                if (!string.IsNullOrEmpty(ClientLookupId))
                {
                    ClientLookupId = ClientLookupId.ToUpper();
                    PrevMaintLibraryList = PrevMaintLibraryList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    PrevMaintLibraryList = PrevMaintLibraryList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(FrequencyType))
                {
                    FrequencyType = FrequencyType.ToUpper();
                    PrevMaintLibraryList = PrevMaintLibraryList.Where(x => (!string.IsNullOrWhiteSpace(x.FrequencyType) && x.FrequencyType.ToUpper().Equals(FrequencyType))).ToList();
                }
                if (CreateDate != null)
                {
                    PrevMaintLibraryList = PrevMaintLibraryList.Where(x => (x.CreateDate != null && x.CreateDate.Value.Date.Equals(CreateDate.Value.Date))).ToList();

                }
                if (JobDuration.HasValue)
                {
                    PrevMaintLibraryList = PrevMaintLibraryList.Where(x => x.JobDuration.Equals(JobDuration)).ToList();
                }
                if (!string.IsNullOrEmpty(Frequency))
                {
                    Frequency = Frequency.ToUpper();
                    PrevMaintLibraryList = PrevMaintLibraryList.Where(x => (!string.IsNullOrWhiteSpace(Convert.ToString(x.Frequency)) && Convert.ToString(x.Frequency).ToUpper().Contains(Frequency))).ToList();
                }

            }
            return PrevMaintLibraryList;
        }
        #endregion
        #region Task
        [HttpPost]
        public string PopulateTasks(int? draw, int? start, int? length, long PrevMaintLibraryId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PrevMaintLibraryWrapper pmlWrapper = new PrevMaintLibraryWrapper(userData);
            var Tasks = pmlWrapper.PopulateTasks(PrevMaintLibraryId);
            Tasks = this.GetAllTasksSortByColumnWithOrder(order, orderDir, Tasks);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Tasks.Count();
            totalRecords = Tasks.Count();
            int initialPage = start.Value;
            var filteredResult = Tasks
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            bool isActionAddBtnShow = userData.Security.PrevMaintLibrary.Create;
            bool isActionDelBtnShow = userData.Security.PrevMaintLibrary.Delete;
            bool isActionEditBtnShow = userData.Security.PrevMaintLibrary.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, isActionAddBtnShow = isActionAddBtnShow, IsActionEditBtnShow = isActionEditBtnShow, isActionDelBtnShow = isActionDelBtnShow }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<TaskModel> GetAllTasksSortByColumnWithOrder(string order, string orderDir, List<TaskModel> data)
        {
            List<TaskModel> lst = new List<TaskModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaskId).ToList() : data.OrderBy(p => p.TaskId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaskId).ToList() : data.OrderBy(p => p.TaskId).ToList();
                    break;
            }
            return lst;
        }
        [HttpPost]
        public ActionResult DeleteTasks(long PrevMaintLibraryId, long PrevMaintLibraryTaskId)
        {
            PrevMaintLibraryWrapper pmlWrapper = new PrevMaintLibraryWrapper(userData);
            if (pmlWrapper.DeleteTask(PrevMaintLibraryTaskId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddTask(long PrevMaintLibraryId, string ClientLookUpId)
        {
            PrevMaintLibraryVM objPrevMaintLibVM = new PrevMaintLibraryVM();
            TaskModel objModel = new TaskModel();
            PrevMaintLibraryWrapper pmlWrapper = new PrevMaintLibraryWrapper(userData);
            objModel.PrevMaintLibraryId = PrevMaintLibraryId;
            objModel.ClientLookUpId = ClientLookUpId;
            objModel.PrevMaintLibraryTaskId = 0;
            objPrevMaintLibVM.taskModel = objModel;
            LocalizeControls(objPrevMaintLibVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/Configuration/PreventiveMaintenanceLibrary/_AddEditPMLibraryTask.cshtml", objPrevMaintLibVM);
        }
        public ActionResult EditTask(long PrevMaintLibraryId, string ClientLookUpId, long PrevMaintLibraryTaskId, string TaskId, string Description)
        {
            PrevMaintLibraryVM objPrevMaintLibVM = new PrevMaintLibraryVM();
            TaskModel objModel = new TaskModel();
            objModel.PrevMaintLibraryId = PrevMaintLibraryId;
            objModel.ClientLookUpId = ClientLookUpId;
            objModel.PrevMaintLibraryTaskId = PrevMaintLibraryTaskId;
            objModel.TaskId = TaskId;
            objModel.Description = Description;
            objPrevMaintLibVM.taskModel = objModel;
            LocalizeControls(objPrevMaintLibVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return PartialView("~/Views/Configuration/PreventiveMaintenanceLibrary/_AddEditPMLibraryTask.cshtml", objPrevMaintLibVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTask(PrevMaintLibraryVM objVM)
        {
            if (ModelState.IsValid)
            {
                PrevMaintLibraryWrapper pmlWrapper = new PrevMaintLibraryWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = pmlWrapper.AddOrUpdateTask(objVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PrevMaintLibraryId = objVM.taskModel.PrevMaintLibraryId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}