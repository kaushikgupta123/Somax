using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    class usp_SecurityGroup_CreateWithPermission
    {
        private static string STOREDPROCEDURE_NAME = "usp_SecurityGroup_CreateWithPermission";

        public usp_SecurityGroup_CreateWithPermission()
        {
        }

        public static long CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_Client obj,
            string SecurityGroupName
       )
        {
            long retSecurityGroupId = 0;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetOutputParameter(SqlDbType.BigInt, "SecurityGroupId");
            command.SetStringInputParameter(SqlDbType.NVarChar, "SecurityGroupName", SecurityGroupName, 255);          

            // Execute stored procedure.
            command.ExecuteNonQuery();

            retSecurityGroupId = (long)command.Parameters["@SecurityGroupId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            return (retSecurityGroupId);
        }
    }
}
