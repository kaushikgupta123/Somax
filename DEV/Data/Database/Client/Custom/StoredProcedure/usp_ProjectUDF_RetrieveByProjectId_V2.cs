using Database.Business;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.SqlClient;

namespace Database.StoredProcedure
{
     class usp_ProjectUDF_RetrieveByProjectId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_ProjectUDF_RetrieveByProjectId_V2";
        public usp_ProjectUDF_RetrieveByProjectId_V2()
        {
                
        }
        public static b_ProjectUDF CallStoredProcedure(
     SqlCommand command,
     long callerUserInfoId,
     string callerUserName,
     b_ProjectUDF obj
     )
        {
            b_ProjectUDF records = new b_ProjectUDF();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ProjectId", obj.ProjectId);
            try
            {
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    records = b_ProjectUDF.ProcessRowForRetrieveByProjectId(reader);
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
