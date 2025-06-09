using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Database.Business;


namespace Database.StoredProcedure
{
    public class usp_PersonnelList_RetrieveByFilterText
    {
        private static string STOREDPROCEDURE_NAME = "usp_Personnel_RetrieveByFilterText";

        public usp_PersonnelList_RetrieveByFilterText()
        {
        }

        public static List<b_Personnel> CallStoredProcedure(
          SqlCommand command,
          long callerUserInfoId,
          string callerUserName,
          long clientId,
          long siteid,
          string FilterText,
          int startIndex,
          int EndIndex
      )
        {
            List<b_Personnel> records = new List<b_Personnel>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", siteid);
            command.SetStringInputParameter(SqlDbType.NVarChar, "FilterText", FilterText, 100);
            command.SetInputParameter(SqlDbType.Int, "StartIndex", startIndex);
            command.SetInputParameter(SqlDbType.Int, "EndIndex", EndIndex);

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
