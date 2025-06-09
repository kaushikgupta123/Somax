using Admin.Models;

using Business.Authentication;
//using System.Net;
using Common.Constants;
using Common.Enumerations;

using DataContracts;

using Presentation.Common;
using System;
using System.IO;
using System.Text;
using System.Web;
using RazorEngine;
using Business.Common;
using Admin.Common;
namespace Admin.BusinessWrapper
{
    public class LoginWrapper : ILoginWrapper
    {
        public VMLogin GetLogIn(UserInfoDetails _objUser)
        {
            //DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            VMLogin objVMLogin = new VMLogin();
            Authentication auth = new Authentication()
            {
                UserName = _objUser.UserName.Trim(),
                Password = _objUser.Password,
                website = WebSiteEnum.Client,
                BrowserInfo = HttpContext.Current.Request.Browser.Type + " " + HttpContext.Current.Request.Browser.Version,
                IpAddress = HttpContext.Current.Request.UserHostAddress
            };
            auth.VerifyAdminLogin();
            objVMLogin.IsMatchTempPassword = auth.MatchTempPassword;
            objVMLogin.IsResetPassword = auth.IsResetPassword;
            if (auth.IsAuthenticated)
            {
                // store the newly created session id into cookie
                Cookie.Set(CookiesConstants.SOMAX_USER, auth.SessionId.ToString());
                // store the newly created session id into session
                UserSession.SessionId = auth.SessionId;
                DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
                auth.UserData = new UserData() { SessionId = auth.SessionId, WebSite = WebSiteEnum.Client };
                auth.UserData.RetrieveAdmin(dbKey);

                _objUser.ClientId = auth.UserData.DatabaseKey.Client.ClientId;
                _objUser.SiteId = auth.UserData.DatabaseKey.User.DefaultSiteId;
                _objUser.UserEmail = auth.UserData.DatabaseKey.User.Email;
                _objUser.PersonnelId = auth.UserData.DatabaseKey.Personnel.PersonnelId;
                //_objUser.dbKeyinfo = auth.UserData.DatabaseKey;
                auth.UserData.DatabaseKey.Pwd = WebSecurity.Encrypt(auth.Password, "somaxsom");
                System.Web.HttpContext.Current.Session["AdminUserData"] = auth.UserData;
                //HttpContext.Current.Session["InterfacePropData"] = RetrieveAllInterfacePropertiesByClientIdSiteId(); //need to ask
                if (auth.UserData.DatabaseKey.User.UserType != UserTypeConstants.SOMAXAdmin)
                {
                    objVMLogin.UserInfoDetails = _objUser;
                    objVMLogin.UserInfoDetails.FailureMessage = ErrorMessageConstants.Unauthorised_Msg;
                    objVMLogin.UserInfoDetails.FailureReason = FailureReasonConstant.UnAuthorisedMsg;
                    return objVMLogin;
                }
                //CheckUIVersion(auth, objVMLogin);//need to ask
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
        //need to ask
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
        //public List<InterfacePropModel> RetrieveAllInterfacePropertiesByClientIdSiteId()
        //{
        //    var userData = (UserData)HttpContext.Current.Session["AdminUserData"];
        //    List<InterfaceProp> ipropList = new List<InterfaceProp>();
        //    List<InterfacePropModel> ipropModelList = new List<InterfacePropModel>();
        //    InterfaceProp iprop = new InterfaceProp();
        //    iprop.ClientId = userData.DatabaseKey.Client.ClientId;
        //    InterfacePropModel objInterfacePropModel;
        //    // iprop.RetrieveInterfaceProperties(userData.DatabaseKey);
        //    ipropList = iprop.RetrieveInterfacePropertiesByClientIdSiteId(userData.DatabaseKey);
        //    foreach (var item in ipropList)
        //    {
        //        objInterfacePropModel = new InterfacePropModel();
        //        objInterfacePropModel.InterfaceType = !string.IsNullOrEmpty(item.InterfaceType) ? item.InterfaceType.Trim() : string.Empty;
        //        objInterfacePropModel.InUse = item.InUse;
        //        objInterfacePropModel.Switch1 = item.Switch1;
        //        objInterfacePropModel.Switch2 = item.Switch2;
        //        objInterfacePropModel.Switch3 = item.Switch3;
        //        ipropModelList.Add(objInterfacePropModel);
        //    }
        //    return ipropModelList;

        //}
        public bool SendForgetUserIdByMail(string mailId)
        {
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();

            LoginInfo LoginInfo = new LoginInfo() { UserName = mailId, Email = mailId };
            LoginInfo.RetrieveBySearchCriteriaAdmin(dbKey);
            var result = false;
            if (LoginInfo.LoginInfoId > 0)
            {
                try
                {
                    result = SendForgotLoginIdEmail(LoginInfo.UserName, LoginInfo.Email, dbKey);

                }
                catch 
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
            var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Admin\Views\Login\ResetIdMailTemplate.cshtml");
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
            // Email email = new Email() { Body = body.ToString(), Subject = Subject };
            EmailModule email = new Presentation.Common.EmailModule() { Body = body.ToString(), Subject = Subject };
            email.Recipients.Add(EmailAddress);
            try
            {
                //email.Send();
                email.SendEmail();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        } //Som-1192
        public bool SendForgetPasswordResend(string userId)
        {  //Exit function if the username is null
           //if (string.IsNullOrEmpty(txtForgotPasswordLoginId.Text)) { return; }

            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();

            AccountPassword password = new AccountPassword();
            IP_AccountPasswordData ipData = new IP_AccountPasswordData()
            {
                DbKey = dbKey,
                UserName = userId
            };

            // If the username is found, then send an email to user with a temporary password and a link for resetting password.
            password.RequestNewPasswordAdmin(ipData);
            var result = false;
            try
            {
                result = SendResetPasswordEmail(password, dbKey);   //for change passowrd link and temporary passowrd

                UserInfo userInfo = new UserInfo() { ClientId = password.LoginInfo.ClientId, UserInfoId = password.LoginInfo.UserInfoId };
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
            var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Admin\Views\Login\PasswordResetMailTemplate.cshtml");
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
        public static string ParseTemplate(string content)
        {
            string _mode = DateTime.Now.Ticks + new Random().Next(1000, 100000).ToString();
            Razor.Compile(content, _mode);
            return Razor.Parse(content);
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
            var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Admin\Views\Login\PasswordChangeConfirmMailTemplate.cshtml");
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
        public AccountPassword GetUserDataForPasswordCreate(string userId)
        {
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            AccountPassword password = new AccountPassword();
            IP_AccountPasswordData ipData = new IP_AccountPasswordData()
            {
                DbKey = dbKey,
                UserName = userId
            };
            password.GetLogInInfoDataAdmin(ipData);
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
                PasswordCode = createPassword.PasswordCode
            };
            AccountPassword password = new AccountPassword();

            password.CreatePassword(ipData);
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
        private void CreateEventLog(Int64 objId, string eventVal = "")
        {
            UserEventLog log = new UserEventLog();
            var userData = (UserData)HttpContext.Current.Session["AdminUserData"];
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
    }
}