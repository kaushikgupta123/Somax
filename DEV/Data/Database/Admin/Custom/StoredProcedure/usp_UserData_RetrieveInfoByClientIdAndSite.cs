using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
    public class usp_UserData_RetrieveInfoByClientIdAndSite
    {
        private static string STOREDPROCEDURE_NAME = "usp_UserData_RetrieveInfoByClientIdAndSite";

        public usp_UserData_RetrieveInfoByClientIdAndSite()
         {
         }

        public static List<b_UserSearch> CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_UserSearch obj
      )
        {
            List<b_UserSearch> records = new List<b_UserSearch>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "DefaultSiteId", obj.DefaultSiteId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    //// Add the record to the list.
                    records.Add(b_UserSearch.ProcessRowForGetUserListFromAdmin(reader));
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
