using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.IoTEvent;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.IoTEvent;
using Common.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.IoTEvent
{
    public class IoTEventController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.Sensors)]
        public ActionResult Index()
        {
            IoTEventWrapper evWrapper = new IoTEventWrapper(userData);
            IoTEventVM ioTEventVM = new IoTEventVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            ioTEventVM.ioTEventModel = new IoTEventModel();
            ioTEventVM.security = this.userData.Security;

            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            var ToggleStatusList = UtilityFunction.AllIoTEventOpenProcessedStatusTypes();
            if (ToggleStatusList != null)
            {
                ioTEventVM.OpenFlagList = ToggleStatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            ioTEventVM.ioTEventModel = new IoTEventModel();
            var TypeList = commonWrapper.GetListFromConstVals(LookupListConstants.EVENT_INFO_TYPE).ToList();
            if (TypeList != null)
            {
                ioTEventVM.ioTEventModel.EventTypeList = TypeList.ToList().Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var StatusList = commonWrapper.GetListFromConstVals(LookupListConstants.EVENT_INFO_STATUS).ToList();
            if (StatusList != null)
            {
                ioTEventVM.ioTEventModel.EventStatusList = StatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var DispositionList = commonWrapper.GetListFromConstVals(LookupListConstants.EVENT_INFO_DISPOSITION).ToList();
            if (DispositionList != null)
            {
                ioTEventVM.ioTEventModel.EventDispositionList = DispositionList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            LocalizeControls(ioTEventVM, LocalizeResourceSetConstants.IoTEvent);
            return View(ioTEventVM);
        }
        #endregion

        #region ChunkSearch
        public string GetIotEventChunkSearch(int? draw, int? start, int? length, int customQueryDisplayId = 0, long IoTEventId = 0, string ioTEventSource = "", string ioTEventType = "", string ioTStatus = "", string iotDisposition = "", string iotWorkOrderId = "", string iotFaultCode = "", string assetClentLookupId = "", DateTime? iotCreateDate = null, string IoTDeviceID = "", string txtSearchval = "")
        {

            string orderbycol = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            List<string> TypeList = new List<string>();
            List<string> StatusList = new List<string>();
            List<string> DispositionList = new List<string>();

            IoTEventWrapper ioTEventWrapper = new IoTEventWrapper(userData);
            List<IoTEventModel> eventInfoModelList = ioTEventWrapper.GetIotEventInfoGridData(customQueryDisplayId, skip, length ?? 0, orderbycol, orderDir, IoTEventId, ioTEventSource, ioTEventType, ioTStatus, iotDisposition, iotWorkOrderId, iotFaultCode, assetClentLookupId, iotCreateDate, IoTDeviceID, txtSearchval);
            if (eventInfoModelList != null && eventInfoModelList.Count > 0)
            {
                TypeList = eventInfoModelList.Select(r => r.EventType).GroupBy(x => x.ToString()).Select(x => x.First()).ToList();
                StatusList = eventInfoModelList.Select(r => r.Status).GroupBy(x => x.ToString()).Select(x => x.First()).ToList();
                DispositionList = eventInfoModelList.Select(r => r.Disposition).GroupBy(x => x.ToString()).Select(x => x.First()).ToList();
            }

            var totalRecords = 0;
            var recordsFiltered = 0;
            if (eventInfoModelList != null && eventInfoModelList.Count > 0)
            {
                recordsFiltered = eventInfoModelList[0].TotalCount;
                totalRecords = eventInfoModelList[0].TotalCount;

            }

            int initialPage = start.Value;
            var filteredResult = eventInfoModelList.ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, TypeList = TypeList, StatusList = StatusList, DispositionList = DispositionList }, JsonSerializerDateSettings);
        }



        public string GetIoTEventPrintData(string colname, string coldir, int customQueryDisplayId = 0, long iotEventId = 0, string iotEventSource = "", string iotEventType = "", string assetClentLookupId = "", string iotStatus = "", string iotDisposition = "", string iotWorkOrderId = "", string iotFaultCode = "", DateTime? iotCreateDate = null, string iotDeviceID = "", string txtSearchval = "")
        {

            List<IoTEventPrintModel> IoTEventPrintModelList = new List<IoTEventPrintModel>();
            IoTEventPrintModel objIoTEventPrintModel;
            IoTEventWrapper evWrapper = new IoTEventWrapper(userData);

            List<IoTEventModel> ioteventMainMasterList = evWrapper.GetIotEventInfoGridData(customQueryDisplayId, 0, 100000, colname, coldir, iotEventId, iotEventSource, iotEventType, iotStatus, iotDisposition, iotWorkOrderId, iotFaultCode, assetClentLookupId,
                 iotCreateDate, iotDeviceID, txtSearchval);

            if (ioteventMainMasterList != null)
            {
                foreach (var items in ioteventMainMasterList)
                {
                    objIoTEventPrintModel = new IoTEventPrintModel();

                    objIoTEventPrintModel.IoTEventId = items.IoTEventId;
                    objIoTEventPrintModel.SourceType = items.SourceType;
                    objIoTEventPrintModel.EventType = items.EventType;
                    objIoTEventPrintModel.AssetID = items.AssetID;
                    objIoTEventPrintModel.AssetName = items.AssetName;
                    objIoTEventPrintModel.Status = items.Status;
                    objIoTEventPrintModel.Disposition = items.Disposition;
                    objIoTEventPrintModel.WOClientLookupId = items.WOClientLookupId;
                    objIoTEventPrintModel.FaultCode = items.FaultCode;
                    objIoTEventPrintModel.CreateDate = items.CreateDate;
                    objIoTEventPrintModel.IoTDeviceClientLookupId = items.IoTDeviceClientLookupId;
                    objIoTEventPrintModel.ProcessDate = items.ProcessDate;
                    objIoTEventPrintModel.ProcessBy_Personnel = items.ProcessBy_Personnel;
                    objIoTEventPrintModel.Comments = items.Comments;
                    IoTEventPrintModelList.Add(objIoTEventPrintModel);
                }

            }
            return JsonConvert.SerializeObject(new { data = IoTEventPrintModelList }, JsonSerializerDateSettings);
        }

        #endregion


        #region Details IoTEvent
        public PartialViewResult IoTEventDetails(long IoTEventId = 0)
        {
            IoTEventWrapper evWrapper = new IoTEventWrapper(userData);
            IoTEventVM objVM = new IoTEventVM();

            IoTEventModel obj = evWrapper.IotEventRetriveById(IoTEventId);
            objVM.ioTEventModel = obj;
            objVM.dismissModel = new IoTEventDismissModel();
            objVM.acknowledgeModel = new IoTEventAcknowledgeModel();
            objVM.dismissModel.IoTEventId = objVM.acknowledgeModel.IoTEventId = IoTEventId;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var Dismiss = AllLookUps.Where(x => x.ListName == LookupListConstants.EVENT_DISMISS).ToList();
                var Fault = AllLookUps.Where(x => x.ListName == LookupListConstants.EVENT_FAULTS).ToList();

                objVM.dismissModel.DismissReasonList = Dismiss.Select(x => new SelectListItem { Text = x.ListValue + " | " + x.Description, Value = x.ListValue });
                objVM.acknowledgeModel.FaultCodeList = Fault.Select(x => new SelectListItem { Text = x.ListValue + " | " + x.Description, Value = x.ListValue });
            }
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.IoTEvent);
            return PartialView("~/Views/IoTEvent/_IoTEventDetails.cshtml", objVM);


        }
        #endregion

        #region Modal -Popup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DismissEvent(IoTEventVM objVM)
        {
            if (ModelState.IsValid)
            {
                string IsAddOrUpdate = string.Empty;
                IoTEventWrapper evWrapper = new IoTEventWrapper(userData);
                List<string> ErrorMsg = evWrapper.DismissEvent(objVM.dismissModel);

                if (ErrorMsg != null && ErrorMsg.Count > 0)
                {
                    return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Issuccess = true }, JsonRequestBehavior.AllowGet);
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
        public JsonResult AcknowledgeEvent(IoTEventVM objVM)
        {
            if (ModelState.IsValid)
            {
                string IsAddOrUpdate = string.Empty;
                IoTEventWrapper evWrapper = new IoTEventWrapper(userData);
                List<string> ErrorMsg = evWrapper.AcknowledgeEvent(objVM.acknowledgeModel);

                if (ErrorMsg != null && ErrorMsg.Count > 0)
                {
                    return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    return Json(new { Issuccess = true }, JsonRequestBehavior.AllowGet);
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