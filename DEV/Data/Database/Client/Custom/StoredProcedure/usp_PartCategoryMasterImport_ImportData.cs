using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    class usp_PartCategoryMasterImport_ImportData
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PartCategoryMasterImport_ImportData";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PartCategoryMasterImport_ImportData()
        {
        }

        
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PartCategoryMasterImport obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter error_message_count = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PartCategoryMasterImportId", obj.PartCategoryMasterImportId);

            error_message_count = command.Parameters.Add("@error_message_count", SqlDbType.Int);
            error_message_count.Direction = ParameterDirection.Output;
            // Execute stored procedure.
            command.ExecuteNonQuery();
            obj.error_message_count = (int)error_message_count.Value;
            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
