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
    /// Business object that stores a record from the DataImportLog table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_DataImportLog : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_DataImportLog ()
        {
            ClientId = 0;
            DataImportLogId = 0;
            SiteId = 0;
            RunBy_PersonnelId = 0;
            Type = String.Empty;
            SuccessfulTransactions = 0;
            Filename = String.Empty;
            RunDate = new System.Nullable<System.DateTime>();
        }

        /// <summary>
        /// DataImportLogId property
        /// </summary>
        public long DataImportLogId { get; set; }

        /// <summary>
        /// SiteId property
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// RunBy_PersonnelId property
        /// </summary>
        public long RunBy_PersonnelId { get; set; }

        /// <summary>
        /// Type property
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// SuccessfulTransactions property
        /// </summary>
        public int SuccessfulTransactions { get; set; }

        /// <summary>
        /// Filename property
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// RunDate property
        /// </summary>
        public DateTime? RunDate { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_DataImportLog object.
        /// This routine should be applied to the usp_DataImportLog_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_DataImportLog_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_DataImportLog object</returns>
        public static object ProcessRow (SqlDataReader reader)
        {
            // Create instance of object
            b_DataImportLog obj = new b_DataImportLog();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object) obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_DataImportLog object.
        /// This routine should be applied to the usp_DataImportLog_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_DataImportLog_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase (SqlDataReader reader)
        {
        int i = 0;
        try
        {

                        // ClientId column, bigint, not null
                        ClientId = reader.GetInt64(i++);

                        // DataImportLogId column, bigint, not null
                        DataImportLogId = reader.GetInt64(i++);

                        // SiteId column, bigint, not null
                        SiteId = reader.GetInt64(i++);

                        // RunBy_PersonnelId column, bigint, not null
                        RunBy_PersonnelId = reader.GetInt64(i++);

                        // Type column, nvarchar(15), not null
                        Type = reader.GetString(i++);

                        // SuccessfulTransactions column, int, not null
                        SuccessfulTransactions = reader.GetInt32(i++);

                        // Filename column, nvarchar(255), not null
                        Filename = reader.GetString(i++);

            // RunDate column, datetime2, not null
            if (false == reader.IsDBNull(i))
            {
                    RunDate = reader.GetDateTime(i);
            }
            else
            {
                    RunDate = DateTime.MinValue;
            }
            i++;            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                
                
            try { reader["ClientId"].ToString(); }
            catch { missing.Append("ClientId "); }
            
            try { reader["DataImportLogId"].ToString(); }
            catch { missing.Append("DataImportLogId "); }
            
            try { reader["SiteId"].ToString(); }
            catch { missing.Append("SiteId "); }
            
            try { reader["RunBy_PersonnelId"].ToString(); }
            catch { missing.Append("RunBy_PersonnelId "); }
            
            try { reader["Type"].ToString(); }
            catch { missing.Append("Type "); }
            
            try { reader["SuccessfulTransactions"].ToString(); }
            catch { missing.Append("SuccessfulTransactions "); }
            
            try { reader["Filename"].ToString(); }
            catch { missing.Append("Filename "); }
            
            try { reader["RunDate"].ToString(); }
            catch { missing.Append("RunDate "); }
            
                
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
        /// Insert this object into the database as a DataImportLog table record.
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
                Database.StoredProcedure.usp_DataImportLog_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the DataImportLog table record represented by this object in the database.
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
                Database.StoredProcedure.usp_DataImportLog_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the DataImportLog table record represented by this object from the database.
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
                Database.StoredProcedure.usp_DataImportLog_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all DataImportLog table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_DataImportLog[] that contains the results</param>
        public void RetrieveAllFromDatabase (
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_DataImportLog[] data
        )
        {
            Database.SqlClient.ProcessRow<b_DataImportLog> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_DataImportLog[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_DataImportLog>(reader => { b_DataImportLog obj = new b_DataImportLog(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_DataImportLog_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_DataImportLog[])results.ToArray(typeof(b_DataImportLog));
                }
                else
                {
                    data = new b_DataImportLog[0];
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
        /// Retrieve DataImportLog table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_DataImportLog[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_DataImportLog> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_DataImportLog>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_DataImportLog_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /// Test equality of two b_DataImportLog objects.
        /// </summary>
        /// <param name="obj">b_DataImportLog object to compare against the current object.</param>
        public bool Equals (b_DataImportLog obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (DataImportLogId != obj.DataImportLogId) return false;
            if (SiteId != obj.SiteId) return false;
            if (RunBy_PersonnelId != obj.RunBy_PersonnelId) return false;
            if (!Type.Equals(obj.Type)) return false;
            if (SuccessfulTransactions != obj.SuccessfulTransactions) return false;
            if (!Filename.Equals(obj.Filename)) return false;
            if (!RunDate.Equals(obj.RunDate)) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_DataImportLog objects.
        /// </summary>
        /// <param name="obj1">b_DataImportLog object to use in the comparison.</param>
        /// <param name="obj2">b_DataImportLog object to use in the comparison.</param>
        public static bool Equals (b_DataImportLog obj1, b_DataImportLog obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
