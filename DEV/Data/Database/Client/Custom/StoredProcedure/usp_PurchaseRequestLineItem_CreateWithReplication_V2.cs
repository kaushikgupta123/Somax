/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* b_PurchaseRequestLineItem.cs (Data Object)
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
    /// Access the usp_PurchaseRequestLineItem_Create stored procedure.
    /// </summary>
    public class usp_PurchaseRequestLineItem_CreateWithReplication_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseRequestLineItem_CreateWithReplication_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PurchaseRequestLineItem_CreateWithReplication_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_PurchaseRequestLineItem_CreateWithReplication stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure (
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PurchaseRequestLineItem obj
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
            command.SetOutputParameter(SqlDbType.BigInt, "PurchaseRequestLineItemId");
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseRequestId", obj.PurchaseRequestId);
            command.SetInputParameter(SqlDbType.BigInt, "AccountId", obj.AccountId);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToID", obj.ChargeToID);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetInputParameter(SqlDbType.BigInt, "Creator_PersonnelId", obj.Creator_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetInputParameter(SqlDbType.DateTime2, "RequiredDate", obj.RequiredDate);
            command.SetInputParameter(SqlDbType.Int, "LineNumber", obj.LineNumber);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetInputParameter(SqlDbType.BigInt, "PartStoreroomId", obj.PartStoreroomId);
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseOrderLineItemId", obj.PurchaseOrderLineItemId);
            command.SetInputParameter(SqlDbType.Decimal, "OrderQuantity", obj.OrderQuantity);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UnitofMeasure", obj.UnitofMeasure, 15);
            command.SetInputParameter(SqlDbType.Decimal, "UnitCost", obj.UnitCost);
            command.SetInputParameter(SqlDbType.Bit, "AutoGenerated", obj.AutoGenerated);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PurchaseUOM", obj.PurchaseUOM, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SupplierPartId", obj.SupplierPartId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SupplierPartAuxiliaryId", obj.SupplierPartAuxiliaryId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ManufacturerPartId", obj.ManufacturerPartId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Manufacturer", obj.Manufacturer, 31);
            command.SetInputParameter(SqlDbType.Decimal, "PurchaseQuantity", obj.PurchaseQuantity);
            command.SetInputParameter(SqlDbType.Decimal, "UOMConversion", obj.UOMConversion);
            command.SetInputParameter(SqlDbType.BigInt, "VendorCatalogItemId", obj.VendorCatalogItemId);
            command.SetInputParameter(SqlDbType.BigInt, "UNSPSC", obj.UNSPSC);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PurchaseRequestLineItemId = (long)command.Parameters["@PurchaseRequestLineItemId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int) RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}