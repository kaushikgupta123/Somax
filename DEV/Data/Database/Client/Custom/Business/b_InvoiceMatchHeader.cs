/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* Part.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
*============ ======== ================== ==========================================================
* 2015-Oct-29 SOM-838  Roger Lawton       Remove PONumber - Clean up a bit
*                                         Added UpdateByPKForeignKeys method
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
    /// <summary>
    /// Business object thay>t stores a record from the TechSpecs table.
    /// </summar
    public partial class b_InvoiceMatchHeader
    {

        /// <summary>
        /// Retrieve User table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        #region Property
        public string DateRange { get; set; }
        public string DateColumn { get; set; }
        public string VendorName { get; set; }
        public int PersonnelId { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string VendorClientLookupId { get; set; }
        public string POClientLookUpId { get; set; }
        public int CountLineItem { get; set; }
        public decimal ItemTotal { get; set; }
        public decimal Total { get; set; }
        public decimal Variance { get; set; }
        public DateTime CreateDate { get; set; }//V2-981
        public string CreateBy { get; set; }//V2-981

        public DateTime ModifyDate { get; set; }//V2-981

        public string ModifyBy { get; set; }//V2-981

        public string Responsible { get; set; }//V2-981
        public string ResponsibleWithClientLookupId { get; set; }//V2-981
        

        public string AuthorizedToPayBy { get; set; }//V2-981

        public string PaidBy { get; set; }//V2-981

        public int NumberOfLineItems { get; set; }
        public string Flag { get; set; }

        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public Int32 ChildCount { get; set; }
        public string CompleteATPStartDateVw { get; set; }//V2-373
        public string CompleteATPEndDateVw { get; set; }//V2-373
        public string CompletePStartDateVw { get; set; }//V2-373
        public string CompletePEndDateVw { get; set; }//V2-373
        public string CreateStartDateVw { get; set; }//V2-1061
        public string CreateEndDateVw { get; set; }//V2-1061
        public int InvoiceVariance { get; set; } //V2-1061
        #endregion
        public void RetrieveAllForSearch(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_InvoiceMatchHeader> results
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

                results = Database.StoredProcedure.usp_InvoiceMatchHeader_RetrieveAllForSearch.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void RetrieveChunkSearch(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_InvoiceMatchHeader> results
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

                results = Database.StoredProcedure.usp_InvoiceMatchHeader_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void ValidateByClientLookupId(
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
                results = Database.StoredProcedure.usp_InvoiceMatchHeader_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void ChangeClientLookupId(
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
                Database.StoredProcedure.usp_InvoiceMatchHeader_ChangeClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public void RetrieveByPKForeignKeysFromDatabase(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_InvoiceMatchHeader> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_InvoiceMatchHeader>(reader => { this.LoadFromDatabaseForInvoiceMatchHeaderRetriveByPk(reader); return this; });
                Database.StoredProcedure.usp_InvoiceMatchHeader_RetrieveByPKForeignKeys_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
            Database.SqlClient.ProcessRow<b_InvoiceMatchHeader> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_InvoiceMatchHeader>(reader => { this.LoadFromDatabaseForInvoiceMatchHeaderRetriveByPk(reader); return this; });
                Database.StoredProcedure.usp_InvoiceMatchHeader_RetrieveByPKForeignKeys_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void LoadFromDatabaseForInvoiceMatchHeaderSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }



        public static b_InvoiceMatchHeader ProcessRowForInvoiceMatchHeaderRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_InvoiceMatchHeader invoiceMatchHeader = new b_InvoiceMatchHeader();

            // Load the object from the database
            invoiceMatchHeader.LoadFromDatabaseForInvoiceMatchHeaderRetriveAllForSearch(reader);
            return invoiceMatchHeader;
        }

        public static b_InvoiceMatchHeader ProcessRowForRetriveChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_InvoiceMatchHeader invoiceMatchHeader = new b_InvoiceMatchHeader();

            // Load the object from the database
            invoiceMatchHeader.LoadFromDatabaseRetrieveChunkSearch(reader);
            return invoiceMatchHeader;
        }

        public int LoadFromDatabaseForInvoiceMatchHeaderRetriveByPk(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                // VendorClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    VendorClientLookupId = reader.GetString(i++);
                }
                else
                {
                    VendorClientLookupId = ""; i++;
                }
                //POClientLookUpId
                if (false == reader.IsDBNull(i))
                {
                    POClientLookUpId = reader.GetString(i++);
                }
                else
                {
                    POClientLookUpId = ""; i++;
                }
                //NoofLineItems
                if (false == reader.IsDBNull(i))
                {
                    CountLineItem = reader.GetInt32(i++);
                }
                else
                {
                    CountLineItem = 0; i++;
                }
                //VendorName
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i++);
                }
                else
                {
                    VendorName = ""; i++;
                }
                //ItemTotal
                if (false == reader.IsDBNull(i))
                {
                    ItemTotal = reader.GetDecimal(i++);
                }
                else
                {
                    ItemTotal = 0; i++;
                }
                //Total
                if (false == reader.IsDBNull(i))
                {
                    Total = reader.GetDecimal(i++);
                }
                else
                {
                    Total = 0; i++;
                }
                //Variance
                if (false == reader.IsDBNull(i))
                {
                    Variance = reader.GetDecimal(i++);
                }
                else
                {
                    Variance = 0; i++;
                }
                //AuthorizedToPayBy
                if (false == reader.IsDBNull(i))
                {
                    AuthorizedToPayBy = reader.GetString(i++);
                }
                else
                {
                    AuthorizedToPayBy = ""; i++;
                }
                //PaidBy
                if (false == reader.IsDBNull(i))
                {
                    PaidBy = reader.GetString(i++);
                }
                else
                {
                    PaidBy = ""; i++;
                }
                //CreateDate
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i++);
                }
                else
                {
                    CreateDate = DateTime.MinValue; i++;
                }
                //CreateBy
                if (false == reader.IsDBNull(i)) 
                {
                    CreateBy = reader.GetString(i++);
                }
                else
                {
                    CreateBy = ""; i++;
                }
                //ModifyDate
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i++);
                }
                else
                {
                    ModifyDate = DateTime.MinValue; i++;
                }
                //ModifyBy
                if (false == reader.IsDBNull(i)) 
                {
                    ModifyBy = reader.GetString(i++);
                }
                else
                {
                    ModifyBy = ""; i++;
                }
                //Responsible
                if (false == reader.IsDBNull(i)) 
                {
                    Responsible = reader.GetString(i++);
                }
                else
                {
                    Responsible = ""; i++;
                }
                //ResponsibleWithClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ResponsibleWithClientLookupId = reader.GetString(i++);
                }
                else
                {
                    ResponsibleWithClientLookupId = ""; i++;
                }
                i++;

            }
            catch (Exception ex)
            {
                StringBuilder missing = new StringBuilder();

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["POClientLookUpId"].ToString(); }
                catch { missing.Append("POClientLookUpId "); }

                try { reader["CountLineItem"].ToString(); }
                catch { missing.Append("CountLineItem "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["ItemTotal"].ToString(); }
                catch { missing.Append("ItemTotal "); }

                try { reader["Total"].ToString(); }
                catch { missing.Append("Total "); }

                try { reader["Variance"].ToString(); }
                catch { missing.Append("Variance "); }

                try { reader["AuthorizedtoPayBy"].ToString(); }
                catch { missing.Append("AuthorizedtoPayBy "); }

                try { reader["PaidBy"].ToString(); }
                catch { missing.Append("PaidBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["Responsible"].ToString(); }
                catch { missing.Append("Responsible "); }

                try { reader["ResponsibleWithClientLookupId"].ToString(); }
                catch { missing.Append("ResponsibleWithClientLookupId "); }
                //

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

        public int LoadFromDatabaseForInvoiceMatchHeaderRetriveAllForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);
                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);
                // SiteId column, bigint, not null
                InvoiceMatchHeaderId = reader.GetInt64(i++);
                // ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i++);
                }
                else
                {
                    ClientLookupId = ""; i++;
                }
                // Status
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i++);
                }
                else
                {
                    Status = ""; i++;
                }
                // VendorClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    VendorClientLookupId = reader.GetString(i++);
                }
                else
                {
                    VendorClientLookupId = ""; i++;
                }
                // VendorName
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i++);
                }
                else
                {
                    VendorName = ""; i++;
                }

                // DueDate
                if (false == reader.IsDBNull(i))
                {
                    ReceiptDate = reader.GetDateTime(i);
                }
                else
                {
                    ReceiptDate = DateTime.MinValue;
                }
                i++;
                // Purchase Order ID
                if (false == reader.IsDBNull(i))
                {
                    POClientLookUpId = reader.GetString(i);
                }
                else
                {
                    POClientLookUpId = string.Empty;
                }
                i++;
                // InvoiceDate    SOM-1534
                if (false == reader.IsDBNull(i))
                {
                    InvoiceDate = reader.GetDateTime(i);
                }
                else
                {
                    InvoiceDate = DateTime.MinValue;
                }
                i++;

            }
            catch (Exception ex)
            {
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["InvoiceMatchHeaderId"].ToString(); }
                catch { missing.Append("InvoiceMatchHeaderId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["ReceiptDate"].ToString(); }
                catch { missing.Append("ReceiptDate "); }

                try { reader["POClientLookUpId"].ToString(); }
                catch { missing.Append("POClientLookUpId "); }

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
        public int LoadFromDatabaseRetrieveChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);
                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);
                // SiteId column, bigint, not null
                InvoiceMatchHeaderId = reader.GetInt64(i++);
                // ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i++);
                }
                else
                {
                    ClientLookupId = ""; i++;
                }
                // Status
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i++);
                }
                else
                {
                    Status = ""; i++;
                }
                // VendorClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    VendorClientLookupId = reader.GetString(i++);
                }
                else
                {
                    VendorClientLookupId = ""; i++;
                }
                // VendorName
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i++);
                }
                else
                {
                    VendorName = ""; i++;
                }

                // DueDate
                if (false == reader.IsDBNull(i))
                {
                    ReceiptDate = reader.GetDateTime(i);
                }
                else
                {
                    ReceiptDate = DateTime.MinValue;
                }
                i++;
                // Purchase Order ID
                if (false == reader.IsDBNull(i))
                {
                    POClientLookUpId = reader.GetString(i);
                }
                else
                {
                    POClientLookUpId = string.Empty;
                }
                i++;
                // InvoiceDate    SOM-1534
                if (false == reader.IsDBNull(i))
                {
                    InvoiceDate = reader.GetDateTime(i);
                }
                else
                {
                    InvoiceDate = DateTime.MinValue;
                }
                i++;

                // ChildCount
                ChildCount = reader.GetInt32(i);
                i++;

                //TotalCount
                TotalCount = reader.GetInt32(i);
                i++;

            }
            catch (Exception ex)
            {
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["InvoiceMatchHeaderId"].ToString(); }
                catch { missing.Append("InvoiceMatchHeaderId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["ReceiptDate"].ToString(); }
                catch { missing.Append("ReceiptDate "); }

                try { reader["POClientLookUpId"].ToString(); }
                catch { missing.Append("POClientLookUpId "); }

                try { reader["InvoiceDate"].ToString(); }
                catch { missing.Append("InvoiceDate "); }

                try { reader["ChildCount"].ToString(); }
                catch { missing.Append("ChildCount "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

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
        /// Update the InvoiceMatchHeader table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void UpdateByForeignKeys(
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
                Database.StoredProcedure.usp_InvoiceMatchHeader_UpdateByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void DeleteInvoiceMatchHeaderAndInvoiceMatchItemsId(SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName)
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
                Database.StoredProcedure.usp_InvoiceMatchHeader_Delete_LineItems_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, SiteId, this.InvoiceMatchHeaderId);

            }
            finally
            {
                if (null != command)
                {
                    //command.Dispose();
                    //command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }

        //V2-1061

        public void ValidateVarianceCheck(
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
                results = Database.StoredProcedure.usp_InvoiceMatchHeader_ValidateVarianceCheck_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
    }
}
