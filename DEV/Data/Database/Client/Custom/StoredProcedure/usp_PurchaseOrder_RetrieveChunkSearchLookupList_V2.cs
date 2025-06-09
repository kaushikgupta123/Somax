using Database.Business;
using Database.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PurchaseOrder_RetrieveChunkSearchLookupList_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseOrder_RetrieveChunkSearchLookupList_V2";
        public usp_PurchaseOrder_RetrieveChunkSearchLookupList_V2()
        {

        }

        public static List<b_PurchaseOrder> CallStoredProcedure(SqlCommand command, long callerUserInfoId,
                                         string callerUserName, b_PurchaseOrder obj)
        {
            List<b_PurchaseOrder> records = new List<b_PurchaseOrder>();
            b_PurchaseOrder b_PurchaseOrder = new b_PurchaseOrder();
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "POClientLookupId", obj.ClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorClientLookupId", obj.VendorClientLookupId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorName", obj.VendorName, 63);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Int, "Page", obj.offset1);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", obj.nextrow);
            command.SetStringInputParameter(SqlDbType.VarChar, "OrderColumn", obj.orderbyColumn, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "OrderDirection", obj.orderBy, 256);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    b_PurchaseOrder = b_PurchaseOrder.ProcessRowForChunkSearchLookupList(reader);
                    records.Add(b_PurchaseOrder);
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