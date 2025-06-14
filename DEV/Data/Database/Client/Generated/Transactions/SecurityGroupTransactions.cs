/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
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
using Common.Enumerations;
using Database;
using Database.Business;

namespace Database
{
    public class SecurityGroup_TransactionBaseClass : AbstractTransactionManager
    {
        public SecurityGroup_TransactionBaseClass()
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
            if (SecurityGroup == null)
            {
                string message = "SecurityGroup has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
            
            
            // Explicitly set tenant id from dbkey
            this.SecurityGroup.ClientId = this.dbKey.Client.ClientId;
            
        }

        public b_SecurityGroup SecurityGroup { get; set; }
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

    public class SecurityGroup_Create : SecurityGroup_TransactionBaseClass
    {
      
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SecurityGroup.SecurityGroupId > 0)
            {
                string message = "SecurityGroup has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            SecurityGroup.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(SecurityGroup.SecurityGroupId > 0);
        }
    }

    public class SecurityGroup_Retrieve : SecurityGroup_TransactionBaseClass
    {
    
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SecurityGroup.SecurityGroupId == 0)
            {
                string message = "SecurityGroup has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            SecurityGroup.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class SecurityGroup_Update : SecurityGroup_TransactionBaseClass
    {
    
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SecurityGroup.SecurityGroupId == 0)
            {
                string message = "SecurityGroup has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            SecurityGroup.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
			// If no have been made, no change log is created
			if (ChangeLog != null) {ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class SecurityGroup_Delete : SecurityGroup_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SecurityGroup.SecurityGroupId == 0)
            {
                string message = "SecurityGroup has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            SecurityGroup.DeleteFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class SecurityGroup_RetrieveAll : AbstractTransactionManager
    {

        public SecurityGroup_RetrieveAll()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

  
        public List<b_SecurityGroup> SecurityGroupList { get; set; }

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
            b_SecurityGroup[] tmpArray = null;
            b_SecurityGroup o = new b_SecurityGroup();

            o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SecurityGroupList = new List<b_SecurityGroup>(tmpArray);
        }
    }
}
