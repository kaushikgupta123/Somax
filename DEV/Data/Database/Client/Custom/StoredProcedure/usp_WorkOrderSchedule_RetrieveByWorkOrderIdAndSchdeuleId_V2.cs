using Database.Business;
using Database.SqlClient;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_WorkOrderSchedule_RetrieveByWorkOrderIdAndSchdeuleId_V2 stored procedure.
    /// </summary>
    public class usp_WorkOrderSchedule_RetrieveByWorkOrderIdAndSchdeuleId_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrderSchedule_RetrieveByWorkOrderIdAndSchdeuleId_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WorkOrderSchedule_RetrieveByWorkOrderIdAndSchdeuleId_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_WorkOrderSchedule_RetrieveByPK stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static b_WorkOrderSchedule CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_WorkOrderSchedule obj
        )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_WorkOrderSchedule record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "WorkOrderId", obj.WorkOrderId);
            command.SetInputParameter(SqlDbType.BigInt, "WorkOrderSchedId", obj.WorkOrderSchedId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    record = (b_WorkOrderSchedule)b_WorkOrderSchedule.ProcessRowForRetrieveWorkOrderIdAndSchdeuleId(reader);
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
            return record;
        }
    }
}

