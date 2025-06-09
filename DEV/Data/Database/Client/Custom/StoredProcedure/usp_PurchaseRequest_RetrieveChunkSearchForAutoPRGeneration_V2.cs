using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PurchaseRequest_RetrieveChunkSearchForAutoPRGeneration_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseRequest_RetrieveChunkSearchForAutoPRGeneration_V2";

        public usp_PurchaseRequest_RetrieveChunkSearchForAutoPRGeneration_V2()
        {
        }

        public static List<b_PurchaseRequest> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_PurchaseRequest obj
       )
        {
            List<b_PurchaseRequest> records = new List<b_PurchaseRequest>();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "offset1", obj.offset1, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "nextrow", obj.nextrow, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorIDList", obj.VendorIDList.TrimStart(','), 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PartClientLookupId", obj.PartClientLookupId, 70);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UnitofMeasure", obj.UnitofMeasure, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorClientLookupId", obj.VendorClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorName", obj.VendorName, 63);
            command.SetInputParameter(SqlDbType.Decimal, "QtyToOrder", obj.QtyToOrder);
            command.SetInputParameter(SqlDbType.Decimal, "LastPurchaseCost", obj.LastPurchaseCost);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_PurchaseRequest tmpPurchaseRequest = b_PurchaseRequest.ProcessRowChunkSearchForAutoPRGeneration(reader);
                    records.Add(tmpPurchaseRequest);
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
