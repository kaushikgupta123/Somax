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
    /// Access the usp_PartHistory_Create stored procedure.
    /// </summary>
    public class usp_PartHistory_PartIssue
    {
        /// <summary>
        /// Constants.
        /// </summary>
        //private static string STOREDPROCEDURE_NAME = "usp_PartHistory_PartIssue";
        private static string STOREDPROCEDURE_NAME = "usp_PartHistory_PartIssue";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PartHistory_PartIssue()
        {
        }

        /// <summary>
        /// Static method to call the usp_PartHistory_Create stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure (
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PartHistory obj
        )
        {
            SqlParameter        RETURN_CODE_parameter = null;
            int                 retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.VarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType_Primary", obj.ChargeType_Primary, 15);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId_Primary", obj.ChargeToId_Primary);
            command.SetInputParameter(SqlDbType.Decimal, "TransactionQuantity", obj.TransactionQuantity);
            command.SetInputParameter(SqlDbType.Bit, "PerformAdjustment", obj.IsPerformAdjustment);
            //command.SetInputParameter(SqlDbType.BigInt, "PerformedById", obj.PersonnelId);
            //command.SetInputParameter(SqlDbType.BigInt, "RequestorId", obj.PersonnelId);
            command.SetInputParameter(SqlDbType.BigInt, "PerformedById", obj.PerformedById);
            command.SetInputParameter(SqlDbType.BigInt, "RequestorId", obj.RequestorId);
            command.SetOutputParameter(SqlDbType.BigInt, "PartHistoryId");
            command.SetStringInputParameter(SqlDbType.NVarChar, "Comments", obj.Comments,254);


            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PartHistoryId = command.Parameters["@PartHistoryId"].Value==null?0 : (long)command.Parameters["@PartHistoryId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int) RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}