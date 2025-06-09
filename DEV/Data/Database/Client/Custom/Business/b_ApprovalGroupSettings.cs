using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    public partial class b_ApprovalGroupSettings
    {
        #region Property
        
        #endregion
        public int LoadForRetrieveByClientIdFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ApprovalGroupSettingsId column, bigint, not null
                ApprovalGroupSettingsId = reader.GetInt64(i++);

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // WorkRequests column, bit, not null
                WorkRequests = reader.GetBoolean(i++);

                // PurchaseRequests column, bit, not null
                PurchaseRequests = reader.GetBoolean(i++);

                // MaterialRequests column, bit, not null
                MaterialRequests = reader.GetBoolean(i++);

                // SanitationRequests column, bit, not null
                SanitationRequests = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ApprovalGroupSettingsId"].ToString(); }
                catch { missing.Append("ApprovalGroupSettingsId "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["WorkRequests"].ToString(); }
                catch { missing.Append("WorkRequests "); }

                try { reader["PurchaseRequests"].ToString(); }
                catch { missing.Append("PurchaseRequests "); }

                try { reader["MaterialRequests"].ToString(); }
                catch { missing.Append("MaterialRequests "); }

                try { reader["SanitationRequests"].ToString(); }
                catch { missing.Append("SanitationRequests "); }


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

        public void RetreiveApprovalGroupSettingsList(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref b_ApprovalGroupSettings[] data)
        {
            Database.SqlClient.ProcessRow<b_ApprovalGroupSettings> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_ApprovalGroupSettings[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                processRow = new Database.SqlClient.ProcessRow<b_ApprovalGroupSettings>(reader => { b_ApprovalGroupSettings obj = new b_ApprovalGroupSettings(); obj.LoadForRetrieveByClientIdFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_ApprovalGroupSettings_RetrieveByClientId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName,this);

                // Extract the results
                if (null != results)
                {
                    data = (b_ApprovalGroupSettings[])results.ToArray(typeof(b_ApprovalGroupSettings));
                }
                else
                {
                    data = new b_ApprovalGroupSettings[0];
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

        #region V2-730
        public static object ProcessRowForLogin(SqlDataReader reader)
        {
            // Create instance of object
            b_ApprovalGroupSettings obj = new b_ApprovalGroupSettings();

            // Load the object from the database
            obj.LoadFromDatabaseForLogin(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_ApprovalGroupSettings object.
        /// This routine should be applied to the usp_ApprovalGroupSettings_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_ApprovalGroupSettings_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabaseForLogin(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // WorkRequests column, bit, not null
                WorkRequests = reader.GetBoolean(i++);

                // PurchaseRequests column, bit, not null
                PurchaseRequests = reader.GetBoolean(i++);

                // MaterialRequests column, bit, not null
                MaterialRequests = reader.GetBoolean(i++);

                // SanitationRequests column, bit, not null
                SanitationRequests = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["WorkRequests"].ToString(); }
                catch { missing.Append("WorkRequests "); }

                try { reader["PurchaseRequests"].ToString(); }
                catch { missing.Append("PurchaseRequests "); }

                try { reader["MaterialRequests"].ToString(); }
                catch { missing.Append("MaterialRequests "); }

                try { reader["SanitationRequests"].ToString(); }
                catch { missing.Append("SanitationRequests "); }


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
