/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2013 by SOMAX Inc.
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
    /// Business object that stores a record from the AlertFollow table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_AlertFollow : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_AlertFollow ()
        {
            ClientId = 0;
            AlertFollowId = 0;
            UserInfoId = 0;
            ObjectId = 0;
            ObjectType = String.Empty;
            CreatedDate = new System.Nullable<System.DateTime>();
        }

        /// <summary>
        /// AlertFollowId property
        /// </summary>
        public long AlertFollowId { get; set; }

        /// <summary>
        /// UserInfoId property
        /// </summary>
        public long UserInfoId { get; set; }

        /// <summary>
        /// ObjectId property
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        /// ObjectType property
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// CreatedDate property
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_AlertFollow object.
        /// This routine should be applied to the usp_AlertFollow_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_AlertFollow_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_AlertFollow object</returns>
        public static object ProcessRow (SqlDataReader reader)
        {
            // Create instance of object
            b_AlertFollow obj = new b_AlertFollow();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object) obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_AlertFollow object.
        /// This routine should be applied to the usp_AlertFollow_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_AlertFollow_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase (SqlDataReader reader)
        {
        int i = 0;
        try
        {

                        // ClientId column, bigint, not null
                        ClientId = reader.GetInt64(i++);

                        // AlertFollowId column, bigint, not null
                        AlertFollowId = reader.GetInt64(i++);

                        // UserInfoId column, bigint, not null
                        UserInfoId = reader.GetInt64(i++);

                        // ObjectId column, bigint, not null
                        ObjectId = reader.GetInt64(i++);

                        // ObjectType column, nvarchar(127), not null
                        ObjectType = reader.GetString(i++);

            // CreatedDate column, datetime2, not null
            if (false == reader.IsDBNull(i))
            {
                    CreatedDate = reader.GetDateTime(i);
            }
            else
            {
                    CreatedDate = DateTime.MinValue;
            }
            i++;            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                
                
            try { reader["ClientId"].ToString(); }
            catch { missing.Append("ClientId "); }
            
            try { reader["AlertFollowId"].ToString(); }
            catch { missing.Append("AlertFollowId "); }
            
            try { reader["UserInfoId"].ToString(); }
            catch { missing.Append("UserInfoId "); }
            
            try { reader["ObjectId"].ToString(); }
            catch { missing.Append("ObjectId "); }
            
            try { reader["ObjectType"].ToString(); }
            catch { missing.Append("ObjectType "); }
            
            try { reader["CreatedDate"].ToString(); }
            catch { missing.Append("CreatedDate "); }
            
                
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
        /// Insert this object into the database as a AlertFollow table record.
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
                Database.StoredProcedure.usp_AlertFollow_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the AlertFollow table record represented by this object in the database.
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
                Database.StoredProcedure.usp_AlertFollow_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the AlertFollow table record represented by this object from the database.
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
                Database.StoredProcedure.usp_AlertFollow_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all AlertFollow table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_AlertFollow[] that contains the results</param>
        public void RetrieveAllFromDatabase (
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_AlertFollow[] data
        )
        {
            Database.SqlClient.ProcessRow<b_AlertFollow> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_AlertFollow[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_AlertFollow>(reader => { b_AlertFollow obj = new b_AlertFollow(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_AlertFollow_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_AlertFollow[])results.ToArray(typeof(b_AlertFollow));
                }
                else
                {
                    data = new b_AlertFollow[0];
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
        /// Retrieve AlertFollow table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_AlertFollow[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_AlertFollow> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_AlertFollow>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_AlertFollow_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /// Test equality of two b_AlertFollow objects.
        /// </summary>
        /// <param name="obj">b_AlertFollow object to compare against the current object.</param>
        public bool Equals (b_AlertFollow obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (AlertFollowId != obj.AlertFollowId) return false;
            if (UserInfoId != obj.UserInfoId) return false;
            if (ObjectId != obj.ObjectId) return false;
            if (!ObjectType.Equals(obj.ObjectType)) return false;
            if (!CreatedDate.Equals(obj.CreatedDate)) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_AlertFollow objects.
        /// </summary>
        /// <param name="obj1">b_AlertFollow object to use in the comparison.</param>
        /// <param name="obj2">b_AlertFollow object to use in the comparison.</param>
        public static bool Equals (b_AlertFollow obj1, b_AlertFollow obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
