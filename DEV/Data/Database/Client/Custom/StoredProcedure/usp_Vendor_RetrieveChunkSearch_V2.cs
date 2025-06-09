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
* 
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
    public class usp_Vendor_RetrieveChunkSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Vendor_RetrieveChunkSearch_V2";

        public usp_Vendor_RetrieveChunkSearch_V2()
        {
        }

        public static List<b_Vendor> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_Vendor obj
       )
        {
            List<b_Vendor> records = new List<b_Vendor>();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 10);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.offset1);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.nextrow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorId", obj.ClientLookupId, 31);            
            command.SetStringInputParameter(SqlDbType.NVarChar, "Name", obj.Name, 67);            
            command.SetStringInputParameter(SqlDbType.NVarChar, "AddressCity", obj.AddressCity, 67);
            command.SetStringInputParameter(SqlDbType.VarChar, "AddressState", obj.AddressState, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type,31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Terms", obj.Terms, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "FOBCode", obj.FOBCode, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "IsExternal", obj.External, 2);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 800);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();               
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_Vendor tmpVendor = b_Vendor.ProcessRowForChunkSearch(reader);
                    tmpVendor.ClientId = obj.ClientId;
                    records.Add(tmpVendor);
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

