using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    // <summary>
    /// Access the usp_WorkOrder_RetriveByTypeChart stored procedure.
    /// </summary>
    public class usp_WorkOrder_RetrieveByTypeChart
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_RetrieveByTypeChartNew";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WorkOrder_RetrieveByTypeChart()
        {
        }

        /// <summary>
        /// Static method to call the usp_WorkOrder_RetriveByTypeChart stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<KeyValuePair<string, long>> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long ClientId,
            long SiteId,
            int TimeFrame,
            int Flag
        )
        {
            List<KeyValuePair<string, long>> records = new List<KeyValuePair<string, long>>();
            SqlDataReader reader = null;
            //b_Account record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", SiteId);
            command.SetInputParameter(SqlDbType.Int, "TimeFrame", TimeFrame);
            command.SetInputParameter(SqlDbType.Int, "Flag", Flag);
            

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    long count = (long)reader.GetInt32(1);
                    records.Add(new KeyValuePair<string, long>(name, count));
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
