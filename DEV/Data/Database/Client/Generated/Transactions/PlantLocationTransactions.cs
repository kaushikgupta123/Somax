/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2017 by SOMAX Inc.
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
    public class PlantLocation_TransactionBaseClass : AbstractTransactionManager
    {
        public PlantLocation_TransactionBaseClass()
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
            if (PlantLocation == null)
            {
                string message = "PlantLocation has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
            
            
            // Explicitly set tenant id from dbkey
            this.PlantLocation.ClientId = this.dbKey.Client.ClientId;
            
        }

        public b_PlantLocation PlantLocation { get; set; }
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

    public class PlantLocation_Create : PlantLocation_TransactionBaseClass
    {
      
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PlantLocation.PlantLocationId > 0)
            {
                string message = "PlantLocation has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PlantLocation.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PlantLocation.PlantLocationId > 0);
        }
    }

    public class PlantLocation_Retrieve : PlantLocation_TransactionBaseClass
    {
    
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PlantLocation.PlantLocationId == 0)
            {
                string message = "PlantLocation has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PlantLocation.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class PlantLocation_Update : PlantLocation_TransactionBaseClass
    {
    
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PlantLocation.PlantLocationId == 0)
            {
                string message = "PlantLocation has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            PlantLocation.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
			// If no have been made, no change log is created
			if (ChangeLog != null) {ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class PlantLocation_Delete : PlantLocation_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PlantLocation.PlantLocationId == 0)
            {
                string message = "PlantLocation has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            PlantLocation.DeleteFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class PlantLocation_RetrieveAll : AbstractTransactionManager
    {

        public PlantLocation_RetrieveAll()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

  
        public List<b_PlantLocation> PlantLocationList { get; set; }

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
            b_PlantLocation[] tmpArray = null;
            b_PlantLocation o = new b_PlantLocation();
			
			  
            // Explicitly set tenant id from dbkey
               o.ClientId = this.dbKey.Client.ClientId;
            

            o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PlantLocationList = new List<b_PlantLocation>(tmpArray);
        }
    }
}
