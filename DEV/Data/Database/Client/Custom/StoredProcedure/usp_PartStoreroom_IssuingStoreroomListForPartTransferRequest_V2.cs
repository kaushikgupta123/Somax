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
    public class usp_PartStoreroom_IssuingStoreroomListForPartTransferRequest_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PartStoreroom_IssuingStoreroomListForPartTransferRequest_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PartStoreroom_IssuingStoreroomListForPartTransferRequest_V2()
        {

        }
        public static List<b_PartStoreroom> CallStoredProcedure(
      SqlCommand command,
      long callerUserInfoId,
      string callerUserName,
      b_PartStoreroom obj
  )
        {
            List<b_PartStoreroom> records = new List<b_PartStoreroom>();
            // ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_PartStoreroom record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {

                    records.Add(b_PartStoreroom.ProcessPartStoreroomRetrieveForPartTransferRequest(reader));
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
