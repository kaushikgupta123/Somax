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

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the Client table.
    /// </summary>
    public partial class b_SiteMaintenance
    {
        /// <summary>
        /// Retrieve SiteMaintenance table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void RetrieveOutageSiteMaintenance(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_SiteMaintenance> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SiteMaintenance>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_SiteMaintenance_RetrieveOutage.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        #region System Unavailable Message

        /// <summary>
        /// EasternStartTime property
        /// </summary>
        public string EasternStartTime { get; set; }

        /// <summary>
        /// EasternEndTime property
        /// </summary>
        public string EasternEndTime { get; set; }

        /// <summary>
        /// TimeZone property
        /// </summary>
        public string TimeZone { get; set; }
        /// <summary>
        /// SameDay property
        /// </summary>
        public string SameDay { get; set; }

        #region property V2-994
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public Int32 TotalCount { get; set; }
        public DateTime CreateDate { get; set; }

        #endregion

        public void RetrieveNextSchSiteMaintenance(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          string timeZone,
          string sameDay
      )
        {
            Database.SqlClient.ProcessRow<b_SiteMaintenance> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SiteMaintenance>(reader => { this.LoadFromNewDatabase(reader); return this; });
                Database.StoredProcedure.usp_SiteMaintenance_RetrieveNextSchMaintain.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, timeZone, sameDay, this);

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

        public static b_SiteMaintenance ProcessRowForRetrieveChunkSearchFromDetails(SqlDataReader reader)
        {
            b_SiteMaintenance SiteMaintenance = new b_SiteMaintenance();

            SiteMaintenance.LoadFromDatabaseForRetrieveChunkSearchFromDetails(reader);
            return SiteMaintenance;
        }
        public int LoadFromDatabaseForRetrieveChunkSearchFromDetails(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                SiteMaintenanceId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    LoginPageMessage = reader.GetString(i);
                }
                else
                {
                    LoginPageMessage = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    DowntimeStart = reader.GetDateTime(i);
                }
                else
                {
                    DowntimeStart = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    DowntimeEnd = reader.GetDateTime(i);
                }
                else
                {
                    DowntimeEnd = DateTime.MinValue;
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

                try { reader["SiteMaintenanceId"].ToString(); }
                catch { missing.Append("SiteMaintenanceId "); }

                try { reader["LoginPageMessage"].ToString(); }
                catch { missing.Append("LoginPageMessage "); }

                try { reader["DowntimeStart"].ToString(); }
                catch { missing.Append("DowntimeStart "); }

                try { reader["DowntimeEnd"].ToString(); }
                catch { missing.Append("DowntimeEnd "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

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

        #region V2-994
        public void RetrieveChunkSearchSiteMaintenanceDetails(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_SiteMaintenance> results
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
                results = Database.StoredProcedure.usp_SiteMaintenance_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public int LoadFromNewDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // SiteMaintenanceId column, bigint, not null
                SiteMaintenanceId = reader.GetInt64(i++);

                // HeaderText column, nvarchar(255), not null
                HeaderText = reader.GetString(i++);

                // MessageText column, nvarchar(4000), not null
                MessageText = reader.GetString(i++);

                // DowntimeStart column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    DowntimeStart = reader.GetDateTime(i);
                }
                else
                {
                    DowntimeStart = DateTime.MinValue;
                }
                i++;
                // DowntimeEnd column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    DowntimeEnd = reader.GetDateTime(i);
                }
                else
                {
                    DowntimeEnd = DateTime.MinValue;
                }
                i++;
                // LoginPageMessage column, nvarchar(MAX), not null
                LoginPageMessage = reader.GetString(i++);

                // DashboardMessage column, nvarchar(MAX), not null
                DashboardMessage = reader.GetString(i++);

                // UpdateIndex column, bigint, not null
                UpdateIndex = reader.GetInt64(i++);

                // EasternStartTime column, nvarchar(255), not null
                EasternStartTime = reader.GetString(i++);

                // HeaderText column, nvarchar(255), not null
                EasternEndTime = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["SiteMaintenanceId"].ToString(); }
                catch { missing.Append("SiteMaintenanceId "); }

                try { reader["HeaderText"].ToString(); }
                catch { missing.Append("HeaderText "); }

                try { reader["MessageText"].ToString(); }
                catch { missing.Append("MessageText "); }

                try { reader["DowntimeStart"].ToString(); }
                catch { missing.Append("DowntimeStart "); }

                try { reader["DowntimeEnd"].ToString(); }
                catch { missing.Append("DowntimeEnd "); }

                try { reader["LoginPageMessage"].ToString(); }
                catch { missing.Append("LoginPageMessage "); }

                try { reader["DashboardMessage"].ToString(); }
                catch { missing.Append("DashboardMessage "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["EasternStartTime"].ToString(); }
                catch { missing.Append("EasternStartTime "); }

                try { reader["EasternEndTime"].ToString(); }
                catch { missing.Append("EasternEndTime "); }

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
