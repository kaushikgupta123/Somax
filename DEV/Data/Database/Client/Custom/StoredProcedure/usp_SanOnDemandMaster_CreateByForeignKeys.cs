/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date        Log Id     Person               Description
 * =========== ========== ==================== =================================
 * 2012-Oct-21            Roger Lawton         Add BIM Identifier
 * 2013-Apr-18            Nick Fuchs           Added custom Columns
 * 2013-Aug-04  201350051 Nick Fuchs           Added remaining custom columns
 * 2013-Aug-07            Roger Lawton         Removed Custom Columns
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
    /// Access the usp_Equipment_Create stored procedure.
    /// </summary>
    public class usp_SanOnDemandMaster_CreateByForeignKeys
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_SanOnDemandMaster_Create";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_SanOnDemandMaster_CreateByForeignKeys()
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
            b_SanOnDemandMaster obj

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
            command.SetOutputParameter(SqlDbType.BigInt, "SanOnDemandMasterId");
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookUpId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 15);
            command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", obj.InactiveFlag);
            command.SetInputParameter(SqlDbType.Bit, "Del", obj.Del);


            // Execute stored procedure.
            command.ExecuteNonQuery();

            if (!string.IsNullOrEmpty(command.Parameters["@SanOnDemandMasterId"].Value.ToString()))
            {
                obj.SanOnDemandMasterId = (long)command.Parameters["@SanOnDemandMasterId"].Value;
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);




        }
    }
}