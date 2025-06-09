using Client.BusinessWrapper.LaborSchedule;
using Client.Models.LaborScheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Globalization;
using Common.Constants;
using System.Web.Mvc;
using Client.Controllers.Common;
using Client.ActionFilters;
using Client.Common;
using Client.BusinessWrapper.Common;

namespace Client.Controllers.LaborScheduling
{
    public class LaborSchedulingDailyController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.WorkOrder_LaborScheduling)]
        public ActionResult Index()
        {
            LaborSchedulingVM LVM = new LaborSchedulingVM();
            LaborSchedulingModel objLaborModel = new LaborSchedulingModel();
            objLaborModel.Hours = 0;
            objLaborModel.Date = DateTime.Today;
            var PersonnelLookUplist = GetList_PersonnelV2();
            if (PersonnelLookUplist != null)
            {
                objLaborModel.PersonnelIdList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            LVM.LaborSchedulingModel = objLaborModel;
            LocalizeControls(LVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return View(LVM);
        }
        [HttpGet]
        public JsonResult GetLaborScheduling(int PersonnelId, string Date, string Flag = "1")
        {
            DateTime dt = DateTime.ParseExact(Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            Date = dt.ToString("MM/dd/yyyy");
            LaborScheduleWrapper pWrapper = new LaborScheduleWrapper(userData);
            List<LaborSchedulingModel> LabourSchedulingList = pWrapper.PopulateLaborScheduling(Convert.ToString(PersonnelId), Date, Flag);
            return Json(LabourSchedulingList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string GetLaborSchedulingMainGrid(int? draw, int? start, int? length, int PersonnelId, string Date, string Flag = "1")
        {
            decimal totalHours = 0;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            DateTime dt = DateTime.ParseExact(Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            Date = dt.ToString("MM/dd/yyyy");
            LaborScheduleWrapper pWrapper = new LaborScheduleWrapper(userData);
            List<LaborSchedulingModel> LabourSchedulingList = pWrapper.PopulateLaborScheduling(Convert.ToString(PersonnelId), Date, Flag);
            if (LabourSchedulingList != null && LabourSchedulingList.Count > 0)
            {
                totalHours = LabourSchedulingList.Sum(x => x.Hours).Value;
                LabourSchedulingList = this.GetLabourSchedulingListGridSortByColumnWithOrder(order, orderDir, LabourSchedulingList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = LabourSchedulingList.Count();
            totalRecords = LabourSchedulingList.Count();
            int initialPage = start.Value;
            var filteredResult = LabourSchedulingList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, totalHours = totalHours }, JsonSerializerDateSettings);
        }
        private List<LaborSchedulingModel> GetLabourSchedulingListGridSortByColumnWithOrder(string order, string orderDir, List<LaborSchedulingModel> data)
        {
            List<LaborSchedulingModel> lst = new List<LaborSchedulingModel>();
            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Hours).ToList() : data.OrderBy(p => p.Hours).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Date).ToList() : data.OrderBy(p => p.Date).ToList();
                    break;

                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        [HttpPost]
        public JsonResult DeleteLabor(string[] WorkSchedlIds, int PersonnelId, string Date)
        {
            DateTime dt = DateTime.ParseExact(Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            //Date = dt.ToString("MM/dd/yyyy");
            LaborScheduleWrapper pWrapper = new LaborScheduleWrapper(userData);
            List<LaborSchedulingModel> LabourSchedulingList = pWrapper.PopulateLaborSchedulingAfterRemove(WorkSchedlIds, Convert.ToString(PersonnelId), dt, "1");
            return Json(LabourSchedulingList, JsonRequestBehavior.AllowGet);
        }
        #region Available Work Orders
        [HttpGet]
        public JsonResult GetLaborAvailable(string ClientLookupId, string ChargeTo, string ChargeToName,
            string Description, List<string> Status = null, List<string> Priority = null, bool? DownRequired = null, List<string> Assigned = null, List<string> Type = null,
            DateTime? StartDate = null, decimal Duration = 0, DateTime? RequiredDate = null, string flag = "0")
        {
            LaborScheduleWrapper pWrapper = new LaborScheduleWrapper(userData);
            string _RequiredDate = string.Empty;
            string _StartDate = string.Empty;
            _RequiredDate = RequiredDate.HasValue ? RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _StartDate = StartDate.HasValue ? StartDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            List<AvailableSchedulingModel> LabourAvailableList = pWrapper.PopulateLaborAvailableV2("1", "asc", 0, 0, ClientLookupId, ChargeTo,
                ChargeToName, Description, Status, Priority, DownRequired, Assigned, Type, _StartDate, Duration, _RequiredDate, flag);
            return Json(LabourAvailableList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        
        public string GetAvailableWorkOrderMainGrid(int? draw, int? start, int? length, string ClientLookupId, string ChargeTo, string ChargeToName,
            string Description, List<string> Status = null, List<string> Priority = null, bool? DownRequired = null, List<string> Assigned=null, List<string> Type=null,
            DateTime? StartDate=null, decimal Duration=0, DateTime? RequiredDate=null, string flag="")

        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            string _RequiredDate = string.Empty;
            string _StartDate = string.Empty;
            _RequiredDate = RequiredDate.HasValue ? RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _StartDate = StartDate.HasValue ? StartDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            LaborScheduleWrapper pWrapper = new LaborScheduleWrapper(userData);
            start = start.HasValue
             ? start / length
             : 0;
            int skip = start * length ?? 0;
            List<AvailableSchedulingModel> LabourAvailableList = pWrapper.PopulateLaborAvailableV2(order, orderDir, skip, length ?? 0, ClientLookupId, ChargeTo,
                ChargeToName, Description, Status, Priority, DownRequired, Assigned,Type,_StartDate ,Duration,_RequiredDate, flag);

            var totalRecords = 0;
            var recordsFiltered = 0;
            // RKL - Support Ticket 10066
            // JIRA - V2-1066
            // Data is filtered before we get here 
            // Why do we need this 
            /*
            int initialPage = start.Value;
            var filteredResult = LabourAvailableList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            */
            if (LabourAvailableList != null && LabourAvailableList.Count > 0)
            {
                recordsFiltered = LabourAvailableList[0].TotalCount;
                totalRecords = LabourAvailableList[0].TotalCount;
            }
            

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = LabourAvailableList }, JsonSerializerDateSettings);
        }
        private List<AvailableSchedulingModel> GetLabourAvailableListGridSortByColumnWithOrder(string order, string orderDir, List<AvailableSchedulingModel> data)
        {
            List<AvailableSchedulingModel> lst = new List<AvailableSchedulingModel>();
            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo).ToList() : data.OrderBy(p => p.ChargeTo).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToName).ToList() : data.OrderBy(p => p.ChargeToName).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Priority).ToList() : data.OrderBy(p => p.Priority).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DownRequired).ToList() : data.OrderBy(p => p.DownRequired).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Assigned).ToList() : data.OrderBy(p => p.Assigned).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                    break;
                case "10":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StartDate).ToList() : data.OrderBy(p => p.StartDate).ToList();
                    break;
                case "11":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Duration).ToList() : data.OrderBy(p => p.Duration).ToList();
                    break;
                case "12":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RequiredDate).ToList() : data.OrderBy(p => p.RequiredDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        public PartialViewResult AvailableWorkOrders(int PersonnelId, DateTime? Date)
        {
            LaborSchedulingVM LVM = new LaborSchedulingVM();
            LaborSchedulingModel LC = new LaborSchedulingModel();
            LC.PersonnelId = PersonnelId;
            LC.Date = Date;
            LVM.LaborSchedulingModel = LC;
            #region V2-984
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            AvailableSchedulingModel availableSchedulingModel = new AvailableSchedulingModel();
            AllLookUps = commonWrapper.GetAllLookUpList();
            //status
            var StatusList = commonWrapper.GetListFromConstVals(LookupListConstants.WO_Status);
            if (StatusList != null)
            {
                availableSchedulingModel.StatusList = StatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            //priority
            var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority);
            if (Priority != null)
            {
                var tmpPriority = Priority.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                availableSchedulingModel.PriorityList = tmpPriority.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).Distinct().ToList();
            }
            //Downrequired
            var DownRequiredStatusList = UtilityFunction.DownRequiredStatusTypesWithBoolValue();
            if (DownRequiredStatusList != null)
            {
                availableSchedulingModel.DownRequiredInactiveFlagList = DownRequiredStatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            //assigned
            var PersonnelLookUplist = GetList_PersonnelV2();
            if (PersonnelLookUplist != null)
            {
                availableSchedulingModel.PersonnelIdList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            //type
            var Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
            if (Type != null)
            {
                var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                availableSchedulingModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
            }
            LVM.AvailableSchedulingModel = availableSchedulingModel;
            #endregion
            LocalizeControls(LVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/LaborSchedulingDaily/AvailableWorkOrders.cshtml", LVM);
        }
        [HttpPost]
        public JsonResult AddAvailableWorkOrder(string[] WorkOrderIds, int PersonnelId, string Date, string flag)
        {
            DateTime dt = DateTime.ParseExact(Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            // Date = dt.ToString("MM/dd/yyyy");
            LaborScheduleWrapper pWrapper = new LaborScheduleWrapper(userData);
            List<AWOAddResult> LabourAvailableList = pWrapper.PopulateLaborAvailableAfterAdd(WorkOrderIds, Convert.ToString(PersonnelId), dt, flag);
            return Json(LabourAvailableList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult UpdateWoHours(long WorkOrderSchedId, decimal hours, int PersonnelId, string Date)
        {
            decimal totalHours = 0;
            LaborScheduleWrapper laborScheduleWrapper = new LaborScheduleWrapper(userData);
            DateTime dt = DateTime.ParseExact(Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            Date = dt.ToString("MM/dd/yyyy");

            var updateErrorResult = laborScheduleWrapper.UpdateSchedulingRecords(WorkOrderSchedId, hours);

            if (string.IsNullOrEmpty(updateErrorResult))
            {
                var LabourSchedulingList = laborScheduleWrapper.PopulateLaborScheduling(Convert.ToString(PersonnelId), Date, "1");
                if (LabourSchedulingList != null && LabourSchedulingList.Count > 0)
                {
                    totalHours = LabourSchedulingList.Sum(x => x.Hours).Value;
                }
                return Json(new { Result = JsonReturnEnum.success.ToString(), totalHours = totalHours }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}