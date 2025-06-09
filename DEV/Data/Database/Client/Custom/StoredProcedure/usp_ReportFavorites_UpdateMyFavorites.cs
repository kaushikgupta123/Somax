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
    public class usp_ReportFavorites_UpdateMyFavorites
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_ReportFavorites_UpdateMyFavorites";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_ReportFavorites_UpdateMyFavorites()
        {
        }

        /// <summary>
        /// Static method to call the usp_ReportFavorites_UpdateMyFavorites stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="obj">b_ReportFavorites Object to get the parametrs value</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            Database.SqlClient.ProcessRow<b_ReportFavorites> processRow,
            long callerUserInfoId,
            string callerUserName,
            b_ReportFavorites obj
        )
        {

            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            //
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ReportFavoritesId", obj.ReportFavoritesId);
            command.SetInputParameter(SqlDbType.Bit, "Del", obj.Del);
            try
            {
                // Execute stored procedure.
                command.ExecuteNonQuery();

                obj.ReportFavoritesId = (long)command.Parameters["@ReportFavoritesId"].Value;
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
        }
    }
}
