using Database.Business;
using Database.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    class usp_ServiceOrder_ServiceOrderHistory_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_ServiceOrder_ServiceOrderHistory_V2";

        public usp_ServiceOrder_ServiceOrderHistory_V2()
        {

        }

        public static List<b_ServiceOrder> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_ServiceOrder obj
       )
        {
            List<b_ServiceOrder> records = new List<b_ServiceOrder>();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "offset1", obj.offset1, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "nextrow", obj.nextrow, 30);
            command.SetInputParameter(SqlDbType.BigInt, "AssetId", obj.EquipmentId);
            command.SetInputParameter(SqlDbType.BigInt, "ServiceOrderId", obj.ServiceOrderId);

            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetClientLookupId", obj.EquipmentClientLookupId, 62);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetName", obj.AssetName, 126);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 30);
            if (obj.CreateDate == default(DateTime))
                command.SetInputParameter(SqlDbType.Date, "CreatedDate", null);
            else
                command.SetInputParameter(SqlDbType.Date, "CreatedDate", obj.CreateDate);
            command.SetInputParameter(SqlDbType.Date, "CompletedDate", obj.CompleteDate);

            reader = command.ExecuteReader();
            try
            {
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_ServiceOrder tmpServiceOrder = b_ServiceOrder.ProcessRowForServiceOrderHistory(reader);
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
