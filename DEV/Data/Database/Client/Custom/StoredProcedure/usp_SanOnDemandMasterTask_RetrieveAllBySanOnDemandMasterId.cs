using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{   
    public class usp_SanOnDemandMasterTask_RetrieveAllBySanOnDemandMasterId
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_SanOnDemandMasterTask_RetrieveAllBySanOnDemandMasterId";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_SanOnDemandMasterTask_RetrieveAllBySanOnDemandMasterId()
        {
        }

        public static List<b_SanOnDemandMasterTask> CallStoredProcedure(
             SqlCommand command,
             long callerUserInfoId,
             string callerUserName,
             b_SanOnDemandMasterTask obj
         )
        {
            List<b_SanOnDemandMasterTask> records = new List<b_SanOnDemandMasterTask>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SanOnDemandMasterId", obj.SanOnDemandMasterId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_SanOnDemandMasterTask)b_SanOnDemandMasterTask.ProcessRow(reader));
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
