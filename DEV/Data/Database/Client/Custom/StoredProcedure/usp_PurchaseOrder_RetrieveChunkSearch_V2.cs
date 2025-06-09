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
    public class usp_PurchaseOrder_RetrieveChunkSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseOrder_RetrieveChunkSearch_V2";

        public usp_PurchaseOrder_RetrieveChunkSearch_V2()
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
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 30);
            
            command.SetStringInputParameter(SqlDbType.NVarChar, "offset1", obj.offset1, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "nextrow", obj.nextrow, 10);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 500);            
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 500);            
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorClientLookupId", obj.VendorClientLookupId, 500);
            command.SetStringInputParameter(SqlDbType.VarChar, "VendorName", obj.VendorName, 500);           
            command.SetStringInputParameter(SqlDbType.NVarChar, "Attention", obj.Attention, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PhoneNo", obj.VendorPhoneNumber, 500); 
            command.SetStringInputParameter(SqlDbType.NVarChar, "Reason", obj.Reason, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Buyer", obj.Buyer_PersonnelName, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TotalCost", Convert.ToString(obj.TtlCost), 500);
            command.SetInputParameter(SqlDbType.BigInt, "FilterValue", obj.FilterValue);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 800);
            //v2-347
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteStartDateVw", obj.CompleteStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteEndDateVw", obj.CompleteEndDateVw, 500);
            //V2-364
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateStartDateVw", obj.CreateStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateEndDateVw", obj.CreateEndDateVw, 500);
            //V2-364
            command.SetStringInputParameter(SqlDbType.NVarChar, "StartCompleteDate", obj.StartCompleteDate, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "EndCompleteDate", obj.EndCompleteDate, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "StartCreateDate", obj.StartCreateDate, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "EndCreateDate", obj.EndCreateDate, 500);
            command.SetInputParameter(SqlDbType.DateTime, "Required", obj.Required); //V2-1171




            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();                
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_PurchaseOrder tmpPurchaseOrder = b_PurchaseOrder.ProcessRowForChunkSearch(reader);
                    tmpPurchaseOrder.ClientId = obj.ClientId;
                    records.Add(tmpPurchaseOrder);
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

