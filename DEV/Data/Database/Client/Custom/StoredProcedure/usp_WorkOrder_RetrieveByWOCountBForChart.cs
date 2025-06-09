using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
    public class usp_WorkOrder_RetrieveByWOCountBForChart
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_RetrieveByWOCountBForChart";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WorkOrder_RetrieveByWOCountBForChart()
        {
        }

        /// <summary>
        /// Static method to call the usp_Equipment_RetrieveAll stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<KeyValuePair<string, List<b_WorkOrder>>> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long ClientId,
            long SiteId,
            int TimeFrame
        )
        {
            List<KeyValuePair<string, long>> records = new List<KeyValuePair<string, long>>();
            List<KeyValuePair<string, List<b_WorkOrder>>> records1 = new List<KeyValuePair<string, List<b_WorkOrder>>>();
            b_WorkOrder wo;
            List<b_WorkOrder> Lwo;

            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            // Setup command.
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
                string series = string.Empty;

                var moreResults = true;
                while (moreResults)
                {
                    Lwo = new List<b_WorkOrder>();
                    // Get the result count
                    while (reader.Read())
                    {
                        wo = new b_WorkOrder();
                        series = reader.GetString(0);
                        if (series == "Series1")
                        {
                            wo.WOCountAsType = (long)reader.GetInt32(1);
                            wo.SourceType = reader.GetString(2);
                            Lwo.Add(wo);
                        }
                        else
                        {
                            wo.WOCountAsStatus= (long)reader.GetInt32(1);
                            wo.SourceType = reader.GetString(2);
                            Lwo.Add(wo);
                        }
                       
                        long count = (long)reader.GetInt32(1);
                        
                        records.Add(new KeyValuePair<string, long>(series, count));
                        
                    }
                    records1.Add(new KeyValuePair<string, List<b_WorkOrder>>(series, Lwo));
                    moreResults = reader.NextResult();
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
            return records1;
        }
    }
}
