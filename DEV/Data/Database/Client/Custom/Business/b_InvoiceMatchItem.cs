/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 2015-Oct-21  SOM-822   Roger Lawton      Added DeleteItem Method
* 2016-Nov-18  SOM-1163  Roger Lawton      Updated Methods
**************************************************************************************************
*/

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    /// <summary>
    /// Business object thay>t stores a record from the TechSpecs table.
    /// </summar
    public partial class b_InvoiceMatchItem
    {
        /// <summary>
        /// Retrieve User table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>

        public string DateRange { get; set; }
        public string DateColumn { get; set; }
        public string VendorName { get; set; }
        public int PersonnelId { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string VendorClientLookupId { get; set; }
        public string POClientLookUpId { get; set; }
        //public decimal OrderQuantity { get; set; }
        public Int64 LineNumber { get; set; }
        public string Units { get; set; }
        public decimal TotalCost { get; set; }
        public string PurchaseOrder { get; set; }
        public string Account { get; set; }
        public int SiteId { get; set; }
        public Int64 PurchaseOrderId { get; set; }

        public static b_InvoiceMatchItem ProcessRowForInvoiceMatchItemRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_InvoiceMatchItem InvoiceMatchItem = new b_InvoiceMatchItem();
            InvoiceMatchItem.LoadFromDatabaseExtended(reader);          
            return InvoiceMatchItem;
        }

        public void RetrieveByPKForeignKeysFromDatabaseList(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,

        ref List<b_InvoiceMatchItem> results
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

                results = Database.StoredProcedure.usp_InvoiceMatchItem_RetrieveByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public void RetrieveByPKForeignKeysFromDatabase(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName
         )
        {
            Database.SqlClient.ProcessRow<b_InvoiceMatchItem> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_InvoiceMatchItem>(reader => { this.LoadFromDatabaseExtended(reader); return this; });
                Database.StoredProcedure.usp_InvoiceMatchItem_RetrieveByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId,callerUserName,this);

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

        public void RetrieveByPrimaryKeyFromDatabase(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName
         )
        {
            Database.SqlClient.ProcessRow<b_InvoiceMatchItem> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_InvoiceMatchItem>(reader => { this.LoadFromDatabaseExtended(reader); return this; });
                Database.StoredProcedure.usp_InvoiceMatchItem_RetrieveByPrimaryKey.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);
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
        /// Delete the InvoiceMatchItem table record represented by this object from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void DeleteItem(
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
            Database.StoredProcedure.usp_InvoiceMatchItem_DeleteItem.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void Validate(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<b_StoredProcValidationError> data
      )
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            //data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_InvoiceMatchItem_Validate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public int LoadFromDatabaseExtended(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                // LineNumber
                LineNumber = reader.GetInt64(i++);

                // TotalCost
                TotalCost = reader.GetDecimal(i++);

                // PurchaseOrder
                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrder = reader.GetString(i++);
                }
                else
                {
                    PurchaseOrder = ""; i++;
                }

                // PurchaseOrderId
                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrderId = reader.GetInt64(i++);
                }
                else
                {
                    PurchaseOrderId = 0; i++;
                }

                // Account
                if (false == reader.IsDBNull(i))
                {
                    Account = reader.GetString(i++);
                }
                else
                {
                    Account = ""; i++;
                }

                i++;
            }
            catch (Exception ex)
            {
                StringBuilder missing = new StringBuilder();

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber"); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost"); }

                try { reader["PurchaseOrder"].ToString(); }
                catch { missing.Append("PurchaseOrder"); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId"); }

                try { reader["Account"].ToString(); }
                catch { missing.Append("Account"); }

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
/*
        public int LoadFromDatabaseForInvoiceMatchItemRetriveByPk(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // LineNumber
                LineNumber = reader.GetInt64(i++);

                //InvoiceMatchItemId
                
                InvoiceMatchItemId = reader.GetInt64(i++);
                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i++);
                }
                else
                {
                    Description = ""; i++;
                }

                // OrderQuantity
                OrderQuantity = reader.GetDecimal(i++);
                
                // Units
                if (false == reader.IsDBNull(i))
                {
                    Units = reader.GetString(i++);
                }
                else
                {
                    Units = ""; i++;
                }

                // UnitCost
                UnitCost = reader.GetDecimal(i++);

                // TotalCost
                TotalCost = reader.GetDecimal(i++);

                // PurchaseOrder
                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrder = reader.GetString(i++);
                }
                else
                {
                    PurchaseOrder = ""; i++;
                }

                // PurchaseOrder
                if (false == reader.IsDBNull(i))
                {
                    Account = reader.GetString(i++);
                }
                else
                {
                    Account = ""; i++;
                }

                // PurchaseOrder
                if (false == reader.IsDBNull(i))
                {
                    AccountId = reader.GetInt64(i++);
                }
                else
                {
                    AccountId = 0; i++;
                }

               
                
                i++;
            }
            catch (Exception ex)
            {
                StringBuilder missing = new StringBuilder();

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber"); }

                try { reader["InvoiceMatchItemId"].ToString(); }
                catch { missing.Append("InvoiceMatchItemId"); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description"); }

                try { reader["OrderQuantity"].ToString(); }
                catch { missing.Append("OrderQuantity"); }

                try { reader["Units"].ToString(); }
                catch { missing.Append("Units"); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost"); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost"); }

                try { reader["PurchaseOrder"].ToString(); }
                catch { missing.Append("PurchaseOrder"); }

                try { reader["Account"].ToString(); }
                catch { missing.Append("Account"); }

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId"); }

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

        public int LoadFromDatabaseForInvoiceMatchItemRetriveForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // LineNumber
                LineNumber = reader.GetInt64(i++);

                //InvoiceMatchItemId

                InvoiceMatchItemId = reader.GetInt64(i++);
                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i++);
                }
                else
                {
                    Description = ""; i++;
                }

                // OrderQuantity
                OrderQuantity = reader.GetDecimal(i++);

                // Units
                if (false == reader.IsDBNull(i))
                {
                    Units = reader.GetString(i++);
                }
                else
                {
                    Units = ""; i++;
                }

                // UnitCost
                UnitCost = reader.GetDecimal(i++);

                // TotalCost
                TotalCost = reader.GetDecimal(i++);

                // PurchaseOrder
                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrder = reader.GetString(i++);
                }
                else
                {
                    PurchaseOrder = ""; i++;
                }

                // PurchaseOrder
                if (false == reader.IsDBNull(i))
                {
                    Account = reader.GetString(i++);
                }
                else
                {
                    Account = ""; i++;
                }

                i++;
            }
            catch (Exception ex)
            {
                StringBuilder missing = new StringBuilder();

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber"); }

                try { reader["InvoiceMatchItemId"].ToString(); }
                catch { missing.Append("InvoiceMatchItemId"); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description"); }

                try { reader["OrderQuantity"].ToString(); }
                catch { missing.Append("OrderQuantity"); }

                try { reader["Units"].ToString(); }
                catch { missing.Append("Units"); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost"); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost"); }

                try { reader["PurchaseOrder"].ToString(); }
                catch { missing.Append("PurchaseOrder"); }

                try { reader["Account"].ToString(); }
                catch { missing.Append("Account"); }

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
 */
    }

}
