/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Review Changes
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Business;
using System.Data.SqlClient;
using System.Data;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PurchaseOrder_ValidateByVendorPartNumberPurchaseRequestUnitOfMeasure
    {
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseOrder_ValidateByVendorPartNumberPurchaseRequestUnitOfMeasure";
         /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PurchaseOrder_ValidateByVendorPartNumberPurchaseRequestUnitOfMeasure()
        {
        }

        
        public static List<b_StoredProcValidationError> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PurchaseOrder obj
        )
        {
            List<b_StoredProcValidationError> records = new List<b_StoredProcValidationError>();
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
            command.SetInputParameter(SqlDbType.BigInt, "POImportId", obj.POImportId);

            try
            {
              // Execute stored procedure.
              reader = command.ExecuteReader();

              // Loop through dataset.
              while (reader.Read())
              {
                // Add the record to the list.
                records.Add((b_StoredProcValidationError)b_StoredProcValidationError.ProcessRow(reader));
              }

            }
            catch
            {
              Exception ex;
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
