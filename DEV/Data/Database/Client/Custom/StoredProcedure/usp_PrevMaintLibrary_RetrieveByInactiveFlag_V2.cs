using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
    //*** V2-694
    class usp_PrevMaintLibrary_RetrieveByInactiveFlag
    {
        private static string STOREDPROCEDURE_NAME = "usp_PrevMaintLibrary_RetrieveByInactiveFlag_V2";
        public usp_PrevMaintLibrary_RetrieveByInactiveFlag()
        {
        }
        public static ArrayList CallStoredProcedure(
                   SqlCommand command,
                   Database.SqlClient.ProcessRow<b_PrevMaintLibrary> processRow,
                   long callerUserInfoId,
                   string callerUserName,                 
                   //bool InactiveFlag
                   int InactiveFlg,
                   long clientId
               )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_PrevMaintLibrary record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.Int, "InactiveFlag", InactiveFlg);
            // Setup clientId parameter.
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
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