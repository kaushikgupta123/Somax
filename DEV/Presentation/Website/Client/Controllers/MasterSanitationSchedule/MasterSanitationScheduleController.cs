using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.MasterSanitationSchedule;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.MasterSanitationSchedule;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.MasterSanitationSchedule
{
    public class MasterSanitationScheduleController : SomaxBaseController
    {
        #region Sanitation
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.Master_Schedule_Search)]
        public ActionResult Index()
        {
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM();
            objMasterScheduleVM.security = this.userData.Security;
            LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            return View(objMasterScheduleVM);
        }
        private List<MasterSanitationScheduleModel> GetSanitSearchResult(List<MasterSanitationScheduleModel> SanitList, string Description, string ChargeToClientLookupId, string Frequency, string Assigned, string Shift, string ScheduledDuration, DateTime? NextDue, string InactiveFlag, string SearchText)
        {
            if (SanitList != null)
            {
                SearchText = SearchText.ToUpper();
                DateTime dateTime;
                DateTime.TryParseExact(SearchText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                SanitList = SanitList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(SearchText))
                                                 || (!string.IsNullOrWhiteSpace(x.ChargeToClientLookupId.Trim()) && x.ChargeToClientLookupId.Trim().ToUpper().Contains(SearchText))
                                                 || (!string.IsNullOrWhiteSpace(Convert.ToString(x.Frequency).Trim()) && Convert.ToString(x.Frequency).Trim().ToUpper().Contains(SearchText))
                                                 || (!string.IsNullOrWhiteSpace(Convert.ToString(x.Assigned).Trim()) && Convert.ToString(x.Assigned).Trim().ToUpper().Contains(SearchText))
                                                 || (!string.IsNullOrWhiteSpace(x.Shift.Trim()) && x.Shift.Trim().ToUpper().Contains(SearchText))
                                                 || (!string.IsNullOrWhiteSpace(Convert.ToString(x.ScheduledDuration).Trim()) && Convert.ToString(x.ScheduledDuration).Trim().ToUpper().Contains(SearchText))
                                                || (x.NextDue != null && x.NextDue.Value != default(DateTime) && x.NextDue.Value.Date.Equals(dateTime))
                                                ).ToList();

                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    SanitList = SanitList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeToClientLookupId))
                {
                    Description = Description.ToUpper();
                    SanitList = SanitList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeToClientLookupId) && x.ChargeToClientLookupId.ToUpper().Contains(ChargeToClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Frequency))
                {
                    Frequency = Frequency.ToUpper();
                    SanitList = SanitList.Where(x => (!string.IsNullOrWhiteSpace(Convert.ToString(x.Frequency)) && Convert.ToString(x.Frequency).ToUpper().Contains(Frequency))).ToList();
                }
                if (!string.IsNullOrEmpty(Assigned))
                {
                    Assigned = Assigned.ToUpper();
                    SanitList = SanitList.Where(x => (!string.IsNullOrWhiteSpace(Convert.ToString(x.Assigned)) && Convert.ToString(x.Assigned).ToUpper().Contains(Assigned))).ToList();
                }
                if (!string.IsNullOrEmpty(Shift))
                {
                    Shift = Shift.ToUpper();
                    SanitList = SanitList.Where(x => (!string.IsNullOrWhiteSpace(x.Shift) && x.Shift.ToUpper().Contains(Shift))).ToList();
                }
                if (!string.IsNullOrEmpty(ScheduledDuration))
                {
                    ScheduledDuration = ScheduledDuration.ToUpper();
                    SanitList = SanitList.Where(x => (!string.IsNullOrWhiteSpace(Convert.ToString(x.ScheduledDuration)) && Convert.ToString(x.ScheduledDuration).ToUpper().Contains(ScheduledDuration))).ToList();
                }
                if (NextDue != null)
                {
                    SanitList = SanitList.Where(x => (x.NextDue != null && x.NextDue.Value.Date.Equals(NextDue.Value.Date))).ToList();
                }
                if (InactiveFlag != null && InactiveFlag != "")
                {
                    var v = false;
                    if (InactiveFlag.Equals("true"))
                    {
                        v = false;
                    }
                    else
                    {
                        v = true;
                    }
                    SanitList = SanitList.Where(x => x.InactiveFlag == v).ToList();
                }
            }
            return SanitList;
        }
        [HttpPost]
        public string GetMasterSanitGridData(int? draw, int? start, int? length, string Description = "", string ChargeToClientLookupId = "", string Frequency = "", string Assigned = "", string Shift = "", string ScheduledDuration = "", DateTime? NextDue = null, string InactiveFlag = null, string SearchText = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);

            var mslist = msWrapper.PopulateSanitShedule();
            if (mslist != null)
            {
                mslist = this.GetSanitSearchResult(mslist, Description, ChargeToClientLookupId, Frequency, Assigned, Shift, ScheduledDuration, NextDue, InactiveFlag, SearchText);
                mslist = GetSanitGridSortByColumnWithOrder(colname[0], orderDir, mslist);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = mslist.Count();
            totalRecords = mslist.Count();

            int initialPage = start.Value;

            var filteredResult = mslist
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<MasterSanitationScheduleModel> GetSanitGridSortByColumnWithOrder(string order, string orderDir, List<MasterSanitationScheduleModel> data)
        {
            List<MasterSanitationScheduleModel> lst = new List<MasterSanitationScheduleModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToClientLookupId).ToList() : data.OrderBy(p => p.ChargeToClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Frequency).ToList() : data.OrderBy(p => p.Frequency).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Assigned).ToList() : data.OrderBy(p => p.Assigned).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Shift).ToList() : data.OrderBy(p => p.Shift).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScheduledDuration).ToList() : data.OrderBy(p => p.ScheduledDuration).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NextDue).ToList() : data.OrderBy(p => p.NextDue).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.InactiveFlag).ToList() : data.OrderBy(p => p.InactiveFlag).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
            }
            return lst;
        }

        [HttpGet]
        public string GetSanitMasterPrintData(string colname, string coldir, string Description = "", string ChargeToClientLookupId = "", string Frequency = "", string Assigned = "", string Shift = "", string ScheduledDuration = "", DateTime? NextDue = null, string InactiveFlag = null, string SearchText = "")
        {
            List<MasterSanitationPrintModel> msPrintList = new List<MasterSanitationPrintModel>();
            MasterSanitationPrintModel objMasterSanitationPrintModel;
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            var sanitMastList = msWrapper.PopulateSanitShedule();
            if (sanitMastList != null)
            {
                sanitMastList = GetSanitGridSortByColumnWithOrder(colname, coldir, sanitMastList);
                sanitMastList = this.GetSanitSearchResult(sanitMastList, Description, ChargeToClientLookupId, Frequency, Assigned, Shift, ScheduledDuration, NextDue, InactiveFlag, SearchText);
                foreach (var p in sanitMastList)
                {
                    objMasterSanitationPrintModel = new MasterSanitationPrintModel();
                    objMasterSanitationPrintModel.Description = p.Description;
                    objMasterSanitationPrintModel.ChargeToClientLookupId = p.ChargeToClientLookupId;
                    objMasterSanitationPrintModel.Frequency = p.Frequency ?? 0;
                    objMasterSanitationPrintModel.Assigned = p.Assigned;
                    objMasterSanitationPrintModel.Shift = p.Shift;
                    objMasterSanitationPrintModel.ScheduledDuration = p.ScheduledDuration ?? 0;
                    objMasterSanitationPrintModel.NextDue = p.NextDue;
                    objMasterSanitationPrintModel.InactiveFlag = p.InactiveFlag;
                    msPrintList.Add(objMasterSanitationPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = msPrintList }, JsonSerializerDateSettings);
        }

        #endregion Search
        #region Add-Edit
        public PartialViewResult AddMSSchedule()
        {
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM();
            MasterSanitationScheduleModel objMasterSanitationScheduleModel = new MasterSanitationScheduleModel();
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            var ScheduleTypeList = UtilityFunction.GetSanitationScheduleType();
            if (ScheduleTypeList != null)
            {
                objMasterSanitationScheduleModel.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> onDemandList = new List<DataContracts.LookupList>();
            var AllLookUpList = msWrapper.GetAllLookUpList();
            if (AllLookUpList != null)
            {
                Shift = AllLookUpList.Where(x => x.ListName == LookupListConstants.SANIT_SHIFT).ToList(); //GetShiftList();
                onDemandList = AllLookUpList.Where(x => x.ListName == LookupListConstants.SANIT_ON_DEMAND).ToList();
            }
            if (Shift != null)
            {
                objMasterSanitationScheduleModel.ShiftList = Shift.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
            }
            if (onDemandList != null)
            {
                objMasterSanitationScheduleModel.OnDemandGroupList = onDemandList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).ToList();
            }
            // V2-478 
            // Security Items to include: Sanitation-WB
            // Security Properties      : ItemAccess
            var PersonnelLookUplist = Get_PersonnelList(SecurityConstants.Sanitation_WB,"ItemAccess");
            if (PersonnelLookUplist != null)
            {
                objMasterSanitationScheduleModel.PersonnelList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            var daysOfWeek = UtilityFunction.DaysOfWeekList();
            if (daysOfWeek != null)
            {
                objMasterSanitationScheduleModel.DaysOfWeekList = daysOfWeek.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            objMasterSanitationScheduleModel.PlantLocation = userData.Site.PlantLocation;
            objMasterSanitationScheduleModel.NextDue = DateTime.Now;
            objMasterScheduleVM.MasterSanitModel = objMasterSanitationScheduleModel;
            //v2-609
            objMasterScheduleVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddEditMSSchedule", objMasterScheduleVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMSSchedule(MasterScheduleVM MasterScheduleVM, string Command)
        {
            SanitationMaster objSanitationMaster = new SanitationMaster();
            if (ModelState.IsValid)
            {
                MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
                string Mode = string.Empty;
                if (MasterScheduleVM.MasterSanitModel.SanitationMasterId == 0)
                {
                    objSanitationMaster = msWrapper.AddMSSchedule(MasterScheduleVM.MasterSanitModel);
                    Mode = "add";
                }
                else
                {
                    objSanitationMaster = msWrapper.EditMSSchedule(MasterScheduleVM.MasterSanitModel);
                }
                if (objSanitationMaster.ErrorMessages != null && objSanitationMaster.ErrorMessages.Count > 0)
                {
                    return Json(objSanitationMaster.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, SanitationMasterId = objSanitationMaster.SanitationMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public PartialViewResult EditMSSchedule(long SanitationMasterId)
        {
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM();
            MasterSanitationScheduleModel objMasterSanitationScheduleModel = new MasterSanitationScheduleModel();
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            List<Models.DataModel> obj = new List<DataModel>();
            objMasterSanitationScheduleModel = msWrapper.MasterSanitDetails(SanitationMasterId, obj);
            var ScheduleTypeList = UtilityFunction.GetSanitationScheduleType();
            if (ScheduleTypeList != null)
            {
                objMasterSanitationScheduleModel.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> onDemandList = new List<DataContracts.LookupList>();
            var AllLookUpList = msWrapper.GetAllLookUpList();
            if (AllLookUpList != null)
            {
                Shift = AllLookUpList.Where(x => x.ListName == LookupListConstants.SANIT_SHIFT).ToList(); //GetShiftList();
                onDemandList = AllLookUpList.Where(x => x.ListName == LookupListConstants.SANIT_ON_DEMAND).ToList();
            }
            if (Shift != null)
            {
                objMasterSanitationScheduleModel.ShiftList = Shift.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
            }
            if (onDemandList != null)
            {
                objMasterSanitationScheduleModel.OnDemandGroupList = onDemandList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).ToList();
            }
            // V2-478 
            // Security Items to include: Sanitation-WB
            // Security Properties      : ItemAccess
            var PersonnelLookUplist = Get_PersonnelList(SecurityConstants.Sanitation_WB,"ItemAccess");
            if (PersonnelLookUplist != null)
            {
                objMasterSanitationScheduleModel.PersonnelList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            var daysOfWeek = UtilityFunction.DaysOfWeekList();
            if (daysOfWeek != null)
            {
                objMasterSanitationScheduleModel.DaysOfWeekList = daysOfWeek.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            objMasterSanitationScheduleModel.PlantLocation = userData.Site.PlantLocation;
            objMasterSanitationScheduleModel.ChargeToDescription = objMasterSanitationScheduleModel.ChargeToClientLookupId;
            objMasterScheduleVM.MasterSanitModel = objMasterSanitationScheduleModel;
            //v2-609
            objMasterScheduleVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddEditMSSchedule", objMasterScheduleVM);
        }
        #endregion Add-Edit

        #region Options
        [HttpPost]
        public JsonResult DeleteMSSchedule(long MSScheduleid)
        {
            string result = string.Empty;
            List<String> errorList = new List<string>();
            MasterSanitationScheduleModel objMasterSanitationScheduleModel = new MasterSanitationScheduleModel();
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            errorList = msWrapper.DeleteMSSchedule(MSScheduleid);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(errorList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Details
        [HttpPost]
        public PartialViewResult GetMasterSanitDetails(long SanitationMasterId)
        {
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM();
            MasterSanitationScheduleModel objMasterSanitScheduleModel = new MasterSanitationScheduleModel();
            Task attTask;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            attTask = Task.Factory.StartNew(() => objMasterScheduleVM.attachmentCount = objCommonWrapper.AttachmentCount(SanitationMasterId, AttachmentTableConstant.SanitationMaster, userData.Security.Sanitation.Edit));
            // V2-478 
            // Security Items to include: Sanitation-WB
            // Security Properties      : ItemAccess
            var plist = Get_PersonnelList(SecurityConstants.Sanitation_WB, "ItemAccess");
            //var plist = Get_PersonnelList();
            objMasterSanitScheduleModel = msWrapper.MasterSanitDetails(SanitationMasterId, plist);

            #region Inactive Days
            if (!string.IsNullOrEmpty(objMasterSanitScheduleModel.ExclusionDays) && objMasterSanitScheduleModel.ExclusionDays[0] == '1')
            {
                objMasterSanitScheduleModel.Sunday = true;
            }
            if (!string.IsNullOrEmpty(objMasterSanitScheduleModel.ExclusionDays) && objMasterSanitScheduleModel.ExclusionDays[1] == '1')
            {
                objMasterSanitScheduleModel.Monday = true;
            }
            if (!string.IsNullOrEmpty(objMasterSanitScheduleModel.ExclusionDays) && objMasterSanitScheduleModel.ExclusionDays[2] == '1')
            {
                objMasterSanitScheduleModel.Tuesday = true;
            }
            if (!string.IsNullOrEmpty(objMasterSanitScheduleModel.ExclusionDays) && objMasterSanitScheduleModel.ExclusionDays[3] == '1')
            {
                objMasterSanitScheduleModel.Wednesday = true;
            }
            if (!string.IsNullOrEmpty(objMasterSanitScheduleModel.ExclusionDays) && objMasterSanitScheduleModel.ExclusionDays[4] == '1')
            {
                objMasterSanitScheduleModel.Thursday = true;
            }
            else if (!string.IsNullOrEmpty(objMasterSanitScheduleModel.ExclusionDays) && objMasterSanitScheduleModel.ExclusionDays[5] == '1')
            {
                objMasterSanitScheduleModel.Friday = true;
            }
            if (!string.IsNullOrEmpty(objMasterSanitScheduleModel.ExclusionDays) && objMasterSanitScheduleModel.ExclusionDays[6] == '1')
            {
                objMasterSanitScheduleModel.Saturday = true;
            }
            #endregion

            objMasterScheduleVM.MasterSanitModel = objMasterSanitScheduleModel;
            objMasterScheduleVM.security = this.userData.Security;
            attTask.Wait();
            LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/MasterSanitationSchedule/_MasterScheduleDetail.cshtml", objMasterScheduleVM);
        }
        #endregion Details

        #region Task
        [HttpPost]
        public string PopulateMastSanitTask(int? draw, int? start, int? length, long sanitationMasterId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            List<MasterSanitTaskModel> TaskList = msWrapper.PopulateTask(sanitationMasterId);

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
            var tskSecurity = this.userData.Security.Sanitation.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, tskSecurity = tskSecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<MasterSanitTaskModel> GetAllTaskOrderSortByColumnWithOrder(string order, string orderDir, List<MasterSanitTaskModel> data)
        {
            List<MasterSanitTaskModel> lst = new List<MasterSanitTaskModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OrderNumber).ToList() : data.OrderBy(p => p.OrderNumber).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PerformTime).ToList() : data.OrderBy(p => p.PerformTime).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.InactiveFlag).ToList() : data.OrderBy(p => p.InactiveFlag).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OrderNumber).ToList() : data.OrderBy(p => p.OrderNumber).ToList();
                        break;
                }
            }
            return lst;
        }

        public PartialViewResult AddOrEditTasks(long sanitationMasterId, string msDescription, long sanitationMasterTaskId = 0)
        {
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM();
            MasterSanitTaskModel masterSanitTaskModel = new MasterSanitTaskModel();

            if (sanitationMasterTaskId == 0)
            {
                masterSanitTaskModel.TaskId = msWrapper.CreateTaskNumber(sanitationMasterId);
            }
            else
            {
                masterSanitTaskModel = msWrapper.RetrieveTaskForEdit(sanitationMasterTaskId);
            }
            masterSanitTaskModel.SanitationMasterId = sanitationMasterId;

            objMasterScheduleVM.MasterSanTaskModel = masterSanitTaskModel;

            MasterSanitationScheduleModel masterSanitationScheduleModel = new MasterSanitationScheduleModel();
            masterSanitationScheduleModel.Description = msDescription;

            objMasterScheduleVM.MasterSanitModel = masterSanitationScheduleModel;
            LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/MasterSanitationSchedule/_AddEditTask.cshtml", objMasterScheduleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTask(MasterScheduleVM objMasterScheduleVM)
        {
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                if (objMasterScheduleVM.MasterSanTaskModel.SanitationMasterTaskId != 0)
                {
                    Mode = "update";
                    errorList = msWrapper.UpdateMastSanitTask(objMasterScheduleVM.MasterSanTaskModel);
                }
                else
                {
                    Mode = "add";
                    errorList = msWrapper.CreateMastSanitTask(objMasterScheduleVM.MasterSanTaskModel);
                }
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "success", sjid = objMasterScheduleVM.MasterSanTaskModel.SanitationMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(new { Result = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteTasks(long sanitationMasterTaskId)
        {
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            var deleteResult = msWrapper.DeleteMastSanitTask(sanitationMasterTaskId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Task
        #region Notes
        [HttpPost]
        public string PopulateNotes(int? draw, int? start, int? length, long SanitationMasterId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper cWrapper = new CommonWrapper(userData);
            var Notes = cWrapper.PopulateNotes(SanitationMasterId, "SanitationMaster");
            Notes = GetAllNotesSortByColumnWithOrder(order, orderDir, Notes);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Notes.Count();
            totalRecords = Notes.Count();
            int initialPage = start.Value;
            var filteredResult = Notes
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<NotesModel> GetAllNotesSortByColumnWithOrder(string order, string orderDir, List<NotesModel> data)
        {
            List<NotesModel> lst = new List<NotesModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ModifiedDate).ToList() : data.OrderBy(p => p.ModifiedDate).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                        break;
                }
            }
            return lst;
        }

        [HttpGet]
        public ActionResult AddNotes(long sanitationMasterId, string msDescription, string subject)
        {
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM()
            {
                MasterSanNotesModel = new MasterSanitNotesModel
                {
                    SanitationMasterId = sanitationMasterId,
                    Description = msDescription
                },
                MasterSanitModel = new MasterSanitationScheduleModel
                {
                    Description = msDescription
                }
            };
            LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/MasterSanitationSchedule/_AddEditNotes.cshtml", objMasterScheduleVM);
        }

        public ActionResult EditNote(long sanitationMasterId, long notesId, string msDescription)
        {
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM();
            NotesModel notesModel = new NotesModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            notesModel = commonWrapper.EditNotes(sanitationMasterId, notesId);
            notesModel.NotesId = notesId;
            notesModel.SanitationMasterId = sanitationMasterId;
            notesModel.ClientLookupId = msDescription;
            objMasterScheduleVM.MasterSanNotesModel = new MasterSanitNotesModel();
            objMasterScheduleVM.MasterSanNotesModel.NotesId = notesId;
            objMasterScheduleVM.MasterSanNotesModel.SanitationMasterId = sanitationMasterId;
            objMasterScheduleVM.MasterSanNotesModel.Description = msDescription;
            objMasterScheduleVM.MasterSanNotesModel.Subject = notesModel.Subject;
            objMasterScheduleVM.MasterSanNotesModel.UpdateIndex = notesModel.updatedindex;
            objMasterScheduleVM.MasterSanNotesModel.Content = notesModel.Content;
            MasterSanitationScheduleModel objMastSanitsch = new MasterSanitationScheduleModel();
            objMastSanitsch.SanitationMasterId = sanitationMasterId;
            objMasterScheduleVM.MasterSanitModel = objMastSanitsch;
            LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/MasterSanitationSchedule/_AddEditNotes.cshtml", objMasterScheduleVM);
        }

        [HttpPost]
        public ActionResult SaveNotes(MasterScheduleVM objMasterScheduleVM)
        {
            if (ModelState.IsValid)
            {
                CommonWrapper cWrapper = new CommonWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                NotesModel notesModel = new NotesModel();
                notesModel.Subject = objMasterScheduleVM.MasterSanNotesModel.Subject;
                notesModel.Content = objMasterScheduleVM.MasterSanNotesModel.Content;
                notesModel.Type = objMasterScheduleVM.MasterSanNotesModel.Type;
                notesModel.ObjectId = objMasterScheduleVM.MasterSanNotesModel.SanitationMasterId;
                notesModel.updatedindex = objMasterScheduleVM.MasterSanNotesModel.UpdateIndex;
                notesModel.NotesId = objMasterScheduleVM.MasterSanNotesModel.NotesId;
                if (objMasterScheduleVM.MasterSanNotesModel.NotesId == 0)
                {
                    Mode = "add";
                }
                errorList = cWrapper.AddNotes(notesModel, "SanitationMaster");
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SanitationMasterId = objMasterScheduleVM.MasterSanNotesModel.SanitationMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteNotes(long notesId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            if (commonWrapper.DeleteNotes(notesId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Attachment
        [HttpPost]
        public string PopulateMSAttachments(int? draw, int? start, int? length, long SanitationMasterId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var Attachments = objCommonWrapper.PopulateAttachments(SanitationMasterId, "SanitationMaster", userData.Security.Sanitation.Edit);
            if (Attachments != null)
            {
                Attachments = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, Attachments);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Attachments.Count();
            totalRecords = Attachments.Count();
            int initialPage = start.Value;
            var filteredResult = Attachments
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }

        [HttpGet]
        public PartialViewResult AddMsAttachments(long sanitationMasterId, string description)
        {
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM();
            AttachmentModel objAttachment = new AttachmentModel();
            MasterSanitationScheduleModel mastSanitModel = new MasterSanitationScheduleModel();
            mastSanitModel.SanitationMasterId = sanitationMasterId;
            mastSanitModel.Description = description;
            objAttachment.SanitationMasterId = sanitationMasterId;
            objAttachment.ClientLookupId = description;
            objMasterScheduleVM.MasterSanitModel = mastSanitModel;
            objMasterScheduleVM.MasterSanAttachmentModel = new MasterSanitAttachmentModel();
            objMasterScheduleVM.MasterSanAttachmentModel.SanitationMasterId = sanitationMasterId;
            objMasterScheduleVM.MasterSanAttachmentModel.ClientLookupId = description;
            LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/MasterSanitationSchedule/_AddAttachment.cshtml", objMasterScheduleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMsAttachments()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                Stream stream = Request.Files[0].InputStream;
                Client.Models.AttachmentModel attachmentModel = new Client.Models.AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);
                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["MasterSanAttachmentModel.SanitationMasterId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["MasterSanAttachmentModel.Subject"]) ? "No Subject" : Request.Form["MasterSanAttachmentModel.Subject"];
                attachmentModel.TableName = "SanitationMaster";
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.Sanitation.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.Sanitation.Edit);
                }

                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), sanitId = Convert.ToInt64(Request.Form["MasterSanAttachmentModel.SanitationMasterId"]) }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var fileTypeMessage = UtilityFunction.GetMessageFromResource("spnInvalidFileType", LocalizeResourceSetConstants.Global);
                    return Json(fileTypeMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DownloadAttachment(long _fileinfoId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            DataContracts.Attachment fileInfo = new DataContracts.Attachment();
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                fileInfo = objCommonWrapper.DownloadAttachmentOnPremise(_fileinfoId);
                string contentType = MimeMapping.GetMimeMapping(fileInfo.AttachmentURL);
                return File(fileInfo.AttachmentURL, contentType, fileInfo.FileName + '.' + fileInfo.FileType);
            }
            else
            {
                fileInfo = objCommonWrapper.DownloadAttachment(_fileinfoId);
                return Redirect(fileInfo.AttachmentURL);
            }

        }
        [HttpPost]
        public ActionResult DeleteMsAttachments(long _fileAttachmentId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool deleteResult = false;
            string Message = string.Empty;
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                deleteResult = objCommonWrapper.DeleteAttachmentOnPremise(_fileAttachmentId, ref Message);
            }
            else
            {
                deleteResult = objCommonWrapper.DeleteAttachment(_fileAttachmentId);

            }
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString(), Message = Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Tools
        public string PopulateMastSanitTools(int? draw, int? start, int? length, long sanitationMasterId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            List<MasterSanitationPlanningModel> Tools = msWrapper.PopulateTool(sanitationMasterId);

            if (Tools != null)
            {
                Tools = this.GetAllToolsOrderSortByColumnWithOrder(order, orderDir, Tools);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Tools.Count();
            totalRecords = Tools.Count();
            int initialPage = start.Value;
            var filteredResult = Tools
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var toolSecurity = this.userData.Security.Sanitation.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, toolSecurity = toolSecurity });
        }
        private List<MasterSanitationPlanningModel> GetAllToolsOrderSortByColumnWithOrder(string order, string orderDir, List<MasterSanitationPlanningModel> data)
        {
            List<MasterSanitationPlanningModel> lst = new List<MasterSanitationPlanningModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CategoryValue).ToList() : data.OrderBy(p => p.CategoryValue).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Instructions).ToList() : data.OrderBy(p => p.Instructions).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CategoryValue).ToList() : data.OrderBy(p => p.CategoryValue).ToList();
                        break;
                }
            }
            return lst;
        }

        public PartialViewResult AddorEditTool(long sanitationMasterId, string msDescription, long SanitationPlanningId = 0)
        {
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM();
            MasterSanitationPlanningModel objMasterSanitationPlanningModel = new MasterSanitationPlanningModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);

            List<DataContracts.LookupList> ToolLookUplist = commonWrapper.GetAllLookUpList().Where(x => x.ListName == SanitationJobConstant.SanitationPlanning_Tool).ToList();
            if (ToolLookUplist != null)
            {
                objMasterSanitationPlanningModel.CategoryIdList = ToolLookUplist != null ? ToolLookUplist.Select(x => new SelectListItem { Text = x.ListValue.ToString() + " | " + x.Description, Value = x.ListValue.ToString() }) : new SelectList(new[] { "" });
            }

            if (SanitationPlanningId == 0) //add
            {
                objMasterSanitationPlanningModel.SanitationMasterId = sanitationMasterId;
                objMasterSanitationPlanningModel.MasterSanitationDescription = msDescription;
            }
            else //edit
            {
                objMasterSanitationPlanningModel = msWrapper.GetTool(sanitationMasterId, SanitationPlanningId);
                objMasterSanitationPlanningModel.MasterSanitationDescription = msDescription;

                objMasterSanitationPlanningModel.CategoryIdList = ToolLookUplist != null ? ToolLookUplist.Select(x => new SelectListItem { Text = x.ListValue.ToString() + " | " + x.Description, Value = x.ListValue.ToString() }) : new SelectList(new[] { "" });
            }
            objMasterScheduleVM.MasterSanPlanningModel = objMasterSanitationPlanningModel;
            LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/MasterSanitationSchedule/_AddEditTools.cshtml", objMasterScheduleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditTool(MasterScheduleVM objMasterScheduleVM)
        {
            if (ModelState.IsValid)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
                string Mode = string.Empty;
                List<string> errorMessage = new List<string>();
                List<DataContracts.LookupList> ToolLookUplist = commonWrapper.GetAllLookUpList().Where(x => x.ListName == SanitationJobConstant.SanitationPlanning_Tool).ToList();
                if (ToolLookUplist != null)
                {
                    if (!string.IsNullOrEmpty(objMasterScheduleVM.MasterSanPlanningModel.CategoryValue))
                    {
                        string Description = ToolLookUplist.Where(x => x.ListValue == objMasterScheduleVM.MasterSanPlanningModel.CategoryValue).Select(x => x.Description).FirstOrDefault();
                        objMasterScheduleVM.MasterSanPlanningModel.Description = Description;
                    }
                }
                if (objMasterScheduleVM.MasterSanPlanningModel.SanitationPlanningId == 0)
                {
                    errorMessage = msWrapper.AddTool(objMasterScheduleVM.MasterSanPlanningModel);
                    Mode = "add";
                }
                else
                {
                    errorMessage = msWrapper.EditTool(objMasterScheduleVM.MasterSanPlanningModel);
                }
                if (errorMessage != null && errorMessage.Count > 0)
                {
                    return Json(errorMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SanitationMasterId = objMasterScheduleVM.MasterSanPlanningModel.SanitationMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteTools(long _SanitationPlanningId)
        {
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            var deleteResult = msWrapper.DeleteTools(_SanitationPlanningId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Chemicals
        public string PopulateMastSanitChemicals(int? draw, int? start, int? length, long sanitationMasterId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            List<MasterSanitationPlanningModel> Chemicals = msWrapper.PopulateChemical(sanitationMasterId);

            if (Chemicals != null)
            {
                Chemicals = this.GetAllToolsOrderSortByColumnWithOrder(order, orderDir, Chemicals);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Chemicals.Count();
            totalRecords = Chemicals.Count();
            int initialPage = start.Value;
            var filteredResult = Chemicals
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var chemSecurity = this.userData.Security.Sanitation.Edit; ;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, chemSecurity = chemSecurity });
        }
        public PartialViewResult AddOrEditChemical(long sanitationMasterId, string msDescription, long? SanitationPlanningId)
        {
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM();
            MasterSanitationPlanningModel objMasterSanitationPlanningModel = new MasterSanitationPlanningModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            if (SanitationPlanningId != null)
            {
                objMasterSanitationPlanningModel = msWrapper.ChemicalRetriveById(SanitationPlanningId ?? 0);
            }
            List<DataContracts.LookupList> ChemicalSuppliesLookUplist = commonWrapper.GetAllLookUpList().Where(x => x.ListName == SanitationJobConstant.SanitationChemicalSupplies_Tool).ToList();
            if (ChemicalSuppliesLookUplist != null)
            {
                objMasterSanitationPlanningModel.CategoryIdList = ChemicalSuppliesLookUplist != null ? ChemicalSuppliesLookUplist.Select(x => new SelectListItem { Text = x.ListValue.ToString() + " | " + x.Description, Value = x.ListValue.ToString() + "|" + x.Description }) : new SelectList(new[] { "" });

            }
            objMasterSanitationPlanningModel.SanitationMasterId = sanitationMasterId;
            objMasterSanitationPlanningModel.MasterSanitationDescription = msDescription;

            objMasterScheduleVM.MasterSanPlanningModel = objMasterSanitationPlanningModel;
            //List<DataContracts.LookupList> ToolLookUplist = commonWrapper.GetAllLookUpList().Where(x => x.ListName == SanitationJobConstant.SanitationPlanning_Tool).ToList();
            LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/MasterSanitationSchedule/_AddEditChemicals.cshtml", objMasterScheduleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditChemicals(MasterScheduleVM objMasterScheduleVM)
        {
            if (ModelState.IsValid)
            {
                MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
                string Mode = string.Empty;
                List<string> errorMessage = new List<string>();
                if (objMasterScheduleVM.MasterSanPlanningModel.SanitationPlanningId == 0)
                {
                    errorMessage = msWrapper.AddChemical(objMasterScheduleVM.MasterSanPlanningModel);
                    Mode = "add";
                }
                else
                {
                    errorMessage = msWrapper.EditChemical(objMasterScheduleVM.MasterSanPlanningModel);
                }
                if (errorMessage != null && errorMessage.Count > 0)
                {
                    return Json(errorMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), SanitationMasterId = objMasterScheduleVM.MasterSanPlanningModel.SanitationMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult DeleteChemicals(long SanitationPlanningId)
        {
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            var deleteResult = msWrapper.DeleteChemical(SanitationPlanningId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #endregion Sanitation

        #region JobGeneration

        public JsonResult GetOnDemandList()
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> onDemandLookUpList = new List<DataContracts.LookupList>();
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                onDemandLookUpList = AllLookUps.Where(x => x.ListName == SanitationJobConstant.SANIT_ON_DEMAND).ToList();
            }
            return Json(onDemandLookUpList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult JobGenerationProcess(MasterScheduleVM objVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsgList = new List<string>();

                string TopMessage = string.Empty;
                int SanitationMasterCount = 0;
                int SanitationJobCount = 0;
                List<string> listOfSanitation = new List<string>();//361074
                MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
                ErrorMsgList = msWrapper.OnProccess(objVM.sanitationJobGenerationModel, ref TopMessage, ref SanitationMasterCount, ref SanitationJobCount, ref listOfSanitation);

                if (ErrorMsgList != null && ErrorMsgList.Count > 0)
                {
                    return Json("ErrorMsgList", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), TopMessage = TopMessage, SanitationMasterCount = SanitationMasterCount, SanitationJobCount = SanitationJobCount, IsPrint = objVM.sanitationJobGenerationModel.IsPrint, listOfSanitation = listOfSanitation }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PrintSaniList(List<string> listOfSanitation)
        {
            if (listOfSanitation.Count > 0)
            {
                using (var ms = new MemoryStream())
                {
                    using (var doc = new iTextSharp.text.Document())
                    {
                        using (var copy = new iTextSharp.text.pdf.PdfSmartCopy(doc, ms))
                        {
                            doc.Open();
                            foreach (var sanitationJobId in listOfSanitation)
                            {
                                var msSinglePDf = new MemoryStream(PrintGetByteStream(Convert.ToInt32(sanitationJobId)));
                                using (var reader = new iTextSharp.text.pdf.PdfReader(msSinglePDf))
                                {
                                    copy.AddDocument(reader);
                                }
                            }
                            doc.Close();
                        }
                    }
                    byte[] pdf = ms.ToArray();
                    string strPdf = System.Convert.ToBase64String(pdf);
                    var returnOjb = new { success = true, pdf = strPdf };
                    var jsonResult = Json(returnOjb, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
            }
            else
            {
                var returnOjb = new { success = false };
                return Json(returnOjb, JsonRequestBehavior.AllowGet);
            }
        }
        [EncryptedActionParameter]
        public Byte[] PrintGetByteStream(long SanitationMasterId)
        {
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM();
            MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
            #region SanitationDetails
            SanitationJobDetailsModel objMasterSanitScheduleModel = new SanitationJobDetailsModel();
            objMasterSanitScheduleModel = msWrapper.RetrieveBy_SanitationJobId(SanitationMasterId);
            objMasterScheduleVM.JobDetailsModel = objMasterSanitScheduleModel;
            #endregion
            #region Tools
            List<DataContracts.SanitationPlanning> ToolsList = new List<DataContracts.SanitationPlanning>();
            Task task1 = Task.Factory.StartNew(() => ToolsList = msWrapper.PopulateToolsPrint(objMasterSanitScheduleModel.SanitationMasterId ?? 0)).ContinueWith(_ => { objMasterScheduleVM.SanitationToolPrint = ToolsList.Where(t => t.SanitationJobId == objMasterSanitScheduleModel.SanitationJobId).ToList(); });
            #endregion
            #region Chemicals
            List<DataContracts.SanitationPlanning> ChemicalsList = new List<DataContracts.SanitationPlanning>();
            Task task2 = Task.Factory.StartNew(() => ToolsList = msWrapper.PopulateChemicalsPrint(objMasterSanitScheduleModel.SanitationMasterId ?? 0)).ContinueWith(_ => { objMasterScheduleVM.PopulateChemicalsPrint = ChemicalsList.Where(t => t.SanitationJobId == objMasterSanitScheduleModel.SanitationJobId).ToList(); });
            #endregion
            #region Tasks
            List<DataContracts.SanitationJobTask> TaskList = new List<DataContracts.SanitationJobTask>();
            Task task3 = Task.Factory.StartNew(() => TaskList = msWrapper.PopulateTaskPrint(SanitationMasterId)).ContinueWith(_ => { objMasterScheduleVM.SanitationTaskPrint = TaskList.Where(t => t.SanitationJobId == objMasterSanitScheduleModel.SanitationJobId).ToList(); });
            #endregion
            #region Labour
            List<DataContracts.Timecard> laborList = new List<DataContracts.Timecard>();
            Task task4 = Task.Factory.StartNew(() => objMasterScheduleVM.laborList = laborList);
            #endregion
            Task.WaitAll(task1, task2, task3, task4);
            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                   "--header-spacing \"3\" " +
                                   "--footer-html \"{1}\" " +
                                   "--footer-spacing \"8\" " +
                                   "--footer-font-size \"10\" " +
                                   "--header-font-size \"10\" ",
                                   Url.Action("Header", "MasterSanitationSchedule", new { id = userData.LoginAuditing.SessionId, SanitationMasterId = SanitationMasterId }, Request.Url.Scheme),
                                   Url.Action("Footer", "MasterSanitationSchedule", new { id = userData.LoginAuditing.SessionId }, Request.Url.Scheme));

            LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            var mailpdft = new ViewAsPdf("_MasterSanitationPrintTemplate", objMasterScheduleVM)
            {
                PageMargins = new Margins(43, 12, 22, 12),// it’s in millimeters
                CustomSwitches = customSwitches
            };
            Byte[] PdfData = mailpdft.BuildPdf(ControllerContext);
            return PdfData;
        }

        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Header(string id, long SanitationMasterId)
        {
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM();
            if (CheckLoginSession(id))
            {
                MasterSanitationScheduleWrapper msWrapper = new MasterSanitationScheduleWrapper(userData);
                #region SanitationDetails
                SanitationJobDetailsModel objMasterSanitScheduleModel = new SanitationJobDetailsModel();
                objMasterSanitScheduleModel = msWrapper.RetrieveBy_SanitationJobId(SanitationMasterId);
                objMasterScheduleVM.JobDetailsModel = objMasterSanitScheduleModel;
                #endregion
            }

            LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            return View("_PrintHeader", objMasterScheduleVM);
        }

        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Footer(string id)
        {
            MasterScheduleVM objMasterScheduleVM = new MasterScheduleVM();
            if (CheckLoginSession(id))
            {
                LocalizeControls(objMasterScheduleVM, LocalizeResourceSetConstants.SanitationDetails);
            }
            return View("_PrintFooter", objMasterScheduleVM);
        }
        #endregion JobGeneration
        #region V2-1071 DevExpressPrint
        //[HttpPost]
        public JsonResult SetPrintSanitationJobListFromIndex(List<long> listOfSanitation)
        {
            Session["PrintMSJList"] = listOfSanitation;
            return Json(new { success = true },JsonRequestBehavior.AllowGet);
        }
        [EncryptedActionParameter]
        public ActionResult GenerateSanitationJobPrint()
        {
            List<long> SanitationJobIds = new List<long>();
            if (Session["PrintMSJList"] != null)
            {
                SanitationJobIds = (List<long>)Session["PrintMSJList"];
            }
            SanitationJobWrapper sjWrapper = new SanitationJobWrapper(userData);
            var objPrintModelList = sjWrapper.PrintDevExpressFromIndex(SanitationJobIds);
            return View("MSJDevExpressPrint", objPrintModelList);
        }
        #endregion
    }
}