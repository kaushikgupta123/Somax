using Database.Business;
using Database.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    class usp_UserData_RetrievePersonnelChild_V2
    {/// <summary>
     /// Constants.
     /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_UserData_RetrievePersonnelChild_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_UserData_RetrievePersonnelChild_V2()
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
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = STOREDPROCEDURE_NAME;
            command.Parameters.Clear();

            // Setup RETURN_CODE parameter.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.DefaultSiteId);            
            command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);            
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    records.Add(b_UserSearch.ProcessRowForUserCraftDetails(reader));
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
