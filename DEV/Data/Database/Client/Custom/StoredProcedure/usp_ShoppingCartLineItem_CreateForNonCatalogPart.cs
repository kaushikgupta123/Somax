/*
 **************************************************************************************************
 * PROPRIETARY DATA 
 **************************************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 **************************************************************************************************
 * Copyright (c) 2012 - 2018 by SOMAX Inc.
 * All rights reserved. 
 **************************************************************************************************
 * Date        Log Id   Person          Description
 * =========== ======== =============== ===========================================================
 * 2018-Apr-19 SOM-1593 Roger Lawton    Change quantity from int to decimal
 **************************************************************************************************
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
    public class usp_ShoppingCartLineItem_CreateForNonCatalogPart
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_ShoppingCartLineItem_CreateForNonCatalogPart";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_ShoppingCartLineItem_CreateForNonCatalogPart()
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
             b_ShoppingCartLineItem obj
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
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetOutputParameter(SqlDbType.BigInt, "ShoppingCartLineItemId");
            command.SetInputParameter(SqlDbType.BigInt, "ShoppingCartId", obj.ShoppingCartId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetInputParameter(SqlDbType.DateTime2, "RequiredDate", obj.RequiredDate);
            command.SetInputParameter(SqlDbType.Int, "LineNumber", obj.LineNumber);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetInputParameter(SqlDbType.Decimal, "OrderQuantity", obj.OrderQuantity);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UnitofMeasure", obj.UnitofMeasure, 15);
            command.SetInputParameter(SqlDbType.Decimal, "UnitCost", obj.UnitCost);
            command.SetInputParameter(SqlDbType.BigInt, "VendorId", obj.VendorId);
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseRequestLineItemId", obj.PurchaseRequestLineItemId);
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseOrderLineItemId", obj.PurchaseOrderLineItemId);
            command.SetInputParameter(SqlDbType.BigInt, "VendorCatalogItemId", obj.VendorCatalogItemId);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId", obj.ChargeToId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetInputParameter(SqlDbType.BigInt, "AccountId", obj.AccountId);
            command.SetInputParameter(SqlDbType.Decimal, "PurchaseQuantity", obj.PurchaseQuantity);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PurchaseUOM", obj.PurchaseUOM, 15);
            command.SetInputParameter(SqlDbType.Decimal, "PurchaseCost", obj.PurchaseCost);
            command.SetInputParameter(SqlDbType.Decimal, "UOMConversion", obj.UOMConversion);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Category", obj.Category, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToClientLookupId", obj.ChargeToClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PartCatagoryClientLookupID", obj.Category, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VendorID_ClientLookupID", obj.VendorID_ClientLookupID, 31);



            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.ShoppingCartLineItemId = (long)command.Parameters["@ShoppingCartLineItemId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}