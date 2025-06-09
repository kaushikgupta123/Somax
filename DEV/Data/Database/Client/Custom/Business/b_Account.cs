/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date        JIRA Item Person          Description
* =========== ========= =============== =================================================
* 2014-Aug-02 SOM-264   Roger Lawton    Added InactiveFlad to LoadFromDatabaseForClientIdLookup
* 2016-Jan-07 SOM-882   Roger Lawton    RetrieveAllTemplatesWithClient method
*                                       Removed ClientId parameter
*                                       Added b_Account Parameter
* 2016-Aug-21 SOM-1049 Roger Lawton     Changed to use similar data retrieval functionality as
*                                       other pages 
**************************************************************************************************
*/

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    /// <summary>
    /// Business object thay>t stores a record from the TechSpecs table.
    /// </summar
    public partial class b_Account
    {

        #region Property
       public string SiteName { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public Int32 TotalCount { get; set; }
        #endregion

        public static b_Account ProcessRowForClientIdLookup(SqlDataReader reader)
        {
            // Create instance of object
            b_Account account = new b_Account();

            // Load the object from the database
            account.LoadFromDatabaseForClientIdLookup(reader);

            // Return result
            return account;
        }

        public void LoadFromDatabaseForClientIdLookup(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // AccountId column, bigint, not null
                AccountId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Account Name column, nvarchar(31), not null
                Name = reader.GetString(i++);

                // Inactive Flag
                InactiveFlag = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        /// <summary>
        /// Retrieve User table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void RetrieveClientLookupIdBySearchCriteriaFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Account> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Account> results = null;
            data = new List<b_Account>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Account_RetrieveClientLookupIdBySearchCriteria.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Account>();
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
                results = Database.StoredProcedure.usp_Account_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void RetrieveAllClientLookupId(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
           ref List<b_Account> data
        )
        {
            {

                SqlCommand command = null;
                string message = String.Empty;
                List<b_Account> results = null;
                data = new List<b_Account>();

                try
                {
                    // Create the command to use in calling the stored procedures
                    command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;

                    // Call the stored procedure to retrieve the data
                    results = Database.StoredProcedure.usp_Account_RetrieveAllClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                    if (results != null)
                    {
                        data = results;
                    }
                    else
                    {
                        data = new List<b_Account>();
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

        
           public void RetrieveForSearch(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref b_Account[] data)
           {
             Database.SqlClient.ProcessRow<b_Account> processRow = null;
             ArrayList results = null;
             SqlCommand command = null;
             string message = String.Empty;

             // Initialize the results
             data = new b_Account[0];

             try
             {
               // Create the command to use in calling the stored procedures
               command = new SqlCommand();
               command.Connection = connection;
               command.Transaction = transaction;

               // Call the stored procedure to retrieve the data
               processRow = new Database.SqlClient.ProcessRow<b_Account>(reader => { b_Account obj = new b_Account(); obj.LoadFromDatabase(reader); return obj; });
               results = Database.StoredProcedure.usp_Account_RetrieveForSearch.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

               // Extract the results
               if (null != results)
               {
                 data = (b_Account[])results.ToArray(typeof(b_Account));
               }
               else
               {
                 data = new b_Account[0];
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
        public void RetrieveForSearch_V2(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref b_Account[] data)
        {            
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Account[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                //processRow = new Database.SqlClient.ProcessRow<b_Account>(reader => { b_Account obj = new b_Account(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Account_RetrieveForSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_Account[])results.ToArray(typeof(b_Account));
                }
                else
                {
                    data = new b_Account[0];
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
                //processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public static b_Account ProcessRowForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_Account account = new b_Account();

            // Load the object from the database
            account.LoadFromDatabaseForSearch(reader);

            // Return result
            return account;
        }
        public void LoadFromDatabaseForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // AccountId column, bigint, not null
                AccountId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Name column, nvarchar(63), not null
                Name = reader.GetString(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // IsExternal column, bit, not null
                IsExternal = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["IsExternal"].ToString(); }
                catch { missing.Append("IsExternal "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void RetrieveForSearchforSuperUser_V2(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref b_Account[] data)
        {
            Database.SqlClient.ProcessRow<b_Account> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Account[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Account>(reader => { b_Account obj = new b_Account(); obj.LoadFromDatabaseforSuperUser_V2(reader); return obj; });
                results = Database.StoredProcedure.usp_Account_RetrieveForSearchForSuperUser_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_Account[])results.ToArray(typeof(b_Account));
                }
                else
                {
                    data = new b_Account[0];
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


        public int LoadFromDatabaseforSuperUser_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // AccountId column, bigint, not null
                AccountId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);               

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);                

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // Name column, nvarchar(63), not null
                Name = reader.GetString(i++);                

                // IsExternal column, bit, not null
                IsExternal = reader.GetBoolean(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
                // SiteName column,String.not null
                SiteName = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["ParentId"].ToString(); }
                catch { missing.Append("ParentId "); }

                try { reader["IsExternal"].ToString(); }
                catch { missing.Append("IsExternal "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }
                

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

        public void RetrieveAllTemplatesWithClient(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_Account> data
)
        {
            Database.SqlClient.ProcessRow <b_Account> processRow = null;
            List<b_Account> results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new List<b_Account>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Account>(reader => { b_Account obj = new b_Account(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Account_AllTemplatesWithClient.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Account>();
                }

                // Clear the results collection
                //if (null != results)
                //{
                //    results.Clear();
                //    results = null;
                //}
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
                Database.StoredProcedure.usp_Account_ChangeClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all Account table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_Account[] that contains the results</param>
        public void RetrieveAll_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_Account[] data
        )
        {
            Database.SqlClient.ProcessRow<b_Account> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Account[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Account>(reader => { b_Account obj = new b_Account(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Account_RetrieveAll_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SiteId);

                // Extract the results
                if (null != results)
                {
                    data = (b_Account[])results.ToArray(typeof(b_Account));
                }
                else
                {
                    data = new b_Account[0];
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
                results = Database.StoredProcedure.usp_Account_ValidateByInactivateorActivate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void UpdateByActivateorInactivate(
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
                Database.StoredProcedure.usp_Account_UpdateByActivateorInactivate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        //V2-379
        public void RetrieveAll_AccountByActiveState_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            bool IsActive,
            ref b_Account[] data
        )
        {
            Database.SqlClient.ProcessRow<b_Account> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Account[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Account>(reader => { b_Account obj = new b_Account(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Account_RetrieveByActiveState_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SiteId, IsActive);

                // Extract the results
                if (null != results)
                {
                    data = (b_Account[])results.ToArray(typeof(b_Account));
                }
                else
                {
                    data = new b_Account[0];
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


        public  void RetrieveByPKCustomFromDatabase(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
     string callerUserName
       )
        {
            Database.SqlClient.ProcessRow<b_Account> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Account>(reader => { this.LoadFoRetrieveByPKCustomrFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_Account_RetrieveByPKCustom_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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


        public int LoadFoRetrieveByPKCustomrFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // AccountId column, bigint, not null
                AccountId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // Name column, nvarchar(63), not null
                Name = reader.GetString(i++);

                // ParentId column, bigint, not null
                ParentId = reader.GetInt64(i++);

                // IsExternal column, bit, not null
                IsExternal = reader.GetBoolean(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
                // SiteName column, int, not null
                SiteName = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["ParentId"].ToString(); }
                catch { missing.Append("ParentId "); }

                try { reader["IsExternal"].ToString(); }
                catch { missing.Append("IsExternal "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }


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
        /// Load the current row in the input SqlDataReader into a b_Account object.
        /// This routine should be applied to the usp_Account_RetrieveLookupListBySearchCriteria_V2 stored procedure.
        /// </summary>
        public static object ProcessRowRetrieveLookupListBySearchCriteria(SqlDataReader reader)
        {
            // Create instance of object
            b_Account obj = new b_Account();

            // Load the object from the database
            obj.LoadFromDatabaseProcessRowRetrieveLookupListBySearchCriteria(reader);

            // Return result
            return (object)obj;
        }
       
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabaseProcessRowRetrieveLookupListBySearchCriteria(SqlDataReader reader)
        {
            int i = 0;
            try
            {


                // AccountId column, bigint, not null
                AccountId = reader.GetInt64(i++);


                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

             
                // Name column, nvarchar(63), not null
                Name = reader.GetString(i++);

             
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

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
            return i;
        }

        #region Retrieve Account Lookuplist ChunkSearch Mobile_V2
        public void RetrieveAccountLookuplistChunkSearchMobile_V2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_Account> results
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

                results = Database.StoredProcedure.usp_Account_RetrieveLookupListBySearchCriteriaMobile_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_Account ProcessRowForChunkSearchAccountLookupList_Mobile(SqlDataReader reader)
        {
            b_Account Account = new b_Account();

            Account.LoadFromDatabaseForAccountLookupListChunkSearchV2_Mobile(reader);
            return Account;
        }
        public int LoadFromDatabaseForAccountLookupListChunkSearchV2_Mobile(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // AccountId column, bigint, not null
                AccountId = reader.GetInt64(i++);

                // SiteId Id
                SiteId = reader.GetInt64(i++);

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

                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append(" Name "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("  TotalCount "); }


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
        #endregion end Mobile_V2
    }
}
