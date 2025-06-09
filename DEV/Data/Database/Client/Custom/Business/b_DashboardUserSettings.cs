using System;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
    public partial class b_DashboardUserSettings
    {
        public void RetrieveByDashboardIdandUserinfoidFromDatabase(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName
     )
        {
            Database.SqlClient.ProcessRow<b_DashboardUserSettings> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_DashboardUserSettings>(reader => { this.LoadFromDatabaseForDashBoardUserSettings(reader); return this; });
                StoredProcedure.usp_DashboardUserSettings_RetrieveByDashboardListingIdAndUserInfoId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

       
        public int LoadFromDatabaseForDashBoardUserSettings(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // DashboardUserSettingsId column, bigint, not null
                DashboardUserSettingsId = reader.GetInt64(i++);

                // UserInfoId column, bigint, not null
                UserInfoId = reader.GetInt64(i++);

                // DashboardListingId column, bigint, not null
                DashboardListingId = reader.GetInt64(i++);

                // SettingInfo column, nvarchar(MAX), not null
                SettingInfo = reader.GetString(i++);

                // IsDefault column, bit, not null
                IsDefault = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["DashboardUserSettingsId"].ToString(); }
                catch { missing.Append("DashboardUserSettingsId "); }

                try { reader["UserInfoId"].ToString(); }
                catch { missing.Append("UserInfoId "); }

                try { reader["DashboardListingId"].ToString(); }
                catch { missing.Append("DashboardListingId "); }

                try { reader["SettingInfo"].ToString(); }
                catch { missing.Append("SettingInfo "); }

                try { reader["IsDefault"].ToString(); }
                catch { missing.Append("IsDefault "); }


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

        public void RetrieveDefaultDashboardListingId(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName
     )
        {
            //Database.SqlClient.ProcessRow<b_DashboardUserSettings> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                //processRow = new Database.SqlClient.ProcessRow<b_DashboardUserSettings>(reader => { this.LoadFromDatabase(reader); return this; });
                StoredProcedure.usp_DashboardUserSettings_RetrieveDefaultDashboardId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                //processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void UpdateFromDashboard_V2(
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
                Database.StoredProcedure.usp_DashboardUserSettings_UpdateFromDashboard_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public void InsertFromDashboard_V2(
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
                Database.StoredProcedure.usp_DashboardUserSettings_CreateFromDashboard_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
