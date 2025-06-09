using Business.Authentication;
using Client.Common;
using Common.Constants;
using Common.Enumerations;
using Presentation.Common;
using SomaxMVC.Models;
using System.Web.Mvc;
using DataContracts;
using Client.BusinessWrapper.Common;
using System.Collections.Generic;
using System.Linq;
using Client.Models;
using System.Configuration;
using Utility;
using System;
using Client.Models.PartIssue;
using Client.BusinessWrapper.PartIssue;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Web;

using Client.Models.Common;
using Newtonsoft.Json;



namespace Client.Controllers
{
    public class PartIssueController : Controller
    {
        protected JsonSerializerSettings JsonSerializer12HoursDateAndTimeSettings { get; set; }
        internal DataContracts.UserData userData { get; set; }
        public ActionResult Index()
        {
            var partIssueVM = new PartIssueVM();
            PartIssueModel partIssueModel = new PartIssueModel();
            string url = Request.QueryString["url"];
            if (!string.IsNullOrEmpty(url))
            {
                if (VerifyAuthentication(url))
                {
                    userData = (DataContracts.UserData)Session["userData"];
                    partIssueVM.userData = userData;
                    CommonWrapper commonWrapper = new CommonWrapper(userData);
                    var userAgent = Request.UserAgent.ToLower();
                    var isMobile = Request.Browser.IsMobileDevice ||
                                   userAgent.Contains("iphone") ||
                                   userAgent.Contains("android") ||
                                   userAgent.Contains("blackberry") ||
                                   userAgent.Contains("opera mini") ||
                                   userAgent.Contains("windows ce") ||
                                   userAgent.Contains("palm");

                    partIssueVM.IsMobile = isMobile;
                }
            }
            LocalizeControls(partIssueVM, LocalizeResourceSetConstants.PartDetails);
            return View(partIssueVM);
        }
        private bool VerifyAuthentication(string encUrl)
        {
            var url = encUrl.Decrypt();
            string ClientID = url.Split(new char[] { '&' }).GetValue(0).ToString().Split(new char[] { '=' }).GetValue(1).ToString();
            string SiteID = url.Split(new char[] { '&' }).GetValue(1).ToString().Split(new char[] { '=' }).GetValue(1).ToString();
            string LoginInfoID = url.Split(new char[] { '&' }).GetValue(2).ToString().Split(new char[] { '=' }).GetValue(1).ToString();
            Authentication auth = new Authentication()
            {
                website = WebSiteEnum.Client,
                BrowserInfo = System.Web.HttpContext.Current.Request.Browser.Type + " " + System.Web.HttpContext.Current.Request.Browser.Version,
                IpAddress = System.Web.HttpContext.Current.Request.UserHostAddress
            };

            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            auth.VerifyAuthenticateLogin(Convert.ToInt64(LoginInfoID));
            if (auth.IsAuthenticated)
            {
                // store the newly created session id into cookie
                Presentation.Common.Cookie.Set(CookiesConstants.SOMAX_USER, auth.SessionId.ToString());
                // store the newly created session id into session
                UserSession.SessionId = auth.SessionId;
                auth.UserData = new DataContracts.UserData() { SessionId = auth.SessionId, WebSite = WebSiteEnum.Client };
                auth.UserData.Retrieve(dbKey);
                IdentitySignin(auth.UserData, false);
                System.Web.HttpContext.Current.Session["userData"] = auth.UserData;
            }
            return auth.IsAuthenticated;
        }
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        public void IdentitySignin(UserData appUserState, bool isPersistent = false)
        {
            var claims = new List<Claim>();

            // create required claims
            claims.Add(new Claim(ClaimTypes.NameIdentifier, appUserState.DatabaseKey.Personnel.PersonnelId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, appUserState.DatabaseKey.UserName));

            // custom – my serialized AppUserState object
            claims.Add(new Claim("userState", appUserState.ToString()));

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties()
            {
                AllowRefresh = false,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddHours(10)
            }, identity);
        }

