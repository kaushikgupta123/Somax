using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.FleetService;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.Common;
using Client.Models.FleetScheduledService;
using Client.Models.FleetService;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Client.Models.Common.UserMentionDataModel;
//using System.Threading.Tasks;

namespace Client.Controllers.FleetService
{
    public class FleetServiceController : SomaxBaseController
    {

        #region Search Fleet Service 
        [CheckUserSecurity(securityType = SecurityConstants.Fleet_Service_Order)]
        public ActionResult Index()
        {
            FleetServiceVM fltserviceVM = new FleetServiceVM();
            FleetServiceModel objFleetServiceModel = new FleetServiceModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string mode = Convert.ToString(TempData["Mode"]);
            fltserviceVM.IsRedirectFromAsset = false;
            if (mode == "DetailFromFleetAsset")
            {
                long SOId = Convert.ToInt64(TempData["ServiceOrderId"]);
                fltserviceVM.SOId = SOId;
                fltserviceVM.IsRedirectFromAsset = true;
            }

            objFleetServiceModel.ScheduleWorkList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.ServiceOrder);
            fltserviceVM.FleetServiceModel = objFleetServiceModel;
            IEnumerable<SelectListItem> AssignedList;
            IEnumerable<SelectListItem> PersonnelList;
            this.PopulatePopUp(out AssignedList, out PersonnelList);
            fltserviceVM.PersonnelList = PersonnelList;
            fltserviceVM.security = this.userData.Security;
            fltserviceVM.FleetServiceModel.DateRangeDropListForSOCreatedate = UtilityFunction.GetTimeRangeDropForAllStatusFS().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            fltserviceVM.FleetServiceModel.DateRangeDropListForSO = UtilityFunction.GetTimeRangeDropForSO().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> ShiftList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                ShiftList = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                TypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.SO_TYPE).ToList();
                if (ShiftList != null)
                {
                    fltserviceVM.LookupShiftList = ShiftList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
                if (TypeList != null)
                {
                    fltserviceVM.LookupTypeList = TypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }

            }
            LocalizeControls(fltserviceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return View(fltserviceVM);
        }
        [HttpPost]
        public string GetFleetServiceGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, DateTime? CompleteStartDateVw = null, DateTime? CompleteEndDateVw = null, string personnelList = "",
                string AssetID = "", string Name = "", string Description = "", string Shift = "", string Type = "", string VIN = "", string searchText = "", string order = "2", string orderDir = "asc")
        {
            var filter = CustomQueryDisplayId;
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            //string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            FleetServiceWrapper fltServiceWrapper = new FleetServiceWrapper(userData);
            List<FleetServiceModel> fleetServiceList = fltServiceWrapper.GetFleetServiceGridData(CustomQueryDisplayId,order, orderDir, skip, length ?? 0,
                CreateStartDateVw, CreateEndDateVw, CompleteStartDateVw, CompleteEndDateVw, personnelList, AssetID, Name, Description, Shift, Type, VIN, searchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (fleetServiceList != null && fleetServiceList.Count > 0)
            {
                recordsFiltered = fleetServiceList[0].TotalCount;
                totalRecords = fleetServiceList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = fleetServiceList
              .ToList();

            bool IsFleetServiceOrderAccessSecurity = true;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsFleetServiceTaskAccessSecurity = IsFleetServiceOrderAccessSecurity }, JsonSerializerDateSettings);
        }

        #endregion

        #region ServiceOrder Line Item
        public ActionResult GetServiceOrderInnerGrid(long ServiceOrderID)
        {
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            FleetServiceWrapper FSWrapper = new FleetServiceWrapper(userData);
            objFleetServiceVM.FleetServiceLineItemModelList = FSWrapper.PopulateLineitems(ServiceOrderID);
            LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return View("_InnerGridSOLineItem", objFleetServiceVM);
        }
        #endregion

        #region Fleet Service Details
        public PartialViewResult FleetServiceDetails(long ServiceOrderId)
        {
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            CompleteServiceOrderModel objCompleteServiceOrderModel = new CompleteServiceOrderModel();
            FleetServiceWrapper objWrapper = new FleetServiceWrapper(userData);
            ServiceOrderScheduleModel soScheduleModel = new ServiceOrderScheduleModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objFleetServiceVM.FleetServiceModel = objWrapper.RetrieveByServiceOrderId(ServiceOrderId);
            long EquipmentId = objFleetServiceVM.FleetServiceModel.EquipmentId;
            Task attTask;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            attTask = Task.Factory.StartNew(() => objFleetServiceVM.attachmentCount = objCommonWrapper.AttachmentCount(ServiceOrderId, AttachmentTableConstant.ServiceOrder, userData.Security.Fleet_ServiceOrder.Edit));

            string ClientLookupId = objFleetServiceVM.FleetServiceModel.ClientLookupId;
            string AssetName = objFleetServiceVM.FleetServiceModel.AssetName;
            string Meter1Type = objFleetServiceVM.FleetServiceModel.Meter1Type;
            decimal Meter1CurrentReading = objFleetServiceVM.FleetServiceModel.Meter1CurrentReading;
            string Meter2Type = objFleetServiceVM.FleetServiceModel.Meter2Type;
            decimal Meter2CurrentReading = objFleetServiceVM.FleetServiceModel.Meter2CurrentReading;
            DateTime? ScheduleDate = objFleetServiceVM.FleetServiceModel.ScheduleDate;
            string Assigned = objFleetServiceVM.FleetServiceModel.Assigned;
            DateTime? CompleteDate = objFleetServiceVM.FleetServiceModel.CompleteDate;
            long SOId = objFleetServiceVM.FleetServiceModel.ServiceOrderId;
            long Assigned_personnalid = objFleetServiceVM.FleetServiceModel.Assign_PersonnelId;
            string Status = objFleetServiceVM.FleetServiceModel.Status;
            string EquipmentClientLookupId = objFleetServiceVM.FleetServiceModel.EquipmentClientLookupId;

            objFleetServiceVM._FleetServiceSummaryModel = GetFleetServiceSummary(EquipmentId, ClientLookupId, AssetName, Meter1Type, Meter1CurrentReading, Meter2Type, Meter2CurrentReading, ScheduleDate, Assigned, CompleteDate, SOId, Assigned_personnalid, Status, EquipmentClientLookupId);
            objFleetServiceVM._FleetServiceSummaryModel.IsDetail = true;
           
            var totalList = objWrapper.SOSchedulePersonnelList();
            soScheduleModel.Personnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            soScheduleModel.Schedulestartdate = objFleetServiceVM.FleetServiceModel.ScheduleDate;
            soScheduleModel.ServiceOrderId = objFleetServiceVM.FleetServiceModel.ServiceOrderId;
            objFleetServiceVM.soScheduleModel = soScheduleModel;
            objFleetServiceVM._userdata = userData;
            //V2-483 - 2021-Feb-13
            var CancelLookUpListSo = AllLookUps.Where(x => x.ListName == LookupListConstants.FleetServiceOrderCancel).ToList();
            //var CancelLookUpListSo = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
            if (CancelLookUpListSo != null)
            {
                objFleetServiceVM.CancelServiceOrderModal = new CancelServiceOrderModal();
                objFleetServiceVM.CancelServiceOrderModal.ServiceOrderId = ServiceOrderId;
                objFleetServiceVM.CancelServiceOrderModal.CancelReasonListSo = CancelLookUpListSo.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            objFleetServiceVM.security = this.userData.Security;

            //Complete Service Order
            objCompleteServiceOrderModel.ServiceOrderId = objFleetServiceVM.FleetServiceModel.ServiceOrderId;
            objCompleteServiceOrderModel.EquipmentId = objFleetServiceVM.FleetServiceModel.EquipmentId;
            objCompleteServiceOrderModel.EquipmentClientLookupId = objFleetServiceVM.FleetServiceModel.EquipmentClientLookupId;
            objCompleteServiceOrderModel.Meter1Type = objFleetServiceVM.FleetServiceModel.Meter1Type;
            objCompleteServiceOrderModel.Meter1CurrentReading = objFleetServiceVM.FleetServiceModel.Meter1CurrentReading;
            objCompleteServiceOrderModel.Meter2Type = objFleetServiceVM.FleetServiceModel.Meter2Type;
            objCompleteServiceOrderModel.Meter2CurrentReading = objFleetServiceVM.FleetServiceModel.Meter2CurrentReading;
            if (objFleetServiceVM.FleetServiceModel.Meter1CurrentReadingDate != null && objFleetServiceVM.FleetServiceModel.Meter1CurrentReadingDate != default(DateTime))
            {
                objCompleteServiceOrderModel.Meter1DayDiff = Math.Abs((DateTime.Today - Convert.ToDateTime(objFleetServiceVM.FleetServiceModel.Meter1CurrentReadingDate)).Days);
            }
            if (objFleetServiceVM.FleetServiceModel.Meter2CurrentReadingDate != null && objFleetServiceVM.FleetServiceModel.Meter2CurrentReadingDate != default(DateTime))
            {
                objCompleteServiceOrderModel.Meter2DayDiff = Math.Abs((DateTime.Today - Convert.ToDateTime(objFleetServiceVM.FleetServiceModel.Meter2CurrentReadingDate)).Days);
            }
            objCompleteServiceOrderModel.Meter1Units = objFleetServiceVM.FleetServiceModel.Meter1Units;
            objCompleteServiceOrderModel.Meter2Units = objFleetServiceVM.FleetServiceModel.Meter2Units;

            objFleetServiceVM.CompleteServiceOrderModel = objCompleteServiceOrderModel;
            LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return PartialView("_FleetServiceDetails", objFleetServiceVM);
        }
        public PartialViewResult ServiceLineItemDetails(long ServiceOrderId)
        {
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);

            objFleetServiceVM.FleetServiceLineItemModelList = objFleetServiceWrapper.PopulateLineitems(ServiceOrderId);

            if (objFleetServiceVM.FleetServiceLineItemModelList.Count > 0)
            {
                var AllLookUps = comWrapper.GetAllLookUpList();
                if (AllLookUps != null)
                {
                    var RepairReasonList = AllLookUps.Where(x => x.ListName == LookupListConstants.SO_RepairReason).ToList();
                    if (RepairReasonList != null)
                    {
                        objFleetServiceVM.LookupRepairReasonList = RepairReasonList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                    }

                }

                var AllHierachichalLookUps = comWrapper.GetHierarchicalListByName(LookupListConstants.VMRS_Code);
                if (AllHierachichalLookUps != null)
                {
                    var VMRSSystemList = AllHierachichalLookUps.Where(x => x.ListName == LookupListConstants.VMRS_Code).ToList();
                    if (VMRSSystemList != null)
                    {
                        objFleetServiceVM.VMRSSystemList = VMRSSystemList;
                    }
                }
            }
            objFleetServiceVM.security = this.userData.Security;
            LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return PartialView("_ServiceOrderLineItemDetails", objFleetServiceVM);
        }
        [HttpPost]
        public string PopulateLabor(int? draw, int? start, int? length, long ServiceOrderId, long ServiceOrderLineItemId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            FleetServiceWrapper objWrapper = new FleetServiceWrapper(userData);
            List<ServiceOrderLabourModel> ServiceOrderLabourList = objWrapper.RetrieveLabourByServiceOrderId(ServiceOrderId, ServiceOrderLineItemId);
            if (ServiceOrderLabourList != null)
            {
                ServiceOrderLabourList = this.GetAllLaborSortByColumnWithOrder(order, orderDir, ServiceOrderLabourList);
            }
            else
            {
                ServiceOrderLabourList = new List<ServiceOrderLabourModel>();
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = ServiceOrderLabourList.Count();
            totalRecords = ServiceOrderLabourList.Count();
            int initialPage = start.Value;
            var filteredResult = ServiceOrderLabourList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<ServiceOrderLabourModel> GetAllLaborSortByColumnWithOrder(string order, string orderDir, List<ServiceOrderLabourModel> data)
        {
            List<ServiceOrderLabourModel> lst = new List<ServiceOrderLabourModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StartDate).ToList() : data.OrderBy(p => p.StartDate).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Hours).ToList() : data.OrderBy(p => p.Hours).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Cost).ToList() : data.OrderBy(p => p.Cost).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                    break;
            }
            return lst;
        }
        public string PopulatePart(int? draw, int? start, int? length, long ServiceOrderId, long ServiceOrderLineItemId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
            List<ServiceOrderIssuePartModel> PartsList = objFleetServiceWrapper.RetrievePartByServiceOrderId(ServiceOrderId, ServiceOrderLineItemId);
            if (PartsList != null)
            {
                PartsList = this.GetAllPartsSortByColumnWithOrder(order, orderDir, PartsList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PartsList.Count();
            totalRecords = PartsList.Count();
            int initialPage = start.Value;
            var filteredResult = PartsList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }
        private List<ServiceOrderIssuePartModel> GetAllPartsSortByColumnWithOrder(string order, string orderDir, List<ServiceOrderIssuePartModel> data)
        {
            List<ServiceOrderIssuePartModel> lst = new List<ServiceOrderIssuePartModel>();
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
        [HttpPost]
        public string PopulateOthers(int? draw, int? start, int? length, long ServiceOrderId, long ServiceOrderLineItemId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
            List<ServiceOrderOthers> OtherList = objFleetServiceWrapper.PopulateOthers(ServiceOrderId, ServiceOrderLineItemId);
            if (OtherList != null)
            {
                OtherList = this.GetAllActualOtherSortByColumnWithOrder(order, orderDir, OtherList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = OtherList.Count();
            totalRecords = OtherList.Count();
            int initialPage = start.Value;
            var filteredResult = OtherList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            bool isVendorcolShow = userData.Site.UseVendorMaster;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, isVendorcolShow = isVendorcolShow }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<ServiceOrderOthers> GetAllActualOtherSortByColumnWithOrder(string order, string orderDir, List<ServiceOrderOthers> data)
        {
            List<ServiceOrderOthers> lst = new List<ServiceOrderOthers>();
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

        #region Line Item
        public PartialViewResult AddLineItem(long ServiceOrderId, string CliemtLookupId)
        {
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            FleetServiceWrapper objWrapper = new FleetServiceWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            List<HierarchicalList> VMRSSystemList = new List<HierarchicalList>();

            var AllLookUps = comWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var RepairReasonList = AllLookUps.Where(x => x.ListName == LookupListConstants.SO_RepairReason).ToList();
                if (RepairReasonList != null)
                {
                    objFleetServiceVM.LookupRepairReasonList = RepairReasonList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }

            VMRSSystemList = comWrapper.GetHierarchicalListByName(LookupListConstants.VMRS_Code);
            if (VMRSSystemList != null)
            {
                objFleetServiceVM.VMRSSystemList = VMRSSystemList;
            }

            objFleetServiceVM.ServiceTaskList = objWrapper.PopulateServiceTask();
            objFleetServiceVM.FleetServiceModel = new FleetServiceModel();
            objFleetServiceVM.FleetServiceModel.ClientLookupId = CliemtLookupId;
            objFleetServiceVM.FleetServiceModel.ServiceOrderId = ServiceOrderId;
            objFleetServiceVM.security = this.userData.Security;
            LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return PartialView("_AddLineItem", objFleetServiceVM);
        }
        [HttpPost]
        public JsonResult GetHierarchicalList(string ListName, string List1Value)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            List<HierarchicalList> HierarchicalList = new List<HierarchicalList>();
            List<SelectListItem> AssemblyList = new List<SelectListItem>();

            HierarchicalList = comWrapper.GetHierarchicalListByName(ListName);
            if (HierarchicalList != null && HierarchicalList.Count > 0)
            {
                AssemblyList = HierarchicalList
                            .Select(x => new { x.Level1Value, x.Level2Value, x.Level2Description })
                            .Distinct()
                            .Where(x => x.Level1Value == List1Value)
                            .Select(x => new SelectListItem { Text = x.Level2Value + " - " + x.Level2Description, Value = x.Level2Value })
                            .OrderBy(x => x.Value).ToList();
            }
            return Json(new { Result = JsonReturnEnum.success.ToString(), data = AssemblyList });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ServiceOrderLineItemAddEdit(FleetServiceVM objFleetServiceVM)
        {
            List<string> ErrorList = new List<string>();
            FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                ServiceOrderLineItem objLineItem = new ServiceOrderLineItem();
                if (objFleetServiceVM.FleetServiceLineItemModel.ServiceOrderLineItemId > 0)
                {
                    Mode = "Edit";
                    objLineItem = objFleetServiceWrapper.EditLineItem(objFleetServiceVM.FleetServiceLineItemModel);
                }
                else
                {
                    Mode = "Add";
                    objLineItem = objFleetServiceWrapper.AddLineItem(objFleetServiceVM.FleetServiceLineItemModel);
                }

                if (objLineItem.ErrorMessages != null && objLineItem.ErrorMessages.Count > 0)
                {
                    return Json(objLineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Mode = Mode, ServiceOrderId = objFleetServiceVM.FleetServiceLineItemModel.ServiceOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteLineItem(long ServiceOrderLineItemId)
        {
            ServiceOrderLineItem objLineItem = new ServiceOrderLineItem();
            FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
            bool result = objFleetServiceWrapper.DeleteLineItem(ServiceOrderLineItemId);
            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ValidateForDeleteLineItem(long ServiceOrderId, long ServiceOrderLineItemId)
        {
            ServiceOrderLineItem objLineItem = new ServiceOrderLineItem();
            FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
            LineItemDeleteParameter objParam = objFleetServiceWrapper.ValidateForDeleteLineItem(ServiceOrderId, ServiceOrderLineItemId);
            return Json(objParam, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Labour
        [HttpGet]
        public PartialViewResult AddEditLabor(long ServiceOrderId, long ServiceOrderLineItemId, long TimeCardId = 0)
        {
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
            ServiceOrderLabourModel objServiceOrderLabour = new ServiceOrderLabourModel();
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);

            var PersonnelLookUplist = GetList_Personnel();
            objFleetServiceVM.PersonnelLabourList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null;

            if (TimeCardId > 0)
            {
                ViewBag.Mode = "Edit";
                objServiceOrderLabour = objFleetServiceWrapper.RetrieveByTimecardid(TimeCardId, ServiceOrderId, ServiceOrderLineItemId);
                objServiceOrderLabour.TimecardId = TimeCardId;
            }
            else
            {
                ViewBag.Mode = "Add";
            }
            objServiceOrderLabour.ServiceOrderId = ServiceOrderId;
            objServiceOrderLabour.ServiceOrderLineItemId = ServiceOrderLineItemId;
            objFleetServiceVM.ServiceOrderLabour = objServiceOrderLabour;

            var AllLookUps = objCommonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var VMRSWorkAccomplishedList = AllLookUps.Where(x => x.ListName == LookupListConstants.VMRS_WA).ToList();
                if (VMRSWorkAccomplishedList != null && VMRSWorkAccomplishedList.Count > 0)
                {
                    objFleetServiceVM.VMRSWorkAccomplishedList = VMRSWorkAccomplishedList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
            }

            LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return PartialView("_AddEditLabor", objFleetServiceVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveLabor(FleetServiceVM objFleetServiceVM)
        {
            if (ModelState.IsValid)
            {
                FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
                if (objFleetServiceVM.ServiceOrderLabour.StartDate == null)
                {
                    objFleetServiceVM.ServiceOrderLabour.StartDate = DateTime.UtcNow;
                }

                Timecard result = new Timecard();
                string mode = string.Empty;
                if (objFleetServiceVM.ServiceOrderLabour.TimecardId == 0)
                {
                    mode = "add";
                    result = objFleetServiceWrapper.AddLabour(objFleetServiceVM.ServiceOrderLabour);
                }
                else
                {
                    mode = "update";
                    result = objFleetServiceWrapper.EditLabour(objFleetServiceVM.ServiceOrderLabour);
                }
                if (result.ErrorMessages != null && result.ErrorMessages.Count > 0)
                {
                    return Json(result.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var returnObj = new
                    {
                        Result = JsonReturnEnum.success.ToString(),
                        Mode = mode,
                        objFleetServiceVM.ServiceOrderLabour.ServiceOrderLineItemId,
                        objFleetServiceVM.ServiceOrderLabour.ServiceOrderId
                    };
                    return Json(returnObj, JsonRequestBehavior.AllowGet);

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
        public ActionResult DeleteLabour(long TimeCardId)
        {
            FleetServiceWrapper objWrapper = new FleetServiceWrapper(userData);
            var deleteResult = objWrapper.DeleteLabour(TimeCardId);
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

        #region Labour Timer
        [HttpPost]
        public PartialViewResult AddLaborTimer(long ServiceOrderId, long ServiceOrderLineItemId)
        {
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
            ServiceOrderLabourTimerModel objServiceOrderLabour = new ServiceOrderLabourTimerModel();
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);

            //var PersonnelLookUplist = GetList_Personnel();
            //objFleetServiceVM.PersonnelLabourList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null;

            objServiceOrderLabour.ServiceOrderId = ServiceOrderId;
            objServiceOrderLabour.ServiceOrderLineItemId = ServiceOrderLineItemId;
            objFleetServiceVM.ServiceOrderLabourTimer = objServiceOrderLabour;

            var AllLookUps = objCommonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var VMRSWorkAccomplishedList = AllLookUps.Where(x => x.ListName == LookupListConstants.VMRS_WA).ToList();
                if (VMRSWorkAccomplishedList != null && VMRSWorkAccomplishedList.Count > 0)
                {
                    objFleetServiceVM.VMRSWorkAccomplishedList = VMRSWorkAccomplishedList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
            }

            LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return PartialView("_AddLaborTimer", objFleetServiceVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveLaborTimer(FleetServiceVM objFleetServiceVM)
        {
            if (ModelState.IsValid)
            {
                FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
                if (objFleetServiceVM.ServiceOrderLabourTimer.StartDate == null)
                {
                    objFleetServiceVM.ServiceOrderLabourTimer.StartDate = DateTime.UtcNow;
                }

                Timecard result = new Timecard();
                string mode = "add";
                decimal Hours = 0M;
                Hours = Math.Round(Convert.ToInt32(objFleetServiceVM.ServiceOrderLabourTimer.TimeSpan.Hours) +
                          Decimal.Divide(objFleetServiceVM.ServiceOrderLabourTimer.TimeSpan.Minutes, 60) +
                          Decimal.Divide(objFleetServiceVM.ServiceOrderLabourTimer.TimeSpan.Seconds, 60 * 60),2);

                ServiceOrderLabourModel serviceOrderLabourModel = new ServiceOrderLabourModel() {
                    ServiceOrderId = objFleetServiceVM.ServiceOrderLabourTimer.ServiceOrderId,
                    ServiceOrderLineItemId = objFleetServiceVM.ServiceOrderLabourTimer.ServiceOrderLineItemId,
                    VMRSWorkAccomplished = objFleetServiceVM.ServiceOrderLabourTimer.VMRSWorkAccomplished,
                    PersonnelID = userData.DatabaseKey.Personnel.PersonnelId,
                    Hours = Hours,
                    StartDate = DateTime.UtcNow
                };

                result = objFleetServiceWrapper.AddLabour(serviceOrderLabourModel);

                if (result.ErrorMessages != null && result.ErrorMessages.Count > 0)
                {
                    return Json(result.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var returnObj = new
                    {
                        Result = JsonReturnEnum.success.ToString(),
                        Mode = mode,
                        objFleetServiceVM.ServiceOrderLabourTimer.ServiceOrderLineItemId,
                        objFleetServiceVM.ServiceOrderLabourTimer.ServiceOrderId
                    };
                    return Json(returnObj, JsonRequestBehavior.AllowGet);

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

        #region Parts
        [HttpGet]
        public PartialViewResult AddIssueParts(long ServiceOrderId, long ServiceOrderLineItemId, string ClientLookupId)
        {
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            ServiceOrderIssuePartModel objParts = new ServiceOrderIssuePartModel();
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);

            objParts.ClientLookupId = ClientLookupId;
            objParts.ServiceOrderId = ServiceOrderId;
            objParts.ServiceOrderLineItemId = ServiceOrderLineItemId;
            objFleetServiceVM.IssuePartModel = objParts;

            var AllLookUps = objCommonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var VMRSFailureList = AllLookUps.Where(x => x.ListName == LookupListConstants.VMRS_Fail).ToList();
                if (VMRSFailureList != null && VMRSFailureList.Count > 0)
                {
                    objFleetServiceVM.VMRSFailureList = VMRSFailureList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
            }

            LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return PartialView("_AddIssueParts", objFleetServiceVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveIssueParts(FleetServiceVM objFleetServiceVM)
        {
            // For validation , we are using existing ValidateAdd sp
            // requestor id and performed by id value is taken from the personnel list drodown in inventory checkout
            // here logged in user personnel id has been used for both the fields and IssuedTo
            if (ModelState.IsValid)
            {
                FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
                PartHistory result = new PartHistory();
                objFleetServiceVM.IssuePartModel.PartStoreroomId = objFleetServiceWrapper.GetStoreroomId(objFleetServiceVM.IssuePartModel.PartId);
                result = objFleetServiceWrapper.AddIssuePart(objFleetServiceVM.IssuePartModel);

                if (result.ErrorMessages != null && result.ErrorMessages.Count > 0)
                {
                    return Json(result.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var returnObj = new
                    {
                        Result = JsonReturnEnum.success.ToString(),
                        objFleetServiceVM.IssuePartModel.ServiceOrderLineItemId,
                        objFleetServiceVM.IssuePartModel.ServiceOrderId
                    };
                    return Json(returnObj, JsonRequestBehavior.AllowGet);

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

        #region Others
        [HttpGet]
        public PartialViewResult AddEditOthers(long ServiceOrderId, long ServiceOrderLineItemId, long OtherCostsId = 0)
        {
            var SourceTypeList = UtilityFunction.populateSourceList();
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
            ServiceOrderOthers objServiceOrderOthers = new ServiceOrderOthers();

            if (OtherCostsId > 0)
            {
                ViewBag.Mode = "Edit";
                objServiceOrderOthers = objFleetServiceWrapper.RetrieveByOthercostid(OtherCostsId, ServiceOrderId, ServiceOrderLineItemId);
                objServiceOrderOthers.OtherCostsId = OtherCostsId;
            }
            else
            {
                ViewBag.Mode = "Add";
            }

            objServiceOrderOthers.SourceList = SourceTypeList != null ? SourceTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : null;
            objServiceOrderOthers.ServiceOrderId = ServiceOrderId;
            objServiceOrderOthers.ServiceOrderLineItemId = ServiceOrderLineItemId;
            objFleetServiceVM.ServiceOrderOthers = objServiceOrderOthers;

            LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return PartialView("_AddEditOthers", objFleetServiceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveOthers(FleetServiceVM objFleetServiceVM)
        {
            FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
            if (ModelState.IsValid)
            {
                OtherCosts result = new OtherCosts();
                string mode = string.Empty;
                if (objFleetServiceVM.ServiceOrderOthers.OtherCostsId == 0)
                {
                    mode = "add";
                    result = objFleetServiceWrapper.AddOthers(objFleetServiceVM.ServiceOrderOthers);
                }
                else
                {
                    mode = "Edit";
                    result = objFleetServiceWrapper.EditOtherscost(objFleetServiceVM.ServiceOrderOthers);
                }
                if (result.ErrorMessages != null && result.ErrorMessages.Count > 0)
                {
                    return Json(result.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var returnObj = new
                    {
                        Result = JsonReturnEnum.success.ToString(),
                        Mode = mode,
                        objFleetServiceVM.ServiceOrderOthers.ServiceOrderLineItemId,
                        objFleetServiceVM.ServiceOrderOthers.ServiceOrderId
                    };
                    return Json(returnObj, JsonRequestBehavior.AllowGet);

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
        public ActionResult DeleteOther(long OtherCostsId)
        {
            FleetServiceWrapper objWrapper = new FleetServiceWrapper(userData);
            var deleteResult = objWrapper.DeleteOther(OtherCostsId);
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

        #endregion

        #region Export
        [HttpPost]
        public JsonResult SetPrintData(SOPrintParams sOPrintParams)
        {
            Session["PRINTPARAMS"] = sOPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [EncryptedActionParameter]
        public ActionResult ExportASPDF(string d = "")
        {
            FleetServiceWrapper fSWrapper = new FleetServiceWrapper(userData);
            FleetServicePDFPrintModel objFleetServicePrintModel;
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            List<FleetServicePDFPrintModel> fSSearchModelList = new List<FleetServicePDFPrintModel>();
            LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            var locker = new object();

            SOPrintParams fSPrintParams = (SOPrintParams)Session["PRINTPARAMS"];

            List<FleetServiceModel> fSList = fSWrapper.GetFleetServiceGridData(fSPrintParams.CustomQueryDisplayId,fSPrintParams.colname, fSPrintParams.coldir, 0, 100000,
                fSPrintParams.CreateStartDateVw, fSPrintParams.CreateEndDateVw, fSPrintParams.CompleteStartDateVw, fSPrintParams.CompleteEndDateVw, fSPrintParams.personnelList,
               fSPrintParams.AssetID, fSPrintParams.AssetName, fSPrintParams.Description, fSPrintParams.Shift, fSPrintParams.Type, fSPrintParams.VIN, fSPrintParams.searchText);


            foreach (var f in fSList)
            {
                objFleetServicePrintModel = new FleetServicePDFPrintModel();
                objFleetServicePrintModel.ClientLookupId = f.ClientLookupId;
                objFleetServicePrintModel.EquipmentClientLookupId = f.EquipmentClientLookupId;
                objFleetServicePrintModel.AssetName = f.AssetName;
                objFleetServicePrintModel.Status = f.Status;
                objFleetServicePrintModel.Type = f.Type;
                if (f.CreateDate != null && f.CreateDate != default(DateTime))
                {
                    objFleetServicePrintModel.CreateDateString = f.CreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objFleetServicePrintModel.CreateDateString = "";
                }

                objFleetServicePrintModel.Assigned = f.Assigned;
                if (f.ScheduleDate != null && f.ScheduleDate != default(DateTime))
                {
                    objFleetServicePrintModel.ScheduleDateString = f.ScheduleDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objFleetServicePrintModel.ScheduleDateString = "";
                }
                if (f.CompleteDate != null && f.CompleteDate != default(DateTime))
                {
                    objFleetServicePrintModel.CompleteDateString = f.CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objFleetServicePrintModel.CompleteDateString = "";
                }

                if (f.ChildCount > 0)
                {
                    objFleetServicePrintModel.LineItemModelList = fSWrapper.PopulateLineitems(f.ServiceOrderId);
                    objFleetServicePrintModel.Total = objFleetServicePrintModel.LineItemModelList.Sum(x => x.Total);
                }
                lock (locker)
                {
                    fSSearchModelList.Add(objFleetServicePrintModel);
                }
            }
            objFleetServiceVM.FleetServicePDFPrintModel = fSSearchModelList;
            objFleetServiceVM.tableHaederProps = fSPrintParams.tableHaederProps;
            if (d == "d")
            {
                return new PartialViewAsPdf("FSGirdPdfPrintTemplate", objFleetServiceVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    FileName = "Service Orders.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new ViewAsPdf("FSGirdPdfPrintTemplate", objFleetServiceVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }

        }

        [HttpGet]
        public string GetFSPrintData(string colname, string coldir, int CustomQueryDisplayId = 3, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, DateTime? CompleteStartDateVw = null, DateTime? CompleteEndDateVw = null, string personnelList = "",
                       string AssetID = "", string AssetName = "", string Description = "", string Shift = "", string Type = "", string VIN = "", string searchText = "")
        {
            List<FleetServicePrintModel> fsSearchModelList = new List<FleetServicePrintModel>();
            FleetServicePrintModel objFleetServicePrintModel;
            FleetServiceWrapper fSWrapper = new FleetServiceWrapper(userData);
            List<FleetServiceModel> fSList = fSWrapper.GetFleetServiceGridData(CustomQueryDisplayId,colname, coldir, 0, 100000, CreateStartDateVw, CreateEndDateVw, CompleteStartDateVw, CompleteEndDateVw, personnelList, AssetID, AssetName, Description, Shift, Type, VIN, searchText);
            foreach (var fs in fSList)
            {
                objFleetServicePrintModel = new FleetServicePrintModel();
                objFleetServicePrintModel.ClientLookupId = fs.ClientLookupId;
                objFleetServicePrintModel.EquipmentClientLookupId = fs.EquipmentClientLookupId;
                objFleetServicePrintModel.AssetName = fs.AssetName;
                objFleetServicePrintModel.Status = fs.Status;
                objFleetServicePrintModel.Type = fs.Type;
                objFleetServicePrintModel.CreateDate = fs.CreateDate != null ? fs.CreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
                objFleetServicePrintModel.Assigned = fs.Assigned;
                objFleetServicePrintModel.ScheduleDate = fs.ScheduleDate != null ? fs.ScheduleDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
                objFleetServicePrintModel.CompleteDate = fs.CompleteDate != null ? fs.CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
                fsSearchModelList.Add(objFleetServicePrintModel);
            }
            return JsonConvert.SerializeObject(new { data = fsSearchModelList }, JsonSerializerDateSettings);
        }
        #endregion

        #region ServiceOrder Add/Edit
        public PartialViewResult FleetServiceAddOrEdit(long FleetServiceId)
        {
            FleetServiceWrapper fltServiceWrapper = new FleetServiceWrapper(userData);
            FleetServiceVM fltServiceVM = new FleetServiceVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> ShiftList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                ShiftList = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                TypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.SO_TYPE).ToList();
                if (ShiftList != null)
                {
                    fltServiceVM.LookupShiftList = ShiftList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
                if (TypeList != null)
                {
                    fltServiceVM.LookupTypeList = TypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }

            }
            fltServiceVM.FleetServiceModel = new FleetServiceModel();
            fltServiceVM.FleetServiceModel.Pagetype = "Add";
            if (FleetServiceId != 0)
            {
                fltServiceVM.FleetServiceModel = fltServiceWrapper.RetrieveByServiceOrderId(FleetServiceId);
                fltServiceVM.FleetServiceModel.Pagetype = "Edit";
            }

            LocalizeControls(fltServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return PartialView("~/Views/FleetService/_FleetServiceAddOrEdit.cshtml", fltServiceVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FleetServiceOrderAddOrEdit(FleetServiceVM objFS, string Command)
        {
            List<string> ErrorList = new List<string>();
            FleetServiceWrapper fSWrapper = new FleetServiceWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                if (objFS.FleetServiceModel.ServiceOrderId > 0)
                {
                    Mode = "Edit";
                }
                else
                {
                    Mode = "Add";
                }
                ServiceOrder fleetservice = new ServiceOrder();
                string Equip_ClientLookupId = objFS.FleetServiceModel.EquipmentClientLookupId.ToUpper().Trim();
                fleetservice = fSWrapper.AddOrEditFleetServiceOrder(Equip_ClientLookupId, objFS);
                if (fleetservice.ErrorMessages != null && fleetservice.ErrorMessages.Count > 0)
                {
                    return Json(fleetservice.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, Mode = Mode, ServiceOrderId = fleetservice.ServiceOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Search View
        private void PopulatePopUp(out IEnumerable<SelectListItem> AssignedList, out IEnumerable<SelectListItem> PersonnelList, long ServiceOrderId = 0)
        {
            FleetServiceWrapper fsWrapper = new FleetServiceWrapper(userData);
            var totalList = fsWrapper.SOSchedulePersonnelList(Convert.ToString(ServiceOrderId));
            AssignedList = totalList[1].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            PersonnelList = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
        }
        #endregion

        #region Event Log
        [HttpPost]
        public PartialViewResult LoadActivity(long ServiceOrderId)
        {
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            FleetServiceWrapper objFleetServiceWrapper = new FleetServiceWrapper(userData);
            List<EventLogModel> EventLogList = objFleetServiceWrapper.PopulateEventLog(ServiceOrderId);
            objFleetServiceVM.EventLogList = EventLogList;
            //LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("_ActivityLog", objFleetServiceVM);
        }
        #endregion

        #region Comments
        [HttpPost]
        public PartialViewResult LoadComments(long ServiceOrderId)
        {
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Personnel> personnelsList = new List<Personnel>();
            List<Notes> NotesList = new List<Notes>();
            UserMentionData userMentionData;
            List<UserMentionData> userMentionDataList = new List<UserMentionData>();

            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => personnelsList = coWrapper.MentionList(""));
            tasks[1] = Task.Factory.StartNew(() => NotesList = coWrapper.PopulateComment(ServiceOrderId, "ServiceOrder"));
            Task.WaitAll(tasks);

            if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {
                foreach (var myuseritem in personnelsList)
                {
                    userMentionData = new UserMentionData();
                    userMentionData.id = myuseritem.UserName;
                    userMentionData.name = myuseritem.FullName;
                    userMentionData.type = myuseritem.PersonnelInitial;
                    userMentionDataList.Add(userMentionData);
                }
                objFleetServiceVM.userMentionData = userMentionDataList;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                objFleetServiceVM.NotesList = NotesList;
            }
            LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return PartialView("_CommentsList", objFleetServiceVM);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddComments(long ServiceOrderId, string content, string ClientLookupId, List<string> userList, long noteId = 0, long updatedindex = 0)
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
            notesModel.ObjectId = ServiceOrderId;
            notesModel.Content = content;
            notesModel.NotesId = noteId;
            notesModel.ClientLookupId = ClientLookupId;
            notesModel.updatedindex = updatedindex;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateComment(notesModel, ref Mode, "ServiceOrder", userMentionDataList);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), ServiceOrderId = ServiceOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Private method
        private FleetServiceSummaryModel GetFleetServiceSummary(long EquipmentId, string ClientLookupId, string Name, string Meter1Type, decimal Meter1CurrentReading, string Meter2Type, decimal Meter2CurrentReading, DateTime? ScheduleDate, string Assigned, DateTime? CompleteDate, long ServiceOrderId, long Assigned_personnalid, string Status, string EquipmentClientLookupId)
        {
            //int Flag = 1;
            //long thisCount = 0;
            FleetServiceSummaryModel summary = new FleetServiceSummaryModel();

            summary.ServiceOrder_ClientLookupId = ClientLookupId;
            summary.EquipmentName = Name;
            summary.Meter1Type = Meter1Type;
            summary.Meter1CurrentReading = Meter1CurrentReading;
            summary.Meter2Type = Meter2Type;
            summary.Meter2CurrentReading = Meter2CurrentReading;
            summary.Meter1Type = Meter1Type;
            summary.ScheduleDate = ScheduleDate;
            summary.Assigned = Assigned;
            summary.CompleteDate = CompleteDate;
            summary.ServiceOrderId = ServiceOrderId;
            summary.SOAssigned_PersonnelId = Assigned_personnalid;
            summary.Status = Status;
            summary.Equipment_ClientLookupId = EquipmentClientLookupId;

            FleetServiceWrapper faWrapper = new FleetServiceWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            summary.ImageUrl = comWrapper.GetAzureImageUrl(ServiceOrderId, AttachmentTableConstant.ServiceOrder);
            summary.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (summary.ClientOnPremise)
            {
                summary.ImageUrl = comWrapper.GetAzureImageUrl(ServiceOrderId, AttachmentTableConstant.ServiceOrder);
            }
            else
            {
                summary.ImageUrl = comWrapper.GetOnPremiseImageUrl(ServiceOrderId, AttachmentTableConstant.ServiceOrder);
            }
            return summary;
        }
        #endregion

        #region Attachment side menu

        [HttpPost]
        public string PopulateAttachment(int? draw, int? start, int? length, long ServiceOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AttachmentList = objCommonWrapper.PopulateAttachments(ServiceOrderId, "ServiceOrder", userData.Security.Fleet_ServiceOrder.Edit);
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
        [HttpGet]
        public ActionResult ShowAddAttachment(long EquipmentId, string ClientLookupId, string Name, string Meter1Type, decimal Meter1CurrentReading, string Meter2Type, decimal Meter2CurrentReading, string ScheduleDate, string Assigned, string CompleteDate,
            long ServiceOrderId, long Assign_PersonnelId, string Meter1Units, string Meter2Units, double Meter1DayDiff, double Meter2DayDiff, string EquipmentClientLookupId, string Status)
        {

            FleetServiceVM _FleetServiceObj = new FleetServiceVM();
            FleetServiceWrapper FAWrapper = new FleetServiceWrapper(userData);
            ServiceOrderScheduleModel soScheduleModel = new ServiceOrderScheduleModel();
            CompleteServiceOrderModel objCompleteServiceOrderModel = new CompleteServiceOrderModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            DateTime? ScheduleDate_ = null;
            DateTime? CompleteDate_ = null;
            if (!string.IsNullOrEmpty(ScheduleDate))
            {
                ScheduleDate_ = DateTime.Parse(ScheduleDate);
            }
            if (!string.IsNullOrEmpty(CompleteDate))
            {
                CompleteDate_ = DateTime.Parse(CompleteDate);
            }
            FleetServiceSummaryModel smry = new FleetServiceSummaryModel();
            smry = GetFleetServiceSummary(EquipmentId, ClientLookupId, Name, Meter1Type, Meter1CurrentReading, Meter2Type, Meter2CurrentReading, ScheduleDate_, Assigned, CompleteDate_, ServiceOrderId, Assign_PersonnelId, Status, EquipmentClientLookupId);
            smry.Meter1Units = Meter1Units;
            smry.Meter2Units = Meter2Units;
            smry.Meter1DayDiff = Meter1DayDiff;
            smry.Meter2DayDiff = Meter2DayDiff;
            smry.IsDetail = false;
            _FleetServiceObj._FleetServiceSummaryModel = smry;

            AttachmentModel Attachment = new AttachmentModel();
            Attachment.EquipmentId = EquipmentId;
            Attachment.ClientLookupId = ClientLookupId;
            Attachment.ServiceOrderId = ServiceOrderId;
            _FleetServiceObj.attachmentModel = Attachment;
            _FleetServiceObj._userdata = userData;
            _FleetServiceObj.security = this.userData.Security;
            LocalizeControls(_FleetServiceObj, LocalizeResourceSetConstants.FleetServiceOrder);
            return PartialView("_AttachmentAdd", _FleetServiceObj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttachment()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                Stream stream = Request.Files[0].InputStream;
                var fileName = Request.Files[0].FileName;
                AttachmentModel attachmentModel = new AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();

                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);
                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.ServiceOrderId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = "ServiceOrder";
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.Equipment.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.Equipment.Edit);
                }
                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), equipmentid = Convert.ToInt64(Request.Form["attachmentModel.ServiceOrderId"]) }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteAttachment(long _fileAttachmentId)
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

        #endregion

        #region Photos Side Menu

        public JsonResult DeleteImageFromAzure(string _ServiceOrderId, string TableName, bool Profile, bool Image)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteAzureImage(Convert.ToInt64(_ServiceOrderId), AttachmentTableConstant.ServiceOrder, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteImageFromOnPremise(string _ServiceOrderId, string TableName, bool Profile, bool Image)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteOnPremiseImage(Convert.ToInt64(_ServiceOrderId), AttachmentTableConstant.ServiceOrder, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Action Menu

        #region Add/Remove Schedule
        public ActionResult RemoveScheduleSO(long ServiceOrderId)
        {
            FleetServiceWrapper fsWrapper = new FleetServiceWrapper(userData);
            ServiceOrderScheduleModel objServiceOrderSchedule = new ServiceOrderScheduleModel();
            objServiceOrderSchedule.ServiceOrderId = ServiceOrderId;
            string Statusmsg = string.Empty;
            string StatusType = "Remove";
            var So = fsWrapper.AddRemoveScheduleRecord(objServiceOrderSchedule, ref Statusmsg, StatusType);
            return Json(new { data = Statusmsg, serviceorderId = So.ServiceOrderId, error = So.ErrorMessages }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PopulatePopUpJs(long ServiceOrderId = 0)
        {
            FleetServiceVM objServiceOrderVM = new FleetServiceVM();
            IEnumerable<SelectListItem> AssignedList;
            IEnumerable<SelectListItem> PersonnelList;
            PopulatePopUp(out AssignedList, out PersonnelList, ServiceOrderId);
            objServiceOrderVM.AssignedList = AssignedList;
            objServiceOrderVM.PersonnelList = PersonnelList;
            return Json(new { AssignedList = objServiceOrderVM.AssignedList, PersonnelList = objServiceOrderVM.PersonnelList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddSchedule(FleetServiceVM fsVM)
        {
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            FleetServiceWrapper fsWrapper = new FleetServiceWrapper(userData);
            FleetServiceModel fsModel = new FleetServiceModel();
            string Statusmsg = string.Empty;
            string StatusType = "Add";
            System.Text.StringBuilder PersonnelWoList = new System.Text.StringBuilder();
            if (string.IsNullOrEmpty(fsVM.soScheduleModel.ServiceOrderIds))
            {
                var objserviceOrder = fsWrapper.AddRemoveScheduleRecord(fsVM.soScheduleModel, ref Statusmsg, StatusType);
                if (objserviceOrder.ErrorMessages != null && objserviceOrder.ErrorMessages.Count > 0)
                {
                    return Json(objserviceOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), serviceorderid = fsVM.soScheduleModel.ServiceOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string[] serviceorderIds = fsVM.soScheduleModel.ServiceOrderIds.Split(',');
                string[] clientLookupIds = fsVM.soScheduleModel.ClientLookupIds.Split(',');
                string[] status = fsVM.soScheduleModel.Status.Split(',');
                List<BatchCompleteResultModel> WoBatchList = new List<BatchCompleteResultModel>();
                List<string> errorMessage = new List<string>();
                System.Text.StringBuilder failedSoList = new System.Text.StringBuilder();
                for (int i = 0; i < serviceorderIds.Length; i++)
                {
                    if (status[i] == ServiceOrderStatusConstant.Open || status[i] == ServiceOrderStatusConstant.Scheduled)
                    {
                        fsVM.soScheduleModel.ServiceOrderId = Convert.ToInt64(serviceorderIds[i]);
                        var objServiceOrder = fsWrapper.AddRemoveScheduleRecord(fsVM.soScheduleModel, ref Statusmsg, StatusType);
                        if (objServiceOrder.ErrorMessages != null && objServiceOrder.ErrorMessages.Count > 0)
                        {
                            string errormessage = "Failed to schedule " + objServiceOrder.ClientLookupId + ": " + objServiceOrder.ErrorMessages[0];
                            errorMessage.Add(errormessage);
                        }
                    }
                    else
                    {
                        failedSoList.Append(clientLookupIds[i] + ",");
                    }
                }
                if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedSoList.ToString()))
                {
                    if (!string.IsNullOrEmpty(failedSoList.ToString()))
                    {
                        errorMessage.Add("Service Order(s) " + failedSoList + " can't be scheduled. Please check the status.");
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

        #region Cancel Job
        [HttpPost]
        public JsonResult CancelJob(FleetServiceVM fleetServiceVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                string result = string.Empty;
                long ServiceorderId = fleetServiceVM.CancelServiceOrderModal.ServiceOrderId;
                string CancelReason = fleetServiceVM.CancelServiceOrderModal.CancelReasonSo;
                FleetServiceWrapper fsWrapper = new FleetServiceWrapper(userData);
                ServiceOrder Sojob = fsWrapper.CancelJob(ServiceorderId, CancelReason);
                if (Sojob.ErrorMessages != null && Sojob.ErrorMessages.Count > 0)
                {
                    return Json(new { data = Sojob.ErrorMessages }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = JsonReturnEnum.success.ToString();
                }
                return Json(new { data = result, ServiceorderId = ServiceorderId }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Reopen 
        public JsonResult ReopenJob(long ServiceorderId, string status, string clientLookupId = "")
        {
            string result = string.Empty;
            FleetServiceWrapper fsWrapper = new FleetServiceWrapper(userData);
            ServiceOrder objServiceOrder = new ServiceOrder();
            List<string> errorMessage = new List<string>();
            System.Text.StringBuilder failedSoList = new System.Text.StringBuilder();
            if (status == ServiceOrderStatusConstant.Complete || status == ServiceOrderStatusConstant.Canceled)
            {
                objServiceOrder = fsWrapper.ReopenJob(ServiceorderId);
                if (objServiceOrder.ErrorMessages != null && objServiceOrder.ErrorMessages.Count > 0)
                {
                    string errormessage = "Failed to Reopen " + objServiceOrder.ClientLookupId + ": " + objServiceOrder.ErrorMessages[0];
                    errorMessage.Add(errormessage);
                }
            }
            else
            {
                failedSoList.Append(clientLookupId);
            }

            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedSoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedSoList.ToString()))
                {
                    errorMessage.Add("Service Order(s) " + failedSoList + " can't be Reopen. Please check the status.");
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Complete
        [HttpPost]
        public JsonResult CompleteServiceOrder(FleetServiceVM fleetServiceVM)
        {
            FleetServiceWrapper fleetServiceWrapper = new FleetServiceWrapper(userData);
            List<string> ErrorList = new List<string>();
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                ErrorList = fleetServiceWrapper.CompleteFleetService(fleetServiceVM.CompleteServiceOrderModel);
                if (ErrorList != null && ErrorList.Count > 0)
                {
                    return Json(ErrorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
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

        #region Hover for Assigned user
        [HttpPost]
        public JsonResult PopulateHover(long ServiceOrderId = 0)
        {
            FleetServiceWrapper fsWrapper = new FleetServiceWrapper(userData);
            string personnelList = fsWrapper.RetrievePersonnelInitial(ServiceOrderId);
            if (!string.IsNullOrEmpty(personnelList))
            {
                personnelList = personnelList.Trim().TrimEnd(',');
            }
            return Json(new { personnelList = personnelList }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Service order history
        [HttpPost]
        public string ServiceOrderHistoryData(int? draw, int? start, int? length, long AssetID = 0, long ServiceOrderId = 0,
            string ClientLookupId = "", string AssetClientLookupId = "", string AssetName = "", string Status = "", string Type = "", string CreatedDate = "", string CompletedDate = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            FleetServiceWrapper fltServiceWrapper = new FleetServiceWrapper(userData);
            List<FleetServiceModel> fleetServiceList = fltServiceWrapper.ServiceOrderHistory(order, orderDir, skip, length ?? 0, AssetID, ServiceOrderId, ClientLookupId, AssetClientLookupId, AssetName, Status, Type, CreatedDate, CompletedDate);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (fleetServiceList != null && fleetServiceList.Count > 0)
            {
                recordsFiltered = fleetServiceList[0].TotalCount;
                totalRecords = fleetServiceList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = fleetServiceList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        public ActionResult GetServiceOrderHistoryInnerGrid(long ServiceOrderID)
        {
            FleetServiceVM objFleetServiceVM = new FleetServiceVM();
            FleetServiceWrapper FSWrapper = new FleetServiceWrapper(userData);
            objFleetServiceVM.FleetServiceLineItemModelList = FSWrapper.PopulateLineitems(ServiceOrderID);
            LocalizeControls(objFleetServiceVM, LocalizeResourceSetConstants.FleetServiceOrder);
            return View("_InnerGridSOHistoryLineItem", objFleetServiceVM);
        }

        #endregion

        #region Redirect Details From Fleet Asset
        public RedirectResult DetailFromFleetAsset(long ServiceOrderId)
        {
            TempData["Mode"] = "DetailFromFleetAsset";
            string SOString = Convert.ToString(ServiceOrderId);
            TempData["ServiceOrderId"] = SOString;
            return Redirect("/FleetService/Index?page=Fleet_Service_Orders");
        }
        #endregion

        #region Scheduled service
        //V2-421
        [HttpPost]
        public string ScheduledServiceGridData(int? draw, int? start, int? length, long AssetID = 0, string ServiceTaskDesc = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            FleetServiceWrapper fltServiceWrapper = new FleetServiceWrapper(userData);
            List<FleetScheduledServiceSearchModel> fleetServiceList = fltServiceWrapper.RetrieveScheduledServiceByAssetId(order, orderDir, skip, length ?? 0, AssetID, ServiceTaskDesc);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (fleetServiceList != null && fleetServiceList.Count > 0)
            {
                recordsFiltered = fleetServiceList[0].TotalCount;
                totalRecords = fleetServiceList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = fleetServiceList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        [HttpPost]
        public JsonResult BulkLineItemAdd(string[] ScheduledServiceIds, long ServiceOrderId)
        {
            string ModelValidationFailedMessage = string.Empty;
            int updatedItemCount = 0;
            if (ModelState.IsValid)
            {
                FleetServiceWrapper fsWrapper = new FleetServiceWrapper(userData);
                var objError = fsWrapper.BulkLineItemAdd(ScheduledServiceIds, ServiceOrderId);

                if (objError != null && objError.Count > 0)
                {
                    return Json(objError, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (ScheduledServiceIds.Length > 0)
                    {
                        updatedItemCount = ScheduledServiceIds.Length;
                    }
                    return Json(new { Result = JsonReturnEnum.success.ToString(), UpdatedItemCount = updatedItemCount }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}