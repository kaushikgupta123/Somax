/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2014-Dec-20 SOM-451  Roger Lawton       Add Last Purchased Cost to screen 
* 2022-May-09 V2-692   Roger Lawton       Change SP used to update
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
    /// Access the usp_Equipment_UpdateByPK stored procedure.
    /// </summary>
    public class usp_PartStorerooms_UpdateByPartId
    {
        /// <summary>
        /// Constants.
        /// </summary>
        //private static string STOREDPROCEDURE_NAME = "usp_PartStorerooms_UpdateByPartId";
        private static string STOREDPROCEDURE_NAME = "usp_PartStorerooms_UpdateByPartId_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PartStorerooms_UpdateByPartId()
        {
        }

        /// <summary>
        /// Static method to call the usp_PartStorerooms_UpdateByPartId_V2 stored procedure using SqlClient.
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
            SqlParameter updateIndexOut_parameter = null;
            SqlParameter storeroom_updateIndexOut_parameter = null;
            int retCode = 0;
            
            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
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
            command.SetInputParameter(SqlDbType.Int, "UpdateIndex", obj.UpdateIndex);
            command.SetInputParameter(SqlDbType.BigInt, "PartStoreroomId", obj.PartStoreroomId);
            command.SetInputParameter(SqlDbType.Int, "CountFrequency", obj.CountFrequency);
            command.SetInputParameter(SqlDbType.DateTime2, "LastCounted", obj.LastCounted);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Location1_1", obj.Location1_1, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Location1_2", obj.Location1_2, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Location1_3", obj.Location1_3, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Location1_4", obj.Location1_4, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Location1_5", obj.Location1_5, 31);
            command.SetInputParameter(SqlDbType.Decimal, "QtyMaximum", obj.QtyMaximum);
            command.SetInputParameter(SqlDbType.Decimal, "QtyOnHand", obj.QtyOnHand);
            command.SetInputParameter(SqlDbType.Decimal, "QtyReorderLevel", obj.QtyReorderLevel);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ReorderMethod", obj.ReorderMethod, 15);
            command.SetInputParameter(SqlDbType.Int, "Storeroom_UpdateIndex", obj.Storeroom_UpdateIndex);
            command.SetInputParameter(SqlDbType.Bit, "AutoPurch", obj.AutoPurch);
            // Setup updateIndexOut parameter.
            updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
            updateIndexOut_parameter.Direction = ParameterDirection.Output;
            // Setup storeroom updateIndexOut parameter.
            storeroom_updateIndexOut_parameter = command.Parameters.Add("@Storeroom_UpdateIndexOut", SqlDbType.Int);
            storeroom_updateIndexOut_parameter.Direction = ParameterDirection.Output;

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.UpdateIndex = (int)updateIndexOut_parameter.Value;
            obj.Storeroom_UpdateIndex = (int)storeroom_updateIndexOut_parameter.Value; 

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
