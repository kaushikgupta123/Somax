using Database.Business;
using Database.SqlClient;

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_BBUKPI_RetrieveChunkForSiteSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_BBUKPI_RetrieveChunkForSiteSearch_V2";
        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_BBUKPI_RetrieveChunkForSiteSearch_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_BBUKPI_RetrieveChunkForSiteSearch_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static b_BBUKPI CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_BBUKPI obj
        )
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "Sites", obj.Sites, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateStartDateVw", obj.CreateStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateEndDateVw", obj.CreateEndDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SubmitStartDateVw", obj.SubmitStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SubmitEndDateVw", obj.SubmitEndDateVw, 500);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText ", obj.SearchText, 800);
            command.SetStringInputParameter(SqlDbType.NVarChar, "YearWeekLists", obj.YearWeekLists, 700);
            try
            {

                List<b_BBUKPI> records = new List<b_BBUKPI>();
                // Execute stored procedure.               
                reader = command.ExecuteReader();
                obj.listOfBBUKPI = new List<b_BBUKPI>();
                while (reader.Read())
                {
                    b_BBUKPI tmpBBUKPIs = b_BBUKPI.ProcessRetrieveChunkSiteSearch(reader);
                    obj.listOfBBUKPI.Add(tmpBBUKPIs);
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
            return obj;
        }
    }
}
