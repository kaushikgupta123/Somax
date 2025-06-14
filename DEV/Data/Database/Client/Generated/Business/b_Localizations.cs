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
    /// Business object that stores a record from the Localizations table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_Localizations : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_Localizations()
        {
            ClientId = 0;
            LocalizationId = 0;
            ResourceId = String.Empty;
            Value = String.Empty;
            LocaleId = String.Empty;
            ResourceSet = String.Empty;
            Type = String.Empty;
            BinFile = new byte[0];
            TextFile = String.Empty;
            Filename = String.Empty;
            Comment = String.Empty;
            ValueType = 0;
            Updated = new System.Nullable<System.DateTime>();
        }
        

        /// <summary>
        /// Localizationd property
        /// </summary>
        public long LocalizationId { get; set; }

        /// <summary>
        /// ResourceId property
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// Value property
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// LocaleId property
        /// </summary>
        public string LocaleId { get; set; }

        /// <summary>
        /// ResourceSet property
        /// </summary>
        public string ResourceSet { get; set; }

        /// <summary>
        /// Type property
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// BinFile property
        /// </summary>
        public byte[] BinFile { get; set; }

        /// <summary>
        /// TextFile property
        /// </summary>
        public string TextFile { get; set; }

        /// <summary>
        /// Filename property
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Comment property
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// ValueType property
        /// </summary>
        public int ValueType { get; set; }

        /// <summary>
        /// Updated property
        /// </summary>
        public DateTime? Updated { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_Localizations object.
        /// This routine should be applied to the usp_Localizations_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Localizations_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_Localizations object</returns>
        public static object ProcessRow(SqlDataReader reader)
        {
            // Create instance of object
            b_Localizations obj = new b_Localizations();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_Localizations object.
        /// This routine should be applied to the usp_Localizations_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Localizations_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // Localizationd column, bigint, not null
                LocalizationId = reader.GetInt64(i++);

                // ResourceId column, nvarchar(1024), not null
                ResourceId = reader.GetString(i++);

                // Value column, nvarchar(MAX), not null
                Value = reader.GetString(i++);

                // LocaleId column, nvarchar(10), not null
                LocaleId = reader.GetString(i++);

                // ResourceSet column, nvarchar(512), not null
                ResourceSet = reader.GetString(i++);

                // Type column, nvarchar(512), not null
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = "";
                }
                i++;

                // BinFile column, varbinary(MAX), not null
                if (false == reader.IsDBNull(i))
                {
                    BinFile = reader.GetSqlBytes(i).Value;
                }
                else
                {
                    BinFile = new Byte[0];
                }
                i++;

                // TextFile column, nvarchar(MAX), not null
                if (false == reader.IsDBNull(i))
                {
                    TextFile = reader.GetString(i);
                }
                else
                {
                    TextFile = "";
                }
                i++;
                // Filename column, nvarchar(128), not null
                if (false == reader.IsDBNull(i))
                {
                    Filename = reader.GetString(i);
                }
                else
                {
                    Filename = "";
                }
                i++;
                // Comment column, nvarchar(512), not null
                if (false == reader.IsDBNull(i))
                {
                    Comment = reader.GetString(i);
                }
                else
                {
                    Comment = "";
                }
                i++;
                // ValueType column, int, not null
                if (false == reader.IsDBNull(i))
                {
                    ValueType = reader.GetInt32(i);
                }
                else
                {
                    ValueType = 0;
                }
                i++;
                // Updated column, datetime, not null
                if (false == reader.IsDBNull(i))
                {
                    Updated = reader.GetDateTime(i);
                }
                else
                {
                    Updated = DateTime.MinValue;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["Localizationd"].ToString(); }
                catch { missing.Append("Localizationd "); }

                try { reader["ResourceId"].ToString(); }
                catch { missing.Append("ResourceId "); }

                try { reader["Value"].ToString(); }
                catch { missing.Append("Value "); }

                try { reader["LocaleId"].ToString(); }
                catch { missing.Append("LocaleId "); }

                try { reader["ResourceSet"].ToString(); }
                catch { missing.Append("ResourceSet "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["BinFile"].ToString(); }
                catch { missing.Append("BinFile "); }

                try { reader["TextFile"].ToString(); }
                catch { missing.Append("TextFile "); }

                try { reader["Filename"].ToString(); }
                catch { missing.Append("Filename "); }

                try { reader["Comment"].ToString(); }
                catch { missing.Append("Comment "); }

                try { reader["ValueType"].ToString(); }
                catch { missing.Append("ValueType "); }

                try { reader["Updated"].ToString(); }
                catch { missing.Append("Updated "); }


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
        /// Insert this object into the database as a Localizations table record.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void InsertIntoDatabase(
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
                Database.StoredProcedure.usp_Localizations_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the Localizations table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void UpdateInDatabase(
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
                Database.StoredProcedure.usp_Localizations_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the Localizations table record represented by this object from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void DeleteFromDatabase(
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
                Database.StoredProcedure.usp_Localizations_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all Localizations table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_Localizations[] that contains the results</param>
        public void RetrieveAllFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_Localizations[] data
        )
        {
            ProcessRow<b_Localizations> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Localizations[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new ProcessRow<b_Localizations>(reader => { b_Localizations obj = new b_Localizations(); obj.LoadFromDatabase(reader); return obj; });
                Database.StoredProcedure.usp_Localizations_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_Localizations[])results.ToArray(typeof(b_Localizations));
                }
                else
                {
                    data = new b_Localizations[0];
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
        /// Retrieve Localizations table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_Localizations[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            ProcessRow<b_Localizations> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new ProcessRow<b_Localizations>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_Localizations_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /// Test equality of two b_Localizations objects.
        /// </summary>
        /// <param name="obj">b_Localizations object to compare against the current object.</param>
        public bool Equals(b_Localizations obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (LocalizationId != obj.LocalizationId) return false;
            if (!ResourceId.Equals(obj.ResourceId)) return false;
            if (!Value.Equals(obj.Value)) return false;
            if (!LocaleId.Equals(obj.LocaleId)) return false;
            if (!ResourceSet.Equals(obj.ResourceSet)) return false;
            if (!Type.Equals(obj.Type)) return false;
            if (!BinFile.Equals(obj.BinFile)) return false;
            if (!TextFile.Equals(obj.TextFile)) return false;
            if (!Filename.Equals(obj.Filename)) return false;
            if (!Comment.Equals(obj.Comment)) return false;
            if (ValueType != obj.ValueType) return false;
            if (!Updated.Equals(obj.Updated)) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_Localizations objects.
        /// </summary>
        /// <param name="obj1">b_Localizations object to use in the comparison.</param>
        /// <param name="obj2">b_Localizations object to use in the comparison.</param>
        public static bool Equals(b_Localizations obj1, b_Localizations obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
