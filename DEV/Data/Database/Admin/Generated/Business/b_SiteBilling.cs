/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2023 by SOMAX Inc.
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
    /// Business object that stores a record from the SiteBilling table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_SiteBilling : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_SiteBilling ()
        {
            ClientId = 0;
            SiteId = 0;
            SiteBillingId = 0;
            AnniversaryDate = new System.Nullable<System.DateTime>();
            InvoiceFreq = String.Empty;
            Terms = String.Empty;
            CurrentInvoice = String.Empty;
            InvoiceDate = new System.Nullable<System.DateTime>();
            NextInvoiceDate = new System.Nullable<System.DateTime>();
            QuoteRequired = false;
        }

        /// <summary>
        /// SiteId property
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// SiteBillingId property
        /// </summary>
        public long SiteBillingId { get; set; }

        /// <summary>
        /// AnniversaryDate property
        /// </summary>
        public DateTime? AnniversaryDate { get; set; }

        /// <summary>
        /// InvoiceFreq property
        /// </summary>
        public string InvoiceFreq { get; set; }

        /// <summary>
        /// Terms property
        /// </summary>
        public string Terms { get; set; }

        /// <summary>
        /// CurrentInvoice property
        /// </summary>
        public string CurrentInvoice { get; set; }

        /// <summary>
        /// InvoiceDate property
        /// </summary>
        public DateTime? InvoiceDate { get; set; }

        /// <summary>
        /// NextInvoiceDate property
        /// </summary>
        public DateTime? NextInvoiceDate { get; set; }

        /// <summary>
        /// QuoteRequired property
        /// </summary>
        public bool QuoteRequired { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_SiteBilling object.
        /// This routine should be applied to the usp_SiteBilling_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_SiteBilling_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_SiteBilling object</returns>
        public static object ProcessRow (SqlDataReader reader)
        {
            // Create instance of object
            b_SiteBilling obj = new b_SiteBilling();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object) obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_SiteBilling object.
        /// This routine should be applied to the usp_SiteBilling_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_SiteBilling_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase (SqlDataReader reader)
        {
        int i = 0;
        try
        {

                        // ClientId column, bigint, not null
                        ClientId = reader.GetInt64(i++);

                        // SiteId column, bigint, not null
                        SiteId = reader.GetInt64(i++);

                        // SiteBillingId column, bigint, not null
                        SiteBillingId = reader.GetInt64(i++);

            // AnniversaryDate column, datetime2, not null
            if (false == reader.IsDBNull(i))
            {
                    AnniversaryDate = reader.GetDateTime(i);
            }
            else
            {
                    AnniversaryDate = DateTime.MinValue;
            }
            i++;
                        // InvoiceFreq column, nvarchar(15), not null
                        InvoiceFreq = reader.GetString(i++);

                        // Terms column, nvarchar(31), not null
                        Terms = reader.GetString(i++);

                        // CurrentInvoice column, nvarchar(15), not null
                        CurrentInvoice = reader.GetString(i++);

            // InvoiceDate column, datetime2, not null
            if (false == reader.IsDBNull(i))
            {
                    InvoiceDate = reader.GetDateTime(i);
            }
            else
            {
                    InvoiceDate = DateTime.MinValue;
            }
            i++;
            // NextInvoiceDate column, datetime2, not null
            if (false == reader.IsDBNull(i))
            {
                    NextInvoiceDate = reader.GetDateTime(i);
            }
            else
            {
                    NextInvoiceDate = DateTime.MinValue;
            }
            i++;
                        // QuoteRequired column, bit, not null
                        QuoteRequired = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                
                
            try { reader["ClientId"].ToString(); }
            catch { missing.Append("ClientId "); }
            
            try { reader["SiteId"].ToString(); }
            catch { missing.Append("SiteId "); }
            
            try { reader["SiteBillingId"].ToString(); }
            catch { missing.Append("SiteBillingId "); }
            
            try { reader["AnniversaryDate"].ToString(); }
            catch { missing.Append("AnniversaryDate "); }
            
            try { reader["InvoiceFreq"].ToString(); }
            catch { missing.Append("InvoiceFreq "); }
            
            try { reader["Terms"].ToString(); }
            catch { missing.Append("Terms "); }
            
            try { reader["CurrentInvoice"].ToString(); }
            catch { missing.Append("CurrentInvoice "); }
            
            try { reader["InvoiceDate"].ToString(); }
            catch { missing.Append("InvoiceDate "); }
            
            try { reader["NextInvoiceDate"].ToString(); }
            catch { missing.Append("NextInvoiceDate "); }
            
            try { reader["QuoteRequired"].ToString(); }
            catch { missing.Append("QuoteRequired "); }
            
                
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
        /// Insert this object into the database as a SiteBilling table record.
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
                Database.StoredProcedure.usp_SiteBilling_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the SiteBilling table record represented by this object in the database.
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
                Database.StoredProcedure.usp_SiteBilling_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the SiteBilling table record represented by this object from the database.
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
                Database.StoredProcedure.usp_SiteBilling_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all SiteBilling table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_SiteBilling[] that contains the results</param>
        public void RetrieveAllFromDatabase (
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_SiteBilling[] data
        )
        {
            Database.SqlClient.ProcessRow<b_SiteBilling> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_SiteBilling[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SiteBilling>(reader => { b_SiteBilling obj = new b_SiteBilling(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_SiteBilling_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_SiteBilling[])results.ToArray(typeof(b_SiteBilling));
                }
                else
                {
                    data = new b_SiteBilling[0];
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
        /// Retrieve SiteBilling table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_SiteBilling[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_SiteBilling> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SiteBilling>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_SiteBilling_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /// Test equality of two b_SiteBilling objects.
        /// </summary>
        /// <param name="obj">b_SiteBilling object to compare against the current object.</param>
        public bool Equals (b_SiteBilling obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (SiteId != obj.SiteId) return false;
            if (SiteBillingId != obj.SiteBillingId) return false;
            if (!AnniversaryDate.Equals(obj.AnniversaryDate)) return false;
            if (!InvoiceFreq.Equals(obj.InvoiceFreq)) return false;
            if (!Terms.Equals(obj.Terms)) return false;
            if (!CurrentInvoice.Equals(obj.CurrentInvoice)) return false;
            if (!InvoiceDate.Equals(obj.InvoiceDate)) return false;
            if (!NextInvoiceDate.Equals(obj.NextInvoiceDate)) return false;
            if (QuoteRequired != obj.QuoteRequired) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_SiteBilling objects.
        /// </summary>
        /// <param name="obj1">b_SiteBilling object to use in the comparison.</param>
        /// <param name="obj2">b_SiteBilling object to use in the comparison.</param>
        public static bool Equals (b_SiteBilling obj1, b_SiteBilling obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
