using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database.Business;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    public class usp_Meter_RetrieveByClientLookUpId
    {
         /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Meter_RetrieveByClientLookUpId";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Meter_RetrieveByClientLookUpId()
        {
        }

        public static b_Meter CallStoredProcedure(
            SqlCommand command,        
            long callerUserInfoId,
            string callerUserName,
            b_Meter obj
        )
        {
            b_Meter records = new b_Meter();
            SqlDataReader reader = null;
          //  b_Part record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "MeterClientLookupId", obj.ClientLookupId,31);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                   // record = processRow(reader);

                    // Add the record to the list.
                    records = b_Meter.ProcessRowForRetrieveByClientLookUpId(reader);
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
