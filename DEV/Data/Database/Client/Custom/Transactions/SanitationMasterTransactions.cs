/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
* 2015-Mar-12 SOM-585  Roger Lawton        Added Items to support sanitation
***************************************************************************************************
*/
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using Database.StoredProcedure;

namespace Database
{
    public class SanitationMasterTransactions_RetrieveToSearchCriteria : SanitationMaster_TransactionBaseClass
    {
        #region Properties

        public List<b_SanitationMaster> SearchResult;
        #endregion

        #region Constructor
        public SanitationMasterTransactions_RetrieveToSearchCriteria()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }
        #endregion

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }



        public override void PerformWorkItem()
        {
            base.UseTransaction = false;

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

                // Call the stored procedure to retrieve the data
                SearchResult = usp_SanitationMaster_RetrieveToSearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName,this.SanitationMaster);
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


        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }

    }
    // RKL - NOT USED ANYWHERE THAT I CAN FIND
    /*
    public class SanitationMasterTransactions_ValidateAdd : SanitationMaster_TransactionBaseClass
    {
        public SanitationMasterTransactions_ValidateAdd()
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

                SanitationMaster.ValidateProcessFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
                        ref errors);

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
    */
    public class SanitationMasterTransactions_SaveAs : SanitationMaster_TransactionBaseClass
    {
        public SanitationMasterTransactions_SaveAs()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets

        public override void PerformWorkItem()
        {
            SanitationMaster.SanitationMaster_SaveAs
                (this.Connection,
                this.Transaction,
                this.CallerUserInfoId,
                this.CallerUserName);
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


    public class SanitationMasterTransactions_Delete : SanitationMaster_TransactionBaseClass
    {
        public SanitationMasterTransactions_Delete()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets

        public override void PerformWorkItem()
        {
            SanitationMaster.SanitationMaster_Delete
                (this.Connection,
                this.Transaction,
                this.CallerUserInfoId,
                this.CallerUserName);
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

    public class SanitationMasterTransactions_CreateByFK : SanitationMaster_TransactionBaseClass
    {
        public SanitationMasterTransactions_CreateByFK()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationMaster.SanitationMasterId > 0)
            {
                string message = "SanitationMaster has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            SanitationMaster.SanitationMaster_CreateByFK(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(SanitationMaster.SanitationMasterId > 0);
        }
    }

    public class SanitationMasterTransactions_RetrieveByFK : SanitationMaster_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationMaster.SanitationMasterId == 0)
            {
                string message = "SanitationMaster has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            SanitationMaster.SanitationMaster_RetrieveByFK(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName);
        }
    }
    public class SanitationMasterTransactions_UpdateByFK : SanitationMaster_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationMaster.SanitationMasterId == 0)
            {
                string message = "SanitationMaster has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            SanitationMaster.SanitationMaster_UpdateByFK(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class SanitationMasterTransactions_Validate : SanitationMaster_TransactionBaseClass
    {
        // public bool CreateMode { get; set; }
        // public string Requestor_PersonnelClientLookupId { get; set; }

        public SanitationMasterTransactions_Validate()
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

                SanitationMaster.ValidateSanitationMaster(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
}
