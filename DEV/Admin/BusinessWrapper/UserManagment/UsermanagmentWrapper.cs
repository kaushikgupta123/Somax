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
using Admin.Models;
using Admin.Models.UserManagement;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Admin.BusinessWrapper
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
        public UserModel GetdUserdetailsByUserInfoID(long userInfoId, long ClientId)
        {

            UserInfo userdet = new UserInfo()
            {
                UserInfoId = userInfoId,
                ClientId = ClientId,
            };
            userdet.RetrieveUserDetailsByUserInfoId(this.userData.DatabaseKey);
            UserModel _userModel = new UserModel();
            _userModel.UserInfoId = userdet.UserInfoId;
            _userModel.UserName = userdet.UserName;
            _userModel.FirstName = userdet.FirstName;
            _userModel.MiddleName = userdet.MiddleName;
            _userModel.LastName = userdet.LastName;
            _userModel.Email = userdet.Email;
            _userModel.SecurityProfileDescription = userdet.SecurityProfileDescription;
            _userModel.SecurityProfileName = userdet.SecurityProfileName;
            _userModel.IsActive = userdet.IsActive;
            _userModel.UserType = userdet.UserType;
            _userModel.IsSuperUser = userdet.IsSuperUser;
            _userModel.DefaultSiteId = userdet.DefaultSiteId;
            _userModel.PersonnelId = userdet.Personnel_PersonnelId;
            _userModel.CraftId = userdet.Personnel_CraftId;
            return _userModel;
        }

        #region  Security Profile Ids
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

        #region Search Grid
        public List<DataContracts.Client> GetAllActiveClientForAdmin()
        {
            DataContracts.Client client = new DataContracts.Client();
            var retData = client.RetrieveAllActiveClient(this.userData.DatabaseKey);
            DataContracts.Client clientModel;
            List<DataContracts.Client> ClientModelList = new List<DataContracts.Client>();
            foreach (var item in retData)
            {
                clientModel = new DataContracts.Client();
                clientModel.ClientId = item.ClientId;
                clientModel.Name = item.CompanyName;
                ClientModelList.Add(clientModel);
            }
            return ClientModelList;
        }
        public List<UserManagementModel> PopulateuserManagmentForAdmin(int Start = 0, int length = 0, int CaseNo = 0, string UserName = "", string LastName = "", string FirstName = "",
                                    string Email = "", long ClientId = 0, string SearchText = "", string OrderByColumn = "0", string OrderBy = "asc", long DefaultSiteId = 0)
        {
            UserSearch user = new UserSearch()
            {

                ClientId = ClientId,
                DefaultSiteId = DefaultSiteId,

                CaseNo = CaseNo,
                UserName = UserName,
                LastName = LastName,
                FirstName = FirstName,
                Email = Email,

                SearchText = SearchText,
                Offset = Start,
                Nextrow = length,
                OrderBy = OrderBy,
                OrderByColumn = OrderByColumn,

            };
            List<UserManagementModel> UserManagementModelList = new List<UserManagementModel>();
            List<UserSearch> UserSearchlList = user.RetrieveUserSearchListForAdmin(userData.DatabaseKey);

            UserManagementModel objUserManagementModel = new UserManagementModel();

            foreach (var data in UserSearchlList)
            {
                objUserManagementModel = new UserManagementModel();
                objUserManagementModel.ClientId = data.ClientId;
                objUserManagementModel.UserName = data.UserName;
                objUserManagementModel.LastName = data.LastName;
                objUserManagementModel.FirstName = data.FirstName;
                objUserManagementModel.SecurityProfile = data.SecurityProfileName;
                objUserManagementModel.Email = data.Email;
                objUserManagementModel.CompanyName = data.CompanyName;
                objUserManagementModel.TotalCount = data.TotalCount;
                objUserManagementModel.SiteCount = data.SiteCount;
                objUserManagementModel.IsActive = data.IsActive;
                objUserManagementModel.UserInfoId = data.UserInfoId;
                UserManagementModelList.Add(objUserManagementModel);
            }
            return UserManagementModelList;
        }
        public List<InnerGridUserManagement> PopulateuserManagmentInnerGridDetails(long UserInfoId, long ClientId)
        {
            Personnel personnel = new Personnel()
            {
                UserInfoId = UserInfoId,
                ClientId = ClientId

            };
            List<InnerGridUserManagement> InnerGridCraftList = new List<InnerGridUserManagement>();
            List<Personnel> PersonnelList = personnel.RetrieveByUserInfoIdForAdminUserManagementChildGrid(userData.DatabaseKey);
            InnerGridUserManagement objInnerGrid = new InnerGridUserManagement();

            foreach (var data in PersonnelList)
            {
                objInnerGrid = new InnerGridUserManagement();
                objInnerGrid.SiteName = data.UserSiteName;
                objInnerGrid.CraftDescription = data.CraftDescription;
                objInnerGrid.SiteId = data.SiteId;
                objInnerGrid.CraftId = data.CraftId;
                objInnerGrid.CraftDescription = data.CraftDescription;
                objInnerGrid.Personnel_ClientLookupId = data.Personnel_ClientLookupId;
                objInnerGrid.Planner = data.Planner;
                objInnerGrid.Buyer = data.Buyer;
                InnerGridCraftList.Add(objInnerGrid);
            }
            return InnerGridCraftList;
        }
        #endregion
        #region Populate Activity
        public List<LoginAuditingInfo> PopulateActivity(long ClientId, string order = "0", string orderDir = "asc", int skip = 0, int length = 10)
        {
            LoginAuditingInfo objLoginAuditingInfoModel;
            List<LoginAuditingInfo> LoginAuditingInfoModelList = new List<LoginAuditingInfo>();

            LoginAuditing log = new LoginAuditing();
            List<LoginAuditing> data = new List<LoginAuditing>();
            log.ClientId = ClientId;
            log.order = order;
            log.orderdir = orderDir;
            log.offset = skip;
            log.next = length;
            data = log.RetriveByClientId(this._dbKey);

            if (data != null)
            {
                foreach (var item in data)
                {
                    objLoginAuditingInfoModel = new LoginAuditingInfo();
                    objLoginAuditingInfoModel.ClientId = item.ClientId;
                    objLoginAuditingInfoModel.LoginAuditingId = item.LoginAuditingId;
                    objLoginAuditingInfoModel.LoginInfoId = item.LoginInfoId;
                    objLoginAuditingInfoModel.UserInfoId = item.UserInfoId;
                    objLoginAuditingInfoModel.SessionId = item.SessionId;
                    if (item.CreateDate != null && item.CreateDate == default(DateTime))
                    {
                        objLoginAuditingInfoModel.CreateDate = null;
                    }
                    else
                    {
                        objLoginAuditingInfoModel.CreateDate = item.CreateDate;
                    }
                    objLoginAuditingInfoModel.Browser = item.Browser;
                    objLoginAuditingInfoModel.IPAddress = item.IPAddress;
                    objLoginAuditingInfoModel.TotalCount = item.TotalCount;
                    LoginAuditingInfoModelList.Add(objLoginAuditingInfoModel);
                }
            }
            return LoginAuditingInfoModelList;
        }

        public List<DataContracts.Site> GetSiteForAdmin_V2()
        {
            DataContracts.Site site = new DataContracts.Site();
            var siteList = site.RetrieveForLookupListAdmin_V2(this.userData.DatabaseKey);
            DataContracts.Site siteModel;
            List<DataContracts.Site> SiteModelList = new List<DataContracts.Site>();
            foreach (var item in siteList)
            {
                 siteModel = new DataContracts.Site();
                siteModel.SiteId = item.SiteId;
                siteModel.ClientId = item.ClientId;
                siteModel.ClientName = item.ClientName;
                siteModel.Name = item.Name;
                SiteModelList.Add(siteModel);
            }
            return SiteModelList;
        }
        #endregion

        #region   User Management - Add/Remove Sites
        public List<UserManagementPersonnelModel> PopulatePersonnelSites(long userInfoId, long ClientId)
        {
            List<UserDataSet> lstuserdet = new List<UserDataSet>();
            UserDataSet userdet = new UserDataSet();
            userdet.User.UserInfoId = userInfoId;
            userdet.User.ClientId = ClientId;
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
            DataContracts.LoginInfo loginInfo = new DataContracts.LoginInfo() { UserInfoId = umSite.UserInfoId,ClientId=umSite.ClientId };
            loginInfo.RetrieveByUserInfoIdforAdmin(userData.DatabaseKey);

            Personnel personnel = new Personnel()
            {
                ClientId = umSite.ClientId,
                UserInfoId = umSite.UserInfoId,
                SiteId = umSite.SiteId,
                ClientLookupId = loginInfo.UserName,
                NameFirst = umSite.FirstName ?? string.Empty,
                NameMiddle = umSite.MiddleName ?? string.Empty,
                NameLast = umSite.LastName ?? string.Empty,
                CraftId = umSite.CraftId,
                Buyer = umSite.Buyer
            };
            personnel.PersonnelCreateforAdmin(userData.DatabaseKey);
            //--------------Create Permission record------------
            UserPermission userpermission = new UserPermission();
            userpermission.UserInfoId = umSite.UserInfoId;
            userpermission.ClientId = umSite.ClientId;
            userpermission.SiteId = umSite.SiteId;
            userpermission.PermissionType = "G";
            userpermission.UpdateIndex = 0;
            userpermission.CreateForAdmin(this.userData.DatabaseKey);   /*create User Permission*/
            //------Update Site User Counts---------
            // RKL - 2025-Jan-23 - Begin
            // V2-1028 and V2-1113
            // Need to respect the excluded users list
            //
            UserDetails userdet = new UserDetails();
            userdet.UserInfoId = userInfoId;
            userdet.ClientId = umSite.ClientId;
            userdet.DefaultSiteId = umSite.SiteId;
            userdet.RetrieveSiteUserCounts_V2(userData.DatabaseKey);
            DataContracts.Site site = new DataContracts.Site();
            site.ClientId = umSite.ClientId;
            site.SiteId = umSite.SiteId;
            site.RetrieveForAdmin(userData.DatabaseKey);
            site.AppUsers = Convert.ToInt32(userdet.CountWebAppUser);
            site.LimitedUsers = Convert.ToInt32(userdet.CountLimitedUser);
            site.WorkRequestUsers = Convert.ToInt32(userdet.CountWorkRequestUser);
            site.SuperUsers = Convert.ToInt32(userdet.CountSuperUser);
            site.ProdAppUsers = Convert.ToInt32(userdet.CountProdUser);
            //if (umSite.UserType.ToUpper() == UserTypeConstants.Full.ToUpper() || umSite.UserType.ToUpper() == UserTypeConstants.Admin.ToUpper())
            //{
            //    site.AppUsers = Convert.ToInt32(site.AppUsers) + 1;
            //}
            //if (umSite.UserType.ToUpper() == UserTypeConstants.Admin.ToUpper() && umSite.IsSuperUser == true)
            //{
            //    site.SuperUsers = Convert.ToInt32(site.SuperUsers) + 1;

            //}
            ////V2-1054
            //if (umSite.UserType.ToUpper() == UserTypeConstants.WorkRequest.ToUpper())
            //{
            //    site.WorkRequestUsers = Convert.ToInt32(site.WorkRequestUsers) + 1;

            //}
            // RKL - 2025-Jan-23 - Begin
            site.UpdateForAdmin(this.userData.DatabaseKey);
            CreateEventLogForAdmin(umSite.UserInfoId, umSite.SiteId, umSite.ClientId, EventStatusConstants.AddSite);
            umSite.UserInfoId = userInfoId;
            umSite.ErrorMessages = ErroMsgList;
            return umSite.ErrorMessages;
        }
        public UserDetails ValidateAddSite(long userInfoId, long _siteid, bool _siteControlled, string _userType,long ClientId)
        {
            UserDetails userdet = new UserDetails();
            userdet.UserInfoId = userInfoId;
            userdet.ObjectId = _siteid;
            userdet.ClientId =ClientId;
            userdet.SiteControlled = _siteControlled;
            userdet.UserType = _userType;
            userdet.ValidateNewSiteForExistingUser_V2(this.userData.DatabaseKey);
            return userdet;
        }

        public UserDetails ValidateRemoveSite(long _userInfoId, long _siteId, bool _siteControlled, string _userType,long ClientId)
        {
            UserDetails userdet = new UserDetails();
            userdet.UserInfoId = _userInfoId;
            userdet.ObjectId = _siteId;
            userdet.ClientId = ClientId;
            userdet.SiteControlled = _siteControlled;
            userdet.UserType = _userType;
            userdet.ValidateRemoveSiteForExistingUser_V2(this.userData.DatabaseKey);
            return userdet;
        }

        public bool UmPersonnelDelete(long personnelId, long userInfoId, long siteId, string userType, bool isSuperUser, long defaultSiteId, long ClientId)
        {
            try
            {
                Personnel personnel = new Personnel()
                {
                    PersonnelId = personnelId,
                    ClientId = ClientId
                };
                personnel.DeleteForAdmin(userData.DatabaseKey);

                UserPermission userPermission = new UserPermission()
                {
                    UserInfoId = userInfoId,
                    SiteId = siteId,
                    ClientId = ClientId
                };
                userPermission.DeleteBySiteAndUserForAdmin(userData.DatabaseKey);
                UserDetails userdet = new UserDetails();
                userdet.UserInfoId = userInfoId;
                userdet.ClientId = ClientId;
                userdet.DefaultSiteId = siteId;
                userdet.RetrieveSiteUserCountsForAdmin_V2(userData.DatabaseKey);

                DataContracts.Site site = new DataContracts.Site();
                site.ClientId = ClientId;
                site.SiteId = siteId;
                //------Update Site User Counts---------
                // RKL - 2025-Jan-23 - Begin
                // V2-1028 and V2-1113
                // Need to respect the excluded users list
                //
                site.RetrieveForAdmin(userData.DatabaseKey);
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
                site.UpdateForAdmin(this.userData.DatabaseKey);

                CreateEventLogForAdmin(userdet.UserInfoId, siteId, userdet.ClientId, EventStatusConstants.RemoveSite);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CreateEventLogForAdmin(long objId, long siteId, long ClientId, string eventVal = "")
        {

            UserEventLog log = new UserEventLog();
            log.ClientId = ClientId;
            log.SiteId = siteId;
            log.ObjectId = objId;
            log.Event = eventVal;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = string.Empty;
            log.SourceId = 0;
            log.CreateForAdmin(userData.DatabaseKey);
        }
        public List<SelectListItem> GetAllAssignedSites(long userInfoId,long ClientId)
        {
           DataContracts.Site site = new DataContracts.Site();
            site.ClientId = ClientId;
            site.AuthorizedUser = userInfoId;
            List<DataContracts.Site> obj_Site = site.RetrieveAllAssignedSiteByUserForAdmin(userData.DatabaseKey);
            var Sites = obj_Site.Select(x => new SelectListItem { Text = x.Name + "-" + x.Description, Value = x.SiteId.ToString() }).ToList();
            return Sites;
        }
        public List<DataModel> GetLookUpList_Craft(long ClientId, long DefautSiteId)
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            Craft craft = new Craft
            {
                ClientId = ClientId,
                SiteId = DefautSiteId
            };
           
            List<Craft> CraftList = craft.RetriveAllForSiteForAdmin(userData.DatabaseKey).Where(a => a.InactiveFlag == false).ToList();
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

        public int GetCountForAdmin(long userInfoId, long siteid,long ClientId)
        {
            DataContracts.Site site = new DataContracts.Site();
            site.ClientId = userData.DatabaseKey.User.ClientId;
            site.AuthorizedUser = userInfoId;
            site.SiteId = siteid;
            site.ClientId = ClientId;
            int count = 0;
            var sitebuyer = site.RetrieveDefauiltBuyerForAdmin(userData.DatabaseKey, site);
            if (sitebuyer != null && sitebuyer.Count > 0)
            {
                count = sitebuyer[0].DefaultBuyerCount;
            }
            return count;
        }
        #endregion

    }

}

