/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date        Task ID   Person             Description
 * =========== ======== =================== ===================================
 * 2011-Dec-14 20110039 Roger Lawton        Added Lookuplist validation
 * 2012-Feb-02 20120001 Roger Lawton        Changed to support additional columns on search
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using Common.Enumerations;

namespace Database.Transactions
{
    public class RetrieveIdFromClientLookupId : AbstractTransactionManager
    {
        public string Table { get; set; }
        public string LookupId { get; set; }
        public long ClientId { get; set; }

        public long Id{ get; set; }

        public RetrieveIdFromClientLookupId()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
            UseTransaction = false;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (string.IsNullOrWhiteSpace(Table) || string.IsNullOrWhiteSpace(LookupId))
            {
                string message = "Table and LookupId must be set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
            ClientId = dbKey.Client.ClientId;
        }

        public override void PerformWorkItem()
        {
            Id = DataBusinessBase.RetrieveIdByClientLookupId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, this.ClientId, this.Table, this.LookupId);
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


    public class RetrieveDistinctColumnFromTable : AbstractTransactionManager
    {
        public string Table { get; set; }
        public string Column { get; set; }
        public long ClientId { get; set; }

        public List<string> ColumnEntries { get; set; }

        public RetrieveDistinctColumnFromTable()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
            UseTransaction = false;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (string.IsNullOrWhiteSpace(Table) || string.IsNullOrWhiteSpace(Column))
            {
                string message = "Table and Column must be set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
            ClientId = dbKey.Client.ClientId;
        }

        public override void PerformWorkItem()
        {
            ColumnEntries = DataBusinessBase.RetrieveDistinctEntriesFromTable(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, this.ClientId, this.Table, this.Column);
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
