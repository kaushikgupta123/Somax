using Database.Business;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_PMSchedAssign_RetrieveByPrevMaintSchedId_V2 stored procedure.
    /// </summary>
    public class usp_PMSchedAssign_RetrieveByPrevMaintSchedId_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PMSchedAssign_RetrieveByPrevMaintSchedId_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PMSchedAssign_RetrieveByPrevMaintSchedId_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_PMSchedAssign_RetrieveByPrevMaintSchedId_V2 stored procedure using SqlClient.
        /// </summary>
        
        public static List<b_PMSchedAssign> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PMSchedAssign obj
        )
        {
            List<b_PMSchedAssign> records = new List<b_PMSchedAssign>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PrevMaintSchedId", obj.PrevMaintSchedId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    records.Add((b_PMSchedAssign)b_PMSchedAssign.ProcessRow(reader));
                }

                reader.NextResult();
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
