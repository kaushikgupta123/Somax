using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    public class usp_MeterReading_CreateReading
    {
        private static string STOREDPROCEDURE_NAME = "usp_MeterReading_CreateReading";
        public usp_MeterReading_CreateReading()
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
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "MeterId", obj.MeterId); 
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.meter_clientlookupid, 31);
            command.SetInputParameter(SqlDbType.Decimal, "Reading", obj.Reading);
            command.SetInputParameter(SqlDbType.DateTime, "ReadingDate", obj.ReadingDate);
            command.SetInputParameter(SqlDbType.BigInt, "ReadingBy_PersonnelId", obj.ReadingBy_PersonnelId);
            command.SetOutputParameter(SqlDbType.BigInt, "MeterReadingId");

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.MeterReadingId = (long)command.Parameters["@MeterReadingId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
