using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Purchase_Order;
using Client.Common;
using Client.Controllers.Common;
using Client.DevExpressReport;
using Client.DevExpressReport.EPM;
using Client.Models.Common;
using Client.Models.PurchaseOrder;

using Common.Constants;
using DataContracts;
using DevExpress.XtraReports.UI;
using Newtonsoft.Json;

using QRCoder;

using Rotativa;
using Rotativa.Options;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.PurchaseOrderReceipt
{
    public class PurchaseOrderReceiptController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.Purchasing_Receive)]
        public ActionResult Index()
        {
            PurchaseOrderWrapper pWrappers = new PurchaseOrderWrapper(userData);
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderModel objPurchaseOrderModel = new PurchaseOrderModel();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            var schduleJobList = pWrapper.DisplayIdList();
            if (schduleJobList != null)
            {
                objPurchaseOrderModel.TextSearchList = schduleJobList.Select(x => new SelectListItem { Text = x.Key, Value = x.Value }).OrderBy(x => x.Value);
            }
            // V2-851
            //var VendorsLookUplist = GetLookupList_Vendor();
            //if (VendorsLookUplist != null)
            //{
            //    objPurchaseOrderModel.VendorsList = VendorsLookUplist.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.Vendor.ToString() });
            //}
            CommonWrapper cWrapper = new CommonWrapper(userData);//V2-331
            objPurchaseOrderModel.SchedulePurchaseReceiptList = cWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.PurchaseOrderReceipt, false);//V2-331

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var statuslist = commonWrapper.GetListFromConstVals("POReceiptStatus");
            objPurchaseOrderModel.StatusList = statuslist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).ToList();
            objPurchaseOrderVM.PurchaseOrderModel = objPurchaseOrderModel;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return View(objPurchaseOrderVM);
        }
        #region V2-331

        [HttpPost]
        public string GetPOGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, string PurchaseOrder = "", string Status = "", string VendorClientLookupId = "",
                                string VendorName = "", DateTime? CreateDate = null, string Attention = "", string VendorPhoneNumber = "", DateTime? CompleteDate = null, string Reason = "",
                                string Buyer_PersonnelName = "", string TotalCost = "", string txtSearchval = "", string Order = "0"//, string orderDir = "asc")
            )
        {
            var filter = CustomQueryDisplayId;
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            List<string> statusList = new List<string>();
            List<PurchaseOrderModel> pOList = pWrapper.GetPORChunkList(CustomQueryDisplayId, skip, length ?? 0, Order, orderDir, PurchaseOrder, Status, VendorClientLookupId, VendorName, CreateDate, Attention, txtSearchval);

            var totalRecords = 0;
            var recordsFiltered = 0;
            if (pOList != null && pOList.Count > 0)
            {
                recordsFiltered = pOList[0].TotalCount;
                totalRecords = pOList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = pOList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);

        }
        #endregion


        #region V2-331        
        [HttpGet]
        public string GetPOPrintData(string colname, string coldir, int CustomQueryDisplayId = 0, DateTime? CompleteStartDateVw = null, DateTime? CompleteEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, string PurchaseOrder = "", string Status = "", string VendorClientLookupId = "",
                                   string VendorName = "", DateTime? CreateDate = null,
                                   string Attention = "", string VendorPhoneNumber = "", DateTime? StartCompleteDate = null, DateTime? EndCompleteDate = null, string Reason = "",
                                   string Buyer_PersonnelName = "", string TotalCost = "", string FilterValue = "", string txtSearchval = "")
        {
            List<PORPrintModel> poSearchModelList = new List<PORPrintModel>();
            PORPrintModel objPOPrintModel;
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            List<PurchaseOrderModel> poList = pWrapper.GetPORChunkList(CustomQueryDisplayId,
                0, 100000, colname, coldir, PurchaseOrder, Status, VendorClientLookupId, VendorName,
                CreateDate, Attention, txtSearchval);
            foreach (var p in poList)
            {
                objPOPrintModel = new PORPrintModel();
                objPOPrintModel.ClientLookupId = p.ClientLookupId;
                objPOPrintModel.Status = p.Status;
                objPOPrintModel.VendorClientLookupId = p.VendorClientLookupId;
                objPOPrintModel.VendorName = p.VendorName;
                objPOPrintModel.CreateDate = p.CreateDate;
                objPOPrintModel.Attention = p.Attention;
                poSearchModelList.Add(objPOPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = poSearchModelList }, JsonSerializerDateSettings);
        }
        #endregion

        private List<PurchaseOrderModel> GetPoReceiptSearchData(List<PurchaseOrderModel> poList, string PurchaseOrder, string Status, string VendorClientLookupId,
                                    string VendorName, DateTime? CreateDate, string Attention)
        {
            if (!string.IsNullOrEmpty(PurchaseOrder))
            {
                PurchaseOrder = PurchaseOrder.ToUpper();
                poList = poList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(PurchaseOrder))).ToList();
            }
            if (!string.IsNullOrEmpty(Status))
            {
                poList = poList.Where(x => (!string.IsNullOrWhiteSpace(x.Status) && x.Status.Equals(Status))).ToList();
            }
            if (!string.IsNullOrEmpty(VendorClientLookupId))
            {
                poList = poList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorClientLookupId) && x.VendorClientLookupId.Equals(VendorClientLookupId))).ToList();
            }
            if (!string.IsNullOrEmpty(VendorName))
            {
                VendorName = VendorName.ToUpper();
                poList = poList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorName) && x.VendorName.ToUpper().Contains(VendorName))).ToList();
            }
            if (CreateDate != null)
            {
                poList = poList.Where(x => (x.CreateDate != null && x.CreateDate.Value.Date.Equals(CreateDate.Value.Date))).ToList();
            }
            if (!string.IsNullOrEmpty(Attention))
            {
                Attention = Attention.ToUpper();
                poList = poList.Where(x => (!string.IsNullOrWhiteSpace(x.Attention) && x.Attention.ToUpper().Contains(Attention))).ToList();
            }
            return poList;
        }
        #endregion

        #region Add
        [HttpPost]
        public ActionResult AddPurchaseOrderReceipt(long PurchaseOrderId, int ItemsReceived, int ItemsIssued, int NoOfItems)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderModel objPurchaseOrderModel = new PurchaseOrderModel();
            PurchaseOrderReceiptModel objPurchaseOrderReceiptModel = new PurchaseOrderReceiptModel();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            objPurchaseOrderModel = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
            if (objPurchaseOrderModel != null)
            {
                objPurchaseOrderVM.PurchaseOrderModel = objPurchaseOrderModel;
            }
            objPurchaseOrderReceiptModel.ItemsReceived = ItemsReceived;
            objPurchaseOrderReceiptModel.ItemsIssued = ItemsIssued;
            objPurchaseOrderReceiptModel.NoOfItems = NoOfItems;
            objPurchaseOrderReceiptModel.PurchaseOrderId = PurchaseOrderId;
            //V2-947
            BusinessWrapper.Configuration.ClientSetUp.ClientSetUpWrapper csWrapper = new BusinessWrapper.Configuration.ClientSetUp.ClientSetUpWrapper(userData);
            var formsettingdetails = csWrapper.FormSettingsDetails();
            if (formsettingdetails != null)
            {
                objPurchaseOrderReceiptModel.PrintReceiptCheck = formsettingdetails.PORPrint;
            }
            if (objPurchaseOrderReceiptModel != null)
            {
                objPurchaseOrderVM.purchaseOrderReceiptModel = objPurchaseOrderReceiptModel;
            }
          
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_PurchaseOrderReceiptAdd", objPurchaseOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPurchaseOrderReceiptHeader(PurchaseOrderVM PurchaseOrderVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
                PurchaseOrderReceiptModel objPurchaseOrderReceiptModel = new PurchaseOrderReceiptModel();
                objPurchaseOrderReceiptModel = PurchaseOrderVM.purchaseOrderReceiptModel;
                objPurchaseOrderReceiptModel.PurchaseOrderId = PurchaseOrderVM.purchaseOrderReceiptModel.PurchaseOrderId;
                var receiptitems = pWrapper.AddPurchaseOrderReceipt(objPurchaseOrderReceiptModel);
                if (receiptitems != null && receiptitems.ErrorMessages != null && receiptitems.ErrorMessages.Count > 0)
                {
                    return Json(receiptitems.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), POReceiptHeaderId = receiptitems.POReceiptHeaderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Details
        public PartialViewResult Details(long PurchaseOrderId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderModel objPOModel = new PurchaseOrderModel();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> VoidLookUpList = new List<DataContracts.LookupList>();
            bool result = false;
            objPOModel = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
            if (objPOModel != null)
            {
                objPurchaseOrderVM.PurchaseOrderModel = objPOModel;
            }
            objPurchaseOrderVM.PurchaseOrderModel.Sec_PurchaseEdit = userData.Security.Purchasing.Edit;
            string BuyerPersonal = string.Empty;
            if (objPOModel.Buyer_PersonnelId != 0)
            {
                BuyerPersonal = pWrapper.GetBuyerName(objPOModel.Buyer_PersonnelId ?? 0);
            }
            objPurchaseOrderVM.PurchaseOrderModel.Buyer_PersonnelName = BuyerPersonal;
            ViewBag.LineItemSecurity = userData.Security.Purchasing.Edit;
            ViewBag.LineItemStatus = objPOModel.Status;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                VoidLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.PoVoid_Reason).ToList();
            }
            if (VoidLookUpList != null)
            {
                objPurchaseOrderVM.PurchaseOrderModel.VoidReasonList = VoidLookUpList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
            }
            objPurchaseOrderVM.PurchaseOrderModel.Sec_Void = userData.Security.Purchasing.Void;
            objPurchaseOrderVM.PurchaseOrderModel.Sec_ForceComplete = userData.Security.Purchasing.ForceComplete;
            objPurchaseOrderVM.PurchaseOrderModel.IsPurchasingReceive = userData.Security.Purchasing.Receive;
            if (objPOModel.PurchaseOrderId > 0)
            {
                objPurchaseOrderVM.PurchaseOrderModel._POVendorCountE2V = true;
                objPurchaseOrderVM.PurchaseOrderModel.PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToLower();
                objPurchaseOrderVM.PurchaseOrderModel.UseVendorMaster = userData.Site.UseVendorMaster;
            }
            else
            {
                result = pWrapper.PoVendorNoEmail(objPOModel.VendorId);
                if (result == true)
                {
                    objPurchaseOrderVM.PurchaseOrderModel._POVendorCountE2V = true;
                }
            }
            int prCount = 0;
            var list_pr = pWrapper.PopulateRequestDetails(PurchaseOrderId, ref prCount);
            if (prCount > 0)
            {
                objPurchaseOrderVM.PurchaseOrderModel._PRCountRD = true;
                objPurchaseOrderVM.PurchaseOrderModel.purchaseRequestModel = list_pr;
            }
            #region V2-1115
            bool EPMInvoiceImportInUse = false;

            var InterfacePropData = (List<InterfacePropModel>)Session["InterfacePropData"];
            if (InterfacePropData != null && InterfacePropData.Count > 0)
            {
                EPMInvoiceImportInUse = InterfacePropData.Where(x => x.InterfaceType == InterfacePropConstants.EPMInvoiceImport).Select(x => x.InUse).FirstOrDefault();
            }
            objPurchaseOrderVM.EPMInvoiceImportInUse = EPMInvoiceImportInUse;
            #endregion
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_PurchaseOrderReceiptDetails", objPurchaseOrderVM);
        }
        #endregion

        #region Line Item
        public string PopulateLineItem(int? draw, int? start, int? length, string searchText, decimal? Quantity, decimal? UnitCost, decimal? QtyReceived, decimal? QtyToDate, decimal? BackOrdered, long PurchaseOrderId = 0, string LineNumber = "", String PartId = "",
                                    string Description = "", string UOM = "", string Status = "", string Part_ManufacturerID = "")
        {
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var LineItems = pWrapper.PopulatePOLineItem(PurchaseOrderId);
            LineItems = this.GetLineItemsByColumnWithOrder(order, orderDir, LineItems);
            var statusList = LineItems.Select(r => r.Status_Display).GroupBy(x => x.ToString()).Select(x => x.First());
            if (LineItems != null && !string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToUpper();
                int VAL;
                bool res = int.TryParse(searchText, out VAL);
                LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.PartClientLookupId) && x.PartClientLookupId.ToUpper().Contains(searchText))
                                                        || (x.LineNumber != 0 && x.LineNumber.Equals(searchText)
                                                        || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(searchText))
                                                        || (!string.IsNullOrWhiteSpace(x.UnitOfMeasure) && x.UnitOfMeasure.ToUpper().Contains(searchText))
                                                        || (res == true && x.OrderQuantity.Equals(VAL))
                                                        || (res == true && x.UnitCost.Equals(VAL))
                                                        || (res == true && x.QuantityBackOrdered.Equals(VAL))
                                                        || (res == true && x.QuantityReceived.Equals(VAL))
                                                        || (res == true && x.QuantityToDate.Equals(VAL))
                                                        || (!string.IsNullOrWhiteSpace(x.Status_Display) && x.Status_Display.ToUpper().Contains(searchText))
                                                        || (!string.IsNullOrWhiteSpace(x.Part_ManufacturerID) && x.Part_ManufacturerID.ToUpper().Contains(searchText)))
                                                        ).ToList();
            }

            if (LineItems != null)
            {
                if (!string.IsNullOrEmpty(LineNumber))
                {
                    int LineNumberVAL;
                    bool res = int.TryParse(LineNumber, out LineNumberVAL);
                    LineItems = LineItems.Where(x => x.LineNumber.Equals(Convert.ToInt32(LineNumberVAL))).ToList();
                }
                if (!string.IsNullOrEmpty(PartId))
                {
                    PartId = PartId.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.PartClientLookupId) && x.PartClientLookupId.ToUpper().Contains(PartId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(Part_ManufacturerID))
                {
                    Part_ManufacturerID = Part_ManufacturerID.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.Part_ManufacturerID) && x.Part_ManufacturerID.ToUpper().Contains(Part_ManufacturerID))).ToList();
                }
                if (Quantity.HasValue)
                {
                    LineItems = LineItems.Where(x => x.OrderQuantity.Equals(Quantity)).ToList();
                }
                if (!string.IsNullOrEmpty(UOM))
                {
                    UOM = UOM.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.UnitOfMeasure) && x.UnitOfMeasure.ToUpper().Contains(UOM))).ToList();
                }
                if (UnitCost.HasValue)
                {
                    LineItems = LineItems.Where(x => x.UnitCost.Equals(UnitCost)).ToList();
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    Status = Status.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.Status_Display) && x.Status_Display.ToUpper().Contains(Status))).ToList();
                }
                if (QtyReceived.HasValue)
                {
                    LineItems = LineItems.Where(x => x.QuantityReceived.Equals(QtyReceived)).ToList();
                }
                if (QtyToDate.HasValue)
                {
                    LineItems = LineItems.Where(x => x.QuantityToDate.Equals(QtyToDate)).ToList();
                }
                if (BackOrdered.HasValue)
                {
                    LineItems = LineItems.Where(x => x.QuantityBackOrdered.Equals(BackOrdered)).ToList();
                }
            }
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
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, statuslist = statusList });
        }

        [HttpGet]
        public JsonResult PopulateLineItemdata(decimal? Quantity, decimal? UnitCost, decimal? QtyReceived, decimal? QtyToDate, decimal? BackOrdered, long PurchaseOrderId = 0, string LineNumber = "", String PartId = "",
                             string Description = "", string UOM = "", string Status = "", string Part_ManufacturerID = "")
        {
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            var LineItems = pWrapper.PopulatePOLineItem(PurchaseOrderId);
            if (LineItems != null)
            {
                if (!string.IsNullOrEmpty(LineNumber))
                {
                    int LineNumberVAL;
                    bool res = int.TryParse(LineNumber, out LineNumberVAL);
                    LineItems = LineItems.Where(x => x.LineNumber.Equals(Convert.ToInt32(LineNumberVAL))).ToList();
                }
                if (!string.IsNullOrEmpty(PartId))
                {
                    PartId = PartId.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.PartClientLookupId) && x.PartClientLookupId.ToUpper().Contains(PartId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(Part_ManufacturerID))
                {
                    Part_ManufacturerID = Part_ManufacturerID.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.Part_ManufacturerID) && x.Part_ManufacturerID.ToUpper().Contains(Part_ManufacturerID))).ToList();
                }
                if (Quantity.HasValue)
                {
                    LineItems = LineItems.Where(x => x.OrderQuantity.Equals(Quantity)).ToList();
                }
                if (!string.IsNullOrEmpty(UOM))
                {
                    UOM = UOM.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.UnitOfMeasure) && x.UnitOfMeasure.ToUpper().Contains(UOM))).ToList();
                }
                if (UnitCost.HasValue)
                {
                    LineItems = LineItems.Where(x => x.UnitCost.Equals(UnitCost)).ToList();
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    Status = Status.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.Status_Display) && x.Status_Display.ToUpper().Contains(Status))).ToList();
                }
                if (QtyReceived.HasValue)
                {
                    LineItems = LineItems.Where(x => x.QuantityReceived.Equals(QtyReceived)).ToList();
                }
                if (QtyToDate.HasValue)
                {
                    LineItems = LineItems.Where(x => x.QuantityToDate.Equals(QtyToDate)).ToList();
                }
                if (BackOrdered.HasValue)
                {
                    LineItems = LineItems.Where(x => x.QuantityBackOrdered.Equals(BackOrdered)).ToList();
                }
            }
            return Json(LineItems, JsonRequestBehavior.AllowGet);
        }
        private List<POLineItemModel> GetLineItemsByColumnWithOrder(string order, string orderDir, List<POLineItemModel> data)
        {
            List<POLineItemModel> lst = new List<POLineItemModel>();
            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LineNumber).ToList() : data.OrderBy(p => p.LineNumber).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Part_ManufacturerID).ToList() : data.OrderBy(p => p.Part_ManufacturerID).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OrderQuantity).ToList() : data.OrderBy(p => p.OrderQuantity).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitOfMeasure).ToList() : data.OrderBy(p => p.UnitOfMeasure).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QuantityReceived).ToList() : data.OrderBy(p => p.QuantityReceived).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QuantityToDate).ToList() : data.OrderBy(p => p.QuantityToDate).ToList();
                    break;
                case "10":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status_Display).ToList() : data.OrderBy(p => p.Status_Display).ToList();
                    break;
                case "11":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QuantityBackOrdered).ToList() : data.OrderBy(p => p.QuantityBackOrdered).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        [HttpPost]
        public ActionResult AddPurchaseOrderReceiptLineItem(List<PurchaseOrderReceiptLineItemModel> PORData)
        {
            long PurchaseId = 0;
            long POReceiptHeaderId = 0;
            POReceipt receiptitems = new POReceipt();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            PurchaseOrderReceiptModel objPurchaseOrderReceiptModel = new PurchaseOrderReceiptModel();
            if (PORData != null)
            {
                PurchaseId = PORData[0].PurchaseOrderId;
                POReceiptHeaderId = PORData[0].POReceiptHeaderId;
                objPurchaseOrderReceiptModel.PurchaseOrderId = PurchaseId;
                objPurchaseOrderReceiptModel.POReceiptHeaderId = POReceiptHeaderId;
                receiptitems = pWrapper.AddPOReceiptItems(PurchaseId, PORData);
            }
            if (receiptitems != null && receiptitems.ErrorMessages != null && receiptitems.ErrorMessages.Count > 0)
            {
                return Json(new { Result = receiptitems.ErrorMessages }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region QRCode 

        [HttpPost]
        public PartialViewResult POReceiptDetailPartQRcode(string[] PartClientLookups)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            QRCodeModel qRCodeModel = new QRCodeModel();
            List<string> partClientLookUpNames = new List<string>();
            foreach (var e in PartClientLookups)
            {
                partClientLookUpNames.Add(Convert.ToString(e));
            }
            qRCodeModel.PartIdsList = partClientLookUpNames;
            objPurchaseOrderVM.qRCodeModel = qRCodeModel;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_POReceiptDetailsQRCode", objPurchaseOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetPartIdlist(PurchaseOrderVM purchaseOrderVM)
        {
            TempData["POReceiptQRCodePartIdList"] = purchaseOrderVM.qRCodeModel.PartIdsList;
            return Json(new { JsonReturnEnum.success });
        }
        [EncryptedActionParameter]
        public ActionResult QRCodeGenerationUsingRotativa(bool SmallLabel)
        {
            var purchaseOrderVM = new PurchaseOrderVM();
            var qRCodeModel = new QRCodeModel();

            ViewBag.SmallQR = SmallLabel;
            if (TempData["POReceiptQRCodePartIdList"] != null)
            {
                qRCodeModel.PartIdsList = (List<string>)TempData["POReceiptQRCodePartIdList"];
            }
            else
            {
                qRCodeModel.PartIdsList = new List<string>();
            }
            purchaseOrderVM.qRCodeModel = qRCodeModel;

            return new PartialViewAsPdf("_POReceiptQRCodeTemplate", purchaseOrderVM)
            {
                PageMargins = { Left = 1, Right = 1, Top = 1, Bottom = 1 },
                PageHeight = SmallLabel ? 25 : 28,
                PageWidth = SmallLabel ? 54 : 89,
            };
        }

        public JsonResult GetQRcodeInfo(int partid,long PartStoreroomId)
        {
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            PurchaseOrderReceiptModel partinfo = new PurchaseOrderReceiptModel();
            PurchaseOrderReceiptModel PartStoreroom = new PurchaseOrderReceiptModel();
            if(userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                // RKL-MAIL-Label Printing from Receipts
                partinfo = pWrapper.populatePartDetailsMultiStoreroom(partid, PartStoreroomId);
                PartStoreroom = partinfo;
            }
            else
            {
                partinfo = pWrapper.populatePartDetails(partid);
                PartStoreroom = pWrapper.populatePartStoreDetails(partid);
            }
          
            var clientLookupId = partinfo.ClientLookupId;
            var description = partinfo.Description;
            var uom = partinfo.UOM;
            System.Text.StringBuilder sbLoc = new System.Text.StringBuilder();
            if (!string.IsNullOrEmpty(PartStoreroom.Location1_5))
                sbLoc.Append(PartStoreroom.Location1_5);
            if (sbLoc.Length > 0 && !string.IsNullOrEmpty(PartStoreroom.Location1_1))
                sbLoc.Append("-" + PartStoreroom.Location1_1);
            if (sbLoc.Length == 0 && !string.IsNullOrEmpty(PartStoreroom.Location1_1))
                sbLoc.Append(PartStoreroom.Location1_1);
            if (sbLoc.Length > 0 && !string.IsNullOrEmpty(PartStoreroom.Location1_2))
                sbLoc.Append("-" + PartStoreroom.Location1_2);
            if (sbLoc.Length == 0 && !string.IsNullOrEmpty(PartStoreroom.Location1_2))
                sbLoc.Append(PartStoreroom.Location1_2);
            if (sbLoc.Length > 0 && !string.IsNullOrEmpty(PartStoreroom.Location1_3))
                sbLoc.Append("-" + PartStoreroom.Location1_3);
            if (sbLoc.Length == 0 && !string.IsNullOrEmpty(PartStoreroom.Location1_3))
                sbLoc.Append(PartStoreroom.Location1_3);
            if (sbLoc.Length > 0 && !string.IsNullOrEmpty(PartStoreroom.Location1_4))
                sbLoc.Append("-" + PartStoreroom.Location1_4);
            if (sbLoc.Length == 0 && !string.IsNullOrEmpty(PartStoreroom.Location1_4))
                sbLoc.Append(PartStoreroom.Location1_4);
            var pTLocation = sbLoc.ToString();
            var minimum = PartStoreroom.Minimum;
            var maximum = PartStoreroom.Maximum;
            var manufacturer = PartStoreroom.Manufacturer;
            var manufacturerId = PartStoreroom.ManufacturerId;
            return Json(new { ClientLookupId = clientLookupId, Description = description, PTLocation = pTLocation, Minimum = minimum, Maximum = maximum, Manufacturer = manufacturer, UOM = uom, ManufacturerId = manufacturerId }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Option Request
        public PartialViewResult ReceiptHistory(long PurchaseOrderId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderModel objPOModel = new PurchaseOrderModel();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            objPOModel = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
            if (objPOModel != null)
            {
                objPurchaseOrderVM.PurchaseOrderModel = objPOModel;
            }
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_ReceiptHistory", objPurchaseOrderVM);
        }
        public ActionResult PopulateReceiptHistoryGrid(int PurchaseOderId)
        {
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            var LineItems = pWrapper.PopulatePOLineItem(PurchaseOderId);
            return Json(new { data = LineItems }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult POInnerGrid(long PurchaseOrderLineItemId)
        {
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            var LineItemMasterList = pWrapper.InnerGridReceiptHistory(PurchaseOrderLineItemId);
            PurchaseOrderVM objPO = new PurchaseOrderVM();
            objPO.LineItemList = LineItemMasterList;
            LocalizeControls(objPO, LocalizeResourceSetConstants.PurchaseOrder);
            return View(objPO);
        }
        [HttpPost]
        public JsonResult UpdateRevisedComments(List<InnerGridLineItemModel> list)
        {
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            POReceipt poReceiptUpdate = pWrapper.UpdateRevised(list);
            if (poReceiptUpdate.ErrorMessages != null && poReceiptUpdate.ErrorMessages.Count > 0)
            {
                return Json(poReceiptUpdate.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Private method
        private List<PurchaseOrderModel> GetAllPOSortByColumnWithOrder(string order, string orderDir, List<PurchaseOrderModel> data)
        {
            List<PurchaseOrderModel> lst = new List<PurchaseOrderModel>();
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
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Attention).ToList() : data.OrderBy(p => p.Attention).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorPhoneNumber).ToList() : data.OrderBy(p => p.VendorPhoneNumber).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompleteDate).ToList() : data.OrderBy(p => p.CompleteDate).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Reason).ToList() : data.OrderBy(p => p.Reason).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Buyer_PersonnelName).ToList() : data.OrderBy(p => p.Buyer_PersonnelName).ToList();
                    break;
                case "10":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region Print PO Receipt
        [HttpPost]
        public JsonResult PrintPOReceipt(long POReceiptHeaderId)
        {
            using (var ms = new MemoryStream())
            {
                using (var doc = new iTextSharp.text.Document())
                {
                    using (var copy = new iTextSharp.text.pdf.PdfSmartCopy(doc, ms))
                    {
                        doc.Open();
                        var msSinglePDf = new MemoryStream(PrintGetByteStream(POReceiptHeaderId));
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
        [ActionFilters.EncryptedActionParameter]
        public Byte[] PrintGetByteStream(long receiptHeaderNo)
        {
            PurchaseOrderVM objVM = new PurchaseOrderVM();
            PurchaseOrderVM objHeaderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            objVM.poPrintReceiptmodel = new POPrintReceiptModel();

            objVM.poPrintReceiptmodel = pWrapper.POReceiptPrint(receiptHeaderNo);
            objHeaderVM.poPrintReceiptmodel = pWrapper.POReceiptHeaderPrint(receiptHeaderNo);

            var poReceiptHeaderjson = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objHeaderVM.poPrintReceiptmodel);
            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                   "--header-spacing \"3\" " +
                                   "--footer-html \"{1}\" " +
                                   "--footer-spacing \"8\" " +
                                   "--footer-font-size \"10\" " +
                                   "--header-font-size \"10\" ",
                                   Url.Action("Header", "PurchaseOrderReceipt", new { id = userData.LoginAuditing.SessionId, POReceiptJson = poReceiptHeaderjson }, Request.Url.Scheme),
                                   Url.Action("Footer", "PurchaseOrderReceipt", new { id = userData.LoginAuditing.SessionId }, Request.Url.Scheme)
                                   );

            LocalizeControls(objVM, LocalizeResourceSetConstants.WorkOrderDetails);
            var mailpdft = new ViewAsPdf("PrintPOReceiptTemplate", objVM)
            {
                PageMargins = new Margins(43, 12, 22, 12),// it’s in millimeters
                CustomSwitches = customSwitches
            };
            Byte[] PdfData = mailpdft.BuildPdf(ControllerContext);
            return PdfData;
        }
        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Header(string id, string POReceiptJson)
        {
            POPrintReceiptModel POReceiptModel = new POPrintReceiptModel();
            POReceiptModel = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<POPrintReceiptModel>(POReceiptJson);
            PurchaseOrderVM objVM = new PurchaseOrderVM();
            if (CheckLoginSession(id))
            {
                CommonWrapper comWrapper = new CommonWrapper(userData);
                PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
                objVM.poPrintReceiptmodel = new POPrintReceiptModel();
                objVM.poPrintReceiptmodel = POReceiptModel;
                objVM.poPrintReceiptmodel.AzureImageURL = comWrapper.GetClientLogoUrl();
                LocalizeControls(objVM, LocalizeResourceSetConstants.PurchaseOrder);
            }
            return View("PrintHeader", objVM);
        }

        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Footer(string id)
        {
            PurchaseOrderVM objVM = new PurchaseOrderVM();
            if (CheckLoginSession(id))
            {
                LocalizeControls(objVM, LocalizeResourceSetConstants.PurchaseOrder);
            }
            return View("PrintFooter", objVM);
        }
        #endregion

        #region V2-947

        [HttpPost]
        public JsonResult SetPrintPORFromIndex(long PurchaseOrderId = 0 ,long POReceiptHeaderId = 0)
        {
            Session["DevExpressPrintPORPurchaseOrderId"] = PurchaseOrderId;
            Session["DevExpressPrintPORPOReceiptHeaderId"] = POReceiptHeaderId;
            return Json(new { success = true });
        }
        public ActionResult GeneratePurchaseOrderReceiptPrint()
        {
            long PurchaseOrderId = 0;
            long POReceiptHeaderId = 0;
            if (Session["DevExpressPrintPORPurchaseOrderId"] != null && Session["DevExpressPrintPORPOReceiptHeaderId"] != null)
            {
                PurchaseOrderId = (long)Session["DevExpressPrintPORPurchaseOrderId"];
                POReceiptHeaderId = (long)Session["DevExpressPrintPORPOReceiptHeaderId"];

            }
            var objPrintModelList = PrintDevExpressFromIndex(PurchaseOrderId, POReceiptHeaderId);
           
            return View("DevExpressPrint", objPrintModelList);
        }

        public List<PurchaseOrderReceiptDevExpressPrintModel> PrintDevExpressFromIndex(long PurchaseOrderId, long POReceiptHeaderId)
        {
           
            PurchaseOrderWrapper poWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
           var PurchaseOrderReceiptDevExpressPrintList = new List<PurchaseOrderReceiptDevExpressPrintModel>();
            var PurchaseOrderReceiptBunchListInfo = poWrapper.RetrieveAllPrintForPOreceipt(PurchaseOrderId, POReceiptHeaderId);


            PurchaseOrder PurchaseOrderReceiptDetails = PurchaseOrderReceiptBunchListInfo.POPurchaseOrder;
            List<POReceipt> listOfPurchaseOrderLineItem = PurchaseOrderReceiptBunchListInfo.POReceiptItemlist;
            #region V2-1011
            POHeaderUDF PurchaseOrderHeaderUDFDetails = PurchaseOrderReceiptBunchListInfo.POHeaderUDF;
            List<Notes> listOfNotes = PurchaseOrderReceiptBunchListInfo.listOfNotes;
            #endregion

            //  var ImageUrl = GenerateImageUrl();// no need to call for each id as it is dependent on client id

            CommonWrapper comWrapper = new CommonWrapper(userData);
            string AzureImageURL = GenerateImageUrlDevExpress();

            //-- binding for devexpress begin
            PurchaseOrderReceiptDevExpressPrintModel purchaseOrderReceiptDevExpressPrintModel = new PurchaseOrderReceiptDevExpressPrintModel();
            BindPurchaseOrderReceiptDetails(PurchaseOrderReceiptDetails, ref purchaseOrderReceiptDevExpressPrintModel, AzureImageURL);
            BindPurchaseOrderReceiptLineItemTable(listOfPurchaseOrderLineItem, ref purchaseOrderReceiptDevExpressPrintModel);
            #region V2-1011
            BindPurchaseOrderHeaderUDFDetails(PurchaseOrderHeaderUDFDetails, ref purchaseOrderReceiptDevExpressPrintModel);
            BindCommentsTable(listOfNotes, ref purchaseOrderReceiptDevExpressPrintModel);
            #endregion
            BusinessWrapper.Configuration.ClientSetUp.ClientSetUpWrapper csWrapper = new BusinessWrapper.Configuration.ClientSetUp.ClientSetUpWrapper(userData);
            var formsettingdetails = csWrapper.FormSettingsDetails();
            if (formsettingdetails != null)
            {
                purchaseOrderReceiptDevExpressPrintModel.PORPrint = formsettingdetails.PORPrint;
                purchaseOrderReceiptDevExpressPrintModel.PORHeader = formsettingdetails.PORHeader;
                purchaseOrderReceiptDevExpressPrintModel.PORLine2 = formsettingdetails.PORLine2;
                #region V2-1011
                purchaseOrderReceiptDevExpressPrintModel.PORUIC = formsettingdetails.PORUIC;
                purchaseOrderReceiptDevExpressPrintModel.PORComments = formsettingdetails.PORComments;
                #endregion
            }
            purchaseOrderReceiptDevExpressPrintModel.OnPremise=userData.DatabaseKey.Client.OnPremise;
            PurchaseOrderReceiptDevExpressPrintList.Add(purchaseOrderReceiptDevExpressPrintModel);
            return PurchaseOrderReceiptDevExpressPrintList;
        }

        private void BindPurchaseOrderReceiptDetails(PurchaseOrder PurchaseOrderDetails, ref PurchaseOrderReceiptDevExpressPrintModel purchaseOrderReceiptDevExpressPrintModel, string AzureImageUrl)
        {
            #region Header

            purchaseOrderReceiptDevExpressPrintModel.PurchaseOrderId = PurchaseOrderDetails.PurchaseOrderId;
            purchaseOrderReceiptDevExpressPrintModel.ClientlookupId = PurchaseOrderDetails.ClientLookupId;
            purchaseOrderReceiptDevExpressPrintModel.AzureImageUrl = AzureImageUrl;

            if (PurchaseOrderDetails.ReceiveDate == null && PurchaseOrderDetails.ReceiveDate == DateTime.MinValue)
            {
                purchaseOrderReceiptDevExpressPrintModel.ReceiveDate = "";
            }
            else
            {
                purchaseOrderReceiptDevExpressPrintModel.ReceiveDate = Convert.ToDateTime(PurchaseOrderDetails.ReceiveDate)
                    .ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            }
           

            #endregion
            #region Vendor/POR Header

            purchaseOrderReceiptDevExpressPrintModel.VendorName = PurchaseOrderDetails.VendorName;
            purchaseOrderReceiptDevExpressPrintModel.VendorEmailAddress = PurchaseOrderDetails.VendorEmailAddress;
            purchaseOrderReceiptDevExpressPrintModel.VendorAddress1 = PurchaseOrderDetails.VendorAddress1;
            purchaseOrderReceiptDevExpressPrintModel.VendorAddress2 = PurchaseOrderDetails.VendorAddress2;
            purchaseOrderReceiptDevExpressPrintModel.VendorAddress3 = PurchaseOrderDetails.VendorAddress3;
            purchaseOrderReceiptDevExpressPrintModel.VendorAddressCity = PurchaseOrderDetails.VendorAddressCity == null ? "" : PurchaseOrderDetails.VendorAddressCity;
            purchaseOrderReceiptDevExpressPrintModel.VendorAddressCountry = PurchaseOrderDetails.VendorAddressCountry == null ? "" : PurchaseOrderDetails.VendorAddressCountry;
            purchaseOrderReceiptDevExpressPrintModel.VendorAddressPostCode = PurchaseOrderDetails.VendorAddressPostCode == null ? "" : PurchaseOrderDetails.VendorAddressPostCode;
            purchaseOrderReceiptDevExpressPrintModel.VendorAddressState = PurchaseOrderDetails.VendorAddressState == null ? "" : PurchaseOrderDetails.VendorAddressState;
            string VenndorAdresssCSPC = "";//Vendor concate the city ,state,postcode and country
            string[] VenndorAdresssCSPCArray = { purchaseOrderReceiptDevExpressPrintModel.VendorAddressCity, purchaseOrderReceiptDevExpressPrintModel.VendorAddressState, purchaseOrderReceiptDevExpressPrintModel.VendorAddressPostCode, purchaseOrderReceiptDevExpressPrintModel.VendorAddressCountry };
            VenndorAdresssCSPC = String.Join(",", VenndorAdresssCSPCArray.Where(x => !string.IsNullOrEmpty(x)).ToArray());
            purchaseOrderReceiptDevExpressPrintModel.VenndorAdresssCSPC = VenndorAdresssCSPC;
            //
            purchaseOrderReceiptDevExpressPrintModel.SiteName = PurchaseOrderDetails.SiteName;
            purchaseOrderReceiptDevExpressPrintModel.SiteAddress1 = PurchaseOrderDetails.SiteAddress1;
            purchaseOrderReceiptDevExpressPrintModel.SiteAddress2 = PurchaseOrderDetails.SiteAddress2;
            purchaseOrderReceiptDevExpressPrintModel.SiteAddress3 = PurchaseOrderDetails.SiteAddress3;
            purchaseOrderReceiptDevExpressPrintModel.SiteAddressCity = PurchaseOrderDetails.SiteAddressCity == null ?"": PurchaseOrderDetails.SiteAddressCity;
            purchaseOrderReceiptDevExpressPrintModel.SiteAddressCountry = PurchaseOrderDetails.SiteAddressCountry == null ? "" : PurchaseOrderDetails.SiteAddressCountry;
            purchaseOrderReceiptDevExpressPrintModel.SiteAddressPostCode = PurchaseOrderDetails.SiteAddressPostCode == null ? "" : PurchaseOrderDetails.SiteAddressPostCode;
            purchaseOrderReceiptDevExpressPrintModel.SiteAddressState = PurchaseOrderDetails.SiteAddressState == null ? "" : PurchaseOrderDetails.SiteAddressState;
            string siteAdresssCSPC = "";//Site concate the city ,state,postcode and country
            string[] siteAdresssCSPCArray = { purchaseOrderReceiptDevExpressPrintModel.SiteAddressCity, purchaseOrderReceiptDevExpressPrintModel.SiteAddressState, purchaseOrderReceiptDevExpressPrintModel.SiteAddressPostCode, purchaseOrderReceiptDevExpressPrintModel.SiteAddressCountry };
            siteAdresssCSPC = String.Join(",", siteAdresssCSPCArray.Where(x => !string.IsNullOrEmpty(x)).ToArray());
            purchaseOrderReceiptDevExpressPrintModel.siteAdresssCSPC = siteAdresssCSPC;
            //

            purchaseOrderReceiptDevExpressPrintModel.BillToName = PurchaseOrderDetails.SiteBillToName;
            purchaseOrderReceiptDevExpressPrintModel.SiteBillToAddress1 = PurchaseOrderDetails.SiteBillToAddress1;
            purchaseOrderReceiptDevExpressPrintModel.SiteBillToAddress2 = PurchaseOrderDetails.SiteBillToAddress2;
            purchaseOrderReceiptDevExpressPrintModel.SiteBillToAddress3 = PurchaseOrderDetails.SiteBillToAddress3;
            purchaseOrderReceiptDevExpressPrintModel.SiteBillToAddressCity = PurchaseOrderDetails.SiteBillToAddressCity == null ? "" : PurchaseOrderDetails.SiteBillToAddressCity;
            purchaseOrderReceiptDevExpressPrintModel.SiteBillToAddressCountry = PurchaseOrderDetails.SiteBillToAddressCountry == null ? "" : PurchaseOrderDetails.SiteBillToAddressCountry;
            purchaseOrderReceiptDevExpressPrintModel.SiteBillToAddressPostCode = PurchaseOrderDetails.SiteBillToAddressPostCode == null ? "" : PurchaseOrderDetails.SiteBillToAddressPostCode;
            purchaseOrderReceiptDevExpressPrintModel.SiteBillToAddressState = PurchaseOrderDetails.SiteBillToAddressState == null ? "" : PurchaseOrderDetails.SiteBillToAddressState;
           
            string sitebillAdresssCSPC = "";//Bill to concate the city ,state,postcode and country
            string[] sitebillAdresssCSPCArray = { purchaseOrderReceiptDevExpressPrintModel.SiteBillToAddressCity, purchaseOrderReceiptDevExpressPrintModel.SiteBillToAddressState, purchaseOrderReceiptDevExpressPrintModel.SiteBillToAddressPostCode, purchaseOrderReceiptDevExpressPrintModel.SiteBillToAddressCountry };
            sitebillAdresssCSPC = String.Join(",", sitebillAdresssCSPCArray.Where(x => !string.IsNullOrEmpty(x)).ToArray());
            purchaseOrderReceiptDevExpressPrintModel.sitebillAdresssCSPC = sitebillAdresssCSPC;
            //

            purchaseOrderReceiptDevExpressPrintModel.ReceiveBy = PurchaseOrderDetails.ReceivedPersonnelName;
            purchaseOrderReceiptDevExpressPrintModel.FreightAmount = PurchaseOrderDetails.FreightAmount.ToString();
            purchaseOrderReceiptDevExpressPrintModel.FreightBill = PurchaseOrderDetails.FreightBill;

            purchaseOrderReceiptDevExpressPrintModel.Carrier = PurchaseOrderDetails.Carrier;
            purchaseOrderReceiptDevExpressPrintModel.Attention = PurchaseOrderDetails.Attention;
            purchaseOrderReceiptDevExpressPrintModel.MessageToVendor = PurchaseOrderDetails.MessageToVendor;
            purchaseOrderReceiptDevExpressPrintModel.ReceiptNumber = PurchaseOrderDetails.ReceiptNumber.ToString();
            purchaseOrderReceiptDevExpressPrintModel.PackingSlip = PurchaseOrderDetails.PackingSlip;
            purchaseOrderReceiptDevExpressPrintModel.MessageToVendor = PurchaseOrderDetails.MessageToVendor;
            purchaseOrderReceiptDevExpressPrintModel.Comments = PurchaseOrderDetails.Comments;

            #endregion

            #region Localization
            purchaseOrderReceiptDevExpressPrintModel.spnVendorPOHeader = PurchaseOrderDetails.SiteBillToAddressCountry;
            purchaseOrderReceiptDevExpressPrintModel.spnVendor = UtilityFunction.GetMessageFromResource("GlobalVendor", LocalizeResourceSetConstants.Global);
            purchaseOrderReceiptDevExpressPrintModel.GlobalShipTo = UtilityFunction.GetMessageFromResource("GlobalShipTo", LocalizeResourceSetConstants.Global);
            purchaseOrderReceiptDevExpressPrintModel.spnPOBillTo = UtilityFunction.GetMessageFromResource("spnPoBillTo", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderReceiptDevExpressPrintModel.spnReccord = UtilityFunction.GetMessageFromResource("spnReccord", LocalizeResourceSetConstants.Global);
            purchaseOrderReceiptDevExpressPrintModel.spnPoCarrier = UtilityFunction.GetMessageFromResource("spnPoCarrier", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderReceiptDevExpressPrintModel.spnReceivedBy = UtilityFunction.GetMessageFromResource("spnReceivedBy", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderReceiptDevExpressPrintModel.spnReceiptNumber = UtilityFunction.GetMessageFromResource("spnReceiptNumber", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderReceiptDevExpressPrintModel.spnPoAttention = UtilityFunction.GetMessageFromResource("spnPoAttention", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderReceiptDevExpressPrintModel.spnPoMessageToVendor = UtilityFunction.GetMessageFromResource("spnPoMessageToVendor", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderReceiptDevExpressPrintModel.spnPackingSlip = UtilityFunction.GetMessageFromResource("spnPackingSlip", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderReceiptDevExpressPrintModel.spnFreightBill = UtilityFunction.GetMessageFromResource("spnFreightBill", LocalizeResourceSetConstants.PurchaseOrder);

            purchaseOrderReceiptDevExpressPrintModel.spnFreightAmt = UtilityFunction.GetMessageFromResource("spnFreightAmt", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderReceiptDevExpressPrintModel.spnPoComments = UtilityFunction.GetMessageFromResource("spnPoComments", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderReceiptDevExpressPrintModel.GlobalPurchaseOrder = UtilityFunction.GetMessageFromResource("spnPurchaseOrder", LocalizeResourceSetConstants.Global);
            purchaseOrderReceiptDevExpressPrintModel.spnPurchaseOrderReceipt = UtilityFunction.GetMessageFromResource("spnPurchaseOrderReceipt", LocalizeResourceSetConstants.SetUpDetails);

            #endregion
        }
        private void BindPurchaseOrderReceiptLineItemTable(List<POReceipt> listOfPOReceiptLineItemInfo, ref PurchaseOrderReceiptDevExpressPrintModel purchaseOrderReceiptDevExpressPrintModel)
        {
            if (listOfPOReceiptLineItemInfo.Count > 0)
            {
                
                foreach (var item in listOfPOReceiptLineItemInfo)
                {
                    var objPORLineItemDevExpressPrintModel = new PurchaseOrderReceiptLineItemDevExpressPrintModel();
                    objPORLineItemDevExpressPrintModel.LineNumber = item.LineNumber;
                    objPORLineItemDevExpressPrintModel.PartClientLookupId = item.PartClientLookupId;
                    objPORLineItemDevExpressPrintModel.Description = item.Description;
                    objPORLineItemDevExpressPrintModel.QuantityReceived = Math.Round(item.QuantityReceived, 2);
                    objPORLineItemDevExpressPrintModel.UnitOfMeasure = item.UnitOfMeasure;
                    objPORLineItemDevExpressPrintModel.UnitCost = Math.Round(item.UnitCost, 2);
                    objPORLineItemDevExpressPrintModel.TotalCost = Math.Round(item.TotalCost, 2);
                    objPORLineItemDevExpressPrintModel.ChargeToClientLookupId = item.ChargeToClientLookupId;
                    objPORLineItemDevExpressPrintModel.AccountClientLookupId = item.AccountClientLookupId;

                    #region Localization
                    objPORLineItemDevExpressPrintModel.globalLineItems = UtilityFunction.GetMessageFromResource("spnLineItems", LocalizeResourceSetConstants.Global);
                    objPORLineItemDevExpressPrintModel.globalLine = UtilityFunction.GetMessageFromResource("globalLine", LocalizeResourceSetConstants.Global);
                    objPORLineItemDevExpressPrintModel.spnPartID = UtilityFunction.GetMessageFromResource("spnPoPartID", LocalizeResourceSetConstants.PurchaseOrder);
                    objPORLineItemDevExpressPrintModel.spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global);
                    objPORLineItemDevExpressPrintModel.spnQtyRec = UtilityFunction.GetMessageFromResource("spnQtyRec", LocalizeResourceSetConstants.PurchaseOrder);
                    objPORLineItemDevExpressPrintModel.spnUOM = UtilityFunction.GetMessageFromResource("spnPdUOM", LocalizeResourceSetConstants.Global);
                    objPORLineItemDevExpressPrintModel.spnPrice = UtilityFunction.GetMessageFromResource("spnPoPrice", LocalizeResourceSetConstants.PurchaseOrder);
                    objPORLineItemDevExpressPrintModel.spnTotal = UtilityFunction.GetMessageFromResource("GlobalTotal", LocalizeResourceSetConstants.Global);
                    objPORLineItemDevExpressPrintModel.GlobalStatus = UtilityFunction.GetMessageFromResource("GlobalStatus", LocalizeResourceSetConstants.Global);
                    objPORLineItemDevExpressPrintModel.GlobalChargeTo = UtilityFunction.GetMessageFromResource("GlobalChargeTo", LocalizeResourceSetConstants.Global);
                    objPORLineItemDevExpressPrintModel.GlobalAccount = UtilityFunction.GetMessageFromResource("GlobalAccount", LocalizeResourceSetConstants.Global);
                    objPORLineItemDevExpressPrintModel.spnPoGrandtotal = UtilityFunction.GetMessageFromResource("spnPoGrandtotal", LocalizeResourceSetConstants.PurchaseOrder);

                    #endregion
                    purchaseOrderReceiptDevExpressPrintModel.PurchaseOrderReceiptLineItemDevExpressPrintModelList.Add(objPORLineItemDevExpressPrintModel);
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

        #region V2-1011
        private void BindPurchaseOrderHeaderUDFDetails(POHeaderUDF PurchaseOrderHeaderUDFDetails, ref PurchaseOrderReceiptDevExpressPrintModel purchaseOrderReceiptDevExpressPrintModel)
        {
            #region Purchase Order Header UIC
            purchaseOrderReceiptDevExpressPrintModel.POHeaderUDF_POId = PurchaseOrderHeaderUDFDetails?.PurchaseOrderId ?? 0;
            if (PurchaseOrderHeaderUDFDetails != null)
            {
                purchaseOrderReceiptDevExpressPrintModel.Text1 = PurchaseOrderHeaderUDFDetails.Text1;
                purchaseOrderReceiptDevExpressPrintModel.Text2 = PurchaseOrderHeaderUDFDetails.Text2;
                purchaseOrderReceiptDevExpressPrintModel.Text3 = PurchaseOrderHeaderUDFDetails.Text3;
                purchaseOrderReceiptDevExpressPrintModel.Text4 = PurchaseOrderHeaderUDFDetails.Text4;
                if (PurchaseOrderHeaderUDFDetails.Date1 == null || PurchaseOrderHeaderUDFDetails.Date1 == default(DateTime))
                {
                    purchaseOrderReceiptDevExpressPrintModel.Date1 = "";
                }
                else
                {
                    purchaseOrderReceiptDevExpressPrintModel.Date1 = Convert.ToDateTime(PurchaseOrderHeaderUDFDetails.Date1)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (PurchaseOrderHeaderUDFDetails.Date2 == null || PurchaseOrderHeaderUDFDetails.Date2 == default(DateTime))
                {
                    purchaseOrderReceiptDevExpressPrintModel.Date2 = "";
                }
                else
                {
                    purchaseOrderReceiptDevExpressPrintModel.Date2 = Convert.ToDateTime(PurchaseOrderHeaderUDFDetails.Date2)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (PurchaseOrderHeaderUDFDetails.Date3 == null || PurchaseOrderHeaderUDFDetails.Date3 == default(DateTime))
                {
                    purchaseOrderReceiptDevExpressPrintModel.Date3 = "";
                }
                else
                {
                    purchaseOrderReceiptDevExpressPrintModel.Date3 = Convert.ToDateTime(PurchaseOrderHeaderUDFDetails.Date3)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (PurchaseOrderHeaderUDFDetails.Date4 == null || PurchaseOrderHeaderUDFDetails.Date4 == default(DateTime))
                {
                    purchaseOrderReceiptDevExpressPrintModel.Date4 = "";
                }
                else
                {
                    purchaseOrderReceiptDevExpressPrintModel.Date4 = Convert.ToDateTime(PurchaseOrderHeaderUDFDetails.Date4)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                purchaseOrderReceiptDevExpressPrintModel.Bit1 = PurchaseOrderHeaderUDFDetails.Bit1 ? "Yes" : "No";
                purchaseOrderReceiptDevExpressPrintModel.Bit2 = PurchaseOrderHeaderUDFDetails.Bit2 ? "Yes" : "No";
                purchaseOrderReceiptDevExpressPrintModel.Bit3 = PurchaseOrderHeaderUDFDetails.Bit3 ? "Yes" : "No";
                purchaseOrderReceiptDevExpressPrintModel.Bit4 = PurchaseOrderHeaderUDFDetails.Bit4 ? "Yes" : "No";
                purchaseOrderReceiptDevExpressPrintModel.Numeric1 = PurchaseOrderHeaderUDFDetails.Numeric1;
                purchaseOrderReceiptDevExpressPrintModel.Numeric2 = PurchaseOrderHeaderUDFDetails.Numeric2;
                purchaseOrderReceiptDevExpressPrintModel.Numeric3 = PurchaseOrderHeaderUDFDetails.Numeric3;
                purchaseOrderReceiptDevExpressPrintModel.Numeric4 = PurchaseOrderHeaderUDFDetails.Numeric4;
                purchaseOrderReceiptDevExpressPrintModel.Select1 = PurchaseOrderHeaderUDFDetails.Select1;
                purchaseOrderReceiptDevExpressPrintModel.Select2 = PurchaseOrderHeaderUDFDetails.Select2;
                purchaseOrderReceiptDevExpressPrintModel.Select3 = PurchaseOrderHeaderUDFDetails.Select3;
                purchaseOrderReceiptDevExpressPrintModel.Select4 = PurchaseOrderHeaderUDFDetails.Select4;
                purchaseOrderReceiptDevExpressPrintModel.Text1Label = PurchaseOrderHeaderUDFDetails.Text1Label;
                purchaseOrderReceiptDevExpressPrintModel.Text2Label = PurchaseOrderHeaderUDFDetails.Text2Label;
                purchaseOrderReceiptDevExpressPrintModel.Text3Label = PurchaseOrderHeaderUDFDetails.Text3Label;
                purchaseOrderReceiptDevExpressPrintModel.Text4Label = PurchaseOrderHeaderUDFDetails.Text4Label;
                purchaseOrderReceiptDevExpressPrintModel.Date1Label = PurchaseOrderHeaderUDFDetails.Date1Label;
                purchaseOrderReceiptDevExpressPrintModel.Date2Label = PurchaseOrderHeaderUDFDetails.Date2Label;
                purchaseOrderReceiptDevExpressPrintModel.Date3Label = PurchaseOrderHeaderUDFDetails.Date3Label;
                purchaseOrderReceiptDevExpressPrintModel.Date4Label = PurchaseOrderHeaderUDFDetails.Date4Label;
                purchaseOrderReceiptDevExpressPrintModel.Bit1Label = PurchaseOrderHeaderUDFDetails.Bit1Label;
                purchaseOrderReceiptDevExpressPrintModel.Bit2Label = PurchaseOrderHeaderUDFDetails.Bit2Label;
                purchaseOrderReceiptDevExpressPrintModel.Bit3Label = PurchaseOrderHeaderUDFDetails.Bit3Label;
                purchaseOrderReceiptDevExpressPrintModel.Bit4Label = PurchaseOrderHeaderUDFDetails.Bit4Label;
                purchaseOrderReceiptDevExpressPrintModel.Numeric1Label = PurchaseOrderHeaderUDFDetails.Numeric1Label;
                purchaseOrderReceiptDevExpressPrintModel.Numeric2Label = PurchaseOrderHeaderUDFDetails.Numeric2Label;
                purchaseOrderReceiptDevExpressPrintModel.Numeric3Label = PurchaseOrderHeaderUDFDetails.Numeric3Label;
                purchaseOrderReceiptDevExpressPrintModel.Numeric4Label = PurchaseOrderHeaderUDFDetails.Numeric4Label;
                purchaseOrderReceiptDevExpressPrintModel.Select1Label = PurchaseOrderHeaderUDFDetails.Select1Label;
                purchaseOrderReceiptDevExpressPrintModel.Select2Label = PurchaseOrderHeaderUDFDetails.Select2Label;
                purchaseOrderReceiptDevExpressPrintModel.Select3Label = PurchaseOrderHeaderUDFDetails.Select3Label;
                purchaseOrderReceiptDevExpressPrintModel.Select4Label = PurchaseOrderHeaderUDFDetails.Select4Label;

            }
            #endregion
        }
        private void BindCommentsTable(List<Notes> notes, ref PurchaseOrderReceiptDevExpressPrintModel purchaseOrderReceiptDevExpressPrintModel)
        {
            if (notes != null && notes.Count > 0)
            {
                foreach (var item in notes.OrderBy(n => n.NotesId))
                {
                    var note = new POCommentsDevExpressPrintModel();
                    note.PurchaseOrderId = item.ObjectId;
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
                    note.spnGlobalNote = UtilityFunction.GetMessageFromResource("spnGlobalNote", LocalizeResourceSetConstants.Global);
                    note.spnDate = UtilityFunction.GetMessageFromResource("spnDate", LocalizeResourceSetConstants.Global);
                    note.globalOwner = UtilityFunction.GetMessageFromResource("globalOwner", LocalizeResourceSetConstants.Global);
                    note.noteContent = UtilityFunction.GetMessageFromResource("noteContent", LocalizeResourceSetConstants.Global);
                    purchaseOrderReceiptDevExpressPrintModel.POCommentsDevExpressPrintModelList.Add(note);
                }
            }
        }
        #endregion

        #region V2-1089 DevExpress QRCode
        [EncryptedActionParameter]
        public ActionResult QRCodeGenerationUsingDevExpress(bool SmallLabel)
        {
            var PartIdsList = new List<string>();
            if (TempData["POReceiptQRCodePartIdList"] != null)
            {
                PartIdsList = (List<string>)TempData["POReceiptQRCodePartIdList"];
            }
            else
            {
                PartIdsList = new List<string>();
            }
            // Generate QR code report for each part in the PartIdsList
            var masterReport = new XtraReport();
            foreach (var equip in PartIdsList)
            {
                var splitArray = equip.Split(new string[] { "][" }, StringSplitOptions.None);
                if (SmallLabel)
                {
                    // Create a small QR code report
                    var report = new PartSmallQRCodeTemplate
                    {
                        DisplayName = "Part",
                        EquipmentBarCode = splitArray[0],
                        ClientLookupId = splitArray[0],
                        Description = splitArray[1],
                        PartLocation = splitArray[2],
                        IssueUnit = splitArray[3],
                    };

                    report.BindData();
                    report.CreateDocument();
                    masterReport.Pages.AddRange(report.Pages);
                }
                else
                {
                    // Create a large QR code report
                    var report = new PartLargeQRCodeTemplate
                    {
                        DisplayName = "Part",
                        EquipmentBarCode = splitArray[0],
                        ClientLookupId = splitArray[0],
                        Description = splitArray[1],
                        PartLocation = splitArray[2],
                        IssueUnit = splitArray[3],
                        MinQty = splitArray[4],
                        MaxQty = splitArray[5],
                        Manufacturer = splitArray[6],
                        ManufacturerId = splitArray[7]
                    };

                    report.BindData();
                    report.CreateDocument();
                    masterReport.Pages.AddRange(report.Pages);
                }
            }

            // Set the default file name for the QR code report
            masterReport.PrintingSystem.ExportOptions.PrintPreview.DefaultFileName = "Somax | Part QR Code";
            ViewBag.PageTitle = "Somax | Part QR";
            // Return the QR code report view
            return View("DevExpressQRCodeReportViewer", masterReport);
        }
        #endregion

        #region V2-1115
        [HttpPost]
        public JsonResult SetPartIdlistforEPM(string[] PartClientLookups)
        {
            List<string> PartIdsList = new List<string>();
            foreach (var e in PartClientLookups)
            {
                PartIdsList.Add(Convert.ToString(e));
            }
            TempData["POReceiptQRCodePartIdList"] = PartIdsList;
            return Json(new { JsonReturnEnum.success });
        }
        public ActionResult GenerateEPMPartQRcode()
        {
            var PartIdsList = new List<string>();
            if (TempData["POReceiptQRCodePartIdList"] != null)
            {
                PartIdsList = (List<string>)TempData["POReceiptQRCodePartIdList"];
            }
            // Generate QR code report for each part in the PartIdsList
            var masterReport = new XtraReport();
            foreach (var part in PartIdsList)
            {
                var splitArray = part.Split(new string[] { "][" }, StringSplitOptions.None);
                var report = new EPMPartReceiptQRCodeTemplate
                {
                    DisplayName = "Part",
                    PartBarCode = splitArray[0],
                    PartClientLookupId = splitArray[0],
                    WOClientLookupId = splitArray[8],
                    UOMConversion = splitArray[9],
                    Description = splitArray[10],
                    WOCreateBy = splitArray[11]
                };

                report.BindData();
                report.CreateDocument();
                masterReport.Pages.AddRange(report.Pages);
            }
            // Set the default file name for the QR code report
            masterReport.PrintingSystem.ExportOptions.PrintPreview.DefaultFileName = "Somax | Part QR Code";
            ViewBag.PageTitle = "Somax | Part QR";
            // Return the QR code report view
            return View("DevExpressQRCodeReportViewer", masterReport);
        }
        #endregion
    }
}