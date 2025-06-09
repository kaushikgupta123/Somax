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
    /// <summary>
    /// Access the usp_AlertUser_UpdateByPK stored procedure.
    /// </summary>
    public class usp_AlertUser_UpdateByPK
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_AlertUser_UpdateByPK";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_AlertUser_UpdateByPK()
        {
        }

        /// <summary>
        /// Static method to call the usp_AlertUser_UpdateByPK stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="updateIndexOut">int that contains the value of the @UpdateIndexOut parameter</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_AlertUser obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter updateIndexOut_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "AlertUserId", obj.AlertUserId);
            command.SetInputParameter(SqlDbType.BigInt, "UserId", obj.UserId);
            command.SetInputParameter(SqlDbType.BigInt, "AlertsId", obj.AlertsId);
            command.SetInputParameter(SqlDbType.DateTime2, "ActiveDate", obj.ActiveDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Permission", obj.Permission, 127);
            command.SetInputParameter(SqlDbType.Bit, "IsRead", obj.IsRead);
            command.SetInputParameter(SqlDbType.Bit, "IsDeleted", obj.IsDeleted);
            command.SetInputParameter(SqlDbType.Int, "UpdateIndex", obj.UpdateIndex);

            // Setup updateIndexOut parameter.
            updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
            updateIndexOut_parameter.Direction = ParameterDirection.Output;

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.UpdateIndex = (int)updateIndexOut_parameter.Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
