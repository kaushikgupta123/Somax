/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2014 by SOMAX Inc.
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
    /// Access the usp_CustomQueryDisplay_UpdateByPK stored procedure.
    /// </summary>
    public class usp_CustomQueryDisplay_UpdateByPK
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_CustomQueryDisplay_UpdateByPK";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_CustomQueryDisplay_UpdateByPK ()
        {
        }

        /// <summary>
        /// Static method to call the usp_CustomQueryDisplay_UpdateByPK stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="updateIndexOut">int that contains the value of the @UpdateIndexOut parameter</param>
        public static void CallStoredProcedure (
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_CustomQueryDisplay obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter        updateIndexOut_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "CustomQueryDisplayId", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TableName", obj.TableName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Language", obj.Language, 16);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Culture", obj.Culture, 16);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Key", obj.Key, 16);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DisplayText", obj.DisplayText, 127);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CaseNo);
            command.SetInputParameter(SqlDbType.BigInt, "UpdateIndex", obj.UpdateIndex);

                // Setup updateIndexOut parameter.
                updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
                updateIndexOut_parameter.Direction = ParameterDirection.Output;

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.UpdateIndex = (int)updateIndexOut_parameter.Value;

            // Process the RETURN_CODE parameter value
            retCode = (int) RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}