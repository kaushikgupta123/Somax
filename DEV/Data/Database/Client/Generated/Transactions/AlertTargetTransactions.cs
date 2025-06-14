/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2013 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using Common.Enumerations;
using Database.Business;

namespace Database.Transactions
{
    public class AlertTarget_TransactionBaseClass : AbstractTransactionManager
    {
        public AlertTarget_TransactionBaseClass()
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
            if (AlertTarget == null)
            {
                string message = "AlertTarget has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
            
            
            // Explicitly set tenant id from dbkey
            this.AlertTarget.ClientId = this.dbKey.Client.ClientId;
            
        }

        public b_AlertTarget AlertTarget { get; set; }
		public b_ChangeLog ChangeLog { get; set; }

        public override void PerformWorkItem()
        {
            // 
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

    public class AlertTarget_Create : AlertTarget_TransactionBaseClass
    {
      
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (AlertTarget.AlertTargetId > 0)
            {
                string message = "AlertTarget has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            AlertTarget.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(AlertTarget.AlertTargetId > 0);
        }
    }

    public class AlertTarget_Retrieve : AlertTarget_TransactionBaseClass
    {
    
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (AlertTarget.AlertTargetId == 0)
            {
                string message = "AlertTarget has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            AlertTarget.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class AlertTarget_Update : AlertTarget_TransactionBaseClass
    {
    
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (AlertTarget.AlertTargetId == 0)
            {
                string message = "AlertTarget has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            AlertTarget.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
			// If no have been made, no change log is created
			if (ChangeLog != null) {ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class AlertTarget_Delete : AlertTarget_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (AlertTarget.AlertTargetId == 0)
            {
                string message = "AlertTarget has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            AlertTarget.DeleteFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class AlertTarget_RetrieveAll : AbstractTransactionManager
    {

        public AlertTarget_RetrieveAll()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

  
        public List<b_AlertTarget> AlertTargetList { get; set; }

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
            b_AlertTarget[] tmpArray = null;
            b_AlertTarget o = new b_AlertTarget();
            o.ClientId = dbKey.Client.ClientId;
            o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            AlertTargetList = new List<b_AlertTarget>(tmpArray);
        }
    }
}
