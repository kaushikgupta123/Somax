/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * 
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using Database;
using Database.Business;
using Database.Transactions;

namespace DataContracts
{
    /// <summary>
    /// Business object that stores a record from the Storeroom table.
    /// </summary>
    public partial class Storeroom : DataContractBase, IStoredProcedureValidation
    {
        //v2-671
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public string SiteName { get; set; }
        public string SearchText { get; set; }
        public Int32 Case { get; set; }
        public int totalCount { get; set; }
        public long PersonnelId { get; set; }
        public string StoreroomAuthType { get; set; }
        public List<Storeroom> listOfStoreroom { get; set; }// V2-1059
        #region Properties
        public string FullName
        {
            get {return string.Format("{0} - {1}" , Name.Trim() , Description.Trim());}
        }

        #endregion
        public List<Storeroom> RetrieveAuthorizedList(DatabaseKey dbKey)
        {
          Storeroom_RetrieveAuthorizedList trans = new Storeroom_RetrieveAuthorizedList()
          {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName
          };
          trans.Storeroom = this.ToDatabaseObject();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();

          List<Storeroom> storeroomlist = new List<Storeroom>();
          foreach (b_Storeroom storeroom in trans.StoreroomList)
          {
            Storeroom tmp = new Storeroom();
            tmp.UpdateFromDatabaseObject(storeroom);
            storeroomlist.Add(tmp);
          }

          return storeroomlist;
        }

        //v2-671---rnj-
        #region search
        public List<Storeroom> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            Storeroom_RetrieveChunkSearch trans = new Storeroom_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Storeroom = this.ToDatabaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Storeroom> Storeroomlist = new List<Storeroom>();

            foreach (b_Storeroom v in trans.StoreroomList)
            {
                Storeroom tmpVendor = new Storeroom();
                tmpVendor.UpdateForChunkSearchFromDatabaseObject(v);
                Storeroomlist.Add(tmpVendor);
            }
            return Storeroomlist;

        }
        public b_Storeroom ToDatabaseObjectForChunkSearch()
        {
            b_Storeroom dbObj = new b_Storeroom();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.Name = this.Name;
            dbObj.Description = this.Description;
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.SearchText = this.SearchText;
            dbObj.Case = this.Case;
            return dbObj;
        }
        public void UpdateForChunkSearchFromDatabaseObject(b_Storeroom dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.StoreroomId = dbObj.StoreroomId;
            this.SiteId = dbObj.SiteId;
            this.Name = dbObj.Name;
            this.SiteName=dbObj.SiteName;
            this.Description = dbObj.Description;
            this.InactiveFlag = dbObj.InactiveFlag;
            this.totalCount = dbObj.totalCount;

            // Turn on auditing
            AuditEnabled = true;
        }
        //

