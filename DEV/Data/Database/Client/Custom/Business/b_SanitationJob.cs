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
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Database;
namespace Database.Business
{
    public partial class b_SanitationJob
    {
        #region  Properties
        public int FlagSourceType { get; set; }
        public string CreateBy_Name { get; set; }
        public string Creator_PersonnelClientLookupId { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string WOExClientLookupId { get; set; }

        public long PlantLocationId { get; set; }
        public string VerificationCompleteDate { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string Assigned { get; set; }
        public string OnDemandGroup { get; set; }
        public string CompleteBy { get; set; }
        public string ShiftDescription { get; set; }
        public bool ShowCompleted { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string Prefix { get; set; }
        public long PersonnelId { get; set; }
        public int SanitationMasterCount { get; set; }
        public int SanitationJobCount { get; set; }
        public string SanitationJobList { get; set; }
        public string PersonnelIdList { get; set; }
        public DateTime? ScheduledDateFrom { get; set; }
        public DateTime? ScheduledDateTo { get; set; }
        public string CreateByName { get; set; }
        public string PassBy { get; set; }
        public string FailBy { get; set; }
        public string ChartType { get; set; }
        public DateTime TooDay { get; set; }
        public int TimeFrame { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public int offset1 { get; set; }
        public int nextrow { get; set; }
        public string ChargeTo { get; set; }
        public string ChargeToName { get; set; }
        public string AssignedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CompletedDate { get; set; }
        public string VerifyDate { get; set; }
        public string ScheduleDate { get; set; }
        public string SearchText { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public Int32 ChildCount { get; set; }
        public int TotalCount { get; set; }
        public List<b_SanitationJob> ListSanJob { get; set; }
        public string AssetGroup1_ClientLookUpId { get; set; }
        public string AssetGroup2_ClientLookUpId { get; set; }
        public string AssetGroup3_ClientLookUpId { get; set; }
        //v2-398
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string CompleteStartDateVw { get; set; }
        public string CompleteEndDateVw { get; set; }
        public string FailedStartDateVw { get; set; }
        public string FailedEndDateVw { get; set; }
        public string PassedStartDateVw { get; set; }
        public string PassedEndDateVw { get; set; }


        public string FailDescription { get; set; }
        public string SourceIDClientLookUpId { get; set; }
        public string AssetLocation { get; set; }
        public long SanMasterBatchEntryId { get; set; }
     //V2-1071
        public string SanitationJobIdList { get; set; }
        public List<b_SanitationPlanning> listOfSanitationTool { get; set; }
        public List<b_SanitationPlanning> listOfSanitationChemical { get; set; }
        public List<b_SanitationJobTask> listOfSanitationTask { get; set; }
        public List<b_Timecard> listOfTimecard { get; set; }
        //V2-1101
        public long LoggedInUserPEID { get; set; }
        #endregion

        public void SanitationJob_CustomCreate(SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           long SanitationJobBatchEntryId,
           ref b_SanitationJob b_SanitationJob)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                Database.StoredProcedure.usp_SanitationJobGeneration.CallStoredProcedure
                    (command, callerUserInfoId, callerUserName, SanitationJobBatchEntryId, ref b_SanitationJob);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }


        public void RetrieveAllForVerificationWorkBench(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_SanitationJob> results
    )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_SanitationJob_RetrieveForVerificationWorkBench.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void SanitationJob_RetieveBySearchCriteria(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref b_SanitationJob[] data
       )
        {
            Database.SqlClient.ProcessRow<b_SanitationJob> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_SanitationJob[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanitationJob>(reader => { b_SanitationJob obj = new b_SanitationJob(); obj.LoadFromDatabaseByFk(reader); return obj; });
                results = Database.StoredProcedure.usp_SanitationJob_RetieveBySearchCriteria.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_SanitationJob[])results.ToArray(typeof(b_SanitationJob));
                }
                else
                {
                    data = new b_SanitationJob[0];
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void SanitationJob_RetieveAllByFK(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref b_SanitationJob[] data)
        {
            Database.SqlClient.ProcessRow<b_SanitationJob> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_SanitationJob[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanitationJob>(reader => { b_SanitationJob obj = new b_SanitationJob(); obj.LoadFromDatabaseByFk(reader); return obj; });
                results = Database.StoredProcedure.usp_SanitationJob_RetrieveAllByFK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_SanitationJob[])results.ToArray(typeof(b_SanitationJob));
                }
                else
                {
                    data = new b_SanitationJob[0];
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public int LoadFromDatabaseByFk(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                // Shift Description
                if (false == reader.IsDBNull(i))
                {
                    this.ShiftDescription = reader.GetString(i);
                }
                else
                {
                    this.ShiftDescription = "";
                }
                i++;
                // Assigned
                if (false == reader.IsDBNull(i))
                {
                    this.Assigned = reader.GetString(i);
                }
                else
                {
                    this.Assigned = "";
                }
                i++;
                // Complete By
                if (false == reader.IsDBNull(i))
                {
                    this.CompleteBy = reader.GetString(i);
                }
                else
                {
                    this.CompleteBy = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    this.OnDemandGroup = reader.GetString(i);
                }
                else
                {
                    this.OnDemandGroup = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    this.ChargeTo_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    this.ChargeTo_ClientLookupId = "";
                }
                i++;
                //CreateBy
                if (false == reader.IsDBNull(i))
                {
                    this.CreateBy = reader.GetString(i);
                }
                else
                {
                    this.CreateBy = "";
                }
                i++;
                //CreateDate
                if (false == reader.IsDBNull(i))
                {
                    this.CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    this.CreateDate = DateTime.MinValue;
                }
                i++;
                //PassBy
                if (false == reader.IsDBNull(i))
                {
                    this.PassBy = reader.GetString(i);
                }
                else
                {
                    this.PassBy = "";
                }
                i++;
                //FailBy
                if (false == reader.IsDBNull(i))
                {
                    this.FailBy = reader.GetString(i);
                }
                else
                {
                    this.FailBy = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["CompleteBy"].ToString(); }
                catch { missing.Append("CompleteBy "); }
                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }
                try { reader["ShiftDescription"].ToString(); }
                catch { missing.Append("ShiftDescription "); }
                try { reader["OnDemandGroup"].ToString(); }
                catch { missing.Append("OnDemandGroup "); }
                try { reader["ChargeTo_ClientLookupId"].ToString(); }
                catch { missing.Append("ChargeTo_ClientLookupId "); }
                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }
                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }
                try { reader["PassBy"].ToString(); }
                catch { missing.Append("PassBy "); }
                try { reader["FailBy"].ToString(); }
                catch { missing.Append("FailBy "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }
        public int LoadFromDatabaseRetrieV2(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                // Shift Description
                if (false == reader.IsDBNull(i))
                {
                    this.ShiftDescription = reader.GetString(i);
                }
                else
                {
                    this.ShiftDescription = "";
                }
                i++;
                // Assigned
                if (false == reader.IsDBNull(i))
                {
                    this.Assigned = reader.GetString(i);
                }
                else
                {
                    this.Assigned = "";
                }
                i++;
                // Complete By
                if (false == reader.IsDBNull(i))
                {
                    this.CompleteBy = reader.GetString(i);
                }
                else
                {
                    this.CompleteBy = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    this.OnDemandGroup = reader.GetString(i);
                }
                else
                {
                    this.OnDemandGroup = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    this.ChargeTo_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    this.ChargeTo_ClientLookupId = "";
                }
                i++;
                //CreateBy
                if (false == reader.IsDBNull(i))
                {
                    this.CreateBy = reader.GetString(i);
                }
                else
                {
                    this.CreateBy = "";
                }
                i++;
                //CreateDate
                if (false == reader.IsDBNull(i))
                {
                    this.CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    this.CreateDate = DateTime.MinValue;
                }
                i++;
                //PassBy
                if (false == reader.IsDBNull(i))
                {
                    this.PassBy = reader.GetString(i);
                }
                else
                {
                    this.PassBy = "";
                }
                i++;
                //FailBy
                if (false == reader.IsDBNull(i))
                {
                    this.FailBy = reader.GetString(i);
                }
                else
                {
                    this.FailBy = "";
                }
                i++;
                //FailDescription
                if (false == reader.IsDBNull(i))
                {
                    this.FailDescription = reader.GetString(i);
                }
                else
                {
                    this.FailDescription = "";
                }
                i++;
                //SourceIDClientLookUpId
                if (false == reader.IsDBNull(i))
                {
                    this.SourceIDClientLookUpId = reader.GetString(i);
                }
                else
                {
                    this.SourceIDClientLookUpId = "";
                }
                i++;
                //SourceIDClientLookUpId
                if (false == reader.IsDBNull(i))
                {
                    this.AssetLocation = reader.GetString(i);
                }
                else
                {
                    this.AssetLocation = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["CompleteBy"].ToString(); }
                catch { missing.Append("CompleteBy "); }
                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }
                try { reader["ShiftDescription"].ToString(); }
                catch { missing.Append("ShiftDescription "); }
                try { reader["OnDemandGroup"].ToString(); }
                catch { missing.Append("OnDemandGroup "); }
                try { reader["ChargeTo_ClientLookupId"].ToString(); }
                catch { missing.Append("ChargeTo_ClientLookupId "); }
                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }
                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }
                try { reader["PassBy"].ToString(); }
                catch { missing.Append("PassBy "); }
                try { reader["FailBy"].ToString(); }
                catch { missing.Append("FailBy "); }
                try { reader["FailDescription"].ToString(); }
                catch { missing.Append("FailDescription "); }
                try { reader["SourceIDClientLookUpId"].ToString(); }
                catch { missing.Append("SourceIDClientLookUpId "); }
                try { reader["AssetLocation"].ToString(); }
                catch { missing.Append("AssetLocation "); }
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }
        public void SanitationJob_UpdateForComplete(
             SqlConnection connection,
             SqlTransaction transaction,
             long CallerUserPersonnelId,
             string callerUserName
         )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_SanitationJob_UpdateForComplete.CallStoredProcedure(command, CallerUserPersonnelId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }
        //SOM-1628
        public void SanitationJob_UpdateForInterface(SqlConnection connection, SqlTransaction transaction, long calleruserinfoid, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_SanitationJob_UpdateForInterface.CallStoredProcedure(command, calleruserinfoid, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }


        public void SanitationJob_UpdateForReschedule(
             SqlConnection connection,
             SqlTransaction transaction,
             string callerUserName
         )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_SanitationJob_UpdateForReschedule.CallStoredProcedure(command, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void SanitationJob_UpdateForCancel(
             SqlConnection connection,
             SqlTransaction transaction,
             long CallerUserPersonnelId,
             string callerUserName

         )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_SanitationJob_UpdateForCancel.CallStoredProcedure(command, CallerUserPersonnelId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void SanitationJob_UpdateForReopen(
             SqlConnection connection,
             SqlTransaction transaction,
             string callerUserName
         )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_SanitationJob_UpdateForReopen.CallStoredProcedure(command, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void RetrieveByForeignKeysFromDatabase(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName
      )
        {
            Database.SqlClient.ProcessRow<b_SanitationJob> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanitationJob>(reader => { this.LoadFromDatabaseByFk(reader); return this; });
                Database.StoredProcedure.usp_SanitationJob_RetrieveByFK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void RetrieveByV2(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName
   )
        {
            Database.SqlClient.ProcessRow<b_SanitationJob> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanitationJob>(reader => { this.LoadFromDatabaseRetrieV2(reader); return this; });
                Database.StoredProcedure.usp_SanitationJob_Retrieve_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void SanitationJob_CreateByFK(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_SanitationJob_CreateByFk.CallStoredProcedure(command, callerUserName, callerUserInfoId, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }
        public void ValidateSanitationJob_Complete(SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_StoredProcValidationError> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();
            try
            {
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                results = Database.StoredProcedure.usp_SanitationJob_ValidateComplete.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }

        //SanitationJob Generation Report  -  SOM  -  598

        public void SanitationJobGenerationReport(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName

       )
        {
            Database.SqlClient.ProcessRow<b_SanitationJob> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                // processRow = new Database.SqlClient.ProcessRow<b_SanitationJob>(reader => { this.LoadFromDatabaseByFk(reader); return this; });
                Database.StoredProcedure.usp_SanitationMaster_AutoGenCreate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void SanitationJobGenerationReportOnDemand(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName

       )
        {
            Database.SqlClient.ProcessRow<b_SanitationJob> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                // processRow = new Database.SqlClient.ProcessRow<b_SanitationJob>(reader => { this.LoadFromDatabaseByFk(reader); return this; });
                Database.StoredProcedure.usp_SanitationMaster_AutoGenCreateOnDemand.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        //SanitationJob Print Report  -  SOM  -  720

        public void SanitationPrintSanitationJobReport(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName

    )
        {
            Database.SqlClient.ProcessRow<b_SanitationJob> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                // processRow = new Database.SqlClient.ProcessRow<b_SanitationJob>(reader => { this.LoadFromDatabaseByFk(reader); return this; });
                Database.StoredProcedure.usp_SanitationJob_RetrieveAllForReport.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }





        public void RetrieveChunkSearch(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref b_SanitationJob results
        )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_SanitationJob_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void RetrieveAllForSearch(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_SanitationJob> results
       )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_SanitationJob_RetrieveBy_Filter.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void InsertIntoDBForSanitionRequest(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
         string callerUserName
         )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }

                Database.StoredProcedure.usp_SanitationJob_CreateRequest.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }
        public void CreateEXSanitionRequest(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
       string callerUserName
       )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }

                Database.StoredProcedure.usp_SanitationJob_CreateEXSanitRequest.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void ValidateByClientLookupIdAndPersonnelId(SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_StoredProcValidationError> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();
            try
            {
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                results = Database.StoredProcedure.usp_SanitationJob_InsertValidateByPersonnelId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }


        public static b_SanitationJob ProcessRowForSanitationJobRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_SanitationJob sanitationJob = new b_SanitationJob();

            sanitationJob.LoadFromDatabaseForWorkOrderRetriveAllForSearch(reader);

            return sanitationJob;
        }
        public void LoadFromDatabaseForWorkOrderRetriveAllForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // SanitationJobId column, bigint, not null
                SanitationJobId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(15), not null
                ClientLookupId = reader.GetString(i++);

                // Description column, not null
                Description = reader.GetString(i++);

                // ChargeToId column, bigint, not null
                ChargeToId = reader.GetInt64(i++);

                // ChargeToClientLookupId column, nvarchar(63), not null
                ChargeTo_ClientLookupId = reader.GetString(i++);

                // ChargeTo_Name column, nvarchar(63), not null
                ChargeTo_Name = reader.GetString(i++);
                // AssetLocation column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    AssetLocation = reader.GetString(i); ;
                }
                else
                {
                    AssetLocation = "";
                }
                i++;
                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // Extracted column, bit, not null
                Extracted = reader.GetBoolean(i++);

                // Shift column, nvarchar(15), not null
                Shift = reader.GetString(i++);

                // ScheduledDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                // CreateByName column, Nvarchar, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateByName = reader.GetString(i);
                }
                else
                {
                    CreateByName = string.Empty;
                }
                i++;
                // Complete Date, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;
                // Scheduled Date, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ScheduledDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledDate = DateTime.MinValue;
                }
                i++;
                // Assigned nvarchar(63)
                Assigned = reader.GetString(i++);
                // VerifiedBy nvarchar(63
                VerifiedBy = reader.GetString(i++);
                // VerifiedDate, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    VerifiedDate = reader.GetDateTime(i);
                }
                else
                {
                    VerifiedDate = DateTime.MinValue;
                }
                i++;

                // AssetGroup1Desc, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1_ClientLookUpId = reader.GetString(i); ;
                }
                else
                {
                    AssetGroup1_ClientLookUpId = "";
                }
                i++;
                // AssetGroup2_Desc, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2_ClientLookUpId = reader.GetString(i); ;
                }
                else
                {
                    AssetGroup2_ClientLookUpId = "";
                }
                i++;

                // AssetGroup3_Desc, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup3_ClientLookUpId = reader.GetString(i); ;
                }
                else
                {
                    AssetGroup3_ClientLookUpId = "";
                }
                i++;
                //V2-910
                if (false == reader.IsDBNull(i))
                {
                    PassDate = reader.GetDateTime(i);
                }
                else
                {
                    PassDate = DateTime.MinValue;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    FailDate = reader.GetDateTime(i);
                }
                else
                {
                    FailDate = DateTime.MinValue;
                }
                i++;
                //TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch
                {
                    missing.Append("SiteId ");

                    try { reader["SanitationJobId"].ToString(); }
                    catch { missing.Append("SanitationJobId "); }

                    try { reader["ClientLookupId"].ToString(); }
                    catch { missing.Append("ClientLookupId "); }

                    try { reader["Description"].ToString(); }
                    catch { missing.Append("Description"); }

                    try { reader["ChargeToId"].ToString(); }
                    catch { missing.Append("ChargeToId "); }

                    try { reader["ChargeToClientLookupId"].ToString(); }
                    catch { missing.Append("ChargeToClientLookupId "); }

                    try { reader["ChargeTo_Name"].ToString(); }
                    catch { missing.Append("ChargeTo_Name "); }

                    try { reader["Status"].ToString(); }
                    catch { missing.Append("Status "); }

                    try { reader["Shift"].ToString(); }
                    catch { missing.Append("Shift "); }

                    try { reader["CreateDate"].ToString(); }
                    catch { missing.Append("CreateDate "); }

                    try { reader["CompleteDate"].ToString(); }
                    catch { missing.Append("CompleteDate "); }

                    try { reader["ScheduledDate"].ToString(); }
                    catch { missing.Append("ScheduledDate "); }

                    try { reader["Assigned"].ToString(); }
                    catch { missing.Append("Assigned "); }

                    try { reader["VerifiedBy"].ToString(); }
                    catch { missing.Append("VerifiedBy "); }

                    try { reader["VerifiedDate"].ToString(); }
                    catch { missing.Append("VerifiedDate "); }
                }
            }

        }

