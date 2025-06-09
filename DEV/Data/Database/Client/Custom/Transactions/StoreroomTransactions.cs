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

using Common.Enumerations;

using Database.Business;
using Database.StoredProcedure;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace Database.Transactions

{
    public class Storeroom_RetrieveAuthorizedList : Storeroom_TransactionBaseClass
    {
        public List<b_Storeroom> StoreroomList { get; set; }

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
            b_Storeroom[] tmpArray = null;
            b_Storeroom o = new b_Storeroom();
            o.ClientId = dbKey.Client.ClientId;

            o.RetreiveAuthorizedList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            StoreroomList = new List<b_Storeroom>(tmpArray);

        }
    }

    public class Storeroom_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public Storeroom_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public long SiteId { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Storeroom> StoreroomList { get; set; }
        public int ResultCount { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                int tmp;

                StoreroomList = usp_Storeroom_RetrieveLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, Description, Name, SiteId,
                        OrderColumn, OrderDirection, out tmp);

                ResultCount = tmp;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
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

    //******v2-671
    public class Storeroom_RetrieveChunkSearch : Storeroom_TransactionBaseClass
    {
        public Storeroom_RetrieveChunkSearch()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }
        public List<b_Storeroom> StoreroomList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {

                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                List<b_Storeroom> tmpArray = null;
                b_Storeroom obj = new b_Storeroom();
                obj.ClientId = this.dbKey.Client.ClientId;
                Storeroom.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
                StoreroomList = new List<b_Storeroom>(tmpArray);//rnj
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
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

    public class Storeroom_RetrieveCustom : Storeroom_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Storeroom.StoreroomId == 0)
            {
                string message = "Storeroom has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Storeroom.RetrieveByPKCustomFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    //******

    public class Storeroom_RetrieveStoreroomList : Storeroom_TransactionBaseClass
    {
        public List<b_Storeroom> StoreroomList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            
        }

       
        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {

                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                List<b_Storeroom> tmpArray = null;
                b_Storeroom obj = new b_Storeroom();
                obj.ClientId = this.dbKey.Client.ClientId;
                Storeroom.RetrieveStoreroomListFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
                StoreroomList = new List<b_Storeroom>(tmpArray);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }

    public class Storeroom_ValidateSiteId : Storeroom_TransactionBaseClass
    {
        public Storeroom_ValidateSiteId()
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
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;

                Storeroom.ValidateSiteId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

                StoredProcValidationErrorList = errors;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
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
    #region V2-1025
    public class Storeroom_RetrieveAllForLookupListByClientIdSiteId : Storeroom_TransactionBaseClass
    {

        public List<b_Storeroom> StoreroomList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Storeroom> tmpList = null;
            Storeroom.RetrieveAllStoreroomsForLookupListByClientIdSiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Storeroom, ref tmpList);
            StoreroomList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    #endregion

    #region Storeroom Lookuplist chunk search
    public class Storeroom_RetrieveChunkSearchLookupListV2 : Storeroom_TransactionBaseClass
    {
        public List<b_Storeroom> StoreroomList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Storeroom.StoreroomId > 0)
            {
                string message = "Storeroom has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Storeroom> tmpList = null;
            Storeroom.RetrieveStoreroomLookuplistChunkSearchV2(this.Connection, this.Transaction,
                CallerUserInfoId, CallerUserName, ref tmpList);
            StoreroomList = tmpList;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
}
