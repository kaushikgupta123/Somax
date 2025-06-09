using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Personnel_RetrieveChunkSearchLookupList_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Personnel_RetrieveChunkSearchLookupList_V2";

        public usp_Personnel_RetrieveChunkSearchLookupList_V2()
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
            b_Personnel b_Personnel = new b_Personnel();
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;


            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PClientLookupId", obj.ClientLookupId, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "NameFirst", obj.NameFirst, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "NameLast", obj.NameLast, 255);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Int, "Page", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderColumn", obj.OrderbyColumn, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderDirection", obj.OrderBy, 255);


            try
            {

                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    b_Personnel = b_Personnel.ProcessRowForRetriveLookupListActive(reader);
                    records.Add(b_Personnel);
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
