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
   public class usp_ServiceOrderLaborHrs_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_ServiceOrderLaborHrs_V2";

        public usp_ServiceOrderLaborHrs_V2()
        {

        }

        /// <summary>
        /// Static method to call the usp_WorkOrderLaborHrs stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<KeyValuePair<string, decimal>> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long ClientId,
            long SiteId,
            int TimeFrame
        )
        {
            List<KeyValuePair<string, decimal>> records = new List<KeyValuePair<string, decimal>>();
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

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                while (reader.Read())
                {
                    string PID = reader.GetString(0);
                    decimal Hrs = reader.GetDecimal(1);                    
                    records.Add(new KeyValuePair<string, decimal>(PID, Hrs));
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
