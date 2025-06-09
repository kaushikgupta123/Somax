using Database.Business;
using Database.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Storeroom_RetrieveChunkSearchLookupList_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Storeroom_RetrieveChunkSearchLookupList_V2";
        public usp_Storeroom_RetrieveChunkSearchLookupList_V2()
        {
        }

        public static List<b_Storeroom> CallStoredProcedure(SqlCommand command, long callerUserInfoId,
                                         string callerUserName, b_Storeroom obj)
        {
            List<b_Storeroom> records = new List<b_Storeroom>();
            b_Storeroom b_Storeroom = new b_Storeroom();
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Name", obj.Name, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 256);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Int, "Page", obj.offset1);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", obj.nextrow);
            command.SetStringInputParameter(SqlDbType.VarChar, "OrderColumn", obj.orderbyColumn, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "OrderDirection", obj.orderBy, 256);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);


            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    b_Storeroom = b_Storeroom.ProcessRowForChunkSearchLookupList(reader);
                    records.Add(b_Storeroom);
                }
            }
            catch (Exception ex)
            {

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
