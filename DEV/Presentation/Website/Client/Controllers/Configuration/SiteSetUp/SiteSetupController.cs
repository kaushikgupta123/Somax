using Client.BusinessWrapper.Configuration.SiteSetUp;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.MasterSanitationLibrary;
using Client.Models.Configuration.SiteSetUp;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureUtil;
using Newtonsoft.Json;
using DataContracts;
using Client.BusinessWrapper.Common;
using Client.Models;

namespace Client.Controllers.Configuration.SiteSetUp
{
    public class SiteSetupController : ConfigBaseController
    {
        public ActionResult Index()
        {
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);           
            var site = sWrapper.SiteDetails();
            string keylist = AutoGenerateKey.PR_ANNUAL + ',' + AutoGenerateKey.PO_ANNUAL;
            var customerList = sWrapper.CustomIdDetails(keylist);
            // 2022-Feb-09
            // V2
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (ClientOnPremise)
            {  
              site.Logo = commonWrapper.GetOnPremiseImageUrl(site.SiteId, AttachmentTableConstant.Site);
            }
            else
            { 
              site.Logo = commonWrapper.GetAzureImageUrl(site.SiteId, AttachmentTableConstant.Site);// pick logo from attachment table
            }
            #region Dropdown
            var StatusList = UtilityFunction.ActiveInactiveStatusTypes();
            var LanguageList = UtilityFunction.LocalizationTypes();
            var TimeZoneList = GetTimeZoneList();
            var PrintTypeList = UtilityFunction.PrintTypeList();

            var MobileWOTimerList = UtilityFunction.GetMobileWOTimerList();
            //V2-676
            var WOBarCodeList = UtilityFunction.GetWOBarCodeList();
            var CreatorList = sWrapper.CreatorList();
            var NonStockList = commonWrapper.AccountList();
            string PrintType = sWrapper.PrintType();
            #endregion

