using Business.Common;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Common;

namespace Business.Authentication
{
    public class AccountPassword
    {

        #region Properties
        public string Message { get; set; }
        public string TempPassword { get; set; }
        public LoginInfo LoginInfo { get; set; }
        #endregion
        #region Public Methods 
        public bool CreatePasswordResult = false;/*V2-332*/
        public long UserInfoId;/*V2-332*/
        public long UserClientId;/*V2-332*/

        public void CreatePassword(IP_AccountPasswordData ipData)  /*V2-332*/
        {
            //bool result = false;
            if (string.Compare(ipData.NewPassword, ipData.ConfirmPassword, false) != 0)
            {
                Message = ErrorMessageConstants.Passwords_Not_Match;//ipData.Localization.ErrorMessages.Passwords_Not_Match;
                                                                    //result=false;
                CreatePasswordResult = false;
                return;
            }
            Guid passwordCode;
            // Check if the password code can be parsed as guid and code is empty
            if (Guid.TryParse(ipData.PasswordCode, out passwordCode) && !passwordCode.Equals(Guid.Empty))
            {
                LoginInfo loginInfo = new LoginInfo() { ResetPasswordCode = passwordCode };
                loginInfo.RetrieveByResetPasswordCode(ipData.DbKey);

                // Compare the temp password to the one in database & whether temp password is older than one day
                if (string.Compare(loginInfo.TempPassword, ipData.TempPassword, false) == 0)
                {
                    if (loginInfo.ResetPasswordRequestDate.Value.AddDays(1).CompareTo(DateTime.Now) >= 0)
                    {
                        // The user name in upper case is appended to the new password.  Then it's encrypted with SHA512
                        loginInfo.Password = Presentation.Common.Encryption.SHA512Encrypt(loginInfo.UserName.Trim().ToUpper() + ipData.NewPassword);

                        // Reset the password code, request date, and temp password
                        loginInfo.SecurityQuestion = ipData.SecurityQuestion ?? string.Empty;
                        loginInfo.SecurityResponse = ipData.SecurityAnswer ?? string.Empty;
                        loginInfo.ResetPasswordCode = Guid.Empty;
                        loginInfo.ResetPasswordRequestDate = null;
                        loginInfo.TempPassword = string.Empty;
                        loginInfo.FailedAttempts = 0;
                        if (ipData.DbKey.Client.ClientId == 0) { ipData.DbKey.Client.ClientId = loginInfo.ClientId; } /*V2-332*/
                        loginInfo.LastPWChangeDate = DateTime.UtcNow;
                        //loginInfo.UpdatePassword(ipData.DbKey);
                        loginInfo.UpdateCustom(ipData.DbKey);   /*V2-332*/
                        if (loginInfo.ErrorMessages == null || loginInfo.ErrorMessages.Count > 0)
                        {
                            //result = true;
                            CreatePasswordResult = true;
                        }
                        //// Assign the client id if the dbkey is retrieve from GetAdminOnlyKey()
                        //if (ipData.DbKey.Client.ClientId == 0)
                        //{
                        //    ipData.DbKey.Client.ClientId = loginInfo.ClientId;
                        //}

                        //UserInfo userInfo = new UserInfo { ClientId = loginInfo.ClientId, UserInfoId = loginInfo.UserInfoId };
                        //userInfo.Retrieve(ipData.DbKey);
                        UserInfoId = loginInfo.UserInfoId;
                        UserClientId = loginInfo.ClientId;
                    }
                    else
                    {
                        Message = ErrorMessageConstants.Temporary_Password_Expired;
                        CreatePasswordResult = false;
                        return;
                    }
                }
                else
                {
                    Message = "Wrong temporary password ";
                    CreatePasswordResult = false;
                    return;
                }

            }
            else
            {
                CreatePasswordResult = false;
            }
            // return result;
        }

