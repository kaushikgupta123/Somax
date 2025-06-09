using Database.Business;
using Database.SqlClient;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PRLineUDF_RetrieveByPurchaseRequestLineItemId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PRLineUDF_RetrieveByPurchaseRequestLineItemId_V2";
        public usp_PRLineUDF_RetrieveByPurchaseRequestLineItemId_V2()
        {

        }

        public static b_PRLineUDF CallStoredProcedure(
SqlCommand command,
long callerUserInfoId,
string callerUserName,
b_PRLineUDF obj
)
        {
            b_PRLineUDF records = new b_PRLineUDF();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseRequestLineItemId", obj.PurchaseRequestLineItemId);
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            try
            {
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    records = b_PRLineUDF.ProcessRowForRetrieveByPurchaseRequestLineItemId(reader);
                }

            }
            finally
            {
                if (null != reader)
                {
                    if (false == reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
            // Return the result
            return records;
        }
    }
}
