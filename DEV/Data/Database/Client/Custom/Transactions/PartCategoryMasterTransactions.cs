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
using Data.Database;
using Data.Database.Business;
using System.Data.SqlClient;
using Database;
using Common.Enumerations;
using Data.Database.StoredProcedure;
using Database.Business;

namespace Data.Database
{
  
    public class PartCategoryMaster_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public PartCategoryMaster_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_PartCategoryMaster> PartCategoryMasterList { get; set; }
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
                    Transaction = this.Transaction,
                    CommandTimeout = 180            //SOM-1501

                };

                int tmp=0;

               PartCategoryMasterList = usp_PartCategoryMaster_RetrieveLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Description, 
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

    public class PartCategoryMaster_RetrieveByInactiveFlag : PartCategoryMaster_TransactionBaseClass
    {
        public List<b_PartCategoryMaster> PartCategoryMasterList { get; set; }
       // public b_ManufacturerMaster Manufacturer { get; set; }
        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformWorkItem()
        {
            b_PartCategoryMaster[] tmpArray = null;

            // Explicitly set id from dbkey
            PartCategoryMaster.ClientId = this.dbKey.Client.ClientId;


            PartCategoryMaster.PartCategoryMaster_RetrieveByInactiveFlag(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PartCategoryMasterList = new List<b_PartCategoryMaster>(tmpArray);
        }

    }

    public class PartCategoryMaster_ValidateByClientlookupId : PartCategoryMaster_TransactionBaseClass
    {

        public PartCategoryMaster_ValidateByClientlookupId()
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
                PartCategoryMaster.ValidateByClientLookupId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    public class PartCategoryMaster_RetrieveChunkSearch : PartCategoryMaster_TransactionBaseClass
    {

        public PartCategoryMaster_RetrieveChunkSearch()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public List<b_PartCategoryMaster> CategoryList { get; set; }

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
            List<b_PartCategoryMaster> tmpArray = null;

            // Explicitly set tenant id from dbkey
            PartCategoryMaster.ClientId = this.dbKey.Client.ClientId;
            PartCategoryMaster.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            CategoryList = new List<b_PartCategoryMaster>(tmpArray);
            
        }
    }

    public class PartCategoryMaster_RetrieveLookupListChunkSearch : PartCategoryMaster_TransactionBaseClass
    {

        public PartCategoryMaster_RetrieveLookupListChunkSearch()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public List<b_PartCategoryMaster> CategoryList { get; set; }

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
            List<b_PartCategoryMaster> tmpArray = null;

            // Explicitly set tenant id from dbkey
            PartCategoryMaster.ClientId = this.dbKey.Client.ClientId;
            PartCategoryMaster.RetrieveLookupListChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            CategoryList = new List<b_PartCategoryMaster>(tmpArray);

        }
    }
}