        public static b_SanitationJob ProcessRowForSanitationJob(SqlDataReader reader)
        {
            // Create instance of object
            b_SanitationJob obj = new b_SanitationJob();

            // Load the object from the database
            obj.LoadFromDatabaseForSanitationJob(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForSanitationJob(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SanitationJobId column, bigint, not null
                SanitationJobId = reader.GetInt64(i++);

                // SanitationMasterId column, bigint, not null
                //SanitationMasterId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(15), not null
                ClientLookupId = reader.GetString(i++);

                // SourceType column, nvarchar(15), not null
                SourceType = reader.GetString(i++);

                // SourceType column, long, not null
                SourceId = reader.GetInt64(i++);

                // ActualDuration column, decimal(8,2), not null
                ActualDuration = reader.GetDecimal(i++);

                // AssignedTo_PersonnelId column, bigint, not null
                AssignedTo_PersonnelId = reader.GetInt64(i++);

                // CancelReason column, nvarchar(15), not null
                CancelReason = reader.GetString(i++);

                // ChargeToId column, bigint, not null
                ChargeToId = reader.GetInt64(i++);

                // ChargeType column, nvarchar(15), not null
                ChargeType = reader.GetString(i++);

                // ChargeTo_Name column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i);
                }
                else
                {
                    ChargeTo_Name = "";
                }
                i++;

                // CompleteBy_PersonnelId column, bigint, not null
                CompleteBy_PersonnelId = reader.GetInt64(i++);

                // CompleteComments column, nvarchar(MAX), not null
                CompleteComments = reader.GetString(i++);

                // CompleteDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;
                // Description column, nvarchar(MAX), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                { Description = string.Empty; }
                i++;
                // Shift column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Shift = reader.GetString(i);
                }
                else
                { Shift = string.Empty; }
                i++;
                // DownRequired column, bit, not null
                DownRequired = reader.GetBoolean(i++);

                // ScheduledDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ScheduledDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledDate = DateTime.MinValue;
                }
                i++;
                // ScheduledDuration column, decimal(8,2), not null
                ScheduledDuration = reader.GetDecimal(i++);

