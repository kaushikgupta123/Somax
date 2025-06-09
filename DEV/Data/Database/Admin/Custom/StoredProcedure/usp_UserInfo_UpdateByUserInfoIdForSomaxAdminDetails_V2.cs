
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_UserInfo_UpdateByUserInfoIdForSomaxAdminDetails_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_UserInfo_UpdateByUserInfoIdForSomaxAdminDetails_V2";
        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_UserInfo_UpdateByUserInfoIdForSomaxAdminDetails_V2()
        {
        }
        /// <summary>
        /// Static method to call the usp_ScheduledService_UpdateByPK stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_UserInfo obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", obj.UserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", obj.UserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.LoginClientID);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.DefaultSiteId);
            command.SetInputParameter(SqlDbType.BigInt, "ClientUserInfoListID", obj.ClientUserInfoListID);
           
            // Execute stored procedure.
            command.ExecuteNonQuery();
            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
