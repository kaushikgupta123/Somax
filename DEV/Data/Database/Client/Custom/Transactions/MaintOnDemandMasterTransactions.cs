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
using System.Text;
using System.Data.SqlClient;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    public class RetrieveAllMaintOnDemandMaster : AbstractTransactionManager
    {
        public RetrieveAllMaintOnDemandMaster()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public long SiteId { get; set; }

        // Result Sets
        public List<b_MaintOnDemandMaster> MaintOnDemandMasterList { get; set; }
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

                // Call the stored procedure to retrieve the data
                Database.SqlClient.ProcessRow<b_MaintOnDemandMaster> processRow =
                    new Database.SqlClient.ProcessRow<b_MaintOnDemandMaster>(reader =>
                                                                                {
                                                                                    b_MaintOnDemandMaster obj = new b_MaintOnDemandMaster();
                                                                                    obj.LoadFromDatabaseforSearch(reader);
                                                                                    return obj;
                                                                                });

                int tmp;

                MaintOnDemandMasterList = usp_MaintOnDemandMaster_RetrieveList.CallStoredProcedure(command, processRow, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, SiteId, out tmp);

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

    public class Validate_Id : MaintOnDemandMaster_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            MaintOnDemandMaster.ValidateId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


}
