using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PrevMaintMaster_Create_Master_Child
    {
        private static string STOREDPROCEDURE_NAME = "usp_PrevMaintMaster_Create_Master_Child";
        public usp_PrevMaintMaster_Create_Master_Child()
        {
        }

        public static void CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_PrevMaintMaster obj

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
            command.SetInputParameter(SqlDbType.BigInt, "PrevMaintMasterId", obj.PrevMaintMasterId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 31);

            command.Parameters["@PrevMaintMasterId"].Direction = ParameterDirection.InputOutput;

            // Execute stored procedure.
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            { }

            if (!string.IsNullOrEmpty(command.Parameters["@PrevMaintMasterId"].Value.ToString()))
            {
                obj.PrevMaintMasterId = (long)command.Parameters["@PrevMaintMasterId"].Value;
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
