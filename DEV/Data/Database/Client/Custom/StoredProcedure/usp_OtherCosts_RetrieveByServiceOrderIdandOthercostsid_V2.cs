using Database.Business;
using Database.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    class usp_OtherCosts_RetrieveByServiceOrderIdandOthercostsid_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_OtherCosts_RetrieveByServiceOrderIdandOthercostsid_V2";

        public usp_OtherCosts_RetrieveByServiceOrderIdandOthercostsid_V2()
        {
        }

        public static List<b_OtherCosts> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_OtherCosts obj
        )
        {
            List<b_OtherCosts> records = new List<b_OtherCosts>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ObjectType", obj.ObjectType,30);
            command.SetInputParameter(SqlDbType.BigInt, "ObjectId", obj.ObjectId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ObjectType_Secondary", obj.ObjectType_Secondary, 30);
            command.SetInputParameter(SqlDbType.BigInt, "ObjectId_Secondary", obj.ObjectId_Secondary);
            command.SetInputParameter(SqlDbType.BigInt, "OtherCostId", obj.OtherCostsId);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_OtherCosts)b_OtherCosts.ProcessRowForOtherCostsCrossReference(reader));
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
