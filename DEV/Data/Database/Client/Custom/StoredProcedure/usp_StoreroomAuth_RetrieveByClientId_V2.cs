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
    public class usp_StoreroomAuth_RetrieveByClientId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_StoreroomAuth_RetrieveByClientId_V2";
        public usp_StoreroomAuth_RetrieveByClientId_V2()
        {
        }

        public static List<b_StoreroomAuth> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_StoreroomAuth obj
        )
        {
            List<b_StoreroomAuth> records = new List<b_StoreroomAuth>();
            SqlDataReader reader = null;
            
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            //ResultCount = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
            command.SetStringInputParameter(SqlDbType.VarChar, "orderbyColumn", obj.orderbyColumn, 10);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.offset1);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.nextrow);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                while (reader.Read())
                {
                    b_StoreroomAuth tmpStoreroomAuth = b_StoreroomAuth.ProcessRowForChunkSearch(reader);
                    records.Add(tmpStoreroomAuth);
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
