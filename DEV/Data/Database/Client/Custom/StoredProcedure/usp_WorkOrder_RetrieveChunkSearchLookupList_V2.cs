using Database.Business;
using Database.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_WorkOrder_RetrieveChunkSearchLookupList_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_RetrieveChunkSearchLookupList_V2";

        public usp_WorkOrder_RetrieveChunkSearchLookupList_V2()
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

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 31);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeTo_Name", obj.ChargeTo_Name, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "WorkAssigned_Name", obj.WorkAssigned_Name, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Requestor_Name", obj.Requestor_Name, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 256);
            command.SetInputParameter(SqlDbType.Int, "Page", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.VarChar, "orderbyColumn", obj.OrderbyColumn, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "orderBy", obj.OrderBy, 256);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    b_WorkOrder = b_WorkOrder.ProcessRowForChunkSearchLookupList(reader);
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
