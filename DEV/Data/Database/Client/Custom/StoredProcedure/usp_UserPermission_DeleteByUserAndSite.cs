/*
 *  Added By Indusnet Technologies
 */ 
using System;
using System.Data;
using System.Data.SqlClient;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
    public class usp_UserPermission_DeleteByUserAndSite
    {
        private static string STOREDPROCEDURE_NAME = "usp_UserPermission_DeleteByUserAndSite";
        public usp_UserPermission_DeleteByUserAndSite()
        {
        }

        public static void CallStoredProcedure(
          SqlCommand command,
          long callerUserInfoId,
          string callerUserName,
          b_UserPermission obj
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
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);


            // Execute stored procedure.
            command.ExecuteNonQuery();

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }




    }

}
