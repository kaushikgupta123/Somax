using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Extensions;

using Data.Database;
using Database.Business;
using Database;

namespace DataContracts
{
    public partial class Menu : DataContractBase
    {
        #region Property
        public bool ItemAccess { get; set; }
        public Int64 SecurityItemId { get; set; }
        public Int64 UserInfoId { get; set; }
        public string LocalizedName { get; set; }
        public string LocaleId { get; set; }
        public string ResourceSet { get; set; }

        public System.Data.DataTable StatusData = new System.Data.DataTable();
        public string ModuleName { get; set; }
        public string ItemCount { get; set; }
        public Int64 SiteId { get; set; }
        #endregion
        public List<b_Menu> MenuList { get; set; }
        public List<Menu> RetrieveAll(DatabaseKey dbKey)
        {
            List<Menu> result = new List<Menu>();
            Menu_RetrieveAll trans = new  Menu_RetrieveAll
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            result = UpdateFromDatabaseObjectList(trans.MenuList);
            return result;
        }
        public List<Menu> RetrieveAllCustom(DatabaseKey dbKey,bool APM,bool CMMS,bool Sanitation)
        {
            List<Menu> result = new List<Menu>();
            Database.Menu_RetrieveAllCustom trans = new Database.Menu_RetrieveAllCustom
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

                PackageLevel = dbKey.Client.PackageLevel,
                BusinessType = dbKey.Client.BusinessType,
                APM = APM,
                Sanitation = Sanitation,
                CMMS = CMMS,
                UserType = dbKey.User.UserType
            };
            trans.Menu = this.ToDatabaseObjectCustom();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            result = UpdateFromDatabaseObjectList(trans.MenuList);
            return result;
        }
        public List<Menu> RetrieveAllCustomAdmin(DatabaseKey dbKey)
        {
            List<Menu> result = new List<Menu>();
            Menu_RetrieveAllCustomAdmin trans = new Database.Menu_RetrieveAllCustomAdmin
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                //PackageLevel = dbKey.Client.PackageLevel,
                //BusinessType = dbKey.Client.BusinessType,
                //APM = APM,
                //Sanitation = Sanitation,
                //CMMS = CMMS,
                UserType = dbKey.User.UserType
            };
            trans.Menu = this.ToDatabaseObjectCustom();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            result = UpdateFromDatabaseObjectList(trans.MenuList);
            return result;
        }
        public b_Menu ToDatabaseObjectCustom()
        {
            b_Menu dbObj = new b_Menu();
            dbObj.ClientId = this.ClientId;
            dbObj.ResourceSet = this.ResourceSet;
            dbObj.LocaleId = this.LocaleId;
            dbObj.MenuType = this.MenuType;
            dbObj.SiteId = this.SiteId;
            dbObj.UserInfoId = this.UserInfoId;
            return dbObj;
        }
        private List<Menu> UpdateFromDatabaseObjectList(List<b_Menu> menuList)
        {
            List<Menu> result = new List<Menu>();

            foreach (b_Menu dbObj in menuList)
            {
                Menu tmp = new Menu();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.ItemAccess = dbObj.ItemAccess;
                tmp.LocalizedName = dbObj.LocalizedName;
                tmp.LocaleId = dbObj.LocaleId;
                tmp.UserInfoId = dbObj.UserInfoId;
                tmp.SecurityItemId = dbObj.SecurityItemId;
                result.Add(tmp);
            }
            return result;
        }

        public List<Menu> RetrieveMenuStatusList(DatabaseKey dbKey)
        {
            List<Menu> result = new List<Menu>();
            Menu_RetrieveStatusCountPage trans = new Menu_RetrieveStatusCountPage()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Menu = this.ToDatabaseObjectMenuStatusList();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            result = UpdateFromDatabaseObjectMenuStatusList(trans.MenuStatusList);
            return result;

            
        }
        public b_Menu ToDatabaseObjectMenuStatusList()
        {
            b_Menu dbObj = new b_Menu();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;            
            dbObj.StatusData.Dispose();
            dbObj.StatusData = new System.Data.DataTable();
            dbObj.StatusData = this.StatusData;
            return dbObj;
        }
        private List<Menu> UpdateFromDatabaseObjectMenuStatusList(List<b_Menu> menuList)
        {
            List<Menu> result = new List<Menu>();

            foreach (b_Menu dbObj in menuList)
            {
                Menu tmp = new Menu();
                tmp.ModuleName = dbObj.ModuleName;
                tmp.ItemCount = dbObj.ItemCount;
                result.Add(tmp);
            }
            return result;
        }

    }
}
