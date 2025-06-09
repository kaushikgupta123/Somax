using Common.Constants;
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
    public class usp_Equipment_RetrieveInitialSearchConfigurationData
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Equipment_RetrieveInitialSearchConfigurationData";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Equipment_RetrieveInitialSearchConfigurationData()
        {
        }

        /// <summary>
        /// Static method to call the usp_UserData_RetrieveByUserName stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerId">string that identifies the user calling the database</param>
        /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
        /// <param name="sessionId">System.Guid that contains the value of the @SessionId parameter</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static Dictionary<string, List<KeyValuePair<string, string>>> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long clientId
            )
        {
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;


            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);

            Dictionary<string, List<KeyValuePair<string, string>>> result = new Dictionary<string, List<KeyValuePair<string, string>>>();

            try
            {
                string[] categories = new string[] { SearchCategoryConstants.P_QUERY, SearchCategoryConstants.P_SITE, SearchCategoryConstants.P_AREA,
                    SearchCategoryConstants.P_DEPARTMENT, SearchCategoryConstants.P_TYPE, SearchCategoryConstants.P_STATUS };

                // Execute stored procedure.
                reader = command.ExecuteReader();

                for (int i = 0; i <= categories.GetUpperBound(0); i++)
                {
                    result.Add(categories[i], new List<KeyValuePair<string, string>>());

                    while (reader.Read())
                    {
                        result[categories[i]].Add(new KeyValuePair<string, string>(reader.GetString(0), reader.GetString(1)));
                    }
                    if (i != categories.GetUpperBound(0))
                    {
                        reader.NextResult();
                    }
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
