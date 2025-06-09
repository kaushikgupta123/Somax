using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.SqlClient;
using Database.Business;
using System.Collections.Generic;
using System;

namespace Database.StoredProcedure
{
    public class usp_WOPlanLineItem_InCompleteWorkorderByType_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_WOPlanLineItem_InCompleteWorkorderByType_V2";

        public usp_WOPlanLineItem_InCompleteWorkorderByType_V2()
        {
        }

        public static List<b_WorkOrderPlan> CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_WorkOrderPlan obj
    )
        {
            List<b_WorkOrderPlan> records = new List<b_WorkOrderPlan>();

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
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "WorkOrderPlanId", obj.WorkOrderPlanId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                while (reader.Read())
                {
                    b_WorkOrderPlan b_WorkOrderPlan = b_WorkOrderPlan.ProcessRowForIncompleteWorkorderByType(reader);
                    records.Add(b_WorkOrderPlan);
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
