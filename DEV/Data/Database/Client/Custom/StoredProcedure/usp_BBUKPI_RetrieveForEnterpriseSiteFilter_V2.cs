using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
    public class usp_BBUKPI_RetrieveForEnterpriseSiteFilter_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_BBUKPI_RetrieveForEnterpriseSiteFilter_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_BBUKPI_RetrieveForEnterpriseSiteFilter_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_Site_RetrieveAuthorizedForUser stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">integer that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the datebase</param>
        /// <param name="clientId">long that contains the user's client id</param>
        /// <returns>Array of business objects containing the results of the query</returns>
        public static List<b_BBUKPI> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_BBUKPI obj)
        {
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;


            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);

            // Setup the return object
            List<b_BBUKPI> result = new List<b_BBUKPI>();

            try
            {

                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Retrieve List
                while (reader.Read())
                {
                    // Add the record to the list.
                    result.Add((b_BBUKPI)b_BBUKPI.ProcessRetrieveForEnterpriseSiteFilter(reader));
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
            return result;
        }
    }
}
