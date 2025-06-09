using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
    public class usp_Menu_RetrieveByPageInfoStatusCount_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Menu_RetrieveByPageInfoStatusCount_V2";
        public usp_Menu_RetrieveByPageInfoStatusCount_V2()
        {
        }
        public static List<b_Menu> CallStoredProcedure(
          SqlCommand command,
          long callerUserInfoId,
          string callerUserName,
          b_Menu obj
      )
        {
            List<b_Menu> records = new List<b_Menu>();
            SqlDataReader reader = null;
            b_Menu record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId",obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Structured, "StatusList", obj.StatusData);
            try
            {
                
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    ////records.Add(record);
                    // results.Add(new Person(reader.GetValue(0).ToString(), reader.GetValue(1).ToString())); 
                    // Process the current row into a record
                    record = (b_Menu)b_Menu.ProcessRowMenuStatus(reader);

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
