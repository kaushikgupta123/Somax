using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;
using Database;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_Equipment_Parts_Xref_RetrieveByEquipmentId_V2 stored procedure.
    /// </summary>
    public class usp_Equipment_Parts_Xref_RetrieveByEquipmentId_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Equipment_Parts_Xref_RetrieveByEquipmentId_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Equipment_Parts_Xref_RetrieveByEquipmentId_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_Equipment_Parts_Xref_RetrieveByEquipmentId stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
        /// <param name="sessionId">System.Guid that contains the value of the @SessionId parameter</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_Equipment_Parts_Xref> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Equipment_Parts_Xref obj
        )
        {
            List<b_Equipment_Parts_Xref> records = new List<b_Equipment_Parts_Xref>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "EquipmentId", obj.EquipmentId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PartClientLookUpId", obj.PartClientLookUpId, 70);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 127);
            command.SetStringInputParameter(SqlDbType.NVarChar, "StockType", obj.StockType, 15);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_Equipment_Parts_Xref)b_Equipment_Parts_Xref.ProcessRowForEquipmentCrossReference(reader));
                }

                reader.NextResult();

                while (reader.Read())
                {
                    // Add the record to the list.
                    //records.Add(reader.GetString(0));
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