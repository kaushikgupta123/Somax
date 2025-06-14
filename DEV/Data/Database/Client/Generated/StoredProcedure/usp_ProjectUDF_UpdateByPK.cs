/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2023 by SOMAX Inc.
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
    /// Access the usp_ProjectUDF_UpdateByPK stored procedure.
    /// </summary>
    public class usp_ProjectUDF_UpdateByPK
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_ProjectUDF_UpdateByPK";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_ProjectUDF_UpdateByPK()
        {
        }

        /// <summary>
        /// Static method to call the usp_ProjectUDF_UpdateByPK stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_ProjectUDF obj
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
            command.SetInputParameter(SqlDbType.BigInt, "ProjectUDFId", obj.ProjectUDFId);
            command.SetInputParameter(SqlDbType.BigInt, "ProjectId", obj.ProjectId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Text1", obj.Text1, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Text2", obj.Text2, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Text3", obj.Text3, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Text4", obj.Text4, 67);
            command.SetInputParameter(SqlDbType.DateTime2, "Date1", obj.Date1);
            command.SetInputParameter(SqlDbType.DateTime2, "Date2", obj.Date2);
            command.SetInputParameter(SqlDbType.DateTime2, "Date3", obj.Date3);
            command.SetInputParameter(SqlDbType.DateTime2, "Date4", obj.Date4);
            command.SetInputParameter(SqlDbType.Bit, "Bit1", obj.Bit1);
            command.SetInputParameter(SqlDbType.Bit, "Bit2", obj.Bit2);
            command.SetInputParameter(SqlDbType.Bit, "Bit3", obj.Bit3);
            command.SetInputParameter(SqlDbType.Bit, "Bit4", obj.Bit4);
            command.SetInputParameter(SqlDbType.Decimal, "Numeric1", obj.Numeric1);
            command.SetInputParameter(SqlDbType.Decimal, "Numeric2", obj.Numeric2);
            command.SetInputParameter(SqlDbType.Decimal, "Numeric3", obj.Numeric3);
            command.SetInputParameter(SqlDbType.Decimal, "Numeric4", obj.Numeric4);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Select1", obj.Select1, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Select2", obj.Select2, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Select3", obj.Select3, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Select4", obj.Select4, 15);

            // Execute stored procedure.
            command.ExecuteNonQuery();



            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}