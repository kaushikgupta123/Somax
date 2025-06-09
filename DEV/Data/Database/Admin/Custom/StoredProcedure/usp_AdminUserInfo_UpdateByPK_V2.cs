using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_AdminUserInfo_UpdateByPK_V2 stored procedure.
    /// </summary>
    public class usp_AdminUserInfo_UpdateByPK_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_AdminUserInfo_UpdateByPK_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_AdminUserInfo_UpdateByPK_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_UserInfo_UpdateByPK stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="updateIndexOut">int that contains the value of the @UpdateIndexOut parameter</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_UserInfo obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter updateIndexOut_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            //command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);
            command.SetInputParameter(SqlDbType.BigInt, "SecurityProfileId", obj.SecurityProfileId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "FirstName", obj.FirstName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "LastName", obj.LastName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "MiddleName", obj.MiddleName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Email", obj.Email, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Localization", obj.Localization, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UIConfiguration", obj.UIConfiguration, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TimeZone", obj.TimeZone, 31);
            command.SetInputParameter(SqlDbType.Bit, "IsSuperUser", obj.IsSuperUser);
            command.SetInputParameter(SqlDbType.BigInt, "DefaultSiteId", obj.DefaultSiteId);
            command.SetInputParameter(SqlDbType.Bit, "TabletUser", obj.TabletUser);
            command.SetInputParameter(SqlDbType.Bit, "PhoneUser", obj.PhoneUser);
            command.SetInputParameter(SqlDbType.Bit, "WebAppUser", obj.WebAppUser);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UserType", obj.UserType, 31);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", obj.ResultsPerPage);
            command.SetStringInputParameter(SqlDbType.NVarChar, "StartPage", obj.StartPage, 255);
            command.SetInputParameter(SqlDbType.Bit, "IsPasswordTemporary", obj.IsPasswordTemporary);
            command.SetInputParameter(SqlDbType.Bit, "MaintenanceNotify", obj.MaintenanceNotify);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UIVersion", obj.UIVersion, 15);
            command.SetInputParameter(SqlDbType.Bit, "IsSiteAdmin", obj.IsSiteAdmin);
            //command.SetInputParameter(SqlDbType.BigInt, "V2SecurityProfileId", obj.V2SecurityProfileId);
            command.SetInputParameter(SqlDbType.BigInt, "UpdateIndex", obj.UpdateIndex);

            // Setup updateIndexOut parameter.
            updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
            updateIndexOut_parameter.Direction = ParameterDirection.Output;

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.UpdateIndex = (int)updateIndexOut_parameter.Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}