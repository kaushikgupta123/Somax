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
    public class usp_TimeCard_RetrieveSumOfLabourHours_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_TimeCard_RetrieveSumOfLabourHours_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_TimeCard_RetrieveSumOfLabourHours_V2()
        {
        }

        public static List<b_Timecard> CallStoredProcedure(
              SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
          b_Timecard obj
 )
        {
            // long result = 0;
            List<b_Timecard> records = new List<b_Timecard>();
            SqlDataReader reader = null;
            b_Timecard record = null;
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
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                while (reader.HasRows)
                {
                    // Get the result count
                    while (reader.Read())
                    {                      

                        record = (b_Timecard)b_Timecard.ProcessRowForRetrieveTimeCardSumOfLabourHours(reader);

                        //// Add the record to the list.
                        records.Add(record);
                    }
                    reader.NextResult();
                };
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
