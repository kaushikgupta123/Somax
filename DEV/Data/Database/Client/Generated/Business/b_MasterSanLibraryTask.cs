/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2018 by SOMAX Inc.
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
    /// Business object that stores a record from the MasterSanLibraryTask table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_MasterSanLibraryTask : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_MasterSanLibraryTask ()
        {
            ClientId = 0;
            MasterSanLibraryTaskId = 0;
            MasterSanLibraryId = 0;
            TaskId = String.Empty;
            Description = String.Empty;
            Del = false;
            UpdateIndex = 0;
        }

        /// <summary>
        /// MasterSanLibraryTaskId property
        /// </summary>
        public long MasterSanLibraryTaskId { get; set; }

        /// <summary>
        /// MasterSanLibraryId property
        /// </summary>
        public long MasterSanLibraryId { get; set; }

        /// <summary>
        /// TaskId property
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// Description property
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Del property
        /// </summary>
        public bool Del { get; set; }

        /// <summary>
        /// UpdateIndex property
        /// </summary>
        public int UpdateIndex { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_MasterSanLibraryTask object.
        /// This routine should be applied to the usp_MasterSanLibraryTask_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_MasterSanLibraryTask_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_MasterSanLibraryTask object</returns>
        public static object ProcessRow (SqlDataReader reader)
        {
            // Create instance of object
            b_MasterSanLibraryTask obj = new b_MasterSanLibraryTask();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object) obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_MasterSanLibraryTask object.
        /// This routine should be applied to the usp_MasterSanLibraryTask_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_MasterSanLibraryTask_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase (SqlDataReader reader)
        {
        int i = 0;
        try
        {

                        // ClientId column, bigint, not null
                        ClientId = reader.GetInt64(i++);

                        // MasterSanLibraryTaskId column, bigint, not null
                        MasterSanLibraryTaskId = reader.GetInt64(i++);

                        // MasterSanLibraryId column, bigint, not null
                        MasterSanLibraryId = reader.GetInt64(i++);

                        // TaskId column, nvarchar(7), not null
                        TaskId = reader.GetString(i++);

                        // Description column, nvarchar(MAX), not null
                        Description = reader.GetString(i++);

                        // Del column, bit, not null
                        Del = reader.GetBoolean(i++);

                        // UpdateIndex column, int, not null
                        UpdateIndex = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                
                
            try { reader["ClientId"].ToString(); }
            catch { missing.Append("ClientId "); }
            
            try { reader["MasterSanLibraryTaskId"].ToString(); }
            catch { missing.Append("MasterSanLibraryTaskId "); }
            
            try { reader["MasterSanLibraryId"].ToString(); }
            catch { missing.Append("MasterSanLibraryId "); }
            
            try { reader["TaskId"].ToString(); }
            catch { missing.Append("TaskId "); }
            
            try { reader["Description"].ToString(); }
            catch { missing.Append("Description "); }
            
            try { reader["Del"].ToString(); }
            catch { missing.Append("Del "); }
            
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

        /// <summary>
        /// Insert this object into the database as a MasterSanLibraryTask table record.
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
                Database.StoredProcedure.usp_MasterSanLibraryTask_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the MasterSanLibraryTask table record represented by this object in the database.
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
                Database.StoredProcedure.usp_MasterSanLibraryTask_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the MasterSanLibraryTask table record represented by this object from the database.
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
                Database.StoredProcedure.usp_MasterSanLibraryTask_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all MasterSanLibraryTask table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_MasterSanLibraryTask[] that contains the results</param>
        public void RetrieveAllFromDatabase (
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_MasterSanLibraryTask[] data
        )
        {
            Database.SqlClient.ProcessRow<b_MasterSanLibraryTask> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_MasterSanLibraryTask[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_MasterSanLibraryTask>(reader => { b_MasterSanLibraryTask obj = new b_MasterSanLibraryTask(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_MasterSanLibraryTask_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_MasterSanLibraryTask[])results.ToArray(typeof(b_MasterSanLibraryTask));
                }
                else
                {
                    data = new b_MasterSanLibraryTask[0];
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
        /// Retrieve MasterSanLibraryTask table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_MasterSanLibraryTask[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_MasterSanLibraryTask> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_MasterSanLibraryTask>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_MasterSanLibraryTask_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /// Test equality of two b_MasterSanLibraryTask objects.
        /// </summary>
        /// <param name="obj">b_MasterSanLibraryTask object to compare against the current object.</param>
        public bool Equals (b_MasterSanLibraryTask obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (MasterSanLibraryTaskId != obj.MasterSanLibraryTaskId) return false;
            if (MasterSanLibraryId != obj.MasterSanLibraryId) return false;
            if (!TaskId.Equals(obj.TaskId)) return false;
            if (!Description.Equals(obj.Description)) return false;
            if (Del != obj.Del) return false;
            if (UpdateIndex != obj.UpdateIndex) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_MasterSanLibraryTask objects.
        /// </summary>
        /// <param name="obj1">b_MasterSanLibraryTask object to use in the comparison.</param>
        /// <param name="obj2">b_MasterSanLibraryTask object to use in the comparison.</param>
        public static bool Equals (b_MasterSanLibraryTask obj1, b_MasterSanLibraryTask obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
