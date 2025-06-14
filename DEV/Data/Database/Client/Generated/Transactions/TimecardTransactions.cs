/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2021 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;

namespace Database
{
    public class Timecard_TransactionBaseClass : AbstractTransactionManager
    {
        public Timecard_TransactionBaseClass()
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
            if (Timecard == null)
            {
                string message = "Timecard has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
            
            
            // Explicitly set tenant id from dbkey
            this.Timecard.ClientId = this.dbKey.Client.ClientId;
            
        }

        public b_Timecard Timecard { get; set; }
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

    public class Timecard_Create : Timecard_TransactionBaseClass
    {
      
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Timecard.TimecardId > 0)
            {
                string message = "Timecard has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Timecard.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Timecard.TimecardId > 0);
        }
    }

    public class Timecard_Retrieve : Timecard_TransactionBaseClass
    {
    
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Timecard.TimecardId == 0)
            {
                string message = "Timecard has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Timecard.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Timecard_Update : Timecard_TransactionBaseClass
    {
    
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Timecard.TimecardId == 0)
            {
                string message = "Timecard has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Timecard.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
			// If no have been made, no change log is created
			if (ChangeLog != null) {ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class Timecard_Delete : Timecard_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Timecard.TimecardId == 0)
            {
                string message = "Timecard has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Timecard.DeleteFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Timecard_RetrieveAll : AbstractTransactionManager
    {

        public Timecard_RetrieveAll()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

  
        public List<b_Timecard> TimecardList { get; set; }

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
            b_Timecard[] tmpArray = null;
            b_Timecard o = new b_Timecard();
			
			  
            // Explicitly set tenant id from dbkey
               o.ClientId = this.dbKey.Client.ClientId;
            

            o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            TimecardList = new List<b_Timecard>(tmpArray);
        }
    }
}
