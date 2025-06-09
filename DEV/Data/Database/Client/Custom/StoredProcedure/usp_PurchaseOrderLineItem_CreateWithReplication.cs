/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* usp_PurchaseOrderLineItem_CreateWithReplication.cs (stored procedure calling code)
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2015-Mar-18 SOM-608  Roger Lawton       Added 
****************************************************************************************************
*/
using System;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_PurchaseOrderLineItem_Create stored procedure.
    /// </summary>
    public class usp_PurchaseOrderLineItem_CreateWithReplication
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseOrderLineItem_CreateWithReplication";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PurchaseOrderLineItem_CreateWithReplication()
        {
        }

        /// <summary>
        /// Static method to call the usp_PurchaseOrderLineItem_CreateWithReplication stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure (
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PurchaseOrderLineItem obj
        )
        {
            SqlParameter        RETURN_CODE_parameter = null;
            int                 retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetOutputParameter(SqlDbType.BigInt, "PurchaseOrderLineItemId");
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseOrderId", obj.PurchaseOrderId);
            command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            command.SetInputParameter(SqlDbType.BigInt, "AccountId", obj.AccountId);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId", obj.ChargeToId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetInputParameter(SqlDbType.BigInt, "CompleteBy_PersonnelId", obj.CompleteBy_PersonnelId);
            command.SetInputParameter(SqlDbType.DateTime2, "CompleteDate", obj.CompleteDate);
            command.SetInputParameter(SqlDbType.BigInt, "Creator_PersonnelId", obj.Creator_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetInputParameter(SqlDbType.DateTime2, "EstimatedDelivery", obj.EstimatedDelivery);
            command.SetInputParameter(SqlDbType.Int, "LineNumber", obj.LineNumber);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetInputParameter(SqlDbType.BigInt, "PartStoreroomId", obj.PartStoreroomId);
            command.SetInputParameter(SqlDbType.BigInt, "PRCreator_PersonnelId", obj.PRCreator_PersonnelId);
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseRequestId", obj.PurchaseRequestId);
            command.SetInputParameter(SqlDbType.Decimal, "OrderQuantity", obj.OrderQuantity);
            command.SetInputParameter(SqlDbType.Decimal, "OrderQuantityOriginal", obj.OrderQuantityOriginal);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetInputParameter(SqlDbType.Bit, "Taxable", obj.Taxable);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UnitOfMeasure", obj.UnitOfMeasure, 15);
            command.SetInputParameter(SqlDbType.Decimal, "UnitCost", obj.UnitCost);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PurchaseOrderLineItemId = (long)command.Parameters["@PurchaseOrderLineItemId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int) RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}