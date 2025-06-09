using Business.Authentication;
using Common.Constants;
using Common.Enumerations;
using DataContracts;
using SomaxMVC.Models;
using SomaxMVC.ViewModels;
using System.Web;
using Presentation.Common;
using System;
using System.Text;
using Business.Common;
using Client.Models;
using System.IO;
using RazorEngine;
using Client.Common;
using Client.BusinessWrapper.Common;
using Client.Models.Common;
using System.Collections.Generic;

namespace SomaxMVC.BusinessWrapper
{
    public class LoginWrapper
    {
        UserInfoDetails _objUser;

        public LoginWrapper(UserInfoDetails objUser)
        {
            _objUser = objUser;
        }
        public LoginWrapper()
        {

        }
        //public VMLogin GetLogIn()
        //{
        //    VMLogin objVMLogin = new VMLogin();
        //    Authentication auth = new Authentication()
        //    {
        //        UserName = _objUser.UserName.Trim(),
        //        Password = _objUser.Password,
        //        website = WebSiteEnum.Client,
        //        BrowserInfo = HttpContext.Current.Request.Browser.Type + " " + HttpContext.Current.Request.Browser.Version,
        //        IpAddress = HttpContext.Current.Request.UserHostAddress
        //    };
        //    auth.VerifyLogin();
        //    if (auth.IsAuthenticated)
        //    {
        //        // store the newly created session id into cookie
        //        Cookie.Set(CookiesConstants.SOMAX_USER, auth.SessionId.ToString());
        //        // store the newly created session id into session
        //        UserSession.SessionId = auth.SessionId;
        //        DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
        //        auth.UserData = new UserData() { SessionId = auth.SessionId, WebSite = WebSiteEnum.Client };
        //        auth.UserData.Retrieve(dbKey);
        //        auth.UserData.DatabaseKey.Pwd = WebSecurity.Encrypt(auth.Password, "somaxsom");
        //        System.Web.HttpContext.Current.Session["userData"] = auth.UserData;
        //        if (auth.UserData.DatabaseKey.User.UserType == UserTypeConstants.Reference)
        //        {
        //            objVMLogin.UserInfoDetails = _objUser;
        //            objVMLogin.UserInfoDetails.FailureMessage = "User is not authorized to login to the system";//auth.FailureMessage;
        //            objVMLogin.UserInfoDetails.FailureReason = "UnAuthorisedMsg";//auth.FailureReason;
        //            return objVMLogin;
        //        }
        //        CheckUIVersion(auth, objVMLogin);
        //        objVMLogin.IsAuthenticated = true;
        //    }

        //    objVMLogin.UserInfoDetails = _objUser;
        //    objVMLogin.UserInfoDetails.FailureMessage = auth.FailureMessage;
        //    objVMLogin.UserInfoDetails.FailureReason = auth.FailureReason;
        //    if (auth.UserData != null)
        //    {
        //        objVMLogin.UserInfoDetails.UserType = new string[] { auth.UserData.DatabaseKey.User.UserType };
        //    }

        //    return objVMLogin;
        //}

