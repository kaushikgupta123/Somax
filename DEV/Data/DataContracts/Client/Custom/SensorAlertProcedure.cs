/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2013-2017 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person          Description
* =========== ======== ================ ============================================================
* 2017-Aug-08 SOM-1384 Roger Lawton     Reformatted
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using Common.Extensions;
using Database;
using Database.Business;

namespace DataContracts
{
    public partial class SensorAlertProcedure : DataContractBase, IStoredProcedureValidation
    {
        #region validate types

        #endregion

        #region Public Variables
        public long Creator_PersonneIId { get; set; }
        public DateTime CreateDate { get; set; }
        public string ValidateFor = string.Empty;
        #region V2-536
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int TotalCount { get; set; }
        #endregion
        #endregion
        //public List<SensorAlertProcedure> RetrieveAllForSensorAlertData(DatabaseKey dbKey, Business.Localization.Global loc, string Timezone)
        public List<SensorAlertProcedure> RetrieveAllForSensorAlertData(DatabaseKey dbKey, string Timezone)
        {
            SensorAlertProcedure_RetrieveAllForSensorAlertData trans = new SensorAlertProcedure_RetrieveAllForSensorAlertData()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.SensorAlertProcedure = this.ToDBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<SensorAlertProcedure> sensorAlertProcedureList = new List<SensorAlertProcedure>();
            foreach (b_SensorAlertProcedure sensorAlertProcedure in trans.SensorAlertProcedureList)
            {
                SensorAlertProcedure tmpsensorAlertProcedure = new SensorAlertProcedure();

                //tmpsensorAlertProcedure.UpdateFromDatabaseObjectForRetriveAllForSearch(sensorAlertProcedure, Timezone);
                tmpsensorAlertProcedure.UpdateFromDatabaseObjectForRetriveAllForSearch(sensorAlertProcedure, Timezone);
                sensorAlertProcedureList.Add(tmpsensorAlertProcedure);
            }
            return sensorAlertProcedureList;

        }

        //private void UpdateFromDatabaseObjectForRetriveAllForSearch(b_SensorAlertProcedure dbObj, Global loc, string timezone)
        private void UpdateFromDatabaseObjectForRetriveAllForSearch(b_SensorAlertProcedure dbObj, string timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.CreateDate = dbObj.CreateDate;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(timezone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
        }

        public b_SensorAlertProcedure ToDBaseObjectForRetriveAllForSearch()
        {
            b_SensorAlertProcedure dbObj = this.ToDatabaseObject();
            dbObj.CreateDate = this.CreateDate;
            return dbObj;
        }

        #region Add New Sensor Alert Procedure
        //public void Add_SensorAlertProcedure(DatabaseKey dbKey, Business.Localization.Global loc)
        public void Add_SensorAlertProcedure(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateUserInfo";
            Validate<SensorAlertProcedure>(dbKey);
            if (IsValid)
            {
                SensorAlertProcedure_Create trans = new SensorAlertProcedure_Create()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.SensorAlertProcedure = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.SensorAlertProcedure);
            }
        }

        #endregion

        #region Validation Methods
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "ValidateUserInfo")
            {
                SensorAlertProcedure_ValidateByClientLookupId trans = new SensorAlertProcedure_ValidateByClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.SensorAlertProcedure = this.ToDatabaseObject();
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
        #endregion

        #region V2-536
        public List<SensorAlertProcedure> RetrieveForActiveTableLookupList_V2(DatabaseKey dbKey)
        {
            SensorAlertProcedure_RetrieveForActiveTableLookupList_V2 trans = new SensorAlertProcedure_RetrieveForActiveTableLookupList_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SensorAlertProcedure = this.ToDatabaseObjectRetrieveForActiveTableLookupListV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<SensorAlertProcedure> SensorAlertProcedureList = new List<SensorAlertProcedure>();

            foreach (b_SensorAlertProcedure SensorAlertProcedure in trans.SensorAlertProcedureList)
            {
                SensorAlertProcedure tmpSensorAlertProcedure = new SensorAlertProcedure();
                tmpSensorAlertProcedure.UpdateFromDatabaseObjectRetrieveForActiveTableLookupListV2(SensorAlertProcedure);
                SensorAlertProcedureList.Add(tmpSensorAlertProcedure);
                //this.UpdateFromDatabaseObjectExtended(SensorAlertProcedure);

                //SensorAlertProcedureList.Add(this);
            }
            return SensorAlertProcedureList;
        }
        public b_SensorAlertProcedure ToDatabaseObjectRetrieveForActiveTableLookupListV2()
        {
            b_SensorAlertProcedure dbObj = new b_SensorAlertProcedure();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ClientLookUpId = this.ClientLookUpId;
            dbObj.Description = this.Description;
            dbObj.Type = this.Type;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectRetrieveForActiveTableLookupListV2(b_SensorAlertProcedure dbObj)
        {

            this.ClientId = dbObj.ClientId;
            this.SensorAlertProcedureId = dbObj.SensorAlertProcedureId;
            this.SiteId = dbObj.SiteId;
            this.ClientLookUpId = dbObj.ClientLookUpId;
            this.Description = dbObj.Description;
            this.Type = dbObj.Type;
            this.TotalCount = dbObj.TotalCount;
        }
        #endregion
    }

}
