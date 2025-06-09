/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2018 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 2015-Dec-16  SOM-884   Roger Lawton      Do not need to send LaborAccountClientLookupId to the 
*                                          stored procedure
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
    public class usp_ProcessSystem_CreateByPkForeignKey
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_ProcessSystem_CreateByPkForeignKey";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_ProcessSystem_CreateByPkForeignKey()
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
            b_ProcessSystem obj

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
            command.SetOutputParameter(SqlDbType.BigInt, "ProcessSystemId");
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "AreaId", obj.AreaId);
            command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            command.SetInputParameter(SqlDbType.Bit, "InActiveFlag", obj.InActiveFlag);
            command.SetInputParameter(SqlDbType.BigInt, "Labor_AccountId", obj.Labor_AccountId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Process", obj.Process, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "System", obj.System, 15);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "LaborAccountClientLookupId", obj.LaborAccountClientLookupId, 31);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            if (!string.IsNullOrEmpty(command.Parameters["@ProcessSystemId"].Value.ToString()))
            {
                obj.ProcessSystemId = (long)command.Parameters["@ProcessSystemId"].Value;
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);




        }
    }
}