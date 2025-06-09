using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Timecard_RetrieveBySanitationJobId
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Timecard_RetrieveBySanitationJobId";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Timecard_RetrieveBySanitationJobId()
        {
        }

      
        public static List<b_Timecard> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Timecard obj
        )
        {
            List<b_Timecard> records = new List<b_Timecard>();
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
            command.SetInputParameter(SqlDbType.BigInt, "SanitationJobId", obj.ChargeToId_Primary);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_Timecard)b_Timecard.ProcessRowByWorkOrderId(reader));
                }

                reader.NextResult();

                while (reader.Read())
                {
                    // Add the record to the list.
                    //records.Add(reader.GetString(0));
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
