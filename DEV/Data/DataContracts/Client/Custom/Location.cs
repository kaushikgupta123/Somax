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
* Date        Task ID   Person          Description
* =========== ======== ================ ===========================================================
* 2016-Aug-21 SOM-1049 Roger Lawton     Changed to use similar data retrieval functionality as
*                                       other pages 
***************************************************************************************************
 */

using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DataContracts
{
    public partial class Location : DataContractBase, IStoredProcedureValidation
    {
        public List<b_Location> LocationList { get; set; }
        private bool m_validateClientLookupId;     
        #region Transactions

        public List<b_Location> ToDatabaseObjectList()
        {
            List<b_Location> dbObj = new List<b_Location>();
            dbObj = this.LocationList;
            return dbObj;
        }

        [Obsolete]
        public List<Location> RetrieveClientLookupIdBySearchCriteria(DatabaseKey dbKey)
        {
            Location_RetrieveClientLookupIdBySearchCriteria trans = new Location_RetrieveClientLookupIdBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.LocationList = this.ToDatabaseObjectList();
            trans.Location = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Location> locationList = new List<Location>();
            foreach (b_Location location in trans.LocationList)
            {
                Location tmpLocation = new Location()
                {
                    LocationId = location.LocationId,
                    ClientLookupId = location.ClientLookupId,
                    Name = location.Name
                };
                locationList.Add(tmpLocation);
            }

            return locationList;
        }


        public void CreateWithValidation(DatabaseKey dbKey)
        {
            m_validateClientLookupId = true;
            Validate<Location>(dbKey);

            if (IsValid)
            {

                Location_Create trans = new Location_Create();
                trans.Location = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.Location);
            }
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (m_validateClientLookupId)
            {
                LocationValidationByClientLookUpId trans = new LocationValidationByClientLookUpId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.Location = this.ToDatabaseObject();
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

        //public List<Location> RetrieveAllClientLookupId(DatabaseKey dbKey)
        //{      
        //    Location_RetrieveAllClientLookupId trans = new Location_RetrieveAllClientLookupId()
        //    {
        //        CallerUserInfoId = dbKey.User.UserInfoId,
        //        CallerUserName = dbKey.UserName
        //    };
        //    trans.LocationList = this.ToDatabaseObjectList();
        //    trans.Location = this.ToDatabaseObject();
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.Execute();

        //    List<Location> locationList = new List<Location>();
        //    foreach (b_Location location in trans.LocationList)
        //    {
        //        Location tmpLocation = new Location()
        //        {
        //            LocationId = location.LocationId,
        //            ClientLookupId = location.ClientLookupId,
        //            Name = location.Name
        //        };
        //        locationList.Add(tmpLocation);
        //    }

        //    return locationList;
        //}
        public List<Location> RetrieveForSearch(DatabaseKey dbKey)
        {
            Location_RetrieveForSearch trans = new Location_RetrieveForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UseTransaction = false;
            trans.Location = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Location> locationList = new List<Location>();
            foreach (b_Location dbobj in trans.LocationList)
            {
                Location tmp = new Location();
                tmp.UpdateFromDatabaseObject(dbobj);
                locationList.Add(tmp);
            }

            return locationList;
        }

        public List<Location> RetrieveAll(DatabaseKey dbKey)
        {

            Location_RetrieveAll_V2 trans = new Location_RetrieveAll_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Location> LocationList = new List<Location>();
            foreach (b_Location Location in trans.LocationList)
            {
                Location tmpLocation = new Location();

                tmpLocation.UpdateFromDatabaseObject(Location);
                LocationList.Add(tmpLocation);
            }
            return LocationList;
        }

        #region V2
        public List<Location> RetrieveClientLookupIdBySearchCriteriaV2(UserData userData)
        {
            Location_RetrieveClientLookupIdBySearchCriteria_V2 trans = new Location_RetrieveClientLookupIdBySearchCriteria_V2()
            {
                CallerUserInfoId = userData.DatabaseKey.User.UserInfoId,
                CallerUserName = userData.DatabaseKey.UserName,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
            };
            trans.LocationList = this.ToDatabaseObjectList();
            trans.Location = this.ToDatabaseObjectRetrieveClientLookupIdBySearchCriteriaV2();
            trans.dbKey = userData.DatabaseKey.ToTransDbKey();
            trans.Execute();

            List<Location> locationList = new List<Location>();
            foreach (b_Location location in trans.LocationList)
            {
                Location tmpLocation = new Location()
                {
                    LocationId = location.LocationId,
                    ClientLookupId = location.ClientLookupId,
                    Name = location.Name
                };
                locationList.Add(tmpLocation);
            }

            return locationList;
        }
        public b_Location ToDatabaseObjectRetrieveClientLookupIdBySearchCriteriaV2()
        {
            b_Location dbObj = new b_Location();                
            dbObj.ClientLookupId = this.ClientLookupId;         
            return dbObj;
        }
        public void UpdateFromDatabaseObjectRetrieveLookupListBySearchCriteriaV2(b_Location dbObj)
        {
            this.LocationId = dbObj.LocationId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Complex = dbObj.Complex;
            this.Name = dbObj.Name;
            this.Type = dbObj.Type;           
        }
        #endregion
        #endregion

    }
}
