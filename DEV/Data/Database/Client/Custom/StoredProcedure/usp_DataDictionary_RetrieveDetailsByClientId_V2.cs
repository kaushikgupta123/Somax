using Database.Business;
using Database.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_DataDictionary_RetrieveDetailsByClientId_V2
    { /// <summary>
      /// Constants.
      /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_DataDictionary_RetrieveDetailsByClientId_V2";
        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_DataDictionary_RetrieveDetailsByClientId_V2()
        {
        }
        /// <summary>
        /// Static method to call the usp_DataDictionary_RetrieveDetailsByClientId_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_DataDictionary> CallStoredProcedure(
          SqlCommand command,
          long callerUserInfoId,
          string callerUserName,
          b_DataDictionary obj)
        {
            List<b_DataDictionary> records = new List<b_DataDictionary>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "UiViewId", obj.UiViewId);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_DataDictionary)b_DataDictionary.ProcessRowByRetrieveDetailsByClientId(reader));
                }

                reader.NextResult();

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