        public VMLogin GetLogInFromV1Web()
        {
            VMLogin objVMLogin = new VMLogin();
            Authentication auth = new Authentication()
            {
                UserName = _objUser.UserName,
                Password = _objUser.Password,
                website = WebSiteEnum.Client,
                BrowserInfo = HttpContext.Current.Request.Browser.Type + " " + HttpContext.Current.Request.Browser.Version,
                IpAddress = HttpContext.Current.Request.UserHostAddress
            };
            auth.VerifyLogin();

            if (auth.IsAuthenticated)
            {
                // store the newly created session id into cookie
                Cookie.Set(CookiesConstants.SOMAX_USER, auth.SessionId.ToString());
                // store the newly created session id into session
                UserSession.SessionId = auth.SessionId;
                DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
                auth.UserData = new UserData() { SessionId = auth.SessionId, WebSite = WebSiteEnum.Client };
                auth.UserData.Retrieve(dbKey);
                auth.UserData.DatabaseKey.Pwd = WebSecurity.Encrypt(auth.Password, "somaxsom");
                System.Web.HttpContext.Current.Session["userData"] = auth.UserData;
                _objUser.PersonnelId = auth.UserData.DatabaseKey.Personnel.PersonnelId;
                HttpContext.Current.Session["InterfacePropData"] = RetrieveAllInterfacePropertiesByClientIdSiteId();
                if (auth.UserData.DatabaseKey.User.UserType == UserTypeConstants.Reference)
                {
                    objVMLogin.UserInfoDetails = _objUser;
                    objVMLogin.UserInfoDetails.FailureMessage = ErrorMessageConstants.Unauthorised_Msg;//"User is not authorized to login to the system";//auth.FailureMessage;
                    objVMLogin.UserInfoDetails.FailureReason = FailureReasonConstant.UnAuthorisedMsg;// "UnAuthorisedMsg";//auth.FailureReason;
                    return objVMLogin;
                }
                objVMLogin.IsAuthenticated = true;
            }

            objVMLogin.UserInfoDetails = _objUser;
            objVMLogin.UserInfoDetails.FailureMessage = auth.FailureMessage;
            objVMLogin.UserInfoDetails.FailureReason = auth.FailureReason;
            if (auth.UserData != null)
            {
                objVMLogin.UserInfoDetails.UserType = new string[] { auth.UserData.DatabaseKey.User.UserType };
            }

            return objVMLogin;
        }
        private void CheckUIVersion(Authentication auth, VMLogin objVMLogin)
        {
            if (!string.IsNullOrEmpty(auth.UserData.Site.UIVersion))//-- Site UIVersion checking //---SOM-1656---//
            {
                switch (auth.UserData.Site.UIVersion)
                {
                    case SiteUIVersion.V1:
                        objVMLogin.UIVersionRedirectToV1 = true;//--V1 redirect--//
                        break;
                    case SiteUIVersion.V2:
                        objVMLogin.UIVersionRedirectToV1 = false;//--V2 redirect--//
                        break;
                    case SiteUIVersion.Both:
                        switch (auth.UserData.DatabaseKey.User.UIVersion)//--UserInfo UIVersion checking
                        {
                            case UserInfoUIVersion.V1://--V1 redirect--//
                                objVMLogin.UIVersionRedirectToV1 = true;
                                break;
                            case UserInfoUIVersion.V2:
                                objVMLogin.UIVersionRedirectToV1 = false;//--V2 redirect--//
                                break;
                            default:
                                objVMLogin.UIVersionRedirectToV1 = false;
                                break;
                        }
                        break;
                    default:
                        objVMLogin.UIVersionRedirectToV1 = false;//--V1 redirect--//
                        break;
                }
            }
        }
        public bool SendForgetUserIdByMail(string mailId)
        {
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            // RKL - 2024-Aug-22
            // We do not need to exclude SOMAXAdmin users
            //user type somaxadmin is for admin user..
            /*
            UserInfo userInfo = new UserInfo { UserName = mailId, Email = mailId };
            userInfo.RetrieveUserType(dbKey);
            if (userInfo.UserType == UserTypeConstants.SOMAXAdmin)
            {
                return false;
            }
            */

            LoginInfo LoginInfo = new LoginInfo() { UserName = mailId, Email = mailId };
            LoginInfo.RetrieveBySearchCriteria(dbKey);
            var result = false;

            if (LoginInfo.LoginInfoId > 0)
            {
                try
                {
                    result = SendForgotLoginIdEmail(LoginInfo.UserName, LoginInfo.Email, dbKey);

                }
                catch (Exception ex)
                {

                }
            }
            return result;
        }

