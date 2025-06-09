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
* 2014-Sep-05 SOM-304  Roger Lawton       Added Validation, Creat and Update Methods
****************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using System.Data.SqlClient;
using Common.Enumerations;

namespace Database
{
    public class PrevMaintTask_Transaction : AbstractTransactionManager
    {
        public PrevMaintTask_Transaction()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PrevMaintTask == null)
            {
                string message = "PrevMaintTask has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;


            // Explicitly set tenant id from dbkey
            this.PrevMaintTask.ClientId = this.dbKey.Client.ClientId;

        }

        public b_PrevMaintTask PrevMaintTask { get; set; }

        public override void PerformWorkItem()
        {
            // throw new NotImplementedException();
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }

    public class PrevMaintTask_RetrieveByPrevMaintMasterId : PrevMaintTask_Transaction
    {
        public List<b_PrevMaintTask> PrevMaintTaskList { get; set; }
        public long ClientId { get; set; }
        public long PrevMaintMasterId { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_PrevMaintTask> tmpList = new List<b_PrevMaintTask>();
            PrevMaintTask = new b_PrevMaintTask();

            PrevMaintTask.PrevMaintTask_RetrieveByPrevMaintMasterId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ClientId, PrevMaintMasterId, ref tmpList);

            PrevMaintTaskList = tmpList;
        }
    }

    public class PrevMaintTask_RetrieveExtendedInformations : PrevMaintTask_TransactionBaseClass
    {

      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
        if (this.PrevMaintTask.PrevMaintTaskId == 0)
        {
          string message = "PrevMaintTask has an invalid ID.";
          throw new Exception(message);
        }
      }

      public override void PerformWorkItem()
      {
        base.UseTransaction = false;
        PrevMaintTask.RetrieveExtendedInformation(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }
    }

    // Create using lookup ids
    public class PrevMaintTask_CreateByPKForeignKeys : PrevMaintTask_TransactionBaseClass
    {

      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
        if (PrevMaintTask.PrevMaintTaskId > 0)
        {
          string message = "PrevMaintTask has an invalid ID.";
          throw new Exception(message);
        }
      }
      public override void PerformWorkItem()
      {
        PrevMaintTask.CreatyByPKForeignKeys(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }

      public override void Postprocess()
      {
        base.Postprocess();
        System.Diagnostics.Debug.Assert(PrevMaintTask.PrevMaintTaskId > 0);
      }
    }

    public class PrevMaintTask_DeleteRenumber : PrevMaintTask_TransactionBaseClass
    {
      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
        if (PrevMaintTask.PrevMaintTaskId == 0)
        {
          string message = "PrevMaintTask has an invalid ID.";
          throw new Exception(message);
        }
      }

      public override void PerformWorkItem()
      {
        PrevMaintTask.DeleteAndRenumber(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }
    }


    // Update using lookup ids
    public class PrevMaintTask_UpdateByPKForeignKeys : PrevMaintTask_TransactionBaseClass
    {

      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
        if (PrevMaintTask.PrevMaintTaskId == 0)
        {
          string message = "PrevMaintTask has an invalid ID.";
          throw new Exception(message);
        }
      }
      public override void PerformWorkItem()
      {
        PrevMaintTask.UpdateByPKForeignKeys(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }

      public override void Postprocess()
      {
        base.Postprocess();
        System.Diagnostics.Debug.Assert(PrevMaintTask.PrevMaintTaskId > 0);
      }
    }

    // Validate on Insert
    public class PrevMaintTask_ValidateOnInsert : PrevMaintTask_TransactionBaseClass
    {
      public PrevMaintTask_ValidateOnInsert()
      {
      }
      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
      }
      // Result Sets
      public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

      public override void PerformWorkItem()
      {
        List<b_StoredProcValidationError> errors = null;

        PrevMaintTask.ValidateProcessOnInsert(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

        StoredProcValidationErrorList = errors;
      }

      public override void Preprocess()
      {
        // throw new NotImplementedException();
      }

      public override void Postprocess()
      {
        // throw new NotImplementedException();
      }
    }

    // Validate on Save
    public class PrevMaintTask_ValidateOnSave : PrevMaintTask_TransactionBaseClass
    {
      public PrevMaintTask_ValidateOnSave()
      {
      }

      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();

      }

      // Result Sets
      public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

      public override void PerformWorkItem()
      {
        List<b_StoredProcValidationError> errors = null;

        PrevMaintTask.ValidateProcessOnSave(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

        StoredProcValidationErrorList = errors;
      }

      public override void Preprocess()
      {
        // throw new NotImplementedException();
      }

      public override void Postprocess()
      {
        // throw new NotImplementedException();
      }
    }

}

