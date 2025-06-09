using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.EventInfo;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.EventInfo;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.EventInfo
{
    public class EventInfoController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.Sensors)]
        public ActionResult Index()
        {
            EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
            EventInfoVM objVM = new EventInfoVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var schduleWorkList = evWrapper.populateListDetails();
            if (schduleWorkList != null)
            {
                objVM.scheduleList = schduleWorkList.Select(x => new SelectListItem { Text = x.Key, Value = x.Value });
            }
            objVM.security = this.userData.Security;
            objVM.eventInfoModel = new EventInfoModel();
            var TypeList = commonWrapper.GetListFromConstVals(LookupListConstants.EVENT_INFO_TYPE).ToList();
            if (TypeList != null)
            {
                objVM.eventInfoModel.EventTypeList = TypeList.ToList().Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var StatusList = commonWrapper.GetListFromConstVals(LookupListConstants.EVENT_INFO_STATUS).ToList();
            if (StatusList != null)
            {
                objVM.eventInfoModel.EventStatusList = StatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var DispositionList = commonWrapper.GetListFromConstVals(LookupListConstants.EVENT_INFO_DISPOSITION).ToList();
            if (DispositionList != null)
            {
                objVM.eventInfoModel.EventDispositionList = DispositionList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }

            LocalizeControls(objVM, LocalizeResourceSetConstants.EventInfo);
            return View(objVM);
        }

        [HttpPost]
        public string GetEventInfoGridData(int? draw, int? start, int? length, int? SearchTextDropID, long? EventInfoId, long? SensorId, string SourceType = "",
            string EventType = "", string Description = "",
            string Status = "", string Disposition = "",
            string WOClientLookupId = "", string FaultCode = "",
            DateTime? CreateDate = null, DateTime? ProcessDate = null,
            string ProcessBy_Personnel = "", string Comments = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");

            List<string> TypeList = new List<string>();
            List<string> StatusList = new List<string>();
            List<string> DispositionList = new List<string>();

            EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
            var eventInfoList = evWrapper.EventInfoList(SearchTextDropID ?? 0);

            eventInfoList = this.GetAllEventInfoSortByColumnWithOrder(colname[0], orderDir, eventInfoList);
            eventInfoList = GetEventInfoSearchResult(eventInfoList, EventInfoId, SensorId, SourceType, EventType, Description, Status, Disposition, WOClientLookupId, FaultCode, CreateDate, ProcessDate, ProcessBy_Personnel, Comments);
            if (eventInfoList != null && eventInfoList.Count > 0)
            {
                TypeList = eventInfoList.Select(r => r.EventType).GroupBy(x => x.ToString()).Select(x => x.First()).ToList();
                StatusList = eventInfoList.Select(r => r.Status).GroupBy(x => x.ToString()).Select(x => x.First()).ToList();
                DispositionList = eventInfoList.Select(r => r.Disposition).GroupBy(x => x.ToString()).Select(x => x.First()).ToList();
            }

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = eventInfoList.Count();
            totalRecords = eventInfoList.Count();

            int initialPage = start.Value;

            var filteredResult = eventInfoList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, TypeList = TypeList, StatusList = StatusList, DispositionList = DispositionList }, JsonSerializerDateSettings);
        }
        private List<EventInfoModel> GetAllEventInfoSortByColumnWithOrder(string order, string orderDir, List<EventInfoModel> data)
        {
            List<EventInfoModel> lst = new List<EventInfoModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EventInfoId).ToList() : data.OrderBy(p => p.EventInfoId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SourceType).ToList() : data.OrderBy(p => p.SourceType).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EventType).ToList() : data.OrderBy(p => p.EventType).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Disposition).ToList() : data.OrderBy(p => p.Disposition).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.WOClientLookupId).ToList() : data.OrderBy(p => p.WOClientLookupId).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FaultCode).ToList() : data.OrderBy(p => p.FaultCode).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SensorId).ToList() : data.OrderBy(p => p.SensorId).ToList();
                    break;
                case "10":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProcessDate).ToList() : data.OrderBy(p => p.ProcessDate).ToList();
                    break;
                case "11":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProcessBy_Personnel).ToList() : data.OrderBy(p => p.ProcessBy_Personnel).ToList();
                    break;
                case "12":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Comments).ToList() : data.OrderBy(p => p.Comments).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EventInfoId).ToList() : data.OrderBy(p => p.EventInfoId).ToList();
                    break;
            }
            return lst;
        }
        private List<EventInfoModel> GetEventInfoSearchResult(List<EventInfoModel> retList, long? EventInfoId, long? SensorId, string SourceType = "",
            string EventType = "", string Description = "",
            string Status = "", string Disposition = "",
            string WOClientLookupId = "", string FaultCode = "",
            DateTime? CreateDate = null, DateTime? ProcessDate = null,
            string ProcessBy_Personnel = "", string Comments = ""
            )
        {
            if (retList != null)
            {
                if (EventInfoId != null)
                {
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(Convert.ToString(x.EventInfoId)) && Convert.ToString(x.EventInfoId).ToUpper().Contains(Convert.ToString(EventInfoId)))).ToList();
                }
                if (SensorId != null)
                {
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(Convert.ToString(x.SensorId)) && Convert.ToString(x.SensorId).ToUpper().Contains(Convert.ToString(SensorId)))).ToList();
                }
                if (!string.IsNullOrEmpty(SourceType))
                {
                    SourceType = SourceType.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.SourceType) && x.SourceType.ToUpper().Contains(SourceType))).ToList();
                }
                if (!string.IsNullOrEmpty(EventType))
                {
                    EventType = EventType.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.EventType) && x.EventType.ToUpper().Equals(EventType))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    Status = Status.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.Status) && x.Status.ToUpper().Equals(Status))).ToList();
                }
                if (!string.IsNullOrEmpty(Disposition))
                {
                    Disposition = Disposition.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.Disposition) && x.Disposition.ToUpper().Equals(Disposition))).ToList();
                }
                if (!string.IsNullOrEmpty(WOClientLookupId))
                {
                    WOClientLookupId = WOClientLookupId.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.WOClientLookupId) && x.WOClientLookupId.ToUpper().Contains(WOClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(FaultCode))
                {
                    FaultCode = FaultCode.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.FaultCode) && x.FaultCode.ToUpper().Contains(FaultCode))).ToList();
                }
                if (CreateDate != null)
                {
                    retList = retList.Where(x => (x.CreateDate != null && x.CreateDate.Value.Date.Equals(CreateDate.Value.Date))).ToList();
                }
                if (ProcessDate != null)
                {
                    retList = retList.Where(x => (x.ProcessDate != null && x.ProcessDate.Value.Date.Equals(ProcessDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(ProcessBy_Personnel))
                {
                    ProcessBy_Personnel = ProcessBy_Personnel.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.ProcessBy_Personnel) && x.ProcessBy_Personnel.ToUpper().Contains(ProcessBy_Personnel))).ToList();
                }
                if (!string.IsNullOrEmpty(Comments))
                {
                    Comments = Comments.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.Comments) && x.Comments.ToUpper().Contains(Comments))).ToList();
                }
            }
            return retList;
        }
        public string GetEventInfoPrintData(int? SearchTextDropID, long? EventInfoId, long? SensorId, string SourceType = "",
           string EventType = "", string Description = "",
           string Status = "", string Disposition = "",
           string WOClientLookupId = "", string FaultCode = "",
           DateTime? CreateDate = null, DateTime? ProcessDate = null,
           string ProcessBy_Personnel = "", string Comments = "")
        {
            List<EventInfoPrintModel> eventInfoPrintModelList = new List<EventInfoPrintModel>();
            EventInfoPrintModel objEventInfoPrintModel;

            EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
            var eventInfoList = evWrapper.EventInfoList(SearchTextDropID ?? 0);

            eventInfoList = GetEventInfoSearchResult(eventInfoList, EventInfoId, SensorId, SourceType, EventType, Description, Status, Disposition, WOClientLookupId, FaultCode, CreateDate, ProcessDate, ProcessBy_Personnel, Comments);

            foreach (var ev in eventInfoList)
            {
                objEventInfoPrintModel = new EventInfoPrintModel();
                objEventInfoPrintModel.EventInfoId = ev.EventInfoId;
                objEventInfoPrintModel.SourceType = ev.SourceType;
                objEventInfoPrintModel.EventType = ev.EventType;
                objEventInfoPrintModel.Description = ev.Description;
                objEventInfoPrintModel.Status = ev.Status;
                objEventInfoPrintModel.Disposition = ev.Disposition;
                objEventInfoPrintModel.WOClientLookupId = ev.WOClientLookupId;
                objEventInfoPrintModel.FaultCode = ev.FaultCode;
                objEventInfoPrintModel.CreateDate = ev.CreateDate;
                objEventInfoPrintModel.SensorId = ev.SensorId;
                objEventInfoPrintModel.ProcessDate = ev.ProcessDate;
                objEventInfoPrintModel.ProcessBy_Personnel = ev.ProcessBy_Personnel;
                objEventInfoPrintModel.Comments = ev.Comments;
                eventInfoPrintModelList.Add(objEventInfoPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = eventInfoPrintModelList }, JsonSerializerDateSettings);
        }

        #endregion

        #region OpenEventInfoCount
        public JsonResult GetOpenEventInfoCount()
        {
            EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
            int openCount = evWrapper.GetEventInfoCountByStatus(EventStatusConstants.Open);
            return Json(openCount, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Details Event-Info
        public PartialViewResult EventInfoDetails(long EventId = 0)
        {
            EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
            EventInfoVM objVM = new EventInfoVM();

            EventInfoModel obj = evWrapper.EventRetriveById(EventId);

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> FaultListlist = commonWrapper.GetAllLookUpList().Where(x => x.ListName == "EVENT_FAULTS").ToList();

            if (FaultListlist != null && FaultListlist.Count > 0)
            {
                obj.FaultCode = FaultListlist.Where(x => x.ListValue == obj.FaultCode).Select(x => x.ListValue + " - " + x.Description).FirstOrDefault();
            }
            objVM.eventInfoModel = obj;

            objVM.dismissModel = new DismissModel();
            objVM.acknowledgeModel = new AcknowledgeModel();
            objVM.dismissModel.EventInfoId = objVM.acknowledgeModel.EventInfoId = EventId;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var Dismiss = AllLookUps.Where(x => x.ListName == "EVENT_DISMISS").ToList();
                var Fault = AllLookUps.Where(x => x.ListName == "EVENT_FAULTS").ToList();

                objVM.dismissModel.DismissReasonList = Dismiss.Select(x => new SelectListItem { Text = x.ListValue + " | " + x.Description, Value = x.ListValue });
                objVM.acknowledgeModel.FaultCodeList = Fault.Select(x => new SelectListItem { Text = x.ListValue + " | " + x.Description, Value = x.ListValue });
            }
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.EventInfo);
            return PartialView("~/Views/EventInfo/_EventInfoDetails.cshtml", objVM);
        }
        #endregion

        #region Modal -Popup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DismissEvent(EventInfoVM objVM)
        {
            if (ModelState.IsValid)
            {
                string IsAddOrUpdate = string.Empty;
                EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
                string Type = string.Empty;
                List<string> ErrorMsg = evWrapper.DismissEvent(objVM.dismissModel, ref Type);
                if (ErrorMsg != null && ErrorMsg.Count > 0)
                {
                    return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Issuccess = true, Type = Type, Comments = objVM.dismissModel.Comments }, JsonRequestBehavior.AllowGet);
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
        [ValidateAntiForgeryToken]
        public JsonResult AcknowledgeEvent(EventInfoVM objVM)
        {
            if (ModelState.IsValid)
            {
                string IsAddOrUpdate = string.Empty;
                EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
                string Type = string.Empty;
                List<string> ErrorMsg = evWrapper.AcknowledgeEvent(objVM.acknowledgeModel, ref Type);
                if (ErrorMsg != null && ErrorMsg.Count > 0)
                {
                    return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Issuccess = true, DropdownValue = objVM.acknowledgeModel.FaultCode, Type = Type, Comments = objVM.acknowledgeModel.Comments }, JsonRequestBehavior.AllowGet);
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

        #region Event Based Work Order
        [HttpPost]
        public PartialViewResult AddWoOnDemand(long eventInfoId)
        {
            EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            EventInfoVM objVM = new EventInfoVM();
            EventOnDemandModel objEventOnDemandModel = new EventOnDemandModel();
            objVM.eventOnDemandModel = new EventOnDemandModel();

            objEventOnDemandModel.EventInfoId = eventInfoId;
            var OnDemand = evWrapper.GetOndemandList();

            objEventOnDemandModel.OnDemandIDList = OnDemand.Select(x => new SelectListItem { Text = x.ClientLookUpId + "   |   " + x.Description, Value = x.ClientLookUpId.ToString() });

            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();

                    objEventOnDemandModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }
            }

            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();

            objEventOnDemandModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).Where(x => x.Text != "Location");

            var chargetToLookUplist = PopulatelookUpListByType("Equipment");
            if (chargetToLookUplist != null)
            {
                objEventOnDemandModel.ChargeToList = chargetToLookUplist.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
            }

            var PersonnelLookUplist = GetList_Personnel();
            if (PersonnelLookUplist != null)
            {
                objEventOnDemandModel.PersonnelIdList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }

            objVM.eventOnDemandModel = objEventOnDemandModel;

            LocalizeControls(objVM, LocalizeResourceSetConstants.EventInfo);
            return PartialView("~/Views/EventInfo/_AddEventDemand.cshtml", objVM);
        }

        [HttpPost]
        public ActionResult SaveWoOnDemand(EventOnDemandModel eventOnDemandModel)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                EventInfoVM objVM = new EventInfoVM();
                EventOnDemandModel objEventOnDemandModel = new EventOnDemandModel();
                EventInfoWrapper evWrapper = new EventInfoWrapper(userData);

                var errMsg = evWrapper.CreateforEventInfo(eventOnDemandModel);

                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), eventInfoId = eventOnDemandModel.EventInfoId }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion Event Based Work Order

        #region EventInfoDescribe
        public PartialViewResult AddEventInfoDescribe(long EventInfoId)
        {
            EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
            EventInfoVM objVM = new EventInfoVM();
            EventDescribeModel objEventDescribeModel = new EventDescribeModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            objEventDescribeModel.EventInfoId = EventInfoId;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objEventDescribeModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }
            }
            var chargetToLookUplist = PopulatelookUpListByType("Equipment");
            if (chargetToLookUplist != null)
            {
                objEventDescribeModel.ChargeToList = chargetToLookUplist.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " | " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
            }

            var PersonnelLookUplist = GetList_Personnel();
            if (PersonnelLookUplist != null)
            {
                objEventDescribeModel.PersonnelIdList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " | " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            objVM.eventDescribeModel = objEventDescribeModel;
            LocalizeControls(objVM, LocalizeResourceSetConstants.EventInfo);
            return PartialView("~/Views/EventInfo/_AddEventDescribe.cshtml", objVM);
        }
        public ActionResult SetChargeToLookup(string chargeType = "")
        {
            var chargetToLookUplist = PopulatelookUpListByType(chargeType);
            var jsonResult = Json(new { data = chargetToLookUplist }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEventDescribe(EventInfoVM objEventInfoVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsgList = new List<string>();
                EventInfoVM objVM = new EventInfoVM();
                EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
                objVM.eventDescribeModel = new EventDescribeModel();
                WorkOrder objWorkOrder = evWrapper.Event_Describe(objEventInfoVM.eventDescribeModel);
                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), EventInfoId = objEventInfoVM.eventDescribeModel.EventInfoId }, JsonRequestBehavior.AllowGet);
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
