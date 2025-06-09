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
 * Date        Task ID   Person             Description
 * =========== ======== =================== ===================================
 
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Structures;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    public class POReceiptImport2_Validate : POReceiptImport2_TransactionBaseClass
    {
        public POReceiptImport2_Validate()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {

                List<b_StoredProcValidationError> errors = null;
                POReceiptImport2.ValidatePOReceiptImport2(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    
    public class POReceiptImport2_ProcessInterfacePOReceiptImport : POReceiptImport2_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (POReceiptImport2.POReceiptImport2Id == 0)
            {
                string message = "POReceiptImport2 has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            POReceiptImport2.ProcessInterfacePOReceiptImport(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            //if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class POReceiptImport2ExRecPID_Retrieve : POReceiptImport2_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            POReceiptImport2.RetrieveByExRecPIDFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
}
