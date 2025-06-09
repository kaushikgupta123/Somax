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
    public class usp_ProjectTask_RetrieveByProjectId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_ProjectTask_RetrieveByProjectId_V2";
        public usp_ProjectTask_RetrieveByProjectId_V2()
        {

        }

        public static List<b_ProjectTask> CallStoredProcedure(
 SqlCommand command,
 long callerUserInfoId,
 string callerUserName,
 b_ProjectTask obj
 )
        {
            List<b_ProjectTask> records = new List<b_ProjectTask>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "ProjectId", obj.ProjectId);

            try
            {

                // Execute stored procedure.
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_ProjectTask tmpProjectTask = b_ProjectTask.ProcessRowForRetrieveByProjectId(reader);
                    tmpProjectTask.ClientId = obj.ClientId;
                    records.Add(tmpProjectTask);
                }

            }
            finally
            {
                if (null != reader)
                {
                    if (false == reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);


            // Return the result
            return records;
        }
    }
}
