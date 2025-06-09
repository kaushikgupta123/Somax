using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_UserData_UpdateByUserInfoIdWithUserAccess_V2
    {

        private static string STOREDPROCEDURE_NAME = "usp_UserData_UpdateByUserInfoIdWithUserAccess_V2";

        public usp_UserData_UpdateByUserInfoIdWithUserAccess_V2()
        {
        }


        public static void CallStoredProcedure(
         SqlCommand command,
         long callerUserInfoId,
         string callerUserName,
         b_UserDetails obj
     )

        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "DefaultSiteId", obj.DefaultSiteId);
            command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UserType", obj.UserType, 31);
            command.SetInputParameter(SqlDbType.BigInt, "SecurityProfileId", obj.SecurityProfileId);
            command.SetInputParameter(SqlDbType.Bit, "IsSuperUser", obj.IsSuperUser);
            command.SetInputParameter(SqlDbType.Bit, "IsSiteAdmin", obj.IsSiteAdmin);
            command.SetInputParameter(SqlDbType.BigInt, "UserUpdateIndex", obj.UserUpdateIndex);       
            command.ExecuteNonQuery();       

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

        }
    }
}
