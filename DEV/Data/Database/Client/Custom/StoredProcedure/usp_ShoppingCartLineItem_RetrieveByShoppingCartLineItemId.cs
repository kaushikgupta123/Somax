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
using Database.SqlClient;
using Database.Business;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    public class usp_ShoppingCartLineItem_RetrieveByShoppingCartLineItemId
    {
        private static string STOREDPROCEDURE_NAME = "usp_ShoppingCartLineItem_RetrieveByShoppingCartLineItemId";

        public usp_ShoppingCartLineItem_RetrieveByShoppingCartLineItemId()
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
       

        public static ArrayList CallStoredProcedure(
           SqlCommand command,
           Database.SqlClient.ProcessRow<b_ShoppingCartLineItem> processRow,
           long callerUserInfoId,
           string callerUserName,
           b_ShoppingCartLineItem obj
       )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_ShoppingCartLineItem record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "ShoppingCartLineItemId", obj.ShoppingCartLineItemId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = processRow(reader);

                    // Add the record to the list.
                    records.Add(record);
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
