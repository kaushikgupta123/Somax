/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person            Description
* =========== ======== ================== ========================================================
* 2011-Nov-29 20110000 Roger Lawton       Created partial class file
* 2014-Aug-09 SOM-282  Roger Lawton       Added UserIsActive and CurrentIsActive
* 2014-Oct-21 SOM-384  Roger Lawton       Additional Parameters in ValidateNewUserAdd and 
*                                         ValidateUserUpdate
*                                         Removed tablet and phone user properties
* 2015-Nov-06 SOM-851  Roger Lawton       Support Multi-Site - Get list of authorized sites for a 
*                                         particular user (AuthorizedUser) 
***************************************************************************************************
*/


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Database.Business;
using Database.Transactions;
using Database;
using Data.Database;

namespace DataContracts
{
    public partial class Site:DataContractBase,IStoredProcedureValidation
    {
        #region Transaction Methods

        public List<Site> LookupList(DatabaseKey dbKey)
        {
            SiteLookupList trans = new SiteLookupList();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<b_Site> lookup = trans.result;
            List<Site> result = new List<Site>();
            foreach (b_Site li in lookup)
            {
                Site temp = new Site()
                {
                    ClientId = li.ClientId,
                    SiteId = li.SiteId,
                    Name = li.Name,
                    Description = li.Description
                };
                result.Add(temp);
            }
            return result;

        }
       //------------------------------Added By Indusnet Technologies------------------
        public static List<Site> UpdateFromDatabaseObjectList(List<b_Site> dbObjs)
        {
            List<Site> result = new List<Site>();

            foreach (b_Site dbObj in dbObjs)
            {
                Site tmp = new Site();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public List<Site> RetrieveAll(DatabaseKey dbKey)
        {
            Site_RetrieveAll trans = new Site_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SiteList);
        }

        // SOM-851
        public List<Site> RetrieveAuthorizedForUser(DatabaseKey dbKey)
        {
          Site_RetrieveAuthorizedForUser trans = new Site_RetrieveAuthorizedForUser()
          {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName
          };

          trans.dbKey = dbKey.ToTransDbKey();
          trans.UseTransaction = false;
          trans.Site = this.ToDatabaseObject();
          trans.Site.AuthorizedUser = this.AuthorizedUser;
          trans.Execute();
          return UpdateFromDatabaseObjectList(trans.SiteList);
        }

        public List<Site> RetrieveAllFromAdmin(DatabaseKey dbKey,string ConnectionString,long SearchClientId)
        {
            Site_RetrieveAllFromAdmin trans = new Site_RetrieveAllFromAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ConnectionString = ConnectionString;
            trans.SearchClientId = SearchClientId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.dbKey.Client.ClientId = SearchClientId;
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SiteList);
        }
        public List<Site> RetrieveBySearchFromAdmin(DatabaseKey dbKey, string ConnectionString)
        {
            Site_RetrieveBySearchFromAdmin trans = new Site_RetrieveBySearchFromAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ConnectionString = ConnectionString;
            trans.Site = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.dbKey.Client.ClientId = this.ClientId;
            trans.SearchClientId = this.ClientId;
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SiteList);
        }
        public void CreateFromAdmin(DatabaseKey dbKey,string ClientConnectionString)
        {
            Site_CreateFromAdmin trans = new Site_CreateFromAdmin();
            trans.ConnectionString = ClientConnectionString;
            trans.Site = this.ToDatabaseObject();           
            trans.dbKey = dbKey.ToTransDbKey();
            trans.dbKey.Client.ClientId = trans.Site.ClientId;
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.Site);
        }

        public void UpdateFromAdmin(DatabaseKey dbKey,string ClientConnectionString)
        {
            Site_UpdateFromAdmin trans = new Site_UpdateFromAdmin();
            trans.ConnectionString = ClientConnectionString;
            trans.Site = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.dbKey.Client.ClientId = this.ClientId;
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Site);
        }

        public void RetrieveFromAdmin(DatabaseKey dbKey, string ClientConnectionString)
        {
            Site_RetrieveFromAdmin trans = new Site_RetrieveFromAdmin();
            trans.ConnectionString = ClientConnectionString;
            trans.Site = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.dbKey.Client.ClientId = this.ClientId;
            trans.Execute();
            UpdateFromDatabaseObject(trans.Site);
        }


        [DataMember]
        public string LocalizationLanguageAndCulture
        {
            get
            {
                if (!string.IsNullOrEmpty(GetLocalizationValue(0)) && !string.IsNullOrEmpty(GetLocalizationValue(1)))
                {
                    return (GetLocalizationValue(0) + "-" + GetLocalizationValue(1));
                }
                else
                {
                    return (string.Empty);
                }

            }
            set
            {
                string[] temp = value.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                m_LocalizationCulture = temp.Length > 0 ? temp[0] : string.Empty;
                m_LocalizationLanguage = temp.Length > 0 ? temp[1] : string.Empty; ;
                Localization = BuildLocalization();
            }
        }

        [DataMember]
        public string LocalizationCulture
        {
            get { return GetLocalizationValue(1); }
            set
            {
                m_LocalizationCulture = value;
                Localization = BuildLocalization();
            }
        }

        [DataMember]
        public string LocalizationLanguage
        {
            get { return GetLocalizationValue(0); }
            set
            {
                m_LocalizationLanguage = value;
                Localization = BuildLocalization();
            }
        }

        [DataMember]
        public string UIConfigurationLocation
        {
            get { return GetUIConfigurationValue(1); }
            set
            {
                m_UIConfigurationLocation = value;
                UIConfiguration = BuildUIConfiguration();
            }
        }

        [DataMember]
        public string UIConfigurationCompany
        {
            get { return GetUIConfigurationValue(0); }
            set
            {
                m_UIConfigurationCompany = value;
                UIConfiguration = BuildUIConfiguration();
            }
        }

        #region Private Methods
        private string GetLocalizationValue(int index)
        {
            if (m_LocalizationValues == null)
            {
                // Get the localization values from Localization
                string[] temp = Localization.Split(new char[] { '-', '_', '/' }, StringSplitOptions.RemoveEmptyEntries);
                m_LocalizationValues = new List<string>(temp);
            }
            return (m_LocalizationValues.Count > index) ? m_LocalizationValues[index] : string.Empty;
        }

        private string BuildLocalization()
        {
            StringBuilder local = new StringBuilder();
            if (string.IsNullOrEmpty(m_LocalizationLanguage)) { return string.Empty; }
            local.Append(m_LocalizationLanguage);

            if (string.IsNullOrEmpty(m_LocalizationCulture)) { return local.ToString(); }
            local.Append("-");
            local.Append(m_LocalizationCulture);           

            return local.ToString();
        }

        private string BuildUIConfiguration()
        {
            StringBuilder local = new StringBuilder();

            if (string.IsNullOrEmpty(m_UIConfigurationCompany)) { return string.Empty; }
            local.Append(m_UIConfigurationCompany);

            if (string.IsNullOrEmpty(m_UIConfigurationLocation)) { return local.ToString(); }
            local.Append("/");
            local.Append(m_UIConfigurationLocation);          

            return local.ToString();
        }

        private string GetUIConfigurationValue(int index)
        {
            if (m_UIConfigurationValues == null)
            {
                // Get the UI Configuration values from UIConfiguration
                string[] temp = UIConfiguration.Split(new char[] { '-', '_', '/' }, StringSplitOptions.RemoveEmptyEntries);
                m_UIConfigurationValues = new List<string>(temp);
            }
            return (m_UIConfigurationValues.Count > index) ? m_UIConfigurationValues[index] : string.Empty;
        }
        #endregion
        #region Private Variables
        private string m_LocalizationCulture;
        private string m_LocalizationLanguage;       
        private string m_UIConfigurationLocation;
        private string m_UIConfigurationCompany;    
        private List<string> m_LocalizationValues;
        private List<string> m_UIConfigurationValues;

        public long CreatedClientId { get; set; }
        // SOM-851
        public long AuthorizedUser { get; set; }
        // User Validation purposes
        string ValidateFor = string.Empty;
        public bool UserIsActive { get; set; }
        public string UserType { get; set; }
        //public bool WebAppUser { get; set; }
        //public bool TabletUser { get; set; }
        //public bool PhoneUser { get; set; }
        public bool IsSuperUser { get; set; }
        public bool CurrentUserIsActive { get; set; }
        public string CurrentUserType { get; set; }
        //public bool CurrentWebAppUser { get; set; }
        //public bool CurrentTabletUser { get; set; }
        //public bool CurrentPhoneUser { get; set; }
        public bool CurrentIsSuperUser { get; set; }


        #endregion

        public void ValidateNewUserAdd(DatabaseKey dbKey)
        {
            ValidateFor = "NewUserAdd";
            Validate<Site>(dbKey);
        }
        public void ValidateUserUpdate(DatabaseKey dbKey)
        {
            ValidateFor = "UserUpdate";
            Validate<Site>(dbKey);
        }


        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (ValidateFor == "NewUserAdd")
            {
                Site_ValidateNewUserAdd trans = new Site_ValidateNewUserAdd()
                {
                    // SOM-384
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                    IsSuperUser=IsSuperUser,
                    UserType=UserType
                };

                trans.Site = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            else if (ValidateFor == "UserUpdate")
            {
                Site_ValidateUserUpdate trans = new Site_ValidateUserUpdate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                    UserType = UserType,
                    UserIsActive = UserIsActive,
                    //WebAppUser = WebAppUser,
                    //TabletUser = TabletUser,
                    //PhoneUser = PhoneUser,
                    IsSuperUser = IsSuperUser,
                    CurrentUserType = CurrentUserType,
                    CurrentUserIsActive = CurrentUserIsActive,
                    //CurrentWebAppUser = CurrentWebAppUser,
                    //CurrentTabletUser = CurrentTabletUser,
                    //CurrentPhoneUser = CurrentPhoneUser,
                    CurrentIsSuperUser = CurrentIsSuperUser
                };

                trans.Site = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            else if (ValidateFor == "ValidateProcessSystemTree")  //SOM - 899
            {
                Site_ValidateByProcessSystemId trans = new Site_ValidateByProcessSystemId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                // RKL - 2014-Jul-31 - Use ToDatabaseObjectExtended 
                trans.Site = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }

            return errors;
        }

        //SOM - 899

        public void UpdateWithProcessSystemTreeValidation(DatabaseKey dbKey)
        {
           
                 Site_Update trans = new Site_Update();
                 trans.Site = this.ToDatabaseObject();
                 //trans.ChangeLog = GetChangeLogObject(dbKey);
                 trans.dbKey = dbKey.ToTransDbKey();
                 trans.Execute();
                 // The create procedure changed the Update Index.
                 UpdateFromDatabaseObject(trans.Site);
             //}
        }

        //public void RetrieveAssetGroupNameV2ForSite(DatabaseKey dbKey)
        //{
        //    Equipment_RetrieveAll_V2 trans = new Equipment_RetrieveAll_V2();
        //    trans.Equipment = this.ToDatabaseObject();
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.Execute();
        //    UpdateFromDatabaseObject(trans.Equipment);

        //    Site_RetrieveAssetGroupNameV2 trans = new Site_RetrieveAssetGroupNameV2()
        //    {
        //        CallerUserInfoId = dbKey.User.UserInfoId,
        //        CallerUserName = dbKey.UserName,
        //    };

        //    trans.Site = this.ToDatabaseObject();
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.Execute();
        //    UpdateFromDatabaseObject();



        //}
        public List<Site> RetrieveAssetGroupNameV2(DatabaseKey dbKey)
        {
           Site_RetrieveAssetGroupNameV2 trans = new Site_RetrieveAssetGroupNameV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Site> SiteList = new List<Site>();
            foreach (b_Site Site in trans.SiteList)
            {
                Site tmpSite = new Site();
                tmpSite.UpdateFromDatabaseObject(Site);

                SiteList.Add(tmpSite);
            }
            return SiteList;
        }

        //------------------------------Added By Indusnet Technologies------------------
        #endregion

        #region Properties
        public string FullName 
        {
            get {return string.Format("{0} - {1}" , Name.Trim() , Description.Trim());}
        }

        #region V2 435 Admin Site
        public List<b_Site> listOfSite { get; set; }
        public string SearchText { get; set; }
        public Int32 TotalCount { get; set; }       
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public Int32 OffSetVal { get; set; }
        public Int32 NextRow { get; set; }
        public Int64 CustomClientId { get; set; }

        public string ClientName { get; set; }
        public string LocalizationName { get; set; }
        public string TimeZoneName { get; set; }
        public bool ClientSiteControl { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate
        {
            get; set;
        }
        #endregion
        #region V2-419 Enterprise User Management - Add/Remove Sites
        public int DefaultBuyerCount { get; set; }
        #endregion
        #endregion

        #region V2-419 Enterprise User Management - Add/Remove Sites
        public List<Site> RetrieveAllAssignedSiteByUser(DatabaseKey dbKey)
        {
            Site_RetrieveAllAssignedSiteByUser trans = new Site_RetrieveAllAssignedSiteByUser()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.UseTransaction = false;
            trans.Site = this.ToDatabaseObject();
            trans.Site.AuthorizedUser = this.AuthorizedUser;
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SiteList);
        }

        public List<Site> RetrieveDefauiltBuyer(DatabaseKey dbKey, Site sj)
        {
            Site_RetrieveDefaultBuyer trans = new Site_RetrieveDefaultBuyer()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Site = sj.ToDatabaseObjectDefaultBuyer();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectExtendedList(trans.SiteList);
        }

        public b_Site ToDatabaseObjectDefaultBuyer()
        {
            b_Site dbObj = new b_Site();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.AuthorizedUser = this.AuthorizedUser;
            return dbObj;
        }
       
        public void UpdateFromDatabaseObjectExtended(b_Site dbObj)
        {
            this.DefaultBuyerCount = dbObj.DefaultBuyerCount;
        }
        public static List<Site> UpdateFromDatabaseObjectExtendedList(List<b_Site> dbObjs)
        {
            List<Site> result = new List<Site>();

            foreach (b_Site dbObj in dbObjs)
            {
                Site tmp = new Site();
                tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        #endregion

        #region V2-435 Admin Site
        public List<Site> SiteRetrieveChunkSearchV2(DatabaseKey dbKey, string TimeZone)
        {
            Admin_RetrieveSiteChunkSearchV2 trans = new Admin_RetrieveSiteChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Site = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Site> Sitelist = new List<Site>();
            foreach (b_Site Site in trans.Site.listOfSite)
            {
                Site tmpSite= new Site();

                tmpSite.UpdateFromDatabaseObjectForSiteChunkSearch(Site, TimeZone);
                Sitelist.Add(tmpSite);
            }
            return Sitelist;
        }

        public b_Site ToDateBaseObjectForChunkSearch()
        {
            b_Site dbObj = this.ToDatabaseObject();
            dbObj.OrderBy = this.OrderBy;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.CustomClientId = this.CustomClientId;
            dbObj.SearchText = this.SearchText;
            return dbObj;

        }

        public void UpdateFromDatabaseObjectForSiteChunkSearch(b_Site dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Name = dbObj.Name;
            this.AddressCity = dbObj.AddressCity;
            this.AddressState = dbObj.AddressState;
            //this.Status = dbObj.Status;
            this.TotalCount = dbObj.TotalCount;

        }



        public Site RetrieveAllSiteBySiteIdV2(DatabaseKey dbKey)
        {
            Site_RetrieveAllBySiteId_V2 trans = new Site_RetrieveAllBySiteId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Site = this.ToDateBaseObjectForRetrieveSite();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
          

            UpdateFromDatabaseObject(trans.Site);
            this.ClientName = trans.Site.ClientName;
            this.LocalizationName = trans.Site.LocalizationName;
            this.TimeZoneName = trans.Site.TimeZoneName;
            this.ClientSiteControl = trans.Site.ClientSiteControl;
            return this;
            //List<Site> SiteList = new List<Site>();
            //foreach (b_Site Site in trans.SiteList)
            //{
            //    Site tmpSite = new Site();

            //    tmpSite.UpdateFromDatabaseObjectForRetrieveBySiteIdV2(Site);
            //    SiteList.Add(tmpSite);
            //}
            // return objSite;
        }
        public b_Site ToDateBaseObjectForRetrieveSite()
        {
            b_Site dbObj = this.ToDatabaseObject();
            dbObj.CustomClientId = this.CustomClientId;
            dbObj.SiteId = this.SiteId;
          
            return dbObj;

        }

        public void UpdateFromDatabaseObjectForRetrieveBySiteIdV2(b_Site dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ClientName = dbObj.ClientName;
            this.LocalizationName = dbObj.LocalizationName;
            this.TimeZoneName = dbObj.TimeZoneName;
        }

        public void RetrieveCreateModifyDate(DatabaseKey dbKey)
        {
            Site_RetrieveCreateModifyDate trans = new Site_RetrieveCreateModifyDate()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Site = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.Site);
            this.CreateBy = trans.Site.CreateBy;
            this.CreateDate = trans.Site.CreateDate;
            this.ModifyBy = trans.Site.ModifyBy;
            this.ModifyDate = trans.Site.ModifyDate;
        }
        #endregion

        #region V2-806
        public List<Site> RetrieveByClientIdForLookupList(DatabaseKey dbKey, string ConnectionString)
        {
            Site_RetrieveByClientIdForLookupList trans = new Site_RetrieveByClientIdForLookupList()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ConnectionString= ConnectionString;
            trans.Site = this.ToDatabaseObjectExtended();
            //trans.Site.ClientId = this.ClientId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectlist(trans.SiteList));
        }
        public b_Site ToDatabaseObjectExtended()
        {
            b_Site dbObj = new b_Site();
            dbObj.ClientId = this.ClientId;
            return dbObj;
        }
        public List<Site> UpdateFromDatabaseObjectlist(List<b_Site> dbObjlist)
        {
            List<Site> temp = new List<Site>();

            Site objPer;

            foreach (b_Site per in dbObjlist)
            {
                objPer = new Site();
                objPer.UpdateFromDatabaseObject(per);
                temp.Add(objPer);
            }

            return (temp);


        }
        #endregion

        #region V2-964
        public List<Site> RetrieveForAllActiveLookupListV2(DatabaseKey dbKey)
        {
            Site_RetrieveForAllActiveLookupListV2 trans = new Site_RetrieveForAllActiveLookupListV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Site = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectlist(trans.SiteList));
        }
        public List<Site> RetrieveSitesForClientChildGrid(DatabaseKey dbKey)
        {
            RetrieveSitesForClientChildGrid trans = new RetrieveSitesForClientChildGrid()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Site = this.ToDateBaseObjectForClientChildGrid();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Site> Sitelist = new List<Site>();
            foreach (b_Site Site in trans.Site.listOfSite)
            {
                Site tmpSite = new Site();

                tmpSite.UpdateFromDatabaseObjectForClientChildGrid(Site);
                Sitelist.Add(tmpSite);
            }
            return Sitelist;
        }
        public b_Site ToDateBaseObjectForClientChildGrid()
        {
            b_Site dbObj = new b_Site();
            dbObj.CustomClientId = this.CustomClientId;
            return dbObj;

        }
        public void UpdateFromDatabaseObjectForClientChildGrid(b_Site dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.Name = dbObj.Name;
            this.Status = dbObj.Status;
            this.APM = dbObj.APM;
            this.CMMS = dbObj.CMMS;
            this.Sanitation = dbObj.Sanitation;
            this.UpdateIndex = dbObj.UpdateIndex;
        }
        public void CreateFromAdminClient(DatabaseKey dbKey, string ClientConnectionString)
        {
            Site_CreateFromAdminClient trans = new Site_CreateFromAdminClient();
            trans.ConnectionString = ClientConnectionString;
            trans.Site = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.dbKey.Client.ClientId = trans.Site.ClientId;
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.Site);
        }
        public void RetrieveSiteByClientIdSiteId_V2(DatabaseKey dbKey)
        {
            RetrieveSiteByClientIdSiteId_V2 trans = new RetrieveSiteByClientIdSiteId_V2();
            trans.Site = this.ToDatabaseObjectClientIdSiteId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectForClientIdSiteId(trans.Site);
        }
        public b_Site ToDatabaseObjectClientIdSiteId()
        {
            b_Site dbObj = new b_Site();
            dbObj.CustomClientId = this.CustomClientId;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForClientIdSiteId(b_Site dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.Name = dbObj.Name;
            this.Description = dbObj.Description;
            this.TimeZone= dbObj.TimeZone;
            this.APM = dbObj.APM;
            this.CMMS = dbObj.CMMS;
            this.Sanitation = dbObj.Sanitation;
            this.Production = dbObj.Production;
            this.Status = dbObj.Status;
            this.CreateDate = dbObj.CreateDate;
            this.UsePunchOut = dbObj.UsePunchOut;
            this.AppUsers = dbObj.AppUsers;
            this.MaxAppUsers = dbObj.MaxAppUsers;
            this.WorkRequestUsers = dbObj.WorkRequestUsers;
            this.MaxWorkRequestUsers = dbObj.MaxWorkRequestUsers;
            this.SanitationUsers = dbObj.SanitationUsers;
            this.MaxSanitationUsers = dbObj.MaxSanitationUsers;
            this.ProdAppUsers = dbObj.ProdAppUsers;
            this.MaxProdAppUsers = dbObj.MaxProdAppUsers;
            this.TimeZoneName = dbObj.TimeZoneName;
            this.UpdateIndex = dbObj.UpdateIndex;
        }
        public void RetrieveSiteByClientIdSiteId(DatabaseKey dbKey)
        {
            Site_RetrieveSiteByClientIdSiteId trans = new Site_RetrieveSiteByClientIdSiteId();
            trans.Site = this.ToDatabaseObjectByClientIdSiteId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.Site);
        }
        public b_Site ToDatabaseObjectByClientIdSiteId()
        {
            b_Site dbObj = new b_Site();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.Name = this.Name;
            dbObj.Description = this.Description;
            dbObj.Localization = this.Localization;
            dbObj.UIConfiguration = this.UIConfiguration;
            dbObj.Address1 = this.Address1;
            dbObj.Address2 = this.Address2;
            dbObj.Address3 = this.Address3;
            dbObj.AddressCity = this.AddressCity;
            dbObj.AddressCountry = this.AddressCountry;
            dbObj.AddressPostCode = this.AddressPostCode;
            dbObj.AddressState = this.AddressState;
            dbObj.AppUsers = this.AppUsers;
            dbObj.MaxAppUsers = this.MaxAppUsers;
            dbObj.LimitedUsers = this.LimitedUsers;
            dbObj.MaxLimitedUsers = this.MaxLimitedUsers;
            dbObj.SanitationUsers = this.SanitationUsers;
            dbObj.MaxSanitationUsers = this.MaxSanitationUsers;
            dbObj.WorkRequestUsers = this.WorkRequestUsers;
            dbObj.MaxWorkRequestUsers = this.MaxWorkRequestUsers;
            dbObj.PhoneUsers = this.PhoneUsers;
            dbObj.MaxPhoneUsers = this.MaxPhoneUsers;
            dbObj.TabletUsers = this.TabletUsers;
            dbObj.MaxTabletUsers = this.MaxTabletUsers;
            dbObj.SuperUsers = this.SuperUsers;
            dbObj.MaxSuperUsers = this.MaxSuperUsers;
            dbObj.Status = this.Status;
            dbObj.TimeZone = this.TimeZone;
            dbObj.BillToAddress1 = this.BillToAddress1;
            dbObj.BillToAddress2 = this.BillToAddress2;
            dbObj.BillToAddress3 = this.BillToAddress3;
            dbObj.BillToAddressCity = this.BillToAddressCity;
            dbObj.BillToAddressCountry = this.BillToAddressCountry;
            dbObj.BillToAddressPostCode = this.BillToAddressPostCode;
            dbObj.BillToAddressState = this.BillToAddressState;
            dbObj.AutoPurch = this.AutoPurch;
            dbObj.AutoPurch_CreatorId = this.AutoPurch_CreatorId;
            dbObj.AutoPurch_Prefix = this.AutoPurch_Prefix;
            dbObj.AutoPM = this.AutoPM;
            dbObj.AutoPM_CreatorId = this.AutoPM_CreatorId;
            dbObj.AutoSanit = this.AutoSanit;
            dbObj.AutoSanit_CreatorId = this.AutoSanit_CreatorId;
            dbObj.GuestWorkRequest = this.GuestWorkRequest;
            dbObj.BIMURN = this.BIMURN;
            dbObj.ProcessSystemTree = this.ProcessSystemTree;
            dbObj.BillToName = this.BillToName;
            dbObj.BillToComment = this.BillToComment;
            dbObj.ShipToName = this.ShipToName;
            dbObj.ExSiteCode = this.ExSiteCode;
            dbObj.ExOrganizationId = this.ExOrganizationId;
            dbObj.UsePartMaster = this.UsePartMaster;
            dbObj.PartClientLookupId = this.PartClientLookupId;
            dbObj.UseVendorMaster = this.UseVendorMaster;
            dbObj.NoPartIdChange = this.NoPartIdChange;
            dbObj.Logo = this.Logo;
            dbObj.NonStockAccountId = this.NonStockAccountId;
            dbObj.ShoppingCartReviewDefault = this.ShoppingCartReviewDefault;
            dbObj.ShoppingCartIncludeBuyer = this.ShoppingCartIncludeBuyer;
            dbObj.DefaultBuyer = this.DefaultBuyer;
            dbObj.PartMasterReqEmail = this.PartMasterReqEmail;
            dbObj.ExternalSanitation = this.ExternalSanitation;
            dbObj.MaintOnDemand = this.MaintOnDemand;
            dbObj.UseEquipmentMaster = this.UseEquipmentMaster;
            dbObj.ShoppingCart = this.ShoppingCart;
            dbObj.MobileWOTimer = this.MobileWOTimer;
            dbObj.UIVersion = this.UIVersion;
            dbObj.PMLibrary = this.PMLibrary;
            dbObj.APM = this.APM;
            dbObj.CMMS = this.CMMS;
            dbObj.Sanitation = this.Sanitation;
            dbObj.PlantLocation = this.PlantLocation;
            dbObj.AssetGroup1Name = this.AssetGroup1Name;
            dbObj.AssetGroup2Name = this.AssetGroup2Name;
            dbObj.AssetGroup3Name = this.AssetGroup3Name;
            dbObj.VendorMaster_AllowLocal = this.VendorMaster_AllowLocal;
            dbObj.Fleet = this.Fleet;
            dbObj.UsePunchOut = this.UsePunchOut;
            dbObj.AddressISOCountryCode = this.AddressISOCountryCode;
            dbObj.BillToAddressISOCountryCode = this.BillToAddressISOCountryCode;
            dbObj.UsePlanning = this.UsePlanning;
            dbObj.Production = this.Production;
            dbObj.ProdAppUsers = this.ProdAppUsers;
            dbObj.MaxProdAppUsers = this.MaxProdAppUsers;
            dbObj.WOBarcode = this.WOBarcode;
            dbObj.IncludePRReview = this.IncludePRReview;
            dbObj.OnOrderCheck = this.OnOrderCheck;
            dbObj.VendorCompliance = this.VendorCompliance;
            dbObj.SourceAssetAccount = this.SourceAssetAccount;
            dbObj.UpdateIndex = this.UpdateIndex;
            dbObj.CustomClientId = this.CustomClientId;
            return dbObj;
        }
        public List<Site> RetrieveAllSitesFromAdmin(DatabaseKey dbKey, long SearchClientId)
        {
            Site_RetrieveAllSitesFromAdmin trans = new Site_RetrieveAllSitesFromAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SearchClientId = SearchClientId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.dbKey.Client.ClientId = SearchClientId;
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SiteList);
        }
        public void UpdateFromAdmin_V2(DatabaseKey dbKey)
        {
            Site_UpdateFromAdmin_V2 trans = new Site_UpdateFromAdmin_V2();
            trans.Site = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.dbKey.Client.ClientId = this.ClientId;
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Site);
        }
        #endregion

        #region V2-536
        public void UpdateIoTDeviceCount(DatabaseKey dbKey)
        {
            Site_UpdateIoTDeviceCount trans = new Site_UpdateIoTDeviceCount();
            trans.Site = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.dbKey.Client.ClientId = this.ClientId;
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Site);
        }
        #endregion

        #region V2-962
        public List<Site> RetrieveDefauiltBuyerForAdmin(DatabaseKey dbKey, Site sj)
        {
            Site_RetrieveDefaultBuyerForAdmin trans = new Site_RetrieveDefaultBuyerForAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Site = sj.ToDatabaseObjectDefaultBuyer();
            trans.customClientId = this.ClientId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectExtendedList(trans.SiteList);
        }
        public void UpdateForAdmin(DatabaseKey dbKey)
        {
            Site_UpdateforAdmin trans = new Site_UpdateforAdmin();
            trans.Site = this.ToDatabaseObject();
            trans.customClientId = this.ClientId;
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Site);
        }
        public void RetrieveForAdmin(DatabaseKey dbKey)
        {
            Site_RetrieveForAdmin trans = new Site_RetrieveForAdmin();
            trans.Site = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.customclientId = this.ClientId;
            trans.Execute();
            UpdateFromDatabaseObject(trans.Site);
        }
        public List<Site> RetrieveAllAssignedSiteByUserForAdmin(DatabaseKey dbKey)
        {
            Site_RetrieveAllAssignedSiteByUserForAdmin trans = new Site_RetrieveAllAssignedSiteByUserForAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.UseTransaction = false;
            trans.Site = this.ToDatabaseObject();
            trans.Site.AuthorizedUser = this.AuthorizedUser;
            trans.customclientId = this.ClientId;
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SiteList);
        }
        public List<Site> RetrieveForLookupListAdmin_V2(DatabaseKey dbKey)
        {
            Site_RetrieveLookupListForAdmin_V2 trans = new Site_RetrieveLookupListForAdmin_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            //trans.Site = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute(); 
            return UpdateFromDatabaseForAdmin_V2(trans.SiteList);
         
        
        }
        public List<Site> UpdateFromDatabaseForAdmin_V2(List<b_Site> dbObjlist)
        {
            List<Site> temp = new List<Site>();

            Site objPer;

            foreach (b_Site per in dbObjlist)
            {
                objPer = new Site();
                objPer.UpdateFromDatabaseObjectAdmin(per);
                temp.Add(objPer);
            }

            return (temp);
        }
        public void UpdateFromDatabaseObjectAdmin(b_Site dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.ClientName = dbObj.ClientName;
            this.SiteId = dbObj.SiteId;
            this.Name = dbObj.Name;
        }
        #endregion
    }
}