        [HttpGet]
        public JsonResult GetPartIdByClientLookUpId(string clientLookUpId)
        {
            userData = (DataContracts.UserData)Session["userData"];
            var commonWrapper = new CommonWrapper(userData);
            var part = commonWrapper.RetrievePartIdByClientLookUp(clientLookUpId);
            return Json(new { PartId = part.PartId }, JsonRequestBehavior.AllowGet);
        }
        #region return part
        [HttpPost]
        public PartialViewResult LoadReturnPart()
        {
            PartIssueReturnModel objPartIssueReturnModel = new PartIssueReturnModel();
            PartIssueVM objPartIssueVM = new PartIssueVM();
            userData = (DataContracts.UserData)Session["userData"];
            PartIssueWrapper prtissueWrapper = new PartIssueWrapper(userData);
            objPartIssueVM.userData = this.userData;

            objPartIssueReturnModel.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            objPartIssueReturnModel.ClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;

            #region ChargeType
            var ChargeTypeList = UtilityFunction.populateChargeTypeForInventoryCheckout();
            if (ChargeTypeList != null)
            {
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                {
                    ChargeTypeList = ChargeTypeList.Where(x => x.text != "Location").ToList();
                    objPartIssueReturnModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                else
                {
                    objPartIssueReturnModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                objPartIssueReturnModel.ChargeType = ChargeTypeList.Where(x => x.value == "WorkOrder").FirstOrDefault().value.ToString();
            }
            #endregion       
            objPartIssueVM.partIssueReturnModel = objPartIssueReturnModel;
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                objPartIssueReturnModel.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
                objPartIssueReturnModel.StoreroomId = userData.DatabaseKey.Personnel.Default_StoreroomId;
            }
            objPartIssueReturnModel.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            LocalizeControls(objPartIssueVM, LocalizeResourceSetConstants.PartDetails);
            ViewBag.tabtype = "ReturnFrom";
            return PartialView("~/Views/PartIssue/_ReturnPart.cshtml", objPartIssueVM);
        }

        public PartialViewResult LoadReturnPartMobile()
        {
            PartIssueReturnModel objPartIssueReturnModel = new PartIssueReturnModel();
            PartIssueVM objPartIssueVM = new PartIssueVM();
            userData = (DataContracts.UserData)Session["userData"];
            PartIssueWrapper prtissueWrapper = new PartIssueWrapper(userData);
            objPartIssueVM.userData = this.userData;

            objPartIssueReturnModel.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            objPartIssueReturnModel.ClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;

            #region ChargeType
            var ChargeTypeList = UtilityFunction.populateChargeTypeForInventoryCheckout();
            if (ChargeTypeList != null)
            {
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                {
                    ChargeTypeList = ChargeTypeList.Where(x => x.text != "Location").ToList();
                    objPartIssueReturnModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                else
                {
                    objPartIssueReturnModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                objPartIssueReturnModel.ChargeType = ChargeTypeList.Where(x => x.value == "WorkOrder").FirstOrDefault().value.ToString();
            }
            #endregion           
            objPartIssueVM.partIssueReturnModel = objPartIssueReturnModel;
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                objPartIssueReturnModel.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
                objPartIssueReturnModel.StoreroomId = userData.DatabaseKey.Personnel.Default_StoreroomId;
            }
            objPartIssueReturnModel.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            LocalizeControls(objPartIssueVM, LocalizeResourceSetConstants.PartDetails);
            ViewBag.tabtype = "ReturnFrom";
            return PartialView("~/Views/PartIssue/Mobile/_ReturnPart.cshtml", objPartIssueVM);
        }
        [ValidateAntiForgeryToken]
        public JsonResult ValiDateReturnControlls(PartIssueVM objPartIssueVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ReturnConfirmInventorydata(List<PartIssueReturnModel> dataList)
        {
            userData = (DataContracts.UserData)Session["userData"];
            PartIssueWrapper partIssueWrapper = new PartIssueWrapper(userData);
            var partHistoryListTemp = partIssueWrapper.PartReturnConfirmData(dataList);
            return Json(partHistoryListTemp, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Load Issue Parts
        [HttpPost]
        public PartialViewResult LoadIssueParts()
        {
            PartIssueModel objPartIssueModel = new PartIssueModel();
            PartIssueVM objPartIssueVM = new PartIssueVM();
            userData = (DataContracts.UserData)Session["userData"];
            PartIssueWrapper partissueWrapper = new PartIssueWrapper(userData);
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                objPartIssueModel.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
                objPartIssueModel.StoreroomId = userData.DatabaseKey.Personnel.Default_StoreroomId;
            }
            objPartIssueModel.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            objPartIssueVM.userData = this.userData;
            objPartIssueModel.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            objPartIssueModel.ClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;


            #region ChargeType
            var ChargeTypeList = UtilityFunction.populateChargeTypeForInventoryCheckout();
            if (ChargeTypeList != null)
            {
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                {
                    ChargeTypeList = ChargeTypeList.Where(x => x.text != "Location").ToList();
                    objPartIssueModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                else
                {
                    objPartIssueModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                objPartIssueModel.ChargeType = ChargeTypeList.Where(x => x.value == "WorkOrder").FirstOrDefault().value.ToString();
            }
            #endregion
            objPartIssueVM.partIssueModel = objPartIssueModel;
            LocalizeControls(objPartIssueVM, LocalizeResourceSetConstants.PartDetails);
            ViewBag.tabtype = "IssueTo";
            return PartialView("~/Views/PartIssue/_IssueParts.cshtml", objPartIssueVM);
        }

        public PartialViewResult LoadIssuePartsMobile()
        {
            PartIssueModel objPartIssueModel = new PartIssueModel();
            PartIssueVM objPartIssueVM = new PartIssueVM();
            userData = (DataContracts.UserData)Session["userData"];
            PartIssueWrapper partissueWrapper = new PartIssueWrapper(userData);
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                objPartIssueModel.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
                objPartIssueModel.StoreroomId = userData.DatabaseKey.Personnel.Default_StoreroomId;
            }
            objPartIssueModel.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            // 
            objPartIssueVM.userData = this.userData;
            objPartIssueModel.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            objPartIssueModel.ClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;


            #region ChargeType
            var ChargeTypeList = UtilityFunction.populateChargeTypeForInventoryCheckout();
            if (ChargeTypeList != null)
            {
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                {
                    ChargeTypeList = ChargeTypeList.Where(x => x.text != "Location").ToList();
                    objPartIssueModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                else
                {
                    objPartIssueModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                objPartIssueModel.ChargeType = ChargeTypeList.Where(x => x.value == "WorkOrder").FirstOrDefault().value.ToString();
            }
            #endregion
            objPartIssueVM.partIssueModel = objPartIssueModel;
            LocalizeControls(objPartIssueVM, LocalizeResourceSetConstants.PartDetails);
            ViewBag.tabtype = "IssueTo";
            return PartialView("~/Views/PartIssue/Mobile/_IssueParts.cshtml", objPartIssueVM);
        }

        [HttpPost]
        public ActionResult ConfirmInventorydata(List<PartIssueModel> dataList)
        {
            userData = (DataContracts.UserData)Session["userData"];
            PartIssueWrapper partIssueWrapper = new PartIssueWrapper(userData);
            var partHistoryListTemp = partIssueWrapper.ConfirmData(dataList);
            return Json(partHistoryListTemp, JsonRequestBehavior.AllowGet);
        }
        [ValidateAntiForgeryToken]
        public JsonResult ValiDateControlls(PartIssueVM objPartIssueVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PopulateIssuePartSelectTable(long _personnelId, decimal _TransactionQuantity, long _partId, long _chargeToId, string _chargeToClientLookupId, string _chargeType = "", string _comments = "", long _StoreroomId = 0, string _StoreroomName = "")
        {
            userData = (DataContracts.UserData)Session["userData"];

            PartIssueWrapper partIssueWrapper = new PartIssueWrapper(userData);
            var personnelList = partIssueWrapper.FillIssuTo();
            string issueToClientLookupId = string.Empty;
            string chargeToClientLookupId = string.Empty;
            if (personnelList != null)
            {
                issueToClientLookupId = personnelList.Where(x => x.PersonnelId == _personnelId).Select(a => a.ClientLookupId).FirstOrDefault().ToString();
            }

            chargeToClientLookupId = _chargeToClientLookupId;
            var partList = partIssueWrapper.populatePartDetails(_partId, _StoreroomId);
            long PartStoreroomId = partList.PartStoreroomId;
            PartIssueValidationModel objPartIssueValidationModell = new PartIssueValidationModel();

            objPartIssueValidationModell.partHistoryModel = partIssueWrapper.ValidateSelectedItems(issueToClientLookupId, PartStoreroomId, _chargeType, chargeToClientLookupId, _chargeToId, _TransactionQuantity, partList.ClientLookupId, _partId, partList.Description, partList.UPCCode, _comments);

            if (objPartIssueValidationModell != null && objPartIssueValidationModell.partHistoryModel.ErrorMessages.Count > 0)
            {
                objPartIssueValidationModell.IssueToClientLookupId = issueToClientLookupId;
                objPartIssueValidationModell.chargeToClientLookupId = _chargeToClientLookupId;
                objPartIssueValidationModell.objPart = partList;
                objPartIssueValidationModell.PartStoreroomId = PartStoreroomId;
                objPartIssueValidationModell.Comments = _comments;
                objPartIssueValidationModell.StoreroomId = _StoreroomId;
                objPartIssueValidationModell.StoreroomName = _StoreroomName;
                foreach (var v in objPartIssueValidationModell.partHistoryModel.ErrorMessages)
                {
                    objPartIssueValidationModell.ErrorMsg += v + ",";
                }
            }
            else
            {
                objPartIssueValidationModell.IssueToClientLookupId = issueToClientLookupId;
                objPartIssueValidationModell.chargeToClientLookupId = chargeToClientLookupId;
                objPartIssueValidationModell.objPart = partList;
                objPartIssueValidationModell.PartStoreroomId = PartStoreroomId;
                objPartIssueValidationModell.Comments = _comments;
                objPartIssueValidationModell.StoreroomId = _StoreroomId;
                objPartIssueValidationModell.StoreroomName = _StoreroomName;
                objPartIssueValidationModell.ErrorMsg = null;
            }
            return Json(objPartIssueValidationModell, JsonRequestBehavior.AllowGet);
        }
        #endregion     
        #region Localisations
        internal void LocalizeControls(LocalisationBaseVM objComb, string ResourceType)
        {
            LoginCacheSet _logCache = new LoginCacheSet();
            var connstring = ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString();
            //------------Retrieve by Specific Resource Type Localization--------------//

            List<Localizations> locSpecificPageCache = _logCache.GetLocalizationCommon(connstring, ResourceType, userData.Site.Localization);

            //------------Retrieve Global Localization--------------------//
            List<Localizations> locGlobalCache = _logCache.GetLocalizationCommon(connstring, LocalizeResourceSetConstants.Global, userData.Site.Localization);

            if (locSpecificPageCache != null && locSpecificPageCache.Count > 0)
            {
                objComb.Loc = locSpecificPageCache;
            }
            else
            {
                //------Retrieve "en-us" data if other lang fail----------//
                locSpecificPageCache = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), ResourceType, "en-us");
                objComb.Loc = locSpecificPageCache;
            }
            if (locGlobalCache != null && locGlobalCache.Count > 0)
            {
                objComb.Loc.AddRange(locGlobalCache);
            }
            else
            {
                //------Retrieve "en-us" data if other lang fail----------//
                locGlobalCache = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Global, "en-us");
                objComb.Loc.AddRange(locGlobalCache);
            }

        }
        public JsonResult GetDataTableLanguageJson(bool InnerGrid = false, bool nGrid = false)
        {
            DataTableLanguageRoot dtlanguageDetails = new DataTableLanguageRoot();
            string sLengthMenu = "<select class='searchdt-menu select2picker'>" +
                                              "<option value='10'>10</option>" +
                                              "<option value='20'>20</option>" +
                                              "<option value='30'>30</option>" +
                                              "<option value='40'>40</option>" +
                                              "<option value='50'>50</option>" +
                                              "</select>";

            dtlanguageDetails.sEmptyTable = UtilityFunction.GetMessageFromResource("sEmptyTable", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfo = UtilityFunction.GetMessageFromResource("sInfo", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfoEmpty = UtilityFunction.GetMessageFromResource("sInfoEmpty", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfoFiltered = UtilityFunction.GetMessageFromResource("sInfoFiltered", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfoPostFix = UtilityFunction.GetMessageFromResource("sInfoPostFix", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfoThousands = UtilityFunction.GetMessageFromResource("sInfoThousands", LocalizeResourceSetConstants.DataTableEntry);
            if (nGrid)
            {
                dtlanguageDetails.sLengthMenu = sLengthMenu;
            }
            else if (InnerGrid)
            {
                dtlanguageDetails.sLengthMenu = "<select class='innergrid-searchdt-menu form-control search'>" +
                                              "<option value='10'>10</option>" +
                                              "<option value='20'>20</option>" +
                                              "</select>";
            }
            else
            {
                dtlanguageDetails.sLengthMenu = "Page size :&nbsp;" + sLengthMenu;
            }

            dtlanguageDetails.sLoadingRecords = UtilityFunction.GetMessageFromResource("sLoadingRecords", LocalizeResourceSetConstants.DataTableEntry);

            dtlanguageDetails.sProcessing = "<img src='../Content/Images/image_1197421.gif'>";

            dtlanguageDetails.sSearch = UtilityFunction.GetMessageFromResource("sSearch", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sZeroRecords = UtilityFunction.GetMessageFromResource("sZeroRecords", LocalizeResourceSetConstants.DataTableEntry);
            if (InnerGrid || nGrid)
            {
                dtlanguageDetails.oPaginate.sFirst = "<img src='../images/drop-grid-first.png'>";
            }
            else
            {
                dtlanguageDetails.oPaginate.sFirst = UtilityFunction.GetMessageFromResource("sFirst", LocalizeResourceSetConstants.DataTableEntry);
            }
            if (InnerGrid || nGrid)
            {
                dtlanguageDetails.oPaginate.sPrevious = "<img src='../images/drop-grid-prev.png'>";
            }
            else
            {
                dtlanguageDetails.oPaginate.sPrevious = UtilityFunction.GetMessageFromResource("sPrevious", LocalizeResourceSetConstants.DataTableEntry);
            }

            if (InnerGrid || nGrid)
            {
                dtlanguageDetails.oPaginate.sNext = "<img src='../images/drop-grid-next.png'>";
            }
            else
            {
                dtlanguageDetails.oPaginate.sNext = UtilityFunction.GetMessageFromResource("sNext", LocalizeResourceSetConstants.DataTableEntry);
            }
            if (InnerGrid || nGrid)
            {
                dtlanguageDetails.oPaginate.sLast = "<img src='../images/drop-grid-last.png'>";
            }
            else
            {
                dtlanguageDetails.oPaginate.sLast = UtilityFunction.GetMessageFromResource("sLast", LocalizeResourceSetConstants.DataTableEntry);
            }

            dtlanguageDetails.oAria.sSortAscending = UtilityFunction.GetMessageFromResource("sSortAscending", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.oAria.sSortDescending = UtilityFunction.GetMessageFromResource("sSortDescending", LocalizeResourceSetConstants.DataTableEntry);

            return Json(dtlanguageDetails, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpGet]
        public JsonResult GetPersonnelIdByClientLookUpId(string clientLookUpId)
        {
            userData = (DataContracts.UserData)Session["userData"];
            var commonWrapper = new CommonWrapper(userData);
            var personnel = commonWrapper.RetrievePersonnelIdByClientLookUpId(clientLookUpId);
            return Json(new { PersonnelId = personnel.PersonnelId }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEquipmentIdByClientLookUpId(string clientLookUpId)
        {
            userData = (DataContracts.UserData)Session["userData"];
            var commonWrapper = new CommonWrapper(userData);
            Equipment equipment = commonWrapper.RetrieveEquipmentIdByClientLookUpId(clientLookUpId);
            return Json(new { EquipmentId = equipment.EquipmentId }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetWorkorderIdByClientLookUpId(string clientLookUpId)
        {
            userData = (DataContracts.UserData)Session["userData"];
            var commonWrapper = new CommonWrapper(userData);
            WorkOrder workorder = commonWrapper.RetrieveWorkOrderIdByClientLookUpId(clientLookUpId);
            return Json(new { WorkOrderId = workorder.WorkOrderId }, JsonRequestBehavior.AllowGet);
        }

        #region Mobile

        public JsonResult GetActiveAdminOrFullPersonnelLookupListGridData_Mobile(int Start, int Length, string Search = "")
        {
            var modelList = new List<PersonnelLookUpModel>();
            userData = (DataContracts.UserData)Session["userData"];
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0";
            string orderDir = "asc";

            modelList = commonWrapper.GetActiveAdminOrFullChunkSearchPersonnelLookupListGridData(order, orderDir, Start, Length, "", "", "", Search);
            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });

        }

        public string GetActiveAdminOrFullPersonnelLookupListGridData(int? draw, int? start, int? length, string ClientLookupId = "", string NameFirst = "", string NameLast = "", string SearchText = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            userData = (DataContracts.UserData)Session["userData"];
            CommonWrapper _VendorObj = new CommonWrapper(userData);
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            List<PersonnelLookUpModel> pList = _VendorObj.GetActiveAdminOrFullChunkSearchPersonnelLookupListGridData(order, orderDir, skip, length ?? 0, ClientLookupId, NameFirst, NameLast, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (pList != null && pList.Count > 0)
            {
                recordsFiltered = pList[0].TotalCount;
                totalRecords = pList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = pList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }
        #endregion

        #region Mobile
        public JsonResult GetPartLookupListchunksearch_Mobile(int Start, int Length, string Search = "", string Storeroomid = "")
        {
            var modelList = new List<PartXRefGridDataModel>();
            userData = (DataContracts.UserData)Session["userData"];
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = "0"; //Request.Form.GetValues("order[0][column]")[0];
            string orderDir = "asc"; //Request.Form.GetValues("order[0][dir]")[0];

            modelList = commonWrapper.GetPartLookupListGridData_Mobile(order, orderDir, Start, Length, Search, Search, Storeroomid);

            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }
        #endregion

        #region  WorkOrder Chunk LookupList 
        public string GetWorkOrderLookupListChunkSearchForPartsIssue(int? draw, int? start, int? length, string ClientLookupId = "", string Description = "", string ChargeTo = "", string WorkAssigned = "", string Requestor = "", string Status = "")
        {
            List<WorkOrderLookUpModel> modelList = new List<WorkOrderLookUpModel>();
            userData = (DataContracts.UserData)Session["userData"];
            PartIssueWrapper prtissueWrapper = new PartIssueWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            ChargeTo = !string.IsNullOrEmpty(ChargeTo) ? ChargeTo.Trim() : string.Empty;
            WorkAssigned = !string.IsNullOrEmpty(WorkAssigned) ? WorkAssigned.Trim() : string.Empty;
            Requestor = !string.IsNullOrEmpty(Requestor) ? Requestor.Trim() : string.Empty;
            Status = !string.IsNullOrEmpty(Status) ? Status.Trim() : string.Empty;

            modelList = prtissueWrapper.GetWorkOrderLookupListGridData(order, orderDir, skip, length.Value, ClientLookupId, Description, ChargeTo, WorkAssigned, Requestor, Status);
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList });
        }

        #endregion

        #region LookpList
        [HttpPost]
        public JsonResult GetEquipmentLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            var modelList = new List<EquipmentLookupModel>();
            userData = (DataContracts.UserData)Session["userData"];
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = "0";
            string orderDir = "asc";
            modelList = commonWrapper.GetEquipmentLookupListGridDataMobile(order, orderDir, Start, Length, Search, Search);

            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }
        #endregion
    }
}

