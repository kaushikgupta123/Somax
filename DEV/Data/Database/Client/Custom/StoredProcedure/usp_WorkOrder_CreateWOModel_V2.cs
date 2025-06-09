using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_WorkOrder_CreateWOModel_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_CreateWOModel_V2";

        public usp_WorkOrder_CreateWOModel_V2()
        {
        }
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_WorkOrder obj

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
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 15);
            command.SetInputParameter(SqlDbType.BigInt, "WorkOrderId", obj.WorkOrderId);
            command.SetInputParameter(SqlDbType.BigInt, "Creator_PersonnelId", obj.Creator_PersonnelId);
            command.SetOutputParameter(SqlDbType.BigInt, "CreatedWorkOrderId");


            // Execute stored procedure.
            command.ExecuteNonQuery();

            // obj.ChargeTo_Name = (string)chargeTo_Name_parameter.Value;

            if (!string.IsNullOrEmpty(command.Parameters["@CreatedWorkOrderId"].Value.ToString()))
            {
                obj.CreatedWorkOrderId = (long)command.Parameters["@CreatedWorkOrderId"].Value;
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
