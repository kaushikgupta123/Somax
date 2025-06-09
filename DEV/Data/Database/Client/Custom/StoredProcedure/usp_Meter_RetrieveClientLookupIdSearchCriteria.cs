using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    public class usp_Meter_RetrieveClientLookupIdSearchCriteria
    {
        private static string STOREDPROCEDURE_NAME = "usp_Meter_RetrieveClientLookupIdBySearchCriteria";
        public usp_Meter_RetrieveClientLookupIdSearchCriteria()
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 31);

            try
            {


                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    b_Meter tmpEq = b_Meter.ProcessRowForClientIdLookup(reader);
                    tmpEq.ClientId = obj.ClientId;
                    records.Add(tmpEq);
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
