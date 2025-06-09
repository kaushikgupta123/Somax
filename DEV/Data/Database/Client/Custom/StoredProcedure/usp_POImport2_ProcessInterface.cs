/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Review Changes
****************************************************************************************************
 */
using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
namespace Database.StoredProcedure
{
    public class usp_POImport2_ProcessInterface
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_POImport2_ProcessInterface";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_POImport2_ProcessInterface()
        {

        }


        public static void CallStoredProcedure(
            SqlCommand command,
            long CallerUserPersonnelId,
            string callerUserName, long personnelId,
            b_POImport2 obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter updateIndexOut_parameter = null;
            int retCode = 0;
            
            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", CallerUserPersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "POImport2Id", obj.POImport2Id);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);

            // Setup updateIndexOut parameter.


            // Execute stored procedure.
            command.ExecuteNonQuery();

          //  obj.UpdateIndex = (int)updateIndexOut_parameter.Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
