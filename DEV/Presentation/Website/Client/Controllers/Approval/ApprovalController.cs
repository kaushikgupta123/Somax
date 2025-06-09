using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Configuration.ClientSetUp;
using Client.BusinessWrapper.Work_Order;
using Client.Common;
using Client.Controllers.Common;
using Client.Localization;
using Client.Models;
using Client.Models.Approval;
using Client.Models.PurchaseRequest;
using Client.Models.Work_Order;
using Client.Models.Work_Order.UIConfiguration;
using Client.Models.WorkOrder;
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
    public class ApprovalController : SomaxBaseController
    {
        // GET: Approval
        #region Common
        [CheckUserSecurity(securityType = SecurityConstants.Approval)]
        public ActionResult Index()
        {
            ApprovalVM approvalVM = new ApprovalVM();

            SetSecurityCheck(ref approvalVM);
            SetTabCount(ref approvalVM);
            SetDropdownData(ref approvalVM);
          
            LocalizeControls(approvalVM, LocalizeResourceSetConstants.Global);
            return View("~/Views/Approval/Index.cshtml", approvalVM);
        }
        private void SetDropdownData(ref ApprovalVM approvalVM)
        {
            var FilterTypeListForPRList = new List<DropDownModel>();
            var FilterTypeListForWRList = new List<DropDownModel>();
            var FilterTypeListForMRList = new List<DropDownModel>();

            //Task[] tasks = new Task[3];
            //tasks[0] = Task.Factory.StartNew(() => FilterTypeListForPRList = UtilityFunction.FilterTypeListForPR());
            //tasks[1] = Task.Factory.StartNew(() => FilterTypeListForWRList = UtilityFunction.FilterTypeListForWR());
            //tasks[2] = Task.Factory.StartNew(() => FilterTypeListForMRList = UtilityFunction.FilterTypeListForMR());
            //Task.WaitAll(tasks);

            FilterTypeListForPRList = UtilityFunction.FilterTypeListForPR();
            FilterTypeListForWRList = UtilityFunction.FilterTypeListForWR();
            FilterTypeListForMRList = UtilityFunction.FilterTypeListForMR();

            if (FilterTypeListForPRList != null && FilterTypeListForPRList.Count() > 0)
            {
                approvalVM.FilterTypePRList = FilterTypeListForPRList.ToList().Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            }
            if (FilterTypeListForWRList != null && FilterTypeListForWRList.Count() > 0)
            {
                approvalVM.FilterTypeWRList = FilterTypeListForWRList.ToList().Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            }
            if (FilterTypeListForMRList != null && FilterTypeListForMRList.Count() > 0)
            {
                approvalVM.FilterTypeMRList = FilterTypeListForMRList.ToList().Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            }
        }
        private void SetSecurityCheck(ref ApprovalVM approvalVM)
        {
            var AGS_PR = userData.DatabaseKey.ApprovalGroupSettings.PurchaseRequests;
            var AGS_MR = userData.DatabaseKey.ApprovalGroupSettings.MaterialRequests;
            var AGS_WR = userData.DatabaseKey.ApprovalGroupSettings.WorkRequests;

            approvalVM.isMaterialRequestApproval = (AGS_MR == true && userData.Security.MaterialRequest_Approve.Access == true) ? true:false;
            approvalVM.isPurchaseRequestApproval = (AGS_PR == true && userData.Security.PurchaseRequest.Approve == true) ? true : false;
            approvalVM.isWorkRequestApproval = (AGS_WR == true && userData.Security.WorkOrders.Approve == true) ? true : false;
        }
        private void SetTabCount(ref ApprovalVM approvalVM)
        {
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            List<PRApprovalSearchModel> PRlist = new List<PRApprovalSearchModel>();
            List<Models.Approval.ApprovalRouteModel> WRList = new List<Models.Approval.ApprovalRouteModel>();
            List<MaterialRequestModel> MRList = new List<MaterialRequestModel>();
            Task[] tasks = new Task[3];
            var ApporverId = this.userData.DatabaseKey.Personnel.PersonnelId;
            approvalVM.ApproverId = ApporverId;
            tasks[0] = Task.Factory.StartNew(() => PRlist = approvalWrapper.GetApprovalRoutePRGriddata(ApporverId, "AssignedToMe", "0", "asc", 0, 1));
            tasks[1] = Task.Factory.StartNew(() => WRList = approvalWrapper.GetApprovalRouteWRGriddata(ApporverId, "AssignedToMe", "0", "asc", 0, 1));
            tasks[2] = Task.Factory.StartNew(() => MRList = approvalWrapper.GetMaterialRequestList("0", 1 , "asc", 0, "AssignedToMe"));

            Task.WaitAll(tasks);

            if (PRlist != null && PRlist.Count() > 0)
            {
                approvalVM.NumberOfPurchaseRequests = PRlist.FirstOrDefault().TotalCount;
            }
            if (WRList != null && WRList.Count() > 0)
            {
                approvalVM.NumberOfWorkRequests = WRList.FirstOrDefault().TotalCount;
            }
            if (MRList != null && MRList.Count() > 0)
            {
                approvalVM.NumberOfMaterialRequests = MRList.FirstOrDefault().TotalCount;
            }           
        }
        #endregion

        #region Purchase Request
        [HttpPost]
        public PartialViewResult LoadPurchaseRequest()
        {
            ApprovalVM approvalVM = new ApprovalVM();

            LocalizeControls(approvalVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/Approval/_PurchaseRequest.cshtml", approvalVM);
        }
        [HttpPost]
        public string GetPRApprovalGridData(long ApproverId, string FilterType, int? draw, int? start, int? length)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            var PurchaseRequestList = approvalWrapper.GetApprovalRoutePRGriddata(ApproverId, FilterType, order, orderDir, skip, length ?? 0);

            var recordsFiltered = 0;
            var totalRecords = 0;

            start = start.HasValue
                ? start / length
                : 0;

            recordsFiltered = PurchaseRequestList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = PurchaseRequestList.Select(o => o.TotalCount).FirstOrDefault();

            var filteredResult = PurchaseRequestList.ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult });
        }

        public string GetApprovalRoutePRPrintData(string colorder, string coldir, int? start, int? length, long approverId, string filterType)
        {
            List<ApprovalRoutePRPrintModel> filteredResult = new List<ApprovalRoutePRPrintModel>();
            ApprovalRoutePRPrintModel objPRPrintModel;

            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            var personnelid = this.userData.DatabaseKey.Personnel.PersonnelId;
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            length = 10000;
            List<PRApprovalSearchModel> list = approvalWrapper.GetApprovalRoutePRGriddata(approverId, filterType, colorder, coldir, skip, length ?? 0);

            if (list != null)
            {
                foreach (var item in list)
                {
                    objPRPrintModel = new ApprovalRoutePRPrintModel();
                    objPRPrintModel.ClientLookupId = item.ClientLookupId;
                    objPRPrintModel.VendorName = item.VendorName;
                    objPRPrintModel.Date = item.Date;
                    objPRPrintModel.Comments = item.Comments;
                    filteredResult.Add(objPRPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = filteredResult }, JsonSerializerDateSettings);
        }


        public PartialViewResult PurchaseRequestViewDetails(long PurchaseRequestId)
        {
            ApprovalVM approvalVM = new ApprovalVM();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            approvalVM.ViewPurchaseRequest = new Models.PurchaseRequest.UIConfiguration.ViewPurchaseRequestModelDynamic();
            approvalVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPurchaseRequestWidget, userData);
            var purchaseRequestDetailsDynamic = pWrapper.GetPurchaseRequestDetailsByIdDynamic(PurchaseRequestId);
            approvalVM.ViewPurchaseRequest = purchaseRequestDetailsDynamic;
            approvalVM.udata = userData;
            LocalizeControls(approvalVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("~/Views/Approval/PurchaseRequest/_PurchaseRequestViewDetails.cshtml", approvalVM);
        }
        public string PopulateLineItem(int? draw, int? start, int? length, long PurchaseRequestId = 0)
        {
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var PRId = PurchaseRequestId;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var LineItems = pWrapper.PopulateLineitems(PRId);
            LineItems = this.GetLineItemsByColumnWithOrder(order, orderDir, LineItems);
            
          
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = LineItems.Count();
            totalRecords = LineItems.Count();

            int initialPage = start.Value;
            var filteredResult = LineItems
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            #region uiconfig

            CommonWrapper cWrapper = new CommonWrapper(userData);
            var hiddenList = cWrapper.GetHiddenList(UiConfigConstants.PurchaseRequestLineItemSearch).Select(x => x.ColumnName).ToList();

            #endregion
            var IsShoppingCart = userData.Site.ShoppingCart;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, hiddenColumnList = hiddenList, IsShoppingCart = IsShoppingCart }, JsonSerializerDateSettings);
        }

        private List<LineItemModel> GetLineItemsByColumnWithOrder(string order, string orderDir, List<LineItemModel> data)
        {
            List<LineItemModel> lst = new List<LineItemModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LineNumber).ToList() : data.OrderBy(p => p.LineNumber).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OrderQuantity).ToList() : data.OrderBy(p => p.OrderQuantity).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitofMeasure).ToList() : data.OrderBy(p => p.UnitofMeasure).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Account_ClientLookupId).ToList() : data.OrderBy(p => p.Account_ClientLookupId).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RequiredDate).ToList() : data.OrderBy(p => p.RequiredDate).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToClientLookupId).ToList() : data.OrderBy(p => p.ChargeToClientLookupId).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        #region Multilevel Approve and Deny
        [HttpPost]
        public JsonResult MultiLevelApprovePR(long PurchaseRequestId, long ApprovalGroupId, string ClientLookupId)
        {
            var dataModels = Get_MultiLevelApproverList(this.userData.DatabaseKey.Personnel.PersonnelId, ApprovalGroupRequestTypes.PurchaseRequest, PurchaseRequestId, ApprovalGroupId);
            var approvers = new List<SelectListItem>();
            if (dataModels.Count > 0)
            {
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.ApproverName,
                    Value = x.ApproverId.ToString()
                }).ToList();
            }
            return Json(new { data = JsonReturnEnum.success.ToString(), ApproverList = approvers }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult SendPRForMultiLevelApproval(List<SelectListItem> Approvers, long PurchaseRequestId, long ApprovalGroupId)
        {
            ApprovalVM approvalVM = new ApprovalVM();

            MultiLevelApproveApprovalRouteModel multiLevelApproveApprovalRoute = new MultiLevelApproveApprovalRouteModel();

            multiLevelApproveApprovalRoute.ApproverList = Approvers;
            multiLevelApproveApprovalRoute.ApproverCount = Approvers.Count;
            multiLevelApproveApprovalRoute.ObjectId = PurchaseRequestId;
            multiLevelApproveApprovalRoute.ApprovalGroupId = ApprovalGroupId;

            approvalVM.multiLevelApproveApprovalRouteModel = multiLevelApproveApprovalRoute;
            LocalizeControls(approvalVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("~/Views/Approval/PurchaseRequest/_SendPurchaseRequestForMultiLevelApproval.cshtml", approvalVM);
            //return approvalRouteModel;
        }
        [HttpPost]
        public JsonResult SendPRForMultiLevelApprovalSave(ApprovalVM approvalVM)
        {
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            if (ModelState.IsValid)
            {
                string PurchaseRequest = ApprovalGroupRequestTypes.PurchaseRequest;
                approvalVM.multiLevelApproveApprovalRouteModel.RequestType = PurchaseRequest;
                var objPurchaseRequest = approvalWrapper.MultiLevelApprovePR(approvalVM.multiLevelApproveApprovalRouteModel);
                if (objPurchaseRequest.ErrorMessages != null && objPurchaseRequest.ErrorMessages.Count > 0)
                {
                    return Json(objPurchaseRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), PurchaseRequestId = approvalVM.multiLevelApproveApprovalRouteModel.ObjectId, ApprovalGroupId = approvalVM.multiLevelApproveApprovalRouteModel.ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult MultiLevelFinalApproveForPurchaseRequest(long PurchaseRequestId, long ApprovalGroupId)
        {
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            if (ModelState.IsValid)
            {
                var objPurchaseRequest = approvalWrapper.MultiLevelFinalApprovePR(PurchaseRequestId, ApprovalGroupId);
                if (objPurchaseRequest.ErrorMessages != null && objPurchaseRequest.ErrorMessages.Count > 0)
                {
                    return Json(objPurchaseRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PurchaseRequestId = PurchaseRequestId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult MultiLevelDenyPR(long PurchaseRequestId, long ApprovalGroupId, string ClientLookupId)
        {
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            approvalWrapper.MultiLevelDenyPR(PurchaseRequestId, ApprovalGroupId);

            if (approvalWrapper.errorMessage != null && approvalWrapper.errorMessage.Count > 0)
            {
                return Json(new { data = approvalWrapper.errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ReturnPRToRequestor(long PurchaseRequestId, long ApprovalGroupId, string Comments,string ClientLookupId)
        {
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            //string errormessage = string.Empty;
            approvalWrapper.ReturnToRequestorPR(PurchaseRequestId, ApprovalGroupId,Comments);

            if (approvalWrapper.errorMessage != null && approvalWrapper.errorMessage.Count > 0)
            {
                return Json(new { data = approvalWrapper.errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #endregion

        #region Work Request
        [HttpPost]
        public PartialViewResult LoadWorkRequest()
        {
            ApprovalVM approvalVM = new ApprovalVM();
            LocalizeControls(approvalVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/Approval/_WorkRequest.cshtml", approvalVM);
        }
        [HttpPost]
        public string GetApprovalRouteWRListGrid(int? draw, int? start, int? length, long approverId, string FilterType)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            var personnelid = this.userData.DatabaseKey.Personnel.PersonnelId;
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            List<Models.Approval.ApprovalRouteModel> WorkRequestList = approvalWrapper.GetApprovalRouteWRGriddata(approverId, FilterType, order, orderDir, skip, length ?? 0);

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = WorkRequestList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = WorkRequestList.Select(o => o.TotalCount).FirstOrDefault();

            var filteredResult = WorkRequestList.ToList();


            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);

        }

        public string GetApprovalRouteWRPrintData(string colorder,string coldir,int? start, int? length, long approverId, string filterType)
        {
            List<ApprovalRouteWRPrintModel> filteredResult = new List<ApprovalRouteWRPrintModel>();
            ApprovalRouteWRPrintModel objWRPrintModel;
           
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            var personnelid = this.userData.DatabaseKey.Personnel.PersonnelId;
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            length = 10000;
            List<Models.Approval.ApprovalRouteModel> list = approvalWrapper.GetApprovalRouteWRGriddata(approverId, filterType, colorder, coldir, skip, length??0);

            if (list != null)
            {
                foreach (var item in list)
                {
                    objWRPrintModel = new ApprovalRouteWRPrintModel();
                    objWRPrintModel.ClientLookupId = item.ClientLookupId;
                    objWRPrintModel.ChargeTo = item.ChargeTo;
                    objWRPrintModel.ChargeToName = item.ChargeToName;
                    objWRPrintModel.Description = item.Description;
                    objWRPrintModel.Type = item.Type;
                    objWRPrintModel.Date = item.Date;
                    objWRPrintModel.Comments = item.Comments;
                    filteredResult.Add(objWRPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = filteredResult }, JsonSerializerDateSettings);
        }

        [HttpPost]
        public PartialViewResult WorkRequestDetailsView(long workOrderId)
        {
            ApprovalVM approvalVM = new ApprovalVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WorkOrder workOrder = new WorkOrder();
            WorkOrderUDF workOrderUDF = new WorkOrderUDF();
            ClientSetUpWrapper clientSetUpWrapper = new ClientSetUpWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataModel> Account = new List<DataModel>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            approvalVM.workOrderModel = new WorkOrderModel();
            approvalVM.ViewWorkOrderModel = new ViewWorkOrderModelDynamic();
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WoSendForApprovalModel woSendForApprovalModel = new WoSendForApprovalModel();
            approvalVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                    .Retrieve(DataDictionaryViewNameConstant.ViewWorkOrderWidget, userData);
            approvalVM.ViewWorkOrderModel = new ViewWorkOrderModelDynamic();
            Task t1 = Task.Factory.StartNew(() => workOrder = woWrapper.getWorkOderDetailsById_V2(workOrderId));
            Task t2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            Task t3 = Task.Factory.StartNew(() => Account = woWrapper.GetLookupList_Account());
            Task t4 = Task.Factory.StartNew(() => workOrderUDF = woWrapper.getWorkOrderUDFByWorkOrderId(workOrderId));
            Task.WaitAll(t1, t2, t3, t4);
            approvalVM.workOrderModel = woWrapper.initializeControls(workOrder);
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null && Type.Any(cus => cus.ListValue == approvalVM.workOrderModel.Type))
                {
                    approvalVM.workOrderModel.Type = Type.Where(x => x.ListValue == approvalVM.workOrderModel.Type).Select(x => x.Description).First();
                }
                var Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (Shift != null && Shift.Any(cus => cus.ListValue == approvalVM.workOrderModel.Shift))
                {
                    approvalVM.workOrderModel.Shift = Shift.Where(x => x.ListValue == approvalVM.workOrderModel.Shift).Select(x => x.Description).First();
                }
                var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
                if (Priority != null && Priority.Any(cus => cus.ListValue == approvalVM.workOrderModel.Priority))
                {
                    approvalVM.workOrderModel.Priority = Priority.Where(x => x.ListValue == approvalVM.workOrderModel.Priority).Select(x => x.Description).First();
                }
                var Failure = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_FAILURE).ToList();
                if (Failure != null && Failure.Any(cus => cus.ListValue == approvalVM.workOrderModel.FailureCode))
                {
                    approvalVM.workOrderModel.FailureCode = Failure.Where(x => x.ListValue == approvalVM.workOrderModel.FailureCode).Select(x => x.Description).First();
                }
                var CancelLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_Task_Cancel).ToList();
                if (CancelLookUpList != null)
                {
                    approvalVM.woTaskModel = new WoTaskModel();
                    approvalVM.woTaskModel.CancelReasonList = CancelLookUpList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                }
                var CancelLookUpListWo = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
                if (CancelLookUpListWo != null)
                {
                    approvalVM.CancelReasonListWo = CancelLookUpListWo.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                }

                if (!string.IsNullOrEmpty(workOrder.ReasonNotDone))
                {
                    var ReasonNotDone = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_NOT_DONE).ToList();
                    if (ReasonNotDone != null && ReasonNotDone.Any(cus => cus.ListValue == workOrder.ReasonNotDone))
                    {
                        workOrder.ReasonNotDone = ReasonNotDone.Where(x => x.ListValue == workOrder.ReasonNotDone).Select(x => x.Description).FirstOrDefault();
                    }
                }
                if (!string.IsNullOrEmpty(workOrder.ActionCode))
                {
                    var ActionCode = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_ACTION).ToList();
                    if (ActionCode != null && ActionCode.Any(cus => cus.ListValue == workOrder.ActionCode))
                    {
                        workOrder.ActionCode = ActionCode.Where(x => x.ListValue == workOrder.ActionCode).Select(x => x.Description).FirstOrDefault();
                    }
                }
                if (!string.IsNullOrEmpty(workOrder.ReasonforDown))
                {
                    var ReasonforDown = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_DOWN_REASON).ToList();
                    if (ReasonforDown != null && ReasonforDown.Any(cus => cus.ListValue == workOrder.ReasonforDown))
                    {
                        workOrder.ReasonforDown = ReasonforDown.Where(x => x.ListValue == workOrder.ReasonforDown).Select(x => x.Description).FirstOrDefault();
                    }
                }
                if (!string.IsNullOrEmpty(workOrder.RootCauseCode))
                {
                    var RootCauseCode = AllLookUps.Where(x => x.ListName == LookupListConstants.RootCause).ToList();
                    if (RootCauseCode != null && RootCauseCode.Any(cus => cus.ListValue == workOrder.RootCauseCode))
                    {
                        workOrder.RootCauseCode = RootCauseCode.Where(x => x.ListValue == workOrder.RootCauseCode).Select(x => x.Description).FirstOrDefault();
                    }
                }

            }
            if (Account != null && Account.Any(cus => cus.AccountId == approvalVM.workOrderModel.Labor_AccountId))
            {
                approvalVM.workOrderModel.strLabor_AccountId = Account.Where(x => x.AccountId == approvalVM.workOrderModel.Labor_AccountId).Select(x => x.Account).First();
            }
            var totalList = woWrapper.WOSchedulePersonnelList();
            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            var PersonnelLookUplist = Get_PersonnelList(SecurityConstants.WorkOrder_Approve, "ItemAccess");
            if (PersonnelLookUplist != null)
            {
                woSendForApprovalModel.Personnellist = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            woSendForApprovalModel.WorkOrderId = workOrderId;
            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
            if (ScheduleChargeTypeList != null && ScheduleChargeTypeList.Any(cus => cus.value == approvalVM.workOrderModel.ChargeType))
            {
                approvalVM.workOrderModel.ChargeType = ScheduleChargeTypeList.Where(x => x.value == approvalVM.workOrderModel.ChargeType).Select(x => x.text).First();
            }
            var ChargeTypeLookUpList = PopulatelookUpListByType(approvalVM.workOrderModel.ChargeType);
            if (ChargeTypeLookUpList != null && ChargeTypeLookUpList.Any(cus => cus.ChargeToId == approvalVM.workOrderModel.ChargeToId))
            {
                approvalVM.workOrderModel.ChargeToClientLookupId = ChargeTypeLookUpList.Where(x => x.ChargeToId == approvalVM.workOrderModel.ChargeToId).Select(co => co.ChargeToClientLookupId).First();
                approvalVM.workOrderModel.ChargeTo_Name = ChargeTypeLookUpList.Where(x => x.ChargeToId == approvalVM.workOrderModel.ChargeToId).Select(co => co.Name).First();
            }
            approvalVM.ViewWorkOrderModel = woWrapper.MapWorkOrderDataForView(approvalVM.ViewWorkOrderModel, workOrder);
            approvalVM.ViewWorkOrderModel = woWrapper.MapWorkOrderUDFDataForView(approvalVM.ViewWorkOrderModel, workOrderUDF);


            approvalVM.ViewWorkOrderModel.Type = approvalVM.workOrderModel.Type;
            approvalVM.ViewWorkOrderModel.Shift = approvalVM.workOrderModel.Shift;
            approvalVM.ViewWorkOrderModel.Priority = approvalVM.workOrderModel.Priority;
            approvalVM.ViewWorkOrderModel.ChargeTo_Name = approvalVM.workOrderModel.ChargeTo_Name;
            approvalVM.ViewWorkOrderModel.FailureCode = approvalVM.workOrderModel.FailureCode;
            //approvalVM.ViewWorkOrderModel.ReasonNotDone = approvalVM.ReasonNotDone;
            LocalizeControls(approvalVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/Approval/WorkRequest/_WorkRequestDetails.cshtml", approvalVM);
        }

        #region Multilevel Approve and Deny
        [HttpPost]
        public JsonResult MultiLevelApproveWR(long WorkRequestId, long ApprovalGroupId, string ClientLookupId)
        {
            var dataModels = Get_MultiLevelApproverList(this.userData.DatabaseKey.Personnel.PersonnelId, ApprovalGroupRequestTypes.WorkRequest, WorkRequestId, ApprovalGroupId);
            var approvers = new List<SelectListItem>();
            if (dataModels.Count > 0)
            {
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.ApproverName,
                    Value = x.ApproverId.ToString()
                }).ToList();
            }
            return Json(new { data = JsonReturnEnum.success.ToString(), ApproverList = approvers }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult SendWRForMultiLevelApproval(List<SelectListItem> Approvers, long WorkOrderId, long ApprovalGroupId)
        {
            ApprovalVM approvalVM = new ApprovalVM();

            MultiLevelApproveApprovalRouteModel multiLevelApproveApprovalRoute = new MultiLevelApproveApprovalRouteModel();

            multiLevelApproveApprovalRoute.ApproverList = Approvers;
            multiLevelApproveApprovalRoute.ApproverCount = Approvers.Count;
            multiLevelApproveApprovalRoute.ObjectId = WorkOrderId;
            multiLevelApproveApprovalRoute.ApprovalGroupId = ApprovalGroupId;

            approvalVM.multiLevelApproveApprovalRouteModel = multiLevelApproveApprovalRoute;
            LocalizeControls(approvalVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Approval/WorkRequest/_SendWorkOrderForMultiLevelApproval.cshtml", approvalVM);
        }
        [HttpPost]
        public JsonResult SendWRForMultiLevelApprovalSave(ApprovalVM wrVM)
        {
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            if (ModelState.IsValid)
            {
                string WorkRequest = ApprovalGroupRequestTypes.WorkRequest;
                wrVM.multiLevelApproveApprovalRouteModel.RequestType = WorkRequest;
                var objWorkOrder = approvalWrapper.MultiLevelApproveWR(wrVM.multiLevelApproveApprovalRouteModel);
                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderId = wrVM.multiLevelApproveApprovalRouteModel.ObjectId, ApprovalGroupId = wrVM.multiLevelApproveApprovalRouteModel.ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult MultiLevelFinalApproveWorkRequest(long WorkOrderId, long ApprovalGroupId)
        {
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            if (ModelState.IsValid)
            {
                var objWorkOrder = approvalWrapper.MultiLevelFinalApproveWR(WorkOrderId, ApprovalGroupId);
                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderId = WorkOrderId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult MultiLevelDenyWOJob(long WorkorderId, long ApprovalGroupId, string ClientLookupId)
        {
            string Statusmsg = string.Empty;
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            WorkOrder Wojob = approvalWrapper.MultiLevelDenyWO(WorkorderId, ApprovalGroupId, ref Statusmsg);
            return Json(new { data = Statusmsg, WoId = WorkorderId, error = Wojob.ErrorMessages }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Material Request
        [HttpPost]
        public PartialViewResult LoadMaterialRequest()
        {
            ApprovalVM approvalVM = new ApprovalVM();
            LocalizeControls(approvalVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/Approval/_MaterialRequest.cshtml", approvalVM);
        }
        [HttpPost]
        public PartialViewResult Load_MaterialRequestSearchGrid()
        {
            ApprovalVM approvalVM = new ApprovalVM();            
            LocalizeControls(approvalVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/Approval/MaterialRequest/_MaterialRequestSearchGrid.cshtml", approvalVM);
        }

        [HttpPost]
        public string PopulateMaterialRequestSearchGrid(int? draw, int? start, int? length, string FilterTypeCase = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            Client.BusinessWrapper.ApprovalWrapper approvarWrapper = new Client.BusinessWrapper.ApprovalWrapper(userData);
            List<MaterialRequestModel> materialRequestList = new List<MaterialRequestModel>();
            materialRequestList = approvarWrapper.GetMaterialRequestList(order, length ?? 0, orderDir, skip,  FilterTypeCase);
           
            var totalRecords = 0;
            var recordsFiltered = 0;
            
            if (materialRequestList != null && materialRequestList.Count > 0)
            {
                recordsFiltered = materialRequestList[0].TotalCount;
                totalRecords = materialRequestList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = materialRequestList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, security = userData.Security.MaterialRequest_Approve }, JsonSerializerDateSettings);
        }

        public string GetApprovalRouteMRPrintData(string colorder, string coldir, int? start, int? length, string filterType)
        {
            List<ApprovalRouteMRPrintModel> filteredResult = new List<ApprovalRouteMRPrintModel>();
            ApprovalRouteMRPrintModel objMRPrintModel;

            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            var personnelid = this.userData.DatabaseKey.Personnel.PersonnelId;
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            length = 10000;
            List<MaterialRequestModel> list = approvalWrapper.GetMaterialRequestList(colorder, length ?? 0, coldir, skip, filterType);

            if (list != null)
            {
                foreach (var item in list)
                {
                    objMRPrintModel = new ApprovalRouteMRPrintModel();
                    objMRPrintModel.ClientLookupId = item.ClientLookupId;
                    objMRPrintModel.Description = item.Description;
                    objMRPrintModel.UnitCost = item.UnitCost;
                    objMRPrintModel.Quantity = item.Quantity;
                    objMRPrintModel.TotalCost = item.TotalCost;
                    objMRPrintModel.Date = item.Date;
                    objMRPrintModel.Comments = item.Comments;
                    filteredResult.Add(objMRPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = filteredResult }, JsonSerializerDateSettings);
        }
        [HttpPost]
        public PartialViewResult MaterialRequestDetails(long MaterialRequestId)
        {
            ApprovalVM approvalVM = new ApprovalVM();
          
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            Models.MaterialRequest.MaterialRequestSummaryModel mrSummary = new Models.MaterialRequest.MaterialRequestSummaryModel();
            var materialRequestDetails = mrWrapper.PopulateMaterialRequestDetails(MaterialRequestId);
            if (materialRequestDetails != null)
            {
                mrSummary.MaterialRequestId = materialRequestDetails.MaterialRequestId;
                mrSummary.Description = materialRequestDetails.Description;
                mrSummary.Status = materialRequestDetails.Status;
                mrSummary.Personnel_NameFirst = materialRequestDetails.Personnel_NameFirst;
                mrSummary.Personnel_NameLast = materialRequestDetails.Personnel_NameLast;
                mrSummary.CreateDate = materialRequestDetails.CreateDate;
                mrSummary.RequiredDate = materialRequestDetails.RequiredDate;
                mrSummary.CompleteDate = materialRequestDetails.CompleteDate;
                mrSummary.Account_ClientLookupId = materialRequestDetails.Account_ClientLookupId;
            }
            approvalVM.materialRequestSummary = mrSummary;
            //}
            LocalizeControls(approvalVM, LocalizeResourceSetConstants.MaterialRequest);
            return PartialView("~/Views/Approval/MaterialRequest/_MaterialRequestDetails.cshtml", approvalVM);
        }

        #region Multilevel Approve and Deny
        [HttpPost]
        public JsonResult MultiLevelApproveMR(long MaterialRequestId, long ApprovalGroupId, string ClientLookupId)
        {
            var dataModels = Get_MultiLevelApproverList(this.userData.DatabaseKey.Personnel.PersonnelId, ApprovalGroupRequestTypes.MaterialRequest, MaterialRequestId, ApprovalGroupId);
            var approvers = new List<SelectListItem>();
            if (dataModels.Count > 0)
            {
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.ApproverName,
                    Value = x.ApproverId.ToString()
                }).ToList();
            }
            return Json(new { data = JsonReturnEnum.success.ToString(), ApproverList = approvers }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult SendMRForMultiLevelApproval(List<SelectListItem> Approvers, long EstimatedCostsId, long ApprovalGroupId)
        {
            ApprovalVM approvalVM = new ApprovalVM();

            MultiLevelApproveApprovalRouteModel multiLevelApproveApprovalRoute = new MultiLevelApproveApprovalRouteModel();

            multiLevelApproveApprovalRoute.ApproverList = Approvers;
            multiLevelApproveApprovalRoute.ApproverCount = Approvers.Count;
            multiLevelApproveApprovalRoute.ObjectId = EstimatedCostsId;
            multiLevelApproveApprovalRoute.ApprovalGroupId = ApprovalGroupId;

            approvalVM.multiLevelApproveApprovalRouteModel = multiLevelApproveApprovalRoute;
            LocalizeControls(approvalVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/Approval/MaterialRequest/_SendMaterialRequestForMultiLevelApproval.cshtml", approvalVM);
        }
        [HttpPost]
        public JsonResult SendMRForMultiLevelApprovalSave(ApprovalVM wrVM)
        {
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            if (ModelState.IsValid)
            {
                string MaterialRequest = ApprovalGroupRequestTypes.MaterialRequest;
                wrVM.multiLevelApproveApprovalRouteModel.RequestType = MaterialRequest;
                var objEstimatedCosts = approvalWrapper.MultiLevelApproveMR(wrVM.multiLevelApproveApprovalRouteModel);
                if (objEstimatedCosts.ErrorMessages != null && objEstimatedCosts.ErrorMessages.Count > 0)
                {
                    return Json(objEstimatedCosts.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), EstimatedCostsId = wrVM.multiLevelApproveApprovalRouteModel.ObjectId, ApprovalGroupId = wrVM.multiLevelApproveApprovalRouteModel.ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult MultiLevelFinalApproveMR(long EstimatedCostsId, long ApprovalGroupId)
        {
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            if (ModelState.IsValid)
            {
                var objEstimatedCosts = approvalWrapper.MultiLevelFinalApproveMR(EstimatedCostsId, ApprovalGroupId);
                if (objEstimatedCosts.ErrorMessages != null && objEstimatedCosts.ErrorMessages.Count > 0)
                {
                    return Json(objEstimatedCosts.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), EstimatedCostsId = EstimatedCostsId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult MultiLevelDenyMR(long EstimatedCostsId, long ApprovalGroupId, string ClientLookupId)
        {
            string Statusmsg = string.Empty;
            ApprovalWrapper approvalWrapper = new ApprovalWrapper(userData);
            EstimatedCosts ESjob = approvalWrapper.MultiLevelDenyMR(EstimatedCostsId, ApprovalGroupId, ref Statusmsg);
            return Json(new { data = Statusmsg, EstimatedCostsId = EstimatedCostsId, error = ESjob.ErrorMessages }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
    }
}