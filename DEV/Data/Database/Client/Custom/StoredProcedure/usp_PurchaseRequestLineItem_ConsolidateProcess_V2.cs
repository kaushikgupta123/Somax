using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PurchaseRequestLineItem_ConsolidateProcess_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseRequestLineItem_ConsolidateProcess_V2";

        public usp_PurchaseRequestLineItem_ConsolidateProcess_V2()
        {
        }

        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PurchaseRequestLineItem obj
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
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseRequestId", obj.PurchaseRequestId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PurchaseRequestLineItemIds", obj.PRLineItemIds, 1073741823);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
