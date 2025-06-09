using System;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_PasswordSettings_UpdateByClientId_V2 stored procedure.
    /// </summary>
    public class usp_PasswordSettings_UpdateByClientId_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        /// 
        private static string STOREDPROCEDURE_NAME = "usp_PasswordSettings_UpdateByClientId_V2";
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// 
        public usp_PasswordSettings_UpdateByClientId_V2()
        {
        }
        /// <summary>
        /// Static method to call the usp_PasswordSettings_UpdateByPK stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PasswordSettings obj
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
            command.SetInputParameter(SqlDbType.Int, "MaxAttempts", obj.MaxAttempts);
            command.SetInputParameter(SqlDbType.Bit, "PWReqMinLength", obj.PWReqMinLength);
            command.SetInputParameter(SqlDbType.Int, "PWMinLength", obj.PWMinLength);
            command.SetInputParameter(SqlDbType.Bit, "PWReqExpiration", obj.PWReqExpiration);
            command.SetInputParameter(SqlDbType.Int, "PWExpiresDays", obj.PWExpiresDays);
            command.SetInputParameter(SqlDbType.Bit, "PWRequireNumber", obj.PWRequireNumber);
            command.SetInputParameter(SqlDbType.Bit, "PWRequireAlpha", obj.PWRequireAlpha);
            command.SetInputParameter(SqlDbType.Bit, "PWRequireMixedCase", obj.PWRequireMixedCase);
            command.SetInputParameter(SqlDbType.Bit, "PWRequireSpecialChar", obj.PWRequireSpecialChar);
            command.SetInputParameter(SqlDbType.Bit, "PWNoRepeatChar", obj.PWNoRepeatChar);
            command.SetInputParameter(SqlDbType.Bit, "PWNotEqualUserName", obj.PWNotEqualUserName);
            command.SetInputParameter(SqlDbType.Bit, "AllowAdminReset", obj.AllowAdminReset);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }

    }
}
