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

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the ChangeLog table.
    /// </summary>
    public partial class b_ChangeLog 
    {
        public static object ProcessRowExtended(SqlDataReader reader)
        {
            // Create instance of object
            b_ChangeLog obj = new b_ChangeLog();

            // Load the object from the database
            obj.LoadExtendedFromDatabase(reader);

            // Return result
            return (object)obj;
        }


        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_ChangeLog object.
        /// This routine should be applied to the usp_ChangeLog_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_ChangeLog_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public void LoadExtendedFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // ChangeLogId column, bigint, not null
                ChangeLogId = reader.GetInt64(i++);

                // TableName column, nvarchar(63), not null
                TableName = reader.GetString(i++);

                // ObjectId column, bigint, not null
                ObjectId = reader.GetInt64(i++);

                // UserName column, nvarchar(63), not null
                UserName = reader.GetString(i++);

                // UserInfoId column, bigint, not null
                UserInfoId = reader.GetInt64(i++);

                // History column, nvarchar(MAX), not null
                History = reader.GetSqlXml(i++).Value;

                // SiteId column, bigint, null
                if (false == reader.IsDBNull(i))
                {
                    SiteId = reader.GetInt64(i);
                }
                i++;

                // AreaId column, bigint, null
                if (false == reader.IsDBNull(i))
                {
                    AreaId = reader.GetInt64(i);
                }
                i++;

                // DepartmentId column, bigint, null
                if (false == reader.IsDBNull(i))
                {
                    DepartmentId = reader.GetInt64(i);
                }
                i++;

                // StoreRoomId column, bigint, null
                if (false == reader.IsDBNull(i))
                {
                    StoreroomId = reader.GetInt64(i);
                }
                i++;

                // UpdateIndex column, bigint, not null
                UpdateIndex = reader.GetInt64(i++);

                CreateDate = reader.GetDateTime(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["ChangeLogId"].ToString(); }
                catch { missing.Append("ChangeLogId "); }

                try { reader["TableName"].ToString(); }
                catch { missing.Append("TableName "); }

                try { reader["ObjectId"].ToString(); }
                catch { missing.Append("ObjectId "); }

                try { reader["UserName"].ToString(); }
                catch { missing.Append("UserName "); }

                try { reader["UserInfoId"].ToString(); }
                catch { missing.Append("UserInfoId "); }

                try { reader["History"].ToString(); }
                catch { missing.Append("History "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreRoomId"].ToString(); }
                catch { missing.Append("StoreRoomId "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public DateTime CreateDate { get; set; }
 
        /// <summary>
        /// Retrieve ChangeLog table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_ChangeLog[] that contains the results</param>
        public void RetrieveByObjectIdFromDatabase(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
   string callerUserName,
         ref b_ChangeLog[] data
     )
        {
            Database.SqlClient.ProcessRow<b_ChangeLog> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_ChangeLog[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ChangeLog>(reader => { b_ChangeLog obj = new b_ChangeLog(); obj.LoadExtendedFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_ChangeLog_RetrieveByObjectId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, ObjectId, TableName);

                // Extract the results
                if (null != results)
                {
                    data = (b_ChangeLog[])results.ToArray(typeof(b_ChangeLog));
                }
                else
                {
                    data = new b_ChangeLog[0];
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

        public void RetrieveExtendedByPKFromDatabase(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
     string callerUserName
       )
        {
            Database.SqlClient.ProcessRow<b_ChangeLog> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ChangeLog>(reader => { this.LoadExtendedFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_ChangeLog_RetrieveExtendedByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

    }
}
