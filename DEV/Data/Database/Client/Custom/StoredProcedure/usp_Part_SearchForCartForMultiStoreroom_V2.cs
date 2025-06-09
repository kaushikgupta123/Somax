using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Part_SearchForCartForMultiStoreroom_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Part_SearchForCartForMultiStoreroom_V2";

        public usp_Part_SearchForCartForMultiStoreroom_V2()
        {
        }

        public static List<b_Part> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_Part obj
   )
        {
            List<b_Part> records = new List<b_Part>();
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
            command.SetInputParameter(SqlDbType.BigInt, "VendorId", obj.VendorId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "Offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "Nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 500);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    //// Add the record to the list.
                    records.Add(b_Part.ProcessRowSearchForCart(reader));
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
