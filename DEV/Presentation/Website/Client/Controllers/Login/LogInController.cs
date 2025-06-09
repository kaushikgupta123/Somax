using Business.Authentication;

using Client.BusinessWrapper;
using Client.Common;
using Client.Models;
using Client.Models.Account;
using Client.Models.Common;

using Common.Constants;

using DataContracts;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

using Newtonsoft.Json;

using Presentation.Common;

using SomaxMVC.BusinessWrapper;
using SomaxMVC.Models;
using SomaxMVC.ViewModels;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Utility;

using ViewModels;

namespace Client.Controllers
{
    public class LogInController : Controller
    {
        public string Localization { get; set; }
        [AcceptVerbs("Get", "Post")]
        public ActionResult SomaxLogIn(string returnUrl, string Language = "en-us")
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
            ViewBag.ReturnUrl = returnUrl;
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
                title = "SOMAX | Login";
            else
                title = "SOMAX Login - " + environ;
            ViewBag.Title = title;

            if (Request.Cookies[CookiesConstants.SOMAX_LOGIN_ID] != null)
            {
                objUser.UserName = Request.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Value;
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
                // RKL - the start and end date are eastern time zone
                DateTime start_utc = TimeZoneInfo.ConvertTimeToUtc(mydata.DowntimeStart, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                DateTime end_utc = TimeZoneInfo.ConvertTimeToUtc(mydata.DowntimeEnd, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

                /*                
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
                */
                if (timeUtc >= start_utc && timeUtc <= end_utc)
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

            if (TempData["UserInfoDetails"] != null)
            {
                UserInfoDetails userInfoDetails = (UserInfoDetails)TempData["UserInfoDetails"];
                objUser.IsSOMAXAdmin = userInfoDetails.IsSOMAXAdmin;
                objUser.UserName = userInfoDetails.UserName;
                objUser.Password = userInfoDetails.Password;
                objUser.UserInfoId = userInfoDetails.UserInfoId;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/LogIn/_LoginPartial.cshtml", objUser);
            }
            return View(objUser);

        }

        public ActionResult ForgetPasswordPatial()
        {
            UserInfoDetails objUser = new UserInfoDetails();
            LoginCacheSet _logCache = new LoginCacheSet();

            var localizationCacheTest = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Login, "en-us");
            objUser.localization = localizationCacheTest;
            return PartialView("~/Views/LogIn/_ForgetPasswordPartial.cshtml", objUser);
        }

        [HttpPost]
        public ActionResult ChangeToVersion1()
        {
            var data = System.Web.HttpContext.Current.Session["userData"];
            UserData userData = new UserData();
            userData = (UserData)data;
            string remoteUrl = System.Configuration.ConfigurationManager.AppSettings["V1RedirectURL"].ToString();
            string encuname = WebSecurity.Encrypt(userData.DatabaseKey.UserName, "somaxsom");
            string encpwd = userData.DatabaseKey.Pwd;
            string strUrl = remoteUrl + "uName=" + encuname + "&pwd=" + encpwd;
            return Json(new { redirecturl = strUrl }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LogIn(UserInfoDetails objUser)
        //{
        //    VMLogin LogIn = new VMLogin();
        //    if (ModelState.IsValid)
        //    {
        //        LoginWrapper LoginWrapper = new LoginWrapper(objUser);
        //        if (objUser.RememberMe)
        //        {
        //            Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Expires = DateTime.Now.AddDays(30);
        //            Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Value = objUser.UserName;
        //        }
        //        else
        //        {
        //            Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Expires = DateTime.Now.AddDays(-1);
        //            Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Value = null;
        //        }
        //        LogIn = LoginWrapper.GetLogIn();

        //        if (LogIn.IsAuthenticated)
        //        {
        //            string serializeModel = JsonConvert.SerializeObject(LogIn.UserInfoDetails);
        //            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, LogIn.UserInfoDetails.UserName, DateTime.Now, DateTime.Now.AddMinutes(20), objUser.RememberMe, serializeModel);
        //            string encTicket = FormsAuthentication.Encrypt(ticket);
        //            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        //            Response.Cookies.Add(faCookie);

        //            Session[SessionConstants.LOGGED_USERNAME] = objUser.UserName;
        //            Session[SessionConstants.LOGGED_PASSWORD] = objUser.Password;
        //            //-----Call getmenu list here and keep the list in session or temp data which one is better----//
        //            var menuReturnList = GetMenuList("Site");
        //            if (LogIn.UIVersionRedirectToV1 == true)
        //            {
        //                string remoteUrl = System.Configuration.ConfigurationManager.AppSettings["V1RedirectURL"].ToString();
        //                string encuname = WebSecurity.Encrypt(LogIn.UserInfoDetails.UserName, "somaxsom");
        //                string encpwd = WebSecurity.Encrypt(LogIn.UserInfoDetails.Password, "somaxsom");
        //                Response.Redirect(remoteUrl + "uName=" + encuname + "&pwd=" + encpwd);
        //            }
        //            else
        //            {
        //                if (!(menuReturnList.Count == 0 || menuReturnList.Count < 0))
        //                {
        //                    Session["MenuDetails"] = menuReturnList;
        //                    return RedirectToLocal(objUser.ReturnUrl);
        //                }
        //                else
        //                {
        //                    return RedirectToAction("SomaxLogin");
        //                }
        //            }
        //        }
        //    }
        //    LoginCacheSet _logCache = new LoginCacheSet();
        //    objUser.FailureMessage = LogIn.UserInfoDetails.FailureMessage;
        //    objUser.FailureReason = LogIn.UserInfoDetails.FailureReason;
        //    var localizationCacheTest = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Login, "en-us");
        //    objUser.localization = localizationCacheTest;
        //    return View("SomaxLogIn", objUser);
        //}



        [AcceptVerbs("Get", "Post")]
        public ActionResult LogInFromV1Web([System.Web.Http.FromUri] string uName, [System.Web.Http.FromUri] string pwd)
        {
            VMLogin LogIn = new VMLogin();
            UserInfoDetails objUser = new UserInfoDetails();

            //String htmlUname = uName;
            //String htmlPwd = pwd;
            //StringWriter writerU = new StringWriter();
            //StringWriter writerP = new StringWriter();
            //Server.UrlDecode(htmlUname, writerU);
            //Server.UrlDecode(htmlPwd, writerP);
            //String uString = writerU.ToString();
            //String pString = writerP.ToString();

            //string encuname = WebSecurity.Decrypt(uString, "somaxsom");
            //string encpwd = WebSecurity.Decrypt(pString, "somaxsom");

            //string encuname = WebSecurity.Decrypt(uName.Replace(" ","+"), "somaxsom");
            //string encpwd = WebSecurity.Decrypt(pwd.Replace(" ", "+"), "somaxsom");

            string encuname = WebSecurity.Decrypt(uName, "somaxsom");
            string encpwd = WebSecurity.Decrypt(pwd, "somaxsom");

            objUser.UserName = encuname;
            objUser.Password = encpwd;
            objUser.RememberMe = true;
            if (objUser.RememberMe)
            {
                Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Expires = DateTime.Now.AddDays(30);
                Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Value = objUser.UserName;
            }
            else
            {
                Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Value = null;
            }
            if (ModelState.IsValid)
            {
                LoginWrapper LoginWrapper = new LoginWrapper(objUser);
                LogIn = LoginWrapper.GetLogInFromV1Web();
                if (LogIn.IsAuthenticated)
                {
                    string serializeModel = JsonConvert.SerializeObject(LogIn.UserInfoDetails);
                    //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, LogIn.UserInfoDetails.UserName, DateTime.Now, DateTime.Now.AddMinutes(20), objUser.RememberMe, serializeModel);
                    //string encTicket = FormsAuthentication.Encrypt(ticket);
                    //HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    //Response.Cookies.Add(faCookie);

                    Session[SessionConstants.LOGGED_USERNAME] = objUser.UserName;
                    Session[SessionConstants.LOGGED_PASSWORD] = objUser.Password;
                    //-----Call getmenu list here and keep the list in session or temp data which one is better----//
                    IdentitySignin(LogIn.UserInfoDetails, objUser.RememberMe);
                    var menuReturnList = GetMenuList("Site");
                    if (!(menuReturnList.Count == 0 || menuReturnList.Count < 0))
                    {
                        Session["MenuDetails"] = menuReturnList;
                        return RedirectToLocal(objUser.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("SomaxLogin");
                    }
                }
            }
            LoginCacheSet _logCache = new LoginCacheSet();
            objUser.FailureMessage = LogIn.UserInfoDetails.FailureMessage;
            objUser.FailureReason = LogIn.UserInfoDetails.FailureReason;
            var localizationCacheTest = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Login, "en-us");
            objUser.localization = localizationCacheTest;
            return View("SomaxLogIn", objUser);
        }

        public ActionResult LogOut(string returnUrl = "")
        {
            returnUrl = returnUrl.Replace(" ", "");
            //FormsAuthentication.SignOut();
            IdentitySignout();
            //foreach (var element in System.Runtime.Caching.MemoryCache.Default)
            //{
            //    System.Runtime.Caching.MemoryCache.Default.Remove(element.Key);
            //}
            //Session.Clear();
            //Session.Abandon();
            return RedirectToAction("SomaxLogIn");
        }

        [HttpPost]
        public ActionResult ForgotPasswordSendEMail(string email = "", string user = "")
        {
            UserInfoDetails objUserInfoDetails = new UserInfoDetails();
            objUserInfoDetails.UserEmail = email;
            var result = false;
            LoginWrapper loginWrapper = new LoginWrapper();
            if (!string.IsNullOrEmpty(email))
            {
                result = loginWrapper.SendForgetUserIdByMail(email);
            }
            else
            {
                result = loginWrapper.SendForgetPasswordResend(user);
            }
            if (result)
            {
                return PartialView("~/Views/LogIn/_ResetPasswordPopup.cshtml", objUserInfoDetails);
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
            LoginWrapper loginWrapper = new LoginWrapper();
            ResetPassword resetPassword = new ResetPassword();
            LoginInfo loginInfo = new LoginInfo();
            if (c != Guid.Empty)
            {
                loginInfo = loginWrapper.ResetPasswordData(c);
                resetPassword = new ResetPassword
                {
                    UserName = loginInfo.UserName,
                    SecurityQuestion = loginInfo.SecurityQuestion,
                    PasswordCode = c.ToString()
                };
            };

            //-------V2 - 491
            var passwordSettings = loginWrapper.RetrievePasswordSettings(loginInfo.ClientId);
            if (passwordSettings != null)
            {
                resetPassword.PWReqMinLength = passwordSettings.PWReqMinLength;
                resetPassword.PWMinLength = passwordSettings.PWMinLength;
                resetPassword.PWRequireNumber = passwordSettings.PWRequireNumber;
                resetPassword.PWRequireAlpha = passwordSettings.PWRequireAlpha;
                resetPassword.PWRequireMixedCase = passwordSettings.PWRequireMixedCase;
                resetPassword.PWRequireSpecialChar = passwordSettings.PWRequireSpecialChar;
                resetPassword.PWNoRepeatChar = passwordSettings.PWNoRepeatChar;
                resetPassword.PWNotEqualUserName = passwordSettings.PWNotEqualUserName;
            }
            //------- V2-491

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
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                LoginWrapper loginWrapper = new LoginWrapper();
                var result = loginWrapper.ResetPassword(resetPassword);
                var message = result == false ? "The information you entered does not match our records." : "Password successfully changed";
                return Json(new { result = result, message = message });
            }
            else
            {
                ModelValidationFailedMessage = string.Join("\n", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                //ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(new { result = false, message = ModelValidationFailedMessage });
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
        }
        public static IList<MenuVM> GetMenuList(string _menuType)
        {
            var data = System.Web.HttpContext.Current.Session["userData"];
            UserData userData = new UserData();
            userData = (UserData)data;
            string dbLoc = string.IsNullOrEmpty(userData.Site.Localization) ? "en-us" : userData.Site.Localization;
            // rkl -2019-07-03 - temp
            if (dbLoc != "en-us" && dbLoc != "fr-fr")
            {
                dbLoc = "en-us";
            }

            DataContracts.Menu mainmenu = new DataContracts.Menu()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                CallerUserName = userData.DatabaseKey.UserName,
                ResourceSet = "Menu",
                LocaleId = dbLoc,
                MenuType = _menuType,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                UserInfoId = userData.DatabaseKey.User.UserInfoId
            };
            var menuData = mainmenu.RetrieveAllCustom(userData.DatabaseKey, userData.Site.APM, userData.Site.CMMS, userData.Site.Sanitation);
            menuData = GetMenuListBySecurity(_menuType, userData, menuData);
            bool IsApmOnly = (userData.Site.APM == true && userData.Site.CMMS == false && userData.Site.Sanitation == false);
            bool IsSanitationOnly = (userData.Site.APM == false && userData.Site.CMMS == false && userData.Site.Sanitation == true);
            if (IsApmOnly)
            {
                SetAsAPMOnly(menuData);
            }

            if (IsSanitationOnly)
            {
                SetAsSanitationOnly(menuData);
            }
            //--V2-408--//
            #region V2-408 //-- Fleet Only Access

            bool IsFleetOnly = (userData.Site.Fleet == true && userData.Site.APM == false && userData.Site.CMMS == false && userData.Site.Sanitation == false);
            bool IsFleetAndCMMS = (userData.Site.Fleet == true && userData.Site.APM == false && userData.Site.CMMS == true && userData.Site.Sanitation == false);

            if (IsFleetOnly)
            {
                Int64 ServiceId = CreateMenu(ref menuData);
                SetAsFleetOnly(menuData, ServiceId);
            }
            if (IsFleetAndCMMS)
            {
                SetAsFleetAndCMMS(menuData);
            }
            #endregion
            //--END----V2-408--//
            List<MenuVM> menu = menuData.Where(x => menuData.Any(y => (x.ParentMenuId == 0 || y.MenuId == x.ParentMenuId)))
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
            return menu;
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

        private static List<Menu> GetMenuListBySecurity(string menuType, UserData userData, List<Menu> menuData)
        {
            #region V2-822
            bool OraclePurchaseRequestExportInUse = false;
            var InterfacePropData = (List<InterfacePropModel>)System.Web.HttpContext.Current.Session["InterfacePropData"];
            if (InterfacePropData != null && InterfacePropData.Count > 0)
            {
                OraclePurchaseRequestExportInUse = InterfacePropData.Where(x => x.InterfaceType == ApiConstants.OraclePurchaseRequestExport).Select(x => x.InUse).FirstOrDefault();
            }
            #endregion
            // bool IsApmOnly = (userData.Site.APM == true && userData.Site.CMMS == false);
            if (menuType != null && menuType.ToLower() == "site")
            {
                #region Configuration 
                // 2020-Dec-12-RKL
                // Configuration only available to the admin users 
                // This code allows configuration to a user if they are either Admin or Full
                // Change to be either an admin or issuperuser               
                //if (userData.DatabaseKey.User.UserType.ToUpper() != UserTypeConstants.Admin.ToUpper() && userData.DatabaseKey.User.UserType.ToUpper() != UserTypeConstants.Full.ToUpper())
                if (!(userData.DatabaseKey.User.UserType.ToUpper() == UserTypeConstants.Admin.ToUpper() || userData.DatabaseKey.User.IsSuperUser))
                {
                    menuData = menuData.Where(m => m.MenuId != 50).ToList(); // Configuration
                }
                #endregion Configuration
                #region Maintenance
                if (!(userData.Security.Equipment.Access || userData.Security.WorkOrders.Access || userData.Security.PrevMaint.Access))
                {
                    menuData = menuData.Where(m => m.MenuId != 2).ToList(); // Maintenance
                }
                else
                {
                    #region Equipment Menu
                    if (!userData.Security.Equipment.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 3).ToList(); // Equipment
                    }
                    #endregion
                    #region WorkOrder menu
                    if (!userData.Security.WorkOrders.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 4).ToList(); // WorkOrder
                    }
                    else
                    {
                        if (!userData.Security.WorkOrders.LaborScheduling)
                        {
                            menuData = menuData.Where(m => m.MenuId != 16).ToList(); //Scheduling
                            menuData = menuData.Where(m => m.MenuId != 134).ToList(); //LaborScheduling
                            menuData = menuData.Where(m => m.MenuId != 135).ToList(); //DailyLaborScheduling
                        }

                        if (!userData.Security.WorkOrders.Approve || userData.DatabaseKey.ApprovalGroupSettings.WorkRequests)
                        {
                            menuData = menuData.Where(m => m.MenuId != 9).ToList(); //WO Approval
                        }
                        if (!userData.Security.WorkOrders.Planning)
                        {
                            menuData = menuData.Where(m => m.MenuId != 137).ToList(); //WO Planning
                        }
                    }
                    #endregion
                    #region Preventive Maintenance
                    // RKL - If have meter but not prev maint
                    // 
                    if (!userData.Security.PrevMaint.Access && !userData.Security.Meters.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 5).ToList(); // Preventive Maintenance
                    }
                    else
                    {
                        if (!userData.Security.PrevMaint.Access)
                        {
                            menuData = menuData.Where(m => m.MenuId != 13).ToList(); //
                        }
                        if (!userData.Security.PrevMaint.PrevMaintPMForecast)
                        {
                            menuData = menuData.Where(m => m.MenuId != 14).ToList(); //PM Forecast
                        }
                        if (!userData.Security.Meters.Access)
                        {
                            menuData = menuData.Where(m => m.MenuId != 115).ToList(); //PM Meters
                        }
                        if (!userData.Security.PrevMaint.Generate_WorkOrders)
                        {
                            menuData = menuData.Where(m => m.MenuId != 140).ToList(); //PM Work Order Generation
                        }

                    }
                    #endregion
                }
                #endregion

                #region Sensor Menu
                if (!userData.Security.Sensors.Access)
                {
                    menuData = menuData.Where(m => m.MenuId != 18).ToList(); // Sensor Menu
                }
                else if (!(userData.Security.Sensors.Search && userData.Site.APM == true))
                {
                    menuData = menuData.Where(m => m.MenuId != 19).ToList(); // Device Search
                }
                else
                {
                    if (!userData.Security.Sensors.AlertProcedures)
                    {
                        menuData = menuData.Where(m => m.MenuId != 20).ToList(); // Alert Procedures
                    }
                }
                #endregion

                #region Inventory

                if (!((userData.Security.Parts.Access) || (userData.Security.Vendors.Access) || (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster && (userData.Security.Parts.SiteReview || userData.Security.PartMasterRequest.Access))))
                {
                    menuData = menuData.Where(m => m.MenuId != 21).ToList(); //Inventory
                }
                else
                {
                    if (!userData.Security.Parts.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 22).ToList(); //Parts
                    }

                    if (!(userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster && (userData.Security.Parts.SiteReview || userData.Security.PartMasterRequest.Access)))
                    {
                        menuData = menuData.Where(m => m.MenuId != 112).ToList();//PartsManagement
                    }
                    else
                    {
                        if (!userData.Security.Parts.SiteReview)
                        {
                            menuData = menuData.Where(m => m.MenuId != 113).ToList();//Part Master Review
                        }
                        if (!userData.Security.PartMasterRequest.Access)
                        {
                            menuData = menuData.Where(m => m.MenuId != 114).ToList(); //PartMRequest
                        }
                    }

                    if (userData.Security.Parts.Access)
                    {
                        if (!userData.Security.Parts.Checkout)
                        {
                            menuData = menuData.Where(m => m.MenuId != 33).ToList(); //Checkout
                        }
                        if (!userData.Security.Parts_Issue.Access)
                        {
                            menuData = menuData.Where(m => m.MenuId != 157).ToList(); //Issue
                        }
                        if (!userData.Security.Parts.Receipt)
                        {
                            menuData = menuData.Where(m => m.MenuId != 34).ToList(); //Receipt
                        }
                        if (!userData.Security.Parts.Physical)
                        {
                            menuData = menuData.Where(m => m.MenuId != 32).ToList(); //Physical Inventory
                        }
                        if (!userData.Security.Parts.CycleCount)
                        {
                            menuData = menuData.Where(m => m.MenuId != 142).ToList(); //Part Cycle
                        }
                        if (!userData.Security.Vendor_Create_Vendor_Request.Access)
                        {
                            menuData = menuData.Where(m => m.MenuId != 153).ToList(); //Vendor Request
                        }
                        if (!userData.Security.Parts.MaterialRequest)
                        {
                            menuData = menuData.Where(m => m.MenuId != 145).ToList(); //Material Request
                        }
                        if (!userData.Security.Parts.MultiSiteSearch)
                        {
                            menuData = menuData.Where(m => m.MenuId != 108).ToList(); //Multisite search
                        }
                        if (!(userData.DatabaseKey.Client.UseMultiStoreroom && userData.Security.PartTransfer_Auto_Transfer_Generation.Access))
                        {
                            menuData = menuData.Where(m => m.MenuId != 158).ToList(); //For Auto Transfer Request Setup V2-1059
                        }
                        if (!(userData.Security.PartTransfers.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster == true))
                        {
                            menuData = menuData.Where(m => m.MenuId != 111).ToList(); //Part Transfer
                        }
                    }
                    else
                    {
                        menuData = menuData.Where(m => m.MenuId != 33).ToList(); //Checkout
                        menuData = menuData.Where(m => m.MenuId != 34).ToList(); //Receipt
                        menuData = menuData.Where(m => m.MenuId != 32).ToList(); //Physical Inventory
                        menuData = menuData.Where(m => m.MenuId != 142).ToList(); //Part Cycle
                        menuData = menuData.Where(m => m.MenuId != 145).ToList(); //Material Request
                        menuData = menuData.Where(m => m.MenuId != 108).ToList(); //Multisite search
                        menuData = menuData.Where(m => m.MenuId != 111).ToList(); //Part Transfer
                    }

                    if (!userData.Security.Vendors.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 26).ToList(); //Vendors
                    }

                    if (!userData.DatabaseKey.Client.UseMultiStoreroom)
                    {
                        menuData = menuData.Where(m => m.MenuId != 147).ToList(); //Storeroom Transfer V2-751
                    }
                }

                #endregion

                #region Procurement Menu
                if (!(userData.Security.Purchasing.Access || userData.Security.PurchaseRequest.Access))//(userData.Security.Purchasing.Access || userData.Security.PurchaseRequest.Access || userData.Security.Purchasing.Approve || userData.Security.Purchasing.ReceiveAccess))
                {
                    menuData = menuData.Where(m => m.MenuId != 92).ToList(); // Procurement Menu
                }
                else
                {
                    if (!userData.Security.PurchaseRequest.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 93).ToList(); // Purchase Request
                    }
                    if (!userData.Security.PurchaseRequest.AutoGeneration)
                    {
                        menuData = menuData.Where(m => m.MenuId != 141).ToList(); // Purchase Request AutoGeneration
                    }
                    if (!userData.Security.Purchasing.Approve || userData.DatabaseKey.ApprovalGroupSettings.PurchaseRequests)
                    {
                        menuData = menuData.Where(m => m.MenuId != 94).ToList(); // Purchase Approval
                    }
                    else
                    {
                        if (OraclePurchaseRequestExportInUse) // V2-822
                        {
                            if (!(userData.DatabaseKey.Personnel.ExOracleUserId != "" && userData.Security.PurchaseRequest.Approve)) // V2-822
                            {
                                menuData = menuData.Where(m => m.MenuId != 94).ToList(); // Purchase Approval
                            }
                        }
                    }
                    if (!userData.Security.Purchasing.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 95).ToList(); // Purchase Order
                    }
                    if (!userData.Security.InvoiceMatching.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 97).ToList(); // Invoice Matching
                    }
                    if (!userData.Security.Purchasing.ReceiveAccess)
                    {
                        menuData = menuData.Where(m => m.MenuId != 96).ToList(); // Purchase Receipt
                    }
                }
                #endregion

