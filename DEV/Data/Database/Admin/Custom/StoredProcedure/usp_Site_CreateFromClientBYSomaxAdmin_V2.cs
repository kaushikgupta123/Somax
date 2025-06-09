using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Site_CreateFromClientBYSomaxAdmin_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Site_CreateFromClientBYSomaxAdmin_V2";

        public usp_Site_CreateFromClientBYSomaxAdmin_V2()
        {
        }

        public static void CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_Site obj
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "Name", obj.Name, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Timezone", obj.TimeZone, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Localization", obj.Localization, 31);
            command.SetInputParameter(SqlDbType.Bit, "APM", obj.APM);
            command.SetInputParameter(SqlDbType.Bit, "CMMS", obj.CMMS);
            command.SetInputParameter(SqlDbType.Bit, "Sanitation", obj.Sanitation);
            command.SetInputParameter(SqlDbType.Int, "SiteMaxAppUsers", obj.MaxAppUsers);
            command.SetInputParameter(SqlDbType.Int, "SiteMaxSanitationUsers", obj.MaxSanitationUsers);
            command.SetOutputParameter(SqlDbType.BigInt, "CreatedSiteId");
            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.SiteId = (long)command.Parameters["@CreatedSiteId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
