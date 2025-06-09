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
    public class usp_WorkOrderSource
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrderSource";

        public usp_WorkOrderSource()
        {

        }
        /// <summary>
        /// Static method to call the usp_WorkOrder_RetrieveByWOStatusForChart_1 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        /// <summary>
        /// Static method to call the usp_Equipment_RetrieveAll stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_WorkOrder> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long ClientId,
            long SiteId,
            int TimeFrame
        )
        {
            b_WorkOrder wo;
            List<b_WorkOrder> Lwo= new List<b_WorkOrder>();

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

                // Get the result count
                while (reader.Read())
                {
                    wo = new b_WorkOrder();
                    wo.SourceType = reader.GetString(0);
                    wo.Month = reader.GetInt32(1);
                    wo.WOCount = reader.GetInt32(2);
                    Lwo.Add(wo);
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
            return Lwo;
        }
    }
}
