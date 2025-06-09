
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    public partial class b_PasswordSettings
    {
        ///  property
        public int MaxAttempts { get; set; }
        ///  property
        public void UpdatePasswordSettingsByClientId(
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
                Database.StoredProcedure.usp_PasswordSettings_UpdateByClientId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public  void RetrieveByClientId(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
   string callerUserName
     )
        {
            Database.SqlClient.ProcessRow<b_PasswordSettings> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PasswordSettings>(reader => { this.LoadFromDatabaseRetrieveByClientId(reader); return this; });
                Database.StoredProcedure.usp_PasswordSettings_RetrieveByClientId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

                // PasswordSettingsId column, bigint, not null
                PasswordSettingsId = reader.GetInt64(i++);

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PWReqMinLength column, bit, not null
                PWReqMinLength = reader.GetBoolean(i++);

                // PWMinLength column, int, not null
                PWMinLength = reader.GetInt32(i++);

                // PWReqExpiration column, bit, not null
                PWReqExpiration = reader.GetBoolean(i++);

                // PWExpiresDays column, int, not null
                PWExpiresDays = reader.GetInt32(i++);

                // PWRequireNumber column, bit, not null
                PWRequireNumber = reader.GetBoolean(i++);

                // PWRequireAlpha column, bit, not null
                PWRequireAlpha = reader.GetBoolean(i++);

                // PWRequireMixedCase column, bit, not null
                PWRequireMixedCase = reader.GetBoolean(i++);

                // PWRequireSpecialChar column, bit, not null
                PWRequireSpecialChar = reader.GetBoolean(i++);

                // PWNoRepeatChar column, bit, not null
                PWNoRepeatChar = reader.GetBoolean(i++);

                // PWNotEqualUserName column, bit, not null
                PWNotEqualUserName = reader.GetBoolean(i++);

                // AllowAdminReset column, bit, not null
                AllowAdminReset = reader.GetBoolean(i++);

                // MaxAttempts column, int, not null
                MaxAttempts = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["PasswordSettingsId"].ToString(); }
                catch { missing.Append("PasswordSettingsId "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PWReqMinLength"].ToString(); }
                catch { missing.Append("PWReqMinLength "); }

                try { reader["PWMinLength"].ToString(); }
                catch { missing.Append("PWMinLength "); }

                try { reader["PWReqExpiration"].ToString(); }
                catch { missing.Append("PWReqExpiration "); }

                try { reader["PWExpiresDays"].ToString(); }
                catch { missing.Append("PWExpiresDays "); }

                try { reader["PWRequireNumber"].ToString(); }
                catch { missing.Append("PWRequireNumber "); }

                try { reader["PWRequireAlpha"].ToString(); }
                catch { missing.Append("PWRequireAlpha "); }

                try { reader["PWRequireMixedCase"].ToString(); }
                catch { missing.Append("PWRequireMixedCase "); }

                try { reader["PWRequireSpecialChar"].ToString(); }
                catch { missing.Append("PWRequireSpecialChar "); }

                try { reader["PWNoRepeatChar"].ToString(); }
                catch { missing.Append("PWNoRepeatChar "); }

                try { reader["PWNotEqualUserName"].ToString(); }
                catch { missing.Append("PWNotEqualUserName "); }

                try { reader["AllowAdminReset"].ToString(); }
                catch { missing.Append("AllowAdminReset "); }

                try { reader["MaxAttempts"].ToString(); }
                catch { missing.Append("MaxAttempts "); }
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
