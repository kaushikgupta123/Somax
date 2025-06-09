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
    public class usp_ReportListing_RetrieveRecentReports
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_ReportListing_RetrieveRecentReports";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_ReportListing_RetrieveRecentReports()
        {
        }
        /// <summary>
        /// Static method to call the usp_ReportListing_RetrieveRecentReports stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="clientId">long that contains the value of the @clientId parameter</param>
        /// <param name="siteId">long that contains the value of the @siteId parameter</param>
        /// <param name="PersonnelId">long that contains the value of the @PersonnelId parameter</param>
        /// <param name="ResultCount">int that contains the value of the @ResultCount parameter.Number of Results that will be retrieved.</param>
        /// <param name="obj">Object of b_ReportListing</param>
        /// <returns>List of b_ReportListing</returns>
        public static List<b_ReportListing> CallStoredProcedure(
    SqlCommand command,
    Database.SqlClient.ProcessRow<b_ReportListing> processRow,
    long callerUserInfoId,
    string callerUserName,
    long clientId,
    long siteId,
    long PersonnelId,
    int ResultCount,
    b_ReportListing obj
)
        {
            List<b_ReportListing> records = new List<b_ReportListing>();
            SqlDataReader reader = null;
            b_ReportListing record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", siteId);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", PersonnelId);
            command.SetInputParameter(SqlDbType.Int, "ResultCount", obj.EventLogResultCount);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
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
