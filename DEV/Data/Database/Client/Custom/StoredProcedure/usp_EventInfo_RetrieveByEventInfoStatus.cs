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
    public class usp_EventInfo_RetrieveByEventInfoStatus
    {
        private static string STOREDPROCEDURE_NAME = "usp_EventInfo_RetrieveByEventInfoStatus_V2";

        public usp_EventInfo_RetrieveByEventInfoStatus()
        {
        }
        public static int CallStoredProcedure(
      SqlCommand command,
      long callerUserInfoId,
      string callerUserName,
      b_EventInfo obj
      )
        {
            List<b_EventInfo> records = new List<b_EventInfo>();
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;
            int retCount = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "EventInfoStatus", obj.Status,256);
            try
            {
                retCount = (Int32)command.ExecuteScalar();
            }
            finally
            {
                //if (null != reader)
                //{
                //    if (false == reader.IsClosed)
                //    {
                //        reader.Close();
                //    }
                //    reader = null;
                //}
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);


            // Return the result
            return retCount;
        }
    }
}
