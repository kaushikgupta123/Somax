/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* b_AlertTarget
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2016-Sep-01 SOM-1037 Roger Lawton       Added RetrieveTargetList
* 2016-Oct-31 SOM-652  Roger Lawton       Added email for target                                     
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
    /// Business object that stores a record from the AlertTarget table.InsertIntoDatabase
    /// </summary>
    public partial class b_AlertTarget : DataBusinessBase
    {
        #region properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Personnel_ClientLookupId { get; set; }
        public string email_address { get; set; }
        public string UserName { get; set; }
        #endregion properties

        /// <summary>
        /// Retrieve AlertTarget entries for a particular AlertSetup record
        /// </summary>
        public void RetrieveTargetList(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, long AlertSetupId, bool Include_Inactive, ref b_AlertTarget[] data)
        {
            ProcessRow<b_AlertTarget> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_AlertTarget[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new ProcessRow<b_AlertTarget>(reader => { b_AlertTarget obj = new b_AlertTarget(); obj.LoadFromDatabaseExtended(reader); return obj; });
                results = Database.StoredProcedure.usp_AlertTarget_RetrieveTargetList.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, AlertSetupId, Include_Inactive);

                // Extract the results
                if (null != results)
                {
                    data = (b_AlertTarget[])results.ToArray(typeof(b_AlertTarget));
                }
                else
                {
                    data = new b_AlertTarget[0];
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
        public void LoadFromDatabaseExtended(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {
                FirstName = reader.GetString(i++);

                LastName = reader.GetString(i++);

                Personnel_ClientLookupId = reader.GetString(i++);

                email_address = reader.GetString(i++);

                UserName = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["FirstName"].ToString(); }
                catch { missing.Append("FirstName"); }

                try { reader["LastName"].ToString(); }
                catch { missing.Append("LastName"); }

                try { reader["Personnel_ClientLookupId"].ToString(); }
                catch { missing.Append("Personnel_ClientLookupId"); }

                try { reader["email_address"].ToString(); }
                catch { missing.Append("email_address"); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

    }
}
