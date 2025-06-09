using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    class usp_Storeroom_RetrieveForChunkSearch_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Storeroom_RetrieveForChunkSearch_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Storeroom_RetrieveForChunkSearch_V2()
        {
        }
        public static List<b_Storeroom> CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_Storeroom obj
        )
        {
            List<b_Storeroom> records = new List<b_Storeroom>();
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
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.Case);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.offset1);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.nextrow);

            command.SetStringInputParameter(SqlDbType.NVarChar, "Name", obj.Name, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 800);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_Storeroom tmpStoreroom = b_Storeroom.ProcessRowForChunkSearch(reader);
                    records.Add(tmpStoreroom);
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

