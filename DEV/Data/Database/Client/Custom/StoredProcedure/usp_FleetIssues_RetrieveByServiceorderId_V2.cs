using Database.Business;
using Database.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_FleetIssues_RetrieveByServiceorderId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_FleetIssues_RetrieveByServiceorderId_V2";

        public usp_FleetIssues_RetrieveByServiceorderId_V2()
        {

        }


        public static List<b_FleetIssues> CallStoredProcedure(
          SqlCommand command,          
          long callerUserInfoId,
          string callerUserName,
          b_FleetIssues obj
      )
        {
            List<b_FleetIssues> records = new List<b_FleetIssues>();
            SqlDataReader reader = null;
            b_FleetIssues record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "Serviceorderid", obj.ServiceOrderId);
            

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    
                    record = b_FleetIssues.ProcessRetrieveByServiceOrderIdV2(reader); 

                    //// Add the record to the list.
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
