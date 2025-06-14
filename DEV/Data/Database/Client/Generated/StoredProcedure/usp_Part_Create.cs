/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2019 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND. MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
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
    /// Access the usp_Part_Create stored procedure.
    /// </summary>
    public class usp_Part_Create
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Part_Create";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Part_Create()
        {
        }

        /// <summary>
        /// Static method to call the usp_Part_Create stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Part obj
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
            command.SetOutputParameter(SqlDbType.BigInt, "PartId");
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "AreaId", obj.AreaId);
            command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 70);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ABCCode", obj.ABCCode, 7);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ABCStoreCost", obj.ABCStoreCost, 7);
            command.SetInputParameter(SqlDbType.BigInt, "AccountId", obj.AccountId);
            command.SetInputParameter(SqlDbType.BigInt, "AltPartId1", obj.AltPartId1);
            command.SetInputParameter(SqlDbType.BigInt, "AltPartId2", obj.AltPartId2);
            command.SetInputParameter(SqlDbType.BigInt, "AltPartId3", obj.AltPartId3);
            command.SetInputParameter(SqlDbType.Decimal, "AppliedCost", obj.AppliedCost);
            command.SetInputParameter(SqlDbType.Decimal, "AverageCost", obj.AverageCost);
            command.SetInputParameter(SqlDbType.Bit, "Consignment", obj.Consignment);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CostCalcMethod", obj.CostCalcMethod, 15);
            command.SetInputParameter(SqlDbType.Decimal, "CostMultiplier", obj.CostMultiplier);
            command.SetInputParameter(SqlDbType.Bit, "Critical", obj.Critical);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", obj.InactiveFlag);
            command.SetStringInputParameter(SqlDbType.NVarChar, "IssueUnit", obj.IssueUnit, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Manufacturer", obj.Manufacturer, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ManufacturerId", obj.ManufacturerId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "MSDSContainerCode", obj.MSDSContainerCode, 7);
            command.SetStringInputParameter(SqlDbType.NVarChar, "MSDSPressureCode", obj.MSDSPressureCode, 7);
            command.SetStringInputParameter(SqlDbType.NVarChar, "MSDSReference", obj.MSDSReference, 31);
            command.SetInputParameter(SqlDbType.Bit, "MSDSRequired", obj.MSDSRequired);
            command.SetStringInputParameter(SqlDbType.NVarChar, "MSDSTemperatureCode", obj.MSDSTemperatureCode, 7);
            command.SetInputParameter(SqlDbType.Bit, "NoEquipXref", obj.NoEquipXref);
            command.SetInputParameter(SqlDbType.Bit, "PrintNoLabel", obj.PrintNoLabel);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PurchaseText", obj.PurchaseText, 1073741823);
            command.SetInputParameter(SqlDbType.Bit, "RepairablePart", obj.RepairablePart);
            command.SetStringInputParameter(SqlDbType.NVarChar, "StockType", obj.StockType, 15);
            command.SetInputParameter(SqlDbType.Decimal, "TaxLevel1", obj.TaxLevel1);
            command.SetInputParameter(SqlDbType.Decimal, "TaxLevel2", obj.TaxLevel2);
            command.SetInputParameter(SqlDbType.Bit, "Taxable", obj.Taxable);
            command.SetInputParameter(SqlDbType.Bit, "Tool", obj.Tool);
            command.SetInputParameter(SqlDbType.Int, "Type", obj.Type);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UPCCode", obj.UPCCode, 31);
            command.SetInputParameter(SqlDbType.Bit, "UseCostMultiplier", obj.UseCostMultiplier);
            command.SetInputParameter(SqlDbType.Bit, "Chemical", obj.Chemical);
            command.SetInputParameter(SqlDbType.Bit, "AutoPurch", obj.AutoPurch);
            command.SetInputParameter(SqlDbType.BigInt, "PartMasterId", obj.PartMasterId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PrevClientLookupId", obj.PrevClientLookupId, 70);
            command.SetInputParameter(SqlDbType.BigInt, "DefaultStoreroom", obj.DefaultStoreroom);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PartId = (long)command.Parameters["@PartId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
