/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person           Description
* ===========  ========= ================ ========================================================
* 2015-Feb--2  SOM-520   Roger Lawton     Make sure logininfo.username is trimmed before encrypting
**************************************************************************************************
*/

using Common.Constants;
using Common.Enumerations;
using Common.Extensions;

using DataContracts;
//using Business.Common;
using Presentation.Common;

using System;
using System.Configuration;

namespace Business.Authentication
{
    [Serializable()]
    public class Authentication
    {
        #region Constants
        private const string DEFAULT_LANGUAGE = "en";
        #endregion

        #region Member Variables
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FailureMessage { get; set; }
        public string FailureReason { get; set; }
        public string BrowserInfo { get; set; }
        public string IpAddress { get; set; }
        public bool IsAuthenticated { get; set; }
        public WebSiteEnum website { get; set; }
        public Guid SessionId { get; set; }
        public UserData UserData { get; set; }

        //----------------------[Added By Indusnet Technologies]---------------
        public LoginInfo LoginInfo { get; set; }
        public bool MatchTempPassword { get; set; }
        public bool IsResetPassword { get; set; }

        #endregion

        #region Private Variables
        private DatabaseKey _dbKey;
        private LoginDataSet _loginData;
        #endregion

        #region Constructors
        public Authentication()
        {
            _dbKey = GetAdminOnlyKey();
            _loginData = new LoginDataSet();
            IsAuthenticated = false;
            MatchTempPassword = false;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a DatabaseKey object populated with the admin connection string. 
        /// This should only be used by methods that do not require authentication, such as site maintenance messages.
        /// </summary>
        /// <returns></returns>
        public static DatabaseKey GetAdminOnlyKey()
        {
            DatabaseKey dbKey = new DatabaseKey()
            {
                Client = new Client(),
                User = new UserInfo(),
                UserName = ""
            };

            if (ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING] != null)
            {
                dbKey.AdminConnectionString = ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ConnectionString;
            }
            return dbKey;
        }

