/*
 *  Added By Indusnet Technologies
 * 
 */

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PurchaseRequest_RetrieveForInformation
    {
       private static string STOREDPROCEDURE_NAME = "usp_PurchaseRequest_RetrieveForInformation";

        public usp_PurchaseRequest_RetrieveForInformation()
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
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseOrderId", obj.PurchaseOrderId);
          
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_PurchaseRequest tmpPurchaseRequest = (b_PurchaseRequest)b_PurchaseRequest.ProcessRowForPurchaseRequestRetrieveForInformation(reader);
                    tmpPurchaseRequest.ClientId = obj.ClientId;                    
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
