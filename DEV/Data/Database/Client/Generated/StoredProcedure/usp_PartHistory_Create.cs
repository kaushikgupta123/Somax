/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2021 by SOMAX Inc.
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
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_PartHistory_Create stored procedure.
    /// </summary>
    public class usp_PartHistory_Create
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PartHistory_Create";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PartHistory_Create()
        {
        }

        /// <summary>
        /// Static method to call the usp_PartHistory_Create stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PartHistory obj
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
            command.SetOutputParameter(SqlDbType.BigInt, "PartHistoryId");
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetInputParameter(SqlDbType.BigInt, "PartStoreroomId", obj.PartStoreroomId);
            command.SetInputParameter(SqlDbType.BigInt, "AccountId", obj.AccountId);
            command.SetInputParameter(SqlDbType.Decimal, "AverageCostBefore", obj.AverageCostBefore);
            command.SetInputParameter(SqlDbType.Decimal, "AverageCostAfter", obj.AverageCostAfter);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType_Primary", obj.ChargeType_Primary, 15);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId_Primary", obj.ChargeToId_Primary);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Comments", obj.Comments, 254);
            command.SetInputParameter(SqlDbType.Decimal, "Cost", obj.Cost);
            command.SetInputParameter(SqlDbType.Decimal, "CostAfter", obj.CostAfter);
            command.SetInputParameter(SqlDbType.Decimal, "CostBefore", obj.CostBefore);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
            command.SetInputParameter(SqlDbType.BigInt, "PerformedById", obj.PerformedById);
            command.SetInputParameter(SqlDbType.Decimal, "QtyAfter", obj.QtyAfter);
            command.SetInputParameter(SqlDbType.Decimal, "QtyBefore", obj.QtyBefore);
            command.SetInputParameter(SqlDbType.BigInt, "RequestorId", obj.RequestorId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "StockType", obj.StockType, 15);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            command.SetInputParameter(SqlDbType.DateTime2, "TransactionDate", obj.TransactionDate);
            command.SetInputParameter(SqlDbType.Decimal, "TransactionQuantity", obj.TransactionQuantity);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TransactionType", obj.TransactionType, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UnitofMeasure", obj.UnitofMeasure, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreatedBy", obj.CreatedBy, 254);
            command.SetInputParameter(SqlDbType.DateTime2, "CreatedDate", obj.CreatedDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType_Secondary", obj.ChargeType_Secondary, 15);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId_Secondary", obj.ChargeToId_Secondary);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VMRSFailure", obj.VMRSFailure, 15);
            command.SetInputParameter(SqlDbType.BigInt, "EstimatedCostsId", obj.EstimatedCostsId);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PartHistoryId = (long)command.Parameters["@PartHistoryId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}