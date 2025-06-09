
using System;

using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_Alerts_Create stored procedure.
    /// </summary>
    public class usp_Alerts_Create
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Alerts_Create";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Alerts_Create()
        {
        }

        /// <summary>
        /// Static method to call the usp_Alerts_Create stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Alerts obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetOutputParameter(SqlDbType.BigInt, "AlertsId");
            command.SetInputParameter(SqlDbType.BigInt, "AlertDefineId", obj.AlertDefineId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Headline", obj.Headline, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Summary", obj.Summary, 127);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Details", obj.Details, 4000);
            command.SetStringInputParameter(SqlDbType.NVarChar, "From", obj.From, 127);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AlertType", obj.AlertType, 127);
            command.SetInputParameter(SqlDbType.BigInt, "ObjectId", obj.ObjectId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ObjectName", obj.ObjectName, 127);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Action", obj.Action, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ObjectState", obj.ObjectState, 31);
            command.SetInputParameter(SqlDbType.Bit, "IsCleared", obj.IsCleared);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Notes", obj.Notes, 4000);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.AlertsId = (long)command.Parameters["@AlertsId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
