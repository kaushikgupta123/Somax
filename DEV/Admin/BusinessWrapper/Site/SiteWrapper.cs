using Admin.Common;
using Admin.Models.Client;
using Admin.Models.Site;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;

namespace Admin.BusinessWrapper.Site
{
    public class SiteWrapper
    {
        private DatabaseKey _dbKey;
        public readonly UserData _userData;

        List<string> errorMessage = new List<string>();
        public SiteWrapper(UserData userData)
        {
            this._userData = userData;
            _dbKey = _userData.DatabaseKey;
        }
        public UserData userData { get; set; }

        #region GetSite
        public List<SiteSearchModel> GetSiteGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, long siteid = 0, long Clientid = 0, string name = "", string addressCity = "", string addressState = "", string searchText = "")
        {
            SiteSearchModel SiteSearchModel;
            List<SiteSearchModel> SiteSearchModelList = new List<SiteSearchModel>();
            List<DataContracts.Site> siteList = new List<DataContracts.Site>();
            DataContracts.Site site = new DataContracts.Site();
            site.OrderbyColumn = orderbycol;
            site.OrderBy = orderDir;
            site.OffSetVal = skip;
            site.NextRow = length;
            site.SiteId = siteid;
            site.CustomClientId = Clientid;

            site.Name = name;
            site.AddressCity = addressCity;
            site.AddressState = addressState;
            site.SearchText = searchText;
            siteList = site.SiteRetrieveChunkSearchV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in siteList)
            {
                SiteSearchModel = new SiteSearchModel();
                SiteSearchModel.SiteId = item.SiteId;
                SiteSearchModel.Name = item.Name;
                SiteSearchModel.AddressCity = item.AddressCity;
                SiteSearchModel.AddressState = item.AddressState;
                SiteSearchModel.TotalCount = item.TotalCount;
                SiteSearchModelList.Add(SiteSearchModel);
            }

