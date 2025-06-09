
/*
 *  Added By Indusnet Technologies
 */

using System.Data;
using System.Data.SqlClient;
using Database.SqlClient;

namespace Database.StoredProcedure
{
   public class usp_SecurityItem_CreateTemplate
   {
           

       private static string STOREDPROCEDURE_NAME = "usp_SecurityItem_CreateTemplate";

       public usp_SecurityItem_CreateTemplate()
        {
        }

       public static int CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           long ClientId
       )
       {
           SqlParameter RETURN_CODE_parameter = null;
           int retCode = 0;

           int retSecurityItemId = 0;

           // Setup command.
           command.SetProcName(STOREDPROCEDURE_NAME);
           RETURN_CODE_parameter = command.GetReturnCodeParameter();
           command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
           command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
           command.SetInputParameter(SqlDbType.BigInt, "ClientId", ClientId);
           command.SetOutputParameter(SqlDbType.Int, "NoOfTemplateCreated");


           // Execute stored procedure.
           command.ExecuteNonQuery();

           retSecurityItemId = (int)command.Parameters["@NoOfTemplateCreated"].Value;

           // Process the RETURN_CODE parameter value
           retCode = (int)RETURN_CODE_parameter.Value;
           AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

           return (retSecurityItemId);
       }




    }

}
