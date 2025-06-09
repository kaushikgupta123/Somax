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
    class usp_Metrics_RetrieveInventoryDetailsByMetricsName_ForAdmin_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Metrics_RetrieveInventoryDetailsByMetricsName_ForAdmin_V2";
        public usp_Metrics_RetrieveInventoryDetailsByMetricsName_ForAdmin_V2()
        {

        }

        /// <summary>
        /// Static method to call the usp_Attachment_RetrieveAll stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>

        public static List<b_Metrics> CallStoredProcedure(
    SqlCommand command,
    long callerUserInfoId,
    string callerUserName,
    b_Metrics obj
    )
        {
            List<b_Metrics> records = new List<b_Metrics>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);

            try
            {

                // Execute stored procedure.
                reader = command.ExecuteReader();

                //// Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_Metrics tmpMetrics = b_Metrics.ProcessRowForRetrieveInventoryDetailsForAdmin(reader);
                    tmpMetrics.ClientId = obj.ClientId;
                    records.Add(tmpMetrics);
                    //records = b_ServiceOrder.ProcessRowForRetrieveByEquipmentId(reader);

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
