using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{

    public class usp_ApprovalRoute_UpdateByObjectId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_ApprovalRoute_UpdateByObjectId_V2";

        public usp_ApprovalRoute_UpdateByObjectId_V2()
        {
        }

        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_ApprovalRoute obj
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
            command.SetInputParameter(SqlDbType.BigInt, "ObjectId", obj.ObjectId);
            command.SetInputParameter(SqlDbType.BigInt, "ApprovalGroupId", obj.ApprovalGroupId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ProcessResponse", obj.ProcessResponse, 20);
            command.SetInputParameter(SqlDbType.BigInt, "ApproverId", obj.ApproverId);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.UpdateIndexOut = (int)RETURN_CODE_parameter.Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
