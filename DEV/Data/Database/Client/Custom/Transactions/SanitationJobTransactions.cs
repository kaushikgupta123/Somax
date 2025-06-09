/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014-2018 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person        Description
* =========== ======== ============== ==============================================================
* 2015-Mar-21 SOM-585  Roger Lawton   Changed Parameters
* 2018-Jun-20 SOM-1628 Roger Lawton   Added Update for Interface and Validate for Interface
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
    public class SanitationJobTransactions_CustomCreate : SanitationJob_TransactionBaseClass
    {
        public long SanitationJobBatchEntryId { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId > 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_SanitationJob b_SanitationJob = new b_SanitationJob();
            //b_SanitationJob.ClientLookupId = SanitationJob.ClientLookupId;
            //b_SanitationJob.Requestor_PersonnelId = SanitationJob.Requestor_PersonnelId;
            b_SanitationJob.SanitationJob_CustomCreate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, SanitationJobBatchEntryId, ref b_SanitationJob);
            SanitationJob = b_SanitationJob;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class SanitationJob_RetrieveAllForVerificationWorkBench : SanitationJob_TransactionBaseClass
    {

        public List<b_SanitationJob> SanitationVerificationJobList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId > 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_SanitationJob> tmpList = null;
            SanitationJob.RetrieveAllForVerificationWorkBench(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            SanitationVerificationJobList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class SanitationJobTransactions_RetieveBySearchCriteria : SanitationJob_TransactionBaseClass
    {

        public List<b_SanitationJob> SanitationJobList { get; set; }

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
            b_SanitationJob[] tmpArray = null;
            //SanitationJob
            // Explicitly set tenant id from dbkey
            SanitationJob.ClientId = this.dbKey.Client.ClientId;


            SanitationJob.SanitationJob_RetieveBySearchCriteria(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SanitationJobList = new List<b_SanitationJob>(tmpArray);
        }
    }

    public class SanitationJobTransactions_UpdateForComplete : SanitationJob_TransactionBaseClass
    {
        public long CallerUserPersonnelId { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId == 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            SanitationJob.SanitationJob_UpdateForComplete(this.Connection, this.Transaction, CallerUserPersonnelId, CallerUserName);
        }
    }
    public class SanitationJobTransactions_UpdateForInterface : SanitationJob_TransactionBaseClass
    {
      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
      }

      public override void PerformWorkItem()
      {
        SanitationJob.SanitationJob_UpdateForInterface(this.Connection, this.Transaction, CallerUserInfoId,CallerUserName);
      }
    }


  public class SanitationJobTransactions_UpdateForReschedule : SanitationJob_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId == 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            SanitationJob.SanitationJob_UpdateForReschedule(this.Connection, this.Transaction, CallerUserName);
        }
    }

    public class SanitationJobTransactions_UpdateForCancel : SanitationJob_TransactionBaseClass
    {
        public long CallerUserPersonnelId { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId == 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            SanitationJob.SanitationJob_UpdateForCancel(this.Connection, this.Transaction, CallerUserPersonnelId, CallerUserName);
        }
    }

    public class SanitationJobTransactions_UpdateForReopen : SanitationJob_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId == 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            SanitationJob.SanitationJob_UpdateForReopen(this.Connection, this.Transaction, CallerUserName);
        }
    }
    public class SanitationJobTransactions_RetieveAllByFk : SanitationJob_TransactionBaseClass
    {

        public List<b_SanitationJob> SanitationJobList { get; set; }

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
            b_SanitationJob[] tmpArray = null;
            //b_SanitationJob o = new b_SanitationJob();

            //SanitationJob
            // Explicitly set tenant id from dbkey
            SanitationJob.ClientId = this.dbKey.Client.ClientId;


            SanitationJob.SanitationJob_RetieveAllByFK(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SanitationJobList = new List<b_SanitationJob>(tmpArray);
        }
    }
    public class SanitationJobTransactions_RetrieveByForeignKeys : SanitationJob_TransactionBaseClass
    {
        public string CompleteBy { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId == 0)
            {
                string message = "SanitationMasterId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            SanitationJob.RetrieveByForeignKeysFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }

    public class SanitationJobTransactions_RetrieveByV2 : SanitationJob_TransactionBaseClass
    {
        public string CompleteBy { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId == 0)
            {
                string message = "SanitationMasterId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            SanitationJob.RetrieveByV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }
    public class SanitationJobTransactions_CreateByFk : SanitationJob_TransactionBaseClass
    {


        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId > 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            SanitationJob.SanitationJob_CreateByFK(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class SanitationJob_Validate : SanitationJob_TransactionBaseClass
    {
        public string ValidateType { get; set; }

        public SanitationJob_Validate()
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

                if (this.ValidateType == "Complete")
                {
                    SanitationJob.ValidateSanitationJob_Complete(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);
                }
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

    //Sanitation Job Report SOM-598
    public class SanitationJobTransactions_GenerationReport : SanitationJob_TransactionBaseClass
    {
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

            SanitationJob.SanitationJobGenerationReport(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }
    public class SanitationJobTransactions_GenerationReportOnDemand : SanitationJob_TransactionBaseClass
    {
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

            SanitationJob.SanitationJobGenerationReportOnDemand(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }
    public class SanitationJobTransactions_PrintSanitationJobReport : SanitationJob_TransactionBaseClass
    {
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

            SanitationJob.SanitationPrintSanitationJobReport(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }

    public class SanitationJobSearch_RetrieveAllForSearch : SanitationJob_TransactionBaseClass
    {

        public List<b_SanitationJob> SanitationJobList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId > 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_SanitationJob> tmpList = null;
            SanitationJob.RetrieveAllForSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            //WorkOrder.LoadFromDatabaseWithDepartName(
            SanitationJobList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class SanitationJobSearch_RetrieveChunkSearch : SanitationJob_TransactionBaseClass
    {

        public List<b_SanitationJob> SanitationJobList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId < 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_SanitationJob tmpList = null;
            SanitationJob.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            SanitationJobList = tmpList.ListSanJob;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class SanitationJob_ValidateByClientlookupIdAndPersonnelId : SanitationJob_TransactionBaseClass
    {

        public SanitationJob_ValidateByClientlookupIdAndPersonnelId()
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
                SanitationJob.ValidateByClientLookupIdAndPersonnelId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    //=============================
    public class SanitationRequest_EXCreate : SanitationJob_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId > 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            SanitationJob.CreateEXSanitionRequest(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(SanitationJob.SanitationJobId > 0);
        }
    }

    public class SanitationRequest_Create : SanitationJob_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId > 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            SanitationJob.InsertIntoDBForSanitionRequest(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(SanitationJob.SanitationJobId > 0);
        }
    }
    //===================
    public class SanitationJobTransactions_RetrieveBy_SanitationJobId : SanitationJob_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId == 0)
            {
                string message = "SanitationMaster has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            SanitationJob.RetrieveBy_SanitationJobId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }
    //=====================
    public class SanitationJob_Validate_ForUpdate_ByClientlookupIdAndPersonnelId : SanitationJob_TransactionBaseClass
    {

        public SanitationJob_Validate_ForUpdate_ByClientlookupIdAndPersonnelId()
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
                SanitationJob.Validate_ForUpdate_ByClientLookupIdAndPersonnelId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
  //SOM-1628
  public class SanitationJob_Validate_ForInterface : SanitationJob_TransactionBaseClass
  {
    public SanitationJob_Validate_ForInterface()
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
        SanitationJob.Validate_ForInterface(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);
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
  public class SanitationJob_UpdateByPK_ForeignKeys : SanitationJob_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId <= 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            SanitationJob.Update_IntoDBForSanitionJob(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(SanitationJob.SanitationJobId > 0);
        }
    }
    //============SOM-1265=========
    #region :: Approve Work Bench Work ::
    public class SanitationJob_RetrieveAllForApproveWorkBench : SanitationJob_TransactionBaseClass
    {

        public List<b_SanitationJob> SanitationJobList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId > 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_SanitationJob> tmpList = null;
            SanitationJob.RetrieveAllForApproveWorkBench(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            SanitationJobList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class SanitationJob_UpdateFor_ApproveWorkBench : SanitationJob_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId <= 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            SanitationJob.SanitionJob_Update_ApproveWorkBench(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(SanitationJob.SanitationJobId > 0);
        }
    }

    #endregion

    //============SOM-1334============
    public class SanitationJobOnDemandJobAndRequest_Create : SanitationJob_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId > 0)
            {
                string message = "SanitationJob has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            SanitationJob.Insert_SanitationJobOnDemandJobAndRequest(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(SanitationJob.SanitationJobId > 0);
        }
    }

    public class SanitationJobTransactions_RetrieveForExtraction : SanitationJob_TransactionBaseClass
    {
        public List<b_SanitationJob> SanitationJobList { get; set; }
        public string CompleteBy { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (SanitationJob.SanitationJobId == 0)
            //{
            //    string message = "SanitationMasterId has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        //public override void PerformWorkItem()
        //{
        //    b_SanitationJob[] tmpArray = null;
        //    base.UseTransaction = false;
        //    SanitationJob.SanitationJob_RetrieveForExtraction(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
        //    SanitationJobList = new List<b_SanitationJob>(tmpArray);
        //}
    }

    public class SanitationJob_RetrieveDashboardChart : SanitationJob_TransactionBaseClass
    {
        public List<b_SanitationJob> SanitationJobList { get; set; }

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
            List<b_SanitationJob> tmpArray = null;

            SanitationJob.SanitationJob_RetrieveDashboardChart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SanitationJobList = new List<b_SanitationJob>();
            foreach (b_SanitationJob tmpObj in tmpArray)
            {
                SanitationJobList.Add(tmpObj);
            }
        }
    }


    public class SanitationJob_WRDashboardRetrieveBy_Filter_V2 : SanitationJob_TransactionBaseClass
    {
        public List<b_SanitationJob> SanitationJobList { get; set; }

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
            List<b_SanitationJob> tmpArray = null;

            SanitationJob.SanitationJob_WRDashboardRetrieveBy_Filter_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SanitationJobList = new List<b_SanitationJob>();
            foreach (b_SanitationJob tmpObj in tmpArray)
            {
                SanitationJobList.Add(tmpObj);
            }
        }
    }

    #region V2-912
    public class SanitationJob_Validate_ForUpdate_ByClientlookupIdAndPersonnelId_V2 : SanitationJob_TransactionBaseClass
    {

        public SanitationJob_Validate_ForUpdate_ByClientlookupIdAndPersonnelId_V2()
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
                SanitationJob.Validate_ForUpdate_ByClientLookupIdAndPersonnelId_V2(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    #endregion

    #region V2-992
    public class SanitationJobTransactions_GenerationSanMasterBatchEntry_Days : SanitationJob_TransactionBaseClass
    {
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

            SanitationJob.SanitationJobGenerationFromSanMasterBatchEntry_Days(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }
    public class SanitationJobTransactions_GenerationFromSanMasterBatchEntry_OnDemand : SanitationJob_TransactionBaseClass
    {
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

            SanitationJob.SanitationJobGenerationFromSanMasterBatchEntry_OnDemand(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }
    #endregion
    public class SanitationJob_RetrieveAllByWorkOrdeV2DevExpressPrint : SanitationJob_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJob.SanitationJobId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_SanitationJob tmpList = null;
            SanitationJob.RetrieveAllBySanitationJobV2PrintForDevExpress(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
}

