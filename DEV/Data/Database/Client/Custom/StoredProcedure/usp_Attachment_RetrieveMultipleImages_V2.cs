using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Attachment_RetrieveMultipleImages_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Attachment_RetrieveMultipleImages_V2";
        public usp_Attachment_RetrieveMultipleImages_V2()
        {
        }
        public static ArrayList CallStoredProcedure(
           SqlCommand command,
           Database.SqlClient.ProcessRow<b_Attachment> processRow,
           long callerUserInfoId,
           string callerUserName,
           b_Attachment objid
       )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_Attachment record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", objid.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ObjectName", objid.ObjectName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ObjectId", objid.ObjectId);
            //command.SetInputParameter(SqlDbType.Bit, "Profile", objid.Profile);
            //command.SetInputParameter(SqlDbType.Bit, "Image", objid.Image);
            command.SetStringInputParameter(SqlDbType.NVarChar, "offset1", objid.offset1, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "nextrow", objid.nextrow, 30);

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
