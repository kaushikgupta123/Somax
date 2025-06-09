using System;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_RetrieveFromPersonnelAndUserdetailsByUserInfoId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_RetrieveFromPersonnelAndUserdetailsByUserInfoId_V2";

        public usp_RetrieveFromPersonnelAndUserdetailsByUserInfoId_V2()
        {
        }

        public static b_UserDetails CallStoredProcedure(
          SqlCommand command,
          long callerUserInfoId,
          string callerUserName,
          b_UserDetails obj
      )
        {
            b_UserDetails records = new b_UserDetails();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records = b_UserDetails.ProcessRowForPersonnelAndUserdetails(reader);
                }
                // 
                //reader.NextResult();

                //while (reader.Read())
                //{
                //    // Add the record to the list.
                //    records.Add(reader.GetString(0));
                //}
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
