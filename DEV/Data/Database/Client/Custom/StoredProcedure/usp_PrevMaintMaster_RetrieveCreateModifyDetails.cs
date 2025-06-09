using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PrevMaintMaster_RetrieveCreateModifyDetails
    {
        private static string STOREDPROCEDURE_NAME = "usp_PrevMaintMaster_RetrieveCreateModifyDetails";

        public usp_PrevMaintMaster_RetrieveCreateModifyDetails()
        {

        }
        public static ArrayList CallStoredProcedure(
           SqlCommand command,
           Database.SqlClient.ProcessRow<b_PrevMaintMaster> processRow,
           long callerUserInfoId,
           string callerUserName,
           b_PrevMaintMaster obj
       )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_PrevMaintMaster record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PrevMaintMasterId", obj.PrevMaintMasterId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = processRow(reader);

                    // Add the record to the list.
                    records.Add(record);
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
