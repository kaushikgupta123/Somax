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
using  Database.Business;
using System.Collections.Generic;
using Database.SqlClient;
using System.Globalization;

namespace  Database.StoredProcedure
{
   public class usp_InvoiceMatchHeader_RetrieveChunkSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_InvoiceMatchHeader_RetrieveChunkSearch_V2";//V2-373

        public usp_InvoiceMatchHeader_RetrieveChunkSearch_V2()
        {
        }

        public static List<b_InvoiceMatchHeader> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_InvoiceMatchHeader obj
       )
        {
            List<b_InvoiceMatchHeader> records = new List<b_InvoiceMatchHeader>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;
            var ReceiptDate = obj.ReceiptDate.HasValue ? obj.ReceiptDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            var InvoiceDate= obj.InvoiceDate.HasValue ? obj.InvoiceDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 30);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "offset1", obj.offset1, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "nextrow", obj.nextrow, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorClientLookupId", obj.VendorClientLookupId, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorName", obj.VendorName, 167);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ReceiptDate", ReceiptDate, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "InvoiceDate", InvoiceDate, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "POClientLookupId", obj.POClientLookUpId, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteATPStartDateVw", obj.CompleteATPStartDateVw, 500);//V2-373
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteATPEndDateVw", obj.CompleteATPEndDateVw, 500);//V2-373
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompletePStartDateVw", obj.CompletePStartDateVw, 500);//V2-373
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompletePEndDateVw", obj.CompletePEndDateVw, 500);//V2-373
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateStartDateVw", obj.CreateStartDateVw, 500);//V2-1061
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateEndDateVw", obj.CreateEndDateVw, 500);//V2-1061
            try
            {               
                // Execute stored procedure.
                reader = command.ExecuteReader();               
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_InvoiceMatchHeader tmpWorkOrders = b_InvoiceMatchHeader.ProcessRowForRetriveChunkSearch(reader);
                    tmpWorkOrders.ClientId = obj.ClientId;
                    records.Add(tmpWorkOrders);
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
