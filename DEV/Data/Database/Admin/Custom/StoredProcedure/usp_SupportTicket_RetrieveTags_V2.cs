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
    public class usp_SupportTicket_RetrieveTags_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_SupportTicket_RetrieveTags_V2";
        public usp_SupportTicket_RetrieveTags_V2()
        {

        }

        public static List<List<b_SupportTicket>> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_SupportTicket obj
       )
        {
            List<b_SupportTicket> AllRecords = new List<b_SupportTicket>();
            List<b_SupportTicket> ScheduledRecords = new List<b_SupportTicket>();

            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    AllRecords.Add((b_SupportTicket)b_SupportTicket.ProcessRowRetrievetags(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    // Add the record to the list.
                    ScheduledRecords.Add((b_SupportTicket)b_SupportTicket.ProcessRowRetrievetags(reader));
                }
                obj.TotalRecords = new List<List<b_SupportTicket>>();
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
