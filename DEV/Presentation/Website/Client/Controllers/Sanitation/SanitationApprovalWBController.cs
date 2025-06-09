using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Common;
using Client.Models.Sanitation;
using Client.Models.Work_Order;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.Sanitation
{
    public class SanitationApprovalWBController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.SanitationJob_ApprovalWorkbench)]
        public ActionResult Index()
        {
            SanitationVM objSanitVM = new SanitationVM();
            SanitationJobModel sanModel = new SanitationJobModel();
            SanitationJobWrapper sanWrapper = new SanitationJobWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> ShiftLookUpList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> DenyLookUpList = new List<DataContracts.LookupList>();

            var StatusList = UtilityFunction.WorkBenchStatusList();
            if (StatusList != null)
            {
                sanModel.WbStatusList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            var CreateDatesList = UtilityFunction.WorkBenchCreateDatesList();
            if (CreateDatesList != null)
            {
                sanModel.CreateDatesList = CreateDatesList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                ShiftLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (ShiftLookUpList != null)
                {
                    sanModel.ShiftList = ShiftLookUpList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }

                DenyLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_REASON_DENY).ToList();
                if (DenyLookUpList != null)
                {
                    sanModel.DenyReasonList = DenyLookUpList.Select(x => new SelectListItem { Text = x.ListValue, Value = x.ListValue.ToString() });
                }

            }
            var PersonnelLookUplist = GetList_Personnel();
            if (PersonnelLookUplist != null)
            {
                sanModel.WorkAssignedLookUpList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }

            objSanitVM.sanitationJobModel = sanModel;
            objSanitVM.security= this.userData.Security;
            LocalizeControls(objSanitVM, LocalizeResourceSetConstants.SanitationDetails);
            return View(objSanitVM);
        }

        public string GetSanitationAppWBGrid(int? draw, int? start, int? length, string status, string Createdates, string JobId = "", string description = "", string chargeto = "", string Chargetoname = "", long workassigned = 0,
                                             string shift = "", DateTime? scheduledate = null, decimal scheduleduration = 0, DateTime? createdate = null, string createdby = "")
        {
            ActualWBdropDownsModel actualWBdropDownsModel = new ActualWBdropDownsModel();
            SanitationJobWrapper saWrapper = new SanitationJobWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> ShiftLookUpList = new List<DataContracts.LookupList>();

            List<SanitationJobModel> sanApproveList = saWrapper.GetWOApprovalWorkBenchDetails(status, Createdates);
            //-------------------------------------
            List<string> cbList = new List<string>();
            if (sanApproveList != null)
            {
                cbList = sanApproveList.Select(r => r.CreateBy_PersonnelId).GroupBy(x => x.ToString()).Select(x => x.First()).ToList();
            }
            //-------------------------------------
            sanApproveList = this.GetAppworkBenchDetailsByColumnWithOrder(order, orderDir, sanApproveList);

            if (sanApproveList != null)
            {
                if (!string.IsNullOrEmpty(JobId))
                {
                    JobId = JobId.ToUpper();
                    sanApproveList = sanApproveList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(JobId))).ToList();
                }
                if (!string.IsNullOrEmpty(description))
                {
                    description = description.ToUpper();
                    sanApproveList = sanApproveList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(description))).ToList();
                }
                if (!string.IsNullOrEmpty(chargeto))
                {
                    chargeto = chargeto.ToUpper();
                    sanApproveList = sanApproveList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_ClientLookupId) && x.ChargeTo_ClientLookupId.ToUpper().Contains(chargeto))).ToList();
                }
                if (!string.IsNullOrEmpty(Chargetoname))
                {
                    Chargetoname = Chargetoname.ToUpper();
                    sanApproveList = sanApproveList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_Name) && x.ChargeTo_Name.ToUpper().Contains(Chargetoname))).ToList();
                }
                if (workassigned != 0)
                {
                    sanApproveList = sanApproveList.Where(x => x.AssignedTo_PersonnelId.Equals(workassigned)).ToList();
                }
                if (!string.IsNullOrEmpty(shift))
                {
                    shift = shift.ToUpper();
                    sanApproveList = sanApproveList.Where(x => (!string.IsNullOrWhiteSpace(x.Shift) && x.Shift.ToUpper().Contains(shift))).ToList();
                }
                if (scheduledate != null)
                {
                    sanApproveList = sanApproveList.Where(x => x.ScheduledDate.Equals(scheduledate)).ToList();
                }
                if (scheduleduration != 0)
                {
                    sanApproveList = sanApproveList.Where(x => x.ScheduledDuration.Equals(scheduleduration)).ToList();
                }
                if (createdate != null)
                {
                    sanApproveList = sanApproveList.Where(x => x.CreateDate.HasValue && x.CreateDate.Value.Date.Equals(createdate.Value.Date)).ToList();
                }
                if (!string.IsNullOrEmpty(createdby))
                {
                    createdby = createdby.ToUpper();
                    sanApproveList = sanApproveList.Where(x => (!string.IsNullOrWhiteSpace(x.CreateBy_PersonnelId) && x.CreateBy_PersonnelId.ToUpper().Contains(createdby))).ToList();
                }
            }

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = sanApproveList.Count();
            totalRecords = sanApproveList.Count();

            int initialPage = start.Value;

            var filteredResult = sanApproveList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();


            var PersonnelLookUplist = GetList_Personnel();
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                ShiftLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (ShiftLookUpList != null)
                {
                    actualWBdropDownsModel.ShiftList = ShiftLookUpList.Select(x => new DataTableDropdownModel { label = x.Description, value = x.ListValue }).ToList();
                }
            }

            foreach (var r in filteredResult)
            {
                r.WorkAssignedList = PersonnelLookUplist.Select(x => new DataTableDropdownModel { label = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, value = x.AssignedTo_PersonnelId.ToString() }).ToList();
                r.ShiftListdropDown = ShiftLookUpList.Select(x => new DataTableDropdownModel { label = x.Description, value = x.ListValue }).ToList();
            }
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, options = actualWBdropDownsModel, cbList = cbList }, JsonSerializerDateSettings);
        }

        public JsonResult GetAppWBGrid(int? draw, int? start, int? length, string status, string Createdates, string JobId = "", string description = "", string chargeto = "", string Chargetoname = "", long workassigned = 0,
                                            string shift = "", DateTime? scheduledate = null, decimal scheduleduration = 0, DateTime? createdate = null, string createdby = "")
        {
            SanitationJobWrapper saWrapper = new SanitationJobWrapper(userData);
            List<SanitationJobModel> sanApproveList = saWrapper.GetWOApprovalWorkBenchDetails(status, Createdates);
            if (sanApproveList != null)
            {
                if (!string.IsNullOrEmpty(JobId))
                {
                    JobId = JobId.ToUpper();
                    sanApproveList = sanApproveList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(JobId))).ToList();
                }
                if (!string.IsNullOrEmpty(description))
                {
                    description = description.ToUpper();
                    sanApproveList = sanApproveList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(description))).ToList();
                }
                if (!string.IsNullOrEmpty(chargeto))
                {
                    chargeto = chargeto.ToUpper();
                    sanApproveList = sanApproveList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_ClientLookupId) && x.ChargeTo_ClientLookupId.ToUpper().Contains(chargeto))).ToList();
                }
                if (!string.IsNullOrEmpty(Chargetoname))
                {
                    Chargetoname = Chargetoname.ToUpper();
                    sanApproveList = sanApproveList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_Name) && x.ChargeTo_Name.ToUpper().Contains(Chargetoname))).ToList();
                }
                if (workassigned != 0)
                {
                    sanApproveList = sanApproveList.Where(x => x.AssignedTo_PersonnelId.Equals(workassigned)).ToList();
                }
                if (!string.IsNullOrEmpty(shift))
                {
                    shift = shift.ToUpper();
                    sanApproveList = sanApproveList.Where(x => (!string.IsNullOrWhiteSpace(x.Shift) && x.Shift.ToUpper().Contains(shift))).ToList();
                }
                if (scheduledate != null)
                {
                    sanApproveList = sanApproveList.Where(x => x.ScheduledDate.Equals(scheduledate)).ToList();
                }
                if (scheduleduration != 0)
                {
                    sanApproveList = sanApproveList.Where(x => x.ScheduledDuration.Equals(scheduleduration)).ToList();
                }
                if (createdate != null)
                {
                    sanApproveList = sanApproveList.Where(x => x.CreateDate.Equals(createdate)).ToList();
                }
                if (!string.IsNullOrEmpty(createdby))
                {
                    createdby = createdby.ToUpper();
                    sanApproveList = sanApproveList.Where(x => (!string.IsNullOrWhiteSpace(x.CreateBy) && x.CreateBy.ToUpper().Contains(createdby))).ToList();
                }
            }
            return Json(sanApproveList, JsonRequestBehavior.AllowGet);
        }

        private List<SanitationJobModel> GetAppworkBenchDetailsByColumnWithOrder(string order, string orderDir, List<SanitationJobModel> data)
        {
            List<SanitationJobModel> lst = new List<SanitationJobModel>();

            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_ClientLookupId).ToList() : data.OrderBy(p => p.ChargeTo_ClientLookupId).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_Name).ToList() : data.OrderBy(p => p.ChargeTo_Name).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssignedTo_PersonnelId).ToList() : data.OrderBy(p => p.AssignedTo_PersonnelId).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Shift).ToList() : data.OrderBy(p => p.Shift).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScheduledDate).ToList() : data.OrderBy(p => p.ScheduledDate).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScheduledDuration).ToList() : data.OrderBy(p => p.ScheduledDuration).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                case "10":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateBy).ToList() : data.OrderBy(p => p.CreateBy).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }

        #region Approve Sanitation WorkBench
        [HttpPost]
        public ActionResult ApproveSanitationWB(List<SanitationApproveWBModel> WOData)
        {
            SanitationVM objSanVM = new SanitationVM();
            SanitationJobWrapper sanWrapper = new SanitationJobWrapper(userData);
            SanitationJob sanJob = sanWrapper.ApproveSanitationWBDetails(WOData);

            if (sanJob.ErrorMessages != null && sanJob.ErrorMessages.Count > 0)
            {
                return Json(new { Result = sanJob.ErrorMessages }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = "success" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Deny Sanitation WB
        [HttpPost]
        public JsonResult DenySanitationWB(string[] wOIds, string DeniedReason, string DeniedComments)
        {
            SanitationJobWrapper sanWrapper = new SanitationJobWrapper(userData);
            var retValue = sanWrapper.DenySanitationWB(wOIds, DeniedReason, DeniedComments);
            if (retValue)
            {
                return Json(new { Result = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = "failed" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}