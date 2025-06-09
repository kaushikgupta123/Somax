using AzureUtil;
using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Work_Order;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.Common;
using Client.Models.Work_Order;
using Client.Models.WorkOrder;
using Common.Constants;
using DataContracts;
using iTextSharp.text.pdf;
using Microsoft.WindowsAzure.Storage.Blob;
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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

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
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
                if (CancelReason != null)
                {
                    objWorkOrderVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                }
            }
            string mode = Convert.ToString(TempData["Mode"]);
            if (string.IsNullOrEmpty(mode))
            {
                var schduleWorkList = woWrapper.populateListDetails();
                if (schduleWorkList != null)
                {
                    woModel.ScheduleWorkList = schduleWorkList.Select(x => new SelectListItem { Text = x.Key, Value = x.Value });
                }
                objWorkOrderVM.workOrderModel = woModel;
            }
            else
            {
                var schduleWorkList = woWrapper.populateListDetails();
                if (schduleWorkList != null)
                {
                    woModel.ScheduleWorkList = schduleWorkList.Select(x => new SelectListItem { Text = x.Key, Value = x.Value });
                }

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
                var ChargeTypeList = UtilityFunction.populateChargeType();
                if (ChargeTypeList != null)
                {
                    RequestModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }

                List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
                RequestModel.ChargeToList = defaultChargeToList;
                RequestModel.PlantLocation = userData.Site.PlantLocation;
                objWorkOrderVM.woRequestModel = RequestModel;
                objWorkOrderVM.IsAddRequest = true;
                objWorkOrderVM.workOrderModel = woModel;
            }
            else if (mode == "DetailFromEquipment")
            {
                long workOrderId = Convert.ToInt64(TempData["workOrderId"]);
                objWorkOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(workOrderId);

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

                objWorkOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(objWorkOrderVM.workOrderModel.WorkOrderId, objWorkOrderVM.workOrderModel.ClientLookupId, objWorkOrderVM.workOrderModel.Status, objWorkOrderVM.workOrderModel.Description);
                objWorkOrderVM.workOrderModel.WorkOrderId = objWorkOrderVM.workOrderModel.WorkOrderId;

                #region Follow Up

                objWorkOrderVM.woRequestModel = new WoRequestModel();

                if (Type != null)
                {
                    objWorkOrderVM.woRequestModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
                }
                if (ScheduleChargeTypeList != null)
                {
                    objWorkOrderVM.woRequestModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).Where(x => x.Text != "Location");
                }
                List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
                objWorkOrderVM.woRequestModel.ChargeToList = defaultChargeToList;
                objWorkOrderVM.woRequestModel.WorkOrderId = objWorkOrderVM.workOrderModel.WorkOrderId;

                #endregion
                objWorkOrderVM.woRequestModel.PlantLocation = userData.Site.PlantLocation;
                objWorkOrderVM._userdata = this.userData;
                objWorkOrderVM.security = this.userData.Security;
                objWorkOrderVM.IsMaintOnDemand = userData.Site.MaintOnDemand;
                objWorkOrderVM.downtimeModel = new DowntimeModel();
                objWorkOrderVM.downtimeModel.WorkOrderId = workOrderId;
                ViewBag.SecurityEstimate = userData.Security.WorkOrders.Edit;
                objWorkOrderVM.IsAddWorkOrderFromEquipment = true;
            }
            else if (mode == "addWorkOrder")
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
                var PersonnelLookUplist = GetList_Personnel();
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

            objWorkOrderVM.security = this.userData.Security;
            objWorkOrderVM.workOrderModel.BusinessType = this.userData.DatabaseKey.Client.BusinessType.ToUpper();
            objWorkOrderVM.IsMaintOnDemand = userData.Site.MaintOnDemand;
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return View(objWorkOrderVM);
        }

        #region New code implementation for grid load and advance search
        public string GetWorkOrderMaintGrid(int? draw, int? start, int? length, int CustomQueryDisplayId = 0,
           string workorder = "", string description = "", string Chargeto = "", string Chargetoname = "", string type = "", string status = "",
           string shift = "", string priority = "", DateTime? Created = null, string creator = "", string assigned = "", DateTime? Scheduled = null, string failcode = "",
           DateTime? ActualFinish = null, string completioncomments = "")
        {

            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<string> statusList = new List<string>();
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");

            string _created = string.Empty;
            string _scheduled = string.Empty;
            string _actualFinish = string.Empty;

            _created = Created.HasValue ? Created.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _scheduled = Scheduled.HasValue ? Scheduled.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _actualFinish = ActualFinish.HasValue ? ActualFinish.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;

            List<WorkOrderModel> woMaintMasterList = woWrapper.populateWODetails(CustomQueryDisplayId, out Dictionary<string, Dictionary<string, string>> lookupLists,
                skip, length ?? 0, colname[0], orderDir, workorder, description, Chargeto, Chargetoname, type, status, shift, priority, _created,
                creator, assigned, _scheduled, failcode, _actualFinish, completioncomments);

            var totalRecords = 0;
            var recordsFiltered = 0;

            recordsFiltered = woMaintMasterList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = woMaintMasterList.Select(o => o.TotalCount).FirstOrDefault();

            var filteredResult = woMaintMasterList
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, lookupLists = lookupLists }, JsonSerializerDateSettings);
        }
        #endregion


        [HttpGet]
        public string GetWorkOrderPrintData(string _colname, string _coldir, int _CustomQueryDisplayId = 0, string _ClientLookupId = "", string _Description = "", string _ChargeToClientLookupId = "", string _ChargeToName = "",
                              string _Type = "", string _Status = "", string _Shift = null, string _Priority = "", DateTime? _Created = null, string _Creator = "", string _Assigned = "", DateTime? _Scheduled = null, string _Failcode = "", DateTime? _ActualFinish = null, string _Completioncomments = "")
        {
            List<WorkOrderPrintModel> WorkOrderPrintModelList = new List<WorkOrderPrintModel>();
            WorkOrderPrintModel objWorkOrderPrintModel;
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);

            var woList = woWrapper.populateWODetails(_CustomQueryDisplayId);
            woList = this.GetAllWorkOrderSortByColumnWithOrder(_colname, _coldir, woList);
            if (woList != null)
            {
                if (!string.IsNullOrEmpty(_ClientLookupId))
                {
                    _ClientLookupId = _ClientLookupId.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(_Description))
                {
                    _Description = _Description.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(_Description))).ToList();
                }
                if (!string.IsNullOrEmpty(_ChargeToClientLookupId))
                {
                    _ChargeToClientLookupId = _ChargeToClientLookupId.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeToClientLookupId) && x.ChargeToClientLookupId.ToUpper().Contains(_ChargeToClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(_ChargeToName))
                {
                    _ChargeToName = _ChargeToName.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_Name) && x.ChargeTo_Name.ToUpper().Contains(_ChargeToName))).ToList();
                }
                if (!string.IsNullOrEmpty(_Type) && _Type != "--Select--")
                {
                    _Type = _Type.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.Type) && x.Type.ToUpper().Contains(_Type))).ToList();
                }
                if (!string.IsNullOrEmpty(_Status))
                {
                    _Status = _Status.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.Status) && x.Status.ToUpper().Contains(_Status))).ToList();
                }
                if (!string.IsNullOrEmpty(_Shift) && _Shift != "--Select--")
                {
                    _Shift = _Shift.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.Shift) && x.Shift.ToUpper().Contains(_Shift))).ToList();
                }
                if (!string.IsNullOrEmpty(_Priority) && _Priority != "--Select--")
                {
                    _Priority = _Priority.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.Priority) && x.Priority.ToUpper().Contains(_Priority))).ToList();
                }
                if (_Created != null)
                {
                    woList = woList.Where(x => (x.CreateDate != null && x.CreateDate.Value.Date.Equals(_Created.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(_Creator))
                {
                    _Creator = _Creator.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.Creator) && x.Creator.ToUpper().Contains(_Creator))).ToList();
                }
                if (!string.IsNullOrEmpty(_Assigned))
                {
                    _Assigned = _Assigned.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.Assigned) && x.Assigned.ToUpper().Contains(_Assigned))).ToList();
                }
                if (_Scheduled != null)
                {
                    woList = woList.Where(x => (x.ScheduledStartDate != null && x.ScheduledStartDate.Value.Date.Equals(_Scheduled.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(_Failcode))
                {
                    _Failcode = _Failcode.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.FailureCode) && x.FailureCode.ToUpper().Contains(_Failcode))).ToList();
                }
                if (_ActualFinish != null)
                {
                    woList = woList.Where(x => (x.ActualFinishDate != null && x.ActualFinishDate.Value.Date.Equals(_ActualFinish.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(_Completioncomments))
                {
                    _Completioncomments = _Completioncomments.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.CompleteComments) && x.CompleteComments.ToUpper().Contains(_Completioncomments))).ToList();
                }
                foreach (var v in woList)
                {
                    objWorkOrderPrintModel = new WorkOrderPrintModel();
                    objWorkOrderPrintModel.ClientLookupId = v.ClientLookupId;
                    objWorkOrderPrintModel.Description = v.Description;
                    objWorkOrderPrintModel.ChargeToClientLookupId = v.ChargeToClientLookupId;
                    objWorkOrderPrintModel.ChargeTo_Name = v.ChargeTo_Name;
                    objWorkOrderPrintModel.Type = v.Type;
                    objWorkOrderPrintModel.Status = v.Status;
                    objWorkOrderPrintModel.Shift = v.Shift;
                    objWorkOrderPrintModel.Priority = v.Priority;
                    objWorkOrderPrintModel.CreateDate = v.CreateDate;
                    objWorkOrderPrintModel.Creator = v.Creator;
                    objWorkOrderPrintModel.Assigned = v.Assigned;
                    objWorkOrderPrintModel.ScheduledStartDate = v.ScheduledStartDate;
                    objWorkOrderPrintModel.FailureCode = v.FailureCode;
                    objWorkOrderPrintModel.ActualFinishDate = v.ActualFinishDate;
                    objWorkOrderPrintModel.CompleteComments = v.CompleteComments;
                    WorkOrderPrintModelList.Add(objWorkOrderPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = WorkOrderPrintModelList }, JsonSerializerDateSettings);
        }



        private List<WorkOrderModel> GetAllWorkOrderSortByColumnWithOrder(string order, string orderDir, List<WorkOrderModel> data)
        {
            List<WorkOrderModel> lst = new List<WorkOrderModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;               
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_Name).ToList() : data.OrderBy(p => p.ChargeTo_Name).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                // RKL - to get around error - not sure why
                case null:
                    orderDir = "ASC";
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }


        public PartialViewResult WorkOrderDetails(long workOrderId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataModel> Account = new List<DataModel>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();

            Task t1 = Task.Factory.StartNew(() => objWorksOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(workOrderId));
            Task t2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            Task t3 = Task.Factory.StartNew(() => Account = woWrapper.GetLookupList_Account());
            Task.WaitAll(t1, t2);
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
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
            }
            t3.Wait();
            if (Account != null && Account.Any(cus => cus.AccountId == objWorksOrderVM.workOrderModel.Labor_AccountId))
            {
                objWorksOrderVM.workOrderModel.strLabor_AccountId = Account.Where(x => x.AccountId == objWorksOrderVM.workOrderModel.Labor_AccountId).Select(x => x.Account).First();
            }
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
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(objWorksOrderVM.workOrderModel.WorkOrderId, objWorksOrderVM.workOrderModel.ClientLookupId, objWorksOrderVM.workOrderModel.Status, objWorksOrderVM.workOrderModel.Description, objWorksOrderVM.workOrderModel);


            //#region Follow Up

            objWorksOrderVM.woRequestModel = new WoRequestModel();
            objWorksOrderVM.woRequestModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
            objWorksOrderVM.woRequestModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });//.Where(x => x.Text != "Location");
            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            objWorksOrderVM.woRequestModel.ChargeToList = defaultChargeToList;
            objWorksOrderVM.woRequestModel.WorkOrderId = objWorksOrderVM.workOrderModel.WorkOrderId;

            //#endregion

            //#region Emergency

            //objWorksOrderVM.woEmergencyDescribeModel = new WoEmergencyDescribeModel();
            //objWorksOrderVM.woEmergencyDescribeModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
            //objWorksOrderVM.woEmergencyDescribeModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).Where(x => x.Text != "Location");
            //objWorksOrderVM.woEmergencyDescribeModel.ChargeToList = defaultChargeToList;
            //objWorksOrderVM.woEmergencyDescribeModel.WorkOrderId = objWorksOrderVM.workOrderModel.WorkOrderId;

            //objWorksOrderVM.woEmergencyOnDemandModel = new WoEmergencyOnDamandModel();
            //objWorksOrderVM.woEmergencyOnDemandModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
            //objWorksOrderVM.woEmergencyOnDemandModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).Where(x => x.Text != "Location");
            //objWorksOrderVM.woEmergencyOnDemandModel.ChargeToList = defaultChargeToList;
            //objWorksOrderVM.woEmergencyOnDemandModel.WorkOrderId = objWorksOrderVM.workOrderModel.WorkOrderId;

            //#endregion


            objWorksOrderVM._userdata = this.userData;
            objWorksOrderVM.security = this.userData.Security;
            objWorksOrderVM.IsMaintOnDemand = userData.Site.MaintOnDemand;
            objWorksOrderVM.downtimeModel = new DowntimeModel();
            objWorksOrderVM.downtimeModel.WorkOrderId = workOrderId;
            ViewBag.SecurityEstimate = userData.Security.WorkOrders.Edit;
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_WorkOrderDetails.cshtml", objWorksOrderVM);
        }
        public RedirectResult DetailFromEquipment(long workOrderId)
        {
            TempData["Mode"] = "DetailFromEquipment";
            string wOrderString = Convert.ToString(workOrderId);
            TempData["workOrderId"] = wOrderString;
            return Redirect("/WorkOrder/Index?page=Maintenance_Work_Order_Search");
        }
        public PartialViewResult AddWorkOrders()
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
            var ChargeTypeList = UtilityFunction.populateChargeType();
            if (ChargeTypeList != null)
            {
                objWoModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ChargeTypeLookUpList = PopulatelookUpListByType("");
            if (ChargeTypeLookUpList != null)
            {
                objWoModel.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
            }
            var PersonnelLookUplist = GetList_Personnel();
            if (PersonnelLookUplist != null)
            {
                objWoModel.WorkAssignedLookUpList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            objWoModel.PlantLocation = userData.Site.PlantLocation;
            objWorksOrderVM.workOrderModel = objWoModel;
            objWorksOrderVM.security = this.userData.Security;
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_WorkOrderAdd.cshtml", objWorksOrderVM);
        }
        public ActionResult Add()
        {
            TempData["Mode"] = "addWorkOrder";
            return Redirect("/WorkOrder/Index?page=Maintenance_Work_Order_Search");
        }
        public PartialViewResult EditWorkOrder(long workOrderId)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Failure = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Priority = new List<DataContracts.LookupList>();
            List<DataModel> Account = new List<DataModel>();

            Task t1= Task.Factory.StartNew(()=> objWorksOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(workOrderId));
            Task t2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            Task t3 = Task.Factory.StartNew(() => Account = woWrapper.GetLookupList_Account());
            Task.WaitAll(t1, t2);
            if (objWorksOrderVM.workOrderModel.RequiredDate != null && objWorksOrderVM.workOrderModel.RequiredDate.Value == default(DateTime))
            {
                objWorksOrderVM.workOrderModel.RequiredDate = null;
            }
            #region dropdown
            if (AllLookUps != null)
            {
                if (objWorksOrderVM.workOrderModel.SourceType == "PreventiveMaint")
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
            objWorksOrderVM.workOrderModel.PlantLocation = userData.Site.PlantLocation;
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(objWorksOrderVM.workOrderModel.WorkOrderId, objWorksOrderVM.workOrderModel.ClientLookupId, objWorksOrderVM.workOrderModel.Status, objWorksOrderVM.workOrderModel.Description, objWorksOrderVM.workOrderModel);
            objWorksOrderVM.security = this.userData.Security;
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
            List<AttachmentModel> AttachmentModelList = comWrapper.PopulateAttachments(workOrderId, "workorder", true, "woprint");
            string attachUrl = string.Empty;
            List<AttachmentModel> WoPdfAttachment = new List<AttachmentModel>();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WoPdfAttachment = AttachmentModelList;
            if (WoPdfAttachment != null && WoPdfAttachment.Count > 0)
            {
                WoPdfAttachment = WoPdfAttachment.Where(x => x.ContentType.Contains("pdf") && x.Profile==false && x.Image==false).ToList();
            }
            using (var ms = new MemoryStream())
            {
                using (var doc = new iTextSharp.text.Document())
                {
                    using (var copy = new iTextSharp.text.pdf.PdfSmartCopy(doc, ms))
                    {
                        doc.Open();
                        var msSinglePDf = new MemoryStream(PrintPdfImageGetByteStream(workOrderId, AttachmentModelList));
                        using (var reader = new iTextSharp.text.pdf.PdfReader(msSinglePDf))
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
                objVM.IsDescribeAdd = true;

            objVM.IsAddWorkOrderFromDashBoard = true;

            if (addtype == "Demand")
            {
                WoOnDemandModel objWoModel = new WoOnDemandModel();
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                var AllLookUps = commonWrapper.GetAllLookUpList();
                var Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objWoModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
                var ChargeTypeList = UtilityFunction.populateChargeType();
                if (ChargeTypeList != null)
                {
                    objWoModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                var ChargeTypeLookUpList = PopulatelookUpListByType("");
                if (ChargeTypeLookUpList != null)
                {
                    objWoModel.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
                }

                DataContracts.MaintOnDemandMaster maintOnDemandMaster = new DataContracts.MaintOnDemandMaster();
                maintOnDemandMaster.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                var maintOnDemandMasterList = maintOnDemandMaster.RetrieveAllBySiteId(this.userData.DatabaseKey, this.userData.Site.TimeZone).Where(a => a.InactiveFlag == false).ToList();
                if (maintOnDemandMasterList != null)
                {
                    objWoModel.MaintOnDemandList = maintOnDemandMasterList.Select(x => new SelectListItem { Text = x.ClientLookUpId + " - " + x.Description, Value = x.ClientLookUpId });
                }
                objVM.woOnDemandModel = objWoModel;
            }
            else if (addtype == "Describe")
            {
                WoDescriptionModel objWoModel = new WoDescriptionModel();
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                var AllLookUps = commonWrapper.GetAllLookUpList();
                var Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objWoModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
                var ChargeTypeList = UtilityFunction.populateChargeType();
                if (ChargeTypeList != null)
                {
                    objWoModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                var ChargeTypeLookUpList = PopulatelookUpListByType("");
                if (ChargeTypeLookUpList != null)
                {
                    objWoModel.ChargeTypelookUpList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToClientLookupId.ToString() });
                }

                objVM.woDescriptionModel = objWoModel;
            }

            objVM.security = this.userData.Security;

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
                WorkOrder objWorkOrder = new DataContracts.WorkOrder();

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
        public PartialViewResult AddTasks(long workOrderId, string ClientLookupId, string Status, string Description)
        {
            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                woTaskModel = new WoTaskModel
                {
                    WorkOrderId = workOrderId,
                    ClientLookupId = ClientLookupId,
                    ChargeTypeList = ScheduleChargeTypeList != null ? ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : null,
                    ChargeTypelookUpList = new SelectList(new[] { "" })
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, Description)
            };
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddWoTask.cshtml", objWorksOrderVM);
        }
        public PartialViewResult EditTasks(long workOrderId, long _taskId, string ClientLookupId, string Status, string Description)
        {
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
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, Description)
            };

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

        public PartialViewResult AddAssignment(long workOrderId, string ClientLookupId, string Status, string Description)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            var PersonnelLookUplist = GetList_Personnel();
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                woAssignmentModel = new WoAssignmentModel
                {
                    WorkOrderId = workOrderId,
                    ClientLookupId = ClientLookupId,
                    WorkAssignedLookUpList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null,
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, Description)
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

        public PartialViewResult EditAssignment(long WorkOrderID, long _assignmentId, string ClientLookupId, string Status, string Description, string AssignedTo_PersonnelClientLookupId)
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
                workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderID, ClientLookupId, Status, Description)
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
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<Notes> NoteList = woWrapper.PopulateNote(workOrderId);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotes(WorkOrderVM woVM)
        {
            if (ModelState.IsValid)
            {
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = woWrapper.AddOrUpdateNote(woVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderId = woVM.notesModel.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteNotes(long _notesId)
        {
            WorkOrderWrapper EWrapper = new WorkOrderWrapper(userData);
            var deleteResult = EWrapper.DeleteNote(_notesId);
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

        #endregion

        #region Attachment
        [HttpPost]
        public string PopulateAttachment(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AttachmentList = objCommonWrapper.PopulateAttachments(workOrderId, "WorkOrder", userData.Security.WorkOrders.Edit);
            if (AttachmentList != null)
            {
                AttachmentList = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, AttachmentList);
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
        public PartialViewResult AddAttachments(long workOrderID, string ClientLookupId, string Status, string Description)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM();
            AttachmentModel Attachment = new AttachmentModel();
            objWorksOrderVM.attachmentModel = Attachment;
            objWorksOrderVM.attachmentModel.ClientLookupId = ClientLookupId;
            Attachment.ClientLookupId = ClientLookupId;
            Attachment.WorkOrderId = workOrderID;
            objWorksOrderVM.attachmentModel = Attachment;
            objWorksOrderVM.workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderID, ClientLookupId, Status, Description);
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
                Client.Models.AttachmentModel attachmentModel = new Client.Models.AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);
                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.WorkOrderId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = "WorkOrder";
                bool attachStatus = false;
                fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.WorkOrders.Edit);
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
            DataContracts.Attachment fileInfo = new DataContracts.Attachment();
            fileInfo = objCommonWrapper.DownloadAttachment(_fileinfoId);

            return Redirect(fileInfo.AttachmentURL);
        }
        [HttpPost]
        public ActionResult DeleteAttachments(long _fileAttachmentId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var deleteResult = objCommonWrapper.DeleteAttachment(_fileAttachmentId);
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

        #region Estimate-Part
        [HttpPost]
        public string PopulateEstimatePart(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<EstimatePart> EstimatePartList = woWrapper.populateEstimatedParts(workOrderId);
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
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }
        [HttpGet]
        public PartialViewResult AddEstimatesPart(long WorkOrderId, string ClientLookupId, string Status, string Description)
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                estimatePart = new EstimatePart
                {
                    WorkOrderId = WorkOrderId,
                    MainClientLookupId = ClientLookupId,
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderId, ClientLookupId, Status, Description)
            };
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddEstimatePart", objWorkOrderVM);
        }
        public PartialViewResult EditEstimatesPart(long WorkOrderId, string MainClientLookupId, string PartclintLookupId, long EstimatedCostsId, string Description, long? UnitCost, long? Quantity, long? TotalCost, string Status)
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                estimatePart = new EstimatePart
                {
                    WorkOrderId = WorkOrderId,
                    MainClientLookupId = MainClientLookupId,
                    ClientLookupId = PartclintLookupId,
                    EstimatedCostsId = EstimatedCostsId,
                    UnitCost = UnitCost,
                    Quantity = Quantity,
                    TotalCost = TotalCost,

                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderId, MainClientLookupId, Status, Description)
            };
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
            long orderQty = 0, long lineNo = 0,
            long receivequantity = 0, string srcData = ""
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
        public PartialViewResult AddEstimatesLabor(long workOrderId, string ClientLookupId, string Status, string Description)
        {
            var craftDetails = GetLookUpList_Craft();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                estimateLabor = new EstimateLabor
                {
                    workOrderId = workOrderId,
                    ClientLookupId = ClientLookupId,
                    CraftList = craftDetails != null ? craftDetails.Select(x => new SelectListItem { Text = x.ClientLookUpId + " - " + x.Description + " - " + x.ChargeRate, Value = x.CraftId.ToString() }) : null,
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, Description)
            };
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddEstimateLabor", objWorkOrderVM);
        }

        [HttpGet]
        public PartialViewResult EditEstimatesLabor(long WorkOrderId, string ClientLookupId, string Status, string Description, long EstimatedCostsId, decimal? duration, long categoryId, decimal? quantity)
        {
            var craftDetails = GetLookUpList_Craft();
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
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderId, ClientLookupId, Status, Description)
            };
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
        public PartialViewResult AddEstimatesOther(long workOrderId, string ClientLookupId, string Status, string Description)
        {
            var SourceTypeList = UtilityFunction.populateSourceList();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                estimateOtherModel = new EstimateOther
                {
                    workOrderId = workOrderId,
                    ClientLookupId = ClientLookupId,
                    SourceList = SourceTypeList != null ? SourceTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : null,
                    Source = "Internal",
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, Description),
                _userdata = this.userData

            };
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddEstimateOther", objWorkOrderVM);
        }
        public PartialViewResult EditEstimatesOther(long workOrderId, string ClientLookupId, long EstimatedCostsId, string description, string source, long unitCost, long quantity, long totalCost, long updateIndex, long? vendorId, string VendorClientLookupId, string Status, string summarydescription)
        {
            var SourceTypeList = UtilityFunction.populateSourceList();
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
                    UnitCost = unitCost,
                    Quantity = quantity,
                    UpdateIndex = updateIndex,
                    VendorClientLookupId = VendorClientLookupId,
                    SourceList = SourceTypeList != null ? SourceTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : null,
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, summarydescription),
                _userdata = this.userData
            };
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
        public PartialViewResult AddLabor(long WorkOrderID, string ClientLookupId, string Status, string Description)
        {
            var PersonnelLookUplist = GetList_Personnel();
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                woLaborModel = new WoActualLabor
                {
                    workOrderId = WorkOrderID,
                    PersonnelClientLookupId = ClientLookupId,
                    WorkAssignedLookUpList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null,
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderID, ClientLookupId, Status, Description)
            };
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/WorkOrder/_AddWoLabor.cshtml", objWorksOrderVM);
        }
        public PartialViewResult EditActualLabor(long WorkOrderID, string ClientLookupId, long timeCardId, long personnelID, decimal hours, string startDate, string Status, string Description)
        {
            var PersonnelLookUplist = GetList_Personnel();
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                woLaborModel = new WoActualLabor
                {
                    workOrderId = WorkOrderID,
                    PersonnelClientLookupId = ClientLookupId,
                    PersonnelID = personnelID,
                    TimecardId = timeCardId,
                    StartDate = !string.IsNullOrEmpty(startDate) ? DateTime.Parse(startDate) : DateTime.Now,
                    Hours = hours,
                    WorkAssignedLookUpList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(WorkOrderID, ClientLookupId, Status, Description)
            };
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

        public PartialViewResult AddActualOther(long workOrderId, string ClientLookupId, string Status, string Description)
        {
            var SourceTypeList = UtilityFunction.populateSourceList();
            var VendorLookUpList = RetrieveForLookupVendor();
            WorkOrderVM objWorkOrderVM = new WorkOrderVM()
            {
                actualOther = new ActualOther
                {
                    workOrderId = workOrderId,
                    ClientLookupId = ClientLookupId,
                    SourceList = SourceTypeList != null ? SourceTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : null,
                    VendorLookUpList = VendorLookUpList != null ? VendorLookUpList.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.VendorId.ToString() }) : null,
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, Description)
            };
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("_AddActualOther", objWorkOrderVM);
        }

        public PartialViewResult EditActualOther(long workOrderId, string ClientLookupId, string VendorName, long otherCostsId, string description, string source, long unitCost, long quantity, long totalCost, long? vendorId, string Status, string summarydescription)
        {
            var SourceTypeList = UtilityFunction.populateSourceList();
            var VendorLookUpList = RetrieveForLookupVendor();
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
                },
                workOrderSummaryModel = GetWorkOrderSummaryModel(workOrderId, ClientLookupId, Status, summarydescription)
            };
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

        #region Private Method
        private WorkOrderSummaryModel GetWorkOrderSummaryModel(long WorkOrderId, string ClientLookupId, string Status, string Description, WorkOrderModel workOrder = null)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            WorkOrderSummaryModel workOrderSummaryModel = new WorkOrderSummaryModel();
            CommonWrapper comWrapper = new CommonWrapper(userData);
            workOrderSummaryModel.WorkOrder_ClientLookupId = ClientLookupId;
            workOrderSummaryModel.Status = Status;
            workOrderSummaryModel.Description = Description;
            workOrderSummaryModel.ImageUrl = comWrapper.GetAzureImageUrl(WorkOrderId, AttachmentTableConstant.WorkOrder);
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
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
            WorkOrder objWorkOrder = new DataContracts.WorkOrder();
            WorkOrderModel objWorkOrderModel = new WorkOrderModel();
            objWorkOrderModel.WorkOrderId = WorkOrderId;
            objWorkOrderModel.CompleteComments = CompleteComments;
            objWorkOrder = woWrapper.CompleteWO(objWorkOrderModel);
            return objWorkOrder;
        }
        public JsonResult CompleteWorkOrder(long WorkOrderId, string CompleteComments)
        {
            WorkOrder objWorkOrder = new DataContracts.WorkOrder();
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
            BatchCompleteResultModel objWoBatchList;
            WorkOrder objWorkOrder = new DataContracts.WorkOrder();
            string strF = UtilityFunction.GetMessageFromResource("GlobalFailed", LocalizeResourceSetConstants.Global);
            string strSc = UtilityFunction.GetMessageFromResource("GlobalSuccess", LocalizeResourceSetConstants.Global);
            foreach (var item in model.list)
            {
                objWorkOrder = this.WorkOrderComplete(item.WorkOrderId, model.comments);
                objWoBatchList = new BatchCompleteResultModel();
                objWoBatchList.ClientLookupId = item.ClientLookupId;
                objWoBatchList.ErrMsg = objWorkOrder.ErrorMessages;
                objWoBatchList.Status = (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0) ? strF : strSc;
                WoBatchList.Add(objWoBatchList);
            };
            return Json(WoBatchList, JsonRequestBehavior.AllowGet);
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
                    return Json(new { Issuccess = true, msg = IsAddOrUpdate, WorkOrderId = objInventoryCheckVM.downtimeModel.WorkOrderId }, JsonRequestBehavior.AllowGet);
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
            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
            if (ScheduleChargeTypeList != null && ScheduleChargeTypeList.Any(cus => cus.value == objWorkOrderVM.workOrderModel.ChargeType))
            {
                objWorkOrderVM.workOrderModel.ChargeType = ScheduleChargeTypeList.Where(x => x.value == objWorkOrderVM.workOrderModel.ChargeType).Select(x => x.text).First();
            }
            if (Type != null)
            {
                objWorkOrderVM.woRequestModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
            }
            if (ScheduleChargeTypeList != null)
            {
                objWorkOrderVM.woRequestModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });//.Where(x => x.Text != "Location");
            }
            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            objWorkOrderVM.woRequestModel.ChargeToList = defaultChargeToList;
            objWorkOrderVM.woRequestModel.WorkOrderId = objWorkOrderVM.workOrderModel.WorkOrderId;
            objWorkOrderVM.woRequestModel.ClientLookupId = ClientLookupId;
            objWorkOrderVM.woRequestModel.ChargeType = objWorkOrderVM.workOrderModel.ChargeType;
            objWorkOrderVM.woRequestModel.Description = objWorkOrderVM.workOrderModel.Description;
            objWorkOrderVM.woRequestModel.ChargeToClientLookupId = objWorkOrderVM.workOrderModel.ChargeToClientLookupId;
            objWorkOrderVM.woRequestModel.Type = objWorkOrderVM.workOrderModel.Type;
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

            objWorksOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(WorkoderId);
            objWorksOrderVM.woEmergencyOnDemandModel.ClientLookupId = objWorksOrderVM.workOrderModel.ClientLookupId;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objWorksOrderVM.woEmergencyOnDemandModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
            objWorksOrderVM.woEmergencyOnDemandModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });//.Where(x => x.Text != "Location");
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
            objWorksOrderVM.workOrderModel = woWrapper.getWorkOderDetailsById(WorkoderId);
            objWorksOrderVM.woEmergencyDescribeModel.ClientLookupId = objWorksOrderVM.workOrderModel.ClientLookupId;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objWorksOrderVM.woEmergencyDescribeModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            var ScheduleChargeTypeList = UtilityFunction.populateChargeType();

            objWorksOrderVM.woEmergencyDescribeModel.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });//.Where(x => x.Text != "Location");
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

        #region Print All WorkOrderPages


        public ActionResult PrintWoList(List<Int64> listwo)
        {
            if (listwo.Count > 0)
            {
                bool jsonStringExceed = false;
                HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
                Int64 fileSizeCounter = 0;
                Int32 maxPdfSize = section.MaxRequestLength;
                string attachUrl = string.Empty;
                using (var ms = new MemoryStream())
                {
                    using (var doc = new iTextSharp.text.Document())
                    {
                        using (var copy = new iTextSharp.text.pdf.PdfSmartCopy(doc, ms))
                        {
                            doc.Open();
                            foreach (var workOrderId in listwo)
                            {
                                CommonWrapper comWrapper = new CommonWrapper(userData);
                                List<AttachmentModel> AttachmentModelList = comWrapper.PopulateAttachments(workOrderId, "workorder", true, "woprint");
                                var msSinglePDf = new MemoryStream(PrintPdfImageGetByteStream(workOrderId, AttachmentModelList));
                                if (AttachmentModelList != null && AttachmentModelList.Count > 0)
                                {
                                    AttachmentModelList = AttachmentModelList.Where(x => x.ContentType.Contains("pdf") && x.Profile == false && x.Image == false).ToList();
                                }

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

                                if (AttachmentModelList != null && AttachmentModelList.Count > 0)
                                {
                                    foreach (var itemAttach in AttachmentModelList)
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
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;                    
                }
            }
            else
            {
                var returnOjb = new { success = false };
                return Json(returnOjb, JsonRequestBehavior.AllowGet);
            }
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
            AzureImageURL= comWrapper.GetClientLogoUrl();
            objWorksOrderVM.workOrderModel.AzureImageURL = AzureImageURL;
            WoPhotoAttachment = WoPhotoAttachment.Where(a => a.ContentType.Contains("image")).ToList();
            objWorksOrderVM.AttachmentList = comWrapper.GetSasImageUrlList(ref WoPhotoAttachment);

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

            Task.WaitAll(task1,task3,task4,task5,task6,task7);

            AzureImageURL = comWrapper.GetClientLogoUrl();
            objWorksOrderVM.workOrderModel.AzureImageURL = AzureImageURL;
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

        #region Cancel & Print WO-list From Search
        public JsonResult CancelWoList(WoCancelAndPrintModel model)
        {
            WorkOrderVM objWorksOrderVM = new WorkOrderVM(); WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            objWorksOrderVM.security = this.userData.Security;
            bool HasAuthToCancel = false;
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
                if (item.Status == WorkOrderStatusConstants.Complete || item.Status == WorkOrderStatusConstants.Canceled)
                {
                    failedWoList.Append(item.ClientLookupId + ",");
                }
                else
                {
                    WorkOrder Wojob = woWrapper.CancelJob(item.WorkOrderId, model.cancelreason, model.comments);
                }
            }
            if (!string.IsNullOrEmpty(failedWoList.ToString()))
            {
                return Json(new { data = "Work Order(s) " + failedWoList + " can't be cancelled. Please check the status." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PrintWoListFromIndex(WoCancelAndPrintModel model)
        {
            if (model.list.Count > 0)
            {
                return PrintPDFFromIndex(model);

            }
            else
            {
                var returnOjb = new { success = false };
                return Json(returnOjb, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PrintPDFFromIndex(WoCancelAndPrintModel model)
        {
            var ms = new MemoryStream();
            bool jsonStringExceed = false;
            HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            Int64 fileSizeCounter = 0;
            Int32 maxPdfSize = section.MaxRequestLength;
            string attachUrl = string.Empty;
            var doc = new iTextSharp.text.Document();
            var copy = new iTextSharp.text.pdf.PdfSmartCopy(doc, ms);
            doc.Open();
            foreach (var item in model.list)
            {
                CommonWrapper comWrapper = new CommonWrapper(userData);
                List<AttachmentModel> AttachmentModelList = comWrapper.PopulateAttachments(item.WorkOrderId, "workorder", true, "woprint");
                var msSinglePDf = new MemoryStream(PrintPdfImageGetByteStream(item.WorkOrderId, AttachmentModelList));
                if (AttachmentModelList != null && AttachmentModelList.Count > 0)
                {
                    AttachmentModelList = AttachmentModelList.Where(x => x.ContentType.Contains("pdf") && x.Profile == false && x.Image == false).ToList();
                }
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
                if (AttachmentModelList != null && AttachmentModelList.Count > 0)
                {
                    foreach (var itemAttach in AttachmentModelList)
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

    }
}
