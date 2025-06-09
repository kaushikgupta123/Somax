using System;
using System.Collections.Generic;
using System.Linq;
using Client.BusinessWrapper.SanitationVerification;
using Client.Models.Sanitation;
using Common.Constants;
using Newtonsoft.Json;
using System.Web.Mvc;
using Client.Common;
using Client.Models.Common;
using Client.Models.Work_Order;
using Client.BusinessWrapper.Common;
using Client.Controllers.Common;
using Client.ActionFilters;

namespace Client.Controllers.Sanitation
{
    public class SanitationVerificationController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.Sanitation_Verification)]
        public ActionResult Index()
        {
            SanitationVM objSVM = new SanitationVM();
            SanitationVerificationModel SVModel = new SanitationVerificationModel();
            SanitationJobModel SJmodel = new SanitationJobModel();
            SanitationVerificationWrapper SVWrapper = new SanitationVerificationWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> ShiftLookUpList = new List<DataContracts.LookupList>();

            var StatusList = UtilityFunction.WorkBenchStatusList();
            if (StatusList != null)
            {
                SVModel.scheduleStatusList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var CreateDateList = UtilityFunction.WorkBenchCreateDatesList();
            if (CreateDateList != null)
            {
                SVModel.scheduleCreateDateList = CreateDateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                ShiftLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (ShiftLookUpList != null)
                {
                    SJmodel.ShiftList = ShiftLookUpList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }
            }
            var PersonnelLookUplist = GetList_Personnel();
            if (PersonnelLookUplist != null)
            {
                SJmodel.WorkAssignedLookUpList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            objSVM.sanitationJobModel = SJmodel;
            objSVM.SanitationVerificationModel = SVModel;
            objSVM.security = this.userData.Security;
            LocalizeControls(objSVM, LocalizeResourceSetConstants.SanitationDetails);
            return View(objSVM);
        }
        [HttpPost]
        public string GetSanitationVerificationData(int? draw, int? start, int? length, int StatusTypeId = 0, int CreatedDatesId = 0, string JobId = "", string description = "", string chargeToLookUpId = "", string Chargetoname = "", long workassigned = 0, string shift = "")
        {
            ActualWBdropDownsModel actualWBdropDownsModel = new ActualWBdropDownsModel();
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> FailReasonList = new List<DataContracts.LookupList>();
            SanitationVerificationWrapper SVWrapper = new SanitationVerificationWrapper(userData);
            var SVMasterList = SVWrapper.RetrieveSanitationVerificationdata(StatusTypeId, CreatedDatesId);
            SVMasterList = this.GetAllSanitationSortByColumnWithOrder(order, orderDir, SVMasterList);
            if (SVMasterList != null)
            {
                if (!string.IsNullOrEmpty(JobId))
                {
                    JobId = JobId.ToUpper();
                    SVMasterList = SVMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(JobId))).ToList();
                }
                if (!string.IsNullOrEmpty(description))
                {
                    description = description.ToUpper();
                    SVMasterList = SVMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(description))).ToList();
                }
                if (!string.IsNullOrEmpty(chargeToLookUpId))
                {
                    chargeToLookUpId = chargeToLookUpId.ToUpper();
                    SVMasterList = SVMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_ClientLookupId) && x.ChargeTo_ClientLookupId.ToUpper().Contains(chargeToLookUpId))).ToList();
                }
                if (!string.IsNullOrEmpty(Chargetoname))
                {
                    Chargetoname = Chargetoname.ToUpper();
                    SVMasterList = SVMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_Name) && x.ChargeTo_Name.ToUpper().Contains(Chargetoname))).ToList();
                }
                if (workassigned != 0)
                {
                    SVMasterList = SVMasterList.Where(x => x.AssignedTo_PersonnelId.Equals(workassigned)).ToList();
                }
                if (!string.IsNullOrEmpty(shift))
                {
                    shift = shift.ToUpper();
                    SVMasterList = SVMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Shift) && x.Shift.ToUpper().Contains(shift))).ToList();
                }
            }

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = SVMasterList.Count();
            totalRecords = SVMasterList.Count();

            int initialPage = start.Value;

            var filteredResult = SVMasterList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                FailReasonList = AllLookUps.Where(x => x.ListName == LookupListConstants.SANIT_FAIL_RESN).ToList();
                if (FailReasonList != null)
                {
                    actualWBdropDownsModel.FailReasonList = FailReasonList.Select(x => new DataTableDropdownModel { label = x.Description, value = x.ListValue }).ToList();
                }

            }           
            foreach (var r in filteredResult)
            {
                r.FailReasonList = FailReasonList.Select(x => new DataTableDropdownModel { label = x.Description, value = x.ListValue }).ToList();
            }
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, options = actualWBdropDownsModel }, JsonSerializerDateSettings);
        }

        [HttpGet]
        public JsonResult GetSanitVerification(int? draw, int? start, int? length, int StatusTypeId = 0, int CreatedDatesId = 0, string JobId = "", string description = "", string chargeToLookUpId = "", string Chargetoname = "", long workassigned = 0, string shift = "")
        {
            SanitationVerificationWrapper SVWrapper = new SanitationVerificationWrapper(userData);
            var SVMasterList = SVWrapper.RetrieveSanitationVerificationdata(StatusTypeId, CreatedDatesId);
            if (SVMasterList != null)
            {
                if (!string.IsNullOrEmpty(JobId))
                {
                    JobId = JobId.ToUpper();
                    SVMasterList = SVMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(JobId))).ToList();
                }
                if (!string.IsNullOrEmpty(description))
                {
                    description = description.ToUpper();
                    SVMasterList = SVMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(description))).ToList();
                }
                if (!string.IsNullOrEmpty(chargeToLookUpId))
                {
                    chargeToLookUpId = chargeToLookUpId.ToUpper();
                    SVMasterList = SVMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_ClientLookupId) && x.ChargeTo_ClientLookupId.ToUpper().Contains(chargeToLookUpId))).ToList();
                }
                if (!string.IsNullOrEmpty(Chargetoname))
                {
                    Chargetoname = Chargetoname.ToUpper();
                    SVMasterList = SVMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_Name) && x.ChargeTo_Name.ToUpper().Contains(Chargetoname))).ToList();
                }
                if (workassigned != 0)
                {
                    SVMasterList = SVMasterList.Where(x => x.AssignedTo_PersonnelId.Equals(workassigned)).ToList();
                }
                if (!string.IsNullOrEmpty(shift))
                {
                    shift = shift.ToUpper();
                    SVMasterList = SVMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Shift) && x.Shift.ToUpper().Contains(shift))).ToList();
                }
            }
            return Json(SVMasterList, JsonRequestBehavior.AllowGet);
        }

        private List<SanitationVerificationModel> GetAllSanitationSortByColumnWithOrder(string order, string orderDir, List<SanitationVerificationModel> data)
        {

            List<SanitationVerificationModel> lst = new List<SanitationVerificationModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_ClientLookupId).ToList() : data.OrderBy(p => p.ChargeTo_ClientLookupId).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_Name).ToList() : data.OrderBy(p => p.ChargeTo_Name).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompleteDate).ToList() : data.OrderBy(p => p.CompleteDate).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompleteComments).ToList() : data.OrderBy(p => p.CompleteComments).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FailReason).ToList() : data.OrderBy(p => p.FailReason).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FailComment).ToList() : data.OrderBy(p => p.FailComment).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Assigned_PersonnelClientLookupId).ToList() : data.OrderBy(p => p.Assigned_PersonnelClientLookupId).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Shift).ToList() : data.OrderBy(p => p.Shift).ToList();
                    break;
            }
            return lst;
        }

        public JsonResult SavePassListFromGrid(List<SVGridModel> list)
        {
            SanitationVerificationWrapper SVWrapper = new SanitationVerificationWrapper(userData);

            var result = SVWrapper.UpdatePassedSVListGrid(list);
            if (result != null && result.Count == 0)
            {
                return Json(new { Result = "success", SanitationJobId = list[0].SanitationJobId }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = "failed" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveFailListFromGrid(List<SVGridModel> list)
        {
            SanitationVerificationWrapper SVWrapper = new SanitationVerificationWrapper(userData);
            var result = SVWrapper.UpdateFailedSVListGrid(list);
            if (result != null && result.Count == 0)
            {
                return Json(new { Result = "success", SanitationJobId = list[0].SanitationJobId }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = "failed" }, JsonRequestBehavior.AllowGet);
        }
    }
}