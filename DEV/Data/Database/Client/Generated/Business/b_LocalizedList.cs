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
    /// Business object that stores a record from the LocalizedList table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_LocalizedList : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_LocalizedList ()
        {
            ClientId = 0;
            LocalizedListId = 0;
            ListName = String.Empty;
            ListKey = String.Empty;
            Language = String.Empty;
            Culture = String.Empty;
            Description = String.Empty;
            IsHidden = false;
            IsReadOnly = false;
            UpdateIndex = 0;
        }

        /// <summary>
        /// LocalizedListId property
        /// </summary>
        public long LocalizedListId { get; set; }

        /// <summary>
        /// ListName property
        /// </summary>
        public string ListName { get; set; }

        /// <summary>
        /// ListKey property
        /// </summary>
        public string ListKey { get; set; }

        /// <summary>
        /// Language property
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Culture property
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Description property
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// IsHidden property
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// IsReadOnly property
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// UpdateIndex property
        /// </summary>
        public int UpdateIndex { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_LocalizedList object.
        /// This routine should be applied to the usp_LocalizedList_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_LocalizedList_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_LocalizedList object</returns>
        public static object ProcessRow (SqlDataReader reader)
        {
            // Create instance of object
            b_LocalizedList obj = new b_LocalizedList();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object) obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_LocalizedList object.
        /// This routine should be applied to the usp_LocalizedList_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_LocalizedList_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase (SqlDataReader reader)
        {
        int i = 0;
        try
        {

                        // ClientId column, bigint, not null
                        ClientId = reader.GetInt64(i++);

                        // LocalizedListId column, bigint, not null
                        LocalizedListId = reader.GetInt64(i++);

                        // ListName column, nvarchar(15), not null
                        ListName = reader.GetString(i++);

                        // ListKey column, nvarchar(15), not null
                        ListKey = reader.GetString(i++);

                        // Language column, nvarchar(3), not null
                        Language = reader.GetString(i++);

                        // Culture column, nvarchar(3), not null
                        Culture = reader.GetString(i++);

                        // Description column, nvarchar(63), not null
                        Description = reader.GetString(i++);

                        // IsHidden column, bit, not null
                        IsHidden = reader.GetBoolean(i++);

                        // IsReadOnly column, bit, not null
                        IsReadOnly = reader.GetBoolean(i++);

                        // UpdateIndex column, int, not null
                        UpdateIndex = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                
                
            try { reader["ClientId"].ToString(); }
            catch { missing.Append("ClientId "); }
            
            try { reader["LocalizedListId"].ToString(); }
            catch { missing.Append("LocalizedListId "); }
            
            try { reader["ListName"].ToString(); }
            catch { missing.Append("ListName "); }
            
            try { reader["ListKey"].ToString(); }
            catch { missing.Append("ListKey "); }
            
            try { reader["Language"].ToString(); }
            catch { missing.Append("Language "); }
            
            try { reader["Culture"].ToString(); }
            catch { missing.Append("Culture "); }
            
            try { reader["Description"].ToString(); }
            catch { missing.Append("Description "); }
            
            try { reader["IsHidden"].ToString(); }
            catch { missing.Append("IsHidden "); }
            
            try { reader["IsReadOnly"].ToString(); }
            catch { missing.Append("IsReadOnly "); }
            
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
        /// Insert this object into the database as a LocalizedList table record.
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
                Database.StoredProcedure.usp_LocalizedList_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the LocalizedList table record represented by this object in the database.
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
                Database.StoredProcedure.usp_LocalizedList_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the LocalizedList table record represented by this object from the database.
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
                Database.StoredProcedure.usp_LocalizedList_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all LocalizedList table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_LocalizedList[] that contains the results</param>
        public void RetrieveAllFromDatabase (
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_LocalizedList[] data
        )
        {
            Database.SqlClient.ProcessRow<b_LocalizedList> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_LocalizedList[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_LocalizedList>(reader => { b_LocalizedList obj = new b_LocalizedList(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_LocalizedList_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_LocalizedList[])results.ToArray(typeof(b_LocalizedList));
                }
                else
                {
                    data = new b_LocalizedList[0];
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
        /// Retrieve LocalizedList table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_LocalizedList[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_LocalizedList> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_LocalizedList>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_LocalizedList_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /// Test equality of two b_LocalizedList objects.
        /// </summary>
        /// <param name="obj">b_LocalizedList object to compare against the current object.</param>
        public bool Equals (b_LocalizedList obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (LocalizedListId != obj.LocalizedListId) return false;
            if (!ListName.Equals(obj.ListName)) return false;
            if (!ListKey.Equals(obj.ListKey)) return false;
            if (!Language.Equals(obj.Language)) return false;
            if (!Culture.Equals(obj.Culture)) return false;
            if (!Description.Equals(obj.Description)) return false;
            if (IsHidden != obj.IsHidden) return false;
            if (IsReadOnly != obj.IsReadOnly) return false;
            if (UpdateIndex != obj.UpdateIndex) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_LocalizedList objects.
        /// </summary>
        /// <param name="obj1">b_LocalizedList object to use in the comparison.</param>
        /// <param name="obj2">b_LocalizedList object to use in the comparison.</param>
        public static bool Equals (b_LocalizedList obj1, b_LocalizedList obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
