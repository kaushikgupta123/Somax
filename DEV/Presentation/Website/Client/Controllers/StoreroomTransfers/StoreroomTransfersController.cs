using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.Common;
using Rotativa;
using Client.Models.StoreroomTransfer;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Rotativa.Options;
using System.IO;
using RazorEngine;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.MultiStoreroomPart;

namespace Client.Controllers.NewLaborScheduling
{
    public class StoreroomTransfersController : BaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.StoreroomTransfer)]
        #region Search
        public ActionResult Index()
        {
            StoreroomTransferVM storeroomTransferVM = new StoreroomTransferVM();
            var commonWrapper = new CommonWrapper(userData);
            string mode = Convert.ToString(TempData["Mode"]);
            if (mode == "OutgoingTransfers")
            {
                var StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
                ViewBag.IsIncomingPage = false;
                storeroomTransferVM.StoreroomList = StoreroomList;
            }
            else
            {
                var StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.MaintainReceiveTransfer);
                storeroomTransferVM.StatusList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.StoreroomTransfer);
                ViewBag.IsIncomingPage = true;
                storeroomTransferVM.StoreroomList = StoreroomList;
            }

            LocalizeControls(storeroomTransferVM, LocalizeResourceSetConstants.StoreroomTransfers);
            return View(storeroomTransferVM);
        }
        public ActionResult IncomingTransfers()
        {
            TempData["Mode"] = "IncomingTransfers";
            return Redirect("/StoreroomTransfers/Index?page=Inventory_IncomingStoreroomTransfers");
        }
        public ActionResult OutgoingTransfers()
        {
            TempData["Mode"] = "OutgoingTransfers";
            return Redirect("/StoreroomTransfers/Index?page=Inventory_OutgoingStoreroomTransfers");
        }
        #region Incoming Transfer
        public string GetStoreroomTransferGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, long? StoreroomId = null, string Order = "1", string SearchText = "")
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var filter = CustomQueryDisplayId;
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            StoreroomTransferWrapper stWrapper = new StoreroomTransferWrapper(userData);
            List<StoreroomTransferModel> stList = stWrapper.GetStoreroomTransferChunkList(CustomQueryDisplayId, skip, length ?? 0, Order, orderDir, StoreroomId, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (stList != null && stList.Count > 0)
            {
                recordsFiltered = stList[0].TotalCount;
                totalRecords = stList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = stList
              .ToList();
            var IsMaintain = false;
            var IsReceive = false;
            if (StoreroomId!= null)
            {
                var commonWrapper = new CommonWrapper(userData);
                var StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Maintain);
                IsMaintain=StoreroomList.Where(m=>m.Value==StoreroomId.ToString()).Count()>0?true:false;
                StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.ReceiveTransfer);
                IsReceive = StoreroomList.Where(m => m.Value == StoreroomId.ToString()).Count() > 0 ? true : false;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsMaintain= IsMaintain, IsReceive= IsReceive }, JsonSerializerDateSettings);
        }
        #endregion
        #region Outgoing Transfer
        public string GetStoreroomTransferOutgoingTransferGridData(int? draw, int? start, int? length, long? StoreroomId = null, string Order = "1", string SearchText = "")
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            StoreroomTransferWrapper stWrapper = new StoreroomTransferWrapper(userData);
            List<StoreroomTransferModel> stList = stWrapper.GetStoreroomTransferOutgoingTransferChunkList(skip, length ?? 0, Order, orderDir, StoreroomId, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (stList != null && stList.Count > 0)
            {
                recordsFiltered = stList[0].TotalCount;
                totalRecords = stList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = stList
              .ToList();
            var IsIssue = false;
            if (StoreroomId != null)
            {
                var commonWrapper = new CommonWrapper(userData);
                var StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
                IsIssue = StoreroomList.Where(m => m.Value == StoreroomId.ToString()).Count() > 0 ? true : false;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsIssue= IsIssue }, JsonSerializerDateSettings);
        }
        #endregion
        #endregion

        #region Process Incoming StoreroomTransfer
        [HttpPost]
        public ActionResult ProcessIncomingStoreroomTransfers(List<StoreroomTransferProcessReceiptsModel> STData)
        {
            StoreroomTransferWrapper sanWrapper = new StoreroomTransferWrapper(userData);
            var Result = new List<string>();

            if (STData != null && STData.Count > 0)
            {
                Result = sanWrapper.ReceiptProcess(STData);
            }
            if (Result != null && Result.Count > 0)
            {
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = "success" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Process Outgoing StoreroomTransfer
        [HttpPost]
        public ActionResult ProcessOutgoingStoreroomTransfers(List<StoreroomTransferProcessIssuesModel> STData)
        {
            StoreroomTransferWrapper sanWrapper = new StoreroomTransferWrapper(userData);
            var Result = new List<string>();

            if (STData != null && STData.Count > 0)
            {
                Result = sanWrapper.IssueProcess(STData);
            }
            if (Result != null && Result.Count > 0)
            {
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = "success" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Add Transfers Request
        [HttpGet]
        public PartialViewResult AddTransferRequest(long RequestStoreroomId,string RequestStoreroomName)
        {
            StoreroomTransferVM storeroomTransferVM = new StoreroomTransferVM();
            AddTransferRequest addTransferRequest = new AddTransferRequest();
            addTransferRequest.RequestStoreroomId = RequestStoreroomId;
            addTransferRequest.RequestStoreroomName = RequestStoreroomName;
            storeroomTransferVM.addTransferRequest = addTransferRequest;
            LocalizeControls(storeroomTransferVM, LocalizeResourceSetConstants.StoreroomTransfers);
            return PartialView("~/Views/StoreroomTransfers/_AddTransferRequest.cshtml", storeroomTransferVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveAddTransferRequest(StoreroomTransferVM model)
        {
            string ModelValidationFailedMessage = string.Empty;
            var PartStoreroomIdAndStoreroomId = model.addTransferRequest.IssuePartStoreroomIdAndStoreroomId.Split('#');
            long IssueStoreroomId = Convert.ToInt64(PartStoreroomIdAndStoreroomId[1]);
            if (ModelState.IsValid && IssueStoreroomId != model.addTransferRequest.RequestStoreroomId)
            {
                StoreroomTransferWrapper SWrapper = new StoreroomTransferWrapper(userData);
                StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
                storeroomTransfer = SWrapper.savePartTransferRequest(model.addTransferRequest);
                if (storeroomTransfer.ErrorMessages != null && storeroomTransfer.ErrorMessages.Count > 0)
                {
                    return Json(storeroomTransfer.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), data = model.addTransferRequest }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetIssueStoreroomList(long PartId, long RequestStoreroomId)
        {
            string ModelValidationFailedMessage = string.Empty;
            MultiStoreroomPartWrapper MSWrapper = new MultiStoreroomPartWrapper(userData);
            var StoreroomList = MSWrapper.GetIssuingStoreroomList(PartId, RequestStoreroomId);
            return Json(new { Result = JsonReturnEnum.success.ToString(), StoreroomList = StoreroomList }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Update Transfer Request
        [HttpGet]
        public PartialViewResult UpdateTransferRequest(long StoreroomTransferId, long RequestStoreroomId, string RequestStoreroomName, string PartClientLookupId)
        {
            StoreroomTransferVM storeroomTransferVM = new StoreroomTransferVM();
            AddTransferRequest addTransferRequest = new AddTransferRequest();
            StoreroomTransferWrapper SWrapper = new StoreroomTransferWrapper(userData);
            var Retrievedata = SWrapper.StoreroomTransferRetrievebyStoreroomTransferId(StoreroomTransferId);
            addTransferRequest.RequestStoreroomId = Retrievedata.Item1.RequestStoreroomId;
            addTransferRequest.RequestPartStoreroomId = Retrievedata.Item1.RequestPTStoreroomID;
            addTransferRequest.ClientLookupId = PartClientLookupId;
            addTransferRequest.PartId = Retrievedata.Item1.IssuePartId;
            addTransferRequest.RequestQuantity = Retrievedata.Item1.RequestQuantity;
            addTransferRequest.RequestStoreroomName = Retrievedata.Item2;
            addTransferRequest.StoreroomTransferId = StoreroomTransferId;
            addTransferRequest.Reason = Retrievedata.Item1.Reason;
            addTransferRequest.IssuePartStoreroomIdAndStoreroomId = Retrievedata.Item1.IssuePTStoreroomID + "#" + Retrievedata.Item1.IssueStoreroomId;
            MultiStoreroomPartWrapper MSWrapper = new MultiStoreroomPartWrapper(userData);
            addTransferRequest.StoreroomList = MSWrapper.GetIssuingStoreroomList(addTransferRequest.PartId, addTransferRequest.RequestStoreroomId);
            storeroomTransferVM.addTransferRequest = addTransferRequest;

            LocalizeControls(storeroomTransferVM, LocalizeResourceSetConstants.StoreroomTransfers);
            return PartialView("~/Views/StoreroomTransfers/_UpdateTransferRequest.cshtml", storeroomTransferVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateTransferRequest(StoreroomTransferVM model)
        {
            string ModelValidationFailedMessage = string.Empty;
            var PartStoreroomIdAndStoreroomId = model.addTransferRequest.IssuePartStoreroomIdAndStoreroomId.Split('#');
            long IssueStoreroomId = Convert.ToInt64(PartStoreroomIdAndStoreroomId[1]);
            if (ModelState.IsValid && IssueStoreroomId != model.addTransferRequest.RequestStoreroomId)
            {
                StoreroomTransferWrapper SWrapper = new StoreroomTransferWrapper(userData);
                StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
                storeroomTransfer = SWrapper.UpdateTransferRequest(model.addTransferRequest);
                if (storeroomTransfer.ErrorMessages != null && storeroomTransfer.ErrorMessages.Count > 0)
                {
                    return Json(storeroomTransfer.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), data = model.addTransferRequest }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete Transfer Request
        public JsonResult DeleteTransferRequest(long StoreroomTransferId)
        {
            string ModelValidationFailedMessage = string.Empty;

            if (StoreroomTransferId > 0)
            {
                StoreroomTransferWrapper SWrapper = new StoreroomTransferWrapper(userData);
                StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
                storeroomTransfer = SWrapper.DeleteTransferRequest(StoreroomTransferId);
                if (storeroomTransfer.ErrorMessages != null && storeroomTransfer.ErrorMessages.Count > 0)
                {
                    return Json(storeroomTransfer.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), data = StoreroomTransferId }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Force Complete
        public JsonResult ForceCompleteTransferRequest(long StoreroomTransferId)
        {
            string ModelValidationFailedMessage = string.Empty;

            if (StoreroomTransferId > 0)
            {
                StoreroomTransferWrapper SWrapper = new StoreroomTransferWrapper(userData);
                PartHistory returnObj = SWrapper.ForceComplete(StoreroomTransferId);
                if (returnObj.ErrorMessages != null && returnObj.ErrorMessages.Count > 0)
                {
                    return Json(returnObj.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), data = StoreroomTransferId }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Outgoing Transfers Action Function  Deny
        public JsonResult DenyTransferRequest(long StoreroomTransferId)
        {
            string ModelValidationFailedMessage = string.Empty;

            if (StoreroomTransferId > 0)
            {
                StoreroomTransferWrapper SWrapper = new StoreroomTransferWrapper(userData);
                StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
                storeroomTransfer = SWrapper.DenyTransferRequest(StoreroomTransferId);
                if (storeroomTransfer.ErrorMessages != null && storeroomTransfer.ErrorMessages.Count > 0)
                {
                    return Json(storeroomTransfer.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), data = StoreroomTransferId }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
