using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Client_CreateBYSomaxAdmin_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Client_CreateBYSomaxAdmin_V2";

        public usp_Client_CreateBYSomaxAdmin_V2()
        {
        }

        public static void CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_Client obj
       )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetOutputParameter(SqlDbType.BigInt, "CreatedClientId");
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompanyName", obj.CompanyName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "LegalName", obj.LegalName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PrimaryContact", obj.PrimaryContact, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Email", obj.Email, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "BusinessType", obj.BusinessType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PackageLevel", obj.PackageLevel, 15);
            command.SetInputParameter(SqlDbType.Int, "MaxSites", obj.MaxSites);
            command.SetInputParameter(SqlDbType.Bit, "SiteControl", obj.SiteControl);
            command.SetInputParameter(SqlDbType.Int, "MaxAppUsers", obj.MaxAppUsers);
            command.SetInputParameter(SqlDbType.Int, "MaxSanitationUsers", obj.MaxSanitationUsers);
            command.SetInputParameter(SqlDbType.Int, "MaxWorkRequestUsers", obj.MaxWorkRequestUsers);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Language", obj.LocalizationLanguage, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Culture", obj.LocalizationCulture, 256);
            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.CreatedClientId = (long)command.Parameters["@CreatedClientId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
