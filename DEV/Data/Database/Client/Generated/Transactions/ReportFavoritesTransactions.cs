/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2020 by SOMAX Inc.
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
    public class ReportFavorites_TransactionBaseClass : AbstractTransactionManager
    {
        public ReportFavorites_TransactionBaseClass()
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
            if (ReportFavorites == null)
            {
                string message = "ReportFavorites has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
            
            
        }

        public b_ReportFavorites ReportFavorites { get; set; }
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

    public class ReportFavorites_Create : ReportFavorites_TransactionBaseClass
    {
      
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ReportFavorites.ReportFavoritesId > 0)
            {
                string message = "ReportFavorites has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            ReportFavorites.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(ReportFavorites.ReportFavoritesId > 0);
        }
    }

    public class ReportFavorites_Retrieve : ReportFavorites_TransactionBaseClass
    {
    
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ReportFavorites.ReportFavoritesId == 0)
            {
                string message = "ReportFavorites has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            ReportFavorites.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class ReportFavorites_Update : ReportFavorites_TransactionBaseClass
    {
    
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ReportFavorites.ReportFavoritesId == 0)
            {
                string message = "ReportFavorites has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            ReportFavorites.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
			// If no have been made, no change log is created
			if (ChangeLog != null) {ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class ReportFavorites_Delete : ReportFavorites_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ReportFavorites.ReportFavoritesId == 0)
            {
                string message = "ReportFavorites has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            ReportFavorites.DeleteFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class ReportFavorites_RetrieveAll : AbstractTransactionManager
    {

        public ReportFavorites_RetrieveAll()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

  
        public List<b_ReportFavorites> ReportFavoritesList { get; set; }

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
            b_ReportFavorites[] tmpArray = null;
            b_ReportFavorites o = new b_ReportFavorites();
			
			  

            o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ReportFavoritesList = new List<b_ReportFavorites>(tmpArray);
        }
    }
}
