using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    public class usp_POPrint_RetrieveAllByPurchaseOrder_V2
    {        /// <summary>
             /// Constants.
             /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_POPrint_RetrieveAllByPurchaseOrder_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_POPrint_RetrieveAllByPurchaseOrder_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_POPrint_RetrieveAllByPurchaseOrder_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static b_PurchaseOrder CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PurchaseOrder obj
        )
        {
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PurchaseOrderIDList", obj.PurchaseOrderIDList.TrimStart(','), 1073741823);
            command.CommandTimeout = 300;

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
              
                obj.listOfPO = new List<b_PurchaseOrder>();
                while (reader.Read())
                {
                    b_PurchaseOrder tmpPurchaseOrders = b_PurchaseOrder.ProcessRowForPurchaseOrderPrint(reader);
                    obj.listOfPO.Add(tmpPurchaseOrders);
                }
                reader.NextResult();
                obj.listOfPOHeaderUDF = new List<b_POHeaderUDF>();
                while (reader.Read())
                {
                    b_POHeaderUDF tmpPurchaseOrdersHeaderUDF = b_POHeaderUDF.ProcessRowForPurchaseOrderUDFPrint(reader);
                    obj.listOfPOHeaderUDF.Add(tmpPurchaseOrdersHeaderUDF);
                }
                reader.NextResult();
                obj.listOfPOLineItem = new List<b_PurchaseOrderLineItem>();
                while (reader.Read())
                {
                    b_PurchaseOrderLineItem tmpPOLineItem = b_PurchaseOrderLineItem.ProcessRowForPurchaseOrderLineItemPrint(reader);
                    obj.listOfPOLineItem.Add(tmpPOLineItem);
                }
                reader.NextResult();
                obj.listOfPOLineUDF = new List<b_POLineUDF>();
                while (reader.Read())
                {
                    b_POLineUDF tmpPOLineUDF = b_POLineUDF.ProcessRowForPurchaseOrderLineItemUDFPrint(reader);
                    obj.listOfPOLineUDF.Add(tmpPOLineUDF);
                }
                reader.NextResult();
                obj.listOfAttachment = new List<b_Attachment>();
                while (reader.Read())
                {
                    b_Attachment tmpAttachments = b_Attachment.ProcessRowForPOAttachmentPrint(reader);
                    obj.listOfAttachment.Add(tmpAttachments);
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
            return obj;
        }
    }
}
