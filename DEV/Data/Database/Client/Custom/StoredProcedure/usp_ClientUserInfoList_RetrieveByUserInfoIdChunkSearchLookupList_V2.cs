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
    public class usp_ClientUserInfoList_RetrieveByUserInfoIdChunkSearchLookupList_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_ClientUserInfoList_RetrieveByUserInfoIdChunkSearchLookupList_V2";
        public usp_ClientUserInfoList_RetrieveByUserInfoIdChunkSearchLookupList_V2()
        {
        }

        public static List<b_ClientUserInfoList> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_ClientUserInfoList obj
        )
        {
            List<b_ClientUserInfoList> records = new List<b_ClientUserInfoList>();
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            //ResultCount = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", obj.UserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientName", obj.ClientName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SiteName", obj.SiteName, 63);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", obj.nextrow);
            command.SetInputParameter(SqlDbType.Int, "Page", obj.offset1);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 10);
            command.SetStringInputParameter(SqlDbType.VarChar, "orderbyColumn", obj.orderbyColumn, 10);
          
            

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                while (reader.Read())
                {
                    b_ClientUserInfoList tmpClientUserInfoList = b_ClientUserInfoList.ProcessRowForChunkSearchClientUserInfoList(reader);
                    records.Add(tmpClientUserInfoList);
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
