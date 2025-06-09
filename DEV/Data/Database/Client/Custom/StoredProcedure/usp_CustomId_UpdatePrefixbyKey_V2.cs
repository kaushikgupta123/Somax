using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    class usp_CustomId_UpdatePrefixbyKey_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_CustomId_UpdatePrefixbyKey_V2";

        public usp_CustomId_UpdatePrefixbyKey_V2()
        {
        }

        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_CustomIdResult obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter PRupdateIndexOut_parameter = null;
            SqlParameter POupdateIndexOut_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);            
            command.SetStringInputParameter(SqlDbType.NVarChar, "PRPrefix", obj.PRPrefix,4);
            command.SetStringInputParameter(SqlDbType.NVarChar, "POPrefix", obj.POPrefix,4);

            // Setup updateIndexOut parameter.
            PRupdateIndexOut_parameter = command.Parameters.Add("@PRUpdateIndexOut", SqlDbType.Int);
            PRupdateIndexOut_parameter.Direction = ParameterDirection.Output;
            POupdateIndexOut_parameter= command.Parameters.Add("@POUpdateIndexOut", SqlDbType.Int);
            POupdateIndexOut_parameter.Direction= ParameterDirection.Output;
            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PRUpdateIndexOut = (int)PRupdateIndexOut_parameter.Value;
            obj.POUpdateIndexOut = (int)POupdateIndexOut_parameter.Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
