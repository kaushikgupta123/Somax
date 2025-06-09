using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Database;
using Database.Business;
using System.Collections;

namespace Database.StoredProcedure
{

    public class usp_ShoppingCartLineItem_RetrieveByPartId
    {
        private static string STOREDPROCEDURE_NAME = "usp_ShoppingCartLineItem_RetrieveByPartId";

        /// <summary>
        /// Constructor
        /// </summary>
        public usp_ShoppingCartLineItem_RetrieveByPartId()
        {
        }


        public static ArrayList CallStoredProcedure(
            SqlCommand command,
            Database.SqlClient.ProcessRow<b_ShoppingCartLineItem> processRow,
            long callerUserInfoId,
            string callerUserName,
            b_ShoppingCartLineItem b_ShoppingCartLineItem
        )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_ShoppingCartLineItem record = null;
            SqlParameter RETURN_CODE_parameter = null;
          
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", b_ShoppingCartLineItem.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", b_ShoppingCartLineItem.PartId);
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
            catch (Exception ex) { }
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
