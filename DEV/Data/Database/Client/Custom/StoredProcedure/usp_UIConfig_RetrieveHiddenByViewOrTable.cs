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
    public class usp_UIConfig_RetrieveHiddenByViewOrTable
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_UIConfig_RetrieveHiddenByViewOrTable";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_UIConfig_RetrieveHiddenByViewOrTable()
        {
        }

        /// <summary>
        /// Static method to call the usp_UIConfig_RetrieveHiddenByViewOrTable stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_UIConfig> CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_UIConfig obj
       )
        {
            List<b_UIConfig> records = new List<b_UIConfig>();
            SqlDataReader reader = null;
            b_UIConfig record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;


            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ViewName", obj.ViewName, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TableName", obj.TableName, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "IsHide", obj.isHide, 12);
            command.SetStringInputParameter(SqlDbType.NVarChar, "IsRequired", obj.isRequired, 12);
            command.SetStringInputParameter(SqlDbType.NVarChar, "IsExternal", obj.Isexternal, 12);


            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                //while (reader.Read())
                //{
                //    ResultCount = reader.GetInt32(0);
                //}

                //reader.NextResult();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = (b_UIConfig)b_UIConfig.ProcessRowForusp_UIConfig_RetrieveHiddenByViewOrTable(reader);

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
