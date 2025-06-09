using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PartHistory_RetrieveForReview
    {
        private static string STOREDPROCEDURE_NAME = "usp_PartHistory_RetrieveForReview";

        public usp_PartHistory_RetrieveForReview()
        {

        }


        public static List<b_PartHistoryReview> CallStoredProcedure(
          SqlCommand command,
         // Database.SqlClient.ProcessRow<b_PartHistoryReview> processRow,
          long callerUserInfoId,
          string callerUserName,
          b_PartHistoryReview obj    
         // out int ResultCount
      )
        {
            List<b_PartHistoryReview> records = new List<b_PartHistoryReview>();
            SqlDataReader reader = null;
            b_PartHistoryReview record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

          //  ResultCount = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
           // command.SetStringInputParameter(SqlDbType.NVarChar, "TranType_List", obj.TransactionType, 256);
            //command.SetInputParameter(SqlDbType.Bit, "Receipts", obj.Receipts);
            //command.SetInputParameter(SqlDbType.Bit, "Reversals", obj.Reversals);
            command.SetStringInputParameter(SqlDbType.Char, "DateRange", obj.DateRange, 256);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

               
                // Loop through dataset.
                while (reader.Read())
                {
                  
                    records.Add((b_PartHistoryReview)b_PartHistoryReview.ProcessRow(reader));
                     
                }
                 reader.NextResult();

                while (reader.Read())
                {
                    // Add the record to the list.
                    //records.Add(reader.GetString(0));
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
