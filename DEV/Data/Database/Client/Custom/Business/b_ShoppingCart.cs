/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Changed Parameters
****************************************************************************************************
 */
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_ShoppingCart
    {
        public decimal LineItems { get; set; }
        public string CreateBy_Name { get; set; }
        public string ApprovedBy_Name { get; set; }
        public string ProcessedBy_Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string Created { get; set; }
        public string StatusDrop { get; set; }
        public long PersonnelId { get; set; }
        public string CreatedBy { get; set; }
        public decimal TotalCost { get; set; }
        public string Flag { get; set; }
        public int CartsCreated { get; set; }
        public int CartItemsCreated { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Vendor_Name { get; set; }
        /// <summary>
        /// Retrieve ShoppingCart table records with specified primary key from the database.
        /// Include the data needed for notification purposes
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_ShoppingCart[] that contains the results</param>
        public void RetrieveForNotification(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_ShoppingCart> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ShoppingCart>(reader => { this.LoadFromDatabaseForNotification(reader); return this; });
                Database.StoredProcedure.usp_ShoppingCart_RetrieveForNotification.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void RetrieveAll(
                SqlConnection connection,
                SqlTransaction transaction,
                long callerUserInfoId,
                string callerUserName,
                ref b_ShoppingCart[] data
           )
        {
            Database.SqlClient.ProcessRow<b_ShoppingCart> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_ShoppingCart[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ShoppingCart>(reader => { b_ShoppingCart obj = new b_ShoppingCart(); obj.LoadFromDatabaseForNotification(reader); return obj; });
                //processRow = new Database.SqlClient.ProcessRow<b_ShoppingCart>(reader => { b_ShoppingCart obj = new b_ShoppingCart(); obj.LoadFromDB(reader); return obj; });
                results = Database.StoredProcedure.usp_ShoppingCart_RetrieveAllData.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_ShoppingCart[])results.ToArray(typeof(b_ShoppingCart));
                }
                else
                {
                    data = new b_ShoppingCart[0];
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
        public int LoadFromDatabaseForNotification(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                // Approved_Date column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                // Created By - Last Name + " " + First Name
                CreateBy_Name = reader.GetString(i++);
                // Approved By - Last Name + " " First Name 
                ApprovedBy_Name = reader.GetString(i++);
                // Processed By - Last Name + " " First Name 
                ProcessedBy_Name = reader.GetString(i++);
                // Number of line Items
                LineItems = reader.GetInt32(i++);
                // Total Cost  
                TotalCost = reader.GetDecimal(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["CreatedBy_Name"].ToString(); }
                catch { missing.Append("CreatedBy_Name "); }

                try { reader["ApprovedBy_Name"].ToString(); }
                catch { missing.Append("ApprovedBy_Name "); }

                try { reader["ProcessedBy_Name"].ToString(); }
                catch { missing.Append("ProcessedBy_Name "); }

                try { reader["LineItems"].ToString(); }
                catch { missing.Append("LineItems "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

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
        public int LoadFromDB(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {


                // CreateDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

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
            return i;
        }

        public void ShoppingCart_ConvertToPurchaseRequest(
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

                Database.StoredProcedure.ShoppingCart_ConvertToPurchaseRequest.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void UpdateByPKForeignKeysInDatabase(
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
                Database.StoredProcedure.usp_ShoppingCart_UpdateByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void ShoppingCart_ValidateByClientLookupId(SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_StoredProcValidationError> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();
            try
            {
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                results = Database.StoredProcedure.usp_ShoppingCart_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        public void RetrieveByShoppingCartId(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
     string callerUserName
       )
        {
            Database.SqlClient.ProcessRow<b_ShoppingCart> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ShoppingCart>(reader => { this.LoadFromDB(reader); return this; });
                Database.StoredProcedure.usp_ShoppingCart_RetrieveByShoppingCartId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public void RetrieveAllWorkbenchSearch(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_ShoppingCart> results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_ShoppingCart_WorkBenchRetrieveAll.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void ReviewWorkbenchSearch(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_ShoppingCart> results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_ShoppingCart_RetrieveReviewWorkBench.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void LoadFromDatabaseExtended(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ClientId = reader.GetInt64(i);
                }
                else
                {
                    ClientId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ShoppingCartId = reader.GetInt64(i);
                }
                else
                {
                    ShoppingCartId = 0;
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    Reason = reader.GetString(i);
                }
                else
                {
                    Reason = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    BuyerReview = reader.GetBoolean(i);
                }
                else
                {
                    BuyerReview = false;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CreatedBy = reader.GetString(i);
                }
                else
                {
                    CreatedBy = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0.00M;
                }
                i++;
            }

            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ShoppingCartId"].ToString(); }
                catch { missing.Append("ShoppingCartId "); }

                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason "); }

                try { reader["CreatedBy"].ToString(); }
                catch { missing.Append("CreatedBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }


                // SOM-1231
                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }
                // SOM-398
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public  void AutoGeneration(
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
                Database.StoredProcedure.usp_ShoppingCart_AutoGeneration.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void LoadCartForAutoGenNotification(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ShoppingCartId = reader.GetInt64(i++);
                StartDate = reader.GetDateTime(i++);
                CartItemsCreated = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ShoppingCartId"].ToString(); }
                catch { missing.Append("ShoppingCartId "); }

                try { reader["StartDate"].ToString(); }
                catch { missing.Append("StartDate "); }

                try { reader["CartItemsCreated"].ToString(); }
                catch { missing.Append("CartItemsCreated"); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
       
    }
}
