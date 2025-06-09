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
    public class usp_PurchaseRequest_RetrieveChunkSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseRequest_RetrieveChunkSearch_V2";

        public usp_PurchaseRequest_RetrieveChunkSearch_V2()
        {
        }

        public static List<b_PurchaseRequest> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_PurchaseRequest obj
       )
        {
            List<b_PurchaseRequest> records = new List<b_PurchaseRequest>();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 30);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "offset1", obj.offset1, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "nextrow", obj.nextrow, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Reason", obj.Reason, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Creator_PersonnelName", obj.Creator_PersonnelName, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorClientLookupId", obj.VendorClientLookupId, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorName", obj.VendorName, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PurchaseOrderClientLookupId", obj.PurchaseOrderClientLookupId, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Processed_PersonnelName", obj.Processed_PersonnelName, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateStartDate", obj.CreateStartDate, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateEndDate", obj.CreateEndDate, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ProcessedStartDate", obj.ProcessedStartDate, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ProcessedEndDate", obj.ProcessedEndDate, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateStartDateVw", obj.CreateStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateEndDateVw", obj.CreateEndDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ProcessedStartDateVw", obj.ProcessedStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ProcessedEndDateVw", obj.ProcessedEndDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CancelandDeniedStartDateVw", obj.CancelandDeniedStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CancelandDeniedEndDateVw", obj.CancelandDeniedEndDateVw, 500);



            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();               
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_PurchaseRequest tmpPurchaseRequest = b_PurchaseRequest.ProcessRowForPurchaseRequestRetriveAllForSearch(reader);
                    tmpPurchaseRequest.ClientId = obj.ClientId;
                    records.Add(tmpPurchaseRequest);
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
