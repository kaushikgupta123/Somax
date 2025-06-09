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
   public class usp_POReceipt_RetrievePOFromParts
    {
       private static string STOREDPROCEDURE_NAME = "usp_POReceipt_RetrievePOFromParts";

       public usp_POReceipt_RetrievePOFromParts()
        {
        }

       public static List<b_POReceiptItem> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_POReceiptItem obj
       )
        {
            List<b_POReceiptItem> records = new List<b_POReceiptItem>();
            SqlDataReader reader = null;
            b_POReceiptItem record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetStringInputParameter(SqlDbType.Char, "DateRange", obj.DateRange, 15);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    record = new b_POReceiptItem();
                    record = b_POReceiptItem.ProcessRowForRetrievePOFromParts(reader);
                    record.ClientId = obj.ClientId;
                    records.Add(record);
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
