using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.BBUKPISite;

using Common.Constants;

using DataContracts;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Client.Controllers
{

    public class BBUKPISiteController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.BBUKPI_Site)]
        public ActionResult Index()
        {
            BBUKPISiteWrapper bbuwrapper = new BBUKPISiteWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            BBUKPISiteVM bbuVM = new BBUKPISiteVM();
            var yearweeklist = bbuwrapper.GetYearWeekForFilter();
            bbuVM.YearWeekList = yearweeklist;
            bbuVM.CustomQueryDisplayList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.BBUKPI, false);
            bbuVM.DateRangeDropListForSiteCreatedate = UtilityFunction.GetTimeRangeDropForBBUKPISiteCreateDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            bbuVM.DateRangeDropListForSiteSubmitdate = UtilityFunction.GetTimeRangeDropForBBUKPISiteSubmitDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });

            LocalizeControls(bbuVM, LocalizeResourceSetConstants.BBUKPIDetails);
            return View(bbuVM);
        }

        [HttpPost]
        public string GetBBUKPISiteGridData(int? draw, int? start, int? length, string Order = "0", int customQueryDisplayId = 1, string Week = "", string Year = "", string Status = "", decimal? PMWOCompleted = null, int? WOBacklogCount = null, string SubmitStartDateVw = "", string SubmitEndDateVw = "", string CreateStartDateVw = "", string CreateEndDateVw = "", string SearchText = ""
       //, DateTime? SubmitDate = null, DateTime? CreateDate = null , string orderDir = "asc"
       , string YearWeekfilters = "")
        {
            //string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;

            BBUKPISiteWrapper wrapper = new BBUKPISiteWrapper(userData);
            var DataList = wrapper.GetBBUKPISiteGridData(customQueryDisplayId, Order, orderDir, skip, length ?? 0, Week, Year, Status, PMWOCompleted ?? 0, WOBacklogCount ?? 0, SubmitStartDateVw, SubmitEndDateVw, CreateStartDateVw, CreateEndDateVw, SearchText, YearWeekfilters);

            var totalRecords = 0;
            var recordsFiltered = 0;

            if (DataList != null && DataList.Count > 0)
            {
                recordsFiltered = DataList[0].TotalCount;
                totalRecords = DataList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            var filteredResult = DataList.ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = DataList }, JsonSerializerDateSettings);
        }

        [HttpGet]
        public string GetBBUKPISitePrintData(int? length, string Order = "0", string OrderDir = "", int customQueryDisplayId = 1, string Week = "", string Year = "", string Status = "", decimal? PMWOCompleted = null, int? WOBacklogCount = null, string SubmitStartDateVw = "", string SubmitEndDateVw = "", string CreateStartDateVw = "", string CreateEndDateVw = "", string SearchText = ""
       //, DateTime? SubmitDate = null, DateTime? CreateDate = null , string orderDir = "asc"
       , List<string> YearWeekfilters = null)
        {
            BBUKPISiteWrapper wrapper = new BBUKPISiteWrapper(userData);
            var DataList = wrapper.GetBBUKPISitePrintData(customQueryDisplayId, Order, OrderDir, 0, 100000, Week, Year, Status, PMWOCompleted ?? 0, WOBacklogCount ?? 0, SubmitStartDateVw, SubmitEndDateVw, CreateStartDateVw, CreateEndDateVw, SearchText, YearWeekfilters);

            return JsonConvert.SerializeObject(new { data = DataList }, JsonSerializerDateSettings);
        }
        #endregion

        #region Details
        public PartialViewResult BBUKPISiteDetails(long BBUKPIId)
        {
            BBUKPISiteVM bBUKPISiteVM = new BBUKPISiteVM();
            BBUKPISiteModel model = new BBUKPISiteModel();
            BBUKPISiteWrapper siteWrapper = new BBUKPISiteWrapper(userData);

            model = siteWrapper.PopulateBBUKPIDetails(BBUKPIId);

            bBUKPISiteVM.udata = this.userData;
            //bBUKPISiteVM.security = this.userData.Security;
            bBUKPISiteVM.BBUKPISiteModel = model;

            LocalizeControls(bBUKPISiteVM, LocalizeResourceSetConstants.BBUKPIDetails);
            return PartialView("_BBUKPISiteDetails", bBUKPISiteVM);
        }
        #endregion

        #region Activity
        [HttpPost]
        public PartialViewResult LoadActivity(long ObjectId)
        {
            BBUKPISiteVM bBUKPISiteVM = new BBUKPISiteVM();
            BBUKPISiteWrapper siteWrapper = new BBUKPISiteWrapper(userData);

            List<EventLogModel> EventLogList = siteWrapper.PopulateEventLog(ObjectId);
            bBUKPISiteVM.eventLogList = EventLogList;

            LocalizeControls(bBUKPISiteVM, LocalizeResourceSetConstants.BBUKPIDetails);
            return PartialView("_ActivityLog", bBUKPISiteVM);
        }
        #endregion

        #region Comment
        [HttpPost]
        public PartialViewResult LoadComments(long ObjectId)
        {
            BBUKPISiteVM bBUKPISiteVM = new BBUKPISiteVM();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Personnel> personnelsList = new List<Personnel>();
            List<Notes> NotesList = new List<Notes>();
            //UserMentionData userMentionData;
            //List<UserMentionData> userMentionDatas = new List<UserMentionData>();

            Task[] tasks = new Task[1];
            //tasks[0] = Task.Factory.StartNew(() => personnelsList = coWrapper.MentionList(""));
            tasks[0] = Task.Factory.StartNew(() => NotesList = coWrapper.PopulateComment(ObjectId, "BBUKPI"));
            Task.WaitAll(tasks);

            //if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            //{
            //    foreach (var myuseritem in personnelsList)
            //    {
            //        userMentionData = new UserMentionData();
            //        userMentionData.id = myuseritem.UserName;
            //        userMentionData.name = myuseritem.FullName;
            //        userMentionData.type = myuseritem.PersonnelInitial;
            //        userMentionDatas.Add(userMentionData);
            //    }
            //    objWorksOrderVM.userMentionDatas = userMentionDatas;
            //}
            if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {
                bBUKPISiteVM.NotesList = NotesList;
            }
            LocalizeControls(bBUKPISiteVM, LocalizeResourceSetConstants.BBUKPIDetails);
            return PartialView("_CommentsList", bBUKPISiteVM);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddComments(long BBUKPIId, string content, /*string woClientLookupId, List<string> userList,*/ long noteId = 0, long updatedindex = 0)
        {
            CommonWrapper coWrapper = new CommonWrapper(userData);
            //var namelist = coWrapper.MentionList("");
            //List<UserMentionData> userMentionDataList = new List<UserMentionData>();
            //UserMentionData objUserMentionData;
            //if (userList != null && userList.Count > 0)
            //{
            //    foreach (var item in userList)
            //    {
            //        objUserMentionData = new UserMentionData();//new UserMentionData();
            //        objUserMentionData.userId = namelist.Where(x => x.UserName == item).Select(y => y.PersonnelId).FirstOrDefault();
            //        objUserMentionData.userName = item;
            //        objUserMentionData.emailId = namelist.Where(x => x.UserName == item).Select(y => y.Email).FirstOrDefault();
            //        userMentionDataList.Add(objUserMentionData);
            //    }
            //}

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

        #region Edit
        [HttpPost]
        public ActionResult BBUKPISiteEdit(long BBUKPIId)
        {
            BBUKPISiteVM bBUKPISiteVM = new BBUKPISiteVM();
            BBUKPISiteEditModel model = new BBUKPISiteEditModel();
            BBUKPISiteWrapper siteWrapper = new BBUKPISiteWrapper(userData);

            model = siteWrapper.PopulateBBUKPIDetailsForEdit(BBUKPIId);

            bBUKPISiteVM.udata = this.userData;
            //bBUKPISiteVM.security = this.userData.Security;
            bBUKPISiteVM.BBUKPISiteEditModel = model;

            LocalizeControls(bBUKPISiteVM, LocalizeResourceSetConstants.BBUKPIDetails);
            return PartialView("_BBUKPISiteEdit", bBUKPISiteVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BBUKPISiteEditInfo(BBUKPISiteVM siteVM)
        {
            BBUKPISiteWrapper siteWrapper = new BBUKPISiteWrapper(userData);
            BBUKPI bBUKPI = new BBUKPI();

            if (ModelState.IsValid)
            {
                bBUKPI = siteWrapper.BBUKPISiteEdit(siteVM);
                if (bBUKPI.ErrorMessages != null && bBUKPI.ErrorMessages.Count > 0)
                {
                    return Json(bBUKPI.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), bBUKPI.BBUKPIId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Submit        
        [HttpPost]
        public JsonResult BBUKPISiteSubmit(long BBUKPIId, long ClientId)
        {
            BBUKPISiteWrapper siteWrapper = new BBUKPISiteWrapper(userData);
            BBUKPI bBUKPI = new BBUKPI();

            if (ModelState.IsValid)
            {
                bBUKPI = siteWrapper.BBUKPISiteSubmit(BBUKPIId, ClientId);
                if (bBUKPI.ErrorMessages != null && bBUKPI.ErrorMessages.Count > 0)
                {
                    return Json(bBUKPI.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), bBUKPI.BBUKPIId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
