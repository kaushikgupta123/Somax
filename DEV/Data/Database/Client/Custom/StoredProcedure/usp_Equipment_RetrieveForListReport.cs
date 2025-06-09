using Common.Structures;
using Database.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_Equipment_RetrieveForListReport
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Equipment_RetrieveForListReport";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Equipment_RetrieveForListReport()
        {
        }

        /// <summary>
        /// Static method to call the usp_Equipment_RetrieveAll stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<ReportDataStructure> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            string primaryColumn,
            string primaryTable,
            string joinOnColumn,
            string searchOnColumn,
            string joinedTable,
            List<string> selectedValues,
            List<string> selectedColumns,
            List<string> selectedNumerics,
            string dateSelection,
            DateTime dateStart,
            DateTime dateEnd,
            string column,
            string searchText,
            bool useLike
        )
        {

            // Preallocating significant space for the list will reduce overhead that would occur when the list grows dynamically.
            // In order to avoid memory issues, however, it's best to use this only when a large data set is likely, such as in reports.
            List<ReportDataStructure> records = new List<ReportDataStructure>(100000);
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            StringBuilder whereClause = new StringBuilder();
            StringBuilder fromClause = new StringBuilder();

            fromClause.AppendFormat("FROM [dbo].[{0}] ", primaryTable);
            if (!string.IsNullOrWhiteSpace(joinedTable))
            {
                fromClause.AppendFormat(", [dbo].[{0}] ", joinedTable);
                whereClause.AppendFormat(" AND [{0}].[{1}] = [{2}].[{3}]", primaryTable, primaryColumn, joinedTable, joinOnColumn);
            }

            // Simple where clause. Limit to selected values, if any.
            if (selectedValues != null && selectedValues.Count > 0)
            {
                bool addComma = false;
                if (string.IsNullOrWhiteSpace(joinedTable))
                {
                    whereClause.AppendFormat(" AND [{0}].[{1}] IN (", primaryTable, string.IsNullOrWhiteSpace(searchOnColumn) ? primaryColumn : searchOnColumn);
                }
                else
                {
                    whereClause.AppendFormat(" AND [{0}].[{1}] IN (", joinedTable, string.IsNullOrWhiteSpace(searchOnColumn) ? joinOnColumn : searchOnColumn);
                }
                foreach (string val in selectedValues)
                {
                    whereClause.Append(string.Format("{1}'{0}'", val.Trim(), (addComma ? ", " : "")));
                    addComma = true;
                }
                whereClause.Append(")");
            }

            // Add Date Selection to clause

            if (!string.IsNullOrWhiteSpace(dateSelection))
            {
                DateTime minDate = dateStart == DateTime.MinValue ? DateTime.Parse("1/1/1900") : dateStart;
                DateTime maxDate = dateEnd == DateTime.MinValue ? DateTime.Parse("1/1/2200") : dateEnd;
                whereClause.AppendFormat(" AND [{0}].[{1}] > '{2}' AND [{0}].[{1}] < '{3}'", primaryTable, dateSelection, minDate.ToShortDateString(), maxDate.ToShortDateString());
            }

            // Add Column Selection to clause

            if (!string.IsNullOrWhiteSpace(column))
            {
                whereClause.AppendFormat(" AND [{0}].[{1}] LIKE '{2}{3}%'", primaryTable, column, useLike ? "%" : "", searchText);
            }

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "FromClause", fromClause.ToString(), 4000);
            command.SetStringInputParameter(SqlDbType.NVarChar, "WhereClause", whereClause.ToString(), 4000);

            try
            {
                Debug.WriteLine("Built command object");
                Debug.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));

                // Execute stored procedure.
                reader = command.ExecuteReader();
                Debug.WriteLine("reader = command.ExecuteReader();");
                Debug.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));

                // Loop through dataset.
                while (reader.Read())
                {
                    ReportDataStructure tmp = new ReportDataStructure()
                    {
                        RowTextEntries = new List<string>(),
                        RowNumericEntries = new List<decimal>()
                    };

                    // Grab the Group-By Value
                    if (!string.IsNullOrWhiteSpace(primaryColumn))
                    {
                        tmp.GroupByKey = reader[primaryColumn].ToString();
                    }

                    // Grab the selected columns - we'll order by the 1st column
                    foreach (string s in selectedColumns)
                    {
                        tmp.RowTextEntries.Add(reader[s].ToString());
                    }

                    // Grab the select numeric columns as well 

                    foreach (string s in selectedNumerics)
                    {
                        try
                        {
                            tmp.RowNumericEntries.Add((decimal)reader[s]);
                        }
                        catch
                        {
                            tmp.RowNumericEntries.Add(0);
                        }
                    }

                    // Process the current row into a record
                    records.Add(tmp);
                }
                Debug.WriteLine("while (reader.Read())");
                Debug.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
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
