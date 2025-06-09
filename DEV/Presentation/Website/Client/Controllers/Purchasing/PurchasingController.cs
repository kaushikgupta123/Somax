using Client.BusinessWrapper.Purchase_Order;
using Client.Models.PurchaseOrder;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Client.Common;
using Client.Models;
using Rotativa;
using Client.BusinessWrapper.Common;
using Client.ActionFilters;
using Rotativa.Options;
using System.IO;
using RazorEngine;
using Client.Controllers.Common;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Configuration;
using System.Globalization;
using Client.Models.PartLookup;
using System.Net;
using System.Xml.Serialization;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Configuration.SiteSetUp;
using Client.Models.PunchoutOrderExport;
using Client.Models.PunchoutModel;
using Common.Extensions;
using System.Web;
using Client.Localization;
using Client.DevExpressReport;
using Org.BouncyCastle.Crypto;
using System.Windows.Interop;
using Database.Business;
using Client.BusinessWrapper.Configuration.ClientSetUp;
using System.Net.Http;
using DevExpress.Xpo;
using Client.Models.Common;
using Client.DevExpressReport.EPM;
using Client.Common.Constants;
using Client.Models.PurchaseOrder.UIConfiguration;

namespace Client.Controllers.Purchasing
{
    public class PurchasingController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.Purchasing)]
        public ActionResult Index()
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderModel objPurchaseOrderModel = new PurchaseOrderModel();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> FOBLookUplist = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> TermsListLookUplist = new List<DataContracts.LookupList>();
            ViewBag.IsPurchasingDetailsFromNotification = false;
            var VenlookUpList = GetLookupList_Vendor5000();
            if (VenlookUpList != null)
            {
                objPurchaseOrderModel.VendorsList = VenlookUpList.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.Vendor.ToString() });
            }
            var statuslist = commonWrapper.GetListFromConstVals("PurchaseOrderStatus");
            objPurchaseOrderModel.StatusList = statuslist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).ToList();
            string mode = Convert.ToString(TempData["Mode"]);

            if (mode == "addPurchaseOrder")
            {
                var BuyerLookUplist = GetLookUpList_Personnel();
                if (BuyerLookUplist != null)
                {
                    objPurchaseOrderModel.BuyerList = BuyerLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
                }
                var AllLookUps = commonWrapper.GetAllLookUpList();
                if (AllLookUps != null)
                {
                    FOBLookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_FOBCODE).ToList();
                    TermsListLookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_TERMS).ToList();
                }
                if (FOBLookUplist != null)
                {
                    objPurchaseOrderModel.FOBList = FOBLookUplist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
                }
                if (TermsListLookUplist != null)
                {
                    objPurchaseOrderModel.TermsList = TermsListLookUplist.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                }

                #region uiconfig
                objPurchaseOrderModel.ViewName = UiConfigConstants.PurchaseOrderAdd;  //--V2-375//
                objPurchaseOrderModel.IsExternal = false;  //--V2-375//
                CommonWrapper cWrapper = new CommonWrapper(userData);
                //var totalList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.PurchaseOrderAdd, UiConfigConstants.IsHideNone, UiConfigConstants.IsRequiredNone, UiConfigConstants.IsExternalFalse, UiConfigConstants.TargetView);
                var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.PurchaseOrderAdd, UiConfigConstants.IsExternalFalse);
                var hidList = totalList.Where(x => x.Hide == true);
                objPurchaseOrderModel.hiddenColumnList = new List<string>();
                foreach (var item in hidList)
                {
                    objPurchaseOrderModel.hiddenColumnList.Add(item.ColumnName);
                }
                var dsablList = totalList.Where(x => x.Disable == true);
                objPurchaseOrderModel.disabledColumnList = new List<string>();
                foreach (var item in dsablList)
                {
                    objPurchaseOrderModel.disabledColumnList.Add(item.ColumnName);
                }
                var reqList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
                objPurchaseOrderModel.requiredColumnList = new List<string>();
                foreach (var item in reqList)
                {
                    objPurchaseOrderModel.requiredColumnList.Add(item.ColumnName);
                }
                #endregion

                objPurchaseOrderVM.PurchaseOrderModel = objPurchaseOrderModel;
                objPurchaseOrderVM.PurchaseOrderModel.IsPurchaseOrderAdd = true;
            }
            else if (mode == "addPurchaseOrderDynamic")
            {
                List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
                AllLookUps = commonWrapper.GetAllLookUpList();
                objPurchaseOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                             .Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrder, userData);
                IList<string> LookupNames = objPurchaseOrderVM.UIConfigurationDetails.ToList()
                                                .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                                .Select(s => s.LookupName)
                                                .ToList();
                if (AllLookUps != null)
                {
                    objPurchaseOrderVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                              .Select(s => new UILookupList
                                                              { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                              .ToList();
                }
                //V2-738
                if (userData.DatabaseKey.Client.UseMultiStoreroom)
                {
                    objPurchaseOrderVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
                }
                objPurchaseOrderVM.udata = userData;
                objPurchaseOrderVM.PurchaseOrderModel = objPurchaseOrderModel;
                objPurchaseOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrder, userData);
                objPurchaseOrderVM.addPurchaseOrder = new Models.PurchaseOrder.UIConfiguration.AddPurchaseOrderModelDynamic();
                objPurchaseOrderVM.IsPurchaseOrderDynamic = true;
                var BuyerLookUplist = GetLookUpList_Personnel();
                if (BuyerLookUplist != null)
                {
                    objPurchaseOrderVM.BuyerList = BuyerLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
                }
                //V2-1086
                var ShipTolist = GetLookupList_ShipToAddress();
                if (ShipTolist != null)
                {
                    objPurchaseOrderVM.ShipToList = ShipTolist.Select(x => new SelectListItem { Text = x.ClientLookUpId, Value = x.ShipToId.ToString() });
                    objPurchaseOrderVM.ShipToClientLookupList = ShipTolist.Select(x => new SelectListItem { Text = x.ClientLookUpId, Value = x.ClientLookUpId.ToString() });
                }
            }
            else if (mode == "DetailFromPart")
            {
                //
                objPurchaseOrderVM.PurchaseOrderModel = objPurchaseOrderModel;
                long POId = Convert.ToInt64(TempData["PurchaseOrderId"]);
                string Status = Convert.ToString(TempData["Status"]);
                objPurchaseOrderVM.PurchaseOrderModel.PurchaseOrderId = POId;
                objPurchaseOrderVM.PurchaseOrderModel.IsRedirectFromPart = true;
                objPurchaseOrderVM.PurchaseOrderModel.Status = Status;
            }
            else if (mode == "DetailFromNotification")
            {
                objPurchaseOrderVM.PurchaseOrderModel = objPurchaseOrderModel;
                long PurchaseOrderLineItemId = Convert.ToInt64(TempData["PurchaseOrderLineItemId"]);
                string Status = Convert.ToString(TempData["Status"]);
                objPurchaseOrderVM.PurchaseOrderModel.Status = Status;
                objPurchaseOrderVM.PurchaseOrderModel.PurchaseOrderId = pWrapper.GetPurchaseOrderIdFromPurchaseOrderLineItemId(PurchaseOrderLineItemId);
                ViewBag.IsPurchasingDetailsFromNotification = true;
            }
            else
            {
                objPurchaseOrderModel.ScheduleWorkList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.PurchaseOrder, false);
                objPurchaseOrderModel.DateRangeDropList = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
                objPurchaseOrderModel.DateRangeDropListForCreateDate = UtilityFunction.GetTimeRangeDropForPOCreateDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-364
                objPurchaseOrderVM.PurchaseOrderModel = objPurchaseOrderModel;
            }

            #region V2-796
            var OraclePOImportInUse = commonWrapper.CheckIsActiveInterface(ApiConstants.OraclePOImport);
            objPurchaseOrderVM.OraclePOImportInUse = OraclePOImportInUse;
            #endregion


            objPurchaseOrderVM.security = userData.Security;
            objPurchaseOrderVM.udata = userData;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return View(objPurchaseOrderVM);
        }

      
        [HttpPost]
        public string GetPOGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, DateTime? CompleteStartDateVw = null,
            DateTime? CompleteEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, string PurchaseOrder = "", string Status = "", string VendorClientLookupId = "",
            string VendorName = "", DateTime? StartCreateDate = null, DateTime? EndCreateDate = null, string Attention = "", string VendorPhoneNumber = "", DateTime? StartCompleteDate = null, DateTime? EndCompleteDate = null, string Reason = "",
            string Buyer_PersonnelName = "", string TotalCost = "", string FilterValue = "", DateTime? Required = null, string txtSearchval = "",
            string Order = "2"/*, string orderDir = "asc"*/)//PO Sorting
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var filter = CustomQueryDisplayId;
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            List<PurchaseOrderModel> pOList = pWrapper.GetPurchaseOrderChunkList(CustomQueryDisplayId, CompleteStartDateVw, CompleteEndDateVw, CreateStartDateVw, CreateEndDateVw, skip, length ?? 0, Order, orderDir, PurchaseOrder, Status, VendorClientLookupId, VendorName, StartCreateDate, EndCreateDate, Attention, VendorPhoneNumber, StartCompleteDate, EndCompleteDate, Reason, Buyer_PersonnelName, TotalCost, FilterValue, txtSearchval, Required);
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

            #region uiconfig

            CommonWrapper cWrapper = new CommonWrapper(userData);
            //var hiddenList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.PurchaseOrderSearch, UiConfigConstants.IsHideTrue, UiConfigConstants.IsRequiredNone, UiConfigConstants.IsExternalNone, UiConfigConstants.TargetView).Select(x => x.ColumnName).ToList();
            var hiddenList = cWrapper.GetHiddenList(UiConfigConstants.PurchaseOrderSearch).Select(x => x.ColumnName).ToList();

            #endregion

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, hiddenColumnList = hiddenList }, JsonSerializerDateSettings);
        }

        [HttpGet]
        public string GetPOPrintData(string colname, string coldir, int CustomQueryDisplayId = 0, DateTime? CompleteStartDateVw = null, DateTime? CompleteEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, string PurchaseOrder = "", string Status = "", string VendorClientLookupId = "",
                                    string VendorName = "", DateTime? StartCreateDate = null, DateTime? EndCreateDate = null, string Attention = "", string VendorPhoneNumber = "", DateTime? StartCompleteDate = null, DateTime? EndCompleteDate = null, string Reason = "",
                                    string Buyer_PersonnelName = "", string TotalCost = "", string FilterValue = "", DateTime? Required = null, string txtSearchval = "")
        {
            List<POPrintModel> poSearchModelList = new List<POPrintModel>();
            POPrintModel objPOPrintModel;
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            List<PurchaseOrderModel> pOList = pWrapper.GetPurchaseOrderChunkList(CustomQueryDisplayId, CompleteStartDateVw, CompleteEndDateVw, CreateStartDateVw, CreateEndDateVw, 0, 100000, colname,
                coldir, PurchaseOrder, Status, VendorClientLookupId, VendorName, StartCreateDate, EndCreateDate, Attention, VendorPhoneNumber, StartCompleteDate, EndCompleteDate, Reason, Buyer_PersonnelName, TotalCost, FilterValue, txtSearchval, Required);

            foreach (var p in pOList)
            {
                objPOPrintModel = new POPrintModel();
                objPOPrintModel.ClientLookupId = p.ClientLookupId;
                objPOPrintModel.Status = p.Status;
                objPOPrintModel.VendorClientLookupId = p.VendorClientLookupId;
                objPOPrintModel.VendorName = p.VendorName;
                objPOPrintModel.CreateDate = p.CreateDate;
                objPOPrintModel.Attention = p.Attention;
                objPOPrintModel.VendorPhoneNumber = p.VendorPhoneNumber;
                objPOPrintModel.CompleteDate = p.CompleteDate;
                objPOPrintModel.Reason = p.Reason;
                objPOPrintModel.Buyer_PersonnelName = p.Buyer_PersonnelName;
                objPOPrintModel.TotalCost = p.TotalCost;
                #region V2-1171
                objPOPrintModel.Required = p.Required;
                #endregion
                poSearchModelList.Add(objPOPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = poSearchModelList }, JsonSerializerDateSettings);
        }
        [HttpPost]
        public JsonResult SetPrintData(POPrintParams pOPrintParams)
        {
            Session["POINTPARAMS"] = pOPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public ActionResult ExportASPDF(string d = "")
        {
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            PurchaseOrderPDFPrintModel purchaseOrderPDFPrintModel;
            List<PurchaseOrderPDFPrintModel> purchaseOrderPDFPrintModelList = new List<PurchaseOrderPDFPrintModel>();
            PurchaseOrderVM purchaseOrderVM = new PurchaseOrderVM();
            var locker = new object();

            POPrintParams pOPrintParams = (POPrintParams)Session["POINTPARAMS"];

            List<PurchaseOrderModel> pOList = pWrapper.GetPurchaseOrderChunkList(pOPrintParams.CustomQueryDisplayId, pOPrintParams.CompleteStartDateVw, pOPrintParams.CompleteEndDateVw, pOPrintParams.CreateStartDateVw, pOPrintParams.CreateEndDateVw, 0, 100000, pOPrintParams.colname,
               pOPrintParams.coldir, pOPrintParams.PurchaseOrder, pOPrintParams.Status, pOPrintParams.VendorClientLookupId, pOPrintParams.VendorName, pOPrintParams.StartCreateDate, pOPrintParams.EndCreateDate, pOPrintParams.Attention, pOPrintParams.VendorPhoneNumber, pOPrintParams.StartCompleteDate, pOPrintParams.EndCompleteDate, pOPrintParams.Reason, pOPrintParams.Buyer_PersonnelName, pOPrintParams.TotalCost, pOPrintParams.FilterValue, pOPrintParams.txtSearchval, pOPrintParams.Required);

            foreach (var p in pOList)
            {
                purchaseOrderPDFPrintModel = new PurchaseOrderPDFPrintModel();
                purchaseOrderPDFPrintModel.ClientLookupId = p.ClientLookupId;
                purchaseOrderPDFPrintModel.Status = UtilityFunction.GetMessageFromResource(p.Status, LocalizeResourceSetConstants.StatusDetails);
                purchaseOrderPDFPrintModel.VendorClientLookupId = p.VendorClientLookupId;
                purchaseOrderPDFPrintModel.VendorName = p.VendorName;
                if (p.CreateDate != null && p.CreateDate != default(DateTime))
                {
                    purchaseOrderPDFPrintModel.CreateDateString = p.CreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    purchaseOrderPDFPrintModel.CreateDateString = "";
                }
                purchaseOrderPDFPrintModel.Attention = p.Attention;
                purchaseOrderPDFPrintModel.VendorPhoneNumber = p.VendorPhoneNumber;
                if (p.CompleteDate != null && p.CompleteDate != default(DateTime))
                {
                    purchaseOrderPDFPrintModel.CompleteDateString = p.CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    purchaseOrderPDFPrintModel.CompleteDateString = "";
                }
                purchaseOrderPDFPrintModel.Reason = p.Reason;
                purchaseOrderPDFPrintModel.Buyer_PersonnelName = p.Buyer_PersonnelName;
                purchaseOrderPDFPrintModel.TotalCost = p.TotalCost;
                #region V2-1171
                if (p.Required != null && p.Required != default(DateTime))
                {
                    purchaseOrderPDFPrintModel.RequiredDateString = p.Required.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    purchaseOrderPDFPrintModel.RequiredDateString = "";
                }
                #endregion
                if (p.ChildCount > 0)
                {
                    purchaseOrderPDFPrintModel.LineItemModelList = pWrapper.PopulatePOLineItem(p.PurchaseOrderId);
                    purchaseOrderPDFPrintModel.Total = purchaseOrderPDFPrintModel.LineItemModelList.Sum(x => x.TotalCost);
                }
                lock (locker)
                {
                    purchaseOrderPDFPrintModelList.Add(purchaseOrderPDFPrintModel);
                }
            }
            purchaseOrderVM.purchaseOrderPDFPrintModels = purchaseOrderPDFPrintModelList;
            purchaseOrderVM.tableHaederProps = pOPrintParams.tableHaederProps;
            LocalizeControls(purchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            if (d == "d")
            {
                return new PartialViewAsPdf("POGridPdfPrintTemplate", purchaseOrderVM)
                {
                    PageSize = Size.A4,
                    FileName = "Purchase Order.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new ViewAsPdf("POGridPdfPrintTemplate", purchaseOrderVM)
                {
                    PageSize = Size.A4,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
        }
        private List<PurchaseOrderModel> GetPurchasingSearchData(List<PurchaseOrderModel> poList, string PurchaseOrder, string Status, string VendorClientLookupId,
                                string VendorName, DateTime? CreateDate, string Attention, string VendorPhoneNumber, DateTime? CompleteDate, string Reason,
                                string Buyer_PersonnelName, string TotalCost)
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
                VendorClientLookupId = VendorClientLookupId.ToUpper();
                poList = poList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorClientLookupId) && x.VendorClientLookupId.ToUpper().Contains(VendorClientLookupId))).ToList();
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
            if (!string.IsNullOrEmpty(VendorPhoneNumber))
            {
                VendorPhoneNumber = VendorPhoneNumber.ToUpper();
                poList = poList.Where(x => (!string.IsNullOrWhiteSpace(x.VendorPhoneNumber) && x.VendorPhoneNumber.ToUpper().Contains(VendorPhoneNumber))).ToList();
            }
            if (CompleteDate != null)
            {
                poList = poList.Where(x => (x.CompleteDate != null && x.CompleteDate.Value.Date.Equals(CompleteDate.Value.Date))).ToList();
            }
            if (!string.IsNullOrEmpty(Reason))
            {
                Reason = Reason.ToUpper();
                poList = poList.Where(x => (!string.IsNullOrWhiteSpace(x.Reason) && x.Reason.ToUpper().Contains(Reason))).ToList();
            }
            if (!string.IsNullOrEmpty(Buyer_PersonnelName))
            {
                Buyer_PersonnelName = Buyer_PersonnelName.ToUpper();
                poList = poList.Where(x => (!string.IsNullOrWhiteSpace(x.Buyer_PersonnelName) && x.Buyer_PersonnelName.ToUpper().Contains(Buyer_PersonnelName))).ToList();
            }
            if (!string.IsNullOrEmpty(TotalCost))
            {
                decimal number;
                if (Decimal.TryParse(TotalCost, out number))
                    poList = poList.Where(x => x.TotalCost.Equals(number)).ToList();
            }
            return poList;
        }

        public JsonResult PrintPOListFromIndex(MainPOPrintModel model)
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

        public JsonResult PrintPDFFromIndex(MainPOPrintModel model)
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
                var msSinglePDf = new MemoryStream(PDFForMailAttachment(item.PurchaseOrderId));
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
        #region details
        public PartialViewResult Details(long PurchaseOrderId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderModel objPOModel = new PurchaseOrderModel();
            PurchaseOrderEmailModel objEmailModel = new PurchaseOrderEmailModel();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> VoidLookUpList = new List<DataContracts.LookupList>();

            List<Models.EventLogModel> ListOfEvents = new List<Models.EventLogModel>();
            POHeaderUDF pOHeaderUDF = new POHeaderUDF();

            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            //objPOModel = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
            PurchaseOrder objPurchaseOrder = new PurchaseOrder();
            objPurchaseOrder = pWrapper.RetrievePOByPurchaseOrderId(PurchaseOrderId);
            objPurchaseOrderVM.UIConfigurationDetails = new List<Client.Common.UIConfigurationDetailsForModelValidation>();
            objPurchaseOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPurchaseOrderWidget, userData);
            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => objPurchaseOrderVM.attachmentCount = objCommonWrapper.AttachmentCount(PurchaseOrderId, AttachmentTableConstant.PurchaseOrder, userData.Security.Purchasing.Edit));
            tasks[1] = Task.Factory.StartNew(() => pOHeaderUDF = pWrapper.RetrievePOUDFByPurchaseOrderId(PurchaseOrderId));
            Task.WaitAll(tasks);
            objPurchaseOrderVM.ViewPurchaseOrder = new Models.PurchaseOrder.UIConfiguration.ViewPurchaseOrderModelDynamic();
            objPurchaseOrderVM.ViewPurchaseOrder = pWrapper.MapPurchaseOrderDataForView(objPurchaseOrderVM.ViewPurchaseOrder, objPurchaseOrder);
            objPurchaseOrderVM.ViewPurchaseOrder = pWrapper.MapPOHeaderUDFDataForView(objPurchaseOrderVM.ViewPurchaseOrder, pOHeaderUDF);
            objPOModel = pWrapper.initializeControls(objPurchaseOrder);
            #region TimeLine           
            #endregion
            if (objPOModel != null)
            {
                objPurchaseOrderVM.PurchaseOrderModel = objPOModel;
            }
            objPurchaseOrderVM.PurchaseOrderModel.Sec_PurchaseEdit = userData.Security.Purchasing.Edit;
            ViewBag.LineItemSecurity = userData.Security.Purchasing.Edit;
            ViewBag.LineItemStatus = objPOModel.Status;
            ViewBag.IsPunchout = objPOModel.IsPunchout;
            ViewBag.IsExternal = objPOModel.IsExternal;

            objPurchaseOrderVM.PurchaseOrderModel.PurchaseOrderSendPunchOutPOSecurity = userData.Security.Purchasing.SendPunchoutPO;
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                objPurchaseOrderVM.PurchaseOrderModel.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.Account.ToString() });
            }
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
            if (objPOModel.PurchaseOrderId > 0)
            {
                objPurchaseOrderVM.PurchaseOrderModel._POVendorCountE2V = true;
                objPurchaseOrderVM.PurchaseOrderModel.PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToLower();
                objPurchaseOrderVM.PurchaseOrderModel.UseVendorMaster = userData.Site.UseVendorMaster;
            }
            else
            {
                objPurchaseOrderVM.PurchaseOrderModel._POVendorCountE2V = pWrapper.PoVendorNoEmail(objPOModel.VendorId);
            }
            int prCount = 0;
            var list_pr = pWrapper.PopulateRequestDetails(PurchaseOrderId, ref prCount);
            if (prCount > 0)
            {
                objPurchaseOrderVM.PurchaseOrderModel._PRCountRD = true;
                objPurchaseOrderVM.PurchaseOrderModel.purchaseRequestModel = list_pr;
            }
            bool visibilityAddLineItem = (objPOModel.Status != PurchaseOrderStatusConstants.Void && objPOModel.Sec_PurchaseEdit == true);
            ViewBag.VisibilityAddLineItem = visibilityAddLineItem;
            objPurchaseOrderVM.POEmailModel = objEmailModel;
            if (objPurchaseOrderVM.PurchaseOrderModel.CcEmailId != null)
            {
                objPurchaseOrderVM.POEmailModel.CcEmailId = objPurchaseOrderVM.PurchaseOrderModel.CcEmailId;
            }

            if (objPurchaseOrderVM.PurchaseOrderModel.ToEmailId != null)
            {
                objPurchaseOrderVM.POEmailModel.ToEmailId = objPurchaseOrderVM.PurchaseOrderModel.ToEmailId;
            }
            objPurchaseOrderVM.security = userData.Security;
            //V2-738
            objPurchaseOrderVM.udata = userData;

            #region V2-796
            var OraclePOImportInUse = commonWrapper.CheckIsActiveInterface(ApiConstants.OraclePOImport);
            objPurchaseOrderVM.OraclePOImportInUse = OraclePOImportInUse;
            #endregion
            objPurchaseOrderVM.PurchaseOrderModel.SingleStockLineItem = userData.Site.SingleStockLineItem; //V2-1032

            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);

            #region uiconfig

            CommonWrapper cWrapper = new CommonWrapper(userData);
            string isExternal = "";
            if (objPOModel.IsExternal)
            {
                isExternal = UiConfigConstants.IsExternalTrue;
            }
            else
            {
                isExternal = UiConfigConstants.IsExternalFalse;
            }
            //objPurchaseOrderVM.PurchaseOrderModel.hiddenColumnList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.PurchaseOrderDetail, UiConfigConstants.IsHideTrue, UiConfigConstants.IsRequiredNone, isExternal, UiConfigConstants.TargetView).Select(x => x.ColumnName).ToList();
            objPurchaseOrderVM.PurchaseOrderModel.hiddenColumnList = cWrapper.GetHiddenList(UiConfigConstants.PurchaseOrderDetail, isExternal).Select(x => x.ColumnName).ToList();
            #endregion

            return PartialView("_PurchasingDetails", objPurchaseOrderVM);
        }

        [EncryptedActionParameter]
        public ActionResult Print(long PurchaseOrderId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            PurchaseOrderModel objPOModel = new PurchaseOrderModel();
            objPOModel = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
            objPOModel.AzureImageURL = comWrapper.GetClientLogoUrl();

            if (objPOModel.Required != null && objPOModel.Required.Value == default(DateTime))
            {
                objPOModel.Required = null;
            }
            if (objPOModel != null)
            {
                objPurchaseOrderVM.PurchaseOrderModel = objPOModel;
            }
            #region LineItems
            List<POLineItemModel> lst = new List<POLineItemModel>();
            if (userData.DatabaseKey.Client.UseMultiStoreroom) /*738*/
            {
                lst = pWrapper.PopulatePOLineItemForMultiStoreroom(PurchaseOrderId, objPOModel.StoreroomId ?? 0);
            }
            else
            {
                lst = pWrapper.PopulatePOLineItem(PurchaseOrderId);
            }
            if (lst != null)
            {
                objPurchaseOrderVM.POLineItemList = lst;
            }
            #endregion
            if (userData.DatabaseKey.Client.ClientId == 4)
            {
                string customSwitches = string.Format("--header-html  \"{0}\" " +
                                "--header-spacing \"1\" " +
                                  "--footer-html \"{1}\" " +
                                   "--footer-spacing \"8\" " +
                                   "--page-offset 0 --footer-center [page]/[toPage] --footer-font-size \"8\" " +
                                "--header-font-size \"10\" ",
                                Url.Action("Header", "Purchasing", new { id = userData.LoginAuditing.SessionId, PurchaseOrderId = PurchaseOrderId }, Request.Url.Scheme),
                                Url.Action("Footer", "Purchasing", new { id = userData.LoginAuditing.SessionId }, Request.Url.Scheme));

                LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
                return new ViewAsPdf("BBU/POImportPrintTemplate_V1", objPurchaseOrderVM)
                {
                    PageMargins = new Margins(16, 12, 21, 12),// it’s in millimeters
                    CustomSwitches = customSwitches
                };
            }
            else
            {
                string customSwitches = string.Format("--header-html  \"{0}\" " +
                                "--header-spacing \"1\" " +
                                "--header-font-size \"10\" "
                               ,
                                Url.Action("Header", "Purchasing", new { id = userData.LoginAuditing.SessionId, PurchaseOrderId = PurchaseOrderId }, Request.Url.Scheme));

                LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
                return new ViewAsPdf("PODetailPrintTemplate", objPurchaseOrderVM)
                {
                    PageMargins = new Margins(43, 12, 21, 12),// it’s in millimeters
                    CustomSwitches = customSwitches
                };
            }


        }
        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Header(string id, long PurchaseOrderId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            if (CheckLoginSession(id))
            {
                PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
                CommonWrapper comWrapper = new CommonWrapper(userData);
                PurchaseOrderModel objPOModel = new PurchaseOrderModel();
                objPOModel = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
                objPOModel.AzureImageURL = comWrapper.GetClientLogoUrl();
                if (objPOModel.Required != null && objPOModel.Required.Value == default(DateTime))
                {
                    objPOModel.Required = null;
                }
                if (objPOModel != null)
                {
                    objPurchaseOrderVM.PurchaseOrderModel = objPOModel;
                }
            }
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            if (userData.DatabaseKey.Client.ClientId == 4)
            {
                return View("BBU/PrintHeader_V1", objPurchaseOrderVM);
            }
            else
            {
                return View("PrintHeader", objPurchaseOrderVM);
            }

        }

        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Footer(string id)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            if (CheckLoginSession(id))
            {
                LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            }
            if (userData.DatabaseKey.Client.ClientId == 4)
            {
                objPurchaseOrderVM.PurchaseOrderModel = new PurchaseOrderModel();
                objPurchaseOrderVM.PurchaseOrderModel.PrintDate = DateTime.UtcNow.ToUserTimeZone(userData.Site.TimeZone).ToString("g"); ;
                return View("BBU/PrintFooter_V1", objPurchaseOrderVM);
            }
            else
            {
                return View("PrintFooter", objPurchaseOrderVM);
            }
        }

        public ActionResult GetPOInnerGrid(long PurchaseOrderID)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            objPurchaseOrderVM.POLineItemList = pWrapper.PopulatePOLineItem(PurchaseOrderID);
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return View("_InnerGridPOLineItem", objPurchaseOrderVM);
        }
        #endregion
        #region Line Item
        public string PopulateLineItem(int? draw, int? start, int? length, string searchText, long PurchaseOrderId = 0, string LineNumber = "", String PartId = "",
                                    string Description = "", decimal Quantity = 0, string PurchaseUOM = "", string UnitCost = "", string TotalCost = "", string Status = "", string Account = "", string ChargeToClientLookupId = "", long StoreroomId = 0)
        {
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            List<POLineItemModel> LineItems = new List<POLineItemModel>();
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                LineItems = pWrapper.PopulatePOLineItemForMultiStoreroom(PurchaseOrderId, StoreroomId);
            }
            else
            {
                LineItems = pWrapper.PopulatePOLineItem(PurchaseOrderId);

            }

            LineItems = this.GetLineItemsByColumnWithOrder(order, orderDir, LineItems);
            var statusList = LineItems.Select(r => r.Status_Display).GroupBy(x => x.ToString()).Select(x => x.First());
            {
                searchText = searchText.ToUpper();
                int VAL;
                bool res = int.TryParse(searchText, out VAL);
                LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.PartClientLookupId) && x.PartClientLookupId.ToUpper().Contains(searchText))
                                                        || (x.LineNumber != 0 && x.LineNumber.Equals(searchText)
                                                        || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(searchText))
                                                        || (!string.IsNullOrWhiteSpace(x.PurchaseUOM) && x.PurchaseUOM.ToUpper().Contains(searchText))
                                                        || (res == true && x.OrderQuantity.Equals(VAL))
                                                        || (res == true && x.UnitCost.Equals(VAL))
                                                        || (res == true && x.TotalCost.Equals(VAL))
                                                        || (!string.IsNullOrWhiteSpace(x.Status_Display) && x.Status_Display.ToUpper().Contains(searchText))
                                                        || (!string.IsNullOrWhiteSpace(x.AccountClientLookupId) && x.AccountClientLookupId.Equals(Account))
                                                        || (!string.IsNullOrWhiteSpace(x.ChargeToClientLookupId) && x.ChargeToClientLookupId.Equals(ChargeToClientLookupId)))
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
                if (Quantity != 0)
                {
                    LineItems = LineItems.Where(x => x.OrderQuantity.Equals(Quantity)).ToList();
                }
                if (!string.IsNullOrEmpty(PurchaseUOM))
                {
                    PurchaseUOM = PurchaseUOM.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.PurchaseUOM) && x.PurchaseUOM.ToUpper().Contains(PurchaseUOM))).ToList();
                }
                if (!string.IsNullOrEmpty(UnitCost))
                {
                    decimal number;
                    if (Decimal.TryParse(UnitCost, out number))
                        LineItems = LineItems.Where(x => x.UnitCost.Equals(number)).ToList();
                }
                if (!string.IsNullOrEmpty(TotalCost))
                {
                    decimal number;
                    if (Decimal.TryParse(TotalCost, out number))
                        LineItems = LineItems.Where(x => x.TotalCost.Equals(number)).ToList();
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    Status = Status.ToUpper();
                    LineItems = LineItems.Where(x => (!string.IsNullOrWhiteSpace(x.Status_Display) && x.Status_Display.ToUpper().Contains(Status))).ToList();
                }
                if (!string.IsNullOrEmpty(Account))
                {
                    LineItems = LineItems.Where(x => x.AccountClientLookupId.Equals(Account)).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeToClientLookupId))
                {
                    LineItems = LineItems.Where(x => x.ChargeToClientLookupId.Equals(ChargeToClientLookupId)).ToList();
                }
            }

            // LineItems.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = LineItems.Count();
            totalRecords = LineItems.Count();
            int initialPage = start.Value;
            var GrandTotalCost = LineItems.Sum(m => m.TotalCost);
            var filteredResult = LineItems
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            #region uiconfig

            CommonWrapper cWrapper = new CommonWrapper(userData);
            //var hiddenList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.PurchaseOrderLineItemSearch, UiConfigConstants.IsHideTrue, UiConfigConstants.IsRequiredNone, UiConfigConstants.IsExternalNone, UiConfigConstants.TargetView).Select(x => x.ColumnName).ToList();
            var hiddenList = cWrapper.GetHiddenList(UiConfigConstants.PurchaseOrderLineItemSearch).Select(x => x.ColumnName).ToList();

            #endregion   

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, statuslist = statusList, hiddenColumnList = hiddenList, GrandTotalCost = Math.Round(GrandTotalCost, 2) });
        }
        private List<POLineItemModel> GetLineItemsByColumnWithOrder(string order, string orderDir, List<POLineItemModel> data)
        {
            List<POLineItemModel> lst = new List<POLineItemModel>();
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
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitOfMeasure).ToList() : data.OrderBy(p => p.UnitOfMeasure).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status_Display).ToList() : data.OrderBy(p => p.Status_Display).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToClientLookupId).ToList() : data.OrderBy(p => p.ChargeToClientLookupId).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
            }
            return lst;
        }


        [HttpPost]
        public ActionResult EditLineItem(long LineItemId, long PurchaseOrderId, string POClientLookupId, bool IsPunchout = false)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataModel> acclookUpList = new List<DataModel>();
            List<DataContracts.LookupList> allLookUpList = new List<DataContracts.LookupList>();
            Task[] tasks = new Task[2];

            tasks[0] = Task.Factory.StartNew(() => acclookUpList = GetAccountByActiveState(true));
            tasks[1] = Task.Factory.StartNew(() => allLookUpList = commonWrapper.GetAllLookUpList());

            var LineItem = pWrapper.GetLineItem(LineItemId, PurchaseOrderId);
            //var PODetails = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);

            ViewBag.IsPunchout = IsPunchout;
            // 2022-Apr-21 - RKL - The edit view uses the "chuncked" lookups for this
            //  We do not need to set the LineItem.ChargeTypeLookupList
            /*
            var ChargeTypeLookUpList = PopulatelookUpListByType(LineItem.ChargeType);
            if (ChargeTypeLookUpList != null)
            {
                if (LineItem.ChargeType == "WorkOrder")
                {
                    LineItem.ChargeToList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId, Value = x.ChargeToId.ToString() });
                }
                else
                {
                    LineItem.ChargeToList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToId.ToString() });
                }
            }
            */
            if (LineItem.EstimatedDelivery != null && LineItem.EstimatedDelivery.Value == default(DateTime))
            {
                LineItem.EstimatedDelivery = null;
            }
            LineItem.PurchaseOrder_ClientLookupId = POClientLookupId;
            //V2-379
            //var AcclookUpList = GetLookupList_Account();

            var scheduleChargeTypeList = UtilityFunction.populateChargeTypeForLineItem();
            if (scheduleChargeTypeList != null)
            {
                LineItem.ChargeTypeList = scheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            Task.WaitAll(tasks);
            if (!tasks[0].IsFaulted && tasks[0].IsCompleted && !tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                #region Task1
                acclookUpList = GetAccountByActiveState(true);
                LineItem.AccountList = acclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
                #endregion

                #region Task2
                allLookUpList = commonWrapper.GetAllLookUpList();
                if (allLookUpList != null)
                {
                    List<DataContracts.LookupList> objLookup = allLookUpList.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                    if (objLookup != null)
                    {
                        LineItem.UOMList = objLookup.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                    }
                }
                #endregion

            }

            #region uiconfig
            CommonWrapper cWrapper = new CommonWrapper(userData);
            //var totalList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.PurchaseOrderLineItemEdit, UiConfigConstants.IsHideNone, UiConfigConstants.IsRequiredNone, UiConfigConstants.IsExternalNone, UiConfigConstants.TargetView);
            var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.PurchaseOrderLineItemEdit);
            var hidList = totalList.Where(x => x.Hide == true);
            LineItem.hiddenColumnList = new List<string>();
            foreach (var item in hidList)
            {
                LineItem.hiddenColumnList.Add(item.ColumnName);
            }
            var dsablList = totalList.Where(x => x.Disable == true);
            LineItem.disabledColumnList = new List<string>();
            foreach (var item in dsablList)
            {
                LineItem.disabledColumnList.Add(item.ColumnName);
            }
            var reqList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
            LineItem.requiredColumnList = new List<string>();
            foreach (var item in reqList)
            {
                LineItem.requiredColumnList.Add(item.ColumnName);
            }
            #endregion

            LineItem.ViewName = UiConfigConstants.PurchaseOrderLineItemEdit;
            objPurchaseOrderVM.lineItem = LineItem;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_EditLineItem", objPurchaseOrderVM);
        }

        [HttpGet]
        public ActionResult GetChargeToName(long _Id, string _type)
        {
            string ModelValidationFailedMessage = string.Empty;
            BusinessWrapper.EquipmentWrapper eWrapper = new BusinessWrapper.EquipmentWrapper(userData);
            BusinessWrapper.Work_Order.WorkOrderWrapper eWorkOrderWrapper = new BusinessWrapper.Work_Order.WorkOrderWrapper(userData);
            BusinessWrapper.Purchase_Order.PurchaseOrderWrapper ePOWrapper = new BusinessWrapper.Purchase_Order.PurchaseOrderWrapper(userData);
            if (_type == "Equipment")
            {
                var EquipmentDetails = eWrapper.GetEquipmentDetailsById(_Id);
                return Json(EquipmentDetails, JsonRequestBehavior.AllowGet);
            }
            else if (_type == "WorkOrder")
            {
                var WorkOrderDetails = eWorkOrderWrapper.getWorkOderDetailsById(_Id);
                return Json(WorkOrderDetails, JsonRequestBehavior.AllowGet);
            }
            else if (_type == "Account")
            {
                var AccountDetails = ePOWrapper.RetrieveaccountDetails(_Id);
                return Json(AccountDetails, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPRLineItem(PurchaseOrderVM _PurchaseOrder, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                LineItem LineItems = new LineItem()
                {
                    PurchaseOrderLineItemId = _PurchaseOrder.lineItem.PurchaseOrderLineItemId,
                    PurchaseOrderId = _PurchaseOrder.lineItem.PurchaseOrderId,
                    Description = _PurchaseOrder.lineItem.Description,
                    OrderQuantity = _PurchaseOrder.lineItem.OrderQuantity,
                    //UnitOfMeasure = _PurchaseOrder.lineItem.PurchaseUOM, // V2-553
                    AccountId = _PurchaseOrder.lineItem.AccountId,
                    UnitCost = _PurchaseOrder.lineItem.UnitCost,
                    Taxable = _PurchaseOrder.lineItem.Taxable,
                    EstimatedDelivery = _PurchaseOrder.lineItem.EstimatedDelivery,
                    PartId = _PurchaseOrder.lineItem.PartId,
                    ChargeType = _PurchaseOrder.lineItem.ChargeType,
                    ChargeToId = _PurchaseOrder.lineItem.ChargeToId,
                    ChargeTo_Name = _PurchaseOrder.lineItem.ChargeTo_Name,
                    PurchaseUOM = _PurchaseOrder.lineItem.PurchaseUOM ?? string.Empty// V2-553

                };
                PurchaseOrderLineItem poLineItem = pWrapper.UpdateLineItem(LineItems);
                if (poLineItem.ErrorMessages != null && poLineItem.ErrorMessages.Count > 0)
                {
                    return Json(poLineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PurchaseOrderId = _PurchaseOrder.lineItem.PurchaseOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult AddPartInInventory(long PurchaseOrderId, string ClientLookupId, long vendorId = 0, long StoreroomId = 0)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            partLookupVM.PurchaseOrderId = PurchaseOrderId;
            partLookupVM.ClientLookupId = ClientLookupId;
            partLookupVM.VendorId = vendorId;
            partLookupVM.StoreroomId = StoreroomId;
            partLookupVM.ShoppingCart = userData.Site.ShoppingCart;
            partLookupVM.IsOnOderCheck = userData.Site.OnOrderCheck;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/views/partlookup/indexPO.cshtml", partLookupVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPartNonInInventory(PurchaseOrderVM _PurchaseOrder, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            List<string> errorMessages = new List<string>();
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                errorMessages = pWrapper.InsertLineItem(_PurchaseOrder);
                if (errorMessages == null || errorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PurchaseOrderId = _PurchaseOrder.lineItem.PurchaseOrderId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteLineItem(long _PurchaseOrderId, long _PurchaseOrderLineItemId)
        {
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            var deleteResult = pWrapper.DeleteLineItem(_PurchaseOrderLineItemId, _PurchaseOrderId);
            if (deleteResult)
            {
                return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonReturnEnum.failed.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult AddNonPartInInventory(long PurchaseOrderId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var PODetails = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
            LineItem LineItems = new LineItem();
            LineItems.PurchaseOrderId = PurchaseOrderId;
            LineItems.PurchaseOrder_ClientLookupId = PODetails.ClientLookupId;
            var ScheduleChargeTypeList = UtilityFunction.populateChargeTypeForLineItem();
            if (ScheduleChargeTypeList != null)
            {
                LineItems.ChargeTypeList = ScheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ChargeTypeLookUpList = PopulatelookUpListByType(LineItems.ChargeType);
            if (ChargeTypeLookUpList != null)
            {
                if (LineItems.ChargeType == "WorkOrder")
                {
                    LineItems.ChargeToList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId, Value = x.ChargeToId.ToString() });
                }
                else
                {
                    LineItems.ChargeToList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToId.ToString() });
                }
            }
            //V2-379
            //var AcclookUpList = (GetLookupList_Account().Where(x => x.InactiveFlag == false)).ToList();
            var AcclookUpList = GetAccountByActiveState(true);
            LineItems.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            LineItems.AccountId = userData.Site.NonStockAccountId;//RNJ
            var AllLookUpList = commonWrapper.GetAllLookUpList();
            if (AllLookUpList != null)
            {
                List<DataContracts.LookupList> objLookup = AllLookUpList.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (objLookup != null)
                {
                    LineItems.UOMList = objLookup.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }

            #region uiconfig
            CommonWrapper cWrapper = new CommonWrapper(userData);
            //var totalList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.PurchaseOrderLineItemAdd, UiConfigConstants.IsHideNone, UiConfigConstants.IsRequiredNone, UiConfigConstants.IsExternalNone, UiConfigConstants.TargetView);
            var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.PurchaseOrderLineItemAdd);
            var hidList = totalList.Where(x => x.Hide == true);
            LineItems.hiddenColumnList = new List<string>();
            foreach (var item in hidList)
            {
                LineItems.hiddenColumnList.Add(item.ColumnName);
            }
            var dsablList = totalList.Where(x => x.Disable == true);
            LineItems.disabledColumnList = new List<string>();
            foreach (var item in dsablList)
            {
                LineItems.disabledColumnList.Add(item.ColumnName);
            }
            var reqList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
            LineItems.requiredColumnList = new List<string>();
            foreach (var item in reqList)
            {
                LineItems.requiredColumnList.Add(item.ColumnName);
            }
            LineItems.ViewName = UiConfigConstants.PurchaseOrderLineItemAdd;
            #endregion

            objPurchaseOrderVM.lineItem = LineItems;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_AddNonPartInInventory", objPurchaseOrderVM);
        }
        public string GetSelectPartsGridData(int? draw, int? start, int? length, string searchText, string PartId = "", string Description = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            var pSelectedList = pWrapper.PopulateSelectParts();
            pSelectedList = this.GetAllSelectedPartsByColumnWithOrder(order, orderDir, pSelectedList);
            if (pSelectedList != null && !string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToUpper();
                int quantity;
                bool res = int.TryParse(searchText, out quantity);
                pSelectedList = pSelectedList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(searchText))
                                                        || (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(searchText))
                                                        || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(searchText))
                                                        || (!string.IsNullOrWhiteSpace(x.ManufacturerId) && x.ManufacturerId.ToUpper().Contains(searchText))
                                                        || (!string.IsNullOrWhiteSpace(x.Manufacturer) && x.Manufacturer.ToUpper().Contains(searchText))
                                                        || (res == true && x.Quantity.Equals(Convert.ToInt32(quantity)))
                                                        ).ToList();
            }
            if (pSelectedList != null)
            {
                if (!string.IsNullOrEmpty(PartId))
                {
                    PartId = PartId.ToUpper();
                    pSelectedList = pSelectedList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(PartId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    pSelectedList = pSelectedList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = pSelectedList.Count();
            totalRecords = pSelectedList.Count();
            int initialPage = start.Value;
            var filteredResult = pSelectedList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult });
        }
        public JsonResult SavePartInInventory(List<LineItem> list)
        {
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            var result = pWrapper.UpadatePartIn(list);
            if (result != null && result.Count == 0)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), purchaserequestid = list[0].PurchaseRequestId }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region PO-Add/Edit
        public RedirectResult Add()
        {
            TempData["Mode"] = "addPurchaseOrderDynamic";/*"addPurchaseOrder";*/
            return Redirect("/Purchasing/Index?page=Procurement_Orders");
        }
        public PartialViewResult AddPO()
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            PurchaseOrderModel pModel = new PurchaseOrderModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> FOBLookUplist = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> TermsListLookUplist = new List<DataContracts.LookupList>();
            var BuyerLookUplist = GetLookUpList_Personnel();
            if (BuyerLookUplist != null)
            {
                pModel.BuyerList = BuyerLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                FOBLookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_FOBCODE).ToList();
                TermsListLookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_TERMS).ToList();
            }
            if (FOBLookUplist != null)
            {
                pModel.FOBList = FOBLookUplist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
            }
            if (TermsListLookUplist != null)
            {
                pModel.TermsList = TermsListLookUplist.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
            }
            var VenlookUpList = GetLookupList_Vendor5000();
            if (VenlookUpList != null)
            {
                pModel.VendorsList = VenlookUpList.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.Vendor.ToString() });
            }

            #region uiconfig
            pModel.ViewName = UiConfigConstants.PurchaseOrderAdd;  //--V2-375//
            pModel.IsExternal = false;  //--V2-375//
            CommonWrapper cWrapper = new CommonWrapper(userData);
            //var totalList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.PurchaseOrderAdd, UiConfigConstants.IsHideNone, UiConfigConstants.IsRequiredNone, UiConfigConstants.IsExternalFalse, UiConfigConstants.TargetView);
            var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.PurchaseOrderAdd, UiConfigConstants.IsExternalFalse);
            var hidList = totalList.Where(x => x.Hide == true);
            pModel.hiddenColumnList = new List<string>();
            foreach (var item in hidList)
            {
                pModel.hiddenColumnList.Add(item.ColumnName);
            }
            var dsablList = totalList.Where(x => x.Disable == true);
            pModel.disabledColumnList = new List<string>();
            foreach (var item in dsablList)
            {
                pModel.disabledColumnList.Add(item.ColumnName);
            }
            var reqList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
            pModel.requiredColumnList = new List<string>();
            foreach (var item in reqList)
            {
                pModel.requiredColumnList.Add(item.ColumnName);
            }
            #endregion

            objPurchaseOrderVM.PurchaseOrderModel = pModel;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("~/Views/Purchasing/_POAdd.cshtml", objPurchaseOrderVM);
        }
        public PartialViewResult EditPO(long PurchaseOrderId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            PurchaseOrderModel pModel = new PurchaseOrderModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> FOBLookUplist = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> TermsListLookUplist = new List<DataContracts.LookupList>();
            var PODetails = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
            var BuyerLookUplist = GetLookUpList_Personnel();
            if (BuyerLookUplist != null)
            {
                PODetails.BuyerList = BuyerLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                FOBLookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_FOBCODE).ToList();
                TermsListLookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_TERMS).ToList();
            }
            if (FOBLookUplist != null)
            {
                PODetails.FOBList = FOBLookUplist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
            }
            if (TermsListLookUplist != null)
            {
                PODetails.TermsList = TermsListLookUplist.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
            }
            var VenlookUpList = GetLookupList_Vendor5000();
            if (VenlookUpList != null)
            {
                PODetails.VendorsList = VenlookUpList.Select(x => new SelectListItem { Text = x.Vendor + " - " + x.Name, Value = x.Vendor.ToString() });
            }
            if (PODetails.Required != null && PODetails.Required.Value == default(DateTime))
            {
                PODetails.Required = null;
            }
            if (PODetails.CompleteDate != null && PODetails.CompleteDate.Value == default(DateTime))
            {
                PODetails.CompleteDate = null;
            }

            #region uiconfig
            PODetails.ViewName = UiConfigConstants.PurchaseOrderEdit;  //--V2-375//
            CommonWrapper cWrapper = new CommonWrapper(userData);
            string isExternal = "";
            if (PODetails.IsExternal)
            {
                isExternal = UiConfigConstants.IsExternalTrue;
            }
            else
            {
                isExternal = UiConfigConstants.IsExternalFalse;
            }
            //var totalList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.PurchaseOrderEdit, UiConfigConstants.IsHideNone, UiConfigConstants.IsRequiredNone, isExternal, UiConfigConstants.TargetView);
            var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.PurchaseOrderEdit, isExternal);
            var hidList = totalList.Where(x => x.Hide == true);
            PODetails.hiddenColumnList = new List<string>();
            foreach (var item in hidList)
            {
                PODetails.hiddenColumnList.Add(item.ColumnName);
            }
            var dsablList = totalList.Where(x => x.Disable == true);
            PODetails.disabledColumnList = new List<string>();
            foreach (var item in dsablList)
            {
                PODetails.disabledColumnList.Add(item.ColumnName);
            }
            var reqList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
            PODetails.requiredColumnList = new List<string>();
            foreach (var item in reqList)
            {
                PODetails.requiredColumnList.Add(item.ColumnName);
            }
            #endregion

            objPurchaseOrderVM.PurchaseOrderModel = PODetails;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("~/Views/Purchasing/_POAdd.cshtml", objPurchaseOrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPurchaseOrders(PurchaseOrderVM PurchaseOrderVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            var errors = string.Empty;
            if (ModelState.IsValid)
            {
                PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
                PurchaseOrder objPurchaseOrder = new DataContracts.PurchaseOrder();
                if (PurchaseOrderVM.PurchaseOrderModel.PurchaseOrderId != 0)
                {
                    objPurchaseOrder = pWrapper.EditpurchaseOrder(PurchaseOrderVM.PurchaseOrderModel);
                }
                else
                {
                    Mode = "add";
                    objPurchaseOrder = pWrapper.AddpurchaseOrder(PurchaseOrderVM.PurchaseOrderModel);
                }
                if (objPurchaseOrder.ErrorMessages != null && objPurchaseOrder.ErrorMessages.Count > 0)
                {
                    return Json(objPurchaseOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, PurchaseOrderId = objPurchaseOrder.PurchaseOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateEmailStatus(PurchaseOrderVM PurchaseOrderVM)
        {
            MemoryStream stream = new MemoryStream();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            string Msg = string.Empty;
            //dynamic ModelList = new object();
            string emailHtmlBody = string.Empty;
            string Status = "EmailToVendor";
            string VoidComntValue = "";
            if (Status != "RequestDetails" && Status != "CreatedLastUpdated")
            {
                var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Views\Shared\CommonEmailTemplate.cshtml");
                var templateContent = string.Empty;
                using (var reader = new StreamReader(templateFilePath))
                {
                    templateContent = reader.ReadToEnd();
                }
                emailHtmlBody = ParseTemplate(templateContent);
                stream = new MemoryStream(PDFForMailAttachment(PurchaseOrderVM.PurchaseOrderModel.PurchaseOrderId));
                Msg = pWrapper.UpdateStatus(emailHtmlBody, stream, PurchaseOrderVM.PurchaseOrderModel.PurchaseOrderId, Status, VoidComntValue, PurchaseOrderVM.POEmailModel.ToEmailId, PurchaseOrderVM.POEmailModel.CcEmailId, PurchaseOrderVM.POEmailModel.MailBodyComments);
            }
            //return Json(new { Result = Msg, data = ModelList }, JsonRequestBehavior.AllowGet);
            return Json(new { Result = Msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateStatus(long PurchaseOrderId, string Status, string VoidComntValue = null,
                               string ToEmail = null, string CCEmail = null, string MailBodyComments = null)
        {
            MemoryStream stream = new MemoryStream();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            string Msg = string.Empty;
            dynamic ModelList = new object();
            string emailHtmlBody = string.Empty;
            if (Status != "RequestDetails" && Status != "CreatedLastUpdated")
            {
                var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Views\Shared\CommonEmailTemplate.cshtml");
                var templateContent = string.Empty;
                using (var reader = new StreamReader(templateFilePath))
                {
                    templateContent = reader.ReadToEnd();
                }
                emailHtmlBody = ParseTemplate(templateContent);
                stream = new MemoryStream(PDFForMailAttachment(PurchaseOrderId));
                Msg = pWrapper.UpdateStatus(emailHtmlBody, stream, PurchaseOrderId, Status, VoidComntValue, ToEmail, CCEmail, MailBodyComments);
            }
            return Json(new { Result = Msg, data = ModelList }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UpdateforceCompleteStatus(long PurchaseOrderId, int lineitemcount = 0)
        {
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            string Msg = string.Empty;
            Msg = pWrapper.UpdateForceCompleteStatus(PurchaseOrderId, lineitemcount);
            return Json(new { Result = Msg }, JsonRequestBehavior.AllowGet);
        }
        public static string ParseTemplate(string content)
        {
            string _mode = DateTime.Now.Ticks + new Random().Next(1000, 100000).ToString();
            Razor.Compile(content, _mode);
            return Razor.Parse(content);
        }

        public Byte[] PDFForMailAttachment(long PurchaseOrderId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            PurchaseOrderModel objPOModel = new PurchaseOrderModel();
            objPOModel = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
            objPOModel.AzureImageURL = comWrapper.GetClientLogoUrl();

            if (objPOModel.Required != null && objPOModel.Required.Value == default(DateTime))
            {
                objPOModel.Required = null;
            }
            if (objPOModel != null)
            {
                objPurchaseOrderVM.PurchaseOrderModel = objPOModel;
            }
            #region LineItems
            List<POLineItemModel> lst = new List<POLineItemModel>();
            if (userData.DatabaseKey.Client.UseMultiStoreroom) /*738*/
            {
                lst = pWrapper.PopulatePOLineItemForMultiStoreroom(PurchaseOrderId, objPOModel.StoreroomId ?? 0);
            }
            else
            {
                lst = pWrapper.PopulatePOLineItem(PurchaseOrderId);
            }
            if (lst != null)
            {
                objPurchaseOrderVM.POLineItemList = lst;
            }
            #endregion

            Byte[] PdfData = null;
            if (userData.DatabaseKey.Client.ClientId == 4)
            {

                string customSwitches = string.Format("--header-html  \"{0}\" " +
                "--header-spacing \"1\" " +
                  "--footer-html \"{1}\" " +
                   "--footer-spacing \"8\" " +
                   "--page-offset 0 --footer-center [page]/[toPage] --footer-font-size \"8\" " +
                "--header-font-size \"10\" ",
                Url.Action("Header", "Purchasing", new { id = userData.LoginAuditing.SessionId, PurchaseOrderId = PurchaseOrderId }, Request.Url.Scheme),
                Url.Action("Footer", "Purchasing", new { id = userData.LoginAuditing.SessionId }, Request.Url.Scheme));

                LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
                var mailpdft = new ViewAsPdf("BBU/POImportPrintTemplate_V1", objPurchaseOrderVM)
                {
                    PageMargins = new Margins(16, 12, 21, 12),// it’s in millimeters
                    CustomSwitches = customSwitches
                };
                PdfData = mailpdft.BuildPdf(ControllerContext);
            }
            else
            {
                string customSwitches = string.Format("--header-html  \"{0}\" " +
                                "--header-spacing \"1\" " +
                                "--header-font-size \"10\" ",
                                Url.Action("Header", "Purchasing", new { id = userData.LoginAuditing.SessionId, PurchaseOrderId = PurchaseOrderId }, Request.Url.Scheme));

                LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
                var mailpdft = new ViewAsPdf("PODetailPrintTemplate", objPurchaseOrderVM)
                {
                    PageMargins = new Margins(43, 12, 21, 12),// it’s in millimeters
                    CustomSwitches = customSwitches
                };
                PdfData = mailpdft.BuildPdf(ControllerContext);
            }
            return PdfData;
        }
        #endregion
        #region Notes
        [HttpPost]
        public string PopulateNotes(int? draw, int? start, int? length, long PurchaseOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            var Notes = pWrapper.PopulateNotes(PurchaseOrderId);
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
        [HttpGet]
        public ActionResult AddNotes(long PurchaseOrderId, string ClientLookupId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderModel objPurchaseOrderModel = new PurchaseOrderModel();
            Models.PurchaseOrder.NotesModel notesModel = new Models.PurchaseOrder.NotesModel();
            notesModel.PurchaseOrderId = PurchaseOrderId;
            objPurchaseOrderModel.PurchaseOrderId = PurchaseOrderId;
            objPurchaseOrderModel.ClientLookupId = ClientLookupId;
            objPurchaseOrderVM.PurchaseOrderModel = objPurchaseOrderModel;
            notesModel.ClientLookupId = ClientLookupId;
            objPurchaseOrderVM.notesModel = notesModel;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_AddPONotes", objPurchaseOrderVM);
        }
        [HttpPost]
        public ActionResult AddNotes(PurchaseOrderVM _PurchaseOrder)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            List<string> errorMessages = new List<string>();
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                if (_PurchaseOrder.notesModel.NotesId == 0)
                {
                    Mode = "add";
                }
                errorMessages = pWrapper.AddPoNotes(_PurchaseOrder);
                if (errorMessages == null || errorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PurchaseOrderId = _PurchaseOrder.notesModel.PurchaseOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EditNote(long PurchaseOrderId, long NotesId, string ClientLookupId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            Models.PurchaseOrder.NotesModel objNotesModel = new Models.PurchaseOrder.NotesModel();
            PurchaseOrderModel objPurchaseOrderModel = new PurchaseOrderModel();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            objPurchaseOrderModel.PurchaseOrderId = PurchaseOrderId;
            objPurchaseOrderVM.PurchaseOrderModel = objPurchaseOrderModel;
            objPurchaseOrderModel.ClientLookupId = ClientLookupId;
            objNotesModel.ClientLookupId = ClientLookupId;
            objNotesModel = pWrapper.EditPoNotes(PurchaseOrderId, NotesId);
            objPurchaseOrderVM.notesModel = objNotesModel;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_AddPONotes", objPurchaseOrderVM);
        }
        [HttpPost]
        public ActionResult DeleteNotes(string _notesId)
        {
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            if (pWrapper.DeletePoNotes(_notesId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Attachments
        [HttpPost]
        public string PopulateAttachments(int? draw, int? start, int? length, long PurchaseOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var Attachments = objCommonWrapper.PopulateAttachments(PurchaseOrderId, "PurchaseOrder", userData.Security.Purchasing.Edit);
            if (Attachments != null)
            {
                Attachments = GetAllAttachmentsPrintWithFormSortByColumnWithOrder(order, orderDir, Attachments);
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
        [HttpGet]
        public PartialViewResult AddAttachments(long PurchaseOrderId, string ClientLookupId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            Models.PurchaseOrder.AttachmentModel Attachment = new Models.PurchaseOrder.AttachmentModel();
            Attachment.PurchaseOrderId = PurchaseOrderId;
            objPurchaseOrderVM.attachmentModel = Attachment;
            objPurchaseOrderVM.attachmentModel.ClientLookupId = ClientLookupId;
            Attachment.ClientLookupId = ClientLookupId;
            Attachment.PurchaseOrderId = PurchaseOrderId;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_AddPOAttachment", objPurchaseOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttachments()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                Mode = "add";
                Stream stream = Request.Files[0].InputStream;
                Client.Models.AttachmentModel attachmentModel = new Client.Models.AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);
                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.PurchaseOrderId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = "PurchaseOrder";
                attachmentModel.PrintwithForm = Convert.ToBoolean(Request.Form["attachmentModel.PrintwithForm"].Split(',').FirstOrDefault());
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.Purchasing.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.Purchasing.Edit);
                }
                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), PurchaseOrderId = Convert.ToInt64(Request.Form["attachmentModel.PurchaseOrderId"]), mode = Mode }, JsonRequestBehavior.AllowGet);
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
        public ActionResult EditAttachment(long PurchaseOrderId, long FileAttachmentId, string ClientLookupId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            Models.PurchaseOrder.AttachmentModel objAttachmentModel = new Models.PurchaseOrder.AttachmentModel();
            PurchaseOrderModel objPurchaseOrderModel = new PurchaseOrderModel();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            objPurchaseOrderModel.PurchaseOrderId = PurchaseOrderId;
            objPurchaseOrderVM.PurchaseOrderModel = objPurchaseOrderModel;
            objPurchaseOrderModel.ClientLookupId = ClientLookupId;
            objAttachmentModel = pWrapper.EditPoAttachment(PurchaseOrderId, FileAttachmentId);
            objAttachmentModel.ClientLookupId = ClientLookupId;
            objPurchaseOrderVM.attachmentModel = objAttachmentModel;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_EditPOAttachment", objPurchaseOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAttachments()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                Mode = "edit";
                Client.Models.AttachmentModel attachmentModel = new Client.Models.AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Boolean fileAtt = new Boolean();
                attachmentModel.AttachmentId = Convert.ToInt64(Request.Form["attachmentModel.AttachmentId"]);
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.PurchaseOrderId"]);
                attachmentModel.Description = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.PrintwithForm = Convert.ToBoolean(Request.Form["attachmentModel.PrintwithForm"].Split(',').FirstOrDefault());
                fileAtt = objCommonWrapper.EditAttachment(attachmentModel);
                if (fileAtt)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PurchaseOrderId = Convert.ToInt64(Request.Form["attachmentModel.PurchaseOrderId"]), mode = Mode }, JsonRequestBehavior.AllowGet);
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
        #region Event Log
        [HttpPost]
        public string PopulateEventLog(int? draw, int? start, int? length, long PurchaseOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            var EventLog = pWrapper.PopulateEventLog(PurchaseOrderId);
            EventLog = this.GetAllEventLogSortByColumnWithOrder(order, orderDir, EventLog);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = EventLog.Count();
            totalRecords = EventLog.Count();
            int initialPage = start.Value;
            var filteredResult = EventLog
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
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
        #region private-method
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
        private List<PartInInventoryModel> GetAllSelectedPartsByColumnWithOrder(string order, string orderDir, List<PartInInventoryModel> data)
        {
            List<PartInInventoryModel> lst = new List<PartInInventoryModel>();
            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ManufacturerId).ToList() : data.OrderBy(p => p.ManufacturerId).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Manufacturer).ToList() : data.OrderBy(p => p.Manufacturer).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        private List<Client.Models.PurchaseOrder.NotesModel> GetAllNotesSortByColumnWithOrder(string order, string orderDir, List<Client.Models.PurchaseOrder.NotesModel> data)
        {
            List<Client.Models.PurchaseOrder.NotesModel> lst = new List<Client.Models.PurchaseOrder.NotesModel>();
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
        #endregion

        #region Send Punchout Order
        [HttpPost]
        public JsonResult CreatePunchOutOrder(long PurchaseOrderId, long SiteId, long ClientId, long VendorId, long CreatedBy_PersonnelId)
        {
            VendorWrapper vWrapper = new VendorWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            SiteSetUpWrapper siteSetUpWrapper = new SiteSetUpWrapper(userData);
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            PunchoutAPIResponse punchoutAPIResponse = new PunchoutAPIResponse();

            Models.VendorsModel objVen = new Models.VendorsModel();
            Personnel personnel = new Personnel();
            Site site = new Site();

            Task[] tasks = new Task[3];
            tasks[0] = Task.Factory.StartNew(() => objVen = vWrapper.populateVendorDetails(VendorId));
            tasks[1] = Task.Factory.StartNew(() => personnel = commonWrapper.GetPersonnelByPersonnelId(CreatedBy_PersonnelId));
            tasks[2] = Task.Factory.StartNew(() => site = siteSetUpWrapper.RetriveSiteDetailsByClientAndSite(ClientId, SiteId));
            Task.WaitAll(tasks);

            if (!tasks[0].IsFaulted && tasks[0].IsCompleted && !tasks[1].IsFaulted && tasks[1].IsCompleted && !tasks[2].IsFaulted && tasks[2].IsCompleted)
            {
                var destinationURL = objVen.SendPunchoutPOURL;
                if (string.IsNullOrEmpty(objVen.SendPunchoutPOURL))
                {
                    punchoutAPIResponse.ResponseCode = 422;
                    punchoutAPIResponse.ResponseText = "Vendor Send Punchout PO URL can't be empty.";
                    return Json(punchoutAPIResponse, JsonRequestBehavior.AllowGet);
                }
                var requestToSend = pWrapper.GetPunchoutOrderMessageData(PurchaseOrderId, objVen, personnel, site);

                punchoutAPIResponse = pWrapper.postXMLData(destinationURL, requestToSend);

                if (punchoutAPIResponse.ResponseCode == 200 && punchoutAPIResponse.ResponseMessage.ToLower() == "ok")
                {
                    pWrapper.UpdatePOOnOrderSetupResponse(PurchaseOrderId, ClientId);//--- Update Code
                    pWrapper.UpdatePOEventLogOnOrderSetupResponse(PurchaseOrderId, ClientId, SiteId);
                }
            }
            return Json(punchoutAPIResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Redirect Details From Part
        public RedirectResult DetailFromPart(long PurchaseOrderId, string Status)
        {
            TempData["Mode"] = "DetailFromPart";
            string POString = Convert.ToString(PurchaseOrderId);
            TempData["PurchaseOrderId"] = POString;
            TempData["Status"] = Status;
            return Redirect("/Purchasing/Index?page=Procurement_Orders");
        }
        #endregion
        #region V2-1147 Redirect Details From Notification 
        public RedirectResult DetailFromNotification(long PurchaseOrderLineItemId, string alertName)
        {
            TempData["Mode"] = "DetailFromNotification";
            string PurchaseOrderLineItemIdString = Convert.ToString(PurchaseOrderLineItemId);
            TempData["PurchaseOrderLineItemId"] = PurchaseOrderLineItemIdString;
            TempData["alertName"] = alertName;
            return Redirect("/Purchasing/Index?page=Procurement_Orders");
        }
        #endregion
        #region V2-653 Add Purchase Order Dynamic
        [HttpGet]
        public ActionResult ShowAddPurchaseOrderDynamic()
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            objPurchaseOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrder, userData);
            objPurchaseOrderVM.addPurchaseOrder = new Models.PurchaseOrder.UIConfiguration.AddPurchaseOrderModelDynamic();
            objPurchaseOrderVM.IsPurchaseOrderDynamic = true;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objPurchaseOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                         .Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrder, userData);
            IList<string> LookupNames = objPurchaseOrderVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
            if (AllLookUps != null)
            {
                objPurchaseOrderVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }
            //V2-738
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                objPurchaseOrderVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            objPurchaseOrderVM.udata = userData;
            var BuyerLookUplist = GetLookUpList_Personnel();
            if (BuyerLookUplist != null)
            {
                objPurchaseOrderVM.BuyerList = BuyerLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            //V2-1086
            var ShipTolist = GetLookupList_ShipToAddress();
            if (ShipTolist != null)
            {
                objPurchaseOrderVM.ShipToList = ShipTolist.Select(x => new SelectListItem { Text = x.ClientLookUpId, Value = x.ShipToId.ToString() });
                objPurchaseOrderVM.ShipToClientLookupList = ShipTolist.Select(x => new SelectListItem { Text = x.ClientLookUpId, Value = x.ClientLookUpId.ToString() });
            }
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_PurchaseOrderAddDynamic", objPurchaseOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPurchaseOrdersDynamic(PurchaseOrderVM objVM, string Command)
        {
            PurchaseOrder PurchaseOrderObj = new PurchaseOrder();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string errormsg = "Error";
            string errormsgAssetMgt = "ErrorAssetMgt";
            if (ModelState.IsValid)
            {
                #region V2-1112 AddCustomEPMPO
                if (Command == "openCustomEPMPO" || Command == "openAddCustomEPMPO")
                {
                    TempData["purchasingData"] = objVM.addPurchaseOrder;
                    return Json(new { Result = "openModal" }, JsonRequestBehavior.AllowGet);

                }
                #endregion
                else
                {
                    var PurchaseOrderAddObj = pWrapper.AddPurchaseOrderDynamic(objVM);
                    PurchaseOrderObj = PurchaseOrderAddObj.Item1;
                    if (PurchaseOrderAddObj.Item2 == true)
                    {
                        return Json(new { Result = errormsg }, JsonRequestBehavior.AllowGet);
                    }
                    else if (PurchaseOrderAddObj.Item3 == true)
                    {
                        return Json(new { Result = errormsgAssetMgt }, JsonRequestBehavior.AllowGet);
                    }

                }
                if (PurchaseOrderObj.ErrorMessages != null && PurchaseOrderObj.ErrorMessages.Count > 0)
                {
                    return Json(PurchaseOrderObj.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, PurchaseOrderId = PurchaseOrderObj.PurchaseOrderId, mode = "add" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region V2-653 Edit Purchase Order Dynamic
        public PartialViewResult EditPurchaseOrdersDynamic(long PurchaseOrderId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            PurchaseOrderModel pModel = new PurchaseOrderModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            //var PODetails = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
            var BuyerLookUplist = GetLookUpList_Personnel();
            if (BuyerLookUplist != null)
            {
                objPurchaseOrderVM.BuyerList = BuyerLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objPurchaseOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                         .Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrder, userData);
            IList<string> LookupNames = objPurchaseOrderVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
            if (AllLookUps != null)
            {
                objPurchaseOrderVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }
            //V2-738
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                objPurchaseOrderVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            //V2-1086
            var ShipTolist = GetLookupList_ShipToAddress();
            if (ShipTolist != null)
            {
                objPurchaseOrderVM.ShipToList = ShipTolist.Select(x => new SelectListItem { Text = x.ClientLookUpId, Value = x.ShipToId.ToString() });
            }
            objPurchaseOrderVM.udata = userData;
            objPurchaseOrderVM.EditPurchaseOrder = pWrapper.GetPurchaseOrderByIdDynamic(PurchaseOrderId);
            objPurchaseOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                              .Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrder, userData);
            pModel.PurchaseOrderId = objPurchaseOrderVM.EditPurchaseOrder.PurchaseOrderId;
            objPurchaseOrderVM.PurchaseOrderModel = pModel;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("~/Views/Purchasing/_EditPurchaseOrderDynamic.cshtml", objPurchaseOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditPurchaseOrdersDynamic(PurchaseOrderVM objVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                DataContracts.PurchaseOrder po = new DataContracts.PurchaseOrder();
                PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
                po = pWrapper.updatePurchaseOrderDynamic(objVM);
                if (po.ErrorMessages != null && po.ErrorMessages.Count > 0)
                {
                    return Json(po.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, PurchaseOrderId = po.PurchaseOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Add Line Item (Part not in Inventory - Describe what you need)
        public PartialViewResult AddNonPartInInventoryDynamic(long PurchaseOrderId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var PODetails = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
            objPurchaseOrderVM.AddPOLineItemPartNotInInventory = new Models.PurchaseOrder.UIConfiguration.AddPOLineItemPartNotInInventoryDynamic();
            objPurchaseOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrderLineItemPartNotInInventory, userData);
            objPurchaseOrderVM.AddPOLineItemPartNotInInventory.PurchaseOrderId = PurchaseOrderId;
            objPurchaseOrderVM.AddPOLineItemPartNotInInventory.PurchaseOrder_ClientLookupId = PODetails.ClientLookupId;
            var ChargeTypeList = UtilityFunction.populateChargeTypeForLineItem();
            if (ChargeTypeList != null)
            {
                objPurchaseOrderVM.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ChargeTypeLookUpList = PopulatelookUpListByType(objPurchaseOrderVM.AddPOLineItemPartNotInInventory.ChargeType);
            //if (ChargeTypeLookUpList != null)
            //{
            //    if (objPurchaseOrderVM.AddPOLineItemPartNotInInventory.ChargeType == "WorkOrder")
            //    {
            //        objPurchaseOrderVM.ChargeToList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId, Value = x.ChargeToId.ToString() });
            //    }
            //    else
            //    {
            //        objPurchaseOrderVM.ChargeToList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToId.ToString() });
            //    }
            //}
            //var AcclookUpList = GetAccountByActiveState(true);
            //if (AcclookUpList != null)
            //{
            //    objPurchaseOrderVM.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            //}
            objPurchaseOrderVM.AddPOLineItemPartNotInInventory.AccountId = userData.Site.NonStockAccountId;//RNJ
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null && userData.Site.NonStockAccountId > 0)
            {
                AcclookUpList = AcclookUpList.Where(x => x.AccountId == userData.Site.NonStockAccountId).ToList();
                if (AcclookUpList != null)
                {
                    objPurchaseOrderVM.AddPOLineItemPartNotInInventory.AccountClientLookupId = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() }).Where(x => x.Value == userData.Site.NonStockAccountId.ToString()).First().Text;
                }
                else
                {
                    objPurchaseOrderVM.AddPOLineItemPartNotInInventory.AccountClientLookupId = "";
                }
            }
            else
            {
                objPurchaseOrderVM.AddPOLineItemPartNotInInventory.AccountClientLookupId = "";
            }
            objPurchaseOrderVM.AddPOLineItemPartNotInInventory.IsShopingCart = userData.Site.ShoppingCart; //V2-717
            var AllLookUps = commonWrapper.GetAllLookUpList();
            IList<string> LookupNames = LookupNames = objPurchaseOrderVM.UIConfigurationDetails.ToList()
                                        .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                        .Select(s => s.LookupName)
                                        .ToList();
            if (AllLookUps != null)
            {
                objPurchaseOrderVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .Select(s => new Client.Models.UILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();
            }
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_AddNonPartInInventoryDynamic", objPurchaseOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPartNonInInventoryDynamic(PurchaseOrderVM _PurchaseOrder, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            List<string> errorMessages = new List<string>();
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                errorMessages = pWrapper.AddPartNotInInventoryDynamic(_PurchaseOrder);
                if (errorMessages == null || errorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PurchaseOrderId = _PurchaseOrder.AddPOLineItemPartNotInInventory.PurchaseOrderId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region V2-653 Edit Part Not in inventory (PO Line Item)
        [HttpPost]
        public ActionResult EditPOPartNotInInventoryDynamic(long LineItemId, long PurchaseOrderId, string POClientLookupId, bool IsPunchout = false, long StoreroomId = 0)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper poWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            List<DataModel> acclookUpList = new List<DataModel>();
            List<DataContracts.LookupList> allLookUpList = new List<DataContracts.LookupList>();
            //Task[] tasks = new Task[2];
            //tasks[0] = Task.Factory.StartNew(() => acclookUpList = GetAccountByActiveState(true));
            //tasks[1] = Task.Factory.StartNew(() => allLookUpList = commonWrapper.GetAllLookUpList());

            Task[] tasks = new Task[1];
            tasks[0] = Task.Factory.StartNew(() => allLookUpList = commonWrapper.GetAllLookUpList());

            objPurchaseOrderVM.EditPOLineItemPartNotInInventory = poWrapper.GetPOLineItemNotInInventoryByIdDynamic(LineItemId, PurchaseOrderId);
            objPurchaseOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
       .Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrderLineItemPartNotInInventory, userData);
            objPurchaseOrderVM.EditPOLineItemPartNotInInventory.IsPunchout = IsPunchout;
            objPurchaseOrderVM.EditPOLineItemPartNotInInventory.IsShopingCart = userData.Site.ShoppingCart; //V2-717
            //var ChargeTypeLookUpList = PopulatelookUpListByType(objPurchaseOrderVM.EditPOLineItemPartNotInInventory.ChargeType);
            //if (ChargeTypeLookUpList != null)
            //{
            //    if (objPurchaseOrderVM.EditPOLineItemPartNotInInventory.ChargeType == "WorkOrder")
            //    {
            //        objPurchaseOrderVM.ChargeToList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId, Value = x.ChargeToId.ToString() });
            //    }
            //    else
            //    {
            //        objPurchaseOrderVM.ChargeToList = ChargeTypeLookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Name, Value = x.ChargeToId.ToString() });
            //    }
            //}
            objPurchaseOrderVM.EditPOLineItemPartNotInInventory.PurchaseOrder_ClientLookupId = POClientLookupId;
            var scheduleChargeTypeList = UtilityFunction.populateChargeTypeForLineItem();
            if (scheduleChargeTypeList != null)
            {
                objPurchaseOrderVM.ChargeTypeList = scheduleChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            Task.WaitAll(tasks);
            if (!tasks[0].IsFaulted && tasks[0].IsCompleted /*&& !tasks[1].IsFaulted && tasks[1].IsCompleted*/)
            {
                //#region Task1
                //acclookUpList = GetAccountByActiveState(true);
                //objPurchaseOrderVM.AccountList = acclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
                //#endregion

                #region Task
                IList<string> LookupName = LookupName = objPurchaseOrderVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
                if (allLookUpList != null)
                {
                    objPurchaseOrderVM.AllRequiredLookUplist = allLookUpList.Where(x => LookupName.Contains(x.ListName))
                                                          .Select(s => new Client.Models.UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
                }
                #endregion
            }
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_EditNonPartInInventoryDynamic", objPurchaseOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePOPartNotInInventoryDynamic(PurchaseOrderVM PurchaseOrderVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.UpdatePOPartNotInInventoryDynamic(PurchaseOrderVM);
                if (lineItem.ErrorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PurchaseOrderId = PurchaseOrderVM.EditPOLineItemPartNotInInventory.PurchaseOrderId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region V2-653 Edit Part in inventory (PO Line Item)
        [HttpPost]
        public ActionResult EditPOPartInInventoryDynamic(long LineItemId, long PurchaseOrderId, string POClientLookupId, bool IsPunchout = false, long StoreroomId = 0)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper poWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            List<DataModel> acclookUpList = new List<DataModel>();
            List<DataContracts.LookupList> allLookUpList = new List<DataContracts.LookupList>();
            //Task[] tasks = new Task[2];
            //tasks[0] = Task.Factory.StartNew(() => acclookUpList = GetAccountByActiveState(true));
            //tasks[1] = Task.Factory.StartNew(() => allLookUpList = commonWrapper.GetAllLookUpList());

            Task[] tasks = new Task[1];
            tasks[0] = Task.Factory.StartNew(() => allLookUpList = commonWrapper.GetAllLookUpList());

            objPurchaseOrderVM.EditPOLineItemPartInInventory = poWrapper.GetPOLineItemInInventoryByIdDynamic(LineItemId, PurchaseOrderId);
            objPurchaseOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
       .Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrderLineItemPartInInventory, userData);
            objPurchaseOrderVM.EditPOLineItemPartInInventory.IsPunchout = IsPunchout;
            objPurchaseOrderVM.EditPOLineItemPartInInventory.IsShopingCart = userData.Site.ShoppingCart; //V2-717
            Task.WaitAll(tasks);
            if (!tasks[0].IsFaulted && tasks[0].IsCompleted /* && !tasks[1].IsFaulted && tasks[1].IsCompleted*/)
            {
                //#region Task1
                //acclookUpList = GetAccountByActiveState(true);
                //objPurchaseOrderVM.AccountList = acclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
                //#endregion

                #region Task
                IList<string> LookupName = LookupName = objPurchaseOrderVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
                if (allLookUpList != null)
                {
                    objPurchaseOrderVM.AllRequiredLookUplist = allLookUpList.Where(x => LookupName.Contains(x.ListName))
                                                          .Select(s => new Client.Models.UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
                }
                #endregion
            }
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_EditPartInInventoryDynamic", objPurchaseOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePOPartInInventoryDynamic(PurchaseOrderVM PurchaseOrderVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.UpdatePOPartInInventoryDynamic(PurchaseOrderVM);
                if (lineItem.ErrorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PurchaseOrderId = PurchaseOrderVM.EditPOLineItemPartInInventory.PurchaseOrderId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region V2-796

        public PartialViewResult UpdatePODetails(long PurchaseOrderId)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            PurchaseOrderUpdateModel pModel = new PurchaseOrderUpdateModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> FOBLookUplist = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> TermsListLookUplist = new List<DataContracts.LookupList>();
            var PODetails = pWrapper.getPurchaseOderDetailsByIdForUpdateDetails(PurchaseOrderId);
            var BuyerLookUplist = GetLookUpList_Personnel();
            if (BuyerLookUplist != null)
            {
                PODetails.BuyerList = BuyerLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                FOBLookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_FOBCODE).ToList();
                TermsListLookUplist = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_TERMS).ToList();
            }
            if (FOBLookUplist != null)
            {
                PODetails.FOBList = FOBLookUplist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() });
            }
            if (TermsListLookUplist != null)
            {
                PODetails.TermsList = TermsListLookUplist.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
            }
            if (PODetails.Required != null && PODetails.Required.Value == default(DateTime))
            {
                PODetails.Required = null;
            }

            objPurchaseOrderVM.PurchaseOrderUpdateModel = PODetails;
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("~/Views/Purchasing/_UpdatePoDetailsPopup.cshtml", objPurchaseOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdatePurchaseOrdersDetails(PurchaseOrderVM PurchaseOrderVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            var errors = string.Empty;
            if (ModelState.IsValid)
            {
                PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
                PurchaseOrder objPurchaseOrder = new DataContracts.PurchaseOrder();
                objPurchaseOrder = pWrapper.UpdatepurchaseOrderDetails(PurchaseOrderVM.PurchaseOrderUpdateModel);
                if (objPurchaseOrder.ErrorMessages != null && objPurchaseOrder.ErrorMessages.Count > 0)
                {
                    return Json(objPurchaseOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, PurchaseOrderId = objPurchaseOrder.PurchaseOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-884 UnVoid
        public JsonResult UpdateUnVoidStatus(long PurchaseOrderId)
        {
            MemoryStream stream = new MemoryStream();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            string Msg = string.Empty;
            Msg = pWrapper.UpdateUnvoidStatus(PurchaseOrderId);
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region V2-946

        [HttpPost]
        public JsonResult SetPrintPoListFromIndex(MainPOPrintModel model)
        {
            List<long> PurchaseOrderIds = model.list.Select(x => x.PurchaseOrderId).ToList();
            Session["PrintPOList"] = PurchaseOrderIds;
            return Json(new { success = true });
        }
        public ActionResult GeneratePurchaseOrderPrint()
        {
            List<long> PurchaseOrderIds = new List<long>();
            if (Session["PrintPOList"] != null)
            {
                PurchaseOrderIds = (List<long>)Session["PrintPOList"];
            }
            // If PurchaseOrderEPMInvoiceImport is true, generate the EPM print model list and return the corresponding view
            if (IsPurchaseOrderEPMInvoiceImportEnabled())
            {
                var objEPMPrintModelList = EPMPrintDevExpressFromIndex(PurchaseOrderIds);
                return View("POEPMDevExpressPrint", objEPMPrintModelList);
            }
            else
            {
                // Otherwise, generate the standard print model list and return the corresponding view
                var objPrintModelList = PrintDevExpressFromIndex(PurchaseOrderIds);
                return View("PODevExpressPrint", objPrintModelList);
            }
        }
        public List<PurchaseOrderDevExpressPrintModel> PrintDevExpressFromIndex(List<long> PurchaseOrderIds)
        {
            var PurchaseOrderDevExpressPrintModelList = new List<PurchaseOrderDevExpressPrintModel>();
            var PurchaseOrderDevExpressPrintModel = new PurchaseOrderDevExpressPrintModel();
            PurchaseOrderWrapper poWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Notes> NotesList = new List<Notes>();
            var PurchaseOrderBunchListInfo = poWrapper.RetrieveAllByPOV2Print(PurchaseOrderIds);
            Parallel.ForEach(PurchaseOrderIds, p =>
            {
                var notes = commonWrapper.PopulateComment(p, "PurchaseOrder");
                NotesList.AddRange(notes);
            });

            List<PurchaseOrder> listOfPurchaseOrder = PurchaseOrderBunchListInfo.listOfPO;
            List<PurchaseOrderLineItem> listOfPurchaseOrderLineItem = PurchaseOrderBunchListInfo.listOfPOLineItem;
            List<POHeaderUDF> listOfPOHeaderUDF = PurchaseOrderBunchListInfo.listOfPOHeaderUDF;
            List<POLineUDF> listOfPOLineUDF = PurchaseOrderBunchListInfo.listOfPOLineUDF;
            List<Attachment> listOfAttachment = PurchaseOrderBunchListInfo.listOfAttachment;


            var ImageUrl = GenerateImageUrlDevExpress();// no need to call for each id as it is dependent on client id
            foreach (var item in PurchaseOrderIds)
            {
                CommonWrapper comWrapper = new CommonWrapper(userData);

                List<Models.AttachmentModel> AttachmentModelList = comWrapper.PopulateAttachmentForPrint(listOfAttachment.Where(m => m.ObjectId == item).ToList(), item, "poprint");
                PurchaseOrder PurchaseOrderDetails = listOfPurchaseOrder.Where(m => m.PurchaseOrderId == item).FirstOrDefault();
                POHeaderUDF PurchaseOrderHeaderUDFDetails = listOfPOHeaderUDF.Where(m => m.PurchaseOrderId == item).FirstOrDefault();
                List<PurchaseOrderLineItem> listOfPurchaseOrderLineItemInfo = listOfPurchaseOrderLineItem.Where(m => m.PurchaseOrderId == item).ToList();
                List<POLineUDF> listOfPOLineUDFInfo = listOfPOLineUDF.Where(m => m.PurchaseOrderId == item).ToList();
                List<Notes> listOfNotes = NotesList.Where(m => m.ObjectId == item).ToList();

                //-- binding for devexpress begin
                PurchaseOrderDevExpressPrintModel = new PurchaseOrderDevExpressPrintModel();
                BindPurchaseOrderDetails(PurchaseOrderDetails, ref PurchaseOrderDevExpressPrintModel, ImageUrl);
                BindPurchaseOrderHeaderUDFDetails(PurchaseOrderHeaderUDFDetails, ref PurchaseOrderDevExpressPrintModel);
                BindPurchaseOrderLineItemTable(listOfPurchaseOrderLineItemInfo, listOfPOLineUDFInfo, ref PurchaseOrderDevExpressPrintModel);
                BindCommentsTable(listOfNotes, ref PurchaseOrderDevExpressPrintModel);
                BindPdfAttachmentTable(AttachmentModelList, ref PurchaseOrderDevExpressPrintModel);
                ClientSetUpWrapper csWrapper = new ClientSetUpWrapper(userData);
                var formsettingdetails = csWrapper.FormSettingsDetails();
                if (formsettingdetails != null)
                {
                    PurchaseOrderDevExpressPrintModel.POUIC = formsettingdetails.POUIC;
                    PurchaseOrderDevExpressPrintModel.POLine2 = formsettingdetails.POLine2;
                    PurchaseOrderDevExpressPrintModel.POLIUIC = formsettingdetails.POLIUIC;
                    PurchaseOrderDevExpressPrintModel.POComments = formsettingdetails.POComments;
                    PurchaseOrderDevExpressPrintModel.POTandC = formsettingdetails.POTandC;
                    var attachUrl = formsettingdetails.POTandCURL;
                    if (userData.DatabaseKey.Client.OnPremise)
                    {
                        PurchaseOrderDevExpressPrintModel.SASUrl = UtilityFunction.Base64SrcDevexpress(attachUrl);
                    }
                    else
                    {
                        PurchaseOrderDevExpressPrintModel.SASUrl = comWrapper.GetSasAttachmentUrlClientForDevexpressPrint(ref attachUrl);
                    }
                }
                PurchaseOrderDevExpressPrintModel.OnPremise = userData.DatabaseKey.Client.OnPremise;
                PurchaseOrderDevExpressPrintModelList.Add(PurchaseOrderDevExpressPrintModel);
                //-- end             

            }
            return PurchaseOrderDevExpressPrintModelList;
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
        private void BindPurchaseOrderDetails(PurchaseOrder PurchaseOrderDetails, ref PurchaseOrderDevExpressPrintModel purchaseOrderDevExpressPrintModel, string AzureImageUrl)
        {
            #region Header

            purchaseOrderDevExpressPrintModel.PurchaseOrderId = PurchaseOrderDetails.PurchaseOrderId;
            purchaseOrderDevExpressPrintModel.ClientlookupId = PurchaseOrderDetails.ClientLookupId;
            purchaseOrderDevExpressPrintModel.Reason = PurchaseOrderDetails.Reason;
            purchaseOrderDevExpressPrintModel.AzureImageUrl = AzureImageUrl;
            purchaseOrderDevExpressPrintModel.Buyer_PersonnelName = PurchaseOrderDetails.Buyer_PersonnelName;
            if (PurchaseOrderDetails.CreateDate == null || PurchaseOrderDetails.CreateDate == DateTime.MinValue)
            {
                purchaseOrderDevExpressPrintModel.CreateDate = "";
            }
            else
            {
                purchaseOrderDevExpressPrintModel.CreateDate = Convert.ToDateTime(PurchaseOrderDetails.CreateDate)
                    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            if (PurchaseOrderDetails.Required == null || PurchaseOrderDetails.Required == DateTime.MinValue)
            {
                purchaseOrderDevExpressPrintModel.RequiredDate = "";
            }
            else
            {
                purchaseOrderDevExpressPrintModel.RequiredDate = Convert.ToDateTime(PurchaseOrderDetails.Required)
                    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            #region Localization

            purchaseOrderDevExpressPrintModel.GlobalPurchaseOrder = UtilityFunction.GetMessageFromResource("spnPurchaseOrder", LocalizeResourceSetConstants.Global);
            purchaseOrderDevExpressPrintModel.GlobalRequired = UtilityFunction.GetMessageFromResource("spnRequired", LocalizeResourceSetConstants.Global);
            purchaseOrderDevExpressPrintModel.globalCreateDate = UtilityFunction.GetMessageFromResource("globalCreateDate", LocalizeResourceSetConstants.Global);
            purchaseOrderDevExpressPrintModel.PoBuyer = UtilityFunction.GetMessageFromResource("spnPoBuyer", LocalizeResourceSetConstants.PurchaseOrder);
            #endregion

            #endregion
            #region Vendor/PO Header

            purchaseOrderDevExpressPrintModel.VendorName = PurchaseOrderDetails.VendorName;
            purchaseOrderDevExpressPrintModel.VendorEmailAddress = PurchaseOrderDetails.VendorEmailAddress;
            purchaseOrderDevExpressPrintModel.VendorAddress1 = PurchaseOrderDetails.VendorAddress1;
            purchaseOrderDevExpressPrintModel.VendorAddress2 = PurchaseOrderDetails.VendorAddress2;
            purchaseOrderDevExpressPrintModel.VendorAddress3 = PurchaseOrderDetails.VendorAddress3;
            purchaseOrderDevExpressPrintModel.VendorAddressCity = PurchaseOrderDetails.VendorAddressCity;
            purchaseOrderDevExpressPrintModel.VendorAddressCountry = PurchaseOrderDetails.VendorAddressCountry;
            purchaseOrderDevExpressPrintModel.VendorAddressPostCode = PurchaseOrderDetails.VendorAddressPostCode;
            purchaseOrderDevExpressPrintModel.VendorAddressState = PurchaseOrderDetails.VendorAddressState;
            purchaseOrderDevExpressPrintModel.VendorPhoneNumber = PurchaseOrderDetails.VendorPhoneNumber;
            purchaseOrderDevExpressPrintModel.SiteName = PurchaseOrderDetails.SiteName;
            purchaseOrderDevExpressPrintModel.SiteAddress1 = PurchaseOrderDetails.SiteAddress1;
            purchaseOrderDevExpressPrintModel.SiteAddress2 = PurchaseOrderDetails.SiteAddress2;
            purchaseOrderDevExpressPrintModel.SiteAddress3 = PurchaseOrderDetails.SiteAddress3;
            purchaseOrderDevExpressPrintModel.SiteAddressCity = PurchaseOrderDetails.SiteAddressCity;
            purchaseOrderDevExpressPrintModel.SiteAddressCountry = PurchaseOrderDetails.SiteAddressCountry;
            purchaseOrderDevExpressPrintModel.SiteAddressPostCode = PurchaseOrderDetails.SiteAddressPostCode;
            purchaseOrderDevExpressPrintModel.SiteAddressState = PurchaseOrderDetails.SiteAddressState;
            purchaseOrderDevExpressPrintModel.BillToName = PurchaseOrderDetails.SiteBillToName;
            purchaseOrderDevExpressPrintModel.SiteBillToAddress1 = PurchaseOrderDetails.SiteBillToAddress1;
            purchaseOrderDevExpressPrintModel.SiteBillToAddress2 = PurchaseOrderDetails.SiteBillToAddress2;
            purchaseOrderDevExpressPrintModel.SiteBillToAddress3 = PurchaseOrderDetails.SiteBillToAddress3;
            purchaseOrderDevExpressPrintModel.SiteBillToAddressCity = PurchaseOrderDetails.SiteBillToAddressCity;
            purchaseOrderDevExpressPrintModel.SiteBillToAddressCountry = PurchaseOrderDetails.SiteBillToAddressCountry;
            purchaseOrderDevExpressPrintModel.SiteBillToAddressPostCode = PurchaseOrderDetails.SiteBillToAddressPostCode;
            purchaseOrderDevExpressPrintModel.SiteBillToAddressState = PurchaseOrderDetails.SiteBillToAddressState;
            purchaseOrderDevExpressPrintModel.SiteBillToComment = PurchaseOrderDetails.SiteBillToComment;
            purchaseOrderDevExpressPrintModel.Creator_PersonnelName = PurchaseOrderDetails.Creator_PersonnelName;

            purchaseOrderDevExpressPrintModel.Carrier = PurchaseOrderDetails.Carrier;
            purchaseOrderDevExpressPrintModel.Attention = PurchaseOrderDetails.Attention;
            purchaseOrderDevExpressPrintModel.MessageToVendor = PurchaseOrderDetails.MessageToVendor;
            purchaseOrderDevExpressPrintModel.Terms = PurchaseOrderDetails.Terms;
            purchaseOrderDevExpressPrintModel.FOB = PurchaseOrderDetails.FOB;
            purchaseOrderDevExpressPrintModel.VendorCustomerAccount = PurchaseOrderDetails.VendorCustomerAccount;

            #region Localization
            //purchaseOrderDevExpressPrintModel.spnVendorPOHeader = PurchaseOrderDetails.SiteBillToAddressCountry;
            purchaseOrderDevExpressPrintModel.spnVendor = UtilityFunction.GetMessageFromResource("GlobalVendor", LocalizeResourceSetConstants.Global);
            purchaseOrderDevExpressPrintModel.GlobalShipTo = UtilityFunction.GetMessageFromResource("GlobalShipTo", LocalizeResourceSetConstants.Global);
            purchaseOrderDevExpressPrintModel.spnPOBillTo = UtilityFunction.GetMessageFromResource("spnPoBillTo", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderDevExpressPrintModel.spnFOB = UtilityFunction.GetMessageFromResource("spnFOB", LocalizeResourceSetConstants.Global);
            purchaseOrderDevExpressPrintModel.spnPoCarrier = UtilityFunction.GetMessageFromResource("spnPoCarrier", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderDevExpressPrintModel.spnPoTerms = UtilityFunction.GetMessageFromResource("spnPoTerms", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderDevExpressPrintModel.spnPoCustomerAccount = UtilityFunction.GetMessageFromResource("spnPoCustomerAccount", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderDevExpressPrintModel.spnPoAttention = UtilityFunction.GetMessageFromResource("spnPoAttention", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderDevExpressPrintModel.spnPoMessageToVendor = UtilityFunction.GetMessageFromResource("spnPoMessageToVendor", LocalizeResourceSetConstants.PurchaseOrder);
            #endregion
            #endregion
        }
        private void BindPurchaseOrderHeaderUDFDetails(POHeaderUDF PurchaseOrderHeaderUDFDetails, ref PurchaseOrderDevExpressPrintModel purchaseOrderDevExpressPrintModel)
        {
            #region Purchase Order Header UIC
            purchaseOrderDevExpressPrintModel.POHeaderUDF_POId = PurchaseOrderHeaderUDFDetails?.PurchaseOrderId ?? 0;
            if (PurchaseOrderHeaderUDFDetails != null)
            {
                purchaseOrderDevExpressPrintModel.Text1 = PurchaseOrderHeaderUDFDetails.Text1;
                purchaseOrderDevExpressPrintModel.Text2 = PurchaseOrderHeaderUDFDetails.Text2;
                purchaseOrderDevExpressPrintModel.Text3 = PurchaseOrderHeaderUDFDetails.Text3;
                purchaseOrderDevExpressPrintModel.Text4 = PurchaseOrderHeaderUDFDetails.Text4;
                if (PurchaseOrderHeaderUDFDetails.Date1 == null || PurchaseOrderHeaderUDFDetails.Date1 == default(DateTime))
                {
                    purchaseOrderDevExpressPrintModel.Date1 = "";
                }
                else
                {
                    purchaseOrderDevExpressPrintModel.Date1 = Convert.ToDateTime(PurchaseOrderHeaderUDFDetails.Date1)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (PurchaseOrderHeaderUDFDetails.Date2 == null || PurchaseOrderHeaderUDFDetails.Date2 == default(DateTime))
                {
                    purchaseOrderDevExpressPrintModel.Date2 = "";
                }
                else
                {
                    purchaseOrderDevExpressPrintModel.Date2 = Convert.ToDateTime(PurchaseOrderHeaderUDFDetails.Date2)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (PurchaseOrderHeaderUDFDetails.Date3 == null || PurchaseOrderHeaderUDFDetails.Date3 == default(DateTime))
                {
                    purchaseOrderDevExpressPrintModel.Date3 = "";
                }
                else
                {
                    purchaseOrderDevExpressPrintModel.Date3 = Convert.ToDateTime(PurchaseOrderHeaderUDFDetails.Date3)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (PurchaseOrderHeaderUDFDetails.Date4 == null || PurchaseOrderHeaderUDFDetails.Date4 == default(DateTime))
                {
                    purchaseOrderDevExpressPrintModel.Date4 = "";
                }
                else
                {
                    purchaseOrderDevExpressPrintModel.Date4 = Convert.ToDateTime(PurchaseOrderHeaderUDFDetails.Date4)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                purchaseOrderDevExpressPrintModel.Bit1 = PurchaseOrderHeaderUDFDetails.Bit1 ? "Yes" : "No";
                purchaseOrderDevExpressPrintModel.Bit2 = PurchaseOrderHeaderUDFDetails.Bit2 ? "Yes" : "No";
                purchaseOrderDevExpressPrintModel.Bit3 = PurchaseOrderHeaderUDFDetails.Bit3 ? "Yes" : "No";
                purchaseOrderDevExpressPrintModel.Bit4 = PurchaseOrderHeaderUDFDetails.Bit4 ? "Yes" : "No";
                purchaseOrderDevExpressPrintModel.Numeric1 = PurchaseOrderHeaderUDFDetails.Numeric1;
                purchaseOrderDevExpressPrintModel.Numeric2 = PurchaseOrderHeaderUDFDetails.Numeric2;
                purchaseOrderDevExpressPrintModel.Numeric3 = PurchaseOrderHeaderUDFDetails.Numeric3;
                purchaseOrderDevExpressPrintModel.Numeric4 = PurchaseOrderHeaderUDFDetails.Numeric4;
                purchaseOrderDevExpressPrintModel.Select1 = PurchaseOrderHeaderUDFDetails.Select1;
                purchaseOrderDevExpressPrintModel.Select2 = PurchaseOrderHeaderUDFDetails.Select2;
                purchaseOrderDevExpressPrintModel.Select3 = PurchaseOrderHeaderUDFDetails.Select3;
                purchaseOrderDevExpressPrintModel.Select4 = PurchaseOrderHeaderUDFDetails.Select4;
                purchaseOrderDevExpressPrintModel.Text1Label = PurchaseOrderHeaderUDFDetails.Text1Label;
                purchaseOrderDevExpressPrintModel.Text2Label = PurchaseOrderHeaderUDFDetails.Text2Label;
                purchaseOrderDevExpressPrintModel.Text3Label = PurchaseOrderHeaderUDFDetails.Text3Label;
                purchaseOrderDevExpressPrintModel.Text4Label = PurchaseOrderHeaderUDFDetails.Text4Label;
                purchaseOrderDevExpressPrintModel.Date1Label = PurchaseOrderHeaderUDFDetails.Date1Label;
                purchaseOrderDevExpressPrintModel.Date2Label = PurchaseOrderHeaderUDFDetails.Date2Label;
                purchaseOrderDevExpressPrintModel.Date3Label = PurchaseOrderHeaderUDFDetails.Date3Label;
                purchaseOrderDevExpressPrintModel.Date4Label = PurchaseOrderHeaderUDFDetails.Date4Label;
                purchaseOrderDevExpressPrintModel.Bit1Label = PurchaseOrderHeaderUDFDetails.Bit1Label;
                purchaseOrderDevExpressPrintModel.Bit2Label = PurchaseOrderHeaderUDFDetails.Bit2Label;
                purchaseOrderDevExpressPrintModel.Bit3Label = PurchaseOrderHeaderUDFDetails.Bit3Label;
                purchaseOrderDevExpressPrintModel.Bit4Label = PurchaseOrderHeaderUDFDetails.Bit4Label;
                purchaseOrderDevExpressPrintModel.Numeric1Label = PurchaseOrderHeaderUDFDetails.Numeric1Label;
                purchaseOrderDevExpressPrintModel.Numeric2Label = PurchaseOrderHeaderUDFDetails.Numeric2Label;
                purchaseOrderDevExpressPrintModel.Numeric3Label = PurchaseOrderHeaderUDFDetails.Numeric3Label;
                purchaseOrderDevExpressPrintModel.Numeric4Label = PurchaseOrderHeaderUDFDetails.Numeric4Label;
                purchaseOrderDevExpressPrintModel.Select1Label = PurchaseOrderHeaderUDFDetails.Select1Label;
                purchaseOrderDevExpressPrintModel.Select2Label = PurchaseOrderHeaderUDFDetails.Select2Label;
                purchaseOrderDevExpressPrintModel.Select3Label = PurchaseOrderHeaderUDFDetails.Select3Label;
                purchaseOrderDevExpressPrintModel.Select4Label = PurchaseOrderHeaderUDFDetails.Select4Label;

            }
            #endregion
        }

        private void BindPurchaseOrderLineItemTable(List<PurchaseOrderLineItem> listOfPurchaseOrderLineItemInfo, List<POLineUDF> listOfPOLineUDF, ref PurchaseOrderDevExpressPrintModel purchaseOrderDevExpressPrintModel)
        {
            if (listOfPurchaseOrderLineItemInfo.Count > 0)
            {
                foreach (var item in listOfPurchaseOrderLineItemInfo)
                {
                    var objPOLineItemDevExpressPrintModel = new POLineItemDevExpressPrintModel();

                    objPOLineItemDevExpressPrintModel.LineNumber = item.LineNumber;
                    objPOLineItemDevExpressPrintModel.PartClientLookupId = item.PartClientLookupId;
                    objPOLineItemDevExpressPrintModel.Description = item.Description;
                    objPOLineItemDevExpressPrintModel.OrderQuantity = Math.Round(item.OrderQuantity, 2);
                    objPOLineItemDevExpressPrintModel.UnitOfMeasure = item.UnitOfMeasure;
                    objPOLineItemDevExpressPrintModel.UnitCost = Math.Round(item.UnitCost, 2);
                    objPOLineItemDevExpressPrintModel.TotalCost = Math.Round(item.TotalCost, 2);
                    objPOLineItemDevExpressPrintModel.Status_Display = item.Status_Display;
                    objPOLineItemDevExpressPrintModel.ChargeToClientLookupId = item.ChargeToClientLookupId;
                    objPOLineItemDevExpressPrintModel.AccountClientLookupId = item.AccountClientLookupId;
                    objPOLineItemDevExpressPrintModel.Manufacturer = item.Part_Manufacturer;
                    objPOLineItemDevExpressPrintModel.ManufacturerID = item.Part_ManufacturerID;
                    #region Localization
                    objPOLineItemDevExpressPrintModel.globalLineItems = UtilityFunction.GetMessageFromResource("spnLineItems", LocalizeResourceSetConstants.Global);
                    objPOLineItemDevExpressPrintModel.globalLine = UtilityFunction.GetMessageFromResource("globalLine", LocalizeResourceSetConstants.Global);
                    objPOLineItemDevExpressPrintModel.spnPartID = UtilityFunction.GetMessageFromResource("spnPartID", LocalizeResourceSetConstants.Global);
                    objPOLineItemDevExpressPrintModel.spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global);
                    objPOLineItemDevExpressPrintModel.spnQty = UtilityFunction.GetMessageFromResource("spnQty", LocalizeResourceSetConstants.PurchaseOrder);
                    objPOLineItemDevExpressPrintModel.spnUOM = UtilityFunction.GetMessageFromResource("spnPdUOM", LocalizeResourceSetConstants.Global);
                    objPOLineItemDevExpressPrintModel.spnPrice = UtilityFunction.GetMessageFromResource("spnPoPrice", LocalizeResourceSetConstants.PurchaseOrder);
                    objPOLineItemDevExpressPrintModel.spnTotal = UtilityFunction.GetMessageFromResource("GlobalTotal", LocalizeResourceSetConstants.Global);
                    objPOLineItemDevExpressPrintModel.GlobalStatus = UtilityFunction.GetMessageFromResource("GlobalStatus", LocalizeResourceSetConstants.Global);
                    objPOLineItemDevExpressPrintModel.GlobalChargeTo = UtilityFunction.GetMessageFromResource("GlobalChargeTo", LocalizeResourceSetConstants.Global);

                    objPOLineItemDevExpressPrintModel.GlobalAccount = UtilityFunction.GetMessageFromResource("GlobalAccount", LocalizeResourceSetConstants.Global);
                    objPOLineItemDevExpressPrintModel.GlobalManufacturer = UtilityFunction.GetMessageFromResource("spnManufacturer", LocalizeResourceSetConstants.Global);
                    objPOLineItemDevExpressPrintModel.GlobalManufacturerID = UtilityFunction.GetMessageFromResource("spnManufacturerID", LocalizeResourceSetConstants.Global);
                    objPOLineItemDevExpressPrintModel.spnPoGrandtotal = UtilityFunction.GetMessageFromResource("spnPoGrandtotal", LocalizeResourceSetConstants.PurchaseOrder);

                    #endregion                
                    POLineUDF POLineUDFDetails = listOfPOLineUDF.Where(m => m.PurchaseOrderLineItemId == item.PurchaseOrderLineItemId).FirstOrDefault();
                    BindPOLineUDFDetails(POLineUDFDetails, ref objPOLineItemDevExpressPrintModel);
                    purchaseOrderDevExpressPrintModel.POLineItemDevExpressPrintModelList.Add(objPOLineItemDevExpressPrintModel);
                }
            }
        }
        private void BindPOLineUDFDetails(POLineUDF POLineUDFDetails, ref POLineItemDevExpressPrintModel pOLineItemDevExpressPrintModel)
        {
            pOLineItemDevExpressPrintModel.POLineUDF_POLIId = POLineUDFDetails?.PurchaseOrderLineItemId ?? 0;
            pOLineItemDevExpressPrintModel.POLineUDF_POId = POLineUDFDetails?.PurchaseOrderId ?? 0;
            if (POLineUDFDetails != null)
            {
                pOLineItemDevExpressPrintModel.Text1 = POLineUDFDetails.Text1;
                pOLineItemDevExpressPrintModel.Text2 = POLineUDFDetails.Text2;
                pOLineItemDevExpressPrintModel.Text3 = POLineUDFDetails.Text3;
                pOLineItemDevExpressPrintModel.Text4 = POLineUDFDetails.Text4;
                if (POLineUDFDetails.Date1 == null || POLineUDFDetails.Date1 == default(DateTime))
                {
                    pOLineItemDevExpressPrintModel.Date1 = "";
                }
                else
                {
                    pOLineItemDevExpressPrintModel.Date1 = Convert.ToDateTime(POLineUDFDetails.Date1)
                            .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (POLineUDFDetails.Date2 == null || POLineUDFDetails.Date2 == default(DateTime))
                {
                    pOLineItemDevExpressPrintModel.Date2 = "";
                }
                else
                {
                    pOLineItemDevExpressPrintModel.Date2 = Convert.ToDateTime(POLineUDFDetails.Date2)
                            .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (POLineUDFDetails.Date3 == null || POLineUDFDetails.Date3 == default(DateTime))
                {
                    pOLineItemDevExpressPrintModel.Date3 = "";
                }
                else
                {
                    pOLineItemDevExpressPrintModel.Date3 = Convert.ToDateTime(POLineUDFDetails.Date3)
                            .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (POLineUDFDetails.Date4 == null || POLineUDFDetails.Date4 == default(DateTime))
                {
                    pOLineItemDevExpressPrintModel.Date4 = "";
                }
                else
                {
                    pOLineItemDevExpressPrintModel.Date4 = Convert.ToDateTime(POLineUDFDetails.Date4)
                            .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                pOLineItemDevExpressPrintModel.Bit1 = POLineUDFDetails.Bit1 ? "Yes" : "No";
                pOLineItemDevExpressPrintModel.Bit2 = POLineUDFDetails.Bit2 ? "Yes" : "No";
                pOLineItemDevExpressPrintModel.Bit3 = POLineUDFDetails.Bit3 ? "Yes" : "No";
                pOLineItemDevExpressPrintModel.Bit4 = POLineUDFDetails.Bit4 ? "Yes" : "No";
                pOLineItemDevExpressPrintModel.Numeric1 = POLineUDFDetails.Numeric1;
                pOLineItemDevExpressPrintModel.Numeric2 = POLineUDFDetails.Numeric2;
                pOLineItemDevExpressPrintModel.Numeric3 = POLineUDFDetails.Numeric3;
                pOLineItemDevExpressPrintModel.Numeric4 = POLineUDFDetails.Numeric4;
                pOLineItemDevExpressPrintModel.Select1 = POLineUDFDetails.Select1;
                pOLineItemDevExpressPrintModel.Select2 = POLineUDFDetails.Select2;
                pOLineItemDevExpressPrintModel.Select3 = POLineUDFDetails.Select3;
                pOLineItemDevExpressPrintModel.Select4 = POLineUDFDetails.Select4;
                pOLineItemDevExpressPrintModel.Text1Label = POLineUDFDetails.Text1Label;
                pOLineItemDevExpressPrintModel.Text2Label = POLineUDFDetails.Text2Label;
                pOLineItemDevExpressPrintModel.Text3Label = POLineUDFDetails.Text3Label;
                pOLineItemDevExpressPrintModel.Text4Label = POLineUDFDetails.Text4Label;
                pOLineItemDevExpressPrintModel.Date1Label = POLineUDFDetails.Date1Label;
                pOLineItemDevExpressPrintModel.Date2Label = POLineUDFDetails.Date2Label;
                pOLineItemDevExpressPrintModel.Date3Label = POLineUDFDetails.Date3Label;
                pOLineItemDevExpressPrintModel.Date4Label = POLineUDFDetails.Date4Label;
                pOLineItemDevExpressPrintModel.Bit1Label = POLineUDFDetails.Bit1Label;
                pOLineItemDevExpressPrintModel.Bit2Label = POLineUDFDetails.Bit2Label;
                pOLineItemDevExpressPrintModel.Bit3Label = POLineUDFDetails.Bit3Label;
                pOLineItemDevExpressPrintModel.Bit4Label = POLineUDFDetails.Bit4Label;
                pOLineItemDevExpressPrintModel.Numeric1Label = POLineUDFDetails.Numeric1Label;
                pOLineItemDevExpressPrintModel.Numeric2Label = POLineUDFDetails.Numeric2Label;
                pOLineItemDevExpressPrintModel.Numeric3Label = POLineUDFDetails.Numeric3Label;
                pOLineItemDevExpressPrintModel.Numeric4Label = POLineUDFDetails.Numeric4Label;
                pOLineItemDevExpressPrintModel.Select1Label = POLineUDFDetails.Select1Label;
                pOLineItemDevExpressPrintModel.Select2Label = POLineUDFDetails.Select2Label;
                pOLineItemDevExpressPrintModel.Select3Label = POLineUDFDetails.Select3Label;
                pOLineItemDevExpressPrintModel.Select4Label = POLineUDFDetails.Select4Label;

            }
        }
        private void BindCommentsTable(List<Notes> notes, ref PurchaseOrderDevExpressPrintModel purchaseOrderDevExpressPrintModel)
        {
            if (notes != null && notes.Count > 0)
            {
                foreach (var item in notes.OrderBy(n => n.ObjectId))
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
                    purchaseOrderDevExpressPrintModel.POCommentsDevExpressPrintModelList.Add(note);
                }
            }
        }
        [HttpPost]
        public ActionResult UpdateEmailStatusDevexpresss(PurchaseOrderVM PurchaseOrderVM)
        {
            PurchaseOrderWrapper poWrapper = new PurchaseOrderWrapper(userData);
            MemoryStream stream = new MemoryStream();
            string Status = "EmailToVendor";
            string VoidComntValue = "";
            string emailHtmlBody = string.Empty;
            var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Views\Shared\CommonEmailTemplate.cshtml");
            var templateContent = string.Empty;
            using (var reader = new StreamReader(templateFilePath))
            {
                templateContent = reader.ReadToEnd();
            }
            emailHtmlBody = ParseTemplate(templateContent);
            List<long> PurchaseOrderIds = new List<long>();
            PurchaseOrderIds.Add(PurchaseOrderVM.PurchaseOrderModel.PurchaseOrderId);
            // If PurchaseOrderEPMInvoiceImport is true, generate the EPM print model list and return the corresponding view
            if (IsPurchaseOrderEPMInvoiceImportEnabled())
            {
                EPMPurchaseOrderPrintTemplate epmreport = new EPMPurchaseOrderPrintTemplate();
                var objEPMPrintModelList = EPMPrintDevExpressFromIndex(PurchaseOrderIds);
                epmreport.DataSource = objEPMPrintModelList;
                epmreport.ExportToPdf(stream);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                poWrapper.UpdateStatus(emailHtmlBody, stream, PurchaseOrderVM.PurchaseOrderModel.PurchaseOrderId, Status, VoidComntValue, PurchaseOrderVM.POEmailModel.ToEmailId, PurchaseOrderVM.POEmailModel.CcEmailId, PurchaseOrderVM.POEmailModel.MailBodyComments);
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                PurchaseOrderPrintTemplate report = new PurchaseOrderPrintTemplate();
                var objPrintModelList = PrintDevExpressFromIndex(PurchaseOrderIds);
                report.DataSource = objPrintModelList;
                report.ExportToPdf(stream);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                poWrapper.UpdateStatus(emailHtmlBody, stream, PurchaseOrderVM.PurchaseOrderModel.PurchaseOrderId, Status, VoidComntValue, PurchaseOrderVM.POEmailModel.ToEmailId, PurchaseOrderVM.POEmailModel.CcEmailId, PurchaseOrderVM.POEmailModel.MailBodyComments);
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region EPM PO DEVEXPRESS PRINT V2-1112 
        private bool IsPurchaseOrderEPMInvoiceImportEnabled()
        {
            // Check if the PurchaseOrderEPMInvoiceImport flag is set to false initially
            bool PurchaseOrderEPMInvoiceImport = false;

            // Retrieve the InterfacePropData from the session and cast it to a list of InterfacePropModel
            var InterfacePropData = (List<InterfacePropModel>)Session["InterfacePropData"];

            // Check if InterfacePropData is not null and contains elements
            if (InterfacePropData != null && InterfacePropData.Count > 0)
            {
                // Set the PurchaseOrderEPMInvoiceImport flag based on the InterfaceType and InUse properties
                PurchaseOrderEPMInvoiceImport = InterfacePropData
                    .Where(x => x.InterfaceType == InterfacePropConstants.EPMInvoiceImport)
                    .Select(x => x.InUse)
                    .FirstOrDefault();
            }
            return PurchaseOrderEPMInvoiceImport;
        }
        public List<PurchaseOrderEPMDevExpressPrintModel> EPMPrintDevExpressFromIndex(List<long> PurchaseOrderIds)
        {
            var PurchaseOrderEPMDevExpressPrintModelList = new List<PurchaseOrderEPMDevExpressPrintModel>();
            var PurchaseOrderEPMDevExpressPrintModel = new PurchaseOrderEPMDevExpressPrintModel();
            PurchaseOrderWrapper poWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            // List to store notes
            List<Notes> NotesList = new List<Notes>();
            // Retrieve purchase order information
            var PurchaseOrderEPMBunchListInfo = poWrapper.RetrieveByEPMPOPrintV2(PurchaseOrderIds);
            // Parallel processing of purchase order IDs to populate comments
            Parallel.ForEach(PurchaseOrderIds, p =>
            {
                // Populate comments for each purchase order
                var notes = commonWrapper.PopulateComment(p, "PurchaseOrder");
                // Add the retrieved notes to the list
                NotesList.AddRange(notes);
            });

            List<PurchaseOrder> listOfPurchaseOrderEPM = PurchaseOrderEPMBunchListInfo.listOfPO;
            List<PurchaseOrderLineItem> listOfPurchaseOrderEPMLineItem = PurchaseOrderEPMBunchListInfo.listOfPOLineItem;

            foreach (var item in PurchaseOrderIds)
            {
                CommonWrapper comWrapper = new CommonWrapper(userData);

                PurchaseOrder PurchaseOrderDetails = listOfPurchaseOrderEPM.Where(m => m.PurchaseOrderId == item).FirstOrDefault();
                List<PurchaseOrderLineItem> listOfPurchaseOrderEPMLineItemInfo = listOfPurchaseOrderEPMLineItem.Where(m => m.PurchaseOrderId == item).ToList();
                List<Notes> listOfNotes = NotesList.Where(m => m.ObjectId == item).ToList();

                //-- binding for devexpress begin
                PurchaseOrderEPMDevExpressPrintModel = new PurchaseOrderEPMDevExpressPrintModel();
                BindPurchaseOrderEPMDetails(PurchaseOrderDetails, ref PurchaseOrderEPMDevExpressPrintModel);
                BindPurchaseOrderEPMLineItemTable(listOfPurchaseOrderEPMLineItemInfo, ref PurchaseOrderEPMDevExpressPrintModel);
                BindCommentsEPMTable(listOfNotes, ref PurchaseOrderEPMDevExpressPrintModel);
                PurchaseOrderEPMDevExpressPrintModelList.Add(PurchaseOrderEPMDevExpressPrintModel);
                //-- end             

            }
            return PurchaseOrderEPMDevExpressPrintModelList;
        }
        private void BindPurchaseOrderEPMDetails(PurchaseOrder PurchaseOrderDetails, ref PurchaseOrderEPMDevExpressPrintModel purchaseOrderEPMDevExpressPrintModel)
        {
            #region Vendor/PO Header
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string resetUrl = commonWrapper.GetHostedUrl();
            purchaseOrderEPMDevExpressPrintModel.eastPenn_immagePath = resetUrl + EPMPurchaseOrderConstants.EastPenn.eastPenn_immagePath;
            purchaseOrderEPMDevExpressPrintModel.PurchaseOrderId = PurchaseOrderDetails.PurchaseOrderId;
            purchaseOrderEPMDevExpressPrintModel.ClientlookupId = PurchaseOrderDetails.ClientLookupId;
            if (PurchaseOrderDetails.CreateDate == null || PurchaseOrderDetails.CreateDate == DateTime.MinValue)
            {
                purchaseOrderEPMDevExpressPrintModel.CreateDate = "";
            }
            else
            {
                purchaseOrderEPMDevExpressPrintModel.CreateDate = Convert.ToDateTime(PurchaseOrderDetails.CreateDate)
                    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            if (PurchaseOrderDetails.Status == "Complete")
            {
                purchaseOrderEPMDevExpressPrintModel.Status = EPMPurchaseOrderConstants.EastPenn.eastPenn_status;
            }
            else
            {
                purchaseOrderEPMDevExpressPrintModel.Status = PurchaseOrderDetails.Status.ToUpper();
            }
            purchaseOrderEPMDevExpressPrintModel.Number = PurchaseOrderDetails?.ClientLookupId?.ToUpper();
            if (PurchaseOrderDetails.StatusDate == null || PurchaseOrderDetails.StatusDate == DateTime.MinValue)
            {
                purchaseOrderEPMDevExpressPrintModel.SDate = Convert.ToDateTime(DateTime.UtcNow)
                    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture); ;
            }
            else
            {
                purchaseOrderEPMDevExpressPrintModel.SDate = Convert.ToDateTime(PurchaseOrderDetails.StatusDate)
                    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            purchaseOrderEPMDevExpressPrintModel.StoreroomId = PurchaseOrderDetails.StoreroomId;
            if (PurchaseOrderDetails.StoreroomId == 074)
            {

                purchaseOrderEPMDevExpressPrintModel.CompanyAddress_Line1 = EPMPurchaseOrderConstants.EastPenn.CompanyAddress_Line1_Storeroom074;
                purchaseOrderEPMDevExpressPrintModel.CompanyAddress_Line2 = EPMPurchaseOrderConstants.EastPenn.CompanyAddress_Line2_Storeroom074;
                purchaseOrderEPMDevExpressPrintModel.CompanyAddress_Line3 = EPMPurchaseOrderConstants.EastPenn.CompanyAddress_Line3_Storeroom074;
            }
            else
            {
                purchaseOrderEPMDevExpressPrintModel.CompanyAddress_Line1 = EPMPurchaseOrderConstants.EastPenn.CompanyAddress_Line1_StoreroomOthers;
                purchaseOrderEPMDevExpressPrintModel.CompanyAddress_Line2 = EPMPurchaseOrderConstants.EastPenn.CompanyAddress_Line2_StoreroomOthers;
                purchaseOrderEPMDevExpressPrintModel.CompanyAddress_Line3 = EPMPurchaseOrderConstants.EastPenn.CompanyAddress_Line3_StoreroomOthers;
            }
            purchaseOrderEPMDevExpressPrintModel.ShipTo = PurchaseOrderDetails.Shipto;
            purchaseOrderEPMDevExpressPrintModel.ShipToClientLookupId = PurchaseOrderDetails?.Shipto_ClientLookUpId;
            purchaseOrderEPMDevExpressPrintModel.ShipToAddress1 = PurchaseOrderDetails?.ShipToAddress1?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.ShipToAddress2 = PurchaseOrderDetails?.ShipToAddress2?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.ShipToAddress3 = PurchaseOrderDetails?.ShipToAddress3?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.ShipToAddressCity = PurchaseOrderDetails?.ShipToAddressCity?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.ShipToAddressState = PurchaseOrderDetails?.ShipToAddressState?.ToLower();
            purchaseOrderEPMDevExpressPrintModel.ShipToAddressPostCode = PurchaseOrderDetails?.ShipToAddressPostCode?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.ShipToAddressCountry = PurchaseOrderDetails?.ShipToAddressCountry?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.VendorName = PurchaseOrderDetails?.VendorName?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.VendorAddress1 = PurchaseOrderDetails?.VendorAddress1?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.VendorAddress2 = PurchaseOrderDetails?.VendorAddress2?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.VendorAddress3 = PurchaseOrderDetails?.VendorAddress3?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.VendorAddressCity = PurchaseOrderDetails?.VendorAddressCity?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.VendorAddressState = PurchaseOrderDetails?.VendorAddressState?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.VendorAddressPostCode = PurchaseOrderDetails?.VendorAddressPostCode?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.VendorAddressCountry = PurchaseOrderDetails?.VendorAddressCountry?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.SiteName = PurchaseOrderDetails?.SiteName?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.SiteBillToName = PurchaseOrderDetails?.SiteBillToName?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.SiteAddress1 = PurchaseOrderDetails?.SiteAddress1?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.SiteAddress2 = PurchaseOrderDetails?.SiteAddress2?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.SiteAddress3 = PurchaseOrderDetails?.SiteAddress3?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.SiteAddressCity = PurchaseOrderDetails?.SiteAddressCity?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.SiteAddressState = PurchaseOrderDetails?.SiteAddressState?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.SiteAddressPostCode = PurchaseOrderDetails?.SiteAddressPostCode?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.SiteAddressCountry = PurchaseOrderDetails?.SiteAddressCountry?.ToUpper();

            if (PurchaseOrderDetails.Required == null || PurchaseOrderDetails.Required == DateTime.MinValue)
            {
                purchaseOrderEPMDevExpressPrintModel.Required = "";
            }
            else
            {
                purchaseOrderEPMDevExpressPrintModel.Required = Convert.ToDateTime(PurchaseOrderDetails.Required)
                    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            purchaseOrderEPMDevExpressPrintModel.Terms = PurchaseOrderDetails?.Terms?.ToUpper();
            purchaseOrderEPMDevExpressPrintModel.Carrier = PurchaseOrderDetails?.Carrier?.ToUpper();
            #endregion
            #region Vendor/PO Header Localization
            purchaseOrderEPMDevExpressPrintModel.GlobalPurchaseOrder = UtilityFunction.GetMessageFromResource("spnPurchaseOrder", LocalizeResourceSetConstants.Global).ToUpper();
            purchaseOrderEPMDevExpressPrintModel.spnRequired = UtilityFunction.GetMessageFromResource("spnRequired", LocalizeResourceSetConstants.Global).ToUpper();
            purchaseOrderEPMDevExpressPrintModel.globalShipTo = UtilityFunction.GetMessageFromResource("GlobalShipTo", LocalizeResourceSetConstants.Global).ToUpper();
            purchaseOrderEPMDevExpressPrintModel.globalSpnTo = UtilityFunction.GetMessageFromResource("globalSpnTo", LocalizeResourceSetConstants.Global).ToUpper();
            purchaseOrderEPMDevExpressPrintModel.globalInvoiceTo = UtilityFunction.GetMessageFromResource("globalInvoiceTo", LocalizeResourceSetConstants.Global).ToUpper();
            purchaseOrderEPMDevExpressPrintModel.globalOrderPlaced = UtilityFunction.GetMessageFromResource("globalOrderPlaced", LocalizeResourceSetConstants.Global).ToUpper();
            purchaseOrderEPMDevExpressPrintModel.globalNumber = UtilityFunction.GetMessageFromResource("globalNumber", LocalizeResourceSetConstants.Global).ToUpper();
            purchaseOrderEPMDevExpressPrintModel.globalStatus = UtilityFunction.GetMessageFromResource("GlobalStatus", LocalizeResourceSetConstants.Global).ToUpper();
            purchaseOrderEPMDevExpressPrintModel.globalBlanketOrderRef = UtilityFunction.GetMessageFromResource("globalBlanketOrderRef", LocalizeResourceSetConstants.Global).ToUpper();
            purchaseOrderEPMDevExpressPrintModel.spnPhoneNumber = UtilityFunction.GetMessageFromResource("spnPhoneNumber", LocalizeResourceSetConstants.Global);
            purchaseOrderEPMDevExpressPrintModel.spnFax = UtilityFunction.GetMessageFromResource("spnFax", LocalizeResourceSetConstants.Global);
            purchaseOrderEPMDevExpressPrintModel.spnAcknowledgement = UtilityFunction.GetMessageFromResource("spnAcknowledgement", LocalizeResourceSetConstants.Global);
            purchaseOrderEPMDevExpressPrintModel.spnPoTerms = UtilityFunction.GetMessageFromResource("spnPoTerms", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderEPMDevExpressPrintModel.spnPoCarrier = UtilityFunction.GetMessageFromResource("spnPoCarrier", LocalizeResourceSetConstants.PurchaseOrder);
            purchaseOrderEPMDevExpressPrintModel.spnDeliveryTerms = UtilityFunction.GetMessageFromResource("spnDeliveryTerms", LocalizeResourceSetConstants.Global);
            #endregion
        }
        private void BindPurchaseOrderEPMLineItemTable(List<PurchaseOrderLineItem> listOfPurchaseOrderEPMLineItemInfo, ref PurchaseOrderEPMDevExpressPrintModel purchaseOrderEPMDevExpressPrintModel)
        {
            if (listOfPurchaseOrderEPMLineItemInfo.Count > 0)
            {
                foreach (var item in listOfPurchaseOrderEPMLineItemInfo)
                {
                    var objPOLineItemEPMDevExpressPrintModel = new POLineItemEPMDevExpressPrintModel();

                    objPOLineItemEPMDevExpressPrintModel.LineNumber = item.LineNumber;
                    objPOLineItemEPMDevExpressPrintModel.EPMPart = item.EPMPart;
                    objPOLineItemEPMDevExpressPrintModel.SubPart = item.SUPPart;
                    objPOLineItemEPMDevExpressPrintModel.PartDescription = item?.Description+"~/\n"+ item?.Manufacturer + "~/\n"+ item?.ChargeToClientLookupId; 
                    if (item.EstimatedDelivery == null || item.EstimatedDelivery == DateTime.MinValue)
                    {
                        objPOLineItemEPMDevExpressPrintModel.EstimatedDelivery = "";
                    }
                    else
                    {
                        objPOLineItemEPMDevExpressPrintModel.EstimatedDelivery = Convert.ToDateTime(item.EstimatedDelivery)
                            .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    objPOLineItemEPMDevExpressPrintModel.OrderQuantity = Math.Round(item.OrderQuantity, 2);
                    objPOLineItemEPMDevExpressPrintModel.UnitOfMeasure = item.UnitOfMeasure;
                    objPOLineItemEPMDevExpressPrintModel.UnitCost = Math.Round(item.UnitCost, 2);
                    objPOLineItemEPMDevExpressPrintModel.Extension = item.Extension;
                    objPOLineItemEPMDevExpressPrintModel.Status_Display = item.Status_Display;
                    objPOLineItemEPMDevExpressPrintModel.ChargeToClientLookupId = item?.ChargeToClientLookupId;
                    objPOLineItemEPMDevExpressPrintModel.Manufacturer = item?.Manufacturer;

                    #region Localization
                    objPOLineItemEPMDevExpressPrintModel.globalLineItems = UtilityFunction.GetMessageFromResource("spnLineItems", LocalizeResourceSetConstants.Global);
                    objPOLineItemEPMDevExpressPrintModel.globalLine = UtilityFunction.GetMessageFromResource("globalLine", LocalizeResourceSetConstants.Global);
                    objPOLineItemEPMDevExpressPrintModel.globalEPMPart = UtilityFunction.GetMessageFromResource("globalEPMPart", LocalizeResourceSetConstants.Global);
                    objPOLineItemEPMDevExpressPrintModel.globalSUPPart = UtilityFunction.GetMessageFromResource("globalSUPPart", LocalizeResourceSetConstants.Global);
                    objPOLineItemEPMDevExpressPrintModel.spnPartDescription = UtilityFunction.GetMessageFromResource("spnPartDescription", LocalizeResourceSetConstants.Global);
                    objPOLineItemEPMDevExpressPrintModel.globalUOM = UtilityFunction.GetMessageFromResource("globalUOM", LocalizeResourceSetConstants.Global);
                    objPOLineItemEPMDevExpressPrintModel.spnQty = UtilityFunction.GetMessageFromResource("spnQuantity", LocalizeResourceSetConstants.Global);
                    objPOLineItemEPMDevExpressPrintModel.spnPrice = UtilityFunction.GetMessageFromResource("spnPoPrice", LocalizeResourceSetConstants.PurchaseOrder);
                    objPOLineItemEPMDevExpressPrintModel.globalExtension = UtilityFunction.GetMessageFromResource("globalExtension", LocalizeResourceSetConstants.Global);
                    objPOLineItemEPMDevExpressPrintModel.GlobalStatus = UtilityFunction.GetMessageFromResource("GlobalStatus", LocalizeResourceSetConstants.Global);
                    objPOLineItemEPMDevExpressPrintModel.GlobalChargeTo = UtilityFunction.GetMessageFromResource("GlobalChargeTo", LocalizeResourceSetConstants.Global);
                    objPOLineItemEPMDevExpressPrintModel.GlobalGrandTotal = UtilityFunction.GetMessageFromResource("GlobalGrandTotal", LocalizeResourceSetConstants.Global);

                    #endregion                

                    purchaseOrderEPMDevExpressPrintModel.POLineItemEPMDevExpressPrintModelList.Add(objPOLineItemEPMDevExpressPrintModel);
                }
            }
        }
        private void BindCommentsEPMTable(List<Notes> notes, ref PurchaseOrderEPMDevExpressPrintModel purchaseOrderEPMDevExpressPrintModel)
        {
            if (notes != null && notes.Count > 0)
            {
                foreach (var item in notes.OrderBy(n => n.ObjectId))
                {
                    var note = new POCommentsEPMDevExpressPrintModel();
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
                    purchaseOrderEPMDevExpressPrintModel.POCommentsEPMDevExpressPrintModelList.Add(note);
                }
            }
        }

        #endregion

        #region V2-949
        private void BindPdfAttachmentTable(List<Models.AttachmentModel> POPdfAttachment, ref PurchaseOrderDevExpressPrintModel purchaseOrderDevExpressPrintModel)
        {
            if (POPdfAttachment != null && POPdfAttachment.Count > 0)
            {
                CommonWrapper comWrapper = new CommonWrapper(userData);
                foreach (var itemAttach in POPdfAttachment)
                {
                    var objAttachmentDevExpressPrintModel = new AttachmentDevExpressPrintModel();
                    var attachUrl = itemAttach.AttachmentUrl;
                    if (userData.DatabaseKey.Client.OnPremise)
                    {
                        objAttachmentDevExpressPrintModel.SASUrl = UtilityFunction.Base64SrcDevexpress(attachUrl);
                    }
                    else
                    {
                        objAttachmentDevExpressPrintModel.SASUrl = comWrapper.GetSasAttachmentUrl(ref attachUrl);
                    }
                    purchaseOrderDevExpressPrintModel.AttachmentDevExpressPrintModelList.Add(objAttachmentDevExpressPrintModel);
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

        #region V2-1047 PO Model 
        [HttpPost]
        public ActionResult Modelconfirm(long PurchaseOrderId = 0)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            List<POLineItemModel> DirectBuyLineItemList = new List<POLineItemModel>();

            DirectBuyLineItemList = pWrapper.PopulatePOLineItemForDirectLineItems(PurchaseOrderId, "nonstock");
            if (DirectBuyLineItemList.Count > 0)
            {
                objPurchaseOrderVM.POLineItemList = DirectBuyLineItemList;
                var ChargeTypeList = UtilityFunction.populateChargeTypeForLineItem();
                if (ChargeTypeList != null)
                {
                    objPurchaseOrderVM.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
                return PartialView("_DirectBuyLineItems", objPurchaseOrderVM);
            }
            else
            {
                var successStatus = pWrapper.ConvertPOtoPR(PurchaseOrderId, objPurchaseOrderVM);
                if (successStatus.Item1 == "success")
                {
                    return Json(new { success = true, purchaseRequestId = successStatus.Item2 });
                }
                else
                {
                    return Json(new { success = false, purchaseRequestId = 0 });
                }

            }
        }
        [HttpPost]
        public string DirectBuyLineItemsModelconfirm(int? draw, int? start, int? length, long PurchaseOrderId = 0)
        {

            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            List<POLineItemModel> DirectBuyLineItemList = pWrapper.PopulatePOLineItemForDirectLineItems(PurchaseOrderId, "nonstock");
            var recordsFiltered = 0;
            var totalRecords = 0;
            recordsFiltered = DirectBuyLineItemList.Count();
            totalRecords = DirectBuyLineItemList.Count();
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            var ChargeTypeList = UtilityFunction.populateChargeTypeForLineItem();
            if (ChargeTypeList != null)
            {
                objPurchaseOrderVM.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            return JsonConvert.SerializeObject(new { recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = DirectBuyLineItemList, ChargeTypeList = objPurchaseOrderVM.ChargeTypeList }, JsonSerializer12HoursDateAndTimeSettings);
        }
        [HttpPost]
        public ActionResult SaveDirectBuyLineItem(PurchaseOrderVM PurchaseOrderVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            if (PurchaseOrderVM.POLineItemList != null && PurchaseOrderVM.POLineItemList.Count > 0)
            {
                var successStatus = pWrapper.ConvertPOtoPR(PurchaseOrderVM.POLineItemList[0].PurchaseOrderId, PurchaseOrderVM);
                if (successStatus.Item1 == "success")
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), purchaseRequestId = successStatus.Item2 }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.failed.ToString(), purchaseRequestId = 0 }, JsonRequestBehavior.AllowGet);

                }

            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region V2-1032 SingleStockLineItem
        public PartialViewResult AddPartInInventorySingleStockLineItemDynamic(long PurchaseOrderId, long StoreroomId = 0)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var PODetails = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
            objPurchaseOrderVM.AddPOLineItemPartInInventory = new Models.PurchaseOrder.UIConfiguration.AddPOLineItemPartInInventoryModelDynamic();
            objPurchaseOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrderLineItemStockSingle, userData);
            objPurchaseOrderVM.AddPOLineItemPartInInventory.PurchaseOrderId = PurchaseOrderId;
            objPurchaseOrderVM.AddPOLineItemPartInInventory.PurchaseOrder_ClientLookupId = PODetails.ClientLookupId;
            objPurchaseOrderVM.AddPOLineItemPartInInventory.VendorId = PODetails.VendorId;
            var ChargeTypeList = UtilityFunction.populateChargeTypeForLineItem();
            if (ChargeTypeList != null)
            {
                objPurchaseOrderVM.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ChargeTypeLookUpList = PopulatelookUpListByType(objPurchaseOrderVM.AddPOLineItemPartInInventory.ChargeType);

            objPurchaseOrderVM.AddPOLineItemPartInInventory.AccountId = userData.Site.NonStockAccountId;//RNJ
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null && userData.Site.NonStockAccountId > 0)
            {
                AcclookUpList = AcclookUpList.Where(x => x.AccountId == userData.Site.NonStockAccountId).ToList();
                if (AcclookUpList != null)
                {
                    objPurchaseOrderVM.AddPOLineItemPartInInventory.AccountClientLookupId = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() }).Where(x => x.Value == userData.Site.NonStockAccountId.ToString()).First().Text;
                }
                else
                {
                    objPurchaseOrderVM.AddPOLineItemPartInInventory.AccountClientLookupId = "";
                }
            }
            else
            {
                objPurchaseOrderVM.AddPOLineItemPartInInventory.AccountClientLookupId = "";
            }
            objPurchaseOrderVM.AddPOLineItemPartInInventory.PartClientLookupId = "";
            objPurchaseOrderVM.AddPOLineItemPartInInventory.IsShopingCart = userData.Site.ShoppingCart;
            objPurchaseOrderVM.AddPOLineItemPartInInventory.IsOnOderCheck = userData.Site.OnOrderCheck;
            objPurchaseOrderVM.AddPOLineItemPartInInventory.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            objPurchaseOrderVM.AddPOLineItemPartInInventory.StoreroomId = StoreroomId;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            IList<string> LookupNames = LookupNames = objPurchaseOrderVM.UIConfigurationDetails.ToList()
                                        .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                        .Select(s => s.LookupName)
                                        .ToList();
            if (AllLookUps != null)
            {
                objPurchaseOrderVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .Select(s => new Client.Models.UILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();
                List<DataContracts.LookupList> objLookup = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (objLookup != null)
                {
                    objPurchaseOrderVM.AddPOLineItemPartInInventory.UOMList = objLookup.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_AddPartInInventoryDynamic", objPurchaseOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPartInInventorySingleStockDynamic(PurchaseOrderVM _PurchaseOrder, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            List<string> errorMessages = new List<string>();
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                errorMessages = pWrapper.AddPartInInventorySingleStockDynamic(_PurchaseOrder);
                if (errorMessages == null || errorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PurchaseOrderId = _PurchaseOrder.AddPOLineItemPartInInventory.PurchaseOrderId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult EditPOPartInInventorySingleStockDynamic(long LineItemId, long PurchaseOrderId, string POClientLookupId, bool IsPunchout = false, long StoreroomId = 0)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            PurchaseOrderWrapper poWrapper = new PurchaseOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            List<DataModel> acclookUpList = new List<DataModel>();
            List<DataContracts.LookupList> allLookUpList = new List<DataContracts.LookupList>();

            Task[] tasks = new Task[1];
            tasks[0] = Task.Factory.StartNew(() => allLookUpList = commonWrapper.GetAllLookUpList());

            objPurchaseOrderVM.EditPOLineItemPartInInventorySingleStock = poWrapper.GetPOLineItemInInventorySingleStockByIdDynamic(LineItemId, PurchaseOrderId);
            objPurchaseOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
       .Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrderLineItemStockSingle, userData);
            var PODetails = poWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
            objPurchaseOrderVM.EditPOLineItemPartInInventorySingleStock.PurchaseOrder_ClientLookupId = PODetails.ClientLookupId;
            objPurchaseOrderVM.EditPOLineItemPartInInventorySingleStock.IsPunchout = IsPunchout;
            objPurchaseOrderVM.EditPOLineItemPartInInventorySingleStock.IsShopingCart = userData.Site.ShoppingCart;
            objPurchaseOrderVM.EditPOLineItemPartInInventorySingleStock.IsOnOderCheck = userData.Site.OnOrderCheck;
            objPurchaseOrderVM.EditPOLineItemPartInInventorySingleStock.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            objPurchaseOrderVM.EditPOLineItemPartInInventorySingleStock.StoreroomId = StoreroomId;
            objPurchaseOrderVM.EditPOLineItemPartInInventorySingleStock.VendorId = PODetails.VendorId;
            Task.WaitAll(tasks);
            if (!tasks[0].IsFaulted && tasks[0].IsCompleted /* && !tasks[1].IsFaulted && tasks[1].IsCompleted*/)
            {

                #region Task
                IList<string> LookupName = LookupName = objPurchaseOrderVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
                if (allLookUpList != null)
                {
                    objPurchaseOrderVM.AllRequiredLookUplist = allLookUpList.Where(x => LookupName.Contains(x.ListName))
                                                          .Select(s => new Client.Models.UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
                }
                #endregion
            }
            LocalizeControls(objPurchaseOrderVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_EditPartInInventorySingleStockDynamic", objPurchaseOrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePOPartInInventorySingleStockDynamic(PurchaseOrderVM PurchaseOrderVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.UpdatePOPartInInventorySingleStockDynamic(PurchaseOrderVM);
                if (lineItem.ErrorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PurchaseOrderId = PurchaseOrderVM.EditPOLineItemPartInInventorySingleStock.PurchaseOrderId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region V2-1079
        [HttpPost]
        public async Task<ActionResult> SendEDIPOtoVendor(long PurchaseOrderId)
        {
            var Message = string.Empty;
            string LoginSessionId = userData.LoginAuditing.SessionId.ToString();
            var apiUrl = WebConfigurationManager.AppSettings["InterfaceAPIUrl"] + "api/EPMPOEDIExportDelim/";
            var apiParams = new EPMPOEDIExportAPIParams
            {
                LoginSessionId = LoginSessionId,
                PurchaseOrderId = PurchaseOrderId
            };
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync($"{apiUrl}", apiParams);

                    if (response.IsSuccessStatusCode)
                    {
                        PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
                        var purchaseOrder = pWrapper.UpdatePurchaseOrderForEDIExport(PurchaseOrderId);
                        Message = UtilityFunction.GetMessageFromResource("spnPOExportSuccess", LocalizeResourceSetConstants.PurchaseOrder);
                        return Json(new { success = true, PurchaseOrderId = purchaseOrder.PurchaseOrderId, message = Message });
                    }
                    else
                    {
                        Message = UtilityFunction.GetMessageFromResource("spnPOExportFailed", LocalizeResourceSetConstants.PurchaseOrder);
                        return Json(new { success = false, message = Message });
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return Json(new { success = false, message = Message });
            }
        }
        #endregion

        #region V2-1112
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPurchaseOrdersCustomEPMDynamic(PurchaseOrderVM objVM, string Command)
        {
            AddPurchaseOrderModelDynamic addPurchaseOrder= (AddPurchaseOrderModelDynamic)TempData["purchasingData"] ;
            objVM.addPurchaseOrder = addPurchaseOrder;
            PurchaseOrder PurchaseOrderObj = new PurchaseOrder();
            PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string errormsg = "Error";
            string errormsgAssetMgt = "ErrorAssetMgt";
            if (ModelState.IsValid)
            {
                var PurchaseOrderAddObj = pWrapper.AddCustomEPMPurchaseOrderDynamic(objVM);
                PurchaseOrderObj = PurchaseOrderAddObj.Item1;
                if (PurchaseOrderAddObj.Item2 == true)
                {
                    return Json(new { Result = errormsg }, JsonRequestBehavior.AllowGet);
                }
                else if (PurchaseOrderAddObj.Item3 == true)
                {
                    return Json(new { Result = errormsgAssetMgt }, JsonRequestBehavior.AllowGet);
                }

                if (PurchaseOrderObj.ErrorMessages != null && PurchaseOrderObj.ErrorMessages.Count > 0)
                {
                    return Json(PurchaseOrderObj.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, PurchaseOrderId = PurchaseOrderObj.PurchaseOrderId, mode = "add" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
