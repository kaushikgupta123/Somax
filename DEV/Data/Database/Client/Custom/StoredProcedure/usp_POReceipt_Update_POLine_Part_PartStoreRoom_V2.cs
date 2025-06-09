using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
    public class usp_POReceipt_Update_POLine_Part_PartStoreRoom_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_POReceipt_Update_POLine_Part_PartStoreRoom_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_POReceipt_Update_POLine_Part_PartStoreRoom_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_PrevMaintTask_UpdateByPK stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_POReceiptItem obj
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
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.POPart.PartId);
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseOrderLineItemId", obj.PurchaseOrderLineItemId);
            command.SetInputParameter(SqlDbType.BigInt, "PartStoreroomId", obj.POPartStoreRoom.PartStoreroomId);
            command.SetInputParameter(SqlDbType.BigInt, "ReceivedBy_PersonnelId", obj.POHeader.ReceiveBy_PersonnelID);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.POlineItem.Status, 15);
            command.SetInputParameter(SqlDbType.Decimal, "AppliedCost", obj.POPart.AppliedCost);
            command.SetInputParameter(SqlDbType.Decimal, "AverageCost", obj.POPart.AverageCost);
            // SOM-529
            decimal quantity = 0.0M;
            if (obj.Reversed)
            {
                quantity = obj.QuantityReceived * -1;
            }
            else
            {
                quantity = obj.QuantityReceived;
            }
            command.SetInputParameter(SqlDbType.Decimal, "QtyReceived", quantity);
            // Execute stored procedure.
            command.ExecuteNonQuery();

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
