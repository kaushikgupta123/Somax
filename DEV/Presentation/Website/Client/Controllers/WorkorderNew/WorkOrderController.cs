using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Configuration.ClientSetUp;
using Client.BusinessWrapper.InventoryCheckout;
using Client.BusinessWrapper.Work_Order;
using Client.BusinessWrapper.WorkOrderPlanning;
using Client.Common;
using Client.Controllers.Common;
using Client.Localization;
using Client.Models;
using Client.Models.Common;
using Client.Models.Configuration.ClientSetUp;
using Client.Models.LaborScheduling;
using Client.Models.PartLookup;
using Client.Models.Work_Order;
using Client.Models.Work_Order.UIConfiguration;
using Client.Models.WorkOrder;
using Common.Constants;
using Common.Extensions;
using DataContracts;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Rotativa;
using Rotativa.Options;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

using static Client.Models.Common.UserMentionDataModel;

namespace Client.Controllers.Maintenance
{
    public class WorkOrderController : SomaxBaseController
    {
        #region Workorder_search
        [CheckUserSecurity(securityType = SecurityConstants.WorkOrders)]
        public ActionResult Index()
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            WorkOrderModel woModel = new WorkOrderModel();
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            WoSendForApprovalModel woSendForApprovalModel = new WoSendForApprovalModel();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            ClientSetUpWrapper clientSetUpWrapper = new ClientSetUpWrapper(userData);
            WoCompletionSettingsModel completionSettingsModel = new WoCompletionSettingsModel();
            #region V2-730
            var isworkrequest = woWrapper.RetrieveApprovalGroupMaterialRequestStatus("WorkRequests");
            objWorkOrderVM.IsWorkRequestApproval = isworkrequest;
            #endregion
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
                if (CancelReason != null)
                {
                    objWorkOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                }
            }
            woModel.ScheduleWorkList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.WorkOrder, false);
            string mode = Convert.ToString(TempData["Mode"]);
            if (string.IsNullOrEmpty(mode))
            {

                objWorkOrderVM.workOrderModel = woModel;
                if (AllLookUps != null)
                {
                    var Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                    if (Shift != null)
                    {
                        var tmpShift = Shift.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                        woModel.ShiftList = tmpShift.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                    }

                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE || x.ListName == LookupListConstants.Preventive_Maint_WO_Type || x.ListName == LookupListConstants.UP_WO_TYPE || x.ListName == LookupListConstants.WR_WO_TYPE).ToList();
                    if (Type != null)
                    {
                        var tmpTypeList = Type.GroupBy(x => x.ListValue + " - " + x.Description).Select(x => x.FirstOrDefault()).ToList();
                        woModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                    }
                    var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority);
                    if (Priority != null)
                    {
                        var tmpPriority = Priority.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                        woModel.PriorityList = tmpPriority.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).Distinct().ToList();
                    }
                    var Failure = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_FAILURE);
                    if (Failure != null)
                    {
                        var tmpFailureList = Failure.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                        woModel.FailureList = tmpFailureList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                    }
                    EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
                    var ast1 = eWrapper.GetAssetGroup1Dropdowndata();
                    if (ast1 != null)
                    {
                        woModel.AssetGroup1List = ast1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
                    }
                    var ast2 = eWrapper.GetAssetGroup2Dropdowndata();
                    if (ast2 != null)
                    {
                        woModel.AssetGroup2List = ast2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
                    }
                    var ast3 = eWrapper.GetAssetGroup3Dropdowndata();
                    if (ast3 != null)
                    {
                        woModel.AssetGroup3List = ast3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
                    }
                    var SourceTypeList = UtilityFunction.PopulateSourceTypeList();
                    if (SourceTypeList != null)
                    {
                        woModel.SourceTypeList = SourceTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                    }
                    //status
                    var StatusList = commonWrapper.GetListFromConstVals(LookupListConstants.WO_Status);
                    if (StatusList != null)
                    {
                        woModel.StatusList = StatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                    }
                }
            }
            else
            {
                if (AllLookUps != null)
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                    if (Type != null)
                    {
                        var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                        woModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                    }
                }
            }

            if (mode == "ADDWOREQUEST")
            {
                WoRequestModel RequestModel = new WoRequestModel();
                if (Type != null)
                {
                    RequestModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
                }
                List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
                RequestModel.ChargeToList = defaultChargeToList;
                RequestModel.PlantLocation = userData.Site.PlantLocation;
                objWorkOrderVM.woRequestModel = RequestModel;
                objWorkOrderVM.IsAddRequest = true;
                objWorkOrderVM.workOrderModel = woModel;
            }
            else if (mode == "AddWoRequestDynamic") //611
            {
                objWorkOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.AddWorkRequest, userData);
                IList<string> LookupNames = objWorkOrderVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == DataDictionaryLookupTypeConstant.LookupList && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
                objWorkOrderVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                      .Select(s => new WOAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();
                objWorkOrderVM.IsWorkOrderRequest = false; //V2-1052
                if (mode == "AddWoRequestDynamic")
                {
                    objWorkOrderVM.IsWorkOrderRequest = true;
                    objWorkOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE)
                                                   .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                   .Select(s => new WOAddUILookupList
                                                   { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                   .ToList());
                }
                objWorkOrderVM.IsAddWoRequestDynamic = true;
                objWorkOrderVM.AddWorkRequest = new AddWorkRequestModelDynamic();
            }
            else if (mode == "DetailFromEquipment")
            {
                long workOrderId = Convert.ToInt64(TempData["workOrderId"]);
                #region V2-752
                WorkOrder workOrder = new WorkOrder();
                WorkOrderUDF workOrderUDF = new WorkOrderUDF();
                Task t1 = Task.Factory.StartNew(() => workOrder = woWrapper.getWorkOderDetailsById_V2(workOrderId));
                Task t2 = Task.Factory.StartNew(() => workOrderUDF = woWrapper.getWorkOrderUDFByWorkOrderId(workOrderId));
                Task t4 = Task.Factory.StartNew(() => objWorkOrderVM.attachmentCount = commonWrapper.AttachmentCount(workOrderId,
                    AttachmentTableConstant.WorkOrder, userData.Security.WorkOrders.Edit));
                Task.WaitAll(t1, t2, t4);

                objWorkOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                        .Retrieve(DataDictionaryViewNameConstant.ViewWorkOrderWidget, userData);
                objWorkOrderVM.workOrderModel = woWrapper.initializeControls(workOrder);

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

                #endregion

                objWorkOrderVM.workOrderModel.PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToLower();
                if (AllLookUps != null)
                {
                    var Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                    if (Shift != null && Shift.Any(cus => cus.ListValue == objWorkOrderVM.workOrderModel.Shift))
                    {
                        objWorkOrderVM.workOrderModel.Shift = Shift.Where(x => x.ListValue == objWorkOrderVM.workOrderModel.Shift).Select(x => x.Description).First();
                    }
                    var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
                    if (Priority != null && Priority.Any(cus => cus.ListValue == objWorkOrderVM.workOrderModel.Priority))
                    {
                        objWorkOrderVM.workOrderModel.Priority = Priority.Where(x => x.ListValue == objWorkOrderVM.workOrderModel.Priority).Select(x => x.Description).First();
                    }
                    var Failure = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_FAILURE).ToList();
                    if (Failure != null && Failure.Any(cus => cus.ListValue == objWorkOrderVM.workOrderModel.FailureCode))
                    {
                        objWorkOrderVM.workOrderModel.FailureCode = Failure.Where(x => x.ListValue == objWorkOrderVM.workOrderModel.FailureCode).Select(x => x.Description).First();
                    }
                    var CancelLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_Task_Cancel).ToList();
                    if (CancelLookUpList != null)
                    {
                        objWorkOrderVM.woTaskModel = new WoTaskModel();
                        objWorkOrderVM.woTaskModel.CancelReasonList = CancelLookUpList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                    }
                    var CancelLookUpListWo = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
                    if (CancelLookUpListWo != null)
                    {
                        objWorkOrderVM.CancelReasonListWo = CancelLookUpListWo.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                    }
                }
                if (Type != null && Type.Any(cus => cus.ListValue == objWorkOrderVM.workOrderModel.Type))
                {
                    objWorkOrderVM.workOrderModel.Type = Type.Where(x => x.ListValue == objWorkOrderVM.workOrderModel.Type).Select(x => x.Description).First();
                }
                var Account = woWrapper.GetLookupList_Account();
                if (Account != null && Account.Any(cus => cus.AccountId == objWorkOrderVM.workOrderModel.Labor_AccountId))
                {
                    objWorkOrderVM.workOrderModel.strLabor_AccountId = Account.Where(x => x.AccountId == objWorkOrderVM.workOrderModel.Labor_AccountId).Select(x => x.Account).First();
                }
                var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
                if (ScheduleChargeTypeList != null && ScheduleChargeTypeList.Any(cus => cus.value == objWorkOrderVM.workOrderModel.ChargeType))
                {
                    objWorkOrderVM.workOrderModel.ChargeType = ScheduleChargeTypeList.Where(x => x.value == objWorkOrderVM.workOrderModel.ChargeType).Select(x => x.text).First();
                }

                var ChargeTypeLookUpList = PopulatelookUpListByType(objWorkOrderVM.workOrderModel.ChargeType);
                if (ChargeTypeLookUpList != null && ChargeTypeLookUpList.Any(cus => cus.ChargeToId == objWorkOrderVM.workOrderModel.ChargeToId))
                {
                    objWorkOrderVM.workOrderModel.ChargeToClientLookupId = ChargeTypeLookUpList.Where(x => x.ChargeToId == objWorkOrderVM.workOrderModel.ChargeToId).Select(co => co.ChargeToClientLookupId).First();
                    objWorkOrderVM.workOrderModel.ChargeTo_Name = ChargeTypeLookUpList.Where(x => x.ChargeToId == objWorkOrderVM.workOrderModel.ChargeToId).Select(co => co.Name).First();
                }

                objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(objWorkOrderVM.workOrderModel.WorkOrderId, objWorkOrderVM.workOrderModel.ClientLookupId, objWorkOrderVM.workOrderModel.Status, objWorkOrderVM.workOrderModel.Description, objWorkOrderVM.workOrderModel.ProjectClientLookupId, objWorkOrderVM.workOrderModel);
                objWorkOrderVM.workOrderModel.WorkOrderId = objWorkOrderVM.workOrderModel.WorkOrderId;
                objWorkOrderVM.workOrderSummaryModel.Type = objWorkOrderVM.workOrderModel.Type;
                objWorkOrderVM.workOrderSummaryModel.Priority = objWorkOrderVM.workOrderModel.Priority;
                objWorkOrderVM.workOrderSummaryModel.ChargeToClientLookupId = objWorkOrderVM.workOrderModel.ChargeToClientLookupId;
                objWorkOrderVM.workOrderSummaryModel.ChargeTo_Name = objWorkOrderVM.workOrderModel.ChargeTo_Name;
                objWorkOrderVM.workOrderSummaryModel.ScheduledStartDate = objWorkOrderVM.workOrderModel.ScheduledStartDate;
                objWorkOrderVM.workOrderSummaryModel.ScheduledDuration = objWorkOrderVM.workOrderModel.ScheduledDuration;
                objWorkOrderVM.workOrderSummaryModel.AssignedFullName = objWorkOrderVM.workOrderModel.AssignedFullName;
                objWorkOrderVM.workOrderSummaryModel.CompleteDate = objWorkOrderVM.workOrderModel.CompleteDate;
                objWorkOrderVM.workOrderSummaryModel.security = this.userData.Security;
                //V2-463
                objWorkOrderVM.workOrderSummaryModel.EquipDownDate = objWorkOrderVM.workOrderModel.EquipDownDate != null ? objWorkOrderVM.workOrderModel.EquipDownDate : null;
                objWorkOrderVM.workOrderSummaryModel.EquipDownHours = objWorkOrderVM.workOrderModel.EquipDownHours;
                //V2-599
                objWorkOrderVM.workOrderSummaryModel.AssetLocation = objWorkOrderVM.workOrderModel.AssetLocation;
                objWorkOrderVM.workOrderSummaryModel.Assigned = objWorkOrderVM.workOrderModel.Assigned;
                objWorkOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = objWorkOrderVM.workOrderModel.WorkAssigned_PersonnelId;
                objWorkOrderVM.workOrderSummaryModel.IsDetail = true;

                #region//*****V2-847
                objWorkOrderVM.workOrderSummaryModel.AssetGroup1Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 1" : this.userData.Site.AssetGroup1Name;
                objWorkOrderVM.workOrderSummaryModel.AssetGroup2Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup2Name) ? "Asset Group 2" : this.userData.Site.AssetGroup2Name;
                objWorkOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = workOrder.AssetGroup1ClientlookupId;
                objWorkOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = workOrder.AssetGroup2ClientlookupId;
                #endregion//*****

                var PersonnelLookUplist = Get_PersonnelList(SecurityConstants.WorkOrder_Approve, "ItemAccess");
                if (PersonnelLookUplist != null)
                {
                    woSendForApprovalModel.Personnellist = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
                }
                woSendForApprovalModel.WorkOrderId = workOrderId;

                #region Follow Up
                objWorkOrderVM.woRequestModel = new WoRequestModel();

                if (Type != null)
                {
                    objWorkOrderVM.woRequestModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
                }
                if (ScheduleChargeTypeList != null)
                {
                    objWorkOrderVM.woRequestModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).Where(x => x.Text != ChargeType.Location);
                }
                List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
                objWorkOrderVM.woRequestModel.ChargeToList = defaultChargeToList;
                objWorkOrderVM.woRequestModel.WorkOrderId = objWorkOrderVM.workOrderModel.WorkOrderId;
                #endregion

                objWorkOrderVM.woRequestModel.PlantLocation = userData.Site.PlantLocation;
                objWorkOrderVM.security = this.userData.Security;
                objWorkOrderVM.IsMaintOnDemand = userData.Site.MaintOnDemand;
                objWorkOrderVM.downtimeModel = new DowntimeModel();
                if (objWorkOrderVM.workOrderModel.EquipDownDate != null)
                {
                    objWorkOrderVM.downtimeModel.Downdate = objWorkOrderVM.workOrderModel.EquipDownDate;
                    objWorkOrderVM.downtimeModel.Minutes = Convert.ToDecimal(objWorkOrderVM.workOrderModel.EquipDownHours.ToString("0.00"));
                }

                objWorkOrderVM.downtimeModel.WorkOrderId = workOrderId;
                ViewBag.SecurityEstimate = userData.Security.WorkOrders.Edit;
                objWorkOrderVM.IsAddWorkOrderFromEquipment = true;
                //V2-752
                objWorkOrderVM.ViewWorkOrderModel = new ViewWorkOrderModelDynamic();
                objWorkOrderVM.ViewWorkOrderModel = woWrapper.MapWorkOrderDataForView(objWorkOrderVM.ViewWorkOrderModel, workOrder);
                objWorkOrderVM.ViewWorkOrderModel = woWrapper.MapWorkOrderUDFDataForView(objWorkOrderVM.ViewWorkOrderModel, workOrderUDF);
                objWorkOrderVM.ViewWorkOrderModel.Type = objWorkOrderVM.workOrderModel.Type;
                objWorkOrderVM.ViewWorkOrderModel.Shift = objWorkOrderVM.workOrderModel.Shift;
                objWorkOrderVM.ViewWorkOrderModel.Priority = objWorkOrderVM.workOrderModel.Priority;
                objWorkOrderVM.ViewWorkOrderModel.ChargeTo_Name = objWorkOrderVM.workOrderModel.ChargeTo_Name;
                objWorkOrderVM.ViewWorkOrderModel.FailureCode = objWorkOrderVM.workOrderModel.FailureCode;
                objWorkOrderVM.ViewWorkOrderModel.ReasonNotDone = workOrder.ReasonNotDone;
                //V2-726 Start
                var ismaterialrequest = woWrapper.RetrieveApprovalGroupMaterialRequestStatus("MaterialRequests");
                if (isworkrequest)
                {
                    approvalRouteModel = SendWRForApproval(workOrderId);
                }
                approvalRouteModel.IsWorkRequest = isworkrequest;
                approvalRouteModel.IsMaterialRequest = ismaterialrequest;
                //V2 726 End
                //V2-752
                objWorkOrderVM.woSendForApprovalModel = woSendForApprovalModel;



            }
            else if (mode == "addWorkOrder") //As per v2-611 dynamic ui this "addWorkOrder" changed to "addWorkOrderDynamic".
            {
                if (AllLookUps != null)
                {
                    var Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                    if (Shift != null)
                    {
                        var tmpShift = Shift.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                        woModel.ShiftList = tmpShift.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                    }
                    var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
                    if (Priority != null)
                    {
                        var tmpPriority = Priority.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                        woModel.PriorityList = tmpPriority.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).Distinct().ToList();
                    }
                }

                var Account = woWrapper.GetLookupList_Account();
                if (Account != null)
                {
                    woModel.AccountLookUpList = Account.Select(x => new SelectListItem { Text = x.Account, Value = x.AccountId.ToString() });
                }

                if (AllLookUps != null)
                {
                    var Failure = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_FAILURE).ToList();
                    woModel.FailureList = Failure.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                }
                var ChargeTypeList = UtilityFunction.populateChargeType();
                if (ChargeTypeList != null)
                {
                    woModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }

                var ChargeTypeLookUpList = PopulatelookUpListByType("");
                if (ChargeTypeLookUpList != null)
                {
                    woModel.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
                }
                var PersonnelLookUplist = GetList_PersonnelV2();
                if (PersonnelLookUplist != null)
                {
                    woModel.WorkAssignedLookUpList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
                }
                woModel.PlantLocation = userData.Site.PlantLocation;
                objWorkOrderVM.IsAddWorkOrderFromDashBoard = true;
                objWorkOrderVM.workOrderModel = woModel;
                objWorkOrderVM.security = this.userData.Security;
                objWorkOrderVM.IsMaintOnDemand = userData.Site.MaintOnDemand;
            }
            else if (mode == "addWorkOrderDynamic")
            {
                objWorkOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                               .Retrieve(DataDictionaryViewNameConstant.AddWorkOrder, userData);
                IList<string> LookupNames = objWorkOrderVM.UIConfigurationDetails.ToList()
                                           .Where(x => x.LookupType == DataDictionaryLookupTypeConstant.LookupList && !string.IsNullOrEmpty(x.LookupName))
                                           .Select(s => s.LookupName)
                                           .ToList();
                if (LookupNames.Contains(LookupListConstants.WO_Priority))
                {
                    objWorkOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority)
                                                         .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                         .Select(s => new WOAddUILookupList
                                                         { text = s.Description, value = s.ListValue, lookupname = s.ListName })
                                                         .ToList());
                    LookupNames.Remove(LookupListConstants.WO_Priority);
                }

                objWorkOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                          .Select(s => new WOAddUILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList());
                var SourceTypeList = UtilityFunction.PopulateSourceTypeList();
                if (SourceTypeList != null)
                {
                    objWorkOrderVM.SourceTypeList = SourceTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                }
                var StatusList = commonWrapper.GetListFromConstVals(LookupListConstants.WO_Status);
                if (StatusList != null)
                {
                    objWorkOrderVM.StatusList = StatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                }
                objWorkOrderVM.PlantLocation = userData.Site.PlantLocation;
                objWorkOrderVM.IsAddWorkOrderDynamic = true;
                objWorkOrderVM.IsAddWorkOrderFromDashBoard = true;
                objWorkOrderVM._userdata = this.userData;
                objWorkOrderVM.security = this.userData.Security;
                objWorkOrderVM.IsMaintOnDemand = userData.Site.MaintOnDemand;
                objWorkOrderVM.AddWorkorder = new Models.Work_Order.UIConfiguration.AddWorkOrderModelDynamic();

            }
            else if (mode == "DetailFromDashboard")
            {
                long workOrderId = Convert.ToInt64(TempData["workOrderId"]);
                WorkOrder workOrder = new WorkOrder();
                WorkOrderUDF workOrderUDF = new WorkOrderUDF();
                Task t1 = Task.Factory.StartNew(() => workOrder = woWrapper.getWorkOderDetailsById_V2(workOrderId));
                Task t2 = Task.Factory.StartNew(() => workOrderUDF = woWrapper.getWorkOrderUDFByWorkOrderId(workOrderId));
                Task t4 = Task.Factory.StartNew(() => objWorkOrderVM.attachmentCount = commonWrapper.AttachmentCount(workOrderId,
                    AttachmentTableConstant.WorkOrder, userData.Security.WorkOrders.Edit));
                Task completionSettingsTask = Task.Factory.StartNew(() => completionSettingsModel = clientSetUpWrapper.CompletionSettingsDetails());
                Task.WaitAll(t1, t2, t4, completionSettingsTask);

                objWorkOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                        .Retrieve(DataDictionaryViewNameConstant.ViewWorkOrderWidget, userData);
                objWorkOrderVM.workOrderModel = woWrapper.initializeControls(workOrder);
                objWorkOrderVM.UseWOCompletionWizard = completionSettingsModel.UseWOCompletionWizard;
                objWorkOrderVM.workOrderModel.PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToLower();



                if (AllLookUps != null)
                {
                    var Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                    if (Shift != null && Shift.Any(cus => cus.ListValue == objWorkOrderVM.workOrderModel.Shift))
                    {
                        objWorkOrderVM.workOrderModel.Shift = Shift.Where(x => x.ListValue == objWorkOrderVM.workOrderModel.Shift).Select(x => x.Description).First();
                    }
                    var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
                    if (Priority != null && Priority.Any(cus => cus.ListValue == objWorkOrderVM.workOrderModel.Priority))
                    {
                        objWorkOrderVM.workOrderModel.Priority = Priority.Where(x => x.ListValue == objWorkOrderVM.workOrderModel.Priority).Select(x => x.Description).First();
                    }
                    var Failure = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_FAILURE).ToList();
                    if (Failure != null && Failure.Any(cus => cus.ListValue == objWorkOrderVM.workOrderModel.FailureCode))
                    {
                        objWorkOrderVM.workOrderModel.FailureCode = Failure.Where(x => x.ListValue == objWorkOrderVM.workOrderModel.FailureCode).Select(x => x.Description).First();
                    }
                    var CancelLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_Task_Cancel).ToList();
                    if (CancelLookUpList != null)
                    {
                        objWorkOrderVM.woTaskModel = new WoTaskModel();
                        objWorkOrderVM.woTaskModel.CancelReasonList = CancelLookUpList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                    }
                    var CancelLookUpListWo = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
                    if (CancelLookUpListWo != null)
                    {
                        objWorkOrderVM.CancelReasonListWo = CancelLookUpListWo.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                    }
                }
                if (Type != null && Type.Any(cus => cus.ListValue == objWorkOrderVM.workOrderModel.Type))
                {
                    objWorkOrderVM.workOrderModel.Type = Type.Where(x => x.ListValue == objWorkOrderVM.workOrderModel.Type).Select(x => x.Description).First();
                }
                var Account = woWrapper.GetLookupList_Account();
                if (Account != null && Account.Any(cus => cus.AccountId == objWorkOrderVM.workOrderModel.Labor_AccountId))
                {
                    objWorkOrderVM.workOrderModel.strLabor_AccountId = Account.Where(x => x.AccountId == objWorkOrderVM.workOrderModel.Labor_AccountId).Select(x => x.Account).First();
                }
                var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
                if (ScheduleChargeTypeList != null && ScheduleChargeTypeList.Any(cus => cus.value == objWorkOrderVM.workOrderModel.ChargeType))
                {
                    objWorkOrderVM.workOrderModel.ChargeType = ScheduleChargeTypeList.Where(x => x.value == objWorkOrderVM.workOrderModel.ChargeType).Select(x => x.text).First();
                }

                var ChargeTypeLookUpList = PopulatelookUpListByType(objWorkOrderVM.workOrderModel.ChargeType);
                if (ChargeTypeLookUpList != null && ChargeTypeLookUpList.Any(cus => cus.ChargeToId == objWorkOrderVM.workOrderModel.ChargeToId))
                {
                    objWorkOrderVM.workOrderModel.ChargeToClientLookupId = ChargeTypeLookUpList.Where(x => x.ChargeToId == objWorkOrderVM.workOrderModel.ChargeToId).Select(co => co.ChargeToClientLookupId).First();
                    objWorkOrderVM.workOrderModel.ChargeTo_Name = ChargeTypeLookUpList.Where(x => x.ChargeToId == objWorkOrderVM.workOrderModel.ChargeToId).Select(co => co.Name).First();
                }

                objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(objWorkOrderVM.workOrderModel.WorkOrderId, objWorkOrderVM.workOrderModel.ClientLookupId, objWorkOrderVM.workOrderModel.Status, objWorkOrderVM.workOrderModel.Description, objWorkOrderVM.workOrderModel.ProjectClientLookupId);
                objWorkOrderVM.workOrderModel.WorkOrderId = objWorkOrderVM.workOrderModel.WorkOrderId;
                objWorkOrderVM.workOrderSummaryModel.Type = objWorkOrderVM.workOrderModel.Type;
                objWorkOrderVM.workOrderSummaryModel.Priority = objWorkOrderVM.workOrderModel.Priority;
                objWorkOrderVM.workOrderSummaryModel.ChargeToClientLookupId = objWorkOrderVM.workOrderModel.ChargeToClientLookupId;
                objWorkOrderVM.workOrderSummaryModel.ChargeTo_Name = objWorkOrderVM.workOrderModel.ChargeTo_Name;
                objWorkOrderVM.workOrderSummaryModel.ScheduledStartDate = objWorkOrderVM.workOrderModel.ScheduledStartDate;
                objWorkOrderVM.workOrderSummaryModel.ScheduledDuration = objWorkOrderVM.workOrderModel.ScheduledDuration;
                objWorkOrderVM.workOrderSummaryModel.AssignedFullName = objWorkOrderVM.workOrderModel.AssignedFullName;
                objWorkOrderVM.workOrderSummaryModel.CompleteDate = objWorkOrderVM.workOrderModel.CompleteDate;
                objWorkOrderVM.workOrderSummaryModel.security = this.userData.Security;
                //V2-463
                objWorkOrderVM.workOrderSummaryModel.EquipDownDate = objWorkOrderVM.workOrderModel.EquipDownDate != null ? objWorkOrderVM.workOrderModel.EquipDownDate : null;
                objWorkOrderVM.workOrderSummaryModel.EquipDownHours = objWorkOrderVM.workOrderModel.EquipDownHours;
                //V2-599
                objWorkOrderVM.workOrderSummaryModel.AssetLocation = objWorkOrderVM.workOrderModel.AssetLocation;
                var PersonnelLookUplist = Get_PersonnelList(SecurityConstants.WorkOrder_Approve, "ItemAccess");
                if (PersonnelLookUplist != null)
                {
                    woSendForApprovalModel.Personnellist = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
                }
                woSendForApprovalModel.WorkOrderId = workOrderId;
                #region Follow Up

                objWorkOrderVM.woRequestModel = new WoRequestModel();

                if (Type != null)
                {
                    objWorkOrderVM.woRequestModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
                }
                if (ScheduleChargeTypeList != null)
                {
                    objWorkOrderVM.woRequestModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).Where(x => x.Text != ChargeType.Location);
                }
                List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
                objWorkOrderVM.woRequestModel.ChargeToList = defaultChargeToList;
                objWorkOrderVM.woRequestModel.WorkOrderId = objWorkOrderVM.workOrderModel.WorkOrderId;

                #endregion
                objWorkOrderVM.woRequestModel.PlantLocation = userData.Site.PlantLocation;
                objWorkOrderVM.security = this.userData.Security;
                objWorkOrderVM.IsMaintOnDemand = userData.Site.MaintOnDemand;
                objWorkOrderVM.downtimeModel = new DowntimeModel();
                if (objWorkOrderVM.workOrderModel.EquipDownDate != null)
                {
                    objWorkOrderVM.downtimeModel.Downdate = objWorkOrderVM.workOrderModel.EquipDownDate;
                    objWorkOrderVM.downtimeModel.Minutes = Convert.ToDecimal(objWorkOrderVM.workOrderModel.EquipDownHours.ToString("0.00"));
                }
                objWorkOrderVM.downtimeModel.WorkOrderId = workOrderId;
                ViewBag.SecurityEstimate = userData.Security.WorkOrders.Edit;
                objWorkOrderVM.IsDetailWorkOrderFromDashBoard = true;
                objWorkOrderVM.woSendForApprovalModel = woSendForApprovalModel;

                objWorkOrderVM.ViewWorkOrderModel = new ViewWorkOrderModelDynamic();
                objWorkOrderVM.ViewWorkOrderModel = woWrapper.MapWorkOrderDataForView(objWorkOrderVM.ViewWorkOrderModel, workOrder);
                objWorkOrderVM.ViewWorkOrderModel = woWrapper.MapWorkOrderUDFDataForView(objWorkOrderVM.ViewWorkOrderModel, workOrderUDF);

                objWorkOrderVM.ViewWorkOrderModel.Type = objWorkOrderVM.workOrderModel.Type;
                objWorkOrderVM.ViewWorkOrderModel.Shift = objWorkOrderVM.workOrderModel.Shift;
                objWorkOrderVM.ViewWorkOrderModel.Priority = objWorkOrderVM.workOrderModel.Priority;
                objWorkOrderVM.ViewWorkOrderModel.ChargeTo_Name = objWorkOrderVM.workOrderModel.ChargeTo_Name;
                objWorkOrderVM.ViewWorkOrderModel.FailureCode = objWorkOrderVM.workOrderModel.FailureCode;
                objWorkOrderVM.ViewWorkOrderModel.ReasonNotDone = workOrder.ReasonNotDone;

            }
            else if (mode == "DetailFromNotification") //V2-1136
            {
                long workOrderId = Convert.ToInt64(TempData["workOrderId"]);
                #region V2-752
                WorkOrder workOrder = new WorkOrder();
                WorkOrderUDF workOrderUDF = new WorkOrderUDF();
                Task t1 = Task.Factory.StartNew(() => workOrder = woWrapper.getWorkOderDetailsById_V2(workOrderId));
                Task t2 = Task.Factory.StartNew(() => workOrderUDF = woWrapper.getWorkOrderUDFByWorkOrderId(workOrderId));
                Task t4 = Task.Factory.StartNew(() => objWorkOrderVM.attachmentCount = commonWrapper.AttachmentCount(workOrderId,
                    AttachmentTableConstant.WorkOrder, userData.Security.WorkOrders.Edit));
                Task.WaitAll(t1, t2, t4);

                objWorkOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                        .Retrieve(DataDictionaryViewNameConstant.ViewWorkOrderWidget, userData);
                objWorkOrderVM.workOrderModel = woWrapper.initializeControls(workOrder);
                objWorkOrderVM.WorkOrderAlertName = Convert.ToString(TempData["AlertName"]);
                objWorkOrderVM.IsDetailFromNotification = true;
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

                #endregion

                objWorkOrderVM.workOrderModel.PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToLower();
                if (AllLookUps != null)
                {
                    var Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                    if (Shift != null && Shift.Any(cus => cus.ListValue == objWorkOrderVM.workOrderModel.Shift))
                    {
                        objWorkOrderVM.workOrderModel.Shift = Shift.Where(x => x.ListValue == objWorkOrderVM.workOrderModel.Shift).Select(x => x.Description).First();
                    }
                    var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
                    if (Priority != null && Priority.Any(cus => cus.ListValue == objWorkOrderVM.workOrderModel.Priority))
                    {
                        objWorkOrderVM.workOrderModel.Priority = Priority.Where(x => x.ListValue == objWorkOrderVM.workOrderModel.Priority).Select(x => x.Description).First();
                    }
                    var Failure = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_FAILURE).ToList();
                    if (Failure != null && Failure.Any(cus => cus.ListValue == objWorkOrderVM.workOrderModel.FailureCode))
                    {
                        objWorkOrderVM.workOrderModel.FailureCode = Failure.Where(x => x.ListValue == objWorkOrderVM.workOrderModel.FailureCode).Select(x => x.Description).First();
                    }
                    var CancelLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_Task_Cancel).ToList();
                    if (CancelLookUpList != null)
                    {
                        objWorkOrderVM.woTaskModel = new WoTaskModel();
                        objWorkOrderVM.woTaskModel.CancelReasonList = CancelLookUpList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                    }
                    var CancelLookUpListWo = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
                    if (CancelLookUpListWo != null)
                    {
                        objWorkOrderVM.CancelReasonListWo = CancelLookUpListWo.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                    }
                }
                if (Type != null && Type.Any(cus => cus.ListValue == objWorkOrderVM.workOrderModel.Type))
                {
                    objWorkOrderVM.workOrderModel.Type = Type.Where(x => x.ListValue == objWorkOrderVM.workOrderModel.Type).Select(x => x.Description).First();
                }
                var Account = woWrapper.GetLookupList_Account();
                if (Account != null && Account.Any(cus => cus.AccountId == objWorkOrderVM.workOrderModel.Labor_AccountId))
                {
                    objWorkOrderVM.workOrderModel.strLabor_AccountId = Account.Where(x => x.AccountId == objWorkOrderVM.workOrderModel.Labor_AccountId).Select(x => x.Account).First();
                }
                var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
                if (ScheduleChargeTypeList != null && ScheduleChargeTypeList.Any(cus => cus.value == objWorkOrderVM.workOrderModel.ChargeType))
                {
                    objWorkOrderVM.workOrderModel.ChargeType = ScheduleChargeTypeList.Where(x => x.value == objWorkOrderVM.workOrderModel.ChargeType).Select(x => x.text).First();
                }

                var ChargeTypeLookUpList = PopulatelookUpListByType(objWorkOrderVM.workOrderModel.ChargeType);
                if (ChargeTypeLookUpList != null && ChargeTypeLookUpList.Any(cus => cus.ChargeToId == objWorkOrderVM.workOrderModel.ChargeToId))
                {
                    objWorkOrderVM.workOrderModel.ChargeToClientLookupId = ChargeTypeLookUpList.Where(x => x.ChargeToId == objWorkOrderVM.workOrderModel.ChargeToId).Select(co => co.ChargeToClientLookupId).First();
                    objWorkOrderVM.workOrderModel.ChargeTo_Name = ChargeTypeLookUpList.Where(x => x.ChargeToId == objWorkOrderVM.workOrderModel.ChargeToId).Select(co => co.Name).First();
                }

                objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(objWorkOrderVM.workOrderModel.WorkOrderId, objWorkOrderVM.workOrderModel.ClientLookupId, objWorkOrderVM.workOrderModel.Status, objWorkOrderVM.workOrderModel.Description, objWorkOrderVM.workOrderModel.ProjectClientLookupId, objWorkOrderVM.workOrderModel);
                objWorkOrderVM.workOrderModel.WorkOrderId = objWorkOrderVM.workOrderModel.WorkOrderId;
                objWorkOrderVM.workOrderSummaryModel.Type = objWorkOrderVM.workOrderModel.Type;
                objWorkOrderVM.workOrderSummaryModel.Priority = objWorkOrderVM.workOrderModel.Priority;
                objWorkOrderVM.workOrderSummaryModel.ChargeToClientLookupId = objWorkOrderVM.workOrderModel.ChargeToClientLookupId;
                objWorkOrderVM.workOrderSummaryModel.ChargeTo_Name = objWorkOrderVM.workOrderModel.ChargeTo_Name;
                objWorkOrderVM.workOrderSummaryModel.ScheduledStartDate = objWorkOrderVM.workOrderModel.ScheduledStartDate;
                objWorkOrderVM.workOrderSummaryModel.ScheduledDuration = objWorkOrderVM.workOrderModel.ScheduledDuration;
                objWorkOrderVM.workOrderSummaryModel.AssignedFullName = objWorkOrderVM.workOrderModel.AssignedFullName;
                objWorkOrderVM.workOrderSummaryModel.CompleteDate = objWorkOrderVM.workOrderModel.CompleteDate;
                objWorkOrderVM.workOrderSummaryModel.security = this.userData.Security;
                //V2-463
                objWorkOrderVM.workOrderSummaryModel.EquipDownDate = objWorkOrderVM.workOrderModel.EquipDownDate != null ? objWorkOrderVM.workOrderModel.EquipDownDate : null;
                objWorkOrderVM.workOrderSummaryModel.EquipDownHours = objWorkOrderVM.workOrderModel.EquipDownHours;
                //V2-599
                objWorkOrderVM.workOrderSummaryModel.AssetLocation = objWorkOrderVM.workOrderModel.AssetLocation;
                objWorkOrderVM.workOrderSummaryModel.Assigned = objWorkOrderVM.workOrderModel.Assigned;
                objWorkOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = objWorkOrderVM.workOrderModel.WorkAssigned_PersonnelId;
                objWorkOrderVM.workOrderSummaryModel.IsDetail = true;

                #region//*****V2-847
                objWorkOrderVM.workOrderSummaryModel.AssetGroup1Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 1" : this.userData.Site.AssetGroup1Name;
                objWorkOrderVM.workOrderSummaryModel.AssetGroup2Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup2Name) ? "Asset Group 2" : this.userData.Site.AssetGroup2Name;
                objWorkOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = workOrder.AssetGroup1ClientlookupId;
                objWorkOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = workOrder.AssetGroup2ClientlookupId;
                #endregion//*****

                var PersonnelLookUplist = Get_PersonnelList(SecurityConstants.WorkOrder_Approve, "ItemAccess");
                if (PersonnelLookUplist != null)
                {
                    woSendForApprovalModel.Personnellist = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
                }
                woSendForApprovalModel.WorkOrderId = workOrderId;

                #region Follow Up
                objWorkOrderVM.woRequestModel = new WoRequestModel();

                if (Type != null)
                {
                    objWorkOrderVM.woRequestModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
                }
                if (ScheduleChargeTypeList != null)
                {
                    objWorkOrderVM.woRequestModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).Where(x => x.Text != ChargeType.Location);
                }
                List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
                objWorkOrderVM.woRequestModel.ChargeToList = defaultChargeToList;
                objWorkOrderVM.woRequestModel.WorkOrderId = objWorkOrderVM.workOrderModel.WorkOrderId;
                #endregion

                objWorkOrderVM.woRequestModel.PlantLocation = userData.Site.PlantLocation;
                objWorkOrderVM.security = this.userData.Security;
                objWorkOrderVM.IsMaintOnDemand = userData.Site.MaintOnDemand;
                objWorkOrderVM.downtimeModel = new DowntimeModel();
                if (objWorkOrderVM.workOrderModel.EquipDownDate != null)
                {
                    objWorkOrderVM.downtimeModel.Downdate = objWorkOrderVM.workOrderModel.EquipDownDate;
                    objWorkOrderVM.downtimeModel.Minutes = Convert.ToDecimal(objWorkOrderVM.workOrderModel.EquipDownHours.ToString("0.00"));
                }

                objWorkOrderVM.downtimeModel.WorkOrderId = workOrderId;
                ViewBag.SecurityEstimate = userData.Security.WorkOrders.Edit;
             
                //V2-752
                objWorkOrderVM.ViewWorkOrderModel = new ViewWorkOrderModelDynamic();
                objWorkOrderVM.ViewWorkOrderModel = woWrapper.MapWorkOrderDataForView(objWorkOrderVM.ViewWorkOrderModel, workOrder);
                objWorkOrderVM.ViewWorkOrderModel = woWrapper.MapWorkOrderUDFDataForView(objWorkOrderVM.ViewWorkOrderModel, workOrderUDF);
                objWorkOrderVM.ViewWorkOrderModel.Type = objWorkOrderVM.workOrderModel.Type;
                objWorkOrderVM.ViewWorkOrderModel.Shift = objWorkOrderVM.workOrderModel.Shift;
                objWorkOrderVM.ViewWorkOrderModel.Priority = objWorkOrderVM.workOrderModel.Priority;
                objWorkOrderVM.ViewWorkOrderModel.ChargeTo_Name = objWorkOrderVM.workOrderModel.ChargeTo_Name;
                objWorkOrderVM.ViewWorkOrderModel.FailureCode = objWorkOrderVM.workOrderModel.FailureCode;
                objWorkOrderVM.ViewWorkOrderModel.ReasonNotDone = workOrder.ReasonNotDone;
                //V2-726 Start
                var ismaterialrequest = woWrapper.RetrieveApprovalGroupMaterialRequestStatus("MaterialRequests");
                if (isworkrequest)
                {
                    approvalRouteModel = SendWRForApproval(workOrderId);
                }
                approvalRouteModel.IsWorkRequest = isworkrequest;
                approvalRouteModel.IsMaterialRequest = ismaterialrequest;
                //V2 726 End
                //V2-752
                objWorkOrderVM.woSendForApprovalModel = woSendForApprovalModel;

            }
            objWorkOrderVM._userdata = this.userData;
            objWorkOrderVM.security = this.userData.Security;
            if (mode == "AddWoRequestDynamic")
            {
                objWorkOrderVM.BusinessType = this.userData.DatabaseKey.Client.BusinessType.ToUpper();
                objWorkOrderVM.DateRangeDropList = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
                objWorkOrderVM.DateRangeDropListForWO = UtilityFunction.GetTimeRangeDropForWO().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
                objWorkOrderVM.DateRangeDropListForWOCreatedate = UtilityFunction.GetTimeRangeDropForWOCreateDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-364
            }
            else if (mode == "addWorkOrderDynamic")
            {
                objWorkOrderVM.BusinessType = this.userData.DatabaseKey.Client.BusinessType.ToUpper();
            }

            else
            {
                objWorkOrderVM.workOrderModel.BusinessType = this.userData.DatabaseKey.Client.BusinessType.ToUpper();
                objWorkOrderVM.workOrderModel.DateRangeDropList = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
                objWorkOrderVM.workOrderModel.DateRangeDropListForWO = UtilityFunction.GetTimeRangeDropForWO().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
                objWorkOrderVM.workOrderModel.DateRangeDropListForWOCreatedate = UtilityFunction.GetTimeRangeDropForWOCreateDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-364
            }


            objWorkOrderVM.IsMaintOnDemand = userData.Site.MaintOnDemand;
            this.GetAssetGroupHeaderName(woModel);


            IEnumerable<SelectListItem> AssignedList;
            IEnumerable<SelectListItem> PersonnelList;
            this.PopulatePopUp(out AssignedList, out PersonnelList);
            objWorkOrderVM.PersonnelList = PersonnelList;
            woScheduleModel.Personnellist = PersonnelList;

            objWorkOrderVM.woScheduleModel = woScheduleModel;
            completionSettingsModel = clientSetUpWrapper.CompletionSettingsDetails();

            objWorkOrderVM.UseWOCompletionWizard = completionSettingsModel.UseWOCompletionWizard;
            objWorkOrderVM.WOBarcode = userData.Site.WOBarcode;
            objWorkOrderVM.ApprovalRouteModel = approvalRouteModel;
            //***V2-892****
            var DownRequiredStatusList = UtilityFunction.DownRequiredStatusTypesWithBoolValue();
            if (DownRequiredStatusList != null)
            {
                objWorkOrderVM.DownRequiredInactiveFlagList = DownRequiredStatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            //******
            //***V2-894****
            var Assignedlist = GetList_PersonnelV2();
            if (Assignedlist != null)
            {
                objWorkOrderVM.PersonnelIdList = Assignedlist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });

            }
            //******
            #region V2-1078
            WorkOrderPlanningResourceListWrapper workOrderPlanningResourceListWrapper = new WorkOrderPlanningResourceListWrapper(userData);
            // V2-1078 - RKL - 2024-10-22 - Begin
            // Only Planners should be in this list 
            // Changed from using the workOrderPlanningResourceListWrapper.SchedulePersonnelList()
            // To using the GetList_PalnnerPersonnel() method
            var plannerlu = GetList_PalnnerPersonnel();
            if (plannerlu != null)
            {
                objWorkOrderVM.PlannerList = plannerlu.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.PersonnelId.ToString() }).ToList();
            }
            // V2-1078 - RKL - 2024-10-22 - end
            //var totalList = workOrderPlanningResourceListWrapper.SchedulePersonnelList();
            //if (totalList != null && totalList.Count > 0)
            //{
            //    objWorkOrderVM.PlannerList = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            //}
            // V2-1078 - RKL - 2024-10-22 - End
            #endregion
            #region V2-1135
     
           var projectModelList = woWrapper.RetrieveWorkoderProjectLookupList();
            // RKL - The Work Order Advanced Search Fails if you do not set the objWorkOrderVM.ProjectList
            if (projectModelList != null)// && projectModelList.Count > 0)
            {
                objWorkOrderVM.ProjectList = projectModelList.Select(x => new SelectListItem { Text = x.ClientLookupId, Value = x.ProjectId.ToString() }).ToList();

            }
            #endregion
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return View(objWorkOrderVM);
        }
        private void GetAssetGroupHeaderName(WorkOrderModel objWOModel)
        {

            objWOModel.AssetGroup1Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? AssetGroupConstants.AssetGroup1 : this.userData.Site.AssetGroup1Name;
            objWOModel.AssetGroup2Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? AssetGroupConstants.AssetGroup2 : this.userData.Site.AssetGroup2Name;
            objWOModel.AssetGroup3Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? AssetGroupConstants.AssetGroup3 : this.userData.Site.AssetGroup3Name;
        }

        #region New code implementation for grid load and advance search
        public string GetWorkOrderMaintGrid(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, DateTime? CompleteStartDateVw = null,
            DateTime? CompleteEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null,
           string workorder = "", string description = "", string Chargeto = "", string Chargetoname = "", string AssetLocation = "", List<string> types = null,
           List<string> status = null,
           List<string> shifts = null, string AssetGroup1ClientLookUpId = "", string AssetGroup2ClientLookUpId = "", string AssetGroup3ClientLookUpId = "",
           List<string> priorities = null, DateTime? StartCreateDate = null, DateTime? EndCreateDate = null, string creator = "", string assigned = "",
           DateTime? StartScheduledDate = null, DateTime? EndScheduledDate = null, List<string> failcodes = null,
           DateTime? StartActualFinishDate = null, DateTime? EndActualFinishDate = null, string txtSearchval = "", string personnelList = "",
           string ActualDuration = "", List<string> sourcetypes = null, List<string> AssetGroup1Ids = null, List<string> AssetGroup2Ids = null,
           List<string> AssetGroup3Ids = null
           , string Order = "1"
            , bool? downRequired = null,
           List<string> assignedwo = null,
           DateTime? requiredDate = null,
           List<string> planner = null, List<string> projectIds = null
            )//WO Sorting
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<string> statusList = new List<string>();
            decimal decActualuration = 0;
            string _startcreateddate = string.Empty;
            string _endcreateddate = string.Empty;
            string _startscheduled = string.Empty;
            string _endscheduled = string.Empty;
            string _startactualFinish = string.Empty;
            string _endactualFinish = string.Empty;
            string _CompleteStartDateVw = string.Empty;
            string _CompleteEndDateVw = string.Empty;
            string _CreateStartDateVw = string.Empty;
            string _CreateEndDateVw = string.Empty;
            string _RequiredDate = string.Empty; //V2-984

            //V2-347
            _CompleteStartDateVw = CompleteStartDateVw.HasValue ? CompleteStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _CompleteEndDateVw = CompleteEndDateVw.HasValue ? CompleteEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startcreateddate = StartCreateDate.HasValue ? StartCreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endcreateddate = EndCreateDate.HasValue ? EndCreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startscheduled = StartScheduledDate.HasValue ? StartScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endscheduled = EndScheduledDate.HasValue ? EndScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startactualFinish = StartActualFinishDate.HasValue ? StartActualFinishDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endactualFinish = EndActualFinishDate.HasValue ? EndActualFinishDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            //V2-364
            _CreateStartDateVw = CreateStartDateVw.HasValue ? CreateStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _CreateEndDateVw = CreateEndDateVw.HasValue ? CreateEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _RequiredDate = requiredDate.HasValue ? requiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty; //V2-984
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            if (!string.IsNullOrEmpty(ActualDuration))
            {
                decActualuration = decimal.TryParse(ActualDuration, out decActualuration) == true ? decActualuration : -1;
            }
            else
                decActualuration = -1;
            List<WorkOrderModel> woMaintMasterList = woWrapper.populateWODetails(CustomQueryDisplayId,
                skip, length ?? 0, Order, orderDir, _CompleteStartDateVw, _CompleteEndDateVw, _CreateStartDateVw, _CreateEndDateVw, workorder,
                description, Chargeto, Chargetoname, AssetLocation, types, status, shifts, AssetGroup1ClientLookUpId, AssetGroup2ClientLookUpId,
                AssetGroup3ClientLookUpId, priorities, _startcreateddate, _endcreateddate,
                creator, assigned, _startscheduled, _endscheduled, failcodes, _startactualFinish, _endactualFinish, txtSearchval, personnelList,
                decActualuration, sourcetypes, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids, downRequired, assignedwo, _RequiredDate, planner,projectIds);


            var totalRecords = 0;
            var recordsFiltered = 0;

            recordsFiltered = woMaintMasterList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = woMaintMasterList.Select(o => o.TotalCount).FirstOrDefault();

            var filteredResult = woMaintMasterList
                .ToList();


            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, /*lookupLists = lookupLists*/ }, JsonSerializerDateSettings);
        }

        #region Populate Popup for schedule

        public JsonResult PopulatePopUpJs(long workOrderId = 0)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrderModel workOrderModel = new WorkOrderModel();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            IEnumerable<SelectListItem> AssignedList;
            IEnumerable<SelectListItem> PersonnelList;
            PopulatePopUp(out AssignedList, out PersonnelList, workOrderId);
            objWorkOrderVM.AssignedList = AssignedList;
            objWorkOrderVM.PersonnelList = PersonnelList;
            return Json(new { AssignedList = objWorkOrderVM.AssignedList, PersonnelList = objWorkOrderVM.PersonnelList }, JsonRequestBehavior.AllowGet);
        }

        private void PopulatePopUp(out IEnumerable<SelectListItem> AssignedList, out IEnumerable<SelectListItem> PersonnelList, long workOrderId = 0)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrderModel workOrderModel = new WorkOrderModel();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            var totalList = woWrapper.WOSchedulePersonnelList(Convert.ToString(workOrderId));
            AssignedList = totalList[1].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            PersonnelList = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
        }

        #endregion

        #region Hover for Assigned user
        [HttpPost]
        public JsonResult PopulateHover(long workOrderId = 0)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            string personnelList = woWrapper.PopulateHoverList(workOrderId);
            if (!string.IsNullOrEmpty(personnelList))
            {
                personnelList = personnelList.Trim().TrimEnd(',');
            }
            return Json(new { personnelList = personnelList }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion


        #region Card-View
        public PartialViewResult GetCardViewData(int currentpage, int? start, int? length, string currentorderedcolumn,
            string currentorder, int CustomQueryDisplayId = 0, DateTime? CompleteStartDateVw = null, DateTime? CompleteEndDateVw = null,
            DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null,
           string workorder = "", string description = "", string Chargeto = "", string Chargetoname = "", string AssetLocation = "", List<string> types = null,
            List<string> status = null, List<string> shifts = null, string AssetGroup1ClientLookUpId = "", string AssetGroup2ClientLookUpId = "",
           string AssetGroup3ClientLookUpId = "", List<string> priorities = null, DateTime? StartCreateDate = null, DateTime? EndCreateDate = null,
           string creator = "", string assigned = "", DateTime? StartScheduledDate = null, DateTime? EndScheduledDate = null,
            List<string> failcodes = null, DateTime? StartActualFinishDate = null, DateTime? EndActualFinishDate = null, string txtSearchval = "",
           string personnelList = "", string ActualDuration = "", List<string> sourcetypes = null, List<string> AssetGroup1Ids = null,
           List<string> AssetGroup2Ids = null, List<string> AssetGroup3Ids = null, bool? downRequired = null,
           List<string> assignedwo = null,
           DateTime? requiredDate = null, List<string> planner = null, List<string> projectIds = null)
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            WorkOrderModel woModel = new WorkOrderModel();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            ClientSetUpWrapper clientSetUpWrapper = new ClientSetUpWrapper(userData);
            WoCompletionSettingsModel completionSettingsModel = new WoCompletionSettingsModel();
            completionSettingsModel = clientSetUpWrapper.CompletionSettingsDetails();

            objWorkOrderVM.UseWOCompletionWizard = completionSettingsModel.UseWOCompletionWizard;

            decimal decActualuration = 0;
            objWorkOrderVM._userdata = this.userData;

            string _startcreateddate = string.Empty;
            string _endcreateddate = string.Empty;
            string _startscheduled = string.Empty;
            string _endscheduled = string.Empty;
            string _startactualFinish = string.Empty;
            string _endactualFinish = string.Empty;
            string _CompleteStartDateVw = string.Empty;
            string _CompleteEndDateVw = string.Empty;
            //V2-364
            string _CreateStartDateVw = string.Empty;
            string _CreateEndDateVw = string.Empty;
            string _RequiredDate = string.Empty; //V2-984
            //v2-347
            _CompleteStartDateVw = CompleteStartDateVw.HasValue ? CompleteStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _CompleteEndDateVw = CompleteEndDateVw.HasValue ? CompleteEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startcreateddate = StartCreateDate.HasValue ? StartCreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endcreateddate = EndCreateDate.HasValue ? EndCreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startscheduled = StartScheduledDate.HasValue ? StartScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endscheduled = EndScheduledDate.HasValue ? EndScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startactualFinish = StartActualFinishDate.HasValue ? StartActualFinishDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endactualFinish = EndActualFinishDate.HasValue ? EndActualFinishDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            //V2-364
            _CreateStartDateVw = CreateStartDateVw.HasValue ? CreateStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _CreateEndDateVw = CreateEndDateVw.HasValue ? CreateEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _RequiredDate = requiredDate.HasValue ? requiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty; //V2-984
            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;
            if (!string.IsNullOrEmpty(ActualDuration))
            {
                decActualuration = decimal.TryParse(ActualDuration, out decActualuration) == true ? decActualuration : -1;
            }
            else
                decActualuration = -1;
            List<WorkOrderModel> cardData = woWrapper.populateWODetails(CustomQueryDisplayId,
                          skip, length ?? 0, currentorderedcolumn, currentorder, _CompleteStartDateVw, _CompleteEndDateVw, _CreateStartDateVw,
                          _CreateEndDateVw, workorder, description, Chargeto, Chargetoname, AssetLocation, types, status, shifts, AssetGroup1ClientLookUpId,
                          AssetGroup2ClientLookUpId, AssetGroup3ClientLookUpId, priorities, _startcreateddate,
                         _endcreateddate, creator, assigned, _startscheduled, _endscheduled, failcodes, _startactualFinish, _endactualFinish,
                         txtSearchval, personnelList, decActualuration, sourcetypes, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids, downRequired, assignedwo, _RequiredDate, planner,projectIds);
            var recordsFiltered = 0;
            recordsFiltered = cardData.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = cardData.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            var filteredResult = cardData.ToList();

            Parallel.ForEach(cardData, item =>
            {
                item.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
                item.AzureImageURL = WorkOrderImageUrl(item.WorkOrderId);
                item.security = this.userData.Security;
            });
            objWorkOrderVM.workOrderModelList = cardData;
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_WoGridCardView.cshtml", objWorkOrderVM);
        }
        #endregion

        [HttpGet]
        public string GetWorkOrderPrintData(string _colname, string _coldir, int _CustomQueryDisplayId = 0, DateTime? _CompleteStartDateVw = null, DateTime? _CompleteEndDateVw = null, DateTime? _CreateStartDateVw = null, DateTime? _CreateEndDateVw = null, string _ClientLookupId = "", string _Description = "", string _ChargeToClientLookupId = "", string _ChargeToName = "", string _AssetLocation = "",
                                  List<string> _Type = null, List<string> _Status = null, List<string> _Shift = null, string _AssetGroup1ClientLookUpId = null, string _AssetGroup2ClientLookUpId = null, string _AssetGroup3ClientLookUpId = null, List<string> _Priority = null, DateTime? _StartCreateDate = null, DateTime? _EndCreateDate = null, string _Creator = "", string _Assigned = "", DateTime? _StartScheduledDate = null,
                                  DateTime? _EndScheduledDate = null, List<string> _Failcode = null, DateTime? _StartActualFinishDate = null, DateTime? _EndActualFinishDate = null, string _txtSearchval = "", List<string> _SourceType = null, string ActualDuration = "", List<string> _AssetGroup1Id = null, List<string> _AssetGroup2Id = null, List<string> _AssetGroup3Id = null, string _personnelList = "", bool? _downRequired = null, List<string> _assignedwo = null,
           DateTime? _requiredDate = null, List<string> _planner = null, List<string> _projectIds = null)
        {

            List<WorkOrderPrintModel> WorkOrderPrintModelList = new List<WorkOrderPrintModel>();
            WorkOrderPrintModel objWorkOrderPrintModel;
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrderPrintModel workOrderPrintModel = new WorkOrderPrintModel();
            decimal decActualuration = 0;
            if (!string.IsNullOrEmpty(ActualDuration))
            {
                decActualuration = decimal.TryParse(ActualDuration, out decActualuration) == true ? decActualuration : -1;
            }
            else
                decActualuration = -1;
            #region V2-828
            string _startcreateddate1 = string.Empty;
            string _endcreateddate1 = string.Empty;
            string _startscheduled1 = string.Empty;
            string _endscheduled1 = string.Empty;
            string _startactualFinish1 = string.Empty;
            string _endactualFinish1 = string.Empty;
            string _CompleteStartDateVw1 = string.Empty;
            string _CompleteEndDateVw1 = string.Empty;
            string _CreateStartDateVw1 = string.Empty;
            string _CreateEndDateVw1 = string.Empty;
            string _RequiredDate = string.Empty; //V2-984
            //V2-347
            _CompleteStartDateVw1 = _CompleteStartDateVw.HasValue ? _CompleteStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _CompleteEndDateVw1 = _CompleteEndDateVw.HasValue ? _CompleteEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startcreateddate1 = _CreateStartDateVw.HasValue ? _CreateStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endcreateddate1 = _CreateEndDateVw.HasValue ? _CreateEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startscheduled1 = _StartScheduledDate.HasValue ? _StartScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endscheduled1 = _EndScheduledDate.HasValue ? _EndScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _startactualFinish1 = _StartActualFinishDate.HasValue ? _StartActualFinishDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _endactualFinish1 = _EndActualFinishDate.HasValue ? _EndActualFinishDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            //V2-364
            _CreateStartDateVw1 = _StartCreateDate.HasValue ? _StartCreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _CreateEndDateVw1 = _EndCreateDate.HasValue ? _EndCreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _RequiredDate = _requiredDate.HasValue ? _requiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty; //V2-984
            List<WorkOrderModel> woMaintMasterList = woWrapper.populateWODetails(_CustomQueryDisplayId,
                0, 100000, _colname, _coldir, _CompleteStartDateVw1, _CompleteEndDateVw1, _startcreateddate1, _endcreateddate1, _ClientLookupId,
                _Description, _ChargeToClientLookupId, _ChargeToName, _AssetLocation, _Type, _Status, _Shift, _AssetGroup1ClientLookUpId, _AssetGroup2ClientLookUpId,
                _AssetGroup3ClientLookUpId, _Priority, _CreateStartDateVw1, _CreateEndDateVw1,
                _Creator, _Assigned, _startscheduled1, _endscheduled1, _Failcode, _startactualFinish1, _endactualFinish1, _txtSearchval, _personnelList,
                decActualuration, _SourceType, _AssetGroup1Id, _AssetGroup2Id, _AssetGroup3Id, _downRequired, _assignedwo, _RequiredDate, _planner,_projectIds);
            #endregion
            if (woMaintMasterList != null)
            {
                foreach (var v in woMaintMasterList)
                {
                    objWorkOrderPrintModel = new WorkOrderPrintModel();
                    objWorkOrderPrintModel.ClientLookupId = v.ClientLookupId;
                    objWorkOrderPrintModel.Description = v.Description;
                    objWorkOrderPrintModel.ChargeToClientLookupId = v.ChargeToClientLookupId;
                    objWorkOrderPrintModel.ChargeTo_Name = v.ChargeTo_Name;
                    objWorkOrderPrintModel.AssetLocation = v.AssetLocation;
                    objWorkOrderPrintModel.Type = v.Type;
                    objWorkOrderPrintModel.Status = v.Status;
                    objWorkOrderPrintModel.Shift = v.Shift;
                    objWorkOrderPrintModel.AssetGroup1ClientlookupId = v.AssetGroup1ClientlookupId;
                    objWorkOrderPrintModel.AssetGroup2ClientlookupId = v.AssetGroup2ClientlookupId;
                    objWorkOrderPrintModel.AssetGroup3ClientlookupId = v.AssetGroup3ClientlookupId;
                    //<!--(Added on 25/06/2020)-->
                    objWorkOrderPrintModel.AssetGroup1Id = v.AssetGroup1Id;
                    objWorkOrderPrintModel.AssetGroup2Id = v.AssetGroup2Id;
                    objWorkOrderPrintModel.AssetGroup3Id = v.AssetGroup3Id;
                    //<!--(Added on 25/06/2020)-->
                    objWorkOrderPrintModel.Priority = v.Priority;
                    objWorkOrderPrintModel.CreateDate = v.CreateDate;
                    objWorkOrderPrintModel.Creator = v.Creator;
                    objWorkOrderPrintModel.Assigned = v.Assigned;
                    objWorkOrderPrintModel.ScheduledStartDate = v.ScheduledStartDate;
                    objWorkOrderPrintModel.FailureCode = v.FailureCode;
                    objWorkOrderPrintModel.ActualFinishDate = v.ActualFinishDate;
                    objWorkOrderPrintModel.ActualDuration = (v.ActualDuration == null) ? 0 : (decimal)v.ActualDuration;
                    objWorkOrderPrintModel.SourceType = v.SourceType;//<!--Added on 23/06/2020-->
                    objWorkOrderPrintModel.RequiredDate = v.RequiredDate;
                    objWorkOrderPrintModel.AssignedFullName = v.AssignedFullName;
                    objWorkOrderPrintModel.WorkOrderId = v.WorkOrderId;
                    objWorkOrderPrintModel.ProjectClientLookupId = v.ProjectClientLookupId;//V2-850
                    objWorkOrderPrintModel.DownRequired = v.DownRequired;//V2-892
                    objWorkOrderPrintModel.PlannerFullName = v.PlannerFullName;//V2-1078
                    WorkOrderPrintModelList.Add(objWorkOrderPrintModel);
                }
                #region V2-829
                Parallel.ForEach(WorkOrderPrintModelList, item =>
                {
                    if (item.AssignedFullName == "Multiple")
                    {
                        string personnelList = woWrapper.PopulateHoverList(item.WorkOrderId);
                        if (!string.IsNullOrEmpty(personnelList))
                        {
                            item.AssignedFullName = personnelList.Trim().TrimEnd(',');
                        }
                    }
                });
                #endregion
            }
            return JsonConvert.SerializeObject(new { data = WorkOrderPrintModelList }, JsonSerializerDateSettings);
        }

        public PartialViewResult WorkOrderDetails(long workOrderId, bool IsWorkOrderFromEquipment = false)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WoSendForApprovalModel woSendForApprovalModel = new WoSendForApprovalModel();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataModel> Account = new List<DataModel>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            WorkOrder workOrder = new WorkOrder();
            WorkOrderUDF workOrderUDF = new WorkOrderUDF();
            ClientSetUpWrapper clientSetUpWrapper = new ClientSetUpWrapper(userData);
            WoCompletionSettingsModel completionSettingsModel = new WoCompletionSettingsModel();

            //V2-726 Start
            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            var ismaterialrequest = woWrapper.RetrieveApprovalGroupMaterialRequestStatus("MaterialRequests");
            var isworkrequest = woWrapper.RetrieveApprovalGroupMaterialRequestStatus("WorkRequests");
            if (isworkrequest)
            {
                approvalRouteModel = SendWRForApproval(workOrderId);
            }
            approvalRouteModel.IsWorkRequest = isworkrequest;
            approvalRouteModel.IsMaterialRequest = ismaterialrequest;
            //V2 726 End

            #region V2-730
            //V2-730 Start
            objWorksOrderVM.IsWorkRequestApproval = isworkrequest;
            ApprovalRouteModelByObjectId approvalRouteModelByObjectId = new ApprovalRouteModelByObjectId();
            List<ApprovalRouteModelByObjectId> approvalRouteModelByObjectIdList = woWrapper.RetrieveApprovalGroupIdbyObjectId(userData.DatabaseKey.Personnel.PersonnelId, workOrderId, ApprovalGroupRequestTypes.WorkRequest);
            if (approvalRouteModelByObjectIdList != null && approvalRouteModelByObjectIdList.Count > 0)
            {
                objWorksOrderVM.IsWorkRequestApprovalAccessCheck = true;
                approvalRouteModelByObjectId.ApprovalGroupId = approvalRouteModelByObjectIdList[0].ApprovalGroupId;
            }
            else
            {
                objWorksOrderVM.IsWorkRequestApprovalAccessCheck = false;
                approvalRouteModelByObjectId.ApprovalGroupId = 0;
            }
            //V2-730 End
            #endregion

            Task t1 = Task.Factory.StartNew(() => workOrder = woWrapper.getWorkOderDetailsById_V2(workOrderId));
            Task t2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            Task t3 = Task.Factory.StartNew(() => Account = woWrapper.GetLookupList_Account());
            Task t4 = Task.Factory.StartNew(() => workOrderUDF = woWrapper.getWorkOrderUDFByWorkOrderId(workOrderId));
            Task t6 = Task.Factory.StartNew(() => objWorksOrderVM.attachmentCount = commonWrapper.AttachmentCount(workOrderId,
                AttachmentTableConstant.WorkOrder, userData.Security.WorkOrders.Edit));
            Task completionSettingsTask = Task.Factory.StartNew(() => completionSettingsModel = clientSetUpWrapper.CompletionSettingsDetails());
            Task.WaitAll(t1, t2, t3, t4, t6, completionSettingsTask);

            objWorksOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                    .Retrieve(DataDictionaryViewNameConstant.ViewWorkOrderWidget, userData);
            objWorksOrderVM.workOrderModel = woWrapper.initializeControls(workOrder);
            objWorksOrderVM.UseWOCompletionWizard = completionSettingsModel.UseWOCompletionWizard;
            if (AllLookUps != null)
            {
                if (objWorksOrderVM.workOrderModel.SourceType == WorkOrderSourceTypes.Emergency)
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                }
                else if (objWorksOrderVM.workOrderModel.SourceType == WorkOrderSourceTypes.OnDemand)
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                }
                else if (objWorksOrderVM.workOrderModel.SourceType == WorkOrderSourceTypes.WorkRequest)
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE).ToList();
                }
                else
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                }

                if (Type != null && Type.Any(cus => cus.ListValue == objWorksOrderVM.workOrderModel.Type))
                {
                    objWorksOrderVM.workOrderModel.Type = Type.Where(x => x.ListValue == objWorksOrderVM.workOrderModel.Type).Select(x => x.Description).First();
                }
                var Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (Shift != null && Shift.Any(cus => cus.ListValue == objWorksOrderVM.workOrderModel.Shift))
                {
                    objWorksOrderVM.workOrderModel.Shift = Shift.Where(x => x.ListValue == objWorksOrderVM.workOrderModel.Shift).Select(x => x.Description).First();
                }
                var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
                if (Priority != null && Priority.Any(cus => cus.ListValue == objWorksOrderVM.workOrderModel.Priority))
                {
                    objWorksOrderVM.workOrderModel.Priority = Priority.Where(x => x.ListValue == objWorksOrderVM.workOrderModel.Priority).Select(x => x.Description).First();
                }
                var Failure = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_FAILURE).ToList();
                if (Failure != null && Failure.Any(cus => cus.ListValue == objWorksOrderVM.workOrderModel.FailureCode))
                {
                    objWorksOrderVM.workOrderModel.FailureCode = Failure.Where(x => x.ListValue == objWorksOrderVM.workOrderModel.FailureCode).Select(x => x.Description).First();
                }
                var CancelLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_Task_Cancel).ToList();
                if (CancelLookUpList != null)
                {
                    objWorksOrderVM.woTaskModel = new WoTaskModel();
                    objWorksOrderVM.woTaskModel.CancelReasonList = CancelLookUpList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                }
                var CancelLookUpListWo = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
                if (CancelLookUpListWo != null)
                {
                    objWorksOrderVM.CancelReasonListWo = CancelLookUpListWo.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
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
                woScheduleModel.Schedulestartdate = objWorksOrderVM.workOrderModel.ScheduledStartDate;

            }
            if (Account != null && Account.Any(cus => cus.AccountId == objWorksOrderVM.workOrderModel.Labor_AccountId))
            {
                objWorksOrderVM.workOrderModel.strLabor_AccountId = Account.Where(x => x.AccountId == objWorksOrderVM.workOrderModel.Labor_AccountId).Select(x => x.Account).First();
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
            if (ScheduleChargeTypeList != null && ScheduleChargeTypeList.Any(cus => cus.value == objWorksOrderVM.workOrderModel.ChargeType))
            {
                objWorksOrderVM.workOrderModel.ChargeType = ScheduleChargeTypeList.Where(x => x.value == objWorksOrderVM.workOrderModel.ChargeType).Select(x => x.text).First();
            }
            var ChargeTypeLookUpList = PopulatelookUpListByType(objWorksOrderVM.workOrderModel.ChargeType);
            if (ChargeTypeLookUpList != null && ChargeTypeLookUpList.Any(cus => cus.ChargeToId == objWorksOrderVM.workOrderModel.ChargeToId))
            {
                objWorksOrderVM.workOrderModel.ChargeToClientLookupId = ChargeTypeLookUpList.Where(x => x.ChargeToId == objWorksOrderVM.workOrderModel.ChargeToId).Select(co => co.ChargeToClientLookupId).First();
                objWorksOrderVM.workOrderModel.ChargeTo_Name = ChargeTypeLookUpList.Where(x => x.ChargeToId == objWorksOrderVM.workOrderModel.ChargeToId).Select(co => co.Name).First();
            }
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(objWorksOrderVM.workOrderModel.WorkOrderId, objWorksOrderVM.workOrderModel.ClientLookupId, objWorksOrderVM.workOrderModel.Status, objWorksOrderVM.workOrderModel.Description, objWorksOrderVM.workOrderModel.ProjectClientLookupId, objWorksOrderVM.workOrderModel);
            objWorksOrderVM.workOrderSummaryModel.Type = objWorksOrderVM.workOrderModel.Type;
            objWorksOrderVM.workOrderSummaryModel.Priority = objWorksOrderVM.workOrderModel.Priority;
            objWorksOrderVM.workOrderSummaryModel.ChargeToClientLookupId = objWorksOrderVM.workOrderModel.ChargeToClientLookupId;
            objWorksOrderVM.workOrderSummaryModel.ChargeTo_Name = objWorksOrderVM.workOrderModel.ChargeTo_Name;
            objWorksOrderVM.workOrderSummaryModel.AssetLocation = objWorksOrderVM.workOrderModel.AssetLocation;
            objWorksOrderVM.workOrderSummaryModel.ScheduledStartDate = objWorksOrderVM.workOrderModel.ScheduledStartDate;
            objWorksOrderVM.workOrderSummaryModel.ScheduledDuration = objWorksOrderVM.workOrderModel.ScheduledDuration;

            objWorksOrderVM.workOrderSummaryModel.AssignedFullName = objWorksOrderVM.workOrderModel.AssignedFullName;
            objWorksOrderVM.workOrderSummaryModel.Assigned = objWorksOrderVM.workOrderModel.Assigned;
            objWorksOrderVM.workOrderSummaryModel.CompleteDate = objWorksOrderVM.workOrderModel.CompleteDate;
            objWorksOrderVM.workOrderSummaryModel.security = this.userData.Security;
            objWorksOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = objWorksOrderVM.workOrderModel.WorkAssigned_PersonnelId;
            objWorksOrderVM.workOrderSummaryModel.IsDetail = true;
            //V2-463
            objWorksOrderVM.workOrderSummaryModel.EquipDownDate = objWorksOrderVM.workOrderModel.EquipDownDate != null ? objWorksOrderVM.workOrderModel.EquipDownDate : null;
            objWorksOrderVM.workOrderSummaryModel.EquipDownHours = objWorksOrderVM.workOrderModel.EquipDownHours;
            //V2-626
            //#region Follow Up

            #region//*****V2-847

            objWorksOrderVM.workOrderSummaryModel.AssetGroup1Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? AssetGroupConstants.AssetGroup1 : this.userData.Site.AssetGroup1Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup2Name) ? AssetGroupConstants.AssetGroup2 : this.userData.Site.AssetGroup2Name;

            objWorksOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = workOrder.AssetGroup1ClientlookupId;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = workOrder.AssetGroup2ClientlookupId;

            #endregion//*****

            objWorksOrderVM.woRequestModel = new WoRequestModel();
            objWorksOrderVM.woRequestModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
            objWorksOrderVM.woRequestModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });//.Where(x => x.Text != "Location");
            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            objWorksOrderVM.woRequestModel.ChargeToList = defaultChargeToList;
            objWorksOrderVM.woRequestModel.WorkOrderId = objWorksOrderVM.workOrderModel.WorkOrderId;

            objWorksOrderVM._userdata = this.userData;
            objWorksOrderVM.security = this.userData.Security;
            objWorksOrderVM.IsMaintOnDemand = userData.Site.MaintOnDemand;
            objWorksOrderVM.downtimeModel = new DowntimeModel();
            if (objWorksOrderVM.workOrderModel.EquipDownDate != null)
            {
                objWorksOrderVM.downtimeModel.Downdate = objWorksOrderVM.workOrderModel.EquipDownDate;
                objWorksOrderVM.downtimeModel.Minutes = Convert.ToDecimal(objWorksOrderVM.workOrderModel.EquipDownHours.ToString("0.00"));
            }
            objWorksOrderVM.downtimeModel.WorkOrderId = workOrderId;
            ViewBag.SecurityEstimate = userData.Security.WorkOrders.Edit;

            objWorksOrderVM.workOrderModel.PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToLower();
            objWorksOrderVM.woScheduleModel = woScheduleModel;

            objWorksOrderVM.woSendForApprovalModel = woSendForApprovalModel;

            objWorksOrderVM.ViewWorkOrderModel = new ViewWorkOrderModelDynamic();
            objWorksOrderVM.ViewWorkOrderModel = woWrapper.MapWorkOrderDataForView(objWorksOrderVM.ViewWorkOrderModel, workOrder);
            objWorksOrderVM.ViewWorkOrderModel = woWrapper.MapWorkOrderUDFDataForView(objWorksOrderVM.ViewWorkOrderModel, workOrderUDF);

            objWorksOrderVM.ViewWorkOrderModel.Type = objWorksOrderVM.workOrderModel.Type;
            objWorksOrderVM.ViewWorkOrderModel.Shift = objWorksOrderVM.workOrderModel.Shift;
            objWorksOrderVM.ViewWorkOrderModel.Priority = objWorksOrderVM.workOrderModel.Priority;
            objWorksOrderVM.ViewWorkOrderModel.ChargeTo_Name = objWorksOrderVM.workOrderModel.ChargeTo_Name;
            objWorksOrderVM.ViewWorkOrderModel.FailureCode = objWorksOrderVM.workOrderModel.FailureCode;
            objWorksOrderVM.ViewWorkOrderModel.ReasonNotDone = workOrder.ReasonNotDone;
            //V2-676
            objWorksOrderVM.WOBarcode = userData.Site.WOBarcode;
            //V2-726
            objWorksOrderVM.ApprovalRouteModel = approvalRouteModel;
            objWorksOrderVM.ApprovalRouteModelByObjectId = approvalRouteModelByObjectId;
            objWorksOrderVM.workOrderModel.IsUseMultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;//V2-732
            objWorksOrderVM.IsAddWorkOrderFromEquipment = IsWorkOrderFromEquipment;//V2-780            
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Workorder/_WorkOrderDetails.cshtml", objWorksOrderVM);
        }
        public RedirectResult DetailFromEquipment(long workOrderId)
        {
            TempData["Mode"] = "DetailFromEquipment";
            string wOrderString = Convert.ToString(workOrderId);
            TempData["workOrderId"] = wOrderString;
            return Redirect("/WorkOrder/Index?page=Maintenance_Work_Order_Search");
        }
        public ActionResult DetailFromDashboard(long workOrderId)
        {
            TempData["Mode"] = "DetailFromDashboard";
            string wOrderString = Convert.ToString(workOrderId);
            TempData["workOrderId"] = wOrderString;
            return Redirect("/WorkOrder/Index?page=Maintenance_Work_Order_Search");
        }
        #region V2-1136
        public ActionResult DetailFromNotification(long workOrderId ,string alertName)
        {
            TempData["Mode"] = "DetailFromNotification";
            TempData["AlertName"] = alertName;
            string wOrderString = Convert.ToString(workOrderId);
            TempData["workOrderId"] = wOrderString;
            return Redirect("/WorkOrder/Index?page=Maintenance_Work_Order_Search");
        }
        #endregion
        public PartialViewResult AddWorkOrdersOld()
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrderModel objWoModel = new WorkOrderModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (Shift != null)
                {
                    var tmpShift = Shift.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objWoModel.ShiftList = tmpShift.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }
                var Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objWoModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
                var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority);
                if (Priority != null)
                {
                    var tmpPriority = Priority.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objWoModel.PriorityList = tmpPriority.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).Distinct().ToList();
                }
                var Failure = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_FAILURE);
                if (Failure != null)
                {
                    var tmpFailureList = Failure.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objWoModel.FailureList = tmpFailureList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                }
            }
            var Account = woWrapper.GetLookupList_Account();
            if (Account != null)
            {
                objWoModel.AccountLookUpList = Account.Select(x => new SelectListItem { Text = x.Account, Value = x.AccountId.ToString() });
            }
            var ChargeTypeLookUpList = PopulatelookUpListByType("");
            if (ChargeTypeLookUpList != null)
            {
                objWoModel.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
            }
            var PersonnelLookUplist = GetList_PersonnelV2();
            if (PersonnelLookUplist != null)
            {
                objWoModel.WorkAssignedLookUpList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            objWoModel.PlantLocation = userData.Site.PlantLocation;
            objWorksOrderVM.workOrderModel = objWoModel;
            objWorksOrderVM._userdata = this.userData;
            objWorksOrderVM.security = this.userData.Security;
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_WorkOrderAdd.cshtml", objWorksOrderVM);
        }
        public PartialViewResult AddWorkOrders()
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrderModel objWoModel = new WorkOrderModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();

            objWorksOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.AddWorkOrder, userData);
            IList<string> LookupNames = objWorksOrderVM.UIConfigurationDetails.ToList()
                                       .Where(x => x.LookupType == DataDictionaryLookupTypeConstant.LookupList && !string.IsNullOrEmpty(x.LookupName))
                                       .Select(s => s.LookupName)
                                       .ToList();
            if (LookupNames.Contains(LookupListConstants.WO_Priority))
            {
                objWorksOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority)
                                                     .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                     .Select(s => new WOAddUILookupList
                                                     { text = s.Description, value = s.ListValue, lookupname = s.ListName })
                                                     .ToList());
                LookupNames.Remove(LookupListConstants.WO_Priority);
            }

            objWorksOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                 .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                      .Select(s => new WOAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList());
            var SourceTypeList = UtilityFunction.PopulateSourceTypeList();
            if (SourceTypeList != null)
            {
                objWorksOrderVM.SourceTypeList = SourceTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var StatusList = commonWrapper.GetListFromConstVals(LookupListConstants.WO_Status);
            if (StatusList != null)
            {
                objWorksOrderVM.StatusList = StatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            objWorksOrderVM.PlantLocation = userData.Site.PlantLocation;
            objWorksOrderVM.IsAddWorkOrderDynamic = true;
            objWorksOrderVM._userdata = this.userData;
            objWorksOrderVM.security = this.userData.Security;
            objWorksOrderVM.IsMaintOnDemand = userData.Site.MaintOnDemand;
            objWorksOrderVM.AddWorkorder = new AddWorkOrderModelDynamic();

            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_WorkOrderAddDynamic.cshtml", objWorksOrderVM);
        }
        public ActionResult Add()
        {
            TempData["Mode"] = "addWorkOrderDynamic";
            return Redirect("/WorkOrder/Index?page=Maintenance_Work_Order_Search");
        }
        public PartialViewResult EditWorkOrder(long workOrderId, string Assigned, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Failure = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Priority = new List<DataContracts.LookupList>();
            List<DataModel> Account = new List<DataModel>();

            Task t1 = Task.Factory.StartNew(() => objWorksOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(workOrderId));
            Task t2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            //V2-379
            Task t3 = Task.Factory.StartNew(() => Account = GetAccountByActiveState(true));
            Task.WaitAll(t1, t2);
            if (objWorksOrderVM.workOrderModel.RequiredDate != null && objWorksOrderVM.workOrderModel.RequiredDate.Value == default(DateTime))
            {
                objWorksOrderVM.workOrderModel.RequiredDate = null;
            }
            #region dropdown
            if (AllLookUps != null)
            {
                if (objWorksOrderVM.workOrderModel.SourceType == WorkOrderSourceTypes.PreventiveMaint)
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_WO_Type).ToList();
                }
                else
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                }
                Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                Failure = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_FAILURE).ToList();
                Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
            }
            if (Shift != null)
            {
                var tmpShift = Shift.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                objWorksOrderVM.workOrderModel.ShiftList = tmpShift.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
            }
            if (Type != null)
            {
                var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                objWorksOrderVM.workOrderModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
            }
            if (Priority != null)
            {
                var tmpPriority = Priority.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                objWorksOrderVM.workOrderModel.PriorityList = tmpPriority.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).Distinct().ToList();
            }
            if (Failure != null)
            {
                var tmpFailureList = Failure.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                objWorksOrderVM.workOrderModel.FailureList = tmpFailureList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
            }
            t3.Wait();
            if (Account != null)
            {
                objWorksOrderVM.workOrderModel.AccountLookUpList = Account.Select(x => new SelectListItem { Text = x.Account, Value = x.AccountId.ToString() });
            }

            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
            if (ScheduleChargeTypeList != null)
            {
                objWorksOrderVM.workOrderModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ChargeTypeLookUpList = PopulatelookUpListByType(objWorksOrderVM.workOrderModel.ChargeType);
            if (ChargeTypeLookUpList != null)
            {
                objWorksOrderVM.workOrderModel.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
            }
            #endregion
            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorksOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            objWorksOrderVM.workOrderModel.PlantLocation = userData.Site.PlantLocation;

            var totalList = woWrapper.WOSchedulePersonnelList();
            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorksOrderVM.woScheduleModel = woScheduleModel;
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(objWorksOrderVM.workOrderModel.WorkOrderId, objWorksOrderVM.workOrderModel.ClientLookupId, objWorksOrderVM.workOrderModel.Status, objWorksOrderVM.workOrderModel.Description, objWorksOrderVM.workOrderModel.ProjectClientLookupId, objWorksOrderVM.workOrderModel);
            objWorksOrderVM.security = this.userData.Security;
            objWorksOrderVM.workOrderSummaryModel.Type = objWorksOrderVM.workOrderModel.Type;
            objWorksOrderVM.workOrderSummaryModel.Priority = objWorksOrderVM.workOrderModel.Priority;
            objWorksOrderVM.workOrderSummaryModel.ChargeToClientLookupId = objWorksOrderVM.workOrderModel.ChargeToClientLookupId;
            objWorksOrderVM.workOrderSummaryModel.ChargeTo_Name = objWorksOrderVM.workOrderModel.ChargeTo_Name;
            objWorksOrderVM.workOrderSummaryModel.ScheduledStartDate = objWorksOrderVM.workOrderModel.ScheduledStartDate;
            objWorksOrderVM.workOrderSummaryModel.ScheduledDuration = objWorksOrderVM.workOrderModel.ScheduledDuration;

            objWorksOrderVM.workOrderSummaryModel.CompleteDate = objWorksOrderVM.workOrderModel.CompleteDate;
            objWorksOrderVM.workOrderSummaryModel.security = this.userData.Security;
            objWorksOrderVM._userdata = this.userData;
            objWorksOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorksOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorksOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;

            //V2-463
            objWorksOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorksOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_WorkOrderAdd.cshtml", objWorksOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddWorkOrders(WorkOrderVM workOrderVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                WorkOrder objWorkOrder = new DataContracts.WorkOrder();

                if (workOrderVM.workOrderModel.WorkOrderId == 0)
                {
                    Mode = "add";
                    objWorkOrder = woWrapper.addWorkOrder(workOrderVM.workOrderModel);
                }
                else
                {
                    if (Command == "save")
                    {
                        objWorkOrder = woWrapper.editWorkOrder(workOrderVM.workOrderModel);
                    }
                    else if (Command == "complete")
                    {
                        objWorkOrder = woWrapper.CompleteWO(workOrderVM.workOrderModel);
                        Mode = "complete";
                    }
                }
                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = Command, WorkOrderMasterId = objWorkOrder.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        [EncryptedActionParameter]
        public ActionResult Print(long workOrderId)
        {
            bool jsonStringExceed = false;
            HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            Int64 fileSizeCounter = 0;
            Int32 maxPdfSize = section.MaxRequestLength;
            CommonWrapper comWrapper = new CommonWrapper(userData);
            List<AttachmentModel> AttachmentModelList = comWrapper.PopulatePdfPhotoFromPrint(workOrderId, "workorder", true, "woprint");
            string attachUrl = string.Empty;
            List<AttachmentModel> WoPdfAttachment = new List<AttachmentModel>();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WoPdfAttachment = AttachmentModelList;
            if (WoPdfAttachment != null && WoPdfAttachment.Count > 0)
            {
                WoPdfAttachment = WoPdfAttachment.Where(x => x.ContentType.Contains("pdf") && x.Profile == false && x.Image == false).ToList();
            }
            using (var ms = new MemoryStream())
            {
                using (var doc = new iTextSharp.text.Document())
                {
                    using (var copy = new PdfSmartCopy(doc, ms))
                    {
                        doc.Open();

                        if (AttachmentModelList != null && AttachmentModelList.Count > 0)
                        {
                            AttachmentModelList = AttachmentModelList.Where(x => x.Profile == true && x.Image == true).ToList();
                        }


                        var msSinglePDf = new MemoryStream(PrintPdfImageGetByteStream(workOrderId, AttachmentModelList));
                        using (var reader = new PdfReader(msSinglePDf))
                        {
                            fileSizeCounter += reader.FileLength;
                            if (fileSizeCounter < maxPdfSize)
                            {
                                copy.AddDocument(reader);
                                if (WoPdfAttachment != null && WoPdfAttachment.Count > 0)
                                {
                                    foreach (var item in WoPdfAttachment)
                                    {
                                        fileSizeCounter += item.FileSize;
                                        if (fileSizeCounter < maxPdfSize)
                                        {
                                            attachUrl = string.Empty;
                                            attachUrl = item.AttachmentUrl;
                                            attachUrl = comWrapper.GetSasAttachmentUrl(ref attachUrl);
                                            copy.AddDocument(new PdfReader(attachUrl));
                                        }
                                        else
                                        {
                                            jsonStringExceed = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        doc.Close();
                    }
                }
                byte[] pdf = ms.ToArray();
                string strPdf = System.Convert.ToBase64String(pdf);
                if (jsonStringExceed)
                {
                    strPdf = "";
                }
                var returnOjb = new { success = true, pdf = strPdf, jsonStringExceed = jsonStringExceed };
                var jsonResult = Json(returnOjb, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
        }

        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Header(string id, long WorkOrderId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            if (CheckLoginSession(id))
            {
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                CommonWrapper comWrapper = new CommonWrapper(userData);
                #region WorkOrderDetails
                objWorksOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(WorkOrderId);
                objWorksOrderVM.workOrderModel.AzureImageURL = comWrapper.GetClientLogoUrl();
                string angelBgPath = Server.MapPath("~/Content/images/angleBg.jpg");
                #endregion
            }
            objWorksOrderVM._userdata = userData;
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return View("PrintHeader", objWorksOrderVM);
        }

        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Footer(string id)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            if (CheckLoginSession(id))
            {
                LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            }
            return View("PrintFooter", objWorksOrderVM);
        }

        public ViewResult AddWorkOrderFromDashBoard(string addtype)
        {
            WorkOrderVM objVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);

            if (addtype == "Demand")
                objVM.IsOnDemandAdd = true;
            if (addtype == "Describe")
            {
                objVM.IsDescribeAdd = true;
                objVM.IsWoDescDynamic = true;
            }


            objVM.IsAddWorkOrderFromDashBoard = true;
            objVM._userdata = this.userData;

            if (addtype == "Demand")
            {
                WoOnDemandModel objWoModel = new WoOnDemandModel();
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                var AllLookUps = commonWrapper.GetAllLookUpList();
                List<DataContracts.LookupList> Priority = new List<DataContracts.LookupList>();
                var Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objWoModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });

                    Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
                    if (Priority != null)
                    {
                        var tmpPriority = Priority.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                        objWoModel.PriorityList = tmpPriority.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).Distinct().ToList();
                    }
                }
                var ChargeTypeLookUpList = PopulatelookUpListByType("");
                if (ChargeTypeLookUpList != null)
                {
                    objWoModel.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
                }

                MaintOnDemandMaster maintOnDemandMaster = new MaintOnDemandMaster();
                maintOnDemandMaster.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                var maintOnDemandMasterList = maintOnDemandMaster.RetrieveAllBySiteId(this.userData.DatabaseKey, this.userData.Site.TimeZone).Where(a => a.InactiveFlag == false).ToList();
                if (maintOnDemandMasterList != null)
                {
                    objWoModel.MaintOnDemandList = maintOnDemandMasterList.Select(x => new SelectListItem { Text = x.ClientLookUpId + " - " + x.Description, Value = x.ClientLookUpId });
                }
                objVM.woOnDemandModel = objWoModel;
            }
            else if (addtype == "Describe" && objVM.IsWoDescDynamic == false)
            {
                WoDescriptionModel objWoModel = new WoDescriptionModel();
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                var AllLookUps = commonWrapper.GetAllLookUpList();
                var Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                List<DataContracts.LookupList> Priority = new List<DataContracts.LookupList>();

                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objWoModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });

                    Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
                    if (Priority != null)
                    {
                        var tmpPriority = Priority.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                        objWoModel.PriorityList = tmpPriority.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).Distinct().ToList();
                    }
                }
                var ChargeTypeLookUpList = PopulatelookUpListByType("");
                if (ChargeTypeLookUpList != null)
                {
                    objWoModel.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
                }

                objVM.woDescriptionModel = objWoModel;
            }
            else if (addtype == "Describe" && objVM.IsWoDescDynamic == true)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                var AllLookUps = commonWrapper.GetAllLookUpList();
                objVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.WorkOrderDescribeAdd, userData);

                IList<string> LookupNames = objVM.UIConfigurationDetails.ToList()
                                          .Where(x => x.LookupType == DataDictionaryLookupTypeConstant.LookupList && !string.IsNullOrEmpty(x.LookupName))
                                          .Select(s => s.LookupName)
                                          .ToList();
                objVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                          .Select(s => new WOAddUILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
                var ChargeTypeLookUpList = PopulatelookUpListByType("");
                if (ChargeTypeLookUpList != null)
                {
                    objVM.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
                }
                objVM.woDescriptionModelDynamic = new WoDescriptionModelDynamic();
            }
            objVM.security = this.userData.Security;
            objVM._userdata = this.userData;
            LocalizeControls(objVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return View("~/Views/WorkOrder/Index.cshtml", objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddWorkOrdersOnDemand(WorkOrderVM workOrderVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                WorkOrder objWorkOrder = new DataContracts.WorkOrder();
                objWorkOrder = woWrapper.AddWoOnDemand(workOrderVM.woOnDemandModel);
                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Mode = "add";
                    return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderMasterId = objWorkOrder.WorkOrderId, mode = Mode, Command = "save" }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddWorkOrdersDesc(WorkOrderVM workOrderVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                WorkOrder objWorkOrder = new WorkOrder();

                objWorkOrder = woWrapper.AddWoDesc(workOrderVM.woDescriptionModel);

                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Mode = "add";
                    return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderMasterId = objWorkOrder.WorkOrderId, mode = Mode, Command = "save" }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add Work Orders Desc Dynamic
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddWorkOrdersDescDynamic(WorkOrderVM workOrderVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                WorkOrder objWorkOrder = new WorkOrder();

                objWorkOrder = woWrapper.AddWoDescDynamic(workOrderVM.woDescriptionModelDynamic);

                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Mode = "add";
                    return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderMasterId = objWorkOrder.WorkOrderId, mode = Mode, Command = "save" }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Print All WorkOrderPages

        public ActionResult PrintWoList(List<Int64> listwo)
        {
            List<long> WorkOrderIds = listwo;
            Session["PrintWOList"] = WorkOrderIds;
            return Json(new { success = true });

        }
        [EncryptedActionParameter]
        public Byte[] PrintGetByteStream(long workOrderId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string AzureImageURL = string.Empty;

            #region WorkOrderDetails
            Task task1 = Task.Factory.StartNew(() => objWorksOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(workOrderId));
            #endregion
            #region Task
            WorkOrderTask woTask = new WorkOrderTask()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ParentSiteId = this.userData.DatabaseKey.Personnel.SiteId,
                WorkOrderId = workOrderId
            };
            List<WorkOrderTask> woTaskList = new List<WorkOrderTask>();
            Task task3 = Task.Factory.StartNew(() => woTaskList = WorkOrderTask.RetriveByWorkOrderId(this.userData.DatabaseKey, woTask)).ContinueWith(_ => { objWorksOrderVM.WoTaskList = woTaskList; });

            #endregion
            #region Labor
            List<WoActualLabor> woActualLabor = new List<WoActualLabor>();
            Task task4 = Task.Factory.StartNew(() => woActualLabor = woWrapper.PopulateActualLabor(workOrderId)).ContinueWith(_ => { objWorksOrderVM.WoActualLabor = woActualLabor; });
            #endregion
            #region Part
            List<PartHistoryModel> WoPart = new List<PartHistoryModel>();
            Task task5 = Task.Factory.StartNew(() => WoPart = woWrapper.populateActualPart(workOrderId)).ContinueWith(_ => { objWorksOrderVM.WoPart = WoPart; });
            #endregion
            #region Others
            List<ActualOther> WOOthers = new List<ActualOther>();
            Task task6 = Task.Factory.StartNew(() => WOOthers = woWrapper.PopulateActualOther(workOrderId)).ContinueWith(_ => { objWorksOrderVM.WOOthers = WOOthers; });
            #endregion
            #region Summary
            List<ActualSummery> WOSummary = new List<ActualSummery>();
            Task task7 = Task.Factory.StartNew(() => WOSummary = woWrapper.populateActualSummery(workOrderId)).ContinueWith(_ => { objWorksOrderVM.WOSummary = WOSummary; });
            #endregion
            #region Photo
            List<AttachmentModel> WoPhotoAttachment = new List<AttachmentModel>();
            Task task8 = Task.Factory.StartNew(() => WoPhotoAttachment = comWrapper.PopulateAttachments(workOrderId, "workorder", true, "woprint"));
            #endregion

            Task.WaitAll(task1, task3, task4, task5, task6, task7, task8);
            AzureImageURL = comWrapper.GetClientLogoUrl();
            objWorksOrderVM.workOrderModel.AzureImageURL = AzureImageURL;
            WoPhotoAttachment = WoPhotoAttachment.Where(a => a.ContentType.Contains("image")).ToList();
            objWorksOrderVM.AttachmentList = comWrapper.GetSasImageUrlList(ref WoPhotoAttachment);
            objWorksOrderVM._userdata = userData;
            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                   "--header-spacing \"3\" " +
                                   "--footer-html \"{1}\" " +
                                   "--footer-spacing \"8\" " +
                                   "--footer-font-size \"10\" " +
                                   "--header-font-size \"10\" ",
                                   Url.Action("Header", "WorkOrder", new { id = userData.LoginAuditing.SessionId, WorkOrderId = workOrderId }, Request.Url.Scheme),
                                   Url.Action("Footer", "WorkOrder", new { id = userData.LoginAuditing.SessionId }, Request.Url.Scheme));

            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            var mailpdft = new ViewAsPdf("WODetailPrintTemplate", objWorksOrderVM)
            {
                PageMargins = new Margins(43, 12, 22, 12),// it’s in millimeters
                CustomSwitches = customSwitches
            };
            Byte[] PdfData = mailpdft.BuildPdf(ControllerContext);
            return PdfData;
        }

        [EncryptedActionParameter]
        public Byte[] PrintPdfImageGetByteStream(long workOrderId, List<AttachmentModel> WoPhotoAttachment)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string AzureImageURL = string.Empty;

            #region WorkOrderDetails
            Task task1 = Task.Factory.StartNew(() => objWorksOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(workOrderId));
            #endregion

            #region Task
            WorkOrderTask woTask = new WorkOrderTask()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ParentSiteId = this.userData.DatabaseKey.Personnel.SiteId,
                WorkOrderId = workOrderId
            };
            List<WorkOrderTask> woTaskList = new List<WorkOrderTask>();
            Task task3 = Task.Factory.StartNew(() => woTaskList = WorkOrderTask.RetriveByWorkOrderId(this.userData.DatabaseKey, woTask)).ContinueWith(_ => { objWorksOrderVM.WoTaskList = woTaskList; });
            #endregion
            #region Labor
            List<WoActualLabor> woActualLabor = new List<WoActualLabor>();
            Task task4 = Task.Factory.StartNew(() => woActualLabor = woWrapper.PopulateActualLabor(workOrderId)).ContinueWith(_ => { objWorksOrderVM.WoActualLabor = woActualLabor; });
            #endregion
            #region Part
            List<PartHistoryModel> WoPart = new List<PartHistoryModel>();
            Task task5 = Task.Factory.StartNew(() => WoPart = woWrapper.populateActualPart(workOrderId)).ContinueWith(_ => { objWorksOrderVM.WoPart = WoPart; });
            #endregion
            #region Others
            List<ActualOther> WOOthers = new List<ActualOther>();
            Task task6 = Task.Factory.StartNew(() => WOOthers = woWrapper.PopulateActualOther(workOrderId)).ContinueWith(_ => { objWorksOrderVM.WOOthers = WOOthers; });
            #endregion
            #region Summary
            List<ActualSummery> WOSummary = new List<ActualSummery>();
            Task task7 = Task.Factory.StartNew(() => WOSummary = woWrapper.populateActualSummery(workOrderId)).ContinueWith(_ => { objWorksOrderVM.WOSummary = WOSummary; });
            #endregion
            #region Photo
            WoPhotoAttachment = WoPhotoAttachment.Where(a => a.ContentType.Contains("image")).ToList();
            WoPhotoAttachment = comWrapper.GetSasImageUrlList(ref WoPhotoAttachment);
            objWorksOrderVM.AttachmentList = WoPhotoAttachment;
            #endregion


            Task.WaitAll(task1, task3, task4, task5, task6, task7);

            #region Instructions
            var instructions = new List<InstructionModel>();
            instructions = comWrapper.PopulateInstructions(workOrderId, AttachmentTableConstant.WorkOrder);
            objWorksOrderVM.workOrderModel.Intructions = string.Join("<br/>", instructions.Select(x => x.Content));
            #endregion

            AzureImageURL = comWrapper.GetClientLogoUrl();
            objWorksOrderVM.workOrderModel.AzureImageURL = AzureImageURL;

            objWorksOrderVM._userdata = userData;
            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                   "--header-spacing \"3\" " +
                                   "--footer-html \"{1}\" " +
                                   "--footer-spacing \"8\" " +
                                   "--footer-font-size \"10\" " +
                                   "--header-font-size \"10\" ",
                                   Url.Action("Header", "WorkOrder", new { id = userData.LoginAuditing.SessionId, WorkOrderId = workOrderId }, Request.Url.Scheme),
                                   Url.Action("Footer", "WorkOrder", new { id = userData.LoginAuditing.SessionId }, Request.Url.Scheme));

            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            var mailpdft = new ViewAsPdf("WODetailPrintTemplate", objWorksOrderVM)
            {
                PageMargins = new Margins(43, 12, 22, 12),// it’s in millimeters
                CustomSwitches = customSwitches
            };
            Byte[] PdfData = mailpdft.BuildPdf(ControllerContext);
            return PdfData;
        }
        #endregion
        #region V2-663
        [EncryptedActionParameter]
        public Byte[] PrintPdfImageGetByteStream_V2(long workOrderId, List<AttachmentModel> WoPhotoAttachment,
             WorkOrder listOfWO, List<WorkOrderTask> listOfWOTask, List<Timecard> listOfTimecard
            , List<PartHistory> listOfPartHistory, List<OtherCosts> listOfOtherCosts,
            List<OtherCosts> listOfSummery, List<Instructions> listOfInstructions, string AzureImageURL = "")
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            #region WorkOrderDetails
            objWorksOrderVM.workOrderModel = woWrapper.initializeControlsWOPrint(listOfWO);
            #endregion
            #region Task
            objWorksOrderVM.WoTaskList = listOfWOTask;
            #endregion
            #region Labor
            List<WoActualLabor> woActualLabor = new List<WoActualLabor>();
            WoActualLabor objActualLabor;
            foreach (var item in listOfTimecard)
            {
                objActualLabor = new WoActualLabor();
                objActualLabor.PersonnelClientLookupId = item.PersonnelClientLookupId;
                objActualLabor.PersonnelID = item.PersonnelId;
                objActualLabor.NameFull = item.NameFull.Trim(',');
                if (item.StartDate != null && item.StartDate == default(DateTime))
                {
                    objActualLabor.StartDate = null;
                }
                else
                {
                    objActualLabor.StartDate = item.StartDate;
                }
                objActualLabor.Hours = item.Hours;
                objActualLabor.TCValue = item.TCValue;
                objActualLabor.TimecardId = item.TimecardId;
                woActualLabor.Add(objActualLabor);
            }
            objWorksOrderVM.WoActualLabor = woActualLabor;
            #endregion
            #region Part
            List<PartHistoryModel> WoPart = new List<PartHistoryModel>();
            PartHistoryModel partHistoryModel;
            foreach (var item in listOfPartHistory)
            {
                partHistoryModel = new PartHistoryModel();
                partHistoryModel.PartClientLookupId = item.PartClientLookupId;
                partHistoryModel.Description = item.Description;
                partHistoryModel.TransactionQuantity = item.TransactionQuantity;
                partHistoryModel.Cost = item.Cost;
                partHistoryModel.TotalCost = item.TotalCost;
                partHistoryModel.UnitofMeasure = item.UnitofMeasure;
                //V2-624
                partHistoryModel.PartId = item.PartId;
                partHistoryModel.UPCCode = item.UPCCode;
                partHistoryModel.PartHistoryId = item.PartHistoryId;
                if (item.TransactionDate != null && item.TransactionDate == default(DateTime))
                {
                    partHistoryModel.TransactionDate = null;
                }
                else
                {
                    partHistoryModel.TransactionDate = item.TransactionDate.ToUserTimeZone(this.userData.Site.TimeZone);
                }
                WoPart.Add(partHistoryModel);
            }
            objWorksOrderVM.WoPart = WoPart;
            #endregion
            #region Others
            List<ActualOther> WOOthers = new List<ActualOther>();
            ActualOther objActualOther;
            foreach (var item in listOfOtherCosts)
            {
                objActualOther = new ActualOther();
                objActualOther.OtherCostsId = item.OtherCostsId;
                objActualOther.Source = item.Source;
                objActualOther.VendorClientLookupId = item.VendorClientLookupId;
                objActualOther.VendorId = item.VendorId;
                objActualOther.Description = item.Description;
                objActualOther.UnitCost = item.UnitCost;
                objActualOther.Quantity = item.Quantity;
                objActualOther.TotalCost = item.TotalCost;
                WOOthers.Add(objActualOther);
            }
            objWorksOrderVM.WOOthers = WOOthers;
            #endregion
            #region Summary
            List<ActualSummery> WOSummary = new List<ActualSummery>();
            ActualSummery objActualSummery;
            foreach (var item in listOfSummery)
            {
                objActualSummery = new ActualSummery();
                objActualSummery.TotalPartCost = item.TotalPartCost;
                objActualSummery.TotalCraftCost = item.TotalCraftCost;
                objActualSummery.TotalExternalCost = item.TotalExternalCost;
                objActualSummery.TotalInternalCost = item.TotalInternalCost;
                objActualSummery.TotalSummeryCost = item.TotalSummeryCost;
                WOSummary.Add(objActualSummery);
            }
            objWorksOrderVM.WOSummary = WOSummary;
            #endregion
            #region Photo
            WoPhotoAttachment = WoPhotoAttachment.Where(a => a.ContentType.Contains("image")).ToList();
            WoPhotoAttachment = comWrapper.GetSasImageUrlList(ref WoPhotoAttachment);
            objWorksOrderVM.AttachmentList = WoPhotoAttachment;
            #endregion
            #region Instructions
            var instructions = new List<InstructionModel>();
            InstructionModel objInstructionModel;
            foreach (var item in listOfInstructions)
            {
                objInstructionModel = new InstructionModel();
                objInstructionModel.Content = item.Contents;
                instructions.Add(objInstructionModel);
            }

            objWorksOrderVM.workOrderModel.Intructions = string.Join("<br/>", instructions.Select(x => x.Content));
            #endregion
            objWorksOrderVM.workOrderModel.AzureImageURL = AzureImageURL;
            objWorksOrderVM._userdata = userData;
            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                   "--header-spacing \"3\" " +
                                   "--footer-html \"{1}\" " +
                                   "--footer-spacing \"8\" " +
                                   "--footer-font-size \"10\" " +
                                   "--header-font-size \"10\" ",
                                   Url.Action("Header", "WorkOrder", new { id = userData.LoginAuditing.SessionId, WorkOrderId = workOrderId }, Request.Url.Scheme),
                                   Url.Action("Footer", "WorkOrder", new { id = userData.LoginAuditing.SessionId }, Request.Url.Scheme));

            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            var mailpdft = new ViewAsPdf("WODetailPrintTemplate", objWorksOrderVM)
            {
                PageMargins = new Margins(43, 12, 22, 12),// it’s in millimeters
                CustomSwitches = customSwitches
            };
            Byte[] PdfData = mailpdft.BuildPdf(ControllerContext);
            return PdfData;
        }
        #endregion
        #region Cancel Reason
        public JsonResult PopulateCancelReasonDropdown()
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<SelectListItem> cancelReasonList = new List<SelectListItem>();
            List<DataContracts.LookupList> cancelReasonLookUpList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                cancelReasonLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();

            }
            if (cancelReasonLookUpList != null)
            {
                cancelReasonList = cancelReasonLookUpList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() }).ToList();
            }
            return Json(new { cancelReasonList = cancelReasonList }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Photos
        [HttpPost]
        public JsonResult DeleteImageFromAzure(string _WorkOrderId, string TableName, bool Profile, bool Image)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteAzureImage(Convert.ToInt64(_WorkOrderId), AttachmentTableConstant.WorkOrder, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteImageFromOnPremise(string _WorkOrderId, string TableName, bool Profile, bool Image)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteOnPremiseImage(Convert.ToInt64(_WorkOrderId), AttachmentTableConstant.WorkOrder, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Task
        [HttpPost]
        public string PopulateTask(int? draw, int? start, int? length, long workOrderId)
        {
            bool ActionSecurity = false;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<WorkOrderTask> TaskList = woWrapper.PopulateTask(workOrderId, ref ActionSecurity);
            if (TaskList != null)
            {
                TaskList = this.GetAllWOTaskOrderSortByColumnWithOrder(order, orderDir, TaskList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = TaskList.Count();
            totalRecords = TaskList.Count();
            int initialPage = start.Value;
            var filteredResult = TaskList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, ActionSecurity = ActionSecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        [HttpGet]
        public JsonResult PopulateTaskIds(long workOrderId)
        {
            bool ActionSecurity = false;
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<WorkOrderTask> TaskList = woWrapper.PopulateTask(workOrderId, ref ActionSecurity);
            return Json(TaskList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CancelTask(string taskList, string cancelReason)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            string result = string.Empty;
            int successCount = 0;
            result = woWrapper.CancelTask(taskList, cancelReason, ref successCount);
            return Json(new { data = result, successcount = successCount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CompleteTask(string taskList)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            string result = string.Empty;
            int successCount = 0;
            result = woWrapper.CompleteTask(taskList, ref successCount);
            return Json(new { data = result, successcount = successCount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReOpenTask(string taskList, string retData)
        {

            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            string result = string.Empty;
            int successCount = 0;
            result = woWrapper.ReopenTask(taskList, ref successCount);
            return Json(new { data = result, successcount = successCount }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult AddTasks(long workOrderId, string ClientLookupId, string Status, string Description, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                woTaskModel = new WoTaskModel
                {
                    WorkOrderId = workOrderId,
                    ClientLookupId = ClientLookupId,
                    ChargeTypeList = ScheduleChargeTypeList != null ? ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : null,
                    ChargeTypelookUpList = new SelectList(new[] { "" })
                }
            };
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            objWorksOrderVM._userdata = this.userData;
            AllLookUps = commonWrapper.GetAllLookUpList();
            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorksOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var totalList = woWrapper.WOSchedulePersonnelList();

            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorksOrderVM.woScheduleModel = woScheduleModel;
            #region Summery
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorksOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorksOrderVM, objWorksOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorksOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorksOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorksOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorksOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            //V2-463
            objWorksOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorksOrderVM.workOrderSummaryModel, Downdate, DownMinutes);

            #region//*****V2-847
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****

            #endregion
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddWoTask.cshtml", objWorksOrderVM);
        }
        public PartialViewResult EditTasks(long workOrderId, long _taskId, string ClientLookupId, string Status, string Description, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrderTask woTask = woWrapper.RetriveTaskDetails(workOrderId, _taskId);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> cancelReasonLookUpList = new List<DataContracts.LookupList>();


            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
            var ChargeTypeLookUpList = PopulatelookUpListByType(woTask.ChargeType);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                cancelReasonLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_Task_Cancel).ToList();
            }
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                woTaskModel = new WoTaskModel
                {
                    WorkOrderId = workOrderId,
                    WorkOrderTaskId = _taskId,
                    TaskNumber = woTask.TaskNumber,
                    Description = woTask.Description,
                    ChargeType = woTask.ChargeType,
                    ChargeToClientLookupId = woTask.ChargeToClientLookupId,
                    Status = woTask.Status,
                    CompleteBy_PersonnelId = woTask.CompleteBy_PersonnelId,
                    CompleteBy_PersonnelClientLookupId = woTask.CompleteBy_PersonnelClientLookupId,
                    CompleteDate = woTask.CompleteDate,
                    CancelReason = (woTask.CancelReason != null && cancelReasonLookUpList != null) ? cancelReasonLookUpList.Where(x => x.ListValue == woTask.CancelReason).Select(y => y.Description).FirstOrDefault() : "",
                    updatedindex = woTask.UpdateIndex,
                    ChargeToId = woTask.ChargeToId,
                    ClientLookupId = ClientLookupId,
                    ChargeTypeList = ScheduleChargeTypeList != null ? ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : null,
                    ChargeTypelookUpList = ChargeTypeLookUpList != null ? ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() }) : null
                }
            };
            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorksOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            objWorksOrderVM._userdata = this.userData;
            var totalList = woWrapper.WOSchedulePersonnelList();

            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorksOrderVM.woScheduleModel = woScheduleModel;
            #region Summery
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorksOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorksOrderVM, objWorksOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorksOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorksOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorksOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorksOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            //V2-463
            objWorksOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorksOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            #region//*****V2-847
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****

            #endregion
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddWoTask.cshtml", objWorksOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTasks(WorkOrderVM woVM)
        {
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                List<String> errorList = new List<string>();
                errorList = woWrapper.AddOrUpdateTask(woVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderId = woVM.woTaskModel.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteTasks(long taskNumber)
        {
            WorkOrderWrapper EWrapper = new WorkOrderWrapper(userData);
            var deleteResult = EWrapper.DeleteTask(taskNumber);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Assignment
        [HttpPost]
        public string PopulateAssignment(int? draw, int? start, int? length, long workOrderId, string workOrderStatus)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<WorkOrderSchedule> AssignmentList = woWrapper.PopulateAssignment(workOrderId);
            if (AssignmentList != null)
            {
                AssignmentList = this.GetAllAssignmentSortByColumnWithOrder(order, orderDir, AssignmentList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = AssignmentList.Count();
            totalRecords = AssignmentList.Count();
            int initialPage = start.Value;
            var filteredResult = AssignmentList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();


            bool isActionDelBtnShow = workOrderStatus == WorkOrderStatusConstants.WorkRequest ? false : true;
            bool isActionEditBtnShow = userData.Security.WorkOrders.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsActionDelBtnShow = isActionDelBtnShow, IsActionEditBtnShow = isActionEditBtnShow }, JsonSerializerDateSettings);
        }

        public PartialViewResult AddAssignment(long workOrderId, string ClientLookupId, string Status, string Description, string ProjectClientLookupId)
        {

            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            var PersonnelLookUplist = GetList_PersonnelV2();
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                woAssignmentModel = new WoAssignmentModel
                {
                    WorkOrderId = workOrderId,
                    ClientLookupId = ClientLookupId,
                    WorkAssignedLookUpList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null,
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, Description, ProjectClientLookupId)
            };
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddWoAssignment.cshtml", objWorksOrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAssignment(WorkOrderVM woVM)
        {
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                List<String> errorList = new List<string>();
                errorList = woWrapper.AddOrUpdateAssignment(woVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderId = woVM.woAssignmentModel.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult EditAssignment(long WorkOrderID, long _assignmentId, string ClientLookupId, string Status, string Description, string AssignedTo_PersonnelClientLookupId, string ProjectClientLookupId)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrderSchedule workorderschdule = woWrapper.RetriveAssignmentDetails(WorkOrderID, _assignmentId);
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                woAssignmentModel = new WoAssignmentModel
                {
                    WorkOrderId = WorkOrderID,
                    ClientLookupId = ClientLookupId,
                    ScheduledStartDate = workorderschdule.ScheduledStartDate,
                    ScheduledHours = workorderschdule.ScheduledHours,
                    WorkOrderSchedId = _assignmentId,
                    AssignedTo_PersonnelClientLookupId = AssignedTo_PersonnelClientLookupId
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderID, ClientLookupId, Status, Description, ProjectClientLookupId)
            };
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddWoAssignment.cshtml", objWorksOrderVM);
        }
        [HttpPost]
        public ActionResult DeleteAssignment(long WorkOrderID, long _assignmentId)
        {
            WorkOrderWrapper EWrapper = new WorkOrderWrapper(userData);
            var deleteResult = EWrapper.DeleteAssignment(WorkOrderID, _assignmentId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Notes
        [HttpPost]
        public string PopulateNotes(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Notes> NoteList = coWrapper.PopulateNote(workOrderId, AttachmentTableConstant.WorkOrder);
            if (NoteList != null)
            {
                NoteList = this.GetAllNotesSortByColumnWithOrder(order, orderDir, NoteList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = NoteList.Count();
            totalRecords = NoteList.Count();
            int initialPage = start.Value;
            var filteredResult = NoteList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }


        [HttpPost]
        public PartialViewResult LoadComments(long WorkOrderId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Personnel> personnelsList = new List<Personnel>();
            List<Notes> NotesList = new List<Notes>();
            UserMentionData userMentionData;
            List<UserMentionData> userMentionDatas = new List<UserMentionData>();

            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => personnelsList = coWrapper.MentionList(""));
            tasks[1] = Task.Factory.StartNew(() => NotesList = coWrapper.PopulateComment(WorkOrderId, AttachmentTableConstant.WorkOrder));
            Task.WaitAll(tasks);

            if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {
                foreach (var myuseritem in personnelsList)
                {
                    userMentionData = new UserMentionData();
                    userMentionData.id = myuseritem.UserName;
                    userMentionData.name = myuseritem.FullName;
                    userMentionData.type = myuseritem.PersonnelInitial;
                    userMentionDatas.Add(userMentionData);
                }
                objWorksOrderVM.userMentionDatas = userMentionDatas;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                objWorksOrderVM.NotesList = NotesList;
            }
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_CommentsList", objWorksOrderVM);
        }
        public PartialViewResult AddNotes(long WorkOrderID, string ClientLookupId, string Status, string Description)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderID, ClientLookupId, Status, Description),
                notesModel = new WoNotesModel
                {
                    WorkOrderId = WorkOrderID,
                    ClientLookupId = ClientLookupId
                }
            };
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddWoNotes.cshtml", objWorksOrderVM);
        }
        public PartialViewResult EditNotes(long WorkOrderID, long _notesId, string ClientLookupId, string Status, string Description)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                notesModel = woWrapper.GetNoteById(_notesId, WorkOrderID, ClientLookupId),
                workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderID, ClientLookupId, Status, Description)
            };
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddWoNotes.cshtml", objWorksOrderVM);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddComments(long workOrderId, string content, string woClientLookupId, List<string> userList, long noteId = 0, long updatedindex = 0)
        {
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionData> userMentionDataList = new List<UserMentionData>();
            UserMentionData objUserMentionData;
            if (userList != null && userList.Count > 0)
            {
                foreach (var item in userList)
                {
                    objUserMentionData = new UserMentionData();
                    objUserMentionData.userId = namelist.Where(x => x.UserName == item).Select(y => y.PersonnelId).FirstOrDefault();
                    objUserMentionData.userName = item;
                    objUserMentionData.emailId = namelist.Where(x => x.UserName == item).Select(y => y.Email).FirstOrDefault();
                    userMentionDataList.Add(objUserMentionData);
                }
            }

            NotesModel notesModel = new NotesModel();
            notesModel.ObjectId = workOrderId;
            notesModel.Content = content;
            notesModel.NotesId = noteId;
            notesModel.ClientLookupId = woClientLookupId;
            notesModel.updatedindex = updatedindex;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateComment(notesModel, ref Mode, "WorkOrder", userMentionDataList);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderId = workOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Event Log
        [HttpPost]
        public string PopulateEventLog(int? draw, int? start, int? length, long WorkOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<EventLogModel> EventLogList = woWrapper.PopulateEventLog(WorkOrderId);
            EventLogList = this.GetAllEventLogSortByColumnWithOrder(order, orderDir, EventLogList);

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = EventLogList.Count();
            totalRecords = EventLogList.Count();
            int initialPage = start.Value;
            var filteredResult = EventLogList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<EventLogModel> GetAllEventLogSortByColumnWithOrder(string order, string orderDir, List<EventLogModel> data)
        {
            List<EventLogModel> lst = new List<EventLogModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Events).ToList() : data.OrderBy(p => p.Events).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Personnel).ToList() : data.OrderBy(p => p.Personnel).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionDate).ToList() : data.OrderBy(p => p.TransactionDate).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Comments).ToList() : data.OrderBy(p => p.Comments).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Events).ToList() : data.OrderBy(p => p.Events).ToList();
                    break;
            }
            return lst;
        }

        [HttpPost]
        public PartialViewResult LoadActivity(long WorkOrderId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<EventLogModel> EventLogList = woWrapper.PopulateEventLog(WorkOrderId);
            objWorksOrderVM.eventLogList = EventLogList;
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_ActivityLog", objWorksOrderVM);
        }
        #endregion

        #region Attachment
        [HttpPost]
        public string PopulateAttachment(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AttachmentList = objCommonWrapper.PopulateAttachments(workOrderId, AttachmentTableConstant.WorkOrder, userData.Security.WorkOrders.Edit);
            if (AttachmentList != null)
            {
                // AttachmentList = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, AttachmentList); //V2-949
                AttachmentList = GetAllAttachmentsPrintWithFormSortByColumnWithOrder(order, orderDir, AttachmentList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = AttachmentList.Count();
            totalRecords = AttachmentList.Count();
            int initialPage = start.Value;
            var filteredResult = AttachmentList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        public PartialViewResult AddAttachments(long workOrderID, string ClientLookupId, string Status, string Description, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            AttachmentModel Attachment = new AttachmentModel();
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            objWorksOrderVM.attachmentModel = Attachment;
            objWorksOrderVM.attachmentModel.ClientLookupId = ClientLookupId;
            Attachment.ClientLookupId = ClientLookupId;
            Attachment.WorkOrderId = workOrderID;
            objWorksOrderVM.attachmentModel = Attachment;
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderID, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorksOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorksOrderVM, objWorksOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorksOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorksOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorksOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorksOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;

            //V2-463
            objWorksOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorksOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            #region//*****V2-847
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****
            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorksOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }

            var totalList = woWrapper.WOSchedulePersonnelList();

            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorksOrderVM.woScheduleModel = woScheduleModel;
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddWoAttachment.cshtml", objWorksOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttachments()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                Stream stream = Request.Files[0].InputStream;
                AttachmentModel attachmentModel = new AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);
                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.WorkOrderId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = AttachmentTableConstant.WorkOrder;
                attachmentModel.PrintwithForm = Convert.ToBoolean(Request.Form["attachmentModel.PrintwithForm"].Split(',').FirstOrDefault());
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.WorkOrders.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.WorkOrders.Edit);
                }

                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderId = Convert.ToInt64(Request.Form["attachmentModel.WorkOrderId"]) }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var fileTypeMessage = UtilityFunction.GetMessageFromResource("spnInvalidFileType", LocalizeResourceSetConstants.Global);
                    return Json(fileTypeMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DownloadAttachment(long _fileinfoId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            Attachment fileInfo = new Attachment();
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                fileInfo = objCommonWrapper.DownloadAttachmentOnPremise(_fileinfoId);
                string contentType = MimeMapping.GetMimeMapping(fileInfo.AttachmentURL);
                return File(fileInfo.AttachmentURL, contentType, fileInfo.FileName + '.' + fileInfo.FileType);
            }
            else
            {
                fileInfo = objCommonWrapper.DownloadAttachment(_fileinfoId);
                return Redirect(fileInfo.AttachmentURL);
            }

        }
        [HttpPost]
        public ActionResult DeleteAttachments(long _fileAttachmentId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool deleteResult = false;
            string Message = string.Empty;
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                deleteResult = objCommonWrapper.DeleteAttachmentOnPremise(_fileAttachmentId, ref Message);
            }
            else
            {
                deleteResult = objCommonWrapper.DeleteAttachment(_fileAttachmentId);

            }
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString(), Message = Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #region V2-949
        public ActionResult EditAttachment(long fileAttachmentId, long workOrderID, string ClientLookupId, string Status, string Description, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            AttachmentModel Attachment = new AttachmentModel();
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            objWorksOrderVM.attachmentModel = Attachment;
            objWorksOrderVM.attachmentModel.ClientLookupId = ClientLookupId;
            Attachment.ClientLookupId = ClientLookupId;
            Attachment.WorkOrderId = workOrderID;
            objWorksOrderVM.attachmentModel = Attachment;
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderID, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorksOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorksOrderVM, objWorksOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorksOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorksOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorksOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorksOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;

            objWorksOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorksOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorksOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }

            var totalList = woWrapper.WOSchedulePersonnelList();
            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorksOrderVM.woScheduleModel = woScheduleModel;
            Attachment = woWrapper.EditWoAttachment(workOrderID, fileAttachmentId);
            Attachment.ClientLookupId = ClientLookupId;
            objWorksOrderVM.attachmentModel = Attachment;
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_EditWoAttachment.cshtml", objWorksOrderVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAttachments()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                AttachmentModel attachmentModel = new AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Boolean fileAtt = new Boolean();
                attachmentModel.AttachmentId = Convert.ToInt64(Request.Form["attachmentModel.AttachmentId"]);
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.WorkOrderId"]);
                attachmentModel.Description = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.PrintwithForm = Convert.ToBoolean(Request.Form["attachmentModel.PrintwithForm"].Split(',').FirstOrDefault());
                fileAtt = objCommonWrapper.EditAttachment(attachmentModel);
                if (fileAtt)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), WorkOrderId = Convert.ToInt64(Request.Form["attachmentModel.WorkOrderId"]) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var fileTypeMessage = UtilityFunction.GetMessageFromResource("spnInvalidFileType", LocalizeResourceSetConstants.Global);
                    return Json(fileTypeMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region Estimate-Part
        [HttpPost]
        public string PopulateEstimatePart(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<EstimatePart> EstimatePartList = woWrapper.populateEstimatedParts(workOrderId);
            var IsInitiated = EstimatePartList.Where(x => x.Status == MaterialRequestLineStatus.Initiated).Count() > 0 ? true : false;
            if (EstimatePartList != null)
            {
                EstimatePartList = this.GetAllEstimatePartSortByColumnWithOrder(order, orderDir, EstimatePartList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = EstimatePartList.Count();
            totalRecords = EstimatePartList.Count();
            int initialPage = start.Value;
            var filteredResult = EstimatePartList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsInitiated = IsInitiated }, JsonSerializer12HoursDateAndTimeSettings);

        }
        [HttpGet]
        public PartialViewResult AddEstimatesPart(long WorkOrderId, string ClientLookupId, string Status, string Description, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                estimatePart = new EstimatePart
                {
                    WorkOrderId = WorkOrderId,
                    MainClientLookupId = ClientLookupId,
                    ShoppingCart = userData.Site.ShoppingCart //V2-1068  
                }
            };
            var AllLookUps = commonWrapper.GetAllLookUpList();

            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorkOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var totalList = woWrapper.WOSchedulePersonnelList();

            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorkOrderVM.woScheduleModel = woScheduleModel;
            objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderId, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorkOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorkOrderVM, objWorkOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorkOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorkOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorkOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorkOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            //V2-463
            objWorkOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorkOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddEstimatePart", objWorkOrderVM);
        }
        public PartialViewResult EditEstimatesPart(long WorkOrderId, string MainClientLookupId, string PartclientLookupId, long EstimatedCostsId, string Description, decimal UnitCost, string Unit, string AccountClientLookupId, long AccountId, string VendorClientLookupId, long VendorId, string PartCategoryClientLookupId, long PartCategoryMasterId, decimal Quantity, decimal TotalCost, string Status, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, long CategoryId)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                estimatePart = new EstimatePart
                {
                    WorkOrderId = WorkOrderId,
                    MainClientLookupId = MainClientLookupId,
                    ClientLookupId = PartclientLookupId,
                    EstimatedCostsId = EstimatedCostsId,
                    UnitCost = UnitCost,
                    Unit = Unit,
                    AccountId = AccountId,
                    AccountClientLookupId = AccountClientLookupId,
                    VendorId = VendorId,
                    VendorClientLookupId = VendorClientLookupId,
                    PartCategoryMasterId = PartCategoryMasterId,
                    PartCategoryClientLookupId = PartCategoryClientLookupId,
                    Quantity = Quantity,
                    TotalCost = TotalCost,
                    Description = Description,
                    CategoryId = CategoryId,
                    ShoppingCart = userData.Site.ShoppingCart  //V2-1068  
                }
            };
            var AllLookUps = commonWrapper.GetAllLookUpList();

            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorkOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            if (UNIT_OF_MEASURE != null)
            {
                objWorkOrderVM.UnitOfmesureListWo = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            // Check if the user has a shopping cart and the estimate part category is not specified
            if (userData.Site.ShoppingCart && objWorkOrderVM.estimatePart.CategoryId == 0)
            {
                objWorkOrderVM.estimatePart.IsAccountClientLookupIdReq = true;
                objWorkOrderVM.estimatePart.IsPartCategoryClientLookupIdReq = true;
            }
            var totalList = woWrapper.WOSchedulePersonnelList();

            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorkOrderVM.woScheduleModel = woScheduleModel;
            objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderId, MainClientLookupId, Status, Description, ProjectClientLookupId);
            objWorkOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorkOrderVM, objWorkOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorkOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorkOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorkOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorkOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;

            //V2-463
            objWorkOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorkOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddEstimatePart", objWorkOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEstimatesPart(WorkOrderVM workOrderVM)
        {
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                EstimatedCosts objEstimatedCost = new EstimatedCosts();
                if (workOrderVM.estimatePart.EstimatedCostsId != 0)
                {
                    objEstimatedCost = woWrapper.EditEstimatePart(workOrderVM);
                }
                else
                {
                    Mode = "add";
                    objEstimatedCost = woWrapper.AddEstimatePart(workOrderVM);
                }
                if (objEstimatedCost.ErrorMessages != null && objEstimatedCost.ErrorMessages.Count > 0)
                {
                    return Json(objEstimatedCost.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workorderid = workOrderVM.estimatePart.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        public JsonResult DeleteEstimatesPart(long EstimatedCostsId)
        {
            WorkOrderWrapper EWrapper = new WorkOrderWrapper(userData);
            var deleteResult = EWrapper.DeleteEstimatePart(EstimatedCostsId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Estimate-Purchase
        [HttpPost]
        public string PopulateEstimatePurchase(int? draw, int? start, int? length, long workOrderId,
            string clientLookupId,
            string description,
            string unitofMeasure,
            string status,
            DateTime? estimatedDelivery,
            decimal orderQty = 0, int lineNo = 0,
            decimal receivequantity = 0, string srcData = ""
            )
        {
            var filter = srcData;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<POLineItemModel> PurchaseOderList = new List<POLineItemModel>();
            PurchaseOderList = woWrapper.populatePurchasedList(workOrderId);
            if (PurchaseOderList != null)
            {
                PurchaseOderList = this.GetAllEstimatePurchaseOrderSortByColumnWithOrder(order, orderDir, PurchaseOderList);
                #region TextSearch
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.ToUpper();
                    PurchaseOderList = PurchaseOderList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(filter))
                                                            || (Convert.ToString(x.LineNumber).ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(filter))
                                                            || (Convert.ToString(x.OrderQuantity).ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.UnitOfMeasure) && x.UnitOfMeasure.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.Status) && x.Status.ToUpper().Contains(filter))
                                                            || (x.EstimatedDelivery != null && (x.EstimatedDelivery?.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)).Contains(filter))
                                                            || (Convert.ToString(x.ReceivedQuantity).ToUpper().Contains(filter))
                                                             ).ToList();
                }
                #endregion
                #region AdvSearch
                if (!string.IsNullOrEmpty(clientLookupId) && clientLookupId != "0")
                {
                    clientLookupId = clientLookupId.ToUpper();
                    PurchaseOderList = PurchaseOderList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(clientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(description))
                {
                    description = description.ToUpper();
                    PurchaseOderList = PurchaseOderList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(description))).ToList();
                }
                if (!string.IsNullOrEmpty(unitofMeasure))
                {
                    unitofMeasure = unitofMeasure.ToUpper();
                    PurchaseOderList = PurchaseOderList.Where(x => (!string.IsNullOrWhiteSpace(x.UnitOfMeasure) && x.UnitOfMeasure.ToUpper().Contains(unitofMeasure))).ToList();
                }
                if (!string.IsNullOrEmpty(status))
                {
                    status = status.ToUpper();
                    PurchaseOderList = PurchaseOderList.Where(x => (!string.IsNullOrWhiteSpace(x.Status) && x.Status.ToUpper().Equals(status))).ToList();
                }
                if (estimatedDelivery != null)
                {
                    PurchaseOderList = PurchaseOderList.Where(x => (x.EstimatedDelivery != null && x.EstimatedDelivery.Value.Date.Equals(estimatedDelivery.Value.Date))).ToList();
                }
                if (receivequantity != 0)
                {
                    PurchaseOderList = PurchaseOderList.Where(x => x.ReceivedQuantity.Equals(receivequantity)).ToList();
                }
                if (lineNo != 0)
                {
                    PurchaseOderList = PurchaseOderList.Where(x => x.LineNumber.Equals(lineNo)).ToList();
                }
                if (orderQty != 0)
                {
                    PurchaseOderList = PurchaseOderList.Where(x => x.OrderQuantity.Equals(orderQty)).ToList();
                }
                #endregion
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PurchaseOderList.Count();
            totalRecords = PurchaseOderList.Count();
            int initialPage = start.Value;
            var filteredResult = PurchaseOderList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion

        #region Estimate-Labor
        [HttpPost]
        public string PopulateEstimateLabor(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<EstimateLabor> estimateLaborList = woWrapper.populateEstimatedLabors(workOrderId);
            if (estimateLaborList != null)
            {
                estimateLaborList = this.GetAllEstimateLaborsSortByColumnWithOrder(order, orderDir, estimateLaborList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = estimateLaborList.Count();
            totalRecords = estimateLaborList.Count();
            int initialPage = start.Value;
            var filteredResult = estimateLaborList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }

        [HttpGet]
        public PartialViewResult AddEstimatesLabor(long workOrderId, string ClientLookupId, string Status, string Description, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            var craftDetails = GetLookUpList_Craft();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                estimateLabor = new EstimateLabor
                {
                    workOrderId = workOrderId,
                    ClientLookupId = ClientLookupId,
                    CraftList = craftDetails != null ? craftDetails.Select(x => new SelectListItem { Text = x.ClientLookUpId + " - " + x.Description + " - " + x.ChargeRate, Value = x.CraftId.ToString() }) : null,
                }
            };
            var AllLookUps = commonWrapper.GetAllLookUpList();

            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorkOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var totalList = woWrapper.WOSchedulePersonnelList();

            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorkOrderVM.woScheduleModel = woScheduleModel;
            objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorkOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorkOrderVM, objWorkOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorkOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorkOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorkOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorkOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            #region//*****V2-847
            objWorkOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****

            //V2-463
            objWorkOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorkOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddEstimateLabor", objWorkOrderVM);
        }

        [HttpGet]
        public PartialViewResult EditEstimatesLabor(long WorkOrderId, string ClientLookupId, string Status, string Description, long EstimatedCostsId, decimal? duration, long categoryId, decimal? quantity, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            var craftDetails = GetLookUpList_Craft();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                estimateLabor = new EstimateLabor
                {
                    workOrderId = WorkOrderId,
                    ClientLookupId = ClientLookupId,
                    EstimatedCostsId = EstimatedCostsId,
                    Duration = duration,
                    CraftId = categoryId,
                    Quantity = quantity,
                    CraftList = craftDetails != null ? craftDetails.Select(x => new SelectListItem { Text = x.ClientLookUpId + " - " + x.Description + " - " + x.ChargeRate, Value = x.CraftId.ToString() }) : null
                }
            };
            var AllLookUps = commonWrapper.GetAllLookUpList();

            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorkOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var totalList = woWrapper.WOSchedulePersonnelList();

            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorkOrderVM.woScheduleModel = woScheduleModel;
            objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderId, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorkOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorkOrderVM, objWorkOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorkOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorkOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorkOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorkOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;

            //V2-463
            objWorkOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorkOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            #region//*****V2-847
            objWorkOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddEstimateLabor", objWorkOrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEstimatesLabor(WorkOrderVM workOrderVM)
        {
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                EstimatedCosts objEstimatedCost = new EstimatedCosts();
                if (workOrderVM.estimateLabor.EstimatedCostsId != 0)
                {
                    objEstimatedCost = woWrapper.EditEstimateLabor(workOrderVM);
                }
                else
                {
                    Mode = "add";
                    objEstimatedCost = woWrapper.AddEstimateLabor(workOrderVM);
                }
                if (objEstimatedCost.ErrorMessages != null && objEstimatedCost.ErrorMessages.Count > 0)
                {
                    return Json(objEstimatedCost.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workorderid = workOrderVM.estimateLabor.workOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        public JsonResult DeleteEstimatesLabor(long EstimatedCostsId)
        {
            WorkOrderWrapper EWrapper = new WorkOrderWrapper(userData);
            var deleteResult = EWrapper.DeleteEstimateLabour(EstimatedCostsId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Estimate-Other
        [HttpPost]
        public string PopulateEstimateOther(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<EstimateOther> estimateOtherList = woWrapper.populateEstimatedOthers(workOrderId);
            if (estimateOtherList != null)
            {
                estimateOtherList = this.GetAllEstimateOthersSortByColumnWithOrder(order, orderDir, estimateOtherList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = estimateOtherList.Count();
            totalRecords = estimateOtherList.Count();
            int initialPage = start.Value;
            var filteredResult = estimateOtherList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            bool isVendorcolShow = userData.Site.UseVendorMaster;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, isVendorcolShow = isVendorcolShow }, JsonSerializer12HoursDateAndTimeSettings);
        }
        public PartialViewResult AddEstimatesOther(long workOrderId, string ClientLookupId, string Status, string Description, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            var SourceTypeList = UtilityFunction.populateSourceList();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                estimateOtherModel = new EstimateOther
                {
                    workOrderId = workOrderId,
                    ClientLookupId = ClientLookupId,
                    SourceList = SourceTypeList != null ? SourceTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : null,
                    Source = "Internal",
                },
                _userdata = this.userData

            };
            var AllLookUps = commonWrapper.GetAllLookUpList();

            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorkOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var totalList = woWrapper.WOSchedulePersonnelList();

            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorkOrderVM.woScheduleModel = woScheduleModel;
            objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorkOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorkOrderVM, objWorkOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorkOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorkOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorkOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorkOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            //V2-463
            objWorkOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorkOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            #region//*****V2-847
            objWorkOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddEstimateOther", objWorkOrderVM);
        }
        public PartialViewResult EditEstimatesOther(long workOrderId, string ClientLookupId, long EstimatedCostsId, string description, string source, decimal? unitCost, decimal? quantity, decimal? totalCost, long updateIndex, long? vendorId, string VendorClientLookupId, string Status, string summarydescription, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            var SourceTypeList = UtilityFunction.populateSourceList();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                estimateOtherModel = new EstimateOther
                {
                    workOrderId = workOrderId,
                    ClientLookupId = ClientLookupId,
                    EstimatedCostsId = EstimatedCostsId,
                    Description = description,
                    Source = source,
                    VendorId = vendorId,
                    UnitCost = unitCost ?? 0,
                    Quantity = quantity ?? 0,
                    UpdateIndex = updateIndex,
                    VendorClientLookupId = VendorClientLookupId,
                    SourceList = SourceTypeList != null ? SourceTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : null,
                },
                _userdata = this.userData
            };
            var AllLookUps = commonWrapper.GetAllLookUpList();

            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorkOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var totalList = woWrapper.WOSchedulePersonnelList();

            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorkOrderVM.woScheduleModel = woScheduleModel;
            objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, summarydescription, ProjectClientLookupId);
            objWorkOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorkOrderVM, objWorkOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorkOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorkOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorkOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorkOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            //V2-463
            objWorkOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorkOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            #region//*****V2-847
            objWorkOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddEstimateOther", objWorkOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEstimatesOther(WorkOrderVM workOrderVM)
        {
            if (ModelState.IsValid)
            {
                List<String> errorList = new List<string>();

                string Mode = string.Empty;
                EstimatedCostModel objEstimatedCostModel = new EstimatedCostModel();
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                objEstimatedCostModel.WorkOrderId = workOrderVM.estimateOtherModel.workOrderId;
                objEstimatedCostModel.EstimatedCostsId = workOrderVM.estimateOtherModel.EstimatedCostsId;
                objEstimatedCostModel.VendorId = workOrderVM.estimateOtherModel.VendorId ?? 0;
                objEstimatedCostModel.Description = workOrderVM.estimateOtherModel.Description;
                objEstimatedCostModel.UnitCost = workOrderVM.estimateOtherModel.UnitCost;
                objEstimatedCostModel.Quantity = workOrderVM.estimateOtherModel.Quantity;
                objEstimatedCostModel.Source = workOrderVM.estimateOtherModel.Source;
                if (workOrderVM.estimateOtherModel.EstimatedCostsId == 0)
                {
                    Mode = "add";
                    errorList = woWrapper.addUpdateOther(objEstimatedCostModel);
                }
                else
                {
                    objEstimatedCostModel.UpdateIndex = workOrderVM.estimateOtherModel.UpdateIndex;
                    errorList = woWrapper.addUpdateOther(objEstimatedCostModel);
                }

                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workorderid = workOrderVM.estimateOtherModel.workOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteEstimatesOther(long EstimatedCostsId)
        {
            WorkOrderWrapper EWrapper = new WorkOrderWrapper(userData);
            var deleteResult = EWrapper.DeleteEstimateOther(EstimatedCostsId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Estimate-Summery       

        [HttpPost]
        public string PopulateEstimateSummery(long workOrderId)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<EstimatedCostModel> estimateSummeryList = woWrapper.populateEstimatedSummery(workOrderId);

            return JsonConvert.SerializeObject(new { data = estimateSummeryList }, JsonSerializer12HoursDateAndTimeSettings);
        }
        #endregion

        #region Actual-Part
        [HttpPost]
        public string PopulateActualPart(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<PartHistoryModel> actualPartsList = woWrapper.populateActualPart(workOrderId);
            if (actualPartsList != null)
            {
                actualPartsList = this.GetAllActualPartsSortByColumnWithOrder(order, orderDir, actualPartsList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = actualPartsList.Count();
            totalRecords = actualPartsList.Count();
            int initialPage = start.Value;
            var filteredResult = actualPartsList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }
        #endregion

        #region Actual-Labor
        [HttpPost]
        public string PopulateActualLabor(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<WoActualLabor> woActualLaborList = woWrapper.PopulateActualLabor(workOrderId);
            if (woActualLaborList != null)
            {
                woActualLaborList = this.GetAllActualLaborSortByColumnWithOrder(order, orderDir, woActualLaborList);
            }
            else
            {
                woActualLaborList = new List<WoActualLabor>();
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = woActualLaborList.Count();
            totalRecords = woActualLaborList.Count();
            int initialPage = start.Value;
            var filteredResult = woActualLaborList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        public PartialViewResult AddLabor(long WorkOrderID, string ClientLookupId, string Status, string Description, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            var PersonnelLookUplist = GetList_PersonnelV2();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                woLaborModel = new WoActualLabor
                {
                    workOrderId = WorkOrderID,
                    PersonnelClientLookupId = ClientLookupId,
                    WorkAssignedLookUpList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null,
                }
            };
            var AllLookUps = commonWrapper.GetAllLookUpList();

            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorksOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var totalList = woWrapper.WOSchedulePersonnelList();
            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorksOrderVM.woScheduleModel = woScheduleModel;
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderID, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorksOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorksOrderVM, objWorksOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorksOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorksOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorksOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorksOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            //V2-463
            objWorksOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorksOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            #region//*****V2-847
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddWoLabor.cshtml", objWorksOrderVM);
        }
        public PartialViewResult EditActualLabor(long WorkOrderID, string ClientLookupId, long timeCardId, long personnelID, decimal hours, string startDate, string Status, string Description, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            var PersonnelLookUplist = GetList_PersonnelV2();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                woLaborModel = new WoActualLabor
                {
                    workOrderId = WorkOrderID,
                    PersonnelClientLookupId = ClientLookupId,
                    PersonnelID = personnelID,
                    TimecardId = timeCardId,
                    StartDate = !string.IsNullOrEmpty(startDate) ? DateTime.ParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture) : DateTime.Now,
                    Hours = hours,
                    WorkAssignedLookUpList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null
                }
            };
            var AllLookUps = commonWrapper.GetAllLookUpList();

            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorksOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var totalList = woWrapper.WOSchedulePersonnelList();
            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorksOrderVM.woScheduleModel = woScheduleModel;
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderID, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorksOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorksOrderVM, objWorksOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorksOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorksOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorksOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorksOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            //V2-463
            objWorksOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorksOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            #region//*****V2-847
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddWoLabor.cshtml", objWorksOrderVM);
        }
        public ActionResult SaveActualLabor(WorkOrderVM woVM)
        {
            if (ModelState.IsValid)
            {
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                if (woVM.woLaborModel.StartDate == null) woVM.woLaborModel.StartDate = DateTime.UtcNow;
                Timecard result = new Timecard();
                string Mode = string.Empty;
                if (woVM.woLaborModel.TimecardId == 0)
                {
                    Mode = "add";
                    result = woWrapper.AddActualLabor(woVM.woLaborModel);
                }
                else
                {
                    Mode = "update";
                    result = woWrapper.EditActualLabor(woVM.woLaborModel);
                }
                if (result.ErrorMessages != null && result.ErrorMessages.Count > 0)
                {
                    return Json(result.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workorderid = woVM.woLaborModel.workOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteActualLabor(long TimeCardId)
        {
            WorkOrderWrapper EWrapper = new WorkOrderWrapper(userData);
            var deleteResult = EWrapper.DeleteActualLabor(TimeCardId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Actual-Other
        [HttpPost]
        public string PopulateActualOther(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<ActualOther> ActualOtherList = woWrapper.PopulateActualOther(workOrderId);
            if (ActualOtherList != null)
            {
                ActualOtherList = this.GetAllActualOtherSortByColumnWithOrder(order, orderDir, ActualOtherList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = ActualOtherList.Count();
            totalRecords = ActualOtherList.Count();
            int initialPage = start.Value;
            var filteredResult = ActualOtherList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            bool isVendorcolShow = userData.Site.UseVendorMaster;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, isVendorcolShow = isVendorcolShow }, JsonSerializer12HoursDateAndTimeSettings);
        }

        public PartialViewResult AddActualOther(long workOrderId, string ClientLookupId, string Status, string Description, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            var SourceTypeList = UtilityFunction.populateSourceList();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            var VendorLookUpList = RetrieveForLookupVendor();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                actualOther = new ActualOther
                {
                    workOrderId = workOrderId,
                    ClientLookupId = ClientLookupId,
                    SourceList = SourceTypeList != null ? SourceTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : null,
                    VendorLookUpList = VendorLookUpList != null ? VendorLookUpList.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.VendorId.ToString() }) : null,
                }
            };
            var AllLookUps = commonWrapper.GetAllLookUpList();

            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorkOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var totalList = woWrapper.WOSchedulePersonnelList();
            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorkOrderVM.woScheduleModel = woScheduleModel;
            objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorkOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorkOrderVM, objWorkOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorkOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorkOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorkOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorkOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            //V2-463
            objWorkOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorkOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            #region//*****V2-847
            objWorkOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****

            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddActualOther", objWorkOrderVM);
        }

        public PartialViewResult EditActualOther(long workOrderId, string ClientLookupId, string VendorName, long otherCostsId, string description, string source, decimal? unitCost, decimal? quantity, decimal? totalCost, long? vendorId, string Status, string summarydescription, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            var SourceTypeList = UtilityFunction.populateSourceList();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var VendorLookUpList = RetrieveForLookupVendor();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                actualOther = new ActualOther
                {
                    workOrderId = workOrderId,
                    ClientLookupId = ClientLookupId,
                    OtherCostsId = otherCostsId,
                    Description = description,
                    Source = source,
                    UnitCost = unitCost,
                    Quantity = quantity,
                    VendorId = vendorId,
                    VendorClientLookupId = VendorName,
                    SourceList = SourceTypeList != null ? SourceTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : null,
                    VendorLookUpList = VendorLookUpList != null ? VendorLookUpList.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.VendorId.ToString() }) : null,
                }
            };
            var AllLookUps = commonWrapper.GetAllLookUpList();

            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null && CancelReason.Count > 0)
            {
                objWorkOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var totalList = woWrapper.WOSchedulePersonnelList();
            if (totalList != null && totalList.Count > 0)
            {
                woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            }
            objWorkOrderVM.woScheduleModel = woScheduleModel;
            objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, summarydescription, ProjectClientLookupId);
            objWorkOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorkOrderVM, objWorkOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorkOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorkOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorkOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorkOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            //V2-463
            objWorkOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorkOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            #region//*****V2-847
            objWorkOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorkOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddActualOther", objWorkOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddActualWoOther(WorkOrderVM workOrderVM)
        {
            if (ModelState.IsValid)
            {
                List<string> errorlist = new List<string>();
                string Mode = string.Empty;
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                if (workOrderVM.actualOther.OtherCostsId == 0)
                {
                    Mode = "add";
                    errorlist = woWrapper.AddActualOther(workOrderVM.actualOther);
                }
                else
                {
                    errorlist = woWrapper.EditActualOther(workOrderVM.actualOther);
                }
                if (errorlist != null && errorlist.Count > 0)
                {
                    return Json(errorlist, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workorderid = workOrderVM.actualOther.workOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        public JsonResult DeleteActualOther(long OtherCostsId)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            if (woWrapper.DeleteActualOther(OtherCostsId))
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Actual-Summery       

        [HttpPost]
        public string PopulateActualSummery(long workOrderId)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<ActualSummery> actualSummerysList = woWrapper.populateActualSummery(workOrderId);
            return JsonConvert.SerializeObject(new { data = actualSummerysList }, JsonSerializer12HoursDateAndTimeSettings);
        }
        #endregion



        #region AddWorkRequest
        public ActionResult AddWorkRequest()
        {
            TempData["Mode"] = "ADDWOREQUEST";
            return Redirect("/WorkOrder/Index?page=Maintenance_Work_Order_Search");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveWoRequset(WorkOrderVM workOrderVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsg = new List<string>();
                WorkOrderVM objVM = new WorkOrderVM();
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                objVM.woRequestModel = new WoRequestModel();
                workOrderVM.woRequestModel.IsDepartmentShow = true;
                workOrderVM.woRequestModel.IsTypeShow = true;
                workOrderVM.woRequestModel.IsDescriptionShow = true;
                workOrderVM.woRequestModel.ChargeType = ChargeType.Equipment;
                WorkOrder returnObj = woWrapper.AddWorkRequestWrap(workOrderVM.woRequestModel, ref ErrorMsg);
                if (ErrorMsg != null && ErrorMsg.Count > 0)
                {
                    return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = Command, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
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

        #region Cancel Job
        public JsonResult CancelJob(long WorkorderId, string CancelReason = null, string Comments = null)
        {
            string result = string.Empty;
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrder Wojob = woWrapper.CancelJob(WorkorderId, CancelReason, Comments);
            if (Wojob.ErrorMessages != null && Wojob.ErrorMessages.Count > 0)
            {
                return Json(new { data = Wojob.ErrorMessages }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = JsonReturnEnum.success.ToString();
            }
            return Json(new { data = result, WorkorderId = WorkorderId }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Complete
        private WorkOrder WorkOrderComplete(long WorkOrderId, string CompleteComments)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrder objWorkOrder = new WorkOrder();
            WorkOrderModel objWorkOrderModel = new WorkOrderModel();
            objWorkOrderModel.WorkOrderId = WorkOrderId;
            objWorkOrderModel.CompleteComments = CompleteComments;
            objWorkOrder = woWrapper.CompleteWO(objWorkOrderModel);
            return objWorkOrder;
        }
        public JsonResult CompleteWorkOrder(long WorkOrderId, string CompleteComments)
        {
            WorkOrder objWorkOrder = new WorkOrder();
            objWorkOrder = this.WorkOrderComplete(WorkOrderId, CompleteComments);
            if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
            {
                return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult CompleteWorkOrderBatch(WoCancelAndPrintModel model)
        {
            List<string> errMsgList = new List<string>();
            List<BatchCompleteResultModel> WoBatchList = new List<BatchCompleteResultModel>();
            WorkOrder objWorkOrder = new WorkOrder();
            List<string> errorMessage = new List<string>();
            StringBuilder failedWoList = new StringBuilder();
            foreach (var item in model.list)
            {
                if (item.Status == WorkOrderStatusConstants.Approved || item.Status == WorkOrderStatusConstants.Scheduled)
                {
                    objWorkOrder = this.WorkOrderComplete(item.WorkOrderId, model.comments);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to complete " + objWorkOrder.ClientLookupId + ": " + objWorkOrder.ErrorMessages[0];
                        errorMessage.Add(errormessage);
                    }
                }
                else
                {
                    failedWoList.Append(item.ClientLookupId + ",");

                }
            }
            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errorMessage.Add("Work Order(s) " + failedWoList + " can't be completed. Please check the status.");
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Down Time
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ValiDatetimeControlls(WorkOrderVM objInventoryCheckVM)
        {
            if (ModelState.IsValid)
            {
                string IsAddOrUpdate = string.Empty;
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                Downtime Wojob = woWrapper.DowntimeWO(objInventoryCheckVM.downtimeModel.WorkOrderId, objInventoryCheckVM.downtimeModel.Downdate, objInventoryCheckVM.downtimeModel.Minutes, ref IsAddOrUpdate);
                if (Wojob.ErrorMessages != null && Wojob.ErrorMessages.Count > 0)
                {
                    return Json(Wojob.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Issuccess = true, msg = IsAddOrUpdate, Downdate = objInventoryCheckVM.downtimeModel.Downdate, Minutes = objInventoryCheckVM.downtimeModel.Minutes, WorkOrderId = objInventoryCheckVM.downtimeModel.WorkOrderId }, JsonRequestBehavior.AllowGet);
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

        #region Follow up
        public PartialViewResult FollowUpWO(long WorkoderId, string ClientLookupId)
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            WorkOrderModel woModel = new WorkOrderModel();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objWorkOrderVM.woRequestModel = new WoRequestModel();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    woModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            objWorkOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(WorkoderId);
            if (Type != null)
            {
                objWorkOrderVM.woRequestModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
            }
            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            objWorkOrderVM._userdata = this.userData;
            objWorkOrderVM.woRequestModel.ChargeToList = defaultChargeToList;
            objWorkOrderVM.woRequestModel.WorkOrderId = objWorkOrderVM.workOrderModel.WorkOrderId;
            objWorkOrderVM.woRequestModel.ClientLookupId = ClientLookupId;
            objWorkOrderVM.woRequestModel.ChargeType = objWorkOrderVM.workOrderModel.ChargeType;
            objWorkOrderVM.woRequestModel.Description = objWorkOrderVM.workOrderModel.Description;
            objWorkOrderVM.woRequestModel.ChargeToClientLookupId = objWorkOrderVM.workOrderModel.ChargeToClientLookupId;
            objWorkOrderVM.woRequestModel.Type = objWorkOrderVM.workOrderModel.Type;
            objWorkOrderVM.woRequestModel.ChargeTo = objWorkOrderVM.workOrderModel.ChargeToId;
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_menuFollowUpWorkOrder.cshtml", objWorkOrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult FollowUpWorkOrder(WorkOrderVM objVM)
        {
            string result = string.Empty;
            WorkOrder obj = new WorkOrder();
            string IsAddOrUpdate = string.Empty;
            if (ModelState.IsValid)
            {
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                objVM.woRequestModel.IsDepartmentShow = true;
                objVM.woRequestModel.IsTypeShow = true;
                objVM.woRequestModel.IsDescriptionShow = true;

                WorkOrder Wojob = woWrapper.FollowUpWorkOrder(objVM.woRequestModel);
                if (Wojob.ErrorMessages != null && Wojob.ErrorMessages.Count > 0)
                {
                    return Json(Wojob.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Issuccess = true, msg = IsAddOrUpdate, WorkOrderId = Wojob.WorkOrderId }, JsonRequestBehavior.AllowGet);
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

        #region Reopen
        public ActionResult ReopenWO(long workorderId)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            string Statusmsg = string.Empty;
            WorkOrder Wo = woWrapper.ReOpenWorkOrder(workorderId, ref Statusmsg);
            return Json(new { data = Statusmsg, workorderId = workorderId, error = Wo.ErrorMessages }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Approve
        public ActionResult ApproveWO(long workorderId)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            string Statusmsg = string.Empty;
            WorkOrder Wo = woWrapper.ApproveWO(workorderId, ref Statusmsg);
            return Json(new { data = Statusmsg, workorderId = Wo.WorkOrderId, error = Wo.ErrorMessages }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region RemoveSchedule
        public ActionResult RemoveScheduleWO(long workorderId)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            string Statusmsg = string.Empty;
            WorkOrder Wo = woWrapper.RemoveScheduleRecord(workorderId, ref Statusmsg);
            return Json(new { data = Statusmsg, workorderId = Wo.WorkOrderId, error = Wo.ErrorMessages }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveScheduleWoList(WoRemoveScheduleModel model)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM(); WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);

            List<string> errorMessage = new List<string>();

            StringBuilder failedWoList = new StringBuilder();

            foreach (var item in model.list)
            {
                if (item.Status != WorkOrderStatusConstants.Scheduled)
                {
                    failedWoList.Append(item.ClientLookupId + ",");
                }
                else
                {
                    string Statusmsg = string.Empty;
                    WorkOrder Wo = woWrapper.RemoveScheduleRecord(item.WorkOrderId, ref Statusmsg);

                    if (Wo.ErrorMessages != null && Wo.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to remove schedule " + Wo.ClientLookupId + ": " + Wo.ErrorMessages[0];
                        errorMessage.Add(errormessage);
                    }
                }
            }
            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errorMessage.Add("Work Order(s) " + failedWoList + " can't be remove schedule. Please check the status.");
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region Deny Job
        public JsonResult DenyWoJob(long WorkorderId, string Comments)
        {
            string Statusmsg = string.Empty;
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrder Wojob = woWrapper.DenyWO(WorkorderId, Comments, ref Statusmsg);
            return Json(new { data = Statusmsg, WoId = WorkorderId, error = Wojob.ErrorMessages }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Emergency On-Demand
        public PartialViewResult EmergencyOnDemand(string ClientLookupId, long WorkoderId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            objWorksOrderVM.woEmergencyOnDemandModel = new WoEmergencyOnDamandModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();

            objWorksOrderVM.woEmergencyOnDemandModel.WorkOrderId = WorkoderId;
            objWorksOrderVM._userdata = this.userData;
            objWorksOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(WorkoderId);
            objWorksOrderVM.woEmergencyOnDemandModel.ClientLookupId = objWorksOrderVM.workOrderModel.ClientLookupId;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objWorksOrderVM.woEmergencyOnDemandModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            objWorksOrderVM.woEmergencyOnDemandModel.ChargeToList = defaultChargeToList;
            var OnDemand = woWrapper.GetOndemandList();
            objWorksOrderVM.woEmergencyOnDemandModel.OnDemandIDList = OnDemand.Select(x => new SelectListItem { Text = x.ClientLookUpId + "   |   " + x.Description, Value = x.ClientLookUpId.ToString() }); ;

            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_EmergencyOnDemandWoPopup.cshtml", objWorksOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveEmergencyOndemand(WorkOrderVM workOrderVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsgList = new List<string>();
                WorkOrderVM objVM = new WorkOrderVM();
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                objVM.woEmergencyOnDemandModel = new WoEmergencyOnDamandModel();

                workOrderVM.woEmergencyOnDemandModel.IsDepartmentShow = true;
                workOrderVM.woEmergencyOnDemandModel.IsTypeShow = true;
                workOrderVM.woEmergencyOnDemandModel.IsDescriptionShow = true;

                WorkOrder returnObj = woWrapper.Emergency_Ondemand(workOrderVM.woEmergencyOnDemandModel, ref ErrorMsgList);
                if (ErrorMsgList != null && ErrorMsgList.Count > 0)
                {
                    return Json(ErrorMsgList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool IsredirectToSaniPage = (userData.Site.ExternalSanitation == true && userData.Security.SanitationJob.CreateRequest == true) ? true : false;
                    return Json(new { data = JsonReturnEnum.success.ToString(), IsToSanitationPage = IsredirectToSaniPage, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
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

        #region Emergency Describe
        public PartialViewResult EmergencyDescribe(string ClientLookupId, long WorkoderId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            objWorksOrderVM.woEmergencyDescribeModel = new WoEmergencyDescribeModel();
            objWorksOrderVM.woEmergencyDescribeModel.WorkOrderId = WorkoderId;
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            objWorksOrderVM._userdata = this.userData;
            objWorksOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(WorkoderId);
            objWorksOrderVM.woEmergencyDescribeModel.ClientLookupId = objWorksOrderVM.workOrderModel.ClientLookupId;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objWorksOrderVM.woEmergencyDescribeModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            objWorksOrderVM.woEmergencyDescribeModel.ChargeToList = defaultChargeToList;

            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_EmergencyDescribeWoPopup.cshtml", objWorksOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveEmergencyDescribe(WorkOrderVM workOrderVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsgList = new List<string>();
                WorkOrderVM objVM = new WorkOrderVM();
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                objVM.woEmergencyDescribeModel = new WoEmergencyDescribeModel();

                workOrderVM.woEmergencyDescribeModel.IsDepartmentShow = true;
                workOrderVM.woEmergencyDescribeModel.IsTypeShow = true;
                workOrderVM.woEmergencyDescribeModel.IsDescriptionShow = true;

                WorkOrder returnObj = woWrapper.Emergency_Describe(workOrderVM.woEmergencyDescribeModel, ref ErrorMsgList);
                if (ErrorMsgList != null && ErrorMsgList.Count > 0)
                {
                    return Json(ErrorMsgList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool IsredirectToSaniPage = (userData.Site.ExternalSanitation == true && userData.Security.SanitationJob.CreateRequest == true) ? true : false;
                    return Json(new { data = JsonReturnEnum.success.ToString(), IsToSanitationPage = IsredirectToSaniPage, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
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

        #region Sanitation On-Demand
        public PartialViewResult SanitationOnDemand(long WorkoderId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            objWorksOrderVM.sanitationOnDemandWOModel = new SanitationOnDemandWOModel();
            objWorksOrderVM.sanitationOnDemandWOModel.WorkOrderId = WorkoderId;
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            objWorksOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(WorkoderId);
            objWorksOrderVM.sanitationOnDemandWOModel.ClientLookupId = objWorksOrderVM.workOrderModel.ClientLookupId;
            var OnDemand = woWrapper.SanitationOnDemandMaster();
            if (OnDemand != null)
            {
                objWorksOrderVM.sanitationOnDemandWOModel.OnDemandList = OnDemand.Select(x => new SelectListItem { Text = x.Description == "" ? x.ClientLookUpId : x.Description + "  ||  " + x.ClientLookUpId, Value = x.SanOnDemandMasterId.ToString() });
            }

            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_SanitationWoOnDemandWoPopup.cshtml", objWorksOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveSanitationOndemand(WorkOrderVM workOrderVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsg = new List<string>();
                WorkOrderVM objVM = new WorkOrderVM();
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                objVM.sanitationOnDemandWOModel = new SanitationOnDemandWOModel();
                SanitationRequest returnObj = woWrapper.santitation_Ondemand(workOrderVM.sanitationOnDemandWOModel, ref ErrorMsg);
                if (ErrorMsg != null && ErrorMsg.Count > 0)
                {
                    return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["RedirectFromWO"] = Convert.ToString(returnObj.SanitationJobId);
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = "save", sanitationJobId = returnObj.SanitationJobId }, JsonRequestBehavior.AllowGet);
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

        #region Sanitation Describe

        public PartialViewResult SanitationDescribe(long WorkoderId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            objWorksOrderVM.sanitationDescribeWoModel = new SanitationDescribeWoModel();
            objWorksOrderVM.sanitationDescribeWoModel.WorkOrderId = WorkoderId;
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            objWorksOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(WorkoderId);
            objWorksOrderVM.sanitationDescribeWoModel.ClientLookupId = objWorksOrderVM.workOrderModel.ClientLookupId;
            var OnDemand = woWrapper.SanitationOnDemandMaster();
            if (OnDemand != null)
            {
                objWorksOrderVM.sanitationDescribeWoModel.OnDemandList = OnDemand.Select(x => new SelectListItem { Text = x.Description == "" ? x.ClientLookUpId : x.Description + "  ||  " + x.ClientLookUpId, Value = x.SanOnDemandMasterId.ToString() });
            }
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_SanitationWoDescribeModal.cshtml", objWorksOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSanitationDescribe(WorkOrderVM workOrderVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                List<string> ErrorMsgList = new List<string>();
                WorkOrderVM objVM = new WorkOrderVM();
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                objVM.sanitationDescribeWoModel = new SanitationDescribeWoModel();
                SanitationRequest returnObj = woWrapper.santitation_Describe(workOrderVM.sanitationDescribeWoModel, ref ErrorMsgList);
                if (ErrorMsgList != null && ErrorMsgList.Count > 0)
                {
                    return Json(ErrorMsgList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["RedirectFromWO"] = Convert.ToString(returnObj.SanitationJobId);
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = "save", sanitationJobId = returnObj.SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cancel,Deny,Approve & Print WO-list From Search
        public JsonResult CancelWoList(WoCancelAndPrintModel model)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM(); WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            objWorksOrderVM.security = this.userData.Security;
            bool HasAuthToCancel = false;
            List<string> errorMessage = new List<string>();
            if (objWorksOrderVM.security.WorkOrders.Edit && objWorksOrderVM.security.WorkOrders.Cancel)
            {
                HasAuthToCancel = true;
            }
            else
            {
                return Json(new { data = "Unable to Cancel Workoder Due to Permission Label!" }, JsonRequestBehavior.AllowGet);
            }
            System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();

            foreach (var item in model.list)
            {
                //changes for V2-576
                if (!(item.Status == WorkOrderStatusConstants.Approved || item.Status == WorkOrderStatusConstants.Scheduled))
                {
                    failedWoList.Append(item.ClientLookupId + ",");
                }
                else
                {
                    WorkOrder Wojob = woWrapper.CancelJob(item.WorkOrderId, model.cancelreason, model.comments);
                    if (Wojob.ErrorMessages != null && Wojob.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to cancel " + Wojob.ClientLookupId + ": " + Wojob.ErrorMessages[0];
                        errorMessage.Add(errormessage);
                    }
                }
            }
            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errorMessage.Add("Work Order(s) " + failedWoList + " can't be cancelled. Please check the status.");
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PrintWoListFromIndex(WoPrintingModel model)
        {
            if (model.list != null)
            {
                if (model.list.Count > 0)
                {
                    return PrintPDFFromIndex(model, model.PrintingCountConnectionID);
                }
                else
                {
                    var returnOjb = new { success = false };
                    return Json(returnOjb, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var returnOjb = new { success = false };
                return Json(returnOjb, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SetPrintWoListFromIndex(WoPrintingModel model)
        {
            List<long> WorkOrderIds = model.list.Select(x => x.WorkOrderId).ToList();
            Session["PrintWOList"] = WorkOrderIds;
            return Json(new { success = true });
        }
        [EncryptedActionParameter]
        public ActionResult GenerateWorkOrderPrint()
        {
            List<long> WorkOrderIds = new List<long>();
            if (Session["PrintWOList"] != null)
            {
                WorkOrderIds = (List<long>)Session["PrintWOList"];
            }
            var objPrintModelList = PrintDevExpressFromIndex(WorkOrderIds);
            return View("DevExpressPrint", objPrintModelList);
        }
        private List<WorkOrderDevExpressPrintModel> PrintDevExpressFromIndex(List<long> WorkOrderIds)
        {
            var WorkOrderDevExpressPrintModelList = new List<WorkOrderDevExpressPrintModel>();
            var WorkOrderDevExpressPrintModel = new WorkOrderDevExpressPrintModel();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Notes> NotesList = new List<Notes>();
            var WorkOrderIDList = WorkOrderIds;
            var WorkOrderBunchListInfo = woWrapper.RetrieveAllByWorkOrdeV2Print(WorkOrderIDList);
            Parallel.ForEach(WorkOrderIds, p =>
            {
                var notes = commonWrapper.PopulateComment(p, "WorkOrder");
                NotesList.AddRange(notes);
            });

            List<WorkOrder> listOfWO = WorkOrderBunchListInfo.listOfWO;
            List<WorkOrderTask> listOfWOTask = WorkOrderBunchListInfo.listOfWOTask;
            List<Timecard> listOfTimecard = WorkOrderBunchListInfo.listOfTimecard;
            List<PartHistory> listOfPartHistory = WorkOrderBunchListInfo.listOfPartHistory;
            List<OtherCosts> listOfSummery = WorkOrderBunchListInfo.listOfSummery;
            List<Attachment> listOfAttachment = WorkOrderBunchListInfo.listOfAttachment;
            List<OtherCosts> listOfOtherCosts = WorkOrderBunchListInfo.listOfOtherCosts;
            List<Instructions> listOfInstructions = WorkOrderBunchListInfo.listOfInstructions;
            #region V2-944
            List<WorkOrderUDF> listOfWorkOrderUDF = WorkOrderBunchListInfo.listOfWorkOrderUDF;
            List<WorkOrderSchedule> listOfWorkOrderSchedule = WorkOrderBunchListInfo.listOfWorkOrderSchedule;
            #endregion

            var ImageUrl = GenerateImageUrlDevExpress();// no need to call for each id as it is dependent on client id
            foreach (var item in WorkOrderIds)
            {
                CommonWrapper comWrapper = new CommonWrapper(userData);

                List<AttachmentModel> AttachmentModelList = comWrapper.PopulateAttachmentForPrint(listOfAttachment.Where(m => m.ObjectId == item).ToList(), item, "woprint");
                List<AttachmentModel> WoPdfAttachment = new List<AttachmentModel>();
                WoPdfAttachment = AttachmentModelList;
                if (WoPdfAttachment != null && WoPdfAttachment.Count > 0)
                {
                    WoPdfAttachment = WoPdfAttachment.Where(x => x.ContentType.Contains("pdf") && x.Profile == false && x.Image == false && x.PrintwithForm == true).ToList();
                }
                if (AttachmentModelList != null && AttachmentModelList.Count > 0)
                {
                    AttachmentModelList = AttachmentModelList.Where(x => x.Image == true).ToList();
                }

                WorkOrder WorkOrderDetails = listOfWO.Where(m => m.WorkOrderId == item).FirstOrDefault();
                List<WorkOrderTask> listOfWOTasks = listOfWOTask.Where(m => m.WorkOrderId == item).ToList();
                List<Timecard> listOfTimecards = listOfTimecard.Where(m => m.ChargeToId_Primary == item).ToList();
                List<PartHistory> listOfPartHistorys = listOfPartHistory.Where(m => m.ChargeToId_Primary == item).ToList();
                List<OtherCosts> listOtherCosts = listOfOtherCosts.Where(m => m.ObjectId == item).ToList();
                List<OtherCosts> listOfSummary = listOfSummery.Where(m => m.ObjectId == item).ToList();
                List<Instructions> listOfInstruction = listOfInstructions.Where(m => m.ObjectId == item).ToList();
                List<Notes> listOfNotes = NotesList.Where(m => m.ObjectId == item).ToList();
                #region V2-944
                WorkOrderUDF WorkOrderUDFDetails = listOfWorkOrderUDF.Where(m => m.WorkOrderId == item).FirstOrDefault();
                List<WorkOrderSchedule> listOfWorkOrderSchedules = listOfWorkOrderSchedule.Where(m => m.WorkOrderId == item).ToList();
                #endregion

                //-- binding for devexpress begin
                WorkOrderDevExpressPrintModel = new WorkOrderDevExpressPrintModel();

                BindWorkOrderDetails(WorkOrderDetails, ref WorkOrderDevExpressPrintModel, ImageUrl);
                BindTaskTable(listOfWOTasks, ref WorkOrderDevExpressPrintModel);
                BindLaborTable(listOfTimecards, ref WorkOrderDevExpressPrintModel);
                BindPartTable(listOfPartHistorys, ref WorkOrderDevExpressPrintModel);
                BindOthersTable(listOtherCosts, ref WorkOrderDevExpressPrintModel);
                BindSummaryTable(listOfSummary, ref WorkOrderDevExpressPrintModel);
                BindInsturctionsTable(listOfInstruction, ref WorkOrderDevExpressPrintModel);
                BindPhotosTable(AttachmentModelList, ref WorkOrderDevExpressPrintModel);
                BindPdfAttachmentTable(WoPdfAttachment, ref WorkOrderDevExpressPrintModel);
                BindCommentsTable(listOfNotes, ref WorkOrderDevExpressPrintModel);
                #region V2-944
                BindWorkOrderUDFDetails(WorkOrderUDFDetails, ref WorkOrderDevExpressPrintModel);
                BindWorkOrderScheduleTable(listOfWorkOrderSchedules, ref WorkOrderDevExpressPrintModel);
                ClientSetUpWrapper csWrapper = new ClientSetUpWrapper(userData);
                var formsettingdetails = csWrapper.FormSettingsDetails();
                if (formsettingdetails != null)
                {
                    WorkOrderDevExpressPrintModel.WOUIC = formsettingdetails.WOUIC;
                    WorkOrderDevExpressPrintModel.WOLaborRecording = formsettingdetails.WOLaborRecording;
                    WorkOrderDevExpressPrintModel.WOScheduling = formsettingdetails.WOScheduling;
                    WorkOrderDevExpressPrintModel.WOPhotos = formsettingdetails.WOPhotos;
                    WorkOrderDevExpressPrintModel.WOSummary = formsettingdetails.WOSummary;
                    WorkOrderDevExpressPrintModel.WOComments = formsettingdetails.WOComments;
                }
                #endregion
                WorkOrderDevExpressPrintModel.OnPremise = userData.DatabaseKey.Client.OnPremise;
                WorkOrderDevExpressPrintModelList.Add(WorkOrderDevExpressPrintModel);
                //-- end

            }
            return WorkOrderDevExpressPrintModelList;
        }
        private string GenerateImageUrl()
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            var ImagePath = comWrapper.GetClientLogoUrl();
            if (string.IsNullOrEmpty(ImagePath))
            {
                var path = "~/Scripts/ImageZoom/images/NoImage.jpg";
                ImagePath = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Content(path);
            }
            else if (ImagePath.StartsWith("../"))
            {
                ImagePath = ImagePath.Replace("../", Request.Url.Scheme + "://" + Request.Url.Authority + "/");
            }
            return ImagePath;
        }
        private void BindWorkOrderDetails(WorkOrder WorkOrderDetails, ref WorkOrderDevExpressPrintModel workOrderDevExpressPrintModel, string AzureImageUrl)
        {
            workOrderDevExpressPrintModel.WorkOrderId = WorkOrderDetails.WorkOrderId;
            workOrderDevExpressPrintModel.ClientlookupId = WorkOrderDetails.ClientLookupId;
            workOrderDevExpressPrintModel.Description = WorkOrderDetails.Description;
            workOrderDevExpressPrintModel.ChargeType = WorkOrderDetails.ChargeType;
            workOrderDevExpressPrintModel.ChargeToClientLookupId = WorkOrderDetails.ChargeToClientLookupId;
            workOrderDevExpressPrintModel.ChargeTo_Name = WorkOrderDetails.ChargeTo_Name;
            workOrderDevExpressPrintModel.AssetLocation = WorkOrderDetails.AssetLocation;
            workOrderDevExpressPrintModel.DownRequired = WorkOrderDetails.DownRequired ? "Yes" : "No";
            workOrderDevExpressPrintModel.Type = WorkOrderDetails.Type;
            workOrderDevExpressPrintModel.SourceType = WorkOrderDetails.SourceType;
            if (WorkOrderDetails.RequiredDate == null || WorkOrderDetails.RequiredDate == default(DateTime))
            {
                workOrderDevExpressPrintModel.RequiredDate = "";
            }
            else
            {
                workOrderDevExpressPrintModel.RequiredDate = Convert.ToDateTime(WorkOrderDetails.RequiredDate)
                    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            if (WorkOrderDetails.ScheduledStartDate == null || WorkOrderDetails.ScheduledStartDate == default(DateTime))
            {
                workOrderDevExpressPrintModel.ScheduledStartDate = "";
            }
            else
            {
                workOrderDevExpressPrintModel.ScheduledStartDate = Convert.ToDateTime(WorkOrderDetails.ScheduledStartDate)
                    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            workOrderDevExpressPrintModel.ScheduledDuration = WorkOrderDetails.ScheduledDuration;
            if (!string.IsNullOrEmpty(WorkOrderDetails.Assigned))
            {
                workOrderDevExpressPrintModel.Assigned = WorkOrderDetails.Assigned;
            }
            else if (!string.IsNullOrEmpty(WorkOrderDetails.AssignedFullName))
            {
                workOrderDevExpressPrintModel.Assigned = WorkOrderDetails.AssignedFullName;
            }

            workOrderDevExpressPrintModel.CreateBy = WorkOrderDetails.Createby;
            workOrderDevExpressPrintModel.CreateByPersonnelName = WorkOrderDetails.CreateBy_PersonnelName;
            workOrderDevExpressPrintModel.CreateDate = Convert.ToDateTime(WorkOrderDetails.CreateDate)
                    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            var SiteInformation = userData.DatabaseKey.Client.WOPrintMessage.ToString().Replace("\n", " <br> ");
            workOrderDevExpressPrintModel.SiteInformation = HttpUtility.HtmlDecode(SiteInformation);
            workOrderDevExpressPrintModel.AzureImageUrl = AzureImageUrl;

            workOrderDevExpressPrintModel.CompleteBy_PersonnelId = WorkOrderDetails.CompleteBy_PersonnelId;
            workOrderDevExpressPrintModel.CompleteBy_PersonnelClientLookupId = WorkOrderDetails.CompleteBy_PersonnelClientLookupId + "(" + WorkOrderDetails.CompleteBy_PersonnelName + ")";
            if (WorkOrderDetails.CompleteDate == null || WorkOrderDetails.CompleteDate == default(DateTime))
            {
                workOrderDevExpressPrintModel.CompleteDate = "";
            }
            else
            {
                workOrderDevExpressPrintModel.CompleteDate = Convert.ToDateTime(WorkOrderDetails.CompleteDate)
                    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            workOrderDevExpressPrintModel.CompleteComments = WorkOrderDetails.CompleteComments;
            workOrderDevExpressPrintModel.Status = WorkOrderDetails.Status;
            if (userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices
                    && WorkOrderDetails.Status == WorkOrderStatusConstants.Complete
                    && WorkOrderDetails.SignoffBy_PersonnelId > 0)
            {
                workOrderDevExpressPrintModel.IsFoodSafetyShow = true;
            }
            workOrderDevExpressPrintModel.SignoffBy = WorkOrderDetails.SignoffBy_PersonnelClientLookupId + "(" + WorkOrderDetails.SignoffBy_PersonnelClientLookupIdName + ")";

            workOrderDevExpressPrintModel.EquipmentType = WorkOrderDetails.EquipmentType;
            workOrderDevExpressPrintModel.SerialNumber = WorkOrderDetails.SerialNumber;
            workOrderDevExpressPrintModel.Make = WorkOrderDetails.Make;
            workOrderDevExpressPrintModel.Model = WorkOrderDetails.Model;
            workOrderDevExpressPrintModel.AssetGroup1ClientlookupId = WorkOrderDetails.AssetGroup1ClientlookupId;
            workOrderDevExpressPrintModel.AssetGroup2ClientlookupId = WorkOrderDetails.AssetGroup2ClientlookupId;
            workOrderDevExpressPrintModel.AssetGroup3ClientlookupId = WorkOrderDetails.AssetGroup3ClientlookupId;
            workOrderDevExpressPrintModel.RemoveFromService = WorkOrderDetails.RemoveFromService;
            workOrderDevExpressPrintModel.AssetUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/Equipment/RedirectFromWorkOrderPrint?EquipmentId=" + WorkOrderDetails.ChargeToId;
            workOrderDevExpressPrintModel.WOCompCriteriaTitle = WorkOrderDetails.WOCompCriteriaTitle;
            workOrderDevExpressPrintModel.WOCompCriteria = WorkOrderDetails.WOCompCriteria;
            workOrderDevExpressPrintModel.WOCompCriteriaTab = WorkOrderDetails.WOCompCriteriaTab;
            workOrderDevExpressPrintModel.Priority = WorkOrderDetails.Priority; //V2-944
            workOrderDevExpressPrintModel.AccountClientLookupId = WorkOrderDetails.AccountClientLookupId;

            #region Localization
            workOrderDevExpressPrintModel.spnDetails = UtilityFunction.GetMessageFromResource("spnDetails", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.SpnAsset = UtilityFunction.GetMessageFromResource("spnAsset", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.SpnMake = UtilityFunction.GetMessageFromResource("spnMake", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.SpnModel = UtilityFunction.GetMessageFromResource("spnModel", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.GlobalSerialNumber = UtilityFunction.GetMessageFromResource("GlobalSerialNumber", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.AssetGroup1Label = userData.Site.AssetGroup1Name;
            workOrderDevExpressPrintModel.AssetGroup2Label = userData.Site.AssetGroup2Name;
            workOrderDevExpressPrintModel.AssetGroup3Label = userData.Site.AssetGroup3Name;
            workOrderDevExpressPrintModel.SpnLocation = UtilityFunction.GetMessageFromResource("spnLocation", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.GlobalType = UtilityFunction.GetMessageFromResource("GlobalType", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnRequired = UtilityFunction.GetMessageFromResource("spnRequiredDate", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnDownRequired = UtilityFunction.GetMessageFromResource("spnDownRequired", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnScheduledDate = UtilityFunction.GetMessageFromResource("spnScheduledDate", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnScheduledDuration = UtilityFunction.GetMessageFromResource("spnScheduleDuration", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnCreateBy = UtilityFunction.GetMessageFromResource("spnCreatedBy", LocalizeResourceSetConstants.WorkOrderDetails);
            workOrderDevExpressPrintModel.spnWorkAssigned = UtilityFunction.GetMessageFromResource("spnWorkAssigned", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnAddInstructions = UtilityFunction.GetMessageFromResource("spnAddInstructions", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnDate = UtilityFunction.GetMessageFromResource("spnDate", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnHours = UtilityFunction.GetMessageFromResource("spnHours", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnWoEmployeeName = UtilityFunction.GetMessageFromResource("spnWoEmployeeName", LocalizeResourceSetConstants.WorkOrderDetails);
            workOrderDevExpressPrintModel.spnAccount = UtilityFunction.GetMessageFromResource("spnAccount", LocalizeResourceSetConstants.Global);





            workOrderDevExpressPrintModel.spnCompleteComments = UtilityFunction.GetMessageFromResource("spnCompleteComments", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnFoodSafety_1 = UtilityFunction.GetMessageFromResource("spnFoodSafety_1", LocalizeResourceSetConstants.WorkOrderDetails);
            workOrderDevExpressPrintModel.spnFoodSafety_2 = UtilityFunction.GetMessageFromResource("spnFoodSafety_2", LocalizeResourceSetConstants.WorkOrderDetails);
            workOrderDevExpressPrintModel.spnFoodSafety_3 = UtilityFunction.GetMessageFromResource("spnFoodSafety_3", LocalizeResourceSetConstants.WorkOrderDetails);
            workOrderDevExpressPrintModel.spnFoodSafety_4 = UtilityFunction.GetMessageFromResource("spnFoodSafety_4", LocalizeResourceSetConstants.WorkOrderDetails);
            workOrderDevExpressPrintModel.spnFoodSafety_5 = UtilityFunction.GetMessageFromResource("spnFoodSafety_5", LocalizeResourceSetConstants.WorkOrderDetails);
            workOrderDevExpressPrintModel.spnSignoffBy = UtilityFunction.GetMessageFromResource("spnSignoffBy", LocalizeResourceSetConstants.WorkOrderDetails);
            workOrderDevExpressPrintModel.spnOn = UtilityFunction.GetMessageFromResource("spnOn", LocalizeResourceSetConstants.WorkOrderDetails);
            workOrderDevExpressPrintModel.spnSignature = UtilityFunction.GetMessageFromResource("spnSignature", LocalizeResourceSetConstants.WorkOrderDetails);
            workOrderDevExpressPrintModel.GlobalWorkOrder = UtilityFunction.GetMessageFromResource("GlobalWorkOrder", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.globalCreateDate = UtilityFunction.GetMessageFromResource("globalCreateDate", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnCopyRights = UtilityFunction.GetMessageFromResource("spnCopyRights", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnGlobalCompleted = UtilityFunction.GetMessageFromResource("spnGlobalCompleted", LocalizeResourceSetConstants.Global);
            #region V2-944
            workOrderDevExpressPrintModel.spnAssetGroups = UtilityFunction.GetMessageFromResource("spnAssetGroups", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnWorkOrderUIC = UtilityFunction.GetMessageFromResource("spnWorkOrderUIC", LocalizeResourceSetConstants.WorkOrderDetails);
            workOrderDevExpressPrintModel.GlobalStatus = UtilityFunction.GetMessageFromResource("GlobalStatus", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnPriority = UtilityFunction.GetMessageFromResource("spnPriority", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnScheduling = UtilityFunction.GetMessageFromResource("spnScheduling", LocalizeResourceSetConstants.Global);
            workOrderDevExpressPrintModel.spnLaborRecording = UtilityFunction.GetMessageFromResource("spnLaborRecording", LocalizeResourceSetConstants.WorkOrderDetails);
            #endregion
            #endregion
        }
        //we need to add colorhelper nuget package
        //private string ChangeHSLColorCodeToHexCode(string htmlString)
        //{
        //    if (htmlString.Contains("color:hsl("))
        //    {
        //        int h = -1, s = -1, l = -1;

        //        //var rx = new Regex(@"color:hsl\([0-9]+,[0-9]+%,[0-9]+%\)", RegexOptions.Compiled | RegexOptions.IgnoreCase |RegexOptions.IgnorePatternWhitespace);
        //        var rx = new Regex(@"color:hsl\(\s*[0-9]+\s*,\s*[0-9]+\s*%\s*,\s*[0-9]+\s*%\s*\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //        Match match = rx.Match(htmlString);
        //        while (match.Success)
        //        {
        //            var findHSL = new Regex(@"[0-9]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //            Match matchHSL = findHSL.Match(match.Value.ToString());
        //            if (matchHSL.Success)
        //            {
        //                if (int.TryParse(matchHSL.Value, out h))
        //                {
        //                    matchHSL = matchHSL.NextMatch();
        //                    if (matchHSL.Success && int.TryParse(matchHSL.Value, out s))
        //                    {
        //                        matchHSL = matchHSL.NextMatch();
        //                        if (matchHSL.Success && int.TryParse(matchHSL.Value, out l))
        //                        {
        //                            //matchHSL = matchHSL.NextMatch();
        //                        }
        //                    }
        //                }
        //            }
        //            if (h != -1 && s != -1 && l != -1)
        //            {
        //                HEX hex = ColorConverter.HslToHex(new HSL(h, (byte)s, (byte)l));
        //                htmlString = htmlString.Replace(match.Value, "color:#" + hex.ToString());
        //            }
        //            match = match.NextMatch();
        //            h = s = l = -1;
        //        }
        //    }
        //    return htmlString;
        //}
        private void BindTaskTable(List<WorkOrderTask> listOfWOTasks, ref WorkOrderDevExpressPrintModel workOrderDevExpressPrintModel)
        {
            if (listOfWOTasks.Count > 0)
            {
                foreach (var item in listOfWOTasks)
                {
                    var objTasksDevExpressPrintModel = new TasksDevExpressPrintModel();

                    objTasksDevExpressPrintModel.WorkOrderTaskId = item.WorkOrderTaskId;
                    objTasksDevExpressPrintModel.TaskNumber = item.TaskNumber;
                    objTasksDevExpressPrintModel.Description = item.Description;
                    objTasksDevExpressPrintModel.ScheduledDuration = item.ScheduledDuration;
                    objTasksDevExpressPrintModel.Completed = (item.Status == WorkOrderStatusConstants.Complete ? "Yes" : "No");
                    objTasksDevExpressPrintModel.spnTasks = UtilityFunction.GetMessageFromResource("spnTasks", LocalizeResourceSetConstants.Global);
                    objTasksDevExpressPrintModel.spnOrder = UtilityFunction.GetMessageFromResource("spnOrder", LocalizeResourceSetConstants.Global);
                    objTasksDevExpressPrintModel.spnSchedDuration = UtilityFunction.GetMessageFromResource("spnSchedDuration", LocalizeResourceSetConstants.WorkOrderDetails);
                    objTasksDevExpressPrintModel.spnGlobalCompleted = UtilityFunction.GetMessageFromResource("spnGlobalCompleted", LocalizeResourceSetConstants.Global);
                    objTasksDevExpressPrintModel.spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global);
                    workOrderDevExpressPrintModel.TasksDevExpressPrintModelList.Add(objTasksDevExpressPrintModel);

                }
            }
        }
        private void BindLaborTable(List<Timecard> listOfTimecards, ref WorkOrderDevExpressPrintModel workOrderDevExpressPrintModel)
        {
            if (listOfTimecards.Count > 0)
            {
                foreach (var item in listOfTimecards)
                {
                    var objLaborDevExpressPrintModel = new LaborDevExpressPrintModel();

                    objLaborDevExpressPrintModel.TimecardId = item.TimecardId;
                    objLaborDevExpressPrintModel.PersonnelID = item.PersonnelId;
                    objLaborDevExpressPrintModel.PersonnelClientLookupId = item.PersonnelClientLookupId;
                    objLaborDevExpressPrintModel.PersonnelName = item.NameFull.Trim(',');
                    if (item.StartDate == null || item.StartDate == default(DateTime))
                    {
                        objLaborDevExpressPrintModel.StartDate = "";
                    }
                    else
                    {
                        objLaborDevExpressPrintModel.StartDate = Convert.ToDateTime(item.StartDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    objLaborDevExpressPrintModel.Hours = item.Hours;
                    objLaborDevExpressPrintModel.UnitCost = item.TCValue;
                    objLaborDevExpressPrintModel.spnLabor = UtilityFunction.GetMessageFromResource("spnLabor", LocalizeResourceSetConstants.Global);
                    objLaborDevExpressPrintModel.spnPersonnelID = UtilityFunction.GetMessageFromResource("spnPersonnelID", LocalizeResourceSetConstants.WorkOrderDetails);
                    objLaborDevExpressPrintModel.spnName = UtilityFunction.GetMessageFromResource("spnName", LocalizeResourceSetConstants.Global);
                    objLaborDevExpressPrintModel.spnDate = UtilityFunction.GetMessageFromResource("spnDate", LocalizeResourceSetConstants.Global);
                    objLaborDevExpressPrintModel.spnHours = UtilityFunction.GetMessageFromResource("spnHours", LocalizeResourceSetConstants.Global);
                    objLaborDevExpressPrintModel.spnUnitCost = UtilityFunction.GetMessageFromResource("spnUnitCost", LocalizeResourceSetConstants.Global);
                    objLaborDevExpressPrintModel.spnTotalCost = UtilityFunction.GetMessageFromResource("spnTotalCost", LocalizeResourceSetConstants.Global);
                    workOrderDevExpressPrintModel.LaborDevExpressPrintModelList.Add(objLaborDevExpressPrintModel);
                }

            }
        }
        private void BindPartTable(List<PartHistory> listOfPartHistorys, ref WorkOrderDevExpressPrintModel workOrderDevExpressPrintModel)
        {
            if (listOfPartHistorys.Count > 0)
            {
                foreach (var item in listOfPartHistorys)
                {
                    var objPartDevExpressPrintModel = new PartDevExpressPrintModel();

                    objPartDevExpressPrintModel.PartClientlookupId = item.PartClientLookupId;
                    objPartDevExpressPrintModel.Description = item.Description;
                    objPartDevExpressPrintModel.UnitCost = item.Cost;
                    objPartDevExpressPrintModel.UOM = item.UnitofMeasure;
                    objPartDevExpressPrintModel.Quantity = item.TransactionQuantity;
                    objPartDevExpressPrintModel.TotalCost = item.TotalCost;
                    objPartDevExpressPrintModel.spnPart = UtilityFunction.GetMessageFromResource("spnPart", LocalizeResourceSetConstants.Global);
                    objPartDevExpressPrintModel.spnParts = UtilityFunction.GetMessageFromResource("spnParts", LocalizeResourceSetConstants.Global);
                    objPartDevExpressPrintModel.spnUnitCost = UtilityFunction.GetMessageFromResource("spnUnitCost", LocalizeResourceSetConstants.Global);
                    objPartDevExpressPrintModel.spnUOM = UtilityFunction.GetMessageFromResource("spnUOM", LocalizeResourceSetConstants.WorkOrderDetails);
                    objPartDevExpressPrintModel.spnQuantity = UtilityFunction.GetMessageFromResource("spnQuantity", LocalizeResourceSetConstants.Global);
                    objPartDevExpressPrintModel.spnTotalCost = UtilityFunction.GetMessageFromResource("spnTotalCost", LocalizeResourceSetConstants.Global);
                    objPartDevExpressPrintModel.spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global);
                    workOrderDevExpressPrintModel.PartDevExpressPrintModelList.Add(objPartDevExpressPrintModel);
                }
            }
        }
        private void BindOthersTable(List<OtherCosts> listOtherCosts, ref WorkOrderDevExpressPrintModel workOrderDevExpressPrintModel)
        {
            if (listOtherCosts.Count > 0)
            {
                foreach (var item in listOtherCosts)
                {
                    var objOthersDevExpressPrintModel = new OthersDevExpressPrintModel();

                    objOthersDevExpressPrintModel.Source = item.Source;
                    objOthersDevExpressPrintModel.Description = item.Description;
                    objOthersDevExpressPrintModel.VendorId = item.VendorId;
                    objOthersDevExpressPrintModel.VendorClientLookupId = item.VendorClientLookupId;
                    objOthersDevExpressPrintModel.UnitCost = item.UnitCost;
                    objOthersDevExpressPrintModel.Quantity = item.Quantity;
                    objOthersDevExpressPrintModel.TotalCost = item.TotalCost;
                    objOthersDevExpressPrintModel.spnOthers = UtilityFunction.GetMessageFromResource("spnOthers", LocalizeResourceSetConstants.WorkOrderDetails);
                    objOthersDevExpressPrintModel.spnSource = UtilityFunction.GetMessageFromResource("spnSource", LocalizeResourceSetConstants.Global);
                    objOthersDevExpressPrintModel.spnVendor = UtilityFunction.GetMessageFromResource("spnVendor", LocalizeResourceSetConstants.Global);
                    objOthersDevExpressPrintModel.spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global);
                    objOthersDevExpressPrintModel.spnTotalCost = UtilityFunction.GetMessageFromResource("spnTotalCost", LocalizeResourceSetConstants.Global);
                    objOthersDevExpressPrintModel.spnUnitCost = UtilityFunction.GetMessageFromResource("spnUnitCost", LocalizeResourceSetConstants.Global);
                    objOthersDevExpressPrintModel.spnQuantity = UtilityFunction.GetMessageFromResource("spnQuantity", LocalizeResourceSetConstants.Global);
                    workOrderDevExpressPrintModel.OthersDevExpressPrintModelList.Add(objOthersDevExpressPrintModel);
                }
            }
        }
        private void BindSummaryTable(List<OtherCosts> listOfSummary, ref WorkOrderDevExpressPrintModel workOrderDevExpressPrintModel)
        {
            if (listOfSummary.Count > 0)
            {
                foreach (var item in listOfSummary)
                {
                    var objSummaryDevExpressPrintModel = new SummaryDevExpressPrintModel();

                    objSummaryDevExpressPrintModel.PartsCosts = item.TotalPartCost;
                    objSummaryDevExpressPrintModel.CraftsCosts = item.TotalCraftCost;
                    objSummaryDevExpressPrintModel.OtherExternalCosts = item.TotalExternalCost;
                    objSummaryDevExpressPrintModel.OtherInternalCosts = item.TotalInternalCost;
                    objSummaryDevExpressPrintModel.TotalCost = item.TotalSummeryCost;
                    objSummaryDevExpressPrintModel.spnSummary = UtilityFunction.GetMessageFromResource("spnSummary", LocalizeResourceSetConstants.Global);
                    objSummaryDevExpressPrintModel.spnPartsCosts = UtilityFunction.GetMessageFromResource("spnPartsCosts", LocalizeResourceSetConstants.Global);
                    objSummaryDevExpressPrintModel.spnCraftCosts = UtilityFunction.GetMessageFromResource("spnCraftCosts", LocalizeResourceSetConstants.WorkOrderDetails);
                    objSummaryDevExpressPrintModel.spnOtherExternalCosts = UtilityFunction.GetMessageFromResource("spnOtherExternalCosts", LocalizeResourceSetConstants.Global);
                    objSummaryDevExpressPrintModel.spnOtherInternalCosts = UtilityFunction.GetMessageFromResource("spnOtherInternalCosts", LocalizeResourceSetConstants.WorkOrderDetails);
                    objSummaryDevExpressPrintModel.spnTotalCost = UtilityFunction.GetMessageFromResource("spnTotalCost", LocalizeResourceSetConstants.Global);
                    workOrderDevExpressPrintModel.SummaryDevExpressPrintModelList.Add(objSummaryDevExpressPrintModel);
                }
            }
        }
        private void BindInsturctionsTable(List<Instructions> listOfInstruction, ref WorkOrderDevExpressPrintModel workOrderDevExpressPrintModel)
        {
            if (listOfInstruction != null && listOfInstruction.Count > 0)
            {
                workOrderDevExpressPrintModel.Instructions = HttpUtility.HtmlDecode(string.Join("<br/>", listOfInstruction.Select(x => x.Contents)));
            }
        }
        private void BindPhotosTable(List<AttachmentModel> attachmentModels, ref WorkOrderDevExpressPrintModel workOrderDevExpressPrintModel)
        {
            if (attachmentModels.Count > 0)
            {
                foreach (var item in attachmentModels)
                {
                    var objPhotosDevExpressPrintModelList = new PhotosDevExpressPrintModel();


                    if (userData.DatabaseKey.Client.OnPremise)
                    {
                        objPhotosDevExpressPrintModelList.PhotoUrl = UtilityFunction.Base64SrcDevexpress(item.AttachmentUrl);
                    }
                    else
                    {
                        objPhotosDevExpressPrintModelList.PhotoUrl = item.AttachmentUrl;
                    }
                    objPhotosDevExpressPrintModelList.FileName = item.FileName;
                    objPhotosDevExpressPrintModelList.FileType = item.ContentType;
                    objPhotosDevExpressPrintModelList.FileSize = item.FileSize;
                    objPhotosDevExpressPrintModelList.spnPhotos = UtilityFunction.GetMessageFromResource("spnPhotos", LocalizeResourceSetConstants.WorkOrderDetails);
                    objPhotosDevExpressPrintModelList.spnDetails = UtilityFunction.GetMessageFromResource("spnDetails", LocalizeResourceSetConstants.Global);
                    workOrderDevExpressPrintModel.PhotosDevExpressPrintModelList.Add(objPhotosDevExpressPrintModelList);
                }
            }
        }
        private void BindPdfAttachmentTable(List<AttachmentModel> WoPdfAttachment, ref WorkOrderDevExpressPrintModel workOrderDevExpressPrintModel)
        {
            if (WoPdfAttachment != null && WoPdfAttachment.Count > 0)
            {
                CommonWrapper comWrapper = new CommonWrapper(userData);
                foreach (var itemAttach in WoPdfAttachment)
                {
                    var objAttachmentDevExpressPrintModel = new AttachmentDevExpressPrintModel();
                    var attachUrl = itemAttach.AttachmentUrl;
                    // 2023-Mar-15
                    // V2-
                    if (userData.DatabaseKey.Client.OnPremise)
                    {
                        objAttachmentDevExpressPrintModel.SASUrl = UtilityFunction.Base64SrcDevexpress(attachUrl);
                    }
                    else
                    {
                        objAttachmentDevExpressPrintModel.SASUrl = comWrapper.GetSasAttachmentUrl(ref attachUrl);
                    }
                    workOrderDevExpressPrintModel.AttachmentDevExpressPrintModelList.Add(objAttachmentDevExpressPrintModel);
                }
            }
        }
        private void BindCommentsTable(List<Notes> notes, ref WorkOrderDevExpressPrintModel workOrderDevExpressPrintModel)
        {
            if (notes != null && notes.Count > 0)
            {
                foreach (var item in notes.OrderBy(n => n.ObjectId))
                {
                    var note = new NotesDevExpressPrintModel();
                    note.WorkOrderId = item.ObjectId;
                    note.Comments = item.Content;
                    note.OwnerName = item.OwnerName;
                    if (item.CreateDate == null || item.CreateDate == default(DateTime))
                    {
                        note.CreateDate = "";
                    }
                    else
                    {
                        note.CreateDate = Convert.ToDateTime(item.CreateDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    note.SpnLogComment = UtilityFunction.GetMessageFromResource("spnLogComment", LocalizeResourceSetConstants.Global);
                    note.spnDate = UtilityFunction.GetMessageFromResource("spnDate", LocalizeResourceSetConstants.Global);
                    note.spnCommenter = UtilityFunction.GetMessageFromResource("spnCommenter", LocalizeResourceSetConstants.Global);
                    note.spnComment = UtilityFunction.GetMessageFromResource("spnComment", LocalizeResourceSetConstants.Global);
                    workOrderDevExpressPrintModel.NotesDevExpressPrintModelList.Add(note);
                }
            }
        }
        public JsonResult PrintPDFFromIndex(WoPrintingModel model, string PrintingCountConnectionID)
        {
            var ms = new MemoryStream();
            bool jsonStringExceed = false;
            HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            Int64 fileSizeCounter = 0;
            Int32 maxPdfSize = section.MaxRequestLength;
            string attachUrl = string.Empty;
            var doc = new iTextSharp.text.Document();
            var copy = new PdfSmartCopy(doc, ms);
            doc.Open();
            int NoOfWorkOrderPrinted = 0;
            int totalWo = model.list.Count;
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            woWrapper.SendClientProgressBarCurrentStatus(totalWo, NoOfWorkOrderPrinted, PrintingCountConnectionID);
            var WorkOrderIDList = model.list.Select(x => x.WorkOrderId).ToList();
            var WorkOrderBunchListInfo = woWrapper.RetrieveAllByWorkOrdeV2Print(WorkOrderIDList);
            List<WorkOrder> listOfWO = WorkOrderBunchListInfo.listOfWO;
            List<WorkOrderTask> listOfWOTask = WorkOrderBunchListInfo.listOfWOTask;
            List<Timecard> listOfTimecard = WorkOrderBunchListInfo.listOfTimecard;
            List<PartHistory> listOfPartHistory = WorkOrderBunchListInfo.listOfPartHistory;
            List<OtherCosts> listOfSummery = WorkOrderBunchListInfo.listOfSummery;
            List<Attachment> listOfAttachment = WorkOrderBunchListInfo.listOfAttachment;
            List<OtherCosts> listOfOtherCosts = WorkOrderBunchListInfo.listOfOtherCosts;
            List<Instructions> listOfInstructions = WorkOrderBunchListInfo.listOfInstructions;
            foreach (var item in model.list)
            {
                CommonWrapper comWrapper = new CommonWrapper(userData);
                string AzureImageURL = comWrapper.GetClientLogoUrl();

                List<AttachmentModel> AttachmentModelList = comWrapper.PopulateAttachmentForPrint(listOfAttachment.Where(m => m.ObjectId == item.WorkOrderId).ToList(), item.WorkOrderId, "woprint");
                List<AttachmentModel> WoPdfAttachment = new List<AttachmentModel>();
                WoPdfAttachment = AttachmentModelList;
                if (WoPdfAttachment != null && WoPdfAttachment.Count > 0)
                {
                    WoPdfAttachment = WoPdfAttachment.Where(x => x.ContentType.Contains("pdf") && x.Profile == false && x.Image == false).ToList();
                }
                if (AttachmentModelList != null && AttachmentModelList.Count > 0)
                {
                    AttachmentModelList = AttachmentModelList.Where(x => x.Profile == true && x.Image == true).ToList();
                }

                WorkOrder WorkOrderDetails = listOfWO.Where(m => m.WorkOrderId == item.WorkOrderId).FirstOrDefault();
                List<WorkOrderTask> listOfWOTasks = listOfWOTask.Where(m => m.WorkOrderId == item.WorkOrderId).ToList();
                List<Timecard> listOfTimecards = listOfTimecard.Where(m => m.ChargeToId_Primary == item.WorkOrderId).ToList();
                List<PartHistory> listOfPartHistorys = listOfPartHistory.Where(m => m.ChargeToId_Primary == item.WorkOrderId).ToList();
                List<OtherCosts> listOtherCosts = listOfOtherCosts.Where(m => m.ObjectId == item.WorkOrderId).ToList();
                List<OtherCosts> listOfSummary = listOfSummery.Where(m => m.ObjectId == item.WorkOrderId).ToList();
                List<Instructions> listOfInstruction = listOfInstructions.Where(m => m.ObjectId == item.WorkOrderId).ToList();

                var msSinglePDf = new MemoryStream(PrintPdfImageGetByteStream_V2(item.WorkOrderId, AttachmentModelList, WorkOrderDetails, listOfWOTasks, listOfTimecards, listOfPartHistorys, listOtherCosts, listOfSummary, listOfInstruction, AzureImageURL));

                var reader = new iTextSharp.text.pdf.PdfReader(msSinglePDf);
                fileSizeCounter += reader.FileLength;
                if (fileSizeCounter < maxPdfSize)
                {

                    copy.AddDocument(reader);
                }
                else
                {
                    jsonStringExceed = true;
                    break;

                }
                if (WoPdfAttachment != null && WoPdfAttachment.Count > 0)
                {
                    foreach (var itemAttach in WoPdfAttachment)
                    {
                        fileSizeCounter += itemAttach.FileSize;
                        if (fileSizeCounter < maxPdfSize)
                        {
                            attachUrl = string.Empty;
                            attachUrl = itemAttach.AttachmentUrl;
                            attachUrl = comWrapper.GetSasAttachmentUrl(ref attachUrl);
                            copy.AddDocument(new PdfReader(attachUrl));
                        }
                        else
                        {
                            jsonStringExceed = true;
                            break;
                        }
                    }
                }
                NoOfWorkOrderPrinted++;
                woWrapper.SendClientProgressBarCurrentStatus(totalWo, NoOfWorkOrderPrinted, PrintingCountConnectionID);


            }

            doc.Close();
            byte[] pdf = ms.ToArray();
            string strPdf = System.Convert.ToBase64String(pdf);
            if (jsonStringExceed)
            {
                strPdf = "";
            }
            var returnOjb = new { success = true, pdf = strPdf, jsonStringExceed = jsonStringExceed };
            var jsonResult = Json(returnOjb, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult DenyWoList(WoCancelAndPrintModel model)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM(); WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);

            List<string> errorMessage = new List<string>();

            StringBuilder failedWoList = new StringBuilder();

            foreach (var item in model.list)
            {
                if (item.Status != WorkOrderStatusConstants.WorkRequest)
                {
                    failedWoList.Append(item.ClientLookupId + ",");
                }
                else
                {
                    string Statusmsg = string.Empty;
                    WorkOrder Wojob = woWrapper.DenyWO(item.WorkOrderId, model.comments, ref Statusmsg);
                    if (Wojob.ErrorMessages != null && Wojob.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to deny " + Wojob.ClientLookupId + ": " + Wojob.ErrorMessages[0];
                        errorMessage.Add(errormessage);
                    }
                }
            }
            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errorMessage.Add("Work Order(s) " + failedWoList + " can't be denied. Please check the status.");
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ApproveWoList(WoApproveModel model)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM(); WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);

            List<string> errorMessage = new List<string>();

            System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();

            foreach (var item in model.list)
            {
                // RKL - V2-576 - added the check for awaiting approval
                if (item.Status != WorkOrderStatusConstants.WorkRequest && item.Status != WorkOrderStatusConstants.AwaitingApproval)
                {
                    failedWoList.Append(item.ClientLookupId + ",");
                }
                else
                {
                    string Statusmsg = string.Empty;
                    WorkOrder Wo = woWrapper.ApproveWO(item.WorkOrderId, ref Statusmsg);

                    if (Wo.ErrorMessages != null && Wo.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to approve " + Wo.ClientLookupId + ": " + Wo.ErrorMessages[0];
                        errorMessage.Add(errormessage);
                    }
                }
            }
            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errorMessage.Add("Work Order(s) " + failedWoList + " can't be approve. Please check the status.");
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region GetFoodServiesMessage
        public ActionResult GetFoodServicesMessage()
        {

            string FoodServicesMessage = string.Empty;
            FoodServicesMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            FoodServicesMessage = "<div class='list'>" +
           "<p>" + UtilityFunction.GetMessageFromResource("spnFoodSafety_1", LocalizeResourceSetConstants.WorkOrderDetails) + "</p><ul style='list-style-type:none;'>" +
            "<li>" + UtilityFunction.GetMessageFromResource("spnFoodSafety_2", LocalizeResourceSetConstants.WorkOrderDetails) + "</li>" +
            "<li>" + UtilityFunction.GetMessageFromResource("spnFoodSafety_3", LocalizeResourceSetConstants.WorkOrderDetails) + "</li>" +
            "<li>" + UtilityFunction.GetMessageFromResource("spnFoodSafety_4", LocalizeResourceSetConstants.WorkOrderDetails) + "</li>" +
            "<li>" + UtilityFunction.GetMessageFromResource("spnFoodSafety_5", LocalizeResourceSetConstants.WorkOrderDetails) + "</li>" +
          "</ul></div>";
            return Json(new { data = FoodServicesMessage }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Private Method
        private WorkOrderSummaryModel GetWorkOrderSummaryModel(long WorkOrderId, string ClientLookupId, string Status, string Description, string ProjectClientlookupId = "", WorkOrderModel workOrder = null)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrderSummaryModel workOrderSummaryModel = new WorkOrderSummaryModel();
            CommonWrapper comWrapper = new CommonWrapper(userData);
            workOrderSummaryModel.WorkOrderId = WorkOrderId;
            workOrderSummaryModel.WorkOrder_ClientLookupId = ClientLookupId;
            workOrderSummaryModel.Status = Status;
            workOrderSummaryModel.Description = Description;
            if (workOrder != null)
            {
                workOrderSummaryModel.PartsonOrder = workOrder.PartsOnOrder;
            }
            workOrderSummaryModel.ProjectClientLookupId = ProjectClientlookupId;
            workOrderSummaryModel.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            workOrderSummaryModel.ImageUrl = WorkOrderImageUrl(WorkOrderId);
            return workOrderSummaryModel;
        }

        private WorkOrderSummaryModel GetDowntimeforWorkOrderSummary(WorkOrderSummaryModel workOrderSummaryModel, string downtime, string downminutes)
        {
            if (!string.IsNullOrEmpty(downtime))
            {
                workOrderSummaryModel.EquipDownDate = Convert.ToDateTime(downtime);
            }
            workOrderSummaryModel.EquipDownHours = !string.IsNullOrEmpty(downminutes) ? Convert.ToDecimal(downminutes) : 0;
            return workOrderSummaryModel;
        }
        private WorkOrderSummaryModel GetAllWorkOrderSummaryModel(WorkOrderVM objWorksOrderVM, WorkOrderSummaryModel workOrderSummaryModel, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string AssetLocation = "")
        {
            workOrderSummaryModel.Type = Type;
            workOrderSummaryModel.Priority = Priority;
            workOrderSummaryModel.ChargeToClientLookupId = ChargeTo;
            workOrderSummaryModel.ChargeTo_Name = ChargeToName;
            if (!string.IsNullOrEmpty(ScheduledDate))
            {
                workOrderSummaryModel.ScheduledStartDate = DateTime.ParseExact(ScheduledDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            workOrderSummaryModel.AssignedFullName = Assigned;
            if (!string.IsNullOrEmpty(completedate))
            {
                workOrderSummaryModel.CompleteDate = DateTime.ParseExact(completedate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            workOrderSummaryModel.security = this.userData.Security;
            workOrderSummaryModel.AssetLocation = AssetLocation;
            return workOrderSummaryModel;
        }
        private List<WoActualLabor> GetAllActualLaborSortByColumnWithOrder(string order, string orderDir, List<WoActualLabor> data)
        {
            List<WoActualLabor> lst = new List<WoActualLabor>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PersonnelClientLookupId).ToList() : data.OrderBy(p => p.PersonnelClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NameFull).ToList() : data.OrderBy(p => p.NameFull).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StartDate).ToList() : data.OrderBy(p => p.StartDate).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Hours).ToList() : data.OrderBy(p => p.Hours).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TCValue).ToList() : data.OrderBy(p => p.TCValue).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PersonnelClientLookupId).ToList() : data.OrderBy(p => p.PersonnelClientLookupId).ToList();
                    break;
            }

            return lst;
        }
        private List<ActualOther> GetAllActualOtherSortByColumnWithOrder(string order, string orderDir, List<ActualOther> data)
        {
            List<ActualOther> lst = new List<ActualOther>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Source).ToList() : data.OrderBy(p => p.Source).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorClientLookupId).ToList() : data.OrderBy(p => p.VendorClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Source).ToList() : data.OrderBy(p => p.Source).ToList();
                    break;
            }
            return lst;
        }
        private List<WorkOrderSchedule> GetAllAssignmentSortByColumnWithOrder(string order, string orderDir, List<WorkOrderSchedule> data)
        {
            List<WorkOrderSchedule> lst = new List<WorkOrderSchedule>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssignedTo_PersonnelClientLookupId).ToList() : data.OrderBy(p => p.AssignedTo_PersonnelClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.WorkAssigned_Name).ToList() : data.OrderBy(p => p.WorkAssigned_Name).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScheduledStartDate).ToList() : data.OrderBy(p => p.ScheduledStartDate).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScheduledHours).ToList() : data.OrderBy(p => p.ScheduledHours).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssignedTo_PersonnelClientLookupId).ToList() : data.OrderBy(p => p.AssignedTo_PersonnelClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        private List<Notes> GetAllNotesSortByColumnWithOrder(string order, string orderDir, List<Notes> data)
        {
            List<Notes> lst = new List<Notes>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ModifiedDate).ToList() : data.OrderBy(p => p.ModifiedDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
            }
            return lst;
        }
        private List<DataContracts.FileInfo> GetAllAttachmentSortByColumnWithOrder(string order, string orderDir, List<DataContracts.FileInfo> data)
        {
            List<DataContracts.FileInfo> lst = new List<DataContracts.FileInfo>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileName).ToList() : data.OrderBy(p => p.FileName).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileSizeWithUnit).ToList() : data.OrderBy(p => p.FileSizeWithUnit).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
            }
            return lst;
        }
        private List<EstimatePart> GetAllEstimatePartSortByColumnWithOrder(string order, string orderDir, List<EstimatePart> data)
        {
            List<EstimatePart> lst = new List<EstimatePart>();
            if (data != null)
            {
                switch (order)
                {
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Unit).ToList() : data.OrderBy(p => p.Unit).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                        break;
                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AccountClientLookupId).ToList() : data.OrderBy(p => p.AccountClientLookupId).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }
        private List<EstimateLabor> GetAllEstimateLaborsSortByColumnWithOrder(string order, string orderDir, List<EstimateLabor> data)
        {
            List<EstimateLabor> lst = new List<EstimateLabor>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Duration).ToList() : data.OrderBy(p => p.Duration).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        private List<EstimateOther> GetAllEstimateOthersSortByColumnWithOrder(string order, string orderDir, List<EstimateOther> data)
        {
            List<EstimateOther> lst = new List<EstimateOther>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Source).ToList() : data.OrderBy(p => p.Source).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorClientLookupId).ToList() : data.OrderBy(p => p.VendorClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Source).ToList() : data.OrderBy(p => p.Source).ToList();
                    break;
            }
            return lst;
        }

        private List<PartHistoryModel> GetAllActualPartsSortByColumnWithOrder(string order, string orderDir, List<PartHistoryModel> data)
        {
            List<PartHistoryModel> lst = new List<PartHistoryModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionQuantity).ToList() : data.OrderBy(p => p.TransactionQuantity).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Cost).ToList() : data.OrderBy(p => p.Cost).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitofMeasure).ToList() : data.OrderBy(p => p.UnitofMeasure).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionDate).ToList() : data.OrderBy(p => p.TransactionDate).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PerformBy).ToList() : data.OrderBy(p => p.PerformBy).ToList();
                    break;

                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        private List<ActualSummery> GetAllActualEstimateSummerySortByColumnWithOrder(string order, string orderDir, List<ActualSummery> data)
        {
            List<ActualSummery> lst = new List<ActualSummery>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalPartCost).ToList() : data.OrderBy(p => p.TotalPartCost).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCraftCost).ToList() : data.OrderBy(p => p.TotalCraftCost).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalExternalCost).ToList() : data.OrderBy(p => p.TotalExternalCost).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalInternalCost).ToList() : data.OrderBy(p => p.TotalInternalCost).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalSummeryCost).ToList() : data.OrderBy(p => p.TotalSummeryCost).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalPartCost).ToList() : data.OrderBy(p => p.TotalPartCost).ToList();
                    break;
            }
            return lst;
        }

        private List<POLineItemModel> GetAllEstimatePurchaseOrderSortByColumnWithOrder(string order, string orderDir, List<POLineItemModel> data)
        {
            List<POLineItemModel> lst = new List<POLineItemModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LineNumber).ToList() : data.OrderBy(p => p.LineNumber).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OrderQuantity).ToList() : data.OrderBy(p => p.OrderQuantity).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitOfMeasure).ToList() : data.OrderBy(p => p.UnitOfMeasure).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EstimatedDelivery).ToList() : data.OrderBy(p => p.EstimatedDelivery).ToList();
                        break;
                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReceivedQuantity).ToList() : data.OrderBy(p => p.ReceivedQuantity).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }

        private List<WorkOrderTask> GetAllWOTaskOrderSortByColumnWithOrder(string order, string orderDir, List<WorkOrderTask> data)
        {
            List<WorkOrderTask> lst = new List<WorkOrderTask>();
            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaskNumber).ToList() : data.OrderBy(p => p.TaskNumber).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToClientLookupId).ToList() : data.OrderBy(p => p.ChargeToClientLookupId).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaskNumber).ToList() : data.OrderBy(p => p.TaskNumber).ToList();
                    break;
            }
            return lst;
        }
        private List<RequestOrderModel> GetAllRequestOrderSortByColumnWithOrder(string order, string orderDir, List<RequestOrderModel> data)
        {
            List<RequestOrderModel> lst = new List<RequestOrderModel>();
            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Vendor).ToList() : data.OrderBy(p => p.Vendor).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorName).ToList() : data.OrderBy(p => p.VendorName).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Created).ToList() : data.OrderBy(p => p.Created).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region V2-276

        #region Cost
        public JsonResult GetAllCosts(long WorkOrderId, string costtype)
        {
            WorkOrderWrapper workOrderWrapper = new WorkOrderWrapper(userData);
            var data = workOrderWrapper.GetAllCosts(WorkOrderId, costtype);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region V2-288: Request/Order Grid

        [HttpPost]
        public string PopulateRequestOrder(int? draw, int? start, int? length, long workOrderId)
        {
            bool ActionSecurity = false;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<RequestOrderModel> RequestOrderModelList = woWrapper.GetRequestOrder(workOrderId);
            if (RequestOrderModelList != null)
            {
                RequestOrderModelList = this.GetAllRequestOrderSortByColumnWithOrder(order, orderDir, RequestOrderModelList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = RequestOrderModelList.Count();
            totalRecords = RequestOrderModelList.Count();
            int initialPage = start.Value;
            var filteredResult = RequestOrderModelList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, ActionSecurity = ActionSecurity }, JsonSerializerDateSettings);
        }

        #endregion
        #region Add Schedule  /*(V2-293)*/
        [HttpPost]
        public JsonResult AddSchedule(WorkOrderVM woVM)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM(); WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrderModel woModel = new WorkOrderModel();
            System.Text.StringBuilder PersonnelWoList = new System.Text.StringBuilder();
            if (string.IsNullOrEmpty(woVM.woScheduleModel.WorkOrderIds))
            {
                var objWorkOrder = woWrapper.AddScheduleRecord(woVM.woScheduleModel);
                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), workorderid = woVM.woScheduleModel.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string[] workorderIds = woVM.woScheduleModel.WorkOrderIds.Split(',');
                string[] clientLookupIds = woVM.woScheduleModel.ClientLookupIds.Split(',');
                string[] status = woVM.woScheduleModel.Status.Split(',');
                List<BatchCompleteResultModel> WoBatchList = new List<BatchCompleteResultModel>();
                List<string> errorMessage = new List<string>();
                System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();
                for (int i = 0; i < workorderIds.Length; i++)
                {
                    if (status[i] == WorkOrderStatusConstants.Approved || status[i] == WorkOrderStatusConstants.Scheduled)
                    {
                        string Statusmsg = string.Empty;
                        woVM.woScheduleModel.WorkOrderId = Convert.ToInt64(workorderIds[i]);
                        var objWorkOrder = woWrapper.AddScheduleRecord(woVM.woScheduleModel);
                        if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                        {
                            string errormessage = "Failed to schedule " + objWorkOrder.ClientLookupId + ": " + objWorkOrder.ErrorMessages[0];
                            errorMessage.Add(errormessage);
                        }
                    }
                    else
                    {
                        failedWoList.Append(clientLookupIds[i] + ",");
                    }
                }
                if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    if (!string.IsNullOrEmpty(failedWoList.ToString()))
                    {
                        errorMessage.Add("Work Order(s) " + failedWoList + " can't be scheduled. Please check the status.");
                    }
                    return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion


        #region Intruction Detail
        [HttpPost]
        public JsonResult LoadIntructions(long WorkOrderId)
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            InstructionModel objInstruction = new InstructionModel();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var instructions = coWrapper.PopulateInstructions(WorkOrderId, AttachmentTableConstant.WorkOrder);
            objWorkOrderVM.InstructionModel = instructions.FirstOrDefault();
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return Json(new { data = objWorkOrderVM.InstructionModel }, JsonRequestBehavior.AllowGet);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddIntructionDetail(WorkOrderVM workOrderVM)
        {
            CommonWrapper coWrapper = new CommonWrapper(userData);

            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateInstruction(workOrderVM.InstructionModel, ref Mode, AttachmentTableConstant.WorkOrder);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderId = workOrderVM.InstructionModel.ObjectId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


        #region SendForApproval
        [HttpPost]
        public JsonResult WoSendForApproval(WorkOrderVM woVM)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                var objWorkOrder = woWrapper.WoSendForApproval(woVM.woSendForApprovalModel);
                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), workorderid = woVM.woSendForApprovalModel.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region WorkRequest set Planner
        [HttpPost]
        public JsonResult PlanningWoList(WOPlannerModel model)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM(); WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);

            List<string> errorMessage = new List<string>();

            StringBuilder failedWoList = new StringBuilder();

            foreach (var item in model.list)
            {
                if (item.Status != WorkOrderStatusConstants.WorkRequest)
                {
                    failedWoList.Append(item.ClientLookupId + ",");
                }
                else
                {
                    string Statusmsg = string.Empty;
                    WorkOrder Wo = woWrapper.PlanningWO(item.WorkOrderId, model.Planner_PersonnelId, ref Statusmsg);
                    if (Wo.ErrorMessages != null && Wo.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to Planning " + Wo.ClientLookupId + ": " + Wo.ErrorMessages[0];
                        errorMessage.Add(errormessage);
                    }
                }
            }
            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errorMessage.Add("Work Order(s) " + failedWoList + " can't be Planned. Please check the status.");
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult PlanningWo(long WorkOrderId, long Planner_PersonnelId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM(); WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);

            List<string> errorMessage = new List<string>();

            StringBuilder failedWoList = new StringBuilder();

            string Statusmsg = string.Empty;
            WorkOrder Wo = woWrapper.PlanningWO(WorkOrderId, Planner_PersonnelId, ref Statusmsg);
            if (Wo.ErrorMessages != null && Wo.ErrorMessages.Count > 0)
            {
                string errormessage = "Failed to Planning " + Wo.ClientLookupId + ": " + Wo.ErrorMessages[0];
                errorMessage.Add(errormessage);
            }
            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errorMessage.Add("Work Order(s) " + failedWoList + " can't be Planned. Please check the status.");
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult PopulatePersonDropdown()
        {
            List<SelectListItem> personItems = new List<SelectListItem>();
            var PersonnelLookUplist = GetList_PalnnerPersonnel();
            if (PersonnelLookUplist != null)
            {
                personItems = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.PersonnelId.ToString() }).ToList();
            }
            return Json(new { plannerList = personItems }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region V2-611 Add Workorder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddWorkOrdersDynamic(WorkOrderVM workOrderVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                WorkOrder objWorkOrder = new DataContracts.WorkOrder();

                if (workOrderVM.AddWorkorder.WorkOrderId == 0)
                {
                    Mode = "add";
                    objWorkOrder = woWrapper.addWorkOrderDynamic(workOrderVM);
                }
                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = Command, WorkOrderMasterId = objWorkOrder.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region V2-611 Add WorkRequest
        public ActionResult AddWoRequestDynamic()
        {
            TempData["Mode"] = "AddWoRequestDynamic";
            return Redirect("/WorkOrder/Index?page=Maintenance_Work_Order_Search");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveWoRequestDynamic(WorkOrderVM workOrderVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsg = new List<string>();
                WorkOrderVM objVM = new WorkOrderVM();
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                workOrderVM.IsDepartmentShow = true;
                workOrderVM.IsTypeShow = true;
                workOrderVM.IsDescriptionShow = true;
                workOrderVM.ChargeType = ChargeType.Equipment;
                WorkOrder returnObj = woWrapper.AddWorkRequestDynamic(workOrderVM, ref ErrorMsg);
                if (ErrorMsg != null && ErrorMsg.Count > 0)
                {
                    return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = Command, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
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

        #region Edit Work Order Dynamic
        public PartialViewResult EditWorkOrderDynamic(long workOrderID, string ClientLookupId, string Status, string Description, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation = "", string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {

            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();

            objWorksOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.EditWorkOrder, userData);

            Task t1 = Task.Factory.StartNew(() => objWorksOrderVM.EditWorkOrder = woWrapper.getEditWorkorderDetailsDynamic(workOrderID));
            Task t2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            Task.WaitAll(t1, t2);
            IList<string> LookupNames = objWorksOrderVM.UIConfigurationDetails.ToList()
                                       .Where(x => x.LookupType == DataDictionaryLookupTypeConstant.LookupList && !string.IsNullOrEmpty(x.LookupName))
                                       .Select(s => s.LookupName)
                                       .ToList();

            if (LookupNames.Contains(LookupListConstants.WO_Priority))
            {
                objWorksOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority)
                                                    .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                     .Select(s => new WOAddUILookupList
                                                     { text = s.Description, value = s.ListValue, lookupname = s.ListName })
                                                     .ToList());
                LookupNames.Remove(LookupListConstants.WO_Priority);
            }

            objWorksOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                 .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                      .Select(s => new WOAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList());
            objWorksOrderVM.IsPreventiveMaint = false;
            if (objWorksOrderVM.EditWorkOrder.SourceType == WorkOrderSourceTypes.PreventiveMaint)
            {
                objWorksOrderVM.IsPreventiveMaint = true;
            }

            objWorksOrderVM.IsUnplannedWorOrder = false; //V2-1052
            objWorksOrderVM.IsWorkOrderRequest = false;
            if (objWorksOrderVM.EditWorkOrder.SourceType == WorkOrderSourceTypes.Emergency)
            {
                objWorksOrderVM.IsUnplannedWorOrder = true;
                objWorksOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE)
                                                    .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                     .Select(s => new WOAddUILookupList
                                                     { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                               .ToList());
            }
            else if (objWorksOrderVM.EditWorkOrder.SourceType == WorkOrderSourceTypes.OnDemand)
            {
                objWorksOrderVM.IsUnplannedWorOrder = true;
                objWorksOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE)
                                               .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                               .Select(s => new WOAddUILookupList
                                               { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                               .ToList());
            }
            else if (objWorksOrderVM.EditWorkOrder.SourceType == WorkOrderSourceTypes.WorkRequest)
            {
                objWorksOrderVM.IsWorkOrderRequest = true;
                objWorksOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE)
                                               .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                               .Select(s => new WOAddUILookupList
                                               { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                               .ToList());
            }
            var SourceTypeList = UtilityFunction.PopulateSourceTypeList();
            if (SourceTypeList != null)
            {
                objWorksOrderVM.SourceTypeList = SourceTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var StatusList = commonWrapper.GetListFromConstVals(LookupListConstants.WO_Status);
            if (StatusList != null)
            {
                objWorksOrderVM.StatusList = StatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            objWorksOrderVM.PlantLocation = userData.Site.PlantLocation;

            var totalList = woWrapper.WOSchedulePersonnelList();
            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorksOrderVM.woScheduleModel = woScheduleModel;
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderID, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorksOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorksOrderVM, objWorksOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorksOrderVM.security = this.userData.Security;
            objWorksOrderVM._userdata = this.userData;
            objWorksOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorksOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorksOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            #region//*****V2-847

            objWorksOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;

            objWorksOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;

            #endregion//*****
            //V2-463
            objWorksOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorksOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_WorkorderEditDynamic.cshtml", objWorksOrderVM);
        }

        private WorkOrderSummaryModel GetWorkOrderSummaryModelDynamic(WorkOrderVM objVm, string ViewType = "EditWorkOrder")
        {
            WorkOrderSummaryModel workOrderSummaryModel = new WorkOrderSummaryModel();
            CommonWrapper comWrapper = new CommonWrapper(userData);
            if (ViewType == "EditWorkOrder")
            {
                workOrderSummaryModel.WorkOrderId = objVm.EditWorkOrder.WorkOrderId;
                workOrderSummaryModel.WorkOrder_ClientLookupId = objVm.EditWorkOrder.ClientLookupId;
                workOrderSummaryModel.Status = objVm.EditWorkOrder.Status;
                workOrderSummaryModel.Description = objVm.EditWorkOrder.Description;
                workOrderSummaryModel.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
                workOrderSummaryModel.ImageUrl = WorkOrderImageUrl(objVm.EditWorkOrder.WorkOrderId);
                if (objVm.EditWorkOrder != null)
                {
                    workOrderSummaryModel.PartsonOrder = Convert.ToInt32(objVm.EditWorkOrder.PartsOnOrder);
                }
                workOrderSummaryModel.Type = objVm.EditWorkOrder.Type;
                workOrderSummaryModel.Priority = objVm.EditWorkOrder.Priority;
                workOrderSummaryModel.ChargeToClientLookupId = objVm.EditWorkOrder.ChargeToClientLookupId;
                workOrderSummaryModel.ChargeTo_Name = objVm.EditWorkOrder.ChargeTo_Name;
                workOrderSummaryModel.ScheduledStartDate = objVm.EditWorkOrder.ScheduledStartDate;
                workOrderSummaryModel.ScheduledDuration = objVm.EditWorkOrder.ScheduledDuration;
                workOrderSummaryModel.CompleteDate = objVm.EditWorkOrder.CompleteDate;
                workOrderSummaryModel.security = this.userData.Security;
            }
            return workOrderSummaryModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditWorkOrderDynamic(WorkOrderVM workOrderVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                WorkOrder objWorkOrder = new WorkOrder();

                if (Command == "save")
                {
                    objWorkOrder = woWrapper.editWorkOrderDynamic(workOrderVM);
                }

                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = Command, WorkOrderMasterId = objWorkOrder.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region get part id from QR scanner
        [HttpGet]
        public JsonResult GetPartIdByClientLookUpId(string clientLookUpId)
        {
            var commonWrapper = new CommonWrapper(userData);
            var part = commonWrapper.RetrievePartIdByClientLookUp(clientLookUpId);
            return Json(new { PartId = part.PartId, MultiStoreroomError = false }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region add Actual Part
        [HttpPost]
        public PartialViewResult AddActualPartIssue()
        {
            var workOrderVM = new WorkOrderVM();
            var commonWrapper = new CommonWrapper(userData);
            var actualPart = new ActualPart();

            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                actualPart.UseMultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
                workOrderVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            workOrderVM.PartIssueAddModel = actualPart;

            LocalizeControls(workOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddActualPart", workOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ActualPartIssue(WorkOrderVM workorderVM)
        {
            InventoryCheckoutWrapper invWrapper = new InventoryCheckoutWrapper(userData);
            var partList = invWrapper.populatePartDetails(workorderVM.PartIssueAddModel.PartId, workorderVM.PartIssueAddModel.StoreroomId ?? 0);
            if (ModelState.IsValid)
            {
                WorkOrderWrapper workorderWrapper = new WorkOrderWrapper(userData);

                var result = workorderWrapper.PartIssueAddData(workorderVM.PartIssueAddModel, partList.ClientLookupId, partList.Description, partList.UPCCode);
                if (result.Count > 0)
                {
                    return Json(workorderVM.ErrorMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
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

        #region get Equipment id from QR scanner v2-625
        [HttpGet]
        public JsonResult GetEquipmentIdByClientLookUpId(string clientLookUpId)
        {
            WorkOrderWrapper objwoWrapper = new WorkOrderWrapper(userData);
            Equipment equipment = objwoWrapper.GetEquipmentIdByClientLookUpId(clientLookUpId);
            return Json(new { EquipmentId = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ReturnActualPart V2-624
        public JsonResult ReturnActualPartSelectedList(WorkOrderVM model)
        {
            InventoryCheckoutWrapper invWrapper = new InventoryCheckoutWrapper(userData);

            WorkOrderWrapper workorderWrapper = new WorkOrderWrapper(userData);
            List<string> errorMessage = new List<string>();
            System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();
            foreach (var item in model.WoPart)
            {
                model.PartIssueAddModel.PartId = item.PartId;
                model.PartIssueAddModel.Quantity = item.TransactionQuantity;
                model.PartIssueAddModel.Part_ClientLookupId = item.PartClientLookupId;
                model.PartIssueAddModel.Description = item.Description;
                model.PartIssueAddModel.UPCCode = item.UPCCode;
                model.PartIssueAddModel.StoreroomId = item.StoreroomId; //V2-687
                var result = workorderWrapper.ReturnPartData(model.PartIssueAddModel);
                if (result.Count > 0)
                {
                    string errormessage = ErrorMessageConstants.Failed_to_return_part;
                    errorMessage.Add(errormessage);
                    return Json(model.ErrorMessage, JsonRequestBehavior.AllowGet);
                }
            }
            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errorMessage.Add(ErrorMessageConstants.Failed_to_return_part);
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Add Project from Wo Details V2-626
        [HttpPost]
        public JsonResult AddProjecttoWorkorder(long WorkOrderId, long ProjectId, string ProjectClientlookupId = "")
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrder objWorkOrder = new WorkOrder();
            objWorkOrder = woWrapper.AddProjectToWorkorder(WorkOrderId, ProjectId);
            if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
            {
                return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderMasterId = objWorkOrder.WorkOrderId, ProjectId = ProjectId, ProjClientlookupId = ProjectClientlookupId }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Work order completion wizard
        [HttpPost]

        public PartialViewResult CompleteWorkOrderFromWizard(List<WoCancelAndPrintListModel> WorkOrderIds)
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            WoCompletionSettingsModel completionSettingsModel = new WoCompletionSettingsModel();
            ClientSetUpWrapper clientSetUpWrapper = new ClientSetUpWrapper(userData);
            RetrieveDataForUIConfiguration UIConfiguration = new RetrieveDataForUIConfiguration();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            Task[] tasks = new Task[2];

            completionSettingsModel = clientSetUpWrapper.CompletionSettingsDetails();

            objWorkOrderVM.UseWOCompletionWizard = completionSettingsModel.UseWOCompletionWizard;
            objWorkOrderVM.WOCommentTab = completionSettingsModel.WOCommentTab;
            objWorkOrderVM.TimecardTab = completionSettingsModel.TimecardTab;
            objWorkOrderVM.AutoAddTimecard = completionSettingsModel.AutoAddTimecard;

            if (objWorkOrderVM.UseWOCompletionWizard == true)
            {
                #region V2-728
                objWorkOrderVM.WOCompletionCriteriaTab = completionSettingsModel.WOCompCriteriaTab;
                objWorkOrderVM.WOCompletionCriteriaTitle = completionSettingsModel.WOCompCriteriaTitle;
                objWorkOrderVM.WOCompletionCriteria = completionSettingsModel.WOCompCriteria;
                #endregion

                objWorkOrderVM.UIConfigurationDetails = UIConfiguration.Retrieve
                                                        (DataDictionaryViewNameConstant.WorkOrderCompletion, userData);
                AllLookUps = commonWrapper.GetAllLookUpList();

                objWorkOrderVM.CompletionModelDynamic = new WorkOrderCompletionInformationModelDynamic();
                objWorkOrderVM.WorkOrderCompletionWizard = new WorkOrderCompletionWizard();

                #region V2-753
                if (WorkOrderIds.Count == 1)
                {
                    Task taskWODetails;
                    WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                    var WorkOrderId = WorkOrderIds[0].WorkOrderId;
                    taskWODetails = Task.Factory.StartNew(() => objWorkOrderVM.CompletionModelDynamic = woWrapper.RetrieveWorkOrderCompletionInformationByWorkOrderId(WorkOrderId));
                    Task.WaitAll(taskWODetails);
                }
                #endregion

                objWorkOrderVM.WorkOrderCompletionWizard.WorkOrderIds = WorkOrderIds;

                IList<string> LookupNames = objWorkOrderVM.UIConfigurationDetails.ToList()
                                       .Where(x => x.LookupType == DataDictionaryLookupTypeConstant.LookupList && !string.IsNullOrEmpty(x.LookupName))
                                       .Select(s => s.LookupName)
                                       .ToList();

                objWorkOrderVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                      .Select(s => new WOAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();

            }
            objWorkOrderVM._userdata = userData;
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/CompletionWizard/_WorkOrderCompletionWizard.cshtml", objWorkOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CompleteWorkOrderBatchFromWizard(WorkOrderVM model)
        {
            if (model.WorkOrderCompletionWizard != null && !string.IsNullOrEmpty(model.WorkOrderCompletionWizard.WOLaborsString))
            {
                model.WorkOrderCompletionWizard.WOLabors = JsonConvert.DeserializeObject<List<CompletionLaborWizard>>
                                                            (model.WorkOrderCompletionWizard.WOLaborsString);
            }
            if (!ModelState.IsValid)
            {
                return Json(UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global), JsonRequestBehavior.AllowGet);
            }
            List<string> errMsgList = new List<string>();
            WorkOrder objWorkOrder = new WorkOrder();
            WorkOrderWrapper objWrapper = new WorkOrderWrapper(userData);

            StringBuilder failedWoList = new StringBuilder();
            foreach (var item in model.WorkOrderCompletionWizard.WorkOrderIds)
            {
                if (item.Status == WorkOrderStatusConstants.Approved || item.Status == WorkOrderStatusConstants.Scheduled)
                {
                    objWorkOrder = objWrapper.CompleteWOFromWizard(model.CompletionModelDynamic, item.WorkOrderId, model.WorkOrderCompletionWizard,
                        model.TimecardTab, model.AutoAddTimecard, model.WOCompletionCriteriaTab);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to complete " + objWorkOrder.ClientLookupId + ": " + objWorkOrder.ErrorMessages[0];
                        errMsgList.Add(errormessage);
                    }
                }
                else
                {
                    failedWoList.Append(item.ClientLookupId + ",");

                }
            }
            if (errMsgList.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errMsgList.Add("Work Order(s) " + failedWoList + " can't be completed. Please check the status.");
                }
                return Json(new { data = errMsgList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Work order completion Labor tab
        [HttpPost]
        public PartialViewResult AddLaborFromCompletionWizard()
        {
            var PersonnelLookUplist = GetList_PersonnelV2();
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                CompletionLaborWizard = new CompletionLaborWizard
                {
                    WorkAssignedLookUpList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null,
                }
            };
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/CompletionWizard/_AddEditLaborWizard.cshtml", objWorksOrderVM);
        }
        [HttpPost]
        public PartialViewResult EditLaborFromCompletionWizard(long PersonnelID, decimal Hours, DateTime? StartDate)
        {
            var PersonnelLookUplist = GetList_PersonnelV2();
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                CompletionLaborWizard = new CompletionLaborWizard
                {
                    PersonnelID = PersonnelID,
                    Hours = Hours,
                    StartDate = StartDate,
                    WorkAssignedLookUpList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null,
                }
            };
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/CompletionWizard/_AddEditLaborWizard.cshtml", objWorksOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RetrieveCompletionLaborDetails(WorkOrderVM woVM)
        {
            if (ModelState.IsValid)
            {
                PersonnelWrapper personnelWrapper = new PersonnelWrapper(userData);
                Personnel personnel = personnelWrapper.RetrieveForWorkOrderCompletionWizard
                                (woVM.CompletionLaborWizard.PersonnelID ?? 0, woVM.CompletionLaborWizard.Hours);
                return Json(personnel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region Commom
        public string WorkOrderImageUrl(long WorkOrderId)
        {
            string ImageUrl = string.Empty;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (ClientOnPremise)
            {
                ImageUrl = objCommonWrapper.GetOnPremiseImageUrl(WorkOrderId, AttachmentTableConstant.WorkOrder);
            }
            else
            {
                ImageUrl = objCommonWrapper.GetAzureImageUrl(WorkOrderId, AttachmentTableConstant.WorkOrder);
            }
            return ImageUrl;

        }
        #endregion

        #region Material Request V2-690
        public PartialViewResult AddPartInInventory(long WorkOrderID, string ClientLookupId, long vendorId = 0, long StoreroomId = 0)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            partLookupVM.WorkOrderID = WorkOrderID;
            partLookupVM.ClientLookupId = ClientLookupId;
            partLookupVM.VendorId = vendorId;
            partLookupVM.ShoppingCart = userData.Site.ShoppingCart;
            partLookupVM.StoreroomId = StoreroomId;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/views/partlookup/indexWO.cshtml", partLookupVM);
        }

        [HttpGet]
        public PartialViewResult AddEstimatesPartNotInInventory(long WorkOrderId, string ClientLookupId, string Status, string Description, string Type, string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned, string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId, string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            WoScheduleModel woScheduleModel = new WoScheduleModel();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                estimatePart = new EstimatePart
                {
                    WorkOrderId = WorkOrderId,
                    MainClientLookupId = ClientLookupId,
                    ClientLookupId = ClientLookupId,
                    ShoppingCart = userData.Site.ShoppingCart//V2-1068

                }
            };
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            if (UNIT_OF_MEASURE != null)
            {
                objWorkOrderVM.UnitOfmesureListWo = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            // Check if the user has a shopping cart and the estimate part category is not specified
            if (userData.Site.ShoppingCart && objWorkOrderVM.estimatePart.CategoryId == 0)
            {
                // Add your code here to handle the condition
                objWorkOrderVM.estimatePart.IsAccountClientLookupIdReq = true;
                objWorkOrderVM.estimatePart.IsPartCategoryClientLookupIdReq = true;
            }
            var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelReason != null)
            {
                objWorkOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var totalList = woWrapper.WOSchedulePersonnelList();

            woScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            objWorkOrderVM.woScheduleModel = woScheduleModel;
            objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderId, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorkOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorkOrderVM, objWorkOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorkOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorkOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorkOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorkOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            //V2-463
            objWorkOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorkOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddEstimatesPartNotInInventory.cshtml", objWorkOrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEstimatesPartNotInInventory(WorkOrderVM workOrderVM)
        {
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                EstimatedCosts objEstimatedCost = new EstimatedCosts();
                if (workOrderVM.estimatePart.EstimatedCostsId != 0)
                {
                    objEstimatedCost = woWrapper.EditEstimatePart(workOrderVM);
                }
                else
                {
                    Mode = "add";
                    objEstimatedCost = woWrapper.AddEstimatePartNotInInventory(workOrderVM);
                }
                if (objEstimatedCost.ErrorMessages != null && objEstimatedCost.ErrorMessages.Count > 0)
                {
                    return Json(objEstimatedCost.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workorderid = workOrderVM.estimatePart.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
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

        #region EditPartInInventory
        public PartialViewResult EditPartInInventory(long EstimatedCostsId, long WorkOrderId)
        {
            WorkOrderVM objMaterialRequestVM = new WorkOrderVM();
            WorkOrderWrapper mrWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objMaterialRequestVM.security = this.userData.Security;

            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            if (UNIT_OF_MEASURE != null)
            {
                objMaterialRequestVM.UnitOfmesureListWo = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }

            var childItem = mrWrapper.GetLineItem(EstimatedCostsId, WorkOrderId);

            objMaterialRequestVM.PartNotInInventoryModel = childItem;
            objMaterialRequestVM.PartNotInInventoryModel.ShoppingCart = userData.Site.ShoppingCart;
            // Add a comment explaining the purpose of the if statement
            if (userData.Site.ShoppingCart && objMaterialRequestVM.PartNotInInventoryModel.CategoryId == 0)
            {
                objMaterialRequestVM.PartNotInInventoryModel.IsAccountClientLookupIdReq = true;
                objMaterialRequestVM.PartNotInInventoryModel.IsPartCategoryClientLookupIdReq = true;
            }
            LocalizeControls(objMaterialRequestVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_EditPartInInventory.cshtml", objMaterialRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartInInventory(WorkOrderVM PurchaseRequestVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            WorkOrderWrapper pWrapper = new WorkOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.EditPartInInventory(PurchaseRequestVM.PartNotInInventoryModel);
                if (lineItem.ErrorMessages != null && lineItem.ErrorMessages.Count > 0)
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderId = PurchaseRequestVM.PartNotInInventoryModel.ObjectId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region V2-695 Downtime WO
        [HttpPost]
        public string GetWorkOrder_Downtime(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            var Downtime = woWrapper.GetWorkOrderDowntime(workOrderId, skip, length ?? 0, order, orderDir);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            if (Downtime != null && Downtime.Count > 0)
            {
                recordsFiltered = Downtime[0].TotalCount;
                totalRecords = Downtime[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }

            int initialPage = start.Value;
            var filteredResult = Downtime
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            //V2-695
            var secDownTimeAdd = this.userData.Security.WorkOrder_Downtime.Create;
            var secDownTimeEdit = this.userData.Security.WorkOrder_Downtime.Edit;
            var secDownTimeDelete = this.userData.Security.WorkOrder_Downtime.Delete;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, secDownTimeAdd = secDownTimeAdd, secDownTimeEdit = secDownTimeEdit, secDownTimeDelete = secDownTimeDelete }, JsonSerializerDateSettings);
        }

        [HttpPost]
        [ActionName("RedirectDowntime")]
        public ActionResult DownTimeAdd(long ChargeToId, long workOrderID, string ClientLookupId, string Status, string Description, string Type,
                                         string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned,
                                         string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId,
                                         string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderDowntimeModel objDownTimeModel = new WorkOrderDowntimeModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            #region summary
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderID, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorksOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorksOrderVM, objWorksOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorksOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorksOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorksOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorksOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;
            #region//*****V2-847
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****

            #endregion

            objDownTimeModel.WorkOrderId = workOrderID;
            objDownTimeModel.ChargeToId = ChargeToId;
            objDownTimeModel.Downdate = DateTime.Now;
            objWorksOrderVM.wodowntimeModel = objDownTimeModel;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var ReasonforDown = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_DOWN_REASON).ToList();
            if (ReasonforDown != null)
            {
                objWorksOrderVM.ReasonForDownList = ReasonforDown.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
            }
            objWorksOrderVM.workOrderSummaryModel = GetDowntimeforWorkOrderSummary(objWorksOrderVM.workOrderSummaryModel, Downdate, DownMinutes);
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("AddDowntime", objWorksOrderVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownTimeAdd(WorkOrderVM ec)
        {
            List<string> errorMessage = new List<string>();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            Downtime result = new Downtime();

            if (ModelState.IsValid)
            {
                result = woWrapper.AddDownTime(ec.wodowntimeModel);
                if (errorMessage != null && errorMessage.Count > 0)
                {
                    return Json(errorMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderid = ec.wodowntimeModel.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DownTimeDelete(long _DowntimeId)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            if (woWrapper.DeleteDowntime(_DowntimeId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ShowDownTimeEdit(long ChargeToId, long workOrderID, string ClientLookupId, string Status, string Description, string Type,
                                         string Priority, string ChargeTo, string ChargeToName, string ScheduledDate, string Assigned,
                                         string completedate, string ScheduledDuration, string AssignedFullName, long WorkAssigned_PersonnelId,
                                         string Downdate, string DownMinutes, string ProjectClientLookupId, string AssetLocation, long DowntimeId, string AssetGroup1Name = "", string AssetGroup2Name = "", string AssetGroup1ClientlookupId = "", string AssetGroup2ClientlookupId = "")
        {

            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderDowntimeModel objDownTimeModel = new WorkOrderDowntimeModel();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            objWorksOrderVM._userdata = userData;
            #region summary
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderID, ClientLookupId, Status, Description, ProjectClientLookupId);
            objWorksOrderVM.workOrderSummaryModel = GetAllWorkOrderSummaryModel(objWorksOrderVM, objWorksOrderVM.workOrderSummaryModel, Type, Priority, ChargeTo, ChargeToName, ScheduledDate, Assigned, completedate, AssetLocation);
            objWorksOrderVM.workOrderSummaryModel.ScheduledDuration = Convert.ToDecimal(ScheduledDuration);
            objWorksOrderVM.workOrderSummaryModel.AssignedFullName = AssignedFullName;
            objWorksOrderVM.workOrderSummaryModel.Assigned = Assigned;
            objWorksOrderVM.workOrderSummaryModel.WorkAssigned_PersonnelId = WorkAssigned_PersonnelId;

            #region//*****V2-847
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1Name = AssetGroup1Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2Name = AssetGroup2Name;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup1ClientlookupId = AssetGroup1ClientlookupId;
            objWorksOrderVM.workOrderSummaryModel.AssetGroup2ClientlookupId = AssetGroup2ClientlookupId;
            #endregion//*****

            #endregion

            //V2-695
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var ReasonforDown = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_DOWN_REASON).ToList();
            if (ReasonforDown != null)
            {
                objWorksOrderVM.ReasonForDownList = ReasonforDown.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
            }

            objDownTimeModel = woWrapper.RetrieveByDowntimeId(DowntimeId);
            objDownTimeModel.DowntimeId = DowntimeId;
            objDownTimeModel.WorkOrderId = workOrderID;
            objDownTimeModel.ChargeToId = ChargeToId;
            objWorksOrderVM.wodowntimeModel = objDownTimeModel;
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("EditDowntime", objWorksOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownTimeEdit(WorkOrderVM ec)
        {
            List<string> errorMessage = new List<string>();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            Downtime result = new Downtime();
            if (ModelState.IsValid)
            {
                result = woWrapper.EditDownTime(ec.wodowntimeModel);
                if (errorMessage != null && errorMessage.Count > 0)
                {
                    return Json(errorMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderid = result.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region V2-716 Multiple Images
        public PartialViewResult GetImages(int currentpage, int? start, int? length, long WorkOrderId)
        {
            WorkOrderVM workOrderVM = new WorkOrderVM();
            List<ImageAttachmentModel> imgDatalist = new List<ImageAttachmentModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Attachment> Attachments = new List<Attachment>();
            workOrderVM.security = this.userData.Security;
            workOrderVM._userdata = this.userData;
            start = start.HasValue
            ? start / length
            : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;

            if (userData.DatabaseKey.Client.OnPremise)
            {
                Attachments = commonWrapper.GetOnPremiseMultipleImageUrl(skip, length ?? 0, WorkOrderId, AttachmentTableConstant.WorkOrder);
            }
            else
            {
                Attachments = commonWrapper.GetAzureMultipleImageUrl(skip, length ?? 0, WorkOrderId, AttachmentTableConstant.WorkOrder);
            }
            foreach (var attachment in Attachments)
            {
                ImageAttachmentModel imgdata = new ImageAttachmentModel();
                imgdata.AttachmentId = attachment.AttachmentId;
                imgdata.ObjectId = attachment.ObjectId;
                imgdata.Profile = attachment.Profile;
                imgdata.FileName = attachment.FileName;
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(attachment.AttachmentURL))
                {
                    SASImageURL = attachment.AttachmentURL;
                }
                else
                {
                    commonWrapper.GetNoImageUrl();
                }
                imgdata.AttachmentURL = SASImageURL;
                imgdata.ObjectName = attachment.ObjectName;
                imgdata.TotalCount = attachment.TotalCount;
                imgDatalist.Add(imgdata);
            }
            var recordsFiltered = 0;
            recordsFiltered = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            workOrderVM.imageAttachmentModels = imgDatalist;
            LocalizeControls(workOrderVM, LocalizeResourceSetConstants.Global);

            return PartialView("~/Views/WorkOrder/_AllWorkOrderImages.cshtml", workOrderVM);
        }
        #endregion

        #region V2-726
        public PartialViewResult SendForApproval(long WorkOrderId)
        {
            WorkOrderVM objWOVM = new WorkOrderVM();

            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            var dataModels = Get_ApproverList(this.userData.DatabaseKey.Personnel.PersonnelId, ApprovalGroupRequestTypes.MaterialRequest, 1);
            var approvers = new List<SelectListItem>();
            if (dataModels.Count > 0)
            {
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.ApproverName,
                    Value = x.ApproverId.ToString()
                }).ToList();
            }
            else
            {
                var securityName = SecurityConstants.MaterialRequest_Approve;
                var ItemAccess = "ItemAccess";
                dataModels = Get_PersonnelList(securityName, ItemAccess);
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.NameFirst + " " + x.NameLast,
                    Value = x.AssignedTo_PersonnelId.ToString()
                }).ToList();
            }
            approvalRouteModel.ApproverList = approvers;
            approvalRouteModel.ApproverCount = approvers.Count;
            approvalRouteModel.ObjectId = WorkOrderId;

            objWOVM.ApprovalRouteModel = approvalRouteModel;
            ViewBag.IsMaterialRequestDetails = false;
            LocalizeControls(objWOVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_SendItemsForApproval", objWOVM);
        }
        [HttpPost]
        public JsonResult SendForApproval(WorkOrderVM woVM)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                string MaterialRequest = ApprovalGroupRequestTypes.MaterialRequest;
                long ApprovalGroupId = RetrieveApprovalGroupIdByRequestorIdAndRequestType(MaterialRequest);
                woVM.ApprovalRouteModel.ApprovalGroupId = ApprovalGroupId;
                woVM.ApprovalRouteModel.RequestType = MaterialRequest;
                if (ApprovalGroupId >= 0)
                {
                    var objWorkOrder = woWrapper.SendForApproval(woVM.ApprovalRouteModel);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderId = woVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderId = woVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ApprovalRouteModel SendWRForApproval(long WorkOrderId)
        {
            WorkOrderVM objWOVM = new WorkOrderVM();

            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            var dataModels = Get_ApproverList(this.userData.DatabaseKey.Personnel.PersonnelId, ApprovalGroupRequestTypes.WorkRequest, 1);
            var approvers = new List<SelectListItem>();
            if (dataModels.Count > 0)
            {
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.ApproverName,
                    Value = x.ApproverId.ToString()
                }).ToList();
            }
            else
            {
                var securityName = SecurityConstants.WorkOrder_Approve;
                var ItemAccess = "ItemAccess";
                dataModels = Get_PersonnelList(securityName, ItemAccess);
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.NameFirst + " " + x.NameLast,
                    Value = x.AssignedTo_PersonnelId.ToString()
                }).ToList();
            }
            approvalRouteModel.ApproverList = approvers;
            approvalRouteModel.ApproverCount = approvers.Count;
            approvalRouteModel.ObjectId = WorkOrderId;

            objWOVM.ApprovalRouteModel = approvalRouteModel;
            LocalizeControls(objWOVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return approvalRouteModel;
        }
        [HttpPost]
        public JsonResult SendWRForApprovalSave(WorkOrderVM woVM)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                string WorkRequest = ApprovalGroupRequestTypes.WorkRequest;
                long ApprovalGroupId = RetrieveApprovalGroupIdByRequestorIdAndRequestType(WorkRequest);
                woVM.ApprovalRouteModel.ApprovalGroupId = ApprovalGroupId;
                woVM.ApprovalRouteModel.RequestType = WorkRequest;
                if (ApprovalGroupId >= 0)
                {
                    var objWorkOrder = woWrapper.ApproveWR(woVM.ApprovalRouteModel);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderId = woVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderId = woVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region V2-732
        public PartialViewResult PopulateStorerooms()
        {
            WorkOrderVM wrVM = new WorkOrderVM();
            var commonWrapper = new CommonWrapper(userData);
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                wrVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            LocalizeControls(wrVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_StoreroomList", wrVM);
        }
        [HttpPost]
        public JsonResult SelectStoreroom(WorkOrderVM wrVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-730
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
            WorkOrderVM objWRVM = new WorkOrderVM();

            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();

            approvalRouteModel.ApproverList = Approvers;
            approvalRouteModel.ApproverCount = Approvers.Count;
            approvalRouteModel.ObjectId = WorkOrderId;
            approvalRouteModel.ApprovalGroupId = ApprovalGroupId;

            objWRVM.ApprovalRouteModel = approvalRouteModel;
            LocalizeControls(objWRVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_SendWorkOrderForMultiLevelApproval", objWRVM);
        }
        [HttpPost]
        public JsonResult SendWRForMultiLevelApprovalSave(WorkOrderVM prVM)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                string WorkRequest = ApprovalGroupRequestTypes.WorkRequest;
                prVM.ApprovalRouteModel.RequestType = WorkRequest;
                var objWorkOrder = woWrapper.MultiLevelApproveWR(prVM.ApprovalRouteModel);
                if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                {
                    return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderId = prVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = prVM.ApprovalRouteModel.ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult MultiLevelFinalApprove(long WorkOrderId, long ApprovalGroupId)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                var objWorkOrder = woWrapper.MultiLevelFinalApproveWR(WorkOrderId, ApprovalGroupId);
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
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrder Wojob = woWrapper.MultiLevelDenyWO(WorkorderId, ApprovalGroupId, ref Statusmsg);
            return Json(new { data = Statusmsg, WoId = WorkorderId, error = Wojob.ErrorMessages }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region V2-944
        private void BindWorkOrderUDFDetails(WorkOrderUDF WorkOrderUDFDetails, ref WorkOrderDevExpressPrintModel workOrderDevExpressPrintModel)
        {
            workOrderDevExpressPrintModel.WorkOrderUDF_WOId = WorkOrderUDFDetails?.WorkOrderId ?? 0;
            if (WorkOrderUDFDetails != null)
            {
                workOrderDevExpressPrintModel.Text1 = WorkOrderUDFDetails.Text1;
                workOrderDevExpressPrintModel.Text2 = WorkOrderUDFDetails.Text2;
                workOrderDevExpressPrintModel.Text3 = WorkOrderUDFDetails.Text3;
                workOrderDevExpressPrintModel.Text4 = WorkOrderUDFDetails.Text4;
                if (WorkOrderUDFDetails.Date1 == null || WorkOrderUDFDetails.Date1 == default(DateTime))
                {
                    workOrderDevExpressPrintModel.Date1 = "";
                }
                else
                {
                    workOrderDevExpressPrintModel.Date1 = Convert.ToDateTime(WorkOrderUDFDetails.Date1)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (WorkOrderUDFDetails.Date2 == null || WorkOrderUDFDetails.Date2 == default(DateTime))
                {
                    workOrderDevExpressPrintModel.Date2 = "";
                }
                else
                {
                    workOrderDevExpressPrintModel.Date2 = Convert.ToDateTime(WorkOrderUDFDetails.Date2)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (WorkOrderUDFDetails.Date3 == null || WorkOrderUDFDetails.Date3 == default(DateTime))
                {
                    workOrderDevExpressPrintModel.Date3 = "";
                }
                else
                {
                    workOrderDevExpressPrintModel.Date3 = Convert.ToDateTime(WorkOrderUDFDetails.Date3)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (WorkOrderUDFDetails.Date4 == null || WorkOrderUDFDetails.Date4 == default(DateTime))
                {
                    workOrderDevExpressPrintModel.Date4 = "";
                }
                else
                {
                    workOrderDevExpressPrintModel.Date4 = Convert.ToDateTime(WorkOrderUDFDetails.Date4)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                workOrderDevExpressPrintModel.Bit1 = WorkOrderUDFDetails.Bit1 ? "Yes" : "No";
                workOrderDevExpressPrintModel.Bit2 = WorkOrderUDFDetails.Bit2 ? "Yes" : "No";
                workOrderDevExpressPrintModel.Bit3 = WorkOrderUDFDetails.Bit3 ? "Yes" : "No";
                workOrderDevExpressPrintModel.Bit4 = WorkOrderUDFDetails.Bit4 ? "Yes" : "No";
                workOrderDevExpressPrintModel.Numeric1 = WorkOrderUDFDetails.Numeric1;
                workOrderDevExpressPrintModel.Numeric2 = WorkOrderUDFDetails.Numeric2;
                workOrderDevExpressPrintModel.Numeric3 = WorkOrderUDFDetails.Numeric3;
                workOrderDevExpressPrintModel.Numeric4 = WorkOrderUDFDetails.Numeric4;
                workOrderDevExpressPrintModel.Select1 = WorkOrderUDFDetails.Select1;
                workOrderDevExpressPrintModel.Select2 = WorkOrderUDFDetails.Select2;
                workOrderDevExpressPrintModel.Select3 = WorkOrderUDFDetails.Select3;
                workOrderDevExpressPrintModel.Select4 = WorkOrderUDFDetails.Select4;
                workOrderDevExpressPrintModel.Text1Label = WorkOrderUDFDetails.Text1Label;
                workOrderDevExpressPrintModel.Text2Label = WorkOrderUDFDetails.Text2Label;
                workOrderDevExpressPrintModel.Text3Label = WorkOrderUDFDetails.Text3Label;
                workOrderDevExpressPrintModel.Text4Label = WorkOrderUDFDetails.Text4Label;
                workOrderDevExpressPrintModel.Date1Label = WorkOrderUDFDetails.Date1Label;
                workOrderDevExpressPrintModel.Date2Label = WorkOrderUDFDetails.Date2Label;
                workOrderDevExpressPrintModel.Date3Label = WorkOrderUDFDetails.Date3Label;
                workOrderDevExpressPrintModel.Date4Label = WorkOrderUDFDetails.Date4Label;
                workOrderDevExpressPrintModel.Bit1Label = WorkOrderUDFDetails.Bit1Label;
                workOrderDevExpressPrintModel.Bit2Label = WorkOrderUDFDetails.Bit2Label;
                workOrderDevExpressPrintModel.Bit3Label = WorkOrderUDFDetails.Bit3Label;
                workOrderDevExpressPrintModel.Bit4Label = WorkOrderUDFDetails.Bit4Label;
                workOrderDevExpressPrintModel.Numeric1Label = WorkOrderUDFDetails.Numeric1Label;
                workOrderDevExpressPrintModel.Numeric2Label = WorkOrderUDFDetails.Numeric2Label;
                workOrderDevExpressPrintModel.Numeric3Label = WorkOrderUDFDetails.Numeric3Label;
                workOrderDevExpressPrintModel.Numeric4Label = WorkOrderUDFDetails.Numeric4Label;
                workOrderDevExpressPrintModel.Select1Label = WorkOrderUDFDetails.Select1Label;
                workOrderDevExpressPrintModel.Select2Label = WorkOrderUDFDetails.Select2Label;
                workOrderDevExpressPrintModel.Select3Label = WorkOrderUDFDetails.Select3Label;
                workOrderDevExpressPrintModel.Select4Label = WorkOrderUDFDetails.Select4Label;
            }
        }
        private void BindWorkOrderScheduleTable(List<WorkOrderSchedule> listOfWorkOrderSchedules, ref WorkOrderDevExpressPrintModel workOrderDevExpressPrintModel)
        {
            if (listOfWorkOrderSchedules.Count > 0)
            {
                foreach (var item in listOfWorkOrderSchedules)
                {
                    var objWOScheduleDevExpressPrintModel = new WOScheduleDevExpressPrintModel();

                    objWOScheduleDevExpressPrintModel.Assigned = item.NameFirst + " " + item.NameLast;
                    if (item.ScheduledStartDate == null || item.ScheduledStartDate == default(DateTime))
                    {
                        objWOScheduleDevExpressPrintModel.ScheduledStartDate = "";
                    }
                    else
                    {
                        objWOScheduleDevExpressPrintModel.ScheduledStartDate = Convert.ToDateTime(item.ScheduledStartDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    objWOScheduleDevExpressPrintModel.ScheduledHours = item.ScheduledHours;
                    objWOScheduleDevExpressPrintModel.spnGlobalAssigned = UtilityFunction.GetMessageFromResource("spnGlobalAssigned", LocalizeResourceSetConstants.Global);
                    objWOScheduleDevExpressPrintModel.spnDate = UtilityFunction.GetMessageFromResource("spnDate", LocalizeResourceSetConstants.Global);
                    objWOScheduleDevExpressPrintModel.spnDuration = UtilityFunction.GetMessageFromResource("spnDuration", LocalizeResourceSetConstants.Global);
                    workOrderDevExpressPrintModel.WOScheduleDevExpressPrintModelList.Add(objWOScheduleDevExpressPrintModel);
                }

            }
        }
        private string GenerateImageUrlDevExpress()
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            var ImagePath = comWrapper.GetClientLogoUrlForDevExpressPrint();
            if (string.IsNullOrEmpty(ImagePath))
            {
                var path = "~/Scripts/ImageZoom/images/NoImage.jpg";
                ImagePath = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Content(path);
            }
            else if (ImagePath.StartsWith("../"))
            {
                ImagePath = ImagePath.Replace("../", Request.Url.Scheme + "://" + Request.Url.Authority + "/");
            }
            return ImagePath;
        }
        #endregion

        #region V2-1056 Add Sanitation Request
        public PartialViewResult AddSanitationRequestWO(long WorkoderId, string ClientLookupId)
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            objWorkOrderVM.addSanitationRequestModel = new AddSanitationRequestModel();

            objWorkOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(WorkoderId);
            objWorkOrderVM._userdata = this.userData;

            objWorkOrderVM.addSanitationRequestModel.WorkOrderId = objWorkOrderVM.workOrderModel.WorkOrderId;
            objWorkOrderVM.addSanitationRequestModel.ClientLookupId = ClientLookupId;
            objWorkOrderVM.addSanitationRequestModel.ChargeType = objWorkOrderVM.workOrderModel.ChargeType;
            objWorkOrderVM.addSanitationRequestModel.ChargeToClientLookupId = objWorkOrderVM.workOrderModel.ChargeToClientLookupId;
            objWorkOrderVM.addSanitationRequestModel.ChargeTo = objWorkOrderVM.workOrderModel.ChargeToId;
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddSanitationRequestWO.cshtml", objWorkOrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddSanitationRequestWorkOrder(WorkOrderVM objVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsg = new List<string>();
                WorkOrderVM workOrderVM = new WorkOrderVM();
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);

                var returnObj = woWrapper.AddSanitationRequestWorkOrder(objVM.addSanitationRequestModel, ref ErrorMsg);
                if (returnObj.ErrorMessages != null && returnObj.ErrorMessages.Count > 0)
                {
                    return Json(returnObj.ErrorMessages, JsonRequestBehavior.AllowGet);
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

        #region V2-1051
        [HttpPost]
        public ActionResult WOMOdel(long WorkOrderId)
        {
            WorkOrderWrapper WOrapper = new WorkOrderWrapper(userData);
            var objWorkOrder = WOrapper.CreateWorkOrderModel(WorkOrderId);
            if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
            {
                return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString(), CreatedWorkOrderId = objWorkOrder.CreatedWorkOrderId }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region V2-1067 Unplanned WO Dynamic

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveEmergencyDescribeDynamic(WorkOrderVM workOrderVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsgList = new List<string>();
                WorkOrderVM objVM = new WorkOrderVM();
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                objVM.woDescriptionModelDynamic = new WoDescriptionModelDynamic();

                workOrderVM.woDescriptionModelDynamic.IsDepartmentShow = true;
                workOrderVM.woDescriptionModelDynamic.IsTypeShow = true;
                workOrderVM.woDescriptionModelDynamic.IsDescriptionShow = true;
                WorkOrder returnObj = woWrapper.WO_DescribeDynamic(workOrderVM, ref ErrorMsgList);

                if (ErrorMsgList != null && ErrorMsgList.Count > 0)
                {
                    return Json(ErrorMsgList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool IsredirectToSaniPage = (userData.Site.ExternalSanitation == true && userData.Security.SanitationJob.CreateRequest == true) ? true : false;
                    return Json(new { data = JsonReturnEnum.success.ToString(), IsToSanitationPage = IsredirectToSaniPage, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        public PartialViewResult EmergencyDescribeDynamic(string ClientLookupId, long WorkoderId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            objWorksOrderVM.woEmergencyDescribeModel = new WoEmergencyDescribeModel();
            objWorksOrderVM.woEmergencyDescribeModel.WorkOrderId = WorkoderId;
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            objWorksOrderVM._userdata = this.userData;
            objWorksOrderVM.woEmergencyDescribeModel.ClientLookupId = ClientLookupId;
            objWorksOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                    .Retrieve(DataDictionaryViewNameConstant.WorkOrderDescribeAdd, userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            IList<string> LookupNames = objWorksOrderVM.UIConfigurationDetails.ToList()
                           .Where(x => x.LookupType == DataDictionaryLookupTypeConstant.LookupList && !string.IsNullOrEmpty(x.LookupName))
                           .Select(s => s.LookupName)
                           .ToList();
            if (LookupNames.Contains(LookupListConstants.WO_Priority))
            {
                objWorksOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority)
                                                     .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                     .Select(s => new WOAddUILookupList
                                                     { text = s.Description, value = s.ListValue, lookupname = s.ListName })
                                                     .ToList());
                LookupNames.Remove(LookupListConstants.WO_Priority);
            }
            objWorksOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                 .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                      .Select(s => new WOAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList());
            objWorksOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE)
                                               .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                               .Select(s => new WOAddUILookupList
                                               { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                               .ToList());

            objWorksOrderVM.PlantLocation = userData.Site.PlantLocation;
            objWorksOrderVM.IsAddWorkOrderDynamic = true;
            objWorksOrderVM._userdata = this.userData;
            objWorksOrderVM.security = this.userData.Security;
            objWorksOrderVM.IsMaintOnDemand = userData.Site.MaintOnDemand;
            objWorksOrderVM.woDescriptionModelDynamic = new WoDescriptionModelDynamic();


            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_EmergencyDescribeWoPopupDynamic.cshtml", objWorksOrderVM);
        }
        #endregion
    }
}

