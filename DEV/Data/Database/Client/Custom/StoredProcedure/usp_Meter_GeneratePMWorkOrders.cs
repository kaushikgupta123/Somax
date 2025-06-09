using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    public class usp_Meter_GeneratePMWorkOrders
    {
        private static string STOREDPROCEDURE_NAME = "usp_Meter_GeneratePMWorkOrders_V2";
        public usp_Meter_GeneratePMWorkOrders()
        {

        }
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Meter obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter work_order_list_parameter = null;
            SqlParameter work_order_ClientLookup_list_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "MeterId", obj.MeterId);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
            // Setup Work Order List parameter.
            work_order_list_parameter = command.Parameters.Add("@WorkOrderList", SqlDbType.NVarChar,4000);
            work_order_list_parameter.Direction = ParameterDirection.Output;
            // Setup Work Order ClientLookup List parameter.
            work_order_ClientLookup_list_parameter = command.Parameters.Add("@WorkOrderLookupList", SqlDbType.NVarChar, 4000);
            work_order_ClientLookup_list_parameter.Direction = ParameterDirection.Output;

            // Execute stored procedure.
            command.ExecuteNonQuery();            

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            string wo_list = work_order_list_parameter.Value.ToString();
            string woClientLookupId_list = work_order_ClientLookup_list_parameter.Value.ToString();
            if (!string.IsNullOrEmpty(wo_list))
            {
              obj.PMWOList = wo_list.Split(',').Select(long.Parse).ToList();
            }
            //V2-784 Retreive Work Order ClientLookup List
            if (!string.IsNullOrEmpty(woClientLookupId_list))
            {
                obj.PMWOClientLookupIds = woClientLookupId_list;
            }
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
