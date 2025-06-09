using Client.Models.UserProfile;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;

namespace Client.BusinessWrapper.UserProfile
{
    public class UserProfileWrapper
    {
        private DatabaseKey _dbKey;
        private UserData _userData;

        public UserProfileWrapper(UserData userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region GetProfile
        internal UserProfileModel GetUserProfile()
        {
            UserProfileModel objUserProfileModel = new UserProfileModel();

            DataContracts.LoginAuditing loginauditing = new DataContracts.LoginAuditing()
            {
                SessionId = _userData.SessionId
            };

            loginauditing.RetrieveBySessionId(_userData.DatabaseKey);

            DataContracts.UserInfo userinfo = new DataContracts.UserInfo()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                UserInfoId = _userData.DatabaseKey.User.UserInfoId
            };

            DataContracts.LoginInfo logininfo = new DataContracts.LoginInfo()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                LoginInfoId = loginauditing.LoginInfoId
            };

            userinfo.Retrieve(_userData.DatabaseKey);
            logininfo.Retrieve(_userData.DatabaseKey);

            DataContracts.Site site = new DataContracts.Site()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                SiteId = userinfo.DefaultSiteId,
            };
            site.Retrieve(this._userData.DatabaseKey);
            // Populate UserInfo.FirstName data
            objUserProfileModel.UserProfileFirstName = userinfo.FirstName;

            // Populate UserInfo.LastName data
            objUserProfileModel.UserProfileLastName = userinfo.LastName;

            // Populate UserInfo.MiddleName data
            objUserProfileModel.UserProfileMiddleName = userinfo.MiddleName;

            // Populate UserInfo.Email data
            objUserProfileModel.UserProfileEmailAddress = userinfo.Email;

            // Populate UserInfo.Site data
            objUserProfileModel.UserProfileSiteName = site.Name;

            // Populate LoginInfo.SecurityQuestion data
            objUserProfileModel.UserProfileSecurityQuestion = logininfo.SecurityQuestion;

            // Populate LoginInfo.SecurityResponse data
            objUserProfileModel.UserProfileSecurityAnswer = logininfo.SecurityResponse;
            objUserProfileModel.UserInfoUpdateIndex = userinfo.UpdateIndex;
            objUserProfileModel.LoginInfoUpdateIndex = logininfo.UpdateIndex;
            objUserProfileModel.UserProfileUserName = logininfo.UserName;

