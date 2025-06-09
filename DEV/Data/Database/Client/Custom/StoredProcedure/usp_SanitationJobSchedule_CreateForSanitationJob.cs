using System;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    public class usp_SanitationJobSchedule_CreateForSanitationJob
    {
      
        private static string STOREDPROCEDURE_NAME = "usp_SanitationJobSchedule_CreateForSanitationJob";

     
        public usp_SanitationJobSchedule_CreateForSanitationJob()
        {
        }

       
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_SanitationJobSchedule obj
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
            command.SetOutputParameter(SqlDbType.BigInt, "SanitationJobScheduleId");
            command.SetInputParameter(SqlDbType.BigInt, "SanitationJobId", obj.SanitationJobId);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
            command.SetInputParameter(SqlDbType.DateTime2, "ScheduledStartDate", obj.ScheduledStartDate);
            command.SetInputParameter(SqlDbType.Decimal, "ScheduledHours", obj.ScheduledHours);
            command.SetInputParameter(SqlDbType.Bit, "Del", obj.Del);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.SanitationJobScheduleId = (long)command.Parameters["@SanitationJobScheduleId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
