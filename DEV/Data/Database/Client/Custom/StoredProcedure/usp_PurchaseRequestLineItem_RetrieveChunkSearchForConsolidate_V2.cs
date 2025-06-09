using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PurchaseRequestLineItem_RetrieveChunkSearchForConsolidate_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseRequestLineItem_RetrieveChunkSearchForConsolidate_V2";

        public usp_PurchaseRequestLineItem_RetrieveChunkSearchForConsolidate_V2()
        {
        }

        public static List<b_PurchaseRequestLineItem> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PurchaseRequestLineItem obj

        )
        {
            List<b_PurchaseRequestLineItem> records = new List<b_PurchaseRequestLineItem>();
            SqlDataReader reader = null;
            b_PurchaseRequestLineItem record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseRequestId", obj.PurchaseRequestId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 10);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorClientLookupId", obj.VendorClientLookupId, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorName", obj.VendorName, 30);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    record = new b_PurchaseRequestLineItem();
                    record.LoadFromDatabaseForConsolidate(reader);
                    records.Add(record);
                }
            }
            catch (Exception ex)
            {
                throw;
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

            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }
    }
}