            return SiteSearchModelList;
        }

        #endregion
        
        public List<ClientModel> GetAllActiveClient()
        {
            DataContracts.Client client = new DataContracts.Client();
            var retData = client.RetrieveAllActiveClient(this.userData.DatabaseKey);
            ClientModel clientModel;
            List<ClientModel> ClientModelList = new List<ClientModel>();
            foreach (var item in retData)
            {
                clientModel = new ClientModel();
                clientModel.ClientId = item.ClientId;
                clientModel.Name = item.CompanyName;
                ClientModelList.Add(clientModel);
            }
            return ClientModelList;
        }

        #region SiteDetails
        public SiteModel GetSiteDetailsBySiteId(long clientId = 0, long SiteId = 0)
        {
            DataContracts.Site site = new DataContracts.Site
            {
                CustomClientId = clientId,
                SiteId = SiteId

            };
            site.RetrieveAllSiteBySiteIdV2(this.userData.DatabaseKey);
            SiteModel siteModel;
            siteModel = new SiteModel();
            siteModel.SiteId = site.SiteId;
            siteModel.ClientId = site.ClientId;
            siteModel.ClientName = site.ClientName;
            siteModel.Name = site.Name;
            siteModel.Description = site.Description;
            siteModel.Localization = site.Localization;
            siteModel.LocalizationName = site.LocalizationName;
            siteModel.UIConfigurationLocation = site.UIConfigurationLocation;
            siteModel.UIConfigurationCompany = site.UIConfigurationCompany;
            siteModel.Status = site.Status;
            siteModel.TimeZone = site.TimeZone;
            siteModel.TimeZoneName = site.TimeZoneName;
            siteModel.APM = site.APM;
            siteModel.CMMS = site.CMMS;
            siteModel.Sanitation = site.Sanitation;
            siteModel.PMLibrary = site.PMLibrary;
            siteModel.UsePunchOut = site.UsePunchOut;
            siteModel.Address1 = site.Address1;
            siteModel.Address2 = site.Address2;
            siteModel.Address3 = site.Address3;
            siteModel.AddressCity = site.AddressCity;
            siteModel.AddressState = site.AddressState;
            siteModel.AddressCountry = site.AddressCountry;
            siteModel.AddressPostCode = site.AddressPostCode;
            siteModel.MaxAppUsers = site.MaxAppUsers;
            siteModel.MaxWorkRequestUsers = site.MaxWorkRequestUsers;
            siteModel.MaxLimitedUsers = site.MaxLimitedUsers;
            siteModel.MaxSuperUsers = site.SuperUsers;
            siteModel.AppUsers = site.AppUsers;
            siteModel.WorkRequestUsers = site.WorkRequestUsers;
            siteModel.LimitedUsers = site.LimitedUsers;
            siteModel.SuperUsers = site.SuperUsers;
            siteModel.UpdateIndex = site.UpdateIndex;
            siteModel.ClientSiteControl = site.ClientSiteControl;
            return siteModel;
        }

        #endregion
        public CreatedLastUpdatedModel createdLastUpdatedModel(long _SiteId)
        {
            CreatedLastUpdatedModel _CreatedLastUpdatedModel = new CreatedLastUpdatedModel();
            DataContracts.Site site = new DataContracts.Site();
            site.SiteId = _SiteId;

            site.RetrieveCreateModifyDate(userData.DatabaseKey);
            _CreatedLastUpdatedModel.CreatedDateValue = site.CreateDate.ToString();
            _CreatedLastUpdatedModel.CreatedUserValue = site.CreateBy;
            _CreatedLastUpdatedModel.ModifyUserValue = site.ModifyBy;
            _CreatedLastUpdatedModel.ModifyDatevalue = site.ModifyDate.ToString();

            return _CreatedLastUpdatedModel;
        }


        #region Add and Edit
        public DataContracts.Site AddorEditSite(SiteVM objsite)
        {
            string emptyValue = string.Empty;
            SiteModel objSite = new SiteModel();
            List<string> errList = new List<string>();
            DataContracts.Site site = new DataContracts.Site();
            if (objsite.SiteModel.SiteId == 0)
            {

                DataContracts.Client client = new DataContracts.Client();
                client.ClientId = userData.DatabaseKey.Client.ClientId;
                client.CreatedClientId = objsite.SiteModel.ClientId;
                client.RetrieveBySomaxAdmin(userData.DatabaseKey);
                site.ClientId = objsite.SiteModel.ClientId;
                List<DataContracts.Site> siteList = null;               
                    siteList = site.RetrieveAllFromAdmin(this.userData.DatabaseKey, client.ConnectionString, client.ClientId);
                    if (siteList.Count >= client.MaxSites)
                    {
                        errList.Add(UtilityFunction.GetMessageFromResource("NoOfSiteExceedError", LocalizeResourceSetConstants.Global));         


                    }
                    List<DataContracts.Site> obj = siteList.FindAll(x => x.Name.ToLower() == objsite.SiteModel.Name.ToLower());
                    if (obj != null && obj.Count > 0)
                    {

                        errList.Add(UtilityFunction.GetMessageFromResource("DuplicateSiteError", LocalizeResourceSetConstants.Global));  
                    }               

                if (errList.Count == 0)
                {
                    site.Name = objsite.SiteModel.Name != null ? objsite.SiteModel.Name : emptyValue;
                    site.Description = objsite.SiteModel.Description != null ? objsite.SiteModel.Description : emptyValue;
                    site.LocalizationLanguage = objsite.SiteModel.Localization != null ? objsite.SiteModel.Localization : emptyValue;
                    site.UIConfigurationLocation = objsite.SiteModel.UIConfigurationLocation != null ? objsite.SiteModel.UIConfigurationLocation : emptyValue;
                    site.UIConfigurationCompany = objsite.SiteModel.UIConfigurationCompany != null ? objsite.SiteModel.UIConfigurationCompany : emptyValue;
                    site.Status = objsite.SiteModel.Status != null ? objsite.SiteModel.Status : emptyValue;
                    site.TimeZone = objsite.SiteModel.TimeZone != null ? objsite.SiteModel.TimeZone : emptyValue;
                    site.Address1 = objsite.SiteModel.Address1 != null ? objsite.SiteModel.Address1 : emptyValue;
                    site.Address2 = objsite.SiteModel.Address2 != null ? objsite.SiteModel.Address2 : emptyValue;
                    site.Address3 = objsite.SiteModel.Address3 != null ? objsite.SiteModel.Address3 : emptyValue;
                    site.AddressCity = objsite.SiteModel.AddressCity != null ? objsite.SiteModel.AddressCity : emptyValue;
                    site.AddressCountry = objsite.SiteModel.AddressCountry != null ? objsite.SiteModel.AddressCountry : emptyValue;
                    site.AddressState = objsite.SiteModel.AddressState != null ? objsite.SiteModel.AddressState : emptyValue;
                    site.AddressPostCode = objsite.SiteModel.AddressPostCode != null ? objsite.SiteModel.AddressPostCode : emptyValue;
                    site.MaxAppUsers = objsite.SiteModel.MaxAppUsers??0;
                    site.AppUsers = objsite.SiteModel.AppUsers;
                    site.MaxLimitedUsers = objsite.SiteModel.MaxLimitedUsers??0;
                    site.LimitedUsers = objsite.SiteModel.LimitedUsers;
                    site.MaxWorkRequestUsers = objsite.SiteModel.MaxWorkRequestUsers??0;
                    site.WorkRequestUsers = objsite.SiteModel.WorkRequestUsers;
                    site.MaxSuperUsers = objsite.SiteModel.MaxSuperUsers??0;
                    site.SuperUsers = objsite.SiteModel.SuperUsers;
                    site.APM = objsite.SiteModel.APM;
                    site.CMMS = objsite.SiteModel.CMMS;
                    site.PMLibrary = objsite.SiteModel.PMLibrary;
                    site.UsePunchOut = objsite.SiteModel.UsePunchOut;
                    site.Sanitation = objsite.SiteModel.Sanitation;
                    site.CreateFromAdmin(this.userData.DatabaseKey, client.ConnectionString);
                    if (site.SiteId > 0)
                    {
                        client.Sites = client.Sites + 1;
                        client.UpdateBySomaxAdmin(userData.DatabaseKey);

                        DataContracts.LookupList lookuplist = new DataContracts.LookupList()
                        {
                            ClientId = objsite.SiteModel.ClientId,
                            Culture = site.LocalizationCulture,
                            Language = site.LocalizationLanguage
                        };
                        userData.DatabaseKey.AccessingClientId = objsite.SiteModel.ClientId;
                        userData.DatabaseKey.IsAccessClientData = true;
                        lookuplist.Createtemplate(this.userData.DatabaseKey);
                    }
                }
                else
                {
                    site.ErrorMessages = errList;
                }

            }
            else
            {
                DataContracts.Client client = new DataContracts.Client();
                client.ClientId = userData.DatabaseKey.Client.ClientId;
                client.CreatedClientId = objsite.SiteModel.ClientId;
                client.RetrieveBySomaxAdmin(userData.DatabaseKey);

                site.ClientId = objsite.SiteModel.ClientId;
                site.SiteId = objsite.SiteModel.SiteId;
                site.Retrieve(userData.DatabaseKey);
                List<DataContracts.Site> siteList = site.RetrieveAllFromAdmin(this.userData.DatabaseKey, client.ConnectionString, client.ClientId);
                List<DataContracts.Site> obj = siteList.FindAll(x => (x.Name.ToLower() == objsite.SiteModel.Name.ToLower()) && (x.SiteId != objsite.SiteModel.SiteId));

                if (obj != null && obj.Count > 0)
                {
                    errList.Add(UtilityFunction.GetMessageFromResource("DuplicateSiteError", LocalizeResourceSetConstants.Global));
                }
                if (errList.Count == 0)
                {
                    site.ClientId = objsite.SiteModel.ClientId;
                    site.SiteId = objsite.SiteModel.SiteId;
                    site.Name = objsite.SiteModel.Name != null ? objsite.SiteModel.Name : emptyValue;
                    site.Description = objsite.SiteModel.Description != null ? objsite.SiteModel.Description : emptyValue;
                    site.LocalizationLanguage = objsite.SiteModel.Localization != null ? objsite.SiteModel.Localization : emptyValue;
                    site.UIConfigurationLocation = objsite.SiteModel.UIConfigurationLocation != null ? objsite.SiteModel.UIConfigurationLocation : emptyValue;
                    site.UIConfigurationCompany = objsite.SiteModel.UIConfigurationCompany != null ? objsite.SiteModel.UIConfigurationCompany : emptyValue;
                    site.Status = objsite.SiteModel.Status != null ? objsite.SiteModel.Status : emptyValue;
                    site.TimeZone = objsite.SiteModel.TimeZone != null ? objsite.SiteModel.TimeZone : emptyValue;
                    site.Address1 = objsite.SiteModel.Address1 != null ? objsite.SiteModel.Address1 : emptyValue;
                    site.Address2 = objsite.SiteModel.Address2 != null ? objsite.SiteModel.Address2 : emptyValue;
                    site.Address3 = objsite.SiteModel.Address3 != null ? objsite.SiteModel.Address3 : emptyValue;
                    site.AddressCity = objsite.SiteModel.AddressCity != null ? objsite.SiteModel.AddressCity : emptyValue;
                    site.AddressCountry = objsite.SiteModel.AddressCountry != null ? objsite.SiteModel.AddressCountry : emptyValue;
                    site.AddressState = objsite.SiteModel.AddressState != null ? objsite.SiteModel.AddressState : emptyValue;
                    site.AddressPostCode = objsite.SiteModel.AddressPostCode != null ? objsite.SiteModel.AddressPostCode : emptyValue;
                    site.MaxAppUsers = objsite.SiteModel.MaxAppUsers??0;                   
                    site.MaxLimitedUsers = objsite.SiteModel.MaxLimitedUsers??0;                  
                    site.MaxWorkRequestUsers = objsite.SiteModel.MaxWorkRequestUsers??0;                
                    site.MaxSuperUsers = objsite.SiteModel.MaxSuperUsers??0;                 
                    site.APM = objsite.SiteModel.APM;
                    site.CMMS = objsite.SiteModel.CMMS;
                    site.PMLibrary = objsite.SiteModel.PMLibrary;
                    site.UsePunchOut = objsite.SiteModel.UsePunchOut;
                    site.Sanitation = objsite.SiteModel.Sanitation;
                    site.UpdateIndex = objsite.SiteModel.UpdateIndex;
                    site.UpdateFromAdmin(this.userData.DatabaseKey, client.ConnectionString);
                    if (site.SiteId > 0)
                    {
                        DataContracts.LookupList lookuplist = new DataContracts.LookupList()
                        {
                            ClientId = objsite.SiteModel.ClientId,
                            Culture = site.LocalizationCulture,
                            Language = site.LocalizationLanguage
                        };
                        userData.DatabaseKey.AccessingClientId = objsite.SiteModel.ClientId;
                        userData.DatabaseKey.IsAccessClientData = true;
                        lookuplist.Createtemplate(this.userData.DatabaseKey);

                    }
                }
                else
                {
                    site.ErrorMessages = errList;
                }

            }

            return site;
        }
        #endregion

        #region V2-964
        public SiteModelView GetSiteDetailsByClientIdSiteId(long ClientId = 0, long SiteId = 0)
        {
            DataContracts.Site site = new DataContracts.Site()
            {
                CustomClientId = ClientId,
                SiteId = SiteId,
            };
            site.RetrieveSiteByClientIdSiteId_V2(_dbKey);

            SiteModelView siteModelView = new SiteModelView();
            siteModelView.SiteId = site.SiteId;
            siteModelView.ClientId=site.ClientId;
            siteModelView.Name = site.Name;
            siteModelView.Description = site.Description;
            siteModelView.TimeZone = site.TimeZone;
            siteModelView.APM = site.APM;
            siteModelView.CMMS = site.CMMS;
            siteModelView.Sanitation = site.Sanitation;
            siteModelView.Production = site.Production;
            siteModelView.Status = site.Status;
            if (site.CreateDate != null && site.CreateDate == default(DateTime))
            {
                siteModelView.CreateDate = null;
            }
            else
            {
                siteModelView.CreateDate = site.CreateDate;
            }
            siteModelView.UsePunchOut = site.UsePunchOut;
            siteModelView.AppUsers = site.AppUsers;
            siteModelView.MaxAppUsers = site.MaxAppUsers;
            siteModelView.WorkRequestUsers = site.WorkRequestUsers;
            siteModelView.MaxWorkRequestUsers = site.MaxWorkRequestUsers;
            siteModelView.SanitationUsers = site.SanitationUsers;
            siteModelView.MaxSanitationUsers = site.MaxSanitationUsers;
            siteModelView.ProdAppUsers = site.ProdAppUsers;
            siteModelView.MaxProdAppUsers = site.MaxProdAppUsers;
            siteModelView.UpdateIndex = site.UpdateIndex;
            return siteModelView;
        }
        #endregion

        #region V2-1178
        public long ValidateUserExist(SiteVM siteVM)
        {
            UserDetails _userdetails = new UserDetails();
            _userdetails.ClientId = siteVM.UserValidateModel.ClientId;
            _userdetails.SiteId = siteVM.UserValidateModel.SiteId;
            _userdetails.UserName = siteVM.UserValidateModel.UserName;
            var logininfo = _userdetails.RetrieveValidateUserExists(_dbKey);
            long LoginInfoId = logininfo.Count > 0 ? logininfo[0].LoginInfoId : 0;
            return LoginInfoId;
        }
        #endregion
    }
}