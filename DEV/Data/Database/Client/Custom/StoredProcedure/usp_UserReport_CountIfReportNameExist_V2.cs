using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{ /// <summary>
  /// Access the usp_UserReport_CountIfReportNameExist stored procedure.
  /// </summary>
    public class usp_UserReport_CountIfReportNameExist_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        /// 
        private static string STOREDPROCEDURE_NAME = "usp_UserReport_CountIfReportNameExist_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_UserReport_CountIfReportNameExist_V2()
        {
        }

        public static List<b_UserReports> CallStoredProcedure(SqlCommand command, long callerUserInfoId, string callerUserName, b_UserReports obj)

        {
            List<b_UserReports> records = new List<b_UserReports>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ReportName", obj.ReportName, 250);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.                  
                    records.Add((b_UserReports)b_UserReports.ProcessRowForCount(reader));
                }
                reader.NextResult();
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
