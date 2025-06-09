using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_WOCompletionSettings
    {
        public void RetrieveByClientId(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
  string callerUserName
    )
        {
            Database.SqlClient.ProcessRow<b_WOCompletionSettings> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_WOCompletionSettings>(reader => { this.LoadFromDatabaseRetrieveByClientId(reader); return this; });
                Database.StoredProcedure.usp_WOCompletionSettings_RetrieveByClientId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

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

        public int LoadFromDatabaseRetrieveByClientId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // WOCompletionSettingsId column, bigint, not null
                WOCompletionSettingsId = reader.GetInt64(i++);

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // UseWOCompletionWizard column, bit, not null
                if (false == reader.IsDBNull(i))
                {
                    UseWOCompletionWizard = reader.GetBoolean(i);
                }
                else
                {
                    UseWOCompletionWizard = false;
                }
                i++;

                // WOCommentTab column, bit, not null
                if (false == reader.IsDBNull(i))
                {
                    WOCommentTab = reader.GetBoolean(i);
                }
                else
                {
                    WOCommentTab = false;
                }
                i++;

                // TimecardTab column, bit, not null               
                if (false == reader.IsDBNull(i))
                {
                    TimecardTab = reader.GetBoolean(i);
                }
                else
                {
                    TimecardTab = false;
                }
                i++;
                // AutoAddTimecard column, bit, not null               
                if (false == reader.IsDBNull(i))
                {
                    AutoAddTimecard = reader.GetBoolean(i);
                }
                else
                {
                    AutoAddTimecard = false;
                }
                i++;
               
                // WOCompCriteriaTab column, bit, not null
                if (false == reader.IsDBNull(i))
                {
                    WOCompCriteriaTab = reader.GetBoolean(i);
                }
                else
                {
                    WOCompCriteriaTab = false;
                }
                i++;

                //WOCompCriteriaTitle column, string, not null
                WOCompCriteriaTitle = reader.GetString(i++);

                //WOCompCriteria column, string, not null
                WOCompCriteria = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["WOCompletionSettingsId"].ToString(); }
                catch { missing.Append("WOCompletionSettingsId "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["UseWOCompletionWizard"].ToString(); }
                catch { missing.Append("UseWOCompletionWizard "); }

                try { reader["WOCommentTab"].ToString(); }
                catch { missing.Append("WOCommentTab "); }

                try { reader["TimecardTab"].ToString(); }
                catch { missing.Append("TimecardTab "); }

                try { reader["AutoAddTimecard"].ToString(); }
                catch { missing.Append("AutoAddTimecard "); }

                try { reader["WOCompCriteriaTab"].ToString(); }
                catch { missing.Append("WOCompCriteriaTab "); }

                try { reader["WOCompCriteriaTitle"].ToString(); }
                catch { missing.Append("WOCompCriteriaTitle "); }

                try { reader["WOCompCriteria"].ToString(); }
                catch { missing.Append("WOCompCriteria "); }


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
    }



}
