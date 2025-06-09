using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
   public  class usp_UserData_ValidateNewProductionUserAdd_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_UserData_ValidateNewProductionUserAdd_V2";

        public usp_UserData_ValidateNewProductionUserAdd_V2()
        {
        }
        public static List<b_StoredProcValidationError> CallStoredProcedure(
         SqlCommand command,
         long callerUserInfoId,
         string callerUserName,
         b_UserDetails obj
     )
        {
            List<b_StoredProcValidationError> records = new List<b_StoredProcValidationError>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.DefaultSiteId);
            command.SetInputParameter(SqlDbType.Bit, "SiteControled", obj.SiteControlled);
            command.SetInputParameter(SqlDbType.BigInt, "SecurityProfileId", obj.SecurityProfileId);
            command.SetInputParameter(SqlDbType.Bit, "IsSuperUser", obj.IsSuperUser);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UserName", obj.UserName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UserType", obj.UserType, 31);
            command.SetInputParameter(SqlDbType.Bit, "CMMSUser", obj.CMMSUser);
            command.SetInputParameter(SqlDbType.Bit, "SanitationUser", obj.SanitationUser);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_StoredProcValidationError)b_StoredProcValidationError.ProcessRow(reader));
                }

            }
            finally
            {
                if (null != reader)
                {
                    if (false == reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }
    }
}
