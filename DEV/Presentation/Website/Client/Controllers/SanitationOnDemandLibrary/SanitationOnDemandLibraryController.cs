using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Configuration.SanitationOnDemandLibrary;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.SanitationOnDemandLibrary;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.SanitationOnDemandLibrary
{
    public class SanitationOnDemandLibraryController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.Sanitation_OnDemand)]
        public ActionResult Index()
        {
            SanitationOnDemandLibVM sanitationOnDemandLibVM = new SanitationOnDemandLibVM();
            SanitationOnDemandSecurityModel sanitationOnDemandSecurityModel = new SanitationOnDemandSecurityModel();
            sanitationOnDemandSecurityModel.ShowAddBtn = userData.Security.Sanitation.OnDemand;
            sanitationOnDemandLibVM.sanitationOnDemandSecurityModel = sanitationOnDemandSecurityModel;
            LocalizeControls(sanitationOnDemandLibVM, LocalizeResourceSetConstants.LibraryDetails);
            return View("~/Views/SanitationOnDemandLibrary/index.cshtml", sanitationOnDemandLibVM);
        }
        private List<SanitationOnDemandLibModel> GetSanitOnDemandSearchResult(List<SanitationOnDemandLibModel> SanitationOnDemandLibModelList, string ClientLookupId, string Description, DateTime? CreateDate, string SearchText)
        {
            if (SanitationOnDemandLibModelList != null)
            {
                SearchText = SearchText.ToUpper();
                DateTime dateTime;
                DateTime.TryParseExact(SearchText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                SanitationOnDemandLibModelList = SanitationOnDemandLibModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookUpId) && x.ClientLookUpId.ToUpper().Contains(SearchText))
                                                 || (!string.IsNullOrWhiteSpace(x.Description.Trim()) && x.Description.Trim().ToUpper().Contains(SearchText))
                                                || (x.CreateDate != null && x.CreateDate.Value != default(DateTime) && x.CreateDate.Value.Date.Equals(dateTime))
                                                ).ToList();

                if (!string.IsNullOrEmpty(ClientLookupId))
                {
                    ClientLookupId = ClientLookupId.ToUpper();
                    SanitationOnDemandLibModelList = SanitationOnDemandLibModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookUpId) && x.ClientLookUpId.ToUpper().Contains(ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    SanitationOnDemandLibModelList = SanitationOnDemandLibModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (CreateDate != null)
                {
                    SanitationOnDemandLibModelList = SanitationOnDemandLibModelList.Where(x => (x.CreateDate != null && x.CreateDate.Value.Date.Equals(CreateDate.Value.Date))).ToList();
                }
            }
            return SanitationOnDemandLibModelList;
        }
        [HttpPost]
        public string GetSanitOnDemandLibraryGrid(int? draw, int? start, int? length, string ClientLookupId, string Description, DateTime? CreateDate, string SearchText = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            SanitationOnDemandLibraryWrapper sWrapper = new SanitationOnDemandLibraryWrapper(userData);
            var SanLibraryList = sWrapper.PopulateSanitOnDemandLibData();
            if (SanLibraryList != null)
            {
                SanLibraryList = GetSanitOnDemandSearchResult(SanLibraryList, ClientLookupId, Description, CreateDate, SearchText);
                SanLibraryList = this.GetSODLibListGridSortByColumnWithOrder(colname[0], orderDir, SanLibraryList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = SanLibraryList.Count();
            totalRecords = SanLibraryList.Count();
            int initialPage = start.Value;
            var filteredResult = SanLibraryList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<SanitationOnDemandLibModel> GetSODLibListGridSortByColumnWithOrder(string order, string orderDir, List<SanitationOnDemandLibModel> data)
        {
            List<SanitationOnDemandLibModel> lst = new List<SanitationOnDemandLibModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookUpId).ToList() : data.OrderBy(p => p.ClientLookUpId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate.Value.Date).ToList() : data.OrderBy(p => p.CreateDate.Value.Date).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookUpId).ToList() : data.OrderBy(p => p.ClientLookUpId).ToList();
                        break;
                }
            }
            return lst;
        }
        [HttpGet]
        public string GetSanitOnDemandLibraryPrintData(string colname, string coldir, string ClientLookupId = "", string Description = "", DateTime? CreateDate = null, string SearchText = "")
        {
            List<SanitationOnDemandLibPrintModel> sanitationJobPrintModelList = new List<SanitationOnDemandLibPrintModel>();
            SanitationOnDemandLibPrintModel objSanitationOnDemandLibPrintModel;
            SanitationOnDemandLibraryWrapper sWrapper = new SanitationOnDemandLibraryWrapper(userData);
            var SanitLibList = sWrapper.PopulateSanitOnDemandLibData();
            if (SanitLibList != null)
            {
                SanitLibList = this.GetSODLibListGridSortByColumnWithOrder(colname, coldir, SanitLibList);
                SanitLibList = this.GetSanitOnDemandSearchResult(SanitLibList, ClientLookupId, Description, CreateDate, SearchText);
                foreach (var p in SanitLibList)
                {
                    objSanitationOnDemandLibPrintModel = new SanitationOnDemandLibPrintModel();
                    objSanitationOnDemandLibPrintModel.ClientLookUpId = p.ClientLookUpId;
                    objSanitationOnDemandLibPrintModel.Description = p.Description;
                    objSanitationOnDemandLibPrintModel.CreateDate = p.CreateDate;
                    sanitationJobPrintModelList.Add(objSanitationOnDemandLibPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = sanitationJobPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion Search
        #region Details
        public PartialViewResult SanitOnDemandLibDetails(long sanOnDemandMasterId)
        {
            SanitationOnDemandLibVM sanitationOnDemandLibVM = new SanitationOnDemandLibVM();
            SanitationOnDemandLibraryWrapper sWrapper = new SanitationOnDemandLibraryWrapper(userData);
            SanitationOnDemandLibModel sanitationOnDemandLibModel = new SanitationOnDemandLibModel();
            sanitationOnDemandLibModel = sWrapper.PopulateSanitOnDemandLibData(sanOnDemandMasterId);
            sanitationOnDemandLibModel.SanOnDemandMasterId = sanOnDemandMasterId;
            sanitationOnDemandLibVM.sanitationOnDemandLibModel = sanitationOnDemandLibModel;
            SanitationOnDemandSecurityModel sanitationOnDemandSecurityModel = new SanitationOnDemandSecurityModel();
            sanitationOnDemandSecurityModel.ShowAddBtn = userData.Security.Sanitation.OnDemand;
            sanitationOnDemandSecurityModel.ShowEditButton = userData.Security.SanitationJob.Edit;
            sanitationOnDemandLibVM.sanitationOnDemandSecurityModel = sanitationOnDemandSecurityModel;
            LocalizeControls(sanitationOnDemandLibVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/SanitationOnDemandLibrary/_SanitOnDemandLibDetails.cshtml", sanitationOnDemandLibVM);
        }
        [HttpGet]
        public PartialViewResult AddOrEditSanitOnDemandLib(long sanOnDemandMasterId = 0)
        {
            SanitationOnDemandLibVM sanitationOnDemandLibVM = new SanitationOnDemandLibVM();
            SanitationOnDemandLibModel objSanitationOnDemandLibModel = new SanitationOnDemandLibModel();
            if (sanOnDemandMasterId != 0)
            {
                SanitationOnDemandLibraryWrapper sWrapper = new SanitationOnDemandLibraryWrapper(userData);
                objSanitationOnDemandLibModel = sWrapper.PopulateSanitOnDemandLibData(sanOnDemandMasterId);
                objSanitationOnDemandLibModel.SanOnDemandMasterId = sanOnDemandMasterId;
            }
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AllLookUps = objCommonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var LookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.SANIT_ODEM_TYPE).ToList();
                var valType = LookUplist.Where(x => x.ListValue == objSanitationOnDemandLibModel.Type).FirstOrDefault();
                if (valType != null)
                {
                    objSanitationOnDemandLibModel.Type = valType.ListValue;
                }
                objSanitationOnDemandLibModel.TypeList = LookUplist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
            }
            sanitationOnDemandLibVM.sanitationOnDemandLibModel = new SanitationOnDemandLibModel();
            sanitationOnDemandLibVM.sanitationOnDemandLibModel = objSanitationOnDemandLibModel;
            LocalizeControls(sanitationOnDemandLibVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/SanitationOnDemandLibrary/_AddEditSanitOnDemandLib.cshtml", sanitationOnDemandLibVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateSanitOnDemandLib(SanitationOnDemandLibVM sanitationOnDemandLibVM, string Command)
        {
            string Mode = string.Empty;
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                SanitationOnDemandLibModel objSanitationOnDemandLibModel = new SanitationOnDemandLibModel();
                SanitationOnDemandLibraryWrapper sWrapper = new SanitationOnDemandLibraryWrapper(userData);
                SanOnDemandMaster sanOnDemandMaster = new SanOnDemandMaster();
                if (sanitationOnDemandLibVM.sanitationOnDemandLibModel != null && sanitationOnDemandLibVM.sanitationOnDemandLibModel.SanOnDemandMasterId == 0)
                {
                    Mode = "add";
                    sanOnDemandMaster = sWrapper.AddSanit(sanitationOnDemandLibVM.sanitationOnDemandLibModel);
                }
                else
                {
                    Mode = "edit";
                    sanOnDemandMaster = sWrapper.EditSanit(sanitationOnDemandLibVM.sanitationOnDemandLibModel);

                }
                if (sanOnDemandMaster.ErrorMessages != null && sanOnDemandMaster.ErrorMessages.Count > 0)
                {
                    return Json(sanOnDemandMaster.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, SanOnDemandMasterId = sanOnDemandMaster.SanOnDemandMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Details
        #region Task
        #region Populate Task
        [HttpPost]
        public string GetSanitOnDemandLibTaskGrid(int? draw, int? start, int? length, long SanOnDemandMasterId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SanitationOnDemandLibraryWrapper sWrapper = new SanitationOnDemandLibraryWrapper(userData);
            List<SanitOnDemandLibTaskModel> TaskList = sWrapper.PopulateTask(SanOnDemandMasterId);
            if (TaskList != null)
            {
                TaskList = this.GetAllTaskOrderSortByColumnWithOrder(order, orderDir, TaskList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = TaskList.Count();
            totalRecords = TaskList.Count();
            int initialPage = start.Value;
            var filteredResult = TaskList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            bool showAddEditBtn = userData.Security.SanitationJob.Edit;
            bool showDeleteBtn = userData.Security.Sanitation.OnDemand;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, showAddEditBtn = showAddEditBtn,  showDeleteBtn = showDeleteBtn }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<SanitOnDemandLibTaskModel> GetAllTaskOrderSortByColumnWithOrder(string order, string orderDir, List<SanitOnDemandLibTaskModel> data)
        {
            List<SanitOnDemandLibTaskModel> lst = new List<SanitOnDemandLibTaskModel>();
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
        #endregion Populate Grid
        #region AddEdit Task
        public PartialViewResult AddOrEditSanitTask(string taskId = "", long sanOnDemandMasterId = 0, long sanitOnDemandMasterTaskId = 0, string description = "", string clientLookUpId = "")
        {
            SanitationOnDemandLibVM sanitationOnDemandLibVM = new SanitationOnDemandLibVM();
            SanitOnDemandLibTaskModel objSanitOnDemandLibTaskModel = new SanitOnDemandLibTaskModel();
            SanitationOnDemandLibraryWrapper sWrapper = new SanitationOnDemandLibraryWrapper(userData);
            sanitationOnDemandLibVM.sanitOnDemandLibTaskModel = new SanitOnDemandLibTaskModel();
            objSanitOnDemandLibTaskModel.SanOnDemandMasterId = sanOnDemandMasterId;
            objSanitOnDemandLibTaskModel.ClientLookUpId = clientLookUpId;
            if (sanitOnDemandMasterTaskId != 0)
            {
                objSanitOnDemandLibTaskModel.TaskId = taskId;
                objSanitOnDemandLibTaskModel.SanOnDemandMasterTaskId = sanitOnDemandMasterTaskId;
                objSanitOnDemandLibTaskModel.Description = description;
            }
            sanitationOnDemandLibVM.sanitOnDemandLibTaskModel = objSanitOnDemandLibTaskModel;
            LocalizeControls(sanitationOnDemandLibVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/SanitationOnDemandLibrary/_AddEditSanitOnDemandTask.cshtml", sanitationOnDemandLibVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSanitTask(SanitationOnDemandLibVM objVM)
        {
            if (ModelState.IsValid)
            {
                SanitationOnDemandLibraryWrapper sWrapper = new SanitationOnDemandLibraryWrapper(userData);
                string mode = string.Empty;
                List<String> errorList = new List<string>();
                SanOnDemandMasterTask sanitMast = new SanOnDemandMasterTask();
                if (objVM.sanitOnDemandLibTaskModel.SanOnDemandMasterTaskId == 0)
                {
                    mode = "add";
                    sanitMast = sWrapper.AddSanitTask(objVM.sanitOnDemandLibTaskModel);
                    errorList = sanitMast.ErrorMessages;
                }
                else
                {
                    mode = "update";
                    sanitMast = sWrapper.EditSanitTask(objVM.sanitOnDemandLibTaskModel);
                    errorList = sanitMast.ErrorMessages;
                }

                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SanOnDemandMasterId = objVM.sanitOnDemandLibTaskModel.SanOnDemandMasterId, mode = mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteSanitTask(long sanOnDemandMasterTaskId)
        {
            SanitationOnDemandLibraryWrapper sWrapper = new SanitationOnDemandLibraryWrapper(userData);
            if (sWrapper.DeleteSanitTask(sanOnDemandMasterTaskId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion AddEdit Task
        #endregion Task
    }
}
