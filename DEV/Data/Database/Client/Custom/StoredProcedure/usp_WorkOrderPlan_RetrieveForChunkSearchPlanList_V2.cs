using Database.Business;
using Database.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_WorkOrderPlan_RetrieveForChunkSearchPlanList_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrderPlan_RetrieveForChunkSearchPlanList_V2";

        public usp_WorkOrderPlan_RetrieveForChunkSearchPlanList_V2()
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
            command.SetInputParameter(SqlDbType.BigInt, "WorkOrderPlanId", obj.WorkOrderPlanId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText ", obj.SearchText, 800);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetId", obj.EquipmentClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToName", obj.ChargeTo_Name, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, -1);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RequiredDate", obj.RequireDate, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 15);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
                
                // Loop through dataset.
                while (reader.Read())
                {
                    b_WorkOrderPlan b_WorkOrderPlan = b_WorkOrderPlan.ProcessRowForWorkOrderForWorkOrderPlanRetriveAllForSearch(reader);
                    records.Add(b_WorkOrderPlan);
                }
            }
            catch (Exception ex)
            {

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
