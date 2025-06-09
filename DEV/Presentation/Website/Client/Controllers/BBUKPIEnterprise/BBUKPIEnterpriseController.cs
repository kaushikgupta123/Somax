using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Controllers.Common;
using Client.Models.BBUKPIEnterprise;

using Common.Constants;

using Newtonsoft.Json;

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Client.Common;
using Client.Models;
using DataContracts;
using System.Threading.Tasks;
using Client.ActionFilters;

namespace Client.Controllers
{

    public class BBUKPIEnterpriseController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.BBUKPI_Enterprise)]
        public ActionResult Index()
        {
            BBUKPIEnterpriseWrapper bbuwrapper = new BBUKPIEnterpriseWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            BBUKPIEnterpriseVM bbuVM = new BBUKPIEnterpriseVM();
            var siteList = bbuwrapper.GetSitesForSiteFilter();
            bbuVM.SiteList = siteList;
            var yearweeklist = bbuwrapper.GetYearWeekForFilter();
            bbuVM.YearWeekList = yearweeklist;
            bbuVM.CustomQueryDisplayList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.BBUKPI, false);
            bbuVM.DateRangeDropListForEnterpriseCreatedate = UtilityFunction.GetTimeRangeDropForBBUKPIEnterpriseCreateDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            bbuVM.DateRangeDropListForEnterpriseSubmitdate = UtilityFunction.GetTimeRangeDropForBBUKPIEnterpriseSubmitDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            LocalizeControls(bbuVM, LocalizeResourceSetConstants.BBUKPIDetails);
            return View(bbuVM);
        }

        public string GetBBUKPIEnterpriseGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, DateTime? SubmitStartDateVw = null, DateTime? SubmitEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, List<string> Sites = null, string SearchText = "", string Order = "1", string YearWeekfilters = "")
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            string _SubmitStartDateVw = string.Empty;
            string _SubmitEndDateVw = string.Empty;
            string _CreateStartDateVw = string.Empty;
            string _CreateEndDateVw = string.Empty;

            _SubmitStartDateVw = SubmitStartDateVw.HasValue ? SubmitStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _SubmitEndDateVw = SubmitEndDateVw.HasValue ? SubmitEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _CreateStartDateVw = CreateStartDateVw.HasValue ? CreateStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _CreateEndDateVw = CreateEndDateVw.HasValue ? CreateEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            var filter = CustomQueryDisplayId;
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            BBUKPIEnterpriseWrapper bbukpiWrapper = new BBUKPIEnterpriseWrapper(userData);
            List<BBUKPIEnterpriseSearchGridModel> bbukpiList = bbukpiWrapper.GetBBUKPIEnterpriseGriddata(CustomQueryDisplayId, _SubmitStartDateVw, _SubmitEndDateVw, _CreateStartDateVw, _CreateEndDateVw, Sites, Order, orderDir, skip, length ?? 0, SearchText, YearWeekfilters);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (bbukpiList != null && bbukpiList.Count > 0)
            {
                recordsFiltered = bbukpiList[0].TotalCount;
                totalRecords = bbukpiList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = bbukpiList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion

        #region Print
        [HttpGet]
        public string GetBBUKPIEnterprisePrintData(int CustomQueryDisplayId = 0, DateTime? SubmitStartDateVw = null, DateTime? SubmitEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, List<string> Sites = null, string SearchText = "", string colname = "1",string coldir="asc", string YearWeekfilters = "")
        {
            string _SubmitStartDateVw = string.Empty;
            string _SubmitEndDateVw = string.Empty;
            string _CreateStartDateVw = string.Empty;
            string _CreateEndDateVw = string.Empty;

            _SubmitStartDateVw = SubmitStartDateVw.HasValue ? SubmitStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _SubmitEndDateVw = SubmitEndDateVw.HasValue ? SubmitEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _CreateStartDateVw = CreateStartDateVw.HasValue ? CreateStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _CreateEndDateVw = CreateEndDateVw.HasValue ? CreateEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            BBUKPIEnterpriseWrapper bbukpiWrapper = new BBUKPIEnterpriseWrapper(userData);
            List<BBUKPIEnterpriseSearchGridModel> bbukpiList = bbukpiWrapper.GetBBUKPIEnterpriseGriddata(CustomQueryDisplayId, _SubmitStartDateVw, _SubmitEndDateVw, _CreateStartDateVw, _CreateEndDateVw, Sites, colname, coldir, 0, 100000, SearchText, YearWeekfilters);
            List<BBUKPIEnterprisePrintModel> enterprisePrintModelList = new List<BBUKPIEnterprisePrintModel>();
            BBUKPIEnterprisePrintModel objBBUKPIEnterprisePrintModel;
            foreach (var p in bbukpiList)
            {
                objBBUKPIEnterprisePrintModel = new BBUKPIEnterprisePrintModel();
                objBBUKPIEnterprisePrintModel.WeekNumber = p.WeekNumber;
                objBBUKPIEnterprisePrintModel.Year = p.Year;
                objBBUKPIEnterprisePrintModel.SiteName = p.SiteName;
                //objBBUKPIEnterprisePrintModel.Status = p.Status;
                objBBUKPIEnterprisePrintModel.Created = Convert.ToDateTime(p.Created);
                objBBUKPIEnterprisePrintModel.PMPercentCompleted = p.PMPercentCompleted;
                objBBUKPIEnterprisePrintModel.pMFollowUpComp = p.pMFollowUpComp;
                objBBUKPIEnterprisePrintModel.activeMechUsers = p.activeMechUsers;
                objBBUKPIEnterprisePrintModel.WOBacklogCount = p.WOBacklogCount;
                objBBUKPIEnterprisePrintModel.rCACount = p.rCACount;
                objBBUKPIEnterprisePrintModel.tTRCount = p.tTRCount;
                objBBUKPIEnterprisePrintModel.invValueOverMax = p.invValueOverMax;
                objBBUKPIEnterprisePrintModel.phyInvAccuracy = p.phyInvAccuracy;
                objBBUKPIEnterprisePrintModel.cycleCountProgress = p.cycleCountProgress;
                objBBUKPIEnterprisePrintModel.eVTrainingHrs = p.eVTrainingHrs;
                objBBUKPIEnterprisePrintModel.weekStart = p.weekStart;
                objBBUKPIEnterprisePrintModel.weekEnd = p.weekEnd;

                enterprisePrintModelList.Add(objBBUKPIEnterprisePrintModel);
            }
            return JsonConvert.SerializeObject(new { data = enterprisePrintModelList }, JsonSerializerDateSettings);
        }
        #endregion

        #region Details
        public PartialViewResult BBUKPIEnterpriseDetails(long BBUKPIId)
        {
            BBUKPIEnterpriseVM objMaterialRequestVM = new BBUKPIEnterpriseVM();
            BBUKPIEnterpriseModel bBUKPIEnterpriseModel = new BBUKPIEnterpriseModel();
            BBUKPIEnterpriseWrapper mrWrapper = new BBUKPIEnterpriseWrapper(userData);
            bBUKPIEnterpriseModel = mrWrapper.PopulateBBUKPIDetails(BBUKPIId);
            objMaterialRequestVM.udata = this.userData;
            objMaterialRequestVM.security = this.userData.Security;
            objMaterialRequestVM.BBUKPIEnterpriseModel = bBUKPIEnterpriseModel;
            LocalizeControls(objMaterialRequestVM, LocalizeResourceSetConstants.BBUKPIDetails);
            return PartialView("~/Views/BBUKPIEnterprise/_BBUKPIEnterpriseDetails.cshtml", objMaterialRequestVM);
        }
        #endregion

        #region Activity
        [HttpPost]
        public PartialViewResult LoadActivity(long ObjectId)
        {
            BBUKPIEnterpriseVM objBBUEnterpriseVM = new BBUKPIEnterpriseVM();
            BBUKPIEnterpriseWrapper enterpriseWrapper = new BBUKPIEnterpriseWrapper(userData);
            List<EventLogModel> EventLogList = enterpriseWrapper.PopulateEventLog(ObjectId);
            objBBUEnterpriseVM.eventLogList = EventLogList;
            LocalizeControls(objBBUEnterpriseVM, LocalizeResourceSetConstants.BBUKPIDetails);
            return PartialView("_ActivityLog", objBBUEnterpriseVM);
        }
        #endregion

        #region Comment
        [HttpPost]
        public PartialViewResult LoadComments(long ObjectId)
        {
            BBUKPIEnterpriseVM objBBUEnterpriseVM = new BBUKPIEnterpriseVM();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Personnel> personnelsList = new List<Personnel>();
            List<Notes> NotesList = new List<Notes>();

            Task[] tasks = new Task[1];
            tasks[0] = Task.Factory.StartNew(() => NotesList = coWrapper.PopulateComment(ObjectId, "BBUKPI"));
            Task.WaitAll(tasks);

            if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {
                objBBUEnterpriseVM.NotesList = NotesList;
            }
            LocalizeControls(objBBUEnterpriseVM, LocalizeResourceSetConstants.BBUKPIDetails);
            return PartialView("_CommentsList", objBBUEnterpriseVM);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddComments(long BBUKPIId, string content, long noteId = 0, long updatedindex = 0)
        {
            CommonWrapper coWrapper = new CommonWrapper(userData);

            NotesModel notesModel = new NotesModel();
            notesModel.ObjectId = BBUKPIId;
            notesModel.Content = content;
            notesModel.NotesId = noteId;
            notesModel.ClientLookupId = BBUKPIId.ToString();
            notesModel.updatedindex = updatedindex;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateCommentWithoutUserMention(notesModel, ref Mode, "BBUKPI");
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), BBUKPIId = BBUKPIId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ReOpen
        public JsonResult ChangeStatus(long BBUKPIId, string Status)
        {
            BBUKPIEnterpriseWrapper stWrapper = new BBUKPIEnterpriseWrapper(userData);
            var Result = stWrapper.ChangeStatus(BBUKPIId, Status);
            if (Result.ErrorMessages == null || Result.ErrorMessages.Count == 0)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
