using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Wo_Workbench_UpdateRecord
   {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Wo_Workbench_UpdateRecord";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Wo_Workbench_UpdateRecord()
        {
        }

        /// <summary>
        /// Static method to call the usp_WorkOrder_UpdateByForeignKeys stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_WorkOrder obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter updateIndexOut_parameter = null;           
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "ApproveBy_PersonnelId", obj.ApproveBy_PersonnelId);
            command.SetInputParameter(SqlDbType.BigInt, "WorkAssigned_PersonnelId", obj.WorkAssigned_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateBy", obj.Createby, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ModifyBy", obj.ModifyBy, 255);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "WorkOrderId", obj.WorkOrderId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ApproveFlag", obj.ApproveFlag,200);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduleFlag", obj.ScheduleFlag, 200);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DeniedFlag", obj.DeniedFlag,200);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DeniedReson", obj.DeniedReason, 200);
            command.SetInputParameter(SqlDbType.DateTime, "ScheduledStartDate", obj.ScheduledStartDate);
            command.SetInputParameter(SqlDbType.Decimal, "ScheduledDuration", obj.ScheduledDuration);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DeniedComment", obj.DeniedComment, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", obj.Shift,15);
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
