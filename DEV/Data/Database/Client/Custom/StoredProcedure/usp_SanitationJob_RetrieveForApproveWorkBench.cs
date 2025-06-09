using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_SanitationJob_RetrieveForApproveWorkBench
    {
        private static string STOREDPROCEDURE_NAME = "usp_SanitationJob_RetrieveForApproveWorkBench";


        public usp_SanitationJob_RetrieveForApproveWorkBench()
        {
        }

        public static List<b_SanitationJob> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_SanitationJob obj
       )
        {
            List<b_SanitationJob> records = new List<b_SanitationJob>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ApproveCreatedDate", obj.ApproveCreatedDate, 200);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ApproveStatusDrop", obj.ApproveStatusDrop, 200);
            
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_SanitationJob tmpSanitationJob = b_SanitationJob.ProcessRowForSanitationJobApproveWorkBench(reader);
                    tmpSanitationJob.ClientId = obj.ClientId;
                    records.Add(tmpSanitationJob);
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
