/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.. All rights reserved. 
***************************************************************************************************
* Date        JIRA No  Person         Description
* =========== ======== ============== =============================================================
* 2016-Sep-01 SOM-1081 Roger Lawton   Change CompleteComments, Description to support MAX
***************************************************************************************************
*/
using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_WorkOrder_UpdateByForeignKeys stored procedure.
    /// </summary>
    public class usp_PartTransfer_Issue
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PartTransfer_Issue";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PartTransfer_Issue()
        {
        }

        /// <summary>
        /// Static method to call the usp_WorkOrder_UpdateByForeignKeys stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PartTransfer obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter updateIndexOut_parameter = null;                  
             SqlParameter PartTransferEventLogIdOut_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "PartTransferId", obj.PartTransferId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.Decimal, "Quantity", obj.Quantity);
            command.SetStringInputParameter(SqlDbType.NVarChar, "LastEvent", obj.LastEvent, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetInputParameter(SqlDbType.BigInt, "Creator_PersonnelId", obj.Creator_PersonnelId);
            command.SetInputParameter(SqlDbType.BigInt, "LoggedUser_PersonnelId", obj.Creator_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TransactionType", obj.TransactionType, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Comments", obj.Comments, -1);
            command.SetInputParameter(SqlDbType.Int, "UpdateIndex", obj.UpdateIndex);
            updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
            updateIndexOut_parameter.Direction = ParameterDirection.Output;
            PartTransferEventLogIdOut_parameter = command.Parameters.Add("@PartTransferEventLogId", SqlDbType.BigInt);
            PartTransferEventLogIdOut_parameter.Direction = ParameterDirection.Output;


            // Execute stored procedure.
            command.ExecuteNonQuery();
            obj.UpdateIndex = (int)updateIndexOut_parameter.Value;
            obj.PartTransferEventLogId = (Int64)PartTransferEventLogIdOut_parameter.Value;


            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}