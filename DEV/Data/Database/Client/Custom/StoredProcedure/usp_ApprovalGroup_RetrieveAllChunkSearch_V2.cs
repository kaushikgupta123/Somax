using Database.Business;
using Database.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
   public class usp_ApprovalGroup_RetrieveAllChunkSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_ApprovalGroup_RetrieveAllChunkSearch_V2";
        public usp_ApprovalGroup_RetrieveAllChunkSearch_V2()
        {

        }

        public static b_ApprovalGroup CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_ApprovalGroup obj
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
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetInputParameter(SqlDbType.BigInt, "ApprovalGroupId", obj.ApprovalGroupId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RequestType", obj.RequestType, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 400);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup1ClientLookupId", obj.AssetGroup1ClientLookupId, 63);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup2ClientLookupId", obj.AssetGroup2ClientLookupId, 63);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup3ClientLookupId", obj.AssetGroup3ClientLookupId, 63);
            command.SetInputParameter(SqlDbType.BigInt, "AssetGroup1Id", obj.AssetGroup1Id);
            command.SetInputParameter(SqlDbType.BigInt, "AssetGroup2Id", obj.AssetGroup2Id);
            command.SetInputParameter(SqlDbType.BigInt, "AssetGroup3Id", obj.AssetGroup3Id);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 800);

            try
            {

                List<b_ApprovalGroup> records = new List<b_ApprovalGroup>();
                // Execute stored procedure.
                reader = command.ExecuteReader();
                obj.listOfApprovalGroup = new List<b_ApprovalGroup>();
                while (reader.Read())
                {
                    b_ApprovalGroup tmpApprovalGroup = b_ApprovalGroup.ProcessRetrieveApprovalGroupForChunkV2(reader);
                    obj.listOfApprovalGroup.Add(tmpApprovalGroup);
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
