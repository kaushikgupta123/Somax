/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2023 by SOMAX Inc.
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

namespace Database.Transactions
{
    public class RetrieveSiteBillingByClientIdSiteId_V2 : SiteBilling_TransactionBaseClass
    {
    
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (SiteBilling.SiteBillingId == 0)
            //{
            //    string message = "SiteBilling has an invalid ID.";
            //    throw new Exception(message);
            //}
            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            SiteBilling.RetrieveSiteBillingByClientIdSiteId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class SiteBilling_CreateWithCustomClientId : SiteBilling_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SiteBilling.SiteBillingId > 0)
            {
                string message = "SiteBilling has an invalid ID.";
                throw new Exception(message);
            }
            SiteBilling.ClientId = SiteBilling.CustomClientId;
        }
        public override void PerformWorkItem()
        {
            SiteBilling.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(SiteBilling.SiteBillingId > 0);
        }
    }

    public class SiteBilling_UpdateWithCustomClientId : SiteBilling_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SiteBilling.SiteBillingId == 0)
            {
                string message = "SiteBilling has an invalid ID.";
                throw new Exception(message);
            }
            SiteBilling.ClientId = SiteBilling.CustomClientId;
        }

        public override void PerformWorkItem()
        {
            SiteBilling.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    
}
