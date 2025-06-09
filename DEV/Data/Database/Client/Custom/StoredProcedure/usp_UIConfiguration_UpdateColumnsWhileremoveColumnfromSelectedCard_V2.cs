using Database.Business;
using Database.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_UIConfiguration_UpdateColumnsWhileremoveColumnfromSelectedCard_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_UIConfiguration_UpdateColumnsWhileremoveColumnfromSelectedCard_V2";

        public usp_UIConfiguration_UpdateColumnsWhileremoveColumnfromSelectedCard_V2()
        {

        }
        public static void CallStoredProcedure(
         SqlCommand command,
         long callerUserInfoId,
         string callerUserName,
         b_UIConfiguration obj)
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "DataDictionaryId", obj.DataDictionaryId);
            command.SetInputParameter(SqlDbType.Bit, "IsRequired", obj.Required);
            // Execute stored procedure.
            command.ExecuteNonQuery();
            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

        }
    }
}
