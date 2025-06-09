using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Common.Constants;
using Admin.BusinessWrapper;
using Admin.Models;
using System.Configuration;
using Utility;
using Presentation.Common;
using Admin.Common;
using DataContracts;
using Business.Authentication;
using ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginWrapper _loginWrapper;

        public LoginController(ILoginWrapper loginWrapper)
        {
            _loginWrapper = loginWrapper;
        }
        // GET: Login
        [AcceptVerbs("Get", "Post")]
        public ActionResult AdminLogin(string returnUrl = "", string Language = "en-us")
        {
            //----- IE browser
            string userAgent = Request.Headers["User-Agent"].ToString();
            if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
            {
                return View("~/Views/LogIn/_IsIEBrowser.cshtml");
            }
            //----- IE browser
            UserInfoDetails objUser = new UserInfoDetails();
            if (!string.IsNullOrEmpty(returnUrl))
            {
                objUser.ReturnUrl = returnUrl;
            }
            string title;
            LoginCacheSet _logCache = new LoginCacheSet();
            Session["Language"] = Language;
            if (Language != null)
            {
                var localizationCacheTest = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Login, Language);
                objUser.localization = localizationCacheTest;
            }
            string environ = ConfigurationManager.AppSettings[WebConfigConstants.CODE_DEPLOYMENT_ENVIRONMENT];
            if (environ.ToLower() == "prod")
                title = "SOMAX Admin | Login";
            else
                title = "SOMAX Admin Login - " + environ;
            ViewBag.Title = title;

            if (Request.Cookies[CookiesConstants.SOMAX_ADMIN_LOGIN_ID] != null)
            {
                objUser.UserName = Request.Cookies[CookiesConstants.SOMAX_ADMIN_LOGIN_ID].Value;
                objUser.RememberMe = true;
            }

            bool isMaintenece = true;
            string sameDay = string.Empty;
            if (isMaintenece)
            {
                SiteMaintenanceWrapper objSiteMainWrapper = new SiteMaintenanceWrapper();
                var mydata = objSiteMainWrapper.GetNextSitemaintenancewithoutLogin("y");
                string maintenanceMessage = GetSiteMaintenanceMessage(mydata, objSiteMainWrapper);

                var timeUtc = DateTime.UtcNow;
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
                DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                TimeSpan DowntimeStarttime = mydata.DowntimeStart.Date.TimeOfDay;
                TimeSpan currenttime = easternTime.TimeOfDay;
                TimeSpan DowntimeEndtime = mydata.DowntimeEnd.Date.TimeOfDay;

                var starttimeUtc = mydata.DowntimeStart;
                DateTime starteasternTime = TimeZoneInfo.ConvertTimeFromUtc(starttimeUtc, easternZone);
                TimeSpan starttime = starteasternTime.TimeOfDay;
                DateTime endeasternTime = TimeZoneInfo.ConvertTimeFromUtc(mydata.DowntimeEnd, easternZone);
                TimeSpan endtime = endeasternTime.TimeOfDay;

                if (easternTime >= starteasternTime && easternTime <= endeasternTime)
                {
                    SiteMaintenanceModel siteMaintenanceModel = new SiteMaintenanceModel
                    {
                        CallerUserInfoId = mydata.CallerUserInfoId,
                        CallerUserName = mydata.CallerUserName,
                        SiteMaintenanceId = mydata.SiteMaintenanceId,
                        HeaderText = mydata.HeaderText,
                        MessageText = mydata.MessageText,
                        DowntimeStart = mydata.DowntimeStart,
                        DowntimeEnd = mydata.DowntimeEnd,
                        LoginPageMessage = mydata.LoginPageMessage,
                        DashboardMessage = mydata.DashboardMessage,
                        UpdateIndex = mydata.UpdateIndex,
                        EasternStartTime = mydata.EasternStartTime,
                        EasternEndTime = mydata.EasternEndTime
                    };
                    ViewBag.MaintainenceMsg = maintenanceMessage;
                    return View("UnderConstruction", siteMaintenanceModel);
                }

                ViewBag.SystemUnderMaintenanceMessage = maintenanceMessage;
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/LogIn/_LoginPartial.cshtml", objUser);
            }
            return View(objUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(UserInfoDetails objUser)
        {
            VMLogin LogIn = new VMLogin();
            if (ModelState.IsValid)
            {
                //LoginWrapper LoginWrapper = new LoginWrapper(objUser); need to ask setting the value
                //if (objUser.RememberMe)
                //{
                //    Response.Cookies[CookiesConstants.SOMAX_ADMIN_LOGIN_ID].Expires = DateTime.Now.AddDays(30);
                //    Response.Cookies[CookiesConstants.SOMAX_ADMIN_LOGIN_ID].Value = objUser.UserName;
                //}
                //else
                //{
                //    Response.Cookies[CookiesConstants.SOMAX_ADMIN_LOGIN_ID].Expires = DateTime.Now.AddDays(-1);
                //    Response.Cookies[CookiesConstants.SOMAX_ADMIN_LOGIN_ID].Value = null;
                //}
                LogIn = _loginWrapper.GetLogIn(objUser);

                if (LogIn.IsAuthenticated)
                {
                    //string serializeModel = JsonConvert.SerializeObject(LogIn.UserInfoDetails);
                    //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, LogIn.UserInfoDetails.UserName, DateTime.Now, DateTime.Now.AddMinutes(20), objUser.RememberMe, serializeModel);
                    //string encTicket = FormsAuthentication.Encrypt(ticket);
                    //HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    //Response.Cookies.Add(faCookie);

                    Session[SessionConstants.LOGGED_USERNAME] = objUser.UserName;
                    Session[SessionConstants.LOGGED_PASSWORD] = objUser.Password;

                    IdentitySignin(LogIn.UserInfoDetails, objUser.RememberMe);

                    //-----Call getmenu list here and keep the list in session or temp data which one is better----//
                    var menuReturnList = GetMenuList("Admin");
                    if (LogIn.UIVersionRedirectToV1 == true)
                    {
                        string remoteUrl = ConfigurationManager.AppSettings["V1RedirectURL"].ToString();
                        string encuname = WebSecurity.Encrypt(LogIn.UserInfoDetails.UserName, "somaxsom");
                        string encpwd = WebSecurity.Encrypt(LogIn.UserInfoDetails.Password, "somaxsom");

                        String htmlUname = encuname;
                        String htmlPwd = encpwd;
                        StringWriter writerU = new StringWriter();
                        StringWriter writerP = new StringWriter();
                        Server.UrlEncode(htmlUname, writerU);
                        Server.UrlEncode(htmlPwd, writerP);
                        String uString = writerU.ToString();
                        String pString = writerP.ToString();
                        Response.Redirect(remoteUrl + "uName=" + uString + "&pwd=" + pString, false);
                        //Response.Redirect(remoteUrl + "uName=" + encuname + "&pwd=" + encpwd);
                    }
                    else
                    {
                        if (LogIn.IsMatchTempPassword) /*V2-332*/
                        {
                            return RedirectToAction("CreatePassword", new { userId = LogIn.UserInfoDetails.UserName.Encrypt(), IsResetPassword = LogIn.IsResetPassword, email = LogIn.UserInfoDetails.UserEmail.Encrypt() });
                        }
                        if (!(menuReturnList.Count == 0 || menuReturnList.Count < 0))
                        {
                            Session["AdminMenuDetails"] = menuReturnList;
                            return RedirectToLocal(objUser.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("AdminLogin");
                        }
                    }
                }
                else
                {
                    SiteMaintenanceWrapper objSiteMainWrapper = new SiteMaintenanceWrapper();
                    var mydata = objSiteMainWrapper.GetNextSitemaintenancewithoutLogin("y");
                    ViewBag.SystemUnderMaintenanceMessage = GetSiteMaintenanceMessage(mydata, objSiteMainWrapper);
                }
            }
            LoginCacheSet _logCache = new LoginCacheSet();
            objUser.FailureMessage = LogIn.UserInfoDetails.FailureMessage;
            objUser.FailureReason = LogIn.UserInfoDetails.FailureReason;
            var localizationCacheTest = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Login, "en-us");
            objUser.localization = localizationCacheTest;
            return View("AdminLogIn", objUser);
        }
        public static IList<MenuVM> GetMenuList(string _menuType)
        {
            var data = System.Web.HttpContext.Current.Session["AdminUserData"];
            UserData userData = new UserData();
            userData = (UserData)data;
            string dbLoc = string.IsNullOrEmpty(userData.Site.Localization) ? "en-us" : userData.Site.Localization;
            // rkl -2019-07-03 - temp
            if (dbLoc != "en-us" && dbLoc != "fr-fr")
            {
                dbLoc = "en-us";
            }

            Menu mainmenu = new Menu()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                CallerUserName = userData.DatabaseKey.UserName,
                ResourceSet = "Menu",
                LocaleId = dbLoc,
                MenuType = _menuType,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                UserInfoId = userData.DatabaseKey.User.UserInfoId
            };
            var menuData = mainmenu.RetrieveAllCustomAdmin(userData.DatabaseKey);
            //menuData = GetMenuListBySecurity(_menuType, userData, menuData);
            //bool IsApmOnly = (userData.Site.APM == true && userData.Site.CMMS == false && userData.Site.Sanitation == false);
            //bool IsSanitationOnly = (userData.Site.APM == false && userData.Site.CMMS == false && userData.Site.Sanitation == true);
            //if (IsApmOnly)
            //{
            //    SetAsAPMOnly(menuData);
            //}

            //if (IsSanitationOnly)
            //{
            //    SetAsSanitationOnly(menuData);
            //}
            //--V2-408--//
            //#region V2-408 //-- Fleet Only Access

            //bool IsFleetOnly = (userData.Site.Fleet == true && userData.Site.APM == false && userData.Site.CMMS == false && userData.Site.Sanitation == false);
            //bool IsFleetAndCMMS = (userData.Site.Fleet == true && userData.Site.APM == false && userData.Site.CMMS == true && userData.Site.Sanitation == false);

            //if (IsFleetOnly)
            //{
            //    Int64 ServiceId = CreateMenu(ref menuData);
            //    SetAsFleetOnly(menuData, ServiceId);
            //}
            //if (IsFleetAndCMMS)
            //{
            //    SetAsFleetAndCMMS(menuData);
            //}
            //#endregion
            //--END----V2-408--//
            /*V2-962 Remove the Menu Support Tickets  -- menuData.Where(k => k.MenuId != 149)*/
            List<MenuVM> menu = menuData.Where(k => k.MenuId != 149).Where(x => menuData.Any(y => (x.ParentMenuId == 0 || y.MenuId == x.ParentMenuId)))
                                                 .Select(ml =>
                                                         new MenuVM
                                                         {
                                                             ClientId = ml.ClientId,
                                                             MenuId = ml.MenuId,
                                                             SecurityItemId = ml.SecurityItemId,
                                                             UserInfoId = ml.UserInfoId,
                                                             MenuName = ml.MenuName,
                                                             ParentMenuId = ml.ParentMenuId,
                                                             MenuLevel = ml.MenuLevel,
                                                             MenuUrl = ml.MenuUrl,
                                                             MenuPosition = ml.MenuPosition,
                                                             ToolTip = ml.ToolTip,//0
                                                             CssClass = ml.CssClass,
                                                             itemAccess = ml.ItemAccess,
                                                             LocalizedName = ml.LocalizedName,
                                                             LocaleId = ml.LocaleId,
                                                             MenuType = ml.MenuType
                                                         }).Where(x => x.itemAccess == true).OrderBy(b => b.MenuPosition).OrderBy(a => a.ParentMenuId).ToList();
            //Children under Menu--//
            Dictionary<long, MenuVM> dict = menu.ToDictionary(loc => loc.MenuId);
            foreach (MenuVM loc in dict.Values)
            {
                if (loc != null && loc.ParentMenuId != 0 && loc.ParentMenuId != loc.MenuId && dict.ContainsKey(loc.ParentMenuId))
                {
                    MenuVM parent = dict[loc.ParentMenuId];
                    parent.Childlist.Add(loc);
                }
            }
            //---Subchild listing---//
            List<MenuVM> root = dict.Values.Where(loc => loc.ParentMenuId == 0).ToList();
            foreach (var r in root)
            {
                SetLevel(r, 0);
            }
            //IList<MenuVM> menu=new List<MenuVM>();
            return menu;
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                //return RedirectToAction("Dashboard", "Dashboard");
                return Redirect("/Admin/Dashboard/Dashboard");
            }
        }
        public ActionResult ForgetPasswordPatial()
        {
            UserInfoDetails objUser = new UserInfoDetails();
            LoginCacheSet _logCache = new LoginCacheSet();

            var localizationCacheTest = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Login, "en-us");
            objUser.localization = localizationCacheTest;
            return PartialView("~/Views/Login/_ForgetPasswordPartial.cshtml", objUser);
        }
        [HttpPost]
        public ActionResult ForgotPasswordSendEMail(string email = "", string user = "")
        {
            UserInfoDetails objUserInfoDetails = new UserInfoDetails();
            objUserInfoDetails.UserEmail = email;
            var result = false;
            //LoginWrapper loginWrapper = new LoginWrapper();
            if (!string.IsNullOrEmpty(email))
            {
                result = _loginWrapper.SendForgetUserIdByMail(email);
            }
            else
            {
                result = _loginWrapper.SendForgetPasswordResend(user);
            }
            if (result)
            {
                return PartialView("~/Views/Login/_ResetPasswordPopup.cshtml", objUserInfoDetails);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ResetPassword(Guid c)   //fired when the url for reset password clicked from reset password mail
        {
            //----- IE browser
            string userAgent = Request.Headers["User-Agent"].ToString();
            if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
            {
                return View("~/Views/LogIn/_IsIEBrowser.cshtml");
            }
            //----- IE browser
            //Guid code;
            ResetPassword resetPassword = new ResetPassword();
            LoginInfo loginInfo = new LoginInfo();
            if (c != Guid.Empty)
            {
                loginInfo = _loginWrapper.ResetPasswordData(c);
                resetPassword = new ResetPassword
                {
                    UserName = loginInfo.UserName,
                    SecurityQuestion = loginInfo.SecurityQuestion,
                    PasswordCode = c.ToString()
                };
            };
            // Check if the code expired only when the user name is not null
            if (!string.IsNullOrEmpty(loginInfo.UserName) && AccountPassword.RequestExpired(loginInfo))
            {
                // The request will expire in one day
                TempData["message"] = "The request for new password expired.";
            }
            return View("ResetPassword", resetPassword);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ResetPassword(ResetPassword resetPassword)
        {
            //LoginWrapper loginWrapper = new LoginWrapper();
            var result = _loginWrapper.ResetPassword(resetPassword);
            var message = result == false ? "The information you entered does not match our records." : "Password successfully changed";
            return Json(new { result = result, message = message });
        }
        private static void SetLevel(MenuVM mh, int level)
        {
            mh.MenuLevel = level;
            if (mh.Childlist != null)
            {
                foreach (var item in mh.Childlist)
                {
                    SetLevel(item, level + 1);
                }
            }
        }
        public ActionResult LogOut(string returnUrl = "")
        {
            returnUrl = returnUrl.Replace(" ", "");
            IdentitySignout();
            //FormsAuthentication.SignOut();
            //foreach (var element in System.Runtime.Caching.MemoryCache.Default)
            //{
            //    System.Runtime.Caching.MemoryCache.Default.Remove(element.Key);
            //}
            //Session.Clear();
            //Session.Abandon();
            return RedirectToAction("AdminLogIn");
        }
        public ActionResult CreatePassword(string userId, bool IsResetPassword, string email = "")
        {
            //----- IE browser
            string userAgent = Request.Headers["User-Agent"].ToString();
            if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
            {
                return View("~/Views/LogIn/_IsIEBrowser.cshtml");
            }
            //----- IE browser
            //LoginWrapper loginWrapper = new LoginWrapper();
            CreatePassword createPassword = new CreatePassword();
            LoginInfo loginInfo = new LoginInfo();

            if (!string.IsNullOrEmpty(userId))
            {
                userId = userId.Decrypt();
                if (!string.IsNullOrEmpty(email))
                {
                    email = email.Decrypt();
                }
                var v = _loginWrapper.GetUserDataForPasswordCreate(userId);
                createPassword.UserName = v.LoginInfo.UserName;
                createPassword.TempPassword = Convert.ToString(v.LoginInfo.TempPassword);
                createPassword.PasswordCode = Convert.ToString(v.LoginInfo.ResetPasswordCode);
                createPassword.ResetPasswordRequestDate = v.LoginInfo.ResetPasswordRequestDate;
                createPassword.UserMail = email;
                createPassword.SecurityQuestion = v.LoginInfo.SecurityQuestion;
                createPassword.SecurityAnswer = v.LoginInfo.SecurityResponse;
                createPassword.IsResetPassword = IsResetPassword;
                var secQuestList = SecurityQuestConstant.SecurityQuestValues();
                if (secQuestList != null)
                {
                    createPassword.SecurityQuestList = secQuestList.Select(x => new SelectListItem { Text = x.text, Value = x.text });
                }
            }
            LoginCacheSet _logCache = new LoginCacheSet();
            var localizationCacheTest = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Login, "en-us");
            createPassword.localization = localizationCacheTest;
            return PartialView("~/Views/Login/_NewUserPasswordCreate.cshtml", createPassword);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreatePassword(CreatePassword createPassword)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                //LoginWrapper loginWrapper = new LoginWrapper();
                var result = _loginWrapper.CreateNewPassword(createPassword);
                if (result == JsonReturnEnum.success.ToString())
                {
                    return Json(new { result = result, message = "Password successfully created" });
                }
                else if (result == JsonReturnEnum.failed.ToString())
                {
                    return Json(new { result = result, message = "The information you entered does not match our records." });
                }
                else
                {
                    return Json(new { result = JsonReturnEnum.failed.ToString(), message = result });
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(new { result = JsonReturnEnum.failed.ToString(), message = ModelValidationFailedMessage });
            }
        }

        #region Under Maintenance Action
        public ActionResult UnderMaintenencePage()
        {
            var data = System.Web.HttpContext.Current.Session["AdminUserData"];
            UserData userData = new UserData();
            userData = (UserData)data;
            var mydata = userData.SiteMaint;
            string maintenanceMessage = string.Empty;
            SiteMaintenanceWrapper objSiteMainWrapper = new SiteMaintenanceWrapper();
            if (mydata.SiteMaintenanceId != 0)
            {
                if (mydata.DowntimeStart.Date == mydata.DowntimeEnd.Date && mydata.DowntimeStart.Date != DateTime.Now.Date)
                {
                    maintenanceMessage = string.Format("{0} from {1} to {2} (Eastern) on {3}",
                    mydata.LoginPageMessage, mydata.EasternStartTime, mydata.EasternEndTime, mydata.DowntimeStart.ToString("dd-MM-yyyy"));
                }
                else if (mydata.DowntimeStart.Date == mydata.DowntimeEnd.Date && mydata.DowntimeStart.Date == DateTime.Now.Date)
                {
                    maintenanceMessage = string.Format("{0} today from {1} to {2} (Eastern)",
                    mydata.LoginPageMessage, mydata.EasternStartTime, mydata.EasternEndTime);
                }
                else
                {
                    mydata = objSiteMainWrapper.GetNextSitemaintenancewithoutLogin("n");
                    maintenanceMessage = string.Format("{0} from {1} {2} to {3} {4}",
                    mydata.LoginPageMessage, mydata.DowntimeStart.ToString("dd-MM-yyyy"), mydata.EasternStartTime, mydata.DowntimeEnd.ToString("dd-MM-yyyy"), mydata.EasternEndTime);
                }
            }
            SiteMaintenanceModel siteMaintenanceModel = new SiteMaintenanceModel
            {
                CallerUserInfoId = mydata.CallerUserInfoId,
                CallerUserName = mydata.CallerUserName,
                SiteMaintenanceId = mydata.SiteMaintenanceId,
                HeaderText = mydata.HeaderText,
                MessageText = mydata.MessageText,
                DowntimeStart = mydata.DowntimeStart,
                DowntimeEnd = mydata.DowntimeEnd,
                LoginPageMessage = mydata.LoginPageMessage,
                DashboardMessage = mydata.DashboardMessage,
                UpdateIndex = mydata.UpdateIndex,
                EasternStartTime = mydata.EasternStartTime,
                EasternEndTime = mydata.EasternEndTime
            };
            ViewBag.MaintainenceMsg = maintenanceMessage;
            return View("UnderConstruction", siteMaintenanceModel);
        }
        private string GetSiteMaintenanceMessage(SiteMaintenance mydata, SiteMaintenanceWrapper objSiteMainWrapper)
        {
            string maintenanceMessage = string.Empty;
            if (mydata.SiteMaintenanceId != 0)
            {
                mydata.EasternEndTime = UtilityFunction.GetAMPMWithSpace(mydata.EasternEndTime);
                mydata.EasternStartTime = UtilityFunction.GetAMPMWithSpace(mydata.EasternStartTime);
                if (mydata.DowntimeStart.Date == mydata.DowntimeEnd.Date && mydata.DowntimeStart.Date != DateTime.Now.Date)
                {
                    maintenanceMessage = string.Format("{0} from {1} to {2} (Eastern) on {3}",
                    mydata.LoginPageMessage, mydata.EasternStartTime, mydata.EasternEndTime, mydata.DowntimeStart.ToString("dd-MM-yyyy"));
                }
                else if (mydata.DowntimeStart.Date == mydata.DowntimeEnd.Date && mydata.DowntimeStart.Date == DateTime.Now.Date)
                {
                    maintenanceMessage = string.Format("{0} today from {1} to {2} (Eastern)",
                    mydata.LoginPageMessage, mydata.EasternStartTime, mydata.EasternEndTime);
                }
                else
                {
                    mydata = objSiteMainWrapper.GetNextSitemaintenancewithoutLogin("n");
                    maintenanceMessage = string.Format("{0} from {1} {2} to {3} {4}",
                    mydata.LoginPageMessage, mydata.DowntimeStart.ToString("dd-MM-yyyy"), mydata.EasternStartTime, mydata.DowntimeEnd.ToString("dd-MM-yyyy"), mydata.EasternEndTime);
                }
            }
            return maintenanceMessage;
        }
        #endregion

        #region Authentication
        public void IdentitySignin(UserInfoDetails appUserState, bool isPersistent = false)
        {
            var claims = new List<Claim>();

            // create required claims
            claims.Add(new Claim(ClaimTypes.NameIdentifier, appUserState.UserName));
            claims.Add(new Claim(ClaimTypes.Name, appUserState.UserName));

            // custom – my serialized AppUserState object
            claims.Add(new Claim("userState", appUserState.ToString()));

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, identity);
        }

        public void IdentitySignout()
        {
            foreach (var element in System.Runtime.Caching.MemoryCache.Default)
            {
                System.Runtime.Caching.MemoryCache.Default.Remove(element.Key);
            }
            Session.Clear();
            Session.Abandon();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                                            DefaultAuthenticationTypes.ExternalCookie);
        }
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        #endregion
    }

    internal class ChallengeResult : HttpUnauthorizedResult
    {
        private const string XsrfKey = "*$%som-7*&$";
        public ChallengeResult(string provider, string redirectUri)
            : this(provider, redirectUri, null)
        { }

        public ChallengeResult(string provider, string redirectUri, string userId)
        {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
        }

        public string LoginProvider { get; set; }
        public string RedirectUri { get; set; }
        public string UserId { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            if (UserId != null)
                properties.Dictionary[XsrfKey] = UserId;

            var owin = context.HttpContext.GetOwinContext();
            owin.Authentication.Challenge(properties, LoginProvider);
        }
    }
}