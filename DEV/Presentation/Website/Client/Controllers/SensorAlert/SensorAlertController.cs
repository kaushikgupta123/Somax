using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.SensorAlert;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.SensorAlert;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.SensorAlert
{
    public class SensorAlertController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.AlertProcedures)]
        public ActionResult Index()
        {
            SensorAlertVM sensorAlertVM = new SensorAlertVM();
            sensorAlertVM.security = this.userData.Security;
            LocalizeControls(sensorAlertVM, LocalizeResourceSetConstants.SensorAlertDetails);
            return View("~/Views/SensorAlert/Index.cshtml", sensorAlertVM);
        }
        private List<SensorAlertModel> GetSensorAlertSearchResult(List<SensorAlertModel> sensorAlertModelList, string clientLookupId, string description, string type, DateTime? createDate, string searchText)
        {
            if (sensorAlertModelList != null)
            {
                searchText = searchText.ToUpper();
                DateTime dateTime;
                DateTime.TryParseExact(searchText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                sensorAlertModelList = sensorAlertModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookUpId) && x.ClientLookUpId.ToUpper().Contains(searchText))
                                                 || (!string.IsNullOrWhiteSpace(x.Description.Trim()) && x.Description.Trim().ToUpper().Contains(searchText))
                                                 || (!string.IsNullOrWhiteSpace(x.Type.Trim()) && x.Type.Trim().ToUpper().Contains(searchText))
                                                || (x.CreateDate != null && x.CreateDate.Value != default(DateTime) && x.CreateDate.Value.Date.Equals(dateTime))
                                                ).ToList();

                if (!string.IsNullOrEmpty(clientLookupId))
                {
                    clientLookupId = clientLookupId.ToUpper();
                    sensorAlertModelList = sensorAlertModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookUpId) && x.ClientLookUpId.ToUpper().Contains(clientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(description))
                {
                    description = description.ToUpper();
                    sensorAlertModelList = sensorAlertModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(description))).ToList();
                }
                if (!string.IsNullOrEmpty(type))
                {
                    type = type.ToUpper();
                    sensorAlertModelList = sensorAlertModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Type) && x.Type.ToUpper().Equals(type))).ToList();
                }
                if (createDate != null)
                {
                    sensorAlertModelList = sensorAlertModelList.Where(x => (x.CreateDate != null && x.CreateDate.Value.Date.Equals(createDate.Value.Date))).ToList();
                }
            }            
            return sensorAlertModelList;
        }
        [HttpPost]
        public string GetSensorAlertGrid(int? draw, int? start, int? length, string clientLookupId, string description, string type, DateTime? createDate, string searchText = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            SensorAlertWrapper sWrapper = new SensorAlertWrapper(userData);
            var sensorList = sWrapper.GetSensorAlertDetails();
            List<string> typeList = new List<string>();
            if (sensorList != null)
            {
                typeList = sensorList.Where(x => !string.IsNullOrEmpty(x.Type)).Select(x => x.Type).Distinct().ToList();
                sensorList = GetSensorAlertSearchResult(sensorList, clientLookupId, description, type, createDate, searchText);
                sensorList = this.GetSensorAlertGridSortByColumnWithOrder(colname[0], orderDir, sensorList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = sensorList.Count();
            totalRecords = sensorList.Count();
            int initialPage = start.Value;
            var filteredResult = sensorList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, typeList = typeList }, JsonSerializerDateSettings);
        }
        private List<SensorAlertModel> GetSensorAlertGridSortByColumnWithOrder(string order, string orderDir, List<SensorAlertModel> data)
        {
            List<SensorAlertModel> lst = new List<SensorAlertModel>();
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                        break;
                    case "3":
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
        public string GetSensorAlertPrintData(string colname, string coldir, string clientLookupId = "", string description = "", string type = "", DateTime? createDate = null, string searchText = "")
        {
            List<SensorAlertPrintModel> sensorAlertPrintModelList = new List<SensorAlertPrintModel>();
            SensorAlertPrintModel objSensorAlertPrintModel;
            SensorAlertWrapper sWrapper = new SensorAlertWrapper(userData);
            var sensorAlertList = sWrapper.GetSensorAlertDetails();            
            List<string> typeList = new List<string>();
            if (sensorAlertList != null)
            {
                typeList = sensorAlertList.Where(x => !string.IsNullOrEmpty(x.Type)).Select(x => x.Type).Distinct().ToList();
                sensorAlertList = GetSensorAlertSearchResult(sensorAlertList, clientLookupId, description, type, createDate, searchText);
                sensorAlertList = this.GetSensorAlertGridSortByColumnWithOrder(colname, coldir, sensorAlertList);
            }
            if (sensorAlertList != null)
            {
                sensorAlertList = this.GetSensorAlertSearchResult(sensorAlertList, clientLookupId, description, type, createDate, searchText);
                foreach (var p in sensorAlertList)
                {
                    objSensorAlertPrintModel = new SensorAlertPrintModel();
                    objSensorAlertPrintModel.ClientLookUpId = p.ClientLookUpId;
                    objSensorAlertPrintModel.Description = p.Description;
                    objSensorAlertPrintModel.Type = p.Type;
                    objSensorAlertPrintModel.CreateDate = p.CreateDate;
                    sensorAlertPrintModelList.Add(objSensorAlertPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = sensorAlertPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion Search

        #region Details
        public PartialViewResult SensorAlertDetails(long sensorAlertProcedureId)
        {
            SensorAlertVM sensorAlertVM = new SensorAlertVM();
            SensorAlertWrapper sWrapper = new SensorAlertWrapper(userData);
            SensorAlertModel objSensorAlertModel = new SensorAlertModel();
            objSensorAlertModel = sWrapper.GetSensorAlertData(sensorAlertProcedureId);
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AllLookUps = objCommonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var LookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.SENSOR_ALT_TYPE).ToList();
                var valType = LookUplist.Where(x => x.ListValue == objSensorAlertModel.Type).FirstOrDefault();
                if (valType != null)
                {
                    objSensorAlertModel.Type = valType.ListValue;
                }
                objSensorAlertModel.TypeList = LookUplist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
            }
            sensorAlertVM.sensorAlertModel = objSensorAlertModel;
            sensorAlertVM.security = this.userData.Security; ;
            LocalizeControls(sensorAlertVM, LocalizeResourceSetConstants.SensorAlertDetails);
            return PartialView("~/Views/SensorAlert/_SensorAlertDetail.cshtml", sensorAlertVM);
        }
        [HttpGet]
        public PartialViewResult AddOrEditSensorAlert(long sensorAlertProcedureId = 0)
        {
            SensorAlertVM sensorAlertVM = new SensorAlertVM();
            SensorAlertModel objSensorAlertModel = new SensorAlertModel();
            if (sensorAlertProcedureId != 0)
            {
                SensorAlertWrapper sWrapper = new SensorAlertWrapper(userData);
                objSensorAlertModel = sWrapper.GetSensorAlertData(sensorAlertProcedureId);
                objSensorAlertModel.SensorAlertProcedureId = sensorAlertProcedureId;
            }
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AllLookUps = objCommonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var LookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.SENSOR_ALT_TYPE).ToList();
                var valType = LookUplist.Where(x => x.ListValue == objSensorAlertModel.Type).FirstOrDefault();
                if (valType != null)
                {
                    objSensorAlertModel.Type = valType.ListValue;
                }
                objSensorAlertModel.TypeList = LookUplist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
            }
            sensorAlertVM.sensorAlertModel = new SensorAlertModel();
            sensorAlertVM.sensorAlertModel = objSensorAlertModel;
            LocalizeControls(sensorAlertVM, LocalizeResourceSetConstants.SensorAlertDetails);
            return PartialView("~/Views/SensorAlert/_SensorAlertAddEdit.cshtml", sensorAlertVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateSensorAlert(SensorAlertVM sensorAlertVM, string Command)
        {
            string Mode = string.Empty;
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                SensorAlertModel objSanitationOnDemandLibModel = new SensorAlertModel();
                SensorAlertWrapper sWrapper = new SensorAlertWrapper(userData);
                SensorAlertProcedure objSensorAlertProcedure = new SensorAlertProcedure();
                if (sensorAlertVM.sensorAlertModel != null && sensorAlertVM.sensorAlertModel.SensorAlertProcedureId == 0)
                {
                    Mode = "add";
                    objSensorAlertProcedure = sWrapper.AddSensorAlert(sensorAlertVM.sensorAlertModel);
                }
                else
                {
                    Mode = "edit";
                    objSensorAlertProcedure = sWrapper.EditSensorAlert(sensorAlertVM.sensorAlertModel);

                }
                if (objSensorAlertProcedure.ErrorMessages != null && objSensorAlertProcedure.ErrorMessages.Count > 0)
                {
                    return Json(objSensorAlertProcedure.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, SensorAlertProcedureId = objSensorAlertProcedure.SensorAlertProcedureId, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public string GetSensorAlertTaskGrid(int? draw, int? start, int? length, long sensorAlertProcedureId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SensorAlertWrapper sWrapper = new SensorAlertWrapper(userData);
            List<SensorAlertTaskModel> SensorAlertTaskList = new List<SensorAlertTaskModel>();
            SensorAlertTaskList = sWrapper.PopulateSaTask(sensorAlertProcedureId);
            if (SensorAlertTaskList != null)
            {
                SensorAlertTaskList = this.GetAllTaskOrderSortByColumnWithOrder(order, orderDir, SensorAlertTaskList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = SensorAlertTaskList.Count();
            totalRecords = SensorAlertTaskList.Count();
            int initialPage = start.Value;
            var filteredResult = SensorAlertTaskList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            bool showActionButtons = userData.Security.Sensors.AlertProcedures;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, showActionButtons= showActionButtons }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<SensorAlertTaskModel> GetAllTaskOrderSortByColumnWithOrder(string order, string orderDir, List<SensorAlertTaskModel> data)
        {
            List<SensorAlertTaskModel> lst = new List<SensorAlertTaskModel>();
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

        public PartialViewResult AddOrEditSaTask(string taskId = "", long sensorAlertProcedureId = 0, long sensorAlertProcedureTaskId = 0, string description = "", string clientLookUpId = "")
        {
            SensorAlertVM sensorAlertVM = new SensorAlertVM();
            SensorAlertTaskModel objSensorAlertTaskModel = new SensorAlertTaskModel();
            SensorAlertWrapper sWrapper = new SensorAlertWrapper(userData);
            sensorAlertVM.sensorAlertTaskModel = new SensorAlertTaskModel();
            objSensorAlertTaskModel.SensorAlertProcedureId = sensorAlertProcedureId;
            //objSensorAlertTaskModel.TaskId = clientLookUpId;
            objSensorAlertTaskModel.ClientLookUpId = clientLookUpId;
            if (sensorAlertProcedureTaskId != 0)
            {
                objSensorAlertTaskModel.TaskId = taskId;
                objSensorAlertTaskModel.SensorAlertProcedureTaskId = sensorAlertProcedureTaskId;
                objSensorAlertTaskModel.Description = description;
            }
            sensorAlertVM.sensorAlertTaskModel = objSensorAlertTaskModel;
            LocalizeControls(sensorAlertVM, LocalizeResourceSetConstants.SensorAlertDetails);
            return PartialView("~/Views/Sensoralert/_TaskAddEdit.cshtml", sensorAlertVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSaTask(SensorAlertVM objVM)
        {
            if (ModelState.IsValid)
            {
                SensorAlertWrapper sWrapper = new SensorAlertWrapper(userData);
                string mode = string.Empty;
                List<String> errorList = new List<string>();
                SensorAlertProcedureTask saProcTask = new SensorAlertProcedureTask();
                if (objVM.sensorAlertTaskModel.SensorAlertProcedureTaskId == 0)
                {
                    mode = "add";
                    saProcTask = sWrapper.AddSaTask(objVM.sensorAlertTaskModel);
                    errorList = saProcTask.ErrorMessages;
                }
                else
                {
                    mode = "update";
                    saProcTask = sWrapper.EditSaTask(objVM.sensorAlertTaskModel);
                    errorList = saProcTask.ErrorMessages;
                }

                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SensorAlertProcedureId = objVM.sensorAlertTaskModel.SensorAlertProcedureId, mode = mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteSaTask(long sensorAlertProcedureTaskId)
        {
            SensorAlertWrapper sWrapper = new SensorAlertWrapper(userData);
            if (sWrapper.DeleteSaTask(sensorAlertProcedureTaskId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Task
    }
}