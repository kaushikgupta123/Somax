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

using Newtonsoft.Json;

namespace DataContracts
{
    [JsonObject]
    public partial class MasterSanLibrary : DataContractBase, IStoredProcedureValidation
    {
        public DateTime CreateDate { get; set; }
        public string Createby { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ValidateFor = string.Empty;

     
        public List<MasterSanLibrary> RetrieveAll(DatabaseKey dbKey)
        {
            MasterSanLibrary_RetrieveAll trans = new MasterSanLibrary_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.MasterSanLibraryList);
        }
        public List<MasterSanLibrary> RetrieveAllCustom(DatabaseKey dbKey)
        {
            MasterSanLibrary_RetrieveAllCustom trans = new MasterSanLibrary_RetrieveAllCustom()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.MasterSanLibrary = new b_MasterSanLibrary();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.MasterSanLibraryList);
        }
        public static List<MasterSanLibrary> UpdateFromDatabaseObjectList(List<b_MasterSanLibrary> dbObjs)
        {
            List<MasterSanLibrary> result = new List<MasterSanLibrary>();

            foreach (b_MasterSanLibrary dbObj in dbObjs)
            {
                MasterSanLibrary tmp = new MasterSanLibrary();
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
                MasterSanLibrary_ValidateClientLookupIdTransaction trans = new MasterSanLibrary_ValidateClientLookupIdTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.MasterSanLibrary = this.ToDatabaseObject();
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
        public void  ValidateByClientLookupId(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateByClientLookupId";
            Validate<MasterSanLibrary>(dbKey);
           
        }

       
    }
}
