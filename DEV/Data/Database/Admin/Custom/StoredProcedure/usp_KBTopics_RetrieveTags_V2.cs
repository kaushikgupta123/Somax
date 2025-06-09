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
    class usp_KBTopics_RetrieveTags_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_KBTopics_RetrieveTags_V2";
        public usp_KBTopics_RetrieveTags_V2()
        {
                
        }

        public static List<List<b_KBTopics>> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_KBTopics obj
       )
        {
            List<b_KBTopics> AllRecords = new List<b_KBTopics>();
            List<b_KBTopics> ScheduledRecords = new List<b_KBTopics>();

            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    AllRecords.Add((b_KBTopics)b_KBTopics.ProcessRowRetrievetags(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    // Add the record to the list.
                    ScheduledRecords.Add((b_KBTopics)b_KBTopics.ProcessRowRetrievetags(reader));
                }
                obj.TotalRecords = new List<List<b_KBTopics>>();
                obj.TotalRecords.Add(AllRecords);
                obj.TotalRecords.Add(ScheduledRecords);
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
            return obj.TotalRecords;
        }


    }
}
