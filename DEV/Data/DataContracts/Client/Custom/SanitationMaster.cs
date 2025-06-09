/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
* 2015-Mar-12 SOM-585  Roger Lawton        Added Items to support sanitation
***************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;
using Common.Extensions;

namespace DataContracts
{
    public partial class SanitationMaster : DataContractBase, IStoredProcedureValidation
    {
        #region Private Variables
        private bool m_validateClientLookupId;
        public string ChargeToClientLookupId{get;set;}
        public string ChargeToName { get; set; }
        public string Assigned{get;set;}
        public long PlantLocationId { get; set; }
        #endregion

        #region Transactions
        public List<SanitationMaster> RetrieveAll(DatabaseKey dbKey)
        {
            SanitationMaster_RetrieveAll trans = new SanitationMaster_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SanitationMasterList);
        }

        public List<SanitationMaster> RetrieveToSearchCriteria(DatabaseKey dbKey)
        {
            SanitationMasterTransactions_RetrieveToSearchCriteria trans = new SanitationMasterTransactions_RetrieveToSearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SanitationMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The Next Due Date does not need to be converted to the local time zone - we are only interested in the date not the time

            List<SanitationMaster> SanitationMasterList = new List<SanitationMaster>();
            SanitationMaster temp;
           
            trans.SearchResult.ForEach(x =>
            {
                temp = new SanitationMaster();
                temp.UpdateFromDatabaseObject(x);
                temp.ChargeToClientLookupId = x.ChargeToClientLookupId;
                temp.Assigned = x.Assigned;
                SanitationMasterList.Add(temp);
            });

            return SanitationMasterList;
        }
        #endregion

        public static List<SanitationMaster> UpdateFromDatabaseObjectList(List<b_SanitationMaster> dbObjs)
        {
            List<SanitationMaster> result = new List<SanitationMaster>();

            foreach (b_SanitationMaster dbObj in dbObjs)
            {
                SanitationMaster tmp = new SanitationMaster();
                tmp.UpdateFromDatabaseObject(dbObj);
                 result.Add(tmp);
            }
            return result;
        }

        public void SanitationMaster_SaveAs(DatabaseKey dbKey,long SanitationMasterId)
        {
              Validate<SanitationMaster>(dbKey);
              if (IsValid)
              {
                  SanitationMasterTransactions_SaveAs trans = new SanitationMasterTransactions_SaveAs()
                  {
                      CallerUserInfoId = dbKey.User.UserInfoId,
                      CallerUserName = dbKey.UserName,
                  };
                  trans.SanitationMaster = new b_SanitationMaster();
                  trans.SanitationMaster = this.ToDatabaseObject();
                  trans.SanitationMaster.SanitationMasterId = SanitationMasterId;
                  trans.SanitationMaster.ChargeToClientLookupId = this.ChargeToClientLookupId;
                  trans.dbKey = dbKey.ToTransDbKey();
                  trans.Execute();

                  this.UpdateFromDatabaseObject(trans.SanitationMaster);
              }
        }

        public void SanitationMaster_Delete(DatabaseKey dbKey)
        {
            SanitationMasterTransactions_Delete trans = new SanitationMasterTransactions_Delete()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SanitationMaster = new b_SanitationMaster();
            //trans.PrevMaintMaster.PrevMaintMasterId = this.PrevMaintMasterId;
            trans.SanitationMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }

        public void SanitationMaster_CreateByFK(DatabaseKey dbKey)
        {
            Validate<SanitationMaster>(dbKey);
            if (IsValid)
            {
                SanitationMasterTransactions_CreateByFK trans = new SanitationMasterTransactions_CreateByFK()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.SanitationMaster = new b_SanitationMaster();
                trans.SanitationMaster = this.ToDatabaseObject();
                trans.SanitationMaster.ChargeToClientLookupId = this.ChargeToClientLookupId;
                trans.SanitationMaster.PlantLocationId = this.PlantLocationId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                this.UpdateFromDatabaseObject(trans.SanitationMaster);
            }
        }

        public void SanitationMaster_RetrieveByFK(DatabaseKey dbKey)
        {
            SanitationMasterTransactions_RetrieveByFK trans = new SanitationMasterTransactions_RetrieveByFK();
            trans.SanitationMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject_RetrieveByFK(trans.SanitationMaster);
            this.ChargeToClientLookupId=trans.SanitationMaster.ChargeToClientLookupId;
        }

        public void UpdateFromDatabaseObject_RetrieveByFK(b_SanitationMaster dbObj)
        {
          this.UpdateFromDatabaseObject(dbObj);
          this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
          this.ChargeToName = dbObj.ChargeToName;
          this.Assigned = dbObj.Assigned;

        }

        public void SanitationMaster_UpdateByFK(DatabaseKey dbKey)
        {
             Validate<SanitationMaster>(dbKey);
             if (IsValid)
             {
                 SanitationMasterTransactions_UpdateByFK trans = new SanitationMasterTransactions_UpdateByFK();
                 trans.SanitationMaster = this.ToDatabaseObject();
                 trans.ChangeLog = GetChangeLogObject(dbKey);
                 trans.SanitationMaster.ChargeToClientLookupId = this.ChargeToClientLookupId;
                trans.SanitationMaster.PlantLocationId = this.PlantLocationId;
                trans.dbKey = dbKey.ToTransDbKey();
                 trans.Execute();

                 // The create procedure changed the Update Index.
                 UpdateFromDatabaseObject(trans.SanitationMaster);
             }
        }

        #region validation 
        public void ValidateAdd(DatabaseKey dbKey)
        {
            m_validateClientLookupId = true;
            Validate<SanitationMaster>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();


            SanitationMasterTransactions_Validate trans = new SanitationMasterTransactions_Validate
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanitationMaster = this.ToDatabaseObject();
            trans.SanitationMaster.ChargeToClientLookupId = this.ChargeToClientLookupId;
            trans.SanitationMaster.PlantLocationId = this.PlantLocationId;
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


            return errors;
            //List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            //if (m_validateClientLookupId)
            //{
            //    SanitationMasterTransactions_ValidateAdd trans = new SanitationMasterTransactions_ValidateAdd
            //    {
            //        CallerUserInfoId = dbKey.User.UserInfoId,
            //        CallerUserName = dbKey.UserName,
            //    };
            //    trans.SanitationMaster = this.ToDatabaseObject();
            //    trans.dbKey = dbKey.ToTransDbKey();
            //    trans.Execute();


            //    if (trans.StoredProcValidationErrorList != null)
            //    {
            //        foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
            //        {
            //            StoredProcValidationError tmp = new StoredProcValidationError();
            //            tmp.UpdateFromDatabaseObject(error);
            //            errors.Add(tmp);
            //        }
            //    }
            //}

            //return errors;
        }
        #endregion
    }
}
