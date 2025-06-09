using Database.Business;
using Database.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    class usp_UserData_RetrieveChunkSearch_Enterprise_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_UserData_RetrieveChunkSearch_Enterprise_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_UserData_RetrieveChunkSearch_Enterprise_V2()
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
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CaseNo);
            command.SetStringInputParameter(SqlDbType.VarChar, "SelectedSites", obj.SelectedSites, 100);
            command.SetStringInputParameter(SqlDbType.VarChar, "orderbyColumn", obj.OrderByColumn, 10);
            command.SetStringInputParameter(SqlDbType.VarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset", obj.Offset);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UserName", obj.UserName, 63);            
            command.SetStringInputParameter(SqlDbType.VarChar, "LastName", obj.LastName, 63);
            command.SetStringInputParameter(SqlDbType.VarChar, "FirstName", obj.FirstName, 63);
            command.SetStringInputParameter(SqlDbType.VarChar, "Email", obj.Email,63);
            command.SetInputParameter(SqlDbType.BigInt, "CraftId", obj.PersonnelCraftId);
            command.SetStringInputParameter(SqlDbType.VarChar, "SearchText", obj.SearchText, 800);
            command.SetStringInputParameter(SqlDbType.VarChar, "SecurityProfileIds", obj.SecurityProfileIds,500);
            command.SetStringInputParameter(SqlDbType.VarChar, "UserType", obj.UserType, 500);
            command.SetStringInputParameter(SqlDbType.VarChar, "Shift", obj.Shift, 500);
            command.SetInputParameter(SqlDbType.Bit, "IsActiveStatus", obj.IsActiveStatus);
            command.SetStringInputParameter(SqlDbType.VarChar, "EmployeeId", obj.EmployeeId, 31); //V2-1160
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add(b_UserSearch.ProcessRowForUserSearchList_Enterprise(reader));
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
