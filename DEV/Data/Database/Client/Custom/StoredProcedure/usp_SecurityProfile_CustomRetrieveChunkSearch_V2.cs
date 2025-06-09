using Database.Business;
using Database.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_SecurityProfile_CustomRetrieveChunkSearch_V2
    {

        private static string STOREDPROCEDURE_NAME = "usp_SecurityProfile_CustomRetrieveChunkSearch_V2";

        public usp_SecurityProfile_CustomRetrieveChunkSearch_V2()
        {
        }

        public static ArrayList CallStoredProcedure(
         SqlCommand command,
         Database.SqlClient.ProcessRow<b_SecurityProfile> processRow,
         long callerUserInfoId,
         string callerUserName,
        b_SecurityProfile obj
     )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_SecurityProfile record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);            
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderDirection, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.Page);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.ResultsPerPage);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 800);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Name", obj.Name, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 255);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = processRow(reader);

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
