/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =================================================
* 2015-Nov-04 SOM-839    Roger Lawton      Changed parameters
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
    /// Access the usp_Timecard_CreateByForeignKeys stored procedure.
    /// </summary>
    public class usp_Timecard_CreateByForeignKeys
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Timecard_CreateByForeignKeys";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Timecard_CreateByForeignKeys()
        {
        }

        /// <summary>
        /// Static method to call the usp_Timecard_CreateByForeignKeys stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Timecard obj

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
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetOutputParameter(SqlDbType.BigInt, "TimecardId");
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType_Primary", obj.ChargeType_Primary, 15);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId_Primary", obj.ChargeToId_Primary);
            command.SetInputParameter(SqlDbType.Decimal, "Hours", obj.Hours);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
            command.SetInputParameter(SqlDbType.DateTime2, "StartDate", obj.StartDate);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.TimecardId = (long)command.Parameters["@TimecardId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}