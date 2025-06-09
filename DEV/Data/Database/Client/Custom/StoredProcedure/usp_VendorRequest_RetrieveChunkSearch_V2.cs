using Database.Business;
using Database.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_VendorRequest_RetrieveChunkSearch_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_VendorRequest_RetrieveChunkSearch_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_VendorRequest_RetrieveChunkSearch_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_VendorRequest_RetrieveChunkSearch_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_VendorRequest> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
           b_VendorRequest VendorRequest

        )
        {
            List<b_VendorRequest> records = new List<b_VendorRequest>();
            SqlDataReader reader = null;
            b_VendorRequest record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

         

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", VendorRequest.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", VendorRequest.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Name", VendorRequest.Name, 126);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AddressCity", VendorRequest.AddressCity, 126);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AddressState", VendorRequest.AddressState, 126);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", VendorRequest.Type, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", VendorRequest.Status, 30);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", VendorRequest.CustomQueryDisplayId);
            command.SetInputParameter(SqlDbType.Int, "nextrow", VendorRequest.nextrow);
            command.SetInputParameter(SqlDbType.Int, "offset1", VendorRequest.offset1);
            command.SetStringInputParameter(SqlDbType.VarChar, "orderbyColumn", VendorRequest.orderbyColumn, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "orderBy", VendorRequest.orderBy, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", VendorRequest.SearchText, 800);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                //// Get the result count
                //while (reader.Read())
                //{
                //    ResultCount = reader.GetInt32(0);
                //}

                //reader.NextResult();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = (b_VendorRequest)b_VendorRequest.ProcessRowForRetrieveBySearchCriteria_V2(reader);

                    //// Add the record to the list.
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
