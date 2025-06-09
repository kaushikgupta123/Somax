using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    public class usp_Meter_RetrieveBySearchCriteria
    {
        private static string STOREDPROCEDURE_NAME = "usp_Meter_RetrieveBySearchCriteria";
        public usp_Meter_RetrieveBySearchCriteria()
        {

        }
        public static List<b_Meter> CallStoredProcedure(
            SqlCommand command,
            Database.SqlClient.ProcessRow<b_Meter> processRow,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            string query,
            string site,
            string area,
            string department,
            string type,
            string status,
            string dateSelection,
            DateTime dateStart,
            DateTime dateEnd,
            string column,
            string searchText,
            int page,
            int resultsPerPage,
            bool useLike,
            string orderColumn,
            string orderDirection,
            out int ResultCount
        )
        {
            List<b_Meter> records = new List<b_Meter>();
            SqlDataReader reader = null;
            b_Meter record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            ResultCount = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetStringInputParameter(SqlDbType.VarChar, "Query", query, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "Site", site, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "Area", area, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "Department", department, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "Type", type, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "Status", status, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "DateSelection", dateSelection, 256);
            command.SetInputParameter(SqlDbType.DateTime2, "DateStart", dateStart);
            command.SetInputParameter(SqlDbType.DateTime2, "DateEnd", dateEnd);
            command.SetStringInputParameter(SqlDbType.VarChar, "Column", column, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "SearchText", searchText, 256);
            command.SetInputParameter(SqlDbType.Int, "Page", page);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", resultsPerPage);
            command.SetInputParameter(SqlDbType.Bit, "UseLike", ((useLike) ? (0x1) : (0x0)));
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
