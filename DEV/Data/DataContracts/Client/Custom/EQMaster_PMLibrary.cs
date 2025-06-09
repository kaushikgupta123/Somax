/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Sep-18 SOM-106  Roger Lawton       Added 
****************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;
using Database.Client.Custom.Transactions;

namespace DataContracts
{
    public partial class EQMaster_PMLibrary : DataContractBase, IStoredProcedureValidation
    {
        #region Private Variables
        private int m_ValidateType;   // 1 - Add, 2 - Save
        #endregion
        #region constants
        private const int Validate_Add = 1;
        private const int Validate_Save = 2;
        #endregion
        public string Description { get; set; }
        public int Frequency { get; set; }
        public string FrequencyType { get; set; }
        public string ClientLookupId { get; set; }


        public static List<EQMaster_PMLibrary> UpdateFromDatabaseObjectList(List<b_EQMaster_PMLibrary> dbObjs)
        {
            List<EQMaster_PMLibrary> result = new List<EQMaster_PMLibrary>();

            foreach (b_EQMaster_PMLibrary dbObj in dbObjs)
            {
                EQMaster_PMLibrary tmp = new EQMaster_PMLibrary();
                tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromDatabaseObjectExtended(b_EQMaster_PMLibrary dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Description = dbObj.Description;
            this.Frequency = dbObj.Frequency;
            this.FrequencyType = dbObj.FrequencyType;
        }


        public b_EQMaster_PMLibrary ToDatabaseObjectExtended()
        {
            b_EQMaster_PMLibrary dbObj = this.ToDatabaseObject();
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Description = this.Description;
            dbObj.Frequency = this.Frequency;
            dbObj.FrequencyType = this.FrequencyType;
            return dbObj;
        }



        public static List<EQMaster_PMLibrary> RetrieveListByEQMasterId(DatabaseKey dbKey, EQMaster_PMLibrary eqmxref)
        {
            EQMaster_PMLibrary_RetrieveListByEQMasterId trans = new EQMaster_PMLibrary_RetrieveListByEQMasterId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.EQMaster_PMLibrary = eqmxref.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return EQMaster_PMLibrary.UpdateFromDatabaseObjectList(trans.EQMasterPMList);
        }

        public void CreateByFK(DatabaseKey dbKey)
        {
            Validate<EQMaster_PMLibrary>(dbKey);

            if (IsValid)
            {
                EQMaster_PMLibrary_CreateByFK trans = new EQMaster_PMLibrary_CreateByFK();
                trans.EQMaster_PMLibrary = this.ToDatabaseObject();
                trans.EQMaster_PMLibrary.ClientLookupId = this.ClientLookupId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.EQMaster_PMLibrary);
            }
        }
        public void UpdateByFK(DatabaseKey dbKey)
        {

            EQMaster_PMLibrary_UpdateByPK trans = new EQMaster_PMLibrary_UpdateByPK();
            trans.EQMaster_PMLibrary = this.ToDatabaseObject();
            trans.EQMaster_PMLibrary.ClientLookupId = this.ClientLookupId;
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.EQMaster_PMLibrary);
        }
        public void ValidateAdd(DatabaseKey dbKey)
        {
            m_ValidateType = Validate_Add;
            Validate<EQMaster_PMLibrary>(dbKey);

        }

        public void ValidateSave(DatabaseKey dbKey)
        {
            m_ValidateType = Validate_Save;
            Validate<EQMaster_PMLibrary>(dbKey);

        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            EQMaster_PMLibrary_ValidateByClientlookupId trans = new EQMaster_PMLibrary_ValidateByClientlookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.EQMaster_PMLibrary = this.ToDatabaseObject();
            trans.EQMaster_PMLibrary.ClientLookupId = this.ClientLookupId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            return errors;
        }
    }
}
