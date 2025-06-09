using Database.Business;
using System.Data.SqlClient;
using System.Data;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_PMSchedAssign_Create stored procedure.
    /// </summary>
    public class usp_PMSchedAssign_Create
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PMSchedAssign_Create";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PMSchedAssign_Create()
        {
        }

        /// <summary>
        /// Static method to call the usp_PMSchedAssign_Create stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PMSchedAssign obj
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
            command.SetOutputParameter(SqlDbType.BigInt, "PMSchedAssignId");
            command.SetInputParameter(SqlDbType.BigInt, "PrevMaintSchedId", obj.PrevMaintSchedId);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
            command.SetInputParameter(SqlDbType.Decimal, "ScheduledHours", obj.ScheduledHours);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PMSchedAssignId = (long)command.Parameters["@PMSchedAssignId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
