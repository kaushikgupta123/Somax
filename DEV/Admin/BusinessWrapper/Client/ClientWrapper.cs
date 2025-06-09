using Admin.Common;
using Admin.Models;
using Admin.Models.Client;
using Admin.Models.ClientMessages;
using Admin.Models.Site;
using Common.Constants;
using Common.Extensions;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;

using Unity.Injection;

namespace Admin.BusinessWrapper.Client
{
    public class ClientWrapper
    {
        private DatabaseKey _dbKey;
        public readonly UserData _userData;

        List<string> errorMessage = new List<string>();
        public ClientWrapper(UserData userData)
        {
            this._userData = userData;
            _dbKey = _userData.DatabaseKey;
        }
        public UserData userData { get; set; }

        #region Search

        public List<ClientSearchModel> GetClientGridData(int CustomQueryDisplayId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, long Clientid = 0, string name = "", string Contact = "", string Email = "", string searchText = "")
        {
            ClientSearchModel ClientSearchModel;
            List<ClientSearchModel> ClientSearchModelList = new List<ClientSearchModel>();
            List<DataContracts.Client> clientList = new List<DataContracts.Client>();
            DataContracts.Client client = new DataContracts.Client();
            client.CustomQueryDisplayId = CustomQueryDisplayId;
            client.OrderbyColumn = orderbycol;
            client.OrderBy = orderDir;
            client.OffSetVal = skip;
            client.NextRow = length;
            client.CreatedClientId = Clientid;
            client.Name = name;
            client.Contact = Contact;
            client.Email = Email;
            client.SearchText = searchText;
            clientList = client.ClientRetrieveChunkSearchV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in clientList)
            {
                ClientSearchModel = new ClientSearchModel();
                ClientSearchModel.ClientId = item.ClientId;
                ClientSearchModel.Name = item.Name;
                ClientSearchModel.Contact = item.Contact;
                ClientSearchModel.Email = item.Email;
                ClientSearchModel.BusinessType = item.BusinessType;
                ClientSearchModel.PackageLevel = item.PackageLevel;
                ClientSearchModel.CreateDate = item.CreateDate;
                ClientSearchModel.Status = item.Status;
                ClientSearchModel.TotalCount = item.TotalCount;
                ClientSearchModelList.Add(ClientSearchModel);
            }

            return ClientSearchModelList;
        }

        #endregion

        #region Details
        public ClientModel GetClientDetailsById(long ClientId)
        {
            ClientModel ClientDetails = new ClientModel();
            DataContracts.Client client = new DataContracts.Client()
            {
                CreatedClientId = ClientId,
            };
            client.RetrieveBySomaxAdmin_V2(_dbKey);

            ClientDetails = initializeDetailsControls(client);

            return ClientDetails;
        }


        public ClientModel initializeDetailsControls(DataContracts.Client obj)
        {
            ClientModel objclient = new ClientModel();
            objclient.ClientId = obj.ClientId;
            objclient.Name = obj.CompanyName;
            objclient.Contact = obj.PrimaryContact;
            objclient.Email = obj.Email;
            objclient.BusinessType = obj.BusinessType;
            objclient.PackageLevel = obj.PackageLevel;
            objclient.CreateDate = obj.CreateDate;
            objclient.Status = obj.Status;
            objclient.LegalName = obj.LegalName;
            objclient.NumberOfEmployees = obj.NumberOfEmployees;
            objclient.AnnualSales = obj.AnnualSales;
            objclient.TaxIDNumber = obj.TaxIDNumber;
            objclient.VATNumber = obj.VATNumber;
            objclient.Website = obj.Website;
            objclient.DateEstablished = obj.DateEstablished;
            objclient.NumberOfLocations = obj.NumberOfLocations;
            objclient.OfficerName = obj.OfficerName;
            objclient.OfficerPhone = obj.OfficerPhone;
            objclient.DunnsNumber = obj.DunnsNumber;
            objclient.AppUsers = obj.AppUsers;
            objclient.MaxAppUsers = obj.MaxAppUsers;
            objclient.LimitedUsers = obj.LimitedUsers;
            objclient.MaxLimitedUsers = obj.MaxLimitedUsers;
            objclient.PhoneUsers = obj.PhoneUsers;
            objclient.MaxPhoneUsers = obj.MaxPhoneUsers;
            objclient.WorkRequestUsers = obj.WorkRequestUsers;
            objclient.MaxWorkRequestUsers = obj.MaxWorkRequestUsers;
            objclient.SiteControl = obj.SiteControl;
            objclient.Purchasing = obj.Purchasing;
            objclient.Sanitation = obj.Sanitation;
            objclient.SanitationUsers = obj.SanitationUsers;
            objclient.MaxSanitationUsers = obj.MaxSanitationUsers;
            objclient.SuperUsers = obj.SuperUsers;
            objclient.MaxSuperUsers = obj.MaxSuperUsers;
            objclient.PrimarySICCode = obj.PrimarySICCode;
            objclient.NAICSCode = obj.NAICSCode;
            objclient.Sites = obj.Sites;
            objclient.MaxSites = obj.MaxSites;
            objclient.MinorityStatus = obj.MinorityStatus;
            objclient.Localization = obj.LocalizationLanguageAndCulture;
            objclient.LocalizationLocation = obj.LocalizationLocation;
            objclient.LocalizationCompany = obj.LocalizationCompany;
            objclient.LocalizationHierarchicalLevel1 = obj.LocalizationHierarchicalLevel1;
            objclient.LocalizationHierarchicalLevel2 = obj.LocalizationHierarchicalLevel2;
            objclient.DefaultTimeZone = obj.DefaultTimeZone;
            objclient.DefaultCustomerManager = obj.DefaultCustomerManager;
            objclient.MaxAttempts = obj.MaxAttempts;
            objclient.MaxTimeOut = obj.MaxTimeOut;
            objclient.ConnectionString = obj.ConnectionString;
            objclient.TabletUsers = obj.TabletUsers;
            objclient.MaxTabletUsers = obj.MaxTabletUsers;
            objclient.UIConfiguration = obj.UIConfigurationCompany;
            objclient.UIConfigurationLocation = obj.UIConfigurationLocation;
            objclient.UIConfigurationHierarchicalLevel1 = obj.UIConfigurationHierarchicalLevel1;
            objclient.UIConfigurationHierarchicalLevel2 = obj.UIConfigurationHierarchicalLevel2;
            objclient.WOPrintMessage = obj.WOPrintMessage;
            objclient.PMLibCopy = obj.PMLibCopy;
            objclient.AssetTree = obj.AssetTree;
            objclient.ProdAppUsers = obj.ProdAppUsers;
            objclient.MaxProdAppUsers = obj.MaxProdAppUsers;
            objclient.UpdateIndex = obj.UpdateIndex;
            objclient.TimeZoneName = obj.TimeZoneName;

            return objclient;
        }
        #endregion

