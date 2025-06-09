using System;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PartHistory_PartIssueForMultiStoreroom_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PartHistory_PartIssueForMultiStoreroom_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PartHistory_PartIssueForMultiStoreroom_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_PartHistory_PartIssueForMultiStoreroom_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>

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
            command.SetStringInputParameter(SqlDbType.VarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType_Primary", obj.ChargeType_Primary, 15);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId_Primary", obj.ChargeToId_Primary);
            command.SetInputParameter(SqlDbType.Decimal, "TransactionQuantity", obj.TransactionQuantity);
            command.SetInputParameter(SqlDbType.Bit, "PerformAdjustment", obj.IsPerformAdjustment);
            //command.SetInputParameter(SqlDbType.BigInt, "PerformedById", obj.PersonnelId);
            //command.SetInputParameter(SqlDbType.BigInt, "RequestorId", obj.PersonnelId);
            command.SetInputParameter(SqlDbType.BigInt, "PerformedById", obj.PerformedById);
            command.SetInputParameter(SqlDbType.BigInt, "RequestorId", obj.RequestorId);
            command.SetOutputParameter(SqlDbType.BigInt, "PartHistoryId");
            command.SetStringInputParameter(SqlDbType.NVarChar, "Comments", obj.Comments, 254);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);


            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PartHistoryId = command.Parameters["@PartHistoryId"].Value == null ? 0 : (long)command.Parameters["@PartHistoryId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
