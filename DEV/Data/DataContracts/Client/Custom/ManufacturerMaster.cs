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

using Data.Database;
using Data.Database.Business;
using DataContracts;
using Database.Business;

namespace Data.DataContracts
{
    public partial class ManufacturerMaster : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public string Inactive { get; set; }
        string ValidateFor = string.Empty;

        #endregion


        public List<ManufacturerMaster> RetrieveAll(DatabaseKey dbKey)
        {
            ManufacturerMaster_RetrieveAll trans = new ManufacturerMaster_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ManufacturerMaster> ManufacturerMasterList = new List<ManufacturerMaster>();
            foreach (b_ManufacturerMaster ManufacturerMaster in trans.ManufacturerMasterList)
            {
                ManufacturerMaster tmpManufacturerMaster = new ManufacturerMaster();

                tmpManufacturerMaster.UpdateFromDatabaseObject(ManufacturerMaster);
                ManufacturerMasterList.Add(tmpManufacturerMaster);
            }
            return ManufacturerMasterList;

        }
        public void Validate(DatabaseKey dbKey)
        {

            Validate<ManufacturerMaster>(dbKey);
        }
        public void Add(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForClientlookupId&ManufacturerMaster";
            Validate<ManufacturerMaster>(dbKey);
            if (IsValid)
            {
                ManufacturerMaster_Create trans = new ManufacturerMaster_Create();
                trans.ManufacturerMaster = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.ManufacturerMaster);
            }
        }

        #region Validation Methods
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "ValidateForClientlookupId&ManufacturerMaster")
            {
                ManufacturerMaster_ValidateByClientlookupId trans = new ManufacturerMaster_ValidateByClientlookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.ManufacturerMaster = this.ToDatabaseObject();

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

        public List<ManufacturerMaster> ManufacturerMaster_RetrieveAll_ByInactiveFlag(DatabaseKey dbKey)
        {
            ManufacturerMasterRetrieveAll_ByInactiveFlag trans = new ManufacturerMasterRetrieveAll_ByInactiveFlag()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ManufacturerMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ManufacturerMaster> ManufacturerList = new List<ManufacturerMaster>();
            foreach (b_ManufacturerMaster ManufacturerMaster in trans.ManufacturerList)
            {
                ManufacturerMaster tmpManufacturerMaster = new ManufacturerMaster();

                tmpManufacturerMaster.UpdateFromDatabaseObjectForRetriveByInactiveFlag(ManufacturerMaster);
                ManufacturerList.Add(tmpManufacturerMaster);
            }
            return ManufacturerList;
        }
        public void UpdateFromDatabaseObjectForRetriveByInactiveFlag(b_ManufacturerMaster dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            /*switch (dbObj.InactiveFlag)
            {
                case true:
                    Inactive = loc.ActiveMethod.True;
                    break;
                case false:
                    Inactive = loc.ActiveMethod.False;
                    break;
                default:
                    break;
            }*/
        }





    }
}
