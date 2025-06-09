using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    public class usp_PRPrint_RetrieveAllByPurchaseRequest_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PRPrint_RetrieveAllByPurchaseRequest_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PRPrint_RetrieveAllByPurchaseRequest_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_Equipment_RetrieveChunkSearch_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static b_PurchaseRequest CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PurchaseRequest obj
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "PurchaseRequestIDList", obj.PurchaseRequestIDList.TrimStart(','), 1073741823);
            command.CommandTimeout = 300;

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                obj.listOfPR = new List<b_PurchaseRequest>();
                while (reader.Read())
                {
                    b_PurchaseRequest tmpPurchaseRequests = b_PurchaseRequest.ProcessRowForPurchaseRequestPrint(reader);
                    obj.listOfPR.Add(tmpPurchaseRequests);
                }
                reader.NextResult();
                obj.listOfPRHeaderUDF = new List<b_PRHeaderUDF>();
                while (reader.Read())
                {
                    b_PRHeaderUDF tmpPRHeaderUDF = b_PRHeaderUDF.ProcessRowForPRHeaderUDFPrint(reader);
                    obj.listOfPRHeaderUDF.Add(tmpPRHeaderUDF);
                }
                reader.NextResult();

                obj.listOfPRLI = new List<b_PurchaseRequestLineItem>();
                while (reader.Read())
                {
                    b_PurchaseRequestLineItem tmpPurchaseRequestLineItem = b_PurchaseRequestLineItem.ProcessRowFortmpPurchaseRequestLineItemPrint(reader);
                    obj.listOfPRLI.Add(tmpPurchaseRequestLineItem);
                }
                reader.NextResult();
                obj.listOfPRLineUDF = new List<b_PRLineUDF>();
                while (reader.Read())
                {
                    b_PRLineUDF tmpPartHistory = b_PRLineUDF.ProcessRowForPRprLineUDFPrint(reader);
                    obj.listOfPRLineUDF.Add(tmpPartHistory);
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
