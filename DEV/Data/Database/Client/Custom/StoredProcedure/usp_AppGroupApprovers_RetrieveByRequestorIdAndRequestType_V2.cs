using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
    public class usp_AppGroupApprovers_RetrieveByRequestorIdAndRequestType_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_AppGroupApprovers_RetrieveByRequestorIdAndRequestType_V2";
        public usp_AppGroupApprovers_RetrieveByRequestorIdAndRequestType_V2()
        {
        }

        public static List<b_AppGroupApprovers> CallStoredProcedure(SqlCommand command, long callerUserInfoId, string callerUserName, b_AppGroupApprovers obj)
        {
            List<b_AppGroupApprovers> records = new List<b_AppGroupApprovers>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;
            // Setup command
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "RequestorId", obj.RequestorId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RequestType", obj.RequestType, 15);
            command.SetInputParameter(SqlDbType.Int, "Level", obj.Level);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    //// Add the record to the list.
                    records.Add(b_AppGroupApprovers.ProcessRowForFilterApprover(reader));
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
