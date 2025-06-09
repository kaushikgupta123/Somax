using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Business;
using System.Data.SqlClient;
using System.Data;
using Database.SqlClient;

namespace Database.StoredProcedure
{
     
    public class usp_SanitationMaster_CreateByFK
    {
        private static string STOREDPROCEDURE_NAME = "usp_SanitationMaster_CreateByFK";
        public usp_SanitationMaster_CreateByFK()
        {
        }

        public static void CallStoredProcedure(SqlCommand command,long callerUserInfoId,string callerUserName, b_SanitationMaster obj)
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetOutputParameter(SqlDbType.BigInt, "SanitationMasterId");
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "AreaId", obj.AreaId);
            command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            command.SetInputParameter(SqlDbType.BigInt, "Assignto_PersonnelId", obj.Assignto_PersonnelId);
            command.SetInputParameter(SqlDbType.BigInt, "PlantLocationId", obj.PlantLocationId); 
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToClientLookUpId", obj.ChargeToClientLookupId,256);
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

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.SanitationMasterId = (long)command.Parameters["@SanitationMasterId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
