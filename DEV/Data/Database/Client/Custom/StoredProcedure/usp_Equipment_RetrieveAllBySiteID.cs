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
    public class usp_Equipment_RetrieveAllBySiteID
    {
        private static string STOREDPROCEDURE_NAME = "usp_Equipment_RetrieveAllBySiteID";

        public usp_Equipment_RetrieveAllBySiteID()
        {

        }


        public static List<b_Equipment> CallStoredProcedure(
          SqlCommand command,
          SqlClient.ProcessRow<b_Equipment> processRow,
          long callerUserInfoId,
          string callerUserName,
          long clientId,
          long siteId,
          out int ResultCount
      )
        {
            List<b_Equipment> records = new List<b_Equipment>();
            SqlDataReader reader = null;
            b_Equipment record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            ResultCount = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", siteId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                //while (reader.Read())
                //{

                //}

                //reader.NextResult();

                // Loop through dataset.
                while (reader.Read())
                {
                    ResultCount = ResultCount + 1;

                    // Process the current row into a record
                    record = processRow(reader);

                    // Add the record to the list.
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
