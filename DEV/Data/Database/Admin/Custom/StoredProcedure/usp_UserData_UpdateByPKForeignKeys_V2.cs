using Database.Business;
using Database.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_UserData_UpdateByPKForeignKeys_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_UserData_UpdateByPKForeignKeys_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_UserData_UpdateByPKForeignKeys_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_UserInfo_UpdateByPKForeignKeys_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
                SqlCommand command,
                long callerUserInfoId,
                string callerUserName,
                b_UserDetails obj
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
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);
            command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", obj.IsActive);
            command.SetOutputParameter(SqlDbType.BigInt, "LoginInfoId");
            command.SetInputParameter(SqlDbType.Bit, "IsCMMSUser", obj.CMMSUser);
            command.SetInputParameter(SqlDbType.Bit, "IsSanitationUser", obj.SanitationUser);
            command.SetOutputParameter(SqlDbType.BigInt, "AppUserOut");
            command.SetOutputParameter(SqlDbType.BigInt, "LimitedUserOut");
            command.SetOutputParameter(SqlDbType.BigInt, "WorkRequestUserOut");
            command.SetOutputParameter(SqlDbType.BigInt, "SanitationUserOut");
            command.SetOutputParameter(SqlDbType.BigInt, "SuperUserOut");
            command.SetOutputParameter(SqlDbType.BigInt, "SanAppUserOut");
            updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
            updateIndexOut_parameter.Direction = ParameterDirection.Output;
            // Execute stored procedure.
            command.ExecuteNonQuery();
            obj.UserInfoId = (long)command.Parameters["@UserInfoId"].Value;
            obj.LoginInfoId = (long)command.Parameters["@LoginInfoId"].Value;
            obj.CountWebAppUser = (long)command.Parameters["@AppUserOut"].Value;
            obj.CountLimitedUser = (long)command.Parameters["@LimitedUserOut"].Value;
            obj.CountWorkRequestUser = (long)command.Parameters["@WorkRequestUserOut"].Value;
            obj.CountSanitationUser = (long)command.Parameters["@SanitationUserOut"].Value;
            obj.CountSuperUser = (long)command.Parameters["@SuperUserOut"].Value;
            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
