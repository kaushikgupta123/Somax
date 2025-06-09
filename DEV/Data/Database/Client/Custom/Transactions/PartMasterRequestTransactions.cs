/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Changed Parameters
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{


    public class PartMasterRequest_RetrieveExtractData : PartMasterRequest_TransactionBaseClass
    {

        public List<b_PartMasterRequest> partMasterRequestList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_PartMasterRequest> tmpList = null;
            PartMasterRequest.RetrieveExtractData(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            partMasterRequestList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }


    public class PartMasterRequest_RetrieveAllForSearch : PartMasterRequest_TransactionBaseClass
    {

        public List<b_PartMasterRequest> partMasterRequestList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_PartMasterRequest> tmpList = null;
            PartMasterRequest.RetrieveAllForSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            partMasterRequestList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class PartMasterRequest_CreateNewPartNewRequestor : PartMasterRequest_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            PartMasterRequest.CreateNewPartNewRequestor(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PartMasterRequest.PartMasterRequestId > 0);
        }
    }

    public class PartMasterRequestTransactions_RetrieveByPKForPMLocalAdd : PartMasterRequest_TransactionBaseClass
    {
        public string CompleteBy { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartMasterRequest.PartMasterRequestId == 0)
            {
                string message = "PartMasterRequestId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PartMasterRequest.RetrieveByPKForPMLocalAddFromDB(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }
    public class PartMasterRequestTransactions_RetrieveByForeignKeys : PartMasterRequest_TransactionBaseClass
    {
        public string CompleteBy { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartMasterRequest.PartMasterRequestId == 0)
            {
                string message = "PartMasterRequestId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PartMasterRequest.RetrieveByForeignKeysFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }

    public class PartMasterRequest_UpdateByPK : PartMasterRequest_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartMasterRequest.PartMasterRequestId == 0)
            {
                string message = "PartMasterRequest has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            PartMasterRequest.UpdateForPMLocalAddition(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class PartMasterRequest_UpdateForDenied : PartMasterRequest_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartMasterRequest.PartMasterRequestId == 0)
            {
                string message = "PartMasterRequest has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            PartMasterRequest.UpdateForDenied(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }


    public class PartMasterRequest_ValidationPartMasterNumberLookup : PartMasterRequest_TransactionBaseClass
    {

        public PartMasterRequest_ValidationPartMasterNumberLookup()
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
                PartMasterRequest.ValidationPartMasterNumberLookup(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    public class PartMasterRequest_ValidationSomaxPartLookup : PartMasterRequest_TransactionBaseClass
    {

        public PartMasterRequest_ValidationSomaxPartLookup()
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
                PartMasterRequest.ValidationSomaxPartLookup(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    public class PartMasterRequest_CreateByFK : PartMasterRequest_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            PartMasterRequest.CreateByFK(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PartMasterRequest.PartMasterRequestId > 0);
        }
    }
}

