using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.FleetFuel;
using Client.BusinessWrapper.FleetIssue;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.FleetFuel;
using Client.Models.FleetIssue;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
namespace Client.Controllers.FleetIssue
{
    public class FleetIssueController : SomaxBaseController
    {
        #region Fleet Issue Search
        [CheckUserSecurity(securityType = SecurityConstants.Fleet_Issues)]
        public ActionResult Index()
        {
            FleetIssueWrapper fltIssueWrapper = new FleetIssueWrapper(userData);
            FleetIssueVM fltIssueVM = new FleetIssueVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            fltIssueVM.FleetIssueModel = new FleetIssueModel();
            fltIssueVM.security = this.userData.Security;
            fltIssueVM.FleetIssueModel.DateRangeDropListForFFRecorddate = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            fltIssueVM.FleetIssueModel.IssueViewList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.FleetIssues);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> DefectList = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                DefectList = AllLookUps.Where(x => x.ListName == LookupListConstants.FLEET_DEFECTS).ToList();
                if (DefectList != null)
                {
                    fltIssueVM.LookupDefectsList = DefectList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }

            }

            fltIssueVM.FleetIssueModel.DateRangeDropListAllStatus = UtilityFunction.GetTimeRangeDropForAllStatusFI().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            LocalizeControls(fltIssueVM, LocalizeResourceSetConstants.EquipmentDetails);
            return View(fltIssueVM);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetFleetIssueGridData(int? draw, int? start, int? length, int customQueryDisplayId = 1, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, string ClientLookupId = "", string Name = "", string Make = "", string Model = "", string VIN = "", DateTime? StartRecordDate = null, DateTime? EndRecordDate = null, List<string> Defects = null, string SearchText = ""
     )
        {
            List<FleetIssueSearchModel> fltIssueSearchModelList = new List<FleetIssueSearchModel>();
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            string _startRecordDate = string.Empty;
            string _endRecordDate = string.Empty;
            string _createStartDateVw = string.Empty;
            string _createEndDateVw = string.Empty;
            SearchText = SearchText.Replace("%", "[%]");
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            Name = Name.Replace("%", "[%]");
            Make = Make.Replace("%", "[%]");
            Model = Model.Replace("%", "[%]");
            VIN = VIN.Replace("%", "[%]");
            _startRecordDate = StartRecordDate.HasValue ? StartRecordDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endRecordDate = EndRecordDate.HasValue ? EndRecordDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _createStartDateVw = CreateStartDateVw.HasValue ? CreateStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _createEndDateVw = CreateEndDateVw.HasValue ? CreateEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            List<string> typeList = new List<string>();
            FleetIssueWrapper fltIssueWrapper = new FleetIssueWrapper(userData);
            List<FleetIssueSearchModel> fleetIssueList = fltIssueWrapper.GetFleetIssueGridData(customQueryDisplayId, _createStartDateVw, _createEndDateVw, order, orderDir, skip, length ?? 0, ClientLookupId, Name, Make, Model, VIN, _startRecordDate, _endRecordDate, Defects, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            int openCount = 0;
            if (fleetIssueList != null && fleetIssueList.Count > 0)
            {
                recordsFiltered = fleetIssueList[0].TotalCount;
                totalRecords = fleetIssueList[0].TotalCount;
                openCount = fleetIssueList.Count(x => x.Status.ToUpper().Equals(IssueStatusConstants.Open.ToUpper()));
            }
            int initialPage = start.Value;
            var filteredResult = fleetIssueList
              .ToList();
            bool IsFleetIssueEditSecurity = userData.Security.Fleet_Issues.Edit;
            bool IsFleetIssueDeleteSecurity = userData.Security.Fleet_Issues.Delete;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsFleetIssueEditSecurity = IsFleetIssueEditSecurity, IsFleetIssueDeleteSecurity = IsFleetIssueDeleteSecurity, openItemCount = openCount }, JsonSerializer12HoursDateAndTimeUptoMinuteSettings);
        }
        #endregion

        #region  Add or Edit
        public PartialViewResult FleetIssueAddOrEdit(long FleetIssuesId)
        {
            FleetIssueWrapper fIWrapper = new FleetIssueWrapper(userData);
            FleetIssueVM fltissueVM = new FleetIssueVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> DefectList = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                DefectList = AllLookUps.Where(x => x.ListName == LookupListConstants.FLEET_DEFECTS).ToList();
                if (DefectList != null)
                {
                    fltissueVM.LookupDefectsList = DefectList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }

            }
            fltissueVM.FleetIssueModel = new FleetIssueModel();
            fltissueVM.FleetIssueModel.Pagetype = "Add";
            if (FleetIssuesId != 0)
            {
                fltissueVM.FleetIssueModel = fIWrapper.GetEditFleetIssueDetailsById(FleetIssuesId);
                fltissueVM.FleetIssueModel.Pagetype = "Edit";
            }

            LocalizeControls(fltissueVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("~/Views/FleetIssue/_FleetIssueAddOrEdit.cshtml", fltissueVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FleetIssueAddOrEdit(FleetIssueVM objFI)
        {
            List<string> ErrorList = new List<string>();
            FleetIssueWrapper fIWrapper = new FleetIssueWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                if (objFI.FleetIssueModel.FleetIssuesId > 0)
                {
                    Mode = "Edit";
                }
                else
                {
                    Mode = "Add";
                }
                FleetIssues fleetIssue = new FleetIssues();
                string FI_ClientLookupId = objFI.FleetIssueModel.ClientLookupId.ToUpper().Trim();
                fleetIssue = fIWrapper.AddOrEditFleetIssue(FI_ClientLookupId, objFI);
                if (fleetIssue.ErrorMessages != null && fleetIssue.ErrorMessages.Count > 0)
                {
                    return Json(fleetIssue.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion
        #region Delete FleetIssue
        public ActionResult DeleteFleetIssue(long fleetIssuesId)
        {
            FleetIssueWrapper fIWrapper = new FleetIssueWrapper(userData);
            FleetIssues FleetIssues = new FleetIssues();
             FleetIssues = fIWrapper.DeleteFleetIssue(fleetIssuesId);
            string serviceorderTypeErrorcount = string.Empty;
            string errormsg = string.Empty;           
           
            if (FleetIssues.ErrorMessages == null || FleetIssues.ErrorMessages.Count == 0)
            {
                return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
            }
            else if (FleetIssues.ErrorCodd != null)
            {
                errormsg = "ServiceOrderExist";
                return Json(errormsg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonReturnEnum.failed.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Print
        [HttpPost]
        [EncryptedActionParameter]
        public JsonResult SetPrintData(FleetIssuePrintParams fleetIssuePrintParams)
        {
            Session["FLEETISSUEPRINTPARAMS"] = fleetIssuePrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public ActionResult ExportASPDF()
        {
            FleetIssueVM fltIssueVM = new FleetIssueVM();
            FleetIssueWrapper fIWrapper = new FleetIssueWrapper(userData);
            FleetIssuePDFPrintModel fleetIssuePDFPrintModel = new FleetIssuePDFPrintModel();
            List<FleetIssuePDFPrintModel> fleetIssuePDFPrintModelList = new List<FleetIssuePDFPrintModel>();
            FleetIssuePrintParams fleetIssuePrintParams = (FleetIssuePrintParams)Session["FLEETISSUEPRINTPARAMS"];
            FleetIssueSearchModel fleetIssueSearchModel = new FleetIssueSearchModel();
            int customQueryDisplayId = fleetIssuePrintParams.customQueryDisplayId;
            string order = fleetIssuePrintParams.colname;
            string orderDir = fleetIssuePrintParams.coldir;
            string _startRecordDate = string.Empty;
            string _endRecordDate = string.Empty;
            string _startCreateDateVw = string.Empty;
            string _endCreateDateVw = string.Empty;
            string SearchText = fleetIssuePrintParams.SearchText;
            string ClientLookupId = fleetIssuePrintParams.ClientLookupId;
            string Name = fleetIssuePrintParams.Name;
            string Make = fleetIssuePrintParams.Make;
            string Model = fleetIssuePrintParams.Model;
            string VIN = fleetIssuePrintParams.VIN;
            List<string> Defects = fleetIssuePrintParams.Defects;
            _startRecordDate = fleetIssuePrintParams.StartRecordDate.HasValue ? fleetIssuePrintParams.StartRecordDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endRecordDate = fleetIssuePrintParams.EndRecordDate.HasValue ? fleetIssuePrintParams.EndRecordDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startCreateDateVw = fleetIssuePrintParams.StartCreateDateVw.HasValue ? fleetIssuePrintParams.StartCreateDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endCreateDateVw = fleetIssuePrintParams.EndCreateDateVw.HasValue ? fleetIssuePrintParams.EndCreateDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            List<FleetIssueSearchModel> fleetIssueList = fIWrapper.GetFleetIssueGridData(customQueryDisplayId, _startCreateDateVw, _endCreateDateVw, order, orderDir, 0, 100000, ClientLookupId, Name, Make, Model, VIN, _startRecordDate, _endRecordDate, Defects, SearchText);

            foreach (var item in fleetIssueList)
            {
                fleetIssuePDFPrintModel = new FleetIssuePDFPrintModel();
                fleetIssuePDFPrintModel.ClientLookupId = item.ClientLookupId;
                fleetIssuePDFPrintModel.Name = item.Name;
                fleetIssuePDFPrintModel.ImageUrl = item.ImageUrl;
                fleetIssuePDFPrintModel.RecordDate = item.RecordDate;
                fleetIssuePDFPrintModel.Description = item.Description;
                fleetIssuePDFPrintModel.Status = item.Status;
                fleetIssuePDFPrintModel.Defects = item.Defects;
                fleetIssuePDFPrintModel.CompleteDate = item.CompleteDate;
                fleetIssuePDFPrintModelList.Add(fleetIssuePDFPrintModel);

            }
            fltIssueVM.fleetIssuePDFPrintModel = fleetIssuePDFPrintModelList;

            LocalizeControls(fltIssueVM, LocalizeResourceSetConstants.EquipmentDetails);
            return new ViewAsPdf("FleetIssueGridPdfPrintTemplate", fltIssueVM)
            {
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
            };
        }

        [NoDirectAccess]
        public ActionResult PrintASPDF()
        {
            FleetIssueVM fltIssueVM = new FleetIssueVM();
            FleetIssueWrapper fIWrapper = new FleetIssueWrapper(userData);
            FleetIssuePDFPrintModel fleetIssuePDFPrintModel = new FleetIssuePDFPrintModel();
            List<FleetIssuePDFPrintModel> fleetIssuePDFPrintModelList = new List<FleetIssuePDFPrintModel>();
            FleetIssuePrintParams fleetIssuePrintParams = (FleetIssuePrintParams)Session["FLEETISSUEPRINTPARAMS"];
            FleetIssueSearchModel fleetIssueSearchModel = new FleetIssueSearchModel();
            int customQueryDisplayId = fleetIssuePrintParams.customQueryDisplayId;
            string order = fleetIssuePrintParams.colname;
            string orderDir = fleetIssuePrintParams.coldir;
            string _startRecordDate = string.Empty;
            string _endRecordDate = string.Empty;
            string _startCreateDateVw = string.Empty;
            string _endCreateDateVw = string.Empty;
            string SearchText = fleetIssuePrintParams.SearchText;
            string ClientLookupId = fleetIssuePrintParams.ClientLookupId;
            string Name = fleetIssuePrintParams.Name;
            string Make = fleetIssuePrintParams.Make;
            string Model = fleetIssuePrintParams.Model;
            string VIN = fleetIssuePrintParams.VIN;
            List<string> Defects = fleetIssuePrintParams.Defects;
            _startRecordDate = fleetIssuePrintParams.StartRecordDate.HasValue ? fleetIssuePrintParams.StartRecordDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endRecordDate = fleetIssuePrintParams.EndRecordDate.HasValue ? fleetIssuePrintParams.EndRecordDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startCreateDateVw = fleetIssuePrintParams.StartCreateDateVw.HasValue ? fleetIssuePrintParams.StartCreateDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endCreateDateVw = fleetIssuePrintParams.EndCreateDateVw.HasValue ? fleetIssuePrintParams.EndCreateDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            List<FleetIssueSearchModel> fleetIssueList = fIWrapper.GetFleetIssueGridData(customQueryDisplayId, _startCreateDateVw, _endCreateDateVw, order, orderDir, 0, 100000, ClientLookupId, Name, Make, Model, VIN, _startRecordDate, _endRecordDate, Defects, SearchText);

            foreach (var item in fleetIssueList)
            {
                fleetIssuePDFPrintModel = new FleetIssuePDFPrintModel();
                fleetIssuePDFPrintModel.ClientLookupId = item.ClientLookupId;
                fleetIssuePDFPrintModel.Name = item.Name;
                fleetIssuePDFPrintModel.ImageUrl = item.ImageUrl;
                fleetIssuePDFPrintModel.RecordDate = item.RecordDate;
                fleetIssuePDFPrintModel.Description = item.Description;
                fleetIssuePDFPrintModel.Status = item.Status;
                fleetIssuePDFPrintModel.Defects = item.Defects;
                fleetIssuePDFPrintModel.CompleteDate = item.CompleteDate;
                fleetIssuePDFPrintModelList.Add(fleetIssuePDFPrintModel);

            }
            fltIssueVM.fleetIssuePDFPrintModel = fleetIssuePDFPrintModelList;

            LocalizeControls(fltIssueVM, LocalizeResourceSetConstants.EquipmentDetails);

            return new PartialViewAsPdf("FleetIssueGridPdfPrintTemplate", fltIssueVM)
            {
                PageSize = Rotativa.Options.Size.A4,
                FileName = "FleetIssue.pdf",
                PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
            };
        }
        #endregion
    }
}