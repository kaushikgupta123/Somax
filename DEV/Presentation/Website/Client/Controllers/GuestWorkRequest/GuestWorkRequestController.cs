using Business.Authentication;
using Client.Common;
using Client.Models.GuestWorkRequest;
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
using Client.BusinessWrapper.Work_Order;
using DevExpress.Web.Mvc;

namespace Client.Controllers
{
    public class GuestWorkRequestController : Controller
    {
        internal DataContracts.UserData userData { get; set; }
        public ActionResult Index()
        {
            var guestWorkRequestVM = new GuestWorkRequestVM();
            GuestWorkRequestModel guestWorkRequestModel = new GuestWorkRequestModel();
            string url = Request.QueryString["url"];
            if (!string.IsNullOrEmpty(url))
            {
                if(VerifyAuthentication(url))
                {
                    userData = (DataContracts.UserData)Session["userData"];
                    CommonWrapper commonWrapper = new CommonWrapper(userData);
                    List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
                    List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
                    AllLookUps = commonWrapper.GetAllLookUpList();
                    if (AllLookUps != null)
                    {
                        Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE || x.ListName == LookupListConstants.Preventive_Maint_WO_Type || x.ListName == LookupListConstants.UP_WO_TYPE || x.ListName == LookupListConstants.WR_WO_TYPE).ToList();
                        if (Type != null)
                        {
                            var tmpTypeList = Type.GroupBy(x => x.ListValue + " - " + x.Description).Select(x => x.FirstOrDefault()).ToList();
                            guestWorkRequestModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                        }
                    }
                }
            }
            guestWorkRequestVM.GuestWorkRequestModel = guestWorkRequestModel;
            LocalizeControls(guestWorkRequestVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return View(guestWorkRequestVM);
        }
        private bool VerifyAuthentication(string encUrl)
        {
            var url = encUrl.Decrypt();
            string ClientID = url.Split(new char[] { '&' }).GetValue(0).ToString().Split(new char[] { '=' }).GetValue(1).ToString();
            string SiteID = url.Split(new char[] { '&' }).GetValue(1).ToString().Split(new char[] { '=' }).GetValue(1).ToString();
            string GuestUsername = "guest_user_" + ClientID + "_" + SiteID;
            string GuestPassword = GuestUsername;
            UserInfoDetails _objUser = new UserInfoDetails
            {
                UserName = GuestUsername,
                Password = GuestPassword
            };
            Authentication auth = new Authentication()
            {
                UserName = _objUser.UserName.Trim(),
                Password = _objUser.Password,
                website = WebSiteEnum.Client,
                BrowserInfo = System.Web.HttpContext.Current.Request.Browser.Type + " " + System.Web.HttpContext.Current.Request.Browser.Version,
                IpAddress = System.Web.HttpContext.Current.Request.UserHostAddress
            };

            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            auth.VerifyLogin();
            if (auth.IsAuthenticated)
            {
                // store the newly created session id into cookie
                Presentation.Common.Cookie.Set(CookiesConstants.SOMAX_USER, auth.SessionId.ToString());

                // store the newly created session id into session
                UserSession.SessionId = auth.SessionId;
                auth.UserData = new DataContracts.UserData() { SessionId = auth.SessionId, WebSite = WebSiteEnum.Client };
                auth.UserData.Retrieve(dbKey);
                System.Web.HttpContext.Current.Session["userData"] = auth.UserData;
            }
            return auth.IsAuthenticated;
        }
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
        public ActionResult LoadCaptcha()
        {
            return PartialView("_CaptchaPartial");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveWorkRequest(GuestWorkRequestVM model, string Command)
        {
            var SessionData = Session["userData"];
            if (SessionData != null)
            {
                userData = (DataContracts.UserData)Session["userData"];
            }
            if (!CaptchaExtension.GetIsValid("captcha"))
            {
                var captcha = CaptchaExtension.GetCode("captcha");
                return Json(JsonReturnEnum.failed.ToString(), JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                List<string> ErrorMsg = new List<string>();
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                WorkOrder returnObj = woWrapper.AddGuestWorkRequest(model.GuestWorkRequestModel, ref ErrorMsg);
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
    }

}