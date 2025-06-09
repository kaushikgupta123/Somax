using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PurchaseOrders_by_Account
    {
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseOrders_by_Account";

        public usp_PurchaseOrders_by_Account()
        {
        }

        public static List<b_PurchaseOrder> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_PurchaseOrder obj
       )
        {
            List<b_PurchaseOrder> records = new List<b_PurchaseOrder>();
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
            command.SetInputParameter(SqlDbType.DateTime, "stDate", obj.StartDate);
            command.SetInputParameter(SqlDbType.DateTime, "fnDate", obj.EndDate);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_PurchaseOrder tmpPurchaseOrder = b_PurchaseOrder.ProcessRowForPOByAccountReport(reader);
                    tmpPurchaseOrder.ClientId = obj.ClientId;
                    records.Add(tmpPurchaseOrder);
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
