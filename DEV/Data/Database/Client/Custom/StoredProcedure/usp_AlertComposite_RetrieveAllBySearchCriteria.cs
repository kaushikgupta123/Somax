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
    /// <summary>
    /// Access the usp_DeploymentEnvironment_Retrieve stored procedure.
    /// </summary>
    public class usp_AlertComposite_RetrieveAllBySearchCriteria
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_AlertComposite_RetrieveAllBySearchCriteriaV2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_AlertComposite_RetrieveAllBySearchCriteria()
        {
        }

        /// <summary>
        /// Static method to call the usp_UserData_RetrieveByUserName stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerId">string that identifies the user calling the database</param>
        /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
        /// <param name="sessionId">System.Guid that contains the value of the @SessionId parameter</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<KeyValuePair<b_Alerts, b_AlertUser>> CallStoredProcedure(SqlCommand command,
            long callerUserInfoId,
            long clientId,
            long personnelId,
            bool retrieveActionItems,
            bool retrieveMessages,
            string alertType,
            string from,
            string objectName,
            DateTime dateStart,
            DateTime dateEnd,
            string column,
            string searchText,
            int page,
            int resultsPerPage,
            string orderColumn,
            string orderDirection,
            out int resultCount
            )
        {
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", personnelId);
            command.SetInputParameter(SqlDbType.Bit, "RetrieveActionItems", retrieveActionItems);
            command.SetInputParameter(SqlDbType.Bit, "RetrieveMessages", retrieveMessages);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AlertType", alertType, 127);
            command.SetStringInputParameter(SqlDbType.NVarChar, "From", from, 127);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ObjectName", objectName, 127);
            command.SetInputParameter(SqlDbType.DateTime2, "DateStart", dateStart);
            command.SetInputParameter(SqlDbType.DateTime2, "DateEnd", dateEnd);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Column", column, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", searchText, 256);
            command.SetInputParameter(SqlDbType.Int, "Page", page);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", resultsPerPage);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderColumn", orderColumn, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderDirection", orderDirection, 256);

            List<KeyValuePair<b_Alerts, b_AlertUser>> results;
            resultCount = 0;

            try
            {
                results = new List<KeyValuePair<b_Alerts, b_AlertUser>>();

                b_Alerts key;
                b_AlertUser val;

                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                while (reader.Read())
                {
                    resultCount = reader.GetInt32(0);
                }

                reader.NextResult();
                // Get all alert results
                while (reader.Read())
                {
                    // Add the record to the list.
                    key = (b_Alerts)b_Alerts.ProcessRow(reader);
                    val = (b_AlertUser)b_AlertUser.ProcessRowForAlertComposite(reader);
                    results.Add(new KeyValuePair<b_Alerts, b_AlertUser>(key, val));
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
            return results;
        }
    }
}
