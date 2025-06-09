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
   public class usp_Meter_RetrieveBySearchBySiteAndReadingDate
    {
        private static string STOREDPROCEDURE_NAME = "usp_Meter_RetrieveBySearchBySiteAndReadingDate";

        public usp_Meter_RetrieveBySearchBySiteAndReadingDate()
        {
        }

        public static List<b_Meter> CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_Meter obj
        )
        {
            List<b_Meter> records = new List<b_Meter>();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "DateRange", obj.DateRange, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DateColumn", obj.DateColumn, 30);

            try
            {


                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_Meter tmpParts = b_Meter.ProcessRowForSearchBySiteAndReadingDate(reader);
                    tmpParts.ClientId = obj.ClientId;
                    records.Add(tmpParts);
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
