using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Controllers.Common;
using Client.DevExpressReport;
using Client.Localization;
using Client.Models;
using Client.Models.Common;
using Client.Models.Parts;
using Client.Models.Parts.UIConfiguration;


using Common.Constants;

using Database.Business;

using DataContracts;

using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;

using INTDataLayer.EL;

using Newtonsoft.Json;

using Org.BouncyCastle.Crypto;

using QRCoder;

using Rotativa;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using static Client.Models.Common.UserMentionDataModel;

namespace Client.Controllers.Parts
{
    public class PartsController : SomaxBaseController
    {
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

        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.Parts)]
        public ActionResult Index()
        {
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                //return RedirectToAction("Index", "MultiStoreroomPart");
                return Redirect("/MultiStoreroomPart/Index?page=Inventory_Part ");
            }

            PartsVM objPartVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartModel pModel = new PartModel();
            pModel.UserId = userData.DatabaseKey.User.UserInfoId;
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string mode = Convert.ToString(TempData["Mode"]);
            var partConsignmentList = UtilityFunction.PartConsignmentList();
            var ViewSearchList = UtilityFunction.PartViewSearchTypes();
            pModel.PartViewSearchList = ViewSearchList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            var AllLookUps = comWrapper.GetAllLookUpList();
            if (partConsignmentList != null)
            {
                pModel.PartConsignmentList = partConsignmentList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objPartVM.PartModel = pModel;
            if (mode == "add")
            {
                //V2-641
                objPartVM.AddPart = new Models.Parts.UIConfiguration.AddPartModelDynamic();
                objPartVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddPart, userData);
                IList<string> LookupNames = objPartVM.UIConfigurationDetails.ToList()
                                                .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                                .Select(s => s.LookupName)
                                                .ToList();
                if (AllLookUps != null)
                {
                    objPartVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                              .Select(s => new UILookupList
                                                              { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                              .ToList();
                }
                //V2-379
                //var AcclookUpList = GetLookupList_Account();
                var AcclookUpList = GetAccountByActiveState(true);
                if (AcclookUpList != null)
                {
                    objPartVM.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
                }
                //pModel = pWrapper.PopulateDropdownControls(pModel); //Comment for V2-641
                objPartVM.PartModel.IsPartAdd = true;
                ViewBag.IsPartsAdd = true;
            }
            else if (mode == "PartsDetailFromFleetAssets")
            {
                long partId = Convert.ToInt64(TempData["partId"]);
                PartsHistoryModel partsHistoryModel = new PartsHistoryModel();
                PartsReceiptModel partsReceiptModel = new PartsReceiptModel();
                PartSummaryModel partSummaryModel = new PartSummaryModel();
                CreatedLastUpdatedPartModel objCreatedLastUpdatedPartModel = new CreatedLastUpdatedPartModel();
                objCreatedLastUpdatedPartModel = pWrapper.PopulateCreateModifyDate(partId);
                var part = pWrapper.PopulatePartDetails(partId);
                pModel = part;

                #region part status for redirection
                if (part.InactiveFlag == false)
                {
                    if (part.OnHandQuantity <= part.OnOrderQuantity)
                    {
                        pModel.partStatusForRedirection = UtilityFunction.GetMessageFromResource("globalLowParts", LocalizeResourceSetConstants.Global);
                    }
                    else
                    {
                        pModel.partStatusForRedirection = UtilityFunction.GetMessageFromResource("globalActive", LocalizeResourceSetConstants.Global);
                    }
                }
                else
                {
                    pModel.partStatusForRedirection = UtilityFunction.GetMessageFromResource("globalInActive", LocalizeResourceSetConstants.Global);
                }
                #endregion

                var partMasterModel = pWrapper.PopulateReviewSiteDetails(partId);
                objPartVM.partMasterModel = partMasterModel;
                var PartHistorydateList = UtilityFunction.PartDatesList();
                if (PartHistorydateList != null)
                {
                    partsHistoryModel.HistoryDateList = PartHistorydateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                var PartReceiptdateList = UtilityFunction.PartDatesList();
                if (PartReceiptdateList != null)
                {
                    partsReceiptModel.ReceiptDateList = PartReceiptdateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                objPartVM.partsHistoryModel = partsHistoryModel;
                objPartVM.partsReceiptModel = partsReceiptModel;
                objPartVM.partSummaryModel = GetPartSummary(partId);
                pModel.PartImageUrl = objPartVM.partSummaryModel.PartImageUrl;
                objPartVM._userdata = this.userData;
                objPartVM.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
                objPartVM.UsePartMaster = this.userData.Site.UsePartMaster;
                objPartVM.createdLastUpdatedPartModel = objCreatedLastUpdatedPartModel;
                ViewBag.IsPartDetailsFromEquipment = true;
                objPartVM.IsPartDetailsFromEquipment = true;
                //----V2-641
                objPartVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPartWidget, userData);
                PartUDF PartUDF = pWrapper.RetrievePartUDFByPartId(partId);
                objPartVM.ViewPart = new ViewPartModelDynamic();
                objPartVM.ViewPart = pWrapper.MapPartDataForView(objPartVM.ViewPart, pModel);
                objPartVM.ViewPart = pWrapper.MapPartUDFDataForView(objPartVM.ViewPart, PartUDF);
                #region V2-1196
                GetAutoPurchaseConfigurationSetUp(objPartVM, pWrapper, pModel, partId);
                #endregion
            }
            else if (mode == "PartsDetailFromVendor")
            {
                long partId = Convert.ToInt64(TempData["partId"]);
                PartsHistoryModel partsHistoryModel = new PartsHistoryModel();
                PartsReceiptModel partsReceiptModel = new PartsReceiptModel();
                PartSummaryModel partSummaryModel = new PartSummaryModel();
                CreatedLastUpdatedPartModel objCreatedLastUpdatedPartModel = new CreatedLastUpdatedPartModel();
                objCreatedLastUpdatedPartModel = pWrapper.PopulateCreateModifyDate(partId);
                var part = pWrapper.PopulatePartDetails(partId);
                pModel = part;

                #region part status for redirection
                if (part.InactiveFlag == false && part.OnHandQuantity <= part.Minimum)
                {
                    pModel.partStatusForRedirection = UtilityFunction.GetMessageFromResource("globalLowParts", LocalizeResourceSetConstants.Global);
                }
                if (part.InactiveFlag == false)
                {
                    pModel.partStatusForRedirection = UtilityFunction.GetMessageFromResource("globalActive", LocalizeResourceSetConstants.Global);
                }
                if (part.InactiveFlag == true)
                {
                    pModel.partStatusForRedirection = UtilityFunction.GetMessageFromResource("globalInActive", LocalizeResourceSetConstants.Global);
                }
                #endregion

                var partMasterModel = pWrapper.PopulateReviewSiteDetails(partId);
                objPartVM.partMasterModel = partMasterModel;
                var PartHistorydateList = UtilityFunction.PartDatesList();
                if (PartHistorydateList != null)
                {
                    partsHistoryModel.HistoryDateList = PartHistorydateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                var PartReceiptdateList = UtilityFunction.PartDatesList();
                if (PartReceiptdateList != null)
                {
                    partsReceiptModel.ReceiptDateList = PartReceiptdateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                objPartVM.partsHistoryModel = partsHistoryModel;
                objPartVM.partsReceiptModel = partsReceiptModel;
                objPartVM.partSummaryModel = GetPartSummary(partId);
                pModel.PartImageUrl = objPartVM.partSummaryModel.PartImageUrl;
                objPartVM._userdata = this.userData;
                objPartVM.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
                objPartVM.UsePartMaster = this.userData.Site.UsePartMaster;
                objPartVM.createdLastUpdatedPartModel = objCreatedLastUpdatedPartModel;
                ViewBag.IsPartDetailsFromEquipment = true;
                objPartVM.IsPartDetailsFromEquipment = true;
                //---- V2-641
                objPartVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPartWidget, userData);
                PartUDF PartUDF = pWrapper.RetrievePartUDFByPartId(partId);
                objPartVM.ViewPart = new ViewPartModelDynamic();
                objPartVM.ViewPart = pWrapper.MapPartDataForView(objPartVM.ViewPart, pModel);
                objPartVM.ViewPart = pWrapper.MapPartUDFDataForView(objPartVM.ViewPart, PartUDF);
                #region V2-1196
                GetAutoPurchaseConfigurationSetUp(objPartVM, pWrapper, pModel, partId);
                #endregion
            }
            else if (mode == "PartFromPartDetails")
            {
                string statusVal = pModel.PartViewSearchList.FirstOrDefault(z => z.Text == TempData["PartStatus"].ToString()).Value;
                objPartVM.PartStatusVal = statusVal;
            }
            else if (mode == "PartsDetailFromEquipment") //V2-1007
            {
                long partId = Convert.ToInt64(TempData["partId"]);
                long equipmentId = Convert.ToInt64(TempData["equipmentId"]);
                
                PartsHistoryModel partsHistoryModel = new PartsHistoryModel();
                PartsReceiptModel partsReceiptModel = new PartsReceiptModel();
                PartSummaryModel partSummaryModel = new PartSummaryModel();
                CreatedLastUpdatedPartModel objCreatedLastUpdatedPartModel = new CreatedLastUpdatedPartModel();
                objCreatedLastUpdatedPartModel = pWrapper.PopulateCreateModifyDate(partId);
                var part = pWrapper.PopulatePartDetails(partId);
                pModel = part;
                #region V2-1007
                EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
                var Parts = eWrapper.GetEquipmentPartsByEquipmentIdPartId(partId, equipmentId);
                var Equipment_ClientLookupId=Parts.Select(a=>a.Equipment_ClientLookupId).FirstOrDefault().ToString();
                objPartVM.EquipmentId = equipmentId;
                #endregion
                #region part status for redirection
                if (part.InactiveFlag == false)
                {
                    if (part.OnHandQuantity <= part.OnOrderQuantity)
                    {
                        pModel.partStatusForRedirection = UtilityFunction.GetMessageFromResource("globalLowParts", LocalizeResourceSetConstants.Global);
                    }
                    else
                    {
                        pModel.partStatusForRedirection = UtilityFunction.GetMessageFromResource("globalActive", LocalizeResourceSetConstants.Global);
                    }
                }
                else
                {
                    pModel.partStatusForRedirection = UtilityFunction.GetMessageFromResource("globalInActive", LocalizeResourceSetConstants.Global);
                }
                #endregion

                var partMasterModel = pWrapper.PopulateReviewSiteDetails(partId);
                objPartVM.partMasterModel = partMasterModel;
                var PartHistorydateList = UtilityFunction.PartDatesList();
                if (PartHistorydateList != null)
                {
                    partsHistoryModel.HistoryDateList = PartHistorydateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                var PartReceiptdateList = UtilityFunction.PartDatesList();
                if (PartReceiptdateList != null)
                {
                    partsReceiptModel.ReceiptDateList = PartReceiptdateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                objPartVM.partsHistoryModel = partsHistoryModel;
                objPartVM.partsReceiptModel = partsReceiptModel;
                objPartVM.partSummaryModel = GetPartSummary(partId);
                pModel.PartImageUrl = objPartVM.partSummaryModel.PartImageUrl;
                objPartVM._userdata = this.userData;
                objPartVM.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
                objPartVM.UsePartMaster = this.userData.Site.UsePartMaster;
                objPartVM.createdLastUpdatedPartModel = objCreatedLastUpdatedPartModel;
                ViewBag.IsPartDetailsFromEquipment = true;
                objPartVM.IsPartDetailsFromEquipment = true;
                objPartVM.IsAddPartFromEquipment = true; //V2-1007
                objPartVM.Equipment_ClientLookupId = Equipment_ClientLookupId; //V2-1007
                objPartVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPartWidget, userData);
                PartUDF PartUDF = pWrapper.RetrievePartUDFByPartId(partId);
                objPartVM.ViewPart = new ViewPartModelDynamic();
                objPartVM.ViewPart = pWrapper.MapPartDataForView(objPartVM.ViewPart, pModel);
                objPartVM.ViewPart = pWrapper.MapPartUDFDataForView(objPartVM.ViewPart, PartUDF);
                #region V2-1196
                GetAutoPurchaseConfigurationSetUp(objPartVM, pWrapper, pModel, partId);
                #endregion
            }
            else if (mode == "DetailFromNotification") //V2-1147
            {
                long partId = Convert.ToInt64(TempData["partId"]);
                PartsHistoryModel partsHistoryModel = new PartsHistoryModel();
                PartsReceiptModel partsReceiptModel = new PartsReceiptModel();
                PartSummaryModel partSummaryModel = new PartSummaryModel();
                CreatedLastUpdatedPartModel objCreatedLastUpdatedPartModel = new CreatedLastUpdatedPartModel();
                objCreatedLastUpdatedPartModel = pWrapper.PopulateCreateModifyDate(partId);
                var part = pWrapper.PopulatePartDetails(partId);
                pModel = part;
                var partMasterModel = pWrapper.PopulateReviewSiteDetails(partId);
                objPartVM.partMasterModel = partMasterModel;
                var PartHistorydateList = UtilityFunction.PartDatesList();
                if (PartHistorydateList != null)
                {
                    partsHistoryModel.HistoryDateList = PartHistorydateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                var PartReceiptdateList = UtilityFunction.PartDatesList();
                if (PartReceiptdateList != null)
                {
                    partsReceiptModel.ReceiptDateList = PartReceiptdateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                objPartVM.partsHistoryModel = partsHistoryModel;
                objPartVM.partsReceiptModel = partsReceiptModel;
                objPartVM.partSummaryModel = GetPartSummary(partId);
                pModel.PartImageUrl = objPartVM.partSummaryModel.PartImageUrl;
                objPartVM._userdata = this.userData;
                objPartVM.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
                objPartVM.UsePartMaster = this.userData.Site.UsePartMaster;
                objPartVM.createdLastUpdatedPartModel = objCreatedLastUpdatedPartModel;
                ViewBag.IsPartDetailsFromNotification = true;
                objPartVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPartWidget, userData);
                PartUDF PartUDF = pWrapper.RetrievePartUDFByPartId(partId);
                objPartVM.ViewPart = new ViewPartModelDynamic();
                objPartVM.ViewPart = pWrapper.MapPartDataForView(objPartVM.ViewPart, pModel);
                objPartVM.ViewPart = pWrapper.MapPartUDFDataForView(objPartVM.ViewPart, PartUDF);
                #region V2-1196
                GetAutoPurchaseConfigurationSetUp(objPartVM, pWrapper, pModel, partId);
                #endregion
            }
            else
            {
                objPartVM.PartModel = pModel;
            }
            objPartVM.security = this.userData.Security;
            string localurl = string.Empty;
            localurl = GetUrl();
            pModel.hdLoginId = userData.LoginAuditing.SessionId;
            pModel.localurl = localurl;
            pModel.PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToLower();
            objPartVM.PartModel = pModel;

            #region ChargeType
            ParInvCheckoutModel objInventoryCheckoutModel = new ParInvCheckoutModel();
            objPartVM.userData = this.userData;
            var ChargeTypeList = UtilityFunction.populateChargeTypeForInventoryCheckout();
            if (ChargeTypeList != null)
            {
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                {
                    ChargeTypeList = ChargeTypeList.Where(x => x.text != ChargeType.Location).ToList();
                    objInventoryCheckoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                else
                {
                    objInventoryCheckoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
            }
            objPartVM.inventoryCheckoutModel = objInventoryCheckoutModel;
            #endregion

            //V2-389
            //var AllLookUps = comWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var LookupStokeType = AllLookUps.Where(x => x.ListName == LookupListConstants.STOCK_TYPE).ToList();
                if (LookupStokeType != null)
                {
                    objPartVM.PartModel.LookupStokeTypeList = LookupStokeType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }

            }

            //V2 - 635
            objPartVM.partSummaryModel.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            LocalizeControls(objPartVM, LocalizeResourceSetConstants.PartDetails);
            return View(objPartVM);
        }
        #region V2-1196
        private void GetAutoPurchaseConfigurationSetUp(PartsVM objPartVM, PartsWrapper pWrapper, PartModel pModel, long partId)
        {
            if (userData.Security.Parts.ConfigureAutoPurchasing)
            {

                var vendorParts = pWrapper.PopulateParts(partId);
                var partsConfigureAutoPurchasingModel = new PartsConfigureAutoPurchasingModel
                {
                    VendorList = vendorParts.Select(x =>
                        new SelectListItem
                        {
                            Text = $"{x.Vendor_ClientLookupId} - {x.Vendor_Name}",
                            Value = x.Part_Vendor_XrefId.ToString()
                        }).ToList()
                };
                objPartVM.partsConfigureAutoPurchasingModel = partsConfigureAutoPurchasingModel;
                objPartVM.partsConfigureAutoPurchasingModel.PartId = partId;
                objPartVM.partsConfigureAutoPurchasingModel.PartStoreroomId = pModel.PartStoreroomId;
            }
        }
        #endregion


        private List<PartModel> GetPartSearchResult(List<PartModel> partList, string SearchText, int CustomQueryDisplayId, string PartID, string Description, string Section, decimal OnHandQty, decimal MinimumQty, string Manufacturer, string ManufacturerID, string StockType,
           string Row, string Bin, string Shelf, string PlaceArea)
        {
            if (partList != null)
            {
                #region TextSearch
                SearchText = SearchText.ToUpper();
                int VAL;
                bool res = int.TryParse(SearchText, out VAL);
                if (partList.Count > 0)
                {

                    partList = partList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(SearchText))
                                                    || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(SearchText))
                                                    || (!string.IsNullOrWhiteSpace(x.StockType) && x.StockType.ToUpper().Contains(SearchText))
                                                    || (!string.IsNullOrWhiteSpace(x.Section) && x.Section.ToUpper().Contains(SearchText))
                                                    || (res == true && x.MinimumQuantity.Equals(VAL))
                                                    || (res == true && x.OnHandQuantity.Equals(VAL))
                                                    || (!string.IsNullOrWhiteSpace(x.Manufacturer) && x.Manufacturer.ToUpper().Contains(SearchText))
                                                    || (!string.IsNullOrWhiteSpace(x.ManufacturerID) && x.ManufacturerID.ToUpper().Contains(SearchText))
                                                    || (!string.IsNullOrWhiteSpace(x.Row) && x.Row.ToUpper().Contains(SearchText))
                                                    || (!string.IsNullOrWhiteSpace(x.Bin) && x.Bin.ToUpper().Contains(SearchText))
                                                    || (!string.IsNullOrWhiteSpace(x.Shelf) && x.Shelf.ToUpper().Contains(SearchText))
                                                    || (!string.IsNullOrWhiteSpace(x.PlaceArea) && x.PlaceArea.ToUpper().Contains(SearchText))
                                                    ).ToList();
                }
                #endregion
                #region AdvSearch
                if (!string.IsNullOrEmpty(PartID))
                {
                    PartID = PartID.ToUpper();
                    partList = partList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(PartID))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    partList = partList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (MinimumQty != 0)
                {
                    partList = partList.Where(x => x.MinimumQuantity.Equals(MinimumQty)).ToList();
                }
                if (OnHandQty != 0)
                {
                    partList = partList.Where(x => x.OnHandQuantity.Equals(OnHandQty)).ToList();
                }
                if (!string.IsNullOrEmpty(StockType))
                {
                    StockType = StockType.ToUpper();
                    partList = partList.Where(x => (!string.IsNullOrWhiteSpace(x.StockType) && x.StockType.ToUpper().Equals(StockType))).ToList();
                }
                if (!string.IsNullOrEmpty(Section))
                {
                    Section = Section.ToUpper();
                    partList = partList.Where(x => (!string.IsNullOrWhiteSpace(x.Section) && x.Section.ToUpper().Contains(Section))).ToList();
                }
                if (!string.IsNullOrEmpty(Manufacturer))
                {
                    Manufacturer = Manufacturer.ToUpper();
                    partList = partList.Where(x => (!string.IsNullOrWhiteSpace(x.Manufacturer) && x.Manufacturer.ToUpper().Contains(Manufacturer))).ToList();
                }
                if (!string.IsNullOrEmpty(ManufacturerID))
                {
                    ManufacturerID = ManufacturerID.ToUpper();
                    partList = partList.Where(x => (!string.IsNullOrWhiteSpace(x.ManufacturerID) && x.ManufacturerID.ToUpper().Contains(ManufacturerID))).ToList();
                }
                if (!string.IsNullOrEmpty(Row))
                {
                    Row = Row.ToUpper();
                    partList = partList.Where(x => (!string.IsNullOrWhiteSpace(x.Row) && x.Row.ToUpper().Contains(Row))).ToList();
                }
                if (!string.IsNullOrEmpty(Bin))
                {
                    Bin = Bin.ToUpper();
                    partList = partList.Where(x => (!string.IsNullOrWhiteSpace(x.Bin) && x.Bin.ToUpper().Contains(Bin))).ToList();
                }
                if (!string.IsNullOrEmpty(Shelf))
                {
                    Shelf = Shelf.ToUpper();
                    partList = partList.Where(x => (!string.IsNullOrWhiteSpace(x.Shelf) && x.Shelf.ToUpper().Contains(Shelf))).ToList();
                }
                if (!string.IsNullOrEmpty(PlaceArea))
                {
                    PlaceArea = PlaceArea.ToUpper();
                    partList = partList.Where(x => (!string.IsNullOrWhiteSpace(x.PlaceArea) && x.PlaceArea.ToUpper().Contains(PlaceArea))).ToList();
                }
                #endregion
            }
            return partList;
        }

        [HttpPost]
        public string GetPartsMainGrid(int? draw, int? start, int? length, int CustomQueryDisplayId, string SearchText = "", string PartID = "",
                                       string Description = "", string Section = "", decimal MinimumQty = 0, decimal OnHandQty = 0, string Manufacturer = "", string ManufacturerID = "",
                                       string StockType = "", string Row = "",
                                       string Bin = "",
                                       string Shelf = "", string PlaceArea = "",
                                       string Order = "1"//, string orderDir = "asc"
            )//Part Sorting
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
           ? start / length
           : 0;
            int skip = start * length ?? 0;
            PartsWrapper pWrapper = new PartsWrapper(userData);
            List<PartModel> partList = pWrapper.GetPartChunkList(CustomQueryDisplayId, skip, length ?? 0, Order, orderDir, PartID, Description, Section, MinimumQty, OnHandQty, Manufacturer, ManufacturerID, StockType, Row, Bin, SearchText, Shelf, PlaceArea);
            var totalRecords = 0;
            var recordsFiltered = 0;

            if (partList != null && partList.Count > 0)
            {
                recordsFiltered = partList[0].TotalCount;
                totalRecords = partList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = partList
              .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }


        [HttpGet]

        public string GetPartsPrintData(string colname, string coldir, int CustomQueryDisplayId, string SearchText = "", string ClientLookupId = "",
                                           string Description = "", string Section = "", decimal OnHandQty = 0, decimal MinimumQty = 0, string Manufacturer = "", string ManufacturerID = "",
                                            string StockType = "", string Row = "",
                                            string Bin = "", string Shelf = "", string PlaceArea = "")
        {
            List<PartsPrintModel> pSearchModelList = new List<PartsPrintModel>();

            PartsPrintModel objPartsPrintModel;
            PartsWrapper pWrapper = new PartsWrapper(userData);
            var partList = pWrapper.GetDetailsByPartPrint(CustomQueryDisplayId, 0, 100000, colname, coldir, ClientLookupId, Description, Section, MinimumQty, OnHandQty, Manufacturer, ManufacturerID, StockType, Row, Bin, SearchText, Shelf, PlaceArea);
            //partList = this.GetPartGridSortByColumnWithOrder(colname, coldir, partList);
            //partList = this.GetPartSearchResult(partList, SearchText, CustomQueryDisplayId, ClientLookupId, Description, Section, OnHandQty, MinimumQty, Manufacturer, ManufacturerID, StockType, Row, Bin, Shelf, PlaceArea);
            foreach (var p in partList)
            {
                objPartsPrintModel = new PartsPrintModel();
                objPartsPrintModel.ClientLookupId = p.ClientLookupId;
                objPartsPrintModel.Description = p.Description;
                objPartsPrintModel.Section = p.Section;
                objPartsPrintModel.OnHandQuantity = p.OnHandQuantity ?? 0;
                objPartsPrintModel.MinimumQuantity = p.MinimumQuantity;
                objPartsPrintModel.Manufacturer = p.Manufacturer;
                objPartsPrintModel.ManufacturerID = p.ManufacturerID;
                objPartsPrintModel.StockType = p.StockType;
                objPartsPrintModel.Row = p.Row;
                objPartsPrintModel.Bin = p.Bin;
                objPartsPrintModel.UPCCode = p.UPCCode;
                objPartsPrintModel.Shelf = p.Shelf;
                objPartsPrintModel.PreviousId = p.PreviousId;
                objPartsPrintModel.PlaceArea = p.PlaceArea;
                objPartsPrintModel.Consignment = p.Consignment;
                objPartsPrintModel.Maximum = p.Maximum ?? 0;
                pSearchModelList.Add(objPartsPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = pSearchModelList }, JsonSerializerDateSettings);
        }
        private List<PartModel> GetPartGridSortByColumnWithOrder(string order, string orderDir, List<PartModel> data)
        {
            List<PartModel> lst = new List<PartModel>();
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
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Manufacturer).ToList() : data.OrderBy(p => p.Manufacturer).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ManufacturerID).ToList() : data.OrderBy(p => p.ManufacturerID).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }

        public PartialViewResult PartDetails(long PartId)
        {
            PartsVM objPartsVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            #region ChargeType
            ParInvCheckoutModel objInventoryCheckoutModel = new ParInvCheckoutModel();
            objPartsVM.userData = this.userData;
            var ChargeTypeList = UtilityFunction.populateChargeTypeForInventoryCheckout();
            if (ChargeTypeList != null)
            {
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                {
                    ChargeTypeList = ChargeTypeList.Where(x => x.text != ChargeType.Location).ToList();
                    objInventoryCheckoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                else
                {
                    objInventoryCheckoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
            }
            objPartsVM.inventoryCheckoutModel = objInventoryCheckoutModel;
            #endregion
            PartsHistoryModel partsHistoryModel = new PartsHistoryModel();
            PartsReceiptModel partsReceiptModel = new PartsReceiptModel();
            PartSummaryModel partSummaryModel = new PartSummaryModel();
            CreatedLastUpdatedPartModel objCreatedLastUpdatedPartModel = new CreatedLastUpdatedPartModel();
            CommonWrapper comWrapper = new CommonWrapper(userData);
            Task attTask;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            attTask = Task.Factory.StartNew(() => objPartsVM.attachmentCount = objCommonWrapper.AttachmentCount(PartId, AttachmentTableConstant.Part, userData.Security.Parts.Edit));

            objCreatedLastUpdatedPartModel = pWrapper.PopulateCreateModifyDate(PartId);
            var part = pWrapper.PopulatePartDetails(PartId);
            objPartsVM.PartModel = part;         
            var partMasterModel = pWrapper.PopulateReviewSiteDetails(PartId);
            objPartsVM.partMasterModel = partMasterModel;
            var PartHistorydateList = UtilityFunction.PartDatesList();
            if (PartHistorydateList != null)
            {
                partsHistoryModel.HistoryDateList = PartHistorydateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var PartReceiptdateList = UtilityFunction.PartDatesList();
            if (PartReceiptdateList != null)
            {
                partsReceiptModel.ReceiptDateList = PartReceiptdateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objPartsVM.partsHistoryModel = partsHistoryModel;
            objPartsVM.partsReceiptModel = partsReceiptModel;
            objPartsVM.partSummaryModel = GetPartSummary(PartId);
            objPartsVM.PartModel.PartImageUrl = objPartsVM.partSummaryModel.PartImageUrl;
            objPartsVM.security = this.userData.Security;
            objPartsVM._userdata = this.userData;
            objPartsVM.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
            objPartsVM.UsePartMaster = this.userData.Site.UsePartMaster;
            objPartsVM.createdLastUpdatedPartModel = objCreatedLastUpdatedPartModel;
            objPartsVM.PartModel.PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToLower();
            objPartsVM.partSummaryModel.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            attTask.Wait();
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_PartDetails.cshtml", objPartsVM);
        }

        public RedirectResult PartsDetailFromFleetAssets(long partId)
        {
            TempData["Mode"] = "PartsDetailFromFleetAssets";
            string strPartId = Convert.ToString(partId);
            TempData["partId"] = strPartId;
            return Redirect("/Parts/Index?page=Inventory_Part");
        }


        public RedirectResult PartsDetailFromVendor(long partId)
        {
            TempData["Mode"] = "PartsDetailFromVendor";
            string strPartId = Convert.ToString(partId);
            TempData["partId"] = strPartId;
            return Redirect("/Parts/Index?page=Inventory_Part");
        }

        public RedirectResult PartFromPartDetails(string partStatus)
        {
            TempData["Mode"] = "PartFromPartDetails";
            TempData["PartStatus"] = partStatus;
            return Redirect("/Parts/Index?page=Inventory_Part");
        }
        #region V2-1147
        public RedirectResult DetailFromNotification(long partId, string alertName)
        {
            TempData["Mode"] = "DetailFromNotification";
            string strPartId = Convert.ToString(partId);
            TempData["partId"] = strPartId;
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                return Redirect("/MultiStoreroomPart/Index?page=Inventory_Part");
            }
            else
            {
                return Redirect("/Parts/Index?page=Inventory_Part");
            }
            
        }
        #endregion
        #endregion

        #region Part-Add/Edit
        public ActionResult Add()
        {
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                //return RedirectToAction("Index", "MultiStoreroomPart");
                return Redirect("/MultiStoreroomPart/Add");
            }
            TempData["Mode"] = "add";
            return Redirect("/Parts/Index?page=Inventory_Parts");
        }
        public PartialViewResult AddParts()
        {
            PartsVM objPartsVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartModel pModel = new PartModel();
            objPartsVM.security = this.userData.Security;

            var part = pWrapper.PopulateDropdownControls(pModel);
            //V2-379
            //var AcclookUpList = GetLookupList_Account();
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                part.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            }
            objPartsVM.PartModel = part;
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_PartsAdd.cshtml", objPartsVM);
        }
        public PartialViewResult EditParts(long PartId)
        {
            PartsVM objPartsVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            objPartsVM.security = this.userData.Security;
            var part = pWrapper.PopulatePartDetails(PartId);
            part = pWrapper.PopulateDropdownControls(part);
            //V2-379
            //var AcclookUpList = GetLookupList_Account();
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                part.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            }
            objPartsVM.PartModel = part;
            objPartsVM.partSummaryModel = GetPartSummary(PartId);
            objPartsVM.PartModel.PartImageUrl = objPartsVM.partSummaryModel.PartImageUrl;
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_PartsAdd.cshtml", objPartsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddParts(PartsVM PartVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                PartsWrapper pWrapper = new PartsWrapper(userData);
                if (PartVM.PartModel.PartId == 0)
                {
                    Mode = "add";
                }
                var objPart = pWrapper.UpdateParts(PartVM.PartModel);

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
        public JsonResult ChangePartID(PartsVM objVM, string strPartId)
        {
            string result = string.Empty;
            WorkOrder obj = new WorkOrder();
            if (ModelState.IsValid && !string.IsNullOrEmpty(strPartId))
            {
                PartsWrapper pWrapper = new PartsWrapper(userData);
                List<String> errorList = new List<string>();
                errorList = pWrapper.ChangePartID(objVM, Convert.ToInt64(strPartId));
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
        [HttpPost]
        public JsonResult ValidateForActiveInactive(bool InActiveFlag, long PartId, string ClientLookupId)
        {
            string validationMessage = string.Empty;
            PartsWrapper pWrapper = new PartsWrapper(userData);
            string flag = string.Empty;
            if (InActiveFlag)
            {
                flag = ActivationStatusConstant.Activate;
            }
            else
            {
                flag = ActivationStatusConstant.InActivate;
            }
            var part = pWrapper.ValidatePartStatusChange(PartId, flag, ClientLookupId);
            if (part.ErrorMessages != null && part.ErrorMessages.Count > 0)
            {
                return Json(part.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { validationStatus = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult MakeActiveInactive(bool InActiveFlag, long PartId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            var ErrorMessages = pWrapper.MakeActiveInactive(InActiveFlag, PartId);
            if (ErrorMessages != null && ErrorMessages.Count > 0)
            {
                return Json(ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var createEventStatus = pWrapper.CreatePartEvent(PartId, InActiveFlag);
                if (createEventStatus != null && createEventStatus.Count > 0)
                {
                    return Json(createEventStatus, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
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
        public ActionResult DeletePAttachment(long _fileAttachmentId)
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
        public PartialViewResult AddPAttachment(long partId, string ClientLookupId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartsVM objPartsVM = new PartsVM();
            AttachmentModel Attachment = new AttachmentModel();

            PartModel objPart = new PartModel();
            objPart.PartId = partId;
            objPart.ClientLookupId = ClientLookupId;
            Attachment.PartId = partId;
            objPartsVM.attachmentModel = Attachment;
            objPartsVM.PartModel = objPart;
            objPartsVM.partSummaryModel = GetPartSummary(partId);
            objPartsVM.PartModel.PartImageUrl = objPartsVM.partSummaryModel.PartImageUrl;
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_PAttachmentAdd.cshtml", objPartsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPAttachment()
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
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.PartId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = "Part";

                //V2-918
                bool IsduplicateAttachmentFileExist = false;
                IsduplicateAttachmentFileExist = objCommonWrapper.IsduplicateFileExist(attachmentModel.ObjectId, attachmentModel.TableName, userData.Security.Equipment.Edit, attachmentModel.FileName, attachmentModel.FileType);
                if (IsduplicateAttachmentFileExist)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), IsduplicateAttachmentFileExist = IsduplicateAttachmentFileExist, partid = attachmentModel.ObjectId }, JsonRequestBehavior.AllowGet);
                }
                //
               

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
                        //return Json(new { Result = JsonReturnEnum.success.ToString(), partid = Convert.ToInt64(Request.Form["attachmentModel.PartId"]) }, JsonRequestBehavior.AllowGet);
                        return Json(new { Result = JsonReturnEnum.success.ToString(), IsduplicateAttachmentFileExist = IsduplicateAttachmentFileExist, partid = Convert.ToInt64(Request.Form["attachmentModel.PartId"]) }, JsonRequestBehavior.AllowGet);

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
            PartsWrapper _PartsObj = new PartsWrapper(userData);
            var pvx = _PartsObj.PopulateParts(_partId);
            List<PartsVendorGridModel> PartsVendorGridModelList = new List<PartsVendorGridModel>();
            PartsVendorGridModel objPartsVendorGridModel;
            foreach (var v in pvx)
            {
                objPartsVendorGridModel = new PartsVendorGridModel();
                objPartsVendorGridModel.Vendor_ClientLookupId = v.Vendor_ClientLookupId;
                objPartsVendorGridModel.Vendor_Name = v.Vendor_Name;
                objPartsVendorGridModel.ClientId = v.ClientId;
                objPartsVendorGridModel.Part_Vendor_XrefId = v.Part_Vendor_XrefId;
                objPartsVendorGridModel.PartId = v.PartId;
                objPartsVendorGridModel.VendorId = v.VendorId;
                objPartsVendorGridModel.PreferredVendor = v.PreferredVendor;
                objPartsVendorGridModel.CatalogNumber = v.CatalogNumber;
                objPartsVendorGridModel.IssueOrder = v.IssueOrder;
                objPartsVendorGridModel.Manufacturer = v.Manufacturer;
                objPartsVendorGridModel.ManufacturerId = v.ManufacturerId;
                objPartsVendorGridModel.OrderQuantity = v.OrderQuantity;
                objPartsVendorGridModel.OrderUnit = v.OrderUnit;
                objPartsVendorGridModel.Price = v.Price;
                objPartsVendorGridModel.UpdateIndex = v.UpdateIndex;
                objPartsVendorGridModel.UOMConvRequired = v.UOMConvRequired;
                PartsVendorGridModelList.Add(objPartsVendorGridModel);
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
        private List<PartsVendorGridModel> GetAllVendorsSortByColumnWithOrder(string order, string orderDir, List<PartsVendorGridModel> data)
        {
            List<PartsVendorGridModel> lst = new List<PartsVendorGridModel>();
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
        public ActionResult PartsVendorDelete(long _PartVendorXrefId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
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
        public ActionResult PartsVedndorAdd(long _partId, string ClientLookupId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PartsVendorModel partVendorXref = new PartsVendorModel();
            PartsVM partsVM = new PartsVM();
            PartModel partModel = new PartModel();
            List<DataContracts.LookupList> objLookupUnitOfMeasure = new List<DataContracts.LookupList>();
            partVendorXref.PartId = _partId;
            partModel.PartId = _partId;
            partModel.ClientLookupId = ClientLookupId;

            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                objLookupUnitOfMeasure = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (objLookupUnitOfMeasure != null)
                {
                    partVendorXref.OrderUnitList = objLookupUnitOfMeasure.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            partsVM.partsVendorModel = partVendorXref;
            partsVM.PartModel = partModel;
            partsVM.partSummaryModel = GetPartSummary(_partId);
            partsVM.PartModel.PartImageUrl = partsVM.partSummaryModel.PartImageUrl;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_PAddVendor.cshtml", partsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartsVedndorAdd(PartsVM partsVM)
        {
            PartsWrapper _PartsObj = new PartsWrapper(userData);
            string Mode = string.Empty;
            Part_Vendor_Xref pvx = new Part_Vendor_Xref();
            if (ModelState.IsValid)
            {
                if (partsVM.partsVendorModel.PartVendorXrefId != 0)
                {
                    Mode = "update";
                    pvx = _PartsObj.UpdatePartVendorXref(partsVM.partsVendorModel, partsVM.partsVendorModel.PartId);
                }
                else
                {
                    Mode = "add";
                    pvx = _PartsObj.AddPartVendorXref(partsVM.partsVendorModel, partsVM.partsVendorModel.PartId);
                }
                if (pvx.ErrorMessages != null && pvx.ErrorMessages.Count > 0)
                {
                    return Json(pvx.ErrorMessages, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), partid = partsVM.partsVendorModel.PartId, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        public ActionResult PartsVedndorEdit(long partId, long _part_Vendor_XrefId, int updatedIndex, string ClientLookupId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PartsVM partsVM = new PartsVM();
            PartModel partModel = new PartModel();
            List<DataContracts.LookupList> objLookupUnitOfMeasure = new List<DataContracts.LookupList>();
            partModel.ClientLookupId = ClientLookupId;
            partModel.PartId = partId;
            PartsVendorModel partVendorXref = new PartsVendorModel();
            partVendorXref = pWrapper.EditVendor(partId, _part_Vendor_XrefId, partModel.ClientLookupId, updatedIndex);

            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                objLookupUnitOfMeasure = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (objLookupUnitOfMeasure != null)
                {
                    partVendorXref.OrderUnitList = objLookupUnitOfMeasure.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            partsVM.partsVendorModel = partVendorXref;
            partsVM.PartModel = partModel;
            partsVM.partSummaryModel = GetPartSummary(partId);
            partsVM.PartModel.PartImageUrl = partsVM.partSummaryModel.PartImageUrl;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
           return PartialView("~/Views/Parts/_PAddVendor.cshtml", partsVM);
        }
        #endregion

        #region equipment
        [HttpPost]
        public string PopulateEquipment(int? draw, int? start, int? length, long _partId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PartsWrapper _PartsObj = new PartsWrapper(userData);
            var PartsEquipmentGridModelList = _PartsObj.PopulatePartEquipment(_partId);
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
        private List<PartsEquipmentGridModel> GetAllEquipmentsSortByColumnWithOrder(string order, string orderDir, List<PartsEquipmentGridModel> data)
        {
            List<PartsEquipmentGridModel> eqpList = new List<PartsEquipmentGridModel>();
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
        public ActionResult PartsEquipmentAdd(long partId, string clientLookupId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            EquipmentPartXrefModel partEquipmentXref = new EquipmentPartXrefModel();
            PartsVM partsVM = new PartsVM();
            partEquipmentXref.PartId = partId;
            partsVM.equipmentPartXrefModel = partEquipmentXref;
            partsVM.PartModel = new PartModel();
            partsVM.PartModel.ClientLookupId = clientLookupId;
            partsVM.PartModel.PartId = partId;
            partsVM.partSummaryModel = GetPartSummary(partId);
            partsVM.PartModel.PartImageUrl = partsVM.partSummaryModel.PartImageUrl;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_PAddEquipment.cshtml", partsVM);
        }
        [HttpGet]
        public ActionResult PartsEquipmentEdit(long partId, long _equipment_Parts_XrefId, int updatedIndex, string ClientLookupId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartsVM partsVM = new PartsVM();
            PartModel partModel = new PartModel();
            partModel.ClientLookupId = ClientLookupId;
            var _equipmentPartXrefModel = pWrapper.EditPartEquipment(partId, _equipment_Parts_XrefId, ClientLookupId, updatedIndex);
            var EquipmentLookUplist = GetLookUpList_Equipment();
            if (EquipmentLookUplist != null)
            {
                _equipmentPartXrefModel.EquipmentList = EquipmentLookUplist.Select(x => new SelectListItem { Text = x.Equipment + " - " + x.Name, Value = x.Equipment.ToString() });
            }
            partsVM.equipmentPartXrefModel = _equipmentPartXrefModel;
            partModel.PartId = partId;
            partsVM.PartModel = partModel;
            partsVM.partSummaryModel = GetPartSummary(partId);
            partsVM.PartModel.PartImageUrl = partsVM.partSummaryModel.PartImageUrl;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_PAddEquipment.cshtml", partsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartsEquipmentAdd(PartsVM partsVM)
        {
            PartsWrapper _PartsObj = new PartsWrapper(userData);
            string Mode = string.Empty;
            Equipment_Parts_Xref eqx = new Equipment_Parts_Xref();
            if (ModelState.IsValid)
            {
                if (partsVM.equipmentPartXrefModel.Equipment_Parts_XrefId != 0)
                {
                    Mode = "update";
                    eqx = _PartsObj.UpdateEquipmentPartsXref(partsVM.equipmentPartXrefModel, partsVM.equipmentPartXrefModel.PartId);
                }
                else
                {
                    Mode = "add";
                    eqx = _PartsObj.AddEquipmentPartsXref(partsVM.equipmentPartXrefModel, partsVM.equipmentPartXrefModel.PartId);
                }
                if (eqx.ErrorMessages != null && eqx.ErrorMessages.Count > 0)
                {
                    return Json(eqx.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), partid = partsVM.equipmentPartXrefModel.PartId, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteEquipmentPartXref(long _equipment_Parts_XrefId)
        {
            PartsWrapper _PartsObj = new PartsWrapper(userData);
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

        #region PartHistory       
        [HttpPost]
        public string PopulatePartHistoryReview(int? draw, int? start, int? length, decimal? TransactionQuantity, decimal? Cost, long partId = 0, int daterange = 0, string TransactionType = "", string Requestor_Name = "", string PerformBy_Name = "",
                                         DateTime? TransactionDate = null, string ChargeType_Primary = "", string ChargeTo_ClientLookupId = "",
                                             string Account_ClientLookupId = "", string PurchaseOrder_ClientLookupId = "", string Vendor_ClientLookupId = "", string Vendor_Name = ""
                                           )
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PartsWrapper pWrapper = new PartsWrapper(userData);
            List<PartsHistoryModel> _partHistoryReview = pWrapper.GetDetailsPartHistory(partId, daterange);
            var TotalPart = _partHistoryReview;
            _partHistoryReview = this.GetPartHistoryGridSortByColumnWithOrder(order, orderDir, _partHistoryReview);
            if (_partHistoryReview != null)
            {
                if (!string.IsNullOrEmpty(TransactionType) && TransactionType != "0")
                {
                    TransactionType = TransactionType.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.TransactionType) && x.TransactionType.ToUpper().Contains(TransactionType))).ToList();
                }
                if (!string.IsNullOrEmpty(Requestor_Name))
                {
                    Requestor_Name = Requestor_Name.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Requestor_Name) && x.Requestor_Name.ToUpper().Contains(Requestor_Name))).ToList();
                }
                if (!string.IsNullOrEmpty(PerformBy_Name))
                {
                    PerformBy_Name = PerformBy_Name.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.PerformBy_Name) && x.PerformBy_Name.ToUpper().Contains(PerformBy_Name))).ToList();
                }
                if (TransactionDate != null)
                {
                    _partHistoryReview = _partHistoryReview.Where(x => (x.TransactionDate != null && x.TransactionDate.Value.Date.Equals(TransactionDate.Value.Date))).ToList();
                }
                if (TransactionQuantity.HasValue)
                {
                    _partHistoryReview = _partHistoryReview.Where(x => x.TransactionQuantity.Equals(TransactionQuantity)).ToList();
                }
                if (Cost.HasValue)
                {
                    _partHistoryReview = _partHistoryReview.Where(x => x.Cost.Equals(Cost)).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeType_Primary) && ChargeType_Primary != "0")
                {
                    ChargeType_Primary = ChargeType_Primary.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeType_Primary) && x.ChargeType_Primary.ToUpper().Contains(ChargeType_Primary))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeTo_ClientLookupId))
                {
                    ChargeTo_ClientLookupId = ChargeTo_ClientLookupId.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_ClientLookupId) && x.ChargeTo_ClientLookupId.ToUpper().Contains(ChargeTo_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Account_ClientLookupId))
                {
                    Account_ClientLookupId = Account_ClientLookupId.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Account_ClientLookupId) && x.Account_ClientLookupId.ToUpper().Contains(Account_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(PurchaseOrder_ClientLookupId))
                {
                    PurchaseOrder_ClientLookupId = PurchaseOrder_ClientLookupId.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.PurchaseOrder_ClientLookupId) && x.PurchaseOrder_ClientLookupId.ToUpper().Contains(PurchaseOrder_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Vendor_ClientLookupId) && Vendor_ClientLookupId != "0")
                {
                    Vendor_ClientLookupId = Vendor_ClientLookupId.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Vendor_ClientLookupId) && x.Vendor_ClientLookupId.ToUpper().Contains(Vendor_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Vendor_Name))
                {
                    Vendor_Name = Vendor_Name.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Vendor_Name) && x.Vendor_Name.ToUpper().Contains(Vendor_Name))).ToList();
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = _partHistoryReview.Count();
            totalRecords = _partHistoryReview.Count();
            int initialPage = start.Value;
            var filteredResult = _partHistoryReview
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, dataAll = TotalPart }, JsonSerializerDateSettings);
        }

        [HttpGet]
        public string GetPartsHistoryPrintData(decimal? TransactionQuantity, decimal? Cost, long partId = 0, int daterange = 0, string TransactionType = "", string Requestor_Name = "", string PerformBy_Name = "", DateTime? TransactionDate = null, string ChargeType_Primary = "", string ChargeTo_ClientLookupId = "", string Account_ClientLookupId = "", string PurchaseOrder_ClientLookupId = "", string Vendor_ClientLookupId = "", string Vendor_Name = ""
                                           )
        {
            List<PartsHistoryPrintModel> pSearchModelList = new List<PartsHistoryPrintModel>();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartsHistoryPrintModel objPartsHistoryPrintModel;
            List<PartsHistoryModel> _partHistoryReview = pWrapper.GetDetailsPartHistory(partId, daterange);
            if (_partHistoryReview != null)
            {
                if (!string.IsNullOrEmpty(TransactionType) && TransactionType != "0")
                {
                    TransactionType = TransactionType.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.TransactionType) && x.TransactionType.ToUpper().Contains(TransactionType))).ToList();
                }

                if (!string.IsNullOrEmpty(Requestor_Name))
                {
                    Requestor_Name = Requestor_Name.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Requestor_Name) && x.Requestor_Name.ToUpper().Contains(Requestor_Name))).ToList();
                }
                if (!string.IsNullOrEmpty(PerformBy_Name))
                {
                    PerformBy_Name = PerformBy_Name.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.PerformBy_Name) && x.PerformBy_Name.ToUpper().Contains(PerformBy_Name))).ToList();
                }
                if (TransactionDate != null)
                {
                    _partHistoryReview = _partHistoryReview.Where(x => (x.TransactionDate != null && x.TransactionDate.Value.Date.Equals(TransactionDate.Value.Date))).ToList();
                }
                if (TransactionQuantity.HasValue)
                {
                    _partHistoryReview = _partHistoryReview.Where(x => x.TransactionQuantity.Equals(TransactionQuantity)).ToList();
                }
                if (Cost.HasValue)
                {
                    _partHistoryReview = _partHistoryReview.Where(x => x.Cost.Equals(Cost)).ToList();
                }

                if (!string.IsNullOrEmpty(ChargeType_Primary) && ChargeType_Primary != "0")
                {
                    ChargeType_Primary = ChargeType_Primary.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeType_Primary) && x.ChargeType_Primary.ToUpper().Contains(ChargeType_Primary))).ToList();
                }
                if (!string.IsNullOrEmpty(ChargeTo_ClientLookupId))
                {
                    ChargeTo_ClientLookupId = ChargeTo_ClientLookupId.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_ClientLookupId) && x.ChargeTo_ClientLookupId.ToUpper().Contains(ChargeTo_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Account_ClientLookupId))
                {
                    Account_ClientLookupId = Account_ClientLookupId.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Account_ClientLookupId) && x.Account_ClientLookupId.ToUpper().Contains(Account_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(PurchaseOrder_ClientLookupId))
                {
                    PurchaseOrder_ClientLookupId = PurchaseOrder_ClientLookupId.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.PurchaseOrder_ClientLookupId) && x.PurchaseOrder_ClientLookupId.ToUpper().Contains(PurchaseOrder_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Vendor_ClientLookupId) && Vendor_ClientLookupId != "0")
                {
                    Vendor_ClientLookupId = Vendor_ClientLookupId.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Vendor_ClientLookupId) && x.Vendor_ClientLookupId.ToUpper().Contains(Vendor_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Vendor_Name))
                {
                    Vendor_Name = Vendor_Name.ToUpper();
                    _partHistoryReview = _partHistoryReview.Where(x => (!string.IsNullOrWhiteSpace(x.Vendor_Name) && x.Vendor_Name.ToUpper().Contains(Vendor_Name))).ToList();
                }
                foreach (var p in _partHistoryReview)
                {
                    objPartsHistoryPrintModel = new PartsHistoryPrintModel();
                    objPartsHistoryPrintModel.TransactionType = p.TransactionType;
                    objPartsHistoryPrintModel.Requestor_Name = p.Requestor_Name;
                    objPartsHistoryPrintModel.PerformBy_Name = p.PerformBy_Name;
                    objPartsHistoryPrintModel.TransactionDate = p.TransactionDate;
                    objPartsHistoryPrintModel.TransactionQuantity = p.TransactionQuantity;
                    objPartsHistoryPrintModel.Cost = p.Cost;
                    objPartsHistoryPrintModel.ChargeType_Primary = p.ChargeType_Primary;
                    objPartsHistoryPrintModel.ChargeTo_ClientLookupId = p.ChargeTo_ClientLookupId;
                    objPartsHistoryPrintModel.Account_ClientLookupId = p.Account_ClientLookupId;
                    objPartsHistoryPrintModel.PurchaseOrder_ClientLookupId = p.PurchaseOrder_ClientLookupId;
                    objPartsHistoryPrintModel.Vendor_ClientLookupId = p.Vendor_ClientLookupId;
                    objPartsHistoryPrintModel.Vendor_Name = p.Vendor_Name;
                    pSearchModelList.Add(objPartsHistoryPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = pSearchModelList }, JsonSerializerDateSettings);
        }

        private List<PartsHistoryModel> GetPartHistoryGridSortByColumnWithOrder(string order, string orderDir, List<PartsHistoryModel> data)
        {
            List<PartsHistoryModel> lst = new List<PartsHistoryModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionType).ToList() : data.OrderBy(p => p.TransactionType).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Requestor_Name).ToList() : data.OrderBy(p => p.Requestor_Name).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PerformBy_Name).ToList() : data.OrderBy(p => p.PerformBy_Name).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionDate).ToList() : data.OrderBy(p => p.TransactionDate).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionQuantity).ToList() : data.OrderBy(p => p.TransactionQuantity).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Cost).ToList() : data.OrderBy(p => p.Cost).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeType_Primary).ToList() : data.OrderBy(p => p.ChargeType_Primary).ToList();
                        break;
                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_ClientLookupId).ToList() : data.OrderBy(p => p.ChargeTo_ClientLookupId).ToList();
                        break;
                    case "8":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Account_ClientLookupId).ToList() : data.OrderBy(p => p.Account_ClientLookupId).ToList();
                        break;
                    case "9":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PurchaseOrder_ClientLookupId).ToList() : data.OrderBy(p => p.PurchaseOrder_ClientLookupId).ToList();
                        break;
                    case "10":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Vendor_ClientLookupId).ToList() : data.OrderBy(p => p.Vendor_ClientLookupId).ToList();
                        break;
                    case "11":
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

        #region Part Receipt
        [HttpPost]
        public string populatePartReceipt(int? draw, int? start, int? length, decimal? OrderQuantity, decimal? UnitCost, long partId, DateTime? ReceivedDate, int dateRange = 0, string POClientLookupId = "",
                                             string VendorClientLookupId = "", string VendorName = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PartsWrapper pWrapper = new PartsWrapper(userData);
            List<PartsReceiptModel> _partReceiptReview = pWrapper.GetDetailsPartReceipt(partId, dateRange);
            var TotalReceipt = _partReceiptReview;
            _partReceiptReview = this.GetPartReceiptGridSortByColumnWithOrder(order, orderDir, _partReceiptReview);
            if (_partReceiptReview != null)
            {
                if (OrderQuantity.HasValue)
                {
                    _partReceiptReview = _partReceiptReview.Where(x => x.OrderQuantity.Equals(OrderQuantity)).ToList();
                }
                if (UnitCost.HasValue)
                {
                    _partReceiptReview = _partReceiptReview.Where(x => x.UnitCost.Equals(UnitCost)).ToList();
                }
                if (!string.IsNullOrEmpty(POClientLookupId))
                {
                    _partReceiptReview = _partReceiptReview.Where(x => (!string.IsNullOrWhiteSpace(x.POClientLookupId)) && x.POClientLookupId.ToUpper().Contains(POClientLookupId)).ToList();
                }
                if (ReceivedDate != null)
                {
                    _partReceiptReview = _partReceiptReview.Where(x => (x.ReceivedDate != null && x.ReceivedDate.Value.Date.Equals(ReceivedDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorClientLookupId) && VendorClientLookupId != "0")
                {
                    VendorClientLookupId = VendorClientLookupId.ToUpper();
                    _partReceiptReview = _partReceiptReview.Where(x => (!string.IsNullOrWhiteSpace(x.VendorClientLookupId) && x.VendorClientLookupId.ToUpper().Contains(VendorClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorName))
                {
                    VendorName = VendorName.ToUpper();
                    _partReceiptReview = _partReceiptReview.Where(x => (!string.IsNullOrWhiteSpace(x.VendorName) && x.VendorName.ToUpper().Contains(VendorName))).ToList();
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = _partReceiptReview.Count();
            totalRecords = _partReceiptReview.Count();
            int initialPage = start.Value;
            var filteredResult = _partReceiptReview
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, dataAll = TotalReceipt }, JsonSerializerDateSettings);
        }

        [HttpGet]
        public string GetPartsReceiptPrintData(decimal? OrderQuantity, decimal? UnitCost, long partId, DateTime? ReceivedDate, int dateRange = 0, string POClientLookupId = "",
                                             string VendorClientLookupId = "", string VendorName = ""
                                         )
        {
            List<PartsReceiptPrintModel> pSearchModelList = new List<PartsReceiptPrintModel>();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartsReceiptPrintModel objPartsReceiptPrintModel;
            List<PartsReceiptModel> _partReceiptReview = pWrapper.GetDetailsPartReceipt(partId, dateRange);
            if (_partReceiptReview != null)
            {
                if (OrderQuantity.HasValue)
                {
                    _partReceiptReview = _partReceiptReview.Where(x => x.OrderQuantity.Equals(OrderQuantity)).ToList();
                }
                if (UnitCost.HasValue)
                {
                    _partReceiptReview = _partReceiptReview.Where(x => x.UnitCost.Equals(UnitCost)).ToList();
                }
                if (!string.IsNullOrEmpty(POClientLookupId))
                {
                    _partReceiptReview = _partReceiptReview.Where(x => (!string.IsNullOrWhiteSpace(x.POClientLookupId)) && x.POClientLookupId.ToUpper().Contains(POClientLookupId)).ToList();
                }
                if (ReceivedDate != null)
                {
                    _partReceiptReview = _partReceiptReview.Where(x => (x.ReceivedDate != null && x.ReceivedDate.Value.Date.Equals(ReceivedDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorClientLookupId) && VendorClientLookupId != "0")
                {
                    VendorClientLookupId = VendorClientLookupId.ToUpper();
                    _partReceiptReview = _partReceiptReview.Where(x => (!string.IsNullOrWhiteSpace(x.VendorClientLookupId) && x.VendorClientLookupId.ToUpper().Contains(VendorClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(VendorName))
                {
                    VendorName = VendorName.ToUpper();
                    _partReceiptReview = _partReceiptReview.Where(x => (!string.IsNullOrWhiteSpace(x.VendorName) && x.VendorName.ToUpper().Contains(VendorName))).ToList();
                }
                foreach (var p in _partReceiptReview)
                {
                    objPartsReceiptPrintModel = new PartsReceiptPrintModel();
                    objPartsReceiptPrintModel.OrderQuantity = p.OrderQuantity;
                    objPartsReceiptPrintModel.UnitCost = p.UnitCost;
                    objPartsReceiptPrintModel.ReceivedDate = p.ReceivedDate;
                    objPartsReceiptPrintModel.POClientLookupId = p.POClientLookupId;
                    objPartsReceiptPrintModel.VendorClientLookupId = p.VendorClientLookupId;
                    objPartsReceiptPrintModel.VendorName = p.VendorName;
                    pSearchModelList.Add(objPartsReceiptPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = pSearchModelList }, JsonSerializerDateSettings);
        }
        private List<PartsReceiptModel> GetPartReceiptGridSortByColumnWithOrder(string order, string orderDir, List<PartsReceiptModel> data)
        {
            List<PartsReceiptModel> lst = new List<PartsReceiptModel>();
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

        #region QRCode         
        [HttpPost]
        public PartialViewResult PartQRcode(string[] PartClientLookups) // not need this after rotativa
        {
            PartsVM partsVM = new PartsVM();
            QRCodeModel qRCodeModel = new QRCodeModel();
            if (PartClientLookups != null)
            {
                qRCodeModel.PartIdsList = new List<string>(PartClientLookups);
            }
            else
            {
                qRCodeModel.PartIdsList = new List<string>();
            }
            partsVM.qRCodeModel = qRCodeModel;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("_PartQRCode", partsVM);
        }
        [HttpPost]
        public PartialViewResult PartDetailsQRcode(string[] PartClientLookups)
        {
            PartsVM partsVM = new PartsVM();
            QRCodeModel qRCodeModel = new QRCodeModel();
            if (PartClientLookups != null)
            {
                qRCodeModel.PartIdsList = new List<string>(PartClientLookups);
            }
            else
            {
                qRCodeModel.PartIdsList = new List<string>();
            }
            partsVM.qRCodeModel = qRCodeModel;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("_PartDetailsQRCode", partsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetPartIdlist(PartsVM partsVM)
        {
            TempData["QRCodePartIdList"] = partsVM.qRCodeModel.PartIdsList;
            return Json(new { JsonReturnEnum.success });
        }
        [EncryptedActionParameter]
        public ActionResult QRCodeGenerationUsingRotativa(bool SmallLabel)
        {
            var partsVM = new PartsVM();
            var qRCodeModel = new QRCodeModel();

            ViewBag.SmallQR = SmallLabel;
            if (TempData["QRCodePartIdList"] != null)
            {
                qRCodeModel.PartIdsList = (List<string>)TempData["QRCodePartIdList"];
            }
            else
            {
                qRCodeModel.PartIdsList = new List<string>();
            }
            partsVM.qRCodeModel = qRCodeModel;

            return new PartialViewAsPdf("_PartsQRCodeTemplate", partsVM)
            {
                PageMargins = { Left = 1, Right = 1, Top = 1, Bottom = 1 },
                PageHeight = SmallLabel ? 25 : 28,
                PageWidth = SmallLabel ? 54 : 89,
            };
        }
        public ActionResult QrGenerate(string value)
        {
            string QRCodeImagePath = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                string partid = value.Split(new string[] { "][" }, StringSplitOptions.None)[0];
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(partid, QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    QRCodeImagePath = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }
            return Json(new { QRCodeImagePath = QRCodeImagePath, value = value }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PartSummary
        public PartSummaryModel GetPartSummary(long partId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartSummaryModel pSummary = new PartSummaryModel();
            PartModel objPart = pWrapper.PopulatePartDetails(partId);
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            if (objPart != null)
            {
                pSummary.ClientLookupId = objPart.ClientLookupId;
                pSummary.OnHandQuantity = objPart.OnHandQuantity;
                pSummary.OnOrderQuantity = objPart.OnOrderQuantity;
                pSummary.OnRequestQuantity = objPart.OnRequestQuantity;
                pSummary.Description = objPart.Description;
                pSummary.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
                if (pSummary.ClientOnPremise)
                {
                    pSummary.PartImageUrl = objCommonWrapper.GetOnPremiseImageUrl(partId, AttachmentTableConstant.Part);
                }
                else
                {
                    pSummary.PartImageUrl = objCommonWrapper.GetAzureImageUrl(partId, AttachmentTableConstant.Part);
                }

            }


            pSummary.InactiveFlag = objPart.InactiveFlag;
            //}
            //pSummary.PartImageUrl = objCommonWrapper.GetAzureImageUrl(partId, AttachmentTableConstant.Part);
            return pSummary;
        }
        #endregion
        #region Review Site

        [HttpPost]
        public string PopulateReviewSite(int? draw, int? start, int? length, long PartMasterId, string SiteName = "", string ClientLookupId = "", string Description = "", decimal QtyOnHand = 0, decimal QtyOnOrder = 0, DateTime? LastPurchaseDate = null, decimal LastPurchaseCost = 0, string LastPurchaseVendor = "", string InactiveFlag = null)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PartsWrapper pWrapper = new PartsWrapper(userData);
            var ReviewSite = pWrapper.PopulateReviewSite(PartMasterId);

            if (ReviewSite != null)
            {
                ReviewSite = GetSearchResult(ReviewSite, SiteName, ClientLookupId, Description, QtyOnHand, QtyOnOrder, LastPurchaseDate, LastPurchaseCost, LastPurchaseVendor, InactiveFlag);
                ReviewSite = GetAllReviewSiteSortByColumnWithOrder(order, orderDir, ReviewSite);
            }
            ReviewSite = this.GetAllReviewSiteSortByColumnWithOrder(order, orderDir, ReviewSite);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = ReviewSite.Count();
            totalRecords = ReviewSite.Count();
            int initialPage = start.Value;
            var filteredResult = ReviewSite
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<ReviewSiteModel> GetSearchResult(List<ReviewSiteModel> ReviewSiteModelModelList, string SiteName, string ClientLookupId, string Description, decimal QtyOnHand, decimal QtyOnOrder, DateTime? LastPurchaseDate, decimal LastPurchaseCost, string LastPurchaseVendor, string InactiveFlag)
        {
            //-------------text search--------------

            if (ReviewSiteModelModelList != null)
            {
                if (!string.IsNullOrEmpty(SiteName))
                {
                    SiteName = SiteName.ToUpper();
                    ReviewSiteModelModelList = ReviewSiteModelModelList.Where(x => (!string.IsNullOrWhiteSpace(x.SiteName) && x.SiteName.ToUpper().Contains(SiteName))).ToList();
                }
                if (!string.IsNullOrEmpty(ClientLookupId))
                {
                    ClientLookupId = ClientLookupId.ToUpper();
                    ReviewSiteModelModelList = ReviewSiteModelModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    ReviewSiteModelModelList = ReviewSiteModelModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (QtyOnHand != null)
                {
                    ReviewSiteModelModelList = ReviewSiteModelModelList.Where(x => Convert.ToString(x.QtyOnHand).Contains(Convert.ToString(QtyOnHand))).ToList();
                }
                if (QtyOnOrder != null)
                {
                    ReviewSiteModelModelList = ReviewSiteModelModelList.Where(x => Convert.ToString(x.QtyOnOrder).Contains(Convert.ToString(QtyOnOrder))).ToList();
                }
                if (LastPurchaseDate != null)
                {
                    ReviewSiteModelModelList = ReviewSiteModelModelList.Where(x => Convert.ToString(x.LastPurchaseDate).Contains(Convert.ToString(LastPurchaseDate))).ToList();
                }
                if (LastPurchaseCost != null)
                {
                    ReviewSiteModelModelList = ReviewSiteModelModelList.Where(x => Convert.ToString(x.LastPurchaseCost).Contains(Convert.ToString(LastPurchaseCost))).ToList();
                }
                if (!string.IsNullOrEmpty(LastPurchaseVendor))
                {
                    LastPurchaseVendor = LastPurchaseVendor.ToUpper();
                    ReviewSiteModelModelList = ReviewSiteModelModelList.Where(x => (!string.IsNullOrWhiteSpace(x.LastPurchaseVendor) && x.LastPurchaseVendor.ToUpper().Contains(LastPurchaseVendor))).ToList();
                }
                if (InactiveFlag != null && InactiveFlag != "")
                {
                    var v = false;
                    if (InactiveFlag.Equals("true"))
                    {
                        v = false;
                    }
                    else
                    {
                        v = true;
                    }
                    ReviewSiteModelModelList = ReviewSiteModelModelList.Where(x => x.InactiveFlag == v).ToList();
                }
            }
            return ReviewSiteModelModelList;
        }
        private List<ReviewSiteModel> GetAllReviewSiteSortByColumnWithOrder(string order, string orderDir, List<ReviewSiteModel> data)
        {
            List<ReviewSiteModel> lst = new List<ReviewSiteModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SiteName).ToList() : data.OrderBy(p => p.SiteName).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QtyOnHand).ToList() : data.OrderBy(p => p.QtyOnHand).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QtyOnOrder).ToList() : data.OrderBy(p => p.QtyOnOrder).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastPurchaseDate).ToList() : data.OrderBy(p => p.LastPurchaseDate).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastPurchaseCost).ToList() : data.OrderBy(p => p.LastPurchaseCost).ToList();
                        break;
                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastPurchaseVendor).ToList() : data.OrderBy(p => p.LastPurchaseVendor).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SiteName).ToList() : data.OrderBy(p => p.SiteName).ToList();
                        break;
                }
            }
            return lst;
        }

        public PartialViewResult AddPartTransfer(long IssuePartId, string IssueDescription, string IssueClientLookupId, string SiteName, long RequestPartId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartsVM objPartsVM = new PartsVM();
            PartTransferModel pTransferModel = new PartTransferModel();
            PartModel objPartModel = new PartModel();
            objPartsVM.partSummaryModel = GetPartSummary(IssuePartId);
            pTransferModel.IssuePartId = IssuePartId;
            pTransferModel.IssueClientLookupId = IssueClientLookupId;
            pTransferModel.IssueDescription = IssueDescription;
            pTransferModel.SiteName = SiteName;
            pTransferModel.QtyOnHand = Convert.ToDecimal(objPartsVM.partSummaryModel.OnHandQuantity);
            objPartsVM.partSummaryModel = GetPartSummary(RequestPartId);
            objPartModel.PartImageUrl = objPartsVM.partSummaryModel.PartImageUrl;
            pTransferModel.RequestPartId = RequestPartId;
            pTransferModel.RequestClientLookupId = objPartsVM.partSummaryModel.ClientLookupId;
            pTransferModel.RequestDescription = objPartsVM.partSummaryModel.Description;
            objPartsVM.partTransferModel = pTransferModel;
            List<KeyValuePair<string, string>> ddlAccount = new List<KeyValuePair<string, string>>();
            Account acct = new DataContracts.Account()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId
            };
            List<DataContracts.Account> laborList = acct.RetrieveAllTemplatesWithClient(userData.DatabaseKey);
            if (laborList != null)
            {
                laborList = laborList.Where(l => !string.IsNullOrEmpty(l.ClientLookupId) && !l.InactiveFlag).ToList();
            }
            pTransferModel.ShippingAccountList = laborList.Select(x => new SelectListItem { Text = x.ClientLookupId.Trim() + "-" + x.Name + "(" + x.AccountId.ToString() + ")", Value = Convert.ToString(x.AccountId) });
            
            objPartsVM.PartModel = objPartModel;
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_PartTransferCreate.cshtml", objPartsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPartTransfer(PartsVM partsVM, string Command)
        {
            PartsWrapper _PartsObj = new PartsWrapper(userData);
            DataContracts.PartTransfer pt = new DataContracts.PartTransfer();
            if (ModelState.IsValid)
            {
                List<String> errorList = _PartsObj.AddPartTransfer(partsVM.partTransferModel);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PartId = partsVM.partTransferModel.RequestPartId, Command = Command }, JsonRequestBehavior.AllowGet);
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

        #region BulkUpdate
        [HttpPost]
        public PartialViewResult ShowBulkUpdate(string[] PartIds)
        {
            PartsVM objPartsVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartBulkUpdateModel pModel = new PartBulkUpdateModel();
            string str = string.Empty;
            foreach (var v in PartIds)
            {
                str += v + ",";
            }
            if (str != null)
            {
                str = str.Remove(str.Length - 1, 1);
            }
            pModel.PartIdList = str;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> objLookupStokeType = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> objLookupIssueUnit = new List<DataContracts.LookupList>();
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                objLookupStokeType = AllLookUps.Where(x => x.ListName == LookupListConstants.STOCK_TYPE).ToList();
                if (objLookupStokeType != null)
                {
                    pModel.LookupStokeTypeList = objLookupStokeType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }

                objLookupIssueUnit = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (objLookupIssueUnit != null)
                {
                    pModel.LookupIssueUnitList = objLookupIssueUnit.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }

            }
            //V2-379
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                pModel.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.Account.ToString() });
            }
            objPartsVM.partBulkUpdateModel = pModel;
            objPartsVM.userData = userData; // RKL
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_PartBulkUpdate.cshtml", objPartsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartBulkUpdate(PartsVM PartVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            int updatedItemCount = 0;
            if (ModelState.IsValid)
            {
                PartsWrapper pWrapper = new PartsWrapper(userData);
                var objPart = pWrapper.PartBulkUpload(PartVM.partBulkUpdateModel);

                if (objPart != null && objPart.Count > 0)
                {
                    return Json(objPart, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (!String.IsNullOrEmpty(PartVM.partBulkUpdateModel.PartIdList))
                    {
                        updatedItemCount = PartVM.partBulkUpdateModel.PartIdList.Split(',').ToList().Count();
                    }
                    return Json(new { Result = JsonReturnEnum.success.ToString(), UpdatedItemCount = updatedItemCount }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion BulkUpdate
        #region Comment       
        [HttpPost]
        public PartialViewResult LoadComments(long PartId)
        {
            PartsVM objPartVM = new PartsVM();
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
                objPartVM.userMentionDatas = userMentionDatas;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                objPartVM.NotesList = NotesList;
            }
            LocalizeControls(objPartVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("_CommentsList", objPartVM);
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
        #region POPR
        public string PopulatePOPR(int? draw, int? start, int? length, long PartId = 0)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var POPRItems = pWrapper.PopulatePOPRitems(PartId);
            POPRItems = this.GetPOPRItemsByColumnWithOrder(order, orderDir, POPRItems);


            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = POPRItems.Count();
            totalRecords = POPRItems.Count();

            int initialPage = start.Value;
            var filteredResult = POPRItems
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<POPRModel> GetPOPRItemsByColumnWithOrder(string order, string orderDir, List<POPRModel> data)
        {
            List<POPRModel> lst = new List<POPRModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Vendor).ToList() : data.OrderBy(p => p.Vendor).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorName).ToList() : data.OrderBy(p => p.VendorName).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }

        #endregion

        #region Activity
        public PartialViewResult LoadActivity(long PartID)
        {
            PartsVM objPartVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            List<PartsHistoryModel> partsHistoryModelList = pWrapper.PopulatePartHistoryLog(PartID);
            objPartVM.partsHistoryModelList = partsHistoryModelList;
            LocalizeControls(objPartVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("_ActivityLog", objPartVM);
        }
        #endregion


        #region Additional Part Functions
        #region Adjust On Hand Quantity
        [HttpPost]
        public JsonResult SaveItemPhysicalInventory(PartsVM model)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                PartGridPhysicalInvList gridPhysical = new PartGridPhysicalInvList
                {
                    PartId = model.inventoryModel.PartId,
                    QuantityCount = model.inventoryModel.ReceiptQuantity,
                    PartClientLookupId = model.inventoryModel.PartClientLookupId,
                    Description = model.inventoryModel.Description,
                    PartUPCCode = model.inventoryModel.PartUPCCode
                };
                PartsWrapper phyWrapper = new PartsWrapper(userData);
                PartHistory returnObj = phyWrapper.SaveHandsOnQty(gridPhysical);
                return Json(new { Result = JsonReturnEnum.success.ToString(), data = gridPhysical }, JsonRequestBehavior.AllowGet);
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Part Checkout
        [HttpPost]
        public ActionResult SaveInventorydata(PartsVM data)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                var mydata = data.inventoryCheckoutModel;
                PartsWrapper invWrapper = new PartsWrapper(userData);
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

        #endregion

        [HttpGet]
        public JsonResult GetPartIdByClientLookUpId(string clientLookUpId)
        {
            var commonWrapper = new CommonWrapper(userData);
            var part = commonWrapper.RetrievePartIdByClientLookUp(clientLookUpId);
            return Json(new { PartId = part.PartId }, JsonRequestBehavior.AllowGet);
        }

        #region V2-641 Add Part Dynamic Ui Configurations
        public PartialViewResult AddPartsDynamic()
        {
            PartsVM objPartsVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartModel pModel = new PartModel();
            objPartsVM.AddPart = new Models.Parts.UIConfiguration.AddPartModelDynamic();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objPartsVM.security = this.userData.Security;

            //var part = pWrapper.PopulateDropdownControls(pModel);

            var AllLookUps = commonWrapper.GetAllLookUpList();
            objPartsVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddPart, userData);
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
            //V2-379
            //var AcclookUpList = GetLookupList_Account();
            var AcclookUpList = GetAccountByActiveState(true);

            if (AcclookUpList != null)
            {
                objPartsVM.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            }
            //objPartsVM.PartModel = part;
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_AddPartDynamic.cshtml", objPartsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPartsDynamic(PartsVM PartVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                PartsWrapper pWrapper = new PartsWrapper(userData);
                Mode = "add";
                var objPart = pWrapper.AddPartsDynamic(PartVM);

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
        #endregion

        #region V2-641 Edit Part Dynamic Ui Configuration
        public PartialViewResult EditPartsDynamic(long PartId)
        {
            PartsVM objPartsVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            objPartsVM.security = this.userData.Security;
            objPartsVM.EditPart = new Models.Parts.UIConfiguration.EditPartModelDynamic();

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
            objPartsVM.partSummaryModel = GetPartSummary(PartId);
            objPartsVM.EditPart.PartImageUrl = objPartsVM.partSummaryModel.PartImageUrl;
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_EditPartDynamic.cshtml", objPartsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePartsDynamic(PartsVM objPartsVM)
        {
            //string emptyValue = string.Empty;
            PartsWrapper pWrapper = new PartsWrapper(userData);
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

        #region V2-641 For Partdetails Dynamic
        public PartialViewResult PartDetailsDynamic(long PartId)
        {
            PartsVM objPartsVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            #region ChargeType
            ParInvCheckoutModel objInventoryCheckoutModel = new ParInvCheckoutModel();
            objPartsVM.userData = this.userData;
            var ChargeTypeList = UtilityFunction.populateChargeTypeForInventoryCheckout();
            if (ChargeTypeList != null)
            {
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                {
                    ChargeTypeList = ChargeTypeList.Where(x => x.text != ChargeType.Location).ToList();
                    objInventoryCheckoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                else
                {
                    objInventoryCheckoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
            }
            objPartsVM.inventoryCheckoutModel = objInventoryCheckoutModel;
            #endregion
            PartsHistoryModel partsHistoryModel = new PartsHistoryModel();
            PartsReceiptModel partsReceiptModel = new PartsReceiptModel();
            PartSummaryModel partSummaryModel = new PartSummaryModel();
            UpdatePartCostsModel updatePartCostsModel = new UpdatePartCostsModel();
            ChangePartIdModel changePartIdModel = new ChangePartIdModel();

            PartUDF PartUDF = new PartUDF();
            Part Part = new Part();
            CreatedLastUpdatedPartModel objCreatedLastUpdatedPartModel = new CreatedLastUpdatedPartModel();
            CommonWrapper comWrapper = new CommonWrapper(userData);
            // Task attTask;
            Task[] attTask = new Task[4];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            attTask[0] = Task.Factory.StartNew(() => objPartsVM.attachmentCount = objCommonWrapper.AttachmentCount(PartId, AttachmentTableConstant.Part, userData.Security.Parts.Edit));
            attTask[1] = Task.Factory.StartNew(() => objPartsVM.PartModel = pWrapper.PopulatePartDetails_V2(PartId));
            attTask[2] = Task.Factory.StartNew(() => PartUDF = pWrapper.RetrievePartUDFByPartId(PartId));
            attTask[3] = Task.Factory.StartNew(() => objPartsVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPartWidget, userData));
            Task.WaitAll(attTask);
            objCreatedLastUpdatedPartModel = pWrapper.PopulateCreateModifyDate(PartId);

            var partMasterModel = pWrapper.PopulateReviewSiteDetails(PartId);
            objPartsVM.partMasterModel = partMasterModel;
            var PartHistorydateList = UtilityFunction.PartDatesList();
            if (PartHistorydateList != null)
            {
                partsHistoryModel.HistoryDateList = PartHistorydateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var PartReceiptdateList = UtilityFunction.PartDatesList();
            if (PartReceiptdateList != null)
            {
                partsReceiptModel.ReceiptDateList = PartReceiptdateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            objPartsVM.partsHistoryModel = partsHistoryModel;
            objPartsVM.partsReceiptModel = partsReceiptModel;
            objPartsVM.partSummaryModel = GetPartSummary(PartId);
            changePartIdModel.ClientLookupId = objPartsVM.PartModel.ClientLookupId;
            objPartsVM.changePartIdModel = changePartIdModel;
            objPartsVM.PartModel.PartImageUrl = objPartsVM.partSummaryModel.PartImageUrl;
            objPartsVM.security = this.userData.Security;
            objPartsVM._userdata = this.userData;
            objPartsVM.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
            objPartsVM.UsePartMaster = this.userData.Site.UsePartMaster;
            objPartsVM.createdLastUpdatedPartModel = objCreatedLastUpdatedPartModel;
            objPartsVM.PartModel.PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToLower();
            objPartsVM.partSummaryModel.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;


            objPartsVM.ViewPart = new ViewPartModelDynamic();
            objPartsVM.ViewPart = pWrapper.MapPartDataForView(objPartsVM.ViewPart, objPartsVM.PartModel);
            objPartsVM.ViewPart = pWrapper.MapPartUDFDataForView(objPartsVM.ViewPart, PartUDF);

            #region V2-906
            updatePartCostsModel.PartId = PartId;
            updatePartCostsModel.AppliedCost = objPartsVM.PartModel.AppliedCost;
            updatePartCostsModel.AverageCost = objPartsVM.PartModel.AverageCost;
            objPartsVM.UpdatePartCostsModel = updatePartCostsModel;//V2-906
            #endregion
            #region V2-1196

            if(userData.Security.Parts.ConfigureAutoPurchasing)
            {

                var vendorParts = pWrapper.PopulateParts(PartId);
                var partsConfigureAutoPurchasingModel = new PartsConfigureAutoPurchasingModel
                {
                    VendorList = vendorParts.Select(x =>
                        new SelectListItem
                        {
                            Text = $"{x.Vendor_ClientLookupId} - {x.Vendor_Name}",
                            Value = x.Part_Vendor_XrefId.ToString()
                        }).ToList()
                };
                objPartsVM.partsConfigureAutoPurchasingModel = partsConfigureAutoPurchasingModel;
                objPartsVM.partsConfigureAutoPurchasingModel.PartId = PartId;
                objPartsVM.partsConfigureAutoPurchasingModel.PartStoreroomId = objPartsVM.PartModel.PartStoreroomId;
            }
            #endregion
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_PartDetails.cshtml", objPartsVM);
        }


        #endregion

        #region V2-716 Multiple Images
        public PartialViewResult GetImages(int currentpage, int? start, int? length, long PartId)
        {
            PartsVM partsVM = new PartsVM();
            List<ImageAttachmentModel> imgDatalist = new List<ImageAttachmentModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Attachment> Attachments = new List<Attachment>();
            partsVM.security = this.userData.Security;
            partsVM._userdata = this.userData;
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
            partsVM.imageAttachmentModels = imgDatalist;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.Global);

            return PartialView("~/Views/Parts/_AllPartsImages.cshtml", partsVM);
        }
        #endregion

        #region V2-836
        [HttpPost]
        public PartialViewResult GetPartsMainGridMobile(int currentpage = 0, int? start = 0, int? length = 0, int CustomQueryDisplayId = 0,
            string SearchText = "", string PartID = "",
                                       string Description = "", string Section = "", decimal MinimumQty = 0, decimal OnHandQty = 0, string Manufacturer = "", string ManufacturerID = "",
                                       string StockType = "", string Row = "",
                                       string Bin = "",
                                       string Shelf = "", string PlaceArea = "", string Order = "1", string orderDir = "asc")
        {
            PartsVM objPartVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            List<string> statusList = new List<string>();

            objPartVM._userdata = this.userData;

            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;

            List<PartModel> partList = pWrapper.GetPartChunkList(CustomQueryDisplayId, skip, length ?? 0, Order, orderDir, PartID, Description, Section, MinimumQty, OnHandQty, Manufacturer, ManufacturerID, StockType, Row, Bin, SearchText, Shelf, PlaceArea);

            var totalRecords = 0;
            var recordsFiltered = 0;

            recordsFiltered = partList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = partList.Select(o => o.TotalCount).FirstOrDefault();

            var filteredResult = partList
                .ToList();

            ViewBag.Start = skip;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;

            Parallel.ForEach(partList, item =>
            {
                item.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
                item.AzureImageURL = PartsImageUrl(item.PartId);
            });
            objPartVM.PartModelList = partList;
            LocalizeControls(objPartVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_PartGridCardView.cshtml", objPartVM);
        }
        public string PartsImageUrl(long PartId)
        {
            string ImageUrl = string.Empty;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (ClientOnPremise)
            {
                ImageUrl = objCommonWrapper.GetOnPremiseImageUrl(PartId, AttachmentTableConstant.Part);
            }
            else
            {
                ImageUrl = objCommonWrapper.GetAzureImageUrl(PartId, AttachmentTableConstant.Part);
            }
            return ImageUrl;

        }

        #region Details
        public PartialViewResult PartDetailsDynamic_Mobile(long PartId)
        {
            PartsVM objPartsVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            #region ChargeType
            ParInvCheckoutModel objInventoryCheckoutModel = new ParInvCheckoutModel();
            objPartsVM.userData = this.userData;
            var ChargeTypeList = UtilityFunction.populateChargeTypeForInventoryCheckout();
            if (ChargeTypeList != null)
            {
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                {
                    ChargeTypeList = ChargeTypeList.Where(x => x.text != ChargeType.Location).ToList();
                    objInventoryCheckoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                else
                {
                    objInventoryCheckoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
            }
            objPartsVM.inventoryCheckoutModel = objInventoryCheckoutModel;
            #endregion
            PartsHistoryModel partsHistoryModel = new PartsHistoryModel();
            PartsReceiptModel partsReceiptModel = new PartsReceiptModel();
            PartSummaryModel partSummaryModel = new PartSummaryModel();
            PartUDF PartUDF = new PartUDF();
            Part Part = new Part();
            CreatedLastUpdatedPartModel objCreatedLastUpdatedPartModel = new CreatedLastUpdatedPartModel();
            CommonWrapper comWrapper = new CommonWrapper(userData);
            // Task attTask;
            Task[] attTask = new Task[4];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            attTask[0] = Task.Factory.StartNew(() => objPartsVM.attachmentCount = objCommonWrapper.AttachmentCount(PartId, AttachmentTableConstant.Part, userData.Security.Parts.Edit));
            attTask[1] = Task.Factory.StartNew(() => objPartsVM.PartModel = pWrapper.PopulatePartDetails_V2(PartId));
            attTask[2] = Task.Factory.StartNew(() => PartUDF = pWrapper.RetrievePartUDFByPartId(PartId));
            attTask[3] = Task.Factory.StartNew(() => objPartsVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewPartWidget, userData));
            Task.WaitAll(attTask);
            objCreatedLastUpdatedPartModel = pWrapper.PopulateCreateModifyDate(PartId);

            var partMasterModel = pWrapper.PopulateReviewSiteDetails(PartId);
            objPartsVM.partMasterModel = partMasterModel;
            var PartHistorydateList = UtilityFunction.PartDatesList();
            if (PartHistorydateList != null)
            {
                partsHistoryModel.HistoryDateList = PartHistorydateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var PartReceiptdateList = UtilityFunction.PartDatesList();
            if (PartReceiptdateList != null)
            {
                partsReceiptModel.ReceiptDateList = PartReceiptdateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            objPartsVM.partsHistoryModel = partsHistoryModel;
            objPartsVM.partsReceiptModel = partsReceiptModel;
            objPartsVM.partSummaryModel = GetPartSummary_Mobile(PartId);
            objPartsVM.PartModel.PartImageUrl = objPartsVM.partSummaryModel.PartImageUrl;
            objPartsVM.security = this.userData.Security;
            objPartsVM._userdata = this.userData;
            objPartsVM.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
            objPartsVM.UsePartMaster = this.userData.Site.UsePartMaster;
            objPartsVM.createdLastUpdatedPartModel = objCreatedLastUpdatedPartModel;
            objPartsVM.PartModel.PackageLevel = userData.DatabaseKey.Client.PackageLevel.ToLower();
            objPartsVM.partSummaryModel.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            #region V2-1196

            if (userData.Security.Parts.ConfigureAutoPurchasing)
            {

                var vendorParts = pWrapper.PopulateParts(PartId);
                var partsConfigureAutoPurchasingModel = new PartsConfigureAutoPurchasingModel
                {
                    VendorList = vendorParts.Select(x =>
                        new SelectListItem
                        {
                            Text = $"{x.Vendor_ClientLookupId} - {x.Vendor_Name}",
                            Value = x.Part_Vendor_XrefId.ToString()
                        }).ToList()
                };
                objPartsVM.partsConfigureAutoPurchasingModel = partsConfigureAutoPurchasingModel;
                objPartsVM.partsConfigureAutoPurchasingModel.PartId = PartId;
                objPartsVM.partsConfigureAutoPurchasingModel.PartStoreroomId = objPartsVM.PartModel.PartStoreroomId;
            }
            #endregion

            objPartsVM.ViewPart = new ViewPartModelDynamic();
            objPartsVM.ViewPart = pWrapper.MapPartDataForView(objPartsVM.ViewPart, objPartsVM.PartModel);
            objPartsVM.ViewPart = pWrapper.MapPartUDFDataForView(objPartsVM.ViewPart, PartUDF);

            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_PartDetails.cshtml", objPartsVM);
        }
        public PartSummaryModel GetPartSummary_Mobile(long partId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartSummaryModel pSummary = new PartSummaryModel();
            PartModel objPart = pWrapper.PopulatePartDetails(partId);
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            if (objPart != null)
            {
                pSummary.ClientLookupId = objPart.ClientLookupId;
                pSummary.OnHandQuantity = objPart.OnHandQuantity;
                pSummary.OnOrderQuantity = objPart.OnOrderQuantity;
                pSummary.OnRequestQuantity = objPart.OnRequestQuantity;
                pSummary.Description = objPart.Description;
                pSummary.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
                if (pSummary.ClientOnPremise)
                {
                    pSummary.PartImageUrl = objCommonWrapper.GetOnPremiseImageUrl(partId, AttachmentTableConstant.Part);
                }
                else
                {
                    pSummary.PartImageUrl = objCommonWrapper.GetAzureImageUrl(partId, AttachmentTableConstant.Part);
                }

            }
            pSummary.InactiveFlag = objPart.InactiveFlag;
            pSummary.MinimumQuantity = objPart.Minimum;
            pSummary.Maximum = objPart.Maximum;
            pSummary.AppliedCost = objPart.AppliedCost;
            pSummary.IssueUnit = objPart.IssueUnit;
            pSummary.StockType = objPart.StockType;
            pSummary.Manufacturer = objPart.Manufacturer;
            pSummary.ManufacturerID = objPart.ManufacturerID;
            pSummary.Section = objPart.Section;
            pSummary.PlaceArea = objPart.PlaceArea;
            pSummary.Row = objPart.Row;
            pSummary.Shelf = objPart.Shelf;
            pSummary.Bin = objPart.Bin;
            return pSummary;
        }
        #endregion
        #region Add Edit
        public PartialViewResult AddPartsDynamic_Mobile()
        {
            PartsVM objPartsVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartModel pModel = new PartModel();
            objPartsVM.AddPart = new Models.Parts.UIConfiguration.AddPartModelDynamic();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objPartsVM.security = this.userData.Security;

            var AllLookUps = commonWrapper.GetAllLookUpList();
            objPartsVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddPart, userData);
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
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_AddPartDynamicPopUp.cshtml", objPartsVM);
        }
        public PartialViewResult EditPartsDynamic_Mobile(long PartId)
        {
            PartsVM objPartsVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            CommonWrapper comWrapper = new CommonWrapper(userData);
            objPartsVM.security = this.userData.Security;
            objPartsVM.EditPart = new Models.Parts.UIConfiguration.EditPartModelDynamic();

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
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_EditPartDynamicPopUp.cshtml", objPartsVM);
        }
        #endregion

        #region Photos
        [HttpPost]
        public PartialViewResult LoadPhotos_Mobile()
        {
            PartsVM partsVM = new PartsVM();
            partsVM._userdata = this.userData;
            partsVM.security = this.userData.Security;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_ImageDetails.cshtml", partsVM);
        }
        public PartialViewResult GetImages_Mobile(int currentpage, int? start, int? length, long PartId)
        {
            PartsVM partsVM = new PartsVM();
            List<ImageAttachmentModel> imgDatalist = new List<ImageAttachmentModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Attachment> Attachments = new List<Attachment>();
            partsVM.security = this.userData.Security;
            partsVM._userdata = this.userData;
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
            partsVM.imageAttachmentModels = imgDatalist;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.Global);

            return PartialView("~/Views/Parts/Mobile/_AllImages.cshtml", partsVM);
        }
        #endregion

        #region Comment
        [HttpPost]
        public PartialViewResult GetCommentsDetails_Mobile()
        {
            PartsVM objPartsVM = new PartsVM();
            objPartsVM._userdata = this.userData;
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_CommentDetails.cshtml", objPartsVM);
        }
        #endregion

        #region QrCode
        [HttpPost]
        public PartialViewResult PartDetailsQRcode_Mobile(string[] PartClientLookups)
        {
            PartsVM partsVM = new PartsVM();
            QRCodeModel qRCodeModel = new QRCodeModel();
            if (PartClientLookups != null)
            {
                qRCodeModel.PartIdsList = new List<string>(PartClientLookups);
            }
            else
            {
                qRCodeModel.PartIdsList = new List<string>();
            }
            partsVM.qRCodeModel = qRCodeModel;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("Mobile/_PartDetailsQRCode", partsVM);
        }
        #endregion

        #region LookpList
        [HttpPost]
        public JsonResult GetEquipmentLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            var modelList = new List<EquipmentLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0"; 
            string orderDir = "asc"; 

            modelList = commonWrapper.GetEquipmentLookupListGridDataMobile(order, orderDir, Start, Length, Search, Search);
           
            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }
        [HttpPost]
        public JsonResult GetWorkOrderLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            List<WorkOrderLookUpModel> modelList = new List<WorkOrderLookUpModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = "0"; 
            string orderDir = "asc";

            var ClientLookupId = !string.IsNullOrEmpty(Search) ? Search.Trim() : string.Empty;
            var Description = !string.IsNullOrEmpty(Search) ? Search.Trim() : string.Empty;
            var ChargeTo = !string.IsNullOrEmpty(Search) ? Search.Trim() : string.Empty;

            modelList = commonWrapper.GetWorkOrderLookupListGridData(order, orderDir, Start, Length, ClientLookupId, Description, ChargeTo, "", "", "");
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return Json(new { recordsTotal = totalRecords, data = modelList });
        }
        [HttpPost]

        public JsonResult GetLocationLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            List<LocationLookUpModel> modelList = new List<LocationLookUpModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = "0"; 
            string orderDir = "asc"; 

            var clientLookupId = !string.IsNullOrEmpty(Search) ? Search.Trim() : string.Empty;
            var name = !string.IsNullOrEmpty(Search) ? Search.Trim() : string.Empty;

            modelList = commonWrapper.PopulateLocationList_V2(Start, Length, order, orderDir, clientLookupId, name);
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return Json(new { recordsTotal = totalRecords, data = modelList });
        }
        #endregion
        #endregion

        #region V2-840 Card-View
        public PartialViewResult GetCardViewData(int currentpage, int? start, int? length, string currentorderedcolumn,string currentorder, int CustomQueryDisplayId, string SearchText = "", 
                                                 string PartID = "",string Description = "", string Section = "", decimal MinimumQty = 0, decimal OnHandQty = 0, string Manufacturer = "", 
                                                 string ManufacturerID = "",string StockType = "", string Row = "",string Bin = "",string Shelf = "", string PlaceArea = "",string Order = "1")
        {
            PartsVM objPartsVM = new PartsVM();
            PartsModel partsModel = new PartsModel();
            objPartsVM._userdata = this.userData;
            start = start.HasValue
           ? start / length
           : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;
            PartsWrapper pWrapper = new PartsWrapper(userData);
            List<PartModel> cardData = pWrapper.GetPartChunkList(CustomQueryDisplayId, skip, length ?? 0, currentorderedcolumn, currentorder, PartID, Description, Section, MinimumQty, OnHandQty, Manufacturer, ManufacturerID, StockType, Row, Bin, SearchText, Shelf, PlaceArea);
            var totalRecords = 0;

            var recordsFiltered = 0;
            recordsFiltered = cardData.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = cardData.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;//start / length + 1;//
            var filteredResult = cardData.ToList();

            Parallel.ForEach(cardData, item =>
            {
                item.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
                item.AzureImageURL = PartImageUrl(item.PartId);
                item.security = this.userData.Security;
            });
            objPartsVM.PartModelList = cardData;
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_PartGridCardView.cshtml", objPartsVM);
        }

        public string PartImageUrl(long PartId)
        {
            string ImageUrl = string.Empty;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (ClientOnPremise)
            {
                ImageUrl = objCommonWrapper.GetOnPremiseImageUrl(PartId, AttachmentTableConstant.Part);
            }
            else
            {
                ImageUrl = objCommonWrapper.GetAzureImageUrl(PartId, AttachmentTableConstant.Part);
            }
            return ImageUrl;

        }
        #endregion

        #region V2-880
        [HttpPost]
        public string GetParts_VendorCatalogItem(int? draw, int? start, int? length, long PartMasterId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PartsWrapper pWrapper = new PartsWrapper(userData);
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            var VendorcatalogItem = pWrapper.GetPartVendorCatalogItem(PartMasterId, skip, length ?? 0, order, orderDir);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            if (VendorcatalogItem != null && VendorcatalogItem.Count > 0)
            {
                recordsFiltered = VendorcatalogItem[0].TotalCount;
                totalRecords = VendorcatalogItem[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }

            int initialPage = start.Value;
            var filteredResult = VendorcatalogItem
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion

        #region V2-906
        public JsonResult UpdatePartCosts(PartsVM PartVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                PartsWrapper pWrapper = new PartsWrapper(userData);
                string rtrMsg = string.Empty;
                var objPart = pWrapper.GetUpdatePartCosts(PartVM.UpdatePartCostsModel.PartId, PartVM.UpdatePartCostsModel.AppliedCost, PartVM.UpdatePartCostsModel.AverageCost);
                if (objPart.ErrorMessages != null && objPart.ErrorMessages.Count > 0)
                {
                    return Json(objPart.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //-------------
                    var createEventStatus = pWrapper.CreatePartEvent(PartVM.UpdatePartCostsModel.PartId, "CostChange", "");
                    if (createEventStatus != null && createEventStatus.Count > 0)
                    {
                        return Json(createEventStatus, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), PartId = objPart.PartId, mode = Mode }, JsonRequestBehavior.AllowGet);
                    }
                    //-------------
                   
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region V2-918
        #region Attachement
        [HttpPost]
        public PartialViewResult LoadAttachments_Mobile()
        {
            PartsVM partsVM = new PartsVM();
            partsVM._userdata = this.userData;
            partsVM.security = this.userData.Security;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_AttachementList.cshtml", partsVM);
        }

        [HttpGet]
        public PartialViewResult AddPAttachment_Mobile(long partId, string ClientLookupId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartsVM objPartsVM = new PartsVM();
            AttachmentModel Attachment = new AttachmentModel();

            PartModel objPart = new PartModel();
            objPart.PartId = partId;
            objPart.ClientLookupId = ClientLookupId;
            Attachment.PartId = partId;
            objPartsVM.attachmentModel = Attachment;
            objPartsVM.PartModel = objPart;
            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_AddAttachment.cshtml", objPartsVM);
        }
        #endregion
        #region Vendor_Mobile
        [HttpPost]
        public PartialViewResult LoadVendor_Mobile()
        {
            PartsVM partsVM = new PartsVM();
            partsVM._userdata = this.userData;
            partsVM.security = this.userData.Security;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_VendorList.cshtml", partsVM);
        }
        [HttpGet]
        public ActionResult PartsVedndorAdd_Mobile(long _partId, string ClientLookupId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PartsVendorModel partVendorXref = new PartsVendorModel();
            PartsVM partsVM = new PartsVM();
            PartModel partModel = new PartModel();
            List<DataContracts.LookupList> objLookupUnitOfMeasure = new List<DataContracts.LookupList>();
            partVendorXref.PartId = _partId;
            partModel.PartId = _partId;
            partModel.ClientLookupId = ClientLookupId;

            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                objLookupUnitOfMeasure = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (objLookupUnitOfMeasure != null)
                {
                    partVendorXref.OrderUnitList = objLookupUnitOfMeasure.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            partsVM.partsVendorModel = partVendorXref;
            partsVM.PartModel = partModel;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_AddPartVendor.cshtml", partsVM);
        }
        [HttpGet]
        public ActionResult PartsVedndorEdit_Mobile(long partId, long _part_Vendor_XrefId, int updatedIndex, string ClientLookupId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PartsVM partsVM = new PartsVM();
            PartModel partModel = new PartModel();
            List<DataContracts.LookupList> objLookupUnitOfMeasure = new List<DataContracts.LookupList>();
            partModel.ClientLookupId = ClientLookupId;
            partModel.PartId = partId;
            PartsVendorModel partVendorXref = new PartsVendorModel();
            partVendorXref = pWrapper.EditVendor(partId, _part_Vendor_XrefId, partModel.ClientLookupId, updatedIndex);

            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                objLookupUnitOfMeasure = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (objLookupUnitOfMeasure != null)
                {
                    partVendorXref.OrderUnitList = objLookupUnitOfMeasure.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            partsVM.partsVendorModel = partVendorXref;
            partsVM.PartModel = partModel;
            partsVM.partSummaryModel = GetPartSummary(partId);
            partsVM.PartModel.PartImageUrl = partsVM.partSummaryModel.PartImageUrl;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
             return PartialView("~/Views/Parts/Mobile/_AddPartVendor.cshtml", partsVM);
        }
        #endregion
        #region Equipment_Mobile
        [HttpPost]
        public PartialViewResult LoadEquipment_Mobile()
        {
            PartsVM partsVM = new PartsVM();
            partsVM._userdata = this.userData;
            partsVM.security = this.userData.Security;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_EquipmentList.cshtml", partsVM);
        }

        [HttpGet]
        public ActionResult PartsEquipmentAdd_Mobile(long partId, string clientLookupId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            EquipmentPartXrefModel partEquipmentXref = new EquipmentPartXrefModel();
            PartsVM partsVM = new PartsVM();
            partEquipmentXref.PartId = partId;
            partsVM.equipmentPartXrefModel = partEquipmentXref;
            partsVM.PartModel = new PartModel();
            partsVM.PartModel.ClientLookupId = clientLookupId;
            partsVM.PartModel.PartId = partId;
            partsVM.partSummaryModel = GetPartSummary(partId);
            partsVM.PartModel.PartImageUrl = partsVM.partSummaryModel.PartImageUrl;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
             return PartialView("~/Views/Parts/Mobile/_AddPartEquipment.cshtml", partsVM);
        }
        [HttpGet]
        public ActionResult PartsEquipmentEdit_Mobile(long partId, long _equipment_Parts_XrefId, int updatedIndex, string ClientLookupId)
        {
            PartsWrapper pWrapper = new PartsWrapper(userData);
            PartsVM partsVM = new PartsVM();
            PartModel partModel = new PartModel();
            partModel.ClientLookupId = ClientLookupId;
            var _equipmentPartXrefModel = pWrapper.EditPartEquipment(partId, _equipment_Parts_XrefId, ClientLookupId, updatedIndex);
            var EquipmentLookUplist = GetLookUpList_Equipment();
            if (EquipmentLookUplist != null)
            {
                _equipmentPartXrefModel.EquipmentList = EquipmentLookUplist.Select(x => new SelectListItem { Text = x.Equipment + " - " + x.Name, Value = x.Equipment.ToString() });
            }
            partsVM.equipmentPartXrefModel = _equipmentPartXrefModel;
            partModel.PartId = partId;
            partsVM.PartModel = partModel;
            partsVM.partSummaryModel = GetPartSummary(partId);
            partsVM.PartModel.PartImageUrl = partsVM.partSummaryModel.PartImageUrl;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
             return PartialView("~/Views/Parts/Mobile/_AddPartEquipment.cshtml", partsVM);
        }
        #endregion
        [HttpPost]
        public PartialViewResult LoadHistory_Mobile()
        {
            PartsVM partsVM = new PartsVM();
            PartsHistoryModel partsHistoryModel = new PartsHistoryModel();
           
            partsVM._userdata = this.userData;
            partsVM.security = this.userData.Security;
            var PartHistorydateList = UtilityFunction.PartDatesList();
            if (PartHistorydateList != null)
            {
                partsHistoryModel.HistoryDateList = PartHistorydateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            
            partsVM.partsHistoryModel = partsHistoryModel;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_HistoryList.cshtml", partsVM);
        }
        [HttpPost]
        public PartialViewResult LoadReceipt_Mobile()
        {
            PartsVM partsVM = new PartsVM();
            PartsReceiptModel partsReceiptModel = new PartsReceiptModel();
            partsVM._userdata = this.userData;
            partsVM.security = this.userData.Security;
            var PartReceiptdateList = UtilityFunction.PartDatesList();
            if (PartReceiptdateList != null)
            {
                partsReceiptModel.ReceiptDateList = PartReceiptdateList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            partsVM.partsReceiptModel = partsReceiptModel;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_ReceiptList.cshtml", partsVM);
        }

        [HttpPost]
        public PartialViewResult LoadReview_Mobile()
        {
            PartsVM partsVM = new PartsVM();
            partsVM._userdata = this.userData;
            partsVM.security = this.userData.Security;
            LocalizeControls(partsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/Mobile/_ReviewList.cshtml", partsVM);
        }

        #endregion

        #region V2-1007
        public RedirectResult PartsDetailFromEquipment(long partId, long equipmentId)
        {
            TempData["Mode"] = "PartsDetailFromEquipment";
            string strPartId = Convert.ToString(partId);
            TempData["partId"] = strPartId;
            string strEquipmentId = Convert.ToString(equipmentId);
            TempData["equipmentId"] = strEquipmentId;
            return Redirect("/Parts/Index?page=Inventory_Part");
        }
        #endregion

        #region V2-1089 DevExpress QRCode
        [EncryptedActionParameter]
        public ActionResult QRCodeGenerationUsingDevExpress(bool SmallLabel)
        {
            var PartIdsList = new List<string>();
            if (TempData["QRCodePartIdList"] != null)
            {
                PartIdsList = (List<string>)TempData["QRCodePartIdList"];
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
                        MinQty = decimal.Parse(splitArray[4]).ToString("0.#####"),
                        MaxQty = decimal.Parse(splitArray[5]).ToString("0.#####"),
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

        #region V2-1203 PartModel

        [HttpPost]
        public PartialViewResult PartModelWizard(long PartId)
        {
            PartsVM objPartsVM = new PartsVM();
            PartsWrapper pWrapper = new PartsWrapper(userData);
            objPartsVM.security = this.userData.Security;
            objPartsVM.AddPart = new AddPartModelDynamic();
            objPartsVM.PartModel = new PartModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            objPartsVM.AddPart = pWrapper.RetrievePartDetailsByPartIdForPartModel(PartId);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            objPartsVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddPart, userData);
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
            objPartsVM.CurrentPartId = PartId;

            LocalizeControls(objPartsVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/Parts/_AddPartModelWizard.cshtml", objPartsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPartModel(PartsVM partVM)
        {
            PartsWrapper partWrapper = new PartsWrapper(userData);
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
        public ActionResult UpdatePartsConfigureAutoPurchasing(PartsVM partVM)
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
        public ActionResult AddPartsVendorConfigureAutoPurchasing(long PartId,long VendorId,string VendorClientLoopId)
        {
            PartsWrapper _PartsObj = new PartsWrapper(userData);
            Part_Vendor_Xref pvx;
            if (VendorId > 0 && !string.IsNullOrEmpty(VendorClientLoopId))
            {
                    pvx = _PartsObj.AddPartVendorXrefPartsConfigureAutoPurchasing(PartId,VendorId,VendorClientLoopId);
                
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
