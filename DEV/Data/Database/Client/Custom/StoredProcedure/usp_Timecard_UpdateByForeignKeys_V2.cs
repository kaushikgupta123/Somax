using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    class usp_Timecard_UpdateByForeignKeys_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Timecard_UpdateByForeignKeys_V2";

        public usp_Timecard_UpdateByForeignKeys_V2()
        {
        }

        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Timecard obj
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
            command.SetInputParameter(SqlDbType.BigInt, "TimecardId", obj.TimecardId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType_Primary", obj.ChargeType_Primary, 30);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId_Primary", obj.ChargeToId_Primary);
            command.SetInputParameter(SqlDbType.Decimal, "Hours", obj.Hours);
            command.SetInputParameter(SqlDbType.BigInt, "TimeCardCodeId", 0);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
            command.SetInputParameter(SqlDbType.DateTime2, "StartDate", obj.StartDate);
            command.SetInputParameter(SqlDbType.Int, "UpdateIndex", obj.UpdateIndex);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType_Secondary", obj.ChargeType_Secondary, 30);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId_Secondary", obj.ChargeToId_Secondary);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VMRSWorkAccomplished", obj.VMRSWorkAccomplished, 30);
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
