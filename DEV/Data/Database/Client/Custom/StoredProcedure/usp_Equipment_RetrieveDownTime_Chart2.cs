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
    /// Access the  usp_Equipment_RetriveDownTime_Chart stored procedure.
    /// </summary>
    public class usp_Equipment_RetrieveDownTime_Chart2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Equipment_RetrieveDowntimeChart2_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Equipment_RetrieveDownTime_Chart2()
        {
        }

        /// <summary>
        /// Static method to call the usp_Equipment_RetriveDownTime_Chart2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static System.Data.DataTable CallStoredProcedure(
           //public static System.Data.DataTable CallStoredProcedure(
           SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long ClientId,
            long SiteId,
            int TimeFrame
        )
        {
            List<KeyValuePair<string, long>> records = new List<KeyValuePair<string, long>>();
            SqlDataReader reader = null;
            //b_Account record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;
            System.Data.DataTable temp_table = new DataTable();

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", SiteId);
            command.SetInputParameter(SqlDbType.Int, "TimeFrame", TimeFrame);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
                temp_table.Columns.Add("EQID", typeof(string));
                temp_table.Columns.Add("EQNAME", typeof(string));
                temp_table.Columns.Add("MINS", typeof(long));
                System.Data.DataRow temp_row = null;
                while (reader.Read())
                {
                    temp_row = temp_table.NewRow();
                    temp_row["EQID"] = reader.GetString(0);
                    temp_row["EQNAME"] = reader.GetString(1);
                    temp_row["MINS"] = (long)reader.GetInt64(2);
                    temp_table.Rows.Add(temp_row);
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
            return temp_table;
        }
    }
}
