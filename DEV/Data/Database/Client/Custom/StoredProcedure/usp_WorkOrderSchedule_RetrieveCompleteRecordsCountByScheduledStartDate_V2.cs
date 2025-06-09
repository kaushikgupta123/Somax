using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
    public class usp_WorkOrderSchedule_RetrieveCompleteRecordsCountByScheduledStartDate_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrderSchedule_RetrieveCompleteRecordsCountByScheduledStartDate_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WorkOrderSchedule_RetrieveCompleteRecordsCountByScheduledStartDate_V2()
        {
        }

        public static List<b_WorkOrderSchedule> CallStoredProcedure(
              SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
          b_WorkOrderSchedule obj
 )
        {
           // long result = 0;
            List<b_WorkOrderSchedule> records = new List<b_WorkOrderSchedule>();
            SqlDataReader reader = null;
            b_WorkOrderSchedule record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "CaseNo", obj.CaseNo);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                while (reader.Read())
                {
                    // string seriesname = reader.GetString(1);
                    //result = (long)reader.GetInt32(0);

                    record = (b_WorkOrderSchedule)b_WorkOrderSchedule.ProcessRowForRetrieveWorkOrderCompletebySchdeuleStartDate(reader);

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