        public void RetrieveforDetails(DatabaseKey dbKey)
        {

            Storeroom_RetrieveCustom trans = new Storeroom_RetrieveCustom();
            trans.Storeroom = this.ToDatabaseObjectForDetails();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectCustom(trans.Storeroom);
        }
        public b_Storeroom ToDatabaseObjectForDetails()
        {
            b_Storeroom dbObj = new b_Storeroom();
            dbObj.ClientId = this.ClientId;
            dbObj.StoreroomId = this.StoreroomId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectCustom(b_Storeroom dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.StoreroomId = dbObj.StoreroomId;
            this.InactiveFlag = dbObj.InactiveFlag;
            this.Name = dbObj.Name;
            this.Description = dbObj.Description;
            this.SiteName = dbObj.SiteName;

            // Turn on auditing
            AuditEnabled = true;
        }
        #endregion
        #region Retrieve StoreroomList V2-687
        public List<Storeroom> RetrieveStoreroomList(DatabaseKey dbKey)
        {
            Storeroom_RetrieveStoreroomList trans = new Storeroom_RetrieveStoreroomList()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Storeroom = this.ToDatabaseObjectForStoreroomList();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Storeroom> Storeroomlist = new List<Storeroom>();

            foreach (b_Storeroom v in trans.StoreroomList)
            {
                Storeroom tmpVendor = new Storeroom();
                tmpVendor.UpdateForStoreroomListFromDatabaseObject(v);
                Storeroomlist.Add(tmpVendor);
            }
            return Storeroomlist;

        }
        public b_Storeroom ToDatabaseObjectForStoreroomList()
        {
            b_Storeroom dbObj = new b_Storeroom();
            dbObj.ClientId = this.ClientId;
            dbObj.StoreroomAuthType = this.StoreroomAuthType;
            dbObj.SiteId = this.SiteId;
            dbObj.PersonnelId = this.PersonnelId;
            return dbObj;
        }
        public void UpdateForStoreroomListFromDatabaseObject(b_Storeroom dbObj)
        {
            this.StoreroomId = dbObj.StoreroomId;
            this.Name = dbObj.Name;
            this.Description = dbObj.Description;
            // Turn on auditing
            AuditEnabled = true;
        }
        #endregion


        public void CheckDuplicateSite(DatabaseKey dbKey)
        {
            Validate<Storeroom>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            Storeroom_ValidateSiteId trans = new Storeroom_ValidateSiteId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Storeroom = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            return errors;
        }

        #region V2-1025
        public List<Storeroom> RetrieveAllStoreroomForLookupList(DatabaseKey dbKey)
        {
            Storeroom_RetrieveAllForLookupListByClientIdSiteId trans = new Storeroom_RetrieveAllForLookupListByClientIdSiteId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Storeroom = this.ToDatabaseObjectForStoreroomLookupList();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectStoreroomLookuplist(trans.StoreroomList));
        }
        public b_Storeroom ToDatabaseObjectForStoreroomLookupList()
        {
            b_Storeroom dbObj = new b_Storeroom();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }
        public List<Storeroom> UpdateFromDatabaseObjectStoreroomLookuplist(List<b_Storeroom> dbObjlist)
        {
            List<Storeroom> temp = new List<Storeroom>();

            Storeroom objStr;

            foreach (b_Storeroom storeroom in dbObjlist)
            {
                objStr = new Storeroom();
                objStr.StoreroomId= storeroom.StoreroomId;
                objStr.Name= storeroom.Name;
                objStr.Description= storeroom.Description;
                temp.Add(objStr);
            }

            return (temp);
        }
        #endregion

        #region V2-1059
        public b_Storeroom ToDateBaseObjectForStoreroomLookuplistChunkSearch()
        {
            b_Storeroom dbObj = this.ToDatabaseObject();
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.StoreroomId = this.StoreroomId;
            dbObj.Name = this.Name;
            dbObj.PersonnelId = this.PersonnelId;
            return dbObj;
        }
        public List<Storeroom> GetAllStoreroomLookupListV2(DatabaseKey dbKey, string TimeZone)
        {
            Storeroom_RetrieveChunkSearchLookupListV2 trans = new Storeroom_RetrieveChunkSearchLookupListV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                
            };
            trans.Storeroom = this.ToDateBaseObjectForStoreroomLookuplistChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfStoreroom = new List<Storeroom>();

            List<Storeroom> Storeroomlist = new List<Storeroom>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Storeroom storeroom in trans.StoreroomList)
            {
                Storeroom tmpStoreroom = new Storeroom();

                tmpStoreroom.UpdateFromDatabaseObjectForStoreroomLookupListChunkSearch(storeroom, TimeZone);
                Storeroomlist.Add(tmpStoreroom);
            }
            return Storeroomlist;
        }
        public void UpdateFromDatabaseObjectForStoreroomLookupListChunkSearch(b_Storeroom dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.totalCount = dbObj.totalCount;
        }
        public void UpdateFromDatabaseObjectRetrieveLookupListBySearchCriteriaV2(b_Vendor dbObj)
        {
            this.StoreroomId = dbObj.StoreroomId;
            this.Name = dbObj.Name;
        }
        #endregion
    }
}