            #region V2-720 Approval Group Settings
            ApprovalGroupSettingsModel approvalGroupSettingsModel = new ApprovalGroupSettingsModel();
            approvalGroupSettingsModel= sWrapper.ApprovalGroupSettingsDetails();
            #endregion
            SiteSetUpModel siteModel = new SiteSetUpModel()
            {
                #region OverView
                SiteId = site.SiteId,
                Name = site.Name,
                Status = StatusList != null ? StatusList.Where(x => x.value == site.Status).Select(x => x.text).FirstOrDefault() : null,
                Language = LanguageList != null ? LanguageList.Where(x => x.value == site.LocalizationLanguageAndCulture).Select(x => x.text).FirstOrDefault() : null,
                TimeZone = site.TimeZone,
                TimeZoneList = TimeZoneList != null ? TimeZoneList.Select(x => new SelectListItem { Text = x.Value, Value = x.ResourceId.ToString() }) : new SelectList(new[] { "" }),
                CreatorID = site.AutoPurch_CreatorId,
                CreatorList = CreatorList != null ? CreatorList.Select(x => new SelectListItem { Text = x.ClientLookupId + " | " + x.NameFirst + " " + x.NameLast, Value = x.PersonnelId.ToString() }) : new SelectList(new[] { "" }),

                IsondemandWorkorderaccess = site.MaintOnDemand,
                PrintType = PrintType,
                PrintTypeList = PrintTypeList != null ? PrintTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : new SelectList(new[] { "" }),
                PMLibrary = site.PMLibrary,
                //changes for  V2-576
                IsUsePlanning = site.UsePlanning,
                MobileWOTimer = site.MobileWOTimer,
                MobileWOTimerList = MobileWOTimerList != null ? MobileWOTimerList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : new SelectList(new[] { "" }),
                _userdata = this.userData,
                #endregion
                #region Photo
                ImageUrl = site.Logo,
                ClientOnPremise = userData.DatabaseKey.Client.OnPremise,
                #endregion
                #region Bill To
                BillToName = site.BillToName,
                BillToAddress1 = site.BillToAddress1,
                BillToAddress2 = site.BillToAddress2,
                BillToAddress3 = site.BillToAddress3,
                BillToComment = site.BillToComment,
                BillToCity = site.BillToAddressCity,
                BillToState = site.BillToAddressState,
                BillToPostalCode = site.BillToAddressPostCode,
                BillToCountry = site.BillToAddressCountry,
                #endregion
                #region  Purchasing
                AutoPurchPrefix = site.AutoPurch_Prefix,
                NonStockDefaultAccount = site.NonStockAccountId,
                NoStockDefaultAccountList = NonStockList != null ? NonStockList.Select(x => new SelectListItem { Text = x.ClientLookupId + " | " + x.Name, Value = x.AccountId.ToString() }) : new SelectList(new[] { "" }),
                Isincludeinautopurchasing = site.AutoPurch,
                PRPrefix = customerList != null ? customerList.Where(x => x.Key == AutoGenerateKey.PR_ANNUAL).Select(x => x.Prefix).FirstOrDefault() : null,
                POPrefix = customerList != null ? customerList.Where(x => x.Key == AutoGenerateKey.PO_ANNUAL).Select(x => x.Prefix).FirstOrDefault() : null,
                #region V2-820
                IsIncludePRPreview = site.IncludePRReview,
                IsShoppingCartIncludeBuyer = site.ShoppingCartIncludeBuyer,
                ShoppingCartReviewDefault=site.ShoppingCartReviewDefault,
                DefaultBuyer = site.DefaultBuyer,
                #endregion
                #endregion
                #region V2-894
                IsOnOrderCheck = site.OnOrderCheck,
                #endregion

                #region Ship To
                ShipToName = site.ShipToName,
                ShipToAddress1 = site.Address1,
                ShipToAddress2 = site.Address2,
                ShipToAddress3 = site.Address3,
                ShipToCity = site.AddressCity,
                ShipToState = site.AddressState,
                ShipToPostalCode = site.AddressPostCode,
                ShipToCountry = site.AddressCountry,
                #endregion
                #region AssetGroup
                AssetGroup1Name = !string.IsNullOrEmpty(site.AssetGroup1Name) ? site.AssetGroup1Name : AssetGroupConstants.AssetGroup1,
                AssetGroup2Name = !string.IsNullOrEmpty(site.AssetGroup2Name) ? site.AssetGroup2Name : AssetGroupConstants.AssetGroup2,
                AssetGroup3Name = !string.IsNullOrEmpty(site.AssetGroup3Name) ? site.AssetGroup3Name : AssetGroupConstants.AssetGroup3,
                #endregion

                #region Maintenance
                //V2-676
                WOBarCode = site.WOBarcode,
                WOBarCodeList = WOBarCodeList != null ? WOBarCodeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }) : new SelectList(new[] { "" }),
                #endregion
                #region V2-948
                SourceAssetAccount = site.SourceAssetAccount,
                #endregion
                #region V2-1032
                SingleStockLineItem = site.SingleStockLineItem,
                #endregion
                #region V2-1061
                InvoiceVarianceCheck = site.InvoiceVarianceCheck,
                InvoiceVariance = site.InvoiceVariance
                #endregion
            };
            #region V2-820
            var defaultreviewers = new List<SelectListItem>();
            var defaultbuyers = new List<SelectListItem>();
            var dataModels = Get_PRApprovedPersonnelListBy();
            defaultreviewers = dataModels.Select(x => new SelectListItem
            {
                Text = x.NameFirst + " " + x.NameLast,
                Value = x.AssignedTo_PersonnelId.ToString()
            }).ToList();
            var defaultbuyerdataModels = Get_PRApprovedPersonnelListBy(true);
            defaultbuyers = defaultbuyerdataModels.Select(x => new SelectListItem
            {
                Text = x.NameFirst + " " + x.NameLast,
                Value = x.AssignedTo_PersonnelId.ToString()
            }).ToList();
            siteModel.DefaultReviewerList = defaultreviewers;
            siteModel.DefaultBuyerList=defaultbuyers;
            #endregion
            siteModel.approvalGroupSettingsModel = approvalGroupSettingsModel;//V2-720
            siteModel.PackageLevel = this.userData.DatabaseKey.Client.PackageLevel;
            siteModel.security=this.userData.Security;
            LocalizeControls(siteModel, LocalizeResourceSetConstants.SetUpDetails);
            return View("~/Views/Configuration/SiteSetUp/index.cshtml", siteModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSiteSetup(SiteSetUpModel objM)
        {
            if (ModelState.IsValid)
            {
                SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = sWrapper.UpdateSiteSetup(objM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #region Upload/Delete Photo
        [HttpPost]
        public ActionResult UploadNewImageToAzure(string fileName)
        {
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
            AzureSetup aset = new AzureSetup();
            var originalDirectory = new System.IO.DirectoryInfo(string.Format("{0}UploadedImages\\images\\{1}", Server.MapPath(@"\write\"), userData.SessionId.ToString()));
            System.IO.FileInfo[] files = originalDirectory.GetFiles();
            foreach (var file in files)
            {
                if (file.Name == fileName)
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(file.FullName);
                    string rtrData = GetAzureUrl(bytes);
                    bool Issuccess = sWrapper.UpdatePhoto(rtrData);
                    if (Issuccess)
                    {
                        string sasToken = aset.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, rtrData);
                        return Json(new { imageurl = sasToken }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { imageurl = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { imageurl = "" }, JsonRequestBehavior.AllowGet);
        }
        private string GetAzureUrl(byte[] uploadedFile)
        {
            string content_type = string.Empty;
            string rtrData = string.Empty;
            AzureBlob ablob = new AzureBlob();
            AzureSetup aset = new AzureSetup();
            if (uploadedFile.Length > 1)
            {

                Int64 Clientid = userData.DatabaseKey.Client.ClientId;
                Int64 Siteid = userData.DatabaseKey.User.DefaultSiteId;
                Int64 EquipId = userData.DatabaseKey.Personnel.SiteId;// - 1;//userData.DatabaseKey.Personnel.SiteId;
                string imgName = "Site" + "_" + DateTime.Now.Ticks.ToString() + "." + "jpg";
                string fileName = aset.CreateFileNamebyObject("Site", EquipId.ToString(), imgName);
                content_type = MimeMapping.GetMimeMapping(fileName);
                rtrData = aset.UploadToAzureBlob(Clientid, Siteid, fileName, uploadedFile, content_type);
            }
            return rtrData;
        }
        [HttpPost]
        public ActionResult DeleteImageFromAzure()
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            // RKL - In-Premise - Please review 
            // RKL - 2022-May-11 - Check Client.OnPremise (Is this necessary - is this method called if using on-premise)
            //bool lOnPremise = (Convert.ToBoolean(string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["ClientOnPremise"]) ? "false" : System.Configuration.ConfigurationManager.AppSettings["ClientOnPremise"].ToString().Trim().ToLower()));
            if (userData.DatabaseKey.Client.OnPremise == true)
            {
              comWrapper.DeleteOnPremiseImage(userData.DatabaseKey.User.DefaultSiteId, AttachmentTableConstant.Site, ref isSuccess);
            }
            else
            {
              comWrapper.DeleteAzureImage(userData.DatabaseKey.User.DefaultSiteId, AttachmentTableConstant.Site, ref isSuccess);
            }
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteImageFromOnPremise()
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteOnPremiseImage(userData.DatabaseKey.User.DefaultSiteId, AttachmentTableConstant.Site, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AssetGroup1

        public string PopulateAssetGroup1(int? draw, int? start, int? length)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
            List<AssetGroup1Model> AssetGroup1List = sWrapper.PopulateAssetGroup1();
            AssetGroup1List = this.GetAllAssetGroup1SortByColumnWithOrder(order, orderDir, AssetGroup1List);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = AssetGroup1List.Count();
            totalRecords = AssetGroup1List.Count();
            int initialPage = start.Value;
            var filteredResult = AssetGroup1List
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult}, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<AssetGroup1Model> GetAllAssetGroup1SortByColumnWithOrder(string order, string orderDir, List<AssetGroup1Model> data)
        {
            List<AssetGroup1Model> lst = new List<AssetGroup1Model>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.InactiveFlag).ToList() : data.OrderBy(p => p.InactiveFlag).ToList();
                    break;
            }
            return lst;
        }

        public PartialViewResult AddAssetGroup1(string AssetGroupLabel)
        {
            SiteSetUpVM objSiteSetUpVM = new SiteSetUpVM();
            AssetGroup1Model objAssetGroup1Model = new AssetGroup1Model();

            var StatusList = UtilityFunction.InactiveStatusTypesWithBoolValue();
            if (StatusList != null)
            {
                objAssetGroup1Model.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objAssetGroup1Model.AssetGroupLabel = AssetGroupLabel;
            objSiteSetUpVM.assetGroup1Model = objAssetGroup1Model;
            LocalizeControls(objSiteSetUpVM, LocalizeResourceSetConstants.SetUpDetails);
            return PartialView("~/Views/Configuration/SiteSetUp/_AddOrEditAssetGroup1.cshtml", objSiteSetUpVM);
        }
        public PartialViewResult EditAssetGroup1(long AssetGroup1Id,string AssetGroupLabel)
        {
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
            SiteSetUpVM objSiteSetUpVM = new SiteSetUpVM();
            AssetGroup1Model objAssetGroup1Model = new AssetGroup1Model();
            objAssetGroup1Model = sWrapper.GetAssetGroup1ByAssetGroup1Id(AssetGroup1Id);
            var StatusList = UtilityFunction.InactiveStatusTypesWithBoolValue();
            if (StatusList != null)
            {
                objAssetGroup1Model.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objAssetGroup1Model.AssetGroupLabel = AssetGroupLabel;
            objSiteSetUpVM.assetGroup1Model = objAssetGroup1Model;
            LocalizeControls(objSiteSetUpVM, LocalizeResourceSetConstants.SetUpDetails);
            return PartialView("~/Views/Configuration/SiteSetUp/_AddOrEditAssetGroup1.cshtml", objSiteSetUpVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditAssetGroup1(SiteSetUpVM objVM)
        {
            if (ModelState.IsValid)
            {
                SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = sWrapper.AddOrEditAssetGroup1(objVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        #region Delete
        [HttpPost]
        public JsonResult DeleteAssetGroup1(long AssetGroup1Id)
        {
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);

            var delResult = sWrapper.DeleteAssetGroup1(AssetGroup1Id);
            if (delResult != null && delResult.ErrorMessages != null && delResult.ErrorMessages.Count > 0)
            {
                return Json(delResult.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
        #region AssetGroup2

        public string PopulateAssetGroup2(int? draw, int? start, int? length)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
            List<AssetGroup2Model> AssetGroup2List = sWrapper.PopulateAssetGroup2();
            AssetGroup2List = this.GetAllAssetGroup2SortByColumnWithOrder(order, orderDir, AssetGroup2List);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = AssetGroup2List.Count();
            totalRecords = AssetGroup2List.Count();
            int initialPage = start.Value;
            var filteredResult = AssetGroup2List
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult}, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<AssetGroup2Model> GetAllAssetGroup2SortByColumnWithOrder(string order, string orderDir, List<AssetGroup2Model> data)
        {
            List<AssetGroup2Model> lst = new List<AssetGroup2Model>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.InactiveFlag).ToList() : data.OrderBy(p => p.InactiveFlag).ToList();
                    break;
            }
            return lst;
        }

        public PartialViewResult AddAssetGroup2(string AssetGroupLabel)
        {
            SiteSetUpVM objSiteSetUpVM = new SiteSetUpVM();
            AssetGroup2Model objAssetGroup2Model = new AssetGroup2Model();

            var StatusList = UtilityFunction.InactiveStatusTypesWithBoolValue();
            if (StatusList != null)
            {
                objAssetGroup2Model.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objAssetGroup2Model.AssetGroupLabel = AssetGroupLabel;
            objSiteSetUpVM.assetGroup2Model = objAssetGroup2Model;
            LocalizeControls(objSiteSetUpVM, LocalizeResourceSetConstants.SetUpDetails);
            return PartialView("~/Views/Configuration/SiteSetUp/_AddOrEditAssetGroup2.cshtml", objSiteSetUpVM);
        }
        public PartialViewResult EditAssetGroup2(long AssetGroup2Id, string AssetGroupLabel)
        {
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
            SiteSetUpVM objSiteSetUpVM = new SiteSetUpVM();
            AssetGroup2Model objAssetGroup2Model = new AssetGroup2Model();
            objAssetGroup2Model = sWrapper.GetAssetGroup2ByAssetGroup2Id(AssetGroup2Id);
            var StatusList = UtilityFunction.InactiveStatusTypesWithBoolValue();
            if (StatusList != null)
            {
                objAssetGroup2Model.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objAssetGroup2Model.AssetGroupLabel = AssetGroupLabel;
            objSiteSetUpVM.assetGroup2Model = objAssetGroup2Model;
            LocalizeControls(objSiteSetUpVM, LocalizeResourceSetConstants.SetUpDetails);
            return PartialView("~/Views/Configuration/SiteSetUp/_AddOrEditAssetGroup2.cshtml", objSiteSetUpVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditAssetGroup2(SiteSetUpVM objVM)
        {
            if (ModelState.IsValid)
            {
                SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = sWrapper.AddOrEditAssetGroup2(objVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        #region Delete
        [HttpPost]
        public JsonResult DeleteAssetGroup2(long AssetGroup2Id)
        {
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);

            var delResult = sWrapper.DeleteAssetGroup2(AssetGroup2Id);
            if (delResult != null && delResult.ErrorMessages != null && delResult.ErrorMessages.Count > 0)
            {
                return Json(delResult.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region AssetGroup3

        public string PopulateAssetGroup3(int? draw, int? start, int? length)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
            List<AssetGroup3Model> AssetGroup3List = sWrapper.PopulateAssetGroup3();
            AssetGroup3List = this.GetAllAssetGroup3SortByColumnWithOrder(order, orderDir, AssetGroup3List);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = AssetGroup3List.Count();
            totalRecords = AssetGroup3List.Count();
            int initialPage = start.Value;
            var filteredResult = AssetGroup3List
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult}, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<AssetGroup3Model> GetAllAssetGroup3SortByColumnWithOrder(string order, string orderDir, List<AssetGroup3Model> data)
        {
            List<AssetGroup3Model> lst = new List<AssetGroup3Model>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.InactiveFlag).ToList() : data.OrderBy(p => p.InactiveFlag).ToList();
                    break;
            }
            return lst;
        }

        public PartialViewResult AddAssetGroup3(string AssetGroupLabel)
        {
            SiteSetUpVM objSiteSetUpVM = new SiteSetUpVM();
            AssetGroup3Model objAssetGroup3Model = new AssetGroup3Model();

            var StatusList = UtilityFunction.InactiveStatusTypesWithBoolValue();
            if (StatusList != null)
            {
                objAssetGroup3Model.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objAssetGroup3Model.AssetGroupLabel = AssetGroupLabel;
            objSiteSetUpVM.assetGroup3Model = objAssetGroup3Model;
            LocalizeControls(objSiteSetUpVM, LocalizeResourceSetConstants.SetUpDetails);
            return PartialView("~/Views/Configuration/SiteSetUp/_AddOrEditAssetGroup3.cshtml", objSiteSetUpVM);
        }
        public PartialViewResult EditAssetGroup3(long AssetGroup3Id, string AssetGroupLabel)
        {
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
            SiteSetUpVM objSiteSetUpVM = new SiteSetUpVM();
            AssetGroup3Model objAssetGroup3Model = new AssetGroup3Model();
            objAssetGroup3Model = sWrapper.GetAssetGroup3ByAssetGroup3Id(AssetGroup3Id);
            var StatusList = UtilityFunction.InactiveStatusTypesWithBoolValue();
            if (StatusList != null)
            {
                objAssetGroup3Model.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objAssetGroup3Model.AssetGroupLabel = AssetGroupLabel;
            objSiteSetUpVM.assetGroup3Model = objAssetGroup3Model;
            LocalizeControls(objSiteSetUpVM, LocalizeResourceSetConstants.SetUpDetails);
            return PartialView("~/Views/Configuration/SiteSetUp/_AddOrEditAssetGroup3.cshtml", objSiteSetUpVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditAssetGroup3(SiteSetUpVM objVM)
        {
            if (ModelState.IsValid)
            {
                SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = sWrapper.AddOrEditAssetGroup3(objVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }


        #region Delete
        [HttpPost]
        public JsonResult DeleteAssetGroup3(long AssetGroup3Id)
        {
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);

            var delResult = sWrapper.DeleteAssetGroup3(AssetGroup3Id);
            if (delResult != null && delResult.ErrorMessages != null && delResult.ErrorMessages.Count > 0)
            {
                return Json(delResult.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region V2-820
        public List<DataModel> Get_PRApprovedPersonnelListBy(bool buyer =false)
        {
            LookUpListModel model = new LookUpListModel();
            Personnel personnel = new Personnel()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId,
                Buyer = buyer
            };

            List<Personnel> PersonnelList = personnel.RetrieveForLookupListByMultipleSecurityItem(this.userData.DatabaseKey);

            return model.data = ReturnPRPersonnelList(PersonnelList);
        }
        private List<DataModel> ReturnPRPersonnelList(List<Personnel> PersonnelList)
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            foreach (var p in PersonnelList)
            {
                dModel = new DataModel();

                dModel.AssignedTo_PersonnelId = p.PersonnelId;
                dModel.AssignedTo_PersonnelClientLookupId = p.ClientLookupId;
                dModel.NameFirst = p.NameFirst;
                dModel.NameLast = p.NameLast;
                dModel.Buyer = p.Buyer;
                model.data.Add(dModel);
            }
            return model.data;
        }
        #endregion

    }
}