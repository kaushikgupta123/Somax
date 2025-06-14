/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
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
    /// Business object that stores a record from the Notes table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_Notes : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_Notes ()
        {
            ClientId = 0;
            NotesId = 0;
            OwnerId = 0;
            OwnerName = String.Empty;
            Subject = String.Empty;
            Content = String.Empty;
            Type = String.Empty;
            ObjectId = 0;
            TableName = String.Empty;
            UpdateIndex = 0;
        }

        /// <summary>
        /// NotesId property
        /// </summary>
        public long NotesId { get; set; }

        /// <summary>
        /// OwnerId property
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// OwnerName property
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Subject property
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Content property
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Type property
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// ObjectId property
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        /// TableName property
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// UpdateIndex property
        /// </summary>
        public long UpdateIndex { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_Notes object.
        /// This routine should be applied to the usp_Notes_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Notes_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_Notes object</returns>
        public static object ProcessRow (SqlDataReader reader)
        {
            // Create instance of object
            b_Notes obj = new b_Notes();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object) obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_Notes object.
        /// This routine should be applied to the usp_Notes_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Notes_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public void LoadFromDatabase (SqlDataReader reader)
        {
        int i = 0;
        try
        {

                        // ClientId column, bigint, not null
                        ClientId = reader.GetInt64(i++);

                        // NotesId column, bigint, not null
                        NotesId = reader.GetInt64(i++);

                        // OwnerId column, bigint, not null
                        OwnerId = reader.GetInt64(i++);

                        // OwnerName column, nvarchar(127), not null
                        OwnerName = reader.GetString(i++);

                        // Subject column, nvarchar(255), not null
                        Subject = reader.GetString(i++);

                        // Content column, nvarchar(MAX), not null
                        Content = reader.GetString(i++);

                        // Type column, nvarchar(255), not null
                        Type = reader.GetString(i++);

                        // ObjectId column, bigint, not null
                        ObjectId = reader.GetInt64(i++);

                        // TableName column, nvarchar(63), not null
                        TableName = reader.GetString(i++);

                        // UpdateIndex column, bigint, not null
                        UpdateIndex = reader.GetInt64(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                
                
            try { reader["ClientId"].ToString(); }
            catch { missing.Append("ClientId "); }
            
            try { reader["NotesId"].ToString(); }
            catch { missing.Append("NotesId "); }
            
            try { reader["OwnerId"].ToString(); }
            catch { missing.Append("OwnerId "); }
            
            try { reader["OwnerName"].ToString(); }
            catch { missing.Append("OwnerName "); }
            
            try { reader["Subject"].ToString(); }
            catch { missing.Append("Subject "); }
            
            try { reader["Content"].ToString(); }
            catch { missing.Append("Content "); }
            
            try { reader["Type"].ToString(); }
            catch { missing.Append("Type "); }
            
            try { reader["ObjectId"].ToString(); }
            catch { missing.Append("ObjectId "); }
            
            try { reader["TableName"].ToString(); }
            catch { missing.Append("TableName "); }
            
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
}

        /// <summary>
        /// Insert this object into the database as a Notes table record.
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
                Database.StoredProcedure.usp_Notes_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the Notes table record represented by this object in the database.
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
                Database.StoredProcedure.usp_Notes_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the Notes table record represented by this object from the database.
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
                Database.StoredProcedure.usp_Notes_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all Notes table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_Notes[] that contains the results</param>
        public void RetrieveAllFromDatabase (
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_Notes[] data
        )
        {
            ProcessRow<b_Notes> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Notes[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new ProcessRow<b_Notes>(reader => { b_Notes obj = new b_Notes(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Notes_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_Notes[])results.ToArray(typeof(b_Notes));
                }
                else
                {
                    data = new b_Notes[0];
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
        /// Retrieve Notes table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_Notes[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            ProcessRow<b_Notes> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new ProcessRow<b_Notes>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_Notes_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /// Test equality of two b_Notes objects.
        /// </summary>
        /// <param name="obj">b_Notes object to compare against the current object.</param>
        public bool Equals (b_Notes obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (NotesId != obj.NotesId) return false;
            if (OwnerId != obj.OwnerId) return false;
            if (!OwnerName.Equals(obj.OwnerName)) return false;
            if (!Subject.Equals(obj.Subject)) return false;
            if (!Content.Equals(obj.Content)) return false;
            if (!Type.Equals(obj.Type)) return false;
            if (ObjectId != obj.ObjectId) return false;
            if (!TableName.Equals(obj.TableName)) return false;
            if (UpdateIndex != obj.UpdateIndex) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_Notes objects.
        /// </summary>
        /// <param name="obj1">b_Notes object to use in the comparison.</param>
        /// <param name="obj2">b_Notes object to use in the comparison.</param>
        public static bool Equals (b_Notes obj1, b_Notes obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
