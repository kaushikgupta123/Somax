using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Globalization;
using Common.Constants;
using Client.BusinessWrapper.PrevMaintWrapper;
using System.Web.Mvc;
using Client.Models.PreventiveMaintenance;
using Client.Common;
using Client.BusinessWrapper.Common;
using Client.Controllers.Common;
using Client.ActionFilters;

namespace Client.Controllers.Preventive
{
    public class PMForcastController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.PrevMaint_PMForecast)]
        public ActionResult Index()
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PMForcastModel pMForcastModel = new PMForcastModel();
            pMForcastModel.ForecastDate = DateTime.Now.AddDays(7);
            pMForcastModel.OnDemandGroupList = new List<SelectListItem>();
            var ScheduleTypeList = UtilityFunction.GetScheduleType().Where(x => x.value != ScheduleTypeConstants.Meter).ToList();
            if (ScheduleTypeList != null)
            {
                pMForcastModel.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var Assignedlist = GetList_PersonnelV2();
            if (Assignedlist != null)
            {
                pMForcastModel.PersonnelIdList = Assignedlist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });

            }
            //V2-1082
            var AllLookUps = pWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (Shift != null)
                {
                    var tmpShift = Shift.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    pMForcastModel.ShiftList = tmpShift.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }
            }
            var DownRequiredStatusList = UtilityFunction.DownRequiredStatusTypesWithBoolValue();
            if (DownRequiredStatusList != null)
            {
                pMForcastModel.DownRequiredInactiveFlagList = DownRequiredStatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            //V2-1207
            objPrevMaintVM.TypeList = AllLookUps.Where(x => x.ListName.ToLower() == LookupListConstants.Preventive_Maint_WO_Type.ToLower()).ToList().Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();

            objPrevMaintVM.pMForcastModel = pMForcastModel;
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return View(objPrevMaintVM);
        }
        [HttpPost]
        public string GetPreventiveMainForcastGrid(int? draw, int? start, int? length, string ScheduleType, string OnDemandGroup, string ForcastDate, DateTime? SchedueledDate, decimal? Duration, decimal? EstLaborHours, decimal? EstOtherCost, decimal? EstMaterialCost, string AssignedTo_PersonnelClientLookupId = "", string ChargeToClientLookupId = "", string ChargeToName = "", string SearchText = "", string ClientLookupId = "", string Description = "", List<string> assignedPMS = null, string requiredDate = "", bool? downRequired=null, List<string> shifts = null, List<string>TypeList = null)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            DateTime dt = DateTime.ParseExact(ForcastDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            string timeoutError= string.Empty;
            List<PMForcastModel> PrevMaintForcastList = pWrapper.populatePrevMaintForcast(dt, ScheduleType, OnDemandGroup, ref timeoutError,  assignedPMS , requiredDate, downRequired, shifts);
            
            if (PrevMaintForcastList != null && string.IsNullOrEmpty(timeoutError))
            {
                PrevMaintForcastList = this.GetPMForcastListGridSortByColumnWithOrder(order, orderDir, PrevMaintForcastList);
                SearchText = SearchText.ToUpper();
                int VAL;
                bool res = int.TryParse(SearchText, out VAL);
                decimal val;
                bool outval = decimal.TryParse(SearchText, out val);
                DateTime dateTime;
                DateTime.TryParseExact(SearchText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(SearchText))
                                                || (!string.IsNullOrWhiteSpace(x.Description.Trim()) && x.Description.Trim().ToUpper().Contains(SearchText))
                                                || (x.SchedueledDate != null && x.SchedueledDate.Value != default(DateTime) && x.SchedueledDate.Value.Date.Equals(dateTime))
                                                || (!string.IsNullOrWhiteSpace(x.AssignedTo_PersonnelClientLookupId) && x.AssignedTo_PersonnelClientLookupId.ToUpper().Contains(SearchText))
                                                || (!string.IsNullOrWhiteSpace(x.ChargeToClientLookupId) && x.ChargeToClientLookupId.ToUpper().Contains(SearchText))
                                                || (!string.IsNullOrWhiteSpace(x.ChargeToName) && x.ChargeToName.ToUpper().Contains(SearchText))
                                                || (outval == true && x.Duration.Equals(val))
                                                || (outval == true && x.EstLaborHours.Equals(val))
                                                || (outval == true && x.EstLaborCost.Equals(val))
                                                || (outval == true && x.EstOtherCost.Equals(val))
                                                || (outval == true && x.EstMaterialCost.Equals(val))
                                                || (!string.IsNullOrWhiteSpace(x.Shift) && x.Shift.ToUpper().Contains(SearchText))
                                                ).ToList();
                if (!string.IsNullOrEmpty(ClientLookupId))
                {
                    ClientLookupId = ClientLookupId.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeToName))
                {
                    ChargeToName = ChargeToName.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeToName) && x.ChargeToName.ToUpper().Contains(ChargeToName))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (SchedueledDate != null)
                {
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (x.SchedueledDate != null && x.SchedueledDate.Equals(SchedueledDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(AssignedTo_PersonnelClientLookupId))
                {
                    AssignedTo_PersonnelClientLookupId = AssignedTo_PersonnelClientLookupId.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.AssignedTo_PersonnelClientLookupId) && x.AssignedTo_PersonnelClientLookupId.ToUpper().Contains(AssignedTo_PersonnelClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeToClientLookupId))
                {
                    ChargeToClientLookupId = ChargeToClientLookupId.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeToClientLookupId) && x.ChargeToClientLookupId.ToUpper().Contains(ChargeToClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeToName))
                {
                    ChargeToName = ChargeToName.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeToName) && x.ChargeToName.ToUpper().Contains(ChargeToName))).ToList();
                }
                if (Duration.HasValue)
                {
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => x.Duration.Equals(Duration)).ToList();
                }
                if (EstLaborHours.HasValue)
                {
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => x.EstLaborHours.Equals(EstLaborHours)).ToList();
                }
                if (EstOtherCost.HasValue)
                {
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => x.EstOtherCost.Equals(EstOtherCost)).ToList();
                }
                if (EstMaterialCost.HasValue)
                {
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => x.EstMaterialCost.Equals(EstMaterialCost)).ToList();
                }
                if (TypeList != null && TypeList.Count > 0)
                {
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => TypeList.Contains(x.Type)).ToList();
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PrevMaintForcastList.Count();
            totalRecords = PrevMaintForcastList.Count();
            int initialPage = start.Value;
            var filteredResult = PrevMaintForcastList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, timeoutError = timeoutError }, JsonSerializerDateSettings);
        }
        private List<PMForcastModel> GetPMForcastListGridSortByColumnWithOrder(string order, string orderDir, List<PMForcastModel> data)
        {
            List<PMForcastModel> lst = new List<PMForcastModel>();
            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SchedueledDate).ToList() : data.OrderBy(p => p.SchedueledDate).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RequiredDate).ToList() : data.OrderBy(p => p.RequiredDate).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Assigned).ToList() : data.OrderBy(p => p.Assigned).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToClientLookupId).ToList() : data.OrderBy(p => p.ChargeToClientLookupId).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToName).ToList() : data.OrderBy(p => p.ChargeToName).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Duration).ToList() : data.OrderBy(p => p.Duration).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EstLaborHours).ToList() : data.OrderBy(p => p.EstLaborHours).ToList();
                    break;
                case "10":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EstLaborCost).ToList() : data.OrderBy(p => p.EstLaborCost).ToList();
                    break;
                case "11":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EstOtherCost).ToList() : data.OrderBy(p => p.EstOtherCost).ToList();
                    break;
                case "12":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EstMaterialCost).ToList() : data.OrderBy(p => p.EstMaterialCost).ToList();
                    break;
                case "13":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DownRequired).ToList() : data.OrderBy(p => p.DownRequired).ToList();
                    break;
                case "14":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Shift).ToList() : data.OrderBy(p => p.Shift).ToList();
                    break;
                case "15":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        public JsonResult GetOnDemandGroupList()
        {
            List<SelectListItem> OnDemandGroupList = new List<SelectListItem>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var onDemandList = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_Ondemand_Grp).ToList();
                if (onDemandList != null)
                {
                    OnDemandGroupList = onDemandList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).ToList();
                }
            }
            return Json(OnDemandGroupList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string GetPreventiveMainForcastGridPrintData(string _colname, string _coldir, string ScheduleType, string OnDemandGroup, string ForcastDate, DateTime? SchedueledDate, decimal? Duration, decimal? EstLaborHours, decimal? EstOtherCost, decimal? EstMaterialCost, string AssignedTo_PersonnelClientLookupId = "", string ChargeToClientLookupId = "", string ChargeToName = "", string SearchText = "", string ClientLookupId = "", string Description = "", List<string> assignedPMS = null, string requiredDate = "", bool? downRequired=null, List<string> shifts = null, List<string> TypeList = null)
        {
            DateTime dt = DateTime.ParseExact(ForcastDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            string timeoutError = string.Empty;
            List<PMForcastModel> PrevMaintForcastList = pWrapper.populatePrevMaintForcast(dt, ScheduleType, OnDemandGroup, ref timeoutError ,assignedPMS, requiredDate, downRequired, shifts);
            PreventiveMaintenanceForcastPrintModel objPMForcastPrintModel;
            List<PreventiveMaintenanceForcastPrintModel> pMForcastPrintModelList = new List<PreventiveMaintenanceForcastPrintModel>();
            if (PrevMaintForcastList != null)
            {
                SearchText = SearchText.ToUpper();
                int VAL;
                bool res = int.TryParse(SearchText, out VAL);
                decimal val;
                bool outval = decimal.TryParse(SearchText, out val);
                DateTime dateTime;
                DateTime.TryParseExact(SearchText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(SearchText))
                                                || (!string.IsNullOrWhiteSpace(x.Description.Trim()) && x.Description.Trim().ToUpper().Contains(SearchText))
                                                || (x.SchedueledDate != null && x.SchedueledDate.Value != default(DateTime) && x.SchedueledDate.Value.Date.Equals(dateTime))
                                                || (!string.IsNullOrWhiteSpace(x.AssignedTo_PersonnelClientLookupId) && x.AssignedTo_PersonnelClientLookupId.ToUpper().Contains(SearchText))
                                                || (!string.IsNullOrWhiteSpace(x.ChargeToClientLookupId) && x.ChargeToClientLookupId.ToUpper().Contains(SearchText))
                                                || (!string.IsNullOrWhiteSpace(x.ChargeToName) && x.ChargeToName.ToUpper().Contains(SearchText))
                                                || (outval == true && x.Duration.Equals(val))
                                                || (outval == true && x.EstLaborHours.Equals(val))
                                                || (outval == true && x.EstOtherCost.Equals(val))
                                                || (outval == true && x.EstMaterialCost.Equals(val))
                                                || (!string.IsNullOrWhiteSpace(x.Shift) && x.Shift.ToUpper().Contains(SearchText))
                                                ).ToList();
                if (!string.IsNullOrEmpty(ClientLookupId))
                {
                    ClientLookupId = ClientLookupId.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeToName))
                {
                    ChargeToName = ChargeToName.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeToName) && x.ChargeToName.ToUpper().Contains(ChargeToName))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (SchedueledDate != null)
                {
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (x.SchedueledDate != null && x.SchedueledDate.Equals(SchedueledDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(AssignedTo_PersonnelClientLookupId))
                {
                    AssignedTo_PersonnelClientLookupId = AssignedTo_PersonnelClientLookupId.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.AssignedTo_PersonnelClientLookupId) && x.AssignedTo_PersonnelClientLookupId.ToUpper().Contains(AssignedTo_PersonnelClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeToClientLookupId))
                {
                    ChargeToClientLookupId = ChargeToClientLookupId.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeToClientLookupId) && x.ChargeToClientLookupId.ToUpper().Contains(ChargeToClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeToName))
                {
                    ChargeToName = ChargeToName.ToUpper();
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeToName) && x.ChargeToName.ToUpper().Contains(ChargeToName))).ToList();
                }
                if (Duration.HasValue)
                {
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => x.Duration.Equals(Duration)).ToList();
                }
                if (EstLaborHours.HasValue)
                {
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => x.EstLaborHours.Equals(EstLaborHours)).ToList();
                }
                if (EstOtherCost.HasValue)
                {
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => x.EstOtherCost.Equals(EstOtherCost)).ToList();
                }
                if (EstMaterialCost.HasValue)
                {
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => x.EstMaterialCost.Equals(EstMaterialCost)).ToList();
                }
                if (TypeList != null && TypeList.Count > 0)
                {
                    PrevMaintForcastList = PrevMaintForcastList.Where(x => TypeList.Contains(x.Type)).ToList();
                }
            }
            PrevMaintForcastList = this.GetPMForcastListGridSortByColumnWithOrder(_colname, _coldir, PrevMaintForcastList);
            foreach (var p in PrevMaintForcastList)
            {
                objPMForcastPrintModel = new PreventiveMaintenanceForcastPrintModel();
                objPMForcastPrintModel.ClientLookupId = p.ClientLookupId;
                objPMForcastPrintModel.Description = p.Description;
                objPMForcastPrintModel.SchedueledDate = p.SchedueledDate;
                objPMForcastPrintModel.ChargeToClientLookupId = p.ChargeToClientLookupId;
                objPMForcastPrintModel.ChargeToName = p.ChargeToName;
                objPMForcastPrintModel.Duration = p.Duration;
                objPMForcastPrintModel.EstLaborHours = p.EstLaborHours;
                objPMForcastPrintModel.EstLaborCost = p.EstLaborCost;
                objPMForcastPrintModel.EstOtherCost = p.EstOtherCost;
                objPMForcastPrintModel.EstMaterialCost = p.EstMaterialCost;
                objPMForcastPrintModel.RequiredDate = p.RequiredDate;
                objPMForcastPrintModel.Assigned = p.Assigned;
                objPMForcastPrintModel.EstMaterialCost = p.EstMaterialCost;
                objPMForcastPrintModel.DownRequired = p.DownRequired;
                objPMForcastPrintModel.Shift = p.Shift;
                objPMForcastPrintModel.Type = p.Type;
                pMForcastPrintModelList.Add(objPMForcastPrintModel);
            }
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "MM/dd/yyyy";
            return JsonConvert.SerializeObject(new { data = pMForcastPrintModelList , timeoutError = timeoutError }, jsonSettings);
        }
        public JsonResult ProcessPMForecast(PrevMaintVM objPrevMaintVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
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