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
* 2014-Aug-29 SOM-304  Roger Lawton       Added Methods to load and process for lookup
* 2014-Sep-15 SOM-106  Roger Lawton       Added RetrieveForSearch
****************************************************************************************************
*/

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the TechSpecs table.
    /// </summary>
    public partial class b_Vendor
    {
        #region  Properties
        public int CustomQueryDisplayId { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Flag { get; set; }

        //V2-308
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public string SearchText { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public int totalCount { get; set; }

        //V2-375
        public string External { get; set; }

        #endregion

        public void RetrieveChunkSearch(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_Vendor> results
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

                results = Database.StoredProcedure.usp_Vendor_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public static b_Vendor ProcessRowForChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_Vendor vendor = new b_Vendor();
            vendor.LoadFromDatabaseForChunkSearch(reader);
            return vendor;
        }
        public int LoadFromDatabaseForChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                i = LoadFromDatabase(reader);
                totalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString(), ex);
            }

            return i;
        }

        public static b_Vendor ProcessRowForClientIdLookup(SqlDataReader reader)
        {
            // Create instance of object
            b_Vendor vendor = new b_Vendor();

            // Load the object from the database
            vendor.LoadFromDatabaseForClientIdLookup(reader);

            // Return result
            return vendor;
        }

        public void LoadFromDatabaseForClientIdLookup(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PartsId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

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

        // SOM-304
        public void LoadFromDatabaseForLookup(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PartsId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Name nvarchar(63), not null
                Name = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        // SOM-106
        public void LoadFromDatabaseForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // VendorId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // AddressCity column, nvarchar(63), not null
                AddressCity = reader.GetString(i++);

                // AddressState column, nvarchar(63), not null
                AddressState = reader.GetString(i++);

                // Name column, nvarchar(63), not null
                Name = reader.GetString(i++);

                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                // Terms column, nvarchar(15), not null
                Terms = reader.GetString(i++);

                // FOBCode column, nvarchar(15), not null
                FOBCode = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["AddressCity"].ToString(); }
                catch { missing.Append("AddressCity "); }

                try { reader["AddressState"].ToString(); }
                catch { missing.Append("AddressState "); }

                try { reader["FOBCode"].ToString(); }
                catch { missing.Append("FOBCode "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["Terms"].ToString(); }
                catch { missing.Append("Terms "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }
                throw new Exception(msg.ToString(), ex);
            }
        }

        // Added by RKL SOM-106
        public void RetrieveForSearch(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref b_Vendor[] data)
        {
            Database.SqlClient.ProcessRow<b_Vendor> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Vendor[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Vendor>(reader => { b_Vendor obj = new b_Vendor(); obj.LoadFromDatabaseForSearch(reader); return obj; });
                results = Database.StoredProcedure.usp_Vendor_RetrieveForSearch.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_Vendor[])results.ToArray(typeof(b_Vendor));
                }
                else
                {
                    data = new b_Vendor[0];
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

        public void RetrieveClientLookupIdBySearchCriteriaFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Vendor> results
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

                results = Database.StoredProcedure.usp_Vendor_RetrieveClientLookupIdBySearchCriteria.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        // SOM-304
        public void RetrieveForLookup(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref b_Vendor[] data)
        {
            Database.SqlClient.ProcessRow<b_Vendor> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Vendor[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Vendor>(reader => { b_Vendor obj = new b_Vendor(); obj.LoadFromDatabaseForLookup(reader); return obj; });
                results = Database.StoredProcedure.usp_Vendor_RetrieveForLookup.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_Vendor[])results.ToArray(typeof(b_Vendor));
                }
                else
                {
                    data = new b_Vendor[0];
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



        /******************************Added By Indusnet Technologies********************************/
        #region properties

        public string FilterText { get; set; }
        public int FilterStartIndex { get; set; }
        public int FilterEndIndex { get; set; }

        #endregion

        public static b_Vendor ProcessRowForRetrieveByFilterText(SqlDataReader reader)
        {
            // Create instance of object
            b_Vendor vendors = new b_Vendor();

            // Load the object from the database
            vendors.LoadFromDatabaseForRetrieveByFilterText(reader);

            // Return result
            return vendors;
        }

        public void LoadFromDatabaseForRetrieveByFilterText(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PartsId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Description column
                Name = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Description "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public static b_Vendor ProcessRowForFilterVendor(SqlDataReader reader)
        {
            // Create instance of object
            b_Vendor vendors = new b_Vendor();

            // Load the object from the database
            vendors.LoadFromDatabaseForRetrieveByFilterVendor(reader);

            // Return result
            return vendors;
        }

        public void LoadFromDatabaseForRetrieveByFilterVendor(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PartsId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Description column
                Name = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Description "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public void RetrieveVendorByFilterText(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         long ClientId,
         ref List<b_Vendor> results
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

                results = Database.StoredProcedure.usp_Vendor_RetrieveBYFilterText.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, FilterText, FilterStartIndex, FilterEndIndex);

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
                results = Database.StoredProcedure.usp_Vendor_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void ValidateByInactivateorActivate(
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


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Vendor_ValidateByInactivateorActivate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void RetrieveForLookupList(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Vendor obj, ref List<b_Vendor> results)
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

                results = Database.StoredProcedure.usp_Vendor_RetrieveForLookupList.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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

        public void ValidateByClientLookupIdSiteId(
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
                results = Database.StoredProcedure.usp_Vendor_ValidateByClientLookupId_SiteId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void ValidateClientLookupId(
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
                results = Database.StoredProcedure.usp_Vendor_ValidateClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        //-----SOM-788-----//

        public void LoadFromDatabaseCreateModifyDate(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                // Create By
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = "";
                }
                i++;

                // Create Date 
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    CreateDate = DateTime.Now;
                }
                i++;

                // Modify By
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";
                }
                i++;

                // Modify Date 
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    ModifyDate = DateTime.Now;
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

        }

        //public void RetrieveCreateModifyDate(
        //     SqlConnection connection,
        //     SqlTransaction transaction,
        //     long callerUserInfoId,
        //     string callerUserName
        // )
        //{
        //    Database.SqlClient.ProcessRow<b_Vendor> processRow = null;
        //    SqlCommand command = null;
        //    string message = String.Empty;

        //    try
        //    {
        //        // Create the command to use in calling the stored procedures
        //        command = new SqlCommand();
        //        command.Connection = connection;
        //        command.Transaction = transaction;

        //        // Call the stored procedure to retrieve the data
        //        processRow = new Database.SqlClient.ProcessRow<b_Vendor>(reader => { this.LoadFromDatabaseCreateModifyDate(reader); return this; });
        //        Database.StoredProcedure.usp_Vendor_RetrieveCreateModifyDate.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

        //    }
        //    finally
        //    {
        //        if (null != command)
        //        {
        //            command.Dispose();
        //            command = null;
        //        }
        //        processRow = null;
        //        message = String.Empty;
        //        callerUserInfoId = 0;
        //        callerUserName = String.Empty;
        //    }
        //}

        //--Call-from API SOM-918----------------------------------------------------------------
        public void RetrieveBySiteIdAndClientLookUpId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_Vendor> results
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

                results = Database.StoredProcedure.usp_Vendor_RetrieveBySiteIdAndClientLookUpId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        //public void RetrieveByVMId(
        //       SqlConnection connection,
        //       SqlTransaction transaction,
        //       long callerUserInfoId,
        // string callerUserName
        //   )
        //{
        //    Database.SqlClient.ProcessRow<b_Vendor> processRow = null;
        //    SqlCommand command = null;
        //    string message = String.Empty;

        //    try
        //    {
        //        // Create the command to use in calling the stored procedures
        //        command = new SqlCommand();
        //        command.Connection = connection;
        //        command.Transaction = transaction;

        //        // Call the stored procedure to retrieve the data
        //        processRow = new Database.SqlClient.ProcessRow<b_Vendor>(reader => { this.LoadFromDatabase(reader); return this; });
        //        Database.StoredProcedure.usp_Vendor_RetrieveByVMId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

        //    }
        //    finally
        //    {
        //        if (null != command)
        //        {
        //            command.Dispose();
        //            command = null;
        //        }
        //        processRow = null;
        //        message = String.Empty;
        //        callerUserInfoId = 0;
        //        callerUserName = String.Empty;
        //    }
        //}

        /// <summary>
        /// Retrieve all Vendor table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_Vendor[] that contains the results</param>
        public void RetrieveAll_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_Vendor[] data
        )
        {
            Database.SqlClient.ProcessRow<b_Vendor> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Vendor[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Vendor>(reader => { b_Vendor obj = new b_Vendor(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Vendor_RetrieveAll_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SiteId);

                // Extract the results
                if (null != results)
                {
                    data = (b_Vendor[])results.ToArray(typeof(b_Vendor));
                }
                else
                {
                    data = new b_Vendor[0];
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
                StoredProcedure.usp_Vendor_ChangeClientLookupId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void ValidateClientIdVendorAndMaster(
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


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Vendor_ValidateByClientLookupId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region Chunk Search Lookup list
        public void RetrieveVendorLookuplistChunkSearchV2(SqlConnection connection, SqlTransaction transaction,
                        long callerUserInfoId, string callerUserName, ref List<b_Vendor> results)
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

                results = Database.StoredProcedure.usp_Vendor_RetrieveChunkSearchLookupList_V2
                    .CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_Vendor ProcessRowForChunkSearchLookupList(SqlDataReader reader)
        {
            b_Vendor Vendor = new b_Vendor();

            Vendor.LoadFromDatabaseForLookupListChunkSearchV2(reader);
            return Vendor;
        }
        public int LoadFromDatabaseForLookupListChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // VendorId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                //ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                //Name
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                //AddressCity
                if (false == reader.IsDBNull(i))
                {
                    AddressCity = reader.GetString(i);
                }
                else
                {
                    AddressCity = "";
                }
                i++;
                //AddressState
                if (false == reader.IsDBNull(i))
                {
                    AddressState = reader.GetString(i);
                }
                else
                {
                    AddressState = "";
                }
                i++;

                //Type
                if (false == reader.IsDBNull(i))
                {
                    totalCount = reader.GetInt32(i);
                }
                else
                {
                    totalCount = 0;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["AddressCity"].ToString(); }
                catch { missing.Append("AddressCity "); }

                try { reader["AddressState"].ToString(); }
                catch { missing.Append("AddressState "); }

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

        #endregion

        #region V2
        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_Vendor object.
        /// This routine should be applied to the usp_Vendor_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Vendor_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_Vendor object</returns>
        public static object ProcessRowForRetrieveLookupListBySearchCriteria_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_Vendor obj = new b_Vendor();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveLookupListBySearchCriteria_V2(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_Vendor object.
        /// This routine should be applied to the usp_Vendor_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Vendor_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabaseForRetrieveLookupListBySearchCriteria_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                //// ClientId column, bigint, not null
                //ClientId = reader.GetInt64(i++);

                //// VendorId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                //// SiteId column, bigint, not null
                //SiteId = reader.GetInt64(i++);

                //// AreaId column, bigint, not null
                //AreaId = reader.GetInt64(i++);

                //// DepartmentId column, bigint, not null
                //DepartmentId = reader.GetInt64(i++);

                //// StoreroomId column, bigint, not null
                //StoreroomId = reader.GetInt64(i++);

                //// ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                //// Address1 column, nvarchar(63), not null
                //Address1 = reader.GetString(i++);

                //// Address2 column, nvarchar(63), not null
                //Address2 = reader.GetString(i++);

                //// Address3 column, nvarchar(63), not null
                //Address3 = reader.GetString(i++);

                //// AddressCity column, nvarchar(63), not null
                //AddressCity = reader.GetString(i++);

                //// AddressCountry column, nvarchar(63), not null
                //AddressCountry = reader.GetString(i++);

                //// AddressPostCode column, nvarchar(31), not null
                //AddressPostCode = reader.GetString(i++);

                //// AddressState column, nvarchar(63), not null
                //AddressState = reader.GetString(i++);

                //// CustomerAccount column, nvarchar(31), not null
                //CustomerAccount = reader.GetString(i++);

                //// EmailAddress column, nvarchar(63), not null
                //EmailAddress = reader.GetString(i++);

                //// FaxNumber column, nvarchar(31), not null
                //FaxNumber = reader.GetString(i++);

                //// FOBCode column, nvarchar(15), not null
                //FOBCode = reader.GetString(i++);

                //// InactiveFlag column, bit, not null
                //InactiveFlag = reader.GetBoolean(i++);

                //// Name column, nvarchar(63), not null
                Name = reader.GetString(i++);

                //// PhoneNumber column, nvarchar(31), not null
                //PhoneNumber = reader.GetString(i++);

                //// RemitAddress1 column, nvarchar(63), not null
                //RemitAddress1 = reader.GetString(i++);

                //// RemitAddress2 column, nvarchar(63), not null
                //RemitAddress2 = reader.GetString(i++);

                //// RemitAddress3 column, nvarchar(63), not null
                //RemitAddress3 = reader.GetString(i++);

                //// RemitCity column, nvarchar(63), not null
                //RemitCity = reader.GetString(i++);

                //// RemitCountry column, nvarchar(63), not null
                //RemitCountry = reader.GetString(i++);

                //// RemitPostCode column, nvarchar(31), not null
                //RemitPostCode = reader.GetString(i++);

                //// RemitState column, nvarchar(63), not null
                //RemitState = reader.GetString(i++);

                //// RemitUseBusiness column, bit, not null
                //RemitUseBusiness = reader.GetBoolean(i++);

                //// Terms column, nvarchar(15), not null
                //Terms = reader.GetString(i++);

                //// Type column, nvarchar(15), not null
                //Type = reader.GetString(i++);

                //// Website column, nvarchar(127), not null
                //Website = reader.GetString(i++);

                //// VendorMasterId column, bigint, not null
                //VendorMasterId = reader.GetInt64(i++);

                //// AutoEmailPO column, bit, not null
                //AutoEmailPO = reader.GetBoolean(i++);

                //// IsExternal column, bit, not null
                //IsExternal = reader.GetBoolean(i++);

                //// PunchoutIndicator column, bit, not null
                //PunchoutIndicator = reader.GetBoolean(i++);

                //// VendorDomain column, nvarchar(15), not null
                //VendorDomain = reader.GetString(i++);

                //// VendorIdentity column, nvarchar(31), not null
                //VendorIdentity = reader.GetString(i++);

                //// SharedSecret column, nvarchar(15), not null
                //SharedSecret = reader.GetString(i++);

                //// SenderDomain column, nvarchar(15), not null
                //SenderDomain = reader.GetString(i++);

                //// SenderIdentity column, nvarchar(31), not null
                //SenderIdentity = reader.GetString(i++);

                //// PunchoutURL column, nvarchar(MAX), not null
                //PunchoutURL = reader.GetString(i++);

                //// AutoSendPunchOutPO column, bit, not null
                //AutoSendPunchOutPO = reader.GetBoolean(i++);

                //// UpdateIndex column, int, not null
                //UpdateIndex = reader.GetInt32(i++);

                //Total Count
                totalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                //try { reader["ClientId"].ToString(); }
                //catch { missing.Append("ClientId "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                //try { reader["SiteId"].ToString(); }
                //catch { missing.Append("SiteId "); }

                //try { reader["AreaId"].ToString(); }
                //catch { missing.Append("AreaId "); }

                //try { reader["DepartmentId"].ToString(); }
                //catch { missing.Append("DepartmentId "); }

                //try { reader["StoreroomId"].ToString(); }
                //catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                //try { reader["Address1"].ToString(); }
                //catch { missing.Append("Address1 "); }

                //try { reader["Address2"].ToString(); }
                //catch { missing.Append("Address2 "); }

                //try { reader["Address3"].ToString(); }
                //catch { missing.Append("Address3 "); }

                //try { reader["AddressCity"].ToString(); }
                //catch { missing.Append("AddressCity "); }

                //try { reader["AddressCountry"].ToString(); }
                //catch { missing.Append("AddressCountry "); }

                //try { reader["AddressPostCode"].ToString(); }
                //catch { missing.Append("AddressPostCode "); }

                //try { reader["AddressState"].ToString(); }
                //catch { missing.Append("AddressState "); }

                //try { reader["CustomerAccount"].ToString(); }
                //catch { missing.Append("CustomerAccount "); }

                //try { reader["EmailAddress"].ToString(); }
                //catch { missing.Append("EmailAddress "); }

                //try { reader["FaxNumber"].ToString(); }
                //catch { missing.Append("FaxNumber "); }

                //try { reader["FOBCode"].ToString(); }
                //catch { missing.Append("FOBCode "); }

                //try { reader["InactiveFlag"].ToString(); }
                //catch { missing.Append("InactiveFlag "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                //try { reader["PhoneNumber"].ToString(); }
                //catch { missing.Append("PhoneNumber "); }

                //try { reader["RemitAddress1"].ToString(); }
                //catch { missing.Append("RemitAddress1 "); }

                //try { reader["RemitAddress2"].ToString(); }
                //catch { missing.Append("RemitAddress2 "); }

                //try { reader["RemitAddress3"].ToString(); }
                //catch { missing.Append("RemitAddress3 "); }

                //try { reader["RemitCity"].ToString(); }
                //catch { missing.Append("RemitCity "); }

                //try { reader["RemitCountry"].ToString(); }
                //catch { missing.Append("RemitCountry "); }

                //try { reader["RemitPostCode"].ToString(); }
                //catch { missing.Append("RemitPostCode "); }

                //try { reader["RemitState"].ToString(); }
                //catch { missing.Append("RemitState "); }

                //try { reader["RemitUseBusiness"].ToString(); }
                //catch { missing.Append("RemitUseBusiness "); }

                //try { reader["Terms"].ToString(); }
                //catch { missing.Append("Terms "); }

                //try { reader["Type"].ToString(); }
                //catch { missing.Append("Type "); }

                //try { reader["Website"].ToString(); }
                //catch { missing.Append("Website "); }

                //try { reader["VendorMasterId"].ToString(); }
                //catch { missing.Append("VendorMasterId "); }

                //try { reader["AutoEmailPO"].ToString(); }
                //catch { missing.Append("AutoEmailPO "); }

                //try { reader["IsExternal"].ToString(); }
                //catch { missing.Append("IsExternal "); }

                //try { reader["PunchoutIndicator"].ToString(); }
                //catch { missing.Append("PunchoutIndicator "); }

                //try { reader["VendorDomain"].ToString(); }
                //catch { missing.Append("VendorDomain "); }

                //try { reader["VendorIdentity"].ToString(); }
                //catch { missing.Append("VendorIdentity "); }

                //try { reader["SharedSecret"].ToString(); }
                //catch { missing.Append("SharedSecret "); }

                //try { reader["SenderDomain"].ToString(); }
                //catch { missing.Append("SenderDomain "); }

                //try { reader["SenderIdentity"].ToString(); }
                //catch { missing.Append("SenderIdentity "); }

                //try { reader["PunchoutURL"].ToString(); }
                //catch { missing.Append("PunchoutURL "); }

                //try { reader["AutoSendPunchOutPO"].ToString(); }
                //catch { missing.Append("AutoSendPunchOutPO "); }

                //try { reader["UpdateIndex"].ToString(); }
                //catch { missing.Append("UpdateIndex "); }


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
        #endregion
    }
}



/*****************************End Added By Indusnet Technologies*****************************/