        /// <summary>Verify a user name and password</summary>
        /// <returns>Return true if the user is authenticated.  Otherwise, return false.</return>
        public void VerifyLogin()
        {
            if (!string.IsNullOrEmpty(UserName.Trim()) && !string.IsNullOrEmpty(Password.Trim()))
            {
                _loginData.UserName = UserName;
                _loginData.RetrieveByUsername_V2(_dbKey);

                _dbKey.Client.ClientId = _loginData.LoginInfo.ClientId; // Since the DB has not been initialized, we need to explicitly set this value.

                if (IsUserFound())
                {
                    _loginData.UserType = string.IsNullOrEmpty(_loginData.UserType) ? "" : _loginData.UserType;
                    if (_loginData.UserType.ToLower() != UserTypeConstants.SOMAXAdmin.ToLower() || (_loginData.UserType.ToLower() == UserTypeConstants.SOMAXAdmin.ToLower() && _loginData.LoginInfo.SOMAXAdmin == true))
                    {
                        if (IsAccountActive())
                        {
                            if (IsClientActive())
                            {
                                if (IsSiteActive())
                                {
                                    if (MaxAttemptsNotExceed())
                                    {
                                        // The user name in upper case is appended to the password.  
                                        // The string is encrypted with SHA512, and then  
                                        // it's compared to the encrypted password in the database
                                        // SOM-520 - Begin
                                        string hashedPwd = Encryption.SHA512Encrypt(UserName.Trim().ToUpper() + Password);
                                        //string hashedPwd = Encryption.SHA512Encrypt(UserName.ToUpper() + Password);
                                        // SOM-520 - End

                                        if (PasswordMatch(hashedPwd))
                                        {
                                            if (MatchTempPassword == true)
                                            {
                                                if (_loginData.LoginInfo.ResetPasswordRequestDate.Value.AddDays(1).CompareTo(DateTime.Now) >= 0)  /*V2-332*/
                                                {
                                                    IsAuthenticated = true;
                                                    AuthenticationSuccess(_loginData.LoginInfo);
                                                }
                                                else
                                                {
                                                    FailureMessage = ErrorMessageConstants.Temporary_Password_Expired;  /*new constant for V2-332*/
                                                    FailureReason = FailureReasonConstant.TemporaryPasswordExpired;   /*used for V2-332*/
                                                }
                                            }
                                            else
                                            {
                                                IsAuthenticated = true;
                                                AuthenticationSuccess(_loginData.LoginInfo);
                                            }
                                            //----------------------[Added By Indusnet Technologies]---------------
                                            this.LoginInfo = _loginData.LoginInfo;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        FailureMessage = ErrorMessageConstants.Unauthorised_Msg;
                        FailureReason = FailureReasonConstant.UnAuthorisedMsg;
                    }
                }
                else
                {
                    FailureMessage = ErrorMessageConstants.UserName_Incorrect;
                    FailureReason = FailureReasonConstant.UserNameIncorrect;
                }
            }
        }
        public void VerifyAuthenticateLogin(Int64 logininfoid) //V2-1178
        {
           
                _loginData.LoginInfo.LoginInfoId = logininfoid;
                _loginData.RetrieveByLoginInfoId_V2(_dbKey);
                _dbKey.Client.ClientId = _loginData.LoginInfo.ClientId; // Since the DB has not been initialized, we need to explicitly set this value.
                    _loginData.UserType = string.IsNullOrEmpty(_loginData.UserType) ? "" : _loginData.UserType;
                    if (_loginData.UserType.ToLower() != UserTypeConstants.SOMAXAdmin.ToLower() || (_loginData.UserType.ToLower() == UserTypeConstants.SOMAXAdmin.ToLower() && _loginData.LoginInfo.SOMAXAdmin == true))
                    {
                        if (IsAccountActive())
                        {
                            if (IsClientActive())
                            {
                                if (IsSiteActive())
                                {
                                    if (MaxAttemptsNotExceed())
                                    {                                       
                                    IsAuthenticated = true;
                                    AuthenticationSuccess(_loginData.LoginInfo);
                                    this.LoginInfo = _loginData.LoginInfo;
                                this._dbKey.Personnel.UserName = _loginData.LoginInfo.UserName;
                                }
                                }
                            }
                        }
                    }
                    else
                    {
                        FailureMessage = ErrorMessageConstants.Unauthorised_Msg;
                        FailureReason = FailureReasonConstant.UnAuthorisedMsg;
                    }
                
        }
        public void VerifyLoginSSO(Int64 userInfoId)
        {

            _loginData.UserName = UserName;
            _loginData.UserInfoIdSSO = userInfoId;
            _loginData.RetrieveBySSO(_dbKey);

            _dbKey.Client.ClientId = _loginData.LoginInfo.ClientId; // Since the DB has not been initialized, we need to explicitly set this value.

            if (IsUserFound())
            {
                _loginData.UserType = string.IsNullOrEmpty(_loginData.UserType) ? "" : _loginData.UserType;
                if (_loginData.UserType.ToLower() != UserTypeConstants.SOMAXAdmin.ToLower())
                {
                    if (IsAccountActive())
                    {
                        if (IsClientActive())
                        {
                            if (IsSiteActive())
                            {
                                if (MaxAttemptsNotExceed())
                                {
                                    // The user name in upper case is appended to the password.  
                                    // The string is encrypted with SHA512, and then  
                                    // it's compared to the encrypted password in the database
                                    // SOM-520 - Begin
                                    //string hashedPwd = Encryption.SHA512Encrypt(UserName.Trim().ToUpper() + Password);
                                    //string hashedPwd = Encryption.SHA512Encrypt(UserName.ToUpper() + Password);
                                    // SOM-520 - End

                                    IsAuthenticated = true;
                                    AuthenticationSuccess(_loginData.LoginInfo);
                                    this.LoginInfo = _loginData.LoginInfo;

                                    //if (PasswordMatch(hashedPwd))
                                    //{
                                    //    if (MatchTempPassword == true)
                                    //    {
                                    //        if (_loginData.LoginInfo.ResetPasswordRequestDate.Value.AddDays(1).CompareTo(DateTime.Now) >= 0)  /*V2-332*/
                                    //        {
                                    //            IsAuthenticated = true;
                                    //            AuthenticationSuccess(_loginData.LoginInfo);
                                    //        }
                                    //        else
                                    //        {
                                    //            FailureMessage = ErrorMessageConstants.Temporary_Password_Expired;  /*new constant for V2-332*/
                                    //            FailureReason = FailureReasonConstant.TemporaryPasswordExpired;   /*used for V2-332*/
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        IsAuthenticated = true;
                                    //        AuthenticationSuccess(_loginData.LoginInfo);
                                    //    }
                                    //    //----------------------[Added By Indusnet Technologies]---------------
                                    //    this.LoginInfo = _loginData.LoginInfo;
                                    //}

                                }
                            }
                        }
                    }
                }
                else
                {
                    FailureMessage = ErrorMessageConstants.Unauthorised_Msg;
                    FailureReason = FailureReasonConstant.UnAuthorisedMsg;
                }
            }
            else
            {
                FailureMessage = ErrorMessageConstants.UserName_Incorrect;
                FailureReason = FailureReasonConstant.UserNameIncorrect;
            }

        }

        /// <summary>Verify a user name and password</summary>
        /// <returns>Return true if the user is authenticated.  Otherwise, return false.</return>
        public void VerifyAdminLogin()
        {
            if (!string.IsNullOrEmpty(UserName.Trim()) && !string.IsNullOrEmpty(Password.Trim()))
            {
                _loginData.UserName = UserName;
                _loginData.RetrieveByAdminUsername(_dbKey);

                _dbKey.Client.ClientId = _loginData.LoginInfo.ClientId;
                if (IsUserFound())
                {
                    _loginData.UserType = string.IsNullOrEmpty(_loginData.UserType) ? "" : _loginData.UserType;
                    if (_loginData.UserType.ToLower() == UserTypeConstants.SOMAXAdmin.ToLower())
                    {
                        if (IsAccountActive())
                        {
                            if (MaxAttemptsNotExceed())
                            {
                                // The user name in upper case is appended to the password.  
                                // The string is encrypted with SHA512, and then  
                                // it's compared to the encrypted password in the database
                                // SOM-520 - Begin
                                string hashedPwd = Encryption.SHA512Encrypt(UserName.Trim().ToUpper() + Password);
                                //string hashedPwd = Encryption.SHA512Encrypt(UserName.ToUpper() + Password);
                                // SOM-520 - End

                                if (PasswordMatch(hashedPwd))
                                {
                                    if (MatchTempPassword == true)
                                    {
                                        if (_loginData.LoginInfo.ResetPasswordRequestDate.Value.AddDays(1).CompareTo(DateTime.Now) >= 0)  /*V2-332*/
                                        {
                                            IsAuthenticated = true;
                                            AuthenticationSuccess(_loginData.LoginInfo);
                                        }
                                        else
                                        {
                                            FailureMessage = ErrorMessageConstants.Temporary_Password_Expired;  /*new constant for V2-332*/
                                            FailureReason = FailureReasonConstant.TemporaryPasswordExpired;   /*used for V2-332*/
                                        }
                                    }
                                    else
                                    {
                                        IsAuthenticated = true;
                                        AuthenticationSuccess(_loginData.LoginInfo);
                                    }
                                    //----------------------[Added By Indusnet Technologies]---------------
                                    this.LoginInfo = _loginData.LoginInfo;
                                }

                            }
                        }
                    }
                    else
                    {
                        FailureMessage = ErrorMessageConstants.Unauthorised_Msg;
                        FailureReason = FailureReasonConstant.UnAuthorisedMsg;
                    }
                }
                else
                {
                    FailureMessage = ErrorMessageConstants.UserName_Incorrect;
                    FailureReason = FailureReasonConstant.UserNameIncorrect;
                }
            }
        }

        public void VerifyCurrentUser()
        {
            if (UserData.LoginAuditing.Active)
            {
                // Check if the session id expires
                if (IsSessionAlive(UserData.LoginAuditing.CreateDate, UserData.DatabaseKey.Client.MaxTimeOut))
                {
                    IsAuthenticated = true;
                }
            }
        }

        #endregion

        #region Private Methods
        /// <summary>Create a auditing record after the user is authenticated and save the new session Id into a session variable and a cookies</summary>
        /// <param name="loginInfo">The user LoginInfo object</param>
        private void AuthenticationSuccess(LoginInfo loginInfo)
        {
            SessionId = Guid.NewGuid();

            LoginAuditing loginAuditing = new LoginAuditing()
            {
                SessionId = SessionId,
                LoginInfoId = loginInfo.LoginInfoId,
                UserInfoId = loginInfo.UserInfoId,
                ClientId = loginInfo.ClientId,
                Active = true,
                Browser = BrowserInfo, // HttpContext.Current.Request.Browser.Type + " " + HttpContext.Current.Request.Browser.Version,
                IPAddress = IpAddress //HttpContext.Current.Request.UserHostAddress
            };

            loginAuditing.Create(_dbKey);
            // 2024-May-15 - RKL - V2-10??
            // Do NOT want a changelog entry for successful login 
            bool oldAuditEnable = loginInfo.SetAuditEnable(false);
            // reset the failed attempts when login succeeds 
            loginInfo.FailedAttempts = 0;
            loginInfo.LastLoginDate = DateTime.Now.ToDefaultTimeZone();
            loginInfo.Update(_dbKey);
            loginInfo.SetAuditEnable(oldAuditEnable);
        }

        /// <summary>Check if the user is found in the database</summary>
        /// <returns>Return true if the user is found.  Otherwise, return false.</return>
        private bool IsUserFound()
        {
            bool isFound = _loginData.IsFound;

            if (!isFound)
            {
                // The user name is not found 
                // Localization.ErrorMessages errorMessages = Business.Localization.LocalizationCache.GetErrorMessages(param, website);
                FailureMessage = ErrorMessageConstants.No_Record_Found;
                FailureReason = FailureReasonConstant.UserNotFound;
            }

            return isFound;
        }

        /// <summary>Check if the user account is active</summary>
        /// <returns>Return true if the account is active.  Otherwise, return false.</return>
        private bool IsAccountActive()
        {
            bool isActive = _loginData.LoginInfo.IsActive;

            if (!isActive)
            {
                // Account is inactive

                //Localization.ErrorMessages errorMessages = Business.Localization.LocalizationCache.GetErrorMessages(param, website);
                FailureMessage = ErrorMessageConstants.ACCOUNT_IACTIVE;
                FailureReason = FailureReasonConstant.AccountInactive;

                _loginData.LoginInfo.LastFailureDate = DateTime.Today.ToDefaultTimeZone();
                _loginData.LoginInfo.Update(_dbKey);
            }

            return isActive;

        }

        /// <summary>Check if the max attempts exceed</summary>
        /// <returns>Return true if the max attempts exceed.  Otherwise, return false.</return>
        private bool MaxAttemptsNotExceed()
        {
            bool notExceed = _loginData.Client.MaxAttempts >= _loginData.LoginInfo.FailedAttempts;

            if (!notExceed)
            {
                // Exceed the default number of the failed attempts

                //Localization.ErrorMessages errorMessages = Business.Localization.LocalizationCache.GetErrorMessages(param, website);
                FailureMessage = ErrorMessageConstants.EXCEED_FAILURE_ATTEMPTS; //"Exceed Failure Attempts";
                FailureReason = FailureReasonConstant.MaxAttemptExceed;

                _loginData.LoginInfo.LastFailureDate = DateTime.Today.ToDefaultTimeZone();
                _loginData.LoginInfo.Update(_dbKey);
            }

            return notExceed;
        }

        /// <summary>Verify the password from the database match the password parameter</summary>
        /// <param name="password">The user user password</param>
        /// <returns>Return true if the passwords match.  Otherwise, return false.</return>
        private bool PasswordMatch(string password)
        {
            //bool match = _loginData.LoginInfo.Password.Equals(password) || _loginData.LoginInfo.TempPassword.Equals(password); /*old code*/
            bool match = false;
            if (string.IsNullOrEmpty(_loginData.LoginInfo.TempPassword) && _loginData.LoginInfo.Password.Equals(password))
            {
                match = true;
                MatchTempPassword = false;
            }
            else if (_loginData.LoginInfo.TempPassword.Equals(password))
            {
                match = true;
                MatchTempPassword = true;
                if (!string.IsNullOrEmpty(_loginData.LoginInfo.Password))
                {
                    IsResetPassword = true;
                }
                else
                {
                    IsResetPassword = false;
                }
            }

            if (!match && this.website != WebSiteEnum.Service)
            {
                //  The password doesn't match the one from database

                //Localization.ErrorMessages errorMessages = Business.Localization.LocalizationCache.GetErrorMessages(param, website);
                if (_loginData.Client.MaxAttempts > _loginData.LoginInfo.FailedAttempts)
                {
                    FailureMessage = ErrorMessageConstants.WRONG_PASSWORD;
                    FailureReason = FailureReasonConstant.PasswordNotMatched;
                }
                else
                {
                    // Display different message if it's the last attempt 
                    FailureMessage = ErrorMessageConstants.EXCEED_FAILURE_ATTEMPTS;
                    FailureReason = FailureReasonConstant.MaxAttemptExceed;
                }

                // Increment the number of the failed attempts by 1
                _loginData.LoginInfo.FailedAttempts += 1;
                _loginData.LoginInfo.LastFailureDate = DateTime.Today.ToDefaultTimeZone();
                _loginData.LoginInfo.Update(_dbKey);
            }
            return match;
        }

        /// <summary>Check if the Session Id expires</summary>
        /// <returns>Return false if the session expired.  Otherwise, return true.</return>
        private bool IsSessionAlive(DateTime sessionCreateDate, int timeOutPeriod)
        {
            return (sessionCreateDate.AddMinutes(timeOutPeriod).CompareTo(DateTime.Now.ToDefaultTimeZone()) >= 0);
        }



        #endregion

        #region V2-858
        /// <summary>Check if the user Client is active</summary>
        /// <returns>Return true if the Client is active.  Otherwise, return false.</return>

        private bool IsClientActive()
        {
            bool isActive = _loginData.ClientStatus.ToLower() == "active" ? true : false;

            if (!isActive)
            {
                // Client is inactive

                //Localization.ErrorMessages errorMessages = Business.Localization.LocalizationCache.GetErrorMessages(param, website);
                FailureMessage = ErrorMessageConstants.Client_InActive_Msg;
                FailureReason = FailureReasonConstant.ClientInactiveMsg;

                _loginData.LoginInfo.LastFailureDate = DateTime.Today.ToDefaultTimeZone();
                _loginData.LoginInfo.Update(_dbKey);
            }

            return isActive;

        }
        /// <summary>Check if the user Site is active</summary>
        /// <returns>Return true if the Site is active.  Otherwise, return false.</return>

        private bool IsSiteActive()
        {
            bool isActive = _loginData.SiteStatus.ToLower() == "active" ? true : false;

            if (!isActive)
            {
                // Site is inactive

                //Localization.ErrorMessages errorMessages = Business.Localization.LocalizationCache.GetErrorMessages(param, website);
                FailureMessage = ErrorMessageConstants.Site_InActive_Msg;
                FailureReason = FailureReasonConstant.SiteInactiveMsg;

                _loginData.LoginInfo.LastFailureDate = DateTime.Today.ToDefaultTimeZone();
                _loginData.LoginInfo.Update(_dbKey);
            }

            return isActive;

        }
        #endregion
    }
}
