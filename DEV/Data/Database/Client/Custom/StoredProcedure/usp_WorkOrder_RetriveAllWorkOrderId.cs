using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    //public class usp_WorkOrder_RetriveAllWorkOrderId
    //{
    //    private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_RetrieveAllWorkOrderId_V2";

    //    public usp_WorkOrder_RetriveAllWorkOrderId()
    //    {
    //    }

    //    public static List<b_WorkOrder> CallStoredProcedure(
    //   SqlCommand command,
    //   long callerUserInfoId,
    //   string callerUserName,
    //   b_WorkOrder obj
    //   )
    //    {
    //        List<b_WorkOrder> records = new List<b_WorkOrder>();
    //        SqlDataReader reader = null;
    //        SqlParameter RETURN_CODE_parameter = null;
    //        int retCode = 0;

    //        // Setup command.
    //        command.SetProcName(STOREDPROCEDURE_NAME);
    //        RETURN_CODE_parameter = command.GetReturnCodeParameter();
    //        command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
    //        command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
    //        command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
    //        command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);

    //        try
    //        {
    //            // Execute stored procedure.
    //            reader = command.ExecuteReader();

    //            // Loop through dataset.
    //            while (reader.Read())
    //            {
    //                // Add the record to the list.0
    //                b_WorkOrder tmpWorkOrders = b_WorkOrder.ProcessRowForRetrieveAllWorkOrderId(reader);
    //                tmpWorkOrders.ClientId = obj.ClientId;
    //                records.Add(tmpWorkOrders);
    //            }

    //        }
    //        finally
    //        {
    //            if (null != reader)
    //            {
    //                if (false == reader.IsClosed)
    //                {
    //                    reader.Close();
    //                }
    //                reader = null;
    //            }
    //        }

    //        // Process the RETURN_CODE parameter value
    //        retCode = (int)RETURN_CODE_parameter.Value;
    //        AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);


    //        // Return the result
    //        return records;
    //    }

    //}
}
