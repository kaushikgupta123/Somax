/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person            Description
* =========== ======== ================== ========================================================
* 2014-Aug-11 SOM-282  Roger Lawton       Modify the validation logic 
* 2014-Oct-21 SOM-384  Roger Lawton       Additional Parameters in ValidateNewUserAdd and 
* 2015-Nov-06 SOM-851  Roger Lawton       Support Multi-Site - Get list of authorized sites for a 
*                                         particular user (AuthorizedUser) 
***************************************************************************************************
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;


namespace Database.Business
{
    public partial class b_Site
    {
        #region properties
        public long AuthorizedUser { get; set; }
        #region V2-419 Enterprise User Management - Add/Remove Sites
        public int DefaultBuyerCount { get; set; }
        #endregion


        #region 435 Admin
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public Int32 OffSetVal { get; set; }
        public Int32 NextRow { get; set; }
        public string SearchText { get; set; }
        public List<b_Site> listOfSite { get; set; }
        public Int32 TotalCount { get; set; }
        public Int64 CustomClientId { get; set; }
        public string ClientName { get; set; }

        public string LocalizationName { get; set; }
        public string TimeZoneName { get; set; }
        public bool ClientSiteControl { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        #endregion
        #endregion

        public static b_Site ProcessRowForSiteRetriveFromAdmin(SqlDataReader reader)
        {
            // Create instance of object
            b_Site site = new b_Site();

            // Load the object from the database
            site.LoadFromDatabaseForSiteRetriveFromAdmin(reader);

            // Return result
            return site;
        }

        public void LoadFromDatabaseForSiteRetriveFromAdmin(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);

                SiteId = reader.GetInt64(i++);

                Name = reader.GetString(i++);

                Description = reader.GetString(i++);

                AddressState = reader.GetString(i++);

                AddressCity = reader.GetString(i++);

                AddressCountry = reader.GetString(i++);

                Localization = reader.GetString(i++);

                TimeZone = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["AddressState"].ToString(); }
                catch { missing.Append("AddressState "); }

                try { reader["AddressCity"].ToString(); }
                catch { missing.Append("AddressCity "); }

                try { reader["AddressCountry"].ToString(); }
                catch { missing.Append("AddressCountry "); }

                try { reader["Localization"].ToString(); }
                catch { missing.Append("Localization "); }

