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
    class usp_WorkOrder_WorkOrderPlanLookupListBySearchCriteria_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_WorkOrderPlanLookupListBySearchCriteria_V2";

        public usp_WorkOrder_WorkOrderPlanLookupListBySearchCriteria_V2()
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
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Int, "Page", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderColumn", obj.OrderbyColumn, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderDirection", obj.OrderBy, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.WorkOrderClientLookupId, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeTo", obj.ChargeTo, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeTo_Name", obj.ChargeTo_Name,63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, -1);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Priority", obj.Priority, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 15);

            try
            {

                // Execute stored procedure.
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_WorkOrderPlan tmpServiceOrder = b_WorkOrderPlan.ProcessRowForWorkOrder_WorkOrderPlanLookupListBySearchCriteria(reader);
                    tmpServiceOrder.ClientId = obj.ClientId;
                    records.Add(tmpServiceOrder);
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
