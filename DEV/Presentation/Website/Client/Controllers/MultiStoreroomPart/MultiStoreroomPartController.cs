using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.MultiStoreroomPart;
using Client.Common;
using Client.Controllers.Common;
using Client.DevExpressReport;
using Client.DevExpressReport.EPM;
using Client.Localization;
using Client.Models;
using Client.Models.Common;
using Client.Models.MultiStoreroomPart;
using Common.Constants;

using DataContracts;

using DevExpress.XtraReports.UI;

using Newtonsoft.Json;

using QRCoder;

using Rotativa;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using static Client.Models.Common.UserMentionDataModel;

namespace Client.Controllers.MultiStoreroomPart
{
    public class MultiStoreroomPartController : SomaxBaseController
    {
        // GET: MultiStoreroomPart
        [CheckUserSecurity(securityType = SecurityConstants.Parts)]
        public ActionResult Index()
        {
            if (!userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                return Redirect("/Parts/Index?page=Inventory_Part ");
            }
            MultiStoreroomPartVM multiStoreroomPartVM = new MultiStoreroomPartVM();
            MultiStoreroomPartModel MSPmodel = new MultiStoreroomPartModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            multiStoreroomPartVM.security = userData.Security;
            List<DataContracts.LookupList> objLookupStokeType = new List<DataContracts.LookupList>();
            var ViewSearchList = UtilityFunction.PartViewSearchTypes();
            MSPmodel.PartViewSearchList = ViewSearchList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                objLookupStokeType = AllLookUps.Where(x => x.ListName == LookupListConstants.STOCK_TYPE).ToList();
                if (objLookupStokeType != null)
                {
                    MSPmodel.LookupStokeTypeList = objLookupStokeType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }
            #region V2-1025
            var StoreroomLookUplist = mspWrapper.GetStoreroomListByClientIdSiteId();
            if (StoreroomLookUplist != null)
            {
                multiStoreroomPartVM.StoreroomList = StoreroomLookUplist.Select(x => new SelectListItem { Text = x.Description, Value = x.StoreroomId.ToString() }).ToList();
            }
            #endregion
            multiStoreroomPartVM.MultiStoreroomPartModel = MSPmodel;
            string mode = Convert.ToString(TempData["Mode"]);
            if (mode == "add")
            {
                // var multiStoreroomPart = mspWrapper.PopulateDropdownControls(mspModel);
                multiStoreroomPartVM.AddPart = new Models.MultiStoreroomPart.UIConfiguration.AddPartModelDynamic();
                multiStoreroomPartVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddPart, userData);
                IList<string> LookupNames = multiStoreroomPartVM.UIConfigurationDetails.ToList()
                                                .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                                .Select(s => s.LookupName)
                                                .ToList();

                if (AllLookUps != null)
                {
                    multiStoreroomPartVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                              .Select(s => new UILookupList
                                                              { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                              .ToList();
                }

                //var AcclookUpList = GetLookupList_Account();
                var AcclookUpList = GetAccountByActiveState(true);

                if (AcclookUpList != null)
                {
                    multiStoreroomPartVM.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
                }
                ViewBag.IsAdd = true;
            }
            #region V2-1007
            else if (mode == "MultiStoreroomPartsDetailFromEquipment")
            {
                long partId = Convert.ToInt64(TempData["partId"]);
                long equipmentId = Convert.ToInt64(TempData["equipmentId"]);

                MultiStoreroomPartSummaryModel mPartSummaryModel = new MultiStoreroomPartSummaryModel();
                MultiStoreroomPartHistoryModel MSPHistoryModel = new MultiStoreroomPartHistoryModel();
                MultiStoreroomReceiptModel MSPReceiptModel = new MultiStoreroomReceiptModel();
                ChangePartIdForMultiPartStoreroomPartModel changePartIdModel = new ChangePartIdForMultiPartStoreroomPartModel();
                multiStoreroomPartVM.MultiStoreroomPartModel = mspWrapper.PopulateMultiStoreroomPartDetails(partId);
                mPartSummaryModel = GetPartSummary(partId);
                multiStoreroomPartVM.MultiStoreroomPartModel.PartImageUrl = mPartSummaryModel.PartImageUrl;

                multiStoreroomPartVM.security = this.userData.Security;
                multiStoreroomPartVM._userdata = this.userData;
                EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
                var Parts = eWrapper.GetEquipmentPartsByEquipmentIdPartId(partId, equipmentId);
                var Equipment_ClientLookupId = Parts.Select(a => a.Equipment_ClientLookupId).FirstOrDefault().ToString();
                multiStoreroomPartVM.IsAddPartFromEquipment = true; //V2-1007
                multiStoreroomPartVM.Equipment_ClientLookupId = Equipment_ClientLookupId; //V2-1007
                multiStoreroomPartVM.EquipmentId = equipmentId;
                Task attTask;
                attTask = Task.Factory.StartNew(() => multiStoreroomPartVM.AttachmentCount = commonWrapper.AttachmentCount(partId, AttachmentTableConstant.Part, userData.Security.Parts.Edit));
                Task.WaitAll(attTask);
                var MSPHistorydateList = UtilityFunction.PartDatesList();
                if (MSPHistorydateList != null)
                {
                    MSPHistoryModel.HistoryDateList = MSPHistorydateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                var MSPPartReceiptdateList = UtilityFunction.PartDatesList();
                if (MSPPartReceiptdateList != null)
                {
                    MSPReceiptModel.ReceiptDateList = MSPPartReceiptdateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                multiStoreroomPartVM.MSPReceiptModel = MSPReceiptModel;
                changePartIdModel.ClientLookupId = multiStoreroomPartVM.MultiStoreroomPartModel.ClientLookupId;
                multiStoreroomPartVM.ChangePartIdModel = changePartIdModel;
                multiStoreroomPartVM.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
                multiStoreroomPartVM.multiStoreroomSummaryModel = mPartSummaryModel;
                multiStoreroomPartVM.MSPHistoryModel = MSPHistoryModel;
                #region V2-1187
                PartUDF PartUDF = new PartUDF();
                PartUDF = mspWrapper.RetrievePartUDFByPartId(partId);
                multiStoreroomPartVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPartWidget, userData);
                multiStoreroomPartVM.ViewPart = new Models.MultiStoreroomPart.UIConfiguration.ViewPartModelDynamic();
                Client.Models.Parts.PartModel partModels = mspWrapper.PopulatePartDetails_V2(partId);
                multiStoreroomPartVM.ViewPart = mspWrapper.MapPartDataForView(multiStoreroomPartVM.ViewPart, partModels);
                multiStoreroomPartVM.ViewPart = mspWrapper.MapPartUDFDataForView(multiStoreroomPartVM.ViewPart, PartUDF);
                #endregion
                #region V2-1196

                if (userData.Security.Parts.ConfigureAutoPurchasing)
                {

                    var vendorParts = mspWrapper.PopulateParts(partId);
                    var partsConfigureAutoPurchasingModel = new Client.Models.Parts.PartsConfigureAutoPurchasingModel
                    {
                        VendorList = vendorParts.Select(x =>
                            new SelectListItem
                            {
                                Text = $"{x.Vendor_ClientLookupId} - {x.Vendor_Name}",
                                Value = x.Part_Vendor_XrefId.ToString()
                            }).ToList()
                    };
                    multiStoreroomPartVM.partsConfigureAutoPurchasingModel = partsConfigureAutoPurchasingModel;
                    multiStoreroomPartVM.partsConfigureAutoPurchasingModel.PartId = partId;
                }
                #endregion
                ViewBag.IsMultiStoreroomDetails = true;
                var StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Maintain).Count();
                if (StoreroomList > 0)
                {
                    multiStoreroomPartVM.IsMaintain = true;
                }
                #region V2-1115
                bool EPMInvoiceImportInUse = false;

                var InterfacePropData = (List<InterfacePropModel>)Session["InterfacePropData"];
                if (InterfacePropData != null && InterfacePropData.Count > 0)
                {
                    EPMInvoiceImportInUse = InterfacePropData.Where(x => x.InterfaceType == InterfacePropConstants.EPMInvoiceImport).Select(x => x.InUse).FirstOrDefault();
                }
                multiStoreroomPartVM.EPMInvoiceImportInUse = EPMInvoiceImportInUse;
                #endregion
            }
            #endregion
            #region V2-1147
            else if (mode == "DetailFromNotification")
            {
                long partId = Convert.ToInt64(TempData["partId"]);
                MultiStoreroomPartSummaryModel mPartSummaryModel = new MultiStoreroomPartSummaryModel();
                MultiStoreroomPartHistoryModel MSPHistoryModel = new MultiStoreroomPartHistoryModel();
                MultiStoreroomReceiptModel MSPReceiptModel = new MultiStoreroomReceiptModel();
                ChangePartIdForMultiPartStoreroomPartModel changePartIdModel = new ChangePartIdForMultiPartStoreroomPartModel();
                multiStoreroomPartVM.MultiStoreroomPartModel = mspWrapper.PopulateMultiStoreroomPartDetails(partId);
                mPartSummaryModel = GetPartSummary(partId);
                multiStoreroomPartVM.MultiStoreroomPartModel.PartImageUrl = mPartSummaryModel.PartImageUrl;

                multiStoreroomPartVM.security = this.userData.Security;
                multiStoreroomPartVM._userdata = this.userData;
                ViewBag.IsPartDetailsFromNotification = true;
                Task attTask;
                attTask = Task.Factory.StartNew(() => multiStoreroomPartVM.AttachmentCount = commonWrapper.AttachmentCount(partId, AttachmentTableConstant.Part, userData.Security.Parts.Edit));
                Task.WaitAll(attTask);
                var MSPHistorydateList = UtilityFunction.PartDatesList();
                if (MSPHistorydateList != null)
                {
                    MSPHistoryModel.HistoryDateList = MSPHistorydateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                var MSPPartReceiptdateList = UtilityFunction.PartDatesList();
                if (MSPPartReceiptdateList != null)
                {
                    MSPReceiptModel.ReceiptDateList = MSPPartReceiptdateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                multiStoreroomPartVM.MSPReceiptModel = MSPReceiptModel;
                changePartIdModel.ClientLookupId = multiStoreroomPartVM.MultiStoreroomPartModel.ClientLookupId;
                multiStoreroomPartVM.ChangePartIdModel = changePartIdModel;
                multiStoreroomPartVM.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
                multiStoreroomPartVM.multiStoreroomSummaryModel = mPartSummaryModel;
                multiStoreroomPartVM.MSPHistoryModel = MSPHistoryModel;

                var StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Maintain).Count();
                if (StoreroomList > 0)
                {
                    multiStoreroomPartVM.IsMaintain = true;
                }

                bool EPMInvoiceImportInUse = false;

                var InterfacePropData = (List<InterfacePropModel>)Session["InterfacePropData"];
                if (InterfacePropData != null && InterfacePropData.Count > 0)
                {
                    EPMInvoiceImportInUse = InterfacePropData.Where(x => x.InterfaceType == InterfacePropConstants.EPMInvoiceImport).Select(x => x.InUse).FirstOrDefault();
                }
                #region V2-1187
                PartUDF PartUDF = new PartUDF();
                PartUDF = mspWrapper.RetrievePartUDFByPartId(partId);
                multiStoreroomPartVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPartWidget, userData);
                multiStoreroomPartVM.ViewPart = new Models.MultiStoreroomPart.UIConfiguration.ViewPartModelDynamic();
                Client.Models.Parts.PartModel partModels = mspWrapper.PopulatePartDetails_V2(partId);
                multiStoreroomPartVM.ViewPart = mspWrapper.MapPartDataForView(multiStoreroomPartVM.ViewPart, partModels);
                multiStoreroomPartVM.ViewPart = mspWrapper.MapPartUDFDataForView(multiStoreroomPartVM.ViewPart, PartUDF);
                #endregion
                #region V2-1196

                if (userData.Security.Parts.ConfigureAutoPurchasing)
                {

                    var vendorParts = mspWrapper.PopulateParts(partId);
                    var partsConfigureAutoPurchasingModel = new Client.Models.Parts.PartsConfigureAutoPurchasingModel
                    {
                        VendorList = vendorParts.Select(x =>
                            new SelectListItem
                            {
                                Text = $"{x.Vendor_ClientLookupId} - {x.Vendor_Name}",
                                Value = x.Part_Vendor_XrefId.ToString()
                            }).ToList()
                    };
                    multiStoreroomPartVM.partsConfigureAutoPurchasingModel = partsConfigureAutoPurchasingModel;
                    multiStoreroomPartVM.partsConfigureAutoPurchasingModel.PartId = partId;
                }
                #endregion
                multiStoreroomPartVM.EPMInvoiceImportInUse = EPMInvoiceImportInUse;
            }
            #endregion
            LocalizeControls(multiStoreroomPartVM, LocalizeResourceSetConstants.PartDetails);
            return View(multiStoreroomPartVM);
        }
        #region Search
        [HttpPost]
        public string GetMultiPartStoreroomMainGrid(int? draw, int? start, int? length, int CustomQueryDisplayId, string SearchText = "", string PartID = "",
                                       string Description = "", string StockType = "", string Order = "1", List<string> Storerooms = null)
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
           ? start / length
           : 0;
            int skip = start * length ?? 0;
            MultiStoreroomPartWrapper pWrapper = new MultiStoreroomPartWrapper(userData);
            List<MultiStoreroomPartModel> MultiStoreroompartList = pWrapper.GetMultiPartStoreroomChunkList(CustomQueryDisplayId, skip, length ?? 0, Order, orderDir, PartID, Description, StockType, SearchText, Storerooms);
            var totalRecords = 0;
            var recordsFiltered = 0;

            if (MultiStoreroompartList != null && MultiStoreroompartList.Count > 0)
            {
                recordsFiltered = MultiStoreroompartList[0].TotalCount;
                totalRecords = MultiStoreroompartList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = MultiStoreroompartList
              .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        public ActionResult GetMSPInnerGrid(long PartID)
        {
            MultiStoreroomPartVM objMSPVM = new MultiStoreroomPartVM();
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            objMSPVM.MultiStoreroomPartChildModelList = mspWrapper.PopulateChilditems(PartID);
            LocalizeControls(objMSPVM, LocalizeResourceSetConstants.PartDetails);
            return View("_InnerGridMSPChildItem", objMSPVM);
        }
        [HttpGet]
        public string GetMultiPartStoreroomPrintData(int CustomQueryDisplayId, string SearchText = "", string PartID = "",
                                       string Description = "", string StockType = "", string Order = "1", string coldir = "asc", List<string> Storerooms = null
            )
        {
            List<MultiStoreroomPartPrintModel> mspPrintModelList = new List<MultiStoreroomPartPrintModel>();
            MultiStoreroomPartWrapper pWrapper = new MultiStoreroomPartWrapper(userData);
            List<MultiStoreroomPartModel> MultiStoreroompartList = pWrapper.GetMultiPartStoreroomChunkList(CustomQueryDisplayId, 0, 100000, Order, coldir, PartID, Description, StockType, SearchText, Storerooms);
            foreach (var item in MultiStoreroompartList)
            {
                MultiStoreroomPartPrintModel mspPrintModel = new MultiStoreroomPartPrintModel();
                mspPrintModel.ClientLookupId = item.ClientLookupId;
                mspPrintModel.Description = item.Description;
                mspPrintModel.DefStoreroom = item.DefStoreroom;
                mspPrintModel.StockType = item.StockType;
                mspPrintModel.AppliedCost = item.AppliedCost;
                mspPrintModelList.Add(mspPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = mspPrintModelList }, JsonSerializerDateSettings);
        }
        [HttpPost]
        public JsonResult SetPrintData(MSPPrintParams mspPrintParams)
        {
            Session["PRINTPARAMS"] = mspPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [EncryptedActionParameter]
        public ActionResult ExportASPDF(string d = "")
        {
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            MultiStoreroomPartPDFPrintModel objMSPPrintModel;
            MultiStoreroomPartVM objMSPVM = new MultiStoreroomPartVM();
            List<MultiStoreroomPartPDFPrintModel> MSPPrintModelList = new List<MultiStoreroomPartPDFPrintModel>();
            var locker = new object();

            MSPPrintParams MSPPrintParams = (MSPPrintParams)Session["PRINTPARAMS"];

            List<MultiStoreroomPartModel> pRList = mspWrapper.GetMultiPartStoreroomChunkList(MSPPrintParams.CustomQueryDisplayId, 0, 100000, MSPPrintParams.colname, MSPPrintParams.coldir,
               MSPPrintParams.PartID, MSPPrintParams.Description, MSPPrintParams.StockType, MSPPrintParams.txtSearchval, MSPPrintParams.Storerooms);

            foreach (var p in pRList)
            {
                objMSPPrintModel = new MultiStoreroomPartPDFPrintModel();
                objMSPPrintModel.ClientLookupId = p.ClientLookupId;

                objMSPPrintModel.Description = p.Description;
                objMSPPrintModel.StockType = p.StockType;
                objMSPPrintModel.AppliedCost = p.AppliedCost;
                objMSPPrintModel.DefStoreroom = p.DefStoreroom;
                if (p.ChildCount > 0)
                {
                    objMSPPrintModel.ChildModelList = mspWrapper.PopulateChilditems(p.PartId);
                }
                lock (locker)
                {
                    MSPPrintModelList.Add(objMSPPrintModel);
                }
            }
            objMSPVM.multiStoreroomPartPDFPrintModel = MSPPrintModelList;
            objMSPVM.tableHaederProps = MSPPrintParams.tableHaederProps;
            LocalizeControls(objMSPVM, LocalizeResourceSetConstants.PartDetails);
            if (d == "PDF")
            {
                return new PartialViewAsPdf("MSPGridPdfPrintTemplate", objMSPVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    FileName = "Multi Storeroom Part.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new ViewAsPdf("MSPGridPdfPrintTemplate", objMSPVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }

        }
        #endregion

        #region Multi Storeroom Part Details
        public PartialViewResult MultiStoreroomPartDetails(long PartId)
        {
            MultiStoreroomPartVM objMultiStoreroomVM = new MultiStoreroomPartVM();
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            MultiStoreroomPartSummaryModel mPartSummaryModel = new MultiStoreroomPartSummaryModel();
            MultiStoreroomPartHistoryModel MSPHistoryModel = new MultiStoreroomPartHistoryModel();
            MultiStoreroomReceiptModel MSPReceiptModel = new MultiStoreroomReceiptModel();
            ChangePartIdForMultiPartStoreroomPartModel changePartIdModel = new ChangePartIdForMultiPartStoreroomPartModel();
            objMultiStoreroomVM.MultiStoreroomPartModel = mspWrapper.PopulateMultiStoreroomPartDetails(PartId);
            // RKL - V2-1091
            //       We can get the information that is loaded by the GetPartSummary 
            //       method from the MultiStoreroomPartModel that is returhned from
            //       the PopulateMultiStoreroomPartDetails method. We do not need to 
            //       run the usp_Part_RetrieveByPartId_V2 sp twice
            mPartSummaryModel.ClientLookupId = objMultiStoreroomVM.MultiStoreroomPartModel.ClientLookupId;
            mPartSummaryModel.Description = objMultiStoreroomVM.MultiStoreroomPartModel.Description;
            mPartSummaryModel.IssueUnit = objMultiStoreroomVM.MultiStoreroomPartModel.IssueUnit;
            mPartSummaryModel.AppliedCost = objMultiStoreroomVM.MultiStoreroomPartModel.AppliedCost;
            mPartSummaryModel.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            mPartSummaryModel.TotalOnHand = objMultiStoreroomVM.MultiStoreroomPartModel.TotalOnHand;
            mPartSummaryModel.TotalOnOrder = objMultiStoreroomVM.MultiStoreroomPartModel.TotalOnOrder;
            mPartSummaryModel.TotalOnRequest = objMultiStoreroomVM.MultiStoreroomPartModel.TotalOnRequest;
            mPartSummaryModel.DefaultStoreroom = objMultiStoreroomVM.MultiStoreroomPartModel.DefStoreroom;
            mPartSummaryModel.StockType = objMultiStoreroomVM.MultiStoreroomPartModel.StockType;
            if (mPartSummaryModel.ClientOnPremise)
            {
                mPartSummaryModel.PartImageUrl = objCommonWrapper.GetOnPremiseImageUrl(PartId, AttachmentTableConstant.Part);
            }
            else
            {
                mPartSummaryModel.PartImageUrl = objCommonWrapper.GetAzureImageUrl(PartId, AttachmentTableConstant.Part);
            }
            mPartSummaryModel.InactiveFlag = objMultiStoreroomVM.MultiStoreroomPartModel.InactiveFlag;
            // RKL - V2-1091 End
            objMultiStoreroomVM.MultiStoreroomPartModel.PartImageUrl = mPartSummaryModel.PartImageUrl;
            objMultiStoreroomVM.security = this.userData.Security;
            objMultiStoreroomVM._userdata = this.userData;

            Task attTask;
            attTask = Task.Factory.StartNew(() => objMultiStoreroomVM.AttachmentCount = objCommonWrapper.AttachmentCount(PartId, AttachmentTableConstant.Part, userData.Security.Parts.Edit));
            Task.WaitAll(attTask);
            var MSPHistorydateList = UtilityFunction.PartDatesList();
            if (MSPHistorydateList != null)
            {
                MSPHistoryModel.HistoryDateList = MSPHistorydateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var MSPPartReceiptdateList = UtilityFunction.PartDatesList();
            if (MSPPartReceiptdateList != null)
            {
                MSPReceiptModel.ReceiptDateList = MSPPartReceiptdateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objMultiStoreroomVM.MSPReceiptModel = MSPReceiptModel;
            changePartIdModel.ClientLookupId = objMultiStoreroomVM.MultiStoreroomPartModel.ClientLookupId;
            objMultiStoreroomVM.ChangePartIdModel = changePartIdModel;
            objMultiStoreroomVM.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
            objMultiStoreroomVM.multiStoreroomSummaryModel = mPartSummaryModel;
            objMultiStoreroomVM.MSPHistoryModel = MSPHistoryModel;
            ViewBag.IsMultiStoreroomDetails = true;
            var StoreroomList = objCommonWrapper.GetStoreroomList(StoreroomAuthType.Maintain).Count();
            if (StoreroomList > 0)
            {
                objMultiStoreroomVM.IsMaintain = true;
            }
            #region V2-1115
            bool EPMInvoiceImportInUse = false;

            var InterfacePropData = (List<InterfacePropModel>)Session["InterfacePropData"];
            if (InterfacePropData != null && InterfacePropData.Count > 0)
            {
                EPMInvoiceImportInUse = InterfacePropData.Where(x => x.InterfaceType == InterfacePropConstants.EPMInvoiceImport).Select(x => x.InUse).FirstOrDefault();
            }
            objMultiStoreroomVM.EPMInvoiceImportInUse = EPMInvoiceImportInUse;
            #endregion
            #region V2-1187
            PartUDF PartUDF = new PartUDF();
            PartUDF = mspWrapper.RetrievePartUDFByPartId(PartId);
            objMultiStoreroomVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPartWidget, userData);
            objMultiStoreroomVM.ViewPart = new Models.MultiStoreroomPart.UIConfiguration.ViewPartModelDynamic();
            Client.Models.Parts.PartModel partModels = mspWrapper.PopulatePartDetails_V2(PartId);
            objMultiStoreroomVM.ViewPart = mspWrapper.MapPartDataForView(objMultiStoreroomVM.ViewPart, partModels);
            objMultiStoreroomVM.ViewPart = mspWrapper.MapPartUDFDataForView(objMultiStoreroomVM.ViewPart, PartUDF);
            #endregion
            #region V2-1196

            if (userData.Security.Parts.ConfigureAutoPurchasing)
            {

                var vendorParts = mspWrapper.PopulateParts(PartId);
                var partsConfigureAutoPurchasingModel = new Client.Models.Parts.PartsConfigureAutoPurchasingModel
                {
                    VendorList = vendorParts.Select(x =>
                        new SelectListItem
                        {
                            Text = $"{x.Vendor_ClientLookupId} - {x.Vendor_Name}",
                            Value = x.Part_Vendor_XrefId.ToString()
                        }).ToList()
                };
                objMultiStoreroomVM.partsConfigureAutoPurchasingModel = partsConfigureAutoPurchasingModel;
                objMultiStoreroomVM.partsConfigureAutoPurchasingModel.PartId = PartId;
            }
            #endregion
            LocalizeControls(objMultiStoreroomVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_MultiStoreroomDetails.cshtml", objMultiStoreroomVM);
        }

        public MultiStoreroomPartSummaryModel GetPartSummary(long partId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            MultiStoreroomPartSummaryModel pSummary = new MultiStoreroomPartSummaryModel();
            var multiStoreroomPartDetails = mspWrapper.PopulateMultiStoreroomPartDetails(partId);
            if (multiStoreroomPartDetails != null)
            {
                pSummary.ClientLookupId = multiStoreroomPartDetails.ClientLookupId;
                pSummary.Description = multiStoreroomPartDetails.Description;
                pSummary.IssueUnit = multiStoreroomPartDetails.IssueUnit;
                pSummary.AppliedCost = multiStoreroomPartDetails.AppliedCost;
                pSummary.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
                pSummary.TotalOnHand = multiStoreroomPartDetails.TotalOnHand;
                pSummary.TotalOnOrder = multiStoreroomPartDetails.TotalOnOrder;
                pSummary.TotalOnRequest = multiStoreroomPartDetails.TotalOnRequest;
                pSummary.DefaultStoreroom = multiStoreroomPartDetails.DefStoreroom;
                pSummary.StockType = multiStoreroomPartDetails.StockType;
                if (pSummary.ClientOnPremise)
                {
                    pSummary.PartImageUrl = objCommonWrapper.GetOnPremiseImageUrl(partId, AttachmentTableConstant.Part);
                }
                else
                {
                    pSummary.PartImageUrl = objCommonWrapper.GetAzureImageUrl(partId, AttachmentTableConstant.Part);
                }
            }
            pSummary.InactiveFlag = multiStoreroomPartDetails.InactiveFlag;
            return pSummary;
        }
        #endregion

        #region Action functions
        #region QRCode         
        [HttpPost]
        public PartialViewResult PartDetailsQRcode(string[] PartClientLookups)
        {
            MultiStoreroomPartVM MSpartVM = new MultiStoreroomPartVM();
            QRCodeModel qRCodeModel = new QRCodeModel();
            if (PartClientLookups != null)
            {
                qRCodeModel.PartIdsList = new List<string>(PartClientLookups);
            }
            else
            {
                qRCodeModel.PartIdsList = new List<string>();
            }
            MSpartVM.QRCodeModel = qRCodeModel;
            LocalizeControls(MSpartVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("_MultiStoreroomPartDetailsQRCode", MSpartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetPartIdlist(MultiStoreroomPartVM MSpartVM)
        {
            TempData["QRCodeMultiStoreroomPartIdList"] = MSpartVM.QRCodeModel.PartIdsList;
            return Json(new { JsonReturnEnum.success });
        }
        [EncryptedActionParameter]
        public ActionResult QRCodeGenerationUsingRotativa(bool SmallLabel)
        {
            var MSpartVM = new MultiStoreroomPartVM();
            var qRCodeModel = new QRCodeModel();

            ViewBag.SmallQR = SmallLabel;
            if (TempData["QRCodeMultiStoreroomPartIdList"] != null)
            {
                qRCodeModel.PartIdsList = (List<string>)TempData["QRCodeMultiStoreroomPartIdList"];
            }
            else
            {
                qRCodeModel.PartIdsList = new List<string>();
            }
            MSpartVM.QRCodeModel = qRCodeModel;

            return new PartialViewAsPdf("_MultiStoreroomPartsQRCodeTemplate", MSpartVM)
            {
                PageMargins = { Left = 1, Right = 1, Top = 1, Bottom = 1 },
                PageHeight = SmallLabel ? 25 : 28,
                PageWidth = SmallLabel ? 54 : 89,
            };
        }
        //public ActionResult QrGenerate(string value)
        //{
        //    string QRCodeImagePath = string.Empty;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //        string partid = value.Split(new string[] { "][" }, StringSplitOptions.None)[0];
        //        QRCodeData qrCodeData = qrGenerator.CreateQrCode(partid, QRCodeGenerator.ECCLevel.H);
        //        QRCode qrCode = new QRCode(qrCodeData);
        //        using (Bitmap bitMap = qrCode.GetGraphic(20))
        //        {
        //            bitMap.Save(ms, ImageFormat.Png);
        //            QRCodeImagePath = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return Json(new { QRCodeImagePath = QRCodeImagePath, value = value }, JsonRequestBehavior.AllowGet);
        //}
        #endregion
        #region Change Part Id
        public JsonResult ChangePartId(MultiStoreroomPartVM objVM, string strPartId)
        {
            string result = string.Empty;
            if (ModelState.IsValid && !string.IsNullOrEmpty(strPartId))
            {
                MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
                List<String> errorList = new List<string>();
                errorList = mspWrapper.ChangePartId(objVM, Convert.ToInt64(strPartId));
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), partid = strPartId }, JsonRequestBehavior.AllowGet);
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

        #endregion

        #region Part-Add/Edit
        public ActionResult Add()
        {
            TempData["Mode"] = "add";
            return Redirect("/MultiStoreroomPart/Index?page=Inventory_Part");
        }
        public PartialViewResult AddParts()
        {
            MultiStoreroomPartVM objMSPVM = new MultiStoreroomPartVM();
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            MultiStoreroomPartModel mspModel = new MultiStoreroomPartModel();
            objMSPVM.security = this.userData.Security;

            var multiStoreroomPart = mspWrapper.PopulateDropdownControls(mspModel);
            //var AcclookUpList = GetLookupList_Account();
            //var AcclookUpList = GetAccountByActiveState(true);
            //if (AcclookUpList != null)
            //{
            //    multiStoreroomPart.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            //}
            objMSPVM.MultiStoreroomPartModel = multiStoreroomPart;
            LocalizeControls(objMSPVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_MultiStoreroomPartsAddParts.cshtml", objMSPVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddParts(MultiStoreroomPartVM mspVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
                if (mspVM.MultiStoreroomPartModel.PartId == 0)
                {
                    Mode = "add";
                }
                var objPart = mspWrapper.SaveParts(mspVM.MultiStoreroomPartModel);

                if (objPart.ErrorMessages != null && objPart.ErrorMessages.Count > 0)
                {
                    return Json(objPart.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, PartId = objPart.PartId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EditParts(long PartId)
        {
            MultiStoreroomPartVM objPartsVM = new MultiStoreroomPartVM();
            MultiStoreroomPartSummaryModel msPartSummaryModel = new MultiStoreroomPartSummaryModel();
            MultiStoreroomPartWrapper pWrapper = new MultiStoreroomPartWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            objPartsVM.security = this.userData.Security;
            var part = pWrapper.PopulateMultiStoreroomPartEditDetails(PartId);
            part = pWrapper.PopulateDropdownControls(part);

            //var AcclookUpList = GetLookupList_Account();
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                part.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            }
            objPartsVM.MultiStoreroomPartModel = part;
            msPartSummaryModel = GetPartSummary(PartId);
            objPartsVM.multiStoreroomSummaryModel = msPartSummaryModel;
            //#region Summary Header
            //msPartSummaryModel.ClientLookupId = part.ClientLookupId;
            //msPartSummaryModel.Description = part.Description;
            //msPartSummaryModel.IssueUnit = part.IssueUnit;
            //msPartSummaryModel.AppliedCost = part.AppliedCost;
            //msPartSummaryModel.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            //if (msPartSummaryModel.ClientOnPremise)
            //{
            //    msPartSummaryModel.PartImageUrl = comWrapper.GetOnPremiseImageUrl(PartId, AttachmentTableConstant.Part);
            //}
            //else
            //{
            //    msPartSummaryModel.PartImageUrl = comWrapper.GetAzureImageUrl(PartId, AttachmentTableConstant.Part);
            //}
            //objPartsVM.multiStoreroomSummaryModel = msPartSummaryModel;
            //#endregion
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_MultiStoreroomPartsAddParts.cshtml", objPartsVM);
        }
        #endregion

        #region Storeroom details/Add/Edit
        public string PopulateStoreroomDetails(int? draw, int? start, int? length, long PartId = 0)
        {
            MultiStoreroomPartVM objMSPVM = new MultiStoreroomPartVM();
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var StoreroomItems = mspWrapper.PopulateChilditems(PartId);
            StoreroomItems = this.GetStoreroomItemsByColumnWithOrder(order, orderDir, StoreroomItems);


            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = StoreroomItems.Count();
            totalRecords = StoreroomItems.Count();

            int initialPage = start.Value;
            var filteredResult = StoreroomItems
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .OrderBy(x=>x.StoreroomName)      // RKL - V2-1091
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<MultiStoreroomPartChildModel> GetStoreroomItemsByColumnWithOrder(string order, string orderDir, List<MultiStoreroomPartChildModel> data)
        {
            List<MultiStoreroomPartChildModel> lst = new List<MultiStoreroomPartChildModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StoreroomName).ToList() : data.OrderBy(p => p.StoreroomName).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QtyOnHand).ToList() : data.OrderBy(p => p.QtyOnHand).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QtyMaximum).ToList() : data.OrderBy(p => p.QtyMaximum).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QtyReorderLevel).ToList() : data.OrderBy(p => p.QtyReorderLevel).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Location1_1).ToList() : data.OrderBy(p => p.Location1_1).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Location1_2).ToList() : data.OrderBy(p => p.Location1_2).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Location1_3).ToList() : data.OrderBy(p => p.Location1_3).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Location1_4).ToList() : data.OrderBy(p => p.Location1_4).ToList();
                    break;

            }
            return lst;
        }

        [HttpGet]
        public PartialViewResult AddEditStoreroom(long partId, long storeroomId, long PartStoreroomId)
        {
            MultiStoreroomPartVM multiStoreroomPartVM = new MultiStoreroomPartVM();
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            MultiStoreroomPartModel mspModel = new MultiStoreroomPartModel();
            StoreroomModel StoreroomModel = new StoreroomModel();
            multiStoreroomPartVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Maintain);
            if (storeroomId == 0)
            {
                ViewBag.Mode = "Add";
                StoreroomModel.PartId = partId;
            }
            else
            {
                var selectedStoreroomItems = mspWrapper.PopulateStoreroomInnerGridViewData(PartStoreroomId);
                //var selectedStoreroomItems = StoreroomItems != null ? StoreroomItems.Where(x => x.StoreroomId == storeroomId).SingleOrDefault() : null;
                StoreroomModel.PartId = partId;
                StoreroomModel.StoreroomId = selectedStoreroomItems.StoreroomId;
                StoreroomModel.PartStoreroomId = selectedStoreroomItems.PartStoreroomId;
                StoreroomModel.Section = selectedStoreroomItems.Location1_1;
                StoreroomModel.Row = selectedStoreroomItems.Location1_2;
                StoreroomModel.Shelf = selectedStoreroomItems.Location1_3;
                StoreroomModel.Bin = selectedStoreroomItems.Location1_4;
                StoreroomModel.Section2 = selectedStoreroomItems.Location2_1;
                StoreroomModel.Row2 = selectedStoreroomItems.Location2_2;
                StoreroomModel.Shelf2 = selectedStoreroomItems.Location2_3;
                StoreroomModel.Bin2 = selectedStoreroomItems.Location2_4;
                StoreroomModel.QuantityOnHand = selectedStoreroomItems.QuantityOnHand;
                StoreroomModel.MaximumQuantity = selectedStoreroomItems.MaximumQuantity;
                StoreroomModel.MinimumQuantity = selectedStoreroomItems.MinimumQuantity;
                StoreroomModel.CountFrequency = selectedStoreroomItems.CountFrequency;
                StoreroomModel.LastCounted = selectedStoreroomItems.LastCounted;
                StoreroomModel.Id = selectedStoreroomItems.StoreroomId;
                StoreroomModel.PartVendorId = selectedStoreroomItems.PartVendorId;
                StoreroomModel.AutoPurchase = selectedStoreroomItems.AutoPurchase;
                StoreroomModel.Critical = selectedStoreroomItems.Critical;
                StoreroomModel.PartVendorIdClientLookupId = selectedStoreroomItems.VendorClientLookupId;

                //StoreroomModel.Maintain = selectedStoreroomItems.Maintain;
            }
            multiStoreroomPartVM.StoreroomModel = StoreroomModel;
            LocalizeControls(multiStoreroomPartVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_AddEditStoreroom.cshtml", multiStoreroomPartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveStoreroom(MultiStoreroomPartVM mspVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
                if (mspVM.StoreroomModel.Id == 0)
                {
                    Mode = "add";
                }
                else
                {
                    Mode = "update";
                }
                var objStoreroom = mspWrapper.SaveStoreroom(mspVM.StoreroomModel);


                if (objStoreroom.ErrorMessages != null && objStoreroom.ErrorMessages.Count != 0)
                {
                    return Json(objStoreroom.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Photos
        public JsonResult DeleteImageFromAzure(string _PartId, string TableName, bool Profile, bool Image)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteAzureImage(Convert.ToInt64(_PartId), AttachmentTableConstant.Part, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteImageFromOnPremise(string _PartId, string TableName, bool Profile, bool Image)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteOnPremiseImage(Convert.ToInt64(_PartId), AttachmentTableConstant.Part, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Comment       
        [HttpPost]
        public PartialViewResult LoadComments(long PartId)
        {
            MultiStoreroomPartVM objMSPartVM = new MultiStoreroomPartVM();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Personnel> personnelsList = new List<Personnel>();
            List<Notes> NotesList = new List<Notes>();
            UserMentionData userMentionData;
            List<UserMentionData> userMentionDatas = new List<UserMentionData>();
            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => personnelsList = coWrapper.MentionList(""));
            tasks[1] = Task.Factory.StartNew(() => NotesList = coWrapper.PopulateComment(PartId, "Part"));
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
                objMSPartVM.userMentionDatas = userMentionDatas;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                objMSPartVM.NotesList = NotesList;
            }
            LocalizeControls(objMSPartVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("_CommentsList", objMSPartVM);
        }
        [ValidateInput(false)]
        [HttpPost]

        public ActionResult AddOrUpdateComment(long partid, string content, string PartClientLookupId, List<string> userList, long noteId = 0, long updatedindex = 0)
        {
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionData> userMentionDataList = new List<UserMentionData>();
            UserMentionData objUserMentionData;
            if (userList != null && userList.Count > 0)
            {
                foreach (var item in userList)
                {
                    objUserMentionData = new UserMentionData(); // UserMentionData();
                    objUserMentionData.userId = namelist.Where(x => x.UserName == item).Select(y => y.PersonnelId).FirstOrDefault();
                    objUserMentionData.userName = item;
                    objUserMentionData.emailId = namelist.Where(x => x.UserName == item).Select(y => y.Email).FirstOrDefault();
                    userMentionDataList.Add(objUserMentionData);
                }
            }
            NotesModel notesModel = new NotesModel();
            notesModel.ObjectId = partid;
            notesModel.Content = content;
            notesModel.NotesId = noteId;
            notesModel.ClientLookupId = PartClientLookupId;
            notesModel.updatedindex = updatedindex;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateComment(notesModel, ref Mode, "Part", userMentionDataList);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), partid = partid, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Attachment
        [HttpPost]
        public string PopulateAttachment(int? draw, int? start, int? length, long _partId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var Attachments = objCommonWrapper.PopulateAttachments(_partId, "Part", userData.Security.Parts.Edit);
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
        [HttpPost]
        public ActionResult DeleteMSPAttachment(long _fileAttachmentId)
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
        [HttpGet]
        public PartialViewResult AddMSPAttachment(long partId, string ClientLookupId)
        {
            MultiStoreroomPartVM objVM = new MultiStoreroomPartVM();
            AttachmentModel Attachment = new AttachmentModel();

            MultiStoreroomPartModel objPart = new MultiStoreroomPartModel();
            objPart.PartId = partId;
            objPart.ClientLookupId = ClientLookupId;
            Attachment.PartId = partId;
            objVM.AttachmentModel = Attachment;
            objVM.MultiStoreroomPartModel = objPart;
            objVM.multiStoreroomSummaryModel = GetPartSummary(partId);
            objVM.MultiStoreroomPartModel.PartImageUrl = objVM.multiStoreroomSummaryModel.PartImageUrl;
            LocalizeControls(objVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_MultiStoreroomPartAttachmentAdd.cshtml", objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMSPAttachment()
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
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["AttachmentModel.PartId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["AttachmentModel.Subject"]) ? "No Subject" : Request.Form["AttachmentModel.Subject"];
                attachmentModel.TableName = "Part";
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.Parts.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.Parts.Edit);
                }
                if (attachStatus)
                {
                    if (fileAtt != null && fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), partid = Convert.ToInt64(Request.Form["AttachmentModel.PartId"]) }, JsonRequestBehavior.AllowGet);
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
        #endregion

        #region Vendor
        [HttpPost]
        public string PopulateVendor(int? draw, int? start, int? length, long _partId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MultiStoreroomPartWrapper _MSPWrapperObj = new MultiStoreroomPartWrapper(userData);
            var pvx = _MSPWrapperObj.PopulateParts(_partId);
            List<MultiStoreroomPartVendorGridModel> PartsVendorGridModelList = new List<MultiStoreroomPartVendorGridModel>();
            MultiStoreroomPartVendorGridModel objMSPVendorGridModel;
            foreach (var v in pvx)
            {
                objMSPVendorGridModel = new MultiStoreroomPartVendorGridModel();
                objMSPVendorGridModel.Vendor_ClientLookupId = v.Vendor_ClientLookupId;
                objMSPVendorGridModel.Vendor_Name = v.Vendor_Name;
                objMSPVendorGridModel.ClientId = v.ClientId;
                objMSPVendorGridModel.Part_Vendor_XrefId = v.Part_Vendor_XrefId;
                objMSPVendorGridModel.PartId = v.PartId;
                objMSPVendorGridModel.VendorId = v.VendorId;
                objMSPVendorGridModel.PreferredVendor = v.PreferredVendor;
                objMSPVendorGridModel.CatalogNumber = v.CatalogNumber;
                objMSPVendorGridModel.IssueOrder = v.IssueOrder;
                objMSPVendorGridModel.Manufacturer = v.Manufacturer;
                objMSPVendorGridModel.ManufacturerId = v.ManufacturerId;
                objMSPVendorGridModel.OrderQuantity = v.OrderQuantity;
                objMSPVendorGridModel.OrderUnit = v.OrderUnit;
                objMSPVendorGridModel.Price = v.Price;
                objMSPVendorGridModel.UpdateIndex = v.UpdateIndex;
                objMSPVendorGridModel.UOMConvRequired = v.UOMConvRequired;
                PartsVendorGridModelList.Add(objMSPVendorGridModel);
            }
            var VendorsList = PartsVendorGridModelList;
            VendorsList = this.GetAllVendorsSortByColumnWithOrder(order, orderDir, VendorsList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = VendorsList.Count();
            totalRecords = VendorsList.Count();

            int initialPage = start.Value;

            var filteredResult = VendorsList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var partVendorSecurity = userData.Security.Parts.Part_Vendor_XRef == true ? "true" : "false";
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, partVendorSecurity = partVendorSecurity }, JsonSerializerDateSettings);
        }
        private List<MultiStoreroomPartVendorGridModel> GetAllVendorsSortByColumnWithOrder(string order, string orderDir, List<MultiStoreroomPartVendorGridModel> data)
        {
            List<MultiStoreroomPartVendorGridModel> lst = new List<MultiStoreroomPartVendorGridModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Vendor_ClientLookupId).ToList() : data.OrderBy(p => p.Vendor_ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Vendor_Name).ToList() : data.OrderBy(p => p.Vendor_Name).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CatalogNumber).ToList() : data.OrderBy(p => p.CatalogNumber).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Manufacturer).ToList() : data.OrderBy(p => p.Manufacturer).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ManufacturerId).ToList() : data.OrderBy(p => p.ManufacturerId).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OrderQuantity).ToList() : data.OrderBy(p => p.OrderQuantity).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OrderUnit).ToList() : data.OrderBy(p => p.OrderUnit).ToList();
                        break;
                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Price).ToList() : data.OrderBy(p => p.Price).ToList();
                        break;
                    case "8":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IssueOrder).ToList() : data.OrderBy(p => p.IssueOrder).ToList();
                        break;
                    case "9":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UOMConvRequired).ToList() : data.OrderBy(p => p.UOMConvRequired).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Vendor_ClientLookupId).ToList() : data.OrderBy(p => p.Vendor_ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }
        [HttpPost]
        public ActionResult MSPVendorDelete(long _PartVendorXrefId)
        {
            MultiStoreroomPartWrapper pWrapper = new MultiStoreroomPartWrapper(userData);
            var deleteResult = pWrapper.DeleteVendor(_PartVendorXrefId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult MSPVedndorAdd(long _partId, string ClientLookupId)
        {
            MultiStoreroomPartWrapper pWrapper = new MultiStoreroomPartWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            MultiStoreroomPartVendorModel mspVendorXref = new MultiStoreroomPartVendorModel();
            MultiStoreroomPartVM mspVM = new MultiStoreroomPartVM();
            MultiStoreroomPartModel mspModel = new MultiStoreroomPartModel();
            List<DataContracts.LookupList> objLookupUnitOfMeasure = new List<DataContracts.LookupList>();
            mspVendorXref.PartId = _partId;
            mspModel.PartId = _partId;
            mspModel.ClientLookupId = ClientLookupId;

            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                objLookupUnitOfMeasure = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (objLookupUnitOfMeasure != null)
                {
                    mspVendorXref.OrderUnitList = objLookupUnitOfMeasure.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            mspVM.MSPVendorModel = mspVendorXref;
            mspVM.MultiStoreroomPartModel = mspModel;
            mspVM.multiStoreroomSummaryModel = GetPartSummary(_partId);
            mspVM.MultiStoreroomPartModel.PartImageUrl = mspVM.multiStoreroomSummaryModel.PartImageUrl;
            LocalizeControls(mspVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("_MultiStoreroomPartVendorAdd", mspVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MSPVedndorAdd(MultiStoreroomPartVM mspVM)
        {
            MultiStoreroomPartWrapper _mspWrapperObj = new MultiStoreroomPartWrapper(userData);
            string Mode = string.Empty;
            Part_Vendor_Xref pvx = new Part_Vendor_Xref();
            if (ModelState.IsValid)
            {
                if (mspVM.MSPVendorModel.PartVendorXrefId != 0)
                {
                    Mode = "update";
                    pvx = _mspWrapperObj.UpdatePartVendorXref(mspVM.MSPVendorModel, mspVM.MSPVendorModel.PartId);
                }
                else
                {
                    Mode = "add";
                    pvx = _mspWrapperObj.AddPartVendorXref(mspVM.MSPVendorModel, mspVM.MSPVendorModel.PartId);
                }
                if (pvx.ErrorMessages != null && pvx.ErrorMessages.Count > 0)
                {
                    return Json(pvx.ErrorMessages, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), partid = mspVM.MSPVendorModel.PartId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult MSPVedndorEdit(long partId, long _part_Vendor_XrefId, int updatedIndex, string ClientLookupId)
        {
            MultiStoreroomPartWrapper pWrapper = new MultiStoreroomPartWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            MultiStoreroomPartVM mspVM = new MultiStoreroomPartVM();
            MultiStoreroomPartModel mspModel = new MultiStoreroomPartModel();
            List<DataContracts.LookupList> objLookupUnitOfMeasure = new List<DataContracts.LookupList>();
            mspModel.ClientLookupId = ClientLookupId;
            mspModel.PartId = partId;
            MultiStoreroomPartVendorModel partVendorXref = new MultiStoreroomPartVendorModel();
            partVendorXref = pWrapper.EditVendor(partId, _part_Vendor_XrefId, mspModel.ClientLookupId, updatedIndex);

            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                objLookupUnitOfMeasure = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (objLookupUnitOfMeasure != null)
                {
                    partVendorXref.OrderUnitList = objLookupUnitOfMeasure.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            mspVM.MSPVendorModel = partVendorXref;
            mspVM.MultiStoreroomPartModel = mspModel;
            mspVM.multiStoreroomSummaryModel = GetPartSummary(partId);
            mspVM.MultiStoreroomPartModel.PartImageUrl = mspVM.multiStoreroomSummaryModel.PartImageUrl;
            LocalizeControls(mspVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("_MultiStoreroomPartVendorAdd", mspVM);
        }
        #endregion

        #region Asset
        [HttpPost]
        public string PopulateEquipment(int? draw, int? start, int? length, long _partId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MultiStoreroomPartWrapper _mspWrapperObj = new MultiStoreroomPartWrapper(userData);
            var PartsEquipmentGridModelList = _mspWrapperObj.PopulateMSPEquipment(_partId);
            PartsEquipmentGridModelList = this.GetAllEquipmentsSortByColumnWithOrder(order, orderDir, PartsEquipmentGridModelList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PartsEquipmentGridModelList.Count();
            totalRecords = PartsEquipmentGridModelList.Count();
            int initialPage = start.Value;
            var filteredResult = PartsEquipmentGridModelList
               .Skip(initialPage * length ?? 0)
               .Take(length ?? 0)
               .ToList();
            var partEquipmentSecurity = userData.Security.Parts.Part_Equipment_XRef == true ? "true" : "false";
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, partEquipmentSecurity = partEquipmentSecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<MultiStoreroomPartEquipmentGridModel> GetAllEquipmentsSortByColumnWithOrder(string order, string orderDir, List<MultiStoreroomPartEquipmentGridModel> data)
        {
            List<MultiStoreroomPartEquipmentGridModel> eqpList = new List<MultiStoreroomPartEquipmentGridModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        eqpList = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Equipment_ClientLookupId).ToList() : data.OrderBy(p => p.Equipment_ClientLookupId).ToList();
                        break;
                    case "1":
                        eqpList = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Equipment_Name).ToList() : data.OrderBy(p => p.Equipment_Name).ToList();
                        break;
                    case "2":
                        eqpList = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QuantityNeeded).ToList() : data.OrderBy(p => p.QuantityNeeded).ToList();
                        break;
                    case "3":
                        eqpList = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QuantityUsed).ToList() : data.OrderBy(p => p.QuantityUsed).ToList();
                        break;
                    case "4":
                        eqpList = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Comment).ToList() : data.OrderBy(p => p.Comment).ToList();
                        break;
                    case "5":
                        eqpList = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EquipmentId).ToList() : data.OrderBy(p => p.EquipmentId).ToList();
                        break;
                    default:
                        eqpList = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Equipment_ClientLookupId).ToList() : data.OrderBy(p => p.Equipment_ClientLookupId).ToList();
                        break;
                }
            }
            return eqpList;
        }
        [HttpGet]
        public ActionResult MSPEquipmentAdd(long partId, string clientLookupId)
        {
            MultiStoreroomPartWrapper pWrapper = new MultiStoreroomPartWrapper(userData);
            MultiStoreroomPartEquipmentXrefModel mspEquipmentXref = new MultiStoreroomPartEquipmentXrefModel();
            MultiStoreroomPartVM mspVM = new MultiStoreroomPartVM();
            mspEquipmentXref.PartId = partId;
            mspVM.MSPEquipmentXrefModel = mspEquipmentXref;
            mspVM.MultiStoreroomPartModel = new MultiStoreroomPartModel();
            mspVM.MultiStoreroomPartModel.ClientLookupId = clientLookupId;
            mspVM.MultiStoreroomPartModel.PartId = partId;
            mspVM.multiStoreroomSummaryModel = GetPartSummary(partId);
            mspVM.MultiStoreroomPartModel.PartImageUrl = mspVM.multiStoreroomSummaryModel.PartImageUrl;
            LocalizeControls(mspVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("_MultiStoreroomPartEquipmentAdd", mspVM);
        }
        [HttpGet]
        public ActionResult MSPEquipmentEdit(long partId, long _equipment_Parts_XrefId, int updatedIndex, string ClientLookupId)
        {
            MultiStoreroomPartWrapper pWrapper = new MultiStoreroomPartWrapper(userData);
            MultiStoreroomPartVM mspVM = new MultiStoreroomPartVM();
            MultiStoreroomPartModel partModel = new MultiStoreroomPartModel();
            partModel.ClientLookupId = ClientLookupId;
            var _mspEquipmentXrefModel = pWrapper.EditPartEquipment(partId, _equipment_Parts_XrefId, ClientLookupId, updatedIndex);
            var EquipmentLookUplist = GetLookUpList_Equipment();
            if (EquipmentLookUplist != null)
            {
                _mspEquipmentXrefModel.EquipmentList = EquipmentLookUplist.Select(x => new SelectListItem { Text = x.Equipment + " - " + x.Name, Value = x.Equipment.ToString() });
            }
            mspVM.MSPEquipmentXrefModel = _mspEquipmentXrefModel;
            partModel.PartId = partId;
            mspVM.MultiStoreroomPartModel = partModel;
            mspVM.multiStoreroomSummaryModel = GetPartSummary(partId);
            mspVM.MultiStoreroomPartModel.PartImageUrl = mspVM.multiStoreroomSummaryModel.PartImageUrl;
            LocalizeControls(mspVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("_MultiStoreroomPartEquipmentAdd", mspVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MSPEquipmentAdd(MultiStoreroomPartVM mspVM)
        {
            MultiStoreroomPartWrapper _mspWrapperObj = new MultiStoreroomPartWrapper(userData);
            string Mode = string.Empty;
            Equipment_Parts_Xref eqx = new Equipment_Parts_Xref();
            if (ModelState.IsValid)
            {
                if (mspVM.MSPEquipmentXrefModel.Equipment_Parts_XrefId != 0)
                {
                    Mode = "update";
                    eqx = _mspWrapperObj.UpdateEquipmentPartsXref(mspVM.MSPEquipmentXrefModel, mspVM.MSPEquipmentXrefModel.PartId);
                }
                else
                {
                    Mode = "add";
                    eqx = _mspWrapperObj.AddEquipmentPartsXref(mspVM.MSPEquipmentXrefModel, mspVM.MSPEquipmentXrefModel.PartId);
                }
                if (eqx.ErrorMessages != null && eqx.ErrorMessages.Count > 0)
                {
                    return Json(eqx.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), partid = mspVM.MSPEquipmentXrefModel.PartId, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteMSPEquipmentXref(long _equipment_Parts_XrefId)
        {
            MultiStoreroomPartWrapper _PartsObj = new MultiStoreroomPartWrapper(userData);
            if (_PartsObj.DeleteEquipment(_equipment_Parts_XrefId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region History 
        [HttpPost]
        public string PopulateMSPHistoryReview(int? draw, int? start, int? length, decimal? TransactionQuantity, decimal? Cost, long partId = 0, int daterange = 0, string TransactionType = "", string Requestor_Name = "", string PerformBy_Name = "",
                                         DateTime? TransactionDate = null, string ChargeType_Primary = "", string ChargeTo_ClientLookupId = "",
                                             string Account_ClientLookupId = "", string PurchaseOrder_ClientLookupId = "", string Vendor_ClientLookupId = "", string Vendor_Name = ""
                                           )
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            List<MultiStoreroomPartHistoryModel> _mspHistoryReview = mspWrapper.GetDetailsMSPHistory(partId, daterange);
            var TotalPart = _mspHistoryReview;
            _mspHistoryReview = this.GetMSPHistoryGridSortByColumnWithOrder(order, orderDir, _mspHistoryReview);
            if (_mspHistoryReview != null)
            {
                if (!string.IsNullOrEmpty(TransactionType) && TransactionType != "0")
                {
                    TransactionType = TransactionType.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.TransactionType) && x.TransactionType.ToUpper().Contains(TransactionType))).ToList();
                }
                if (!string.IsNullOrEmpty(Requestor_Name))
                {
                    Requestor_Name = Requestor_Name.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Requestor_Name) && x.Requestor_Name.ToUpper().Contains(Requestor_Name))).ToList();
                }
                if (!string.IsNullOrEmpty(PerformBy_Name))
                {
                    PerformBy_Name = PerformBy_Name.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.PerformBy_Name) && x.PerformBy_Name.ToUpper().Contains(PerformBy_Name))).ToList();
                }
                if (TransactionDate != null)
                {
                    _mspHistoryReview = _mspHistoryReview.Where(x => (x.TransactionDate != null && x.TransactionDate.Value.Date.Equals(TransactionDate.Value.Date))).ToList();
                }
                if (TransactionQuantity.HasValue)
                {
                    _mspHistoryReview = _mspHistoryReview.Where(x => x.TransactionQuantity.Equals(TransactionQuantity)).ToList();
                }
                if (Cost.HasValue)
                {
                    _mspHistoryReview = _mspHistoryReview.Where(x => x.Cost.Equals(Cost)).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeType_Primary) && ChargeType_Primary != "0")
                {
                    ChargeType_Primary = ChargeType_Primary.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeType_Primary) && x.ChargeType_Primary.ToUpper().Contains(ChargeType_Primary))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeTo_ClientLookupId))
                {
                    ChargeTo_ClientLookupId = ChargeTo_ClientLookupId.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_ClientLookupId) && x.ChargeTo_ClientLookupId.ToUpper().Contains(ChargeTo_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Account_ClientLookupId))
                {
                    Account_ClientLookupId = Account_ClientLookupId.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Account_ClientLookupId) && x.Account_ClientLookupId.ToUpper().Contains(Account_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(PurchaseOrder_ClientLookupId))
                {
                    PurchaseOrder_ClientLookupId = PurchaseOrder_ClientLookupId.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.PurchaseOrder_ClientLookupId) && x.PurchaseOrder_ClientLookupId.ToUpper().Contains(PurchaseOrder_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Vendor_ClientLookupId) && Vendor_ClientLookupId != "0")
                {
                    Vendor_ClientLookupId = Vendor_ClientLookupId.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Vendor_ClientLookupId) && x.Vendor_ClientLookupId.ToUpper().Contains(Vendor_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Vendor_Name))
                {
                    Vendor_Name = Vendor_Name.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Vendor_Name) && x.Vendor_Name.ToUpper().Contains(Vendor_Name))).ToList();
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = _mspHistoryReview.Count();
            totalRecords = _mspHistoryReview.Count();
            int initialPage = start.Value;
            var filteredResult = _mspHistoryReview
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, dataAll = TotalPart }, JsonSerializerDateSettings);
        }

        [HttpGet]
        public string GetMSPHistoryPrintData(decimal? TransactionQuantity, decimal? Cost, long partId = 0, int daterange = 0, string TransactionType = "", string Requestor_Name = "", string PerformBy_Name = "", DateTime? TransactionDate = null, string ChargeType_Primary = "", string ChargeTo_ClientLookupId = "", string Account_ClientLookupId = "", string PurchaseOrder_ClientLookupId = "", string Vendor_ClientLookupId = "", string Vendor_Name = ""
                                           )
        {
            List<MultiStoreroomPartsHistoryPrintModel> mspSearchModelList = new List<MultiStoreroomPartsHistoryPrintModel>();
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            MultiStoreroomPartsHistoryPrintModel objMSPHistoryPrintModel;
            List<MultiStoreroomPartHistoryModel> _mspHistoryReview = mspWrapper.GetDetailsMSPHistory(partId, daterange);
            if (_mspHistoryReview != null)
            {
                if (!string.IsNullOrEmpty(TransactionType) && TransactionType != "0")
                {
                    TransactionType = TransactionType.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.TransactionType) && x.TransactionType.ToUpper().Contains(TransactionType))).ToList();
                }

                if (!string.IsNullOrEmpty(Requestor_Name))
                {
                    Requestor_Name = Requestor_Name.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Requestor_Name) && x.Requestor_Name.ToUpper().Contains(Requestor_Name))).ToList();
                }
                if (!string.IsNullOrEmpty(PerformBy_Name))
                {
                    PerformBy_Name = PerformBy_Name.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.PerformBy_Name) && x.PerformBy_Name.ToUpper().Contains(PerformBy_Name))).ToList();
                }
                if (TransactionDate != null)
                {
                    _mspHistoryReview = _mspHistoryReview.Where(x => (x.TransactionDate != null && x.TransactionDate.Value.Date.Equals(TransactionDate.Value.Date))).ToList();
                }
                if (TransactionQuantity.HasValue)
                {
                    _mspHistoryReview = _mspHistoryReview.Where(x => x.TransactionQuantity.Equals(TransactionQuantity)).ToList();
                }
                if (Cost.HasValue)
                {
                    _mspHistoryReview = _mspHistoryReview.Where(x => x.Cost.Equals(Cost)).ToList();
                }

                if (!string.IsNullOrEmpty(ChargeType_Primary) && ChargeType_Primary != "0")
                {
                    ChargeType_Primary = ChargeType_Primary.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeType_Primary) && x.ChargeType_Primary.ToUpper().Contains(ChargeType_Primary))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeTo_ClientLookupId))
                {
                    ChargeTo_ClientLookupId = ChargeTo_ClientLookupId.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_ClientLookupId) && x.ChargeTo_ClientLookupId.ToUpper().Contains(ChargeTo_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Account_ClientLookupId))
                {
                    Account_ClientLookupId = Account_ClientLookupId.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Account_ClientLookupId) && x.Account_ClientLookupId.ToUpper().Contains(Account_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(PurchaseOrder_ClientLookupId))
                {
                    PurchaseOrder_ClientLookupId = PurchaseOrder_ClientLookupId.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.PurchaseOrder_ClientLookupId) && x.PurchaseOrder_ClientLookupId.ToUpper().Contains(PurchaseOrder_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Vendor_ClientLookupId) && Vendor_ClientLookupId != "0")
                {
                    Vendor_ClientLookupId = Vendor_ClientLookupId.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Vendor_ClientLookupId) && x.Vendor_ClientLookupId.ToUpper().Contains(Vendor_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Vendor_Name))
                {
                    Vendor_Name = Vendor_Name.ToUpper();
                    _mspHistoryReview = _mspHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Vendor_Name) && x.Vendor_Name.ToUpper().Contains(Vendor_Name))).ToList();
                }
                foreach (var p in _mspHistoryReview)
                {
                    objMSPHistoryPrintModel = new MultiStoreroomPartsHistoryPrintModel();
                    objMSPHistoryPrintModel.TransactionType = p.TransactionType;
                    objMSPHistoryPrintModel.Requestor_Name = p.Requestor_Name;
                    objMSPHistoryPrintModel.PerformBy_Name = p.PerformBy_Name;
                    objMSPHistoryPrintModel.TransactionDate = p.TransactionDate;
                    objMSPHistoryPrintModel.TransactionQuantity = p.TransactionQuantity;
                    objMSPHistoryPrintModel.Cost = p.Cost;
                    objMSPHistoryPrintModel.ChargeType_Primary = p.ChargeType_Primary;
                    objMSPHistoryPrintModel.ChargeTo_ClientLookupId = p.ChargeTo_ClientLookupId;
                    objMSPHistoryPrintModel.Account_ClientLookupId = p.Account_ClientLookupId;
                    objMSPHistoryPrintModel.PurchaseOrder_ClientLookupId = p.PurchaseOrder_ClientLookupId;
                    objMSPHistoryPrintModel.Vendor_ClientLookupId = p.Vendor_ClientLookupId;
                    objMSPHistoryPrintModel.Vendor_Name = p.Vendor_Name;
                    objMSPHistoryPrintModel.Storeroom = p.Storeroom;
                    mspSearchModelList.Add(objMSPHistoryPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = mspSearchModelList }, JsonSerializerDateSettings);
        }

        private List<MultiStoreroomPartHistoryModel> GetMSPHistoryGridSortByColumnWithOrder(string order, string orderDir, List<MultiStoreroomPartHistoryModel> data)
        {
            List<MultiStoreroomPartHistoryModel> lst = new List<MultiStoreroomPartHistoryModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionType).ToList() : data.OrderBy(p => p.TransactionType).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Storeroom).ToList() : data.OrderBy(p => p.Storeroom).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Requestor_Name).ToList() : data.OrderBy(p => p.Requestor_Name).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PerformBy_Name).ToList() : data.OrderBy(p => p.PerformBy_Name).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionDate).ToList() : data.OrderBy(p => p.TransactionDate).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionQuantity).ToList() : data.OrderBy(p => p.TransactionQuantity).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Cost).ToList() : data.OrderBy(p => p.Cost).ToList();
                        break;
                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeType_Primary).ToList() : data.OrderBy(p => p.ChargeType_Primary).ToList();
                        break;
                    case "8":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_ClientLookupId).ToList() : data.OrderBy(p => p.ChargeTo_ClientLookupId).ToList();
                        break;
                    case "9":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Account_ClientLookupId).ToList() : data.OrderBy(p => p.Account_ClientLookupId).ToList();
                        break;
                    case "10":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PurchaseOrder_ClientLookupId).ToList() : data.OrderBy(p => p.PurchaseOrder_ClientLookupId).ToList();
                        break;
                    case "11":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Vendor_ClientLookupId).ToList() : data.OrderBy(p => p.Vendor_ClientLookupId).ToList();
                        break;
                    case "12":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Vendor_Name).ToList() : data.OrderBy(p => p.Vendor_Name).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionType).ToList() : data.OrderBy(p => p.TransactionType).ToList();
                        break;
                }
            }
            return lst;
        }
        #endregion

        #region MSP Receipt
        [HttpPost]
        public string populateMSPartReceipt(int? draw, int? start, int? length, decimal? OrderQuantity, decimal? UnitCost, long partId, DateTime? ReceivedDate, int dateRange = 0, string POClientLookupId = "",
                                             string VendorClientLookupId = "", string VendorName = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            List<MultiStoreroomReceiptModel> _mspartReceiptReview = mspWrapper.GetDetailsMSPartReceipt(partId, dateRange);
            var TotalReceipt = _mspartReceiptReview;
            _mspartReceiptReview = this.GetMSPartReceiptGridSortByColumnWithOrder(order, orderDir, _mspartReceiptReview);
            if (_mspartReceiptReview != null)
            {
                if (OrderQuantity.HasValue)
                {
                    _mspartReceiptReview = _mspartReceiptReview.Where(x => x.OrderQuantity.Equals(OrderQuantity)).ToList();
                }
                if (UnitCost.HasValue)
                {
                    _mspartReceiptReview = _mspartReceiptReview.Where(x => x.UnitCost.Equals(UnitCost)).ToList();
                }
                if (!string.IsNullOrEmpty(POClientLookupId))
                {
                    _mspartReceiptReview = _mspartReceiptReview.Where(x => (!string.IsNullOrWhiteSpace(x.POClientLookupId)) && x.POClientLookupId.ToUpper().Contains(POClientLookupId)).ToList();
                }
                if (ReceivedDate != null)
                {
                    _mspartReceiptReview = _mspartReceiptReview.Where(x => (x.ReceivedDate != null && x.ReceivedDate.Value.Date.Equals(ReceivedDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorClientLookupId) && VendorClientLookupId != "0")
                {
                    VendorClientLookupId = VendorClientLookupId.ToUpper();
                    _mspartReceiptReview = _mspartReceiptReview.Where(x => (!string.IsNullOrWhiteSpace(x.VendorClientLookupId) && x.VendorClientLookupId.ToUpper().Contains(VendorClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorName))
                {
                    VendorName = VendorName.ToUpper();
                    _mspartReceiptReview = _mspartReceiptReview.Where(x => (!string.IsNullOrWhiteSpace(x.VendorName) && x.VendorName.ToUpper().Contains(VendorName))).ToList();
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = _mspartReceiptReview.Count();
            totalRecords = _mspartReceiptReview.Count();
            int initialPage = start.Value;
            var filteredResult = _mspartReceiptReview
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, dataAll = TotalReceipt }, JsonSerializerDateSettings);
        }

        [HttpGet]
        public string GetMSPartsReceiptPrintData(decimal? OrderQuantity, decimal? UnitCost, long partId, DateTime? ReceivedDate, int dateRange = 0, string POClientLookupId = "",
                                             string VendorClientLookupId = "", string VendorName = ""
                                         )
        {
            List<MultiStoreroomPartReceipPrintModel> pSearchModelList = new List<MultiStoreroomPartReceipPrintModel>();
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            MultiStoreroomPartReceipPrintModel objMSPartsReceiptPrintModel;
            List<MultiStoreroomReceiptModel> _mspartReceiptReview = mspWrapper.GetDetailsMSPartReceipt(partId, dateRange);
            if (_mspartReceiptReview != null)
            {
                if (OrderQuantity.HasValue)
                {
                    _mspartReceiptReview = _mspartReceiptReview.Where(x => x.OrderQuantity.Equals(OrderQuantity)).ToList();
                }
                if (UnitCost.HasValue)
                {
                    _mspartReceiptReview = _mspartReceiptReview.Where(x => x.UnitCost.Equals(UnitCost)).ToList();
                }
                if (!string.IsNullOrEmpty(POClientLookupId))
                {
                    _mspartReceiptReview = _mspartReceiptReview.Where(x => (!string.IsNullOrWhiteSpace(x.POClientLookupId)) && x.POClientLookupId.ToUpper().Contains(POClientLookupId)).ToList();
                }
                if (ReceivedDate != null)
                {
                    _mspartReceiptReview = _mspartReceiptReview.Where(x => (x.ReceivedDate != null && x.ReceivedDate.Value.Date.Equals(ReceivedDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorClientLookupId) && VendorClientLookupId != "0")
                {
                    VendorClientLookupId = VendorClientLookupId.ToUpper();
                    _mspartReceiptReview = _mspartReceiptReview.Where(x => (!string.IsNullOrWhiteSpace(x.VendorClientLookupId) && x.VendorClientLookupId.ToUpper().Contains(VendorClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorName))
                {
                    VendorName = VendorName.ToUpper();
                    _mspartReceiptReview = _mspartReceiptReview.Where(x => (!string.IsNullOrWhiteSpace(x.VendorName) && x.VendorName.ToUpper().Contains(VendorName))).ToList();
                }
                foreach (var p in _mspartReceiptReview)
                {
                    objMSPartsReceiptPrintModel = new MultiStoreroomPartReceipPrintModel();
                    objMSPartsReceiptPrintModel.OrderQuantity = p.OrderQuantity;
                    objMSPartsReceiptPrintModel.UnitCost = p.UnitCost;
                    objMSPartsReceiptPrintModel.ReceivedDate = p.ReceivedDate;
                    objMSPartsReceiptPrintModel.POClientLookupId = p.POClientLookupId;
                    objMSPartsReceiptPrintModel.VendorClientLookupId = p.VendorClientLookupId;
                    objMSPartsReceiptPrintModel.VendorName = p.VendorName;
                    pSearchModelList.Add(objMSPartsReceiptPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = pSearchModelList }, JsonSerializerDateSettings);
        }
        private List<MultiStoreroomReceiptModel> GetMSPartReceiptGridSortByColumnWithOrder(string order, string orderDir, List<MultiStoreroomReceiptModel> data)
        {
            List<MultiStoreroomReceiptModel> lst = new List<MultiStoreroomReceiptModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.POClientLookupId).ToList() : data.OrderBy(p => p.POClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReceivedDate).ToList() : data.OrderBy(p => p.ReceivedDate).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorClientLookupId).ToList() : data.OrderBy(p => p.VendorClientLookupId).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorName).ToList() : data.OrderBy(p => p.VendorName).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OrderQuantity).ToList() : data.OrderBy(p => p.OrderQuantity).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.POClientLookupId).ToList() : data.OrderBy(p => p.POClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }

        #endregion

        #region V2-716 Multiple Images
        public PartialViewResult GetImages(int currentpage, int? start, int? length, long PartId)
        {
            MultiStoreroomPartVM multiStoreroomPartVM = new MultiStoreroomPartVM();
            List<ImageAttachmentModel> imgDatalist = new List<ImageAttachmentModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Attachment> Attachments = new List<Attachment>();
            multiStoreroomPartVM.security = this.userData.Security;
            multiStoreroomPartVM._userdata=this.userData;
            start = start.HasValue
            ? start / length
            : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;

            if (userData.DatabaseKey.Client.OnPremise)
            {
                Attachments = commonWrapper.GetOnPremiseMultipleImageUrl(skip, length ?? 0, PartId, AttachmentTableConstant.Part);
            }
            else
            {
                Attachments = commonWrapper.GetAzureMultipleImageUrl(skip, length ?? 0, PartId, AttachmentTableConstant.Part);
            }
            foreach (var attachment in Attachments)
            {
                ImageAttachmentModel imgdata = new ImageAttachmentModel();
                imgdata.AttachmentId=attachment.AttachmentId;
                imgdata.ObjectId=attachment.ObjectId;
                imgdata.Profile=attachment.Profile;
                imgdata.FileName=attachment.FileName;
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(attachment.AttachmentURL))
                {
                    SASImageURL= attachment.AttachmentURL;
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
            multiStoreroomPartVM.imageAttachmentModels = imgDatalist;
            LocalizeControls(multiStoreroomPartVM, LocalizeResourceSetConstants.Global);

            return PartialView("~/Views/MultiStoreroomPart/_AllMultiStoreroomImages.cshtml", multiStoreroomPartVM);
        }
        #endregion

        #region V2-755
        #region Part Checkout
        [HttpGet]
        public PartialViewResult PartCheckOut(long partId, long storeroomId, string PartClientLookupId, string Description, string UPCCode)
        {
            MultiStoreroomPartVM multiStoreroomPartVM = new MultiStoreroomPartVM();
            ParInvCheckoutModel checkoutModel = new ParInvCheckoutModel();
            checkoutModel.StoreroomId = storeroomId;
            checkoutModel.PartClientLookupId = PartClientLookupId;
            checkoutModel.PartId = partId;
            checkoutModel.PartDescription= Description;
            checkoutModel.UPCCode = UPCCode;
            var ChargeTypeList = UtilityFunction.populateChargeTypeForInventoryCheckout();
            if (ChargeTypeList != null)
            {
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                {
                    ChargeTypeList = ChargeTypeList.Where(x => x.text != ChargeType.Location).ToList();
                    checkoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                else
                {
                    checkoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
            }
            multiStoreroomPartVM.inventoryCheckoutModel = checkoutModel;
            LocalizeControls(multiStoreroomPartVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_PartCheckoutModal.cshtml", multiStoreroomPartVM);
        }
        [HttpPost]
        public ActionResult SaveInventorydata(MultiStoreroomPartVM data)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                var mydata = data.inventoryCheckoutModel;
                MultiStoreroomPartWrapper invWrapper = new MultiStoreroomPartWrapper(userData);
                mydata.IssueToClentLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
                var partHistoryListTemp = invWrapper.SavePartCheckOut(mydata);
                if (partHistoryListTemp.Count > 0)
                {
                    if (partHistoryListTemp != null && partHistoryListTemp[0].ErrorMessages.Count > 0)
                    {
                        return Json(partHistoryListTemp[0].ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), data = mydata }, JsonRequestBehavior.AllowGet);
                }

            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Adjust On Hand Quantity
        [HttpGet]
        public PartialViewResult ChangeHandsOnQuantity(long partId, long storeroomId, string PartClientLookupId, string Description, string UPCCode)
        {
            MultiStoreroomPartVM multiStoreroomPartVM = new MultiStoreroomPartVM();
            MultiStoreroomPartPhysicalInvModel inventoryModel = new MultiStoreroomPartPhysicalInvModel();
            inventoryModel.StoreroomId = storeroomId;
            inventoryModel.PartClientLookupId = PartClientLookupId;
            inventoryModel.PartId = partId;
            inventoryModel.Description = Description;
            inventoryModel.PartUPCCode = UPCCode;
            multiStoreroomPartVM.inventoryModel = inventoryModel;
            LocalizeControls(multiStoreroomPartVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_ChangeHandsOnQtyModal.cshtml", multiStoreroomPartVM);
        }
        [HttpPost]
        public JsonResult SaveItemPhysicalInventory(MultiStoreroomPartVM model)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                MultiStoreroomPartGridPhysicalInvList gridPhysical = new MultiStoreroomPartGridPhysicalInvList
                {
                    PartId = model.inventoryModel.PartId,
                    QuantityCount = model.inventoryModel.ReceiptQuantity,
                    PartClientLookupId = model.inventoryModel.PartClientLookupId,
                    Description = model.inventoryModel.Description,
                    PartUPCCode = model.inventoryModel.PartUPCCode,
                    StoreroomId= model.inventoryModel.StoreroomId
                };
                MultiStoreroomPartWrapper phyWrapper = new MultiStoreroomPartWrapper(userData);
                PartHistory returnObj = phyWrapper.SaveHandsOnQty(gridPhysical);
                return Json(new { Result = JsonReturnEnum.success.ToString(), data = gridPhysical }, JsonRequestBehavior.AllowGet);
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region V2-751
        [HttpGet]
        public PartialViewResult PartTransferRequest(long partId, string ClientLookupId, long PartStoreroomId, long storeroomId, string Description, string StoreroomName)
        {
            MultiStoreroomPartVM multiStoreroomPartVM = new MultiStoreroomPartVM();
            AddPartTransferRequest PartTransferRequest = new AddPartTransferRequest();
            PartTransferRequest.RequestStoreroomId = storeroomId;
            PartTransferRequest.RequestPartStoreroomId = PartStoreroomId;
            PartTransferRequest.ClientLookupId = ClientLookupId;
            PartTransferRequest.PartId = partId;
            PartTransferRequest.Description = Description;
            PartTransferRequest.RequestStoreroomName = StoreroomName;
            MultiStoreroomPartWrapper MSWrapper = new MultiStoreroomPartWrapper(userData);
            PartTransferRequest.StoreroomList = MSWrapper.GetIssuingStoreroomList(partId, storeroomId);
            multiStoreroomPartVM.addPartTransferRequest = PartTransferRequest;
            LocalizeControls(multiStoreroomPartVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_AddPartTransferRequest.cshtml", multiStoreroomPartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveAddPartTransferRequest(MultiStoreroomPartVM model)
        {
            string ModelValidationFailedMessage = string.Empty;
            var PartStoreroomIdAndStoreroomId = model.addPartTransferRequest.IssuePartStoreroomIdAndStoreroomId.Split('#');
            long IssueStoreroomId = Convert.ToInt64(PartStoreroomIdAndStoreroomId[1]);
            if (ModelState.IsValid && IssueStoreroomId != model.addPartTransferRequest.RequestStoreroomId)
            {
                MultiStoreroomPartWrapper MSWrapper = new MultiStoreroomPartWrapper(userData);
                StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
                storeroomTransfer = MSWrapper.savePartTransferRequest(model.addPartTransferRequest);
                if (storeroomTransfer.ErrorMessages != null && storeroomTransfer.ErrorMessages.Count > 0)
                {
                    return Json(storeroomTransfer.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), data = model.addPartTransferRequest }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region V2-1007
        public RedirectResult MultiStoreroomPartsDetailFromEquipment(long partId, long equipmentId)
        {
            TempData["Mode"] = "MultiStoreroomPartsDetailFromEquipment";
            string strPartId = Convert.ToString(partId);
            TempData["partId"] = strPartId;
            string strEquipmentId = Convert.ToString(equipmentId);
            TempData["equipmentId"] = strEquipmentId;
            return Redirect("/MultiStoreroomPart/Index?page=Inventory_Part");
        }
        #endregion

        #region
        public ActionResult GetStoreroomInnerGridViewData(long PartStoreroomId)
        {
            MultiStoreroomPartVM objMSPVM = new MultiStoreroomPartVM();
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            objMSPVM.StoreroomInnerChild = mspWrapper.PopulateStoreroomInnerGridViewData(PartStoreroomId);
            LocalizeControls(objMSPVM, LocalizeResourceSetConstants.PartDetails);
            return View("_StoreroomChildDetailsView", objMSPVM);
        }
        #endregion

        #region Find Part ClientLookupId V2-1045
        [HttpGet]
        public PartialViewResult FindPart()
        {
            MultiStoreroomPartVM multiStoreroomPartVM = new MultiStoreroomPartVM();
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            MultiStoreroomPartModel mspModel = new MultiStoreroomPartModel();
            StoreroomModel StoreroomModel = new StoreroomModel();
            LocalizeControls(multiStoreroomPartVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_FindPartClientLookupIdModal.cshtml", multiStoreroomPartVM);
        }
        [HttpPost]
        public JsonResult FindPartClientLookupIdForFindPart(MultiStoreroomPartVM model)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                var mspWrapper = new MultiStoreroomPartWrapper(userData);
                var part = mspWrapper.RetrievePartIdByClientLookUpForFindPart(model.multiStoreroomPartClientLookupIdModel.PartClientLookupId);
                if (part.PartId != 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PartId = part.PartId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.failed.ToString(), PartClientLookupId = model.multiStoreroomPartClientLookupIdModel.PartClientLookupId }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region V2-1059
        [HttpGet]
        public PartialViewResult AddToAutoTransferStoreroom(long partId, long partStoreroomId, long storeroomId)
        {
            MultiStoreroomPartVM multiStoreroomPartVM = new MultiStoreroomPartVM();
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            AddToAutoTransfer AutoTransfer = new AddToAutoTransfer();
            AutoTransfer.PartId = partId;
            AutoTransfer.PartStoreroomId = partStoreroomId;
            AutoTransfer.StoreroomId = storeroomId;
            multiStoreroomPartVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Maintain);
            multiStoreroomPartVM.addToAutoTransfer = AutoTransfer;
            LocalizeControls(multiStoreroomPartVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_AddToAutoTransferStoreroom.cshtml", multiStoreroomPartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveAddtoAutoTransfer(MultiStoreroomPartVM model)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                MultiStoreroomPartWrapper MSWrapper = new MultiStoreroomPartWrapper(userData);
                PartStoreroom partStoreroom = new PartStoreroom();
                partStoreroom = MSWrapper.AddtoAutoTransferUpdate(model.addToAutoTransfer);
                if (partStoreroom.ErrorMessages != null && partStoreroom.ErrorMessages.Count > 0)
                {
                    return Json(partStoreroom.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), data = model.addToAutoTransfer }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }

        #region Remove from Auto Transfer Storeroom
        public JsonResult RemovefromAutoTransferStoreroom(long partId, long partStoreroomId, long storeroomId)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                MultiStoreroomPartVM multiStoreroomPartVM = new MultiStoreroomPartVM();
                MultiStoreroomPartWrapper MSWrapper = new MultiStoreroomPartWrapper(userData);
                AddToAutoTransfer AutoTransfer = new AddToAutoTransfer();
                AutoTransfer.PartId = partId;
                AutoTransfer.PartStoreroomId = partStoreroomId;
                AutoTransfer.StoreroomId = storeroomId;

                multiStoreroomPartVM.addToAutoTransfer = AutoTransfer;
                PartStoreroom partStoreroom = new PartStoreroom();
                partStoreroom = MSWrapper.RemovefromAutoTransferUpdate(multiStoreroomPartVM.addToAutoTransfer);
                if (partStoreroom.ErrorMessages != null && partStoreroom.ErrorMessages.Count > 0)
                {
                    return Json(partStoreroom.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), data = AutoTransfer }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region V2-1070 InActivate/Activate
        [HttpPost]
        public JsonResult ValidateForActiveInactive(bool InActiveFlag, long PartId, string ClientLookupId)
        {
            string validationMessage = string.Empty;
            MultiStoreroomPartWrapper pStoreroomWrapper = new MultiStoreroomPartWrapper(userData);
            string flag = string.Empty;
            if (InActiveFlag)
            {
                flag = ActivationStatusConstant.Activate;
            }
            else
            {
                flag = ActivationStatusConstant.InActivate;
            }
            var partStoreroom = pStoreroomWrapper.ValidatePartStatusChange(PartId, flag, ClientLookupId);
            if (partStoreroom.ErrorMessages != null && partStoreroom.ErrorMessages.Count > 0)
            {
                return Json(partStoreroom.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { validationStatus = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult MakeActiveInactive(bool InActiveFlag, long PartId)
        {
            MultiStoreroomPartWrapper pWrapper = new MultiStoreroomPartWrapper(userData);
            var ErrorMessages = pWrapper.MakeActiveInactive(InActiveFlag, PartId);
            if (ErrorMessages != null && ErrorMessages.Count > 0)
            {
                return Json(ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-1089 DevExpress QRCode
        [EncryptedActionParameter]
        public ActionResult QRCodeGenerationUsingDevExpress(bool SmallLabel)
        {
            var PartIdsList = new List<string>();
            if (TempData["QRCodeMultiStoreroomPartIdList"] != null)
            {
                PartIdsList = (List<string>)TempData["QRCodeMultiStoreroomPartIdList"];
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
            TempData["QRCodeMultiStoreroomPartIdList"] = PartIdsList;
            return Json(new { JsonReturnEnum.success });
        }
        public ActionResult GenerateEPMPartQRcode()
        {
            var PartIdsList = new List<string>();
            if (TempData["QRCodeMultiStoreroomPartIdList"] != null)
            {
                PartIdsList = (List<string>)TempData["QRCodeMultiStoreroomPartIdList"];
            }
            // Generate QR code report for each part in the PartIdsList
            var masterReport = new XtraReport();
            foreach (var part in PartIdsList)
            {
                var splitArray = part.Split(new string[] { "][" }, StringSplitOptions.None);
                var report = new EPMPartQRCodeTemplate
                {
                    DisplayName = "Part",
                    PartBarCode = splitArray[0],
                    ClientLookupId = splitArray[0],
                    Description = splitArray[1],
                    IssueUnit = splitArray[3],
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

        #region Part-Add/Edit V2-1187 Add Part Dynamic Ui Configurations
        public PartialViewResult AddPartsDynamic()
        {
            MultiStoreroomPartVM objMSPVM = new MultiStoreroomPartVM();
            MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);
            MultiStoreroomPartModel mspModel = new MultiStoreroomPartModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objMSPVM.security = this.userData.Security;
            objMSPVM.AddPart = new Models.MultiStoreroomPart.UIConfiguration.AddPartModelDynamic();
            var AllLookUps = commonWrapper.GetAllLookUpList();
            objMSPVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddPart, userData);
            IList<string> LookupNames = objMSPVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();

            if (AllLookUps != null)
            {
                objMSPVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }

            var AcclookUpList = GetAccountByActiveState(true);

            if (AcclookUpList != null)
            {
                objMSPVM.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            }
      ;
            LocalizeControls(objMSPVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_AddPartDynamic.cshtml", objMSPVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPartsDynamic(MultiStoreroomPartVM mspVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                MultiStoreroomPartWrapper mspWrapper = new MultiStoreroomPartWrapper(userData);

                var objPart = mspWrapper.AddPartsDynamic(mspVM);

                if (objPart.ErrorMessages != null && objPart.ErrorMessages.Count > 0)
                {
                    return Json(objPart.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, PartId = objPart.PartId, mode = "add" }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EditPartsDynamic(long PartId)
        {
            MultiStoreroomPartVM objPartsVM = new MultiStoreroomPartVM();
            MultiStoreroomPartSummaryModel msPartSummaryModel = new MultiStoreroomPartSummaryModel();
            MultiStoreroomPartWrapper pWrapper = new MultiStoreroomPartWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            objPartsVM.security = this.userData.Security;
            msPartSummaryModel = GetPartSummary(PartId);
            objPartsVM.multiStoreroomSummaryModel = msPartSummaryModel;
            objPartsVM.EditPart = new Models.MultiStoreroomPart.UIConfiguration.EditPartModelDynamic();

            objPartsVM.EditPart = pWrapper.RetrievePartDetailsByPartId(PartId);

            var AllLookUps = comWrapper.GetAllLookUpList();
            objPartsVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.EditPart, userData);
            IList<string> LookupNames = objPartsVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();

            if (AllLookUps != null)
            {
                objPartsVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }

            var AcclookUpList = GetAccountByActiveState(true);

            if (AcclookUpList != null)
            {
                objPartsVM.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            }
            objPartsVM.multiStoreroomSummaryModel = GetPartSummary(PartId);
            objPartsVM.EditPart.PartImageUrl = objPartsVM.multiStoreroomSummaryModel.PartImageUrl;
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_EditPartDynamic.cshtml", objPartsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePartsDynamic(MultiStoreroomPartVM objPartsVM)
        {
            //string emptyValue = string.Empty;
            MultiStoreroomPartWrapper pWrapper = new MultiStoreroomPartWrapper(userData);
            Part part = new Part();

            if (ModelState.IsValid)
            {
                part = pWrapper.EditPartDynamic(objPartsVM);
                if (part.ErrorMessages != null && part.ErrorMessages.Count > 0)
                {
                    return Json(part.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), partid = part.PartId }, JsonRequestBehavior.AllowGet);
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

        #region V2-1203 PartModel

        [HttpPost]
        public PartialViewResult MultiStoreroomPartModelWizard(long PartId)
        {
            MultiStoreroomPartVM objMSPartsVM = new MultiStoreroomPartVM();
            MultiStoreroomPartWrapper pWrapper = new MultiStoreroomPartWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objMSPartsVM.security = this.userData.Security;
            objMSPartsVM.AddPart = new Models.MultiStoreroomPart.UIConfiguration.AddPartModelDynamic();
            objMSPartsVM.AddPart = pWrapper.RetrievePartDetailsByPartIdForMSPartModel(PartId);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            objMSPartsVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddPart, userData);
            IList<string> LookupNames = objMSPartsVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();

            if (AllLookUps != null)
            {
                objMSPartsVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }

            var AcclookUpList = GetAccountByActiveState(true);

            if (AcclookUpList != null)
            {
                objMSPartsVM.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            }
      ;

            objMSPartsVM.CurrentPartId = PartId;

            LocalizeControls(objMSPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/MultiStoreroomPart/_AddMultiStoreroomPartModelWizard.cshtml", objMSPartsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMSPartModel(MultiStoreroomPartVM partVM)
        {
            MultiStoreroomPartWrapper partWrapper = new MultiStoreroomPartWrapper(userData);
            if (ModelState.IsValid)
            {
                Part part = partWrapper.AddPartsModel(partVM);
                if (part.ErrorMessages != null && part.ErrorMessages.Count > 0)
                {
                    return Json(part.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PartId = part.PartId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global),
                    JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region V2-1196
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePartsConfigureAutoPurchasing(MultiStoreroomPartVM partVM)
        {
            PartsWrapper partWrapper = new PartsWrapper(userData);
            if (ModelState.IsValid)
            {
                PartStoreroom partStoreroom = partWrapper.UpdatePartsConfigureAutoPurchasing(partVM.partsConfigureAutoPurchasingModel);
                if (partStoreroom.ErrorMessages != null && partStoreroom.ErrorMessages.Count > 0)
                {
                    return Json(partStoreroom.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PartId = partStoreroom.PartId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global),
                JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AddPartsVendorConfigureAutoPurchasing(long PartId, long VendorId, string VendorClientLoopId)
        {
            PartsWrapper _PartsObj = new PartsWrapper(userData);
            Part_Vendor_Xref pvx;
            if (VendorId > 0 && !string.IsNullOrEmpty(VendorClientLoopId))
            {
                pvx = _PartsObj.AddPartVendorXrefPartsConfigureAutoPurchasing(PartId, VendorId, VendorClientLoopId);

                if (pvx.ErrorMessages != null && pvx.ErrorMessages.Count > 0)
                {
                    return Json(pvx.ErrorMessages, JsonRequestBehavior.AllowGet);

                }
                else
                {

                    PartsWrapper pWrapper = new PartsWrapper(userData);
                    var vendorParts = pWrapper.PopulateParts(PartId);
                    var VendorList = vendorParts.Select(x =>
                           new SelectListItem
                           {
                               Text = $"{x.Vendor_ClientLookupId} - {x.Vendor_Name}",
                               Value = x.Part_Vendor_XrefId.ToString()
                           }).ToList();

                    return Json(new { Result = JsonReturnEnum.success.ToString(), VendorList = VendorList }, JsonRequestBehavior.AllowGet);
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
    }
}