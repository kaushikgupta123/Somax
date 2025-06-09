using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_WorkOrder_RetrieveLookupListBySearchCriteria stored procedure.
    /// </summary>
    public class usp_WorkOrder_RetrieveLookupListBySearchCriteria_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_RetrieveLookupListBySearchCriteria_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WorkOrder_RetrieveLookupListBySearchCriteria_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_WorkOrder_RetrieveLookupListBySearchCriteria stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_WorkOrder> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            string clientLookupId,
            string description,
            string ChargeTo_Name,
            string WorkAssigned_Name,
            string Requestor_Name,
            string Status,
            long siteId,
            int page,
            int resultsPerPage,
            string orderColumn,
            string orderDirection,
            out int ResultCount
        )
        {
            List<b_WorkOrder> records = new List<b_WorkOrder>();
            SqlDataReader reader = null;
            b_WorkOrder record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            ResultCount = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", clientLookupId, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", description, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeTo_Name", ChargeTo_Name, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Requestor_Name", Requestor_Name, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "WorkAssigned_Name", WorkAssigned_Name, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", Status, 15);

            command.SetInputParameter(SqlDbType.BigInt, "SiteId", siteId);
            command.SetInputParameter(SqlDbType.Int, "Page", page);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", resultsPerPage);
            command.SetStringInputParameter(SqlDbType.VarChar, "OrderColumn", orderColumn, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "OrderDirection", orderDirection, 256);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                while (reader.Read())
                {
                    ResultCount = reader.GetInt32(0);
                }

                reader.NextResult();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = (b_WorkOrder)b_WorkOrder.ProcessRowWorkorderLookUp(reader);

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
