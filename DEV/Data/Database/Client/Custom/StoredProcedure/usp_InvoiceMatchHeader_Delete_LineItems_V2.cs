using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Database.SqlClient;
using Database;

namespace Database.StoredProcedure
{
    public class usp_InvoiceMatchHeader_Delete_LineItems_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_InvoiceMatchHeader_Delete_LineItems_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_InvoiceMatchHeader_Delete_LineItems_V2()
        {

        }

        /// <summary>
        /// Static method to call the usp_UserData_RetrieveByUserName stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerId">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
             long clientId,
            long siteId,
           long InvoiceMatchHeaderId
          
            )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "InvoiceMatchHeaderId", InvoiceMatchHeaderId);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", siteId);


            try
            {

                // Execute stored procedure.
                retCode = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            { }
            finally
            {

            }
            //var rma = RETURN_CODE_parameter.Value;
            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

        }
    }
}
