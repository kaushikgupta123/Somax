using Business.Authentication;
using Client.Models;
using Client.Models.ChangeActiveSite;
using Common.Constants;
using Common.Enumerations;
using DataContracts;
using Presentation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.BusinessWrapper.ActiveSiteWrapper
{
    public class ActiveSiteWrapper
    {
        private DatabaseKey _dbKey;
        private UserData _userData;

        public ActiveSiteWrapper(UserData userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        public List<SelectListItem> GetSites()
        {
            Site site = new Site();
            site.ClientId = _userData.DatabaseKey.Personnel.ClientId;
            site.AuthorizedUser = _userData.DatabaseKey.User.UserInfoId;
            // SOM-851
            List<Site> obj_Site = site.RetrieveAuthorizedForUser(_userData.DatabaseKey);

            var Sites = obj_Site.Select(x => new SelectListItem { Text = x.Name + "-" + x.Description, Value = x.SiteId.ToString() }).ToList();
            return Sites;
        }
        public DataContracts.Personnel UpdateSite(ChangeActiveSiteModel model)
        {
            DataContracts.Personnel personnel = new DataContracts.Personnel();

            RetrievePageControls(personnel);
            personnel.SiteId = model.ChangeSiteSiteId;
           long LoginSSOId=  this._userData.DatabaseKey.Personnel.LoginSSOId;
            personnel.UpdateForMultiUserSite(this._userData.DatabaseKey);
            PopulateUserDate(LoginSSOId);
            return personnel;
        }
        protected void RetrievePageControls(DataContracts.Personnel personal)
        {
            DataContracts.UserInfo userInfo = new DataContracts.UserInfo();
            userInfo.UserInfoId = this._userData.DatabaseKey.User.UserInfoId;
            userInfo.Retrieve(this._userData.DatabaseKey);

            personal.ClientId = this._userData.DatabaseKey.User.ClientId;
            personal.UserInfoId = this._userData.DatabaseKey.User.UserInfoId;
            personal.CallerUserName = this._userData.DatabaseKey.User.UserName;
            personal.NameFirst = this._userData.DatabaseKey.User.FirstName;
            personal.NameLast = this._userData.DatabaseKey.User.LastName;
            personal.NameMiddle = this._userData.DatabaseKey.User.MiddleName;
            personal.ClientLookupId = this._userData.DatabaseKey.Personnel.ClientLookupId;
            personal.UpdateIndex = Convert.ToInt32(userInfo.UpdateIndex);
        }
        protected void PopulateUserDate(long LoginSSOId)
        {
            string userName = string.Empty;
            string password = string.Empty;
            if (System.Web.HttpContext.Current.Session[SessionConstants.LOGGED_USERNAME] != null)
            {
                userName = System.Web.HttpContext.Current.Session[SessionConstants.LOGGED_USERNAME] as string;
                //System.Web.HttpContext.Current.Session[SessionConstants.LOGGED_USERNAME] = null;
            }
            if (System.Web.HttpContext.Current.Session[SessionConstants.LOGGED_PASSWORD] != null)
            {
                password = System.Web.HttpContext.Current.Session[SessionConstants.LOGGED_PASSWORD] as string;
                //System.Web.HttpContext.Current.Session[SessionConstants.LOGGED_PASSWORD] = null;
            }
            Authentication auth = new Authentication()
            {
                UserName = userName,
                Password = password,
                website = WebSiteEnum.Client,
                BrowserInfo = HttpContext.Current.Request.Browser.Type + " " + HttpContext.Current.Request.Browser.Version,
                IpAddress = HttpContext.Current.Request.UserHostAddress
            };          
            if(LoginSSOId> 0)
            {             
                auth.VerifyLoginSSO(this._userData.DatabaseKey.Personnel.UserInfoId);
              
            }
            else
            {              
                auth.VerifyLogin();       
            }
         

            if (auth.IsAuthenticated)
            {
                // store the newly created session id into cookie
                Presentation.Common.Cookie.Set(CookiesConstants.SOMAX_USER, auth.SessionId.ToString());

                // store the newly created session id into session
                UserSession.SessionId = auth.SessionId;

                DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
                auth.UserData = new UserData() { SessionId = auth.SessionId, WebSite = WebSiteEnum.Client };
                auth.UserData.Retrieve(dbKey);
                this._userData = auth.UserData;
                 this._userData.DatabaseKey.Personnel.LoginSSOId=LoginSSOId;
                System.Web.HttpContext.Current.Session["userData"] = auth.UserData;
            }
        }

        #region V2-419 Enterprise User Management - Add/Remove Sites
        public List<SelectListItem> GetAllAssignedSites(long userInfoId)
        {
            Site site = new Site();
            site.ClientId = _userData.DatabaseKey.Personnel.ClientId;
            site.AuthorizedUser = userInfoId;
            List<Site> obj_Site = site.RetrieveAllAssignedSiteByUser(_userData.DatabaseKey);
            var Sites = obj_Site.Select(x => new SelectListItem { Text = x.Name + "-" + x.Description, Value = x.SiteId.ToString() }).ToList();
            return Sites;
        }

        public int GetCount(long userInfoId,long siteid)
        {
            Site site = new Site();
            site.ClientId = _userData.DatabaseKey.User.ClientId;
            site.AuthorizedUser = userInfoId;
            site.SiteId = siteid;
            int count = 0;
            var sitebuyer = site.RetrieveDefauiltBuyer(_userData.DatabaseKey, site);
            if (sitebuyer != null && sitebuyer.Count > 0)
            {
                count = sitebuyer[0].DefaultBuyerCount;
            }
            return count;        
        }
        #endregion
    }
}