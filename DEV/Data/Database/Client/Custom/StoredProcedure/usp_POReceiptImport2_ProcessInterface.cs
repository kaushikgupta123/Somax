/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2017 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date        Log Id     Person               Description
 * =========== ========== ==================== =================================

 ******************************************************************************
 */

using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_Equipment_Create stored procedure.
    /// </summary>
    public class usp_POReceiptImport2_ProcessInterface
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_POReceiptImport2_ProcessInterface";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_POReceiptImport2_ProcessInterface()
        {
        }

        /// <summary>
        /// Static method to call the usp_Equipment_CreateByForeignKeys stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_POReceiptImport2 obj

        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "POReceiptImport2Id",obj.POReceiptImport2Id);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.SiteId);
            command.SetOutputParameter(SqlDbType.BigInt, "PurchaseOrderLineItemId");
            command.SetOutputParameter(SqlDbType.Decimal, "ReceivedQuantity");
            command.SetOutputParameter(SqlDbType.Bit, "SendAlert");

            // Execute stored procedure.
            command.ExecuteNonQuery();

            if (!string.IsNullOrEmpty(command.Parameters["@POReceiptImport2Id"].Value.ToString()))
            {
                obj.POReceiptImport2Id = (long)command.Parameters["@POReceiptImport2Id"].Value;
            }
            if (!string.IsNullOrEmpty(command.Parameters["@PurchaseOrderLineItemId"].Value.ToString()))
            {
                obj.PurchaseOrderLineItemId = (long)command.Parameters["@PurchaseOrderLineItemId"].Value;
            }
            if (!string.IsNullOrEmpty(command.Parameters["@ReceivedQuantity"].Value.ToString()))
            {
                obj.ReceivedQuantity = (decimal)command.Parameters["@ReceivedQuantity"].Value;                 
            }
            if (!string.IsNullOrEmpty(command.Parameters["@SendAlert"].Value.ToString()))
            {
                obj.SendAlert = (bool)command.Parameters["@SendAlert"].Value;
            }
            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);




        }
    }
}