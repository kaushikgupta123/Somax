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
* 2015-Mar-21 SOM-585  Roger Lawton        Review Modifications
****************************************************************************************************
 */
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    public partial class b_SanitationJobTask
    {
        #region Properties
        public string CompleteBy { get; set; }
        public long SiteId { get; set; }

        #endregion
        
        public static object SanitationJobTaskProcessRow(SqlDataReader reader)
        {
            // Create instance of object
            b_SanitationJobTask obj = new b_SanitationJobTask();

            // Load the object from the database
            obj.AllSanitationJobTaskLoadFromDatabase(reader);

            // Return result
            return (object)obj;
        }
        public int AllSanitationJobTaskLoadFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SanitationJobTaskId column, bigint, not null
                SanitationJobTaskId = reader.GetInt64(i++);

                // SanitationJobId column, bigint, not null
                SanitationJobId = reader.GetInt64(i++);

                // CancelReason column, nvarchar(15), not null
                CancelReason = reader.GetString(i++);

                // CompleteBy_PersonnelId column, bigint, not null
                CompleteBy_PersonnelId = reader.GetInt64(i++);

                // CompleteComments column, nvarchar(MAX), not null
                CompleteBy = reader.GetString(i++);

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
                Description = reader.GetString(i++);

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // TaskId column, nvarchar(3), not null
                TaskId = reader.GetString(i++);

                // RecordedValue column, nvarchar(31), not null
                RecordedValue = reader.GetString(i++);

                // PerformTime column, nvarchar(10), not null
                PerformTime = reader.GetString(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SanitationJobTaskId"].ToString(); }
                catch { missing.Append("SanitationJobTaskId "); }

                try { reader["SanitationJobId"].ToString(); }
                catch { missing.Append("SanitationJobId "); }

                try { reader["CancelReason"].ToString(); }
                catch { missing.Append("CancelReason "); }

                try { reader["CompleteBy_PersonnelId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelId "); }

                try { reader["CompleteBy"].ToString(); }
                catch { missing.Append("CompleteBy "); }

                try { reader["CompleteComments"].ToString(); }
                catch { missing.Append("CompleteComments "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["TaskId"].ToString(); }
                catch { missing.Append("TaskId "); }

                try { reader["RecordedValue"].ToString(); }
                catch { missing.Append("RecordedValue "); }

                try { reader["PerformTime"].ToString(); }
                catch { missing.Append("PerformTime "); }

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
        //public void SanitationJobTask_RetrieveForWorkBench(
        //    SqlConnection connection,
        //    SqlTransaction transaction,
        //    ref List<b_SanitationJobTask> data
        //)
        //{
        //    //Database.SqlClient.ProcessRow<b_SanitationJobTask> processRow = null;
        //    List<b_SanitationJobTask> results = null;
        //    SqlCommand command = null;
        //    string message = String.Empty;

        //    // Initialize the results
        //    data = new List<b_SanitationJobTask>();

        //    try
        //    {
        //        // Create the command to use in calling the stored procedures
        //        command = new SqlCommand();
        //        command.Connection = connection;
        //        command.Transaction = transaction;

        //        // Call the stored procedure to retrieve the data
        //        //processRow = new Database.SqlClient.ProcessRow<b_SanitationJobTask>(reader => { b_SanitationJobTask obj = new b_SanitationJobTask(); obj.LoadFromDatabase(reader); return obj; });
        //        results = Database.StoredProcedure.usp_SanitationJobTask_RetrieveForWorkBench.CallStoredProcedure(command, this);

        //        // Extract the results
        //        if (null != results)
        //        {
        //            data = new List<b_SanitationJobTask>(results);
        //        }
        //        else
        //        {
        //            data = new List<b_SanitationJobTask>();
        //        }

        //        // Clear the results collection
        //        if (null != results)
        //        {
        //            results.Clear();
        //            results = null;
        //        }
        //    }
        //    finally
        //    {
        //        if (null != command)
        //        {
        //            command.Dispose();
        //            command = null;
        //        }
        //        //processRow = null;
        //        results = null;
        //        message = String.Empty;
        //    }
        //}



        public void RetrieveBySanitationJob(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_SanitationJobTask> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_SanitationJobTask> results = null;
            data = new List<b_SanitationJobTask>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_SanitationJobTask_RetrieveBySanitationJob.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_SanitationJobTask>();
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

        public void UpdateBySanitationJobTaskId(
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
                Database.StoredProcedure.usp_SanitationJobTask_UpdateBySanitationJobTaskId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void RetrieveBy_SanitationJobID(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_SanitationJobTask> data
    )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_SanitationJobTask> results = null;
            data = new List<b_SanitationJobTask>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_SanitationJobTask_RetrieveBySanitationJobId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_SanitationJobTask>();
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
        public void InsertNew_IntoDatabase(
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
                Database.StoredProcedure.usp_SanitationJobTask_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void UpdateInDatabaseBy_SanitationJobTaskID(
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
                Database.StoredProcedure.usp_SanitationJobTask_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void RetrieveSingleBy_SanitationJobId(
              SqlConnection connection,
              SqlTransaction transaction,
              long callerUserInfoId,
        string callerUserName
          )
        {
            Database.SqlClient.ProcessRow<b_SanitationJobTask> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanitationJobTask>(reader => { this.SingleSanitationJobTaskLoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_SanitationJobTask_RetrieveSingleBy_SanitationJobId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public int SingleSanitationJobTaskLoadFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SanitationJobTaskId column, bigint, not null
                SanitationJobTaskId = reader.GetInt64(i++);

                // SanitationJobId column, bigint, not null
                SanitationJobId = reader.GetInt64(i++);

                SanitationMasterTaskId = reader.GetInt64(i++);

                // CancelReason column, nvarchar(15), not null
                CancelReason = reader.GetString(i++);

                // CompleteBy_PersonnelId column, bigint, not null
                CompleteBy_PersonnelId = reader.GetInt64(i++);

                // CompleteComments column, nvarchar(MAX), not null
                CompleteBy = reader.GetString(i++);

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
                Description = reader.GetString(i++);

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // TaskId column, nvarchar(3), not null
                TaskId = reader.GetString(i++);

                // RecordedValue column, nvarchar(31), not null
                RecordedValue = reader.GetString(i++);

                // PerformTime column, nvarchar(10), not null
                PerformTime = reader.GetString(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SanitationJobTaskId"].ToString(); }
                catch { missing.Append("SanitationJobTaskId "); }

                try { reader["SanitationJobId"].ToString(); }
                catch { missing.Append("SanitationJobId "); }

                try { reader["SanitationMasterTaskId"].ToString(); }
                catch { missing.Append("SanitationMasterTaskId "); }

                try { reader["CancelReason"].ToString(); }
                catch { missing.Append("CancelReason "); }

                try { reader["CompleteBy_PersonnelId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelId "); }
                try { reader["CompleteBy"].ToString(); }
                catch { missing.Append("CompleteBy "); }

                try { reader["CompleteComments"].ToString(); }
                catch { missing.Append("CompleteComments "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["TaskId"].ToString(); }
                catch { missing.Append("TaskId "); }

                try { reader["RecordedValue"].ToString(); }
                catch { missing.Append("RecordedValue "); }

                try { reader["PerformTime"].ToString(); }
                catch { missing.Append("PerformTime "); }

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
        //V2-1071
        public static b_SanitationJobTask ProcessRowForDevExpressSanitationJobTask(SqlDataReader reader)
        {
            // Create instance of object
            b_SanitationJobTask obj = new b_SanitationJobTask();

            // Load the object from the database
            obj.LoadFromDatabaseForDevExpressSanitationJobTask(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForDevExpressSanitationJobTask(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // SanitationJobId column, bigint, not null
                SanitationJobId = reader.GetInt64(i++);

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SanitationJobTaskId column, bigint, not null
                SanitationJobTaskId = reader.GetInt64(i++);

                // CancelReason column, nvarchar(15), not null
                CancelReason = reader.GetString(i++);

                // CompleteBy_PersonnelId column, bigint, not null
                CompleteBy_PersonnelId = reader.GetInt64(i++);

                // CompleteComments column, nvarchar(MAX), not null
                CompleteBy = reader.GetString(i++);

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
                Description = reader.GetString(i++);

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // TaskId column, nvarchar(3), not null
                TaskId = reader.GetString(i++);

                // RecordedValue column, nvarchar(31), not null
                RecordedValue = reader.GetString(i++);

                // PerformTime column, nvarchar(10), not null
                PerformTime = reader.GetString(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["SanitationJobId"].ToString(); }
                catch { missing.Append("SanitationJobId "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SanitationJobTaskId"].ToString(); }
                catch { missing.Append("SanitationJobTaskId "); }

                try { reader["CancelReason"].ToString(); }
                catch { missing.Append("CancelReason "); }

                try { reader["CompleteBy_PersonnelId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelId "); }

                try { reader["CompleteBy"].ToString(); }
                catch { missing.Append("CompleteBy "); }

                try { reader["CompleteComments"].ToString(); }
                catch { missing.Append("CompleteComments "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["TaskId"].ToString(); }
                catch { missing.Append("TaskId "); }

                try { reader["RecordedValue"].ToString(); }
                catch { missing.Append("RecordedValue "); }

                try { reader["PerformTime"].ToString(); }
                catch { missing.Append("PerformTime "); }

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
        //
    }
}
