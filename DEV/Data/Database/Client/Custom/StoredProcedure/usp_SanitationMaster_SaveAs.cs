using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_SanitationMaster_SaveAs
    {
        private static string STOREDPROCEDURE_NAME = "usp_SanitationMaster_SaveAs";


        public usp_SanitationMaster_SaveAs()
        {
        }

        public static void CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_SanitationMaster obj

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
            command.SetInputParameter(SqlDbType.BigInt, "SanitationMasterId", obj.SanitationMasterId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "AreaId", obj.AreaId);
            command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            command.SetInputParameter(SqlDbType.BigInt, "Assignto_PersonnelId", obj.Assignto_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToClientLookUpId", obj.ChargeToClientLookupId, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ExclusionDays", obj.ExclusionDays, 50);
            command.SetInputParameter(SqlDbType.Int, "Frequency", obj.Frequency);
            command.SetInputParameter(SqlDbType.DateTime2, "LastScheduled", obj.LastScheduled);
            command.SetInputParameter(SqlDbType.DateTime2, "NextDue", obj.NextDue);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OnDemandGroup", obj.OnDemandGroup, 15);
            command.SetInputParameter(SqlDbType.Decimal, "ScheduledDuration", obj.ScheduledDuration);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduledType", obj.ScheduledType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", obj.Shift, 15);
            command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", obj.InactiveFlag);

           // command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 31);

            command.Parameters["@SanitationMasterId"].Direction = ParameterDirection.InputOutput;

            // Execute stored procedure.
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            { }

            if (!string.IsNullOrEmpty(command.Parameters["@SanitationMasterId"].Value.ToString()))
            {
                obj.SanitationMasterId = (long)command.Parameters["@SanitationMasterId"].Value;
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
