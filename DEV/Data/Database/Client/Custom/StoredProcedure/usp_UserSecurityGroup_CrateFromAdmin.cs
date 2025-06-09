
/*
 *  Added By Indusnet Technologies
 * 
 */

using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
   public class usp_UserSecurityGroup_CrateFromAdmin
    {
       private static string STOREDPROCEDURE_NAME = "usp_UserSecurityGroup_CrateFromAdmin";

       public usp_UserSecurityGroup_CrateFromAdmin()
        {
           
        }

       public static void CallStoredProcedure(
          SqlCommand command,
          long callerUserInfoId,
          string callerUserName,
          b_UserSecurityGroup obj
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
           command.SetOutputParameter(SqlDbType.BigInt, "UserSecurityGroupId");         
           command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);

           // Execute stored procedure.
           command.ExecuteNonQuery();

           obj.UserSecurityGroupId = (long)command.Parameters["@UserSecurityGroupId"].Value;

           // Process the RETURN_CODE parameter value
           retCode = (int)RETURN_CODE_parameter.Value;
           AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
       }

    }
}
