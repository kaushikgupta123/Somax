using System;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PartHistory_UpdatePartCounts_V2
    {

        /// <summary>
        /// Constants
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PartHistory_UpdatePartCounts_V2";

        /// <summary>
        /// Constructor
        /// </summary>
        public usp_PartHistory_UpdatePartCounts_V2()
        {

        }
        public static void CallStoredProcedure(
      SqlCommand command,
      long callerUserInfoId,
      string callerUserName,
      b_PartHistory obj
      )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetInputParameter(SqlDbType.BigInt, "PerformById", obj.PerformedById);
            command.SetInputParameter(SqlDbType.Decimal, "QuantityCounted", obj.PartStoreroomQtyOnHand);
            command.SetOutputParameter(SqlDbType.BigInt, "PartHistoryId");
            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PartHistoryId = (long)command.Parameters["@PartHistoryId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }


    }
}
