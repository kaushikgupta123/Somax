using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_SanitationJob_CreateFromSanMasterBatchEntry_Days_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_SanitationJob_CreateFromSanMasterBatchEntry_Days_V2";

        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_SanitationJob obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter SanitationJobIdsList = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Prefix", obj.Prefix, 7);
            command.SetInputParameter(SqlDbType.BigInt, "SanMasterBatchEntryId", obj.SanMasterBatchEntryId);
            command.SetOutputParameter(SqlDbType.BigInt, "SanitationMasterCount");
            command.SetOutputParameter(SqlDbType.BigInt, "SanitationJobCount");
            SanitationJobIdsList = command.Parameters.Add("@SanitationJobIdsList", SqlDbType.NVarChar, -1);
            SanitationJobIdsList.Direction = ParameterDirection.Output;
            try
            {
                // Execute stored procedure.
                command.ExecuteNonQuery();
                obj.SanitationJobCount = Convert.ToInt32(command.Parameters["@SanitationJobCount"].Value.ToString());
                obj.SanitationMasterCount = Convert.ToInt32(command.Parameters["@SanitationMasterCount"].Value.ToString());
                obj.SanitationJobList = (string)SanitationJobIdsList.Value;
            }
            catch (Exception ex) { }
            finally
            {


                // Process the RETURN_CODE parameter value
                retCode = (int)RETURN_CODE_parameter.Value;
                AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

                // Return the result
                //return result;
            }
        }
    }
}
