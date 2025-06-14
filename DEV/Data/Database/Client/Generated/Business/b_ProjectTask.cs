/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2021 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the ProjectTask table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_ProjectTask : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_ProjectTask ()
        {
            ClientId = 0;
            ProjectTaskId = 0;
            ProjectId = 0;
            WorkOrderId = 0;
            StartDate = new System.Nullable<System.DateTime>();
            EndDate = new System.Nullable<System.DateTime>();
            Progress = 0;
            Type = String.Empty;
        }

        /// <summary>
        /// ProjectTaskId property
        /// </summary>
        public long ProjectTaskId { get; set; }

        /// <summary>
        /// ProjectId property
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// WorkOrderId property
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// StartDate property
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// EndDate property
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Progress property
        /// </summary>
        public decimal Progress { get; set; }

        /// <summary>
        /// Type property
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_ProjectTask object.
        /// This routine should be applied to the usp_ProjectTask_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_ProjectTask_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_ProjectTask object</returns>
        public static object ProcessRow (SqlDataReader reader)
        {
            // Create instance of object
            b_ProjectTask obj = new b_ProjectTask();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object) obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_ProjectTask object.
        /// This routine should be applied to the usp_ProjectTask_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_ProjectTask_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase (SqlDataReader reader)
        {
        int i = 0;
        try
        {

                        // ClientId column, bigint, not null
                        ClientId = reader.GetInt64(i++);

                        // ProjectTaskId column, bigint, not null
                        ProjectTaskId = reader.GetInt64(i++);

                        // ProjectId column, bigint, not null
                        ProjectId = reader.GetInt64(i++);

                        // WorkOrderId column, bigint, not null
                        WorkOrderId = reader.GetInt64(i++);

            // StartDate column, datetime2, not null
            if (false == reader.IsDBNull(i))
            {
                    StartDate = reader.GetDateTime(i);
            }
            else
            {
                    StartDate = DateTime.MinValue;
            }
            i++;
            // EndDate column, datetime2, not null
            if (false == reader.IsDBNull(i))
            {
                    EndDate = reader.GetDateTime(i);
            }
            else
            {
                    EndDate = DateTime.MinValue;
            }
            i++;
                        // Progress column, decimal(3,2), not null
                        Progress = reader.GetDecimal(i++);

                        // Type column, nvarchar(15), not null
                        Type = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                
                
            try { reader["ClientId"].ToString(); }
            catch { missing.Append("ClientId "); }
            
            try { reader["ProjectTaskId"].ToString(); }
            catch { missing.Append("ProjectTaskId "); }
            
            try { reader["ProjectId"].ToString(); }
            catch { missing.Append("ProjectId "); }
            
            try { reader["WorkOrderId"].ToString(); }
            catch { missing.Append("WorkOrderId "); }
            
            try { reader["StartDate"].ToString(); }
            catch { missing.Append("StartDate "); }
            
            try { reader["EndDate"].ToString(); }
            catch { missing.Append("EndDate "); }
            
            try { reader["Progress"].ToString(); }
            catch { missing.Append("Progress "); }
            
            try { reader["Type"].ToString(); }
            catch { missing.Append("Type "); }
            
                
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

        /// <summary>
        /// Insert this object into the database as a ProjectTask table record.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void InsertIntoDatabase (
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
                Database.StoredProcedure.usp_ProjectTask_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        /// <summary>
        /// Update the ProjectTask table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void UpdateInDatabase (
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
                Database.StoredProcedure.usp_ProjectTask_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        /// <summary>
        /// Delete the ProjectTask table record represented by this object from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void DeleteFromDatabase (
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
                Database.StoredProcedure.usp_ProjectTask_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        /// <summary>
        /// Retrieve all ProjectTask table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_ProjectTask[] that contains the results</param>
        public void RetrieveAllFromDatabase (
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_ProjectTask[] data
        )
        {
            Database.SqlClient.ProcessRow<b_ProjectTask> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_ProjectTask[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ProjectTask>(reader => { b_ProjectTask obj = new b_ProjectTask(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_ProjectTask_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_ProjectTask[])results.ToArray(typeof(b_ProjectTask));
                }
                else
                {
                    data = new b_ProjectTask[0];
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

        /// <summary>
        /// Retrieve ProjectTask table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_ProjectTask[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_ProjectTask> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ProjectTask>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_ProjectTask_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        /// <summary>
        /// Test equality of two b_ProjectTask objects.
        /// </summary>
        /// <param name="obj">b_ProjectTask object to compare against the current object.</param>
        public bool Equals (b_ProjectTask obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (ProjectTaskId != obj.ProjectTaskId) return false;
            if (ProjectId != obj.ProjectId) return false;
            if (WorkOrderId != obj.WorkOrderId) return false;
            if (!StartDate.Equals(obj.StartDate)) return false;
            if (!EndDate.Equals(obj.EndDate)) return false;
            if (Progress != obj.Progress) return false;
            if (!Type.Equals(obj.Type)) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_ProjectTask objects.
        /// </summary>
        /// <param name="obj1">b_ProjectTask object to use in the comparison.</param>
        /// <param name="obj2">b_ProjectTask object to use in the comparison.</param>
        public static bool Equals (b_ProjectTask obj1, b_ProjectTask obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
