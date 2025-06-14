/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2018 by SOMAX Inc.
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
    /// Access the usp_PartImportLog_Create stored procedure.
    /// </summary>
    public class usp_PartImportLog_Create
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PartImportLog_Create";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PartImportLog_Create ()
        {
        }

        /// <summary>
        /// Static method to call the usp_PartImportLog_Create stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure (
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PartImportLog obj
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
            command.SetOutputParameter(SqlDbType.BigInt, "PartImportLogId");
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "DataImportLogId", obj.DataImportLogId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 70);
            command.SetInputParameter(SqlDbType.Decimal, "AverageCost", obj.AverageCost);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 127);
            command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", obj.InactiveFlag);
            command.SetStringInputParameter(SqlDbType.NVarChar, "IssueUnit", obj.IssueUnit, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Manufacturer", obj.Manufacturer, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ManufacturerId", obj.ManufacturerId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "StockType", obj.StockType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UPCCode", obj.UPCCode, 31);
            command.SetInputParameter(SqlDbType.Int, "CountFrequency", obj.CountFrequency);
            command.SetInputParameter(SqlDbType.Decimal, "QtyMaximum", obj.QtyMaximum);
            command.SetInputParameter(SqlDbType.Decimal, "QtyOnHand", obj.QtyOnHand);
            command.SetInputParameter(SqlDbType.Decimal, "QtyReorderLevel", obj.QtyReorderLevel);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PartImportLogId = (long)command.Parameters["@PartImportLogId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int) RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}