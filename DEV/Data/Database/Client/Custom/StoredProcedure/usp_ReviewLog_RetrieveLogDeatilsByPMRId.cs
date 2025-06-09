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
    public class usp_ReviewLog_RetrieveLogDeatilsByPMRId
    {
        private static string STOREDPROCEDURE_NAME = "usp_ReviewLog_RetrieveByTable";

        public usp_ReviewLog_RetrieveLogDeatilsByPMRId()
        {
        }

        public static List<b_ReviewLog> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_ReviewLog obj
       )
        {
            List<b_ReviewLog> records = new List<b_ReviewLog>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TableName", obj.TableName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Function", obj.Function, 15);
            command.SetInputParameter(SqlDbType.BigInt, "ObjectId", obj.ObjectId);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_ReviewLog tmpReviewLog = b_ReviewLog.ProcessRowForRetriveAllForSearch(reader);
                    tmpReviewLog.ClientId = obj.ClientId;
                    records.Add(tmpReviewLog);
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
