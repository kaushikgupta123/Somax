using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Database.Business;
using Database.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_VendorInsurance_RetrieveChunkSearchByVendorId_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_VendorInsurance_RetrieveChunkSearchByVendorId_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_VendorInsurance_RetrieveChunkSearchByVendorId_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_PartStoreroom_RetrieveForChildGridByPartId_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static List<b_VendorInsurance> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_VendorInsurance obj
        )
        {
            List<b_VendorInsurance> records = new List<b_VendorInsurance>();
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "offset1", obj.offset1, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "nextrow", obj.nextrow, 30);
            command.SetInputParameter(SqlDbType.BigInt, "VendorId", obj.VendorId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_VendorInsurance tmpMaterialRequest = b_VendorInsurance.ProcessRowForVendorInsuranceChunkSearchSearchByVendorId(reader);
                    tmpMaterialRequest.ClientId = obj.ClientId;
                    records.Add(tmpMaterialRequest);
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
            return records;
        }
    }
}