                #region Personnel Menu
                if (!userData.Security.Personnel.Access)
                {
                    menuData = menuData.Where(m => m.MenuId != 136).ToList(); // Personnel
                }
                #endregion

                #region Project Menu
                if (!userData.Security.Project.Access)
                {
                    menuData = menuData.Where(m => m.MenuId != 138).ToList(); // V2-594
                }
                #endregion

                #region Reports Menu
                if (!userData.Security.Reports.Access)
                {
                    menuData = menuData.Where(m => m.MenuId != 29).ToList(); // Reports
                }
                #endregion

                #region Sanitation Menu
                if (!userData.Security.Sanitation.Access)
                {
                    menuData = menuData.Where(m => m.MenuId != 81).ToList(); // Sanitation
                    menuData = menuData.Where(m => m.MenuId != 82).ToList(); //Jobs
                    menuData = menuData.Where(m => m.MenuId != 83).ToList(); //Master schedule
                }
                else
                {
                    //if (userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices) //--V2-278
                    if (userData.Site.Sanitation == true)
                    {
                        if (!userData.Security.SanitationJob.Access)
                        {
                            menuData = menuData.Where(m => m.MenuId != 85).ToList(); // Sanitation Job Search
                        }
                        if (!(userData.Security.Sanitation.Approve && !userData.Site.ExternalSanitation))
                        {
                            menuData = menuData.Where(m => m.MenuId != 86).ToList(); // Sanitation Job Approval
                        }
                        if (!(userData.Security.Sanitation.Verification && !userData.Site.ExternalSanitation))
                        {
                            menuData = menuData.Where(m => m.MenuId != 88).ToList(); //Sanitation Verification
                        }
                        //if (!(userData.Security.Sanitation.OnDemand && userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices)) //--V2-278
                        if (!(userData.Security.Sanitation.OnDemand))
                        {
                            menuData = menuData.Where(m => m.MenuId != 69).ToList(); //Sanitation On-Demand
                        }
                        if (!(userData.Security.Sanitation.Access))
                        {
                            menuData = menuData.Where(m => m.MenuId != 89).ToList(); //Master Schedule Search
                        }
                        // We do not yet have Sanitation Forecast implemented.
                        menuData = menuData.Where(m => m.MenuId != 90).ToList(); //Master Schedule Forecast
                        //if (!(userData.Security.Sanitation.JobGeneration && !userData.Site.ExternalSanitation))
                        //{
                        //    menuData = menuData.Where(m => m.MenuId != 90).ToList(); //Master Schedule Forecast
                        //}
                    }
                    else
                    {
                        menuData = menuData.Where(m => m.MenuId != 85).ToList(); // Sanitation Job Search
                        menuData = menuData.Where(m => m.MenuId != 86).ToList(); // Sanitation Job Approval
                        menuData = menuData.Where(m => m.MenuId != 88).ToList(); //Sanitation Verification
                        menuData = menuData.Where(m => m.MenuId != 89).ToList(); //Master Schedule Search
                        menuData = menuData.Where(m => m.MenuId != 90).ToList(); //Master Schedule Forecast
                    }
                }
                #endregion
                #region Fleet
                if (!(userData.Security.Fleet_Assets.Access || (userData.Site.Fleet && userData.Security.Fleet_MeterHistory.Access) || userData.Site.Fleet && userData.Security.Fleet_FuelTracking.Access || userData.Site.Fleet && userData.Security.Fleet_Scheduled.Access || userData.Site.Fleet && userData.Security.Fleet_Issues.Access || userData.Site.Fleet && userData.Security.Fleet_ServiceOrder.Access))
                {
                    menuData = menuData.Where(m => m.MenuId != 116).ToList(); // Fleet
                }
                else
                {
                    #region Fleet Asset
                    if (!(userData.Security.Fleet_Assets.Access))
                    {
                        menuData = menuData.Where(m => m.MenuId != 117).ToList(); // Assets

                    }
                    #endregion

                    #region Fleet Meter
                    if (!(userData.Site.Fleet && userData.Security.Fleet_MeterHistory.Access))
                    {
                        menuData = menuData.Where(m => m.MenuId != 118).ToList();  // Meter History
                    }
                    #endregion

                    #region Fleet Fuel
                    if (!(userData.Site.Fleet && userData.Security.Fleet_FuelTracking.Access))
                    {
                        menuData = menuData.Where(m => m.MenuId != 119).ToList(); //Fuel
                    }
                    #endregion

                    #region Fleet Scheduled
                    if (!(userData.Site.Fleet && userData.Security.Fleet_Scheduled.Access))
                    {
                        menuData = menuData.Where(m => m.MenuId != 121).ToList(); //Scheduled
                    }
                    #endregion

                    #region Fleet Issues
                    if (!(userData.Site.Fleet && userData.Security.Fleet_Issues.Access))
                    {
                        menuData = menuData.Where(m => m.MenuId != 122).ToList(); //Issues
                    }
                    #endregion

                    #region Fleet Service Order
                    if (!(userData.Site.Fleet && userData.Security.Fleet_ServiceOrder.Access))
                    {
                        menuData = menuData.Where(m => m.MenuId != 123).ToList(); //Service Order
                    }
                    #endregion
                }
                #endregion
                #region Approval Menu
                if (!(userData.DatabaseKey.ApprovalGroupSettings.PurchaseRequests && userData.Security.PurchaseRequest.Approve) && !(userData.DatabaseKey.ApprovalGroupSettings.MaterialRequests && userData.Security.MaterialRequest_Approve.Access) && !(userData.DatabaseKey.ApprovalGroupSettings.WorkRequests && userData.Security.WorkOrders.Approve))
                {
                    menuData = menuData.Where(m => m.MenuId != 148).ToList(); // Approval

                }
                #endregion
                #region BBU-KPI
                if (!(userData.Security.BBUKPI_Site.Access || userData.Security.BBUKPI_Enterprise.Access))
                {
                    menuData = menuData.Where(m => m.MenuId != 150).ToList();
                }
                else
                {
                    #region BBU-KPI-Site
                    if (!userData.Security.BBUKPI_Site.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 151).ToList();
                    }
                    #endregion
                    #region BBU-KPI-Enterprise
                    if (!userData.Security.BBUKPI_Enterprise.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 152).ToList();
                    }
                    #endregion
                }
                #endregion
                #region Analytics
                if(!userData.Security.Analytics.WorkOrderStatus)
                {
                    menuData = menuData.Where(m => m.MenuId != 160).ToList();
                    menuData = menuData.Where(m => m.MenuId != 161).ToList();
                }
                #endregion
            }
            else
            {
                //if (!(userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && (userData.DatabaseKey.User.IsSiteAdmin || userData.DatabaseKey.User.IsSuperUser)))
                //{
                //  //  menuData = menuData.Where(m => m.MenuId != 51).ToList(); //Configuration Dashboard
                //    menuData = menuData.Where(m => m.MenuId != 52).ToList(); //Company
                //    menuData = menuData.Where(m => m.MenuId != 53).ToList(); //Site
                //    menuData = menuData.Where(m => m.MenuId != 54).ToList(); //Lookup Lists
                //    menuData = menuData.Where(m => m.MenuId != 100).ToList(); //Crafts
                //    menuData = menuData.Where(m => m.MenuId != 57).ToList(); //User Management
                //    menuData = menuData.Where(m => m.MenuId != 99).ToList(); //Notifications

                //}
                //#region SecurityProfile
                //if (!(userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.DatabaseKey.User.IsSuperUser))
                //{
                //    menuData = menuData.Where(m => m.MenuId != 56).ToList(); //SecurityProfile
                //}
                //#endregion
                #region Enterprise Level - 
                if (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && !userData.DatabaseKey.User.IsSuperUser)
                {
                    menuData = menuData.Where(m => m.MenuId != 52).ToList();  // Company
                    menuData = menuData.Where(m => m.MenuId != 54).ToList();  // Lookup Lists
                    menuData = menuData.Where(m => m.MenuId != 100).ToList(); // Crafts
                    menuData = menuData.Where(m => m.MenuId != 56).ToList();  // Security
                }
                #endregion
                #region CustomSecurityProfile
                if (!(userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.DatabaseKey.User.IsSuperUser))
                {
                    menuData = menuData.Where(m => m.MenuId != 131).ToList(); //CustomSecurityProfile
                }
                #endregion
                // RKL - 2022-May-4
                #region UI Configuration 
                if (!(userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.DatabaseKey.User.IsSuperUser))
                {
                    menuData = menuData.Where(m => m.MenuId != 139).ToList(); //UI Configuration
                }
                #endregion

                #region Crafts
                //if (IsApmOnly)
                //{
                //    menuData = menuData.Where(m => m.MenuId != 100).ToList();
                //}
                #endregion
                #region Libraries
                if (!(userData.Security.OnDemandLibrary.Access || userData.Security.PrevMaintLibrary.Access || (userData.Security.Sanitation.OnDemand && userData.Site.Sanitation == true) || userData.Security.MasterSanitation.Access || (userData.Site.Fleet && userData.Security.Fleet_ServiceTask.Access))) //Libraries
                {
                    menuData = menuData.Where(m => m.MenuId != 55).ToList();
                }
                else
                {
                    if (!userData.Security.OnDemandLibrary.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 67).ToList(); //Maintenance On Demand
                    }

                    if (!userData.Security.PrevMaintLibrary.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 68).ToList(); //Preventive Maintenance
                    }

                    if (!userData.Security.MasterSanitation.Access)
                    {
                        menuData = menuData.Where(m => m.MenuId != 70).ToList(); // Master Sanitation
                    }
                    #region Fleet Service Task
                    if (!(userData.Site.Fleet && userData.Security.Fleet_ServiceTask.Access))
                    {
                        menuData = menuData.Where(m => m.MenuId != 120).ToList(); //Service Task
                    }
                    #endregion
                }
                #endregion
                #region Storeroom
                // RKL - V2- 
                //if (!(userData.DatabaseKey.User.UserType.ToUpper() == UserTypeConstants.Admin.ToUpper() || userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.UseMultiStoreroom==true))
                if (!(userData.DatabaseKey.Client.UseMultiStoreroom == true))
                {
                    menuData = menuData.Where(m => m.MenuId != 144).ToList(); // Storeroom V2-671
                }
                #endregion

                #region Account
                if (!userData.Security.Accounts.Access)
                {
                    menuData = menuData.Where(m => m.MenuId != 27).ToList(); //Account
                }
                #endregion
                #region Approval Groups
                if (!userData.Security.ApprovalGroupsConfiguration.Access)
                {
                    menuData = menuData.Where(m => m.MenuId != 146).ToList(); //Approval Groups
                }
                #endregion
                #region Ship To Addresses
                if (!userData.Security.ShipToAddress.Access)
                {
                    menuData = menuData.Where(m => m.MenuId != 159).ToList(); //Ship To Addresses
                }
                #endregion
                //#region Notification
                //if (IsApmOnly)
                //{
                //    menuData = menuData.Where(m => m.MenuId != 99).ToList();
                //}
                //#endregion
                #region Master Menu                
                if (!((userData.Security.EquipmentMaster != null && userData.Security.EquipmentMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) || ((userData.Security.VendorMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UseVendorMaster) || (userData.Security.VendorCatalog.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UseVendorMaster)) || ((userData.Security.PartMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster) || (userData.Security.ManufacturerMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster))))
                {
                    menuData = menuData.Where(m => m.MenuId != 102).ToList();
                }
                else
                {
                    if (!(userData.Security.EquipmentMaster != null && userData.Security.EquipmentMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise))
                    {
                        menuData = menuData.Where(m => m.MenuId != 103).ToList(); //Equipment Master

                    }
                    if (!((userData.Security.VendorMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UseVendorMaster) || (userData.Security.VendorCatalog.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UseVendorMaster)))
                    {
                        menuData = menuData.Where(m => m.MenuId != 105).ToList();
                    }
                    else
                    {
                        if (!(userData.Security.VendorMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UseVendorMaster))
                        {
                            menuData = menuData.Where(m => m.MenuId != 106).ToList(); //Vendor Master
                        }

                        if (!(userData.Security.VendorCatalog.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UseVendorMaster))
                        {
                            menuData = menuData.Where(m => m.MenuId != 107).ToList(); //Vendor Catalog
                        }
                    }
                    if (!((userData.Security.PartMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster) || (userData.Security.ManufacturerMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster)))
                    {
                        menuData = menuData.Where(m => m.MenuId != 104).ToList();
                    }
                    else
                    {
                        if (!(userData.Security.PartMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster))
                        {
                            menuData = menuData.Where(m => m.MenuId != 109).ToList(); //Part Master
                        }

                        if (!(userData.Security.ManufacturerMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster))
                        {
                            menuData = menuData.Where(m => m.MenuId != 110).ToList(); //Manufacturer Master
                        }
                        if (!(userData.Security.PartCategoryMaster.Access && userData.Site.UsePartMaster))
                        {
                            menuData = menuData.Where(m => m.MenuId != 143).ToList(); //Category Master
                        }
                    }
                }
                #endregion
            }
            return menuData;
        }

        private static void SetAsAPMOnly(List<Menu> menuData)
        {
            foreach (var m in menuData)
            {
                if (m.MenuId == 3)
                {
                    m.ParentMenuId = 0;
                    m.CssClass = "flaticon-interface-6";
                }
                if (m.MenuId == 101)
                {
                    m.ParentMenuId = 0;
                    m.CssClass = "flaticon-calendar";
                    m.MenuPosition = 4;
                }
            }
        }
        private static void SetAsSanitationOnly(List<Menu> menuData)
        {
            foreach (var m in menuData)
            {
                if (m.MenuId == 3)
                {
                    m.ParentMenuId = 0;
                    m.CssClass = "flaticon-interface-6";
                }
                if (m.MenuId == 82)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("spnSanitationJob", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = 0;
                    m.MenuLevel = 0;
                    m.CssClass = "flaticon-layers";
                }
                if (m.MenuId == 83)
                {
                    m.ParentMenuId = 0;
                    m.MenuLevel = 0;
                    m.CssClass = "flaticon-time-2";
                }
                if (m.MenuId == 69)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("spnOnDemandProcedure", LocalizeResourceSetConstants.Menu);
                }
            }
        }

        private static void SetAsFleetOnly(List<Menu> menuData, Int64 ServiceId)
        {

            foreach (var m in menuData)
            {
                if (m.MenuId == 0)
                {
                    m.ParentMenuId = 0;
                    m.CssClass = "flaticon-line-graph";
                }

                if (m.MenuId == 116)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("Equipment", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = 0;
                    m.MenuLevel = 1;
                    m.MenuPosition = 1;
                    m.CssClass = "flaticon-truck";
                }
                if (m.MenuId == 117)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("Search", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = 116;
                    m.MenuLevel = 2;
                    m.CssClass = "m-menu__link-bullet--dot";
                }

                if (m.MenuId == 118)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("Meter History", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = 116;
                    m.MenuLevel = 2;
                    m.CssClass = "m-menu__link-bullet--dot";
                }
                if (m.MenuId == 119)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("Fuel", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = 116;
                    m.MenuLevel = 2;
                    m.CssClass = "m-menu__link-bullet--dot";
                }
                if (m.MenuId == 121)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("ScheduledService", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = ServiceId;
                    m.MenuLevel = 2;
                    m.MenuPosition = 3;
                    m.CssClass = "m-menu__link-bullet--dot";
                }
                if (m.MenuId == 122)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("Issues", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = ServiceId;
                    m.MenuLevel = 2;
                    m.MenuPosition = 1;
                    m.CssClass = "m-menu__link-bullet--dot";
                }
                if (m.MenuId == 123)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("ServiceOrders", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = ServiceId;
                    m.MenuLevel = 2;
                    m.MenuPosition = 2;
                    m.CssClass = "m-menu__link-bullet--dot";
                }
                if (m.MenuId == 21)
                {
                    m.MenuPosition = 3;
                }
                if (m.MenuId == 65)
                {
                    m.MenuPosition = 4;
                }
            }
        }

        private static void SetAsFleetAndCMMS(List<Menu> menuData)
        {
            foreach (var m in menuData)
            {
                if (m.MenuId == 0)
                {
                    m.ParentMenuId = 0;
                    m.CssClass = "flaticon-line-graph";
                }
                if (m.MenuId == 116)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("Fleet", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = 0;
                    m.MenuLevel = 1;
                    m.CssClass = "flaticon-truck";
                }
                if (m.MenuId == 117)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("Assets", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = 116;
                    m.MenuLevel = 2;
                    m.MenuPosition = 1;
                    m.CssClass = "m-menu__link-bullet--dot";
                }

                if (m.MenuId == 118)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("Meter History", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = 116;
                    m.MenuLevel = 2;
                    m.MenuPosition = 2;
                    m.CssClass = "m-menu__link-bullet--dot";
                }
                if (m.MenuId == 119)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("Fuel", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = 116;
                    m.MenuLevel = 2;
                    m.MenuPosition = 3;
                    m.CssClass = "m-menu__link-bullet--dot";
                }
                if (m.MenuId == 121)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("ScheduledService", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = 116;
                    m.MenuLevel = 2;
                    m.MenuPosition = 5;
                    m.CssClass = "m-menu__link-bullet--dot";
                }
                if (m.MenuId == 122)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("Issues", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = 116;
                    m.MenuLevel = 2;
                    m.MenuPosition = 4;
                    m.CssClass = "m-menu__link-bullet--dot";
                }
                if (m.MenuId == 123)
                {
                    m.LocalizedName = UtilityFunction.GetMessageFromResource("ServiceOrders", LocalizeResourceSetConstants.Menu);
                    m.ParentMenuId = 116;
                    m.MenuLevel = 2;
                    m.MenuPosition = 6;
                    m.CssClass = "m-menu__link-bullet--dot";
                }
            }
        }

        private static Int64 CreateMenu(ref List<Menu> menuData)
        {
            //--V2-408--//
            Menu menuService = new Menu();
            Int64 ServiceId = menuData.Max(a => a.MenuId);
            ServiceId += 1;
            menuService.LocalizedName = UtilityFunction.GetMessageFromResource("Service", LocalizeResourceSetConstants.Menu);
            menuService.MenuId = ServiceId;
            menuService.MenuName = "Service";
            menuService.ParentMenuId = 0;
            menuService.MenuLevel = 0;
            menuService.MenuUrl = "#";
            menuService.MenuPosition = 2;
            menuService.MenuType = "Site";
            menuService.InactiveFlag = false;
            menuService.ItemAccess = true;
            menuService.CssClass = "flaticon-layers";
            menuData.Add(menuService);

            return ServiceId;
        }

        //[AcceptVerbs("Get", "Post")]
        //public ActionResult LogInSSO([System.Web.Http.FromUri] string email, [System.Web.Http.FromUri] string emailType)
        //{
        //    UserInfoDetails objUser = new UserInfoDetails();
        //    objUser.UserEmail = email;
        //    objUser.UserName = "";
        //    objUser.ReturnUrl = "/Dashboard/Dashboard";
        //    VMLogin LogIn = new VMLogin();
        //    LoginWrapper LoginWrapper = new LoginWrapper(objUser);
        //    if (!string.IsNullOrEmpty(email))
        //    {
        //        if (objUser.RememberMe)
        //        {
        //            Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Expires = DateTime.Now.AddDays(30);
        //            Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Value = objUser.UserName;
        //        }
        //        else
        //        {
        //            Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Expires = DateTime.Now.AddDays(-1);
        //            Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Value = null;
        //        }
        //        LogIn = LoginWrapper.GetLogInSSO( emailType);
        //        objUser.UserName = objUser.UserName;
        //        if (LogIn.IsAuthenticated)
        //        {
        //            string serializeModel = JsonConvert.SerializeObject(LogIn.UserInfoDetails);
        //            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, LogIn.UserInfoDetails.UserName, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout), objUser.RememberMe, serializeModel);
        //            string encTicket = FormsAuthentication.Encrypt(ticket);
        //            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        //            Response.Cookies.Add(faCookie);
        //            var menuReturnList = GetMenuList("Site");
        //            if (LogIn.UIVersionRedirectToV1 == true)
        //            {
        //                string remoteUrl = System.Configuration.ConfigurationManager.AppSettings["V1RedirectURL"].ToString();
        //                string encuname = WebSecurity.Encrypt(LogIn.UserInfoDetails.UserName, "somaxsom");
        //                string encpwd = WebSecurity.Encrypt(LogIn.UserInfoDetails.Password, "somaxsom");

        //                String htmlUname = encuname;
        //                String htmlPwd = encpwd;
        //                StringWriter writerU = new StringWriter();
        //                StringWriter writerP = new StringWriter();
        //                Server.UrlEncode(htmlUname, writerU);
        //                Server.UrlEncode(htmlPwd, writerP);
        //                String uString = writerU.ToString();
        //                String pString = writerP.ToString();
        //                Response.Redirect(remoteUrl + "uName=" + uString + "&pwd=" + pString, false);
        //                //Response.Redirect(remoteUrl + "uName=" + encuname + "&pwd=" + encpwd);
        //            }
        //            if (!(menuReturnList.Count == 0 || menuReturnList.Count < 0))
        //            {
        //                Session["MenuDetails"] = menuReturnList;
        //                return RedirectToLocal(objUser.ReturnUrl);
        //            }
        //            else
        //            {
        //                return RedirectToAction("SomaxLogin");
        //            }
        //        }
        //        else
        //        {
        //            SiteMaintenanceWrapper objSiteMainWrapper = new SiteMaintenanceWrapper();
        //            var mydata = objSiteMainWrapper.GetNextSitemaintenancewithoutLogin("y");
        //            ViewBag.SystemUnderMaintenanceMessage = GetSiteMaintenanceMessage(mydata, objSiteMainWrapper);
        //        }
        //    }
        //    else
        //    {

        //    }
        //    LoginCacheSet _logCache = new LoginCacheSet();
        //    objUser.FailureMessage = LogIn.UserInfoDetails.FailureMessage;
        //    objUser.FailureReason = LogIn.UserInfoDetails.FailureReason;
        //    var localizationCacheTest = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Login, "en-us");
        //    objUser.localization = localizationCacheTest;
        //    return View("SomaxLogIn", objUser);

        //}
        #region V2-332

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(UserInfoDetails objUser)  //Sign In on click
        {
            VMLogin LogIn = new VMLogin();
            if (ModelState.IsValid)
            {
                LoginWrapper LoginWrapper = new LoginWrapper(objUser);
                if (objUser.RememberMe)
                {
                    Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Value = objUser.UserName;
                }
                else
                {
                    Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Value = null;
                }
                if (objUser.ClientUserInfoListId > 0)
                {
                    objUser.ClientUserInfoListId = LoginWrapper.UpdateCustomForSomaxAdminDetails((objUser.ClientUserInfoListId ?? 0), objUser.UserInfoId ?? 0) == 0 ? null : objUser.ClientUserInfoListId;//updating record V2-911

                }
                LogIn = LoginWrapper.GetLogIn();
                SetIsLoggedInFromMobileFlag(objUser.IsLoggedInFromMobile);

                if (LogIn.IsAuthenticated)
                {                   
                    if (LogIn.IsPasswordExpired)
                    {
                        return PartialView("~/Views/Login/_PasswordExpired.cshtml", LogIn);
                    }

                    Session[SessionConstants.LOGGED_USERNAME] = objUser.UserName;
                    Session[SessionConstants.LOGGED_PASSWORD] = objUser.Password;


                    IdentitySignin(LogIn.UserInfoDetails, objUser.RememberMe);
                    //-----Call getmenu list here and keep the list in session or temp data which one is better----//
                    var menuReturnList = GetMenuList("Site");
                    if (LogIn.UIVersionRedirectToV1 == true)
                    {
                        RedirectToV1(LogIn.UserInfoDetails);
                    }
                    else
                    {
                        if (LogIn.IsMatchTempPassword) /*V2-332*/
                        {
                            return RedirectToAction("CreatePassword", new { userId = LogIn.UserInfoDetails.UserName.Encrypt(), IsResetPassword = LogIn.IsResetPassword, email = LogIn.UserInfoDetails.UserEmail.Encrypt() });
                        }
                        else if (LogIn.UserInfoDetails.IsSOMAXAdmin == true && objUser.ClientUserInfoListId == null)
                        {
                            TempData["UserInfoDetails"] = objUser;
                            return RedirectToAction("SomaxLogin");
                        }
                        else if (!(menuReturnList.Count == 0 || menuReturnList.Count < 0))
                        {
                            Session["MenuDetails"] = menuReturnList;
                            return RedirectToLocal(objUser.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("SomaxLogin");
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
            return View("SomaxLogIn", objUser);
        }
        public ActionResult CreatePassword(string userId, bool IsResetPassword, string email = "", bool IsPasswordExpired = false)
        {
            //----- IE browser
            string userAgent = Request.Headers["User-Agent"].ToString();
            if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
            {
                return View("~/Views/LogIn/_IsIEBrowser.cshtml");
            }
            //----- IE browser
            LoginWrapper loginWrapper = new LoginWrapper();
            CreatePassword createPassword = new CreatePassword();
            LoginInfo loginInfo = new LoginInfo();

            if (!string.IsNullOrEmpty(userId))
            {
                userId = userId.Decrypt();
                if (!string.IsNullOrEmpty(email))
                {
                    email = email.Decrypt();
                }
                var v = loginWrapper.GetUserDataForPasswordCreate(userId);
                createPassword.UserName = v.LoginInfo.UserName;
                createPassword.TempPassword = Convert.ToString(v.LoginInfo.TempPassword);
                createPassword.PasswordCode = Convert.ToString(v.LoginInfo.ResetPasswordCode);
                createPassword.ResetPasswordRequestDate = v.LoginInfo.ResetPasswordRequestDate;
                createPassword.UserMail = email;
                createPassword.SecurityQuestion = v.LoginInfo.SecurityQuestion;
                createPassword.SecurityAnswer = v.LoginInfo.SecurityResponse;
                createPassword.IsResetPassword = IsResetPassword;
                createPassword.IsPasswordExpired = IsPasswordExpired;
                createPassword.UserInfoId = v.LoginInfo.UserInfoId;
                createPassword.LoginInfoId = v.LoginInfo.LoginInfoId;
                var secQuestList = SecurityQuestConstant.SecurityQuestValues();
                if (secQuestList != null)
                {
                    createPassword.SecurityQuestList = secQuestList.Select(x => new SelectListItem { Text = x.text, Value = x.text });
                }

                //-------V2 - 491
                var passwordSettings = loginWrapper.RetrievePasswordSettings(v.LoginInfo.ClientId);
                if (passwordSettings != null)
                {
                    createPassword.PWReqMinLength = passwordSettings.PWReqMinLength;
                    createPassword.PWMinLength = passwordSettings.PWMinLength;
                    createPassword.PWRequireNumber = passwordSettings.PWRequireNumber;
                    createPassword.PWRequireAlpha = passwordSettings.PWRequireAlpha;
                    createPassword.PWRequireMixedCase = passwordSettings.PWRequireMixedCase;
                    createPassword.PWRequireSpecialChar = passwordSettings.PWRequireSpecialChar;
                    createPassword.PWNoRepeatChar = passwordSettings.PWNoRepeatChar;
                    createPassword.PWNotEqualUserName = passwordSettings.PWNotEqualUserName;
                }
                //------- V2-491
            }
            LoginCacheSet _logCache = new LoginCacheSet();
            var localizationCacheTest = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Login, "en-us");
            createPassword.localization = localizationCacheTest;
            return PartialView("~/Views/LogIn/_NewUserPasswordCreate.cshtml", createPassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreatePassword(CreatePassword createPassword)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                LoginWrapper loginWrapper = new LoginWrapper();
                var result = loginWrapper.CreateNewPassword(createPassword);
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

        private void RedirectToV1(UserInfoDetails userInfoDetails)
        {
            string remoteUrl = System.Configuration.ConfigurationManager.AppSettings["V1RedirectURL"].ToString();
            string encuname = WebSecurity.Encrypt(userInfoDetails.UserName, "somaxsom");
            string encpwd = WebSecurity.Encrypt(userInfoDetails.Password, "somaxsom");

            String htmlUname = encuname;
            String htmlPwd = encpwd;
            StringWriter writerU = new StringWriter();
            StringWriter writerP = new StringWriter();
            Server.UrlEncode(htmlUname, writerU);
            Server.UrlEncode(htmlPwd, writerP);
            String uString = writerU.ToString();
            String pString = writerP.ToString();
            Response.Redirect(remoteUrl + "uName=" + uString + "&pwd=" + pString, false);
        }
        #endregion

        #region UnderMaintenanceAction
        public ActionResult UnderMaintenencePage()
        {
            var data = System.Web.HttpContext.Current.Session["userData"];
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
                // RKL - the state and end date are in easter (to match V1)
                // If we end up storing the start/end date in UTC then we don't have to convert to UTC
                // Working with local for the messaging
                DateTime start_local = TimeZoneInfo.ConvertTimeToUtc(mydata.DowntimeStart, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToLocalTime();
                DateTime end_local = TimeZoneInfo.ConvertTimeToUtc(mydata.DowntimeEnd, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToLocalTime();
                if (start_local.Date == end_local.Date && start_local.Date != DateTime.Now.Date)
                {
                    maintenanceMessage = string.Format("{0} from {1} to {2} on {3}",
                                            mydata.LoginPageMessage,
                                            start_local.ToLocalTime().ToString("t"),
                                            end_local.ToLocalTime().ToString("t"),
                                            start_local.ToLocalTime().ToString("D"));
                }
                else if (start_local.Date == end_local.Date && start_local.Date == DateTime.Now.Date)
                {
                    maintenanceMessage = string.Format("{0} today from {1} to {2}",
                                            mydata.LoginPageMessage,
                                            start_local.ToLocalTime().ToString("t"),
                                            end_local.ToLocalTime().ToString("t"));
                }
                else
                {
                    maintenanceMessage = string.Format("{0} from {1} to {2}",
                                            mydata.LoginPageMessage,
                                            start_local.ToLocalTime().ToString("f"),
                                            end_local.ToLocalTime().ToString("f"));
                }
                /*  
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
                */
            }
            return maintenanceMessage;
        }
        #endregion


        #region External-Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl, bool IsLoggedInFromMobile = false)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Login", new { ReturnUrl = returnUrl, IsLoggedInFromMobile }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl, bool IsLoggedInFromMobile = false)
        {
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = "/Dashboard/Dashboard";

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                IdentitySignout();
                return RedirectToAction("SomaxLogin");
            }

            // AUTHENTICATED!
            var name = loginInfo.ExternalIdentity.Name;
            var provider = loginInfo.Login.LoginProvider;
            var providerKey = loginInfo.Login.ProviderKey;
            var email = loginInfo.Email;

            VMLogin LogIn = new VMLogin();
            UserInfoDetails objUser = new UserInfoDetails();
            objUser.UserEmail = loginInfo.Email;
            objUser.UserName = "";

            objUser.ReturnUrl = returnUrl;
            LoginWrapper LoginWrapper = new LoginWrapper(objUser);
            if (provider.ToUpper() == "GOOGLE")
            {
                LogIn = LoginWrapper.GetLogInSSO("Gmail", loginInfo.Email);
            }
            else if (provider.ToUpper() == "MICROSOFT")
            {
                LogIn = LoginWrapper.GetLogInSSO("Microsoft", loginInfo.Email);
            }
            SetIsLoggedInFromMobileFlag(IsLoggedInFromMobile);
            if (LogIn.IsAuthenticated)
            {
                bool IsRedirect = LogIn.IsPasswordExpired || LogIn.UIVersionRedirectToV1 || LogIn.IsMatchTempPassword;
                if (IsRedirect)
                {
                    IdentitySignout();
                }
                if (LogIn.IsPasswordExpired)
                {
                    return PartialView("~/Views/Login/_PasswordExpired.cshtml", LogIn);
                }
                if (LogIn.UIVersionRedirectToV1 == true)
                {
                    RedirectToV1(LogIn.UserInfoDetails);
                }
                else if (LogIn.IsMatchTempPassword) /*V2-332*/
                {
                    return RedirectToAction("CreatePassword",
                        new { userId = LogIn.UserInfoDetails.UserName.Encrypt(), IsResetPassword = LogIn.IsResetPassword, email = LogIn.UserInfoDetails.UserEmail.Encrypt() });
                }
                else
                {
                    Session[SessionConstants.LOGGED_USERNAME] = objUser.UserName;
                    Session[SessionConstants.LOGGED_PASSWORD] = objUser.Password;
                    var menuReturnList = GetMenuList("Site");
                    if (!(menuReturnList.Count == 0 || menuReturnList.Count < 0))
                    {
                        Session["MenuDetails"] = menuReturnList;
                        IdentitySignin(LogIn.UserInfoDetails, false);
                        return RedirectToLocal(objUser.ReturnUrl);
                    }
                    else
                    {
                        IdentitySignout();
                        return RedirectToAction("SomaxLogin");
                    }
                }
            }
            else
            {
                ExternalLoginUnaothorizedModel unAuthData = new ExternalLoginUnaothorizedModel();
                unAuthData.Provider = provider;
                unAuthData.Email = email;
                unAuthData.Name = name;
                unAuthData.ErrorMessage = LogIn.UserInfoDetails.FailureMessage;
                TempData["UNAUTHDATA"] = unAuthData;
                return RedirectToAction("UnAuthError", "error");
            }

            LoginCacheSet _logCache = new LoginCacheSet();
            objUser.FailureMessage = LogIn.UserInfoDetails.FailureMessage;
            objUser.FailureReason = LogIn.UserInfoDetails.FailureReason;
            var localizationCacheTest = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Login, "en-us");
            objUser.localization = localizationCacheTest;
            return View("SomaxLogIn", objUser);
        }

        public void IdentitySignin(UserInfoDetails appUserState, bool isPersistent = false)
        {
            var claims = new List<Claim>();

            // create required claims
            claims.Add(new Claim(ClaimTypes.NameIdentifier, appUserState.PersonnelId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, appUserState.UserName));

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

        private void SetIsLoggedInFromMobileFlag(bool IsLoggedInFromMobile)
        {
            if (Session["userData"] != null)
            {
                var userData = (UserData)Session["userData"];
                userData.IsLoggedInFromMobile = IsLoggedInFromMobile;
                Session["userData"] = userData;
            }
        }       
        #region V2-911
        [HttpPost]
        public string RetrieveByUserInfoIdChunkSearchLookupList(int? draw, int? start, int? length, long UserInfoId, string UserName, string ClientName = "", string SiteName = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            LoginWrapper eWrapper = new LoginWrapper();
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            var Result = eWrapper.RetrieveByUserInfoIdChunkSearchLookupList(UserInfoId, UserName, ClientName, SiteName, order, orderDir, length ?? 0, skip);

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            if (Result != null && Result.Count > 0)
            {
                recordsFiltered = Result[0].totalCount;
                totalRecords = Result[0].totalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = Result
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult });
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

        #region V2-926 Windows AD Auth
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginUsingWindowsAD(UserInfoDetails objUser)
        {
            VMLogin LogIn = new VMLogin();
            string ErrorMessage = "";
            if (ModelState.IsValid)
            {
                LoginWrapper LoginWrapper = new LoginWrapper(objUser);
                if (objUser.RememberMe)
                {
                    Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Value = objUser.UserName;
                }
                else
                {
                    Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies[CookiesConstants.SOMAX_LOGIN_ID].Value = null;
                }
                //login
                string UN = objUser.UserName;
                objUser.UserName = FormatUserNameForWindowsAD(objUser.UserName);
                bool IsAuthenticated = PasswordMatchForWindowsAD(objUser.UserName, objUser.Password, ref ErrorMessage);
                //login
                objUser.UserEmail = objUser.UserName;
                objUser.UserName = "";
                if (IsAuthenticated)
                {
                    LogIn = LoginWrapper.GetLogInSSO("WindowsAD", objUser.UserEmail);
                    
                    if (!string.IsNullOrEmpty(LogIn.UserInfoDetails.FailureMessage))
                    {
                        ErrorMessage = LogIn.UserInfoDetails.FailureMessage;
                    }
                }
                if (IsAuthenticated && string.IsNullOrEmpty(ErrorMessage))
                {
                    SetIsLoggedInFromMobileFlag(objUser.IsLoggedInFromMobile);                  
                    bool IsRedirect = LogIn.IsPasswordExpired || LogIn.UIVersionRedirectToV1 || LogIn.IsMatchTempPassword;
                    if (IsRedirect)
                    {
                        IdentitySignout();
                    }

                    if (LogIn.IsPasswordExpired)
                    {
                        return PartialView("~/Views/Login/_PasswordExpired.cshtml", LogIn);
                    }
                    if (LogIn.UIVersionRedirectToV1 == true)
                    {
                        RedirectToV1(LogIn.UserInfoDetails);
                    }
                    else if (LogIn.IsMatchTempPassword) /*V2-332*/
                    {
                        return RedirectToAction("CreatePassword",
                            new { userId = LogIn.UserInfoDetails.UserName.Encrypt(), IsResetPassword = LogIn.IsResetPassword, email = LogIn.UserInfoDetails.UserEmail.Encrypt() });
                    }
                    else
                    {

                        Session[SessionConstants.LOGGED_USERNAME] = objUser.UserName;
                        Session[SessionConstants.LOGGED_PASSWORD] = objUser.Password;

                        var menuReturnList = GetMenuList("Site");

                        if (!(menuReturnList.Count == 0 || menuReturnList.Count < 0))
                        {
                            Session["MenuDetails"] = menuReturnList;
                            IdentitySignin(LogIn.UserInfoDetails, false);
                            return RedirectToLocal(objUser.ReturnUrl);
                        }
                        else
                        {
                            IdentitySignout();
                            return RedirectToAction("SomaxLogin");
                        }
                    }
                }
                //else
                //{
                //    ExternalLoginUnaothorizedModel unAuthData = new ExternalLoginUnaothorizedModel();
                //    unAuthData.Provider = provider;
                //    unAuthData.Email = email;
                //    unAuthData.Name = name;
                //    unAuthData.ErrorMessage = LogIn.UserInfoDetails.FailureMessage;
                //    TempData["UNAUTHDATA"] = unAuthData;
                //    // IdentitySignout();
                //    return RedirectToAction("UnAuthError", "error");
                //}
            }
            LoginCacheSet _logCache = new LoginCacheSet();
            objUser.FailureMessage = ErrorMessage;
            //objUser.FailureReason = LogIn.UserInfoDetails.FailureReason;
            var localizationCacheTest = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Login, "en-us");
            objUser.localization = localizationCacheTest;
            return View("SomaxLogIn", objUser);
        }
        private bool PasswordMatchForWindowsAD(string UserName, string Password, ref string ErrorMessage)
        {
            var Server = ConfigurationManager.AppSettings["LDAPServerForWindowsAD"].ToString(); // "3.12.235.171:389"; 
            bool IsAuthenticated = false;
            using (var connection = new LdapConnection(new LdapDirectoryIdentifier(Server)))
            {
                var SSLEnabled = ConfigurationManager.AppSettings["LDAPServerSSLEnabled"];
                if (SSLEnabled != null && Convert.ToBoolean(SSLEnabled))
                {
                    connection.SessionOptions.SecureSocketLayer = true;
                    connection.SessionOptions.ProtocolVersion = 3;
                    connection.AuthType = AuthType.Basic;
                    connection.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback((con, cer) => true);
                }
                try
                {
                    connection.Bind(new NetworkCredential(UserName.Trim(), Password));
                    IsAuthenticated = true;
                }
                catch (Exception ex)
                {
                    IsAuthenticated = false;
                    ErrorMessage = ex.Message;
                }
                connection.Dispose();
            }
            return IsAuthenticated;
        }
        private string FormatUserNameForWindowsAD(string UserName)
        {
            var regex = new Regex("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$");
            Match match = regex.Match(UserName);
            if (!match.Success)
            {
                string DomainName = ConfigurationManager.AppSettings["LDAPServerDomain"];
                if (!string.IsNullOrEmpty(DomainName) && !DomainName.Contains("@"))
                {
                    UserName = UserName + "@" + DomainName;
                }
            }
            return UserName;
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