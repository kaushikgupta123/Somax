using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_ResetMeter_Create
    {
        private static string STOREDPROCEDURE_NAME = "usp_ResetMeter_Create";
        public usp_ResetMeter_Create()
        {

        }
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_MeterReading obj
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
            //command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetOutputParameter(SqlDbType.BigInt, "MeterReadingId");
            command.SetStringInputParameter(SqlDbType.NVarChar, "MeterId", obj.meter_clientlookupid,31);
            command.SetInputParameter(SqlDbType.Decimal, "Reading", obj.Reading);
            command.SetInputParameter(SqlDbType.DateTime, "ReadingDate", obj.ReadingDate);
            command.SetStringInputParameter(SqlDbType.VarChar, "ReadingBy", obj.ReadByClientLookupId,63);
            command.SetInputParameter(SqlDbType.Bit, "Reset", obj.Reset);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.MeterReadingId = (long)command.Parameters["@MeterReadingId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
