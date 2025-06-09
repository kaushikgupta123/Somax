/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2018 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 
****************************************************************************************************
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
using Common.Extensions;
//using Data.Common.Interfaces;
//using Localization;
//using DataContracts.PaginatedResultSet;


using Newtonsoft.Json;

namespace DataContracts
{
    [JsonObject]
    public partial class PrevMaintLibrary : DataContractBase, IStoredProcedureValidation
    {
        public DateTime CreateDate { get; set; }
        public string Createby { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ValidateFor = string.Empty;      
        public int InactiveFlg { get; set; }//added by rnj
        public List<PrevMaintLibrary> RetrieveAll(DatabaseKey dbKey)
        {
            PrevMaintLibrary_RetrieveAll trans = new PrevMaintLibrary_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.PrevMaintLibraryList);
        }
        public List<PrevMaintLibrary> RetrieveAllCustom(DatabaseKey dbKey)
        {
            PrevMaintLibrary_RetrieveAllCustom trans = new PrevMaintLibrary_RetrieveAllCustom()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.PrevMaintLibrary = new b_PrevMaintLibrary();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.PrevMaintLibraryList);
        }
        //*** V2-694
        public List<PrevMaintLibrary> RetrieveByInactiveFlag(DatabaseKey dbKey)
        {
            PrevMaintLibrary_RetrieveByInactiveFlag trans = new PrevMaintLibrary_RetrieveByInactiveFlag()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.InactiveFlg = this.InactiveFlg;
            trans.PrevMaintLibrary = new b_PrevMaintLibrary();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.PrevMaintLibraryList);
        }
        
        public static List<PrevMaintLibrary> UpdateFromDatabaseObjectList(List<b_PrevMaintLibrary> dbObjs)
        {
            List<PrevMaintLibrary> result = new List<PrevMaintLibrary>();

            foreach (b_PrevMaintLibrary dbObj in dbObjs)
            {
                PrevMaintLibrary tmp = new PrevMaintLibrary();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.CreateDate = dbObj.CreateDate;
                result.Add(tmp);
            }
            return result;
        }
      
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (ValidateFor == "ValidateByClientLookupId")
            {
                PrevMaintLibrary_ValidateClientLookupIdTransaction trans = new PrevMaintLibrary_ValidateClientLookupIdTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PrevMaintLibrary = this.ToDatabaseObject();
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

            if (ValidateFor == "ValidateForChangeClientLookupId")
            {
                PMLibrary_ValidateByClientlookupIdForChange trans = new PMLibrary_ValidateByClientlookupIdForChange()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PrevMaintLibrary = this.ToDatabaseObject();
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
        //public void  ValidateByClientLookupId(DatabaseKey dbKey, Business.Localization.Global loc)
        //{
        //    ValidateFor = "ValidateByClientLookupId";
        //    Validate<PrevMaintLibrary>(dbKey);

        //}
        public void ValidateByClientLookupId(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateByClientLookupId";
            Validate<PrevMaintLibrary>(dbKey);

        }

        public void ValidateChangeLookupId(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForChangeClientLookupId";
            Validate<PrevMaintLibrary>(dbKey);

        }

        public void UpdateForLibClientLookupId(DatabaseKey dbKey)
        {

            PMLibrary_LibUpdateForClientLookupId_V2 trans = new PMLibrary_LibUpdateForClientLookupId_V2();
            trans.PrevMaintLibrary = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.PrevMaintLibrary);
        }

    }
}
