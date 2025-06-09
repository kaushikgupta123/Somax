
/*
 * Added By Indusnet Technologies
 */ 

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
namespace Database.StoredProcedure
{
   public class usp_UserPermission_RetrieveByGrant
    {
       private static string STOREDPROCEDURE_NAME = "usp_UserPermission_RetrieveByGrant";

       public usp_UserPermission_RetrieveByGrant()
        {
        }
       public static long CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_UserPermission obj
       )
       {
          
           SqlParameter RETURN_CODE_parameter = null;
           int retCode = 0;
           long siteCount = 0;
           // Setup command.
           command.SetProcName(STOREDPROCEDURE_NAME);
           RETURN_CODE_parameter = command.GetReturnCodeParameter();
           command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
           command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
           command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
           command.SetOutputParameter(SqlDbType.BigInt, "SiteCount");
           // Execute stored procedure.
           command.ExecuteNonQuery();
           siteCount = (long)command.Parameters["@SiteCount"].Value;
           obj.SiteCount = siteCount;

           // Process the RETURN_CODE parameter value
           retCode = (int)RETURN_CODE_parameter.Value;
           AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

           // Return the result
           return siteCount;
       }




    }

}
