using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    public class usp_Meter_UpdateByPKForeignKeys
    {
        private static string STOREDPROCEDURE_NAME = "usp_Meter_UpdateByPKForeignKeys";
        public usp_Meter_UpdateByPKForeignKeys()
        {

        }
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Meter obj
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
            command.SetInputParameter(SqlDbType.BigInt, "MeterId", obj.MeterId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "AreaId", obj.AreaId);
            command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Name", obj.Name, 256);
            command.SetInputParameter(SqlDbType.Decimal, "ReadingCurrent", obj.ReadingCurrent);
            command.SetInputParameter(SqlDbType.DateTime, "ReadingDate", obj.ReadingDate);
            command.SetInputParameter(SqlDbType.Decimal, "ReadingLife", obj.ReadingLife);
            command.SetInputParameter(SqlDbType.Decimal, "ReadingMax", obj.ReadingMax);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ReadingUnits", obj.ReadingUnits, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 31);
              

            // Setup updateIndexOut parameter.
            updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
            updateIndexOut_parameter.Direction = ParameterDirection.Output;

            // Execute stored procedure.
            command.ExecuteNonQuery();            

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
