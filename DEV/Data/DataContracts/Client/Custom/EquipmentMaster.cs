/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2014-2018 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
***************************************************************************************************
*/

using Database;
using Database.Business;
using System;
using System.Collections.Generic;

namespace DataContracts
{
    public partial class EquipmentMaster : DataContractBase, IStoredProcedureValidation
    {

        public string Inactive { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public int Validateflag { get; set; }
      
        #region Transactions 

        public void RetrieveCreateModifyDate(DatabaseKey dbKey)
        {
            EquipmentMaster_RetrieveCreateModifyDate trans = new EquipmentMaster_RetrieveCreateModifyDate()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.EquipmentMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.EquipmentMaster);
            this.CreateBy = trans.EquipmentMaster.CreateBy;
            this.CreateDate = trans.EquipmentMaster.CreateDate;
            this.ModifyBy = trans.EquipmentMaster.ModifyBy;
            this.ModifyDate = trans.EquipmentMaster.ModifyDate;
        }

        public void SaveValidation(DatabaseKey dbkey)
        {
          Validateflag = 0;
          Validate<EquipmentMaster>(dbkey);
        }
        public void createEquipmentMaster(DatabaseKey dbKey)
        {
            Validateflag = 1;
            Validate<EquipmentMaster>(dbKey);

            if (IsValid)
            {
                EquipmentMaster_Create trans = new EquipmentMaster_Create();
                trans.EquipmentMaster = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.EquipmentMaster);
            }
        }
        public List<EquipmentMaster> EquipmentMaster_RetrieveAll(DatabaseKey dbKey)
        {
            EquipmentMaster_RetrieveAll trans = new EquipmentMaster_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<EquipmentMaster> EquipmentMasterList = new List<EquipmentMaster>();
            foreach (b_EquipmentMaster EquipmentMaster in trans.EquipmentMasterList)
            {
                EquipmentMaster tmpEquipmentMaster = new EquipmentMaster();

                tmpEquipmentMaster.UpdateFromDatabaseObjectForRetrive(EquipmentMaster);
                EquipmentMasterList.Add(tmpEquipmentMaster);
            }
            return EquipmentMasterList;
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            EquipmentMaster_ValidateByName trans = new EquipmentMaster_ValidateByName()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.EquipmentMaster = this.ToDatabaseObject();
            trans.EquipmentMaster.Validateflag = Validateflag;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            return errors;
        }

        public void UpdateFromDatabaseObjectForRetrive(b_EquipmentMaster dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            switch (dbObj.InactiveFlag)
            {
                case true:
                    //Inactive = loc.ActiveMethod.True;
                    break;
                case false:
                   // Inactive = loc.ActiveMethod.False;
                    break;
                default:
                    break;
            }

        }
        #endregion
    }
}