        private bool SendForgotLoginIdEmail(string UserName, string EmailAddress, DatabaseKey dbKey)
        {
            string strScheme = HttpContext.Current.Request.Url.Scheme;//return http or https
            string strAuthority = HttpContext.Current.Request.Url.Authority;// host name localhost:80
            //SOM-1192
            string LoginUrl = string.Format("{0}://{1}", strScheme, strAuthority);
            StringBuilder body = new StringBuilder();

            string Emailbody = string.Empty;
            var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Views\Login\ResetIdMailTemplate.cshtml");
            var templateContent = string.Empty;
            using (var reader = new StreamReader(templateFilePath))
            {
                templateContent = reader.ReadToEnd();
            }
            string emailHtmlBody = ParseTemplate(templateContent);

            UserData userData = new UserData();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string resetUrl = commonWrapper.GetHostedUrl();

            string output = emailHtmlBody.Replace("firstname", UserName).
                            Replace("headerBgURL", resetUrl + SomaxAppConstants.HeaderMailTemplate).
                            Replace("somaxLogoURL", resetUrl + SomaxAppConstants.SomaxLogoForMailTemplate).
                            Replace("username", UserName).Replace("spnloginurl", resetUrl).
                            Replace("footerBgURL", resetUrl + SomaxAppConstants.FooterMailTemplate);
            body.Append(output);

            string Subject = "Your SOMAX Online Account";
            //Email email = new Email() { Body = body.ToString(), Subject = Subject };
            EmailModule email = new Presentation.Common.EmailModule() { Body = body.ToString(), Subject = Subject };
            email.Recipients.Add(EmailAddress);
            email.MailBody = body.ToString();
            try
            {
                email.SendEmail();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        } //Som-1192

        public static string ParseTemplate(string content)
        {
            string _mode = DateTime.Now.Ticks + new Random().Next(1000, 100000).ToString();
            Razor.Compile(content, _mode);
            return Razor.Parse(content);
        }


        public bool SendForgetPasswordResend(string userId)
        {  //Exit function if the username is null
           //if (string.IsNullOrEmpty(txtForgotPasswordLoginId.Text)) { return; }

            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            UserInfo userInfo = new UserInfo { UserName = userId, Email = userId };
            // RKL - 2024-Aug-22
            // We do not need to exclude SOMAXAdmin users
            //user type somaxadmin is for admin user..
            /*
            userInfo.RetrieveUserType(dbKey);
            if (userInfo.UserType == UserTypeConstants.SOMAXAdmin)
            {
                return false;
            }
            */
            AccountPassword password = new AccountPassword();
            IP_AccountPasswordData ipData = new IP_AccountPasswordData()
            {
                DbKey = dbKey,
                UserName = userId
            };

            // If the username is found, then send an email to user with a temporary password and a link for resetting password.
            password.RequestNewPassword(ipData);
            var result = false;
            try
            {
                result = SendResetPasswordEmail(password, dbKey);   //for change passowrd link and temporary passowrd

                userInfo = new UserInfo() { ClientId = password.LoginInfo.ClientId, UserInfoId = password.LoginInfo.UserInfoId };
                userInfo.Retrieve(dbKey);
            }
            catch (Exception ex)
            {

            }
            return result;
            //Always tell the user it was successful, because we don't want hackers to try to guess the users of the system.
            // lblMessage.Text = loc.Global.RequestPasswordSuccess;
        }

        private bool SendResetPasswordEmail(AccountPassword password, DatabaseKey dbKey)
        {
            UserInfo userInfo = new UserInfo() { ClientId = password.LoginInfo.ClientId, UserInfoId = password.LoginInfo.UserInfoId };
            userInfo.Retrieve(dbKey);

            // Build the url of the reset password page
            string[] url = new string[5];

            //SOM-1192
            string strScheme = HttpContext.Current.Request.Url.Scheme;//return http or https
            string strAuthority = HttpContext.Current.Request.Url.Authority;// host name localhost:80

            url[0] = strScheme;
            url[1] = strAuthority;
            url[2] = VirtualPathUtility.ToAbsolute("~/LogIn/ResetPassword");
            url[3] = QueryStringConstants.RESET_PASSWORD;
            url[4] = password.LoginInfo.ResetPasswordCode.ToString();

            string resetUrl = string.Format("{0}://{1}{2}?{3}={4}", url);
            UserData userData = new UserData();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string resetBgUrl = commonWrapper.GetHostedUrl();
            //--for footer part--
            //string resetUrlFooter = commonWrapper.GetHostedUrl();

            StringBuilder body = new StringBuilder();
            string Emailbody = string.Empty;
            var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Views\Login\PasswordResetMailTemplate.cshtml");
            var templateContent = string.Empty;
            using (var reader = new StreamReader(templateFilePath))
            {
                templateContent = reader.ReadToEnd();
            }
            string emailHtmlBody = ParseTemplate(templateContent);

            string output = emailHtmlBody.
                            //Replace("firstname", UserName).
                            Replace("headerBgURL", resetBgUrl + SomaxAppConstants.HeaderMailTemplate).
                            Replace("somaxLogoURL", resetBgUrl + SomaxAppConstants.SomaxLogoForMailTemplate).
                            Replace("spnloginurl", resetUrl).
                            Replace("spnPassword", password.TempPassword).
                            Replace("footerBgURL", resetBgUrl + SomaxAppConstants.FooterMailTemplate).
                            Replace("contactbase", resetBgUrl);
            body.Append(output);

            //Email email = new Email() { Body = body.ToString(), Subject = "Reset Password" };
            EmailModule email = new Presentation.Common.EmailModule() { Body = body.ToString(), Subject = "Reset Password" };
            email.Recipients.Add(userInfo.Email);
            email.MailBody = body.ToString();
            // email.ToEmailAddress = userInfo.Email;
            try
            {
                email.SendEmail();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool ResetPassword(ResetPassword resetPassword)
        {
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();

            IP_AccountPasswordData ipData = new IP_AccountPasswordData()
            {
                DbKey = dbKey,
                // Localization = ClientLocalization.Global,
                NewPassword = resetPassword.NewPassword,
                ConfirmPassword = resetPassword.ConfirmPassword,
                SecurityAnswer = resetPassword.SecurityAnswer,
                TempPassword = resetPassword.TempPassword,
                PasswordCode = resetPassword.PasswordCode
            };
            AccountPassword password = new AccountPassword();
            //--generating password changed confirmation mail body------
            UserData userData = new UserData();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string resetBgUrl = commonWrapper.GetHostedUrl();

            StringBuilder body = new StringBuilder();
            string Emailbody = string.Empty;
            var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Views\Login\PasswordChangeConfirmMailTemplate.cshtml");
            var templateContent = string.Empty;
            using (var reader = new StreamReader(templateFilePath))
            {
                templateContent = reader.ReadToEnd();
            }
            string emailHtmlBody = ParseTemplate(templateContent);

            string output = emailHtmlBody.
                            Replace("headerBgURL", resetBgUrl + SomaxAppConstants.HeaderMailTemplate).
                            Replace("somaxLogoURL", resetBgUrl + SomaxAppConstants.SomaxLogoForMailTemplate).
                            Replace("footerBgURL", resetBgUrl + SomaxAppConstants.FooterMailTemplate).
                            Replace("contactbase", resetBgUrl);
            body.Append(output);

            if (password.Reset(ipData, body))
            {
                return true;//litMessage.Text = password.Message;
            }
            else
            {
                return false;// litFailureMessage.Text = password.Message;
            }
        }

        public LoginInfo ResetPasswordData(Guid code)
        {
            LoginInfo loginInfo = new LoginInfo();
            UserInfo userInfo = new UserInfo();

            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();

            // Retrieve LoginInfo if the query string is a guid
            loginInfo.ResetPasswordCode = code;
            loginInfo.RetrieveByResetPasswordCode(dbKey);

            if (loginInfo.UserInfoId > 0)
            {
                dbKey.Client.ClientId = loginInfo.ClientId;
                userInfo = new UserInfo() { ClientId = loginInfo.ClientId, UserInfoId = loginInfo.UserInfoId };
                userInfo.Retrieve(dbKey);
            }
            return loginInfo;
        }

        #region V2-332

        public VMLogin GetLogIn()
        {
            VMLogin objVMLogin = new VMLogin();
            Authentication auth = new Authentication()
            {
                UserName = _objUser.UserName.Trim(),
                Password = _objUser.Password,
                website = WebSiteEnum.Client,
                BrowserInfo = HttpContext.Current.Request.Browser.Type + " " + HttpContext.Current.Request.Browser.Version,
                IpAddress = HttpContext.Current.Request.UserHostAddress
            };

            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            auth.VerifyLogin();
            return AuthenticateLogin(auth, dbKey);
        }

        public VMLogin GetLogInSSO(string emailType, string email = "")
        {

            Authentication auth = new Authentication()
            {
                UserName = _objUser.UserEmail,
                Password = _objUser.Password,
                website = WebSiteEnum.Client,
                BrowserInfo = HttpContext.Current.Request.Browser.Type + " " + HttpContext.Current.Request.Browser.Version,
                IpAddress = HttpContext.Current.Request.UserHostAddress
            };

            LoginSSO sso = new LoginSSO();
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            sso.EmailId = email; // pass email id
            sso.EmailType = emailType; // pass dynamic value
            sso.RetrieveByEmail(dbKey);
            if (sso.UserInfoId != 0)
            {
                auth.VerifyLoginSSO(sso.UserInfoId);
                if (auth.IsAuthenticated && auth.LoginInfo != null)
                {
                    _objUser.UserName = auth.LoginInfo.UserName;
                }
                //auth.VerifyLogin();
                //-------End V2-542 -------//

                return AuthenticateLoginsSO(auth, dbKey, sso.LoginSSOId);
            }
            else
            {
                VMLogin objVMLogin = new VMLogin();
                if (emailType == "WindowsAD")
                {
                    _objUser.FailureMessage = ErrorMessageConstants.UserNotRegistered;
                }
                else
                {
                    _objUser.FailureMessage = ErrorMessageConstants.UNAUTHORIZEDGOOGLEACCOUNT;
                }
                _objUser.FailureReason = FailureReasonConstant.UnAuthorisedMsg;
                objVMLogin.UserInfoDetails = _objUser;
                return objVMLogin;
            }

        }
        private VMLogin AuthenticateLoginsSO(Authentication auth, DatabaseKey dbKey,Int64 loginSSOId)
        {
            VMLogin objVMLogin = new VMLogin();

            objVMLogin.IsMatchTempPassword = auth.MatchTempPassword;
            objVMLogin.IsResetPassword = auth.IsResetPassword;
            if (auth.IsAuthenticated)
            {
                // store the newly created session id into cookie
                Cookie.Set(CookiesConstants.SOMAX_USER, auth.SessionId.ToString());
                // store the newly created session id into session
                UserSession.SessionId = auth.SessionId;
                //DatabaseKey dbKey = Authentication.GetAdminOnlyKey(); //--V2-542 declared above--//
                auth.UserData = new UserData() { SessionId = auth.SessionId, WebSite = WebSiteEnum.Client };
                auth.UserData.Retrieve(dbKey);

                #region V2-491 Verify Password Expiration

                DataContracts.PasswordSettings PS = new DataContracts.PasswordSettings();
                PS.ClientId = auth.UserData.DatabaseKey.Client.ClientId;
                PS.RetrieveByClientId(auth.UserData.DatabaseKey);

                objVMLogin.PWExpiresDays = PS.PWExpiresDays;
                objVMLogin.PWReqExpiration = PS.PWReqExpiration;
                if (objVMLogin.PWReqExpiration)
                {
                    //code to add days
                    var daysCount = 0;
                    DateTime lastpassChangeDate = DateTime.Parse(auth.LoginInfo.LastPWChangeDate.ToString());
                    DateTime chkExpired = lastpassChangeDate.AddDays(objVMLogin.PWExpiresDays);
                    //DateTime chkExpired = lastpassChangeDate;
                    DateTime currentDate = DateTime.UtcNow;
                    if (objVMLogin.PWExpiresDays > 0)
                    {
                        if (chkExpired.Date <= currentDate.Date)
                        {
                            //code for notofication and redirect to reset password page
                            daysCount = (chkExpired - currentDate).Days;
                            objVMLogin.ExpiresDaysCount = daysCount;
                            //redirect code to reset password
                            objVMLogin.IsPasswordExpired = true;
                            objVMLogin.IsResetPassword = true;
                            objVMLogin.UserNameEncript = auth.UserName.Encrypt();
                            objVMLogin.EmailEncript = auth.UserData.DatabaseKey.User.Email != "" ? auth.UserData.DatabaseKey.User.Email.Encrypt() : auth.UserData.DatabaseKey.User.Email;
                        }
                        else
                        {
                            objVMLogin.IsPasswordExpired = false;
                            daysCount = (chkExpired - currentDate).Days;
                            objVMLogin.ExpiresDaysCount = daysCount;
                        }
                    }
                }
                #endregion


                _objUser.ClientId = auth.UserData.DatabaseKey.Client.ClientId;
                _objUser.SiteId = auth.UserData.DatabaseKey.User.DefaultSiteId;
                _objUser.UserEmail = auth.UserData.DatabaseKey.User.Email;
                _objUser.PersonnelId = auth.UserData.DatabaseKey.Personnel.PersonnelId;
                //_objUser.dbKeyinfo = auth.UserData.DatabaseKey;
                _objUser.IsSOMAXAdmin = auth.LoginInfo.SOMAXAdmin;
                _objUser.UserInfoId = auth.LoginInfo.UserInfoId;
                auth.UserData.DatabaseKey.Personnel.LoginSSOId = loginSSOId;
                auth.UserData.DatabaseKey.Pwd = WebSecurity.Encrypt(auth.Password, "somaxsom");
                System.Web.HttpContext.Current.Session["userData"] = auth.UserData;
                HttpContext.Current.Session["InterfacePropData"] = RetrieveAllInterfacePropertiesByClientIdSiteId();
                if (auth.UserData.DatabaseKey.User.UserType == UserTypeConstants.Reference)
                {
                    objVMLogin.UserInfoDetails = _objUser;
                    objVMLogin.UserInfoDetails.FailureMessage = ErrorMessageConstants.Unauthorised_Msg;// "User is not authorized to login to the system";//auth.FailureMessage;
                    objVMLogin.UserInfoDetails.FailureReason = FailureReasonConstant.UnAuthorisedMsg;// "UnAuthorisedMsg";//auth.FailureReason;
                    return objVMLogin;
                }
                CheckUIVersion(auth, objVMLogin);
                objVMLogin.IsAuthenticated = true;
            }

            objVMLogin.UserInfoDetails = _objUser;
            objVMLogin.UserInfoDetails.FailureMessage = auth.FailureMessage;
            objVMLogin.UserInfoDetails.FailureReason = auth.FailureReason;
            if (auth.UserData != null)
            {
                objVMLogin.UserInfoDetails.UserType = new string[] { auth.UserData.DatabaseKey.User.UserType };
            }

            return objVMLogin;
        }

        private VMLogin AuthenticateLogin(Authentication auth, DatabaseKey dbKey)
        {
            VMLogin objVMLogin = new VMLogin();

            objVMLogin.IsMatchTempPassword = auth.MatchTempPassword;
            objVMLogin.IsResetPassword = auth.IsResetPassword;
            if (auth.IsAuthenticated)
            {
                // store the newly created session id into cookie
                Cookie.Set(CookiesConstants.SOMAX_USER, auth.SessionId.ToString());
                // store the newly created session id into session
                UserSession.SessionId = auth.SessionId;
                //DatabaseKey dbKey = Authentication.GetAdminOnlyKey(); //--V2-542 declared above--//
                auth.UserData = new UserData() { SessionId = auth.SessionId, WebSite = WebSiteEnum.Client };
                auth.UserData.Retrieve(dbKey);

                #region V2-491 Verify Password Expiration

                DataContracts.PasswordSettings PS = new DataContracts.PasswordSettings();
                PS.ClientId = auth.UserData.DatabaseKey.Client.ClientId;
                PS.RetrieveByClientId(auth.UserData.DatabaseKey);

                objVMLogin.PWExpiresDays = PS.PWExpiresDays;
                objVMLogin.PWReqExpiration = PS.PWReqExpiration;
                if (objVMLogin.PWReqExpiration)
                {
                    //code to add days
                    var daysCount = 0;
                    DateTime lastpassChangeDate = DateTime.Parse(auth.LoginInfo.LastPWChangeDate.ToString());
                    DateTime chkExpired = lastpassChangeDate.AddDays(objVMLogin.PWExpiresDays);
                    //DateTime chkExpired = lastpassChangeDate;
                    DateTime currentDate = DateTime.UtcNow;
                    if (objVMLogin.PWExpiresDays > 0)
                    {
                        if (chkExpired.Date <= currentDate.Date)
                        {
                            //code for notofication and redirect to reset password page
                            daysCount = (chkExpired - currentDate).Days;
                            objVMLogin.ExpiresDaysCount = daysCount;
                            //redirect code to reset password
                            objVMLogin.IsPasswordExpired = true;
                            objVMLogin.IsResetPassword = true;
                            objVMLogin.UserNameEncript = auth.UserName.Encrypt();
                            objVMLogin.EmailEncript = auth.UserData.DatabaseKey.User.Email != "" ? auth.UserData.DatabaseKey.User.Email.Encrypt() : auth.UserData.DatabaseKey.User.Email;
                        }
                        else
                        {
                            objVMLogin.IsPasswordExpired = false;
                            daysCount = (chkExpired - currentDate).Days;
                            objVMLogin.ExpiresDaysCount = daysCount;
                        }
                    }
                }
                #endregion


                _objUser.ClientId = auth.UserData.DatabaseKey.Client.ClientId;
                _objUser.SiteId = auth.UserData.DatabaseKey.User.DefaultSiteId;
                _objUser.UserEmail = auth.UserData.DatabaseKey.User.Email;
                _objUser.PersonnelId = auth.UserData.DatabaseKey.Personnel.PersonnelId;
                //_objUser.dbKeyinfo = auth.UserData.DatabaseKey;
                _objUser.IsSOMAXAdmin = auth.LoginInfo.SOMAXAdmin;
                _objUser.UserInfoId = auth.LoginInfo.UserInfoId;                
                auth.UserData.DatabaseKey.Pwd = WebSecurity.Encrypt(auth.Password, "somaxsom");
                System.Web.HttpContext.Current.Session["userData"] = auth.UserData;
                HttpContext.Current.Session["InterfacePropData"] = RetrieveAllInterfacePropertiesByClientIdSiteId();
                if (auth.UserData.DatabaseKey.User.UserType == UserTypeConstants.Reference)
                {
                    objVMLogin.UserInfoDetails = _objUser;
                    objVMLogin.UserInfoDetails.FailureMessage = ErrorMessageConstants.Unauthorised_Msg;// "User is not authorized to login to the system";//auth.FailureMessage;
                    objVMLogin.UserInfoDetails.FailureReason = FailureReasonConstant.UnAuthorisedMsg;// "UnAuthorisedMsg";//auth.FailureReason;
                    return objVMLogin;
                }
                CheckUIVersion(auth, objVMLogin);
                objVMLogin.IsAuthenticated = true;
            }

            objVMLogin.UserInfoDetails = _objUser;
            objVMLogin.UserInfoDetails.FailureMessage = auth.FailureMessage;
            objVMLogin.UserInfoDetails.FailureReason = auth.FailureReason;
            if (auth.UserData != null)
            {
                objVMLogin.UserInfoDetails.UserType = new string[] { auth.UserData.DatabaseKey.User.UserType };
            }

            return objVMLogin;
        }
        public AccountPassword GetUserDataForPasswordCreate(string userId) /*V2-332*/
        {
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            AccountPassword password = new AccountPassword();
            IP_AccountPasswordData ipData = new IP_AccountPasswordData()
            {
                DbKey = dbKey,
                UserName = userId
            };
            password.GetLogInInfoData(ipData);
            return (password);
        }

        public string CreateNewPassword(CreatePassword createPassword)  /*V2-332*/
        {
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            IP_AccountPasswordData ipData = new IP_AccountPasswordData()
            {
                DbKey = dbKey,
                UserName = createPassword.UserName,
                NewPassword = createPassword.NewPassword,
                ConfirmPassword = createPassword.ConfirmPassword,
                SecurityQuestion = createPassword.SecurityQuestion,
                SecurityAnswer = createPassword.SecurityAnswer,
                TempPassword = createPassword.TempPassword,
                PasswordCode = createPassword.PasswordCode,
                LoginInfoId = createPassword.LoginInfoId
            };
            AccountPassword password = new AccountPassword();
            if (createPassword.IsPasswordExpired)
            {
                password.CreatePasswordManual(ipData);
            }
            else
            {
                password.CreatePassword(ipData);
            }

            var result = (password.CreatePasswordResult == true ? JsonReturnEnum.success.ToString() : JsonReturnEnum.failed.ToString());
            if (result == JsonReturnEnum.success.ToString())
            {
                CreateEventLog(password.UserInfoId, EventStatusConstants.ResetPassword);
            }
            else
            {
                if (!string.IsNullOrEmpty(password.Message))
                {
                    result = password.Message;
                }
            }
            return result;
        }

        private void CreateEventLog(Int64 objId, string eventVal = "")  /*V2-332*/
        {
            UserEventLog log = new UserEventLog();
            var userData = (UserData)HttpContext.Current.Session["userData"];
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            log.ObjectId = objId;
            log.Event = eventVal;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }

        #endregion

        #region V2-375
        public List<InterfacePropModel> RetrieveAllInterfacePropertiesByClientIdSiteId()
        {
            var userData = (UserData)HttpContext.Current.Session["userData"];
            List<InterfaceProp> ipropList = new List<InterfaceProp>();
            List<InterfacePropModel> ipropModelList = new List<InterfacePropModel>();
            InterfaceProp iprop = new InterfaceProp();
            iprop.ClientId = userData.DatabaseKey.Client.ClientId;
            InterfacePropModel objInterfacePropModel;
            // iprop.RetrieveInterfaceProperties(userData.DatabaseKey);
            ipropList = iprop.RetrieveInterfacePropertiesByClientIdSiteId(userData.DatabaseKey);
            foreach (var item in ipropList)
            {
                objInterfacePropModel = new InterfacePropModel();
                objInterfacePropModel.InterfaceType = !string.IsNullOrEmpty(item.InterfaceType) ? item.InterfaceType.Trim() : string.Empty;
                objInterfacePropModel.InUse = item.InUse;
                objInterfacePropModel.Switch1 = item.Switch1;
                objInterfacePropModel.Switch2 = item.Switch2;
                objInterfacePropModel.Switch3 = item.Switch3;
                ipropModelList.Add(objInterfacePropModel);
            }
            return ipropModelList;

        }

        #endregion

        #region V2-491
        public PasswordSettings RetrievePasswordSettings(long ClientId)
        {
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            PasswordSettings passwordSettings = new PasswordSettings();
            if (ClientId > 0)
            {
                dbKey.Client.ClientId = ClientId;
                passwordSettings.ClientId = ClientId;
                passwordSettings.RetrieveByClientId(dbKey);
            }
            return passwordSettings;
        }
        #endregion
        #region V2-911
        public List<ClientUserInfoList> RetrieveByUserInfoIdChunkSearchLookupList(long UserInfoId, string UserName, string ClientName = "", string SiteName = "", string orderbycol = "", string orderDir = "", int length = 0, int skip = 0)
        {
            List<ClientUserInfoList> clientUserInfoList = new List<ClientUserInfoList>();
            ClientUserInfoList objClientUserInfoList = new ClientUserInfoList();
            objClientUserInfoList.CallerUserInfoId = UserInfoId;
            objClientUserInfoList.CallerUserName = UserName;
            objClientUserInfoList.orderbyColumn = orderbycol;
            objClientUserInfoList.orderBy = orderDir;
            objClientUserInfoList.offset1 = skip;
            objClientUserInfoList.nextrow = length;
            objClientUserInfoList.UserInfoId = UserInfoId;
            objClientUserInfoList.ClientName = ClientName.Trim();
            objClientUserInfoList.SiteName = SiteName.Trim();
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            clientUserInfoList = objClientUserInfoList.RetrieveChunkSearchLookupList(dbKey);

            return clientUserInfoList;
        }
        public long UpdateCustomForSomaxAdminDetails(long ClientUserInfoListId, long UserInfoId)
        {
            var userData = (UserData)HttpContext.Current.Session["userData"];
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            if (ClientUserInfoListId > 0)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.CallerUserInfoId = UserInfoId;
                userInfo.UserInfoId = UserInfoId;
                userInfo.UserName = userData.DatabaseKey.UserName;
                userInfo.CallerUserName = userData.DatabaseKey.UserName;
                userInfo.LoginClientID = userData.DatabaseKey.Client.ClientId;
                userInfo.DefaultSiteId = userData.DatabaseKey.User.DefaultSiteId;
                userInfo.ClientUserInfoListID = ClientUserInfoListId;
                userInfo.UpdateCustomForSomaxAdminDetails(dbKey);
                return ClientUserInfoListId;
            }
            else
            {
                ClientUserInfoListId = 0;
            }
            return ClientUserInfoListId;
        }


        #endregion
    }
}