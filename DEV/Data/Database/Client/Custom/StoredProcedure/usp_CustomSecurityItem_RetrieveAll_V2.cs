using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

using Database.Business;

namespace Database.StoredProcedure
{
    public class usp_CustomSecurityItem_RetrieveAll_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_CustomSecurityItem_RetrieveAll_V2";
        public usp_CustomSecurityItem_RetrieveAll_V2()
        {
        }

        public static List<b_SecurityItem> CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_SecurityItem obj
    )
        {
            List<b_SecurityItem> records = new List<b_SecurityItem>();
            SqlDataReader reader = null;
            b_SecurityItem record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);            
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);          
            command.SetInputParameter(SqlDbType.BigInt, "SecurityProfileId", obj.SecurityProfileId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SecurityItemTab", obj.SecurityItemTab, 20);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PackageLevel", obj.PackageLevel, 15);
            command.SetInputParameter(SqlDbType.Int, "ProductGrouping", obj.ProductGrouping);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = b_SecurityItem.ProcessRowForRetrivalAllByClientAndSecurityProfile(reader);

                    // Add the record to the list.                    
                    records.Add(record);
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
