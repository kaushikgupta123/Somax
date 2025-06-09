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
    public class usp_DataConstant_RetrieveLocaleForConstantTypeWithID_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_DataConstant_RetrieveLocaleForConstantTypeWithID_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_DataConstant_RetrieveLocaleForConstantTypeWithID_V2()
        {

        }

        /// <summary>
        /// List of object of b_DataConstant
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="obj">b_DataConstant Object to get the parametrs value</param>
        /// <returns>List containing the results of the query</returns>
        public static List<b_DataConstant> CallStoredProcedure(SqlCommand command,
            Database.SqlClient.ProcessRow<b_DataConstant> processRow,
            long callerUserInfoId,
            string callerUserName,
            b_DataConstant obj)
        {
            List<b_DataConstant> records = new List<b_DataConstant>();
            SqlDataReader reader = null;
            b_DataConstant record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetStringInputParameter(SqlDbType.NVarChar, "ConstantType", obj.ConstantType,50);
            command.SetStringInputParameter(SqlDbType.NVarChar, "LocaleId", obj.LocaleId, 10);
            command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", obj.InactiveFlag);

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
