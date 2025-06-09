using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_IoTReadingImport_Process
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_IoTReadingImport_Process";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_IoTReadingImport_Process()
        {
        }

        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_IoTReadingImport obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter alertName_parameter = null;
            SqlParameter IoTEventId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "IoTReadingImportId", obj.IoTReadingImportId);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "IoTDevice", obj.IoTDevice, 31);
            alertName_parameter = command.Parameters.Add("@AlertName", SqlDbType.NVarChar, 63);
            alertName_parameter.Direction = ParameterDirection.Output;
            IoTEventId_parameter = command.Parameters.Add("@IoTEventId", SqlDbType.BigInt);
            IoTEventId_parameter.Direction = ParameterDirection.Output;


            // Execute stored procedure.
            command.ExecuteNonQuery();

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            string alertName = alertName_parameter.Value.ToString();
            long IoTEventId = (long)IoTEventId_parameter.Value;
            obj.AlertName = alertName;
            obj.IoTEventId = IoTEventId;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
