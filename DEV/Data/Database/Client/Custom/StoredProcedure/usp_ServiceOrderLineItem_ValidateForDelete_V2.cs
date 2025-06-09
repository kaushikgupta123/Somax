using Database.Business;
using Database.SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    class usp_ServiceOrderLineItem_ValidateForDelete_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_ServiceOrderLineItem_ValidateForDelete_V2";

        public usp_ServiceOrderLineItem_ValidateForDelete_V2()
        {

        }

        public static b_ServiceOrderLineItem CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_ServiceOrderLineItem obj
       )
        {
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "ServiceOrderId", obj.ServiceOrderId);
            command.SetInputParameter(SqlDbType.BigInt, "ServiceOrderLineItemId", obj.ServiceOrderLineItemId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    int i = 0;
                    obj.DeleteAllowed = reader.GetString(i++) == "true" ? true : false;
                    obj.LabourExists = reader.GetString(i++) == "true" ? true : false;
                    obj.PartExists = reader.GetString(i++) == "true" ? true : false;
                    obj.OthersExists = reader.GetString(i++) == "true" ? true : false;
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
            return obj;
        }
    }
}
