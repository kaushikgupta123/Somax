/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* PrevMaintTask.cs (DataContract)
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Jul-29 SOM-259  Roger Lawton       Added AssignedTo_ClientLookupId and ChargeTo_ClientLookupId
*                                         Properties
*                                         Added ToDataBaseObjectExtended method
*                                         Added UpdateFromDataBaseObjectExtended method
*                                         Modified PrevMaintTaskRetrieveByPrevMaintMasterId method
*                                         to use the new extended methods.
*                                         Set the UseTransaction property to false - only reading 
*                                         data - not writing - do not need a transaction
* 2014-Sep-04 SOM-304  Roger Lawton       Added Validation Methods
****************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Reflection;

using Database;
using Database.Business;

namespace DataContracts
{
    public partial class PrevMaintTask : DataContractBase, IStoredProcedureValidation

    {
      #region Private Variables
      private int m_ValidateType;   // 1 - Add, 2 - Save
      #endregion
      #region constants
      private const int Validate_Add = 1;
      private const int Validate_Save = 2;
      #endregion

      #region properties
      public string AssignedTo_ClientLookupId { get; set; }
      public string ChargeToClientLookupId { get; set; }
      public long SiteId { get; set; }
      #endregion properties

      public List<PrevMaintTask> PrevMaintTaskRetrieveByPrevMaintMasterId(DatabaseKey dbKey, long PrevMaintMasterId)
        {
            List<PrevMaintTask> PrevMaintTaskList = new List<PrevMaintTask>();

            Database.PrevMaintTask_RetrieveByPrevMaintMasterId trans =
                new Database.PrevMaintTask_RetrieveByPrevMaintMasterId();


            trans.PrevMaintTask = new b_PrevMaintTask();
            trans.UseTransaction = false;
            trans.ClientId = dbKey.Personnel.ClientId;
            trans.PrevMaintMasterId = PrevMaintMasterId;
            trans.CallerUserInfoId = dbKey.User.UserInfoId;
            trans.CallerUserName = dbKey.UserName;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            PrevMaintTask task;
            trans.PrevMaintTaskList.ForEach(b_obj =>
            {
                task = new PrevMaintTask();
                task.UpdateFromDataBaseObjectExtended(b_obj);
                PrevMaintTaskList.Add(task);
            });

            return PrevMaintTaskList;
        }

      public void RetrieveExtendedInformation(DatabaseKey dbKey)
      {
        PrevMaintTask_RetrieveExtendedInformations trans = new PrevMaintTask_RetrieveExtendedInformations();
        trans.PrevMaintTask = this.ToDataBaseObjectExtended();
        trans.dbKey = dbKey.ToTransDbKey();
        trans.Execute();
        UpdateFromDataBaseObjectExtended(trans.PrevMaintTask);
      }

      public b_PrevMaintTask ToDataBaseObjectExtended()
      {
        b_PrevMaintTask dbObj = this.ToDatabaseObject();
        dbObj.AssignedTo_ClientLookupId = this.AssignedTo_ClientLookupId;
        dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
        dbObj.SiteId = this.SiteId;
        return dbObj;
      }

      public void UpdateFromDataBaseObjectExtended(b_PrevMaintTask dbObj)
      {
        this.UpdateFromDatabaseObject(dbObj);
        this.AssignedTo_ClientLookupId = dbObj.AssignedTo_ClientLookupId;
        this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
        this.SiteId = dbObj.SiteId;
      }

      public void CreateByForeignKeys(DatabaseKey dbKey)
      {
        PrevMaintTask_CreateByPKForeignKeys trans = new PrevMaintTask_CreateByPKForeignKeys();
        trans.PrevMaintTask = this.ToDataBaseObjectExtended();
        trans.dbKey = dbKey.ToTransDbKey();
        trans.Execute();

        UpdateFromDataBaseObjectExtended(trans.PrevMaintTask);
      }

      public void UpdateByForeignKeys(DatabaseKey dbKey)
      {
        PrevMaintTask_UpdateByPKForeignKeys trans = new PrevMaintTask_UpdateByPKForeignKeys();
        trans.PrevMaintTask = this.ToDataBaseObjectExtended();
        trans.dbKey = dbKey.ToTransDbKey();
        trans.Execute();

        UpdateFromDataBaseObjectExtended(trans.PrevMaintTask);
      }

      public void DeleteRenumber(DatabaseKey dbKey)
      {
        PrevMaintTask_DeleteRenumber trans = new PrevMaintTask_DeleteRenumber();
        trans.PrevMaintTask = this.ToDatabaseObject();
        trans.dbKey = dbKey.ToTransDbKey();
        trans.Execute();
      }

      public void ValidateAdd(DatabaseKey dbKey)
      {
        m_ValidateType = Validate_Add;
        Validate<PrevMaintTask>(dbKey);

      }

      public void ValidateSave(DatabaseKey dbKey)
      {
        m_ValidateType = Validate_Save;
        Validate<PrevMaintTask>(dbKey);

      }

      public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
      {
        List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
        // Add
        if (m_ValidateType == Validate_Add)
        {
          PrevMaintTask_ValidateOnInsert trans = new PrevMaintTask_ValidateOnInsert()
          {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName,
          };
          trans.PrevMaintTask = this.ToDataBaseObjectExtended();
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
        // Save 
        if (m_ValidateType == Validate_Save)
        {
          PrevMaintTask_ValidateOnSave trans = new PrevMaintTask_ValidateOnSave()
          {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName,
          };
          trans.PrevMaintTask = this.ToDataBaseObjectExtended();
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
    }
}
