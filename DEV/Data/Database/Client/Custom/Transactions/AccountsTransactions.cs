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
using Database;
using Database.Business;
using Database.StoredProcedure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Database
{
    public class Account_RetrieveBySearchCriteria : AbstractTransactionManager
    {
        public Account_RetrieveBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public string Query { get; set; }
        public string Area { get; set; }
        public string Site { get; set; }
        public string Department { get; set; }

        public string ColumnName { get; set; }
        public string ColumnSearchText { get; set; }
        public bool MatchAnywhere { get; set; }

        public string DateSelection { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Account> AccountList { get; set; }
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

                AccountList = usp_Account_RetrieveBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, Query, Site, Area, Department,
                    DateSelection, DateStart, DateEnd, ColumnName, ColumnSearchText, PageNumber, ResultsPerPage, MatchAnywhere, OrderColumn, OrderDirection, out tmp);

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

    public class Account_RetrieveAllBySiteID : AbstractTransactionManager
    {
        public Account_RetrieveAllBySiteID()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public string Query { get; set; }
        public string Area { get; set; }
        public string Site { get; set; }
        public string Department { get; set; }

        public string ColumnName { get; set; }
        public string ColumnSearchText { get; set; }
        public bool MatchAnywhere { get; set; }

        public string DateSelection { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Account> AccountList { get; set; }
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

                AccountList = usp_Account_RetrieveAllBySiteId.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, Query, Site, Area, Department,
                    DateSelection, DateStart, DateEnd, ColumnName, ColumnSearchText, PageNumber, ResultsPerPage, MatchAnywhere, OrderColumn, OrderDirection, out tmp);

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


    public class Account_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public Account_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public long SiteId { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Account> AccountList { get; set; }
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

                AccountList = usp_Account_RetrieveLookupListBySearchCriteria_V2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Name, SiteId,
                        PageNumber, ResultsPerPage, OrderColumn, OrderDirection, out tmp);

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


    public class Account_RetrieveClientLookupIdBySearchCriteria : Account_TransactionBaseClass
    {
        public List<b_Account> AccountList { get; set; }

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
            List<b_Account> tmpList = null;

            Account.RetrieveClientLookupIdBySearchCriteriaFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            AccountList = new List<b_Account>();
            foreach (b_Account tmpObj in tmpList)
            {
                AccountList.Add(tmpObj);
            }
        }
    }



    public class AccountValidationByClientLookUpId : Account_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (ProcedureMaster.ProcedureMasterId > 0)
            //{
            //    string message = "ProcedureMaster has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Account.ValidateByClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    public class Account_RetrieveAllClientLookupId : Account_TransactionBaseClass
    {
        public List<b_Account> AccountList { get; set; }

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
            List<b_Account> tmpList = null;

            Account.RetrieveAllClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            AccountList = new List<b_Account>();
            foreach (b_Account tmpObj in tmpList)
            {
                AccountList.Add(tmpObj);
            }
        }
    }

    public class Account_RetrieveForSearch : Account_TransactionBaseClass
    {
      public List<b_Account> AccountList { get; set; }

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
        b_Account[] tmpList = null;

        Account.RetrieveForSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

        AccountList = new List<b_Account>(tmpList);
      }
    }

    public class Account_RetrieveForSearch_V2 : Account_TransactionBaseClass
    {
        public List<b_Account> AccountList { get; set; }
        public override void PerformWorkItem()
        {
            b_Account[] tmpList = null;
            Account.RetrieveForSearch_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            AccountList = new List<b_Account>(tmpList);
        }
    }
    public class Account_RetrieveForSearchforSuperUser_V2 : Account_TransactionBaseClass
    {
        public List<b_Account> AccountList { get; set; }

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
            b_Account[] tmpList = null;

            Account.RetrieveForSearchforSuperUser_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            AccountList = new List<b_Account>(tmpList);
        }
    }

    
    public class Account_RetrieveAllTemplatesWithClient : Account_TransactionBaseClass
    {


        public List<b_Account> AccountList { get; set; }


        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Account> tmpList = new List<b_Account>();
            /*
            b_Account o = new b_Account();

              o.ClientId = dbKey.Client.ClientId;
              o.RetrieveAllTemplatesWithClient(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            */
            Account.RetrieveAllTemplatesWithClient(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            AccountList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
        public class Account_ChangeClientLookupId : Account_TransactionBaseClass
        {
            public override void PerformLocalValidation()
            {
                base.PerformLocalValidation();
                if (Account.AccountId == 0)
                {
                    string message = "Account has an invalid ID.";
                    throw new Exception(message);
                }
            }

            public override void PerformWorkItem()
            {
                Account.ChangeClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
                if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
            }
        }
    }


    public class Account_RetrieveAll_V2 : AbstractTransactionManager
    {

        public Account_RetrieveAll_V2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Account> AccountList { get; set; }

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
            b_Account[] tmpArray = null;
            b_Account o = new b_Account();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;
            o.SiteId = this.dbKey.User.DefaultSiteId;

            o.RetrieveAll_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            AccountList = new List<b_Account>(tmpArray);
        }
    }

    public class Account_RetrieveByActiveState_V2 : AbstractTransactionManager
    {
        public bool IsActive { get; set; }

        public Account_RetrieveByActiveState_V2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public List<b_Account> AccountList { get; set; }

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
            b_Account[] tmpArray = null;
            b_Account o = new b_Account();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;
            o.SiteId = this.dbKey.User.DefaultSiteId;

            o.RetrieveAll_AccountByActiveState_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, IsActive, ref tmpArray);

            AccountList = new List<b_Account>(tmpArray);
        }
    }

    public class Account_ValidateByInactivateorActivate : Account_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Account.ValidateByInactivateorActivate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    public class Account_UpdateByActivateorInactivate : Account_TransactionBaseClass
    {


        public List<b_Account> AccountList { get; set; }


        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Account> tmpList = new List<b_Account>();
           
            Account.UpdateByActivateorInactivate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
       
    }

    public class Account_RetrieveCustom : Account_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Account.AccountId == 0)
            {
                string message = "Account has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Account.RetrieveByPKCustomFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    #region Account Lookuplist chunk search for Mobile V2
    public class Account_RetrieveChunkSearchLookupListMobile_V2 : Account_TransactionBaseClass
    {
        public List<b_Account> AccountList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Account.AccountId > 0)
            {
                string message = "Account has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Account> tmpList = null;
            Account.RetrieveAccountLookuplistChunkSearchMobile_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            AccountList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
}
