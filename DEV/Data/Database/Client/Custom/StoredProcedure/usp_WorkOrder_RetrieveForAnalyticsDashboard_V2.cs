using Database.Business;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_WorkOrder_RetrieveForAnalyticsDashboard_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_RetrieveForAnalyticsDashboard_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WorkOrder_RetrieveForAnalyticsDashboard_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_WorkOrder_RetrieveForAnalyticsDashboard_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>b_WorkOrder containing the results of the query</returns>
        public static b_WorkOrder CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_WorkOrder obj
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
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            try
            {

                List<b_WorkOrder> records = new List<b_WorkOrder>();
                // Execute stored procedure.               
                reader = command.ExecuteReader();
                obj.listOfWO = new List<b_WorkOrder>();
                while (reader.Read())
                {
                    b_WorkOrder tmpWorkOrders = b_WorkOrder.ProcessAnalyticsWOStatusDashboardV2(reader);
                    obj.listOfWO.Add(tmpWorkOrders);
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
            return obj;
        }
    }
}
