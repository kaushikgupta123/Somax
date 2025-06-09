/**************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2020 by SOMAX Inc..
* All rights reserved. 
***************************************************************************************************
* User Management Business Wrapper 
***************************************************************************************************
* Date        Log Entry Person         Description
* =========== ========= ============== ============================================================
* 2020-Oct-27 V2-415    Roger Lawton   Remove the Inventory User Type - No Longer Supported
***************************************************************************************************
*/
using Business.Authentication;
//using Business.Common;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Models;
using Client.Models.Configuration.UserManagement;
using Common.Constants;
using Common.Enumerations;
using Database.Business;

using DataContracts;
using Presentation.Common;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Client.BusinessWrapper.Configuration.UserManagment
{
    public class UsermanagmentWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        UserManagementVM userdetVMLoc;
        public UsermanagmentWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
            userdetVMLoc = new UserManagementVM();
        }
        #region Populate PopulateDropdownControls
        public UserModel PopulateDropdownControls(UserManagementVM userdetVM, UserModel objUser = null)
        {
            if (objUser == null)
            {
                objUser = new UserModel();
            }
            var AllLookUpLists = GetAllLookUpList();
            if (AllLookUpLists != null)
            {
                List<DataContracts.LookupList> ShiftLookUpList = AllLookUpLists.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (ShiftLookUpList != null)
                {
                    objUser.LookupShiftList = ShiftLookUpList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }
            var craftDetails = GetLookUpList_Craft();
            if (craftDetails != null)
            {
                objUser.LookupCraftList = craftDetails.Select(x => new SelectListItem { Text = x.ClientLookUpId + " - " + x.Description + " - " + x.ChargeRate, Value = x.CraftId.ToString() });
            }
            //var UserTypeList = NewLoadUserTypes(userdetVM, objUser);
            //if (UserTypeList != null)
            //{
            //    objUser.LookupUserTypeList = UserTypeList.Select(x => new SelectListItem { Text = x.Text, Value = x.Value });
            //}
            //var SecurityProfilesList = LoadSecurityProfilesList(userdetVM, objUser);
            //if (SecurityProfilesList != null)
            //{
            //    objUser.LookupSecurityProfileIdList = SecurityProfilesList.Select(x => new SelectListItem { Text = x.Text, Value = x.Value });
            //}
            return objUser;
        }
        public List<SelectListItem> LoadSecurityProfilesList(UserManagementVM userdetVMLoc, UserModel userdet = null)
        {
            //LocalizeControls(userdetVMLoc, LocalizeResourceSetConstants.UserDetails);
            List<KeyValuePair<string, string>> profiles = new List<KeyValuePair<string, string>>();
            DataContracts.SecurityProfile secprof = new DataContracts.SecurityProfile()
            {
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            List<DataContracts.SecurityProfile> splist = new List<DataContracts.SecurityProfile>();
            splist = secprof.RetrieveAllProfiles(this.userData.DatabaseKey);
            if (userData.DatabaseKey.Client.ClientId != 4 && userData.DatabaseKey.Client.ClientId != 6)
            {
                splist.RemoveAll(SecurityProfile => SecurityProfile.SecurityProfileId == 11);
            }
            List<DataContracts.SecurityProfile> splist_sorted = splist.AsQueryable().OrderBy(x => x.SortOrder).ToList();
            List<KeyValuePair<string, string>> prof_names = new List<KeyValuePair<string, string>>();
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Admin, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Admin").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile7, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile7").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_WorkRequest, userdetVMLoc.Loc.Where(a => a.ResourceId == "spnWorkRequest").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Sanitation, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Sanitation").FirstOrDefault().Value.ToString()));
            if (userData.DatabaseKey.Client.ClientId == 4 || userData.DatabaseKey.Client.ClientId == 6)
            {
                prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Inventory, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Inventory").FirstOrDefault().Value.ToString()));
            }
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile1, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile1").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile2, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile2").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile3, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile3").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile4, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile4").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile5, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile5").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile6, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile6").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile8, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile8").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile9, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile9").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile10, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile10").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile11, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile11").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile12, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile12").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile13, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile13").FirstOrDefault().Value.ToString()));
            prof_names.Add(new KeyValuePair<string, string>(SecurityProfileConstants.SOMAX_Profile14, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Profile14").FirstOrDefault().Value.ToString()));

            // SOM-1717 Mods to clarify the user type and security profile 
            // Go throught the profile contract list and set up the profile KVP for the lookup

            foreach (DataContracts.SecurityProfile sp in splist_sorted)
            {
                string prof_desc = (from prof_item in prof_names where prof_item.Key == sp.Name select prof_item.Value).First();
                profiles.Add(new KeyValuePair<string, string>(sp.SecurityProfileId.ToString(), prof_desc));

                // Insert the reference item - just after sanitation
                if (sp.Name == SecurityProfileConstants.SOMAX_Sanitation)
                {
                    profiles.Add(new KeyValuePair<string, string>("0", UserTypeConstants.Reference));
                }
            }

            // The only security profile for an empty (profileid = 0) is "Reference"  
            // all other security profiles have a non-zero value.  
            long profileid = userdet.SecurityProfileId ?? 0;
            //if (profileid == 0)
            //{
            //    string cUserType = userdet.UserType;
            //    string cProfileName;
            //    switch (cUserType)
            //    {
            //        case UserTypeConstants.Reference:
            //            cProfileName = "";
            //            break;
            //        case UserTypeConstants.WorkRequest:
            //            cProfileName = SecurityProfileConstants.SOMAX_WorkRequest;
            //            break;
            //        case UserTypeConstants.Inventory:
            //            cProfileName = SecurityProfileConstants.SOMAX_Inventory;
            //            break;
            //        default:
            //            cProfileName = SecurityProfileConstants.SOMAX_Admin;
            //            break;
            //    }
            //    if (cProfileName == "")
            //        profileid = 0;
            //    else
            //        profileid = (from item in splist where item.Name == cProfileName select item.SecurityProfileId).First();
            //}
            return profiles.Select(s => new SelectListItem
            {
                Text = s.Value,
                Value = s.Key
            }).ToList();
        }

        protected List<SelectListItem> NewLoadUserTypes(UserManagementVM userdetVMLoc, UserModel userdet = null)
        {
            //LocalizeControls(userdetVMLoc, LocalizeResourceSetConstants.UserDetails);
            List<KeyValuePair<string, string>> user_types = new List<KeyValuePair<string, string>>();
            user_types = new List<KeyValuePair<string, string>>();
            user_types.Add(new KeyValuePair<string, string>(UserTypeConstants.Full, userdetVMLoc.Loc.Where(a => a.ResourceId == "Full").FirstOrDefault().Value.ToString()));
            user_types.Add(new KeyValuePair<string, string>(UserTypeConstants.Admin, "Admin"));
            user_types.Add(new KeyValuePair<string, string>(UserTypeConstants.WorkRequest, userdetVMLoc.Loc.Where(a => a.ResourceId == "spnWorkRequest").FirstOrDefault().Value.ToString()));
            user_types.Add(new KeyValuePair<string, string>(UserTypeConstants.Reference, userdetVMLoc.Loc.Where(a => a.ResourceId == "Reference").FirstOrDefault().Value.ToString()));


            //if (userData.DatabaseKey.Client.ClientId == 4 || userData.DatabaseKey.Client.ClientId == 6)
            //{
            //    user_types.Add(new KeyValuePair<string, string>(UserTypeConstants.Inventory, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Inventory").FirstOrDefault().Value.ToString()));
            //}
            //string usertype = userdet.UserType;

            //if (string.IsNullOrEmpty(usertype))
            //{
            //    usertype = UserTypeConstants.Full;
            //}
            return user_types.Select(s => new SelectListItem
            {
                Text = s.Value,
                Value = s.Key
            }).ToList();
        }

        protected List<SelectListItem> LoadUserTypes(UserManagementVM userdetVMLoc, UserModel userdet = null)
        {
            //LocalizeControls(userdetVMLoc, LocalizeResourceSetConstants.UserDetails);
            List<KeyValuePair<string, string>> user_types = new List<KeyValuePair<string, string>>();
            user_types = new List<KeyValuePair<string, string>>();
            user_types.Add(new KeyValuePair<string, string>(UserTypeConstants.Full, userdetVMLoc.Loc.Where(a => a.ResourceId == "Full").FirstOrDefault().Value.ToString()));
            user_types.Add(new KeyValuePair<string, string>(UserTypeConstants.Reference, userdetVMLoc.Loc.Where(a => a.ResourceId == "Reference").FirstOrDefault().Value.ToString()));
            user_types.Add(new KeyValuePair<string, string>(UserTypeConstants.WorkRequest, userdetVMLoc.Loc.Where(a => a.ResourceId == "spnWorkRequest").FirstOrDefault().Value.ToString()));
            // V2-415
            //if (userData.DatabaseKey.Client.ClientId == 4 || userData.DatabaseKey.Client.ClientId == 6)
            //{
            //    user_types.Add(new KeyValuePair<string, string>(UserTypeConstants.Inventory, userdetVMLoc.Loc.Where(a => a.ResourceId == "SOMAX_Inventory").FirstOrDefault().Value.ToString()));
            //}
            string usertype = userdet.UserType;

            if (string.IsNullOrEmpty(usertype))
            {
                usertype = UserTypeConstants.Full;
            }
            return user_types.Select(s => new SelectListItem
            {
                Text = s.Value,
                Value = s.Key
            }).ToList();
        }
        public List<DataModel> GetLookUpList_Craft()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            List<Craft> CraftList = new Craft().RetriveAllForSite(userData.DatabaseKey).Where(a => a.InactiveFlag == false).ToList();
            foreach (var c in CraftList)
            {
                dModel = new DataModel();
                dModel.CraftId = c.CraftId;
                dModel.ClientLookUpId = c.ClientLookupId;
                dModel.Description = c.Description;
                dModel.ChargeRate = c.ChargeRate;
                model.data.Add(dModel);
            }
            return model.data;
        }

        //#region Localization control
        //private void LocalizeControls(LocalisationBaseVM objComb, string ResourceType)
        //{
        //    LoginCacheSet _logCache = new LoginCacheSet();
        //    var connstring = ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString();
        //    List<Localizations> locSpecificPageCache = _logCache.GetLocalizationCommon(connstring, ResourceType, userData.DatabaseKey.User.Localization);
        //    List<Localizations> locGlobalCache = _logCache.GetLocalizationCommon(connstring, LocalizeResourceSetConstants.Global, userData.DatabaseKey.User.Localization);
        //    if (locSpecificPageCache != null && locSpecificPageCache.Count > 0)
        //    {
        //        objComb.Loc = locSpecificPageCache;
        //    }
        //    else
        //    {
        //        locSpecificPageCache = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), ResourceType, "en-us");
        //        objComb.Loc = locSpecificPageCache;
        //    }
        //    if (locGlobalCache != null && locGlobalCache.Count > 0)
        //    {
        //        objComb.Loc.AddRange(locGlobalCache);
        //    }
        //    else
        //    {
        //        locGlobalCache = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), ResourceType, "en-us");
        //        objComb.Loc.AddRange(locGlobalCache);
        //    }
        //}
        //#endregion      

        #endregion

        #region Add-Edit  
        void RetrieveUserDetailsControls(UserDetails userde, UserModel _userModels, long ObjectId)
        {
            userde.ClientId = userData.DatabaseKey.Client.ClientId;
            userde.UserInfoId = ObjectId;
            userde.SiteControlled = userData.DatabaseKey.Client.SiteControl;
            userde.ClientUpdateIndex = userData.DatabaseKey.Client.UpdateIndex;
            // userde.SecurityQuestion = _userModels.SecurityQuestion;    /*V2-332*/
            // userde.SecurityResponse = _userModels.SecurityResponse;   /*V2-332*/
            // userde.IsActive = _userModels.IsActive;                  /*V2-332*/
            if (userde.IsActive)
                userde.Personnel.InactiveFlag = false;
            else
                userde.Personnel.InactiveFlag = true;
            userde.FirstName = _userModels.FirstName ?? string.Empty;
            userde.Personnel.NameFirst = _userModels.FirstName ?? string.Empty;
            userde.MiddleName = _userModels.MiddleName ?? string.Empty;
            userde.Personnel.NameMiddle = _userModels.MiddleName ?? string.Empty;
            userde.LastName = _userModels.LastName ?? string.Empty;
            userde.Personnel.NameLast = _userModels.LastName ?? string.Empty;
            userde.Email = _userModels.Email ?? string.Empty;
            userde.Personnel.Email = _userModels.Email ?? string.Empty;
            // V2-805 - 2022-11-21 - RKL
            // You cannot change the user's default site 
            // nor can you change the personnel.siteid value       
            //userde.DefaultSiteId = userData.DatabaseKey.User.DefaultSiteId;
            //userde.Personnel.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            //userde.UserType = _userModels.UserType;                           /*V2-332*/
            userde.SecurityProfileId = _userModels.SecurityProfileId ?? 0;
            //userde.IsSuperUser = _userModels.IsSuperUser;                     /*V2-332*/
            userde.Personnel.CraftId = _userModels.CraftId ?? 0;
            userde.Personnel.Shift = _userModels.Shift ?? string.Empty;
            userde.Personnel.Address1 = _userModels.Address1 ?? string.Empty;
            userde.Personnel.Address2 = _userModels.Address2 ?? string.Empty;
            userde.Personnel.Address3 = _userModels.Address3 ?? string.Empty;
            userde.Personnel.AddressCity = _userModels.AddressCity ?? string.Empty;
            userde.Personnel.AddressState = _userModels.AddressState ?? string.Empty;
            userde.Personnel.AddressCountry = _userModels.AddressCountry ?? string.Empty;
            userde.Personnel.AddressPostCode = _userModels.AddressPostCode ?? string.Empty;
            userde.Personnel.Phone1 = _userModels.Phone1 ?? string.Empty;
            userde.Personnel.Phone2 = _userModels.Phone2 ?? string.Empty;
            userde.Personnel.Buyer = _userModels.Buyer;
            userde.EmployeeId=_userModels.EmployeeId ?? string.Empty; //V2-877
        }
        void ValidateUserType(UserDetails userdets)
        {
            string cUserType = userdets.UserType;
            DataContracts.SecurityProfile secprof = new DataContracts.SecurityProfile()
            {
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            if (cUserType == UserTypeConstants.Full)
            {
                if (userdets.SecurityProfileId == 0)
                {
                    secprof.Name = SecurityProfileConstants.SOMAX_Admin;
                    secprof.RetrieveByName(userData.DatabaseKey);
                    userdets.SecurityProfileId = secprof.SecurityProfileId;
                }
            }
            else
            {
                userdets.IsSuperUser = false;
                switch (cUserType)
                {
                    case UserTypeConstants.Reference:
                        secprof.Name = "";
                        break;
                    case UserTypeConstants.WorkRequest:
                        secprof.Name = SecurityProfileConstants.SOMAX_WorkRequest;
                        break;
                        // SOM-748
                        // V2-415 - RKL - 2020-Oct-27
                        //case UserTypeConstants.Inventory:
                        //    secprof.Name = SecurityProfileConstants.SOMAX_Inventory;
                        //    break;
                }
                secprof.RetrieveByName(userData.DatabaseKey);
                userdets.SecurityProfileId = secprof.SecurityProfileId;
            }
        }

        private void ResetPassword(long ClientId, long UserInfoId)
        {

            DataContracts.DatabaseKey Localdbkey = new DataContracts.DatabaseKey();
            //Localdbkey = UserData.DatabaseKey;

            DataContracts.Client client = new DataContracts.Client();
            DataContracts.UserInfo userinfo = new DataContracts.UserInfo()
            {
                UserInfoId = UserInfoId
            };

            if (ClientId > 0)
            {
                client.ClientId = userData.DatabaseKey.Client.ClientId;
                client.CreatedClientId = UserSession.AccessClientId;
                client.RetrieveBySomaxAdmin(userData.DatabaseKey);
            }
            Localdbkey.Client = client;
            Localdbkey.AdminConnectionString = userData.DatabaseKey.AdminConnectionString;

            Localdbkey.User = userData.DatabaseKey.User;
            userinfo.Retrieve(Localdbkey);
            Localdbkey.User = userinfo;

            AccountPassword password = new AccountPassword();
            IP_AccountPasswordData ipData = new IP_AccountPasswordData()
            {
                DbKey = Localdbkey,
                UserName = userinfo.UserName
                // IsNew = true
            };

            // If the username is found, then send an email to user with a temporary password and a link for resetting password.
            password.RequestNewPassword(ipData);

            UserInfo userInfo = new UserInfo() { ClientId = password.LoginInfo.ClientId, UserInfoId = password.LoginInfo.UserInfoId };
            userInfo.Retrieve(Localdbkey);

            // Build the url of the reset password page
            string[] url = new string[4];
            url[0] = HttpContext.Current.Request.Url.Host;
            url[1] = VirtualPathUtility.ToAbsolute("~/ResetPassword.aspx");
            url[2] = QueryStringConstants.RESET_PASSWORD;
            url[3] = password.LoginInfo.ResetPasswordCode.ToString();

            string resetUrl = string.Format("http://{0}{1}?{2}={3}", url);

            StringBuilder body = new StringBuilder();
            body.Append("Your account has been created.  Please reset your password.<br /><br />");
            body.Append("In order to reset the password, please click the following URL:<br/>");
            body.Append(string.Format("<a href='{0}'>{0}</a><br /><br />", resetUrl));
            //body.Append("You will be prompted to enter the following temporary password and answer a security question.<br />");
            body.Append("You will be prompted to enter the following temporary password.<br />");
            body.Append(string.Format("Temporary Password: <b>{0}<b/>", password.TempPassword));

            //Email email = new Email() { Body = body.ToString(), Subject = "New Password" };
            EmailModule email = new Presentation.Common.EmailModule() { Body = body.ToString(), Subject = "New Password" };
            email.Recipients.Add(userinfo.Email);

            //  email.Recipients.Add(_userModel.Email);
            email.SendEmail();
        }
        #endregion

        #region AddNew User 

        //public UserModel AddNewUser(UserModel _userModel)
        //{
        //    string msg = String.Empty;
        //    bool valid = true;
        //    List<string> ErroMsgList = new List<string>();
        //    UserDetails userdetails = new UserDetails()
        //    {
        //        ClientId = userData.DatabaseKey.Client.ClientId,
        //        SiteControlled = userData.DatabaseKey.Client.SiteControl,
        //    };
        //    RetrieveUserDetails(userdetails, _userModel);
        //    ValidateUserType(userdetails);
        //    userdetails.ValidateNewUserAdd(this.userData.DatabaseKey);
        //    ErroMsgList.AddRange(userdetails.ErrorMessages);
        //    if (valid == true && ErroMsgList.Count == 0) // Require ErroMsgList.Count checking .Valid checking not necessary  * Anadi
        //    {
        //        if (userdetails.IsActive && userData.DatabaseKey.Client.SiteControl)
        //        {
        //            DataContracts.Site site = new DataContracts.Site()
        //            {
        //                ClientId = userData.DatabaseKey.Client.ClientId,
        //                SiteId = userData.DatabaseKey.User.DefaultSiteId,
        //                IsSuperUser = userdetails.IsSuperUser,
        //                UserType = userdetails.UserType
        //            };
        //            site.ValidateNewUserAdd(this.userData.DatabaseKey);
        //            ErroMsgList.AddRange(site.ErrorMessages);
        //        }
        //        if (ErroMsgList.Count == 0)
        //        {
        //            userdetails.CreateNewUserWithLoginData(userData.DatabaseKey);
        //            if (userdetails.UserInfoId > 0)
        //            {
        //                userdetails.Personnel.UserInfoId = userdetails.UserInfoId;
        //                userdetails.Personnel.Create(userData.DatabaseKey);
        //                DataContracts.UserPermission userpermission = new DataContracts.UserPermission()
        //                {
        //                    ClientId = userData.DatabaseKey.Client.ClientId,
        //                    UserInfoId = userdetails.UserInfoId,
        //                    SiteId = userData.DatabaseKey.User.DefaultSiteId,
        //                    PermissionType = "G",
        //                    UpdateIndex = 0,
        //                };
        //                userpermission.Create(this.userData.DatabaseKey);
        //                if (userdetails.IsActive)
        //                {
        //                    userdetails.RetrieveSiteUserCounts(userData.DatabaseKey);
        //                    DataContracts.Site site = new DataContracts.Site()
        //                    {
        //                        ClientId = userData.DatabaseKey.Client.ClientId,
        //                        SiteId = userData.DatabaseKey.User.DefaultSiteId
        //                    };
        //                    site.Retrieve(userData.DatabaseKey);
        //                    site.AppUsers = Convert.ToInt32(userdetails.CountWebAppUser);
        //                    site.LimitedUsers = Convert.ToInt32(userdetails.CountLimitedUser);
        //                    site.WorkRequestUsers = Convert.ToInt32(userdetails.CountWorkRequestUser);
        //                    site.SanitationUsers = Convert.ToInt32(userdetails.CountSanitationUser);
        //                    site.SuperUsers = Convert.ToInt32(userdetails.CountSuperUser);
        //                    site.Update(this.userData.DatabaseKey);
        //                }
        //                if (!string.IsNullOrEmpty(_userModel.Email))
        //                {
        //                    SendLoginCredentialByMailToUser(_userModel);
        //                }
        //            }
        //        }
        //    }
        //    _userModel.UserInfoId = userdetails.UserInfoId;
        //    _userModel.ErrorMessages = ErroMsgList; //userdetails.ErrorMessages change to ErroMsgList for all type of error  * Anadi
        //    return _userModel;
        //}

        //void RetrieveUserDetails(UserDetails userde, UserModel _userModels)
        //{
        //    if (!string.IsNullOrWhiteSpace(_userModels.Password))
        //    {
        //        userde.Password = Encryption.SHA512Encrypt(_userModels.UserName.Trim().ToUpper() + _userModels.Password);
        //    }
        //    userde.UserName = _userModels.UserName;
        //    userde.Personnel.ClientLookupId = _userModels.UserName;//18-04
        //    userde.SecurityQuestion = _userModels.SecurityQuestion;
        //    userde.SecurityResponse = _userModels.SecurityResponse;
        //    userde.IsActive = _userModels.IsActive;
        //    if (userde.IsActive)
        //        userde.Personnel.InactiveFlag = false;
        //    else
        //        userde.Personnel.InactiveFlag = true;
        //    userde.FirstName = _userModels.FirstName ?? string.Empty;
        //    userde.Personnel.NameFirst = _userModels.FirstName ?? string.Empty;
        //    userde.MiddleName = _userModels.MiddleName ?? string.Empty;
        //    userde.Personnel.NameMiddle = _userModels.MiddleName ?? string.Empty;
        //    userde.LastName = _userModels.LastName ?? string.Empty;
        //    userde.Personnel.NameLast = _userModels.LastName ?? string.Empty;
        //    userde.Email = _userModels.Email ?? string.Empty;
        //    userde.Localization = userData.DatabaseKey.User.Localization;
        //    userde.UIConfiguration = userData.DatabaseKey.User.UIConfiguration;
        //    userde.DefaultSiteId = userData.DatabaseKey.User.DefaultSiteId;
        //    userde.Personnel.SiteId = userData.DatabaseKey.User.DefaultSiteId;
        //    userde.ResultsPerPage = 10;
        //    userde.TimeZone = userData.DatabaseKey.User.TimeZone;
        //    userde.IsPasswordTemporary = false;
        //    userde.UserType = _userModels.UserType;
        //    userde.SecurityProfileId = _userModels.SecurityProfileId ?? 0;
        //    userde.IsSuperUser = _userModels.IsSuperUser;
        //    userde.Personnel.CraftId = _userModels.CraftId ?? 0;
        //    userde.Personnel.Shift = _userModels.Shift ?? string.Empty;
        //    userde.Personnel.Address1 = _userModels.Address1 ?? string.Empty;
        //    userde.Personnel.Address2 = _userModels.Address2 ?? string.Empty;
        //    userde.Personnel.Address3 = _userModels.Address3 ?? string.Empty;
        //    userde.Personnel.AddressCity = _userModels.AddressCity ?? string.Empty;
        //    userde.Personnel.AddressState = _userModels.AddressState ?? string.Empty;
        //    userde.Personnel.AddressCountry = _userModels.AddressCountry ?? string.Empty;
        //    userde.Personnel.AddressPostCode = _userModels.AddressPostCode ?? string.Empty;
        //    userde.Personnel.Phone1 = _userModels.Phone1 ?? string.Empty;
        //    userde.Personnel.Phone2 = _userModels.Phone2 ?? string.Empty;
        //    userde.Personnel.Buyer = _userModels.Buyer;
        //}

        //protected void SendLoginCredentialByMailToUser(UserModel _userModel)
        //{
        //    string resetUrl = GetHostedUrl();
        //    StringBuilder body = new StringBuilder();
        //    string Emailbody = string.Empty;
        //    var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Views\Configuration\UserManagement\UserCreatemailTemplate.cshtml");
        //    var templateContent = string.Empty;
        //    using (var reader = new StreamReader(templateFilePath))
        //    {
        //        templateContent = reader.ReadToEnd();
        //    }
        //    string emailHtmlBody = ParseTemplate(templateContent);
        //    string output = emailHtmlBody.
        //                    Replace("headerBgURL", resetUrl + SomaxAppConstants.HeaderMailTemplate).
        //                    Replace("somaxLogoURL", resetUrl + SomaxAppConstants.SomaxLogoForMailTemplate).
        //                    Replace("spnfirstame", _userModel.FirstName).
        //                    Replace("spnlastname", _userModel.LastName).
        //                    Replace("spUserID", _userModel.UserName).
        //                    Replace("spPassword", _userModel.Password).
        //                    Replace("spnloginurl", resetUrl).
        //                    Replace("footerURL", resetUrl + SomaxAppConstants.FooterMailTemplate);
        //    body.Append(output);
        //    Email email = new Email() { Body = body.ToString(), Subject = SomaxAppConstants.UserAddSubject};
        //    var adminMail = userData.DatabaseKey.User.Email;
        //    string targetMail = String.IsNullOrEmpty(_userModel.Email) ? adminMail : _userModel.Email;
        //    email.Recipients.Add(targetMail);
        //    email.Send();
        //}

        public int CountIfUserExist(string UserName)
        {
            UserDetails _userdetails = new UserDetails();
            _userdetails.ClientId = userData.DatabaseKey.Client.ClientId;
            _userdetails.UserName = UserName;
            var cnt = _userdetails.RetrieveCountForUserExist(userData.DatabaseKey);
            int count = cnt.Count;
            return count;
        }

        #endregion

        #region Notes
        public List<string> AddNotes(NotesModel notesModel, string tableName)
        {
            Notes notes = new Notes()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                OwnerId = notesModel.UserInfoId,
                OwnerName = notesModel.OwnerName,
                Subject = notesModel.Subject,
                Content = notesModel.Content,
                Type = notesModel.Type ?? "",
                ObjectId = notesModel.ObjectId,
                TableName = "Personnel"
            };
            notes.Create(userData.DatabaseKey);
            return notes.ErrorMessages;
        }
        public NotesModel RetrieveNotesForEdit(long objectId, long notesId)
        {
            NotesModel objNotesModel = new Models.NotesModel();
            Notes note = new Notes() { NotesId = notesId };
            note.Retrieve(userData.DatabaseKey);
            objNotesModel.NotesId = note.NotesId;
            objNotesModel.updatedindex = note.UpdateIndex;
            objNotesModel.Subject = note.Subject;
            objNotesModel.Content = note.Content;
            objNotesModel.ObjectId = objectId;
            objNotesModel.updatedindex = note.UpdateIndex;
            return objNotesModel;
        }
        
        public List<string> EditNotes(NotesModel notesModel, string tableName)
        {
            Notes notes = new Notes()
            {
                NotesId = notesModel.NotesId
            };
            notes.Retrieve(userData.DatabaseKey);
            notes.Subject = notesModel.Subject;
            notes.Content = notesModel.Content;
            notes.Type = notesModel.Type ?? "";
            notes.UpdateIndex = notesModel.updatedindex;
            notes.TableName = "Personnel";
            notes.ObjectId = notesModel.ObjectId;
            notes.Update(userData.DatabaseKey);
            return notes.ErrorMessages;
        }
        //public bool DeleteNotes(long notesId, long userInfoId)
        //{
        //    try
        //    {
        //        UserDetails userdet = new DataContracts.UserDetails();
        //        userdet.UserInfoId = userInfoId; // ObjectId;
        //        userdet.ClientId = userData.DatabaseKey.Client.ClientId;
        //        userdet.RetrieveUserDetailsByUserInfoID(this.userData.DatabaseKey);
        //        userdet.RetrievePersonnelByUserInfoId(this.userData.DatabaseKey, string.Empty);
        //        Notes notes = new Notes()
        //        {
        //            NotesId = notesId
        //        };
        //        notes.Delete(userData.DatabaseKey);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        #endregion

        #region Reset-Password
        public ResetPasswordModel GetUserDetails(long UserInfoId)
        {
            ResetPasswordModel objResetPasswordModel = new ResetPasswordModel();
            DataContracts.UserInfo userInfo = new UserInfo()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                UserInfoId = UserInfoId
            };
            DataContracts.UserInfo userInfoForRetMail = new DataContracts.UserInfo() { UserInfoId = UserInfoId };
            userInfoForRetMail.Retrieve(userData.DatabaseKey);
            userInfo.RetrieveLoginInfoByUserInfoId2(userData.DatabaseKey);
            objResetPasswordModel.UserInfoId = userInfo.UserInfoId;
            objResetPasswordModel.UserName = userInfo.LoginUserName;
            objResetPasswordModel.FirstName = userInfo.FirstName;
            objResetPasswordModel.MiddleName = userInfo.MiddleName;
            objResetPasswordModel.LastName = userInfo.LastName;
            objResetPasswordModel.EmailAddress = userInfoForRetMail.Email;
            return objResetPasswordModel;
        }
        public string ResetPassword(ResetPasswordModel objResetPasswordModel)
        {
            string message = string.Empty;
            DataContracts.UserInfo userInfo = new DataContracts.UserInfo() { UserInfoId = objResetPasswordModel.UserInfoId };
            userInfo.Retrieve(userData.DatabaseKey);
            DataContracts.LoginInfo loginInfo = new DataContracts.LoginInfo() { UserInfoId = objResetPasswordModel.UserInfoId };
            loginInfo.RetrieveByUserInfoId(userData.DatabaseKey);

            if (loginInfo.LoginInfoId > 0)
            {
                loginInfo.Password = Encryption.SHA512Encrypt(loginInfo.UserName.Trim().ToUpper() + objResetPasswordModel.ConfirmPassword);
                loginInfo.FailedAttempts = 0;
                loginInfo.Update(userData.DatabaseKey);
                #region Email Send
                userInfo.Retrieve(userData.DatabaseKey);
                string EmailId = userInfo.Email;
                if (!String.IsNullOrEmpty(EmailId))
                {
                    StringBuilder body = new StringBuilder();
                    string Emailbody = string.Empty;
                    var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Views\Configuration\UserManagement\ResetPasswordMailTemplate.cshtml");
                    var templateContent = string.Empty;
                    using (var reader = new StreamReader(templateFilePath))
                    {
                        templateContent = reader.ReadToEnd();
                    }
                    string emailHtmlBody = ParseTemplate(templateContent);
                    string resetUrl = GetHostedUrl();

                    string output = emailHtmlBody.Replace("firstname", objResetPasswordModel.FirstName).
                                    Replace("headerBgURL", resetUrl + SomaxAppConstants.HeaderMailTemplate).
                                    Replace("somaxLogoURL", resetUrl + SomaxAppConstants.SomaxLogoForMailTemplate).
                                    Replace("username", objResetPasswordModel.UserName).Replace("spnloginurl", resetUrl).
                                    Replace("changedby", this.userData.DatabaseKey.UserName).
                                    Replace("footerURL", resetUrl + SomaxAppConstants.FooterMailTemplate);
                    body.Append(output);
                    //Email email = new Email() { Body = body.ToString(), Subject = "SOMAX Password Reset" };
                    EmailModule email = new Presentation.Common.EmailModule() { Body = body.ToString(), Subject = "SOMAX Password Reset" };
                    email.Recipients.Add(EmailId);
                    try
                    {
                        email.SendEmail();
                        message = "SuccessAlert";
                    }
                    catch (Exception ex)
                    {
                        message = "SuccessEmailFail";
                    }
                }
                else
                {
                    message = "TargetUserAlert";
                }
                #endregion
            }
            return message;
        }
        public static string ParseTemplate(string content)
        {
            string _mode = DateTime.Now.Ticks + new Random().Next(1000, 100000).ToString();
            Razor.Compile(content, _mode);
            return Razor.Parse(content);
        }
        #endregion

        #region Contacts
        public List<UserManagementContactModel> PopulateUmContacts(long perosnnelId)
        {

            Contact contact = new Contact()
            {
                ObjectId = perosnnelId,
                TableName = "Personnel"
            };
            var contactList = contact.RetrieveByObjectId(userData.DatabaseKey);
            UserManagementContactModel objUserManagementContactModel;
            List<UserManagementContactModel> UmContactList = new List<UserManagementContactModel>();
            foreach (var p in contactList)
            {
                objUserManagementContactModel = new UserManagementContactModel();
                objUserManagementContactModel.Name = p.Name;
                objUserManagementContactModel.Phone1 = p.Phone1;
                objUserManagementContactModel.Email1 = p.Email1;
                objUserManagementContactModel.OwnerName = p.OwnerName;
                objUserManagementContactModel.ObjectId = p.ObjectId;
                objUserManagementContactModel.ContactId = p.ContactId;
                objUserManagementContactModel.UpdateIndex = p.UpdateIndex;
                objUserManagementContactModel.AddressCountry = p.AddressCountry;
                UmContactList.Add(objUserManagementContactModel);
            }
            return UmContactList;
        }
        public List<string> AddUmContacts(UserManagementContactModel umContact)
        {
            Contact contact = new Contact()
            {
                TableName = "Personnel",
                OwnerId = umContact.OwnerId,
                OwnerName = umContact.OwnerName,
                ObjectId = umContact.ObjectId,
                Name = umContact.Name,
                Address1 = umContact.Address1,
                Address2 = umContact.Address2,
                Address3 = umContact.Address3,
                AddressCity = umContact.AddressCity,
                AddressCountry = umContact.AddressCountry,
                AddressPostCode = umContact.AddressPostCode,
                AddressState = umContact.AddressState,
                Phone1 = umContact.Phone1,
                Phone2 = umContact.Phone2,
                Phone3 = umContact.Phone3,
                Email1 = umContact.Email1,
                Email2 = umContact.Email2
            };
            contact.Create(userData.DatabaseKey);
            return contact.ErrorMessages;
        }
        public UserManagementContactModel RetrieveContactsForEdit(long contactId)
        {
            Contact contact = new Contact()
            {
                ContactId = contactId
            };
            contact.Retrieve(userData.DatabaseKey);
            UserManagementContactModel uContact = new UserManagementContactModel();
            uContact.Name = contact.Name;
            uContact.Address1 = contact.Address1;
            uContact.Address2 = contact.Address2;
            uContact.Address3 = contact.Address3;
            uContact.AddressCity = contact.AddressCity;
            uContact.AddressCountry = contact.AddressCountry;
            uContact.AddressState = contact.AddressState;
            uContact.AddressPostCode = contact.AddressPostCode;
            uContact.Phone1 = contact.Phone1;
            uContact.Phone2 = contact.Phone2;
            uContact.Phone3 = contact.Phone3;
            uContact.Email1 = contact.Email1;
            uContact.Email2 = contact.Email2;
            uContact.UpdateIndex = contact.UpdateIndex;
            return uContact;
        }
        public List<string> EditUmContacts(UserManagementContactModel umContact)
        {
            Contact contact = new Contact()
            {
                ContactId = umContact.ContactId
            };
            contact.Retrieve(userData.DatabaseKey);
            contact.Name = umContact.Name;
            contact.TableName = "Personnel";
            contact.ObjectId = umContact.PersonnelId;
            contact.Address1 = umContact.Address1 ?? "";
            contact.Address2 = umContact.Address2 ?? "";
            contact.Address3 = umContact.Address3 ?? "";
            contact.AddressCountry = umContact.AddressCountry ?? "";
            contact.AddressCity = umContact.AddressCity ?? "";
            contact.AddressState = umContact.AddressState ?? "";
            contact.AddressPostCode = umContact.AddressPostCode ?? "";
            contact.Phone1 = umContact.Phone1 ?? "";
            contact.Phone2 = umContact.Phone2 ?? "";
            contact.Phone3 = umContact.Phone3 ?? "";
            contact.Email1 = umContact.Email1 ?? "";
            contact.Email2 = umContact.Email2 ?? "";
            contact.UpdateIndex = umContact.UpdateIndex;
            contact.Update(userData.DatabaseKey);
            return contact.ErrorMessages;
        }
        public bool UmContactDelete(long contactId)
        {
            try
            {
                Contact contact = new Contact()
                {
                    ContactId = contactId
                };
                contact.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Contacts
        #region Edit User
        public bool IsbbuUsers(string UserName)
        {
            bool IsbbuUser = true;

            List<string> bbuEngUsers = new List<string>();
            bbuEngUsers.Add("bbubvoeng");
            bbuEngUsers.Add("bbudbqeng");
            bbuEngUsers.Add("bbuelkeng");
            bbuEngUsers.Add("BBUESENG");
            bbuEngUsers.Add("BBUFWNENG");
            bbuEngUsers.Add("BBUFWSENG");
            bbuEngUsers.Add("bbugpeng");
            bbuEngUsers.Add("BBUHOUENG");
            bbuEngUsers.Add("BBULVENG");
            bbuEngUsers.Add("bbumrdeng");
            bbuEngUsers.Add("BBUNRENG");
            bbuEngUsers.Add("BBUORLENG");
            bbuEngUsers.Add("BBURWCGRIMSAW");
            bbuEngUsers.Add("bbussceng");
            bbuEngUsers.Add("BBUTPKENG");
            bbuEngUsers.Add("cgrimshaw@bbumail.com");
            bbuEngUsers.Add("swolff@bbumail.com");
            bbuEngUsers.Add("somax_bbu_northumb");
            bbuEngUsers.Add("bbu_somax");
            bbuEngUsers.Add("BBU_ORACLE");
            //bbuEngUsers.Add("somax_bbu_rockwall");
            string currentuser = UserName.Trim();
            string loggedinuser = userData.DatabaseKey.UserName.Trim();
            if (bbuEngUsers.Contains(currentuser, StringComparer.OrdinalIgnoreCase))
            {
                IsbbuUser = bbuEngUsers.Contains(loggedinuser, StringComparer.OrdinalIgnoreCase);
            }
            return IsbbuUser;


        }
        #endregion       
        #region V2-332
        //void RetrieveUserDetails(UserDetails userde, UserModel _userModels)
        /// <summary>
        /// GetModelData fills in the Security ProfileId, IsSuperUser and IsSiteAdmin Flags
        /// The Admin Security Profile is also filled in based on the Package Level and Product Grouping
        /// Package Levels:
        ///   Basic
        ///   Professional
        ///   Enterprise
        /// Product Grouping
        /// 1  - CMMS Only
        /// 2  - Sanitation Only 
        /// 3  - APM Only 
        /// 4  - CMMS and Sanitation
        /// 5  - CMMS and APM
        /// 6  - CMMS, Sanitation and APM
        /// 7  - Fleet Only 
        /// 8  - CMMS and Fleet
        /// 9  - Fleet and APM
        /// 10 - CMMS, Fleet and APM
        /// </summary>
        /// <param name="userde"></param>
        /// <param name="_userModels"></param>
        void GetModelData(UserDetails userde, UserModel _userModels)
        {
            string userType = string.Empty;
            int productGrouping = _userModels.ProductGrouping;
            string packageLevel = string.Empty;

            if (!String.IsNullOrEmpty(_userModels.UserType))
            {
                userType = _userModels.UserType.ToUpper();
            }

            if (!String.IsNullOrEmpty(_userModels.PackageLevel))
            {
                packageLevel = _userModels.PackageLevel.ToUpper();
            }

            //Full User: V2-332:page21 , V2-401:page10
            if (userType == UserTypeConstants.Full.ToUpper())
            {
                userde.SecurityProfileId = _userModels.SecurityProfileId ?? 0;
                userde.IsSuperUser = false;
                userde.IsSiteAdmin = false;
            }
            //Reference User(From User Type Tab): V2-332:page32, V2-401:page25
            else if (userType == UserTypeConstants.Reference.ToUpper())
            {
                userde.SecurityProfileId = 0;
                userde.IsSuperUser = false;
                userde.IsSiteAdmin = false; //--V2-401
            }
            //Production User:V2-613
            if (userType == UserTypeConstants.Production.ToUpper())
            {
                userde.SecurityProfileId = _userModels.SecurityProfileId ?? 0;
                userde.IsSuperUser = false;
                userde.IsSiteAdmin = false;
            }
            //
            // This section fills in the Admin, Enterprise Admin, Request and Enterprise Security Profile based on the Package Level and Product Grouping
            //
            #region PackageLevel_Basic 
            if (packageLevel == PackageLevelConstant.Basic)
            {
                userde.IsSuperUser = false;   // Only applicable to Enterprise Level Customer
                #region Admin User 
                if (userType == UserTypeConstants.Admin.ToUpper())
                { 
                    //Administrator for Site that is CMMS Only and Client Package Level is Basic: page21
                    if (productGrouping == 1)
                    { 
                      userde.SecurityProfileId = 103;
                    }
                    //Administrator for Site that is Sanitation Only: page23
                    else if (productGrouping == 2)
                    { 
                      userde.SecurityProfileId = 115;
                    }
                    //Administrator for Site that is APM Only: page119
                    else if (productGrouping == 3)
                    {
                      userde.SecurityProfileId = 119;
                    }
                    //Administrator for Site that is CMMS & Production and Client Package Level is Basic: V2-613 for productGrouping 11
                    else if (productGrouping == 11)
                    {
                        userde.SecurityProfileId = 288;
                    }
                    //Administrator for Site that is CMMS & Sanitation & Production and Client Package Level is Basic: V2-613 for productGrouping 12
                    else if (productGrouping == 12)
                    {
                        userde.SecurityProfileId = 315;
                    }
                    //Administrator for Site that is CMMS & Sanitation & Production and Client Package Level is Basic: V2-613 for productGrouping 13
                    else if (productGrouping == 13)
                    {
                        userde.SecurityProfileId = 351;
                    }
                    //Administrator for Site that is CMMS & Sanitation & APM & Production and Client Package Level is Basic: V2-613 for productGrouping 14
                    else if (productGrouping == 14)
                    {
                        userde.SecurityProfileId = 384;
                    }
                }
        #endregion Admin User
                #region Request User
                if (userType == UserTypeConstants.WorkRequest.ToUpper())
                { 
                    // Work Request Only for Site that is CMMS Only: page27
                    if (productGrouping == 1)
                    {
                        userde.SecurityProfileId = 104;
                    }
                    // Work Request Only for Site that is Sanitation Only: page28
                    else if (productGrouping == 2)
                    {
                        userde.SecurityProfileId = 116;
                    }
                    //Work Request for Site that is CMMS & Production and Client Package Level is Basic: V2-613 for productGrouping 11
                    else if (productGrouping == 11)
                    {
                        userde.SecurityProfileId = 289;
                    }
                    //Work Request for Site that is CMMS & Sanitation & Production and Client Package Level is Basic: V2-613 for productGrouping 12
                    else if (productGrouping == 12)
                    {
                        userde.SecurityProfileId = 316;
                    }
                    //Work Request for Site that is CMMS & APM & Production and Client Package Level is Basic: V2-613 for productGrouping 13
                    else if (productGrouping == 13)
                    {
                        userde.SecurityProfileId = 352;
                    }
                    //Work Request for Site that is CMMS & Sanitation & APM & Production and Client Package Level is Basic: V2-613 for productGrouping 14
                    else if (productGrouping == 14)
                    {
                        userde.SecurityProfileId = 385;
                    }
                }
                #endregion Request User 
            }
            #endregion PackageLevel_Basic 
            #region PackageLevel_Professional
            if (packageLevel == PackageLevelConstant.Professional.ToUpper())
            {
                #region Admin User
                //Professional - Administrator
                if (userType == UserTypeConstants.Admin.ToUpper())
                {
                    // CMMS Only: page22
                    if (productGrouping == 1)
                    {
                        userde.SecurityProfileId = 110;
                    }
                    // CMMS and Sanitation: page24
                    else if (productGrouping == 4)
                    {
                        userde.SecurityProfileId = 125;
                    }
                    // CMMS and APM: page25
                    else if (productGrouping == 5)
                    {
                      userde.SecurityProfileId = 135;
                    }
                    // CMMS, Sanitation, and APM: page26
                    else if (productGrouping == 6)
                    {
                      userde.SecurityProfileId = 144;
                    }
                    // Fleet Only - Product Group 7
                    else if (productGrouping == 7)
                    {
                      userde.SecurityProfileId = 212;
                    }
                    // CMMS and Fleet - Product Group 8
                    else if (productGrouping == 8)
                    {
                      userde.SecurityProfileId = 224;
                    }
                    // Fleet and APM - Product Group 9
                    else if (productGrouping == 9)
                    {
                      userde.SecurityProfileId = 242;
                    }
                    // CMMS, Fleet and APM - Product Group 10
                    else if (productGrouping == 10)
                    {
                      userde.SecurityProfileId = 258;
                    }
                    // CMMS and Production V2-613- Product Group 11
                    else if (productGrouping==11)     
                    {
                        userde.SecurityProfileId = 297;
                    }
                    // CMMS  and Production ,Sanitation V2-613- Product Group 12
                    else if (productGrouping == 12)
                    {
                        userde.SecurityProfileId = 327;
                    }
                    // CMMS  and Production and APM V2-613- Product Group 13
                    else if (productGrouping == 13)
                    {
                        userde.SecurityProfileId = 362;
                    }
                    // CMMS  and Production and APM and Sanitation V2-613- Product Group 14
                    else if (productGrouping == 14)
                    {
                        userde.SecurityProfileId = 398;
                    }
                }
                #endregion Admin User
                #region Request User 
                // Professional - Work Request
                if (userType == UserTypeConstants.WorkRequest.ToUpper())
                { 
                    //CMMS Only - Product Grouping 1
                    if (productGrouping == 1)
                    {
                      userde.SecurityProfileId = 111;
                    }
                    // CMMS and Sanitation - Product Grouping 4
                    else if (productGrouping == 4)
                    {
                      userde.SecurityProfileId = 126;
                    }
                    // CMMS and APM - Product Grouping 5
                    else if (productGrouping == 5)
                    {
                      userde.SecurityProfileId = 136;
                    }
                    // CMMS, Sanitation and APM - Product Grouping 6
                    else if (productGrouping == 6)
                    {
                      userde.SecurityProfileId = 145;
                    }
                    // CMMS  and Production V2-613- Product Group 11
                    else if (productGrouping == 11)
                    {
                        userde.SecurityProfileId = 298;
                    }
                    // CMMS  and Production ,Sanitation V2-613- Product Group 12
                    else if (productGrouping == 12)
                    {
                        userde.SecurityProfileId = 328;
                    }
                    // CMMS  and Production and APM V2-613- Product Group 13
                    else if (productGrouping == 13)
                    {
                        userde.SecurityProfileId = 363;
                    }
                    // CMMS  and Production and APM and Sanitation V2-613- Product Group 14
                    else if (productGrouping == 14)
                    {
                        userde.SecurityProfileId = 399;
                    }
                }
                #endregion Request User 
            }
            #endregion Packagelevel_Professional
            #region PackageLevel_Enterprise
            if (packageLevel == PackageLevelConstant.Enterprise.ToUpper())
            { 
                // Enterprise
                // Four user types that have "mandated" security profiles:
                // Site Admin
                // Enterprise Admin
                // Enterprise User 
                // Request User 
                #region Admins (Site and Enterprise Admins)
                // Admin (Enterprise and Site)
                if (userType == UserTypeConstants.Admin.ToUpper())
                {
                    // CMMS Only 
                    if(productGrouping ==1)
                    {
                        if(_userModels.SecurityProfileId == 156)        // Enterprise Admin
                        {
                          userde.SecurityProfileId = 156;
                          userde.IsSuperUser = true;
                          userde.IsSiteAdmin = false;
                        }
                        else if(_userModels.SecurityProfileId == 157)   // Site Admin
                        {
                          userde.SecurityProfileId = 157;
                          userde.IsSuperUser = false;
                          userde.IsSiteAdmin = true;
                        }
                    }
                    // Sanitation Only - Product Grouping 2
                    else if (productGrouping == 2)
                    {
                        if(_userModels.SecurityProfileId == 163)        // Enterprise Admin
                        {
                          userde.SecurityProfileId = 163;
                          userde.IsSuperUser = true;
                          userde.IsSiteAdmin = false;
                        }
                        else if (_userModels.SecurityProfileId == 164)  // Site Admin
                        {
                          userde.SecurityProfileId = 164;
                          userde.IsSuperUser = false;
                          userde.IsSiteAdmin = true;

                        }
                    }
                    // APM Only - Product Grouping 3
                    else if(productGrouping == 3)
                    { 
                        if (_userModels.SecurityProfileId == 169)
                        {
                            userde.SecurityProfileId = 169;               // Enterprise Admin
                            userde.IsSuperUser = true;
                            userde.IsSiteAdmin = false;
                        }
                        else if (_userModels.SecurityProfileId == 170)
                        {
                            userde.SecurityProfileId = 170;               // Site Admin
                            userde.IsSuperUser = false;
                            userde.IsSiteAdmin = true;
                        }
                    }
                    // CMMS and Sanitation - Product Grouping 4
                    else if(productGrouping == 4)
                    {
                        if (_userModels.SecurityProfileId == 177)
                        {
                          userde.SecurityProfileId = 177;               // Enterprise Admin
                          userde.IsSuperUser = true;
                          userde.IsSiteAdmin = false;
                        }
                        else if (_userModels.SecurityProfileId == 178)
                        {
                          userde.SecurityProfileId = 178;               // Site Admin
                          userde.IsSuperUser = false;
                          userde.IsSiteAdmin = true;
                        }
                    }
                    // CMMS and APM
                    else if(productGrouping == 5)
                    { 
                        if (_userModels.SecurityProfileId == 189)         // Enterprise Admin
                        {
                          userde.SecurityProfileId = 189;
                          userde.IsSuperUser = true;
                          userde.IsSiteAdmin = false;
                        }
                        else if (_userModels.SecurityProfileId == 190)
                        {
                          userde.SecurityProfileId = 190;                // Site Admin
                          userde.IsSuperUser = false;
                          userde.IsSiteAdmin = true;
                        }
                    }
                    // CMMS, Sanitation and APM
                    else if(productGrouping == 6)
                    {
                        if (_userModels.SecurityProfileId == 200)         // Enterprise Admin
                        {
                            userde.SecurityProfileId = 200;
                            userde.IsSuperUser = true;
                            userde.IsSiteAdmin = false;
                        }
                        else if (_userModels.SecurityProfileId == 201)
                        {
                          userde.SecurityProfileId = 201;                 // Site Admin
                          userde.IsSuperUser = false;
                          userde.IsSiteAdmin = true;
                        }
                    }
                    // Fleet Only 
                    else if(productGrouping == 7)
                    {
                        if(_userModels.SecurityProfileId == 216)          // Enterprise Admin
                        {
                            userde.SecurityProfileId = 216;
                            userde.IsSuperUser = false;
                            userde.IsSiteAdmin = true;
                        }
                        else if (_userModels.SecurityProfileId == 217)
                        {
                          userde.SecurityProfileId = 217;                 // Site Admin
                          userde.IsSuperUser = false;
                          userde.IsSiteAdmin = true;
                        }
                    }
                    // CMMS and Fleet 
                    else if(productGrouping == 8)
                    { 
                        if (_userModels.SecurityProfileId == 233)         // Enterprise Admin
                        {
                            userde.SecurityProfileId = 233;
                            userde.IsSuperUser = false;
                            userde.IsSiteAdmin = true;
                        }
                        else if (_userModels.SecurityProfileId == 234)
                        {
                          userde.SecurityProfileId = 234;                 // Site Admin
                          userde.IsSuperUser = false;
                          userde.IsSiteAdmin = true;
                        }
                    }
                    // Fleet and APM
                    else if(productGrouping == 9)
                    { 
                        if (_userModels.SecurityProfileId == 248)         // Enterprise Admin
                        {
                            userde.SecurityProfileId = 248;
                            userde.IsSuperUser = false;
                            userde.IsSiteAdmin = true;
                        }
                        else if (_userModels.SecurityProfileId == 249)
                        {
                          userde.SecurityProfileId = 249;                 // Site Admin
                          userde.IsSuperUser = false;
                          userde.IsSiteAdmin = true;
                        }
                    }
                    // CMMS, Fleet and APM
                    else if(productGrouping == 10)
                    { 
                        if (_userModels.SecurityProfileId == 269)         // Enterprise Admin
                        {
                            userde.SecurityProfileId = 269;
                            userde.IsSuperUser = false;
                            userde.IsSiteAdmin = true;
                        }
                        else if (_userModels.SecurityProfileId == 270)
                        {
                          userde.SecurityProfileId = 270;                 // Site Admin
                          userde.IsSuperUser = false;
                          userde.IsSiteAdmin = true;
                        }
                    }
                    // CMMS Only and Production V2-613- Product Group 11
                    else if (productGrouping == 11)
                    {
                        if (_userModels.SecurityProfileId == 306)         // Enterprise Admin
                        {
                            userde.SecurityProfileId = 306;
                            userde.IsSuperUser = true;
                            userde.IsSiteAdmin = false;
                        }
                        else if (_userModels.SecurityProfileId == 307)
                        {
                            userde.SecurityProfileId = 307;                 // Site Admin
                            userde.IsSuperUser = false;
                            userde.IsSiteAdmin = true;
                        }
                    }
                    // CMMS  and Production ,Sanitation V2-613- Product Group 12
                    else if (productGrouping == 12)
                    {
                        if (_userModels.SecurityProfileId == 339)         // Enterprise Admin
                        {
                            userde.SecurityProfileId = 339;
                            userde.IsSuperUser = true;
                            userde.IsSiteAdmin = false;
                        }
                        else if (_userModels.SecurityProfileId == 340)
                        {
                            userde.SecurityProfileId = 340;                 // Site Admin
                            userde.IsSuperUser = false;
                            userde.IsSiteAdmin = true;
                        }
                    }
                    // CMMS  and Production and APM V2-613- Product Group 13
                    else if (productGrouping == 13)
                    {
                        if (_userModels.SecurityProfileId == 373)         // Enterprise Admin
                        {
                            userde.SecurityProfileId = 373;
                            userde.IsSuperUser = true;
                            userde.IsSiteAdmin = false;
                        }
                        else if (_userModels.SecurityProfileId == 374)
                        {
                            userde.SecurityProfileId = 374;                 // Site Admin
                            userde.IsSuperUser = false;
                            userde.IsSiteAdmin = true;
                        }
                    }
                    // CMMS  and Production and APM and Sanitation V2-613- Product Group 14
                    else if (productGrouping == 14)
                    {
                        if (_userModels.SecurityProfileId == 412)         // Enterprise Admin
                        {
                            userde.SecurityProfileId = 412;
                            userde.IsSuperUser = true;
                            userde.IsSiteAdmin = false;
                        }
                        else if (_userModels.SecurityProfileId == 413)
                        {
                            userde.SecurityProfileId = 413;                 // Site Admin
                            userde.IsSuperUser = false;
                            userde.IsSiteAdmin = true;
                        }
                    }
                }
        // Request User     
        #endregion  Admins
                #region Request User 
                else if (userType == UserTypeConstants.WorkRequest.ToUpper())
                {
                    userde.IsSuperUser = false;
                    userde.IsSiteAdmin = false;
                    // CMMS Only 
                    if (productGrouping == 1)
                    {
                        userde.SecurityProfileId = 158;
                    }
                    // Sanitation Only 
                    else if (productGrouping == 2)
                    {
                        userde.SecurityProfileId = 165;
                    }
                    // CMMS and Sanitation
                    else if(productGrouping == 4)
                    {
                      userde.SecurityProfileId = 179;
                    }
                    // CMMS and APM
                    else if (productGrouping == 5)
                    {
                        userde.SecurityProfileId = 191;
                    }
                    // CMMS, Sanitation and APM
                    else if (productGrouping == 6)
                    {
                        userde.SecurityProfileId = 202;
                    }
                    // CMMS and Fleet
                    else if (productGrouping == 8)
                    {
                        userde.SecurityProfileId = 235;
                    }
                    // CMMS, Fleet and APM
                    else if (productGrouping == 10)
                    {
                        userde.SecurityProfileId = 271;
                    }
                    // CMMS Only and Production V2-613- Product Group 11
                    else if (productGrouping == 11)
                    {
                        userde.SecurityProfileId = 308;
                    }
                    // CMMS  and Production ,Sanitation V2-613- Product Group 12
                    else if (productGrouping == 12)
                    {
                        userde.SecurityProfileId = 341;
                    }
                    // CMMS  and Production and APM V2-613- Product Group 13
                    else if (productGrouping == 13)
                    {
                        userde.SecurityProfileId = 375;
                    }
                    // CMMS  and Production and APM and Sanitation V2-613- Product Group 14
                    else if (productGrouping == 14)
                    {
                        userde.SecurityProfileId = 414;
                    }
                }
        #endregion Request User 
                #region Enterprise User 
                //Enterprise User : V2-401 page26
                else if (userType == UserTypeConstants.Enterprise.ToUpper())
                {
                    // CMMS Only 
                    if (productGrouping == 1)
                    {
                        userde.SecurityProfileId = 159;
                    }
                    // Sanitation Only 
                    else if (productGrouping == 2)
                    {
                        userde.SecurityProfileId = 166;
                    }
                    // APM Only 
                    else if(productGrouping == 3)
                    {
                      userde.SecurityProfileId = 171;
                    }
                    // CMMS and Sanitation
                    else if(productGrouping == 4)
                    {
                      userde.SecurityProfileId = 183;
                    }
                    // CMMS and APM
                    else if (productGrouping == 5)
                    {
                        userde.SecurityProfileId = 194;
                    }
                    // CMMS, Sanitation and APM
                    else if (productGrouping == 6)
                    {
                        userde.SecurityProfileId = 208;
                    }
                    // Fleet Only 
                    else if (productGrouping == 7)
                    {
                        userde.SecurityProfileId = 218;
                    }
                    // CMMS and Fleet
                    else if (productGrouping == 8)
                    {
                        userde.SecurityProfileId = 238;
                    }
                    // Fleet and APM
                    else if (productGrouping == 9)
                    {
                        userde.SecurityProfileId = 250;
                    }
                    // CMMS, Fleet and APM
                    else if (productGrouping == 10)
                    {
                        userde.SecurityProfileId = 274;
                    }
                    // CMMS Only and Production V2-613- Product Group 11
                    else if (productGrouping == 11)
                    {
                        userde.SecurityProfileId = 311;
                    }
                    // CMMS  and Production ,Sanitation V2-613- Product Group 12
                    else if (productGrouping == 12)
                    {
                        userde.SecurityProfileId = 344;
                    }
                    // CMMS  and Production and APM V2-613- Product Group 13
                    else if (productGrouping == 13)
                    {
                        userde.SecurityProfileId = 378;
                    }
                    // CMMS  and Production and APM and Sanitation V2-613- Product Group 14
                    else if (productGrouping == 14)
                    {
                        userde.SecurityProfileId = 417;
                    }
                }
                #endregion Enterprise User 
            }
            #endregion PackageLevel_Enterprise
            userde.CMMSUser = _userModels.CMMSUser;
            userde.SanitationUser = _userModels.SanitationUser;
            userde.Password = "";
            userde.UserName = _userModels.UserName;
            userde.Personnel.ClientLookupId = _userModels.UserName;//18-04
            userde.SecurityQuestion = _userModels.SecurityQuestion;
            userde.SecurityResponse = _userModels.SecurityResponse;
            userde.IsActive = true;
            if (userde.IsActive)
                userde.Personnel.InactiveFlag = false;
            else
                userde.Personnel.InactiveFlag = true;
            userde.FirstName = _userModels.FirstName ?? string.Empty;
            userde.Personnel.NameFirst = _userModels.FirstName ?? string.Empty;
            userde.MiddleName = _userModels.MiddleName ?? string.Empty;
            userde.Personnel.NameMiddle = _userModels.MiddleName ?? string.Empty;
            userde.LastName = _userModels.LastName ?? string.Empty;
            userde.Personnel.NameLast = _userModels.LastName ?? string.Empty;
            if (userType != UserTypeConstants.Reference.ToUpper())
            {
                userde.Email = _userModels.Email ?? string.Empty;
            }
            userde.Localization = userData.DatabaseKey.User.Localization;
            userde.UIConfiguration = userData.DatabaseKey.User.UIConfiguration;
            if (packageLevel == UserTypeConstants.Enterprise.ToUpper()) //--V2-401
            {
                userde.DefaultSiteId = _userModels.SiteId ?? 0;
                userde.Personnel.SiteId = _userModels.SiteId ?? 0;
                userde.TimeZone = _userModels.TimeZone;
            }
            else
            {
                userde.DefaultSiteId = userData.DatabaseKey.User.DefaultSiteId;
                userde.Personnel.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                userde.TimeZone = userData.DatabaseKey.User.TimeZone;
            }
            userde.ResultsPerPage = 10;

            userde.UserType = _userModels.UserType;
            userde.IsPasswordTemporary = _userModels.UserType != UserTypeConstants.Reference ? true : false;//V2-332,V2-401
            //userde.SecurityProfileId = _userModels.SecurityProfileId ?? 0;
            userde.Personnel.CraftId = _userModels.CraftId ?? 0;
            userde.Personnel.Shift = _userModels.Shift ?? string.Empty;
            userde.Personnel.Address1 = _userModels.Address1 ?? string.Empty;
            userde.Personnel.Address2 = _userModels.Address2 ?? string.Empty;
            userde.Personnel.Address3 = _userModels.Address3 ?? string.Empty;
            userde.Personnel.AddressCity = _userModels.AddressCity ?? string.Empty;
            userde.Personnel.AddressState = _userModels.AddressState ?? string.Empty;
            userde.Personnel.AddressCountry = _userModels.AddressCountry ?? string.Empty;
            userde.Personnel.AddressPostCode = _userModels.AddressPostCode ?? string.Empty;
            userde.Personnel.Phone1 = _userModels.Phone1 ?? string.Empty;
            userde.Personnel.Phone2 = _userModels.Phone2 ?? string.Empty;
            userde.Personnel.Email = _userModels.Email ?? string.Empty;
            if (userType != UserTypeConstants.WorkRequest.ToUpper() && userType != UserTypeConstants.Reference.ToUpper())
            {
                userde.Personnel.Buyer = _userModels.Buyer;
            }
        }

        private string GetTimeZone(long? siteId)   //--for V2-401
        {
            Site site = new Site();
            site.ClientId = userData.DatabaseKey.Client.ClientId;// userData.DatabaseKey.Personnel.ClientId;
            site.SiteId = siteId ?? 0;
            site.Retrieve(_dbKey);
            return (site.TimeZone);
        }
        public UserModel AddNewUser(UserModel _userModel)
        {
            string msg = String.Empty;
            List<string> ErroMsgList = new List<string>();
            UserDetails userdetails = new UserDetails()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteControlled = userData.DatabaseKey.Client.SiteControl,
            };
            if (_userModel.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) //--V2-401
            {
                _userModel.TimeZone = GetTimeZone(_userModel.SiteId);
            }
            GetModelData(userdetails, _userModel);
            if(_userModel.UserType== UserTypeConstants.Production)  //V2 613
            {
                userdetails.ValidateNewProductionUserAdd_V2(this.userData.DatabaseKey);
            }
            else
            {
                userdetails.ValidateNewUserAdd_V2(this.userData.DatabaseKey);
            }           
            ErroMsgList.AddRange(userdetails.ErrorMessages);
            if (ErroMsgList.Count == 0)
            {
                if (userdetails.SecurityProfileId > 0)
                {
                    //----------------Create UserInfo & LoginInfo record-----------------
                    userdetails.CreateNewUserWithLoginData_V2(userData.DatabaseKey);  /*create User*/
                    if (userdetails.UserInfoId > 0)
                    {
                        //----------------Create Personnel record-----------------
                        userdetails.Personnel.UserInfoId = userdetails.UserInfoId;
                        userdetails.Personnel.Create(userData.DatabaseKey);   /*create Personnel*/

                        //--------------Create Permission record------------
                        UserPermission userpermission = new UserPermission();
                        userpermission.UserInfoId = userdetails.UserInfoId;
                        if (_userModel.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) //--V2-401
                        {
                            userpermission.SiteId = _userModel.SiteId ?? 0;
                        }
                        else
                        {
                            userpermission.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                        }
                        userpermission.PermissionType = "G";
                        userpermission.UpdateIndex = 0;
                        userpermission.Create(this.userData.DatabaseKey);   /*create User Permission*/

                        //------Update Site User Counts---------

                        userdetails.RetrieveSiteUserCounts_V2(userData.DatabaseKey);

                        Site site = new Site();
                        site.ClientId = userData.DatabaseKey.Client.ClientId;

                        if (_userModel.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) //--V2-401
                        {
                            site.SiteId = _userModel.SiteId ?? 0;
                        }
                        else
                        {
                            site.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                        }
                        site.Retrieve(userData.DatabaseKey);
                        // RKL - 2025-Jan-10 V2-1028
                        // We are updating regardless of the security profile type 
                        //if (userdetails.CMMSUser || userdetails.SanitationUser)
                        //{
                            site.AppUsers = Convert.ToInt32(userdetails.CountWebAppUser);
                            site.LimitedUsers = Convert.ToInt32(userdetails.CountLimitedUser);
                            site.WorkRequestUsers = Convert.ToInt32(userdetails.CountWorkRequestUser);
                            site.SuperUsers = Convert.ToInt32(userdetails.CountSuperUser);
                            site.ProdAppUsers = Convert.ToInt32(userdetails.CountProdUser);
                        //}
                        //if (userdetails.SanitationUser)
                        //{
                        //    site.SanitationUsers = Convert.ToInt32(userdetails.CountSanitationUser);
                        //}
                        site.Update(this.userData.DatabaseKey);

                        //-------------Add User Eventlog --------
                        if (_userModel.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) //--V2-401
                        {
                            var siteId = _userModel.SiteId ?? 0;
                            CreateEventLog(userdetails.UserInfoId, siteId, EventStatusConstants.Create);
                        }
                        else
                        {
                            var siteId = userData.DatabaseKey.User.DefaultSiteId;
                            CreateEventLog(userdetails.UserInfoId, siteId, EventStatusConstants.Create);
                        }

                        //-------------Create Temporary Password for New User---------
                        DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
                        AccountPassword password = new AccountPassword();
                        IP_AccountPasswordData ipData = new IP_AccountPasswordData()
                        {
                            DbKey = dbKey,
                            UserName = userdetails.UserName,//userinfo.UserName
                        };
                        if (_userModel.UserType != UserTypeConstants.Reference)
                        {
                            string resetUrl = GetHostedUrl();
                            string headerBgURL = resetUrl + SomaxAppConstants.HeaderMailTemplate;
                            string somaxLogoURL = resetUrl + SomaxAppConstants.SomaxLogoForMailTemplate;

                            string spnloginurl = resetUrl;
                            string footerURL = resetUrl + SomaxAppConstants.FooterMailTemplate;

                            ProcessAlert objAlert = new ProcessAlert(this.userData);
                            password.RequestNewPassword(ipData);

                            //-------------Temporary Password Eventlog------------

                            if (_userModel.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise) //--V2-401
                            {
                                var siteId = _userModel.SiteId ?? 0;
                                CreateEventLog(userdetails.UserInfoId, siteId, EventStatusConstants.TemporaryPassword);
                            }
                            else
                            {
                                var siteId = userData.DatabaseKey.User.DefaultSiteId;
                                CreateEventLog(userdetails.UserInfoId, siteId, EventStatusConstants.TemporaryPassword);
                            }

                            //-------------Notify User of Temporary Password by email---------------------
                            string Password = password.TempPassword;
                            var adminMail = userData.DatabaseKey.User.Email;
                            string targetMail = String.IsNullOrEmpty(_userModel.Email) ? adminMail : _userModel.Email;
                            objAlert.CreateAlert<DataContracts.UserDetails>(AlertTypeEnum.NewUserAdded, userdetails.UserInfoId, userdetails.UserName, targetMail, headerBgURL, somaxLogoURL, spnloginurl, footerURL, Password);
                        }
                    }
                }
                else
                {
                    userdetails.ErrorMessages.Add("User has  Not Added Successfully  because selected user access data is not valid");
                    ErroMsgList.AddRange(userdetails.ErrorMessages);
                }
            }
            _userModel.UserInfoId = userdetails.UserInfoId;
            _userModel.ErrorMessages = ErroMsgList;
            return _userModel;
        }


        public UserChangeAccessModel ChangeUserAccess(UserChangeAccessModel _userChangeAccessModel)
        {
            string msg = String.Empty;


            List<string> ErroMsgList = new List<string>();

            UserDetails userdetails = new UserDetails()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteControlled = userData.DatabaseKey.Client.SiteControl,
                DefaultSiteId = _userChangeAccessModel.DefaultSiteId ?? 0,            // Default Site of the target user
                SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0,    // Target Profile                  
                UserInfoId = _userChangeAccessModel.UserInfoId                        // 2022-11-22 - Send to validation 
            };
            userdetails.ErrorMessages = new List<string>();
            if (_userChangeAccessModel.UserType == UserTypeConstants.Production)    //V2 613
            {
                userdetails.ValidateProductionUserAccess_V2(this.userData.DatabaseKey);
            }
            else if((_userChangeAccessModel.OldUserType == UserTypeConstants.WorkRequest || _userChangeAccessModel.OldUserType == UserTypeConstants.Enterprise) &&(_userChangeAccessModel.UserType == UserTypeConstants.Full || _userChangeAccessModel.UserType == UserTypeConstants.Admin))  //V2-659
            {
                userdetails.ValidateUserAccess_V2(this.userData.DatabaseKey);
            } 
            if(userdetails.ErrorMessages !=null && userdetails.ErrorMessages.Count > 0) 
            { 
            ErroMsgList.AddRange(userdetails.ErrorMessages);
            }
            if (ErroMsgList.Count == 0)
            {
                if (_userChangeAccessModel.UserInfoId > 0)
                {
                    //----------------Update user Info Record----------------

                    UserDetails userdet = new UserDetails()
                    {
                        ClientId = userData.DatabaseKey.Client.ClientId,
                        DefaultSiteId = _userChangeAccessModel.DefaultSiteId ?? 0,
                        UserInfoId = _userChangeAccessModel.UserInfoId,
                        UserType = _userChangeAccessModel.UserType,
                        UserUpdateIndex = _userChangeAccessModel.UserUpdateIndex
                    };

                    string userType = string.Empty;
                    int productGrouping = _userChangeAccessModel.ProductGrouping;
                    string packageLevel = string.Empty;

                    if (!string.IsNullOrEmpty(_userChangeAccessModel.UserType))
                    {
                        userType = _userChangeAccessModel.UserType.ToUpper();
                    }
                    if (!string.IsNullOrEmpty(_userChangeAccessModel.PackageLevel))
                    {
                        packageLevel = _userChangeAccessModel.PackageLevel.ToUpper();
                    }
                    long SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                    if (userType == UserTypeConstants.Full.ToUpper())
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;

                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 1 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 103;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 1 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 110;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 2 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 115;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }

                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 3 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 119;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 4 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 125;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 5 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 135;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 6 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 144;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 1 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 104;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 1 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 111;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 2 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 116;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 3 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 120;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 4 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 126;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 5 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 136;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 6 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 145;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 1 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 156)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 2 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 163)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 3 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 169)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 4 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 177)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 5 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 189)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 6 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 200)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 1 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 157)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = true;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 2 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 164)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = true;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 3 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 170)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = true;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 4 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 178)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = true;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 5 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 190)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = true;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 6 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 201)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = true;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 1 && packageLevel == PackageLevelConstant.Enterprise.ToUpper())
                    {
                        userdet.SecurityProfileId = 158;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 2 && packageLevel == PackageLevelConstant.Enterprise.ToUpper())
                    {
                        userdet.SecurityProfileId = 165;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 4 && packageLevel == PackageLevelConstant.Enterprise.ToUpper())
                    {
                        userdet.SecurityProfileId = 179;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 5 && packageLevel == PackageLevelConstant.Enterprise.ToUpper())
                    {
                        userdet.SecurityProfileId = 191;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 6 && packageLevel == PackageLevelConstant.Enterprise.ToUpper())
                    {
                        userdet.SecurityProfileId = 202;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    #region V2-613   Security Profile Id  
                    //Basic :Admin 
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 11 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 288;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 12 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 315;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 13 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 351;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 14 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 384;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    // Professional: Admin
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 11 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 297;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 12 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 327;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 13 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 362;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 14 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 398;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    //Enterprise : site Admin User 
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 11 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 307)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = true;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 12 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 340)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = true;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 13 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 374)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = true;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 14 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 413)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = true;
                    }
                    //Enterprise: Enterprise Admin User 
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 11 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 306)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 12 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 339)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 13 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 373)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Admin.ToUpper() && productGrouping == 14 && packageLevel == PackageLevelConstant.Enterprise.ToUpper() && SecurityProfileId == 412)
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = true;
                        userdet.IsSiteAdmin = false;
                    }
                    //work request user :Basic
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 11 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 289;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 12 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 316;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 13 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 352;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 14 && packageLevel == PackageLevelConstant.Basic.ToUpper())
                    {
                        userdet.SecurityProfileId = 385;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    //work request user :Professional
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 11 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 298;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 12 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 328;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 13 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 363;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 14 && packageLevel == PackageLevelConstant.Professional.ToUpper())
                    {
                        userdet.SecurityProfileId = 399;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    //work request user :Enterprise
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 11 && packageLevel == PackageLevelConstant.Enterprise.ToUpper())
                    {
                        userdet.SecurityProfileId = 308;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 12 && packageLevel == PackageLevelConstant.Enterprise.ToUpper())
                    {
                        userdet.SecurityProfileId = 341;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 13 && packageLevel == PackageLevelConstant.Enterprise.ToUpper())
                    {
                        userdet.SecurityProfileId = 375;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.WorkRequest.ToUpper() && productGrouping == 14 && packageLevel == PackageLevelConstant.Enterprise.ToUpper())
                    {
                        userdet.SecurityProfileId = 414;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    #endregion


                    else if (userType == UserTypeConstants.Enterprise.ToUpper())
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    else if (userType == UserTypeConstants.Production.ToUpper())
                    {
                        userdet.SecurityProfileId = _userChangeAccessModel.SecurityProfileId ?? 0;
                        userdet.IsSuperUser = false;
                        userdet.IsSiteAdmin = false;
                    }
                    if (userdet.SecurityProfileId > 0)
                    {
                        //// Updates the UserInfo ,site and client tables
                        userdet.UpdateByUserInfoIdWithUserAccessV2(userData.DatabaseKey);
                        var siteId = _userChangeAccessModel.DefaultSiteId ?? 0; //userData.DatabaseKey.User.DefaultSiteId;
                        CreateEventLog(_userChangeAccessModel.UserInfoId, siteId, EventStatusConstants.UpdateAccess);

                    }
                    else
                    {
                       
                        userdetails.ErrorMessages.Add("User Access Not Changed Successfully because selected user access data is not valid");
                        ErroMsgList.AddRange(userdetails.ErrorMessages);
                    }

                }
            }
            _userChangeAccessModel.ErrorMessages = ErroMsgList;
            return _userChangeAccessModel;
        }


        ////------IMP: NEVER DELETE THIS COMMENTED CODE----////


        //public UserModel EditUser(UserModel _userModel, long ObjectId)
        //{
        //    string msg = String.Empty;
        //    //bool valid = true;  ????
        //    List<string> ErroMsgList = new List<string>();
        //    UserDetails userdet = new UserDetails()
        //    {
        //        ClientId = userData.DatabaseKey.Client.ClientId,
        //        UserInfoId = ObjectId,
        //        SiteControlled = userData.DatabaseKey.Client.SiteControl,
        //        ClientUpdateIndex = userData.DatabaseKey.Client.UpdateIndex
        //    };
        //    userdet.RetrieveUserDetailsByUserInfoID(userData.DatabaseKey);
        //    userdet.RetrievePersonnelByUserInfoId(userData.DatabaseKey, string.Empty);
        //    RetrieveUserDetailsControls(userdet, _userModel, ObjectId);
        //    //UserDetails objUserDet = new UserDetails()    /*not required V2-332*/
        //    //{
        //    //    UserInfoId = ObjectId,
        //    //    ClientId = userData.DatabaseKey.Client.ClientId
        //    //};
        //    //objUserDet.RetrieveUserDetailsByUserInfoID(userData.DatabaseKey);             
        //    //objUserDet.RetrievePersonnelByUserInfoId(userData.DatabaseKey, string.Empty);
        //    //ValidateUserType(userdet);

        //    if (userdet.IsActive)
        //    {
        //        userdet.ValidateUserUpdate(userData.DatabaseKey);
        //        ErroMsgList.AddRange(userdet.ErrorMessages);
        //    }
        //    else
        //    {
        //        //what will happen in Isactive=false, no else
        //        //as per V1: "Will NOT do this if the new user is inactive because sp does not take that into account"(UserInfoEdit.aspx.cs: Line:282)
        //        //but I have checked the sp usp_UserData_ValidateUserUpdate which does not have IsActive check at all, rather it checks IF(@SiteControled=0)
        //    }
        //    // if (valid == true)    ????
        //    // {
        //    //if (objUserDet.SiteControlled == true)
        //    if (userdet.SiteControlled == true)
        //    {
        //        Site site = new Site()
        //        {
        //            ClientId = userData.DatabaseKey.Client.ClientId,
        //            SiteId = userdet.DefaultSiteId,
        //            IsSuperUser = userdet.IsSuperUser,
        //            UserType = userdet.UserType,
        //            UserIsActive = userdet.IsActive,
        //            CurrentIsSuperUser = userdet.IsSuperUser,// objUserDet.IsSuperUser,
        //            CurrentUserType = userdet.UserType,// objUserDet.UserType,
        //            CurrentUserIsActive = userdet.IsActive //objUserDet.IsActive
        //        };

        //        site.ValidateUserUpdate(userData.DatabaseKey); /*the sp usp_Site_ValidateUserUpdate validates only users with IsActive true*/
        //        ErroMsgList.AddRange(site.ErrorMessages);
        //    }
        //    if (ErroMsgList.Count == 0)
        //    {
        //        // userdet.UpdateUserByUserInfoIdWithLoginData(userData.DatabaseKey);  // Updates the UserInfo and LoginInfo tables
        //        userdet.UpdateUserByUserInfoIdWithLoginDataV2(userData.DatabaseKey);
        //        //// Updates the Personnel table
        //        userdet.Personnel.Update(userData.DatabaseKey); /*sp_Personnel_UpdateFromUserInfo*/

        //        //Personnel pe = new Personnel()
        //        //{
        //        //    ClientId = userData.DatabaseKey.Client.ClientId,
        //        //    UserInfoId = userdet.UserInfoId,
        //        //    NameFirst = userdet.FirstName,
        //        //    NameLast = userdet.LastName,
        //        //    NameMiddle = userdet.MiddleName,
        //        //    Email = userdet.Email,
        //        //    InactiveFlag = !userdet.IsActive
        //        //};
        //        //pe.UpdateFromUserInfo(userData.DatabaseKey); /*usp_Personnel_UpdateByPK: not required since the previous covers all parameters for this also*/

        //        ////----not required : userdet already defined-------
        //        //userdet = new UserDetails()
        //        //{
        //        //    ClientId = userData.DatabaseKey.Client.ClientId,
        //        //    DefaultSiteId = userData.DatabaseKey.User.DefaultSiteId
        //        //};

        //        ////-------This part is not required for V2-332-------////
        //        // Update the site number of users    
        //        //userdet.RetrieveSiteUserCounts(userData.DatabaseKey);
        //        //DataContracts.Site site = new DataContracts.Site()
        //        //{
        //        //    ClientId = userData.DatabaseKey.Client.ClientId,
        //        //    SiteId = userData.DatabaseKey.User.DefaultSiteId
        //        //};
        //        //site.Retrieve(userData.DatabaseKey);
        //        //site.AppUsers = Convert.ToInt32(userdet.CountWebAppUser);
        //        //site.LimitedUsers = Convert.ToInt32(userdet.CountLimitedUser);
        //        //site.WorkRequestUsers = Convert.ToInt32(userdet.CountWorkRequestUser);
        //        //site.SanitationUsers = Convert.ToInt32(userdet.CountSanitationUser);
        //        //site.SuperUsers = Convert.ToInt32(userdet.CountSuperUser);
        //        //site.Update(this.userData.DatabaseKey);
        //    }
        //    else
        //    {
        //        _userModel.UserInfoId = userdet.UserInfoId;   // * Anadi
        //        _userModel.ErrorMessages = ErroMsgList;
        //        return _userModel;
        //    }
        //    //----not required - V2-332-----
        //    //userdet.UserInfoId = ObjectId;
        //    //userdet.ClientId = userData.DatabaseKey.Client.ClientId;
        //    //userdet.RetrieveUserDetailsByUserInfoID(userData.DatabaseKey);
        //    //userdet.RetrievePersonnelByUserInfoId(userData.DatabaseKey, string.Empty);
        //    //-----------------------

        //    //} //
        //    _userModel.UserInfoId = userdet.UserInfoId;
        //    _userModel.ErrorMessages = ErroMsgList;
        //    return _userModel;
        //}
        //---------------------------------------------------------

        public UserModel EditUser(UserModel _userModel, long ObjectId)
        {
            List<string> ErroMsgList = new List<string>();
            UserDetails userdet = new UserDetails()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                UserInfoId = ObjectId,
                SiteControlled = userData.DatabaseKey.Client.SiteControl,
                ClientUpdateIndex = userData.DatabaseKey.Client.UpdateIndex
            };
            userdet.RetrieveUserDetailsByUserInfoID(userData.DatabaseKey);
            userdet.RetrievePersonnelByUserInfoId(userData.DatabaseKey, string.Empty);
            RetrieveUserDetailsControls(userdet, _userModel, ObjectId);

            
            //// Updates the UserInfo and LoginInfo tables
            userdet.UpdateUserByUserInfoIdWithLoginDataV2(userData.DatabaseKey);

            _userModel.UserInfoId = userdet.UserInfoId;
            if (userdet.ErrorMessages != null && userdet.ErrorMessages.Count > 0)
            {
                ErroMsgList.AddRange(userdet.ErrorMessages);
                // _userModel.UserInfoId = userdet.UserInfoId;
                _userModel.ErrorMessages = ErroMsgList;
                return _userModel;
            }
            #region V2-803 For check exist of GmailId and MicrosoftId
            LoginSSO loginssodet = new LoginSSO()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                UserInfoId = ObjectId
            };
            loginssodet.RetrieveLoginSSOByUserInfoId(userData.DatabaseKey);
            loginssodet = new LoginSSO()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                UserInfoId = loginssodet.UserInfoId,
                GMailId = _userModel.GMailId,
                LoginSSOId = _userModel.LoginSSOId,
                MicrosoftMailId = _userModel.MicrosoftMailId,
                WindowsADUserId = _userModel.WindowsADUserId,
            };
            loginssodet.CheckvalidateGmailId(userData.DatabaseKey);
            if (loginssodet.ErrorMessages != null && loginssodet.ErrorMessages.Count > 0)
            {
                ErroMsgList.AddRange(loginssodet.ErrorMessages);
                _userModel.ErrorMessages = ErroMsgList;
                return _userModel;
            }
            #endregion V2-803
            else
            {
                //// Updates the Personnel table
                userdet.Personnel.Update(userData.DatabaseKey);
                if (userdet.ErrorMessages != null && userdet.ErrorMessages.Count > 0)
                {
                    ErroMsgList.AddRange(userdet.ErrorMessages);
                    // _userModel.UserInfoId = userdet.UserInfoId;
                    _userModel.ErrorMessages = ErroMsgList;
                    return _userModel;
                }
                else
                {
                    Personnel pe = new Personnel()
                    {
                        ClientId = userData.DatabaseKey.Client.ClientId,
                        UserInfoId = userdet.UserInfoId,
                        NameFirst = userdet.FirstName,
                        NameLast = userdet.LastName,
                        NameMiddle = userdet.MiddleName,
                        Email = userdet.Email,
                        InactiveFlag = !userdet.IsActive
                    };
                    pe.UpdateFromUserInfo(userData.DatabaseKey);
                    if (pe.ErrorMessages != null && pe.ErrorMessages.Count > 0)
                    {
                        ErroMsgList.AddRange(pe.ErrorMessages);
                        // _userModel.UserInfoId = pe.UserInfoId;
                        _userModel.ErrorMessages = ErroMsgList;
                        return _userModel;
                    }
                     
                }
                #region V2-803 Add/Update LoginSSO

                if (loginssodet.UserInfoId != 0)
                {
                    loginssodet.Update(userData.DatabaseKey);
                }
                else
                {
                    loginssodet.ClientId=userData.DatabaseKey.Client.ClientId;
                    loginssodet.UserInfoId = ObjectId;
                    loginssodet.GMailId = _userModel.GMailId;
                    loginssodet.MicrosoftMailId=_userModel.MicrosoftMailId;
                    loginssodet.WindowsADUserId = _userModel.WindowsADUserId;
                    loginssodet.Create(userData.DatabaseKey);
                }

                #endregion V2-803
            }
            // _userModel.UserInfoId = userdet.UserInfoId;
            _userModel.ErrorMessages = ErroMsgList;
            return _userModel;
        }

        public bool ResetPasswordForExistingUser(ResetPasswordModel resetPasswordModel)
        {
            var result = true;
            try
            {
                //-------------Create Temporary Password for Existing User---------
                DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
                AccountPassword password = new AccountPassword();
                IP_AccountPasswordData ipData = new IP_AccountPasswordData()
                {
                    DbKey = dbKey,
                    UserName = resetPasswordModel.UserName,
                };

                password.RequestNewPassword(ipData);
                //-------------Temporary Password Eventlog------------

                var siteid = userData.DatabaseKey.User.DefaultSiteId;
                CreateEventLog(resetPasswordModel.UserInfoId, siteid, EventStatusConstants.TemporaryPassword);

                //-------------Notify User of Temporary Password by email---------------------
                resetPasswordModel.Password = password.TempPassword;

                string resetUrl = GetHostedUrl();
                string headerBgURL = resetUrl + SomaxAppConstants.HeaderMailTemplate;
                string somaxLogoURL = resetUrl + SomaxAppConstants.SomaxLogoForMailTemplate;

                string spnloginurl = resetUrl;
                string footerURL = resetUrl + SomaxAppConstants.FooterMailTemplate;

                ProcessAlert objAlert = new ProcessAlert(this.userData);


                //-------------Notify User of Temporary Password by email---------------------
                var adminMail = userData.DatabaseKey.User.Email;
                string targetMail = String.IsNullOrEmpty(resetPasswordModel.EmailAddress) ? adminMail : resetPasswordModel.EmailAddress;
                objAlert.CreateAlert<DataContracts.UserDetails>(AlertTypeEnum.ResetPassword, resetPasswordModel.UserInfoId, resetPasswordModel.UserName, targetMail, headerBgURL, somaxLogoURL, spnloginurl, footerURL, resetPasswordModel.Password, resetPasswordModel.FirstName, resetPasswordModel.LastName);

            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        //protected bool SendResetPasswordCredentialByMailToUser(ResetPasswordModel resetPasswordModel)
        //{
        //    bool result;
        //    string resetUrl = GetHostedUrl();
        //    StringBuilder body = new StringBuilder();
        //    string Emailbody = string.Empty;
        //    var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Views\Login\_ResetPasswordMailTemplate.cshtml");
        //    var templateContent = string.Empty;
        //    using (var reader = new StreamReader(templateFilePath))
        //    {
        //        templateContent = reader.ReadToEnd();
        //    }
        //    string emailHtmlBody = ParseTemplate(templateContent);

        //    string output = emailHtmlBody.
        //                    Replace("headerBgURL", resetUrl + SomaxAppConstants.HeaderMailTemplate).
        //                    Replace("somaxLogoURL", resetUrl + SomaxAppConstants.SomaxLogoForMailTemplate).
        //                    Replace("spnfirstame", resetPasswordModel.FirstName).
        //                    Replace("spnlastname", resetPasswordModel.LastName).
        //                    Replace("spUserID", resetPasswordModel.UserName).
        //                    Replace("spnPassword", resetPasswordModel.Password).
        //                    Replace("spnloginurl", resetUrl).
        //                    Replace("footerBgURL", resetUrl + SomaxAppConstants.FooterMailTemplate);
        //    body.Append(output);
        //    //Email email = new Email() { Body = body.ToString(), Subject = "Login Credentials" };
        //    Email email = new Email() { Body = body.ToString(), Subject = SomaxAppConstants.ResetPasswordSubject };
        //    var adminMail = userData.DatabaseKey.User.Email;
        //    string targetMail = String.IsNullOrEmpty(resetPasswordModel.EmailAddress) ? adminMail : resetPasswordModel.EmailAddress;
        //    email.Recipients.Add(targetMail);
        //    try
        //    {
        //        email.Send();
        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //    }
        //    return result;
        //}
        private void CreateEventLog(long objId, long siteId, string eventVal = "")
        {

            UserEventLog log = new UserEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = siteId;
            log.ObjectId = objId;
            log.Event = eventVal;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = string.Empty;
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }

        #endregion

        #region V2-402
        //public List<UserManagementModel> PopulateuserManagmentForEnterprise(int Start = 0, int length = 0, int CaseNo = 0, string UserName = "", string LastName = "", string FirstName = "",
        //                            string Email = "", long CraftId = 0, string SearchText = "", string OrderByColumn = "0", string OrderBy = "asc", string SelectedSites = "", string SecurityProfileIds = "")
        //{
        //    UserSearch user = new UserSearch()
        //    {
        //        ClientId = userData.DatabaseKey.Client.ClientId,
        //        DefaultSiteId = userData.DatabaseKey.User.DefaultSiteId,
        //        PackageLevel = userData.DatabaseKey.Client.PackageLevel,
        //        IsSuperUser = userData.DatabaseKey.User.IsSuperUser,
        //        CaseNo = CaseNo,
        //        UserName = UserName,
        //        LastName = LastName,
        //        FirstName = FirstName,
        //        Email = Email,
        //        PersonnelCraftId = CraftId,
        //        SearchText = SearchText,
        //        Offset = Start,
        //        Nextrow = length,
        //        OrderBy = OrderBy,
        //        OrderByColumn = OrderByColumn,
        //        SelectedSites = SelectedSites,
        //        SecurityProfileIds= SecurityProfileIds
        //    };
        //    List<UserManagementModel> UserManagementModelList = new List<UserManagementModel>();
        //    List<UserSearch> UserSearchlList = user.RetrieveUserSearchListForEnterprise(userData.DatabaseKey);
        //    UserManagementModel objUserManagementModel = new UserManagementModel();

        //    foreach (var data in UserSearchlList)
        //    {
        //        objUserManagementModel = new UserManagementModel();
        //        objUserManagementModel.UserName = data.UserName;
        //        objUserManagementModel.LastName = data.LastName;
        //        objUserManagementModel.FirstName = data.FirstName;
        //        objUserManagementModel.SecurityProfileDescription = data.SecurityProfileDescription;
        //        objUserManagementModel.Email = data.Email;
        //        objUserManagementModel.CraftDescription = data.CraftDescription;
        //        objUserManagementModel.TotalCount = data.TotalCount;
        //        objUserManagementModel.SiteCount = data.SiteCount;
        //        objUserManagementModel.UserInfoId = data.UserInfoId;
        //        UserManagementModelList.Add(objUserManagementModel);
        //    }
        //    return UserManagementModelList;
        //}
        //public List<UserManagementModel> PopulateuserManagmentForBasicProfessional(int Start = 0, int length = 0, int CaseNo = 0, string UserName = "", string LastName = "", string FirstName = "",
        //                            string Email = "", long CraftId = 0, string SearchText = "", string OrderByColumn = "0", string OrderBy = "asc", string SelectedSites = "", string SecurityProfileIds = "")
        //{
        //    UserSearch user = new UserSearch()
        //    {
        //        ClientId = userData.DatabaseKey.Client.ClientId,
        //        DefaultSiteId = userData.DatabaseKey.User.DefaultSiteId,
        //        PackageLevel = userData.DatabaseKey.Client.PackageLevel,
        //        IsSuperUser = userData.DatabaseKey.User.IsSuperUser,
        //        CaseNo = CaseNo,
        //        UserName = UserName,
        //        LastName = LastName,
        //        FirstName = FirstName,
        //        Email = Email,
        //        PersonnelCraftId = CraftId,
        //        SearchText = SearchText,
        //        Offset = Start,
        //        Nextrow = length,
        //        OrderBy = OrderBy,
        //        OrderByColumn = OrderByColumn,
        //        SelectedSites = SelectedSites,
        //        SecurityProfileIds= SecurityProfileIds
        //    };
        //    List<UserManagementModel> UserManagementModelList = new List<UserManagementModel>();
        //    List<UserSearch> UserSearchlList = user.RetrieveUserSearchListForBasicProfessional(userData.DatabaseKey);
        //    UserManagementModel objUserManagementModel = new UserManagementModel();

        //    foreach (var data in UserSearchlList)
        //    {
        //        objUserManagementModel = new UserManagementModel();
        //        objUserManagementModel.UserName = data.UserName;
        //        objUserManagementModel.LastName = data.LastName;
        //        objUserManagementModel.FirstName = data.FirstName;
        //        objUserManagementModel.SecurityProfileDescription = data.SecurityProfileDescription;
        //        objUserManagementModel.Email = data.Email;
        //        objUserManagementModel.CraftDescription = data.CraftDescription;
        //        objUserManagementModel.TotalCount = data.TotalCount;
        //        objUserManagementModel.UserInfoId = data.UserInfoId;
        //        UserManagementModelList.Add(objUserManagementModel);
        //    }
        //    return UserManagementModelList;
        //}
        public List<InnerGridCraft> PopulateuserManagmentCraftDetails(long UserInfoId)
        {
            UserSearch user = new UserSearch()
            {
                UserInfoId = UserInfoId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                DefaultSiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<InnerGridCraft> InnerGridCraftList = new List<InnerGridCraft>();
            List<UserSearch> UserSearchlList = user.RetrieveUserSearchCraftDetails(userData.DatabaseKey);
            InnerGridCraft objInnerGridCraft = new InnerGridCraft();

            foreach (var data in UserSearchlList)
            {
                objInnerGridCraft = new InnerGridCraft();
                objInnerGridCraft.SiteName = data.SiteName;
                objInnerGridCraft.CraftDescription = data.CraftDescription;
                InnerGridCraftList.Add(objInnerGridCraft);
            }
            return InnerGridCraftList;
        }
        #endregion

        #region V2-417 Inactivate and Active Users
        public UserDetails ValidateUserStatusChange(long userInfoId, string flag)
        {
            UserDetails userdtls = new UserDetails()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                UserInfoId = userInfoId,
                Flag = flag,
                UserType = this.userData.DatabaseKey.User.UserType
            };
            //if(flag== "Inactivate")
            if (flag == ActivationStatusConstant.InActivate)
            {
                userdtls.CheckUserIsInactivate(userData.DatabaseKey);
            }
            else
            {
                userdtls.SiteControlled = this.userData.DatabaseKey.Client.SiteControl;
                userdtls.CheckUserIsActivate(userData.DatabaseKey);
            }
            return userdtls;
        }

        public List<string> UpdateUserActiveStatus(long userInfoId, bool inActiveFlag)
        {
            List<string> errList = new List<string>();
            UserDetails userdtls = new UserDetails()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                UserInfoId = userInfoId
            };
            userdtls.RetrieveUserDetailsByUserInfoID(userData.DatabaseKey);
            userdtls.IsActive = !inActiveFlag;
            var PersonnelId = userdtls.PersonnelId;
            userdtls.UpdateByPKForeignKeys_V2(userData.DatabaseKey);
            if (userdtls.ErrorMessages == null || userdtls.ErrorMessages.Count <= 0)
            {
                Personnel personnel = new Personnel()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    PersonnelId = PersonnelId
                };
                personnel.Retrieve(userData.DatabaseKey);
                personnel.InactiveFlag = inActiveFlag;
                personnel.Update(userData.DatabaseKey);
                return errList;
            }
            else
            {
                return userdtls.ErrorMessages;
            }
        }

        public List<string> CreateUserEvent(long userInfoId, bool inActiveFlag)
        {
            UserEventLog userEventLog = new UserEventLog();
            userEventLog.ClientId = userData.DatabaseKey.User.ClientId;
            userEventLog.SiteId = 0;
            userEventLog.ObjectId = userInfoId;
            userEventLog.TransactionDate = DateTime.UtcNow;
            if (inActiveFlag)
            {
                userEventLog.Event = ActivationStatusConstant.InActivate;
            }
            else
            {
                userEventLog.Event = ActivationStatusConstant.Activate;
            }
            userEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            userEventLog.Comments = "";
            userEventLog.SourceId = 0;
            userEventLog.Create(userData.DatabaseKey);
            return userEventLog.ErrorMessages;
        }
        #endregion
        #region V2-419 Enterprise User Management - Add/Remove Sites
        public List<UserManagementPersonnelModel> PopulatePersonnelSites(long userInfoId)
        {
            List<UserDataSet> lstuserdet = new List<UserDataSet>();
            UserDataSet userdet = new UserDataSet();
            userdet.User.UserInfoId = userInfoId;
            userdet.User.ClientId = userData.DatabaseKey.Client.ClientId;
            lstuserdet = userdet.RetrieveUserSiteDetailsByUserInfoID(this.userData.DatabaseKey);
            UserManagementPersonnelModel objUserManagementPersonnlModel;
            List<UserManagementPersonnelModel> UmPersonnelList = new List<UserManagementPersonnelModel>();
            foreach (var p in lstuserdet)
            {
                objUserManagementPersonnlModel = new UserManagementPersonnelModel();
                objUserManagementPersonnlModel.ClientId = p.Personnel.ClientId;
                objUserManagementPersonnlModel.PersonnelId = p.Personnel.PersonnelId;
                objUserManagementPersonnlModel.UserInfoId = p.User.UserInfoId;
                objUserManagementPersonnlModel.SiteId = p.Personnel.SiteId;
                objUserManagementPersonnlModel.UserSiteName = p.Personnel.UserSiteName;
                objUserManagementPersonnlModel.CraftDescription = p.Personnel.CraftDescription;
                UmPersonnelList.Add(objUserManagementPersonnlModel);
            }
            return UmPersonnelList;
        }
        public List<string> AddUmSites(UserSiteModel umSite, long userInfoId)
        {
            string msg = String.Empty;
            List<string> ErroMsgList = new List<string>();
            DataContracts.LoginInfo loginInfo = new DataContracts.LoginInfo() { UserInfoId = umSite.UserInfoId };
            loginInfo.RetrieveByUserInfoId(userData.DatabaseKey);

            Personnel personnel = new Personnel()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                UserInfoId = umSite.UserInfoId,
                SiteId = umSite.SiteId,
                ClientLookupId = loginInfo.UserName,
                NameFirst = umSite.FirstName ?? string.Empty,
                NameMiddle = umSite.MiddleName ?? string.Empty,
                NameLast = umSite.LastName ?? string.Empty,
                CraftId = umSite.CraftId,
                Buyer = umSite.Buyer
            };
            personnel.Create(userData.DatabaseKey);
            //--------------Create Permission record------------
            UserPermission userpermission = new UserPermission();
            userpermission.UserInfoId = umSite.UserInfoId;
            userpermission.ClientId = userData.DatabaseKey.Client.ClientId;
            userpermission.SiteId = umSite.SiteId;
            userpermission.PermissionType = "G";
            userpermission.UpdateIndex = 0;
            userpermission.Create(this.userData.DatabaseKey);   /*create User Permission*/
            //------Update Site User Counts---------
            // RKL - 2025-Jan-23 - Begin
            // V2-1028 and V2-1113
            // Need to respect the excluded users list
            //
            UserDetails userdet = new UserDetails();
            userdet.UserInfoId = umSite.UserInfoId;
            userdet.ClientId = userData.DatabaseKey.Client.ClientId;
            userdet.DefaultSiteId = umSite.SiteId;
            userdet.RetrieveSiteUserCounts_V2(userData.DatabaseKey);
            Site site = new Site();
            site.ClientId = userData.DatabaseKey.Client.ClientId;
            site.SiteId = umSite.SiteId;
            site.Retrieve(userData.DatabaseKey);
            site.AppUsers = Convert.ToInt32(userdet.CountWebAppUser);
            site.LimitedUsers = Convert.ToInt32(userdet.CountLimitedUser);
            site.WorkRequestUsers = Convert.ToInt32(userdet.CountWorkRequestUser);
            site.SuperUsers = Convert.ToInt32(userdet.CountSuperUser);
            site.ProdAppUsers = Convert.ToInt32(userdet.CountProdUser);
            /*
            if (umSite.UserType.ToUpper() == UserTypeConstants.Full.ToUpper() || umSite.UserType.ToUpper() == UserTypeConstants.Admin.ToUpper())
            {
                site.AppUsers = Convert.ToInt32(site.AppUsers) + 1;
            }
            if (umSite.UserType.ToUpper() == UserTypeConstants.Admin.ToUpper() && umSite.IsSuperUser == true)
            {
                site.SuperUsers = Convert.ToInt32(site.SuperUsers) + 1;

            }
            //V2-1054
            if (umSite.UserType.ToUpper() == UserTypeConstants.WorkRequest.ToUpper())
            {
                site.WorkRequestUsers = Convert.ToInt32(site.WorkRequestUsers) + 1;

            }
            */
            //
            // RKL - 2025-Jan-23 - End
            //
            site.Update(this.userData.DatabaseKey);
            CreateEventLog(umSite.UserInfoId, umSite.SiteId, EventStatusConstants.AddSite);
            umSite.UserInfoId = userInfoId;
            umSite.ErrorMessages = ErroMsgList;
            return umSite.ErrorMessages;
        }
        public UserDetails ValidateAddSite(long userInfoId, long _siteid, bool _siteControlled, string _userType)
        {
            UserDetails userdet = new UserDetails();
            userdet.UserInfoId = userInfoId;
            userdet.ObjectId = _siteid;
            userdet.ClientId = userData.DatabaseKey.Client.ClientId;
            userdet.SiteControlled = _siteControlled;
            userdet.UserType = _userType;
            userdet.ValidateNewSiteForExistingUser_V2(this.userData.DatabaseKey);
            return userdet;
        }

        public UserDetails ValidateRemoveSite(long _userInfoId, long _siteId, bool _siteControlled, string _userType)
        {
            UserDetails userdet = new UserDetails();
            userdet.UserInfoId = _userInfoId;
            userdet.ObjectId = _siteId;
            userdet.ClientId = userData.DatabaseKey.Client.ClientId;
            userdet.SiteControlled = _siteControlled;
            userdet.UserType = _userType;
            userdet.ValidateRemoveSiteForExistingUser_V2(this.userData.DatabaseKey);
            return userdet;
        }

        public bool UmPersonnelDelete(long personnelId, long userInfoId, long siteId, string userType, bool isSuperUser, long defaultSiteId)
        {
            try
            {
                Personnel personnel = new Personnel()
                {
                    PersonnelId = personnelId
                };
                personnel.Delete(userData.DatabaseKey);

                UserPermission userPermission = new UserPermission()
                {
                    UserInfoId = userInfoId,
                    SiteId = siteId
                };
                userPermission.DeleteBySiteAndUser(userData.DatabaseKey);
                UserDetails userdet = new UserDetails();
                userdet.UserInfoId = userInfoId;
                userdet.ClientId = userData.DatabaseKey.Client.ClientId;
                userdet.DefaultSiteId = siteId;
                userdet.RetrieveSiteUserCounts_V2(userData.DatabaseKey);
                Site site = new Site();
                site.ClientId = userData.DatabaseKey.Client.ClientId;
                site.SiteId = siteId;
                site.Retrieve(userData.DatabaseKey);
                //------Update Site User Counts---------
                // RKL - 2025-Jan-23 - Begin
                // V2-1028 and V2-1113
                // Need to respect the excluded users list
                //
                site.AppUsers = Convert.ToInt32(userdet.CountWebAppUser);
                site.LimitedUsers = Convert.ToInt32(userdet.CountLimitedUser);
                site.WorkRequestUsers = Convert.ToInt32(userdet.CountWorkRequestUser);
                site.SuperUsers = Convert.ToInt32(userdet.CountSuperUser);
                site.ProdAppUsers = Convert.ToInt32(userdet.CountProdUser);
                /*

                if (userType.ToUpper() == UserTypeConstants.Full.ToUpper() || userType.ToUpper() == UserTypeConstants.Admin.ToUpper())
                {
                    site.AppUsers = Convert.ToInt32(site.AppUsers) - 1;
                }
                if (userType.ToUpper() == UserTypeConstants.Admin.ToUpper() && isSuperUser == true)
                {
                    site.SuperUsers = Convert.ToInt32(site.SuperUsers) - 1;

                }
                //V2-1054
                if (userType.ToUpper() == UserTypeConstants.WorkRequest.ToUpper())
                {
                    site.WorkRequestUsers = Convert.ToInt32(site.WorkRequestUsers) - 1;

                }
                */
                site.Update(this.userData.DatabaseKey);

                CreateEventLog(userdet.UserInfoId, siteId, EventStatusConstants.RemoveSite);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region V2-491 Unlock Account
        public List<string> UnlockAccount(long userinfoId, long Siteid, long personnelid)
        {
            List<string> EMsg = new List<string>();
            DataContracts.LoginInfo loginInfo = new DataContracts.LoginInfo() { UserInfoId = userinfoId };
            loginInfo.RetrieveByUserInfoId(userData.DatabaseKey);

            if (loginInfo.LoginInfoId > 0)
            {
                loginInfo.FailedAttempts = 0;
                loginInfo.Update(userData.DatabaseKey);
                CreateEventLogforPasswordChange(userinfoId, Siteid, EventStatusConstants.UnlockAccount, personnelid);
            }
            else
            {
                EMsg = loginInfo.ErrorMessages;
            }
            return EMsg;
        }
        #region V2-491 Password Change
        internal PasswordSettingsModel PasswordSettingsDetails()
        {
            PasswordSettingsModel PasswordSettingsModel = new PasswordSettingsModel();
            DataContracts.PasswordSettings PS = new DataContracts.PasswordSettings();
            PS.ClientId = userData.DatabaseKey.Client.ClientId;
            PS.RetrieveByClientId(this.userData.DatabaseKey);
            PasswordSettingsModel.MaxAttempts = PS.MaxAttempts;
            PasswordSettingsModel.PWReqMinLength = PS.PWReqMinLength;
            PasswordSettingsModel.PWMinLength = PS.PWMinLength;
            PasswordSettingsModel.PWReqExpiration = PS.PWReqExpiration;
            PasswordSettingsModel.PWExpiresDays = PS.PWExpiresDays;
            PasswordSettingsModel.PWRequireNumber = PS.PWRequireNumber;
            PasswordSettingsModel.PWRequireAlpha = PS.PWRequireAlpha;
            PasswordSettingsModel.PWRequireMixedCase = PS.PWRequireMixedCase;
            PasswordSettingsModel.PWRequireSpecialChar = PS.PWRequireSpecialChar;
            PasswordSettingsModel.PWNoRepeatChar = PS.PWNoRepeatChar;
            PasswordSettingsModel.PWNotEqualUserName = PS.PWNotEqualUserName;
            PasswordSettingsModel.AllowAdminReset = PS.AllowAdminReset;

            return PasswordSettingsModel;
        }
        public ManualResetPasswordModel GetUserDetailsManual(long UserInfoId)
        {
            ManualResetPasswordModel objResetPasswordModel = new ManualResetPasswordModel();
            DataContracts.UserInfo userInfo = new UserInfo()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                UserInfoId = UserInfoId
            };
            DataContracts.UserInfo userInfoForRetMail = new DataContracts.UserInfo() { UserInfoId = UserInfoId };
            userInfoForRetMail.Retrieve(userData.DatabaseKey);
            userInfo.RetrieveLoginInfoByUserInfoId2(userData.DatabaseKey);
            objResetPasswordModel.UserInfoId = userInfo.UserInfoId;
            objResetPasswordModel.UserName = userInfo.LoginUserName;
            objResetPasswordModel.FirstName = userInfo.FirstName;
            objResetPasswordModel.MiddleName = userInfo.MiddleName;
            objResetPasswordModel.LastName = userInfo.LastName;
            objResetPasswordModel.EmailAddress = userInfoForRetMail.Email;
            return objResetPasswordModel;
        }

        public bool ManualResetPassword(ManualResetPasswordModel manualResetPasswordModel)
        {

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

            if (ResetPasswordManual(manualResetPasswordModel, body))
            {
                CreateEventLogforPasswordChange(manualResetPasswordModel.UserInfoId, manualResetPasswordModel.SiteId, EventStatusConstants.ResetPassword, manualResetPasswordModel.PersonnelId);
                return true;//litMessage.Text = password.Message;
            }
            else
            {
                return false;// litFailureMessage.Text = password.Message;
            }
        }

        public bool ResetPasswordManual(ManualResetPasswordModel ipData, StringBuilder mailBody)
        {
            bool success = false;
            if (string.Compare(ipData.Password, ipData.ConfirmPassword, false) != 0)
            {
                return success;
            }

            LoginInfo loginInfo = new LoginInfo()
            { UserInfoId = ipData.UserInfoId };
            loginInfo.RetrieveByUserInfoId(userData.DatabaseKey);


            // SOM-520 - Begin
            loginInfo.Password = Encryption.SHA512Encrypt(loginInfo.UserName.Trim().ToUpper() + ipData.Password);
            // SOM-520 - End
            loginInfo.ResetPasswordCode = Guid.Empty;
            loginInfo.ResetPasswordRequestDate = null;
            loginInfo.TempPassword = string.Empty;
            //Som-1192
            loginInfo.FailedAttempts = 0;
            loginInfo.LastPWChangeDate = DateTime.UtcNow;
            // Assign the client id if the dbkey is retrieve from GetAdminOnlyKey()
            if (userData.DatabaseKey.Client.ClientId == 0) { userData.DatabaseKey.Client.ClientId = loginInfo.ClientId; }
            loginInfo.UpdateCustom(userData.DatabaseKey);
            UserInfo userInfo = new UserInfo { ClientId = loginInfo.ClientId, UserInfoId = loginInfo.UserInfoId };
            userInfo.Retrieve(userData.DatabaseKey);

            // Send email to notify the user
            //Email email = new Email()
             EmailModule email = new EmailModule()
             {
                Subject = "Password Changed",
                Body = mailBody.ToString() //"Your password has Changed."
            };
            var adminMail = userData.DatabaseKey.User.Email;
            string targetMail = String.IsNullOrEmpty(userInfo.Email) ? adminMail : userInfo.Email;
            email.Recipients.Add(targetMail);
            email.SendEmail();

            success = true;

            return success;
        }
        #endregion


        private void CreateEventLogforPasswordChange(long objId, long siteId, string eventVal = "", long personnelid = 0) //V2-491
        {

            UserEventLog log = new UserEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = siteId;
            log.ObjectId = objId;
            log.Event = eventVal;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = personnelid;
            log.Comments = string.Empty;
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region V2-629 change user name

       
        public UserNameChangeModel ChangeUserName(UserNameChangeModel _userNameChangeModel)
        {
            List<string> ErroMsgList = new List<string>();
            LoginInfo loginInfo = new LoginInfo()
            { UserInfoId = _userNameChangeModel.UserInfoId };
            loginInfo.RetrieveByUserInfoId(userData.DatabaseKey);
            loginInfo.Password = Encryption.SHA512Encrypt(_userNameChangeModel.UserName.Trim().ToUpper() + _userNameChangeModel.Password);
            loginInfo.ResetPasswordCode = Guid.Empty;
            loginInfo.ResetPasswordRequestDate = null;
            loginInfo.TempPassword = string.Empty;
            loginInfo.UserName = _userNameChangeModel.UserName;
            loginInfo.FailedAttempts = 0;
            loginInfo.LastPWChangeDate = DateTime.UtcNow;
            // Assign the client id if the dbkey is retrieve from GetAdminOnlyKey()
            if (userData.DatabaseKey.Client.ClientId == 0) { userData.DatabaseKey.Client.ClientId = loginInfo.ClientId; }
            loginInfo.UpdateCustom(userData.DatabaseKey);
        
            if (loginInfo.ErrorMessages != null && loginInfo.ErrorMessages.Count > 0)
            {
                ErroMsgList.AddRange(loginInfo.ErrorMessages);
                _userNameChangeModel.ErrorMessages = ErroMsgList;
                return _userNameChangeModel;
            }
            else
            {
                CreateEventLogforChangeUserName(_userNameChangeModel.UserInfoId, _userNameChangeModel.DefaultSiteId??0, EventStatusConstants.ResetPassword, _userNameChangeModel.DefaultPersonnelId??0);
               
            }          
            _userNameChangeModel.ErrorMessages = ErroMsgList;
            return _userNameChangeModel;
        }

        private void CreateEventLogforChangeUserName(long objId, long siteId, string eventVal = "", long personnelid = 0) //V2-491
        {

            UserEventLog log = new UserEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = siteId;
            log.ObjectId = objId;
            log.Event = eventVal;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = personnelid;
            log.Comments = string.Empty;
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        public UserDetails GetPersonnelAndUserdetailsByUserInfoID(long userInfoId)
        {
            
            DataContracts.UserDetails userdet = new DataContracts.UserDetails()
            {
                UserInfoId = userInfoId,
                ClientId = userData.DatabaseKey.Client.ClientId,
            };
            userdet.RetrievePersonnelAndUserdetailsByUserInfoID(this.userData.DatabaseKey);

            return userdet;
        }

        #region V2-680 User Management - Add/Remove Storerooms
        public List<UserStoreroomModel> PopulatePersonnelStorerooms(long personnelId, string orderbycol = "", int length = 0, string orderDir = "", int skip = 0)
        {
            List<UserStoreroomModel> UserStoreroomModelList = new List<UserStoreroomModel>();
            UserStoreroomModel userStoreroomModel;
            StoreroomAuth storeroomAuth = new StoreroomAuth();
            storeroomAuth.ClientId = userData.DatabaseKey.Client.ClientId;
            storeroomAuth.PersonnelId=personnelId;
            storeroomAuth.orderbyColumn = orderbycol;
            storeroomAuth.orderBy = orderDir;
            storeroomAuth.offset1 = skip;
            storeroomAuth.nextrow = length;

            var uList = storeroomAuth.RetrieveUserStoreroomDetailsByClientId(this.userData.DatabaseKey);
            foreach (var p in uList)
            {
                userStoreroomModel = new UserStoreroomModel();
                userStoreroomModel.StoreroomAuthId = p.StoreroomAuthId;
                userStoreroomModel.SiteName = p.SiteName;
                userStoreroomModel.StoreroomName = p.StoreroomName;
                userStoreroomModel.TotalCount = p.totalCount;
                UserStoreroomModelList.Add(userStoreroomModel);
            }
            return UserStoreroomModelList;
        }
        public List<string> AddUmStorerooms(UserStoreroomModel userStoreroomModel)
        {
            string msg = String.Empty;
            StoreroomAuth storeroomAuth = new StoreroomAuth()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userStoreroomModel.SiteId,
                PersonnelId = GetUserPersonnelIdBySiteIdAndClientId(userStoreroomModel.SiteId, userStoreroomModel.UserInfoId),
                StoreroomId = userStoreroomModel.StoreroomId,
                Maintain = userStoreroomModel.Maintain,
                Issue = userStoreroomModel.Issue,
                IssueTransfer = userStoreroomModel.IssueTransfer,
                ReceiveTransfer = userStoreroomModel.ReceiveTransfer,
                PhysicalInventory = userStoreroomModel.PhysicalInventory,
                Purchase = userStoreroomModel.Purchase,
                ReceivePurchase = userStoreroomModel.ReceivePurchase
            };
            storeroomAuth.CheckDuplicateSite(this.userData.DatabaseKey);
            if(storeroomAuth.ErrorMessages==null || storeroomAuth.ErrorMessages.Count ==0 )
            {
                storeroomAuth.Create(userData.DatabaseKey);
                //add user event log
                CreateEventLog(userStoreroomModel.StoreroomId, userStoreroomModel.SiteId, EventStatusConstants.AddStoreroom);
            }            
            return storeroomAuth.ErrorMessages;
        }
        public List<string> EditUmStorerooms(UserStoreroomModel userStoreroomModel)
        {
            StoreroomAuth storeroomAuth = new StoreroomAuth()
            {
                StoreroomAuthId = userStoreroomModel.StoreroomAuthId
            };
            storeroomAuth.Retrieve(userData.DatabaseKey);
            storeroomAuth.Maintain = userStoreroomModel.Maintain;
            storeroomAuth.Issue = userStoreroomModel.Issue;
            storeroomAuth.IssueTransfer = userStoreroomModel.IssueTransfer;
            storeroomAuth.ReceiveTransfer = userStoreroomModel.ReceiveTransfer;
            storeroomAuth.PhysicalInventory = userStoreroomModel.PhysicalInventory;
            storeroomAuth.Purchase = userStoreroomModel.Purchase;
            storeroomAuth.ReceivePurchase = userStoreroomModel.ReceivePurchase;
            storeroomAuth.Update(userData.DatabaseKey);
            return storeroomAuth.ErrorMessages;
        }
        public UserStoreroomModel RetrieveStoreroomsForEdit(long storeroomAuthId)
        {
            UserStoreroomModel objUserStoreroomModel = new UserStoreroomModel();
            long clientId = userData.DatabaseKey.Personnel.ClientId;
            StoreroomAuth storeroomAuth = new StoreroomAuth() { StoreroomAuthId = storeroomAuthId, ClientId = clientId };
            storeroomAuth.RetrieveByStoreroomAuthId(userData.DatabaseKey);
            objUserStoreroomModel.StoreroomAuthId = storeroomAuth.StoreroomAuthId;
            objUserStoreroomModel.ClientId = storeroomAuth.ClientId;
            objUserStoreroomModel.PersonnelId = storeroomAuth.PersonnelId;
            objUserStoreroomModel.SiteId = storeroomAuth.SiteId;
            objUserStoreroomModel.StoreroomId = storeroomAuth.StoreroomId;
            objUserStoreroomModel.Maintain = storeroomAuth.Maintain;
            objUserStoreroomModel.Issue = storeroomAuth.Issue;
            objUserStoreroomModel.IssueTransfer = storeroomAuth.IssueTransfer;
            objUserStoreroomModel.ReceiveTransfer = storeroomAuth.ReceiveTransfer;
            objUserStoreroomModel.PhysicalInventory = storeroomAuth.PhysicalInventory;
            objUserStoreroomModel.Purchase = (bool)storeroomAuth.Purchase;
            objUserStoreroomModel.ReceivePurchase = storeroomAuth.ReceivePurchase;
            objUserStoreroomModel.SiteName = storeroomAuth.SiteName;
            objUserStoreroomModel.StoreroomName = storeroomAuth.StoreroomName;
            return objUserStoreroomModel;
        }
        public bool UmStoreroomDelete(long storeroomAuthId)
        {
            try
            {
                StoreroomAuth storeroomAuth = new StoreroomAuth()
                {
                    StoreroomAuthId = storeroomAuthId
                };
                storeroomAuth.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<SelectListItem> RetrieveAllStoreroomBySiteId(long siteId)
        {
            StoreroomAuth storeroomAuth = new StoreroomAuth();
            storeroomAuth.ClientId = userData.DatabaseKey.Personnel.ClientId;
            storeroomAuth.SiteId = siteId;
            List<StoreroomAuth> obj_StoreroomAuth = storeroomAuth.RetrieveAllStoreroomBySiteId(userData.DatabaseKey);
            var Storerooms = obj_StoreroomAuth.Select(x => new SelectListItem { Text = x.StoreroomName, Value = x.StoreroomId.ToString() }).ToList();
            return Storerooms;
        }

        public List<SelectListItem> GetAllSelectedUserSites(long userInfoId)
        {
            List<UserDataSet> lstuserdet = new List<UserDataSet>();
            UserDataSet userdet = new UserDataSet();
            userdet.User.UserInfoId = userInfoId;
            userdet.User.ClientId = userData.DatabaseKey.Client.ClientId;
            lstuserdet = userdet.RetrieveUserSiteDetailsByUserInfoID(this.userData.DatabaseKey);        
            var Sites = lstuserdet.Select(x => new SelectListItem { Text = x.Personnel.UserSiteName + "-" + x.Personnel.SiteDescription, Value = x.Personnel.SiteId.ToString() }).ToList();
            return Sites;
        }
        public long GetUserPersonnelIdBySiteIdAndClientId(long siteId, long userInfoId)
        {
            long PersonnelId = 0;
            UserDataSet userDataSet = new UserDataSet();
            userDataSet.User.UserInfoId = userInfoId;
            userDataSet.Client.ClientId = userData.DatabaseKey.Client.ClientId;
            userDataSet.User.DefaultSiteId = siteId;
            userDataSet.RetrievePersonnelByUserInfoId(this.userData.DatabaseKey);
            PersonnelId = userDataSet.Personnel.PersonnelId;
            return PersonnelId;
        }
        #endregion
        #region V2-802 Security Profile Ids
        public IEnumerable<SelectListItem> GetSecurityProfileList()
        {
            DataContracts.SecurityProfile secprof = new DataContracts.SecurityProfile()
            {
                ClientId = userData.DatabaseKey.Client.ClientId
            };
             List<DataContracts.SecurityProfile> splist = new List<DataContracts.SecurityProfile>();
            splist = secprof.CustomSecurityProfileRetrieveByClientIdV2(this.userData.DatabaseKey);
            
            return splist.Select(x => new SelectListItem { Text = x.Name, Value = Convert.ToString(x.SecurityProfileId) });
        }

        #endregion

        #region V2-547 Add Reference User
        public ReferenceUserModel AddReferenceUser(ReferenceUserModel referenceUserModel)
        {
            List<string> ErroMsgList = new List<string>();

           //creating new UserInfo//
            UserInfo userInfo = new UserInfo();
            userInfo.ClientId = userData.DatabaseKey.Client.ClientId;
            userInfo.SecurityProfileId = 0;
            userInfo.FirstName = referenceUserModel.FirstName;
            userInfo.LastName = referenceUserModel.LastName;
            userInfo.MiddleName = referenceUserModel.MiddleName;
            userInfo.Email = "";
            userInfo.Localization = "en-us";
            userInfo.TimeZone = userData.Site.TimeZone;
            userInfo.IsSuperUser = false;
            userInfo.DefaultSiteId = _dbKey.Personnel.SiteId;
            userInfo.UserType = UserTypeConstants.Reference;
            userInfo.ResultsPerPage = 10;
            userInfo.StartPage = "1";
            userInfo.IsPasswordTemporary = false;
            userInfo.Create_V2(_dbKey);
            
            if (userInfo.ErrorMessages == null || userInfo.ErrorMessages.Count == 0)
            {   //adding new login info
                LoginInfo loginInfo = new LoginInfo();
                loginInfo.ClientId = userData.DatabaseKey.Client.ClientId;
                loginInfo.UserInfoId = userInfo.UserInfoId;
                loginInfo.UserName= referenceUserModel.UserName;
                loginInfo.IsActive = true;
                loginInfo.Create(_dbKey);
                if (loginInfo.ErrorMessages != null && loginInfo.ErrorMessages.Count > 0)
                {
                    ErroMsgList.AddRange(loginInfo.ErrorMessages);
                    referenceUserModel.ErrorMessages = ErroMsgList;
                    return referenceUserModel;
                }
                else
                {
                    // adding personnel record 
                    Personnel personnel = new Personnel();
                    personnel.ClientId = userData.DatabaseKey.Client.ClientId;
                    personnel.UserInfoId = userInfo.UserInfoId;
                    personnel.SiteId = _dbKey.Personnel.SiteId;
                    personnel.ClientLookupId = referenceUserModel.UserName;
                    personnel.NameFirst = referenceUserModel.FirstName;
                    personnel.NameMiddle = referenceUserModel.MiddleName;
                    personnel.NameLast = referenceUserModel.LastName;
                    personnel.CraftId = referenceUserModel.CraftId ?? 0;
                    personnel.Buyer = false;
                    personnel.ScheduleEmployee = true;
                    personnel.Planner = false;
                    personnel.Scheduler = false;
                    personnel.Create(_dbKey);
                    if (personnel.ErrorMessages != null && personnel.ErrorMessages.Count > 0)
                    {
                        ErroMsgList.AddRange(loginInfo.ErrorMessages);
                        return referenceUserModel;
                    }
                    else
                    {
                        //Adding  User Event log for newly created Reference User
                        CreateEventLogforAddReference(userInfo.UserInfoId, personnel.SiteId, EventStatusConstants.Create, userData.DatabaseKey.Personnel.PersonnelId);
                        return referenceUserModel;
                    }
                }
            }
            ErroMsgList.AddRange(userInfo.ErrorMessages);
            referenceUserModel.ErrorMessages = ErroMsgList;
            return referenceUserModel;
        }

        private void CreateEventLogforAddReference(long objId, long siteId, string eventVal = "", long personnelid = 0) 
        {

            UserEventLog log = new UserEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = siteId;
            log.ObjectId = objId;
            log.Event = eventVal;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = personnelid;
            log.Comments = string.Empty;
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region V2-905
        public List<UserManagementModel> PopulateuserManagmentForEnterprise(int Start = 0, int length = 0, int CaseNo = 0, string UserName = "", string LastName = "", string FirstName = "",
                                    string Email = "", long CraftId = 0, string SearchText = "", string OrderByColumn = "0", string OrderBy = "asc", string SelectedSites = "", string SecurityProfileIds = "", string UserTypes = "", string Shifts = "", bool? IsActive = null, string EmployeeId = "")
        {
            UserSearch user = new UserSearch()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                DefaultSiteId = userData.DatabaseKey.User.DefaultSiteId,
                PackageLevel = userData.DatabaseKey.Client.PackageLevel,
                IsSuperUser = userData.DatabaseKey.User.IsSuperUser,
                CaseNo = CaseNo,
                UserName = UserName,
                LastName = LastName,
                FirstName = FirstName,
                Email = Email,
                PersonnelCraftId = CraftId,
                SearchText = SearchText,
                Offset = Start,
                Nextrow = length,
                OrderBy = OrderBy,
                OrderByColumn = OrderByColumn,
                SelectedSites = SelectedSites,
                SecurityProfileIds = SecurityProfileIds,
                UserType = UserTypes,
                Shift = Shifts,
                IsActiveStatus = IsActive,
                EmployeeId = EmployeeId  //V2-1160
            };
            List<UserManagementModel> UserManagementModelList = new List<UserManagementModel>();
            List<UserSearch> UserSearchlList = user.RetrieveUserSearchListForEnterprise(userData.DatabaseKey);
            UserManagementModel objUserManagementModel = new UserManagementModel();

            foreach (var data in UserSearchlList)
            {
                objUserManagementModel = new UserManagementModel();
                objUserManagementModel.UserName = data.UserName;
                objUserManagementModel.LastName = data.LastName;
                objUserManagementModel.FirstName = data.FirstName;
                objUserManagementModel.SecurityProfileDescription = data.SecurityProfileDescription;
                objUserManagementModel.Email = data.Email;
                objUserManagementModel.CraftDescription = data.CraftDescription;
                objUserManagementModel.TotalCount = data.TotalCount;
                objUserManagementModel.SiteCount = data.SiteCount;
                objUserManagementModel.UserInfoId = data.UserInfoId;
                UserManagementModelList.Add(objUserManagementModel);
            }
            return UserManagementModelList;
        }
        public List<UserManagementModel> PopulateuserManagmentForBasicProfessional(int Start = 0, int length = 0, int CaseNo = 0, string UserName = "", string LastName = "", string FirstName = "",
                                    string Email = "", long CraftId = 0, string SearchText = "", string OrderByColumn = "0", string OrderBy = "asc", string SelectedSites = "", string SecurityProfileIds = "", string UserTypes = "", string Shifts = "", bool? IsActive = null, string EmployeeId = "")
        {
            UserSearch user = new UserSearch()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                DefaultSiteId = userData.DatabaseKey.User.DefaultSiteId,
                PackageLevel = userData.DatabaseKey.Client.PackageLevel,
                IsSuperUser = userData.DatabaseKey.User.IsSuperUser,
                CaseNo = CaseNo,
                UserName = UserName,
                LastName = LastName,
                FirstName = FirstName,
                Email = Email,
                PersonnelCraftId = CraftId,
                SearchText = SearchText,
                Offset = Start,
                Nextrow = length,
                OrderBy = OrderBy,
                OrderByColumn = OrderByColumn,
                SelectedSites = SelectedSites,
                SecurityProfileIds = SecurityProfileIds,
                UserType = UserTypes,
                Shift = Shifts,
                IsActiveStatus = IsActive,
                EmployeeId = EmployeeId  //V2-1160
            };
            List<UserManagementModel> UserManagementModelList = new List<UserManagementModel>();
            List<UserSearch> UserSearchlList = user.RetrieveUserSearchListForBasicProfessional(userData.DatabaseKey);
            UserManagementModel objUserManagementModel = new UserManagementModel();

            foreach (var data in UserSearchlList)
            {
                objUserManagementModel = new UserManagementModel();
                objUserManagementModel.UserName = data.UserName;
                objUserManagementModel.LastName = data.LastName;
                objUserManagementModel.FirstName = data.FirstName;
                objUserManagementModel.SecurityProfileDescription = data.SecurityProfileDescription;
                objUserManagementModel.Email = data.Email;
                objUserManagementModel.CraftDescription = data.CraftDescription;
                objUserManagementModel.TotalCount = data.TotalCount;
                objUserManagementModel.UserInfoId = data.UserInfoId;
                UserManagementModelList.Add(objUserManagementModel);
            }
            return UserManagementModelList;
        }
        #endregion
    }

}

