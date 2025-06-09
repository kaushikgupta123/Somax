using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_LoginData_RetrieveByUserName_V2 stored procedure.
    /// </summary>
    public class usp_LoginData_RetrieveByUserName_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_LoginData_RetrieveByUserName_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_LoginData_RetrieveByUserName_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_LoginData_RetrieveByUserName_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
        /// <param name="sessionId">System.Guid that contains the value of the @SessionId parameter</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<Object> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_LoginDataSet obj
        )
        {
            List<Object> records = new List<Object>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UserName", obj.UserName, 63);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                Object tempRecord = null;

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    tempRecord = b_Client.ProcessRowForAdmin_V2(reader);
                }

                records.Add(tempRecord);

                reader.NextResult();

                tempRecord = null;

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    tempRecord = b_LoginInfo.ProcessRow(reader);
                }

                records.Add(tempRecord);

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