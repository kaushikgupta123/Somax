/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 */


using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.Business;
using Database.SqlClient;
using Database.StoredProcedure;

namespace Database.Business
{
  /// <summary>
  /// Business object that stores a record from the TechSpecs table.
  /// </summary>
  public partial class b_Storeroom
  {
        // 2012-Mar-16 - Roger Lawton
        /// <summary>
        /// Retrieve all Storeroom table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_Storeroom[] that contains the results</param>
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 Case { get; set; }
        public Int32 nextrow { get; set; }
        public string SiteName { get; set; }
        public string SearchText { get; set; }
        public Int32 totalCount { get; set; }
        public long PersonnelId { get; set; }
        public string StoreroomAuthType { get; set; }
        public void RetreiveAuthorizedList(SqlConnection connection,SqlTransaction transaction,long callerUserInfoId,string callerUserName,ref b_Storeroom[] data )
    {
      Database.SqlClient.ProcessRow<b_Storeroom> processRow = null;
      ArrayList results = null;
      SqlCommand command = null;
      string message = String.Empty;

      // Initialize the results
      data = new b_Storeroom[0];

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_Storeroom>(reader => { b_Storeroom obj = new b_Storeroom(); obj.LoadFromDatabase(reader); return obj; });
        results = Database.StoredProcedure.usp_Storeroom_RetrieveAuthorizedList.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

        // Extract the results
        if (null != results)
        {
          data = (b_Storeroom[])results.ToArray(typeof(b_Storeroom));
        }
        else
        {
          data = new b_Storeroom[0];
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

        //-----------rnj
        public void RetrieveChunkSearch(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_Storeroom> results
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

                results = usp_Storeroom_RetrieveForChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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



        public static b_Storeroom ProcessRowForChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_Storeroom storeroom = new b_Storeroom();
            storeroom.LoadFromDatabaseForChunkSearch(reader);
            return storeroom;
        }
        public int LoadFromDatabaseForChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                //i = LoadFromDatabase(reader);
                ClientId=reader.GetInt64(i++);

                StoreroomId  = reader.GetInt64(i++);

                SiteId = reader.GetInt64(i++);
                
                Name = reader.GetString(i++);
                SiteName = reader.GetString(i++);

                Description = reader.GetString(i++);
                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // totalCount column, int, not null              
                totalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["totalCount"].ToString(); }
                catch { missing.Append("totalCount "); }


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

        public void RetrieveByPKCustomFromDatabase(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
     string callerUserName
       )
        {
            ProcessRow<b_Storeroom> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new ProcessRow<b_Storeroom>(reader => { this.LoadFoRetrieveByPKCustomrFromDatabase(reader); return this; });
                usp_Storeroom_RetrieveByPKCustom_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // Name column, nvarchar(15), not null
                Name = reader.GetString(i++);

                // Description column, nvarchar(255), not null
                Description = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

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

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

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
        //-----------

        #region RetrieveStoreroomList_V2 
        public void RetrieveStoreroomListFromDatabase(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref List<b_Storeroom> results
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

                results = usp_Storeroom_RetrieveStoreroomList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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



        public static b_Storeroom ProcessRowForStoreroomList_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_Storeroom storeroom = new b_Storeroom();
            storeroom.LoadFromDatabaseForStoreroomList_V2(reader);
            return storeroom;
        }
        public int LoadFromDatabaseForStoreroomList_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                
                StoreroomId = reader.GetInt64(i++);
             
                Name = reader.GetString(i++);

                Description = reader.GetString(i++);
              
            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


             

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

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
        public void ValidateSiteId(
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
                results = Database.StoredProcedure.usp_Storeroom_ValidateSiteId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region V2-1025
        public void RetrieveAllStoreroomsForLookupListByClientIdSiteId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Storeroom obj, ref List<b_Storeroom> results)
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

                results = Database.StoredProcedure.usp_Storeroom_RetrieveAllForLookupListByClientIdSiteId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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

        #region Chunk Search Lookup list
        public void RetrieveStoreroomLookuplistChunkSearchV2(SqlConnection connection, SqlTransaction transaction,
                        long callerUserInfoId, string callerUserName, ref List<b_Storeroom> results)
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

                results = Database.StoredProcedure.usp_Storeroom_RetrieveChunkSearchLookupList_V2
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
        public static b_Storeroom ProcessRowForChunkSearchLookupList(SqlDataReader reader)
        {
            b_Storeroom Storeroom = new b_Storeroom();

            Storeroom.LoadFromDatabaseForLookupListChunkSearchV2(reader);
            return Storeroom;
        }
        public int LoadFromDatabaseForLookupListChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);
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
                //Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;
                //totalCount
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

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

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
    }
}