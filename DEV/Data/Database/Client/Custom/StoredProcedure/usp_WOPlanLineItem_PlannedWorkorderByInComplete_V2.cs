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
    public class usp_WOPlanLineItem_PlannedWorkorderByInComplete_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WOPlanLineItem_PlannedWorkorderByInComplete_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WOPlanLineItem_PlannedWorkorderByInComplete_V2()
        {
        }

        public static long CallStoredProcedure(
       SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
          long ClientId,
         long SiteId,
         long WorkOrderPlanId
    )
        {
            long result = 0;
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
            command.SetInputParameter(SqlDbType.BigInt, "WorkOrderPlanId", WorkOrderPlanId);


            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                while (reader.Read())
                {
                    // string seriesname = reader.GetString(1);
                    result = (long)reader.GetInt32(0);                 
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
            return result;

        }
    }
}
