using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_AdminLoginInfo_RetrieveBySearchCriteria_V2 stored procedure.
    /// </summary>
    public class usp_AdminLoginInfo_RetrieveBySearchCriteria_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_AdminLoginInfo_RetrieveBySearchCriteria_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_AdminLoginInfo_RetrieveBySearchCriteria_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_AdminLoginInfo_RetrieveBySearchCriteria_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
		/// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static ArrayList CallStoredProcedure(
            SqlCommand command,
            ProcessRow<b_LoginInfo> processRow,
            long callerUserInfoId,
            string callerUserName,
            string userName,
            string email
        )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_LoginInfo record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            try
            {
                // Setup command.
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = STOREDPROCEDURE_NAME;
                command.Parameters.Clear();

                // Setup RETURN_CODE parameter.
                RETURN_CODE_parameter = command.Parameters.Add("RETURN_CODE", SqlDbType.Int);
                RETURN_CODE_parameter.Direction = ParameterDirection.ReturnValue;
                RETURN_CODE_parameter.Value = 0;


                command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
                command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
                command.SetStringInputParameter(SqlDbType.NVarChar, "UserName", userName, 64);
                command.SetStringInputParameter(SqlDbType.NVarChar, "Email", userName, 256);

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
                if (0 != retCode)
                {
                    string message = "usp_AdminLoginInfo_RetrieveBySearchCriteria_V2 stored procedure returned invalid return code: " + retCode.ToString();
                    throw new Exception(message);
                }
            }
            finally
            {
            }

            // Return the result
            return records;
        }
    }
}