                // Status column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;
                // Creator_PersonnelId column, bigint, not null
                Creator_PersonnelId = reader.GetInt64(i++);

                CreateBy = reader.GetString(i++);

                // ScheduledDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;

                // RequiredDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CreateBy_Name = reader.GetString(i);
                }
                else
                {
                    CreateBy_Name = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CompleteBy = reader.GetString(i);
                }
                else
                {
                    CompleteBy = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i);
                }
                else
                {
                    Assigned = string.Empty;
                }
                i++;

                // ChargeToClientLookupId column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeTo_ClientLookupId = string.Empty;
                }
                i++;
                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SanitationJobId"].ToString(); }
                catch { missing.Append("SanitationJobId "); }

                //try { reader["SanitationMasterId"].ToString(); }
                //catch { missing.Append("SanitationMasterId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["SourceType"].ToString(); }
                catch { missing.Append("SourceType "); }

                try { reader["ActualDuration"].ToString(); }
                catch { missing.Append("ActualDuration "); }

                try { reader["AssignedTo_PersonnelId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelId "); }

                try { reader["CancelReason"].ToString(); }
                catch { missing.Append("CancelReason "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeType"].ToString(); }
                catch { missing.Append("ChargeType "); }
                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["CompleteBy_PersonnelId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelId "); }

                try { reader["CompleteComments"].ToString(); }
                catch { missing.Append("CompleteComments "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["DownRequired"].ToString(); }
                catch { missing.Append("DownRequired "); }

                try { reader["ScheduledDate"].ToString(); }
                catch { missing.Append("ScheduledDate "); }

                try { reader["ScheduledDuration"].ToString(); }
                catch { missing.Append("ScheduledDuration "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Creator_PersonnelId"].ToString(); }
                catch { missing.Append("Creator_PersonnelId "); }

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }

                try { reader["CreateBy_Name"].ToString(); }
                catch { missing.Append("CreateBy_Name "); }

                try { reader["CompleteBy"].ToString(); }
                catch { missing.Append("CompleteBy "); }

                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned"); }


                try { reader["ChargeTo_ClientLookupId"].ToString(); }
                catch { missing.Append("ChargeTo_ClientLookupId "); }


                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }
        public void RetrieveBy_SanitationJobId(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
   string callerUserName
     )
        {
            Database.SqlClient.ProcessRow<b_SanitationJob> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanitationJob>(reader => { this.LoadFromDatabaseForSanitationJob(reader); return this; });
                Database.StoredProcedure.usp_SanitationJob_RetrieveBy_SanitationJobId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void Validate_ForUpdate_ByClientLookupIdAndPersonnelId(SqlConnection connection,
                   SqlTransaction transaction,
                   long callerUserInfoId,
                   string callerUserName,
                   ref List<b_StoredProcValidationError> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();
            try
            {
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                results = Database.StoredProcedure.usp_SanitationJob_ValidateByPersonnelId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        // SOM-1628
        public void Validate_ForInterface(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_StoredProcValidationError> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();
            try
            {
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                results = StoredProcedure.usp_SanitationJob_ValidateForInterface.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        public void Update_IntoDBForSanitionJob(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
      string callerUserName
      )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }

                Database.StoredProcedure.usp_SanitationJob_Update.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }
        public string CheckStatus { get; set; }
        //SOM-1265
        #region :: Approve Work bench Work ::
        public string ApproveStatusDrop { get; set; }
        public string ApproveCreatedDate { get; set; }
        public string Assigned_PersonnelClientLookupId { get; set; }
        public string ModifyBy { get; set; }
        public string ScheduleFlag { get; set; }
        public string ApproveFlag { get; set; }
        public string DeniedFlag { get; set; }
        public string CreateBy_PersonnelId { get; set; }



        public void RetrieveAllForApproveWorkBench(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_SanitationJob> results
        )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_SanitationJob_RetrieveForApproveWorkBench.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public static b_SanitationJob ProcessRowForSanitationJobApproveWorkBench(SqlDataReader reader)
        {
            // Create instance of object
            b_SanitationJob sanitationJob = new b_SanitationJob();

            sanitationJob.LoadFromDatabaseForSanitationJobApproveWorkBench(reader);

            return sanitationJob;
        }

        public void LoadFromDatabaseForSanitationJobApproveWorkBench(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SanitationJobId column, bigint, not null
                SanitationJobId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(15), not null
                ClientLookupId = reader.GetString(i++);

                // Description column, not null
                Description = reader.GetString(i++);

                // Shift column, nvarchar(15), not null
                Shift = reader.GetString(i++);

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // ChargeToId column, bigint, not null
                ChargeToId = reader.GetInt64(i++);

                // ChargeToClientLookupId column, nvarchar(63), not null
                ChargeTo_ClientLookupId = reader.GetString(i++);

                // ChargeTo_Name column, nvarchar(63), not null
                ChargeTo_Name = reader.GetString(i++);

                // AssignedTo_PersonnelId column, bigint, not null
                AssignedTo_PersonnelId = reader.GetInt64(i++);

                // Assigned_PersonnelClientLookupId column, nvarchar(63)
                Assigned_PersonnelClientLookupId = reader.GetString(i++);

                // ScheduledDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ScheduledDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledDate = DateTime.MinValue;
                }
                i++;

                // ScheduledDuration column, decimal(8,2), not null
                ScheduledDuration = reader.GetDecimal(i++);

                // DeniedBy_PersonnelId column, bigint, not null
                DeniedBy_PersonnelId = reader.GetInt64(i++);

                // DeniedDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    DeniedDate = reader.GetDateTime(i);
                }
                else
                {
                    DeniedDate = DateTime.MinValue;
                }
                i++;

                // DeniedReason column, nvarchar(15), not null
                DeniedReason = reader.GetString(i++);

                // CreateBy_PersonnelId column, nvarchar(63), not null
                CreateBy_PersonnelId = reader.GetString(i++);

                // CreateDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    UpdateIndex = reader.GetInt32(i++);
                }
                else
                {
                    UpdateIndex = 0;
                    i++;
                }

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch
                {
                    missing.Append("SiteId ");

                    try { reader["SanitationJobId"].ToString(); }
                    catch { missing.Append("SanitationJobId "); }

                    try { reader["ClientLookupId"].ToString(); }
                    catch { missing.Append("ClientLookupId "); }

                    try { reader["Description"].ToString(); }
                    catch { missing.Append("Description"); }

                    try { reader["Shift"].ToString(); }
                    catch { missing.Append("Shift "); }

                    try { reader["Status"].ToString(); }
                    catch { missing.Append("Status "); }

                    try { reader["ChargeToId"].ToString(); }
                    catch { missing.Append("ChargeToId "); }

                    try { reader["ChargeToClientLookupId"].ToString(); }
                    catch { missing.Append("ChargeToClientLookupId "); }

                    try { reader["ChargeTo_Name"].ToString(); }
                    catch { missing.Append("ChargeTo_Name "); }

                    try { reader["AssignedTo_PersonnelId"].ToString(); }
                    catch
                    {
                        missing.Append("AssignedTo_PersonnelId");

                        try { reader["Assigned_PersonnelClientLookupId"].ToString(); }
                        catch { missing.Append("Assigned_PersonnelClientLookupId"); }

                        try { reader["ScheduledDate"].ToString(); }
                        catch { missing.Append("ScheduledDate"); }

                        try { reader["ScheduledDuration"].ToString(); }
                        catch { missing.Append("ScheduledDuration"); }

                        try { reader["DeniedBy_PersonnelId"].ToString(); }
                        catch { missing.Append("DeniedBy_PersonnelId"); }

                        try { reader["DeniedDate"].ToString(); }
                        catch { missing.Append("DeniedDate"); }

                        try { reader["DeniedReason"].ToString(); }
                        catch { missing.Append("DeniedReason"); }

                        try { reader["CreateBy_PersonnelId"].ToString(); }
                        catch { missing.Append("CreateBy_PersonnelId"); }

                        try { reader["CreateDate"].ToString(); }
                        catch { missing.Append("CreateDate "); }

                        try { reader["UpdateIndex"].ToString(); }
                        catch { missing.Append("CreateDate "); }



                    }
                }

            }




        }



        public static b_SanitationJob ProcessRowForSanitationJobVerificationWorkBench(SqlDataReader reader)
        {
            // Create instance of object
            b_SanitationJob sanitationJob = new b_SanitationJob();

            sanitationJob.LoadFromDatabaseForSanitationJobVerificationWorkBench(reader);

            return sanitationJob;
        }

        public void LoadFromDatabaseForSanitationJobVerificationWorkBench(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SanitationJobId column, bigint, not null
                SanitationJobId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;
                // Description column, not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = string.Empty;
                }
                i++;
                // Shift column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Shift = reader.GetString(i);
                }
                else
                {
                    Shift = string.Empty;
                }
                i++;
                // Status column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;
                // ChargeToId column, bigint, not null
                ChargeToId = reader.GetInt64(i++);

                // ChargeToClientLookupId column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeTo_ClientLookupId = string.Empty;
                }
                i++;
                // ChargeTo_Name column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i);
                }
                else
                {
                    ChargeTo_Name = string.Empty;
                }
                i++;

                // AssignedTo_PersonnelId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    AssignedTo_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    AssignedTo_PersonnelId = 0;
                }
                i++;
                // Assigned_PersonnelClientLookupId column, nvarchar(63)
                if (false == reader.IsDBNull(i))
                {
                    Assigned_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    Assigned_PersonnelClientLookupId = string.Empty;
                }
                i++;
                // ScheduledDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ScheduledDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledDate = DateTime.MinValue;
                }
                i++;

                // ScheduledDuration column, decimal(8,2), not null
                ScheduledDuration = reader.GetDecimal(i++);

                // DeniedBy_PersonnelId column, bigint, not null
                DeniedBy_PersonnelId = reader.GetInt64(i++);

                // DeniedDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    DeniedDate = reader.GetDateTime(i);
                }
                else
                {
                    DeniedDate = DateTime.MinValue;
                }
                i++;

                // DeniedReason column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    DeniedReason = reader.GetString(i);
                }
                else
                {
                    DeniedReason = string.Empty;
                }
                i++;
                // CreateBy_PersonnelId column, nvarchar(63), not null
                CreateBy_PersonnelId = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FailReason = reader.GetString(i);
                }
                else
                {
                    FailReason = string.Empty;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    FailComment = reader.GetString(i);
                }
                else
                {
                    FailComment = string.Empty;
                }
                i++;

                // CreateDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    UpdateIndex = reader.GetInt32(i);
                }
                else
                {
                    UpdateIndex = 0;
                    i++;
                }

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch
                {
                    missing.Append("SiteId ");

                    try { reader["SanitationJobId"].ToString(); }
                    catch { missing.Append("SanitationJobId "); }

                    try { reader["ClientLookupId"].ToString(); }
                    catch { missing.Append("ClientLookupId "); }

                    try { reader["Description"].ToString(); }
                    catch { missing.Append("Description"); }

                    try { reader["Shift"].ToString(); }
                    catch { missing.Append("Shift "); }

                    try { reader["Status"].ToString(); }
                    catch { missing.Append("Status "); }

                    try { reader["ChargeToId"].ToString(); }
                    catch { missing.Append("ChargeToId "); }

                    try { reader["ChargeToClientLookupId"].ToString(); }
                    catch { missing.Append("ChargeToClientLookupId "); }

                    try { reader["ChargeTo_Name"].ToString(); }
                    catch { missing.Append("ChargeTo_Name "); }

                    try { reader["AssignedTo_PersonnelId"].ToString(); }
                    catch
                    {
                        missing.Append("AssignedTo_PersonnelId");

                        try { reader["Assigned_PersonnelClientLookupId"].ToString(); }
                        catch { missing.Append("Assigned_PersonnelClientLookupId"); }

                        try { reader["ScheduledDate"].ToString(); }
                        catch { missing.Append("ScheduledDate"); }

                        try { reader["ScheduledDuration"].ToString(); }
                        catch { missing.Append("ScheduledDuration"); }

                        try { reader["DeniedBy_PersonnelId"].ToString(); }
                        catch { missing.Append("DeniedBy_PersonnelId"); }

                        try { reader["DeniedDate"].ToString(); }
                        catch { missing.Append("DeniedDate"); }

                        try { reader["DeniedReason"].ToString(); }
                        catch { missing.Append("DeniedReason"); }

                        try { reader["CreateBy_PersonnelId"].ToString(); }
                        catch { missing.Append("CreateBy_PersonnelId"); }

                        try { reader["CreateDate"].ToString(); }
                        catch { missing.Append("CreateDate "); }

                        try { reader["UpdateIndex"].ToString(); }
                        catch { missing.Append("CreateDate "); }



                    }
                }

            }




        }

        public void SanitionJob_Update_ApproveWorkBench(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
     )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }

                Database.StoredProcedure.usp_SanitationJob_Update_ApproveWorkBench.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }



        #endregion

        //===================SOM-1334==============
        public void Insert_SanitationJobOnDemandJobAndRequest(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
        string callerUserName
        )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }

                Database.StoredProcedure.usp_SanitationJob_OnDemandJobAndRequest_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void SanitationJob_RetrieveForExtraction(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref b_SanitationJob[] data
    )
        {
            Database.SqlClient.ProcessRow<b_SanitationJob> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;
            ArrayList results = null;
            // Initialize the results
            data = new b_SanitationJob[0];
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanitationJob>(reader => { b_SanitationJob obj = new b_SanitationJob(); obj.LoadFromDatabaseByFk(reader); return obj; });
                results = Database.StoredProcedure.usp_SanitationJob_RetrieveForExtraction.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);
                // Extract the results
                if (null != results)
                {
                    data = (b_SanitationJob[])results.ToArray(typeof(b_SanitationJob));
                }
                else
                {
                    data = new b_SanitationJob[0];
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public static object ProcessRowDashboard(SqlDataReader reader)
        {
            // Create instance of object
            b_SanitationJob obj = new b_SanitationJob();

            // Load the object from the database
            obj.LoadFromDatabaseDashBoard(reader);

            // Return result
            return (object)obj;
        }

        public void LoadFromDatabaseDashBoard(SqlDataReader reader)
        {
            int i = 0;

            try
            {

                SanitationJobCount = reader.GetInt32(i);
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader[" SanitationJobCount"].ToString(); }
                catch { missing.Append(" SanitationJobCount "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }
        }


        public static object ProcessRowWRDashboardFilter(SqlDataReader reader)
        {
            // Create instance of object
            b_SanitationJob obj = new b_SanitationJob();

            // Load the object from the database
            obj.LoadFromDatabaseWRDashBoardFilter(reader);

            // Return result
            return (object)obj;
        }

        public void LoadFromDatabaseWRDashBoardFilter(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                ClientId = reader.GetInt64(i++);
                SiteId = reader.GetInt64(i++);
                SanitationJobId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = string.Empty;
                }
                i++;
                ChargeToId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = string.Empty;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i);
                }
                else
                {
                    ChargeTo_Name = string.Empty;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    this.CreateDate = DateTime.MinValue;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i);
                }
                else
                {
                    Assigned = string.Empty;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    this.CompleteDate = DateTime.MinValue;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["SanitationJobId"].ToString(); }
                catch { missing.Append("SanitationJobId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }
        }


        public void SanitationJob_RetrieveDashboardChart(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_SanitationJob> data)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_SanitationJob> results = null;
            data = new List<b_SanitationJob>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_SanitationJob_RetrieveDashboardChart.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_SanitationJob>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void SanitationJob_WRDashboardRetrieveBy_Filter_V2(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_SanitationJob> data)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_SanitationJob> results = null;
            data = new List<b_SanitationJob>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_SanitationJob_WRDashboardRetrieveBy_Filter_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_SanitationJob>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        #region V2-912
        public void Validate_ForUpdate_ByClientLookupIdAndPersonnelId_V2(SqlConnection connection,
                SqlTransaction transaction,
                long callerUserInfoId,
                string callerUserName,
                ref List<b_StoredProcValidationError> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();
            try
            {
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                results = Database.StoredProcedure.usp_SanitationJob_ValidateByPersonnelId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        #endregion
        #region V2-992
        public void SanitationJobGenerationFromSanMasterBatchEntry_Days(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName

    )
        {
            Database.SqlClient.ProcessRow<b_SanitationJob> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                // processRow = new Database.SqlClient.ProcessRow<b_SanitationJob>(reader => { this.LoadFromDatabaseByFk(reader); return this; });
                Database.StoredProcedure.usp_SanitationJob_CreateFromSanMasterBatchEntry_Days_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void SanitationJobGenerationFromSanMasterBatchEntry_OnDemand(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName

     )
        {
            Database.SqlClient.ProcessRow<b_SanitationJob> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                // processRow = new Database.SqlClient.ProcessRow<b_SanitationJob>(reader => { this.LoadFromDatabaseByFk(reader); return this; });
                Database.StoredProcedure.usp_SanitationJob_CreateFromSanMasterBatchEntry_OnDemand_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        #endregion
        //V2-1071
        public void RetrieveAllBySanitationJobV2PrintForDevExpress(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_SanitationJob results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_SanitationJobPrint_RetrieveAllBySanitationJob_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public static b_SanitationJob ProcessRowForDevExpressSanitationJob(SqlDataReader reader)
        {
            // Create instance of object
            b_SanitationJob obj = new b_SanitationJob();

            // Load the object from the database
            obj.LoadFromDatabaseForDevExpressSanitationJob(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForDevExpressSanitationJob(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SanitationJobId 
                SanitationJobId = reader.GetInt64(i++);

                // ClientLookupId 
                ClientLookupId = reader.GetString(i++);

                // ChargeToId 
                ChargeToId = reader.GetInt64(i++);


                // ChargeTo_ClientLookupId 
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeTo_ClientLookupId = "";
                }
                i++;

                // ChargeTo_Name 
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i);
                }
                else
                {
                    ChargeTo_Name = "";
                }
                i++;
                //ChargeType 
                if (false == reader.IsDBNull(i))
                {
                    ChargeType = reader.GetString(i);
                }
                else
                {
                    ChargeType = "";
                }
                i++;
               // AssetLocation
                if (false == reader.IsDBNull(i))
                {
                    AssetLocation = reader.GetString(i);
                }
                else
                {
                    AssetLocation = "";
                }
                i++;
                // Shift 
                if (false == reader.IsDBNull(i))
                {
                    Shift = reader.GetString(i);
                }
                else
                { Shift = string.Empty; }
                i++;
                // ShiftDescription 
                if (false == reader.IsDBNull(i))
                {
                    ShiftDescription = reader.GetString(i);
                }
                else
                { ShiftDescription = string.Empty; }
                i++;
                // Status column
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;
                // ScheduledDate 
                if (false == reader.IsDBNull(i))
                {
                    ScheduledDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledDate = DateTime.MinValue;
                }
                i++;
                // ScheduledDuration 
                ScheduledDuration = reader.GetDecimal(i++);
                // Description 
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                { Description = string.Empty; }
                i++;
                // AssignedTo_PersonnelId 
                AssignedTo_PersonnelId = reader.GetInt64(i++);
                // Assigned column, nvarchar(MAX), 
                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i);
                }
                else
                { Assigned = string.Empty; }
                i++;
                // CreateDate 
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                // CreateBy 
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = string.Empty;
                }
                i++;
                // CompleteDate  
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;
                // CompleteBy 
                if (false == reader.IsDBNull(i))
                {
                    CompleteBy = reader.GetString(i);
                }
                else
                {
                    CompleteBy = string.Empty;
                }
                i++;
                // CompleteComments
                if (false == reader.IsDBNull(i))
                {
                    CompleteComments = reader.GetString(i);
                }
                else
                {
                    CompleteComments = string.Empty;
                }
                i++;
                // PassDate 
                if (false == reader.IsDBNull(i))
                {
                    PassDate = reader.GetDateTime(i);
                }
                else
                {
                    PassDate = DateTime.MinValue;
                }
                i++;
                // PassBy
                if (false == reader.IsDBNull(i))
                {
                    PassBy = reader.GetString(i);
                }
                else
                {
                    PassBy = string.Empty;
                }
                i++;

                // FailDate 
                if (false == reader.IsDBNull(i))
                {
                    FailDate = reader.GetDateTime(i);
                }
                else
                {
                    FailDate = DateTime.MinValue;
                }
                i++;
                // FailBy
                if (false == reader.IsDBNull(i))
                {
                    FailBy = reader.GetString(i);
                }
                else
                {
                    FailBy = string.Empty;
                }
                i++;
                // FailReason
                if (false == reader.IsDBNull(i))
                {
                    FailReason = reader.GetString(i);
                }
                else
                {
                    FailReason = string.Empty;
                }
                i++;
                // FailComment
                if (false == reader.IsDBNull(i))
                {
                    FailComment = reader.GetString(i);
                }
                else
                {
                    FailComment = string.Empty;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SanitationJobId"].ToString(); }
                catch { missing.Append("SanitationJobId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeTo_ClientLookupId"].ToString(); }
                catch { missing.Append("ChargeTo_ClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["ChargeType"].ToString(); }
                catch { missing.Append("ChargeType "); }

                try { reader["AssetLocation"].ToString(); }
                catch { missing.Append("AssetLocation "); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["ShiftDescription"].ToString(); }
                catch { missing.Append("ShiftDescription "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["ScheduledDate"].ToString(); }
                catch { missing.Append("ScheduledDate "); }

                try { reader["ScheduledDuration"].ToString(); }
                catch { missing.Append("ScheduledDuration "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["AssignedTo_PersonnelId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelId "); }

                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["CompleteBy"].ToString(); }
                catch { missing.Append("CompleteBy "); }

                try { reader["CompleteComments"].ToString(); }
                catch { missing.Append("CompleteComments "); }

                try { reader["PassDate"].ToString(); }
                catch { missing.Append("PassDate "); }

                try { reader["PassBy"].ToString(); }
                catch { missing.Append("PassBy "); }

                try { reader["FailDate"].ToString(); }
                catch { missing.Append("FailDate "); }

                try { reader["FailBy"].ToString(); }
                catch { missing.Append("FailBy "); }

                try { reader["FailReason"].ToString(); }
                catch { missing.Append("FailReason "); }

                try { reader["FailComment"].ToString(); }
                catch { missing.Append("FailComment "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }

    }

}

