using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using Common.Constants;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    public class usp_SanitationPlanning_RetrieveByMasterId
    {
        private static string STOREDPROCEDURE_NAME = "usp_SanitationPlanning_RetrieveByMasterId";

        public usp_SanitationPlanning_RetrieveByMasterId()
        {

        }


        
        public static List<b_SanitationPlanning> CallStoredProcedure(
            SqlCommand command,
           Database.SqlClient.ProcessRow<b_SanitationPlanning> processRow,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            long SanitationMasterId,
            string Category
            )
        {
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            List<b_SanitationPlanning> results = new List<b_SanitationPlanning>();
            b_SanitationPlanning result = new b_SanitationPlanning();

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetInputParameter(SqlDbType.BigInt, "SanitationMasterId", SanitationMasterId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Category", Category,15);

            

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    result = processRow(reader);

                    // Add the record to the list.
                    results.Add(result);
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
            //return records;
            return results;
        }

    }
}
