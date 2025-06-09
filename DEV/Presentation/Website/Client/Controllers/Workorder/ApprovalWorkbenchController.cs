using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Work_Order;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Common;
using Client.Models.Work_Order;
using Client.Models.WorkOrder;

using Common.Constants;

using DataContracts;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.Workorder
{
    public class ApprovalWorkbenchController : SomaxBaseController
    {
        #region Search
        //[CheckUserSecurity(securityType = SecurityConstants.WorkOrder_Approve)]
        [CheckUserSecurity(securityType = SecurityConstants.WorkOrder_ApprovalPage)]
        public ActionResult Index()
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            WorkOrderModel woModel = new WorkOrderModel();
            ApprovalWorkBenchWrapper woWrapper = new ApprovalWorkBenchWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> ShiftLookUpList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> DenyLookUpList = new List<DataContracts.LookupList>();
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                ShiftLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (ShiftLookUpList != null)
                {
                    woModel.ShiftList = ShiftLookUpList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }

                DenyLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_REASON_DENY).ToList();
                if (DenyLookUpList != null)
                {
                    woModel.DenyReasonList = DenyLookUpList.Select(x => new SelectListItem { Text = x.ListValue, Value = x.ListValue.ToString() });
                }
            }
            var StatusList = UtilityFunction.WorkBenchStatusList();
            if (StatusList != null)
            {
                woModel.WbStatusList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            var CreateDatesList = UtilityFunction.WorkBenchCreateDatesList();
            if (CreateDatesList != null)
            {
                woModel.CreateDatesList = CreateDatesList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            var PersonnelLookUplist = GetList_Personnel();
            if (PersonnelLookUplist != null)
            {
                woModel.WorkAssignedLookUpList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            objWorkOrderVM.workOrderModel = woModel;
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return View(objWorkOrderVM);
        }
        public JsonResult GetWorkOrders(string status, string Createdates, string workorder = "", string description = "", long chargeto = 0, string Chargetoname = "", long workassigned = 0, string shift = "", DateTime? scheduledate = null, decimal scheduleduration = 0, DateTime? createdate = null, string createdby = "")
        {
            ActualWBdropDownsModel actualWBdropDownsModel = new ActualWBdropDownsModel();
            ApprovalWorkBenchWrapper woWrapper = new ApprovalWorkBenchWrapper(userData);
            List<WorkOrderModel> woMaintMasterList = woWrapper.GetWOApprovalWorkBenchDetails(status, Createdates);

            if (woMaintMasterList != null)
            {
                if (!string.IsNullOrEmpty(workorder))
                {
                    workorder = workorder.ToUpper();
                    woMaintMasterList = woMaintMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(workorder))).ToList();
                }
                if (!string.IsNullOrEmpty(description))
                {
                    description = description.ToUpper();
                    woMaintMasterList = woMaintMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(description))).ToList();
                }
                if (chargeto != 0)
                {
                    woMaintMasterList = woMaintMasterList.Where(x => x.ChargeToClientLookupId.Equals(chargeto)).ToList();
                }
                if (!string.IsNullOrEmpty(Chargetoname))
                {
                    Chargetoname = Chargetoname.ToUpper();
                    woMaintMasterList = woMaintMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_Name) && x.ChargeTo_Name.ToUpper().Contains(Chargetoname))).ToList();
                }
                if (workassigned != 0)
                {
                    woMaintMasterList = woMaintMasterList.Where(x => x.WorkAssigned_PersonnelId.Equals(workassigned)).ToList();
                }
                if (!string.IsNullOrEmpty(shift))
                {
                    shift = shift.ToUpper();
                    woMaintMasterList = woMaintMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Shift) && x.Shift.ToUpper().Contains(shift))).ToList();
                }
                if (scheduledate != null)
                {
                    woMaintMasterList = woMaintMasterList.Where(x => x.ScheduledStartDate != null && x.ScheduledStartDate.Value.Date.Equals(scheduledate.Value.Date)).ToList();
                }
                if (scheduleduration != 0)
                {
                    woMaintMasterList = woMaintMasterList.Where(x => x.ScheduledDuration.Equals(scheduleduration)).ToList();
                }
                if (createdate != null)
                {
                    woMaintMasterList = woMaintMasterList.Where(x => x.CreateDate != null && x.CreateDate.Value.Date.Equals(createdate.Value.Date)).ToList();
                }
                if (!string.IsNullOrEmpty(createdby))
                {
                    createdby = createdby.ToUpper();
                    woMaintMasterList = woMaintMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Createby) && x.Createby.ToUpper().Contains(createdby))).ToList();
                }
            }
            return Json(woMaintMasterList, JsonRequestBehavior.AllowGet);

        }
        public string GetWorkOrderMaintGrid(int? draw, int? start, int? length, string status, string Createdates, string workorder = "", string description = "", string chargeto = null, string Chargetoname = null, long workassigned = 0, string shift = null, DateTime? scheduledate = null, decimal scheduleduration = 0, DateTime? createdate = null, string createdby = null)
        {
            ActualWBdropDownsModel actualWBdropDownsModel = new ActualWBdropDownsModel();
            List<string> CreatedList = new List<string>();
            ApprovalWorkBenchWrapper woWrapper = new ApprovalWorkBenchWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> ShiftLookUpList = new List<DataContracts.LookupList>();
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            List<WorkOrderModel> woMaintMasterList = woWrapper.GetWOApprovalWorkBenchDetails(status, Createdates);
            if (woMaintMasterList != null)
            {
                CreatedList = woMaintMasterList.Select(x => x.Createby).Distinct().ToList();
            }
            woMaintMasterList = this.GetAppworkBenchDetailsByColumnWithOrder(order, orderDir, woMaintMasterList);

            if (woMaintMasterList != null)
            {
                if (!string.IsNullOrEmpty(workorder))
                {
                    workorder = workorder.ToUpper();
                    woMaintMasterList = woMaintMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(workorder))).ToList();
                }
                if (!string.IsNullOrEmpty(description))
                {
                    description = description.ToUpper();
                    woMaintMasterList = woMaintMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(description))).ToList();
                }
                if (!string.IsNullOrEmpty(chargeto))
                {
                    woMaintMasterList = woMaintMasterList.Where(x => (!string.IsNullOrEmpty(x.ChargeToClientLookupId) && x.ChargeToClientLookupId.ToUpper().Contains(chargeto))).ToList();
                }
                if (!string.IsNullOrEmpty(Chargetoname))
                {
                    Chargetoname = Chargetoname.ToUpper();
                    woMaintMasterList = woMaintMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_Name) && x.ChargeTo_Name.ToUpper().Contains(Chargetoname))).ToList();
                }
                if (workassigned != 0)
                {
                    woMaintMasterList = woMaintMasterList.Where(x => x.WorkAssigned_PersonnelId.Equals(workassigned)).ToList();
                }
                if (!string.IsNullOrEmpty(shift))
                {
                    woMaintMasterList = woMaintMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Shift) && x.Shift.Equals(shift))).ToList();
                }
                if (scheduledate != null)
                {
                    woMaintMasterList = woMaintMasterList.Where(x => x.ScheduledStartDate != null && x.ScheduledStartDate.Value.Date.Equals(scheduledate.Value.Date)).ToList();
                }
                if (scheduleduration != 0)
                {
                    woMaintMasterList = woMaintMasterList.Where(x => x.ScheduledDuration.Equals(scheduleduration)).ToList();
                }
                if (createdate != null)
                {
                    woMaintMasterList = woMaintMasterList.Where(x => x.CreateDate != null && x.CreateDate.Value.Date.Equals(createdate.Value.Date)).ToList();
                }
                if (!string.IsNullOrEmpty(createdby))
                {
                    createdby = createdby.ToUpper();
                    woMaintMasterList = woMaintMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Createby) && x.Createby.ToUpper().Contains(createdby))).ToList();
                }
            }

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = woMaintMasterList.Count();
            totalRecords = woMaintMasterList.Count();

            int initialPage = start.Value;

            var filteredResult = woMaintMasterList
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

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, options = actualWBdropDownsModel, CreatedList = CreatedList }, JsonSerializerDateSettings);
        }
        [HttpPost]
        public JsonResult DenyWorkOrder(string[] wOIds, string Reason, string Comments)
        {
            ApprovalWorkBenchWrapper woWrapper = new ApprovalWorkBenchWrapper(userData);
            var retValue = woWrapper.DenyWorkorder(wOIds, Reason, Comments);
            if (retValue)
            {
                return Json(new { Result = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = "failed" }, JsonRequestBehavior.AllowGet);
        }
        private List<WorkOrderModel> GetAppworkBenchDetailsByColumnWithOrder(string order, string orderDir, List<WorkOrderModel> data)
        {
            List<WorkOrderModel> lst = new List<WorkOrderModel>();

            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToClientLookupId).ToList() : data.OrderBy(p => p.ChargeToClientLookupId).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_Name).ToList() : data.OrderBy(p => p.ChargeTo_Name).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.WorkAssigned_PersonnelId).ToList() : data.OrderBy(p => p.WorkAssigned_PersonnelId).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Shift).ToList() : data.OrderBy(p => p.Shift).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScheduledStartDate).ToList() : data.OrderBy(p => p.ScheduledStartDate).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScheduledDuration).ToList() : data.OrderBy(p => p.ScheduledDuration).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                case "10":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Createby).ToList() : data.OrderBy(p => p.Createby).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }

        #endregion

        #region work-order-info
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddWorkOrderInfo(WorkOrderVM workOrderVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                WorkOrderVM objWorksOrderVM = new WorkOrderVM();
                ApprovalWorkBenchWrapper Wrapper = new ApprovalWorkBenchWrapper(userData);
                WorkOrder WorkOrderJob = Wrapper.AddWorkOrderInfoDetails(workOrderVM);
                if (WorkOrderJob.ErrorMessages != null && WorkOrderJob.ErrorMessages.Count > 0)
                {
                    return Json(WorkOrderJob.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), WorkOrderId = WorkOrderJob.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult WorkOrderInformation(long workOrderId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            ApprovalWorkBenchWrapper woaWrapper = new ApprovalWorkBenchWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Priority = new List<DataContracts.LookupList>();

            objWorksOrderVM.WOInfoModel = woaWrapper.getWorkOderInfoDetailsById(workOrderId);
            objWorksOrderVM._userdata = this.userData;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (Shift != null && Shift.Count() > 0)
                {
                    objWorksOrderVM.WOInfoModel.ShiftList = Shift.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null && Type.Count() > 0)
                {
                    objWorksOrderVM.WOInfoModel.TypeList = Type.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }
                Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
                if (Priority != null && Priority.Count() > 0)
                {
                    objWorksOrderVM.WOInfoModel.PriorityList = Priority.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }
            }

            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
            if (ScheduleChargeTypeList != null && ScheduleChargeTypeList.Count() > 0)
            {
                //objWorksOrderVM.WOInfoModel.ChargeType = ScheduleChargeTypeList.Where(x => x.value == objWorksOrderVM.WOInfoModel.ChargeType).Select(x => x.text).First();
                objWorksOrderVM.WOInfoModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            //if (ScheduleChargeTypeList != null && objWorksOrderVM.WOInfoModel.ChargeType == "")
            //{
            //    objWorksOrderVM.WOInfoModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value });

            //}
            //var ChargeTypeLookUpList = PopulatelookUpListByType(objWorksOrderVM.WOInfoModel.ChargeType);
            //if (ChargeTypeLookUpList != null)
            //{
            //    objWorksOrderVM.WOInfoModel.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = objWorksOrderVM.WOInfoModel.ChargeToId.ToString() });
            //}

            if (objWorksOrderVM.WOInfoModel.ChargeType != null)
            {
                var chargeTypeLookUpList = PopulatelookUpListByType(objWorksOrderVM.WOInfoModel.ChargeType);
                if (chargeTypeLookUpList != null && chargeTypeLookUpList.Count > 0)
                {
                    objWorksOrderVM.WOInfoModel.ChargeTypelookUpList = chargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
                }
            }
            else
            {
                objWorksOrderVM.WOInfoModel.ChargeTypelookUpList = new List<SelectListItem>();
            }

            var PersonnelLookUplist = GetList_Personnel();
            if (PersonnelLookUplist != null && PersonnelLookUplist.Count() > 0)
            {
                objWorksOrderVM.WOInfoModel.WorkAssignedLookUpList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });

            }

            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(objWorksOrderVM.WOInfoModel);
            string TreeHeader = string.Empty;
            if (userData.Site.ProcessSystemTree == true)
            {
                objWorksOrderVM.treeHeader = UtilityFunction.GetMessageFromResource("hdrTreeLookupGlobal", LocalizeResourceSetConstants.Global);
                objWorksOrderVM.treeHeaderVal = "ProcessSystemTree";
            }
            else
            {
                objWorksOrderVM.treeHeader = UtilityFunction.GetMessageFromResource("spnGlobalEquipmentTreeLookup", LocalizeResourceSetConstants.Global);
                objWorksOrderVM.treeHeaderVal = "EquipmentTree";
            }

            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/ApprovalWorkbench/_WorkOrderInfo.cshtml", objWorksOrderVM);
        }
        private WorkOrderSummaryModel GetWorkOrderSummaryModel(WOInfoModel workOrder)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrderSummaryModel workOrderSummaryModel = new WorkOrderSummaryModel();
            CommonWrapper comWrapper = new CommonWrapper(userData);
            workOrderSummaryModel.WorkOrderId = workOrder.WorkOrderId;
            workOrderSummaryModel.WorkOrder_ClientLookupId = workOrder.ClientLookupId;
            workOrderSummaryModel.ImageUrl = comWrapper.GetAzureImageUrl(workOrder.WorkOrderId, AttachmentTableConstant.WorkOrder);
            return workOrderSummaryModel;
        }

        #endregion

        #region Approve WO WorkBench
        [HttpPost]
        public ActionResult ApproveWorkOrder(List<ApproveWorkOrderModel> WOData)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            ApprovalWorkBenchWrapper Wrapper = new ApprovalWorkBenchWrapper(userData);
            var errorMessages = Wrapper.ApproveWODetails(WOData);

            if (errorMessages != null && errorMessages.Count > 0)
            {
                return Json(errorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}