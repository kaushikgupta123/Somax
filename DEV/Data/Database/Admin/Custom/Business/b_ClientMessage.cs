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
using Database.SqlClient;
using System.Collections.Generic;
using static DevExpress.Xpo.Helpers.CannotLoadObjectsHelper;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the Client table.
    /// </summary>
    public partial class b_ClientMessage
    {
        #region V2-993
        #region property
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public Int32 TotalCount { get; set; }
        public DateTime CreateDate { get; set; }
        public string EasternStartTime { get; set; }
        public string EasternEndTime { get; set; }
        public string TimeZone { get; set; }
        public long CustomClientId { get; set; }
        public List<b_ClientMessage> listOfAllClientMeassge { get; set; }

        #endregion

        public static b_ClientMessage ProcessRowForRetrieveChunkSearchFromDetails(SqlDataReader reader)
        {
            b_ClientMessage ClientMessage = new b_ClientMessage();

            ClientMessage.LoadFromDatabaseForRetrieveChunkSearchFromDetails(reader);
            return ClientMessage;
        }
        public int LoadFromDatabaseForRetrieveChunkSearchFromDetails(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);

                ClientMessageId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    Message = reader.GetString(i);
                }
                else
                {
                    Message = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    StartDate = reader.GetDateTime(i);
                }
                else
                {
                    StartDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    EndDate = reader.GetDateTime(i);
                }
                else
                {
                    EndDate = DateTime.MinValue;
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
                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientMessageId"].ToString(); }
                catch { missing.Append("ClientMessageId"); }

                try { reader["Message"].ToString(); }
                catch { missing.Append("Message"); }

                try { reader["StartDate"].ToString(); }
                catch { missing.Append("StartDate"); }

                try { reader["EndDate"].ToString(); }
                catch { missing.Append("EndDate "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate"); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount"); }

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


        public static b_ClientMessage ProcessRowForRetrieveAllClientMessages(SqlDataReader reader)
        {
            b_ClientMessage ClientMessage = new b_ClientMessage();

            ClientMessage.LoadFromDatabaseForRetrieveAllClientMessages(reader);
            return ClientMessage;
        }
        public int LoadFromDatabaseForRetrieveAllClientMessages(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);                

                if (false == reader.IsDBNull(i))
                {
                    Message = reader.GetString(i);
                }
                else
                {
                    Message = "";
                }
                i++;                
               
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["Message"].ToString(); }
                catch { missing.Append("Message "); }
               
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
        /// Update the Retrieve ChunkSearch ClientMessage Details table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="clientId">long that identifies the user calling the database</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void RetrieveChunkSearchClientMessageDetails(
        SqlConnection connection,
        SqlTransaction transaction,
        long clientId,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_ClientMessage> results
        )
        {
            SqlCommand command = null;
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_ClientMessage_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, clientId, this);
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

        #region Client message notification V2-993 

        /// <summary>
        /// Update the ClientMessage table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void RetrieveClientMessageSearch(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
         string timeZone,
         ref List<b_ClientMessage> results
     )
        {
            SqlCommand command = null;
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                results = Database.StoredProcedure.usp_ClientMessageNotification_RetrieveMessage_V2.CallStoredProcedure(command,
                    callerUserInfoId, callerUserName, timeZone, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        #endregion


        #endregion
    }
}