        public bool Reset(IP_AccountPasswordData ipData, StringBuilder mailBody)
        {
            bool success = false;

            if (string.Compare(ipData.NewPassword, ipData.ConfirmPassword, false) != 0)
            {
                Message = ErrorMessageConstants.Passwords_Not_Match;//ipData.Localization.ErrorMessages.Passwords_Not_Match;
                return success;
            }

            Guid passwordCode;
            // Check if the password code can be parsed as guid and code is empty
            if (Guid.TryParse(ipData.PasswordCode, out passwordCode) && !passwordCode.Equals(Guid.Empty))
            {

                LoginInfo loginInfo = new LoginInfo() { ResetPasswordCode = passwordCode };
                loginInfo.RetrieveByResetPasswordCode(ipData.DbKey);

                // SOM-520 - Begin
                string hashedTmpPwd = Presentation.Common.Encryption.SHA512Encrypt(loginInfo.UserName.Trim().ToUpper() + ipData.TempPassword);
                // SOM-520 - End

                // Compare the security response and temp password to the ones in database 
                if (string.Compare(loginInfo.SecurityResponse, ipData.SecurityAnswer, false) == 0
                    && string.Compare(loginInfo.TempPassword, hashedTmpPwd, false) == 0
                    && loginInfo.ResetPasswordRequestDate.Value.AddDays(1).CompareTo(DateTime.Now) >= 0)
                {

                    // The user name in upper case is appended to the new password.  Then it's encrypted with SHA512
                    // Reset the password code, request date, and temp password
                    // SOM-520 - Begin
                    loginInfo.Password = Presentation.Common.Encryption.SHA512Encrypt(loginInfo.UserName.Trim().ToUpper() + ipData.NewPassword);
                    // SOM-520 - End
                    loginInfo.ResetPasswordCode = Guid.Empty;
                    loginInfo.ResetPasswordRequestDate = null;
                    loginInfo.TempPassword = string.Empty;
                    //Som-1192
                    loginInfo.FailedAttempts = 0;
                    // Assign the client id if the dbkey is retrieve from GetAdminOnlyKey()
                    if (ipData.DbKey.Client.ClientId == 0) { ipData.DbKey.Client.ClientId = loginInfo.ClientId; }
                    loginInfo.LastPWChangeDate = DateTime.UtcNow;
                    loginInfo.UpdateCustom(ipData.DbKey);   /*V2-332*/
                                                            // loginInfo.UpdatePassword(ipData.DbKey);

                    UserInfo userInfo = new UserInfo { ClientId = loginInfo.ClientId, UserInfoId = loginInfo.UserInfoId };
                    userInfo.Retrieve(ipData.DbKey);

                    // Send email to notify the user



                    //Email email = new Email()
                    EmailModule email = new Presentation.Common.EmailModule()
                    {
                        Subject = "Password Changed",
                        MailBody = mailBody.ToString()
                        // Body = mailBody.ToString() //"Your password has Changed."
                    };
                    email.Recipients.Add(userInfo.Email);
                    //email.Send();
                    email.SendEmail();
                    Message = ErrorMessageConstants.ResetPasswordSuccess; //ipData.Localization.ResetPasswordSuccess;

                    success = true;
                }
                else
                {
                    // The security response doesn't match the one in database.
                    Message = ErrorMessageConstants.Password_Reset_Fail; // ipData.Localization.ErrorMessages.Password_Reset_Fail;
                }
            }

            return success;
        }

        public void RequestNewPassword(IP_AccountPasswordData ipData)
        {

            LoginInfo = new LoginInfo() { UserName = ipData.UserName, Email = ipData.UserName };
            LoginInfo.RetrieveBySearchCriteria(ipData.DbKey);

            if (LoginInfo.LoginInfoId > 0)
            {
                TempPassword = GetRandomPassword();

                // Generate a reset password code and a temp password which is a 6 characters string generated randomly
                LoginInfo.ResetPasswordCode = Guid.NewGuid();
                LoginInfo.ResetPasswordRequestDate = DateTime.Now;
                // SOM-520 - Begin
                LoginInfo.TempPassword = Presentation.Common.Encryption.SHA512Encrypt(LoginInfo.UserName.Trim().ToUpper() + TempPassword);
                //LoginInfo.TempPassword = Encryption.SHA512Encrypt(LoginInfo.UserName.ToUpper() + TempPassword);
                // SOM-520 - End
                // Assign Password to Temp Password so the new user can reset password by using the reset password page
                if (ipData.IsNew) { LoginInfo.TempPassword = LoginInfo.Password; }

                // Update the client id of dbKey
                ipData.DbKey.Client.ClientId = LoginInfo.ClientId;
                //LoginInfo.Update(ipData.DbKey);
                LoginInfo.UpdateCustom(ipData.DbKey);
            }

        }
        public void RequestNewPasswordAdmin(IP_AccountPasswordData ipData)
        {

            LoginInfo = new LoginInfo() { UserName = ipData.UserName, Email = ipData.UserName };
            LoginInfo.RetrieveBySearchCriteriaAdmin(ipData.DbKey);

            if (LoginInfo.LoginInfoId > 0)
            {
                TempPassword = GetRandomPassword();

                // Generate a reset password code and a temp password which is a 6 characters string generated randomly
                LoginInfo.ResetPasswordCode = Guid.NewGuid();
                LoginInfo.ResetPasswordRequestDate = DateTime.Now;
                // SOM-520 - Begin
                LoginInfo.TempPassword = Presentation.Common.Encryption.SHA512Encrypt(LoginInfo.UserName.Trim().ToUpper() + TempPassword);
                //LoginInfo.TempPassword = Encryption.SHA512Encrypt(LoginInfo.UserName.ToUpper() + TempPassword);
                // SOM-520 - End
                // Assign Password to Temp Password so the new user can reset password by using the reset password page
                if (ipData.IsNew) { LoginInfo.TempPassword = LoginInfo.Password; }

                // Update the client id of dbKey
                ipData.DbKey.Client.ClientId = LoginInfo.ClientId;
                //LoginInfo.Update(ipData.DbKey);
                LoginInfo.UpdateCustom(ipData.DbKey);
            }

        }
        public void GetLogInInfoData(IP_AccountPasswordData ipData)  /*for V2-332*/
        {
            LoginInfo = new LoginInfo() { UserName = ipData.UserName, Email = ipData.UserName };
            LoginInfo.RetrieveBySearchCriteria(ipData.DbKey);
        }
        public void GetLogInInfoDataAdmin(IP_AccountPasswordData ipData)
        {
            LoginInfo = new LoginInfo() { UserName = ipData.UserName, Email = ipData.UserName };
            LoginInfo.RetrieveBySearchCriteriaAdmin(ipData.DbKey);
        }
        public static bool RequestExpired(LoginInfo loginInfo)
        {
            bool expired = true;

            if (loginInfo.ResetPasswordRequestDate.HasValue)
            {
                if (loginInfo.ResetPasswordRequestDate.Value.AddDays(1).CompareTo(DateTime.Now) >= 0)
                {
                    // The request will expire in one day
                    expired = false;
                }
            }

            return expired;
        }

