using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_MaterialRequest_RetrieveChunkSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_MaterialRequest_RetrieveChunkSearch_V2";

        public usp_MaterialRequest_RetrieveChunkSearch_V2()
        {
        }

        public static List<b_MaterialRequest> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_MaterialRequest obj
       )
        {
            List<b_MaterialRequest> records = new List<b_MaterialRequest>();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 30);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "offset1", obj.offset1, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "nextrow", obj.nextrow, 30);
            command.SetInputParameter(SqlDbType.BigInt, "MaterialRequestId", obj.MaterialRequestId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 200);
            command.SetInputParameter(SqlDbType.DateTime2, "RequiredDate", obj.RequiredDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AccountClientLookupId", obj.AccountClientLookupId,30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 30);
            command.SetInputParameter(SqlDbType.DateTime2, "CreateDate", obj.CreateDate);
            command.SetInputParameter(SqlDbType.DateTime2, "CompleteDate", obj.CompleteDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 30);



            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_MaterialRequest tmpMaterialRequest = b_MaterialRequest.ProcessRowForMaterialRequestRetriveAllForSearch(reader);
                    tmpMaterialRequest.ClientId = obj.ClientId;
                    records.Add(tmpMaterialRequest);
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
