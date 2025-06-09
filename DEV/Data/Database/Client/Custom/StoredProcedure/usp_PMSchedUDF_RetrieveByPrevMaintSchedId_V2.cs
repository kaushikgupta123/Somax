using Database.Business;
using Database.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace Database.StoredProcedure
{
   
    public class usp_PMSchedUDF_RetrieveByPrevMaintSchedId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PMSchedUDF_RetrieveByPrevMaintSchedId_V2";
        public usp_PMSchedUDF_RetrieveByPrevMaintSchedId_V2()
        {

        }


        public static b_PMSchedUDF CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PMSchedUDF obj
        )
        {
            b_PMSchedUDF records = new b_PMSchedUDF();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PrevMaintSchedId", obj.PrevMaintSchedId);
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
          

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
               
                while (reader.Read())
                {

                    records = b_PMSchedUDF.ProcessRowByPrevMaintSchedId(reader);
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