                try { reader["TimeZone"].ToString(); }
                catch { missing.Append("TimeZone "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public void RetrieveBySearchFromAdmin(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
   string callerUserName,
         ref List<b_Site> results
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
                results = Database.StoredProcedure.usp_Site_RetrieveBySearchFromAdmin.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        // SOM-851
        public void RetrieveAuthorizedForUser(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Site obj, ref List<b_Site> results)
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

                results = Database.StoredProcedure.usp_Site_RetrieveAuthorizedForUser.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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
        public void ValidateNewUserAddFromDatabase(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          bool IsSuperUser,
          string UserType,
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
                results = Database.StoredProcedure.usp_Site_ValidateNewUserAdd.CallStoredProcedure(command, callerUserInfoId, callerUserName, this,
                  IsSuperUser,UserType);

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

        public void ValidateUserUpdateFromDatabase(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        string UserType,
        bool UserIsActive,
        bool IsSuperUser,
        string CurrentUserType,
        bool CurrentUserIsActive,
        bool CurrentIsSuperUser,
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
                results = Database.StoredProcedure.usp_Site_ValidateUserUpdate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this
                  ,UserType, UserIsActive, IsSuperUser,CurrentUserType, CurrentUserIsActive, CurrentIsSuperUser);

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


        //-----------------SOM-899-------------------------------------------------------------
        public void ValidateByProcessSystemId(
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
                results = Database.StoredProcedure.usp_Site_ValidateByProcessSystemId.CallStoredProcedure(command, callerUserInfoId, callerUserName,this);

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



        public void RetrieveAssetGroupName(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
   string callerUserName,
         ref b_Site[] data
     )
        {
            Database.SqlClient.ProcessRow<b_Site> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Site[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Site>(reader => { b_Site obj = new b_Site(); obj.LoadFromDatabaseForRetrieveAssetGroupName(reader); return obj; });
                results = Database.StoredProcedure.usp_Site_RetrieveAssetGroupName_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SiteId);

                // Extract the results
                if (null != results)
                {
                    data = (b_Site[])results.ToArray(typeof(b_Site));
                }
                else
                {
                    data = new b_Site[0];
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
                //results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void LoadFromDatabaseForRetrieveAssetGroupName(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1Name = reader.GetString(i);
                }
                else
                {
                    AssetGroup1Name = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2Name = reader.GetString(i);
                }
                else
                {
                    AssetGroup2Name = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup3Name = reader.GetString(i);
                }
                else
                {
                    AssetGroup3Name = "";
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["AssetGroup1Name"].ToString(); }
                catch { missing.Append("AssetGroup1Name "); }

                try { reader["AssetGroup2Name"].ToString(); }
                catch { missing.Append("AssetGroup2Name "); }

                try { reader["AssetGroup3Name"].ToString(); }
                catch { missing.Append("AssetGroup3Name "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        #region V2-419 Enterprise User Management - Add/Remove Sites
        public void RetrieveAllAssignedSiteByUser(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Site obj, ref List<b_Site> results)
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

                results = Database.StoredProcedure.usp_Site_RetrieveAllAssignedByUser_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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

        public void RetriveDefaultBuyer(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_Site> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_Site> results = null;
            data = new List<b_Site>();
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Site_RetrieveDefaultBuyerCount_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Site>();
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

        public static object ProcessExdRow(SqlDataReader reader)
        {
            // Create instance of object
            b_Site obj = new b_Site();

            // Load the object from the database
            obj.LoadExdFromDatabase(reader);

            // Return result
            return (object)obj;
        }

        public int LoadExdFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // DefaultBuyerCount column, int, not null
                DefaultBuyerCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["DefaultBuyerCount"].ToString(); }
                catch { missing.Append("DefaultBuyerCount "); }

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

        #region V2-435 Admin Site

        public static b_Site ProcessRetrieveForSiteChunkV2(SqlDataReader reader)
        {
            b_Site Site = new b_Site();

            Site.LoadFromDatabaseForSiteChunkV2ChunkSearchV2(reader);
            return Site;
        }


        public int LoadFromDatabaseForSiteChunkV2ChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // SiteId Id
                SiteId = reader.GetInt64(i++);


                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AddressCity = reader.GetString(i);
                }
                else
                {
                    AddressCity = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AddressState = reader.GetString(i);
                }
                else
                {
                    AddressState = "";
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    UpdateIndex = reader.GetInt32(i++);
                }

                TotalCount = reader.GetInt32(i);
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["AddressCity"].ToString(); }
                catch { missing.Append("AddressCity "); }

                try { reader["AddressState"].ToString(); }
                catch { missing.Append(" AddressState "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append(" UpdateIndex "); }


                try { reader["TotalCount"].ToString(); }
                catch { missing.Append(" TotalCount "); }
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

        public void RetrieveSiteChunkSearchV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Site results
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

                results = Database.StoredProcedure.usp_Admin_SiteRetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        /// <summary>
        /// Retrieve all Site table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_Account[] that contains the results</param>
        public void RetrieveAllSiteBySiteId_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_Site data
        )
        {
            Database.SqlClient.ProcessRow<b_Site> processRow = null;
            List<b_Site> results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Site();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Site>(reader => { b_Site obj = new b_Site(); obj.LoadFromDatabaseCustom_V2(reader); return obj; });
                results = Database.StoredProcedure.usp_Admin_SiteRetrieveBySiteid_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = results[0];
                }
                else
                {
                    data = new b_Site();
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

        public void LoadFromDatabaseCustom_V2(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ClientName = reader.GetString(i);
                }
                else
                {
                    ClientName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    LocalizationName = reader.GetString(i);
                }
                else
                {
                    LocalizationName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    TimeZoneName = reader.GetString(i);
                }
                else
                {
                    TimeZoneName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ClientSiteControl = reader.GetBoolean(i);
                }
                else
                {
                    ClientSiteControl = false;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientName"].ToString(); }
                catch { missing.Append("ClientName "); }

                try { reader["LocalizationName"].ToString(); }
                catch { missing.Append("LocalizationName "); }

                try { reader["TimeZoneName"].ToString(); }
                catch { missing.Append("TimeZoneName "); }

                try { reader["ClientSiteControl"].ToString(); }
                catch { missing.Append("ClientSiteControl "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public void RetrieveCreateModifyDate(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName
     )
        {
            SqlClient.ProcessRow<b_Site> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new SqlClient.ProcessRow<b_Site>(reader => { this.LoadFromDatabaseCreateModifyDate(reader); return this; });
                StoredProcedure.usp_Admin_SiteRetrieveCreateModifyDate_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        #endregion

        #region V2-806
        //public void RetrieveByClientIdForLookupList(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Site obj, ref List<b_Site> results)
        //{
        //    SqlCommand command = null;
        //    string message = String.Empty;

        //    try
        //    {
        //        // Create the command to use in calling the stored procedures
        //        command = new SqlCommand();
        //        command.Connection = connection;
        //        command.Transaction = transaction;

        //        // Call the stored procedure to retrieve the data

        //        results = Database.StoredProcedure.usp_Site_RetrieveByClientIdForLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

        //    }
        //    finally
        //    {
        //        if (null != command)
        //        {
        //            command.Dispose();
        //            command = null;
        //        }

        //        message = String.Empty;
        //        callerUserInfoId = 0;
        //        callerUserName = String.Empty;
        //    }
        //}
        public void RetrieveByClientIdForLookupList(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
   string callerUserName,
         ref List<b_Site> results
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
                results = Database.StoredProcedure.usp_Site_RetrieveByClientIdForLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_Site ProcessRowForLookupListSite(SqlDataReader reader)
        {
            // Create instance of object
            b_Site obj = new b_Site();

            // Load the object from the database
            obj.LoadFromDatabaseForLookupListSite(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForLookupListSite(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // Name column, nvarchar(31), not null
                Name = reader.GetString(i++);
            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

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

        #endregion

        #region V2-964
        public void RetrieveForAllActiveLookupListV2(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Site obj, ref List<b_Site> results)
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

                results = Database.StoredProcedure.usp_Site_RetrieveAllActiveSiteForLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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
        public static b_Site ProcessRowForAllActiveLookupList(SqlDataReader reader)
        {
            // Create instance of object
            b_Site obj = new b_Site();

            // Load the object from the database
            obj.LoadFromDatabaseForAllActiveLookupList(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForAllActiveLookupList(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                SiteId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;
            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

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
        public void RetrieveSiteChildGridForAdmin(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Site results
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

                results = Database.StoredProcedure.usp_Site_RetrieveForChildGridByClientIdFromAdmin_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_Site ProcessRetrieveChildGridForClient(SqlDataReader reader)
        {
            b_Site Site = new b_Site();

            Site.LoadFromDatabaseForChildGridForClient(reader);
            return Site;
        }


        public int LoadFromDatabaseForChildGridForClient(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);

                SiteId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    APM = reader.GetBoolean(i);
                }
                else
                {
                    APM = false;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CMMS = reader.GetBoolean(i);
                }
                else
                {
                    CMMS = false;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Sanitation = reader.GetBoolean(i);
                }
                else
                {
                    Sanitation = false;
                }
                i++;
                UpdateIndex = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["APM"].ToString(); }
                catch { missing.Append(" APM "); }

                try { reader["CMMS"].ToString(); }
                catch { missing.Append(" CMMS "); }

                try { reader["Sanitation"].ToString(); }
                catch { missing.Append(" Sanitation "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append(" UpdateIndex "); }

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
        public void InsertIntoDatabaseFromClientBySomaxAdminV2(
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
                Database.StoredProcedure.usp_Site_CreateFromClientBYSomaxAdmin_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void RetrieveSiteByClientIdSiteId_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_Site> processRow = null;
            //List<b_Site> results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            //data = new b_Site();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                //processRow = new Database.SqlClient.ProcessRow<b_Site>(reader => { b_Site obj = new b_Site(); obj.LoadFromDatabaseForClientIdSiteId_V2(reader); return obj; });
                //results = Database.StoredProcedure.usp_Site_RetrieveByClientIdSiteId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                processRow = new Database.SqlClient.ProcessRow<b_Site>(reader => { this.LoadFromDatabaseForClientIdSiteId_V2(reader); return this; });
                Database.StoredProcedure.usp_Site_RetrieveByClientIdSiteId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                //if (null != results)
                //{
                //    data = results[0];
                //}
                //else
                //{
                //    data = new b_Site();
                //}

                //// Clear the results collection
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
                //results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void LoadFromDatabaseForClientIdSiteId_V2(SqlDataReader reader)
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
                    SiteId = reader.GetInt64(i);
                }
                else
                {
                    SiteId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    TimeZone = reader.GetString(i);
                }
                else
                {
                    TimeZone = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    APM = reader.GetBoolean(i);
                }
                else
                {
                    APM = false;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CMMS = reader.GetBoolean(i);
                }
                else
                {
                    CMMS = false;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Sanitation = reader.GetBoolean(i);
                }
                else
                {
                    Sanitation = false;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Production = reader.GetBoolean(i);
                }
                else
                {
                    Production = false;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
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
                    UsePunchOut = reader.GetBoolean(i);
                }
                else
                {
                    UsePunchOut = false;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AppUsers = reader.GetInt32(i);
                }
                else
                {
                    AppUsers = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    MaxAppUsers = reader.GetInt32(i);
                }
                else
                {
                    MaxAppUsers = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    WorkRequestUsers = reader.GetInt32(i);
                }
                else
                {
                    WorkRequestUsers = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    MaxWorkRequestUsers = reader.GetInt32(i);
                }
                else
                {
                    MaxWorkRequestUsers = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    SanitationUsers = reader.GetInt32(i);
                }
                else
                {
                    SanitationUsers = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    MaxSanitationUsers = reader.GetInt32(i);
                }
                else
                {
                    MaxSanitationUsers = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ProdAppUsers = reader.GetInt32(i);
                }
                else
                {
                    ProdAppUsers = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    MaxProdAppUsers = reader.GetInt32(i);
                }
                else
                {
                    MaxProdAppUsers = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    UpdateIndex = reader.GetInt32(i);
                }
                else
                {
                    UpdateIndex = 0;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId"); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId"); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name"); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description"); }

                try { reader["TimeZone"].ToString(); }
                catch { missing.Append("TimeZone"); }

                try { reader["APM"].ToString(); }
                catch { missing.Append("APM"); }

                try { reader["CMMS"].ToString(); }
                catch { missing.Append("CMMS"); }

                try { reader["Production"].ToString(); }
                catch { missing.Append("Production"); }

                try { reader["Sanitation"].ToString(); }
                catch { missing.Append("Sanitation"); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status"); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate"); }

                try { reader["UsePunchOut"].ToString(); }
                catch { missing.Append("UsePunchOut"); }

                try { reader["AppUsers"].ToString(); }
                catch { missing.Append("AppUsers "); }

                try { reader["MaxAppUsers"].ToString(); }
                catch { missing.Append("MaxAppUsers "); }

                try { reader["WorkRequestUsers"].ToString(); }
                catch { missing.Append("WorkRequestUsers"); }

                try { reader["MaxWorkRequestUsers"].ToString(); }
                catch { missing.Append("MaxWorkRequestUsers"); }

                try { reader["SanitationUsers"].ToString(); }
                catch { missing.Append("SanitationUsers"); }

                try { reader["MaxSanitationUsers"].ToString(); }
                catch { missing.Append("MaxSanitationUsers"); }

                try { reader["ProdAppUsers"].ToString(); }
                catch { missing.Append("ProdAppUsers "); }

                try { reader["MaxProdAppUsers"].ToString(); }
                catch { missing.Append("MaxProdAppUsers "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        #endregion
        #region V2-536
        public void UpdateIoTDeviceCount(
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
                Database.StoredProcedure.usp_Site_IotDeviceCountUpdate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region V2-962

        public void RetrieveSiteForLookupListAdmin_V2(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
   string callerUserName,
         ref List<b_Site> results
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
                results = Database.StoredProcedure.usp_Site_RetrieveAllActiveSiteForLookupListForAdmin_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName,this);

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
        public static b_Site ProcessRowForLookupListSiteForAdmin_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_Site obj = new b_Site();

            // Load the object from the database
            obj.LoadFromDatabaseForLookupListSiteForAdmin_V2(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForLookupListSiteForAdmin_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // CompanyName column, nvarchar(31), not null
                ClientName = reader.GetString(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // Name column, nvarchar(31), not null
                Name = reader.GetString(i++);
            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["ClientName"].ToString(); }
                catch { missing.Append("ClientName "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

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
        #endregion
    }
}