        #region Add and Edit
        public DataContracts.Client AddorEditClient(ClientVM objclient)
        {
            ClientModel objClient = new ClientModel();

            DataContracts.Client client = new DataContracts.Client();
            if (objclient.ClientModel.ClientId == 0)
            {
                client.CompanyName = objclient.ClientModel.Name;
                client.LegalName = objclient.ClientModel.LegalName;
                client.PrimaryContact = objclient.ClientModel.Contact;
                client.Email = objclient.ClientModel.Email;
                client.BusinessType = objclient.ClientModel.BusinessType;
                client.Website = objclient.ClientModel.Website;
                client.AnnualSales = objclient.ClientModel.AnnualSales ?? 0;
                client.LocalizationLanguageAndCulture = objclient.ClientModel.Localization ?? "";
                client.LocalizationLocation = objclient.ClientModel.LocalizationLocation ?? "";
                client.LocalizationCompany = objclient.ClientModel.LocalizationCompany ?? "";
                client.LocalizationHierarchicalLevel1 = objclient.ClientModel.LocalizationHierarchicalLevel1 ?? "";
                client.LocalizationHierarchicalLevel2 = objclient.ClientModel.LocalizationHierarchicalLevel2 ?? "";
                client.UIConfigurationCompany = objclient.ClientModel.UIConfiguration;
                client.UIConfigurationLocation = objclient.ClientModel.UIConfigurationLocation ?? "";
                client.UIConfigurationHierarchicalLevel1 = objclient.ClientModel.UIConfigurationHierarchicalLevel1 ?? "";
                client.UIConfigurationHierarchicalLevel2 = objclient.ClientModel.UIConfigurationHierarchicalLevel2 ?? "";
                client.ConnectionString = objclient.ClientModel.ConnectionString;
                client.DefaultCustomerManager = objclient.ClientModel.DefaultCustomerManager;
                client.DefaultTimeZone = objclient.ClientModel.DefaultTimeZone;
                client.PackageLevel = objclient.ClientModel.PackageLevel;
                client.Status = objclient.ClientModel.Status;
                client.MaxAttempts = objclient.ClientModel.MaxAttempts ?? 0;
                client.MaxTimeOut = objclient.ClientModel.MaxTimeOut ?? 0;
                client.SiteControl = objclient.ClientModel.SiteControl;
                client.MaxSites = objclient.ClientModel.MaxSites ?? 0;
                if (!objclient.ClientModel.SiteControl)
                {

                    client.MaxAppUsers = objclient.ClientModel.MaxAppUsers ?? 0;
                    client.MaxWorkRequestUsers = objclient.ClientModel.MaxWorkRequestUsers ?? 0;
                    client.MaxLimitedUsers = objclient.ClientModel.MaxLimitedUsers ?? 0;
                    client.MaxSuperUsers = objclient.ClientModel.MaxSuperUsers ?? 0;
                }
                if (client.ErrorMessages == null || client.ErrorMessages.Count == 0)

                {

                    client.CreateBySomaxAdmin(this._userData.DatabaseKey);
                }
            }

            else
            {
                client.CreatedClientId = objclient.ClientModel.ClientId;
                client.RetrieveBySomaxAdmin(_dbKey);
                client.CompanyName = objclient.ClientModel.Name ?? "";
                client.LegalName = objclient.ClientModel.LegalName ?? "";
                client.PrimaryContact = objclient.ClientModel.Contact ?? "";
                client.Email = objclient.ClientModel.Email ?? "";
                client.BusinessType = objclient.ClientModel.BusinessType ?? "";
                client.Website = objclient.ClientModel.Website ?? "";
                client.AnnualSales = objclient.ClientModel.AnnualSales ?? 0;
                client.LocalizationLanguageAndCulture = objclient.ClientModel.Localization ?? "";
                client.LocalizationLocation = objclient.ClientModel.LocalizationLocation ?? "";
                client.LocalizationCompany = objclient.ClientModel.LocalizationCompany ?? "";
                client.LocalizationHierarchicalLevel1 = objclient.ClientModel.LocalizationHierarchicalLevel1 ?? "";
                client.LocalizationHierarchicalLevel2 = objclient.ClientModel.LocalizationHierarchicalLevel2 ?? "";
                client.UIConfigurationCompany = objclient.ClientModel.UIConfiguration ?? "";
                client.UIConfigurationLocation = objclient.ClientModel.UIConfigurationLocation ?? "";
                client.UIConfigurationHierarchicalLevel1 = objclient.ClientModel.UIConfigurationHierarchicalLevel1 ?? "";
                client.UIConfigurationHierarchicalLevel2 = objclient.ClientModel.UIConfigurationHierarchicalLevel2 ?? "";
                client.ConnectionString = objclient.ClientModel.ConnectionString ?? "";
                client.DefaultCustomerManager = objclient.ClientModel.DefaultCustomerManager ?? "";
                client.DefaultTimeZone = objclient.ClientModel.DefaultTimeZone ?? "";
                client.PackageLevel = objclient.ClientModel.PackageLevel ?? "";
                client.Status = objclient.ClientModel.Status;
                client.MaxAttempts = objclient.ClientModel.MaxAttempts ?? 0;
                client.MaxTimeOut = objclient.ClientModel.MaxTimeOut ?? 0;
                client.SiteControl = objclient.ClientModel.SiteControl;
                client.MaxSites = objclient.ClientModel.MaxSites ?? 0;
                if (!objclient.ClientModel.SiteControl)
                {
                    client.MaxAppUsers = objclient.ClientModel.MaxAppUsers ?? 0;
                    client.MaxWorkRequestUsers = objclient.ClientModel.MaxWorkRequestUsers ?? 0;
                    client.MaxLimitedUsers = objclient.ClientModel.MaxLimitedUsers ?? 0;
                    client.MaxSuperUsers = objclient.ClientModel.MaxSuperUsers ?? 0;
                }

                if (client.ErrorMessages == null || client.ErrorMessages.Count == 0)

                {
                    client.UpdateBySomaxAdmin(this._userData.DatabaseKey);

                }
            }
            return client;
        }