        #endregion

        #region Private Methods 

        /*************************Change private to public by Indusnet technologies*****************/
        public static string GetRandomPassword()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%&*";

            Random random = new Random();

            char[] randomPassword = new char[12];

            for (int i = 0; i < 12; i++)
            {
                randomPassword[i] = chars[random.Next(chars.Length)];
            }
            return new string(randomPassword);
        }

        #endregion

        #region V2-491
        public void CreatePasswordManual(IP_AccountPasswordData ipData)  
        {
            //bool result = false;
            if (string.Compare(ipData.NewPassword, ipData.ConfirmPassword, false) != 0)
            {
                Message = ErrorMessageConstants.Passwords_Not_Match;//ipData.Localization.ErrorMessages.Passwords_Not_Match;
                                                                    //result=false;
                CreatePasswordResult = false;
                return;
            }

            LoginInfo loginInfo = new LoginInfo() { LoginInfoId = ipData.LoginInfoId };
            loginInfo.RetrieveByLoginInfoId(ipData.DbKey);

            // The user name in upper case is appended to the new password.  Then it's encrypted with SHA512
            loginInfo.Password = Presentation.Common.Encryption.SHA512Encrypt(loginInfo.UserName.Trim().ToUpper() + ipData.NewPassword);

            // Reset the password code, request date, and temp password
            loginInfo.SecurityQuestion = ipData.SecurityQuestion ?? string.Empty;
            loginInfo.SecurityResponse = ipData.SecurityAnswer ?? string.Empty;
            loginInfo.ResetPasswordCode = Guid.Empty;
            loginInfo.ResetPasswordRequestDate = null;
            loginInfo.TempPassword = string.Empty;
            loginInfo.FailedAttempts = 0;
            if (ipData.DbKey.Client.ClientId == 0) { ipData.DbKey.Client.ClientId = loginInfo.ClientId; } /*V2-332*/
            loginInfo.LastPWChangeDate = DateTime.UtcNow;
            loginInfo.UpdateCustom(ipData.DbKey);   /*V2-332*/
            if (loginInfo.ErrorMessages == null || loginInfo.ErrorMessages.Count > 0)
            {
                //result = true;
                CreatePasswordResult = true;
            }
            else
            {
                CreatePasswordResult = false;
            }

            UserInfoId = loginInfo.UserInfoId;
            UserClientId = loginInfo.ClientId;

        }
        #endregion
    }

    public class IP_AccountPasswordData
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string TempPassword { get; set; }
        public string SecurityAnswer { get; set; }
        public string PasswordCode { get; set; }
        public string UserName { get; set; }
        public DatabaseKey DbKey { get; set; }
        //public Localization.Global Localization { get; set; }
        public bool IsNew { get; set; }
        public string SecurityQuestion { get; set; }
        public long LoginInfoId { get; set; }
    }
}
