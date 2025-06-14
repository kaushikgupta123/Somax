/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2020 by SOMAX Inc.
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
    /// Business object that stores a record from the AssetGroup2 table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_AssetGroup2 : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_AssetGroup2 ()
        {
            ClientId = 0;
            AssetGroup2Id = 0;
            SiteId = 0;
            ClientLookupId = String.Empty;
            Description = String.Empty;
            InactiveFlag = false;
            CreatedBy = String.Empty;
            UpdateIndex = 0;
        }

        /// <summary>
        /// AssetGroup2Id property
        /// </summary>
        public long AssetGroup2Id { get; set; }

        /// <summary>
        /// SiteId property
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// ClientLookupId property
        /// </summary>
        public string ClientLookupId { get; set; }

        /// <summary>
        /// Description property
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// InactiveFlag property
        /// </summary>
        public bool InactiveFlag { get; set; }

        /// <summary>
        /// CreatedBy property
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// UpdateIndex property
        /// </summary>
        public int UpdateIndex { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_AssetGroup2 object.
        /// This routine should be applied to the usp_AssetGroup2_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_AssetGroup2_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_AssetGroup2 object</returns>
        public static object ProcessRow (SqlDataReader reader)
        {
            // Create instance of object
            b_AssetGroup2 obj = new b_AssetGroup2();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object) obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_AssetGroup2 object.
        /// This routine should be applied to the usp_AssetGroup2_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_AssetGroup2_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase (SqlDataReader reader)
        {
        int i = 0;
        try
        {

                        // ClientId column, bigint, not null
                        ClientId = reader.GetInt64(i++);

                        // AssetGroup2Id column, bigint, not null
                        AssetGroup2Id = reader.GetInt64(i++);

                        // SiteId column, bigint, not null
                        SiteId = reader.GetInt64(i++);

                        // ClientLookupId column, nvarchar(31), not null
                        ClientLookupId = reader.GetString(i++);

                        // Description column, nvarchar(63), not null
                        Description = reader.GetString(i++);

                        // InactiveFlag column, bit, not null
                        InactiveFlag = reader.GetBoolean(i++);

                        // CreatedBy column, nvarchar(255), not null
                        CreatedBy = reader.GetString(i++);

                        // UpdateIndex column, int, not null
                        UpdateIndex = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                
                
            try { reader["ClientId"].ToString(); }
            catch { missing.Append("ClientId "); }
            
            try { reader["AssetGroup2Id"].ToString(); }
            catch { missing.Append("AssetGroup2Id "); }
            
            try { reader["SiteId"].ToString(); }
            catch { missing.Append("SiteId "); }
            
            try { reader["ClientLookupId"].ToString(); }
            catch { missing.Append("ClientLookupId "); }
            
            try { reader["Description"].ToString(); }
            catch { missing.Append("Description "); }
            
            try { reader["InactiveFlag"].ToString(); }
            catch { missing.Append("InactiveFlag "); }
            
            try { reader["CreatedBy"].ToString(); }
            catch { missing.Append("CreatedBy "); }
            
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
        /// Insert this object into the database as a AssetGroup2 table record.
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
                Database.StoredProcedure.usp_AssetGroup2_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the AssetGroup2 table record represented by this object in the database.
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
                Database.StoredProcedure.usp_AssetGroup2_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the AssetGroup2 table record represented by this object from the database.
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
                Database.StoredProcedure.usp_AssetGroup2_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all AssetGroup2 table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_AssetGroup2[] that contains the results</param>
        public void RetrieveAllFromDatabase (
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_AssetGroup2[] data
        )
        {
            Database.SqlClient.ProcessRow<b_AssetGroup2> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_AssetGroup2[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_AssetGroup2>(reader => { b_AssetGroup2 obj = new b_AssetGroup2(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_AssetGroup2_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_AssetGroup2[])results.ToArray(typeof(b_AssetGroup2));
                }
                else
                {
                    data = new b_AssetGroup2[0];
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
        /// Retrieve AssetGroup2 table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_AssetGroup2[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_AssetGroup2> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_AssetGroup2>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_AssetGroup2_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /// Test equality of two b_AssetGroup2 objects.
        /// </summary>
        /// <param name="obj">b_AssetGroup2 object to compare against the current object.</param>
        public bool Equals (b_AssetGroup2 obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (AssetGroup2Id != obj.AssetGroup2Id) return false;
            if (SiteId != obj.SiteId) return false;
            if (!ClientLookupId.Equals(obj.ClientLookupId)) return false;
            if (!Description.Equals(obj.Description)) return false;
            if (InactiveFlag != obj.InactiveFlag) return false;
            if (!CreatedBy.Equals(obj.CreatedBy)) return false;
            if (UpdateIndex != obj.UpdateIndex) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_AssetGroup2 objects.
        /// </summary>
        /// <param name="obj1">b_AssetGroup2 object to use in the comparison.</param>
        /// <param name="obj2">b_AssetGroup2 object to use in the comparison.</param>
        public static bool Equals (b_AssetGroup2 obj1, b_AssetGroup2 obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