        public int CountIfUserExist(ClientModel objclient)
        {
            DataContracts.Client _client = new DataContracts.Client();
            _client.CreatedClientId = objclient.ClientId;
            _client.Name = objclient.Name;
            var cnt = _client.RetrieveCountForUserExist(_userData.DatabaseKey);
            int count = cnt.Count;
            return count;
        }
        #endregion

        #region V2-964
        #region Search
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
        public List<ClientSearchModel> GetClientGridDataNew(int CustomQueryDisplayId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, long Clientid = 0, long Siteid = 0, string name = "", string Contact = "", string Email = "", string searchText = "")
        {
            ClientSearchModel ClientSearchModel;
            List<ClientSearchModel> ClientSearchModelList = new List<ClientSearchModel>();
            List<DataContracts.Client> clientList = new List<DataContracts.Client>();
            DataContracts.Client client = new DataContracts.Client();
            client.CustomQueryDisplayId = CustomQueryDisplayId;
            client.OrderbyColumn = orderbycol;
            client.OrderBy = orderDir;
            client.OffSetVal = skip;
            client.NextRow = length;
            client.CreatedClientId = Clientid;
            client.SiteId = Siteid;
            client.Name = name;
            client.Contact = Contact;
            client.Email = Email;
            client.SearchText = searchText;
            clientList = client.ClientRetrieveChunkSearchV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in clientList)
            {
                ClientSearchModel = new ClientSearchModel();
                ClientSearchModel.ClientId = item.ClientId;
                ClientSearchModel.Name = item.Name;
                ClientSearchModel.Contact = item.Contact;
                ClientSearchModel.Email = item.Email;
                ClientSearchModel.BusinessType = item.BusinessType;
                ClientSearchModel.PackageLevel = item.PackageLevel;
                ClientSearchModel.CreateDate = item.CreateDate;
                ClientSearchModel.Status = item.Status;
                ClientSearchModel.ChildCount = item.ChildCount;
                ClientSearchModel.TotalCount = item.TotalCount;
                ClientSearchModelList.Add(ClientSearchModel);
            }

