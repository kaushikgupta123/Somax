using Database.Business;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Personnel_RetrieveForLookupListByMultipleSecurityItem_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Personnel_RetrieveForLookupListByMultipleSecurityItem_V2";

        public usp_Personnel_RetrieveForLookupListByMultipleSecurityItem_V2()
        {
        }

        public static List<b_Personnel> CallStoredProcedure(SqlCommand command, long callerUserInfoId, string callerUserName, b_Personnel obj)
        {
            List<b_Personnel> records = new List<b_Personnel>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;
            // Setup command
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Int, "Buyer", obj.Buyer);
            
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    //// Add the record to the list.
                    records.Add(b_Personnel.ProcessRowForFilterPersonnel(reader));
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