            return objUserProfileModel;
        }
        #endregion

        #region UpdateProfile
        internal Tuple<bool, List<string>> UpdateUserProfile(UserProfileModel userProfileModel)
        {
            List<string> messages = new List<string>();

            DataContracts.LoginAuditing loginauditing = new DataContracts.LoginAuditing()
            {
                SessionId = _userData.SessionId
            };

            loginauditing.RetrieveBySessionId(_userData.DatabaseKey);

            DataContracts.UserInfo userinfo = new DataContracts.UserInfo()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                UserInfoId = _userData.DatabaseKey.User.UserInfoId
            };

            DataContracts.LoginInfo logininfo = new DataContracts.LoginInfo()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                LoginInfoId = loginauditing.LoginInfoId
            };

            userinfo.Retrieve(_userData.DatabaseKey);
            logininfo.Retrieve(_userData.DatabaseKey);

            RetrieveUserInfoPageControls(userinfo, userProfileModel);
            RetrieveLoginInfoPageControls(logininfo, userProfileModel);

            string updatedPassword = string.Empty;

            if (!string.IsNullOrEmpty(userProfileModel.UserProfileCurrentPassword) && !string.IsNullOrEmpty(userProfileModel.UserProfileNewPassword) && !string.IsNullOrEmpty(userProfileModel.UserProfileConfirmPassword))
            {

                bool isCheckPasswordSuccess = CheckPassword(logininfo.Password,
                                                userProfileModel.UserProfileCurrentPassword,
                                                userProfileModel.UserProfileNewPassword,
                                                userProfileModel.UserProfileConfirmPassword, out messages);
                if (!isCheckPasswordSuccess)
                {
                    return new Tuple<bool, List<string>>(false, messages);
                }
                updatedPassword = SecurityService.Encryption.HashSHA512(_userData.DatabaseKey.UserName.ToUpper() + userProfileModel.UserProfileNewPassword);
                System.Web.HttpContext.Current.Session[SessionConstants.LOGGED_PASSWORD] = updatedPassword;
            }
            else
            {
                updatedPassword = logininfo.Password;
            }

            if (!string.IsNullOrEmpty(updatedPassword))
            {
                logininfo.Password = updatedPassword;

                userinfo.Update(_userData.DatabaseKey);
                logininfo.Update(_userData.DatabaseKey);
                // SOM-642 - Update the personnel record for data that comes from UserInfo
                // SOM-642 - ALL personnel records associated with this userinfo record should be updated
                Personnel pe = new Personnel()
                {
                    ClientId = _userData.DatabaseKey.Client.ClientId,
                    UserInfoId = _userData.DatabaseKey.User.UserInfoId,
                    NameFirst = userinfo.FirstName,
                    NameLast = userinfo.LastName,
                    NameMiddle = userinfo.MiddleName,
                    Email = userinfo.Email,
                    InactiveFlag = !logininfo.IsActive
                };
                pe.UpdateFromUserInfo(_userData.DatabaseKey);
                ////update UpdateIndex
                //hdnUserInfoUpdateIndex.Value = userinfo.UpdateIndex.ToString();
                //hdnLoginInfoUpdateIndex.Value = logininfo.UpdateIndex.ToString();

            }
            if (userinfo.ErrorMessages != null)
            {
                messages.AddRange(userinfo.ErrorMessages);
            }
            if (logininfo.ErrorMessages != null)
            {
                messages.AddRange(logininfo.ErrorMessages);
            }


            return new Tuple<bool, List<string>>(true, messages);
        }
        //Check the password
        private bool CheckPassword(string currentPassword, string oldPassword, string newPassword, string confirmPassword, out List<string> errorMsg)
        {
            errorMsg = new List<string>();

            //Some field is missing
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                errorMsg.Add("Some field is missing");
                return false;
            }

            //If all textboxes are filled
            //New Password and Confirm Password does not matches
            if (newPassword != confirmPassword)
            {
                errorMsg.Add("New Password and Confirm Password does not match");
                return false;
            }

            //Current Password is Wrong
            if (SecurityService.Encryption.HashSHA512(_userData.DatabaseKey.UserName.ToUpper() + oldPassword) != currentPassword)
            {
                errorMsg.Add("Current Password is Wrong");
                return false;
            }

            return true;
        }
        private void RetrieveUserInfoPageControls(DataContracts.UserInfo userinfo, UserProfileModel userProfileModel)
        {
            userinfo.FirstName = userProfileModel.UserProfileFirstName;
            userinfo.LastName = userProfileModel.UserProfileLastName;
            userinfo.MiddleName = userProfileModel.UserProfileMiddleName ?? string.Empty;
            userinfo.Email = userProfileModel.UserProfileEmailAddress ?? string.Empty;


            //// Retrieve UserInfo.LocalizationCulture data
            //if (uicLocalizationCultureAndLanguage.Readable)
            //{
            //    userinfo.Localization = uicLocalizationCultureAndLanguage.Value;
            //}

            //if (uicTimeZone.Readable)
            //{
            //    userinfo.TimeZone = uicTimeZone.Value;
            //}
            // Retrieve UserInfo.InactiveFlag data            
            //userinfo.UpdateIndex = userProfileModel.LoginInfoUpdateIndex;
        }
        private void RetrieveLoginInfoPageControls(DataContracts.LoginInfo logininfo, UserProfileModel userProfileModel)
        {

            logininfo.SecurityQuestion = userProfileModel.UserProfileSecurityQuestion;
            logininfo.SecurityResponse = userProfileModel.UserProfileSecurityAnswer;
            logininfo.UpdateIndex = userProfileModel.LoginInfoUpdateIndex;
            //logininfo.UpdateIndex = hdnLoginInfoUpdateIndex.Value.ToInt();
        }
        #endregion

        #region V2-491
        public PasswordSettings RetrievePasswordSettings()
        {
            PasswordSettings passwordSettings = new PasswordSettings();
            passwordSettings.ClientId = this._userData.DatabaseKey.Client.ClientId;
            passwordSettings.RetrieveByClientId(this._userData.DatabaseKey);

            return passwordSettings;
        }
        #endregion
    }
}