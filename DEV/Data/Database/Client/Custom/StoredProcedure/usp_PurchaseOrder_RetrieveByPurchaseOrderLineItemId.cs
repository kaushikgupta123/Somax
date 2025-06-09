/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* b_PrevMaintTask.cs (Data Object)
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Jul-29 SOM-259  Roger Lawton       Removed the Process Row delegate
*                                         when using it - all b_PrevMaintTask items in the list 
*                                         had the same values - not sure why and don't currently 
*                                         have time to figure it out
****************************************************************************************************
*/

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PurchaseOrder_RetrieveByPurchaseOrderLineItemId
    {
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseOrder_RetrieveByPurchaseOrderLineItemId";

        public usp_PurchaseOrder_RetrieveByPurchaseOrderLineItemId()
        {
        }

        /// <summary>
        /// Static method to call the usp_PrevMaintTask_RetrieveByPrevMaintMasterId stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_PurchaseOrderLineItem> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PurchaseOrderLineItem obj
           
        )
        {
            List<b_PurchaseOrderLineItem> records = new List<b_PurchaseOrderLineItem>();
            SqlDataReader reader = null;
            b_PurchaseOrderLineItem record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseOrderId", obj.PurchaseOrderId);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                  record = new b_PurchaseOrderLineItem();
                  record.LoadFromDatabaseExtended(reader);
                  records.Add(record);
                }
            }
            catch (Exception ex)
            {
                throw;
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

            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }
    }
}
