/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* PopupAddWorkRequest
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2016-Aug-10 SOM-1037 Roger Lawton       Added to retreive alert information for notification
****************************************************************************************************
 */
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the AlertSetup table.InsertIntoDatabase
    /// </summary>
    public partial class b_AlertSetup : DataBusinessBase
    {
        #region public properties
        public bool Alert_Active { get; set; }
        public bool Alert_TargetList { get; set; }
        public string Alert_LangCult { get; set; }
        public string Alert_Name { get; set; }
        public string Alert_Headline { get; set; }
        public string Alert_Summary { get; set; }
        public string Alert_Type { get; set; }
        public string Alert_Details { get; set; }

        /// <summary>
        /// Description property
        /// </summary>
        public string Description { get; set; }
        public bool TargetList { get; set; }

        public string Name { get; set; }
        public bool SMSSend { get; set; }
        public string Type { get; set; }

        #endregion publis properties

        public void LoadFromDatabaseExtended(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {
                Alert_TargetList = reader.GetBoolean(i++);

                Alert_Active = reader.GetBoolean(i++);

                Alert_LangCult = reader.GetString(i++);

                Alert_Name = reader.GetString(i++);

                Alert_Type = reader.GetString(i++);

                Alert_Headline = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    Alert_Summary = reader.GetString(i);
                }
                else
                {
                    Alert_Summary = string.Empty;
                }
                i++;


                Alert_Details = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["Alert_Active"].ToString(); }
                catch { missing.Append("Alert_Active"); }

                try { reader["Alert_TargetList"].ToString(); }
                catch { missing.Append("Alert_TargetList"); }

                try { reader["Alert_LangCult"].ToString(); }
                catch { missing.Append("Alert_LangCult"); }

                try { reader["Alert_Name"].ToString(); }
                catch { missing.Append("Alert_Name"); }

                try { reader["Alert_Type"].ToString(); }
                catch { missing.Append("Alert_Type"); }

                try { reader["Alert_Headline"].ToString(); }
                catch { missing.Append("Alert_Headline"); }

                try { reader["Alert_Summary"].ToString(); }
                catch { missing.Append("Alert_Summary"); }

                try { reader["Alert_Details"].ToString(); }
                catch { missing.Append("Alert_Details"); }

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
        /// Retrieve AlertSetup with extended information to use when creating notification
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void RetrieveForNotification(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            {
                Database.SqlClient.ProcessRow<b_AlertSetup> processRow = null;
                SqlCommand command = null;
                string message = String.Empty;

                try
                {
                    // Create the command to use in calling the stored procedures
                    command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;

                    // Call the stored procedure to retrieve the data
                    processRow = new Database.SqlClient.ProcessRow<b_AlertSetup>(reader => { this.LoadFromDatabaseExtended(reader); return this; });
                    Database.StoredProcedure.usp_Alerts_RetrieveForNotification.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        }

        public int LoadFromExdDatabase(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);

                // AlertSetupId column, bigint, not null
                AlertSetupId = reader.GetInt64(i++);

                // IsActive column, bit, not null
                IsActive = reader.GetBoolean(i++);

                // EmailSend column, bit, not null
                EmailSend = reader.GetBoolean(i++);

                // EmailAttachment column, bit, not null
                EmailAttachment = reader.GetBoolean(i++);

                // AlertDefineId column, bigint, not null
                AlertDefineId = reader.GetInt64(i++);

                // AlertLocalId column, bigint, not null
                AlertLocalId = reader.GetInt64(i++);

                // Description column, nvarchar(255), not null
                Description = reader.GetString(i++);

                // TargetList column, bit, not null
                TargetList = reader.GetBoolean(i++);

                // Name column, nvarchar(255), not null
                Name = reader.GetString(i++);


                // SMSSend column, bit, not null
                SMSSend = reader.GetBoolean(i++);

                // Type column, nvarchar(255), not null
                Type = reader.GetString(i++);


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["AlertSetupId"].ToString(); }
                catch { missing.Append("AlertSetupId "); }

                try { reader["IsActive"].ToString(); }
                catch { missing.Append("IsActive "); }

                try { reader["EmailSend"].ToString(); }
                catch { missing.Append("EmailSend "); }

                try { reader["EmailAttachment"].ToString(); }
                catch { missing.Append("EmailAttachment "); }


                try { reader["AlertDefineId"].ToString(); }
                catch { missing.Append("AlertDefineId "); }


                try { reader["AlertLocalId"].ToString(); }
                catch { missing.Append("AlertLocalId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

              
                try { reader["TargetList"].ToString(); }
                catch { missing.Append("TargetList "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }


                try { reader["SMSSend"].ToString(); }
                catch { missing.Append("SMSSend "); }

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
            return i;
        }
        public void RetrieveNotificationDetails(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
     string callerUserName
       )
        {
            Database.SqlClient.ProcessRow<b_AlertSetup> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_AlertSetup>(reader => { this.LoadFromExdDatabase(reader); return this; });
                Database.StoredProcedure.usp_AlertSetUp_RetrieveNotification_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

    }
}
