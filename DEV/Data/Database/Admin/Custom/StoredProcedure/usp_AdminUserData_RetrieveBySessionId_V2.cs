using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_AdminUserData_RetrieveBySessionId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_AdminUserData_RetrieveBySessionId_V2";

        public usp_AdminUserData_RetrieveBySessionId_V2()
        {
        }

        public static List<Object> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_UserDataSet obj
        )
        {
            List<Object> records = new List<Object>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.UniqueIdentifier, "SessionId", obj.SessionId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                Object tempRecord = null;

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    tempRecord = b_Client.ProcessRow(reader);
                }

                records.Add(tempRecord);
                tempRecord = null;

                reader.NextResult();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    tempRecord = b_UserInfo.ProcessRow(reader);
                }

                records.Add(tempRecord);
                tempRecord = null;

                reader.NextResult();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    tempRecord = b_LoginInfo.ProcessRow(reader);
                }

                records.Add(tempRecord);
                tempRecord = null;

                reader.NextResult();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    tempRecord = b_LoginAuditing.ProcessRowWithCreateDate(reader);
                }

                records.Add(tempRecord);

                // 
                reader.NextResult();

                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add(reader.GetString(0));
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