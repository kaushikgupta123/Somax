using Database.Business;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_VendorAssetMgt_RetrieveForHeaderByVendorId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_VendorAssetMgt_RetrieveForHeaderByVendorId_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_VendorAssetMgt_RetrieveForHeaderByVendorId_V2()
        {
        }
        public static List<b_VendorAssetMgt> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_VendorAssetMgt obj
        )
        {
            List<b_VendorAssetMgt> records = new List<b_VendorAssetMgt>();
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "VendorId", obj.VendorId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_VendorAssetMgt tmpAssetMgt = b_VendorAssetMgt.ProcessRowForVendorAssetMgtHeaderByVendorId(reader);
                    tmpAssetMgt.ClientId = obj.ClientId;
                    records.Add(tmpAssetMgt);
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
