using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.PartsTransfer;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.PartTransfer;
using Common.Constants;
using Newtonsoft.Json;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.PartTransfer
{
    public class PartTransferController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.PartTransfers)]
        public ActionResult Index()
        {
            PartTransferVM objVM = new PartTransferVM();
            PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
            var schduleWorkList = pWrapper.populateListDetails();
            if (schduleWorkList != null)
            {
                objVM.scheduleList = schduleWorkList.Select(x => new SelectListItem { Text = x.Key, Value = x.Value });
            }
            LocalizeControls(objVM, LocalizeResourceSetConstants.PartTransferDetail);
            return View(objVM);
        }
        [HttpPost]
        public string GetPartTransforGridData(int? draw, int? start, int? length, int? SearchTextDropID,
           long? PartTransferId, decimal? Quantity, string IssuePartId = "", string RequestPartId = "", string Status = "", string IssueSite_Name = "",
           string Description = "", string RequestSite_Name = "",
           string Reason = "", string LastEvent = "",
           DateTime? LastEventDate = null)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            List<string> StatusList = new List<string>();
            PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
            var pTransList = pWrapper.PartTransferList(SearchTextDropID ?? 0);
            if (pTransList != null)
            {
                pTransList = this.GetAllPartTransferSortByColumnWithOrder(colname[0], orderDir, pTransList);
                pTransList = GetPartTransferSearchResult(pTransList, PartTransferId, RequestSite_Name, RequestPartId, Quantity, Status, Description, IssueSite_Name, IssuePartId, Reason, LastEvent, LastEventDate);
            }
            if (pTransList != null && pTransList.Count > 0)
            {
                StatusList = pTransList.Select(r => r.Status).GroupBy(x => x.ToString()).Select(x => x.First()).ToList();
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = pTransList.Count();
            totalRecords = pTransList.Count();

            int initialPage = start.Value;

            var filteredResult = pTransList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, StatusList = StatusList }, JsonSerializerDateSettings);
        }
        private List<PartTransferModel> GetAllPartTransferSortByColumnWithOrder(string order, string orderDir, List<PartTransferModel> data)
        {
            List<PartTransferModel> lst = new List<PartTransferModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartTransferId).ToList() : data.OrderBy(p => p.PartTransferId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RequestSite_Name).ToList() : data.OrderBy(p => p.RequestSite_Name).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RequestPart_ClientLookupId).ToList() : data.OrderBy(p => p.RequestPart_ClientLookupId).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IssuePart_Description).ToList() : data.OrderBy(p => p.IssuePart_Description).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IssueSiteId).ToList() : data.OrderBy(p => p.IssueSiteId).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IssuePartId).ToList() : data.OrderBy(p => p.IssuePartId).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Reason).ToList() : data.OrderBy(p => p.Reason).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastEvent).ToList() : data.OrderBy(p => p.LastEvent).ToList();
                    break;
                case "10":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastEventDate).ToList() : data.OrderBy(p => p.LastEventDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartTransferId).ToList() : data.OrderBy(p => p.PartTransferId).ToList();
                    break;
            }
            return lst;
        }
        private List<PartTransferModel> GetPartTransferSearchResult(List<PartTransferModel> retList, long? PartTransferId, string RequestSite_Name, string RequestPartId, decimal? Quantity, string Status, string Description, string IssueSite_Name, string IssuePartId, string Reason, string LastEvent, DateTime? LastEventDate)
        {
            if (retList != null)
            {
                if (PartTransferId != null)
                {
                    retList = retList.Where(x => (Convert.ToString(x.PartTransferId).Contains(Convert.ToString(PartTransferId)))).ToList();
                }
                if (!string.IsNullOrEmpty(RequestSite_Name))
                {
                    RequestSite_Name = RequestSite_Name.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.RequestSite_Name) && x.RequestSite_Name.ToUpper().Contains(RequestSite_Name))).ToList();
                }
                if (!string.IsNullOrEmpty(RequestPartId))
                {
                    RequestPartId = RequestPartId.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.RequestPart_ClientLookupId) && x.RequestPart_ClientLookupId.ToUpper().Contains(RequestPartId))).ToList();
                }
                if (Quantity != null)
                {
                    retList = retList.Where(x => (Convert.ToString(x.Quantity).Contains(Convert.ToString(Quantity)))).ToList();
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    Status = Status.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.Status) && x.Status.ToUpper().Equals(Status))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.IssuePart_Description) && x.IssuePart_Description.ToUpper().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(IssueSite_Name))
                {
                    IssueSite_Name = IssueSite_Name.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.IssueSite_Name) && x.IssueSite_Name.ToUpper().Contains(IssueSite_Name))).ToList();
                }
                if (!string.IsNullOrEmpty(IssuePartId))
                {
                    IssuePartId = IssuePartId.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.IssuePart_ClientLookupId) && x.IssuePart_ClientLookupId.ToUpper().Contains(IssuePartId))).ToList();
                }
                if (!string.IsNullOrEmpty(Reason))
                {
                    Reason = Reason.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.Reason) && x.Reason.ToUpper().Contains(Reason))).ToList();
                }
                if (!string.IsNullOrEmpty(LastEvent))
                {
                    LastEvent = LastEvent.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.LastEvent) && x.LastEvent.ToUpper().Contains(LastEvent))).ToList();
                }
                if (LastEventDate != null)
                {
                    retList = retList.Where(x => (x.LastEventDate != null && x.LastEventDate.Value.Date.Equals(LastEventDate.Value.Date))).ToList();
                }
            }
            return retList;
        }

        public string GetPartTransferPrintData(string colname, string coldir, int? SearchTextDropID,
           long? PartTransferId, decimal? Quantity, string IssuePartId = "", string RequestPartId = "", string Status = "", string IssueSite_Name = "",
           string Description = "", string RequestSite_Name = "",
           string Reason = "", string LastEvent = "",
           DateTime? LastEventDate = null)
        {
            PartTransferPrintModel partTransferPrintModel;
            List<PartTransferPrintModel> partTransferPrintModelList = new List<PartTransferPrintModel>();
            PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
            var pTransList = pWrapper.PartTransferList(SearchTextDropID ?? 0);
            if (pTransList != null)
            {
                pTransList = this.GetAllPartTransferSortByColumnWithOrder(colname, coldir, pTransList);
                pTransList = GetPartTransferSearchResult(pTransList, PartTransferId, RequestSite_Name, RequestPartId, Quantity, Status, Description, IssueSite_Name, IssuePartId, Reason, LastEvent, LastEventDate);
            }
            foreach (var pTransfer in pTransList)
            {
                partTransferPrintModel = new PartTransferPrintModel();
                partTransferPrintModel.PartTransferId = pTransfer.PartTransferId;
                partTransferPrintModel.RequestSite_Name = pTransfer.RequestSite_Name;
                partTransferPrintModel.RequestPart_ClientLookupId = pTransfer.RequestPart_ClientLookupId;
                partTransferPrintModel.Quantity = pTransfer.Quantity ?? 0;
                partTransferPrintModel.Status = pTransfer.Status;
                partTransferPrintModel.RequestPart_Description = pTransfer.RequestPart_Description;
                partTransferPrintModel.IssueSite_Name = pTransfer.IssueSite_Name;
                partTransferPrintModel.IssuePart_ClientLookupId = pTransfer.IssuePart_ClientLookupId;
                partTransferPrintModel.Reason = pTransfer.Reason;
                partTransferPrintModel.LastEvent = pTransfer.LastEvent;
                partTransferPrintModel.LastEventDate = pTransfer.LastEventDate;
                partTransferPrintModelList.Add(partTransferPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = partTransferPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion

        #region Details
        public PartialViewResult GetPartTransferDetail(long PartTransferId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PartTransferVM partTransferVM = new PartTransferVM();
            PartTransferDenyModel objPartTransferDenyModel = new PartTransferDenyModel();
            PartTransferForceCompleteModel objPartForceTransferModel = new PartTransferForceCompleteModel();
            List<DataContracts.LookupList> DenyReasonList = new List<DataContracts.LookupList>();
            partTransferVM.userdata = userData;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                DenyReasonList = AllLookUps.Where(x => x.ListName == LookupListConstants.PT_TRANS_DENY).ToList();
            }
            if (DenyReasonList != null)
            {
                objPartTransferDenyModel.DenyReasonIdList = DenyReasonList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
            }
            PartTransferCancelModel objPartTransferCancelModel = new PartTransferCancelModel();
            List<DataContracts.LookupList> CancelReasonList = new List<DataContracts.LookupList>();
            if (AllLookUps != null)
            {
                CancelReasonList = AllLookUps.Where(x => x.ListName == LookupListConstants.PT_TRANS_CANCEL).ToList();
            }
            if (CancelReasonList != null)
            {
                objPartTransferCancelModel.CancelReasonList = CancelReasonList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
            }
            PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
            PartTransferModel ptModel = new PartTransferModel();
            var pDetails = pWrapper.GetPartTransferDetail(PartTransferId);
            partTransferVM.parttransfermodel = pDetails;
            // RKL - 2023-04-05 - Use the Request SiteId to retrieve the account list
            var AccountList = commonWrapper.AccountList(pDetails.RequestSiteId);
            if (AccountList != null)
            {
                pDetails.AccountList = AccountList != null ? AccountList.Select(x => new SelectListItem { Text = x.ClientLookupId + "-" + x.Name , Value = x.AccountId.ToString() }) : new SelectList(new[] { "" });
            }
            List<DataContracts.LookupList> ForceReasonList = new List<DataContracts.LookupList>();
            if (AllLookUps != null)
            {
                ForceReasonList = AllLookUps.Where(x => x.ListName == LookupListConstants.PT_TRANS_FCOMP).ToList();
            }
            if (ForceReasonList != null)
            {
                objPartForceTransferModel.ForceCompleteReasonList = ForceReasonList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
            }
            partTransferVM.partTransferForceCompleteModel = objPartForceTransferModel;
            partTransferVM.partTransferForceCompleteModel.PartTransferId = PartTransferId;
            partTransferVM.partTransferIssueModel = new PartTransferIssueModel();
            partTransferVM.partTransferIssueModel.PartTransferId = PartTransferId;
            partTransferVM.partTransferReceiveModel = new PartTransferReceiveModel();
            partTransferVM.partTransferReceiveModel.PartTransferId = PartTransferId;
            objPartTransferDenyModel.PartTransferId = PartTransferId;
            objPartTransferCancelModel.PartTransferId = PartTransferId;
            partTransferVM.partTransferCancelModel = objPartTransferCancelModel;
            partTransferVM.partTransferDenyModel = objPartTransferDenyModel;
            //partTransferVM.showBtnIssue = true;
            //partTransferVM.showBtnReceive = true;
            //partTransferVM.showBtnSend = true;
            //partTransferVM.showBtnSave = true;
            //partTransferVM.showCancelMenu = true;
            //partTransferVM.showDenyMenu = true;
            //partTransferVM.showForceCompleteMenu = true;
            //partTransferVM.showConfirmForceCompleteMenu = true;
            #region V2-862
            SetButtonSecurities(partTransferVM);
            SetMenuSecurity(partTransferVM);
            //#region Show Btn Issue
            //if (partTransferVM.parttransfermodel.IssueSiteId != userData.DatabaseKey.Personnel.SiteId)
            //{
            //    partTransferVM.showBtnIssue = false;
            //}
            //else if (partTransferVM.parttransfermodel.Status == PartTransferStatus.Complete || partTransferVM.parttransfermodel.Status == PartTransferStatus.Open || partTransferVM.parttransfermodel.Status == PartTransferStatus.InTransit || partTransferVM.parttransfermodel.Status == PartTransferStatus.ForceCompPend || partTransferVM.parttransfermodel.Status == PartTransferStatus.Canceled || partTransferVM.parttransfermodel.Status == PartTransferStatus.Denied)
            //{
            //    partTransferVM.showBtnIssue = false;
            //}
            //#endregion
            //#region Show Btn Receive
            //if (partTransferVM.parttransfermodel.RequestSiteId != userData.DatabaseKey.Personnel.SiteId)
            //{
            //    partTransferVM.showBtnReceive = false;
            //}
            //else if (partTransferVM.parttransfermodel.Status == PartTransferStatus.Complete || partTransferVM.parttransfermodel.Status == PartTransferStatus.Open || partTransferVM.parttransfermodel.Status == PartTransferStatus.Waiting || partTransferVM.parttransfermodel.Status == PartTransferStatus.ForceCompPend || partTransferVM.parttransfermodel.Status == PartTransferStatus.Canceled || partTransferVM.parttransfermodel.Status == PartTransferStatus.Denied)
            //{
            //    partTransferVM.showBtnReceive = false;
            //}
            //#endregion
            //#region Show Btn Send
            //if (partTransferVM.parttransfermodel.RequestSiteId != userData.DatabaseKey.Personnel.SiteId)
            //{
            //    partTransferVM.showBtnSend = false;
            //}
            //else if (partTransferVM.parttransfermodel.Status == PartTransferStatus.Waiting || partTransferVM.parttransfermodel.Status == PartTransferStatus.Complete || partTransferVM.parttransfermodel.Status == PartTransferStatus.InTransit || partTransferVM.parttransfermodel.Status == PartTransferStatus.ForceCompPend || partTransferVM.parttransfermodel.Status == PartTransferStatus.Canceled || partTransferVM.parttransfermodel.Status == PartTransferStatus.Denied)
            //{
            //    partTransferVM.showBtnSend = false;
            //}
            //#endregion
            //#region Show Btn Save
            //if (partTransferVM.parttransfermodel.RequestSiteId != userData.DatabaseKey.Personnel.SiteId)
            //{
            //    partTransferVM.showBtnSave = false;
            //}
            //else if (partTransferVM.parttransfermodel.Status == PartTransferStatus.Complete || partTransferVM.parttransfermodel.Status == PartTransferStatus.InTransit || partTransferVM.parttransfermodel.Status == PartTransferStatus.ForceCompPend || partTransferVM.parttransfermodel.Status == PartTransferStatus.Canceled || partTransferVM.parttransfermodel.Status == PartTransferStatus.Denied)
            //{
            //    partTransferVM.showBtnSave = false;
            //}
            //#endregion
            //#region Show Cancel Menu
            //if (partTransferVM.parttransfermodel.RequestSiteId != userData.DatabaseKey.Personnel.SiteId)
            //{
            //    partTransferVM.showCancelMenu = false;
            //}
            //else if (partTransferVM.parttransfermodel.Status == PartTransferStatus.Complete || partTransferVM.parttransfermodel.Status == PartTransferStatus.InTransit || partTransferVM.parttransfermodel.Status == PartTransferStatus.ForceCompPend)
            //{
            //    partTransferVM.showCancelMenu = false;
            //}
            //#endregion
            //#region Show Deny Menu
            //if (partTransferVM.parttransfermodel.IssueSiteId != userData.DatabaseKey.Personnel.SiteId)
            //{
            //    partTransferVM.showDenyMenu = false;
            //}
            //else if (partTransferVM.parttransfermodel.Status == PartTransferStatus.Complete || partTransferVM.parttransfermodel.Status == PartTransferStatus.Open || partTransferVM.parttransfermodel.Status == PartTransferStatus.InTransit || partTransferVM.parttransfermodel.Status == PartTransferStatus.ForceCompPend)
            //{
            //    partTransferVM.showDenyMenu = false;
            //}
            //#endregion
            //#region Show Force Complete Menu
            //if (partTransferVM.parttransfermodel.RequestSiteId != userData.DatabaseKey.Personnel.SiteId)
            //{
            //    partTransferVM.showForceCompleteMenu = false;
            //}
            //else if (partTransferVM.parttransfermodel.Status == PartTransferStatus.Complete || partTransferVM.parttransfermodel.Status == PartTransferStatus.Open || partTransferVM.parttransfermodel.Status == PartTransferStatus.ForceCompPend)
            //{
            //    partTransferVM.showForceCompleteMenu = false;
            //}
            //#endregion
            //#region Show Confirm Force Complete Menu
            //if (partTransferVM.parttransfermodel.IssueSiteId != userData.DatabaseKey.Personnel.SiteId)
            //{
            //    partTransferVM.showConfirmForceCompleteMenu = false;
            //}
            //else if (partTransferVM.parttransfermodel.Status == PartTransferStatus.Complete || partTransferVM.parttransfermodel.Status == PartTransferStatus.Open || partTransferVM.parttransfermodel.Status == PartTransferStatus.Waiting || partTransferVM.parttransfermodel.Status == PartTransferStatus.InTransit)
            //{
            //    partTransferVM.showConfirmForceCompleteMenu = false;
            //}
            //#endregion
            #endregion
            LocalizeControls(partTransferVM, LocalizeResourceSetConstants.PartTransferDetail);
            return PartialView("_PartTransferDetails", partTransferVM);
        }
        #endregion

        #region ButtonEvents
        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SavePartTransfer(PartTransferVM partTransferVM)
        {
            List<string> errMsg = new List<string>();
            if (ModelState.IsValid)
            {
                PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
                errMsg = pWrapper.SavePT(partTransferVM.parttransfermodel);
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(errMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PartTransferId = partTransferVM.parttransfermodel.PartTransferId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.PartTransferDetail);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Save

        #region Issue
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartsTransferIssue(PartTransferVM partTransferVM)
        {
            List<string> errMsg = new List<string>();
            if (ModelState.IsValid)
            {
                PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
                errMsg = pWrapper.SavePTIssue(partTransferVM.partTransferIssueModel);
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(errMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PartTransferId = partTransferVM.partTransferIssueModel.PartTransferId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.PartTransferDetail);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Issue

        #region Receive
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartsTransferReceive(PartTransferVM partTransferVM)
        {
            List<string> errMsg = new List<string>();
            if (ModelState.IsValid)
            {
                PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
                errMsg = pWrapper.SavePartReceive(partTransferVM.partTransferReceiveModel);
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(errMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PartTransferId = partTransferVM.partTransferReceiveModel.PartTransferId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.PartTransferDetail);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion receive

        #region Send
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartTransferSend(PartTransferVM partTransferVM)
        {
            List<string> errMsg = new List<string>();
            PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
            errMsg = pWrapper.SavePartSend(partTransferVM.parttransfermodel);
            if (errMsg != null && errMsg.Count > 0)
            {
                return Json(errMsg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), PartTransferId = partTransferVM.parttransfermodel.PartTransferId }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Send
        #endregion ButtonEvents

        #region ActionEvents


        #region Deny
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartsTransferDeny(PartTransferVM partTransferVM)
        {
            List<string> errMsg = new List<string>();
            if (ModelState.IsValid)
            {
                PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
                errMsg = pWrapper.SavePTDeny(partTransferVM.partTransferDenyModel);
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(errMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PartTransferId = partTransferVM.partTransferDenyModel.PartTransferId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.PartTransferDetail);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Issue

        #region ForceTransfer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ForceTransfer(PartTransferVM partTransferVM)
        {
            List<string> errMsg = new List<string>();
            if (ModelState.IsValid)
            {
                PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
                errMsg = pWrapper.SaveForceComplete(partTransferVM.partTransferForceCompleteModel);
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(errMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PartTransferId = partTransferVM.partTransferForceCompleteModel.PartTransferId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.PartTransferDetail);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion ForceTransfer

        #region Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartTransferCancel(PartTransferVM partTransferVM)
        {
            List<string> errMsg = new List<string>();
            if (ModelState.IsValid)
            {
                PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
                errMsg = pWrapper.PTCancel(partTransferVM.partTransferCancelModel);
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(errMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PartTransferId = partTransferVM.partTransferCancelModel.PartTransferId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.PartTransferDetail);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Cancel

        #region ConfirmForceComplete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ConfirmForceComplete(PartTransferVM partTransferVM)
        {
            List<string> errMsg = new List<string>();
            PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
            errMsg = pWrapper.ConfirmFC(partTransferVM.parttransfermodel);
            if (errMsg != null && errMsg.Count > 0)
            {
                return Json(errMsg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), PartTransferId = partTransferVM.parttransfermodel.PartTransferId }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion ConfirmForceComplete
        #endregion ActionEvents
        #region EventLog
        [HttpPost]
        public string GetEventLogs(int? draw, int? start, int? length, long PartTransferId, string Event = "", string CreatedBy = "", DateTime? Created = null, string Quantity = "", string Comments = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
            var events = pWrapper.GetEventLogs(PartTransferId);
            if (events != null && events.Count > 0)
            {
                events = this.GetAllEventsByColumnWithOrder(order, orderDir, events);
                events = GetEventsSrachResult(events, Event, CreatedBy, Created, Quantity, Comments);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = events.Count();
            totalRecords = events.Count();
            int initialPage = start.Value;
            var filteredResult = events
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var partSecurity = this.userData.Security.Parts.Part_Equipment_XRef;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, partSecurity = partSecurity }, JsonSerializer12HoursDateAndTimeSettings);

        }
        private List<PartTransferEventLogModel> GetAllEventsByColumnWithOrder(string order, string orderDir, List<PartTransferEventLogModel> data)
        {
            List<PartTransferEventLogModel> lst = new List<PartTransferEventLogModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Event).ToList() : data.OrderBy(p => p.Event).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreatedBy).ToList() : data.OrderBy(p => p.CreatedBy).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Created).ToList() : data.OrderBy(p => p.Created).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Comments).ToList() : data.OrderBy(p => p.Comments).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Event).ToList() : data.OrderBy(p => p.Event).ToList();
                    break;
            }

            return lst;
        }
        private List<PartTransferEventLogModel> GetEventsSrachResult(List<PartTransferEventLogModel> retList, string Event = "", string CreatedBy = "", DateTime? Created = null, string Quantity = "", string Comments = "")
        {
            if (!string.IsNullOrEmpty(Event))
            {
                retList = retList.Where(x => !string.IsNullOrWhiteSpace(x.Event) && x.Event.ToUpper().Contains(Event.ToUpper())).ToList();
            }
            if (!string.IsNullOrEmpty(CreatedBy))
            {
                retList = retList.Where(x => !string.IsNullOrWhiteSpace(x.CreatedBy) && x.CreatedBy.ToUpper().Contains(CreatedBy.ToUpper())).ToList();
            }
            if (Created != null)
            {
                retList = retList.Where(x => (x.Created != null && x.Created.Value.Date.Equals(Created.Value.Date))).ToList();
            }
            if (!string.IsNullOrEmpty(Quantity))
            {
                retList = retList.Where(x => (Convert.ToString(x.Quantity).Contains(Convert.ToString(Quantity)))).ToList();
            }
            if (!string.IsNullOrEmpty(Comments))
            {
                retList = retList.Where(x => !string.IsNullOrWhiteSpace(x.Comments) && x.Comments.ToUpper().Contains(Comments.ToUpper())).ToList();
            }
            return retList;
        }
        #endregion

        public JsonResult PrintPartTransfer(long transferNo)
        {
            using (var ms = new MemoryStream())
            {
                using (var doc = new iTextSharp.text.Document())
                {
                    using (var copy = new iTextSharp.text.pdf.PdfSmartCopy(doc, ms))
                    {
                        doc.Open();
                        var msSinglePDf = new MemoryStream(PrintGetByteStream(transferNo));
                        using (var reader = new iTextSharp.text.pdf.PdfReader(msSinglePDf))
                        {
                            copy.AddDocument(reader);
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
        [EncryptedActionParameter]
        public Byte[] PrintGetByteStream(long transferNo)
        {
            PartTransferVM objVM = new PartTransferVM();
            PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
            objVM.printPartTransferModel = new PrintPartTransferModel();
            objVM.printPartTransferModel = pWrapper.EventLogPrint(transferNo);
            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                   "--header-spacing \"3\" " +
                                   "--footer-html \"{1}\" " +
                                   "--footer-spacing \"8\" " +
                                   "--footer-font-size \"10\" " +
                                   "--header-font-size \"10\" ",
                                   Url.Action("Header", "PartTransfer", new { id = userData.LoginAuditing.SessionId, PartTransferID = objVM.printPartTransferModel.PartTransferID }, Request.Url.Scheme),
                                   Url.Action("Footer", "PartTransfer", new { id = userData.LoginAuditing.SessionId }, Request.Url.Scheme));

            LocalizeControls(objVM, LocalizeResourceSetConstants.PartTransferDetail);
            var mailpdft = new ViewAsPdf("PrintPartTransferTemplate", objVM)
            {
                PageMargins = new Margins(43, 12, 22, 12),// it’s in millimeters
                CustomSwitches = customSwitches
            };
            Byte[] PdfData = mailpdft.BuildPdf(ControllerContext);
            return PdfData;
        }
        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Header(string id, long PartTransferID)
        {
            PartTransferVM objVM = new PartTransferVM();
            if (CheckLoginSession(id))
            {
                CommonWrapper comWrapper = new CommonWrapper(userData);
                objVM.printPartTransferModel = new PrintPartTransferModel();
                objVM.printPartTransferModel.PartTransferID = PartTransferID;
                objVM.printPartTransferModel.AzureImageURL = comWrapper.GetClientLogoUrl();               
            }          
            LocalizeControls(objVM, LocalizeResourceSetConstants.PartTransferDetail);
            return View("PrintHeader", objVM);
        }

        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Footer(string id)
        {
            PartTransferVM objVM = new PartTransferVM();
            if (CheckLoginSession(id))
            {
                LocalizeControls(objVM, LocalizeResourceSetConstants.PartTransferDetail);
            }
            return View("PrintFooter", objVM);
        }

        [HttpGet]
        public string PartTransferPrintData(string colname, string coldir, long? PartTransferId, decimal? Quantity, string IssuePartId = "", string RequestPartId = "", string Status = "", string IssueSite_Name = "",
           string Description = "", string RequestSite_Name = "",
           string Reason = "", string LastEvent = "",
           DateTime? LastEventDate = null, int SearchTextDropID = 0)
        {
            List<PartTransferPrintModel> ptPrintList = new List<PartTransferPrintModel>();
            PartTransferPrintModel objPartTransferPrintModel;
            PartTransferWrapper pWrapper = new PartTransferWrapper(userData);
            var pTransList = pWrapper.PartTransferList(SearchTextDropID);
            if (pTransList != null)
            {
                pTransList = this.GetPartTransferSearchResult(pTransList, PartTransferId, RequestSite_Name, RequestPartId, Quantity, Status, Description, IssueSite_Name, IssuePartId, Reason, LastEvent, LastEventDate);
                pTransList=this.GetAllPartTransferSortByColumnWithOrder(colname, coldir, pTransList);
                foreach (var p in pTransList)
                {
                    objPartTransferPrintModel = new PartTransferPrintModel();
                    objPartTransferPrintModel.PartTransferId = p.PartTransferId;
                    objPartTransferPrintModel.RequestSite_Name = p.RequestSite_Name;
                    objPartTransferPrintModel.RequestPart_ClientLookupId = p.RequestPart_ClientLookupId;
                    objPartTransferPrintModel.Quantity = p.Quantity;
                    objPartTransferPrintModel.Status = p.Status;
                    objPartTransferPrintModel.RequestPart_Description = p.RequestPart_Description;
                    objPartTransferPrintModel.IssueSite_Name = p.IssueSite_Name;
                    objPartTransferPrintModel.IssuePart_ClientLookupId = p.IssuePart_ClientLookupId;
                    objPartTransferPrintModel.Reason = p.Reason;
                    objPartTransferPrintModel.LastEvent = p.LastEvent;
                    objPartTransferPrintModel.LastEventDate = p.LastEventDate;
                    ptPrintList.Add(objPartTransferPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = ptPrintList }, JsonSerializerDateSettings);
        }

        #region PartTransferSecurity
        private void SetButtonSecurities(PartTransferVM p)
        {
            bool showBtnSave = true;
            bool showBtnIssue = true;
            bool showBtnReceive = true;
            bool showBtnSend = true;
            //---save----
            if (!userData.Security.PartTransfers.Edit)
            {
                showBtnSave = false;
            }
            else if (userData.DatabaseKey.Personnel.SiteId != p.parttransfermodel.RequestSiteId)
            {
                showBtnSave = false;
            }
            else if (p.parttransfermodel.Status == PartTransferStatus.Complete || p.parttransfermodel.Status == PartTransferStatus.Canceled || p.parttransfermodel.Status == PartTransferStatus.Denied)
            {
                showBtnSave = false;
            }
            else
            {
                showBtnSave = true;
            }
            p.showBtnSave = showBtnSave;
            //---Issue---
            if (!userData.Security.PartTransfers.PartTransfers_Process)
            {
                showBtnIssue = false;
            }
            else if (userData.DatabaseKey.Personnel.SiteId != p.parttransfermodel.IssueSiteId)
            {
                showBtnIssue = false;
            }
            else if (p.parttransfermodel.QuantityIssued >= p.parttransfermodel.Quantity)
            {
                showBtnIssue = false;
            }
            else if (p.parttransfermodel.Status == PartTransferStatus.Open || p.parttransfermodel.Status == PartTransferStatus.Complete || p.parttransfermodel.Status == PartTransferStatus.Canceled || p.parttransfermodel.Status == PartTransferStatus.Denied)
            {
                showBtnIssue = false;
            }
            else
            {
                showBtnIssue = true;
            }
            p.showBtnIssue = showBtnIssue;
            //---Receive-----
            if (!userData.Security.PartTransfers.PartTransfers_Process)
            {
                showBtnReceive = false;
            }
            else if (userData.DatabaseKey.Personnel.SiteId != p.parttransfermodel.RequestSiteId)
            {
                showBtnReceive = false;
            }
            else if (p.parttransfermodel.QuantityReceived >= p.parttransfermodel.QuantityIssued)
            {
                showBtnReceive = false;
            }
            else if (p.parttransfermodel.Status == PartTransferStatus.Open || p.parttransfermodel.Status == PartTransferStatus.Complete || p.parttransfermodel.Status == PartTransferStatus.Canceled || p.parttransfermodel.Status == PartTransferStatus.Denied)
            {
                showBtnReceive = false;
            }
            else
            {
                showBtnReceive = true;
            }
            p.showBtnReceive = showBtnReceive;
            //---Send----
            if (!userData.Security.PartTransfers.Edit)
            {
                showBtnSend = false;
            }
            else if (userData.DatabaseKey.Personnel.SiteId != p.parttransfermodel.RequestSiteId)
            {
                showBtnSend = false;
            }
            //else if (p.parttransfermodel.QuantityReceived >= p.parttransfermodel.QuantityIssued)
            //{
            //    showBtnSend = false;
            //}
            else if (p.parttransfermodel.Status != PartTransferStatus.Open)
            {
                showBtnSend = false;
            }
            else
            {
                showBtnSend = true;
            }
            p.showBtnSend = showBtnSend;
        }
        private void SetMenuSecurity(PartTransferVM p)
        {
            //---Cancel---
            if (userData.Security.PartTransfers.Edit && p.parttransfermodel.RequestSiteId == userData.DatabaseKey.Personnel.SiteId)
            {
                if ((p.parttransfermodel.Status == PartTransferStatus.Waiting || p.parttransfermodel.Status == PartTransferStatus.Open) && p.parttransfermodel.QuantityIssued <= 0)
                {
                    p.showCancelMenu = true;
                }
            }
            //---Deny---
            if (userData.Security.PartTransfers.Edit && p.parttransfermodel.IssueSiteId == userData.DatabaseKey.Personnel.SiteId) 
            {
                if ((p.parttransfermodel.Status == PartTransferStatus.Waiting) && p.parttransfermodel.QuantityIssued <= 0)
                {
                    p.showDenyMenu = true;
                }
            }
            //---Force Complete---
            if (userData.Security.PartTransfers.Edit && p.parttransfermodel.RequestSiteId == userData.DatabaseKey.Personnel.SiteId)
            {
                if ((p.parttransfermodel.Status == PartTransferStatus.InTransit || p.parttransfermodel.Status == PartTransferStatus.Waiting) && p.parttransfermodel.QuantityIssued > 0)
                {
                    p.showForceCompleteMenu = true;
                }
            }
            //---Confirm Force Complete---
            if (userData.Security.PartTransfers.Edit && p.parttransfermodel.IssueSiteId == userData.DatabaseKey.Personnel.SiteId)
            {
                if (p.parttransfermodel.Status == PartTransferStatus.ForceCompPend)
                {
                    p.showConfirmForceCompleteMenu = true;
                }
            }
        }
        #endregion PartTransferSecurity
    }
}