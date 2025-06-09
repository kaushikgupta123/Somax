/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Sep-18 SOM-106  Roger Lawton       Added 
****************************************************************************************************
*/

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the Part_Vendor_Xref table.
    /// </summary>
    public partial class b_Part_Vendor_Xref
    {
        public long SiteId { get; set; }
        public string Part_ClientLookupId { get; set; }
        public string Part_Description { get; set; }
        public string Vendor_ClientLookupId { get; set; }
        public string Vendor_Name { get; set; }
        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_Part_Vendor_Xref object.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_Part_Vendor_Xref object</returns>
        public static object ProcessRowExtended(SqlDataReader reader)
        {
            // Create instance of object
            b_Part_Vendor_Xref obj = new b_Part_Vendor_Xref();

            // Load the object from the database
            obj.LoadFromDatabaseExtended(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_Part_Vendor_Xref object.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public void LoadFromDatabaseExtended(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                Part_ClientLookupId = reader.GetString(i++);

                Part_Description = reader.GetString(i++);

                Vendor_ClientLookupId = reader.GetString(i++);

                Vendor_Name = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["Part_ClienLookupId"].ToString(); }
                catch { missing.Append("Part_ClienLookupId "); }

                try { reader["Part_Description"].ToString(); }
                catch { missing.Append("Part_Description "); }

                try { reader["Vendor_ClientLookupId"].ToString(); }
                catch { missing.Append("Vendor_ClientLookupId "); }


                try { reader["Vendor_Name"].ToString(); }
                catch { missing.Append("Vendor_Name "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void RetrieveListByPartId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_Part_Vendor_Xref> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_Part_Vendor_Xref> results = null;
            data = new List<b_Part_Vendor_Xref>();
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Part_Vendor_Xref_RetrieveByPartId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Part_Vendor_Xref>();
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

        public void RetrieveListByVendorId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_Part_Vendor_Xref> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_Part_Vendor_Xref> results = null;
            data = new List<b_Part_Vendor_Xref>();
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Part_Vendor_Xref_RetrieveByVendorId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Part_Vendor_Xref>();
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
        public void RetrieveByPKExtended(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_Part_Vendor_Xref> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Part_Vendor_Xref>(reader => { this.LoadFromDatabaseExtended(reader); return this; });
                Database.StoredProcedure.usp_Part_Vendor_Xref_RetrieveByPKExtended.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void Part_Vendor_Xref_ValidateByDuplicacy(
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
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Part_Vendor_Xref_ValidateByDuplicacy.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        #region V2-1119 Retrieve For ShoppingCartDataImport
        public void RetrieveForShoppingCartDataImport(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_Part_Vendor_Xref> part_Vendor_XrefList)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                //// Call the stored procedure to retrieve the data
                part_Vendor_XrefList = Database.StoredProcedure.usp_Part_Vendor_Xref_RetrieveForShoppingCartDataImport_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static object ProcessRowExtendedForShoppingCartDataImport(SqlDataReader reader)
        {
            // Create instance of object
            b_Part_Vendor_Xref obj = new b_Part_Vendor_Xref();

            // Load the object from the database
            obj.LoadFromDatabaseForShoppingCartDataImport(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseForShoppingCartDataImport(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // Part_Vendor_XrefId column, bigint, not null
                Part_Vendor_XrefId = reader.GetInt64(i++);

                // OrderUnit column, nvarchar(15), not null
                OrderUnit = reader.GetString(i++);

                // Part_ClientLookupId column, bigint, not null
                Part_ClientLookupId = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["Part_Vendor_XrefId"].ToString(); }
                catch { missing.Append("Part_Vendor_XrefId "); }

                try { reader["OrderUnit"].ToString(); }
                catch { missing.Append("OrderUnit "); }

                try { reader["Part_ClientLookupId"].ToString(); }
                catch { missing.Append("Part_ClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        #region V2-1119 Add or Update Part/Vendor Cross-Reference when Processing Shopping Cart Item
        public void Part_Vendor_Xref_Create_Update_Punchout_V2(
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
                Database.StoredProcedure.usp_Part_Vendor_Xref_Create_Update_Punchout_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        #endregion

        #endregion
    }
}
