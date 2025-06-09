using Database.Business;
using Database.SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    class usp_ServiceOrderSchedule_AddScheduleRecord_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_ServiceOrderSchedule_AddScheduleRecord_V2";

        public usp_ServiceOrderSchedule_AddScheduleRecord_V2()
        {
        }

        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_ServiceOrder obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter updateIndexOut_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);

            command.SetInputParameter(SqlDbType.BigInt, "ServiceOrderId", obj.ServiceOrderId);
            command.SetInputParameter(SqlDbType.DateTime2, "ScheduledStartDate", obj.ScheduleDate);

            //command.SetInputParameter(SqlDbType.Decimal, "ScheduledHours", obj.);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PersonnelList", obj.PersonnelList,512);
            command.SetInputParameter(SqlDbType.Bit, "IsDeleteFlag", obj.IsDeleteFlag);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", obj.Shift, 30);

            
            command.SetInputParameter(SqlDbType.Int, "UpdateIndex", obj.UpdateIndex);
            

            // Setup updateIndexOut parameter.
            updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
            updateIndexOut_parameter.Direction = ParameterDirection.Output;


            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.UpdateIndex = (int)updateIndexOut_parameter.Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
