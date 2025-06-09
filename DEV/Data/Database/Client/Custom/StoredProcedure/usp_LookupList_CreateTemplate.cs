/*
 *  Added By Indusnet Technologies
 */

using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
   public class usp_LookupList_CreateTemplate
    {
       private static string STOREDPROCEDURE_NAME = "usp_LookupList_CreateTemplate";

       public usp_LookupList_CreateTemplate()
        {
        }

       public static void CallStoredProcedure(
    SqlCommand command,
    long callerUserInfoId,
    string callerUserName,
    b_LookupList obj
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
           command.SetStringInputParameter(SqlDbType.NVarChar, "Language", obj.Language,256);
           command.SetStringInputParameter(SqlDbType.NVarChar, "Culture", obj.Culture, 256);


           // Execute stored procedure.
           command.ExecuteNonQuery();

           // Process the RETURN_CODE parameter value
           retCode = (int)RETURN_CODE_parameter.Value;
           AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

       }




    }

}
