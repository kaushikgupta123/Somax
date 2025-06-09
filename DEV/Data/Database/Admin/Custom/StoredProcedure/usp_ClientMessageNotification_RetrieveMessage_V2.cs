using Database.Business;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.SqlClient;

namespace Database.StoredProcedure
{

    public class usp_ClientMessageNotification_RetrieveMessage_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_ClientMessageNotification_RetrieveMessage_V2";
        public usp_ClientMessageNotification_RetrieveMessage_V2()
        {

        }
        /// <summary>
        /// Static method to call the usp_ClientMessage_RetrieveChunkSearchForDetails_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="clientId">long that identifies the user calling the database</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_ClientMessage> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            string timeZone,
            b_ClientMessage obj
        )
        {
            List<b_ClientMessage> records = new List<b_ClientMessage>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TimeZone", timeZone, 255);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                obj.listOfAllClientMeassge = new List<b_ClientMessage>();
                while (reader.Read())
                {
                    b_ClientMessage tmpAllClientMessages = b_ClientMessage.ProcessRowForRetrieveAllClientMessages(reader);
                    obj.listOfAllClientMeassge.Add(tmpAllClientMessages);
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

            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
            return records;
        }
    }
}
