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
    /// Access the usp_Part_UpdateBulk stored procedure.
    /// </summary>
    public class usp_Part_UpdateBulk
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Part_UpdateBulk";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Part_UpdateBulk()
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "PartIdList", obj.PartIdList, 512);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "StockType", obj.StockType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "IssueUnit", obj.IssueUnit, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Manufacturer", obj.Manufacturer, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ManufacturerId", obj.ManufacturerId, 63);
            command.SetInputParameter(SqlDbType.BigInt, "AccountId", obj.AccountId);
            // Execute stored procedure.
            command.ExecuteNonQuery();
            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}