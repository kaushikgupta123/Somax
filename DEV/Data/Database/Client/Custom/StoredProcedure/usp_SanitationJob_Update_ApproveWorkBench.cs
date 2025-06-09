using System;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{ 
    public class usp_SanitationJob_Update_ApproveWorkBench
    {      
        private static string STOREDPROCEDURE_NAME = "usp_SanitationJob_Update_ApproveWorkBench";

        
        public usp_SanitationJob_Update_ApproveWorkBench()
        {
        }       
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_SanitationJob obj
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
            command.SetInputParameter(SqlDbType.BigInt, "SanitationJobId", obj.SanitationJobId);
            command.SetInputParameter(SqlDbType.BigInt, "AreaId", obj.AreaId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "AssignedTo_PersonnelId", obj.AssignedTo_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", obj.Shift, 15);
            command.SetInputParameter(SqlDbType.DateTime2, "ScheduledDate", obj.ScheduledDate);
            command.SetInputParameter(SqlDbType.Decimal, "ScheduledDuration", obj.ScheduledDuration);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetInputParameter(SqlDbType.BigInt, "ApproveBy_PersonnelId", obj.ApproveBy_PersonnelId);
            command.SetInputParameter(SqlDbType.BigInt, "DeniedBy_PersonnelId", obj.DeniedBy_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DeniedReson", obj.DeniedReason, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DeniedComment", obj.DeniedComment, 500);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "CreateBy", obj.CreateBy, 255);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "ModifyBy", obj.ModifyBy, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ApproveFlag", obj.ApproveFlag, 200);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduleFlag", obj.ScheduleFlag, 200);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DeniedFlag", obj.DeniedFlag, 200);
            
         
            // Setup updateIndexOut parameter
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
