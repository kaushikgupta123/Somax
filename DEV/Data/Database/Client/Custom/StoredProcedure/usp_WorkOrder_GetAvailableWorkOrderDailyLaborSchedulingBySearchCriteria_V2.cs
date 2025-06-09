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
    class usp_WorkOrder_GetAvailableWorkOrderDailyLaborSchedulingBySearchCriteria_V2
    {
         private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_GetAvailableWorkOrderDailyLaborSchedulingBySearchCriteria_V2";

        public usp_WorkOrder_GetAvailableWorkOrderDailyLaborSchedulingBySearchCriteria_V2()
        {
        }

        public static List<b_WorkOrder> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_WorkOrder obj
   )
        {
            List<b_WorkOrder> records = new List<b_WorkOrder>();
            b_WorkOrder b_WorkOrder = new b_WorkOrder();
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;
            int flag = (obj.ScheduleFlag == null || obj.ScheduleFlag == "") ? 0 : Convert.ToInt32(obj.ScheduleFlag);
            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
	 
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Int, "Flag", flag);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeTo", obj.ChargeTo, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToName", obj.ChargeTo_Name, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Priority", obj.Priority, 500);
            command.SetInputParameter(SqlDbType.Bit, "DownRequired", obj.downRequired);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Assigned", obj.Assigned, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "StartDate", obj.ScheduledDateStart, 500);
            command.SetInputParameter(SqlDbType.Decimal, "Duration", obj.ScheduledHours);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RequiredDate", obj.RequireDate, 50);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    b_WorkOrder = b_WorkOrder.ProcessRowForAvailableWorkDailyLaborSchedulingBySearchV2(reader);
                    records.Add(b_WorkOrder);
                }
            }
            catch (Exception ex)
            { }
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
