using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    public class usp_ShoppingCart_WorkBenchRetrieveAll
    {
        private static string STOREDPROCEDURE_NAME = "usp_ShoppingCart_ApproveWorkBenchRetrieveAll";

        public usp_ShoppingCart_WorkBenchRetrieveAll()
        {
        }

        /// <summary>
        /// Static method to call the usp_PrevMaintTask_RetrieveByPrevMaintMasterId stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_ShoppingCart> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_ShoppingCart obj

        )
        {
            List<b_ShoppingCart> records = new List<b_ShoppingCart>();
            SqlDataReader reader = null;
            b_ShoppingCart record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Created", obj.Created, 200);
            command.SetStringInputParameter(SqlDbType.NVarChar, "StatusDrop", obj.StatusDrop, 200);
            // RKL - 2016-11-07 - Send PersonnelId - not UserInfoId
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    record = new b_ShoppingCart();
                    record.LoadFromDatabaseExtended(reader);
                    records.Add(record);
                }
            }
            catch (Exception ex)
            {
                throw;
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

            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }
    }
}
