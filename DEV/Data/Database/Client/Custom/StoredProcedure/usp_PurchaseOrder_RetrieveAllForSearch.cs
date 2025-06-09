/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person          Description
* =========== ======== ================ ============================================================
* 2016-Aug-17 SOM-1049 Roger Lawton     Changed from CustomQueryDisplayId to CaseNo
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
   public class usp_PurchaseOrder_RetrieveAllForSearch
    {
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseOrder_RetrieveAllForSearch";

        public usp_PurchaseOrder_RetrieveAllForSearch()
        {
        }

        public static List<b_PurchaseOrder> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_PurchaseOrder obj
       )
        {
            List<b_PurchaseOrder> records = new List<b_PurchaseOrder>();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "DateRange", obj.DateRange, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DateColumn", obj.DateColumn, 30);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetInputParameter(SqlDbType.BigInt, "FilterValue", obj.FilterValue);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_PurchaseOrder tmpPurchaseOrders = b_PurchaseOrder.ProcessRowForPurchaseOrderRetriveAllForSearch(reader);
                    tmpPurchaseOrders.ClientId = obj.ClientId;
                    records.Add(tmpPurchaseOrders);
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
