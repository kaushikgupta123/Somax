using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    public class usp_PORPrint_AllRetrieveByPurchaseOrderIdAndPOReceiptHeaderId_V2
    {        /// <summary>
             /// Constants.
             /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PORPrint_AllRetrieveByPurchaseOrderIdAndPOReceiptHeaderId_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PORPrint_AllRetrieveByPurchaseOrderIdAndPOReceiptHeaderId_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_PORPrint_AllRetrieveByPurchaseOrderIdAndPOReceiptHeaderId_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static b_POReceiptItem CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_POReceiptItem obj
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
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseOrderId", obj.PurchaseOrderId);
            command.SetInputParameter(SqlDbType.BigInt, "POReceiptHeaderId", obj.POReceiptHeaderId);

            command.CommandTimeout = 300;

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                obj.POPurchaseOrder = new b_PurchaseOrder();
                while (reader.Read())
                {
                    b_PurchaseOrder tmpPurchaseOrders = b_PurchaseOrder.ProcessRowForPurchaseOrderPrintforPOR(reader);
                    obj.POPurchaseOrder = tmpPurchaseOrders;
                }
                reader.NextResult();
                obj.POReceiptItemlist = new List<b_POReceiptItem>();
                while (reader.Read())
                {
                    b_POReceiptItem tmpPOReceiptItem = b_POReceiptItem.ProcessRowForPOReceiptRetrieveAllPrint(reader);
                    obj.POReceiptItemlist.Add(tmpPOReceiptItem);
                }
                reader.NextResult();
                obj.POHeaderUDF = new b_POHeaderUDF();
                while (reader.Read())
                {
                    b_POHeaderUDF tmpPOHeaderUDF = b_POHeaderUDF.ProcessRowForPurchaseOrderUDFPrint(reader);
                    obj.POHeaderUDF = tmpPOHeaderUDF;
                }
                reader.NextResult();
                obj.listOfNotes = new List<b_Notes>();
                while (reader.Read())
                {
                    b_Notes tmpPOHeaderUDF = (b_Notes)b_Notes.ProcessRowExtended(reader);
                    obj.listOfNotes.Add(tmpPOHeaderUDF);
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
