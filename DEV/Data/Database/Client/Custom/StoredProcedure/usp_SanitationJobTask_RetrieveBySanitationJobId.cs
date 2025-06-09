using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_SanitationJobTask_RetrieveBySanitationJobId
    {

        private static string STOREDPROCEDURE_NAME = "usp_SanitationJobTask_RetrieveBySanitationJobId";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_SanitationJobTask_RetrieveBySanitationJobId()
        {
        }

        /// <summary>
        /// 
        /// </summary>

        public static List<b_SanitationJobTask> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_SanitationJobTask obj
        )
        {
            List<b_SanitationJobTask> records = new List<b_SanitationJobTask>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SanitationJobId", obj.SanitationJobId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_SanitationJobTask)b_SanitationJobTask.SanitationJobTaskProcessRow(reader));
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
