/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2011 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
* 2011-Dec-09 20110019 Roger Lawton        Added ClientLookupId to search results
* 2011-Dec-14 20110039 Roger Lawton        Added Lookuplist validation
* 2014-Aug-10 SOM-280  Roger Lawton        Modified UpdateFromDataObjectList to include 
*                                          LaborAccountClientLookupId
* 2015-Mar-03 SOM-590  Roger Lawton        Removed validation on columns we do not support
* 2015-Sep-14 SOM-805  Roger Lawton        Location - Show Location.ClientLookupId if FACILITIES
***************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Database;
using Database.Business;
using Common.Structures;
//using Business.Localization;
//using DevExpress.Data;
//using DevExpress.Data.Filtering;
using Common.Extensions;

namespace DataContracts
{
    public partial class SanOnDemandMaster : DataContractBase , IStoredProcedureValidation
    {
        #region Properties
        public DateTime CreateDate { get; set; }

        string ValidateFor = string.Empty;
        #endregion


        public List<SanOnDemandMaster> Retrieve_SanOnDemandMaster_ByFilterCriteria(DatabaseKey dbKey, string Timezone)
        {
            SanOnDemandMaster_RetrieveAllForSearch trans = new SanOnDemandMaster_RetrieveAllForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanOnDemandMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<SanOnDemandMaster> SanOnDemandMasterList = new List<SanOnDemandMaster>();
            foreach (b_SanOnDemandMaster SanOnDemandMaster in trans.SanOnDemandMasterList)
            {
                SanOnDemandMaster tmpSanOnDemandMaster = new SanOnDemandMaster();

                tmpSanOnDemandMaster.UpdateFromDatabaseObjectForRetriveAllForSearch(SanOnDemandMaster, Timezone);
                SanOnDemandMasterList.Add(tmpSanOnDemandMaster);
            }
            return SanOnDemandMasterList;

        }
        public List<SanOnDemandMaster> Retrieve_SanOnDemandMaster_ByFilterCriteria_V2(DatabaseKey dbKey, string Timezone)
        {
            SanOnDemandMaster_RetrieveAllForSearch_V2 trans = new SanOnDemandMaster_RetrieveAllForSearch_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanOnDemandMaster = this.ToDatabaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<SanOnDemandMaster> SanOnDemandMasterList = new List<SanOnDemandMaster>();
            foreach (b_SanOnDemandMaster SanOnDemandMaster in trans.SanOnDemandMasterList)
            {
                SanOnDemandMaster tmpSanOnDemandMaster = new SanOnDemandMaster();

                tmpSanOnDemandMaster.UpdateFromDatabaseObjectForRetriveAllForSearch(SanOnDemandMaster, Timezone);
                SanOnDemandMasterList.Add(tmpSanOnDemandMaster);
            }
            return SanOnDemandMasterList;

        }

       public b_SanOnDemandMaster ToDatabaseObjectForRetriveAllForSearch()
        {
            b_SanOnDemandMaster dbObj = new b_SanOnDemandMaster();
            dbObj.ClientId = this.ClientId;     
            dbObj.SiteId = this.SiteId;         
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_SanOnDemandMaster dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.CreateDate = dbObj.CreateDate;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
        }
        public void CreateByPKForeignKeys(DatabaseKey dbKey)
        {
            Validate<SanOnDemandMaster>(dbKey);

            if (IsValid)
            {
                SanOnDemandMaster_CreateByForeignKeys trans = new SanOnDemandMaster_CreateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.SanOnDemandMaster = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.SanOnDemandMaster);
            }
        }
        public void RetrieveBy_SanOnDemandMasterId(DatabaseKey dbKey)
        {
            SanOnDemandMaster_RetrieveBy_SanOnDemandMasterId trans = new SanOnDemandMaster_RetrieveBy_SanOnDemandMasterId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.SanOnDemandMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.SanOnDemandMaster);

        }

        public void UpdateBy_SanOnDemandMasterId(DatabaseKey dbKey)
        {

            SanOnDemandMaster_UpdateBy_SanOnDemandMasterId trans = new SanOnDemandMaster_UpdateBy_SanOnDemandMasterId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            }; 

            trans.SanOnDemandMaster = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.SanOnDemandMaster);
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

                SanOnDemandMaster_ValidateClientLookupId trans = new SanOnDemandMaster_ValidateClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.SanOnDemandMaster = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            


            return errors;
        }

        public List<SanOnDemandMaster> Retrieve_SanOnDemandMaster_ByInActiveFlag(DatabaseKey dbKey,string Timezone)
        {
            SanOnDemandMaster_RetrieveAllByInactiveFlag trans = new SanOnDemandMaster_RetrieveAllByInactiveFlag()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanOnDemandMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<SanOnDemandMaster> SanOnDemandMasterList = new List<SanOnDemandMaster>();
            foreach (b_SanOnDemandMaster SanOnDemandMaster in trans.SanOnDemandMasterList)
            {
                SanOnDemandMaster tmpSanOnDemandMaster = new SanOnDemandMaster();

                tmpSanOnDemandMaster.UpdateFromDatabaseObjectForRetriveAllForSearch(SanOnDemandMaster, Timezone);
                SanOnDemandMasterList.Add(tmpSanOnDemandMaster);
            }
            return SanOnDemandMasterList;

        }
    }
}
