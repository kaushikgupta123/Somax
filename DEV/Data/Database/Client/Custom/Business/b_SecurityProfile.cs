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
* Date         JIRA Item Person           Description
* ===========  ========= ================ ========================================================
* 2014-Oct-27  SOM-384   Roger Lawton     Added SecurityProfileRetrieveByName method
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
   
    public partial class b_SecurityProfile
    {
        #region Property
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public int ResultCount { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
        public string SearchText { get; set; }
        public Int32 TotalCount { get; set; }
        #endregion
        public void RetrieveAllProfiles(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
         string callerUserName,
            long AccessClientId,
          ref b_SecurityProfile[] data
      )
        {
            Database.SqlClient.ProcessRow<b_SecurityProfile> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_SecurityProfile[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new SqlClient.ProcessRow<b_SecurityProfile>(reader => { b_SecurityProfile obj = new b_SecurityProfile(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_SecurityProfile_RetrieveAllProfiles.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName,AccessClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_SecurityProfile[])results.ToArray(typeof(b_SecurityProfile));
                }
                else
                {
                    data = new b_SecurityProfile[0];
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
        

        public void RetrieveAllProfilesforEnterprise(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, long AccessClientId, ref List<b_SecurityProfile> data)
        {
            Database.SqlClient.ProcessRow<b_SecurityProfile> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                // Call the stored procedure to retrieve the data
                data = Database.StoredProcedure.usp_SecurityProfile_RetrieveAllProfilesForEnterPrise_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, AccessClientId,this);
                
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


        public void ValidateSecurityProfileName(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          long AccessClientId,
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
                results = Database.StoredProcedure.usp_SecurityProfile_ValidateProfileName.CallStoredProcedure(command, callerUserInfoId, callerUserName, AccessClientId, this);

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

        public void CreateClone(
              SqlConnection connection,
              SqlTransaction transaction,
              long callerUserInfoId,
              string callerUserName,
            long AccessClientId
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
                Database.StoredProcedure.usp_SecurityProfile_CreateClone.CallStoredProcedure(command, callerUserInfoId, callerUserName,AccessClientId,this);
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

        public void SecurityProfileRename(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,
            long AccessClientId
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
                Database.StoredProcedure.usp_SecurityProfile_Rename.CallStoredProcedure(command, callerUserInfoId, callerUserName,AccessClientId,this);
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

        public void SecurityProfileRetrieveByName( SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_SecurityProfile> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SecurityProfile>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_SecurityProfile_RetrieveByName.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void SecurityProfileRetrieveByPackageLevel(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_SecurityProfile> data, ref int ResCount)
        {
            Database.SqlClient.ProcessRow<b_SecurityProfile> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
               data= Database.StoredProcedure.usp_SecurityProfile_RetrieveByPackageLevel_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);
               ResCount = this.ResultCount;
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

        public static b_SecurityProfile LoadFromDatabaseRetrieveByPackageLevel_V2(SqlDataReader reader)
        {
            b_SecurityProfile obj = new b_SecurityProfile();
            int i = obj.LoadFromDatabase(reader);

            try
            {
                return obj;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        #region for search grid V2-500
        public void RetrieveCustomSecurityProfileChunkSearchV2(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
        string callerUserName,
         ref b_SecurityProfile[] data
     )
        {
            Database.SqlClient.ProcessRow<b_SecurityProfile> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_SecurityProfile[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new SqlClient.ProcessRow<b_SecurityProfile>(reader => { b_SecurityProfile obj = new b_SecurityProfile(); obj.LoadFromDatabaseForChunkSearch(reader); return obj; });
                results = Database.StoredProcedure.usp_SecurityProfile_CustomRetrieveChunkSearch_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_SecurityProfile[])results.ToArray(typeof(b_SecurityProfile));
                }
                else
                {
                    data = new b_SecurityProfile[0];
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

        public void SecurityItemAddInDatabase(
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
                StoredProcedure.usp_SecurityItem_UpdateforSecurityProfile_v2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                //StoredProcedure.usp_securityitem_u
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
        public int LoadFromDatabaseForChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SecurityProfileId column, bigint, not null
                SecurityProfileId = reader.GetInt64(i++);

                // Name column, nvarchar(63), not null
                Name = reader.GetString(i++);

                // Description column, nvarchar(255), not null
                Description = reader.GetString(i++);

                // SortOrder column, int, not null
                SortOrder = reader.GetInt32(i++);

                // Protected column, bit, not null
                Protected = reader.GetBoolean(i++);

                // UserType column, nvarchar(15), not null
                UserType = reader.GetString(i++);

                // ProductGrouping column, int, not null
                ProductGrouping = reader.GetInt32(i++);

                // PackageLevel column, nvarchar(15), not null
                PackageLevel = reader.GetString(i++);

                // CMMSUser column, bit, not null
                CMMSUser = reader.GetBoolean(i++);

                // SanitationUser column, bit, not null
                SanitationUser = reader.GetBoolean(i++);

                // APMUser column, bit, not null
                APMUser = reader.GetBoolean(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
                // UpdateIndex column, int, not null
                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SecurityProfileId"].ToString(); }
                catch { missing.Append("SecurityProfileId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["SortOrder"].ToString(); }
                catch { missing.Append("SortOrder "); }

                try { reader["Protected"].ToString(); }
                catch { missing.Append("Protected "); }

                try { reader["UserType"].ToString(); }
                catch { missing.Append("UserType "); }

                try { reader["ProductGrouping"].ToString(); }
                catch { missing.Append("ProductGrouping "); }

                try { reader["PackageLevel"].ToString(); }
                catch { missing.Append("PackageLevel "); }

                try { reader["CMMSUser"].ToString(); }
                catch { missing.Append("CMMSUser "); }

                try { reader["SanitationUser"].ToString(); }
                catch { missing.Append("SanitationUser "); }

                try { reader["APMUser"].ToString(); }
                catch { missing.Append("APMUser "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

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

        #region Validate Profile Name at the time of Add and Update
        public void ValidateName(
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
                results = Database.StoredProcedure.usp_SecurityProfile_ValidateName_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        #endregion

        #region  V2-802
        public void RetrieveSecurityProfileByClientIdV2(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
        string callerUserName,
         ref b_SecurityProfile[] data
     )
        {
            Database.SqlClient.ProcessRow<b_SecurityProfile> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_SecurityProfile[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new SqlClient.ProcessRow<b_SecurityProfile>(reader => { b_SecurityProfile obj = new b_SecurityProfile(); obj.LoadFromDatabaseForSecurityProfileId(reader); return obj; });
                results = Database.StoredProcedure.usp_SecurityProfile_RetrieveByClientId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_SecurityProfile[])results.ToArray(typeof(b_SecurityProfile));
                }
                else
                {
                    data = new b_SecurityProfile[0];
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

        public int LoadFromDatabaseForSecurityProfileId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // SecurityProfileId column, bigint, not null
                SecurityProfileId = reader.GetInt64(i++);

                // Name column, nvarchar(63), not null
                Name = reader.GetString(i++);

                // Description column, nvarchar(255), not null
                Description = reader.GetString(i++);      
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();       
                try { reader["SecurityProfileId"].ToString(); }
                catch { missing.Append("SecurityProfileId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

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
