using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Common.Constants;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    public class usp_MeterReading_RetrieveInitialSearchConfigurationData
    {
        private static string STOREDPROCEDURE_NAME = "usp_MeterReading_RetrieveInitialSearchConfigurationData";
        public usp_MeterReading_RetrieveInitialSearchConfigurationData()
        {

        }
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
                string[] categories = new string[] { SearchCategoryConstants.P_QUERY, SearchCategoryConstants.P_SITE
                      };

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
