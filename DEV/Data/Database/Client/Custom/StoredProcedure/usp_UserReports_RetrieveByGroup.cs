using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    public class usp_UserReports_RetrieveByGroup
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_UserReports_RetrieveByGroup";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_UserReports_RetrieveByGroup()
        {
        }

        /// <summary>
        /// Static method to call the usp_UserReports_RetrieveByGroup stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_UserReports> CallStoredProcedure(
            SqlCommand command,
            Database.SqlClient.ProcessRow<b_UserReports> processRow,
            long callerUserInfoId,
            string callerUserName,
            b_UserReports obj
        )
        {
            List<b_UserReports> records = new List<b_UserReports>();
            SqlDataReader reader = null;
            //b_UserReports record = null;
           SqlParameter RETURN_CODE_parameter = null;
            //
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
          RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ReportGroup", obj.ReportGroup, 30);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);


            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    // Add the record to the list.
                    records.Add((b_UserReports)b_UserReports.ProcessRowForGroupList(reader));                     

                    
                    
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
