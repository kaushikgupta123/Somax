/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date        Log Id     Person               Description
 * =========== ========== ==================== =================================
 * 2012-Oct-21            Roger Lawton         Add BIM Identifier
 * 2013-Apr-18            Nick Fuchs           Added custom Columns
 * 2013-Aug-04  201350051 Nick Fuchs           Added remaining custom columns
 * 2013-Aug-07            Roger Lawton         Removed Custom Columns
 ******************************************************************************
 */
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_Equipment_Create stored procedure.
    /// </summary>
    public class usp_POReceiptItem_CreateByPK

    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_POReceiptItem_CreateByPK";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_POReceiptItem_CreateByPK()
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
           b_POReceiptItem obj
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
            command.SetOutputParameter(SqlDbType.BigInt, "POReceiptItemId");
            command.SetOutputParameter(SqlDbType.BigInt, "PurchaseOrderId");
            command.SetInputParameter(SqlDbType.BigInt, "POReceiptHeaderId", obj.POReceiptHeaderId);
            command.SetInputParameter(SqlDbType.Int, "PurchaseOrderLineNumber", obj.PurchaseOrderLineNumber);
            command.SetInputParameter(SqlDbType.Decimal, "QuantityReceived", obj.QuantityReceived);
            command.SetInputParameter(SqlDbType.Decimal, "UnitCost", obj.UnitCost);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UnitOfMeasure", obj.UnitOfMeasure, 15);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.POReceiptItemId = (long)command.Parameters["@POReceiptItemId"].Value;
            obj.PurchaseOrderId = (long)command.Parameters["@PurchaseOrderId"].Value;
            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}