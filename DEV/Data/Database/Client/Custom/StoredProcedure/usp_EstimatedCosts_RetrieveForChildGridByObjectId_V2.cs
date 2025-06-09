using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_EstimatedCosts_RetrieveForChildGridByObjectId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_EstimatedCosts_RetrieveForChildGridByObjectId_V2";

        public usp_EstimatedCosts_RetrieveForChildGridByObjectId_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_EstimatedCosts_RetrieveForChildGridByObjectId_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_EstimatedCosts> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_EstimatedCosts obj

        )
        {
            List<b_EstimatedCosts> records = new List<b_EstimatedCosts>();
            SqlDataReader reader = null;
            b_EstimatedCosts record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "ObjectId", obj.ObjectId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ObjectType", obj.ObjectType, 15);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    record = new b_EstimatedCosts();
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
