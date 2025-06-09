using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PartStoreroom_RetrieveChunkSearchForAutoTRGeneration_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PartStoreroom_RetrieveChunkSearchForAutoTRGeneration_V2";

        public usp_PartStoreroom_RetrieveChunkSearchForAutoTRGeneration_V2()
        {
        }

        public static List<b_PartStoreroom> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_PartStoreroom obj
       )
        {
            List<b_PartStoreroom> records = new List<b_PartStoreroom>();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "offset1", obj.offset1, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "nextrow", obj.nextrow, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "StoreroomIdList", obj.StoreroomIdList.TrimStart(','), 1073741823);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_PartStoreroom tmpPurchaseRequest = b_PartStoreroom.ProcessRowChunkSearchForAutoTRGeneration(reader);
                    records.Add(tmpPurchaseRequest);
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
