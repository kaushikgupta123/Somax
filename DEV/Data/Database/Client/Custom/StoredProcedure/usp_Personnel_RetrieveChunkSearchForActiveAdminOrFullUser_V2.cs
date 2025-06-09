using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;


namespace Database.StoredProcedure
{
    public class usp_Personnel_RetrieveChunkSearchForActiveAdminOrFullUser_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Personnel_RetrieveChunkSearchForActiveAdminOrFullUser_V2";

        public usp_Personnel_RetrieveChunkSearchForActiveAdminOrFullUser_V2()
        {
        }

        public static List<b_Personnel> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_Personnel obj
       )
        {
            List<b_Personnel> records = new List<b_Personnel>();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 30);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 200);
            command.SetStringInputParameter(SqlDbType.NVarChar, "NameFirst", obj.NameFirst, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "NameLast", obj.NameLast, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 800);


            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_Personnel tmpPersonnel = b_Personnel.ProcessRowForRetriveLookupListActiveAdminOrFullUser(reader);
                    tmpPersonnel.ClientId = obj.ClientId;
                    records.Add(tmpPersonnel);
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
