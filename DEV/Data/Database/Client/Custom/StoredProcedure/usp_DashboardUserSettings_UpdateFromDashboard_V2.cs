using Database.Business;
using Database.SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_DashboardUserSettings_UpdateFromDashboard_V2 stored procedure.
    /// </summary>
    public class usp_DashboardUserSettings_UpdateFromDashboard_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_DashboardUserSettings_UpdateFromDashboard_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_DashboardUserSettings_UpdateFromDashboard_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_DashboardUserSettings_UpdateFromDashboard_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure (
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_DashboardUserSettings obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "DashboardUserSettingsId", obj.DashboardUserSettingsId);
            command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);
            command.SetInputParameter(SqlDbType.BigInt, "DashboardListingId", obj.DashboardListingId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SettingInfo", obj.SettingInfo, 1073741823);
            command.SetInputParameter(SqlDbType.Bit, "IsDefault", obj.IsDefault);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            

            // Process the RETURN_CODE parameter value
            retCode = (int) RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}