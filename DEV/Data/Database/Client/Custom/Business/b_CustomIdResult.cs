/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 */

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace  Database.Business
{
    /// <summary>
    /// Business object that stores a record from the CustomId table.
    /// </summary>
    [Serializable()]
    public partial class b_CustomIdResult 
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_CustomIdResult()
        {
            KeyList= String.Empty;
            Pattern = String.Empty;
            LastSeed = 0;
            UpdateIndex = 0;
            ModifiedDate = DateTime.MinValue;
        }
        /// <summary>
        /// KeyList property
        /// </summary>
        public string KeyList { get; set; }
        /// <summary>
        /// Pattern property
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// LastSeed property
        /// </summary>
        public long LastSeed { get; set; }

        /// <summary>
        /// UpdateIndex property
        /// </summary>
        public long UpdateIndex { get; set; }

        /// <summary>
        /// ModifiedDate property
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_CustomId object.
        /// This routine should be applied to the usp_CustomId_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_CustomId_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_CustomId object</returns>
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public string PRPrefix { get; set; }
        public string POPrefix { get; set; }
        public int PRUpdateIndexOut { get; set; }
        public int POUpdateIndexOut { get; set; }
        public string Key { get; set; }
        public string Prefix { get; set; }
        
        public static object ProcessRow(SqlDataReader reader)
        {
            // Create instance of object
            b_CustomId obj = new b_CustomId();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_CustomId object.
        /// This routine should be applied to the usp_CustomId_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_CustomId_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public void LoadFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // Pattern column, nvarchar(255), not null
                Pattern = reader.GetString(i++);

                // LastSeed column, bigint, not null
                LastSeed = reader.GetInt64(i++);

                // UpdateIndex column, bigint, not null
                UpdateIndex = reader.GetInt64(i++);

                // UpdateIndex column, bigint, not null
                ModifiedDate = reader.GetDateTime(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["Pattern"].ToString(); }
                catch { missing.Append("Pattern "); }

                try { reader["LastSeed"].ToString(); }
                catch { missing.Append("LastSeed "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["ModifiedDate"].ToString(); }
                catch { missing.Append("ClientId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public void RetrieveNextSeed(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            string key,
            long siteId
        )
        {
             Database.SqlClient.ProcessRow<b_CustomIdResult> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new  Database.SqlClient.ProcessRow<b_CustomIdResult>(reader => { this.LoadFromDatabase(reader); return this; });
                 Database.StoredProcedure.usp_CustomId_RetrieveNextSeed.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, clientId, key, siteId);
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


        public void RetrieveByClientIdandSiteIdandKey_V2(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          long clientId,          
          long siteId,
          ref List<b_CustomIdResult>temp
      )
        {
            Database.SqlClient.ProcessRow<b_CustomIdResult> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_CustomIdResult>(reader => { this.LoadFromDatabase(reader); return this; });
                temp = StoredProcedure.usp_CustomId_RetrieveByClientIdandSiteIdandKey_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, clientId,siteId,this);
                 
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


   

        public static b_CustomIdResult LoadFromDatabaseForSiteIdClientIdRetrieve(SqlDataReader reader)
        {
            b_CustomIdResult obj = new b_CustomIdResult();
            int i = 0;
            try
            {

                obj.ClientId = reader.GetInt64(i++);
                obj.SiteId= reader.GetInt64(i++);               
                if (false == reader.IsDBNull(i))
                {
                    obj.Key = reader.GetString(i);
                  }
                else
                {
                    obj.Key = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    obj.Prefix= reader.GetString(i);
                }
                else
                {
                    obj.Prefix = "";
                }
                i++;
                   

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["Key"].ToString(); }
                catch { missing.Append("Key "); }

                try { reader["Prefix"].ToString(); }
                catch { missing.Append("Prefix "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return obj;
        }
        public void ResetAndRetrieveNextSeed(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        long clientId,
        string key,
        long siteId,
            long updateIndex
    )
        {
             Database.SqlClient.ProcessRow<b_CustomIdResult> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new  Database.SqlClient.ProcessRow<b_CustomIdResult>(reader => { this.LoadFromDatabase(reader); return this; });
                 Database.StoredProcedure.usp_CustomId_ResetAndRetrieveNextSeed.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, clientId, key, siteId, updateIndex);
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



        public void UpdateUpdatePrefixbyKey_V2(
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
                Database.StoredProcedure.usp_CustomId_UpdatePrefixbyKey_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

    }
}
