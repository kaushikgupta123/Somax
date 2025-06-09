using Database.Business;
using Database.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
   public class usp_AppGroupApprovers_RetrieveByApprovalGroupId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_AppGroupApprovers_RetrieveByApprovalGroupId_V2";
        public usp_AppGroupApprovers_RetrieveByApprovalGroupId_V2()
        {

        }

        public static b_AppGroupApprovers CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_AppGroupApprovers obj
       )
        {
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetInputParameter(SqlDbType.BigInt, "ApprovalGroupId", obj.ApprovalGroupId);
            try
            {

                List<b_AppGroupApprovers> records = new List<b_AppGroupApprovers>();
                // Execute stored procedure.
                reader = command.ExecuteReader();
                obj.listOfAppGroupApprovers = new List<b_AppGroupApprovers>();
                while (reader.Read())
                {
                    b_AppGroupApprovers tmpAppGroupApprovers = b_AppGroupApprovers.ProcessRetrieveAppGroupApproversChildGridV2(reader);
                    obj.listOfAppGroupApprovers.Add(tmpAppGroupApprovers);
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
            return obj;
        }
    }
}
