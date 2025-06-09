using Database.Business;
using Database.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_AppGroupApprovers_ValidateUpperLevelExists_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_AppGroupApprovers_ValidateUpperLevelExists_V2";

        /// <summary>
        /// 
        /// </summary>
        public usp_AppGroupApprovers_ValidateUpperLevelExists_V2()
        {
        }


        public static List<b_StoredProcValidationError> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_AppGroupApprovers obj
        )
        {
            List<b_StoredProcValidationError> records = new List<b_StoredProcValidationError>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "AppGroupApproversId", obj.AppGroupApproversId);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_StoredProcValidationError)b_StoredProcValidationError.ProcessRow(reader));
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
