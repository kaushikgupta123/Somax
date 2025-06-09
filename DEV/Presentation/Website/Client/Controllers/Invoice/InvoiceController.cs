using Client.BusinessWrapper.Invoice;
using Client.Models.Invoice;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using Client.Common;
using Client.BusinessWrapper.Common;
using Client.Controllers.Common;
using Client.ActionFilters;
using System.Threading.Tasks;
using System.Web;



namespace Client.Controllers.Invoice
{
    public class InvoiceController : SomaxBaseController
    {
        public ActionResult Add()
        {
            TempData["Mode"] = "add";
            return Redirect("/Invoice/Index?page=Invoice");
        }

        #region Invoice_search
        [CheckUserSecurity(securityType = SecurityConstants.InvoiceMatching)]
        public ActionResult Index()
        {
            InvoiceVM objInvoVM = new InvoiceVM();
            InvoiceMatchHeaderModel invoModel = new InvoiceMatchHeaderModel();
            InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            ////---V2-389---------
            var v = commonWrapper.GetListFromConstVals("InvoiceMatchStatus");
            objInvoVM.StatusList = v.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            ////------------------
            string mode = Convert.ToString(TempData["Mode"]);
            if (mode == "add")
            {
                //V2-1061
                var typelist = commonWrapper.GetListFromConstVals("InvoiceMatchType");
                invoModel.TypeList = typelist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).ToList();
                ViewBag.IsInvoiceAdd = true;
                objInvoVM.InvoiceMatchHeaderModel = invoModel;
            }
            if (mode != "add")
            {
                //objInvoVM.scheduleInvoiceList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.InvoiceMatchHeader, true);//V2-373
                objInvoVM.scheduleInvoiceList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.InvoiceMatchHeader, false);//V2-373
                objInvoVM.DateRangeDropList = UtilityFunction.GetTimeRangeDropFORIHMATP().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-373
                objInvoVM.DatePaidRangeDropList = UtilityFunction.GetTimeRangeDropFORIHMPAID().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-373
            }
            invoModel.DateRangeDropListForInvoiceCreatedate = UtilityFunction.GetTimeRangeDropForInvoiceMatching().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });  //V2-1061
            objInvoVM.security = userData.Security;
            objInvoVM.udata = userData;
            objInvoVM.InvoiceMatchHeaderModel= invoModel;
            LocalizeControls(objInvoVM, LocalizeResourceSetConstants.InvoiceDetails);
            return View(objInvoVM);
        }
        private List<InvoiceMatchHeaderModel> GetInvoiceSearchResult(List<InvoiceMatchHeaderModel> invoMasterList, string invoice = "", string status = "", string vendor = "",
            string vendorname = "", DateTime? receiptdate = null, string purchaseorder = "", DateTime? invoicedate = null, string txtSearchval = "")
        {
            if (!string.IsNullOrEmpty(invoice))
            {
                invoice = invoice.ToUpper();
                invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(invoice))).ToList();
            }
            if (!string.IsNullOrEmpty(status))
            {
                status = status.ToUpper();
                invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Status) && x.Status.ToUpper().Equals(status))).ToList();
            }
            if (!string.IsNullOrEmpty(vendor))
            {
                vendor = vendor.ToUpper();
                invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorClientLookupId) && x.VendorClientLookupId.ToUpper().Contains(vendor))).ToList();
            }
            if (!string.IsNullOrEmpty(vendorname))
            {
                vendorname = vendorname.ToUpper();
                invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorName) && x.VendorName.ToUpper().Contains(vendorname))).ToList();
            }
            if (receiptdate != null)
            {
                invoMasterList = invoMasterList.Where(x => (x.ReceiptDate != null && x.ReceiptDate.Value.Date.Equals(receiptdate.Value.Date))).ToList();
            }
            if (!string.IsNullOrEmpty(purchaseorder))
            {
                purchaseorder = purchaseorder.ToUpper();
                invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.POClientLookUpId) && x.POClientLookUpId.ToUpper().Contains(purchaseorder))).ToList();
            }
            if (invoicedate != null)
            {
                invoMasterList = invoMasterList.Where(x => (x.InvoiceDate != null && x.InvoiceDate.Value.Date.Equals(invoicedate.Value.Date))).ToList();
            }
            return invoMasterList;
        }
        //V2-373
        [HttpGet]
        public string GetINVPrintData(string colname, string coldir, int CustomQueryDisplayId = 0, DateTime? CompleteATPStartDateVw = null,
            DateTime? CompleteATPEndDateVw = null, DateTime? CompletePStartDateVw = null, DateTime? CompletePEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, string invoice = "", string status = "",
            string vendor = "", string vendorname = "", DateTime? receiptdate = null, string purchaseorder = "", DateTime? invoicedate = null, string txtSearchval = "")
        {
            InvoicePrintModel InvPrintModel;
            InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
            List<InvoicePrintModel> pSearchModelList = new List<InvoicePrintModel>();
            var InvList = invoWrapper.RetrieveINVGridChunkSearch(CustomQueryDisplayId, CompleteATPStartDateVw, CompleteATPEndDateVw, CompletePStartDateVw, CompletePEndDateVw, CreateStartDateVw, CreateEndDateVw, 0, 100000, colname, coldir, invoice, status, vendor, vendorname, receiptdate, purchaseorder, invoicedate, txtSearchval);//V2-373
            foreach (var p in InvList)
            {
                InvPrintModel = new InvoicePrintModel();
                InvPrintModel.ClientLookupId = p.ClientLookupId;
                InvPrintModel.Status = p.Status;
                InvPrintModel.VendorClientLookupId = p.VendorClientLookupId;
                InvPrintModel.VendorName = p.VendorName;
                InvPrintModel.ReceiptDate = p.ReceiptDate;
                InvPrintModel.InvoiceDate = p.InvoiceDate;
                InvPrintModel.POClientLookupId = p.POClientLookUpId;
                pSearchModelList.Add(InvPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = pSearchModelList }, JsonSerializerDateSettings);
        }
        public string GetInvoiceMaintGrid(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, DateTime? CompleteATPStartDateVw = null,
            DateTime? CompleteATPEndDateVw = null, DateTime? CompletePStartDateVw = null, DateTime? CompletePEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, string invoice = "", string status = "", string vendor = "", string vendorname = "", DateTime? receiptdate = null, string purchaseorder = "", DateTime? invoicedate = null, string txtSearchval = "",
            string Order = "1"//, string orderDir = "asc")
            )//Invoice Match Sorting
        //V2-373
        {
            start = start.HasValue
       ? start / length
       : 0;
            int skip = start * length ?? 0;
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
            var invoMasterList = invoWrapper.RetrieveINVGridChunkSearch(CustomQueryDisplayId, CompleteATPStartDateVw, CompleteATPEndDateVw, CompletePStartDateVw, CompletePEndDateVw, CreateStartDateVw, CreateEndDateVw, skip, length ?? 0, Order, orderDir, invoice, status, vendor, vendorname, receiptdate, purchaseorder, invoicedate, txtSearchval);//V2-373
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (invoMasterList != null && invoMasterList.Count > 0)
            {
                recordsFiltered = invoMasterList[0].TotalCount;
                totalRecords = invoMasterList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = invoMasterList
              .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        public ActionResult GetInvInnerGrid(long invoiceMatchHeaderId)
        {
            InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
            InvoiceVM objInvoVM = new InvoiceVM();
            var invoMasterList = invoWrapper.RetrieveMatchItemList(Convert.ToInt32(invoiceMatchHeaderId));
            objInvoVM.InvoiceMatchItemModelList = invoMasterList;
            LocalizeControls(objInvoVM, LocalizeResourceSetConstants.InvoiceDetails);
            return View("_InnerGridInvLineItem", objInvoVM);
        }
        private List<InvoiceMatchHeaderModel> GetAllInvoiceSortByColumnWithOrder(string order, string orderDir, List<InvoiceMatchHeaderModel> data)
        {
            List<InvoiceMatchHeaderModel> lst = new List<InvoiceMatchHeaderModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorClientLookupId).ToList() : data.OrderBy(p => p.VendorClientLookupId).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorName).ToList() : data.OrderBy(p => p.VendorName).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReceiptDate).ToList() : data.OrderBy(p => p.ReceiptDate).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.POClientLookUpId).ToList() : data.OrderBy(p => p.POClientLookUpId).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.InvoiceDate).ToList() : data.OrderBy(p => p.InvoiceDate).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }
        public string GetInvoiceLineItemsGrid(int? draw, int? start, int? length, long? invoiceMatchHeaderId, Int64? linenumber, decimal? quantity, decimal? unitcost, decimal? totalcost, string account = "", string srcData = "", string description = "", string unitofmeasure = "", string purchaseOrder = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
            var invoMasterList = invoWrapper.RetrieveMatchItemList(Convert.ToInt32(invoiceMatchHeaderId));
            invoMasterList = this.GetAllInvoiceMatchItemSortByColumnWithOrder(order, orderDir, invoMasterList);
            if (invoMasterList != null && !string.IsNullOrEmpty(srcData))
            {
                srcData = srcData.ToUpper();
                decimal val;
                bool outval = decimal.TryParse(srcData, out val);
                invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(Convert.ToString(x.LineNumber)) && x.LineNumber.ToString() == srcData)
                                                        || (outval == true && x.Quantity.Equals(val))
                                                        || (outval == true && x.UnitCost.Equals(val))
                                                        || (outval == true && x.TotalCost.Equals(val))
                                                        || (!string.IsNullOrWhiteSpace(Convert.ToString(x.Account)) && Convert.ToString(x.Account).ToUpper().Contains(srcData))
                                                        || (!string.IsNullOrWhiteSpace(Convert.ToString(x.Description)) && Convert.ToString(x.Description).ToUpper().Contains(srcData))
                                                        || (!string.IsNullOrWhiteSpace(Convert.ToString(x.UnitOfMeasure)) && Convert.ToString(x.UnitOfMeasure).ToUpper().Contains(srcData))
                                                        || (!string.IsNullOrWhiteSpace(Convert.ToString(x.PurchaseOrder)) && x.PurchaseOrder.ToString() == srcData)
                                                        ).ToList();
            }

            if (invoMasterList != null)
            {
                if (linenumber != null)
                {
                    invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(Convert.ToString(x.LineNumber)) && Convert.ToString(x.LineNumber).ToUpper().Contains(Convert.ToString(linenumber)))).ToList();
                }
                if (quantity != null)
                {
                    invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(Convert.ToString(x.Quantity)) && x.Quantity == quantity)).ToList();
                }
                if (unitcost != null)
                {
                    invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(Convert.ToString(x.UnitCost)) && x.UnitCost == unitcost)).ToList();
                }
                if (totalcost != null)
                {
                    invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(Convert.ToString(x.TotalCost)) && x.TotalCost == totalcost)).ToList();
                }
                if (!string.IsNullOrEmpty(account))
                {
                    account = account.ToUpper();
                    invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Account) && x.Account.Contains(account))).ToList();
                }
                if (!string.IsNullOrEmpty(description))
                {
                    description = description.ToUpper();
                    invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(description))).ToList();
                }
                if (!string.IsNullOrEmpty(unitofmeasure))
                {
                    unitofmeasure = unitofmeasure.ToUpper();
                    invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.UnitOfMeasure) && x.UnitOfMeasure.ToUpper().Contains(unitofmeasure))).ToList();
                }
                if (!string.IsNullOrEmpty(purchaseOrder))
                {
                    purchaseOrder = purchaseOrder.ToUpper();
                    invoMasterList = invoMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.PurchaseOrder) && x.PurchaseOrder.ToUpper().Equals(purchaseOrder))).ToList();
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = invoMasterList.Count();
            totalRecords = invoMasterList.Count();
            int initialPage = start.Value;
            var filteredResult = invoMasterList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult });
        }
        private List<InvoiceMatchItemModel> GetAllInvoiceMatchItemSortByColumnWithOrder(string order, string orderDir, List<InvoiceMatchItemModel> data)
        {
            List<InvoiceMatchItemModel> lst = new List<InvoiceMatchItemModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LineNumber).ToList() : data.OrderBy(p => p.LineNumber).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitOfMeasure).ToList() : data.OrderBy(p => p.UnitOfMeasure).ToList();

                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PurchaseOrder).ToList() : data.OrderBy(p => p.PurchaseOrder).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Account).ToList() : data.OrderBy(p => p.Account).ToList();
                    break;
            }
            return lst;
        }
        public PartialViewResult EditInvoiceListItem(long invoiceMatchItemId, long InvoiceId, string ClientLookupId)
        {
            InvoiceVM vm = new InvoiceVM();
            List<DataContracts.LookupList> UoMeasure = new List<DataContracts.LookupList>();
            InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                UoMeasure = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            }
            InvoiceMatchItemModel woMaintMasterList = invoWrapper.RetrieveMatchItemDetails(invoiceMatchItemId);
            vm.InvoiceMatchItemModel = woMaintMasterList;
            var Account = invoWrapper.GetAccountList();
            if (Account != null)
            {
                vm.InvoiceMatchItemModel.AccountList = Account.Select(x => new SelectListItem { Text = x.Name, Value = x.AccountId.ToString() });
            }
            if (UoMeasure != null)
            {
                vm.InvoiceMatchItemModel.UnitOfMeasureList = UoMeasure.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
            }
            InvoiceMatchHeaderModel InvHeaderModel = new InvoiceMatchHeaderModel();
            InvHeaderModel.ClientLookupId = ClientLookupId;
            vm.InvoiceMatchHeaderModel = InvHeaderModel;
            LocalizeControls(vm, LocalizeResourceSetConstants.InvoiceDetails);
            return PartialView("~/Views/Invoice/_InvoiceWorkListEdit.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditInvoiceReceipt(InvoiceVM invoVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
                DataContracts.InvoiceMatchItem objUpdate = new DataContracts.InvoiceMatchItem();
                objUpdate = invoWrapper.LineItemRowUpdating(invoVM.InvoiceMatchItemModel);
                if (objUpdate.ErrorMessages != null && objUpdate.ErrorMessages.Count > 0)
                {
                    return Json(objUpdate.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), InvoiceMatchHeaderId = objUpdate.InvoiceMatchHeaderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteInvoiceitem(long MatchHeaderId, long MatchItemId)
        {
            InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
            List<String> errorList = new List<string>();
            DataContracts.InvoiceMatchItem objUpdate = new DataContracts.InvoiceMatchItem();
            errorList = invoWrapper.DeleteRowItem(MatchHeaderId, MatchItemId);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(errorList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult AddInvoiceReceipt(long invoiceMatchItemId, string ClientLookupId)
        {
            InvoiceVM vm = new InvoiceVM();
            List<DataContracts.LookupList> UoMeasure = new List<DataContracts.LookupList>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var ChildObj = new InvoiceMatchItemModel();
            vm.InvoiceMatchItemModel = ChildObj;
            InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                UoMeasure = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (UoMeasure != null)
                {
                    vm.InvoiceMatchItemModel.UnitOfMeasureList = UoMeasure.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                }
            }
            var Account = invoWrapper.GetAccountList();
            if (Account != null)
            {
                vm.InvoiceMatchItemModel.AccountList = Account.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            }
            vm.InvoiceMatchItemModel.InvoiceMatchHeaderId = invoiceMatchItemId;
            InvoiceMatchHeaderModel InvHeaderModel = new InvoiceMatchHeaderModel();
            InvHeaderModel.ClientLookupId = ClientLookupId;
            vm.InvoiceMatchHeaderModel = InvHeaderModel;
            LocalizeControls(vm, LocalizeResourceSetConstants.InvoiceDetails);
            return PartialView("~/Views/Invoice/_InvoiceReceiptAdd.cshtml", vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveInvoiceReceipt(InvoiceVM invoVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
                DataContracts.InvoiceMatchItem objUpdate = new DataContracts.InvoiceMatchItem();
                objUpdate = invoWrapper.AddReceipt(invoVM.InvoiceMatchItemModel);
                if (objUpdate.ErrorMessages != null && objUpdate.ErrorMessages.Count > 0)
                {
                    return Json(objUpdate.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = Command, InvoiceMatchHeaderId = objUpdate.InvoiceMatchHeaderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult RenderReceiptView(long VendorId, long InvoiceMatchHeaderId, string ClientLookupId)
        {
            InvoiceVM vm = new InvoiceVM(); var ChildObj = new InvoiceMatchHeaderModel();
            ChildObj.VendorId = VendorId; ChildObj.InvoiceMatchHeaderId = InvoiceMatchHeaderId;
            InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
            ChildObj.ClientLookupId = ClientLookupId;
            vm.InvoiceMatchHeaderModel = ChildObj;
            LocalizeControls(vm, LocalizeResourceSetConstants.InvoiceDetails);
            return PartialView("~/Views/Invoice/_PopupReceiptGrid.cshtml", vm);
        }
        [HttpPost]
        public JsonResult GetAllRecieptData(int VendorId = 0,
        string PurchaseOrder = "",
        DateTime? ReceivedDate = null,
        string PartID = "",
        string Description = "",
        decimal? QuantityReceived = null,
        string UnitofMeasure = "",
        decimal? UnitCost = null,
        decimal? TotalCost = null)
        {
            InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
            var PAMasterList = invoWrapper.ReceiptPopUpPopulate(VendorId);
            if (PAMasterList != null)
            {
                if (!string.IsNullOrEmpty(PurchaseOrder))
                {
                    PurchaseOrder = PurchaseOrder.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.POClientLookupId) && x.POClientLookupId.ToUpper().Contains(PurchaseOrder))).ToList();
                }
                if (ReceivedDate != null)
                {
                    PAMasterList = PAMasterList.Where(x => (x.ReceivedDate != null && x.ReceivedDate.Value.Date.Equals(ReceivedDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(PartID))
                {
                    PartID = PartID.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.PartClientLookupId) && x.PartClientLookupId.ToUpper().Contains(PartID))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (QuantityReceived != null)
                {
                    PAMasterList = PAMasterList.Where(x => (x.QuantityReceived.Value.Equals(QuantityReceived))).ToList();
                }
                if (!string.IsNullOrEmpty(UnitofMeasure))
                {
                    UnitofMeasure = UnitofMeasure.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.UnitOfMeasure) && x.UnitOfMeasure.ToUpper().Contains(UnitofMeasure))).ToList();
                }
                if (UnitCost != null)
                {
                    PAMasterList = PAMasterList.Where(x => (x.UnitCost.Value.Equals(UnitCost))).ToList();
                }
                if (TotalCost != null)
                {
                    PAMasterList = PAMasterList.Where(x => (x.TotalCost.Value.Equals(TotalCost))).ToList();
                }
            }
            return Json(PAMasterList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string PopulateRecieptData(int? draw, int? start, int? length, int VendorId = 0,
        string srcData = "",
        string PurchaseOrder = "",
        DateTime? ReceivedDate = null,
        string PartID = "",
        string Description = "",
        decimal? QuantityReceived = null,
        string UnitofMeasure = "",
        decimal? UnitCost = null,
        decimal? TotalCost = null)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var filter = srcData;
            InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
            var PAMasterList = invoWrapper.ReceiptPopUpPopulate(VendorId);
            PAMasterList = this.GetAllPurchaseApprovalByColumnWithOrder(order, orderDir, PAMasterList);
            if (PAMasterList != null && !string.IsNullOrEmpty(filter))
            {
                filter = filter.ToUpper();
                int VAL;
                bool res = int.TryParse(filter, out VAL);
                decimal val;
                bool outval = decimal.TryParse(filter, out val);
                DateTime dateTime;
                DateTime.TryParseExact(filter, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.POClientLookupId) && x.POClientLookupId.ToUpper().Contains(filter))
                                                        || (x.ReceivedDate != null && x.ReceivedDate.Value != default(DateTime) && x.ReceivedDate.Value.Date.Equals(dateTime))
                                                        || (!string.IsNullOrWhiteSpace(x.PartClientLookupId) && x.PartClientLookupId.ToUpper().Contains(filter))
                                                        || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(filter))
                                                        || (outval == true && x.QuantityReceived.Equals(val))
                                                        || (!string.IsNullOrWhiteSpace(x.UnitOfMeasure) && x.UnitOfMeasure.ToUpper().Contains(filter))
                                                       || (outval == true && x.UnitCost.Equals(val))
                                                        || (outval == true && x.TotalCost.Equals(val))
                                                        ).ToList();
            }

            if (PAMasterList != null)
            {
                if (!string.IsNullOrEmpty(PurchaseOrder))
                {
                    PurchaseOrder = PurchaseOrder.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.POClientLookupId) && x.POClientLookupId.ToUpper().Contains(PurchaseOrder))).ToList();
                }
                if (ReceivedDate != null)
                {
                    PAMasterList = PAMasterList.Where(x => (x.ReceivedDate != null && x.ReceivedDate.Value.Date.Equals(ReceivedDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(PartID))
                {
                    PartID = PartID.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.PartClientLookupId) && x.PartClientLookupId.ToUpper().Contains(PartID))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (QuantityReceived != null)
                {
                    PAMasterList = PAMasterList.Where(x => (x.QuantityReceived.Value.Equals(QuantityReceived))).ToList();
                }
                if (!string.IsNullOrEmpty(UnitofMeasure))
                {
                    UnitofMeasure = UnitofMeasure.ToUpper();
                    PAMasterList = PAMasterList.Where(x => (!string.IsNullOrWhiteSpace(x.UnitOfMeasure) && x.UnitOfMeasure.ToUpper().Contains(UnitofMeasure))).ToList();
                }
                if (UnitCost != null)
                {
                    PAMasterList = PAMasterList.Where(x => (x.UnitCost.Value.Equals(UnitCost))).ToList();
                }
                if (TotalCost != null)
                {
                    PAMasterList = PAMasterList.Where(x => (x.TotalCost.Value.Equals(TotalCost))).ToList();
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PAMasterList.Count();
            totalRecords = PAMasterList.Count();
            int initialPage = start.Value;
            var filteredResult = PAMasterList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<PopupGridViewModel> GetAllPurchaseApprovalByColumnWithOrder(string order, string orderDir, List<PopupGridViewModel> data)
        {
            List<PopupGridViewModel> lst = new List<PopupGridViewModel>();
            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.POClientLookupId).ToList() : data.OrderBy(p => p.POClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReceivedDate).ToList() : data.OrderBy(p => p.ReceivedDate).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QuantityReceived).ToList() : data.OrderBy(p => p.QuantityReceived).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitOfMeasure).ToList() : data.OrderBy(p => p.UnitOfMeasure).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                    break;
            }
            return lst;
        }
        public JsonResult SaveListRecieptFromGrid(List<InvoiceGridListModel> list)
        {
            InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
            var result = invoWrapper.UpdatePopupReceiptListGrid(list);
            if (result)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), invoiceMatchHeaderId = list[0].InvoiceMatchHeaderId }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Edit,Details
        public PartialViewResult InvoiceDetails(long invoiceId)
        {
            InvoiceVM objInvVM = new InvoiceVM();
            InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
            objInvVM.InvoiceMatchHeaderModel = InvWrapper.getInvoiceDetailsById(invoiceId);
            objInvVM.changeInvoiceModel = new ChangeInvoiceModel();
            Task attTask;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            attTask = Task.Factory.StartNew(() => objInvVM.attachmentCount = objCommonWrapper.AttachmentCount(invoiceId, AttachmentTableConstant.InvoiceMatch, userData.Security.InvoiceMatching.Edit));

            InvWrapper.ChangeInvoiceModel(invoiceId, objInvVM);

            LocalizeControls(objInvVM, LocalizeResourceSetConstants.InvoiceDetails);
            return PartialView("~/Views/Invoice/_InvoiceDetailsNew.cshtml", objInvVM);
        }
        public PartialViewResult AddInvoiceMatchHeader()
        {
            InvoiceVM objInvVM = new InvoiceVM();
            InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            InvoiceMatchHeaderModel InvModel = new InvoiceMatchHeaderModel();
            var typelist = comWrapper.GetListFromConstVals("InvoiceMatchType");
            InvModel.TypeList = typelist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).ToList();
            objInvVM.InvoiceMatchHeaderModel = InvModel;
            LocalizeControls(objInvVM, LocalizeResourceSetConstants.InvoiceDetails);
            return PartialView("~/Views/Invoice/_InvoiceAddOrEdit.cshtml", objInvVM);
        }
        public PartialViewResult EditInvoiceMatch(long InvoiceMatchId)
        {
            InvoiceVM objInvVM = new InvoiceVM();
            InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
            objInvVM.InvoiceMatchHeaderModel = InvWrapper.getInvoiceDetailsById(InvoiceMatchId);
            LocalizeControls(objInvVM, LocalizeResourceSetConstants.InvoiceDetails);
            return PartialView("~/Views/Invoice/_InvoiceAddOrEdit.cshtml", objInvVM);
        }
        public PartialViewResult UpdateInvoiceMatch(long InvoiceMatchId)
        {
            InvoiceVM objInvVM = new InvoiceVM();
            InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
            objInvVM.InvoiceMatchHeaderModel.InvoiceMatchHeaderId = InvoiceMatchId;
            LocalizeControls(objInvVM, LocalizeResourceSetConstants.InvoiceDetails);
            return PartialView("~/Views/Invoice/_InvoiceAddOrEdit.cshtml", objInvVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddInvoiceMatchHeader(InvoiceVM InvoiceVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
                InvoiceMatchHeader objInvoiceHeader = new DataContracts.InvoiceMatchHeader();
                if (InvoiceVM.InvoiceMatchHeaderModel.InvoiceMatchHeaderId == 0)
                {
                    Mode = "add";
                    objInvoiceHeader = InvWrapper.AddInvoiceMatchHeader(InvoiceVM.InvoiceMatchHeaderModel);
                }
                else
                {
                    Mode = "Update";
                    objInvoiceHeader = InvWrapper.EditInvoiceHeader(InvoiceVM.InvoiceMatchHeaderModel);
                }
                if (objInvoiceHeader.ErrorMessages != null && objInvoiceHeader.ErrorMessages.Count > 0)
                {
                    return Json(objInvoiceHeader.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = Command, InvoiceMatchHeaderId = objInvoiceHeader.InvoiceMatchHeaderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Notes
        [HttpPost]
        public string PopulateNotes(int? draw, int? start, int? length, long InvoiceId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
            var Notes = InvWrapper.PopulateNotes(InvoiceId);
            Notes = this.GetAllNotesSortByColumnWithOrder(order, orderDir, Notes);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Notes.Count();
            totalRecords = Notes.Count();
            int initialPage = start.Value;
            var filteredResult = Notes
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<Client.Models.Invoice.InvoiceNoteModel> GetAllNotesSortByColumnWithOrder(string order, string orderDir, List<Client.Models.Invoice.InvoiceNoteModel> data)
        {
            List<Client.Models.Invoice.InvoiceNoteModel> lst = new List<Client.Models.Invoice.InvoiceNoteModel>();
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
        public PartialViewResult AddNote(long InvoiceId, string ClientLookupId)
        {
            InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
            InvoiceVM objInvVM = new InvoiceVM();
            InvoiceMatchHeaderModel InvModel = new InvoiceMatchHeaderModel();
            InvoiceNoteModel noteModel = new InvoiceNoteModel();
            noteModel.ClientLookupId = ClientLookupId;
            noteModel.InvoiceId = InvoiceId;
            InvoiceMatchHeaderModel InvHeaderModel = new InvoiceMatchHeaderModel();
            InvHeaderModel.ClientLookupId = ClientLookupId;
            objInvVM.InvoiceMatchHeaderModel = InvHeaderModel;
            objInvVM.NotesModel = noteModel;
            LocalizeControls(objInvVM, LocalizeResourceSetConstants.InvoiceDetails);
            return PartialView("~/Views/Invoice/_AddInvoiceHeaderNote.cshtml", objInvVM);
        }
        public PartialViewResult EditNotes(long InvoiceId, long _notesId, string ClientLookupId)
        {
            InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
            InvoiceVM objInvVM = new InvoiceVM();
            InvoiceNoteModel _NotesModel = new InvoiceNoteModel();
            _NotesModel.ClientLookupId = ClientLookupId;
            _NotesModel.InvoiceId = InvoiceId;
            Client.Models.Invoice.InvoiceNoteModel objNotesModel = new InvoiceNoteModel();
            objNotesModel = InvWrapper.getNoteById(_notesId);
            objNotesModel.ClientLookupId = ClientLookupId;
            objNotesModel.InvoiceId = InvoiceId;
            objNotesModel.NotesId = _notesId;
            objInvVM.NotesModel = objNotesModel;
            LocalizeControls(objInvVM, LocalizeResourceSetConstants.InvoiceDetails);
            return PartialView("~/Views/Invoice/_AddInvoiceHeaderNote.cshtml", objInvVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotes(InvoiceVM InvVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<string> errorMessages = new List<string>();
                InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
                Client.Models.Invoice.InvoiceNoteModel objNotesModel = new Client.Models.Invoice.InvoiceNoteModel();
                objNotesModel.OwnerId = userData.DatabaseKey.User.UserInfoId;
                objNotesModel.OwnerName = userData.DatabaseKey.User.FullName;
                objNotesModel.Subject = InvVM.NotesModel.Subject;
                objNotesModel.Content = InvVM.NotesModel.Content;
                objNotesModel.Type = InvVM.NotesModel.Type;
                objNotesModel.TableName = "InvoiceMatch";
                objNotesModel.ObjectId = InvVM.NotesModel.InvoiceId;
                objNotesModel.updatedindex = InvVM.NotesModel.updatedindex;
                objNotesModel.NotesId = InvVM.NotesModel.NotesId;
                if (InvVM.NotesModel.NotesId == 0)
                {
                    Mode = "add";
                    errorMessages = InvWrapper.AddNote(objNotesModel);
                }
                else
                {
                    objNotesModel.ObjectId = InvVM.NotesModel.InvoiceId;
                    errorMessages = InvWrapper.UpdateNote(objNotesModel);
                }
                if (errorMessages != null && errorMessages.Count > 0)
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), InvoiceId = InvVM.NotesModel.InvoiceId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteNotes(long _notesId)
        {
            InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
            var deleteResult = InvWrapper.DeleteNote(_notesId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Attachment
        public string PopulateAttachment(int? draw, int? start, int? length, long InvoiceId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var Attachments = objCommonWrapper.PopulateAttachments(InvoiceId, "InvoiceMatch", userData.Security.InvoiceMatching.Edit);
            if (Attachments != null)
            {
                Attachments = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, Attachments);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Attachments.Count();
            totalRecords = Attachments.Count();
            int initialPage = start.Value;
            var filteredResult = Attachments
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<Client.Models.Invoice.AttachmentModel> GetAllAttachmentsSortByColumnWithOrder(string order, string orderDir, List<Client.Models.Invoice.AttachmentModel> data)
        {
            List<Client.Models.Invoice.AttachmentModel> lst = new List<Client.Models.Invoice.AttachmentModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileName).ToList() : data.OrderBy(p => p.FileName).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileSize).ToList() : data.OrderBy(p => p.FileSize).ToList();
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
        public PartialViewResult AddAttachments(long InvoiceId, string ClientLookUpId)
        {
            InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
            InvoiceVM objInvVM = new InvoiceVM();
            AttachmentModel Attachment = new AttachmentModel();
            Attachment.ClientLookupId = ClientLookUpId;
            Attachment.InvoiceId = InvoiceId;
            objInvVM.AttachmentModel = Attachment;
            LocalizeControls(objInvVM, LocalizeResourceSetConstants.InvoiceDetails);
            return PartialView("~/Views/Invoice/_AddAttachment.cshtml", objInvVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttachments()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                Stream stream = Request.Files[0].InputStream;
                var fileName = Request.Files[0].FileName;
                Client.Models.AttachmentModel attachmentModel = new Client.Models.AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);
                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.InvoiceId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = "InvoiceMatch";
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.InvoiceMatching.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.InvoiceMatching.Edit);
                }
                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), InvoiceId = Convert.ToInt64(Request.Form["attachmentModel.InvoiceId"]) }, JsonRequestBehavior.AllowGet);
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
        #endregion
        public JsonResult ChangeOptions(long invoiceId, string Mode)
        {
            string Msg = string.Empty;
            InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
            InvWrapper.ChangeOptions(Mode, invoiceId, out Msg);
            return Json(new { data = Msg }, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult DeleteInvoice(long invoiceId)
        {
            string result = string.Empty;
            List<String> errorList = new List<string>();
            InvoiceWrapper invoWrapper = new InvoiceWrapper(userData);
            errorList = invoWrapper.DeleteInvoice(invoiceId);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(errorList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangeInvoiceOptions(ChangeInvoiceModel changeInvoiceModel)
        {

            if (ModelState.IsValid)
            {
                List<string> Errormsg = new List<string>();
                dynamic ModelList = new object();
                InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
                InvoiceMatchHeader objInvoiceHeader = new DataContracts.InvoiceMatchHeader();
                InvWrapper.ChangeInvoiceOptions(changeInvoiceModel, out Errormsg);
                if (Errormsg != null && Errormsg.Count > 0)
                {
                    return Json(Errormsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), invoiceId = changeInvoiceModel.invoiceId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #region EventLog
        [HttpPost]
        public JsonResult CreateInvoiceEventLog(long invoiceId, string eventVal)
        {
            InvoiceWrapper InvWrapper = new InvoiceWrapper(userData);
            InvWrapper.CreateEventLog(invoiceId, eventVal);
            return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
