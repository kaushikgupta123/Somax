using Database.Business;
using Database.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_UserReportGridDefinition_UpdateFilterByReportId_V2 stored procedure.
    /// </summary>
    public class usp_UserReportGridDefinition_UpdateFilterByReportId_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_UserReportGridDefinition_UpdateFilterByReportId_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_UserReportGridDefinition_UpdateFilterByReportId_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_UserData_RetrieveByUserName stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>       
        /// <returns>ArrayList containing the results of the query</returns>
        /// 
        public static void CallStoredProcedure(
          SqlCommand command,
          long callerUserInfoId,
          string callerUserName,
          b_UserReportGridDefintion obj
      )
        {
            List<b_UserReportGridDefintion> records = new List<b_UserReportGridDefintion>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ReportId", obj.ReportId);
            command.SetInputParameter(SqlDbType.Structured, "UserReportList", obj.UserReportList);
            try
            {
                command.ExecuteNonQuery();
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
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

        }
    }
}
