using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
    public class usp_BBUKPI_RetrieveYearWeekForFilter_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_BBUKPI_RetrieveYearWeekForFilter_V2";
        public usp_BBUKPI_RetrieveYearWeekForFilter_V2()
        { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="callerUserInfoId"></param>
        /// <param name="callerUserName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<b_BBUKPI> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_BBUKPI obj)
        {
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;


            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);

            // Setup the return object
            List<b_BBUKPI> result = new List<b_BBUKPI>();

            try
            {

                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Retrieve List
                while (reader.Read())
                {
                    // Add the record to the list.
                    result.Add((b_BBUKPI)b_BBUKPI.ProcessRetrieveYearWeekForFilter(reader));
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
            //return records;
            return result;
        }
    }
}
