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
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using  Common.Enumerations;
using  Database.Business;
using  Database.StoredProcedure;

namespace Database
{
    public class CustomSequentialId_RetrieveNext : AbstractTransactionManager
    {
        public b_CustomIdResult CustomIdResult { get; set; }

        public string Key { get; set; }
        public long ClientId { get; set; }
        public long SiteId { get; set; }

        public CustomSequentialId_RetrieveNext()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = true;
            CustomIdResult.RetrieveNextSeed(this.Connection, this.Transaction, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, Key, SiteId);
        }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }
    }


    public class CustomSequentialId_RetrieveByClientIdandSiteIdandKey_V2 : AbstractTransactionManager
    {
        public b_CustomIdResult CustomIdResult { get; set; }
        public  List<b_CustomIdResult> custList = new List<b_CustomIdResult>();
        

        public long SiteId { get; set; }

        public CustomSequentialId_RetrieveByClientIdandSiteIdandKey_V2()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_CustomIdResult> tempList = new List<b_CustomIdResult>();
            base.UseTransaction = true;
            CustomIdResult.RetrieveByClientIdandSiteIdandKey_V2(this.Connection, this.Transaction, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, ref tempList);
            custList = tempList;
        }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }
    }
   
    public class CustomSequentialId_ResetAndRetrieveNext : AbstractTransactionManager
    {
        public b_CustomIdResult CustomIdResult { get; set; }

        public string Key { get; set; }
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public long UpdateIndex { get; set; }

        public CustomSequentialId_ResetAndRetrieveNext()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = true;
            CustomIdResult.ResetAndRetrieveNextSeed(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ClientId, Key, SiteId, UpdateIndex);
        }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }
    }



    public class CustomId_UpdateUpdatePrefixbyKey_V2 : AbstractTransactionManager
    {
        public b_CustomIdResult CustomIdResult { get; set; }

        public CustomId_UpdateUpdatePrefixbyKey_V2()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;

        }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            CustomIdResult.UpdateUpdatePrefixbyKey_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {

        }
    }
}
