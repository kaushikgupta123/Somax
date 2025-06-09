using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_ResetMeter_ValidateProcess
    {
        private static string STOREDPROCEDURE_NAME = "usp_ResetMeter_ValidateProcess";
        public usp_ResetMeter_ValidateProcess()
        {

        }
        public static List<b_StoredProcValidationError> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_MeterReading obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;
            SqlDataReader reader = null;
            List<b_StoredProcValidationError> records = new List<b_StoredProcValidationError>();
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "MeterId", obj.MeterId);    //obj.MeterId  SOM:928  
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.Decimal, "Reading", obj.Reading);
            command.SetInputParameter(SqlDbType.DateTime, "ReadingDate", obj.ReadingDate);
            

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_StoredProcValidationError)b_StoredProcValidationError.ProcessRow(reader));
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