            return ClientSearchModelList;
        }
        public List<ChildGridModel> PopulateChilditems(long ClientId)
        {
            ChildGridModel objChildGrid;
            List<ChildGridModel> ChildItemList = new List<ChildGridModel>();

            DataContracts.Site site = new DataContracts.Site()
            {
                CustomClientId = ClientId
            };
            List<DataContracts.Site> siteList = site.RetrieveSitesForClientChildGrid(_dbKey);
            if (siteList != null)
            {
                foreach(var item in siteList)
                {
                    objChildGrid = new ChildGridModel();
                    objChildGrid.SiteId = item.SiteId;
                    objChildGrid.ClientId = item.ClientId;
                    objChildGrid.Name = item.Name;
                    objChildGrid.Status = item.Status;
                    objChildGrid.APM = item.APM;
                    objChildGrid.CMMS = item.CMMS;
                    objChildGrid.Sanitation = item.Sanitation;
                    objChildGrid.UpdateIndex = item.UpdateIndex;
                    ChildItemList.Add(objChildGrid);
                }
            }
            return ChildItemList;
        }
        #endregion
        #region Add OR Edit
        public CreateClientModel GetClientDetailsForEdit(long ClientId)
        {
            CreateClientModel objClient = new CreateClientModel();
            DataContracts.Client client = new DataContracts.Client()
            {
                CreatedClientId = ClientId,
            };
            client.RetrieveBySomaxAdmin_V2(_dbKey);

            objClient = initializeDetailsControlsForEdit(client);

            return objClient;
        }
        public CreateClientModel initializeDetailsControlsForEdit(DataContracts.Client obj)
        {
            CreateClientModel objclient = new CreateClientModel();
            objclient.ClientId = obj.ClientId;
            objclient.Name = obj.CompanyName;
            objclient.LegalName = obj.LegalName;
            objclient.PrimaryContact = obj.PrimaryContact;
            objclient.Email = obj.Email;
            objclient.BusinessType = obj.BusinessType;
            objclient.PackageLevel = obj.PackageLevel;
            if (obj.MaxAppUsers > 0)
            {
                objclient.MaxAppUsers = obj.MaxAppUsers;
            }
            if (obj.MaxWorkRequestUsers > 0)
            {
                objclient.MaxWorkRequestUsers = obj.MaxWorkRequestUsers;
            }
            if (obj.MaxSanitationUsers > 0)
            {
                objclient.MaxSanitationUsers = obj.MaxSanitationUsers;
            }
            if (obj.MaxProdAppUsers > 0)
            {
                objclient.MaxProdAppUsers = obj.MaxProdAppUsers;
            }
            if (obj.MaxAttempts > 0)
            {
                objclient.MaxAttempts = obj.MaxAttempts;
            }
            if (obj.MaxTimeOut > 0)
            {
                objclient.MaxTimeOut = obj.MaxTimeOut;
            }
            objclient.SiteControl = obj.SiteControl;
            objclient.MaxSites = obj.MaxSites;
            objclient.ConnectionString = obj.ConnectionString;

            return objclient;
        }
        public DataContracts.Client SaveClient(ClientVM objclient)
        {
            DataContracts.Client client = new DataContracts.Client();
            if (objclient.CreateClientModel.ClientId == 0)
            {
                client.CompanyName = objclient.CreateClientModel.Name;
                client.LegalName = objclient.CreateClientModel.LegalName;
                client.PrimaryContact = objclient.CreateClientModel.PrimaryContact;
                client.Email = objclient.CreateClientModel.Email;
                client.BusinessType = objclient.CreateClientModel.BusinessType;
                client.LocalizationLanguageAndCulture = objclient.CreateClientModel.Localization ?? "";
                client.PackageLevel = objclient.CreateClientModel.PackageLevel;
                client.SiteControl = objclient.CreateClientModel.SiteControl;
                client.MaxSites = objclient.CreateClientModel.MaxSites ?? 0;
                client.MaxAppUsers = objclient.CreateClientModel.MaxAppUsers ?? 0;
                client.MaxSanitationUsers = objclient.CreateClientModel.MaxSanitationUsers ?? 0;
                client.MaxWorkRequestUsers = objclient.CreateClientModel.MaxWorkRequestUsers ?? 0;

                client.CreateBySomaxAdminV2(this._userData.DatabaseKey);
                if(client.ErrorMessages == null || client.ErrorMessages.Count == 0)
                {
                    DataContracts.Site site = new DataContracts.Site();
                    site.ClientId = client.CreatedClientId;
                    site.Name = objclient.CreateClientModel.SiteName;
                    site.Description = objclient.CreateClientModel.Site_Description;
                    site.Localization = objclient.CreateClientModel.Localization ?? "";
                    site.TimeZone = objclient.CreateClientModel.TimeZone;
                    site.APM=objclient.CreateClientModel.APM;
                    site.CMMS=objclient.CreateClientModel.CMMS;
                    site.Sanitation=objclient.CreateClientModel.Sanitation;
                    site.MaxAppUsers = objclient.CreateClientModel.Site_MaxAppUsers ?? 0;
                    site.MaxSanitationUsers = objclient.CreateClientModel.Site_MaxSanitationUsers ?? 0;
                    client.RetrieveBySomaxAdmin_V2(this._dbKey);
                    site.CreateFromAdminClient(this._userData.DatabaseKey, client.ConnectionString);
                }
            }
            else
            {
                client.CreatedClientId = objclient.CreateClientModel.ClientId;
                client.RetrieveBySomaxAdmin(_dbKey);
                client.CompanyName = objclient.CreateClientModel.Name ?? "";
                client.PrimaryContact = objclient.CreateClientModel.PrimaryContact ?? "";
                client.Email = objclient.CreateClientModel.Email ?? "";
                client.BusinessType = objclient.CreateClientModel.BusinessType ?? "";
                client.ConnectionString = objclient.CreateClientModel.ConnectionString ?? "";
                client.PackageLevel = objclient.CreateClientModel.PackageLevel ?? "";
                client.MaxAttempts = objclient.CreateClientModel.MaxAttempts ?? 0;
                client.MaxTimeOut = objclient.CreateClientModel.MaxTimeOut ?? 0;
                client.SiteControl = objclient.CreateClientModel.SiteControl;
                client.MaxSites = objclient.CreateClientModel.MaxSites ?? 0;
                client.MaxAppUsers = objclient.CreateClientModel.MaxAppUsers ?? 0;
                client.MaxWorkRequestUsers = objclient.CreateClientModel.MaxWorkRequestUsers ?? 0;
                client.MaxSanitationUsers = objclient.CreateClientModel.MaxSanitationUsers ?? 0;
                client.MaxProdAppUsers = objclient.CreateClientModel.MaxProdAppUsers ?? 0;
                client.UpdateBySomaxAdmin(this._userData.DatabaseKey);
            }
            return client;
        }
        #endregion

        #region Active Inactive Client
        public List<string> MakeActiveInactiveClient(long ClientId, bool InActiveFlag)
        {
            DataContracts.Client client = new DataContracts.Client();
            client.CreatedClientId = ClientId;
            client.RetrieveBySomaxAdmin(_dbKey);
            if (!InActiveFlag)
            {
                client.Status = "Active";
            }
            else
            {
                client.Status = "Inactive";
            }
            client.UpdateBySomaxAdmin(this._userData.DatabaseKey);
            if (client.ErrorMessages == null || client.ErrorMessages.Count == 0)
            {
                ClientEventLog clientEventLog = new ClientEventLog()
                {
                    ClientId = ClientId,
                    SiteId = 0
                };
                clientEventLog.TransactionDate = DateTime.UtcNow;
                if (!InActiveFlag)
                {
                    clientEventLog.Event = "ActivateClient";
                }
                else
                {
                    clientEventLog.Event = "InactClient";
                }
                clientEventLog.CreateFromAdmin(_dbKey);
            }
            return client.ErrorMessages;
        }
        #endregion

        #region Details
        public ClientModel GetClientDetailsByClientId(long ClientId)
        {
            ClientModel ClientDetails = new ClientModel();
            DataContracts.Client client = new DataContracts.Client()
            {
                CreatedClientId = ClientId,
            };
            client.RetrieveBySomaxAdmin_V2(_dbKey);
            ClientDetails = initializeClientDetailsControlsByClientId(client);
            return ClientDetails;
        }
        public ClientModel initializeClientDetailsControlsByClientId(DataContracts.Client obj)
        {
            ClientModel objclient = new ClientModel();
            objclient.ClientId = obj.ClientId;
            objclient.Name = obj.CompanyName;
            objclient.LegalName = obj.LegalName;
            objclient.Contact = obj.PrimaryContact;
            objclient.NumberOfEmployees = obj.NumberOfEmployees;
            objclient.AnnualSales = obj.AnnualSales;
            objclient.TaxIDNumber = obj.TaxIDNumber;
            objclient.VATNumber = obj.VATNumber;
            objclient.Email = obj.Email;
            objclient.Website = obj.Website;
            objclient.Status = obj.Status;
            objclient.BusinessType = obj.BusinessType;
            objclient.DateEstablished = obj.DateEstablished;
            objclient.NumberOfLocations = obj.NumberOfLocations;
            objclient.OfficerName = obj.OfficerName;
            objclient.OfficerTitle = obj.OfficerTitle;
            objclient.OfficerPhone = obj.OfficerPhone;
            objclient.DunnsNumber = obj.DunnsNumber;
            objclient.PackageLevel = obj.PackageLevel;
            objclient.AppUsers = obj.AppUsers;
            objclient.MaxAppUsers = obj.MaxAppUsers;
            objclient.MaxLimitedUsers = obj.MaxLimitedUsers;
            objclient.LimitedUsers = obj.LimitedUsers;
            objclient.PhoneUsers = obj.PhoneUsers;
            objclient.MaxPhoneUsers = obj.MaxPhoneUsers;
            objclient.WorkRequestUsers = obj.WorkRequestUsers;
            objclient.MaxWorkRequestUsers = obj.MaxWorkRequestUsers;
            objclient.SiteControl = obj.SiteControl;
            objclient.Purchasing = obj.Purchasing;
            objclient.Sanitation = obj.Sanitation;
            objclient.SanitationUsers = obj.SanitationUsers;
            objclient.MaxSanitationUsers = obj.MaxSanitationUsers;
            objclient.SuperUsers = obj.SuperUsers;
            objclient.MaxSuperUsers = obj.MaxSuperUsers;
            objclient.PrimarySICCode = obj.PrimarySICCode;
            objclient.NAICSCode = obj.NAICSCode;
            objclient.Sites = obj.Sites;
            objclient.MaxSites = obj.MaxSites;
            objclient.MinorityStatus = obj.MinorityStatus;
            objclient.Localization = obj.Localization;
            objclient.DefaultTimeZone = obj.DefaultTimeZone;
            objclient.DefaultCustomerManager = obj.DefaultCustomerManager;
            objclient.MaxAttempts = obj.MaxAttempts;
            objclient.MaxTimeOut = obj.MaxTimeOut;
            objclient.ConnectionString = obj.ConnectionString;
            objclient.TabletUsers = obj.TabletUsers;
            objclient.MaxTabletUsers = obj.MaxTabletUsers;
            objclient.UIConfiguration = obj.UIConfiguration;
            objclient.WOPrintMessage = obj.WOPrintMessage;
            objclient.PurchaseTermsandConds = obj.PurchaseTermsandConds;
            objclient.UpdateIndex = obj.UpdateIndex;
            objclient.CreateDate = obj.CreateDate;
            objclient.PMLibCopy = obj.PMLibCopy;
            objclient.AssetTree = obj.AssetTree;
            objclient.ProdAppUsers = obj.ProdAppUsers;
            objclient.MaxProdAppUsers = obj.MaxProdAppUsers;
            return objclient;
        }

        public List<EventLogModel> PopulateEventLog(long ClientId, string order = "0", string orderDir = "asc", int skip = 0, int length = 10)
        {
            EventLogModel objEventLogModel;
            List<EventLogModel> EventLogModelList = new List<EventLogModel>();

            ClientEventLog log = new ClientEventLog();
            List<ClientEventLog> data = new List<ClientEventLog>();
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
                    objEventLogModel = new EventLogModel();
                    objEventLogModel.ClientId = item.ClientId;
                    objEventLogModel.SiteId = item.SiteId;
                    objEventLogModel.EventLogId = item.ClientEventLogId;
                    if (item.TransactionDate != null && item.TransactionDate == default(DateTime))
                    {
                        objEventLogModel.TransactionDate = null;
                    }
                    else
                    {
                        objEventLogModel.TransactionDate = item.TransactionDate;
                    }
                    objEventLogModel.Event = item.Event;
                    objEventLogModel.Comments = item.Comments;
                    objEventLogModel.SiteName = item.SiteName;
                    objEventLogModel.TotalCount = item.TotalCount;
                    EventLogModelList.Add(objEventLogModel);
                }
            }
            return EventLogModelList;
        }

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
        #endregion

        #region Edit Site
        public DataContracts.Site SaveSite(ClientVM objsite)
        {
            string emptyValue = string.Empty;
            List<string> errList = new List<string>();
            DataContracts.Site site = new DataContracts.Site();
            if (objsite.SiteModelView.SiteId == 0)
            {
                DataContracts.Client client = new DataContracts.Client();
                client.ClientId = _userData.DatabaseKey.Client.ClientId;
                client.CreatedClientId = objsite.SiteModelView.ClientId;
                client.RetrieveBySomaxAdmin(_userData.DatabaseKey);
                site.ClientId = objsite.SiteModelView.ClientId;
                List<DataContracts.Site> siteList = null;
                siteList = site.RetrieveAllFromAdmin(this._userData.DatabaseKey, client.ConnectionString, client.ClientId);
                if (siteList.Count >= client.MaxSites)
                {
                    errList.Add(UtilityFunction.GetMessageFromResource("NoOfSiteExceedError", LocalizeResourceSetConstants.Global));


                }
                List<DataContracts.Site> obj = siteList.FindAll(x => x.Name.ToLower() == objsite.SiteModelView.Name.ToLower());
                if (obj != null && obj.Count > 0)
                {

                    errList.Add(UtilityFunction.GetMessageFromResource("DuplicateSiteError", LocalizeResourceSetConstants.Global));
                }

                if (errList.Count == 0)
                {
                    site.ClientId = objsite.SiteModelView.ClientId;
                    site.Name = objsite.SiteModelView.Name != null ? objsite.SiteModelView.Name : emptyValue;
                    site.Description = objsite.SiteModelView.Description != null ? objsite.SiteModelView.Description : emptyValue;
                    site.TimeZone = objsite.SiteModelView.TimeZone != null ? objsite.SiteModelView.TimeZone : emptyValue;
                    site.MaxAppUsers = objsite.SiteModelView.MaxAppUsers ?? 0;
                    site.MaxSanitationUsers = objsite.SiteModelView.MaxSanitationUsers ?? 0;
                    site.APM = objsite.SiteModelView.APM;
                    site.CMMS = objsite.SiteModelView.CMMS;
                    site.Sanitation = objsite.SiteModelView.Sanitation;
                    site.CreateFromAdminClient(this._userData.DatabaseKey, client.ConnectionString);
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
                client.CreatedClientId = objsite.SiteModelView.ClientId;
                client.RetrieveBySomaxAdmin(userData.DatabaseKey);

                site.ClientId = objsite.SiteModelView.ClientId;
                site.CustomClientId = objsite.SiteModelView.ClientId;
                site.SiteId = objsite.SiteModelView.SiteId;
                site.RetrieveSiteByClientIdSiteId(userData.DatabaseKey);
                List<DataContracts.Site> siteList = site.RetrieveAllSitesFromAdmin(this.userData.DatabaseKey, client.ClientId);
                List<DataContracts.Site> obj = siteList.FindAll(x => (x.Name.ToLower() == objsite.SiteModelView.Name.ToLower()) && (x.SiteId != objsite.SiteModelView.SiteId));

                if (obj != null && obj.Count > 0)
                {
                    errList.Add(UtilityFunction.GetMessageFromResource("DuplicateSiteError", LocalizeResourceSetConstants.Global));
                }
                if (errList.Count == 0)
                {
                    site.ClientId = objsite.SiteModelView.ClientId;
                    site.SiteId = objsite.SiteModelView.SiteId;
                    site.Name = objsite.SiteModelView.Name != null ? objsite.SiteModelView.Name : emptyValue;
                    site.Description = objsite.SiteModelView.Description != null ? objsite.SiteModelView.Description : emptyValue;
                    site.TimeZone = objsite.SiteModelView.TimeZone != null ? objsite.SiteModelView.TimeZone : emptyValue;
                    site.MaxAppUsers = objsite.SiteModelView.MaxAppUsers ?? 0;
                    site.MaxSanitationUsers = objsite.SiteModelView.MaxSanitationUsers ?? 0;
                    site.MaxWorkRequestUsers = objsite.SiteModelView.MaxWorkRequestUsers ?? 0;
                    site.MaxProdAppUsers = objsite.SiteModelView.MaxProdAppUsers ?? 0;
                    site.APM = objsite.SiteModelView.APM;
                    site.CMMS = objsite.SiteModelView.CMMS;
                    site.Sanitation = objsite.SiteModelView.Sanitation;
                    site.Production = objsite.SiteModelView.Production;
                    site.UsePunchOut = objsite.SiteModelView.UsePunchOut;
                    site.UpdateIndex = objsite.SiteModelView.UpdateIndex;
                    site.UpdateFromAdmin_V2(this.userData.DatabaseKey);
                }
                else
                {
                    site.ErrorMessages = errList;
                }

            }

            return site;
        }
        #endregion

        #region Edit Site Billing
        public DataContracts.SiteBilling EditSiteBilling(ClientVM objsite)
        {
            string emptyValue = string.Empty;
            List<string> errList = new List<string>();
            DataContracts.SiteBilling siteBilling = new DataContracts.SiteBilling()
            {
                ClientId = objsite.SiteBillingModelView.ClientId,
                SiteId = objsite.SiteBillingModelView.SiteId
            };

            if (objsite.SiteBillingModelView.SiteBillingId == 0)
            {
                siteBilling.ClientId = objsite.SiteBillingModelView.ClientId;
                siteBilling.SiteId = objsite.SiteBillingModelView.SiteId;
                siteBilling.AnniversaryDate = objsite.SiteBillingModelView.AnniversaryDate;
                siteBilling.InvoiceFreq = objsite.SiteBillingModelView.InvoiceFreq;
                siteBilling.Terms = objsite.SiteBillingModelView.Terms != null ? objsite.SiteBillingModelView.Terms : emptyValue;
                siteBilling.CurrentInvoice = objsite.SiteBillingModelView.CurrentInvoice != null ? objsite.SiteBillingModelView.CurrentInvoice : emptyValue;
                siteBilling.InvoiceDate = objsite.SiteBillingModelView.InvoiceDate;
                siteBilling.NextInvoiceDate = objsite.SiteBillingModelView.NextInvoiceDate;
                siteBilling.QuoteRequired = objsite.SiteBillingModelView.QuoteRequired;
                siteBilling.CustomClientId = objsite.SiteBillingModelView.ClientId;
                 siteBilling.CreateSiteBillingWithCustomClientId(this.userData.DatabaseKey);
            }
            else
            {
                siteBilling.ClientId = objsite.SiteBillingModelView.ClientId;
                siteBilling.SiteId = objsite.SiteBillingModelView.SiteId;
                siteBilling.SiteBillingId = objsite.SiteBillingModelView.SiteBillingId;
                siteBilling.AnniversaryDate = objsite.SiteBillingModelView.AnniversaryDate;
                siteBilling.InvoiceFreq = objsite.SiteBillingModelView.InvoiceFreq;
                siteBilling.Terms = objsite.SiteBillingModelView.Terms != null ? objsite.SiteBillingModelView.Terms : emptyValue;
                siteBilling.CurrentInvoice = objsite.SiteBillingModelView.CurrentInvoice != null ? objsite.SiteBillingModelView.CurrentInvoice : emptyValue;
                siteBilling.InvoiceDate = objsite.SiteBillingModelView.InvoiceDate;
                siteBilling.NextInvoiceDate = objsite.SiteBillingModelView.NextInvoiceDate;
                siteBilling.QuoteRequired = objsite.SiteBillingModelView.QuoteRequired;
                siteBilling.CustomClientId = objsite.SiteBillingModelView.ClientId;
                siteBilling.UpdateSiteBillingWithCustomClientId(this.userData.DatabaseKey);
            }
            
            
            return siteBilling;
        }
        #endregion
        #region Active Inactive Site
        public List<string> MakeActiveInactive(long ClientId, long SiteId, bool InActiveFlag,int UpdateIndex)
        {
            DataContracts.Client client = new DataContracts.Client();
            client.CreatedClientId = ClientId;
            client.RetrieveBySomaxAdmin(_dbKey);
            DataContracts.Site site = new DataContracts.Site()
            {
                ClientId = ClientId,
                SiteId = SiteId
            };
            site.CustomClientId = ClientId;
            site.RetrieveSiteByClientIdSiteId(_dbKey);
            if(!InActiveFlag)
            {
                site.Status = "Active";
            }
            else
            {
                site.Status = "Inactive";
            }
            site.UpdateIndex = UpdateIndex;
            site.UpdateFromAdmin_V2(_dbKey);
            if(site.ErrorMessages==null ||  site.ErrorMessages.Count==0)
            {
                ClientEventLog clientEventLog = new ClientEventLog()
                {
                    ClientId = ClientId,
                    SiteId = SiteId
                };
                clientEventLog.TransactionDate = DateTime.UtcNow;
                if (!InActiveFlag)
                {
                    clientEventLog.Event = "ActivateSite";
                }
                else 
                {
                    clientEventLog.Event = "InactivateSite";
                }
                clientEventLog.CreateFromAdmin(_dbKey);
            }
            return site.ErrorMessages;
        }
        #endregion
        #endregion

        #region V2-993
        public List<ClientMessageModel> MessageSelectedClientDetailsChunkSearch(string orderbycol = "", long ClientId = 0, int length = 0, string orderDir = "", int skip = 0)
        {
            List<ClientMessageModel> ClientMessageModelList = new List<ClientMessageModel>();
            ClientMessageModel clientMessageModel;
            ClientMessage clientMessage = new ClientMessage();
            clientMessage.OrderbyColumn = orderbycol;
            clientMessage.OrderBy = orderDir;
            clientMessage.OffSetVal = skip;
            clientMessage.NextRow = length;
            clientMessage.CustomClientId= ClientId;
            var clientMessageList = clientMessage.MessageSelectedClientDetailsChunkSearch(_dbKey);
            foreach (var item in clientMessageList)
            {
                clientMessageModel = new ClientMessageModel();
                clientMessageModel.ClientId = item.ClientId;
                clientMessageModel.ClientMessageId = item.ClientMessageId;
                clientMessageModel.Message = item.Message;
                clientMessageModel.StartDate = item.StartDate;
                clientMessageModel.EndDate = item.EndDate;
                clientMessageModel.CreateDate = item.CreateDate;
                clientMessageModel.TotalCount = item.TotalCount;
                ClientMessageModelList.Add(clientMessageModel);
            }
            return ClientMessageModelList;
        }


        public ClientMessageModel RetrieveMessageSelectedClient(long ClientMessageId,long ClientId)
        {
            var clientMessageModel = new ClientMessageModel();
            ClientMessage clientMessage = new ClientMessage()
            {
                CustomClientId = ClientId,
                ClientMessageId = ClientMessageId,
            };
            clientMessage.Retrieve_V2(_dbKey);
            if (clientMessage != null)
            {
                clientMessageModel.ClientMessageId = clientMessage.ClientMessageId;
                clientMessageModel.Message = clientMessage.Message;
                clientMessageModel.CMStartDate = clientMessage.StartDate?.ToString("MM/dd/yyyy");
                clientMessageModel.CMStartTime = clientMessage.StartDate?.ToString("hh:mm tt");
                clientMessageModel.CMEndDate = clientMessage.EndDate?.ToString("MM/dd/yyyy");
                clientMessageModel.CMEndTime = clientMessage.EndDate?.ToString("hh:mm tt");
            }
            return clientMessageModel;
        }


        public ClientMessage AddMessageSelectedClient(ClientMessageModel model)
        {
           
            ClientMessage clientMessage = new ClientMessage()
            {
                CustomClientId = model.ClientId,
                ClientMessageId = model.ClientMessageId,
                Message = model.Message,
                StartDate = DateTime.ParseExact(model.CMStartDate + " " + model.CMStartTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture),
                EndDate = DateTime.ParseExact(model.CMEndDate + " " + model.CMEndTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture),
            };
            clientMessage.Create_V2(_dbKey);
            return clientMessage;
        }
        public ClientMessage EditMessageSelectedClient(ClientMessageModel model)
        {
            ClientMessage clientMessage = new ClientMessage()
            {
                CustomClientId = model.ClientId,
                ClientMessageId = model.ClientMessageId,
            };
            clientMessage.Retrieve_V2(_dbKey);
            var stardate = model.CMStartDate;
            DateTime StartDate = DateTime.ParseExact(stardate + " " + model.CMStartTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            var enddate = model.CMEndDate;
            DateTime EndDate = DateTime.ParseExact(enddate + " " + model.CMEndTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            clientMessage.ClientId = model.ClientId;
            clientMessage.ClientMessageId = model.ClientMessageId;
            clientMessage.StartDate = StartDate;
            clientMessage.EndDate = EndDate;
            clientMessage.Message = model.Message;
            clientMessage.Update_V2(_dbKey);
            return clientMessage;
        }

        public ClientMessage AddMessageAllClient(ClientMessageModel model)
        {
            var stardate = model.CMStartDate;
            DateTime StartDate = DateTime.ParseExact(stardate + " " + model.CMStartTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            var enddate = model.CMEndDate;
            DateTime EndDate = DateTime.ParseExact(enddate + " " + model.CMEndTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            ClientMessage clientMessage = new ClientMessage()
            {
                CustomClientId = 0,
                ClientMessageId = model.ClientMessageId,
                Message = model.Message,
                StartDate = StartDate,
                EndDate = EndDate,
            };
            clientMessage.Create_V2(_dbKey);
            return clientMessage;
        }
        public ClientMessage EditMessageAllClient(ClientMessageModel model)
        {
            ClientMessage clientMessage = new ClientMessage()
            {
                CustomClientId = model.ClientId,
                ClientMessageId = model.ClientMessageId,
            };
            clientMessage.Retrieve_V2(_dbKey);
            var stardate = model.CMStartDate;
            DateTime StartDate = DateTime.ParseExact(stardate + " " + model.CMStartTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            var enddate = model.CMEndDate;
            DateTime EndDate = DateTime.ParseExact(enddate + " " + model.CMEndTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            clientMessage.ClientId = model.ClientId;
            clientMessage.ClientMessageId = model.ClientMessageId;
            clientMessage.StartDate = StartDate;
            clientMessage.EndDate = EndDate;
            clientMessage.Message = model.Message;
            clientMessage.Update_V2(_dbKey);
            return clientMessage;
        }
        #endregion
    }
}
